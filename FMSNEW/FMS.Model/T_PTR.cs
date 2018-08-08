
namespace FMS.Model
{
    /// <summary>
    /// 物料类别
    /// </summary>
    public class T_PTR
    {
        /// <summary>
        /// 公用ID
        /// </summary>
        public string GUID
        { get; set; }

        /// <summary>
        /// 主类别ID
        /// </summary>
        public string AT_GUID
        { get; set; }

        /// <summary>
        /// 主类别名
        /// </summary>
        public string AidTypeName
        { get; set; }

        /// <summary>
        /// 类别FLAG
        /// </summary>
        public string AID_FLAG
        { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public string C_GUID
        { get; set; }

        /// <summary>
        /// 子类别备注
        /// </summary>
        public string Remark
        { get; set; }

        /// <summary>
        /// 子类别父类ID
        /// </summary>
        public string AST_ParentAidType
        { get; set; }

        /// <summary>
        /// 子类别名
        /// </summary>
        public string ASTTypeName
        { get; set; }

        /// <summary>
        /// 子类别ID
        /// </summary>
        public string AST_GUID
        { get; set; }

        /// <summary>
        /// 折旧年份
        /// </summary>
        public string Depreciation_year
        { get; set; }

        /// <summary>
        /// 资产分类
        /// </summary>
        public string Asset_class
        { get; set; }

        /// <summary>
        /// 通过父类别名查询子类类别
        /// </summary>
        public string invtype { get; set; }

        /// <summary>
        /// 物料id
        /// </summary>
        public string MM_GUID { get; set; }

        /// <summary>
        /// 物料名
        /// </summary>
        public string MM_Name { get; set; }

        /// <summary>
        /// 物料类别id
        /// </summary>
        public string Parent { get; set; }

        /// <summary>
        /// 物料flag
        /// </summary>
        public string MM_FLAG { get; set; }
    }
}

