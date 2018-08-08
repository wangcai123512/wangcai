
namespace FMS.Model
{
    /// <summary>
    /// 币制对象
    /// </summary>
    public class T_ExpenseType
    {
        /// <summary>
        /// 费用类别GUID
        /// </summary>
        public string ET_GUID
        { get; set; }

        /// <summary>
        /// 费用类别
        /// </summary>
        public string ExpenseType
        { get; set; }

        /// <summary>
        /// 营业成本标识
        /// </summary>
        public string ExpenseFlag
        { get; set; }

        /// <summary>
        /// 销售费用标识
        /// </summary>
        public string SaleFlag
        { get; set; }

        /// <summary>
        /// 管理费用标识
        /// </summary>
        public string ManageFlag
        { get; set; }

        /// <summary>
        /// 财务费用标识
        /// </summary>
        public string FinanceFlag
        { get; set; }

        /// <summary>
        /// 标识
        /// </summary>
        public string OtherFlag
        { get; set; }
        
        /// <summary>
        /// 税费计提标识
        /// </summary>
        public string TaxFlag
        { get; set; }
    }
}
