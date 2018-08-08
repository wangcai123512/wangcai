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
    /// 期初余额设置
    /// </summary>
    public class InitialBalanceManagementController:UserController
    {

        public InitialBalanceManagementController()
            : base("InitialBalance_Management")
        { }
        /// <summary>
        /// 列表页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            //new BalanceSvc().GetInitialBalanceRecord(Session["CurrentCompany"].ToString()).FirstOrDefault()
            return View();
        }

        public string UpdInitialBalanceRecord(T_Balance form)
        {
            bool result = false;
            string msg = string.Empty;
            form.C_GUID = Session["CurrentCompany"].ToString();
            result = new BalanceSvc().UpdInitialBalanceRecord(form);
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
