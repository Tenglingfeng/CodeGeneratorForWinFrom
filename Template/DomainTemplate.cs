using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeGenerator.Template
{
    public static class DomainTemplate
    {
        ///  <summary>
        /// Entity模板
        ///  </summary>
        ///  <param name="tableInfoList"></param>
        ///  <param name="tableName"></param>
        ///  <param name="tableComment"></param>
        ///  <param name="projectName"></param>
        ///  <returns></returns>
        public static string EntityTemplate(List<InformationSchema> tableInfoList, string tableName, string tableComment, string projectName)
        {
            if (tableInfoList.Count <= 0)
            {
                throw new Exception($"找不到表{tableName}的相关信息");
            }
            var sb = new StringBuilder();
            var getSet = " { get; set; } ";
            sb.AppendLine("using System;");
            sb.AppendLine("using Volo.Abp.Domain.Entities;\r\n");
            sb.AppendLine($"namespace Benchint.{projectName}.{tableName}s");
            sb.AppendLine("    {\r\n");
            sb.AppendLine($"            /// <summary>");
            sb.AppendLine($"            /// 实体类信息: {tableComment} ");
            sb.AppendLine($"            /// </summary>");
            sb.AppendLine($"            public class {tableName} :Entity<{tableInfoList.FirstOrDefault()?.DataType}>");
            sb.AppendLine("            {");
            foreach (var informationSchema in tableInfoList.Skip(1))
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

        ///  <summary>
        /// Manager模板
        ///  </summary>
        ///  <param name="tableName"></param>
        ///  <param name="tableComment"></param>
        ///  <param name="projectName"></param>
        ///  <returns></returns>
        public static string ManagerTemplate(string tableName, string tableComment, string projectName)
        {
            var first = tableName.Substring(0, 1).ToLower();
            var end = tableName.Substring(1);
            var sb = new StringBuilder();
            sb.AppendLine($"using Benchint.{projectName}.{tableName}s.Repository;\r\n\r\n");
            sb.AppendLine($"namespace Benchint.{projectName}.{tableName}s.DomainService");
            sb.AppendLine("    {\r\n");
            sb.AppendLine($"            /// <summary>");
            sb.AppendLine($"            /// 领域服务实现: {tableComment} ");
            sb.AppendLine($"            /// </summary>");
            sb.AppendLine($"            public class {tableName}Manager  : {projectName}DomainServerBase, I{tableName}Manager");
            sb.AppendLine("            {\r\n");
            sb.AppendLine($"                private readonly I{tableName}Repository _{first + end}Repository;\r\n\r\n");
            sb.AppendLine($"                public {tableName}Manager(I{tableName}Repository repository)");
            sb.AppendLine("                  {");
            sb.AppendLine($"                       _{first + end}Repository = repository;");
            sb.AppendLine("                  }\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n");
            sb.AppendLine("            }");
            sb.AppendLine("    }");
            return sb.ToString();
        }

        ///  <summary>
        /// IManager模板
        ///  </summary>
        ///  <param name="tableName"></param>
        ///  <param name="tableComment"></param>
        ///  <param name="projectName"></param>
        ///  <returns></returns>
        public static string IManagerTemplate(string tableName, string tableComment, string projectName)
        {
            var sb = new StringBuilder();
            sb.AppendLine("using Volo.Abp.Domain.Services;\r\n");
            sb.AppendLine($"namespace Benchint.{projectName}.{tableName}s.DomainService");
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

        ///  <summary>
        /// IRepository模板
        ///  </summary>
        ///  <param name="tableInfoList"></param>
        ///  <param name="tableName"></param>
        ///  <param name="tableComment"></param>
        ///  <param name="projectName"></param>
        ///  <returns></returns>
        public static string IRepositoryTemplate(List<InformationSchema> tableInfoList, string tableName, string tableComment, string projectName)
        {
            var sb = new StringBuilder();
            sb.AppendLine("using Volo.Abp.Domain.Repositories;\r\n");
            sb.AppendLine($"namespace Benchint.{projectName}.{tableName}s.Repository");
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