using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseController;
using System.Web.Mvc;
using FMS.Model;
using FMS.DAL;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Common.Models;

namespace FMS.BLL
{
    /// <summary>
    /// 利润表
    /// </summary>
    public class IncomeStatementController : UserController
    {
        public IncomeStatementController()
            : base("IncomeStatement")
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
        /// 获取利润表列表(废弃)
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// 2017/1/4    liujf   update
        /// </remarks>
        public string GetReportList(string reportDate, string type)
        {
            string CId = CompanyId();

            List<T_Report<T_IncomeStatementTemplate>> lst = ReportSvc.GetIncomeStatementList(CId, reportDate, type);

            return JsonConvert.SerializeObject(lst);
        }

        public string GetReportDateList(string reportDate, string type)
        {
            string CId = CompanyId();

            List<T_Report<T_IncomeStatementTemplate>> lst = ReportSvc.GetReportDateList<T_IncomeStatementTemplate>(CId, reportDate, type);

            return JsonConvert.SerializeObject(lst);
        }

        /// <summary>
        /// 利润表信息页
        /// </summary>
        /// <param name="id">利润表标识</param>
        /// <returns></returns>
        public ActionResult IncomeStatement(string id, string reportDate, string type, string status, string period)
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
            ViewBag.Period = period;
            ViewBag.Type = type;
            ViewBag.RepDate = reportDate;
            ViewBag.status = status;

            if (string.IsNullOrEmpty(id))
            {
                T_Report<T_IncomeStatementTemplate> rep = new ReportSvc().GenPerviewIncomeStatement(CompanyId(), reportDate, type);

                ViewBag.IsView = false;
                ViewBag.RepId = string.Empty;
                return View(rep);
            }
            else
            {

                ViewBag.IsView = true;
                ViewBag.RepStatus = status;
                ViewBag.RepId = id;
                //获取报表日期
                return View();
            }
        }


        /// <summary>
        /// 获取利润表明细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <remarks>
        /// 2017/1/4    liujf   update
        /// </remarks>
        public string GetIncomeStatement(string id, string reportDate, string type)
        {
            T_Report<T_IncomeStatementTemplate> rep = new T_Report<T_IncomeStatementTemplate>();
            if (string.IsNullOrEmpty(id))
            {
                rep = new ReportSvc().GenPerviewIncomeStatement(CompanyId(), reportDate, type);
            }
            else
            {
                //rep = new ReportSvc().GetIncomeStatement(id);
                rep = new ReportSvc().GenPerviewIncomeStatement(CompanyId(), reportDate, type);
            }
            return new JavaScriptSerializer().Serialize(rep.Details);
        }
       /// <summary>
        /// 结账反结账
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// 2017/3/22   sunp   update
        /// </remarks>
        public string settled(string id, string status)
        {
            ExceResult res = new ExceResult();
            bool result = new ReportSvc().settled(id, status);

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

        public string settledTest(string id, string status, string RepDate, string period)
        {
            string CId = CompanyId();
            ExceResult res = new ExceResult();
            bool result = new ReportSvc().settledTest(CId, id, status, RepDate, period);

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
        /// 结账反结账(利润表)
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// 2018/06/07   zm
        /// </remarks>
        public string settledprofit(string id, string status, string repDate, string period)
        {
            string CId = CompanyId();
            ExceResult res = new ExceResult();
            bool result = new ReportSvc().settledprofit(id, status, CId, repDate, period);

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
        /// 结账反结账(现金流量表)
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// 2018/06/07   zm
        /// </remarks>
        public string settledcash(string id, string status, string repDate, string period)
        {
            string CId = CompanyId();
            ExceResult res = new ExceResult();
            bool result = new ReportSvc().settledcash(id, status, CId, repDate, period);

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
        /// 判断是否可以反结账结账
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// 2017/3/29   sunp   update
        /// </remarks>
        public string isFinish(string repDate, string status, string type,string period)
        {
            int count = 0;
            ExceResult res = new ExceResult();
            string CId = CompanyId();
            List<T_Report<IncomeStatement>> rep = new List<T_Report<IncomeStatement>>();
            rep = new ReportSvc().isFinish(repDate, status, out count, type, period, CId);
            string msg = string.Empty;
            bool result = false;
            if (rep.Count == 0)
            {
                msg = General.Resource.Common.Success;
                result = true;
            }
            else
            {
                msg = General.Resource.Common.Failed;
                result = false;
            }

            return string.Format("{{\"Result\":{0},\"Msg\":\"{1}\"}}"
                , result.ToString().ToLower(), msg);

        }
        /// <summary>
        /// 判断流水账部分编辑修改，新增是否已结账
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// 2017/4/3   sunp   update
        /// </remarks>
        public string isCheckout(string repDate, string status)
        {
            int count = 0;
            ExceResult res = new ExceResult();
            string CId = CompanyId();
            List<T_Report<IncomeStatement>> rep = new List<T_Report<IncomeStatement>>();
            rep = new ReportSvc().isCheckout(repDate, status, out count, CId);
            string msg = string.Empty;
            bool result = false;
            if (rep.Count == 0)
            {
                msg = General.Resource.Common.Success;
                result = true;
            }
            else
            {
                msg = General.Resource.Common.Failed;
                result = false;
            }

            return string.Format("{{\"Result\":{0},\"Msg\":\"{1}\"}}"
                , result.ToString().ToLower(), msg);

        }

        /// <summary>
        /// 判断增值税是否结算
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// 2018/7/23   zm   update
        /// </remarks>
        public string isZenZhiCheckout(string repDate, string TaxName,string TaxationType)
        {
            int count = 0;
            ExceResult res = new ExceResult();
            string CId = CompanyId();
            //string c_TaxationType = Session["c_TaxationType"].ToString();
            
            new ReportSvc().isZenZhiCheckout(repDate, out count, CId, TaxName, TaxationType);
            string msg = string.Empty;
            bool result = false;
            if (count == 0)
            {
                msg = General.Resource.Common.Success;
                result = true;
            }
            else
            {
                msg = General.Resource.Common.Failed;
                result = false;
            }

            return string.Format("{{\"Result\":{0},\"Msg\":\"{1}\"}}"
                , result.ToString().ToLower(), msg);

        }
        





        /// <summary>
        /// 更新利润表
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// 2017/1/3    liujf   update
        /// </remarks>
        public string CreateIncomeStatement(string repDate, string type)
        {
            ExceResult res = new ExceResult();
            string result = new ReportSvc().CreateIncomeStatement(CompanyId(), repDate, type);
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

        public string GetRPVoucher(string id, string reportDate, string type, string flag)
        {
            T_Report<T_IncomeStatementTemplate> rep = new T_Report<T_IncomeStatementTemplate>();
                rep = new ReportSvc().GetRPVoucher(CompanyId(), flag, reportDate, type);
            return new JavaScriptSerializer().Serialize(rep.Details);
        }
    }
}
