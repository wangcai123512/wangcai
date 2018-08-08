using System;
using System.Configuration;
using System.Web;
using General.Resource;
using Utilities;

namespace HttpModules
{
    public class AuthenticateRequest : IHttpModule
    {
        bool sso = bool.Parse(ConfigurationManager.AppSettings["SSO"]);
        public void Dispose()
        {

        }

        public void Init(HttpApplication context)
        {
            context.AuthenticateRequest += new EventHandler(this.HRAuthenticateRequest);
            context.AcquireRequestState += new EventHandler(HRAcquireRequestState);
        }

        private void HRAuthenticateRequest(object objSender, EventArgs objEventArgs)
        {
            //首先获取用户登陆信息
            HttpApplication app = (HttpApplication)objSender;
            CustomPrincipal.TrySetUserInfo(app.Context);
            if (!IsBypass(app) &&
                (HttpContext.Current.User == null ||
                    HttpContext.Current.User.Identity == null ||
                    !HttpContext.Current.User.Identity.IsAuthenticated))
            {
                //HttpContext.Current.Response.Redirect("/Common/RedirectToLogin?Msg=" + Common.Access_Denied, true);
            }
            else
            {

            }
        }

        private void HRAcquireRequestState(object objSender, EventArgs objEventArgs)
        {
            HttpApplication app = (HttpApplication)objSender;

            if (!IsBypass(app) &&
                string.IsNullOrEmpty(HttpContext.Current.Request.CurrentExecutionFilePathExtension)
                && app.Context.Session.IsNewSession
                && !sso
                    && HttpContext.Current.Request.RawUrl.ToLower().Length != 1)
            {
                if (HttpContext.Current.Request.RequestContext.HttpContext.User.Identity.IsAuthenticated)
                {
                    HttpContext.Current.Response.Redirect("/Common/RedirectToLogin?Msg=" + Common.Login_in_Time_Out, true);
                }
                else
                {
                    HttpContext.Current.Response.Redirect("/Common/RedirectToLogin?Msg=" + Common.Access_Denied, true);
                }
            }
            else
            {

            }
        }

        /// <summary>
        /// 是否绕行
        /// </summary>
        /// <returns></returns>
        private bool IsBypass(HttpApplication app)
        {
            //如果filePathExtension为空则为asp.net mvc 需要控制
            string filePathExtension = HttpContext.Current.Request.CurrentExecutionFilePathExtension;
            string currentRequestUrl = HttpContext.Current.Request.RawUrl.ToLower();
            if (!string.IsNullOrEmpty(filePathExtension) ||
                string.IsNullOrEmpty(HttpContext.Current.Request.RawUrl) ||
                currentRequestUrl.Length == 1 ||
                currentRequestUrl.Contains("login") ||
                currentRequestUrl.Contains("/Common/") ||
                currentRequestUrl.Contains("restful") ||
                currentRequestUrl.Contains("entryapplication/"))
            {
                //如果长度为1就说明为登录页面不需要跳转
                //Login/GetValidateCode 获取校验码 不需要跳转
                //Login/Register 注册页面也不需要跳转
                //EntryApplication/AppearEntryApplication
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
