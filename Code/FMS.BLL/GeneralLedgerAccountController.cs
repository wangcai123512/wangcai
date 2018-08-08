using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BaseController;
using FMS.DAL;
using FMS.Model;

namespace FMS.BLL
{
    /// <summary>
    /// 总账科目
    /// </summary>
    public class GeneralLedgerAccountController : UserController
    {
        public GeneralLedgerAccountController()
            : base("General_Ledger_Account")
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
                return View(new AccountSvc().GetLedgerAccounts(Session["CurrentCompany"].ToString()).FirstOrDefault(i => i.LA_GUID.Equals(id)));
            }
        }

        /// <summary>
        /// 获取总账科目列表数据
        /// </summary>
        /// <returns></returns>
        public JsonResult GetLedgerAccount()
        {
            return Json(new AccountSvc().GetLedgerAccounts(Session["CurrentCompany"].ToString()));
        }

        /// <summary>
        /// 更新使用的总账科目
        /// </summary>
        /// <param name="accCodes">总账科目代码串</param>
        /// <returns></returns>
        public string UpdUsingLedgerAcc(string accCodes)
        {
            bool result = new AccountSvc().UpdUsingLedgerAcc(accCodes.Trim(','), Session["CurrentCompany"].ToString());
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
            List<T_GeneralLedgerAccount> accs = svc.GetLedgerAccounts(Session["CurrentCompany"].ToString());
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
    }
}
