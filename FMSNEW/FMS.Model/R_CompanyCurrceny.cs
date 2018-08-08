using System;

namespace FMS.Model
{
    /// <summary>
    /// 公司币值关系
    /// </summary>
    public class R_CompanyCurrceny
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        public string R_GUID
        {
            get;
            set;
        }

        /// <summary>
        /// 公司标识
        /// </summary>
        public string C_GUID
        {
            get;
            set;
        }

        /// <summary>
        /// 币值代码
        /// </summary>
        public string Code
        {
            get;
            set;
        }
    }
}
