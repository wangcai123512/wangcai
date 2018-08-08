using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FMS.Model
{
   public class R_UserCompany
    {
        public string UC_GUID
        { get; set; }

        public string U_GUID
        { get; set; }

        public string C_GUID
        { get; set; }

        /// </summary>
        public string LoginName
        { get; set; }

        public string TelName
        { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName
        { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password
        { get; set; }

        /// <summary>
        /// 状态标识
        /// 0：不可进入 1：管理员 2：其他用户
        /// </summary>
        public int State
        { get; set; }

        /// <summary>
        /// 可进入公司标识
        /// </summary>
        public string EnterC_GUID
        { get; set; }

        public string NickName
        { get; set; }

        public string CreateDate
        { get; set; }

        public string GroupCode
        { get; set; }
    }
}
