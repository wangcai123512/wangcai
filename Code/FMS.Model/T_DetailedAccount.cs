
namespace FMS.Model
{
    /// <summary>
    /// 明细科目
    /// </summary>
    public class T_DetailedAccount
    {
        /// <summary>
        /// 科目标识
        /// </summary>
        public string DA_GUID
        { get; set; }

        /// <summary>
        /// 公司标识
        /// </summary>
        public string C_GUID { get; set; }

        /// <summary>
        /// 科目代码
        /// </summary>
        public int AccCode
        { get; set; }

        /// <summary>
        /// 科目名称
        /// </summary>
        public string Name
        { get; set; }

        /// <summary>
        /// 上级科目名称
        /// </summary>
        public string ParentAccGuid
        { get; set; }

        public string hiddenName
        { get; set; }

        public string D_GUID
        { get; set; }
    }
}
