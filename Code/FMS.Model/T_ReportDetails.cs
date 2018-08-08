
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
        public string RGUID { get; set; }

        /// <summary>
        /// 报表标识
        /// </summary>
        public string Rep_GUID { get; set; }

        /// <summary>
        /// 报表项科目代码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 报表项科目名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 报表项期初值
        /// </summary>
        public decimal BeginningValue { get; set; }

        /// <summary>
        /// 报表项期末值
        /// </summary>
        public decimal EndingValue { get; set; }

        /// <summary>
        /// 报表项科目分类
        /// <remarks>扩展字段</remarks>
        /// </summary>
        public string AccGrp { get; set; }
    }
}
