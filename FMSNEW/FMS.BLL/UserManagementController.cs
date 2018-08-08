using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using BaseController;
using FMS.DAL;
using FMS.Model;
using Utility;
using Common.Models;
using Common.DAL;

namespace FMS.BLL
{
    public class UserManagementController : UserController
    {
        public UserManagementController()
            : base("UserManagement")
        { }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddUser()
        {
            return View();
        }

        [HttpPost]
        public string AddUser(string loginName)
        {
            bool result = false;
          
          
            //0:不存在当前平台中，1:存在平台内但不属于此公司，2:已存在此公司
            string counts = new UserService().GetUserByCguid(loginName, Session["CurrentCompanyGuid"].ToString());
            if (counts == "2")
            {
                //return msg = loginName + General.Resource.Common.Exist + "，请重新输入！";
                return counts;
            }
            if (counts == "0") 
            { 
            string guid = Guid.NewGuid().ToString();
            Session["guid"]= guid;
            result = new Common.DAL.Common().AddUser(guid,Session["CurrentCompanyGuid"].ToString(),loginName,null,"0","GP010");
            if (result)
            {
                string iguid = new Common.DAL.Common().AddUserCompany(guid, Session["CurrentCompanyGuid"].ToString(), "0", "GP010", DateTime.Now.ToString(),"");

                string subject = "旺财 Net-Accounting.cn服务中心";
                
                if (loginName.IndexOf('@') > 0)
                {
                    new Common.CommonController().SendInfoToEmail(loginName, subject, "<a href='http://net-accounting.cn/Common/Activate?guid=" + iguid + "&loginName=" + loginName + "'>" + Session["CompanyName"].ToString() + "的管理员" + Session["NickName"].ToString() + "邀请您成为该公司的旺财用户，请点击确认</a>");
                    //new Common.CommonController().SendInfoToEmail(loginName, subject, "<a href='localhost:41955/Common/Activate?guid=" + guid + "'>点击激活您的账号</a>");
                }
                else
                {
                    new Common.CommonController().SendInfoToPhone(loginName, Session["CompanyName"].ToString() + "的管理员" + Session["NickName"].ToString() + "邀请您成为该公司的旺财用户，请点击确认 http://net-accounting.cn/Common/Activate?guid=" + iguid);
                }

            }
                return counts;
            }
            if (counts == "1")
            { 
                //string guid1 = Guid.NewGuid().ToString();
                string iguid1 = new Common.DAL.Common().AddUserCompanyLogin(loginName, Session["CurrentCompanyGuid"].ToString(), "0", "GP010", DateTime.Now.ToString());
                string subject = "旺财 Net-Accounting.cn服务中心";

                if (loginName.IndexOf('@') > 0)
                {
                    new Common.CommonController().SendInfoToEmail(loginName, subject, "<a href='http://net-accounting.cn/Common/LoginActivate?guid=" + iguid1 + "&loginName=" + loginName + "'>" + Session["CompanyName"].ToString() + "的管理员" + Session["NickName"].ToString() + "邀请您成为该公司的旺财用户，请点击确认</a>");
                    //new Common.CommonController().SendInfoToEmail(loginName, subject, "<a href='localhost:41955/Common/Activate?guid=" + guid + "'>点击激活您的账号</a>");
                }
                else
                {
                    new Common.CommonController().SendInfoToPhone(loginName, Session["CompanyName"].ToString() + "的管理员" + Session["NickName"].ToString() + "邀请您成为该公司的旺财用户，请点击确认 http://net-accounting.cn/Common/LoginActivate?guid=" + iguid1);
                }
               
            }
     
            return counts;
        }

        public ActionResult ModifyPassword()
        {
            return View();
        }

        [HttpPost]
        public string ModifyPassword(string newPassword)
        {
            bool result = false;
            string msg = "";
            string loginName = Session["UserLoginName"].ToString();

            List<FMS.Model.T_User> users = new UserService().GetUsers(loginName);
            //记录为0！
            if (users.Count.Equals(1))
            {
                FMS.Model.T_User user = (FMS.Model.T_User)users[0];
                user.Password = newPassword;
                result = new UserService().UpdUserInfos(user);
                if (result)
                {
                    msg = "修改成功";
                }
            }
            else
            {
                result = false;
                msg = "修改失败";
            }
            return msg;
        }

        public ActionResult ExchangeManager()
        {
            return View();
        }

        [HttpPost]
        public string ExchangeManager(FormCollection form)
        {
            bool result = false;
            string msg = "更改失败";
            string loginName = form["LoginName"];
            string validCode = form["ValidateCode"];
            result = new Common.DAL.Common().CheckValidateCode(loginName, validCode);
            if (result)
            {
                List<FMS.Model.T_User> users = new UserService().GetUserManage(loginName);
                //记录不为0！
                if (users.Count.Equals(1))
                {
                    FMS.Model.R_UserCompany user = new FMS.Model.R_UserCompany();
                    user.U_GUID = users[0].U_GUID;
                    user.C_GUID = Session["CurrentCompanyGuid"].ToString();
                    ////并且与当前用户属同一个公司
                    //if (user.C_GUID == Session["MasterCompanyGuid"].ToString())
                    //{
                        

                    string groupCode = new Common.DAL.Common().GetUserGroup(Session["CurrentCompanyGuid"].ToString(), users.First().U_GUID);
                       

                    if (groupCode != null && groupCode != "")
                        {
                            //1.将其GroupCode置为1
                            user.GroupCode = "GP001";
                            
                            //result = new UserService().UpdUserInf(user);
                            string LoginName = null;
                            if (Session["LoginName"] == null)
                            {
                                LoginName = Session["TelName"].ToString();
                            }
                            else
                            {
                                LoginName = Session["LoginName"].ToString();
                            }

                            //2.将当前用户GroupCode置为0
                            List<FMS.Model.T_User> users2 = new UserService().GetUsers(LoginName);

                            FMS.Model.R_UserCompany UserCompany2 = new FMS.Model.R_UserCompany();

                            UserCompany2.U_GUID = users2[0].U_GUID;
                            UserCompany2.C_GUID = Session["CurrentCompanyGuid"].ToString();

                            List<R_UserCompany> UserCompanyList = new List<R_UserCompany>();

                            UserCompanyList.Add(user);
                            UserCompanyList.Add(UserCompany2);

                            UserCompany2.GroupCode = "GP010";
                            result = new UserService().UpdUserInf(UserCompanyList);

                            //修改成功
                            if (result)
                            {   
                                msg = "success";
                                Session["GroupCode"] = UserCompany2.GroupCode;
                                List<string> recs = new UserService().GetUserStateOneModules(Session["CurrentUserGuid"].ToString(), Session["CurrentCompanyGuid"].ToString());
                                Session["ModuleList"] = recs; 
                            }
                            else {
                                msg = "变更管理者失败";
                            }
                        }
                    
                    else
                    {
                        msg = "当前公司无此用户，请先增加新用户";
                    }
                }
                else
                {
                    result = false;
                    msg = "无此邮箱/手机号注册的用户";
                }
            }
            else
            {
                msg = "验证码错误";
            }
            return msg;
        }

        public ActionResult UserList()
        {
            return View();
        }

        public string DelUser(string guid)
        {

            string GroupCode = new Common.DAL.UserService().GetUserC(guid).First().GroupCode;

            if (GroupCode == "GP001")
            {
                return "maserUser";


            }
            else
            {

                if (new UserService().DelUserAndModule(guid))
                {

                    return "success";
                }
                else
                {
                    return "fail";
                }
            }
           }

        public ActionResult UpdateAuthority(string guid)
        {

            List<R_UserCompany> rUserCompanyList = new Common.DAL.UserService().GetUserC(guid);
           
            
            string u_guid = rUserCompanyList.First().U_GUID;

            string GroupCode = rUserCompanyList.First().GroupCode;
        
            

            if (GroupCode == "GP001")
            {
                Response.Write("<script >alert('此用户为管理员无法修改权限！');</script >");
                return View("UserList");
            }
            else 
            {
                List<T_User> users = new FMS.DAL.CompanySvc().GetUserInfo(u_guid,"");
                Session["uName"] = users.First().LoginName;

            string c_guid = new Common.DAL.UserService().GetCompanyC(guid).First().C_GUID;
            Session["UserGuid"] = rUserCompanyList.First().U_GUID;
            Session["ComGuid"] = c_guid;

            List<T_ModuleList> modelList = new Common.DAL.UserService().GetModuleList(u_guid, c_guid);
            ViewBag.ChineseName = modelList;

            return View(); 
            }

        }

        public string GetUserModuleList()
        {

            List<R_UserModule> recs = new UserService().GetUserModules(Session["Guid"].ToString(), Session["CurrentUserGuid"].ToString(), Session["CurrentComGuid"].ToString());
            string json = new JavaScriptSerializer().Serialize(recs);
            return json;
        }
        public string UpdUserModule(string str,string sep)
        {
            List<string> rec = Session["ModuleList"] as List<string>;
            string u_guid = Session["UserGuid"].ToString();
            string c_guid = Session["ComGuid"].ToString();
            if (new UserService().UpdUserModule(u_guid, c_guid, Session["Guid"].ToString(), str, sep))
            {
                List<string> recs = new UserService().GetUserStateOneModules(u_guid,c_guid);
                Session["ModuleList"] = recs;
                return "提交成功";
            }
            else
            {
                return "提交失败";
            }
        }
        public string RightList(string id)
        {
            Session["Guid"] =id;
            List<R_UserModule> recs = new UserService().GetUserModules(Session["Guid"].ToString(), Session["UserGuid"].ToString(), Session["CurrentCompanyGuid"].ToString());
            string json = new JavaScriptSerializer().Serialize(recs);
            return json;

            //List<R_UserModule> recs1 = new List<R_UserModule>();
            //int a = recs.Count();
            //int ii = a / 2;

            //if (ii * 2 == a)
            //{
            //    // 偶数的情况
            //    int index = 0;
            //    for (int i = 0; i < a; i = i + 2)
            //    {
            //        recs1.Add(recs[i]);
            //        recs1[index].State1 = recs[i + 1].State;
            //        recs1[index].ChineseName1 = recs[i + 1].ChineseName;
            //        recs1[index].Guid1 = recs[i + 1].Guid;
            //        index++;
            //    }
            //}
            //else
            //{
            //    //奇数的情况
            //    int index = 0;
            //    for (int i = 0; i < a - 1; i = i + 2)
            //    {
            //        recs1.Add(recs[i]);
            //        recs1[index].State1 = recs[i + 1].State;
            //        recs1[index].ChineseName1 = recs[i + 1].ChineseName;
            //        recs1[index].Guid1 = recs[i + 1].Guid;
            //        index++;
            //    }
            //    recs1.Add(recs[index]);
            //    recs1[index].Guid1 = null;
            //    recs1[index].ChineseName1 = null;
            //    recs1[index].State1 = null;
            //}



            //string json = new JavaScriptSerializer().Serialize(recs1);
            //return json;
        }
    }
}
