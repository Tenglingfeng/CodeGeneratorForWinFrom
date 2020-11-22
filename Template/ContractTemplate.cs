﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeGenerator.Template
{
    public static class ContractTemplate
    {
        /// <summary>
        ///CreateUpdateDto模板
        /// </summary>
        /// <param name="tableInfoList"></param>
        /// <param name="tableName"></param>
        /// <param name="tableComment"></param>
        /// <returns></returns>
        public static string CreateUpdateDtoTemplate(List<InformationSchema> tableInfoList, string tableName, string tableComment)
        {
            if (tableInfoList.Count <= 0)
            {
                throw new Exception($"找不到表{tableName}的相关信息");
            }
            var sb = new StringBuilder();
            var getSet = " { get; set; } ";
            sb.AppendLine("using System;");
            sb.AppendLine("using System.ComponentModel.DataAnnotations;");
            sb.AppendLine("using System.Text;\r\n");
            sb.AppendLine($"namespace {tableName}.Dto");
            sb.AppendLine("    {\r\n");
            sb.AppendLine($"            /// <summary>");
            sb.AppendLine($"            /// 数据模型: 新增/修改{tableComment} ");
            sb.AppendLine($"            /// </summary>");
            sb.AppendLine($"            public class CreateUpdate{tableName}Dto :EntityDto<{tableInfoList.FirstOrDefault()?.DataType}>");
            sb.AppendLine("            {");
            foreach (var informationSchema in tableInfoList.Skip(1))
            {
                sb.AppendLine($"              /// <summary>");
                sb.AppendLine($"              ///  {informationSchema.ColumnComment} ");
                sb.AppendLine($"              /// </summary>");
                if (!informationSchema.IsNullable)
                {
                    sb.AppendLine($"              [Required(ErrorMessage = \"{informationSchema.ColumnComment}不能为空\")]");
                }

                if (!string.IsNullOrEmpty(informationSchema.CharacterMaximumLength) && informationSchema.DataType.Equals("string"))
                {
                    sb.AppendLine(string.Format("              [StringLength( {0}, ErrorMessage = \"{1}输入过长，不能超过{0}位\" )]", informationSchema.CharacterMaximumLength, informationSchema.ColumnComment));
                }

                if (informationSchema.DataType.Equals("int"))
                {
                    sb.AppendLine(
                        $"              [Range(1, {int.MaxValue})]");
                }
                sb.AppendLine($"              public  {informationSchema.DataType}  {informationSchema.ColumnName} {getSet}");
                sb.AppendLine();
            }
            sb.AppendLine("            }");
            sb.AppendLine("    }");
            return sb.ToString();
        }

        /// <summary>
        ///Dto模板
        /// </summary>
        /// <param name="tableInfoList"></param>
        /// <param name="tableName"></param>
        /// <param name="tableComment"></param>
        /// <returns></returns>
        public static string DtoTemplate(List<InformationSchema> tableInfoList, string tableName, string tableComment)
        {
            if (tableInfoList.Count <= 0)
            {
                throw new Exception($"找不到表{tableName}的相关信息");
            }
            var sb = new StringBuilder();
            var getSet = " { get; set; } ";
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Text;\r\n");
            sb.AppendLine($"namespace {tableName}.Dto");
            sb.AppendLine("    {\r\n");
            sb.AppendLine($"            /// <summary>");
            sb.AppendLine($"            /// 数据模型: {tableComment} ");
            sb.AppendLine($"            /// </summary>");
            sb.AppendLine($"            public class {tableName}Dto :EntityDto<{tableInfoList.FirstOrDefault()?.DataType}>");
            sb.AppendLine("            {");
            foreach (var informationSchema in tableInfoList.Skip(1))
            {
                sb.AppendLine($"              /// <summary>");
                sb.AppendLine($"              ///  {informationSchema.ColumnComment} ");
                sb.AppendLine($"              /// </summary>");
                if (!informationSchema.IsNullable)
                {
                    sb.AppendLine($"              [Required(ErrorMessage = \"{informationSchema.ColumnComment}不能为空\")]");
                }

                if (!string.IsNullOrEmpty(informationSchema.CharacterMaximumLength) && informationSchema.DataType.Equals("string"))
                {
                    sb.AppendLine(string.Format("              [StringLength( {0}, ErrorMessage = \"{1}输入过长，不能超过{0}位\" )]", informationSchema.CharacterMaximumLength, informationSchema.ColumnComment));
                }

                if (informationSchema.DataType.Equals("int"))
                {
                    sb.AppendLine(
                        $"              [Range(1, {int.MaxValue})]");
                }
                sb.AppendLine($"              public  {informationSchema.DataType}  {informationSchema.ColumnName} {getSet}");
                sb.AppendLine();
            }
            sb.AppendLine("            }");
            sb.AppendLine("    }");
            return sb.ToString();
        }

        /// <summary>
        ///PagedAndSortedResultRequestDto模板
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="tableComment"></param>
        /// <returns></returns>
        public static string PagedAndSortedResultRequestDtoTemplate(string tableName, string tableComment)
        {
            var sb = new StringBuilder();
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Text;\r\n");
            sb.AppendLine($"namespace {tableName}.Dto");
            sb.AppendLine("    {\r\n");
            sb.AppendLine($"            /// <summary>");
            sb.AppendLine($"            /// 分页排序模型: {tableComment} ");
            sb.AppendLine($"            /// </summary>");
            sb.AppendLine($"            public class {tableName}Dto :PagedAndSortedResultRequestDto");
            sb.AppendLine("            {\r\n");
            sb.AppendLine($"                public {tableName}PagedAndSortedResultRequestDto()");
            sb.AppendLine("                  {");
            sb.AppendLine("                       if (this.Sorting.IsNullOrWhiteSpace())");
            sb.AppendLine("                       {");
            sb.AppendLine($"                          Sorting = \"ReferenceNo Asc\";");
            sb.AppendLine("                       }");
            sb.AppendLine("                   }\r\n\r\n\r\n\r\n");
            sb.AppendLine("            }");
            sb.AppendLine("    }");
            return sb.ToString();
        }
    }
}