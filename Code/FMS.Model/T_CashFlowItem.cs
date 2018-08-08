using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FMS.Model
{
    /// <summary>
    /// 现金流量项目
    /// </summary>
    public class T_CashFlowItem
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        public string R_GUID { get; set; }

        /// <summary>
        /// 编号
        /// </summary>
        public int No { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set;}

        /// <summary>
        /// 父级编号
        /// </summary>
        public string PID { get; set; }

        /// <summary>
        /// 收支标识
        /// </summary>
        public string RP_Flag { get; set; }
    }
}
