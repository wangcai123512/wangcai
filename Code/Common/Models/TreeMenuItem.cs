
namespace Common.Models
{
    public class TreeMenuItem
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        public string GUID { get; set; }
        /// <summary>
        /// 中文名
        /// </summary>
        public string ChineseName { get; set; }
        /// <summary>
        /// 英文名
        /// </summary>
        public string EnglishName { get; set; }

        /// <summary>
        /// 系统名称
        /// </summary>
        public string SystemName { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int OrderNumber { get; set; }
        /// <summary>
        /// 父节点编码
        /// </summary>
        public string ModuleID { get; set; }
        /// <summary>
        /// 是否显示在树
        /// </summary>
        public bool IsShowTree { get; set; }
        /// <summary>
        /// 是否最末尾的结点
        /// </summary>
        public bool IsLastChild { get; set; }
        /// <summary>
        /// URL链接路径
        /// </summary>
        public string URL { get; set; }
        /// <summary>
        /// 权限验证Code
        /// </summary>
        public string SubfunctionCode { get; set; }
        /// <summary>
        /// 功能模块  0-不可用，1-模块，2-功能，3-功能+模块
        /// </summary>
        public int ModuleState { get; set; }
        public int Level { get; set; }
        public int Block { get; set; }
    }
}
