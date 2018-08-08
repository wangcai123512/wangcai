using System;
using System.Security.Principal;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Security;
using SSOModel;

namespace Utilities
{
    public class CustomPrincipal : IPrincipal
    {
        private IIdentity _identity;
        private UserData _userData;

        public CustomPrincipal()
        { }

        public CustomPrincipal(FormsAuthenticationTicket ticket, UserData userData)
        {
            if (ticket == null)
            {
                throw new ArgumentNullException("ticket");
            }
            else if (userData == null)
            {
                throw new ArgumentNullException("userData");
            }
            else
            {
                _identity = new FormsIdentity(ticket);
                _userData = userData;
            }
        }

        public UserData UserData
        {
            get { return _userData; }
        }

        public IIdentity Identity
        {
            get { return _identity; }
        }

        public bool IsInRole(string role)
        {

            return true;
        }


        /// <summary>
        /// 执行用户登录操作
        /// </summary>
        /// <param name="loginName">登录名</param>
        /// <param name="userData">与登录名相关的用户信息</param>
        /// <param name="expiration">登录Cookie的过期时间，单位：分钟。</param>
        public static void SignIn(UserData userData, int expiration = 0)
        {
            HttpContext context = HttpContext.Current;
            string data = null;
            if (string.IsNullOrEmpty(userData.LoginName))
            {
                throw new ArgumentNullException("loginName");
            }
            else if (userData == null)
            {
                throw new ArgumentNullException("userData");
            }
            else if (context == null)
            {
                throw new InvalidOperationException();
            }
            else
            {
                // 1. 把需要保存的用户数据转成一个字符串。
                data = (new JavaScriptSerializer()).Serialize(userData);
                // 2. 创建一个FormsAuthenticationTicket，它包含登录名以及额外的用户数据。
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                    2, userData.LoginName.ToLower(), DateTime.Now, DateTime.Now.AddDays(1), false, data);
                // 3. 加密Ticket，变成一个加密的字符串。
                string cookieValue = FormsAuthentication.Encrypt(ticket);
                // 4. 根据加密结果创建登录Cookie
                HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, cookieValue);
                cookie.HttpOnly = true;
                if (expiration > 0)
                {
                    cookie.Expires = DateTime.Now.AddMinutes(expiration);
                }
                else
                {

                }
                // 5. 写登录Cookie
                context.Response.Cookies.Remove(cookie.Name);
                context.Response.Cookies.Add(cookie);
            }
        }

        /// <summary>
        /// 根据HttpContext对象设置用户标识对象
        /// </summary>
        /// <param name="context"></param>
        public static void TrySetUserInfo(HttpContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            else
            {
                // 1. 读登录Cookie
                HttpCookie cookie = context.Request.Cookies[FormsAuthentication.FormsCookieName];
                if (cookie == null || string.IsNullOrEmpty(cookie.Value))
                {
                    return;
                }
                else
                {

                }
                try
                {
                    UserData userData = null;
                    // 2. 解密Cookie值，获取FormsAuthenticationTicket对象
                    FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);

                    if (ticket != null && string.IsNullOrEmpty(ticket.UserData) == false)
                    // 3. 还原用户数据
                    {
                        userData = (new JavaScriptSerializer()).Deserialize<UserData>(ticket.UserData);
                    }
                    else
                    {

                    }
                    if (ticket != null && userData != null)
                    {
                        context.User = new CustomPrincipal(ticket, userData);
                    }
                    else
                    {

                    }
                    // 4. 构造我们的MyFormsPrincipal实例，重新给context.User赋值。

                }
                catch { /* 有异常也不要抛出，防止攻击者试探。 */ }
            }
        }
    }
}
