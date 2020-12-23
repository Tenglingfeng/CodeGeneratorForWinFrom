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

        /// <summary>
        /// 生成代码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_ClickAsync(object sender, EventArgs e)
        {
            // Task.Run(() =>
            // {
            try
            {
                if (string.IsNullOrWhiteSpace(ProjectName.Text))
                {
                    MessageBox.Show(@"请输入项目名称如:Test");
                    return;
                }
                EnableButton(false);

                var tables = GetSelectedTableNames();

                foreach (var table in tables)
                {
                    var tableName = ReplaceString(table);
                    //获取表结构信息
                    var tableInfoList = DbHelper.DbHelper.GetInformationSchema(connString.Text, table);
                    //替换表字段和表类型
                    tableInfoList.ForEach(t => { t.ColumnName = ReplaceString(t.ColumnName); t.DataType = GetClrType(t.DataType, t.IsNullable); });

                    // todo 生成CreateUpdateDto模板
                    var createUpdateDtoTemplate = ContractTemplate.CreateUpdateDtoTemplate(tableInfoList, tableName,
                        tableInfoList.Select(x => x.TableComment).FirstOrDefault(), ProjectName.Text);

                    // todo 生成Dto模板
                    var dtoTemplate = ContractTemplate.DtoTemplate(tableInfoList, tableName,
                        tableInfoList.Select(x => x.TableComment).FirstOrDefault(), ProjectName.Text);

                    // todo 生成pagedAndSortedResultRequestDto模板
                    var pagedAndSortedResultRequestDtoTemplate = ContractTemplate.PagedAndSortedResultRequestDtoTemplate(tableName,
                        tableInfoList.Select(x => x.TableComment).FirstOrDefault(), ProjectName.Text);

                    //todo 生成iServiceTemplate 模板
                    var iServiceTemplate = ContractTemplate.IServiceTemplate(tableName,
                        tableInfoList.Select(x => x.TableComment).FirstOrDefault(),
                        tableInfoList.Select(x => x.DataType).FirstOrDefault(), ProjectName.Text);

                    // todo 生成IManager模板
                    var iManagerTemplate = DomainTemplate.IManagerTemplate(tableName, tableInfoList.Select(x => x.TableComment).FirstOrDefault(), ProjectName.Text);

                    // todo 生成Manager模板
                    var managerTemplate = DomainTemplate.ManagerTemplate(tableName, tableInfoList.Select(x => x.TableComment).FirstOrDefault(), ProjectName.Text);

                    // todo 生成IRepository模板
                    var iRepositoryTemplate = DomainTemplate.IRepositoryTemplate(tableInfoList, tableName, tableInfoList.Select(x => x.TableComment).FirstOrDefault(), ProjectName.Text);

                    // todo 生成Repository模板
                    var repositoryTemplate = EntityFrameworkCoreTemplate.RepositoryTemplate(GetDbName(), tableName, tableInfoList.Select(x => x.TableComment).FirstOrDefault(), ProjectName.Text, tableInfoList.Select(x => x.DataType).FirstOrDefault());

                    // todo 生成实体类模板
                    var entityTemplate = DomainTemplate.EntityTemplate(tableInfoList, tableName, tableInfoList.Select(x => x.TableComment).FirstOrDefault(), ProjectName.Text);

                    // todo 生成Service模板

                    var appService = ApplicationTemplate.ServiceTemplate(tableName,
                        tableInfoList.Select(x => x.TableComment).FirstOrDefault(),
                        tableInfoList.Select(x => x.DataType).FirstOrDefault(), ProjectName.Text);

                    // todo 生成autoMapperTemplate模板

                    var autoMapperTemplate = ApplicationTemplate.AutoMapperTemplate(tableName, tableInfoList.Select(x => x.TableComment).FirstOrDefault(), ProjectName.Text);

                    //保存Application文件
                    SaveFiles($"Application\\{tableName}s\\", $"{tableName}AppService.cs", appService);
                    SaveFiles($"Application\\{tableName}s\\", $"{tableName}ApplicationAutoMapperProfile.cs", autoMapperTemplate);

                    //保存Contracts文件
                    SaveFiles($"Contracts\\{tableName}s\\Dto\\", $"CreateUpdate{tableName}Dto.cs", createUpdateDtoTemplate);
                    SaveFiles($"Contracts\\{tableName}s\\Dto\\", $"{tableName}Dto.cs", dtoTemplate);
                    SaveFiles($"Contracts\\{tableName}s\\Dto\\", $"{tableName}PagedAndSortedResultRequestDto.cs", pagedAndSortedResultRequestDtoTemplate);
                    SaveFiles($"Contracts\\{tableName}s\\", $"I{tableName}AppService.cs", iServiceTemplate);

                    //保存Domain文件
                    SaveFiles($"Domain\\{tableName}s\\DomainService\\", $"I{tableName}Manager.cs", iManagerTemplate);
                    SaveFiles($"Domain\\{tableName}s\\DomainService\\", $"{tableName}Manager.cs", managerTemplate);
                    SaveFiles($"Domain\\{tableName}s\\Repository\\", $"I{tableName}Repository.cs", iRepositoryTemplate);
                    SaveFiles($"Domain\\{tableName}s\\", $"{tableName}.cs", entityTemplate);

                    //保存EntityFrameworkCore文件
                    SaveFiles($"EntityFrameworkCore\\Repositories\\{tableName}s\\", $"{tableName}Repository.cs", repositoryTemplate);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(@"代码生成失败," + exception.Message);
                EnableButton(true);
                return;
            }
            MessageBox.Show(@"代码生成成功");
            EnableButton(true);
            //  });
        }

        /// <summary>
        /// 禁用/启用所有的控件
        /// </summary>
        private void EnableButton(bool b)
        {
            foreach (Control control in this.Controls)
            {
                control.Enabled = b;
            }
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
            using var fileStream = new FileStream(path + filePath + fileName, FileMode.Create);
            var data = Encoding.Default.GetBytes(template);
            fileStream.Write(data, 0, data.Length);
            fileStream.Flush();
        }

        /// <summary>
        /// 根据数据库类型获取字段类型
        /// </summary>
        /// <param name="dbType"></param>
        /// <param name="isNullable"></param>
        /// <returns></returns>
        private static string GetClrType(string dbType, bool isNullable)
        {
            string result = "";
            switch (dbType)
            {
                case "tinyint":
                case "smallint":
                case "mediumint":
                case "int":
                case "integer": result = "int"; break;

                case "bigint":
                    result = "long"; break;

                case "double":
                    result = "double"; break;

                case "float":
                    result = "float"; break;

                case "decimal":
                    result = "decimal"; break;

                case "numeric":
                case "real":
                    result = "decimal"; break;

                case "bit":
                    result = "bool"; break;

                case "date":
                case "time":
                case "year":
                case "datetime":
                case "timestamp":
                    result = "DateTime"; break;

                case "tinyblob":
                case "blob":
                case "mediumblob":
                case "longblog":
                case "binary":
                case "varbinary":
                    result = "byte[]"; break;

                case "char":
                case "varchar":
                case "tinytext":
                case "text":
                case "mediumtext":
                case "longtext":
                    result = "string"; break;

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
                    result = dbType; break;
            }

            if (!isNullable) return result;
            if (!result.Equals("string"))
            {
                result = $"{result}?";
            }
            return result;
        }

        /// <summary>
        /// 转驼峰命名
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static string ReplaceString(string str)
        {
            var result = str;
            if (!str.Contains("_") && !Regex.IsMatch(str, "[A-Z]"))  //&& Regex.IsMatch(str, "[a-z]")
            {
                result = str[0].ToString().ToUpper() + str.Substring(1);
            }

            if (str.Contains("_"))
            {
                var array = str.Split("_");
                result = array.Aggregate(string.Empty, (current, s) => current + (s[0].ToString().ToUpper() + s.Substring(1)));
            }

            return result;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// 监测表列表选中事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tablesChecked_SelectedValueChanged(object sender, EventArgs e)
        {
            var tableNames = GetSelectedTableNames();
            button1.Enabled = tableNames.Any();
        }
    }
}