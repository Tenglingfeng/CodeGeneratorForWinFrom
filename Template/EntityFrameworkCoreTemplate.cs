using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace CodeGenerator.Template
{
    public static class EntityFrameworkCoreTemplate
    {
        ///  <summary>
        /// Repository模板
        ///  </summary>
        ///  <param name="dbName"></param>
        ///  <param name="tableName"></param>
        ///  <param name="tableComment"></param>
        ///  <param name="projectName"></param>
        ///  <param name="dataType"></param>
        ///  <returns></returns>
        public static string RepositoryTemplate(string dbName, string tableName, string tableComment, string projectName, string dataType)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"using Benchint.{projectName}.{tableName}s;");
            sb.AppendLine($"using Benchint.{projectName}.{tableName}s.Repository;");
            sb.AppendLine($"using Volo.Abp.Domain.Repositories.EntityFrameworkCore;");
            sb.AppendLine($"using Volo.Abp.EntityFrameworkCore;\r\n\r\n");
            sb.AppendLine($"namespace Benchint.{projectName}.Repositories.{tableName}s");
            sb.AppendLine("    {\r\n");
            sb.AppendLine($"            /// <summary>");
            sb.AppendLine($"            /// 仓储实现: {tableComment} ");
            sb.AppendLine($"            /// </summary>");
            sb.AppendLine($"            public class {tableName}Repository : EfCoreRepository<{dbName}Context,{tableName},{dataType}>,I{tableName}Repository");
            sb.AppendLine("            {\r\n");
            sb.AppendLine($"                    public {tableName}Repository(IDbContextProvider<{dbName}Context> dbContextProvider) : base(dbContextProvider)");
            sb.AppendLine("                     {");
            sb.AppendLine("                     }");
            sb.AppendLine("            }");
            sb.AppendLine("    }");
            return sb.ToString();
        }
    }
}