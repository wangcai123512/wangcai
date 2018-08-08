
namespace FMS.Model
{
    /// <summary>
    /// 总账科目
    /// </summary>
    public class T_GeneralLedgerAccount
    {
        /// <summary>
        /// 科目标识
        /// </summary>
        public string LA_GUID
        { get; set; }

        public string DA_GUID
        { get; set; }
        public string TDA_GUID
        {get;set;}
        public string Mark
        { get; set; }
        public string ParentAccGuid
        { get; set; }
        public string State
        {
            get;
            set;
        }
        public decimal Amount
        {
            get;
            set;
        }
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

        public string UinName
        { get; set; }
        /// <summary>
        /// 是否可用
        /// <remarks>扩展字段</remarks>
        /// </summary>
        public bool Useable
        { get; set; }

        /// <summary>
        /// 科目类别
        /// </summary>
        public int AccGroup
        { get; set; }

        /// <summary>
        /// 是否锁定
        /// <remarks>扩展字段</remarks>
        /// </summary>
        public bool IsLocked
        { get; set; }
    }
}
