using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using BaseController;
using FMS.DAL;
using FMS.Model;
using FMS.Resource.FinanceReport;

namespace FMS.BLL
{
    /// <summary>
    /// 资产负债表
    /// </summary>
    public class BalanceSheetController : UserController
    {
        public BalanceSheetController() : base("Balance_Sheet") { }

        /// <summary>
        /// 列表页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        #region BalanceSheet
        /// <summary>
        /// 资产负债表信息页面
        /// </summary>
        /// <param name="id">资产负债表标识</param>
        /// <returns></returns>
        public ActionResult BalanceSheet(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                T_Report rep = new ReportSvc().GenPerviewBalanceSheet(Session["CurrentCompany"].ToString());
                return View(rep);
            }
            else
            {
                return View(new ReportSvc().GetBalanceSheet(id));
            }

        }

        /// <summary>
        /// 更新资产负债表
        /// </summary>
        /// <returns></returns>
        public string UpdBalanceSheet()
        {
            string strFmt = "{{\"Result\":{0},\"Msg\":\"{1}\"}}";
            bool result = new ReportSvc().UpdBalanceSheet(Session["CurrentCompany"].ToString());
            return string.Format(strFmt, result.ToString().ToLower(),
                result ? General.Resource.Common.Success : General.Resource.Common.Failed);
        }

        /// <summary>
        /// 获取资产负债表列表
        /// </summary>
        /// <param name="rows">页大小</param>
        /// <param name="page">页索引</param>
        /// <returns></returns>
        public string GetBalanceSheets(string rows, string page)
        {
            int count = 0;
            List<T_Report> reps = new ReportSvc()
                .GetBalanceSheets(Session["CurrentCompany"].ToString(), int.Parse(page), int.Parse(rows), out count);
            string strFmt = "{{\"total\":{0},\"rows\":{1}}}";
            return string.Format(strFmt, count, new JavaScriptSerializer().Serialize(reps));
        }
        #endregion

        #region BeginningBalance
        /// <summary>
        /// 期初数页面
        /// </summary>
        /// <returns></returns>
        public ActionResult BeginningBalance()
        {
            return View();
        }

        /// <summary>
        /// 验证起初数
        /// </summary>
        /// <returns></returns>
        public string VaildBeginningBalance()
        {
            string strFmt = "{{\"Result\":{0},\"Msg\":\"{1}\"}}";
            List<T_BeginningBalance> beginningBalance =
                new ReportSvc().VaildBeginningBalance(Session["CurrentCompany"].ToString());
            bool result = beginningBalance.Any();
            return string.Format(strFmt, result.ToString().ToLower(),
                result ? string.Empty : FinanceReport.BeginningBalanceError);
        }

        /// <summary>
        /// 获取起初数
        /// </summary>
        /// <returns></returns>
        public string GetBeginningBalance()
        {
            string strFmt = "{{\"total\":{0},\"rows\":{1},\"footer\":{2}}}";
            string strFooter = "[{{\"Acc_Name\":\"资产合计:\",\"Money\":{0}}},{{\"Acc_Name\":\"负债及所有者权益合计:\",\"Money\":\"{1}\"}}]";
            List<T_BeginningBalance> beginningBalance =
                new ReportSvc().GetBeginningBalance(Session["CurrentCompany"].ToString());
            //return string.Format(strFmt, beginningBalance.Count, GenBeginningBalanceJson(beginningBalance,string.Empty));
            return string.Format(strFmt, 
                beginningBalance.Count, 
                GenBeginningBalanceJson(beginningBalance), 
                string.Format(strFooter, 0, 0));
        }

        /// <summary>
        /// 获取期初数json
        /// </summary>
        /// <param name="ds">数据源</param>
        /// <param name="pid">父级标识</param>
        /// 未使用
        /// <returns></returns>
        private string GenBeginningBalanceJson(List<T_BeginningBalance> ds,string pid)
        {
            string strFmt = "{{\"ID\":\"{3}\",\"Name\":\"{0}\",\"Money\":{1},\"children\":{2}}},";
            
            StringBuilder strJson = new StringBuilder("[ ");
            foreach (T_BeginningBalance item in ds.Where(i=>i._parentId.Equals(pid)).OrderBy(i=>i.Acc_Code))
            {
                strJson.AppendFormat(strFmt, item.Acc_Name, item.Money, GenBeginningBalanceJson(ds,item.Acc_GUID),item.Acc_GUID);
            }
            strJson.Remove(strJson.Length - 1, 1);
            strJson.Append("]");
            return strJson.ToString();
        }

        /// <summary>
        /// 获取期初数json
        /// </summary>
        /// <param name="ds">数据源</param>
        /// <returns></returns>
        private string GenBeginningBalanceJson(List<T_BeginningBalance> ds)
        {
            string strRowFmt = "{{\"Acc_GUID\":\"{3}\",\"Acc_Name\":\"{0}\",\"Money\":{1},\"_parentId\":\"{2}\"}},";
            StringBuilder strJson = new StringBuilder("[ ");
            foreach (T_BeginningBalance item in ds.OrderBy(i => i.Acc_Code))
            {
                strJson.AppendFormat(strRowFmt, item.Acc_Name, item.Money, item._parentId, item.Acc_GUID);
            }
            strJson.Remove(strJson.Length - 1, 1);
            strJson.Append("]");
            return strJson.ToString();
        }

        /// <summary>
        /// 更新期初数
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string UpdBeginningBalance(string data)
        {
            string strFmt = "{{\"Result\":{0},\"Msg\":\"{1}\"}}";
            string msg = string.Empty;
            List<T_BeginningBalance> beginningBalance =
                new JavaScriptSerializer().Deserialize<List<T_BeginningBalance>>(data);
            T_BeginningBalance currItem = new T_BeginningBalance();
            while (beginningBalance.Any(i=>i.children.Count > 0))
            {
                currItem = beginningBalance.Where(i => i.children.Count > 0).FirstOrDefault();
                beginningBalance.AddRange(currItem.children);
                currItem.children = new List<T_BeginningBalance>();
            }
            bool result = false;
            if (true)
            {
                result = new ReportSvc().UpdBeginningBalance(beginningBalance, Session["CurrentCompany"].ToString());
                msg = result ? General.Resource.Common.Success : General.Resource.Common.Failed;
            }
            else
            {
                msg = FMS.Resource.FinanceReport.FinanceReport.VaildError;
            }
            return string.Format(strFmt, result.ToString().ToLower(), msg);
        }
        #endregion
        

    }
}
