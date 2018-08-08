using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using BaseController;
using Common.CommonFunction;
using Common.DAL;
using Common.Models;
using Common.UserIdentityVerify;
using PermissionSys.Models;
using SSOModel;
using Utilities;
using Utility;



namespace Common
{
    public class CommonController : BasicController
    {
        #region Index
        public ActionResult Pending(string cGuid = null)
        {
            if (!string.IsNullOrEmpty(cGuid))
            {
                Session["CurrentCompany"] = cGuid;
                Session["EditThreshold"] = new Common.DAL.Common().GetEditThreshold(cGuid);
            }
            return View("Pending");
        }
        #endregion

        #region NavigatorTree
        public string GenerateNavigatorTreeJson(int block)
        {
            UserData userInfo = ((CustomPrincipal)HttpContext.User).UserData;
            UserVerify uv = new UserVerify(RMUrl);
            string CultureFlag = System.Threading.Thread.CurrentThread.CurrentCulture.ToString();
            string zTreeJsom = string.Empty;
            List<TreeMenuItem> menuLst = new List<TreeMenuItem>();
            List<TreeMenuItem> menuLst1 = new List<TreeMenuItem>();
            List<TreeMenuItem> allMenuItem = this.HttpContext.Application["Functions"] as List<TreeMenuItem>;
            if (userInfo.IsSuperAdmin)
            {
                menuLst = allMenuItem;
            }
            else
            {
                SingleSystemLoginModel userModel = uv.GetLoginModel(userInfo.LoginName, userInfo.Password);
                List<string> nodes = new List<string>();
                foreach (string str in userModel.SubFunctionIDs)
                {
                    nodes.AddRange(GetNode(str, allMenuItem));
                }
                menuLst = allMenuItem.Where(i => nodes.Distinct().Contains(i.GUID)).ToList();
            }
            Session["Nodes"] = menuLst.Select(i => i.SubfunctionCode)
                .Where(i => !string.IsNullOrEmpty(i)).ToList();
            menuLst.ForEach(i => i.ModuleID = i.ModuleID ?? string.Empty);
            menuLst1=menuLst.FindAll(i => i.Level != 3);
            zTreeJsom = GenerateJson(CultureFlag, menuLst1, block);
            return zTreeJsom;
        }

        private string GenerateJson(string cultureFlag, List<TreeMenuItem> items,int block)
        {
            string str = GenerateNavigatorTreeNode(cultureFlag,
                items, string.Empty, block);
            return str;
        }

        private string GenerateNavigatorTreeNode(string cultureFlag, List<TreeMenuItem> items, string pGuid,int block)
        {
            StringBuilder strBld = new StringBuilder("[ ");
            List<TreeMenuItem> citems = items.Where(i => i.ModuleID.Equals(pGuid) && i.Block == block).OrderBy(i => i.OrderNumber).ToList();
            foreach (TreeMenuItem mod in citems)
            {
                strBld.Append("{");
                strBld.AppendFormat(
                    "\"id\":\"{0}\",\"text\":\"{1}\",\"attributes\":{2},\"children\":{3}"
            ,
            mod.GUID,
            cultureFlag.Equals("en-US") ? mod.EnglishName : mod.ChineseName,
            "{\"url\":\"" + mod.URL + "\",\"subfunctionCode\":\"" + mod.SubfunctionCode + "\"}",
            items.Any(i => i.ModuleID.Equals(mod.GUID))
            ? GenerateNavigatorTreeNode(cultureFlag, items, mod.GUID, block)
            : "[]"
            );
                strBld.Append("},");
            }
            strBld.Remove(strBld.Length - 1, 1);
            strBld.Append("]");
            return strBld.ToString();
        }

        private List<string> GetNode(string fun, List<TreeMenuItem> allMenuItem)
        {
            List<string> temp = new List<string>();
            string parFun = allMenuItem.Find(i => i.GUID.Equals(fun)).ModuleID;
            if (!string.IsNullOrEmpty(parFun))
            {
                temp.AddRange(GetNode(parFun, allMenuItem));
            }
            temp.Add(fun);
            return temp;
        }
        #endregion

        #region Login
        public ActionResult Login(string LoginName)
        {
            if (string.IsNullOrEmpty(LoginName))
            {
                return View();
            }
            else
            {
                bool result = new UserService().UpdUserState(LoginName);
                if (result)
                {
                    Response.Write("<script >alert('账户已成功激活，请登录！');</script >");
                    
                    return View();
                }
                else
                {
                    
                    return View();
                }
            }
        }

        [HttpPost]
        public string Login(string LoginName, string Password, string ValidateCode)
        {
            bool result = false;
            string msg = string.Empty;
            if (string.IsNullOrEmpty(Password))
            {
                msg = General.Resource.Common.Password + General.Resource.Common.Required;
            }
            else if (!Session["ValidateCode"].Equals(ValidateCode))
            {
                msg = General.Resource.Common.VerifyCodeWrong;
            }
            else
            {
                List<FMS.Model.T_User> users = new UserService().GetUsers(LoginName, Password);
                if (users.Count.Equals(0))
                {
                    msg = General.Resource.Common.AccOrPwdWrong;
                }
                else
                {
                    Session["MasterCompanyGuid"] = users.First().C_GUID;
                    UserData dat = GenerateUserData(users.First().LoginName);
                    base.userData = dat;
                    CustomPrincipal.SignIn(dat);
                    CustomPrincipal.TrySetUserInfo(this.HttpContext.ApplicationInstance.Context);
                    if (Session["LogicGUID"] == null)
                    {
                        Session["LogicGUID"] = Guid.NewGuid().ToString();
                    }
                    result = true;
                }
            }
            return string.Format("{{\"Result\":{0},\"Msg\":\"{1}\"}}",
                result.ToString().ToLower(), msg);
        }

        public ActionResult GetValidateCode()
        {
            string code = ValidateGraphic.CreateValidateCode(4);
            Session["ValidateCode"] = code;
            byte[] bytes = ValidateGraphic.CreateValidateGraphic(code);
            return File(bytes, @"image/jpeg");
        }

        /// <summary>
        /// 生成用户信息
        /// </summary>
        /// <param name="loginName"></param>
        /// <returns></returns>
        private UserData GenerateUserData(string LoginName)
        {
            UserData userData = new UserData();
            userData.GUID = "a003cc90-8652-42f3-8612-48cc7b1d3d11";
            userData.LoginName = LoginName;
            userData.LoginFullName = LoginName;
            userData.LoginUserHRPosts = new string[] { "6a305391-8f24-4625-8bc2-440a2c40927b" };
            userData.IsSuperAdmin = true;
            userData.OriginSystemID = base.SubSystemID;
            userData.Password = "333";
            userData.UserSystemInfos = new SSOModel.SystemInfo[] { new SystemInfo() { ID = "1", Name = "FMS", URL = "" } };
            return userData;
        }

        #endregion

        #region Register
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public string Register(RegInfo registerinfo)
        {
            bool result = false;
            string msg = "";
            registerinfo.Company.C_GUID = Guid.NewGuid().ToString();
            registerinfo.User.C_GUID = registerinfo.Company.C_GUID;
            registerinfo.User.U_GUID = Guid.NewGuid().ToString();
            registerinfo.Company.MasterCompanyGuid = string.Empty;
            registerinfo.Company.AuditDate = DateTime.Now.ToString();
            registerinfo.User.State = 0;
            //根据输进的用户名（邮箱）查找数据库是否存在此邮箱注册记录！
            List<FMS.Model.T_User> users = new UserService().GetUsers(registerinfo.User.LoginName);
            //记录为0！
            if (users.Count.Equals(0))
            {   
                //根据输进的公司名称查找数据是否存在！
                List<FMS.Model.T_Company> Company=new UserService().GetCompany("",registerinfo.Company.Name);
                //记录为0
                if (Company.Count.Equals(0))
                {
                    //开始注册
                    result = new Common.DAL.Common().UpdRegInfo(registerinfo);
                    if (result)
                    {
                        SendMail(registerinfo.User.LoginName);
                    }
                }
                else
                {
                    result = false;
                    msg = General.Resource.Common.CompanyName + General.Resource.Common.Exist;
                    return string.Format("{{\"Result\":{0},\"Msg\":\"{1}\"}}",
              result.ToString().ToLower(), msg);
                }
            }
            else
            {
                result = false;
                msg = registerinfo.User.LoginName + General.Resource.Common.Exist+"，请重新输入！";
                return string.Format("{{\"Result\":{0},\"Msg\":\"{1}\"}}",
              result.ToString().ToLower(), msg);
            }
             msg = result ? "邮件已发送，请前往激活！" : General.Resource.Common.Failed;
            return string.Format("{{\"Result\":{0},\"Msg\":\"{1}\"}}",
                result.ToString().ToLower(), msg);
        }

        private void SendMail(string email)
        {
            MailMessage msg = new MailMessage();
            msg.From = new MailAddress("18068042939@163.com");   //发件人的邮箱地址
            msg.Subject = "*欢迎您的注册FMS*";  //邮件主题
            msg.Body = "<a href='http://192.168.1.112:10010/?LoginName=" + email + "'>点击激活您的账号</a>";//邮件正文
            msg.To.Add(email);
            msg.IsBodyHtml = true;  //邮件正文是否支持html的值
            SmtpClient sc = new SmtpClient();
            sc.Host = "smtp.163.com";
            sc.Port = 25;
            NetworkCredential nc = new NetworkCredential("18068042939@163.com", "chenxiang1225");  //验证凭据 
            sc.Credentials = nc;
            sc.Send(msg);
        }
        #endregion

        #region LogOff
        public ActionResult LogOff()
        {
            if (!Session.IsNewSession)
            {
                FormsAuthentication.SignOut();
                if (SSO)
                {
                    HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, "");
                    cookie.Expires = DateTime.Now.AddDays(-1);
                    cookie.Domain = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["SSODomain"]);
                    Response.Cookies.Add(cookie);
                }
                else
                {
                    if (this.HttpContext.Application.AllKeys.Contains(base.userData.LoginName)
                     && this.HttpContext.Application[base.userData.LoginName].Equals(Session.SessionID))
                    {
                        this.HttpContext.Application.Remove(base.userData.LoginName);
                    }
                    else
                    { }
                }
                if (!userData.IsSuperAdmin)
                {
                    new Log().Loger(base.userData, Session["LogicGUID"].ToString(), Session["IP"].ToString()
                                    , base.RMUrl, 0, false);
                }

            }
            else
            { }
            //记录他已经登出
            Session["IsLogOut"] = "1";
            return new RedirectResult("/");
        }
        #endregion

        #region Language
        public ActionResult ChangeLanguage(string lang)
        {
            //string mainurl = Request.QueryString["mainurl"];
            //string originalUrl = Request.QueryString["originalUrl"];
            //Session["mainurl"] = mainurl;
            if (SSO)
            {
                HttpCookie cookie = new HttpCookie("language", lang);
                cookie.HttpOnly = true;
                cookie.Domain = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["SSODomain"]);
                if (HttpContext == null)
                    throw new InvalidOperationException();
                HttpContext.Response.Cookies.Remove(cookie.Name);
                HttpContext.Response.Cookies.Add(cookie);
            }
            else
            {
                Session["Culture"] = new System.Globalization.CultureInfo(lang);
                Utilities.ResourceLoader.SetCurrentThreadCulture(Session);
            }
            return Pending();
        }
        #endregion

        #region  Redirect
        public ActionResult RedirectToLogin(string Msg)
        {
            string script = string.Format("<script>alert('{0}');window.location.href='{1}'</script>",
                Msg, base.LoginUrl);
            return Content(script);
        }
        #endregion

        //#region Check
        //public ActionResult Check()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public string Check(string LoginName)
        //{
        //    bool result = false;
        //    string msg = string.Empty;
        //    if (string.IsNullOrEmpty(LoginName))
        //    {
        //        msg = "链接地址发生错误，请重新点击链接";
        //    }
        //    else
        //    {
        //        result = new UserService().UpdUserState(LoginName);
        //        if (result)
        //        {
        //            msg = "激活成功";
        //            Login();
        //        }
        //        else
        //        {
        //            msg = "激活不成功";
        //        }
        //    }
        //    return string.Format("{{\"Result\":{0},\"Msg\":\"{1}\"}}",
        //        result.ToString().ToLower(), msg);
        //}

        //#endregion
    }
}
