using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeGenerator.Template
{
    public static class ValidationTemplate
    {
        ///  <summary>
        /// Validation模板
        ///  </summary>
        ///  <param name="tableInfoList"></param>
        ///  <param name="tableName"></param>
        ///  <param name="tableComment"></param>
        ///  <param name="projectName"></param>
        ///  <returns></returns>
        public static string ValidatorTemplate(List<InformationSchema> tableInfoList, string tableName, string tableComment, string projectName)
        {
            if (tableInfoList.Count <= 0)
            {
                throw new Exception($"找不到表{tableName}的相关信息");
            }
            var sb = new StringBuilder();
            sb.AppendLine("using FluentValidation;");
            sb.AppendLine($"namespace Benchint.{projectName}.Validation.{tableName}");
            sb.AppendLine("    {\r\n");
            sb.AppendLine($"            /// <summary>");
            sb.AppendLine($"            /// 数据模型验证: {tableComment} ");
            sb.AppendLine($"            /// </summary>");
            sb.AppendLine($"            public class {tableName}Validator : BaseAbstractValidator<{tableName}>");
            sb.AppendLine("            {");
            sb.AppendLine($"                    public {tableName}Validator()");
            sb.AppendLine("                    {");
            sb.AppendLine("                         SetRules();");
            sb.AppendLine("                     }");
            sb.AppendLine("                     private void SetRules()");
            sb.AppendLine("                     {");

            foreach (var informationSchema in tableInfoList)
            {
                if (informationSchema.IsPrimary)
                {
                    continue;
                }
                var columnName = informationSchema.ColumnName;
                if (columnName.Equals("ResidenterId"))
                {
                    columnName = "UserHealthDocNo";
                    informationSchema.DataType = "string";
                }
                else if (columnName.Equals("OrganizationId"))
                {
                    columnName = "OrganizationNo";
                    informationSchema.DataType = "string";
                }

                if (!informationSchema.IsNullable)
                {
                    if (informationSchema.DataType == "string" && columnName != "UserHealthDocNo" && columnName != "OrganizationNo")
                    {
                        sb.AppendLine($"                                RuleFor(x => x.{columnName})");
                        sb.AppendLine($"                                .NotEmpty().WithMessage(\"{informationSchema.ColumnComment} 不能为空\")");
                        sb.AppendLine(
                            $"                                          .MaximumLength({informationSchema.CharacterMaximumLength})" +
                            $".WithMessage(\"{informationSchema.ColumnComment} 输入过长" +
                            $"，不能超过{informationSchema.CharacterMaximumLength}位\");");
                    }
                    else
                    {
                        sb.AppendLine($"                                RuleFor(x => x.{columnName})");
                        sb.AppendLine($"                                .NotEmpty().WithMessage(\"{informationSchema.ColumnComment} 不能为空\");");
                    }
                }
                else
                {
                    if (informationSchema.DataType == "string" && columnName != "UserHealthDocNo" && columnName != "OrganizationNo")
                    {
                        sb.AppendLine($"                         RuleFor(x => x.{columnName})");
                        sb.AppendLine(
                            $"                                  .MaximumLength({informationSchema.CharacterMaximumLength})" +
                            $".WithMessage(\"{informationSchema.ColumnComment} 输入过长，" +
                            $"不能超过{informationSchema.CharacterMaximumLength}位\");");
                    }
                }
            }
            sb.AppendLine("                     }");
            sb.AppendLine("            }");
            sb.AppendLine("    }");
            return sb.ToString();
        }
    }
}