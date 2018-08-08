using System;
using System.Configuration;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Common.CommonFunction;
using SSOModel;
using Utilities;

namespace BaseController
{
    public class BasicController : Controller
    {
        protected string SubSystemID = Convert.ToString(ConfigurationManager.AppSettings["SubSystemID"]);
        protected string AccountID = Convert.ToString(ConfigurationManager.AppSettings["AccountID"]);
        protected string SystemName = Convert.ToString(ConfigurationManager.AppSettings["SystemName"]);
        protected string RMUrl = string.Empty;
        protected bool SSO = bool.Parse(ConfigurationManager.AppSettings["SSO"]);
        protected string LoginUrl = ConfigurationManager.AppSettings["SSOCloudURLAddress"];
        protected UserData userData
        { get; set; }


        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            RMUrl = requestContext.HttpContext.Application["RMUrl"].ToString();
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            if (SSO)
            {
                InitLanguage(requestContext);
                try
                {
                    CustomPrincipal.TrySetUserInfo(requestContext.HttpContext.ApplicationInstance.Context);
                    userData = ((CustomPrincipal)HttpContext.User).UserData;
                    //WebUser.SignInOfGener(
                    //    new Emp() { No = userData.LoginName, Name = userData.LoginFullName }
                    //    , Session["Culture"].ToString(), userData.LoginName, true);
                }
                catch (Exception)
                {
                    string cloudUrl = ConfigurationManager.AppSettings["SSOCloudURLAddress"];
                    requestContext.HttpContext.Response.Redirect(cloudUrl, true);
                    return;
                }
                if (!userData.IsSuperAdmin
                    && (Session["IsLogOut"] ?? "1").Equals("1"))
                {
                    Session["IsLogOut"] = "0";
                    Session["Employee"] = userData;
                    Session["LogicGUID"] = Guid.NewGuid().ToString();
                    Session["Timeout"] = Session.Timeout;
                    Session["IP"] = this.HttpContext.Request.UserHostAddress;
                    new Log().Loger(userData, Session["LogicGUID"].ToString(), Session["IP"].ToString(),
                        RMUrl, Session.Timeout, true);
                }
                else
                { }
            }
            else
            {
                ResourceLoader.SetCurrentThreadCulture(Session);
                try
                {
                    userData = ((CustomPrincipal)HttpContext.User).UserData;
                }
                catch (Exception)
                {

                }
            }
        }

        private void InitLanguage(RequestContext requestContext)
        {
            HttpCookie cooke = requestContext.HttpContext.Request.Cookies["language"];
            string resultStr = "zh-CN";
            if (cooke == null || string.IsNullOrEmpty(cooke.Value))
            {
                resultStr = "zh-CN";
            }
            else
            {
                resultStr = cooke.Value;
            }
            Session["Culture"] = new System.Globalization.CultureInfo(resultStr);
        }

    }
}