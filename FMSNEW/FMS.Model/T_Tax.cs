using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FMS.Model
{
    /// <summary>
    /// 税费对象
    /// </summary>
    public class T_Tax
    {
        /// <summary>
        /// 税费标识
        /// </summary>
        public string T_GUID { get; set; }

        /// <summary>
        /// 税费名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 税率
        /// </summary>
        public decimal Rate { get; set; }

        /// <summary>
        /// 属性
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 所属公司
        /// </summary>
        public string C_GUID { get; set; }
       
    }
}
