
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
    }
}

