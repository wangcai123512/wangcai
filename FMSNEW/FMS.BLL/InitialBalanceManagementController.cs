using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseController;
using System.Web.Mvc;
using FMS.DAL;
using FMS.Model;
using System.Web.Script.Serialization;

namespace FMS.BLL
{
    /// <summary>
    /// 期初余额设置
    /// </summary>
    public class InitialBalanceManagementController:UserController
    {

        public InitialBalanceManagementController()
            : base("InitialBalanceManagement")
        { }
        /// <summary>
        /// 列表页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            //new BalanceSvc().GetInitialBalanceRecord(Session["CurrentCompanyGuid"].ToString()).FirstOrDefault();
            List<T_BankAccount> bankAccount2 = new BankAccountSvc().GetBankAccount(Session["CurrentCompanyGuid"].ToString());
            ViewBag.BankList2 = bankAccount2;
            List<T_BeginningBalance> balanceList = new BalanceSvc().GetInitialBalanceRecord(Session["CurrentCompanyGuid"].ToString(),null);
            ViewBag.balanceList = balanceList;
            return View();
        }

        public ActionResult InitialBalanceSetting()
        {
            return View();
        }
        public string GetBankInformation() {
            List<T_BankAccount> bankAccount2 = new BankAccountSvc().GetBankAccount(Session["CurrentCompanyGuid"].ToString());
            string json = new JavaScriptSerializer().Serialize(bankAccount2);
            return json;
        }
        public string GetInitialBalanceRecord(string Acc_Name)
        {
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
           
                List<T_BeginningBalance> recs = new BalanceSvc().GetInitialBalanceRecord(C_GUID,Acc_Name);
                string json = new JavaScriptSerializer().Serialize(recs);
                return json;
            
        }
        public string UpdInitialBalanceRecord(List<T_BeginningBalance> list,List<T_BankAccount> bankItems)
        {
            bool result = false;
            string msg = string.Empty;
           foreach (T_BeginningBalance balence in list ){
             balence.R_GUID = Guid.NewGuid().ToString();
            balence.C_GUID = Session["CurrentCompanyGuid"].ToString();
            result = new BalanceSvc().UpdateInitialBalanceRecord(balence);
           }
            if (result)
            {
                BankAccountSvc bankAccount = new BankAccountSvc();
                try
                {
                    foreach (T_BankAccount account in bankItems)
                    {

                        result = bankAccount.UpdBankAmount(account);
                        if (result == false)
                        {
                            return "银行账号期初添加失败";
                        }


                    }
                }
                catch (NullReferenceException){
                    return "请添加银行账号";
                }

                return "提交成功";
            }
            else
            {
                return "提交失败";
            }
           
        }

        /// <summary>
        /// 20180629科目余额查询页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Search()
        {
           
            return View();
        }

        public ActionResult BalanceSearch() {
            return View();
         }
        /// <summary>
        /// 获取科目余额
        /// </summary>
        /// <param name="page">页索引</param>
        /// <param name="rows">页大小</param>
        /// <returns></returns>
        public string GetAccountList(string dateBegin, string dateEnd,string Name)
        {
            int count = 0;
            StringBuilder strJson = new StringBuilder();

            List<HisTr_GeneralLedgerAccount> List = new BalanceSvc().GetGenAccountList(Session["CurrentCompanyGuid"].ToString(), 1, -1, out count, dateBegin, dateEnd , Name);
            string json = new JavaScriptSerializer().Serialize(List);

            return json;
        }

        public string GetLDetailAccount(string LA_GUID)
        {

            List<HisTr_GeneralLedgerAccount> List = new BalanceSvc().GetLDetailAccount(LA_GUID, Session["CurrentCompanyGuid"].ToString());
            string json = new JavaScriptSerializer().Serialize(List);
            return json;
        }

        public string GetDAccountAmount(string strParentID)
        {
            List<HisTr_DetailedAccount> List = new BalanceSvc().GetDAccountAmount(strParentID, Session["CurrentCompanyGuid"].ToString());
            string json = new JavaScriptSerializer().Serialize(List);
            return json;
        }

        public string GetThAccountAmount(string strParentID)
        {
            List<HisTr_ThirdAccount> List = new BalanceSvc().GetThAccountAmount(strParentID, Session["CurrentCompanyGuid"].ToString());
            string json = new JavaScriptSerializer().Serialize(List);
            return json;
        }

    }
}
