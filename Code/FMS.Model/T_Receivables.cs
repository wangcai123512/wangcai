using System;

namespace FMS.Model
{
    public class T_Receivables
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        public string R_GUID
        { get; set; }

        /// <summary>
        /// 公司GUID
        /// </summary>
        public string C_GUID
        { get; set; }

        /// <summary>
        /// 付款人
        /// </summary>
        public string Payer
        { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        public DateTime Date
        { get; set; }

        public DateTime AffirmDate
        {
            get;
            set;
        }

        /// <summary>
        /// 类型
        /// </summary>
        public string InvType
        { get; set; }

        /// <summary>
        /// 单据号
        /// </summary>
        public string InvNo
        { get; set; }

        /// <summary>
        /// 借方总账科目
        /// </summary>
        public string B_GUID
        { get; set; }

        /// <summary>
        /// 借方明细科目
        /// </summary>
        public string BA_GUID
        { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        public decimal Money
        { get; set; }

        ///<summary>
        ///借方总账科目名称
        ///</summary>
        public string LA_Name
        {
            get;
            set;
        }

        ///<summary>
        ///借方明细科目名称
        ///</summary>
        public string DA_Name
        {
            get;
            set;
        }

        ///<summary>
        ///借款方名称
        ///</summary>
        public string RPerName
        {
            get;
            set;
        }

        /// <summary>
        ///<remarks>扩展字段</remarks>
        /// </summary>
        public string DebitLedgerAccount
        { get; set; }

        /// <summary>
        /// <remarks>扩展字段</remarks>
        /// </summary>
        public string DebitLedgerAccountName
        { get; set; }

        /// <summary>
        /// <remarks>扩展字段</remarks>
        /// </summary>
        public string DebitDetailsAccount
        { get; set; }

        /// <summary>
        /// <remarks>扩展字段</remarks>
        /// </summary>
        public string DebitDetailsAccountName
        { get; set; }

        /// <summary>
        ///<remarks>扩展字段</remarks>
        /// </summary>
        public string CreditLedgerAccount
        { get; set; }

        /// <summary>
        /// <remarks>扩展字段</remarks>
        /// </summary>
        public string CreditLedgerAccountName
        { get; set; }

        /// <summary>
        /// <remarks>扩展字段</remarks>
        /// </summary>
        public string CreditDetailsAccount
        { get; set; }

        /// <summary>
        /// <remarks>扩展字段</remarks>
        /// </summary>
        public string CreditDetailsAccountName
        { get; set; }

        /// <summary>
        /// <remarks>扩展字段</remarks>
        /// </summary>
        public string Remark
        {
            get;
            set;
        }
        /// <summary>
        /// <remarks>扩展字段</remarks>
        /// </summary>
        public string Creator
        {
            get;
            set;
        }
        /// <summary>
        /// <remarks>扩展字段</remarks>
        /// </summary>
        public string IE_Flag
        {
            get;
            set;
        }
        /// <summary>
        /// 常用币制
        /// </summary>
        public string Currency
        {
            get;
            set;
        }
        
    }
}
