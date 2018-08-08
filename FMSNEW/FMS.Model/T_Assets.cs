using System;

namespace FMS.Model
{
    /// <summary>
    /// 固定资产对象
    /// </summary>
    public class T_Assets
    {
        /// <summary>
        /// 固定资产唯一标识
        /// </summary>
        public string A_GUID
        { get; set; }

        /// <summary>
        /// 固定资产编号
        /// </summary>
        public string No
        { get; set; }

        /// <summary>
        /// 固定资产名称
        /// </summary>
        public string Name
        { get; set; }

        /// <summary>
        /// 注册日期
        /// </summary>
        public DateTime RegisterDate
        { get; set; }

        /// <summary>
        /// 采购日期
        /// </summary>
        public DateTime PurchaseDate
        { get; set; }

        /// <summary>
        /// 报废类型
        /// </summary>
        public string ScrapType
        { get; set; }

        /// <summary>
        /// 报废日期
        /// </summary>
        public DateTime ScrapDate
        { get; set; }

        /// <summary>
        /// 固定资产分组标识
        /// </summary>
        public string AG_GUID
        { get; set; }

        /// <summary>
        /// 固定资产原值
        /// </summary>
        public decimal AssetsCost
        { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string Creator
        { get; set; }

        /// <summary>
        /// 当前资产值
        /// <remarks>非表字段</remarks>
        /// </summary>
        public decimal CurrentValue
        { get; set; }

        /// <summary>
        /// 公司GUID
        /// </summary>
        public string C_GUID
        {
            get;
            set;
        }
    }
}
