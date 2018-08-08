using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseController;
using System.Web.Mvc;
using FMS.DAL;
using System.Web.Script.Serialization;
using FMS.Model;

namespace FMS.BLL
{
    /// <summary>
    /// 科目汇总查询
    /// </summary>
    public class AccountSummaryController : UserController
    {
        public AccountSummaryController() : base("AccountSummary") { }

        /// <summary>
        /// Index页
        /// </summary>
        /// <returns></returns>
        public ViewResult Index()
        {
            return View();
        }

        /// <summary>
        /// 获取科目汇总信息
        /// </summary>
        /// <remarks>借用T_BeginningBalance模型</remarks>
        /// <returns></returns>
        public string GetAccSum()
        {
            string strFmt = "{{\"total\":{0},\"rows\":{1}}}";
            List<T_BeginningBalance> beginningBalance =
                new ReportSvc().GetAccountSummary(Session["CurrentCompanyGuid"].ToString());
            
            return string.Format(strFmt,
                beginningBalance.Count,
                GenBalanceJson(beginningBalance));
        }

        /// <summary>
        /// 获取科目汇总信息的JSON
        /// </summary>
        /// <param name="ds">数据源</param>
        /// <returns></returns>
        private string GenBalanceJson(List<T_BeginningBalance> ds)
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
        /// 获取科目汇总信息的明细
        /// </summary>
        /// <param name="accId">科目标识</param>
        /// <returns></returns>
        public string GetAccSumDtl(string accId)
        {
            return new JavaScriptSerializer().Serialize(
                new ReportSvc().GetAccountSummaryDetails(Session["CurrentCompanyGuid"].ToString(), accId));
        }
    }
}
