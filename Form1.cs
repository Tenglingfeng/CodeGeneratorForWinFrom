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
using Dapper;

namespace CodeGenerator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private  void button1_ClickAsync(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                try
                {
                    using IDbConnection conn = new MySqlConnection(connString.Text);
                    var tableName = this.tableName.Text;
                    string sql = $@"
                                SELECT
	                                `information_schema`.`COLUMNS`.`COLUMN_NAME`,
	                                `information_schema`.`COLUMNS`.`DATA_TYPE`,
	                                `information_schema`.`COLUMNS`.`COLUMN_COMMENT` 
                                FROM
	                                `information_schema`.`COLUMNS` 
                                WHERE
                                table_name = '{tableName}'
                            ";
                    var sql2 = $@"SELECT
	                                information_schema.`TABLES`.TABLE_COMMENT 
                                FROM
	                                `information_schema`.`TABLES` 
                                WHERE
	                                table_name = '{tableName}'";

                    var tableInfoList = new List<InformationSchema>();
                    var reader = conn.ExecuteReader(sql);

                    while (reader.Read())
                    {
                        var tableInfo = new InformationSchema()
                        {
                            ColumnName = ReplaceString(reader["COLUMN_NAME"].ToString()),
                            DataType = GetClrType(reader["DATA_TYPE"].ToString()),
                            ColumnComment = reader["COLUMN_COMMENT"].ToString()
                        };
                        tableInfoList.Add(tableInfo);
                    }
                    reader.Close();
                    reader.Dispose();
                    var tableComment = conn.QueryFirstOrDefault<string>(sql2);
                    var str = ClassTemplate(tableInfoList, tableName, tableComment);
                    var path = $"{Directory.GetParent(Directory.GetCurrentDirectory()).Parent?.Parent}\\{ReplaceString(tableName)}\\Entity";
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    path = path + $"\\{ReplaceString(tableName)}.cs";
                    FileStream fileStream = new FileStream(path, FileMode.Create);
                    byte[] data = System.Text.Encoding.Default.GetBytes(str);
                    fileStream.WriteAsync(data, 0, data.Length);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(@"代码生成失败,"+exception.Message);
                    return;
                    
                }
                MessageBox.Show(@"代码生成成功");
            });

        }

        private string ClassTemplate(List<InformationSchema> tableInfoList,string tableName,string tableComment)
        {
            if (tableInfoList.Count<=0)
            {
                throw new Exception("找不到表的相关信息");
            }
            var sb = new StringBuilder();
            var getSet = " { get; set; } ";
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Text;\r\n");
            sb.AppendLine($"namespace XX");
            sb.AppendLine("    {\r\n");
            sb.AppendLine($"            /// <summary>");
            sb.AppendLine($"            ///  {tableComment} ");
            sb.AppendLine($"            /// <summary>");
            sb.AppendLine($"            public class {ReplaceString(tableName)}");
            sb.AppendLine("            {");
            foreach (var informationSchema in tableInfoList)
            {
                sb.AppendLine($"              /// <summary>");
                sb.AppendLine($"              ///  {informationSchema.ColumnComment} ");
                sb.AppendLine($"              /// <summary>");
                sb.AppendLine($"              public  {informationSchema.DataType}  {informationSchema.ColumnName} {getSet}");
                sb.AppendLine();
            }
            sb.AppendLine("            }");
            sb.AppendLine("    }");
            return sb.ToString();
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
        private string ReplaceString(string str)
        {
            var result = "";
            if (!str.Contains("_")&&!Regex.IsMatch(str,"[A-Z]"))  //&& Regex.IsMatch(str, "[a-z]")
            {
                result= str[0].ToString().ToUpper()+str.Substring(1);
            }

            if (str.Contains("_"))
            {
                var array = str.Split("_");
                result = array.Aggregate(result, (current, s) => current + (s[0].ToString().ToUpper() + s.Substring(1)));
            }

            return result;
        }
    }
}
