using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FMS.Model
{
    /// <summary>
    /// 税费对象
    /// </summary>
    public class T_WageCost
    {
        /// <summary>
        /// 工资标识
        /// </summary>
        public string W_GUID { get; set; }

        /// <summary>
        /// 所属公司
        /// </summary>
        public string C_GUID { get; set; }

        /// <summary>
        /// 工资发放日期
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// 职工
        /// </summary>
        public string Employee { get; set; }

        /// <summary>
        /// 基本工资
        /// </summary>
        public decimal Cash { get; set; }

        /// <summary>
        /// 个人所得税
        /// </summary>
        public decimal PersonalTaxes { get; set; }

        /// <summary>
        /// 社保福利
        /// </summary>
        public decimal SocialSecurity { get; set; }

        /// <summary>
        /// 个人总数
        /// </summary>
        public decimal Total { get; set; }
    }
}
