using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BaseController;
using FMS.DAL;
using FMS.Model;
using System.Web.Script.Serialization;
using System.Text;

namespace FMS.BLL
{
    /// <summary>
    /// 总账科目
    /// </summary>
    public class GeneralLedgerAccountController : UserController
    {
        public GeneralLedgerAccountController()
            : base("GeneralLedgerAccount")
        { }

        /// <summary>
        /// 列表页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GeneralLedger() {
            return View();
        }
        /// <summary>
        /// 总账科目信息页
        /// </summary>
        /// <param name="id">总账科目标识</param>
        /// <returns></returns>
        public ActionResult LedgerAccount(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return View(new T_GeneralLedgerAccount() { LA_GUID = Guid.NewGuid().ToString() });
            }
            else
            {
                return View(new AccountSvc().GetLedgerAccounts(Session["CurrentCompanyGuid"].ToString()).FirstOrDefault(i => i.LA_GUID.Equals(id)));
            }
        }

       
        /// <summary>
        /// 获取总账科目列表数据
        /// </summary>
        /// <returns></returns>
        public JsonResult GetLedgerAccount()
        {
            return Json(new AccountSvc().GetLedgerAccounts(Session["CurrentCompanyGuid"].ToString()));
        }
        public string GetLAccountList()
        {

            List<T_GeneralLedgerAccount> List = new AccountSvc().GetLedgerAccount(Session["CurrentCompanyGuid"].ToString());
            string json = new JavaScriptSerializer().Serialize(List);
            return json;

        }
        public string GetDetailSubject(string Name)
        {
            var List = new AccountSvc().GetDetailLAByName(Name, Session["CurrentCompanyGuid"].ToString(),"1");
            var strJson = ConvertToSelectJson(List, "Name", "DA_GUID");
            return strJson.ToString();    
        }

        public string GetThirdByParentName(string Name)
        {
            var List = new AccountSvc().GetThirdLAByName(Name, Session["CurrentCompanyGuid"].ToString(), "1");
            var strJson = ConvertToSelectJson(List, "Name", "TDA_GUID");
            return strJson.ToString();
        }

        public string GetThirdByName(string Name,string strType)
        {
            var List = new AccountSvc().GetThirdLAByName(Name, Session["CurrentCompanyGuid"].ToString(), strType);
            var strJson = ConvertToSelectJson(List, "Name", "TDA_GUID");
            return strJson.ToString();
        }

        public string GetLAccount(string Name,string State) {
            int AccGroup = 0;
            switch (Name)
            {
                case "Assets":
                    AccGroup = 1;
                    break;
                case "Liabilities":
                    AccGroup = 2;
                    break;
                case "OwnerEquity":
                    AccGroup = 4;
                    break;
                case "Cost":
                    AccGroup = 5;
                    break;
                case "IncomeStatement":
                    AccGroup = 6;
                    break;
                default:
                    break;
            }
            List<T_GeneralLedgerAccount> List = new AccountSvc().GetLedgerAccount(AccGroup, Session["CurrentCompanyGuid"].ToString(), State);
            string json = new JavaScriptSerializer().Serialize(List);
            return json;
           
        }

        public string GetDetailLAccount(string strParentID,string State)
        {
            List<T_GeneralLedgerAccount> List = new AccountSvc().GetDetailLedgerAccount(strParentID, Session["CurrentCompanyGuid"].ToString(),State);
            string json = new JavaScriptSerializer().Serialize(List);
            return json;
           
        }

        public string GetDetailLAccountByID(string Name)
        {
            var detailList = new AccountSvc().GetDetailLAByName(Name, Session["CurrentCompanyGuid"].ToString(), "1");
            var strJson = ConvertToSelectJson(detailList, "Name", "DA_GUID");
            return strJson.ToString();
        }

        public string GetDetailAccountByName(string Name)
        {

            var detailList = new AccountSvc().GetDetailedAccountByName(Name, Session["CurrentCompanyGuid"].ToString());
            var strJson = ConvertToSelectJson(detailList, "Name", "DA_GUID");
            return strJson.ToString();
        
        }


        public string GetParentGuid(string id)
        {
            T_GeneralLedgerAccount GL = new AccountSvc().GetParentGuid(id);
            string json = new JavaScriptSerializer().Serialize(GL);
            return json;
        }

        public string GetDetailLAccountByName(string Name) {
            List<T_GeneralLedgerAccount> List = new AccountSvc().GetDetailLAByName(Name, Session["CurrentCompanyGuid"].ToString(),"");
            string json = new JavaScriptSerializer().Serialize(List);
            return json;
        }

        public string GetDetailLAccountByName1(string Name)
        {
            List<T_GeneralLedgerAccount> List = new AccountSvc().GetDetailLAByName(Name, Session["CurrentCompanyGuid"].ToString(), "1");
            string json = new JavaScriptSerializer().Serialize(List);
            return json;
        }

        public string GetLAByName(string Name)
        {
            T_GeneralLedgerAccount GL = new AccountSvc().GetLAByName(Name, Session["CurrentCompanyGuid"].ToString());
            string json = new JavaScriptSerializer().Serialize(GL);
            return json;
        }

        public string GetThirdDetailLAccount(string strParentID,string State)
        {
            List<T_GeneralLedgerAccount> List = new AccountSvc().GetThirdLedgerAccount(strParentID, Session["CurrentCompanyGuid"].ToString(),State);
            string json = new JavaScriptSerializer().Serialize(List);
            return json;
        }

        public string AddSubject(string strParentID, int AccCode,string Name,string Subject)
        {
            bool result = false;
            string msg = string.Empty;
            result = new AccountSvc().AddSubject(strParentID, AccCode, Name, Subject, Session["CurrentCompanyGuid"].ToString());
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

        public string DeleteAccount(string id)
        {
            bool result = false;
            string msg = string.Empty;
            result = new AccountSvc().DeleteAccount(id);
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

        public string UpdateUsingLA()
        {
            bool result = false;
            string msg = string.Empty;
            string jsonData = (Request.Form["jsonData1"]).ToString();
            List<T_GeneralLedgerAccount> List = Newtonsoft.Json.JsonConvert.DeserializeObject<List<T_GeneralLedgerAccount>>(jsonData);
            if (List.Count > 0)
            {
                for (int i = 0; i < List.Count; i++)
                {
                    result = new AccountSvc().UpdateUsingLA(List[i].LA_GUID, List[i].State, Session["CurrentCompanyGuid"].ToString());
                }
            }
            
            
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
        public string UpdateUsingDLA() {
            bool result = false;
            string msg = string.Empty;
            string jsonData = (Request.Form["jsonData2"]).ToString();
            List<T_GeneralLedgerAccount> List = Newtonsoft.Json.JsonConvert.DeserializeObject<List<T_GeneralLedgerAccount>>(jsonData);
            if (List.Count > 0)
            {
                for (int i = 0; i < List.Count; i++)
                {
                    result = new AccountSvc().UpdateUsingDLA(List[i].DA_GUID, List[i].ParentAccGuid, List[i].State, Session["CurrentCompanyGuid"].ToString());
                }
            }


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

       

        public string UpdateUsingThLA()
        {
            bool result = false;
            string msg = string.Empty;
            string jsonData = (Request.Form["jsonData3"]).ToString();
            List<T_GeneralLedgerAccount> List = Newtonsoft.Json.JsonConvert.DeserializeObject<List<T_GeneralLedgerAccount>>(jsonData);
            if (List.Count > 0)
            {
                for (int i = 0; i < List.Count; i++)
                {
                    result = new AccountSvc().UpdateUsingThLA(List[i].TDA_GUID, List[i].ParentAccGuid, List[i].State, Session["CurrentCompanyGuid"].ToString());
                }
            }


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
        /// 更新使用的总账科目
        /// </summary>
        /// <param name="accCodes">总账科目代码串</param>
        /// <returns></returns>
        public string UpdUsingLedgerAcc(string accCodes)
        {
            bool result = new AccountSvc().UpdUsingLedgerAcc(accCodes.Trim(','), Session["CurrentCompanyGuid"].ToString());
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
        /// 更新总账科目信息
        /// </summary>
        /// <param name="acc">总账科目对象</param>
        /// <returns></returns>
        public string UpdLedgerAcc(T_GeneralLedgerAccount acc)
        {
            AccountSvc svc = new AccountSvc();
            bool result = false;
            string msg = string.Empty;
            List<T_GeneralLedgerAccount> accs = svc.GetLedgerAccounts(Session["CurrentCompanyGuid"].ToString());
            if (accs.Any(i => !i.LA_GUID.Equals(acc.LA_GUID) && i.AccCode.Equals(acc.AccCode)))
            {
                msg = FMS.Resource.Account.Account.AccExisted;
            }
            else
            {
                result = svc.UpdLedgerAcc(acc);
                msg = result ? General.Resource.Common.Success : General.Resource.Common.Failed;
            }
            return string.Format("{{\"Result\":{0},\"Msg\":\"{1}\"}}"
                , result.ToString().ToLower(), msg);
        }

        /// <summary>
        /// 删除总账科目
        /// </summary>
        /// <param name="id">总账科目标识</param>
        /// <returns></returns>
        public string DelLedgerAcc(string id)
        {
            bool result = new AccountSvc().DelLedgerAcc(id);
            string msg = result ? General.Resource.Common.Success : General.Resource.Common.Failed;
            return string.Format("{{\"Result\":{0},\"Msg\":\"{1}\"}}"
                , result.ToString().ToLower(), msg);
        }


        ///<summary>
        ///验证科目余额
        ///</summary>
        public string CheckSubAmount(string RPer,string SubName,string Amount)
        {
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            var result = AccountSvc.CheckSubjectAmount(RPer,SubName,C_GUID,Amount);
            string msg = string.Empty;
            if (result == 1)
            {

                msg = "符合";
            }
            else
            {
                msg = "输入金额大于科目余额";
            }
            return string.Format("{{\"Result\":{0},\"Msg\":\"{1}\"}}"
                , result.ToString().ToLower(), msg);
        }

    }
}
