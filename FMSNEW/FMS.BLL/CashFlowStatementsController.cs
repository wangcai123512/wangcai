using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseController;
using System.Web.Mvc;
using FMS.Model;
using FMS.DAL;
using System.Web.Script.Serialization;
using EF = FMS.Models;
using Newtonsoft.Json;
using System.Data.Objects;
using Common.Models;

namespace FMS.BLL
{
    /// <summary>
    /// 现金流量表
    /// </summary>
    public class CashFlowStatementsController : UserController
    {
        public CashFlowStatementsController()
            : base("CashFlowStatements")
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
        /// 获取资产负债表列表(废弃)
        /// </summary>
        /// <returns></returns>
        public string GetReportList(string reportDate, string type)
        {
            string CId = CompanyId();

            List<T_Report<T_CashFlowItemTemplate>> lst = ReportSvc.GetCashFlowList(CId, reportDate, type);

            return JsonConvert.SerializeObject(lst);
        }

        public string GetReportDateList(string reportDate, string type)
        {
            string CId = CompanyId();

            List<T_Report<T_CashFlowItemTemplate>> lst = ReportSvc.GetReportDateList<T_CashFlowItemTemplate>(CId, reportDate, type);

            return JsonConvert.SerializeObject(lst);
        }

        /// <summary>
        /// 现金流量表信息页
        /// </summary>
        /// <param name="id">现金流量表标识</param>
        /// <returns></returns>
        public ActionResult CashFlowStatements(string id, string status, string reportDate, string type)
        {
            switch (type)
            {
                case "month":
                    ViewBag.RepTitle = reportDate + "(月度)";
                    break;
                case "quarter":
                    ViewBag.RepTitle = reportDate + "(季度)";
                    break;
                case "year":
                    ViewBag.RepTitle = reportDate + "(年度)";
                    break;
                default:
                    break;
            }

            ViewBag.Type = type;
            ViewBag.RepDate = reportDate;
            ViewBag.status = status;

            if (string.IsNullOrEmpty(id))
            {
                T_Report<T_CashFlowItemTemplate> rep = new T_Report<T_CashFlowItemTemplate>();
                rep = new ReportSvc().GenPerviewCashFlowStatement(CompanyId(), reportDate, type);

                ViewBag.IsView = false;
                ViewBag.RepId = string.Empty;
                return View(rep);
            }
            else
            {
                ViewBag.IsView = true;
                ViewBag.RepId = id;
                //获取报表日期
                return View();
            }

        }
        /// <summary>
        /// 获取现金流量明细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetCashFlowStatement(string id, string reportDate, string type)
        {
            T_Report<T_CashFlowItemTemplate> rep = new T_Report<T_CashFlowItemTemplate>();
            if (string.IsNullOrEmpty(id))
            {
                rep = new ReportSvc().GenPerviewCashFlowStatement(CompanyId(), reportDate, type);
            }
            else
            {
                //rep = new ReportSvc().GetCashFlowStatement(id);
                rep = new ReportSvc().GenPerviewCashFlowStatement(CompanyId(), reportDate, type);
            }

            return JsonConvert.SerializeObject(rep.Details);
        }

        /// <summary>
        /// 获取现金流量表列表
        /// </summary>
        /// <param name="page">页索引</param>
        /// <param name="rows">页大小</param>
        /// <returns></returns>
        public string GetCashFlowStatements(string page = "1", string rows = "10")
        {
            int count = 0;
            List<T_Report<T_CashFlowItemTemplate>> reps = new ReportSvc().GetCashFlowStatements
                (Session["CurrentCompanyGuid"].ToString());
            return new JavaScriptSerializer().Serialize(reps);

        }

        /// <summary>
        /// 更新现金流量表
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// 2017/1/3    liujf   update
        /// </remarks>
        public string UpdCashFlowStatement(string repDate, string type)
        {

            ExceResult res = new ExceResult();
            string result = new ReportSvc().UpdCashFlowStatement(CompanyId(), repDate, type);
            if (string.IsNullOrEmpty(result))
            {
                res.success = true;
            }
            else
            {
                res.success = false;
                res.msg = result;
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(res);

        }

    }
}
