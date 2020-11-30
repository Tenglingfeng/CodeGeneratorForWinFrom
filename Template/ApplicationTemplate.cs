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
        ///  <returns></returns>
        public static string ServiceTemplate(string tableName, string tableComment, string dataTpe)
        {
            var first = tableName.Substring(0, 1).ToLower();
            var end = tableName.Substring(1);
            var sb = new StringBuilder();
            sb.AppendLine("using System;");
            sb.AppendLine($"using {tableName}.Repository;");
            sb.AppendLine($"using {tableName}.Manager;");
            sb.AppendLine($"using {tableName}.Dto;");
            sb.AppendLine("using System.Text;\r\n");
            sb.AppendLine($"namespace {tableName}s");
            sb.AppendLine("    {\r\n");
            sb.AppendLine($"            /// <summary>");
            sb.AppendLine($"            /// 应用服务实现: {tableComment} ");
            sb.AppendLine($"            /// </summary>");
            sb.AppendLine($"            public class {tableName}AppService : BenchintCrudAppService<{tableName}, {tableName}Dto, {dataTpe}, {tableName}PagedAndSortedResultRequestDto, CreateUpdate{tableName}Dto, CreateUpdate{tableName}Dto>, I{tableName}AppService");
            sb.AppendLine("            {");
            sb.AppendLine($"                private readonly I{tableName}Manager _{first + end}Manager;\r\n\r\n");
            sb.AppendLine($"                public {tableName}AppService(I{tableName}Repository repository, I{tableName}Manager {first + end}Manager) : base(repository)");
            sb.AppendLine("                  {");
            sb.AppendLine($"                       _{first + end}Manager = {first + end}Manager;");
            sb.AppendLine("                  }\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n");
            sb.AppendLine("            }");
            sb.AppendLine("    }");
            return sb.ToString();
        }

        /// <summary>
        ///AutoMapper模板
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="tableComment"></param>
        /// <returns></returns>
        public static string AutoMapperTemplate(string tableName, string tableComment)
        {
            var sb = new StringBuilder();

            sb.AppendLine("using AutoMapper;");
            sb.AppendLine("using System.Text;\r\n");
            sb.AppendLine($"namespace {tableName}.Dto");
            sb.AppendLine("    {\r\n");
            sb.AppendLine($"            /// <summary>");
            sb.AppendLine($"            /// 数据模型: {tableComment} ");
            sb.AppendLine($"            /// </summary>");
            sb.AppendLine($"            public class {tableName}ApplicationAutoMapperProfile :Profile");
            sb.AppendLine("            {\r\n\r\n\r\n");
            sb.AppendLine($"                        public {tableName}ApplicationAutoMapperProfile()");
            sb.AppendLine("                         {");
            sb.AppendLine($"                              CreateMap<CreateUpdate{tableName}Dto, {tableName}>();");
            sb.AppendLine($"                              CreateMap<{tableName}, CreateUpdate{tableName}Dto>();");
            sb.AppendLine($"                              CreateMap<{tableName}Dto, {tableName}>();");
            sb.AppendLine($"                              CreateMap<{tableName}, {tableName}Dto>();\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n");
            sb.AppendLine("                         }");
            sb.AppendLine("            }");
            sb.AppendLine("    }");
            return sb.ToString();
        }
    }
}