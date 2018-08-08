
namespace FMS.Model
{
    public class T_ThirdAccount
    {
        /// <summary>
        /// 科目标识
        /// </summary>
        public string TDA_GUID
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

        public string Amount
        { get; set; }

        public string State
        { get; set; }

        public string Level
        { get; set; }

        public string Mark
        { get; set; }

    }
}
