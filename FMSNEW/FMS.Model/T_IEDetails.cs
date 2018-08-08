
namespace FMS.Model
{
    public class T_IEDetails
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        public string R_GUID
        { get; set; }

        /// <summary>
        /// 收入支出记录唯一标识
        /// </summary>
        public string IE_GUID
        { get; set; }

        /// <summary>
        /// 借方总账科目
        /// </summary>
        public string DebitLedgerAccount
        { get; set; }

        /// <summary>
        /// 借方明细科目
        /// </summary>
        public string DebitDetailsAccount
        { get; set; }

        /// <summary>
        /// 贷方总账科目
        /// </summary>
        public string CreditLedgerAccount
        { get; set; }

        /// <summary>
        /// 贷方明细科目
        /// </summary>
        public string CreditDetailsAccount
        { get; set; }

        /// <summary>
        /// 发生金额
        /// </summary>
        public decimal Money
        { get; set; }

        /// <summary>
        /// 解放总账科目名称
        /// 扩展字段
        /// </summary>
        public string DebitLedgerAccountName
        { get; set; }

        /// <summary>
        /// 借方明细科目名称
        /// 扩展字段
        /// </summary>
        public string DebitDetailsAccountName
        { get; set; }

        /// <summary>
        /// 贷方总账科目名称
        /// 扩展字段
        /// </summary>
        public string CreditLedgerAccountName
        { get; set; }

        /// <summary>
        /// 贷方明细科目名称
        /// 扩展字段
        /// </summary>
        public string CreditDetailsAccountName
        { get; set; }

        /// <summary>
        /// 公司GUID
        /// </summary>
        public string C_GUID
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
        /// 常用币制
        /// </summary>
        public string Currency
        {
            get;
            set;
        }
    }
}
