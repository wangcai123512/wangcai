using System;

namespace FMS.Model
{
    public class T_RecPayRecord
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        public string RP_GUID
        { get; set; }

        public string A_GUID
        { get; set; }

        /// <summary>
        /// 公司GUID
        /// </summary>
        public string C_GUID
        { get; set; }

        /// <summary>
        /// 收/付标识
        /// </summary>
        public string RP_Flag
        { get; set; }

        /// <summary>
        /// 单据类型
        /// </summary>
        public string InvType
        { get; set; }

        /// <summary>
        /// 单据类型
        /// </summary>
        public string InvTypeDts
        { get; set; }

        /// <summary>
        /// 单据号
        /// </summary>
        public string InvNo
        { get; set; }

        /// <summary>
        /// 收/付款方
        /// </summary>
        public string RPer
        { get; set; }

        /// <summary>
        /// 银行
        /// </summary>
        public string B_GUID
        { get; set; }

        /// <summary>
        /// 银行账号
        /// </summary>
        public string BA_GUID
        { get; set; }

        /// <summary>
        /// 借方总账科目
        /// </summary>
        public string DebitLedgerAccount
        { get; set; }

        /// <summary>
        /// 借方总账科目名称
        /// <remarks>扩展字段</remarks>
        /// </summary>
        public string DebitLedgerAccountName
        { get; set; }

        /// <summary>
        /// 借方明细科目
        /// </summary>
        public string DebitDetailsAccount
        { get; set; }

        /// <summary>
        /// 借方明细科目名称
        /// <remarks>扩展字段</remarks>
        /// </summary>
        public string DebitDetailsAccountName
        { get; set; }

        /// <summary>
        /// 贷方总账科目
        /// </summary>
        public string CreditLedgerAccount
        { get; set; }

        /// <summary>
        /// 贷方总账科目名称
        /// <remarks>扩展字段</remarks>
        /// </summary>
        public string CreditLedgerAccountName
        { get; set; }

        /// <summary>
        /// 贷方明细科目
        /// </summary>
        public string CreditDetailsAccount
        { get; set; }

        /// <summary>
        /// 贷方明细科目名称
        /// <remarks>扩展字段</remarks>
        /// </summary>
        public string CreditDetailsAccountName
        { get; set; }

        /// <summary>
        /// 发生金额
        /// </summary>
        public decimal SumAmount
        { get; set; }

        /// <summary>
        /// 发生日期
        /// </summary>
        public DateTime Date
        { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
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
        /// 收/付款方
        /// <remarks>扩展字段</remarks>
        /// </summary>
        public string R_PerName
        { get; set; }

        ///<summary>
        ///收/付款池
        ///<summary>
        public string RPable
        {
            get;set;
        }
        /// <summary>
        /// 常用币制
        /// </summary>
        public string Currency
        {
            get;set;
        }

        /// <summary>
        /// 单据类型名
        /// <remarks>扩展字段</remarks>
        /// </summary>
        public string TypeName
        {
            get;set;
        }

        /// <summary>
        /// 现金流量项目
        /// </summary>
        public string CFItemGuid
        {
            get;set;
        }

        /// <summary>
        /// 现金流量项目名称
        /// </summary>
        public string CFItemName
        {
            get;
            set;
        }

        /// <summary>
        /// 现金流量顶级项目
        /// </summary>
        public string CFPItemGuid
        {
            get;
            set;
        }

        /// <summary>
        /// 现金流量顶级项目名称
        /// </summary>
        public string CFPItemName
        {
            get;
            set;
        }

        /// <summary>
        /// 银行账号
        /// <remarks>扩展字段</remarks>
        /// </summary>
        public string BankAccount
        { get; set; }

        /// <summary>
        /// 唯一标识
        /// </summary>
        public string IE_GUID
        { get; set; }
    }
}
