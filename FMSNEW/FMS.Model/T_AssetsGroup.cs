
namespace FMS.Model
{
    /// <summary>
    /// 固定资产分类对象
    /// </summary>
    public class T_AssetsGroup
    {
        /// <summary>
        /// 分组标识
        /// </summary>
        public string AG_GUID
        { get; set; }

        /// <summary>
        /// 分组名称
        /// </summary>
        public string Name
        { get; set; }

        /// <summary>
        /// 折旧方法
        /// </summary>
        public int DepreciationMethod
        { get; set; }

        /// <summary>
        /// 使用寿命
        /// </summary>
        public int Life
        { get; set; }

        /// <summary>
        /// 残值率
        /// </summary>
        public decimal SalvageRate
        { get; set; }

        /// <summary>
        /// 当前公司GUID
        /// </summary>
        public string C_GUID
        { get; set; }
    }
}
