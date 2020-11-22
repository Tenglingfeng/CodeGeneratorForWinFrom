using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CodeGenerator
{
    public class InformationSchema
    {
        /// <summary>
        /// 列名
        /// </summary>
        public string ColumnName { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public string DataType { get; set; }

        /// <summary>
        /// 注释
        /// </summary>

        public string ColumnComment { get; set; }

        /// <summary>
        /// 表说明
        /// </summary>
        public string TableComment { get; set; }

        /// <summary>
        /// 是否为空
        /// </summary>
        public bool IsNullable { get; set; }

        /// <summary>
        /// 字符长度
        /// </summary>
        public string CharacterMaximumLength { get; set; }
    }
}