using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGenerator.Template
{
    public static class ModelTemplate
    {
        /// <summary>
        ///Model模板
        /// </summary>
        /// <param name="tableInfoList"></param>
        /// <param name="tableName"></param>
        /// <param name="tableComment"></param>
        /// <returns></returns>
        public static string Template(List<InformationSchema> tableInfoList, string tableName, string tableComment)
        {
            if (tableInfoList.Count <= 0)
            {
                throw new Exception($"找不到表{tableName}的相关信息");
            }
            var sb = new StringBuilder();
            var getSet = " { get; set; } ";
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Text;\r\n");
            sb.AppendLine($"namespace {tableName}");
            sb.AppendLine("    {\r\n");
            sb.AppendLine($"            /// <summary>");
            sb.AppendLine($"            ///  {tableComment} ");
            sb.AppendLine($"            /// </summary>");
            sb.AppendLine($"            public class {tableName}");
            sb.AppendLine("            {");
            foreach (var informationSchema in tableInfoList)
            {
                sb.AppendLine($"              /// <summary>");
                sb.AppendLine($"              ///  {informationSchema.ColumnComment} ");
                sb.AppendLine($"              /// </summary>");
                sb.AppendLine($"              public  {informationSchema.DataType}  {informationSchema.ColumnName} {getSet}");
                sb.AppendLine();
            }
            sb.AppendLine("            }");
            sb.AppendLine("    }");
            return sb.ToString();
        }
    }
}