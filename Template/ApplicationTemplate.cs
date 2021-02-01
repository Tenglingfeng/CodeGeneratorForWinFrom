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
            sb.AppendLine($"            public class {tableName}AppService : LibraDataCollectionCurdAppService<{tableName}, {tableName}Dto, {dataTpe}, {tableName}PagedAndSortedResultRequestDto, CreateUpdate{tableName}Dto, CreateUpdate{tableName}Dto>, I{tableName}AppService");
            sb.AppendLine("            {");
            sb.AppendLine($"                private readonly I{tableName}Manager _{first + end}Manager;\r\n\r\n");
            sb.AppendLine($"                public {tableName}AppService(I{tableName}Repository repository, I{tableName}Manager {first + end}Manager) : base(repository)");
            sb.AppendLine("                  {");
            sb.AppendLine($"                       _{first + end}Manager = {first + end}Manager;");
            sb.AppendLine("                  }");
            //sb.AppendLine("               /// <summary>");
            //sb.AppendLine($"               ///保存 {tableComment}");
            //sb.AppendLine("               /// </summary>");
            //sb.AppendLine("               /// <param name=\"dto\"></param>");
            //sb.AppendLine("               /// <returns></returns>");
            //sb.AppendLine($"               public async Task<bool> SaveAsync(CreateUpdate{tableName}Dto dto)");
            //sb.AppendLine("                  {");
            //sb.AppendLine($"                     return await SaveAsync(dto, x => 1 == 1);");
            //sb.AppendLine("                  }\r\n\r\n");
            sb.AppendLine("               /// <summary>");
            sb.AppendLine($"               ///保存 {tableComment} 列表");
            sb.AppendLine("               /// </summary>");
            sb.AppendLine("               /// <param name=\"dtos\"></param>");
            sb.AppendLine("               /// <returns></returns>");
            sb.AppendLine($"               public async Task<bool> SaveBatchAsync(IEnumerable<CreateUpdate{tableName}Dto> dtos)");
            sb.AppendLine("                  {");
            sb.AppendLine("                     return await SaveAsync(dtos, x => dtos.Select(x => x.Id).Contains(x.Id));");
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
            sb.AppendLine("            {\r\n\r\n\r\n");
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