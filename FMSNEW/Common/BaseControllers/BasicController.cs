using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Common.CommonFunction;
using Common.Models;
using FMS.Model;
using Newtonsoft.Json;
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
            //RMUrl = requestContext.HttpContext.Application["RMUrl"].ToString();
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
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            Response.Buffer = true;
            Response.ExpiresAbsolute = DateTime.Now.AddDays(-1);
            Response.Cache.SetExpires(DateTime.Now.AddDays(-1));
            Response.Expires = 0;
            Response.CacheControl = "no-cache";
            Response.Cache.SetNoStore();

            base.OnActionExecuted(filterContext);
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


        /// <summary>
        /// 检查列表中某个日期是否在账期内
        /// </summary>
        /// <param name="recordList"></param>
        /// <returns></returns>
        protected List<T_AIDRecord> DateCheck(List<T_AIDRecord> recordList)
        {
            DateTime EditThreshold = DateTime.Parse(Session["EditThreshold"].ToString());

            //日期必须在Now 和 EditThreshold之间
            var dateCheck = recordList.FindAll(a => DateTime.Parse(a.Date) >= EditThreshold && DateTime.Parse(a.Date) <= DateTime.Now);

            return dateCheck;

        }

        /// <summary>
        /// 检查某个日期是否在账期内
        /// </summary>
        /// <param name="checkDate"></param>
        /// <returns></returns>
        protected bool DateCheck(string checkDate)
        {
            DateTime check;
            if (DateTime.TryParse(checkDate, out check))
            {
                DateTime EditThreshold = DateTime.Parse(Session["EditThreshold"].ToString());

                //日期必须在Now 和 EditThreshold之间
                if (check <= DateTime.Now && check >= EditThreshold)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

        }


        /// <summary>
        /// 获取当前日期
        /// </summary>
        /// <returns></returns>
        protected string GetNowDate()
        {
            return DateTime.Now.ToString("yyyy/MM/dd");
        }

        protected string GetDetailDate() {
            return DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss fff");  
        }
        /// <summary>
        /// 获取当前日期以及时分
        /// </summary>
        /// <returns></returns>
        protected string GetMinutesDate()
        {
            return DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd HH:mm");
        }
        /// <summary>
        /// 将数据转换为下拉框的JSON格式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">要转换的对象类表</param>
        /// <param name="labelField">下拉框的显示内容字段</param>
        /// <param name="valueField">下拉框的值字段</param>
        /// <returns>JSON格式的下拉框数据</returns>
        /// <remarks>liujf  2016/11/23  create</remarks>
        protected string ConvertToSelectJson<T>(List<T> list, string labelField, string valueField)  
        {
            List<MultiSelect> selectList = new List<MultiSelect>(); 

            foreach (T t in list)
            {
                PropertyInfo[] propertys = t.GetType().GetProperties();
 
                MultiSelect select = new MultiSelect();
                foreach (PropertyInfo pi in propertys)
                {
                    if (pi.Name == labelField)
                    {
                        select.label = pi.GetValue(t, null).ToString();

                    }
                    else if (pi.Name == valueField)
                    {
                        select.value = pi.GetValue(t, null).ToString();
                    }

                }
                selectList.Add(select);
            }

            if(selectList.Count>0)
            {
                MultiSelect all = new MultiSelect();
                all.label = "全部";
                all.value = string.Empty;
                selectList.Insert(0, all);
            }
            return JsonConvert.SerializeObject(selectList); ;
        }
    }
}