using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.Script.Serialization;
using BaseController;
using FMS.DAL;
using FMS.Model;
using Utility;
using Common.DAL;

namespace FMS.BLL
{
    public class CompanyController : BasicController
    {

       
        /// <summary>
        /// 选择公司页面
        /// </summary>
        /// <returns></returns>
        public ActionResult ChooseCompany()
        {
            return View();
        }

        /// <summary>
        /// 获取公司信息页面
        /// </summary>
        /// <param name="id">公司标识</param>
        public ActionResult CompanyInfo(string id)
        {
            if (string.IsNullOrEmpty(id))
            {

                T_Company model = new T_Company();
                return View("CompanyInfo", model);
            }
            else
            {
                ViewData["C_GUID"] = id;
                Session["CurrentCompany"] = id;
                //ViewData["MasterCompanyGuid"] = "";
                List<T_Company> Info = new List<T_Company>();
                Info = new CompanySvc().GetCompanyInfo(id, "");
                if (Info.Count.Equals(0))
                {
                    T_Company model = new T_Company();
                    return View("CompanyInfo", model);
                }
                else
                {
                    return View("CompanyInfo", Info.FirstOrDefault());
                }

            }
        }

        ///<summary>
        ///检查公司设置
        ///<summary>
        ///<param name='id'>公司标识</param>
        public bool checkCompanySetting(string id)
        {
            bool result = false;
            result = new CompanySvc().checkCompanySetting(id);
            return result;
        }

        /// <summary>
        /// 获取用户列表信息页面
        /// </summary>
        public ActionResult GetUserList()
        {
            return Json(new CompanySvc().GetUserList(Session["MasterCompanyGuid"].ToString()));
        }

        /// <summary>
        /// 获取用户列表信息页面
        /// </summary>
        public string GetUserListString()
        {
            List<R_UserCompany> recs = new CompanySvc().GetUserList(Session["CurrentCompanyGuid"].ToString());
            string json = new JavaScriptSerializer().Serialize(recs);
            return json;
        }

        public string GetUserAllList()
        {
            string count = null;
            List<T_User> users = new List<T_User>();
            StringBuilder strJson = new StringBuilder("[");
            users = new CompanySvc().GetUserAllList();
            count = users.Count().ToString();
            T_User user = new T_User();
            user.Count = count;
            user.U_GUID = Guid.NewGuid().ToString();
            user.State = 1;
            List<T_User> allUser = new List<T_User>();
            allUser.Add(user);
            string json = new JavaScriptSerializer().Serialize(user);
            strJson.Append(json);
            strJson.Append("]");
            return strJson.ToString();
        }

        public String GetAllCompany()
        {
            string  count = null;
            List<T_Company> coms = new List<T_Company>();
            coms = new CompanySvc().GetAllCompany();
            count = coms.Count().ToString();
            T_Company com = new T_Company();
            com.Count = count;
            com.C_GUID = Guid.NewGuid().ToString();
            List<T_Company> company = new List<T_Company>();
            company.Add(com);
            string json = new JavaScriptSerializer().Serialize(company);
            return json;

        }

        public string GetUsersList(string dateBegin, string dateEnd, string GroupCode,int pageIndex = 1)
        {
            int count = 0;
            List<T_User> users = new List<T_User>();
            users = new CompanySvc().GetUserList(dateBegin, dateEnd, GroupCode, pageIndex, -1, out count);
            return new JavaScriptSerializer().Serialize(users);
        }
        
        public string GetCompanyList(string Country, string Pronvice, string City, int pageIndex = 1)
        {
            int count = 0;
            List<T_Company> companys = new List<T_Company>();
            companys = new CompanySvc().GetCompanyList(Country, Pronvice, City, pageIndex, -1, out count);
            return new JavaScriptSerializer().Serialize(companys);
        }

        /// <summary>
        /// 获取公司设置信息
        /// </summary>
        /// <param name="id">公司标识</param>
        public ActionResult CompanySetting(string id)
        {
            ViewData["C_GUID"] = id;
            T_CompanySetting Setting = new T_CompanySetting();
            Setting = new CompanySvc().GetCompanySetting(id);
            if (string.IsNullOrEmpty(Setting.R_GUID) == true)
            {
                Setting.Month = 1;
            }
            else
            {
                Setting.Month = Setting.GetReportStartDateMonth();
            }
            Setting.Year = Setting.GetReportStartDateYear();
            List<R_CompanyCurrceny> Currceny = new List<R_CompanyCurrceny>();
            Currceny = new CompanySvc().GetCompanyCurrceny(id);
            Setting.CompanyCy = Currceny.Select(i => i.Code).ToArray();
            return View("CompanySetting", Setting);
        }


        /// <summary>
        /// 获取公司列表信息
        /// </summary>
        public string GetCompanys(string CompanyName)
        {
            //string json;
            //json= GenerateJson(new CompanySvc().GetCompanys(Session["MasterCompanyGuid"].ToString()), string.Empty);
            //return json;
            //List<T_Company> recs = new CompanySvc().GetCompanys(Session["MasterCompanyGuid"].ToString());
            string name = null;
            if (Session["LoginName"] == null)
            {
                name = Session["TelName"].ToString();
            }
            else {
                name = Session["LoginName"].ToString();
            }
            List<T_Company> recs = new CompanySvc().GetCompanysByUserAccess(name, CompanyName);
            
            string json = new JavaScriptSerializer().Serialize(recs);
            return json;
        }

        public JsonResult GetCompanyss(string id)
        {
            return Json(new CompanySvc().GetCompanys(Session["MasterCompanyGuid"].ToString()));
        }

        /// <summary>
        /// 生成公司列表信息的JSON
        /// </summary>
        /// <param name="ds">数据源</param>
        /// <param name="pid">上级标识</param>
        /// <returns></returns>
        private string GenerateJson(List<T_Company> ds, string pid)
        {
            string strFormatter = "{{\"C_GUID\":\"{0}\",\"Name\":\"{1}\",\"children\":{2}}},";
            StringBuilder strJson = new StringBuilder("[ ");
            string strChildren = string.Empty;
            foreach (T_Company item in ds.Where(i => i.MasterCompanyGuid.Equals(pid)).OrderBy(i => i.CreateDate))
            {
                strChildren = GenerateJson(ds, item.C_GUID);
                strJson.AppendFormat(strFormatter, item.C_GUID, item.Name, strChildren);
            }
            strJson.Remove(strJson.Length - 1, 1);
            strJson.Append("]");
            return strJson.ToString();
        }

        /// <summary>
        /// 获取用户信息页面
        /// </summary>
        /// <param name="id">用户标识</param>
        public ActionResult UserInfo(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                ViewData["U_GUID"] = Guid.NewGuid().ToString();
                T_User model = new T_User();
                return View("UserInfo", model);
            }
            else
            {
                ViewData["U_GUID"] = id;
                List<T_User> info = new List<T_User>();
                info = new CompanySvc().GetUserInfo(id, "");
                return View("UserInfo", info.FirstOrDefault());
            }
        }

        /// <summary>
        /// 删除用户信息
        /// </summary>
        /// <param name="id">用户标识</param>
        public string DelUser(string id)
        {
            bool result = new CompanySvc().DelUser(id);
            string msg = string.Empty;
            if (result)
            {
                msg = General.Resource.Common.Success;
            }
            else
            {
                msg = General.Resource.Common.Failed;
            }
            return string.Format("{{\"Result\":{0},\"Msg\":\"{1}\"}}"
                , result.ToString().ToLower(), msg);
        }
 

        ///<summary>
        ///保存公司设置信息
        ///<summary>
        ///<param name="form">公司设置对象</param>
        public string UpdSetting(T_CompanySetting form)
        {
            bool result = false;
            form.R_GUID = Guid.NewGuid().ToString();
            form.ReportStartDate = new DateTime(form.Year, form.Month, 1);
            form.AuditDate = GetNowDate();
            result = new CompanySvc().UpdSetting(form);
            string msg = string.Empty;
            if (result)
            {
                msg = General.Resource.Common.Success;
            }
            else
            {
                msg = General.Resource.Common.Failed;
            }
            return string.Format("{{\"Result\":{0},\"Msg\":\"{1}\"}}"
                , result.ToString().ToLower(), msg);
        }

        /// <summary>
        /// 保存用户信息
        /// </summary>
        /// <param name="form">用户对象</param>
        public string UpdUserInfo(T_User form)
        {
            bool result = false;
            string msg = string.Empty;
            List<T_User> info = new List<T_User>();
            info = new CompanySvc().GetUserInfo("", form.LoginName);
            if (string.IsNullOrEmpty(form.C_GUID))
            {
                form.C_GUID = Session["MasterCompanyGuid"].ToString();
                if (info.Count.Equals(0))
                {
                    result = new CompanySvc().UpdUserInfo(form);
                    if (result)
                    {
                        msg = General.Resource.Common.Success;
                    }
                    else
                    {
                        msg = General.Resource.Common.Failed;
                    }

                }
                else
                {
                    msg = Common.Resource.RolePermission.LoginName + General.Resource.Common.Exist;
                }
                return string.Format("{{\"Result\":{0},\"Msg\":\"{1}\"}}"
                       , result.ToString().ToLower(), msg);
            }
            else
            {
                if (info.Count.Equals(0))
                {
                    result = new CompanySvc().UpdUserInfo(form);
                    if (result)
                    {
                        msg = General.Resource.Common.Success;
                    }
                    else
                    {
                        msg = General.Resource.Common.Failed;
                    }
                    return string.Format("{{\"Result\":{0},\"Msg\":\"{1}\"}}"
                        , result.ToString().ToLower(), msg);
                }
                else
                {
                    if (info.FirstOrDefault().C_GUID == form.C_GUID & info.FirstOrDefault().U_GUID == form.U_GUID)
                    {
                        result = new CompanySvc().UpdUserInfo(form);
                        if (result)
                        {
                            msg = General.Resource.Common.Success;
                        }
                        else
                        {
                            msg = General.Resource.Common.Failed;
                        }
                        return string.Format("{{\"Result\":{0},\"Msg\":\"{1}\"}}"
                            , result.ToString().ToLower(), msg);
                    }
                    result = false;
                    msg = Common.Resource.RolePermission.LoginName + General.Resource.Common.Exist;
                    return string.Format("{{\"Result\":{0},\"Msg\":\"{1}\"}}",
                  result.ToString().ToLower(), msg);
                }
            }

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
            string loginName = Session["LoginName"].ToString();

            List<FMS.Model.T_User> users = new UserService().GetUsers(loginName);
            //记录为0！
            if (users.Count.Equals(1))
            {
                FMS.Model.T_User user = (FMS.Model.T_User)users[0];
                user.Password = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(newPassword, "MD5");
                result = new UserService().UpdUserIfom(user);
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
        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <returns></returns>
        public ActionResult EditUser() 
        {
            return View();
        }
        [HttpPost]
        public string EditUser(string newPassword)
        {
            bool result = false;
            string msg = "";

            List<FMS.Model.T_User> users = new Common.DAL.UserService().GetUserss(Session["CurrentUserGuid"].ToString(), Session["MasterCompanyGuid"].ToString());
            //记录为0！
            if (users.Count.Equals(1))
            {
                FMS.Model.T_User user = (FMS.Model.T_User)users[0];
                user.Password = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(newPassword,"MD5");
                result = new UserService().UpdUserIfom(user);
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
        /// <summary>
        /// 修改用户昵称
        /// </summary>
        /// <returns></returns>
        public ActionResult EditNickName()
        {
            return View();
        }
        [HttpPost]
        public string EditNickName(string newNickName)
        {
            bool result = false;
            string msg = "";
            //if (Session["LoginName"] == null)
            //{
            //    string loginName = Session["TelName"].ToString();
            //}
            //else { 
            //string loginName = Session["LoginName"].ToString();
            //}
            List<FMS.Model.T_User> users = new Common.DAL.UserService().GetUserss(Session["CurrentUserGuid"].ToString(), Session["MasterCompanyGuid"].ToString());
            //记录为0！
            if (users.Count.Equals(1))
            {
                FMS.Model.T_User user = (FMS.Model.T_User)users[0];
                user.NickName = newNickName;
                result = new UserService().UpdUserIfom(user);
                if (result)
                {
                    Session["NickName"] = newNickName;
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


         public ActionResult ChangeCompanySetting()
        {
            return View();
        }
        [HttpPost]
         public bool UpdateCompanySetting(FormCollection form) 
         {
             bool result = false;
             string currency = form["Currency"];
             string cid = Session["CurrentCompanyGuid"].ToString();
             string cguid = Session["CurrentCompanyGuid"].ToString();
             //string id = "66666666-6666-6666-6666-666666666666";
             /**首先检查切换的统计货币列表是否存在**/
             //List<T_RateHistory> RateList = new List<T_RateHistory>();
             //RateList = new CurrencySvc().GetRateModel(cid, currency);
             ////if (RateList.Count > 0)
             //{
             //    if (new UserService().UpdCompanySetting(cguid, currency))
             //    {
             //        result = true;
             //    }
             //    else
             //    {
             //        result = false;
             //    }
             //}
             //else {
             //    List<T_RateHistory> WageRateList = new List<T_RateHistory>();
             //    WageRateList = new CurrencySvc().GetRateModel(id,currency);
             //    for (var i = 0; i < WageRateList.Count; i++)
             //    {
             //        string newID = Guid.NewGuid().ToString();
             //        T_RateHistory form3 = new T_RateHistory();
             //        form3.GUID = newID;
             //        form3.FAmount = WageRateList[i].FAmount;
             //        form3.FCurrency = WageRateList[i].FCurrency;
             //        form3.TAmount = WageRateList[i].TAmount;
             //        form3.TCurrency = WageRateList[i].TCurrency;
             //        form3.CurrentRecord = WageRateList[i].CurrentRecord;
             //        form3.Currency = WageRateList[i].Currency;
             //        form3.Rate = WageRateList[i].Rate;
             //        form3.TRate = WageRateList[i].TRate;
             //        form3.C_GUID = cguid;
             //        new CurrencySvc().UpdRateHistory(form3);
             //    }
                 if (new UserService().UpdCompanySetting(cguid, currency))
                 {
                     result = true;
                 }
                 else
                 {
                     result = false;
                 }
                return result;
             }
             
    

        /// <summary>
        /// 选择或增加新公司页面
        /// </summary>
        /// <returns></returns>
        public ActionResult ChooseAndAddCompany()
        {
           
            
            return View();
            
        }

        public bool AddNewCompany(FormCollection form)
        {
            bool result = false;

            List<T_Company> Info = new List<T_Company>();
            Info = new CompanySvc().GetCompanyInfo("", form["CompanyID"]);
            if (Info.Count.Equals(0))
            {
                //创建
                //UpdCompany
                T_Company company = new T_Company();

                company.C_GUID = Guid.NewGuid().ToString();
                Session["C_GUID"] = company.C_GUID;
                company.Name = form["Name"];
                company.Taxpayer = form["TaxO"];
                company.CompanyID = form["CompanyID"];
                string address = form["country"] + "-" + form["province"] + "-" + form["city"];
                company.Address = address;
                company.Country = form["country"];
                company.Province = form["province"];
                company.City = form["city"];
                company.Contacter = "";
                company.ContactWay = "";
                company.Type = "";
                company.AuditDate = DateTime.Today.ToString();

                if (new CompanySvc().UpdCompany(company, "", Session["CurrentUserGuid"].ToString()))
                {

                    AddCompanyCurrency(company.C_GUID, form["Currency"]);


                    AddCompanySetting(company.C_GUID, form["Currency"]);

                    if (Session["CurrentCompanyGuid"].ToString() == "")
                    {
                        Session["CurrentCompanyGuid"] = company.C_GUID;
                    }
                    Session["Currency"] = form["Currency"];

                    int count = 0;
                    List<T_ExpenseType> List = new List<T_ExpenseType>();
                    string cid = "66666666-6666-6666-6666-666666666666";
                    List = new DetailSvc().GetExpenseTypeList(cid, out count);
                    string id = company.C_GUID.ToString();
                    for (var i = 0; i < List.Count; i++)
                    {
                        string newID = Guid.NewGuid().ToString();
                        T_ExpenseType form1 = new T_ExpenseType();
                        form1.ET_GUID = newID;
                        form1.ExpenseType = List[i].ExpenseType;
                        form1.ExpenseFlag = List[i].ExpenseFlag;
                        form1.SaleFlag = List[i].SaleFlag;
                        form1.ManageFlag = List[i].ManageFlag;
                        form1.FinanceFlag = List[i].FinanceFlag;
                        form1.OtherFlag = List[i].OtherFlag;
                        form1.TaxFlag = List[i].TaxFlag;
                        result = new DetailSvc().UpdExpenseTypeRecord(form1, id);
                    }
                    List<T_Tax> ListTax = new List<T_Tax>();
                    ListTax = new TaxSvc().GetTax(cid);
                    for (var i = 0; i < ListTax.Count; i++)
                    {
                        string newID = Guid.NewGuid().ToString();
                        T_Tax form2 = new T_Tax();
                        form2.T_GUID = newID;
                        form2.Name = ListTax[i].Name;
                        form2.Rate = ListTax[i].Rate;
                        form2.Type = ListTax[i].Type;
                        form2.C_GUID = id;
                        new TaxSvc().UpdTax(form2);
                    }
                    //List<T_RateHistory> RateList = new List<T_RateHistory>();
                    //RateList = new CurrencySvc().GetRateModel(cid,form["Currency"]);
                    //for (var i = 0; i < RateList.Count; i++)
                    //{
                    //    string newID = Guid.NewGuid().ToString();
                    //    T_RateHistory form3 = new T_RateHistory();
                    //    form3.GUID = newID;
                    //    form3.FAmount = RateList[i].FAmount;
                    //    form3.FCurrency = RateList[i].FCurrency;
                    //    form3.TAmount = RateList[i].TAmount;
                    //    form3.TCurrency = RateList[i].TCurrency;
                    //    form3.CurrentRecord = RateList[i].CurrentRecord;
                    //    form3.Currency = RateList[i].Currency;
                    //    form3.Rate = RateList[i].Rate;
                    //    form3.TRate = RateList[i].TRate;
                    //    form3.C_GUID = id;
                    //    new CurrencySvc().UpdRateHistory(form3);
                    //}


                    /*添加快速关注模板（新建model以及表）**/
                    new AccountSvc().UpdIntAccount(cid, id);
                    
                   string Nowtime = GetMinutesDate().ToString();
                    List<T_QuickAttention> QuickAttenList = new List<T_QuickAttention>();
                    QuickAttenList = new ReportSvc().GetQuickAttentionModel(cid);
                    for (var i = 0; i < QuickAttenList.Count; i++)
                    {
                        string newID = Guid.NewGuid().ToString();
                        T_QuickAttention form4 = new T_QuickAttention();
                        form4.id = newID;
                        form4.c_guid = id;
                        form4.attention_type = QuickAttenList[i].attention_type;
                        form4.attention_type_amount = QuickAttenList[i].attention_type_amount;
                        form4.statistical_time = Convert.ToDateTime(Nowtime);
                        form4.statistical_currency = QuickAttenList[i].statistical_currency;
                        form4.attention_state = QuickAttenList[i].attention_state;
                        form4.push_account = QuickAttenList[i].push_account;
                        form4.push_frequency = QuickAttenList[i].push_frequency;
                        form4.company_name = QuickAttenList[i].company_name;
                        new ReportSvc().UpdQuickAttention(form4);
                    }
                    result = true;
                }
               
            }
            else
            {
                result = false;
            }
            return result;
        }

        public bool AddCompanySetting(string c_guid, string coin = null)
        {
            return new UserService().AddCompanySetting(c_guid, coin);
        }
        //增加财务统计货币
        public bool AddCompanyCurrency(string c_guid, string code)
        {
            string r_guid = Guid.NewGuid().ToString();
            return new Common.DAL.Common().AddCompanyCurrency(r_guid, c_guid, code);
        }
    }

}


