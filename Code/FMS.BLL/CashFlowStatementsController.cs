using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseController;
using System.Web.Mvc;
using FMS.Model;
using FMS.DAL;
using System.Web.Script.Serialization;

namespace FMS.BLL
{
    /// <summary>
    /// 现金流量表
    /// </summary>
    public class CashFlowStatementsController : UserController
    {
        public CashFlowStatementsController()
            : base("Cash_Flow_Statements")
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
        /// 现金流量表信息页
        /// </summary>
        /// <param name="id">现金流量表标识</param>
        /// <returns></returns>
        public ActionResult CashFlowStatement(string id)
        {
            CashFlowStatement rep = new CashFlowStatement();
            if (string.IsNullOrEmpty(id))
            {
                rep = new ReportSvc().GenPerviewCashFlowStatement(Session["CurrentCompany"].ToString());
            }
            else
            {
                rep = new ReportSvc().GetCashFlowStatement(id);
            }
            return View(rep);
        }

        /// <summary>
        /// 获取现金流量表列表
        /// </summary>
        /// <param name="page">页索引</param>
        /// <param name="rows">页大小</param>
        /// <returns></returns>
        public string GetCashFlowStatements(string page, string rows)
        {
            int count = 0;
            List<T_Report> reps = new ReportSvc().GetCashFlowStatements
                (Session["CurrentCompany"].ToString(), int.Parse(page), int.Parse(rows), out count);
            string strFmt = "{{\"total\":{0},\"rows\":{1}}}";
            return string.Format(strFmt, count, new JavaScriptSerializer().Serialize(reps));
        }

        /// <summary>
        /// 更新现金流量表
        /// </summary>
        /// <returns></returns>
        public string UpdCashFlowStatement()
        {
            string strFmt = "{{\"Result\":{0},\"Msg\":\"{1}\"}}";
            bool result = new ReportSvc().UpdCashFlowStatement(Session["CurrentCompany"].ToString());
            return string.Format(strFmt, result.ToString().ToLower(),
                result ? General.Resource.Common.Success : General.Resource.Common.Failed);
        }
        
    }
}
