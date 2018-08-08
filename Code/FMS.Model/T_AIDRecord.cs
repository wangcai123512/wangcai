using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FMS.Model
{
    /// <summary>
    /// 物料，资产对象
    /// </summary>
    public class T_AIDRecord
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
        /// 购进日期
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        public Decimal Amount { get; set; }

        /// <summary>
        /// 货币
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// 供应商
        /// </summary>
        public string RPer { get; set; }

        /// <summary>
        /// 物料类别
        /// </summary>
        public string InvType { get; set; }

        /// <summary>
        /// 物料描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 折旧周期（月）
        /// </summary>
        public int DepreciationPeriod { get; set; }

        /// <summary>
        /// 资产，直接物料，间接物料判别标志
        /// </summary>
        public string AID_Flag { get; set; }

        /// <summary>
        /// 剩余价值
        /// </summary>
        public Decimal SurplusValue { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string RPerName { get; set; }

        /// <summary>
        /// 附件
        /// </summary>
        public string A_GUID { get; set; }
    }
}
