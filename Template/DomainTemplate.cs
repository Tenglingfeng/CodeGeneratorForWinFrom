using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeGenerator.Template
{
    public static class DomainTemplate
    {
        /// <summary>
        ///Entity模板
        /// </summary>
        /// <param name="tableInfoList"></param>
        /// <param name="tableName"></param>
        /// <param name="tableComment"></param>
        /// <returns></returns>
        public static string EntityTemplate(List<InformationSchema> tableInfoList, string tableName, string tableComment)
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
            sb.AppendLine($"            /// 实体类信息: {tableComment} ");
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

        /// <summary>
        ///IManager模板
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="tableComment"></param>
        /// <returns></returns>
        public static string ManagerTemplate(string tableName, string tableComment)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"using {tableName}.Repository;\r\n");
            sb.AppendLine("using System;\r\n");
            sb.AppendLine($"namespace {tableName}.Manager");
            sb.AppendLine("    {\r\n");
            sb.AppendLine($"            /// <summary>");
            sb.AppendLine($"            /// 领域服务实现: {tableComment} ");
            sb.AppendLine($"            /// </summary>");
            sb.AppendLine($"            public class {tableName}Manager  : 项目名称DomainServerBase, I{tableName}Manager");
            sb.AppendLine("            {\r\n");
            sb.AppendLine($"                private readonly I{tableName}Repository _{tableName.ToLower()}Repository;\r\n\r\n");
            sb.AppendLine($"                public {tableName}Manager(I{tableName}Repository repository)");
            sb.AppendLine("                  {");
            sb.AppendLine($"                       _{tableName.ToLower()}Repository = repository;");
            sb.AppendLine("                  }\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n");
            sb.AppendLine("            }");
            sb.AppendLine("    }");
            return sb.ToString();
        }

        /// <summary>
        ///Manager模板
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="tableComment"></param>
        /// <returns></returns>
        public static string IManagerTemplate(string tableName, string tableComment)
        {
            var sb = new StringBuilder();
            sb.AppendLine("using System;\r\n");
            sb.AppendLine($"namespace {tableName}.Manager");
            sb.AppendLine("    {\r\n");
            sb.AppendLine($"            /// <summary>");
            sb.AppendLine($"            /// 领域服务接口: {tableComment} ");
            sb.AppendLine($"            /// </summary>");
            sb.AppendLine($"            public interface I{tableName}Manager : IDomainService");
            sb.AppendLine("            {\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n");
            sb.AppendLine("            }");
            sb.AppendLine("    }");
            return sb.ToString();
        }

        /// <summary>
        ///IRepository模板
        /// </summary>
        /// <param name="tableInfoList"></param>
        /// <param name="tableName"></param>
        /// <param name="tableComment"></param>
        /// <returns></returns>
        public static string IRepositoryTemplate(List<InformationSchema> tableInfoList, string tableName, string tableComment)
        {
            var sb = new StringBuilder();
            sb.AppendLine("using System;\r\n");
            sb.AppendLine($"namespace {tableName}.Repository");
            sb.AppendLine("    {\r\n");
            sb.AppendLine($"            /// <summary>");
            sb.AppendLine($"            /// 仓储接口: {tableComment} ");
            sb.AppendLine($"            /// </summary>");
            sb.AppendLine($"            public interface I{tableName}Repository : IRepository<{tableName}, {tableInfoList.Select(x => x.DataType).FirstOrDefault()}>");
            sb.AppendLine("            {\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n");
            sb.AppendLine("            }");
            sb.AppendLine("    }");
            return sb.ToString();
        }
    }
}