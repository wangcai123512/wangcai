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
using FMS.Model;
using System.IO;
using FMS.DAL;
using Common.Controllers;                                               

namespace Common
{
    public class CommonController : BasicController
    {
        #region Index
        public ActionResult Pending(string cGuid = null)
        {
            if (!string.IsNullOrEmpty(cGuid))
            {
                Session["CurrentCompanyGuid"] = cGuid;
                Session["EditThreshold"] = new Common.DAL.Common().GetEditThreshold(cGuid);
            }
            return View("Pending");
        }

        public ActionResult Index(string cGuid = null)
        {
            if (!string.IsNullOrEmpty(cGuid))
            {
                Session["CurrentCompanyGuid"] = cGuid;
            }
            Session["Currency"] = new Common.DAL.Common().GetCurrencyCode(Session["CurrentCompanyGuid"].ToString());
            Session["EditThreshold"] = new Common.DAL.Common().GetEditThreshold(Session["CurrentCompanyGuid"].ToString());
            List<FMS.Model.T_Company> company = new UserService().GetCompanyInformation(Session["CurrentCompanyGuid"].ToString(), null);
            if (company.First().Taxpayer == null)
            {
                Session["Taxpayer"] = "无";
            }
            else { 
            Session["Taxpayer"] = company.First().Taxpayer.ToString();
            }
            Session["Address"] = company.First().Address.ToString();
            Session["CompanyName"] = company.First().Name;
            Session["ChineseFullName"] = company.First().ChineseFullName;
            Session["FullName"] = company.First().ChineseFullName;
            Session["GroupCode"] = new Common.DAL.Common().GetUserGroup(Session["CurrentCompanyGuid"].ToString(), Session["CurrentUserGuid"].ToString());
            Session["GroupName"] = new Common.DAL.Common().GetGroupCode(Session["CurrentCompanyGuid"].ToString(), Session["CurrentUserGuid"].ToString());
            //List<R_UserCompany> usercompany = new Common.DAL.Common().GetGroup(Session["CurrentCompanyGuid"].ToString(), Session["CurrentUserGuid"].ToString());
            //ViewBag.GroupCode = usercompany;
            //Session["EditThreshold"] = new Common.DAL.Common().GetEditThreshold(Session["CurrentCompany"].ToString());
            //List<FMS.Model.T_Company> company = new UserService().GetCompanyInformation(Session["CurrentCompany"].ToString(), null);
            //Session["CompanyName"] = company.First().Name;
            //查找此用户的权限，写入session中
            List<string> recs = new UserService().GetUserStateOneModules(Session["CurrentUserGuid"].ToString(), Session["CurrentCompanyGuid"].ToString());
            Session["ModuleList"] = recs;
            UserData dat = GenerateUserData(Session["CompanyName"].ToString());
            base.userData = dat;
            CustomPrincipal.SignIn(dat);
            CustomPrincipal.TrySetUserInfo(this.HttpContext.ApplicationInstance.Context);
            if(Session["LogicGUID"] == null)
            {
                Session["LogicGUID"] = Guid.NewGuid().ToString();
            }
            ViewData["CurrentCompanyGuid"] = Session["CurrentCompanyGuid"];
            ViewData["UserName"] = Session["UserName"];
            ViewData["CurrentCompanyGuid"] = Session["CurrentCompanyGuid"];
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            //读取已关注列表
            List<T_QuickAttention> AttentionList = new List<T_QuickAttention>();
            AttentionList = new ReportSvc().GetQuickAttentionList(Session["CurrentCompanyGuid"].ToString());
            for (int i = 0; i < AttentionList.Count; i++)
            {
                AttentionList[i] = AttentionList[i];
            }
            ViewData["AttentionList"] = AttentionList;
            //读取已关注和未关注所有列表
            List<T_QuickAttention> AllAttentionList = new List<T_QuickAttention>();
            AllAttentionList = new ReportSvc().GetAllAttentionList(Session["CurrentCompanyGuid"].ToString());
            for (int i = 0; i < AllAttentionList.Count; i++)
            {
                AllAttentionList[i] = AllAttentionList[i];
            }
            ViewData["AllAttentionList"] = AllAttentionList;
            //查询公司使用状态
            List<R_UserCompany> GCompanycode = new List<R_UserCompany>();
            GCompanycode = new Common.DAL.Common().GetCompanycode(Session["CurrentCompanyGuid"].ToString(), Session["CurrentUserGuid"].ToString());
            ViewData["GCompanycode"] = GCompanycode;
            return View("Index");
        }
        #endregion

          /// <summary>
         /// 首页快速关注统计
         /// </summary>
        /// <param name="recordList">首页快速关注统计</param>
        /// <returns></returns>

        public string UpdAType(string attention_type)
        {

            bool result = false;
            string[] dateSplit = GetDetailDate().ToString().Split('-');
            /**本年时间**/
            string yearBegin = dateSplit[0] + "/01/01";
            /**本月时间**/
            string monthBegin = dateSplit[0] + "/" + dateSplit[1] + "/01";
            /**当前时间**/
            string dateEnd = GetNowDate().ToString();
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            /**更新快速关注表中以下数据**/
            /** 本年至今净利润查询以及更新**/
            /** 本月至今净利润查询以及更新**/
            /** 本年至今净现金流查询以及更新**/
            if(attention_type == "本年至今净现金流")
            {
                 result = new ReportSvc().UpdYearFromNeCashFlows(C_GUID, yearBegin, dateEnd, attention_type);
            }
            /** 本月至今净现金流查询以及更新**/
            if (attention_type == "本月至今净现金流") 
            {
                result = new ReportSvc().UpdYearFromNeCashFlows(C_GUID, monthBegin, dateEnd, attention_type);
            }
            /** 应收款总金额查询以及更新**/
            if (attention_type == "应收款总金额")
            {
                result = new ReportSvc().UpdQAAccounts(C_GUID);
            }
            /** 预期应收款总金额查询以及更新**/
            /** 应付款总金额查询以及更新**/
            /** 总资产查询以及更新**/
            /** 净资产查询以及更新**/
        
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
        /// 修改快速关注
        /// </summary>
        /// <param name="recordList">修改快速关注</param>
        /// <returns></returns>
        public string UpdateQuickAList(List<T_QuickAttention> recordList)
        {
            //提交之前将所有记录改为0
            bool saved;
            string cguid = Session["CurrentCompanyGuid"].ToString();
            string msg = string.Empty;
            saved = new ReportSvc().UpdateQuickAList(recordList, cguid);
            if (saved == true) {
                msg = General.Resource.Common.Success;
            
            }else{
                msg = General.Resource.Common.Failed;
            }
            return string.Format("{{\"Result\":{0},\"Msg\":\"{1}\"}}"
                , saved.ToString().ToLower(), msg);
        }

        /// <summary>
        /// 实时查询快速关注表对已关注的定时发送定制信息(目前仅支持首页统计了数据才更新)
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public string AutoAttention()
        {
            string result = "";
            //首先实时更新数据
            string[] dateSplit = GetDetailDate().ToString().Split('-');
            /**本年时间**/
            string yearBegin = dateSplit[0] + "/01/01";
            /**本月时间**/
            string monthBegin = dateSplit[0] + "/" + dateSplit[1] + "/01";
            /**当前时间**/
            string dateEnd = GetNowDate().ToString();
            DateTime dt = DateTime.Today;  //当前时间
            DateTime startWeek = dt.AddDays(1 - Convert.ToInt32(dt.DayOfWeek.ToString("d")));  //本周周一
            DateTime endWeek = startWeek.AddDays(6);  //本周周日
            DateTime startMonth = dt.AddDays(1 - dt.Day);  //本月月初
            DateTime endMonth = startMonth.AddMonths(1).AddDays(-1);  //本月月末
            DateTime startQuarter = dt.AddMonths(0 - (dt.Month - 1) % 3).AddDays(1 - dt.Day);  //本季度初
            DateTime endQuarter = startQuarter.AddMonths(3).AddDays(-1);//本季度末
            DateTime Nowday = DateTime.Today;  //当前时间
            //表示今天是周日
            T_QuickAttention qucicklist = new T_QuickAttention();
            List<T_QuickAttention> Record = new List<T_QuickAttention>();
            int count = 0;
            int pageIndex = 1;
            int pageSize = -1;
            if ((DateTime.Compare(Nowday, endWeek) == 0) && (DateTime.Compare(Nowday, endMonth) == 0))
            {
                //发送每天以及周末以及月末
                string push_isselect = "isall";
                Record = new ReportSvc().GetAutoCheckQuickList(push_isselect, out count, pageIndex, pageSize);

            }
            else if (DateTime.Compare(Nowday, endWeek) == 0)
            {
                //发送每天以及周末
                string push_isselect = "isweek";
                Record = new ReportSvc().GetAutoCheckQuickList(push_isselect, out count, pageIndex, pageSize);
            }
            else if (DateTime.Compare(Nowday, endMonth) == 0)
            {
                //发送每天以及月末
                string push_isselect = "ismonth";
                Record = new ReportSvc().GetAutoCheckQuickList(push_isselect, out count, pageIndex, pageSize);
            }
            else
            {
                //发送每天
                string push_isselect = "isday";
                Record = new ReportSvc().GetAutoCheckQuickList(push_isselect, out count, pageIndex, pageSize);
            }
            //查询快速关注满足已经快速关注条件的数据
            for (int i = 0; i < Record.Count; i++)
            {
                //当公司相同的时候循环最后将循环的结果遍历出来发给这个公司的关注的人
                string type = Record[i].push_account.ToString();
                string subject = Record[i].company_name.ToString();
                string content = Record[i].push_content.ToString();
                //如果是邮箱和手机  微信号冗余
                try
                {
                    if (type.IndexOf('@') > 0)
                    {
                        SendInfoToEmail(type, subject, content);

                    }
                    else
                    {
                        SendInfoToPhone(type, content);
                    }
                }
                catch (Exception ex)
                {
                    //result = ex.Message.ToString();
                    result = "发送失败";
                }
            }
            return result;
        }
        public ActionResult BackStageManage()
        {
            return View("BackStageManage");
        }

        public ActionResult BackStageUserManage()
        {
            return View("BackStageUserManage");
        }
       
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
            menuLst1 = menuLst.FindAll(i => i.Level != 3);
            zTreeJsom = GenerateJson(CultureFlag, menuLst1, block);
            return zTreeJsom;
        }

        private string GenerateJson(string cultureFlag, List<TreeMenuItem> items, int block)
        {
            string str = GenerateNavigatorTreeNode(cultureFlag,
                items, string.Empty, block);
            return str;
        }

        private string GenerateNavigatorTreeNode(string cultureFlag, List<TreeMenuItem> items, string pGuid, int block)
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
            Session.Clear();
           
          
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
            else if (Session["ValidateCode"]== null)
            {
                msg = General.Resource.Common.VerifyCodeOverdue;
            }
            else if (!Session["ValidateCode"].Equals(ValidateCode))
            {
                msg = General.Resource.Common.VerifyCodeWrong;
            }
            else
            {
                string password = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(Password, "MD5");
                List<FMS.Model.T_User> users = new UserService().GetUsers(LoginName, password);
                
                if (users.Count.Equals(0))
                {
                    msg = General.Resource.Common.AccOrPwdWrong;
                }   
                else
                {
                   
                    if (users[0].SuperUser == "1")
                    {
                        msg = "SuperManager";
                        Session["SuperManager"] = users[0].SuperUser.ToString();
                    }
                    else {
                        List<R_UserCompany> NUser = new UserService().GetNickName(users.First().U_GUID, users.First().C_GUID);
                        Session["MasterCompanyGuid"] = users.First().C_GUID;
                        Session["CurrentCompanyGuid"] = "";
                        Session["CurrentUserGuid"] = users.First().U_GUID;
                        Session["TelName"] = users.First().TelName;
                        Session["LoginName"] = users.First().LoginName;
                        Session["Password"] = users.First().Password;
                        Session["NickName"] = NUser.First().NickName;
                        Session["Language"] = users.First().Language;
                        result = true;
                    }
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

        #region ForgetPassword
        public ActionResult ForgetPassword() 
        {
            return View();
        }

        [HttpPost]
        public string ForgetPassword(FormCollection form) 
        {
            string msg = string.Empty;

            //RegInfo registerinfo = new RegInfo();
            User user = new User();
            user.LoginName = form["LoginName"];
            
            Session["LoginName"] =user.LoginName;
            List<FMS.Model.T_User> users = new UserService().GetUsers(user.LoginName, "", "");
            if (users.Count.Equals(0))
            {
                return "不存在此用户";
            }
           
            
            else 
            { 
               
                if (!CheckValidateCode(user.LoginName, form["ValidateCode"]))
                {
                    return "验证码错误";
                }
            }
             
                return "";

       

        }

        public string CheckUser(string loginName)
        {
            string msg = string.Empty;
            List<FMS.Model.T_User> users = new UserService().GetUsers(loginName, "", "");
            if (users.Count==0)
            {
                return "0";
            }
            else
            {
                return "";
            }
        }
        #endregion

        #region Register
        public ActionResult Register()
        {
            return View();
        }
      

        [HttpPost]
        public string Register(FormCollection form)
        {
            RegInfo registerinfo = new RegInfo();
            User user = new User();
            registerinfo.User = user;
            T_Company company = new T_Company();
            registerinfo.Company = company;
            string name = null;
            if (form["LoginName"].IndexOf('@') > 0)
            {
                registerinfo.User.LoginName = form["LoginName"];
                name = registerinfo.User.LoginName;
            }
            else {
                registerinfo.User.TelName = form["LoginName"];
                name =  registerinfo.User.TelName;
            }
            registerinfo.User.NickName = form["NickName"];
            registerinfo.Company.Name = form["Name"];
            registerinfo.Company.CompanyID = form["CompanyID"];//公司ID唯一
            registerinfo.Company.Taxpayer = form["TaxO"];
            registerinfo.User.Password = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(form["Password"],"MD5");
            
            registerinfo.Company.C_GUID = Guid.NewGuid().ToString();
            registerinfo.User.C_GUID = registerinfo.Company.C_GUID;
            registerinfo.User.U_GUID = Guid.NewGuid().ToString();
            registerinfo.Company.MasterCompanyGuid = string.Empty;
            string address = form["country"] + "-" + form["province"] + "-" + form["city"];
            registerinfo.Company.Address = address;
            registerinfo.Company.Country = form["country"];
            registerinfo.Company.Province = form["province"];
            registerinfo.Company.City = form["city"];
            registerinfo.Company.Contacter = "";
            registerinfo.Company.ContactWay = "";
            registerinfo.Company.Type = "";
            registerinfo.Company.AuditDate = DateTime.Now.ToString();         
            registerinfo.User.State = 1;
            registerinfo.User.Language = form["Language"];
            Session["NickName"] = registerinfo.User.NickName;
            Session["Currency"] = form["Currency"];
            //核对验证码
            string loginNameV = registerinfo.User.LoginName;
            if (form["LoginName"].IndexOf('@') <= 0)
            {
                 loginNameV = registerinfo.User.TelName;
            }
            if (!CheckValidateCode(loginNameV, form["ValidateCode"]))
            {
                return "验证码错误";
            }
            //根据输进的公司ID查找数据是否存在！
            List<FMS.Model.T_Company> Company = new UserService().GetCompany("", registerinfo.Company.CompanyID);
            if (Company.Count.Equals(0))
            {
                List<FMS.Model.T_User> users = new UserService().GetUsers(loginNameV, "", "");
                if (users.Count.Equals(0))
                {
                    new Common.DAL.Common().UpdRegInfo(registerinfo);//公司和用户都不存在，注册公司和用户
                    int count = 0;
                    List<T_ExpenseType> List = new List<T_ExpenseType>();
                    string cid = "66666666-6666-6666-6666-666666666666";
                    List = new DetailSvc().GetExpenseTypeList(cid, out count);
                    string id = registerinfo.User.C_GUID.ToString();
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
                       new DetailSvc().UpdExpenseTypeRecord(form1, id);
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
                    // }

                    new AccountSvc().UpdIntAccount(cid,id);
                    //T_BeginningBalance balence = new T_BeginningBalance();
                    //balence.C_GUID = cid;
                    //balence.Money = 0;
                    //balence.InitialDate = DateTime.Now.ToString("yyyy-MM-dd");
                    //new BalanceSvc().InsInitialBalanceRecord(balence);
                    /**添加快速关注模板（新建model以及表）**/
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
                }
                else
                {
                    return "注册失败：该【用户手机/邮箱】已被注册！";
                }
                //在关系表R_UserCompany中增加记录

                //string uuGuid = new UserService().GetUsers(registerinfo.User.LoginName, "", "").First().U_GUID;

               string uuGuid = "";

               if (users.Count.Equals(0))
               {
                   uuGuid = registerinfo.User.U_GUID;
               }
               else { 
                   uuGuid = users.First().U_GUID;
               }

               if (!users.Count.Equals(0) || new Common.DAL.Common().AddUserCompany(uuGuid, registerinfo.Company.C_GUID, "1", "GP001", DateTime.Now.ToString(), registerinfo.User.NickName) != "")
                {
                    AddCompanyCurrency(registerinfo.Company.C_GUID, form["Currency"]);
                    AddCompanySetting(registerinfo.Company.C_GUID, form["Currency"]);
                    //下面的逻辑与登录成功后一样
                    FMS.Model.T_User loginUser = new UserService().GetUsers(name).First();
                    Session["MasterCompanyGuid"] = loginUser.C_GUID;
                    Session["CurrentCompanyGuid"] = loginUser.C_GUID;
                    Session["TelName"] = loginUser.TelName;
                    Session["LoginName"] = loginUser.LoginName;
                    Session["Password"] = loginUser.Password;
                    Session["GroupCode"] = (string)loginUser.Language;
                    Session["CurrentUserGuid"] = loginUser.U_GUID; 
                    //查找此用户的权限，写入session中
                    List<string> recs = new UserService().GetUserStateOneModules(Session["CurrentUserGuid"].ToString(), Session["CurrentCompanyGuid"].ToString());
                    Session["ModuleList"] = recs;
                    string Lname = null;
                    if (loginUser.LoginName == "" || loginUser.LoginName == null)
                    { 
                        Lname= loginUser.TelName;
                    }
                    else {
                        Lname = loginUser.LoginName; 
                    }
                    UserData dat = GenerateUserData(Lname);
                    base.userData = dat;
                    CustomPrincipal.SignIn(dat);
                    CustomPrincipal.TrySetUserInfo(this.HttpContext.ApplicationInstance.Context);
                    if (Session["LogicGUID"] == null)
                    {
                        Session["LogicGUID"] = Guid.NewGuid().ToString();
                    }
                    return registerinfo.Company.C_GUID;
                }
                else
                {
                    return "注册失败";
                }
            }
            else
            {
                return General.Resource.Common.CompanyID + General.Resource.Common.Exist;
            }
            ////根据输进的用户名（邮箱）查找数据库是否存在此邮箱注册记录！
            //List<FMS.Model.T_User> users = new UserService().GetUsers(registerinfo.User.LoginName, "", "NEW");
            ////记录为0！
            //if (users.Count.Equals(0))
            //{
            //    //根据输进的公司名称查找数据是否存在！
            //    List<FMS.Model.T_Company> Company = new UserService().GetCompany("", registerinfo.Company.Name);
            //    //记录为0
            //    if (Company.Count.Equals(0))
            //    {
            //        //开始注册
            //        if (new Common.DAL.Common().UpdRegInfo(registerinfo))
            //        {
            //            new Common.DAL.Common().AddUserCompany(registerinfo.User.U_GUID, registerinfo.Company.C_GUID, "1", "GP001");
            //            AddCompanyCurrency(registerinfo.Company.C_GUID, form["Currency"]);
            //            AddCompanySetting(registerinfo.Company.C_GUID, form["Currency"]);
            //            //下面的逻辑与登录成功后一样
            //            FMS.Model.T_User loginUser = new UserService().GetUsers(registerinfo.User.LoginName).First();
            //            Session["MasterCompanyGuid"] = loginUser.C_GUID;
            //            Session["CurrentCompanyGuid"] = loginUser.C_GUID;
            //            Session["UserName"] = loginUser.UserName;
            //            Session["LoginName"] = loginUser.LoginName;
            //            Session["Password"] = loginUser.Password;
            //            Session["GroupCode"] = (string)loginUser.GroupCode;
            //            Session["CurrentUserGuid"] = loginUser.U_GUID;
            //            //查找此用户的权限，写入session中 
            //            List<string> recs = new UserService().GetUserStateOneModules(Session["CurrentUserGuid"].ToString());
            //            Session["ModuleList"] = recs;
            //            UserData dat = GenerateUserData(loginUser.LoginName);
            //            base.userData = dat;
            //            CustomPrincipal.SignIn(dat);
            //            CustomPrincipal.TrySetUserInfo(this.HttpContext.ApplicationInstance.Context);
            //            if (Session["LogicGUID"] == null)
            //            {
            //                Session["LogicGUID"] = Guid.NewGuid().ToString();
            //            }
            //            //if (registerinfo.User.LoginName.IndexOf('@') > 0)
            //            //{
            //            //    SendMail(registerinfo.User.LoginName);
            //            //}
            //            //else
            //            //{
            //            //    SendPhone(registerinfo.User.LoginName);
            //            //}
            //            return loginUser.C_GUID;
            //        }
            //        else
            //        {
            //            return "注册失败";
            //        }
            //    }
            //    else
            //    {
            //        return General.Resource.Common.CompanyName + General.Resource.Common.Exist;
            //    }
            //}
            //else
            //{
            //    return registerinfo.User.LoginName + General.Resource.Common.Exist + "，请重新输入！";
            //}
        }
        public string AddUserInfo(FormCollection form,string Name)
        {
            bool result = false;
            string name = null;
            string C_GUID = Session["MasterCompanyGuid"].ToString();
            if (form["TelName"] == "")
            {
                name = form["EmialName"];
            }
            else {
                name = form["TelName"];
            }
            List<FMS.Model.T_User> users = new UserService().GetUsers(name, "", C_GUID);
            if (users.Count.Equals(0))
            {

                if (!CheckValidateCode(name, form["ValidateCode"]))
                {
                    return "验证码错误";
                }
                else
                {
                    result = new UserService().UpdaUserInf(name, C_GUID, Name);
                    if (result)
                    {
                        return "添加成功";
                    }
                    else
                    {
                        return "添加失败";
                    }
                }
            }
            else {
                return "该公司已存在此账号";
            }
        }

        public void SendMail(string email)
        {
            SendInfoToEmail(email, "旺财 Net-Accounting.cn服务中心", "<a href='http://net-accounting.cn/?LoginName=" + email + "'>点击激活您的账号</a>");
        }
        public void SendPhone(string phone)
        {
            SendInfoToPhone(phone, "http://net-accounting.cn/?LoginName=" + phone + "  点击激活您的账号</a>");
        }
        #endregion

        #region TermsOfUse
        public ActionResult TermsOfUse()
        {
            return View();
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
                                    ,base.RMUrl, 0, false);
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
            string script = string.Format("<script>window.location.href='{0}'</script>",
               base.LoginUrl);
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

        public RedirectToRouteResult ChangePage()
        {
            return RedirectToAction("Index", "CompanyInformationSet");
        }
        //发送验证码到邮箱
        public void SendInfoToEmail(string email, string subject, string content)
        {



            try
            {
                string mailAddress = System.Web.Configuration.WebConfigurationManager.AppSettings["EmailAddress"];
                string mailPwd = System.Web.Configuration.WebConfigurationManager.AppSettings["EmailPwd"];
                string stmp = System.Web.Configuration.WebConfigurationManager.AppSettings["stmp"];
                MailMessage msg = new MailMessage();
                
                msg.From = new MailAddress(mailAddress,"旺财服务中心");   //发件人的邮箱地址
                
                msg.Subject = subject;  //邮件主题
                msg.Body = content;// "你的验证码是：" + validCode;//邮件正文
                msg.To.Add(email);
                msg.IsBodyHtml = true;  //邮件正文是否支持html的值
                SmtpClient sc = new SmtpClient();
                sc.Host = stmp;
                sc.Port = 25;
                NetworkCredential nc = new NetworkCredential(mailAddress, mailPwd);  //验证凭据
                //NetworkCredential nc = new NetworkCredential("service@net-accounting.com", "novo321");  //验证凭据
                sc.Credentials = nc;
                sc.Send(msg);
            }
            catch (Exception e)
            {
                string x = e.Message.ToString();
            }

        }
        //发送验证码到手机
        public void SendInfoToPhone(string phoneNum, string content)
        {
            string url = "http://utf8.sms.webchinese.cn/?";
            string strUid = "Uid=novofront-hgx";
            string strKey = "&key=7bc202ef1f6f46f52be0";
            string strMob = "&smsMob=" + phoneNum;
            string strContent = "&smsText=" + content;//你的验证码是：" + validCode;

            url = url + strUid + strKey + strMob + strContent;
            string result = GetHtmlFromUrl(url);

        }
        public string GetHtmlFromUrl(string url)
        {
            string strRet = null;

            if (url == null || url.Trim().ToString() == "")
            {
                return strRet;
            }
            string targeturl = url.Trim().ToString();
            try
            {
                HttpWebRequest hr = (HttpWebRequest)WebRequest.Create(targeturl);
                hr.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1)";
                hr.Method = "GET";
                hr.Timeout = 30 * 60 * 1000;
                WebResponse hs = hr.GetResponse();
                Stream sr = hs.GetResponseStream();
                StreamReader ser = new StreamReader(sr, Encoding.Default);
                strRet = ser.ReadToEnd();
            }
            catch (Exception ex)
            {
                strRet = null;
            }
            return strRet;
        }
        //增加验证码
        public string AddValidCodeThenSend(string loginName)
        {
            string code = ValidateGraphic.CreateValidateCode(6);
            string result = "";
            if (new Common.DAL.Common().AddValidateCode(loginName, code))
            {
                try
                {
                    if (loginName.IndexOf('@') > 0)
                    {
                        SendInfoToEmail(loginName, "旺财 Net-Accounting.cn服务中心", "欢迎您来到旺财 Net-Accounting.cn，让财务更简单！您的验证码是：" + code);
                    }
                    else
                    {
                        SendInfoToPhone(loginName, "欢迎您来到旺财，让财务更简单！您的验证码是：" + code);
                    }
                }
                catch (Exception ex)
                {
                    //result = ex.Message.ToString();
                    result = "发送失败";
                }
            }
            
            return result;
        }

        public string AddValidCode(string loginName)
        {
            string code = ValidateGraphic.CreateValidateCode(6);
            string result = "";
           
            List<FMS.Model.T_User> users = new UserService().GetUserManage(loginName);
            if (users.Count.Equals(1))
            {
                 FMS.Model.R_UserCompany user = new FMS.Model.R_UserCompany();
                    user.U_GUID = users[0].U_GUID;
                    user.C_GUID = Session["CurrentCompanyGuid"].ToString();
                    ////并且与当前用户属同一个公司
                    //if (user.C_GUID == Session["MasterCompanyGuid"].ToString())
                    //{
                        

                    string groupCode = new Common.DAL.Common().GetUserGroup(Session["CurrentCompanyGuid"].ToString(), users.First().U_GUID);
                    string LoginName = null;
                    if (Session["LoginName"] == null)
                    {
                        LoginName = Session["TelName"].ToString();
                    }
                    else {
                        LoginName = Session["LoginName"].ToString();
                    }

                    if (groupCode != null && groupCode != "")
                    {
                        if (new Common.DAL.Common().AddValidateCode(loginName, code))
                        {
                            try
                            {

                                if (loginName.IndexOf('@') > 0)
                                {
                                    SendInfoToEmail(loginName, "旺财 Net-Accounting.cn服务中心", Session["CompanyName"].ToString() + "的管理者" + LoginName + "要将管理者权限转移给您！" + "新管理者验证码：" + code);
                                }
                                else
                                {
                                    SendInfoToPhone(loginName, Session["CompanyName"].ToString() + "的管理者" + LoginName + "要将管理者权限转移给您！" + "新管理者验证码：" + code);
                                }
                                result = "发送成功";
                            }
                            catch (Exception ex)
                            {
                                //result = ex.Message.ToString();
                                result = "发送失败";
                            }
                        }
                       
                    }
                    else {
                        result = "当前公司无此用户，请先增加新用户";
                    }
            }
            else 
            {
                result = "无此邮箱/手机号注册的用户";
            }
            return result;
        }

        //核对验证码
        public bool CheckValidateCode(string loginName, string validCode)
        {
            return new Common.DAL.Common().CheckValidateCode(loginName, validCode);
        }
        //增加财务统计货币
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

        #region Activate
        //添加新用户
        public ActionResult Activate(string guid, string loginName)
        {
            Session["iguid"] = guid;
            //Session["loginName"] = new Common.DAL.Common().GetNewUserInfo(guid);
            if (loginName == null)
            {
                Session["loginName1"] = new Common.DAL.Common().GetNewUserInfo(guid);
            }
            else
            {
                Session["loginName1"] = loginName;
            }
            return View();
        }
        #endregion


        [HttpPost]
        public string Activate(FormCollection form)
        {
           string pwd = form["Password"];
           string rePwd = form["Password2"];
           string pwd1 = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(form["Password"], "MD5");
           string NickName = form["NickName"];
           string result = null;
            if (pwd != rePwd)
            {
                return "密码不一致";
            }
            //核对验证码
            //if (!CheckValidateCode(loginName, code))
            //{
            //    return "验证码错误";
            //}

            //开始激活
            result =  new Common.DAL.Common().ActivateUser(Session["iguid"].ToString(), pwd1, NickName, DateTime.Now.ToString());
            if (result == "0"){
                return "该旺财用户已注册，请前往登录页面";
            }
            if (result == "1")
            {
                FMS.Model.T_User loginUser = new UserService().GetUsers(Session["loginName1"].ToString()).First();
                Session["MasterCompanyGuid"] = loginUser.C_GUID;
                Session["CurrentCompanyGuid"] = loginUser.C_GUID;
                Session["TelName"] = loginUser.TelName;
                Session["LoginName"] = loginUser.LoginName;
                Session["Password"] = loginUser.Password;
                Session["GroupCode"] = (string)loginUser.Language;
                Session["CurrentUserGuid"] = loginUser.U_GUID;
                Session["NickName"] = NickName;
                //查找此用户的权限，写入session中
                List<string> recs = new UserService().GetUserStateOneModules(Session["CurrentUserGuid"].ToString(), Session["CurrentCompanyGuid"].ToString());
                Session["ModuleList"] = recs;
                UserData dat = new UserData();
                if (loginUser.LoginName == null)
                {
                    dat = GenerateUserData(loginUser.TelName);
                }
                else
                {
                    dat = GenerateUserData(loginUser.LoginName);

                }
                base.userData = dat;
                CustomPrincipal.SignIn(dat);
                CustomPrincipal.TrySetUserInfo(this.HttpContext.ApplicationInstance.Context);
                if (Session["LogicGUID"] == null)
                {
                    Session["LogicGUID"] = Guid.NewGuid().ToString();
                }
                return loginUser.C_GUID;
            }
            else
            {
                return "激活失败";
            }
            //RegInfo registerinfo1 = new RegInfo();
            //User user1 = new User();
            //registerinfo1.User = user1;
            //registerinfo1.User.LoginName = form["LoginName"];
            //registerinfo1.User.Password = form["Password"];
            //registerinfo1.User.C_GUID = Session["CurrentCompanyGuid"].ToString();
            //registerinfo1.User.U_GUID = Guid.NewGuid().ToString();
            //registerinfo1.User.State = 1;
            //if (result){
            //List<FMS.Model.T_User> users = new UserService().GetUsers(registerinfo1.User.LoginName, "", "NEW");
            //if (users.Count.Equals(0))
            //{
            //    new Common.DAL.Common().UpdRegInfo(registerinfo1);//公司和用户都不存在，注册公司和用户
            //}                //在关系表R_UserCompany中增加记录
            //    if (new Common.DAL.Common().AddUserCompany(registerinfo1.User.U_GUID, Session["CurrentCompanyGuid"].ToString(), "1", "GP001"))
            //    {

            //        //下面的逻辑与登录成功后一样
            //        FMS.Model.T_User loginUser = new UserService().GetUsers(registerinfo1.User.LoginName).First();
            //        Session["MasterCompanyGuid"] = loginUser.C_GUID;
            //        Session["CurrentCompanyGuid"] = loginUser.C_GUID;
            //        Session["UserName"] = loginUser.UserName;
            //        Session["LoginName"] = loginUser.LoginName;
            //        Session["Password"] = loginUser.Password;
            //        Session["GroupCode"] = (string)loginUser.GroupCode;
            //        Session["CurrentUserGuid"] = loginUser.U_GUID;
            //        //查找此用户的权限，写入session中
            //        List<string> recs = new UserService().GetUserStateOneModules(Session["CurrentUserGuid"].ToString());
            //        Session["ModuleList"] = recs;
            //        UserData dat = GenerateUserData(loginUser.LoginName);
            //        base.userData = dat;
            //        CustomPrincipal.SignIn(dat);
            //        CustomPrincipal.TrySetUserInfo(this.HttpContext.ApplicationInstance.Context);
            //        if (Session["LogicGUID"] == null)
            //        {
            //            Session["LogicGUID"] = Guid.NewGuid().ToString();
            //        }
            //        return loginUser.C_GUID;
            //    }
            //    else
            //    {
            //        return "注册失败";
            //    }
            //}
        }


        public ActionResult LoginActivate(string guid, string loginName)
        {
            Session["iguid1"] = guid;
            if (loginName == null)
            {
                Session["loginName1"] = new Common.DAL.Common().GetNewUserInfo(guid);
            }
            else {
                Session["loginName1"] = loginName;
            }
            Session["Password"] = new Common.DAL.Common().GetNewUserPwd(guid);
            Session ["UserComGuid"] = Session["iguid1"];
            return View();
            //if (string.IsNullOrEmpty(LoginName))
            //{
            //    return View();
            //}
            //else
            //{
            //    bool result = new UserService().UpdUserState(LoginName);
            //    if (result)
            //    {
            //        Response.Write("<script >alert('账户已成功激活，请登录！');</script >");

            //        return View();
            //    }
            //    else
            //    {

            //        return View();
            //    }
            //}
        }

        [HttpPost]
        public string LoginActivate(string LoginName, string Password, string ValidateCode,string date)
        {
            string result = null;
            string msg = string.Empty;
            string psd = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(Password, "MD5");

            if (string.IsNullOrEmpty(Password))
            {
                msg = General.Resource.Common.Password + General.Resource.Common.Required;

            }
            else if (new UserService().GetUsers(Session["loginName1"].ToString(), psd).Count == 0)
            {
                msg = "密码错误";
            }
            else if (!Session["ValidateCode"].Equals(ValidateCode))
            {
                msg = General.Resource.Common.VerifyCodeWrong;
            }
            
            else
            {



                result = new Common.DAL.Common().ActivateLogin(Session["iguid1"].ToString(), DateTime.Now.ToString());
                //if (result == "0")
                //{
                //    msg = "该旺财用户已注册，请前往登陆页面";
                //}
                //if (result == "1")
                //{
                    //if (users.Count.Equals(0))
                    //{
                    //    msg = General.Resource.Common.AccOrPwdWrong;
                    //}
                    //else
                    //{
                   FMS.Model.R_UserCompany loginUser = new UserService().GetUser(Session["loginName1"].ToString(), Session["Password"].ToString(), Session["UserComGuid"].ToString()).First();
                   // Session["MasterCompanyGuid"] = loginUser.C_GUID;
                    Session ["UserComGuid"] =  loginUser.UC_GUID;
                    Session["CurrentCompanyGuid"] = loginUser.C_GUID;
                    Session["CurrentUserGuid"] = loginUser.U_GUID;
                    Session["UserName"] = loginUser.UserName;
                    Session["LoginName"] = loginUser.LoginName;
                    Session["TelName"] = loginUser.TelName;
                    Session["Password"] = loginUser.Password;
                    Session["GroupCode"] = (string)loginUser.GroupCode;
                    Session["NickName"] = loginUser.NickName;                 
                    //查找此用户的权限，写入session中
                    List<string> recs = new UserService().GetUserStateOneModules(Session["CurrentUserGuid"].ToString(), Session["CurrentCompanyGuid"].ToString());
                    Session["ModuleList"] = recs;
                    UserData dat = new UserData();
                    if (loginUser.LoginName == null)
                    {
                         dat = GenerateUserData(loginUser.TelName);
                    }
                    else {
                        dat = GenerateUserData(loginUser.LoginName);
                    
                    }
                    base.userData = dat;
                    CustomPrincipal.SignIn(dat);
                    CustomPrincipal.TrySetUserInfo(this.HttpContext.ApplicationInstance.Context);
                    if (Session["LogicGUID"] == null)
                    {
                        Session["LogicGUID"] = Guid.NewGuid().ToString();
                    }
                    return loginUser.C_GUID;
                   
                    
                //}
            }
            return string.Format("{{\"Msg\":\"{0}\"}}",
                msg);

        }
    }


 }
