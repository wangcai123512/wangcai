using System;

namespace FMS.Model
{
    public class T_IEWriteOff
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
        /// 收支标识
        /// </summary>
        public string IE_Flag
        { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        public DateTime Date
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
        /// 金额
        /// </summary>
        public decimal Amount
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
    }
}
