using System;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using Common.CommonFunction;
using FMS.WebUI.SaaS_Service;
using SSOModel;
using System.Web;
using System.Web.Security;

namespace FMS.WebUI
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Common", action = "Login", id = UrlParameter.Optional }, // Parameter defaults
                new string[] { "Common" }
                );

            routes.MapRoute(
                "RolePermission", // Route name
                "RolePermission/{action}/{id}", // URL with parameters
                new { controller = "RolePermission", action = "Index", id = UrlParameter.Optional }, // Parameter defaults
                new string[] { "Common" }
                );
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
            //using (SaaS_ServiceClient client = new SaaS_ServiceClient())
            //{
            //    string rmUrl = client.GetRMInterfacePath(
            //       Convert.ToString(ConfigurationManager.AppSettings["SubSystemID"]),
            //       Convert.ToString(ConfigurationManager.AppSettings["AccountID"])
            //       );
            //    Application.Add("RMUrl", rmUrl);
            //    Application.Add("SaasModules",
            //        client.GetSubSystemModuleIDComposeForAdmin(ConfigurationManager.AppSettings["SubSystemID"], ConfigurationManager.AppSettings["AccountID"])
            //    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList());
            //}
            Application.Add("Functions", new Common.DAL.Common().GetTreeMenuItem());
        }

        protected void Session_Start(object sender, EventArgs e)
        {
        }

        protected void Session_End(object sender, EventArgs e)
        {
            UserData dat = Session["Employee"] as UserData;

                if (Application.AllKeys.Contains(dat.LoginName)
                && Application[dat.LoginName].Equals(Session.SessionID))
            {
                Application.Remove(Session["EmployeeNumber"].ToString());
            }
            //如果没有登出的话就执行登出session日志
            if (Session["IsLogOut"] != "1")
            {
                new Log().Loger(dat, Session["LogicGUID"].ToString(), Session["IP"].ToString(),
                    Application["RMUrl"].ToString(), Convert.ToInt32(Session["Timeout"]), false);
            }
        }

        #region Uploadify控件上传附件Bug修复
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            try
            {
                string session_param_name = "ASPSESSID";
                string session_cookie_name = "ASP.NET_SessionId";

                if (HttpContext.Current.Request.Form[session_param_name] != null)
                {
                    UpdateCookie(session_cookie_name, HttpContext.Current.Request.Form[session_param_name]);
                }
                else if (HttpContext.Current.Request.QueryString[session_param_name] != null)
                {
                    UpdateCookie(session_cookie_name, HttpContext.Current.Request.QueryString[session_param_name]);
                }
            }
            catch
            {
            }

            try
            {
                string auth_param_name = "AUTHID";
                string auth_cookie_name = FormsAuthentication.FormsCookieName;

                if (HttpContext.Current.Request.Form[auth_param_name] != null)
                {
                    UpdateCookie(auth_cookie_name, HttpContext.Current.Request.Form[auth_param_name]);
                }
                else if (HttpContext.Current.Request.QueryString[auth_param_name] != null)
                {
                    UpdateCookie(auth_cookie_name, HttpContext.Current.Request.QueryString[auth_param_name]);
                }

            }
            catch
            {
            }
        }

        private void UpdateCookie(string cookie_name, string cookie_value)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies.Get(cookie_name);
            if (null == cookie)
            {
                cookie = new HttpCookie(cookie_name);
            }
            cookie.Value = cookie_value;
            HttpContext.Current.Request.Cookies.Set(cookie);
        }
        #endregion
    }
}