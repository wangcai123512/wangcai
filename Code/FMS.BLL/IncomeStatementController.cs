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
    /// 利润表
    /// </summary>
    public class IncomeStatementController:UserController
    {
        public IncomeStatementController()
            : base("Income_Statement")
        { }

        /// <summary>
        /// 列表页
        /// </summary>
        /// <returns></returns>
        public ViewResult Index()
        {
            return View();
        }

        /// <summary>
        /// 利润表信息页
        /// </summary>
        /// <param name="id">利润表标识</param>
        /// <returns></returns>
        public ViewResult IncomeStatement(string id)
        {
            IncomeStatement rep = new IncomeStatement();
            if (string.IsNullOrEmpty(id))
            {
                rep = new ReportSvc().GenPerviewIncomeStatement(Session["CurrentCompany"].ToString());
            }
            else
            {
                rep = new ReportSvc().GetIncomeStatement(id);
            }
            return View(rep);
        }

        /// <summary>
        /// 获取利润表列表信息
        /// </summary>
        /// <param name="page">页索引</param>
        /// <param name="rows">页大小</param>
        /// <returns></returns>
        public string GetIncomeStatements(string page,string rows)
        {
            int count = 0;
            List<T_Report> reps = new ReportSvc().GetIncomeStatements
                (Session["CurrentCompany"].ToString(), int.Parse(page), int.Parse(rows), out count);
            string strFmt = "{{\"total\":{0},\"rows\":{1}}}";
            return string.Format(strFmt, count, new JavaScriptSerializer().Serialize(reps));
        }
        
        /// <summary>
        /// 更新利润表
        /// </summary>
        /// <returns></returns>
        public string UpdIncomeStatement()
        {
            string strFmt = "{{\"Result\":{0},\"Msg\":\"{1}\"}}";
            bool result = new ReportSvc().UpdIncomeStatement(Session["CurrentCompany"].ToString());
            return string.Format(strFmt, result.ToString().ToLower(),
                result ? General.Resource.Common.Success : General.Resource.Common.Failed);
        }
    }
}
