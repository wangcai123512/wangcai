using System;

namespace FMS.Model
{
    public class T_IERecord
    {
        
        /// <summary>
        /// 隐藏名
        /// </summary>
        public string Log
        { get; set; }

        /// <summary>
        /// 凭证序列号
        /// </summary>
        public string id
        { get; set; }

        /// <summary>
        /// 隐藏名
        /// </summary>
        public string Log_c
        { get; set; }

        /// <summary>
        /// 唯一标识
        /// </summary>
        public string IE_GUID
        { get; set; }

        public string A_GUID
        { get; set; }
        public string Business_GUID
        { get; set; }

        public string BusinessName
        { get; set; }

        public string SubBusiness_GUID
        { set; get; }

        public string SubBusinessName
        { get; set; }
        
        ///<summary>
        ///收付款方式
        /// </summary>
        public string RPType
        { get; set; }
       ///<summary>
       ///收付款明细方式
       /// </summary>
        public string DetailRPType
        { get; set; }

        public string RPTypeID
        { get; set; }
        public string DetailRPTypeID
        { get; set; }
        /// <summary>
        /// 收入支出标识
        /// </summary>
        public string IE_Flag
        { get; set; }

        public string Flag
        {
            get;set;
        }
        public string RP_Flag
        { get; set; }
        /// <summary>
        /// 收付款科目
        /// </summary>
        public string RPLA_GUID
        {
            get;
            set;
        }
        /// <summary>
        /// 资产/负债类科目
        /// </summary>
        public string IELA_GUID
        {
            get;
            set;
        }
        /// <summary>
        /// 单据类型
        /// </summary>
        public string InvType
        { get; set; }

        public string RPInvType
        { get; set; }

        public string InvTypeDts
        { get; set; }
        public string CFItemGuid
        { get; set; }
        public string Record
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
        public string CreateDate
        { get; set; }

        /// <summary>
        /// 凭证日期
        /// </summary>
        public string VDate
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
        /// 投资收益/收入类别
        /// </summary>
        public string DetailInvtype
        {
            get;
            set;
        }

        public string DetailInvtype1
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
        /// 账期截止日期
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
        public Decimal bankAmount
        {
            get;
            set;
        }
        public Decimal Tax
        {
            get;
            set;
        }
        public Decimal domesticAmount
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
        public string TaxationGUID
        {
            get;
            set;
        }
        /// <summary>
        /// 已销金额
        /// </summary>
        public Decimal Amount_Used { get; set; }

        /// <summary>
        /// 未销金额
        /// </summary>
        public Decimal ResidualAmount { get; set; }
        /// <summary>
        /// 未销金额
        /// </summary>
        public Decimal DisAmount { get; set; }
        /// <summary>
        /// 总金额
        /// </summary>
        public Decimal SumAmount
        {
            get;
            set;
        }
        public Decimal SumAmount1
        {
            get;
            set;
        }
        public Decimal SumAmount2
        {
            get;
            set;
        }
        
        public Decimal TaxationAmount1
        {
            get;
            set;
        }

        public Decimal Amount1
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

        public string IEGroupID
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

        public string GUID
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
        /// 科目名称（损益类）
        /// </summary>
        public string Profit_Name
        { get; set; }
        /// <summary>
        /// 状态（销账）
        /// </summary>
        public string State
        {
            get;
            set;
        }

        /// <summary>
        /// 物料id
        /// </summary>
        public string BR_GUID
        {
            get;
            set;
        }
        /// <summary>
        /// pagename
        /// </summary>
        public string Pagename
        {
            get;
            set;
        }
        /// <summary>
        /// 凭证摘要
        /// </summary>
        public string Summary
        {
            get;
            set;
        }
        /// <summary>
        /// 凭证科目
        /// </summary>
        /// <summary>

        public string Name { get; set; }

        ///<summary>
        ///凭证二级科目
        ///</summary>

        public string DetailName { get; set; }

        ///<summary>
        ///凭证三级科目
        ///</summary>

        public string ThName { get; set; }


        /// 借方金额
        /// </summary>
        public Decimal DebitAmount { get; set; }
       
        /// <summary>
        /// 贷方金额
        /// </summary>
        public Decimal CreditAmount { get; set; }

        /// <summary>
        /// 物料金额
        /// </summary>
        public string BR_Amount
        {
            get;
            set;
        }
        /// <summary>
        /// 资产分类
        /// </summary>
        public string AssetType { get; set; }
        /// <summary>
        /// 物料类别名称
        /// </summary>
        public string AidTypeName { get; set; }

        /// <summary>
        /// 物料子类别名称
        /// </summary>
        public string ASTTypeName { get; set; }
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
        /// <summary>
        /// 转售金额
        /// </summary>
        public Decimal Resale_Amount { get; set; }

        /// <summary>
        ///应收款（天数）
        /// </summary>
        public string TotalDays
        {
            get;
            set;
        }
        /// <summary>
        ///应收款（逾期天数）
        /// </summary>
        public string OverdueDays
        {
            get;
            set;
        }
        /// <summary>
        ///应收款（占总数比例）
        /// </summary>
        public string ReceiveRatio
        {
            get;
            set;
        }
        /// <summary>
        ///（加权总天数）
        /// </summary>
        public string Sumday
        {
            get;
            set;
        }
        /// <summary>
        /// 总成本
        /// </summary>
        public string CompareMonth
        {
            get;
            set;
        }
        /// <summary>
        /// 工资使用凭证号
        /// </summary>
        public string AccountID
        {
            get;
            set;
        }
    }
}
