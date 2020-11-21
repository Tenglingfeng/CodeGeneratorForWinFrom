using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using CodeGenerator.Template;
using Dapper;
using Microsoft.EntityFrameworkCore.Internal;

namespace CodeGenerator
{
    public partial class Form1 : Form
    {
        private static readonly string path = $"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\\Result\\";

        public Form1()
        {
            InitializeComponent();
            button1.Enabled = false;
        }

        private void button1_ClickAsync(object sender, EventArgs e)
        {
            // Task.Run(() =>
            // {
            try
            {
                var tables = GetSelectedTableNames();

                foreach (var table in tables)
                {
                    var tableName = ReplaceString(table);
                    //获取表结构信息
                    var tableInfoList = DbHelper.DbHelper.GetInformationSchema(connString.Text, table);
                    //替换表字段和表类型
                    tableInfoList.ForEach(t => { t.ColumnName = ReplaceString(t.ColumnName); t.DataType = GetClrType(t.DataType); });

                    // todo 生成实体类模板
                    var modelTemplate = ModelTemplate.Template(tableInfoList, tableName, tableInfoList.Select(x => x.TableComment).FirstOrDefault());

                    SaveFiles($"Models\\", $"{tableName}.cs", modelTemplate);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(@"代码生成失败," + exception.Message);
                return;
            }
            MessageBox.Show(@"代码生成成功");
            //  });
        }

        /// <summary>
        /// 获取选中的表名
        /// </summary>
        /// <returns></returns>
        private IEnumerable<string> GetSelectedTableNames()
        {
            return tablesChecked.Items.Cast<object>().Where((t, i) => tablesChecked.GetItemChecked(i)).Select(t => tablesChecked.GetItemText(t)).ToList();
        }

        /// <summary>
        /// 显示表名列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void connection_Click(object sender, EventArgs e)
        {
            try
            {
                tablesChecked.Items.Clear();

                var tables = DbHelper.DbHelper.GetTables(GetDbName(), connString.Text);

                foreach (var table in tables)
                {
                    tablesChecked.Items.Add(table);
                }

                if (tablesChecked.Items.Any()) return;
                MessageBox.Show(@"未查找到表,请检查数据库连接地址是否正确或包含空格");
                return;
            }
            catch (Exception exception)
            {
                MessageBox.Show(@"数据库连接失败," + exception.Message);
                return;
            }
        }

        /// <summary>
        /// 全选/取消全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Selected_Click(object sender, EventArgs e)
        {
            for (var i = 0; i < tablesChecked.Items.Count;)
            {
                if (tablesChecked.GetItemCheckState(i) == CheckState.Checked)
                {
                    for (var j = 0; j < tablesChecked.Items.Count; j++)
                    {
                        tablesChecked.SetItemCheckState(j, CheckState.Unchecked);
                    }

                    button1.Enabled = false;
                    return;
                }
                else
                {
                    for (var j = 0; j < tablesChecked.Items.Count; j++)
                    {
                        tablesChecked.SetItemCheckState(j, CheckState.Checked);
                    }
                    button1.Enabled = true;
                    return;
                }
            }
        }

        /// <summary>
        /// 获取数据库名称
        /// </summary>
        /// <returns></returns>
        public string GetDbName()
        {
            var startIndex = connString.Text.TrimStart().IndexOf("database=", StringComparison.Ordinal) + 9;
            var endIndex = connString.Text.TrimStart().IndexOf(";", startIndex, StringComparison.Ordinal);
            return connString.Text.Trim().Substring(startIndex, endIndex - startIndex).Trim();
        }

        /// <summary>
        /// 保存文件到桌面
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="fileName"></param>
        /// <param name="template"></param>
        private void SaveFiles(string filePath, string fileName, string template)
        {
            if (!Directory.Exists(path + filePath))
            {
                Directory.CreateDirectory(path + filePath);
            }
            var fileStream = new FileStream(path + filePath + fileName, FileMode.Create);
            var data = System.Text.Encoding.Default.GetBytes(template);
            fileStream.Write(data, 0, data.Length);
            fileStream.Flush();
        }

        /// <summary>
        /// 根据数据库类型获取字段类型
        /// </summary>
        /// <param name="dbType"></param>
        /// <returns></returns>
        private static string GetClrType(string dbType)
        {
            switch (dbType)
            {
                case "tinyint":
                case "smallint":
                case "mediumint":
                case "int":
                case "integer":
                    return "int";

                case "bigint":
                    return "long";

                case "double":
                    return "double";

                case "float":
                    return "float";

                case "decimal":
                    return "decimal";

                case "numeric":
                case "real":
                    return "decimal";

                case "bit":
                    return "bool";

                case "date":
                case "time":
                case "year":
                case "datetime":
                case "timestamp":
                    return "DateTime";

                case "tinyblob":
                case "blob":
                case "mediumblob":
                case "longblog":
                case "binary":
                case "varbinary":
                    return "byte[]";

                case "char":
                case "varchar":
                case "tinytext":
                case "text":
                case "mediumtext":
                case "longtext":
                    return "string";

                case "point":
                case "linestring":
                case "polygon":
                case "geometry":
                case "multipoint":
                case "multilinestring":
                case "multipolygon":
                case "geometrycollection":
                case "enum":
                case "set":
                default:
                    return dbType;
            }
        }

        /// <summary>
        /// 转驼峰命名
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static string ReplaceString(string str)
        {
            var result = "";
            if (!str.Contains("_") && !Regex.IsMatch(str, "[A-Z]"))  //&& Regex.IsMatch(str, "[a-z]")
            {
                result = str[0].ToString().ToUpper() + str.Substring(1);
            }

            if (str.Contains("_"))
            {
                var array = str.Split("_");
                result = array.Aggregate(result, (current, s) => current + (s[0].ToString().ToUpper() + s.Substring(1)));
            }

            return result;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }


        private void tablesChecked_SelectedValueChanged(object sender, EventArgs e)
        {
            var tableNames = GetSelectedTableNames();
            button1.Enabled = tableNames.Any();
        }
    }
}