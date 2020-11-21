using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGenerator
{
  public  class InformationSchema
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
    }
}
