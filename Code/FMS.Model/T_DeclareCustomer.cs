
using System;

namespace FMS.Model
{
    public class T_DeclareCustomer
    {
        /// <summary>
        /// 标识
        /// </summary>
        public string GUID
        { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public string InvType
        { get; set; }

        /// <summary>
        /// 付款方
        /// </summary>
        public string RPer
        { get; set; }

        /// <summary>
        /// 申报金额
        /// </summary>
        public Decimal Amount
        { get; set; }

        /// <summary>
        /// 货币
        /// </summary>
        public string Currency
        { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public string State
        { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        { get; set; }

        /// <summary>
        /// 申报日期
        /// </summary>
        public DateTime Date
        { get; set; }

        public string C_GUID
        { get; set; }

        /// <summary>
        /// 拓展字段
        /// </summary>
        public string RPerName
        { get; set; }

        /// <summary>
        /// 拓展字段
        /// </summary>
        public string AGUID
        { get; set; }
    }
}

