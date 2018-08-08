
using System;

namespace FMS.Model
{
    public class T_DeclareCostSpending
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
        public string DtsInvType
        { get; set; }

        public string DtsInvTypeID
        { get; set; }
        public string Business_GUID
        { get; set; }

        public string SubBusiness_GUID
        { set; get; }

        public string BusinessName
        { get; set; }

        public string SubBusinessName
        { get; set; }
        /// <summary>
        /// 收款方
        /// </summary>
        public string RPer
        { get; set; }

        public string RPType
        { get; set; }

        public string DetailRPType
        { get; set; }
        public string RPTypeID
        { get; set; }
        public string DetailRPTypeID
        { get; set; }
        /// <summary>
        /// 已销金额
        /// </summary>
        public Decimal Amount_Used { get; set; }

        /// <summary>
        /// 未销金额
        /// </summary>

        public Decimal DisAmount { get; set; }
        public Decimal DisAmount1 { get; set; }
        public Decimal ResidualAmount { get; set; }
        /// <summary>
        /// 申请金额
        /// </summary>
        public Decimal Amount
        { get; set; }

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
        /// 借方金额
        /// </summary>
        public Decimal DebitAmount { get; set; }

        /// <summary>
        /// 贷方金额
        /// </summary>
        public Decimal CreditAmount { get; set; }


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
        /// 记录状态
        /// </summary>
        public string Record
        { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        { get; set; }

        /// <summary>
        /// 申请日期
        /// </summary>
        public string Date
        { get; set; }
        
        /// <summary>
        /// 创建日期
        /// </summary>
        public string CreateDate
        { get; set; }

        /// <summary>
        /// 凭证日期
        /// </summary>
        public string VDate
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
        ///<summary>
        ///凭证号
        ///<summary>
        public string VoucherNo
        { get; set; }
        
        public string Profit_GUID
        {
            get;
            set;
        }
        public string DetailPro_GUID
        { get; set; }
        public string Profit_Name
        { get; set; }
        /// <summary>
        /// 已转成本金额
        /// </summary>
        public Decimal CostSum { get; set; }

        public string AccountID { get; set; }
    }
}

