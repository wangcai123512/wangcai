using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FMS.Model
{
    /// <summary>
    /// 银行对象
    /// </summary>
    public class T_Bank
    {
        /// <summary>
        /// 银行对象标识
        /// </summary>
        public string B_GUID { get; set; }

        /// <summary>
        /// 银行名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 公司标识
        /// </summary>
        public string C_GUID { get; set; }
    }
}
