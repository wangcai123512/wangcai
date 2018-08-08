
namespace FMS.Model
{
    /// <summary>
    /// 报表明细对象
    /// </summary>
    public class T_ReportDetails
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        public string GUID { get; set; }

        /// <summary>
        /// 报表ID
        /// </summary>
        public string rep_guid { get; set; }

        /// <summary>
        /// 行次
        /// </summary>
        public string row_no { get; set; }
        /// <summary>
        /// 报表项科目名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 报表项期初值
        /// </summary>
        public decimal beginning_amount { get; set; }

        /// <summary>
        /// 报表项期末值
        /// </summary>
        public decimal ending_amount { get; set; }

       
       
 
    }
}
