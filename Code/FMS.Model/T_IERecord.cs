using System;

namespace FMS.Model
{
    public class T_IERecord
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        public string IE_GUID
        { get; set; }

        public string A_GUID
        { get; set; }

        /// <summary>
        /// 收入支出标识
        /// </summary>
        public string IE_Flag
        { get; set; }

        /// <summary>
        /// 单据类型
        /// </summary>
        public string InvType
        { get; set; }

        /// <summary>
        /// 单据号
        /// </summary>
        public string InvNo
        { get; set; }

        /// <summary>
        /// 收付款方
        /// </summary>
        public string RPer
        { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string Creator
        { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate
        { get; set; }

        /// <summary>
        /// 收付款方名称
        /// <remarks>扩展字段</remarks>
        /// </summary>
        public string RPerName
        { get; set; }

        /// <summary>
        /// 唯一标识
        /// <remarks>扩展字段</remarks>
        /// </summary>
        public string R_GUID
        {
            get;
            set;
        }

        /// <summary>
        /// 付款人
        /// <remarks>扩展字段</remarks>
        /// </summary>
        public string Payer
        {
            get;
            set;
        }

        /// <summary>
        /// 公司GUID
        /// </summary>
        public string C_GUID
        {
            get;
            set;
        }

        /// <summary>
        /// 收款确认日期
        /// </summary>
        public DateTime AffirmDate
        {
            get;
            set;
        }
        /// <summary>
        /// 收款日期
        /// </summary>
        public DateTime Date
        {
            get;
            set;
        }

        /// <summary>
        /// 收入金额
        /// </summary>
        public Decimal Amount
        {
            get;
            set;
        }

        /// <summary>
        /// 税费金额
        /// </summary>
        public Decimal TaxationAmount
        {
            get;
            set;
        }

        /// <summary>
        /// 税种
        /// </summary>
        public string TaxationType
        {
            get;
            set;
        }

        /// <summary>
        /// 总金额
        /// </summary>
        public Decimal SumAmount
        {
            get;
            set;
        }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get;
            set;
        }

        /// <summary>
        /// 货币
        /// </summary>
        public string Currency
        {
            get;
            set;
        }

        /// <summary>
        /// 收款银行
        /// </summary>
        public string B_GUID
        {
            get;
            set;
        }
        /// <summary>
        /// 收款账户
        /// </summary>
        public string BA_GUID
        {
            get;
            set;
        }

        /// <summary>
        /// 详细类别
        /// </summary>
        public string IEGroup
        {
            get;
            set;
        }

        /// <summary>
        /// 详细类别描述
        /// </summary>
        public string IEDescription
        {
            get;
            set;
        }

        /// <summary>
        /// 收付款标识（销账）
        /// </summary>
        public string RP_GUID
        {
            get;
            set;
        }

        public string Profit_GUID
        {
            get;
            set;
        }

        /// <summary>
        /// 状态（销账）
        /// </summary>
        public string State
        {
            get;
            set;
        }

        /// <summary>
        /// 状态
        /// <remarks>扩展字段</remarks>
        /// </summary>
        private string _status;
        public string Status {
            get
            {
               // return _status.Equals("1") ? "已收" : "应收";
                return _status;
            }
            set
            {
                _status = value.Equals (true.ToString())?"1":"0";
            }
        }
    }
}
