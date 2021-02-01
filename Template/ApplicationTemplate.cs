using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeGenerator.Template
{
    public static class ApplicationTemplate
    {
        ///  <summary>
        /// Service模板
        ///  </summary>
        ///  <param name="tableName"></param>
        ///  <param name="tableComment"></param>
        ///  <param name="dataTpe"></param>
        ///  <param name="projectName"></param>
        ///  <returns></returns>
        public static string ServiceTemplate(string tableName, string tableComment, string dataTpe, string projectName)
        {
            var first = tableName.Substring(0, 1).ToLower();
            var end = tableName.Substring(1);
            var sb = new StringBuilder();
            sb.AppendLine($"using Benchint.Abp.Application.Services;");
            sb.AppendLine($"using Benchint.{projectName}.{tableName}s.Repository;");
            sb.AppendLine($"using Benchint.{projectName}.{tableName}s.DomainService;");
            sb.AppendLine($"using Benchint.{projectName}.{tableName}s.Dto;");
            sb.AppendLine("using System.Text;\r\n\r\n");
            sb.AppendLine($"namespace Benchint.{projectName}.{tableName}s");
            sb.AppendLine("    {\r\n");
            sb.AppendLine($"            /// <summary>");
            sb.AppendLine($"            /// 应用服务实现: {tableComment} ");
            sb.AppendLine($"            /// </summary>");
            sb.AppendLine($"            public class {tableName}AppService : BenchintCrudAppService<{tableName}, {tableName}Dto, {dataTpe}, {tableName}PagedAndSortedResultRequestDto, CreateUpdate{tableName}Dto, CreateUpdate{tableName}Dto>, I{tableName}AppService");
            sb.AppendLine("            {");
            sb.AppendLine($"                public {tableName}AppService(I{tableName}Repository repository) : base(repository)");
            sb.AppendLine("                  {");
            sb.AppendLine("                  }");
            sb.AppendLine("            }");
            sb.AppendLine("    }");
            return sb.ToString();
        }

        ///  <summary>
        /// AutoMapper模板
        ///  </summary>
        ///  <param name="tableName"></param>
        ///  <param name="tableComment"></param>
        ///  <param name="projectName"></param>
        ///  <returns></returns>
        public static string AutoMapperTemplate(string tableName, string tableComment, string projectName)
        {
            var sb = new StringBuilder();

            sb.AppendLine("using AutoMapper;");
            sb.AppendLine($"using Benchint.{projectName}.{tableName}s.Dto;\r\n");
            sb.AppendLine($"namespace Benchint.{projectName}.{tableName}s");
            sb.AppendLine("    {\r\n\r\n");
            sb.AppendLine($"            /// <summary>");
            sb.AppendLine($"            /// 数据模型: {tableComment} ");
            sb.AppendLine($"            /// </summary>");
            sb.AppendLine($"            public class {tableName}ApplicationAutoMapperProfile :Profile");
            sb.AppendLine("            {\r\n");
            sb.AppendLine($"                        public {tableName}ApplicationAutoMapperProfile()");
            sb.AppendLine("                         {");
            sb.AppendLine($"                              CreateMap<CreateUpdate{tableName}Dto, {tableName}>();");
            sb.AppendLine($"                              CreateMap<{tableName}, CreateUpdate{tableName}Dto>();");
            sb.AppendLine($"                              CreateMap<{tableName}Dto, {tableName}>();");
            sb.AppendLine($"                              CreateMap<{tableName}, {tableName}Dto>();");
            sb.AppendLine("                         }");
            sb.AppendLine("            }");
            sb.AppendLine("    }");
            return sb.ToString();
        }
    }
}