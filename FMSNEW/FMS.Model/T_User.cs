
namespace FMS.Model
{
    /// <summary>
    /// 用户对象
    /// </summary>
    public class T_User
    {
        /// <summary>
        /// 用户标识
        /// </summary>
        public string U_GUID
        { get; set; }

        /// <summary>
        /// 公司标识
        /// </summary>
        public string C_GUID
        { get; set; }
        /// <summary>
        /// 登录手机号
        /// </summary>
        public string TelName
        { get; set; }
        /// <summary>
        /// 登录名
        /// </summary>
        public string LoginName
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
        /// <summary>
        /// 后台标识
        /// </summary>
        public string SuperUser
        { get; set; }

        public string NickName
        { get; set; }

        public string CreateDate
        { get; set; }

        public string GroupCode
        { get; set; }

        public string Language
        { get; set; }

        public string Count
        { get; set; }

        public string UCount
        { get; set; }
        public string MCount
        { get; set; }
        public string id
        { get; set; }
    }
}

