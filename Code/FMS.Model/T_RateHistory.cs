using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FMS.Model
{
    /// <summary>
    /// 汇率更新
    /// </summary>
    public class T_RateHistory
    {
        /// <summary>
        /// 对象标识
        /// </summary>
        public string GUID { get; set; }

        /// <summary>
        /// 公司标识
        /// </summary>
        public string C_GUID { get; set; }

        /// <summary>
        /// 更新日期
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// 要兑换的金额
        /// </summary>
        public Decimal FAmount { get; set; }

        /// <summary>
        /// 要兑换的货币
        /// </summary>
        public string FCurrency { get; set; }

        /// <summary>
        /// 被兑换的金额
        /// </summary>
        public Decimal TAmount { get; set; }

        /// <summary>
        /// 兑换出的货币
        /// </summary>
        public string TCurrency { get; set; }
    }
}
