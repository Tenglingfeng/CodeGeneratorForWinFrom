using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace CodeGenerator.Template
{
    public static class EntityFrameworkCoreTemplate
    {
        /// <summary>
        ///Repository模板
        /// </summary>
        /// <param name="dbName"></param>
        /// <param name="tableName"></param>
        /// <param name="tableComment"></param>
        /// <returns></returns>
        public static string RepositoryTemplate(string dbName, string tableName, string tableComment)
        {
            var sb = new StringBuilder();
            sb.AppendLine("using System;\r\n");
            sb.AppendLine($"namespace {tableName}.Repository");
            sb.AppendLine("    {\r\n");
            sb.AppendLine($"            /// <summary>");
            sb.AppendLine($"            /// 仓储实现: {tableComment} ");
            sb.AppendLine($"            /// </summary>");
            sb.AppendLine($"            public class {tableName}Repository : EfCoreRepository<{dbName}Context,{tableName}>,I{tableName}Repository");
            sb.AppendLine("            {\r\n");
            sb.AppendLine($"                    private readonly IDbContextProvider<{dbName}Context> _{dbName.ToLower()}ContextProvider;\r\n\r\n");
            sb.AppendLine($"                    public {tableName}Repository(IDbContextProvider<{dbName}Context> dbContextProvider) : base(dbContextProvider)");
            sb.AppendLine("                     {");
            sb.AppendLine($"                             _{dbName.ToLower()}ContextProvider = dbContextProvider;");
            sb.AppendLine("                     }\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n");
            sb.AppendLine("            }");
            sb.AppendLine("    }");
            return sb.ToString();
        }
    }
}