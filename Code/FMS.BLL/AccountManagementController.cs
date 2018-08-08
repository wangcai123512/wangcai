using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseController;
using System.Web.Mvc;
using FMS.DAL;
using FMS.Model;

namespace FMS.BLL
{
    /// <summary>
    /// 账号管理
    /// </summary>
    public class AccountManagementController:UserController
    {

        public AccountManagementController()
            : base("Account_Management")
        { }
        /// <summary>
        /// 列表页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 银行信息页
        /// </summary>
        /// <param name="id">银行标识</param>
        /// <returns></returns>
        public ActionResult Bank(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return View(new T_Bank() { 
                    B_GUID = Guid.NewGuid().ToString()
                });
            }
            else
            {
                return View(new BankAccountSvc().GetBank(Session["CurrentCompany"].ToString(), id).FirstOrDefault());
            }
        }

        /// <summary>
        /// 账号信息页
        /// </summary>
        /// <param name="bid">银行标识</param>
        /// <param name="id">账号标识</param>
        /// <returns></returns>
        public ActionResult BankAccount(string bid,string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return View(new T_BankAccount() { 
                    BA_GUID = Guid.NewGuid().ToString(),
                    C_GUID = Session["CurrentCompany"].ToString(),
                    B_GUID = bid
                });
            }
            else
            {
                return View(new BankAccountSvc().GetBankAccount(Session["CurrentCompany"].ToString(), id).FirstOrDefault());
            }
        }

        /// <summary>
        /// 获取银行和账号的Json
        /// </summary>
        /// <returns></returns>
        public string GetBankAccounts()
        {
            StringBuilder strJson = new StringBuilder("[ ");
            string strFmt = "{{\"ID\":\"{0}\",\"Name\":\"{1}\",\"children\":{2},\"IsRoot\":true}},";
            foreach (T_Bank bank in new BankAccountSvc().GetBank(Session["CurrentCompany"].ToString()))
            {
                strJson.AppendFormat(strFmt,bank.B_GUID,bank.Name,GetJson(bank.B_GUID));
            }
            strJson.Remove(strJson.Length - 1, 1);
            strJson.Append("]");
            return strJson.ToString();
        }

        /// <summary>
        /// 获取银行下账号的Json
        /// </summary>
        /// <param name="pid">父级标识即银行标识</param>
        /// <returns></returns>
        private string GetJson(string pid)
        {
            StringBuilder strJson = new StringBuilder("[ ");
            string strFmt = "{{\"ID\":\"{0}\",\"Name\":\"{1}\",\"children\":{2}}},";
            foreach (T_BankAccount acc in new BankAccountSvc().GetBankAccount(Session["CurrentCompany"].ToString()).Where(i=>i.B_GUID.Equals(pid)))
            {
                strJson.AppendFormat(strFmt, acc.BA_GUID, acc.Account, "[]");
            }
            strJson.Remove(strJson.Length - 1, 1);
            strJson.Append("]");
            return strJson.ToString();
        }

        /// <summary>
        /// 更新银行信息
        /// </summary>
        /// <param name="bank">银行信息对象</param>
        /// <returns></returns>
        public string UpdBank(T_Bank bank)
        {
            bank.C_GUID = Session["CurrentCompany"].ToString();
            bool result = new BankAccountSvc().UpdBank(bank);
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
        /// 更新账号信息
        /// </summary>
        /// <param name="bankAccount">账号信息对象</param>
        /// <returns></returns>
        public string UpdBankAccount(T_BankAccount bankAccount)
        {
            bankAccount.C_GUID = Session["CurrentCompany"].ToString();
            bool result = new BankAccountSvc().UpdBankAccount(bankAccount);
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
        /// 删除银行信息
        /// </summary>
        /// <param name="id">银行标识</param>
        /// <returns></returns>
        public string DelBank(string id)
        {
            bool result = new BankAccountSvc().DelBank(id);
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
        /// 删除账号信息
        /// </summary>
        /// <param name="id">账号标识</param>
        /// <returns></returns>
        public string DelBankAccount(string id)
        {
            bool result = new BankAccountSvc().DelBankAccount(id);
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
    }
}
