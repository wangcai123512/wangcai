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

        /// <summary>
        /// 是否是最近记录
        /// </summary>
        public string CurrentRecord { get; set; }

        /// <summary>
        /// 统计货币
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// 更新日期(string)
        /// </summary>
        public string DateS { get; set; }

        /// <summary>
        /// 更新日期(string)
        /// </summary>
        public string VarString { get; set; }

        /// <summary>
        /// 汇率(string)
        /// </summary>
        public Decimal Rate { get; set; }

        public Decimal TRate { get; set; }
    }
}
