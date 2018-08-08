using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using BaseController;
using Common.Models;
using FMS.DAL;
using FMS.Model;
using FMS.Resource.FinanceReport;
using Newtonsoft.Json;
using System.Data;
using System.Transactions;
using System;
namespace FMS.BLL
{
    /// <summary>
    /// 资产负债表
    /// </summary>
    public class BalanceSheetController : UserController
    {
        public BalanceSheetController() : base("BalanceSheet") { }


        /// <summary>
        /// 列表页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {

            return View();
        }

        /// <summary>
        /// 获取资产负债表列表
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// 2017/1/3    liujf   update
        /// </remarks>
        public string GetReportList(string reportDate, string type)
        {
            string CId = CompanyId();

            List<T_Report<T_BalanceSheetTemplate>> lst = ReportSvc.GetBalanceSheetList(CId, reportDate, type);

            return JsonConvert.SerializeObject(lst);
        }

        public string GetReportDateList(string reportDate, string type)
        {
            string CId = CompanyId();

            List<T_Report<T_BalanceSheetTemplate>> lst = ReportSvc.GetReportDateList<T_BalanceSheetTemplate>(CId, reportDate, type);

            return JsonConvert.SerializeObject(lst);
        }

        #region BalanceSheet
        /// <summary>
        /// 资产负债表信息页面
        /// </summary>
        /// <param name="id">资产负债表标识</param>
        /// <returns></returns>
        /// <remarks>
        /// 2017/1/3    liujf   update
        /// </remarks>
        public ActionResult BalanceSheet(string id, string reportDate, string type, string status,string end)
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
                case "seach":
                    ViewBag.RepTitle = end + "(时间)";
                    break;
                default:
                    break;
            }
            ViewBag.Type = type;
            ViewBag.RepDate = reportDate;
            if (string.IsNullOrEmpty(id))
            {
                T_Report<T_BalanceSheetTemplate> rep = new ReportSvc().GenPerviewBalanceSheet(CompanyId(), reportDate, type);

                ViewBag.IsView = false;
                ViewBag.RepId = string.Empty;
                ViewBag.Status = status;
                ViewBag.End = end;
                return View(rep);
            }
            else
            {
                ViewBag.IsView = true;
                ViewBag.RepId = id;
                ViewBag.Status = status;
                ViewBag.End = end;
                //获取报表日期
                return View();
            }
        }

        /// <summary>
        /// 获取资产负债表明细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <remarks>
        /// 2017/1/3    liujf   update
        /// </remarks>
        public string GetBalanceSheet(string id, string reportDate, string type, string status, string end)
        {
            List<T_BalanceSheetTemplate> lstBanlance = GetBanlance(id, reportDate, type, status, end);
            return new JavaScriptSerializer().Serialize(lstBanlance);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="reportDate"></param>
        /// <param name="type"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public List<T_BalanceSheetTemplate> GetBanlance(string id, string reportDate, string type, string status,string end)
        {
            T_Report<T_BalanceSheetTemplate> rep = new T_Report<T_BalanceSheetTemplate>();
            string Begin_date = string.Empty;
            string End_date = string.Empty;
            string report_fix = "BS";
            ReportSvc svc = new ReportSvc();
            if (type != "seach")
            {
                Common.Function.Common.GetReportDateAndRepNo(reportDate, type, ref report_fix, ref Begin_date, ref End_date);
                var date = DateTime.Parse(Begin_date);
                Begin_date = date.AddMonths(1 - date.Month).ToString("yyyy/MM/dd");
            }
            else
            {
                Begin_date = DateTime.Now.ToString("yyyy")+"/01/01";
                End_date = end;
                report_fix += "D";
            }
            rep.RepNo = report_fix;
            
            if (string.IsNullOrEmpty(id))
            {
                rep = svc.GenPerviewBalanceSheet(CompanyId(), reportDate, type);
            }
            else
            {
                rep = svc.GetBalanceSheet(id);
            }
            if (status != "已结账")
            {
                List<T_BalanceSheetTemplate> lstBan = rep.Details;
                foreach (var item in lstBan)
                {
                    if (item.asset_row_no != 14 && item.asset_row_no != 9 && item.asset_row_no != 24
                    && item.asset_row_no != 26 && item.asset_row_no != 28)
                    {
                        item.asset_start_amount = null;
                        item.asset_end_amount = null;
                    }
                    if (item.debt_row_no != 38 && item.debt_row_no != 40 && item.debt_row_no != 40 && item.debt_row_no != 45)
                    {
                        item.debt_start_amount = null;
                        item.debt_end_amount = null;
                    }
                    if (item.debt_row_no == 51)
                    {
                        item.debt_end_amount = svc.GetProfitYearAmount(CompanyId(), Begin_date, End_date);
                    }
                }
                GetBalanceAmount(rep, Begin_date, End_date);
            }
            return rep.Details;
        }

        public void GetBalanceAmount(T_Report<T_BalanceSheetTemplate> rep, string beginDate, string endDate)
        {
            DataTable dtBanlanceAmount = new DataTable();
            dtBanlanceAmount = new ReportSvc().GetBanlanceAmount(CompanyId(), beginDate, endDate);
            List<T_BalanceSheetTemplate> lstBanlance = rep.Details;
            for (int i = 0; i < lstBanlance.Count(); i++)
            {
                if (lstBanlance[i].asset_row_no != null)
                {
                    DataRow[] drBanlance = dtBanlanceAmount.Select("Balanceitem_name='" + lstBanlance[i].asset_item_name + "'");
                    if (drBanlance.Length > 0)
                    {
                        string strName = string.Empty;
                        for (int j = 0; j < drBanlance.Length; j++)
                        {
                            if (strName != drBanlance[j]["Name"].ToString())
                            {
                                if (drBanlance[j]["addOrDecrease"].ToString() == "0")
                                {
                                    lstBanlance[i].asset_start_amount = Common.Function.Common.GetAmountValue(lstBanlance[i].asset_start_amount.ToString()) + Common.Function.Common.GetAmountValue(drBanlance[j]["OldAmount"].ToString());
                                    lstBanlance[i].asset_end_amount = Common.Function.Common.GetAmountValue(drBanlance[j]["OldAmount"].ToString()) + Common.Function.Common.GetAmountValue(lstBanlance[i].asset_end_amount.ToString()) + Common.Function.Common.GetAmountValue(drBanlance[j]["Amount"].ToString());
                                }
                                else
                                {
                                    lstBanlance[i].asset_start_amount = Common.Function.Common.GetAmountValue(lstBanlance[i].asset_start_amount.ToString()) - Common.Function.Common.GetAmountValue(drBanlance[j]["OldAmount"].ToString());
                                    lstBanlance[i].asset_end_amount = Common.Function.Common.GetAmountValue(drBanlance[j]["OldAmount"].ToString()) + Common.Function.Common.GetAmountValue(lstBanlance[i].asset_end_amount.ToString()) + Common.Function.Common.GetAmountValue(drBanlance[j]["Amount"].ToString());
                                }
                            }
                            //else
                            //{
                            //    if (drBanlance[j]["addOrDecrease"].ToString() == "0")
                            //    {
                            //        lstBanlance[i].asset_end_amount = Common.Function.Common.GetAmountValue(drBanlance[j]["Amount"].ToString());
                            //    }
                            //    else
                            //    {
                            //        lstBanlance[i].asset_end_amount = Common.Function.Common.GetAmountValue(drBanlance[j]["Amount"].ToString());
                            //    }
                            //}
                            strName = drBanlance[j]["Name"].ToString();
                        }
                    }
                }
                if (lstBanlance[i].debt_row_no != null)
                {
                    DataRow[] drBanlance = dtBanlanceAmount.Select("Balanceitem_name='" + lstBanlance[i].debt_item_name + "'");
                    if (drBanlance.Length > 0)
                    {
                        string strName = string.Empty;
                        for (int j = 0; j < drBanlance.Length; j++)
                        {
                            if (strName != drBanlance[j]["Name"].ToString())
                            {
                                if (drBanlance[j]["addOrDecrease"].ToString() == "0")
                                {
                                    lstBanlance[i].debt_start_amount = Common.Function.Common.GetAmountValue(lstBanlance[i].debt_start_amount.ToString()) + Common.Function.Common.GetAmountValue(drBanlance[j]["OldAmount"].ToString());
                                    lstBanlance[i].debt_end_amount = Common.Function.Common.GetAmountValue(drBanlance[j]["OldAmount"].ToString()) + Common.Function.Common.GetAmountValue(lstBanlance[i].debt_end_amount.ToString()) + Common.Function.Common.GetAmountValue(drBanlance[j]["Amount"].ToString());
                                }
                                else
                                {
                                    lstBanlance[i].debt_start_amount = Common.Function.Common.GetAmountValue(lstBanlance[i].debt_start_amount.ToString()) - Common.Function.Common.GetAmountValue(drBanlance[j]["OldAmount"].ToString());
                                    lstBanlance[i].debt_end_amount = Common.Function.Common.GetAmountValue(drBanlance[j]["OldAmount"].ToString())+Common.Function.Common.GetAmountValue(lstBanlance[i].debt_end_amount.ToString()) + Common.Function.Common.GetAmountValue(drBanlance[j]["Amount"].ToString());
                                }
                            }
                            //else
                            //{
                            //    if (drBanlance[j]["addOrDecrease"].ToString() == "0")
                            //    {
                            //        lstBanlance[i].debt_end_amount = Common.Function.Common.GetAmountValue(drBanlance[j]["Amount"].ToString());
                            //    }
                            //    else
                            //    {
                            //        lstBanlance[i].debt_end_amount = Common.Function.Common.GetAmountValue(drBanlance[j]["Amount"].ToString());
                            //    }
                            //}
                            strName = drBanlance[j]["Name"].ToString();
                        }
                    }
                }
            }
        }


        /// <summary>
        /// 更新资产负债表
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// 2017/1/3    liujf   update
        /// </remarks>
        /// 
        [HttpPost]
        public string CreateBalanceSheet(string repDate, string type, List<T_BalanceSheetTemplate> lstBanlance)
        {
            ExceResult res = new ExceResult();
            string result = "";
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {
                string strReportID = Guid.NewGuid().ToString();
                string strRepNo = "BS";
                string strStart = string.Empty;
                string strEnd = string.Empty;
                Common.Function.Common.GetReportDateAndRepNo(repDate, type, ref strRepNo, ref strStart, ref strEnd);

                result = new ReportSvc().CreateReportInfo(CompanyId(), repDate, type, strReportID, strRepNo + repDate);
                if (string.IsNullOrEmpty(result))
                {
                    T_ReportDetails report = new T_ReportDetails();
                    DataTable dtBanlance = Common.Function.Common.GetTableByEntity<T_ReportDetails>(report);
                    foreach (var item in lstBanlance)
                    {
                        //result = new ReportSvc().CreateReportDetailByBanlance(item, strReportID);
                        //if (!string.IsNullOrEmpty(result))
                        //{
                        //    break;
                        //}
                        addDataToTableByEntity(item, dtBanlance, strReportID);
                    }
                    result = new ReportSvc().CreateReportDetailByBanlance(dtBanlance, typeof(T_ReportDetails).Name);
                }
                if (string.IsNullOrEmpty(result))
                {
                    scope.Complete();
                }
            }
            //string result = new ReportSvc().UpdBalanceSheet(CompanyId(), repDate,type,lstBanlance);

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

        [HttpPost]
        public string UpdBalanceSheet(string id, string repDate, string type, List<T_BalanceSheetTemplate> lstBanlance)
        {
            ExceResult res = new ExceResult();
            string result = "";
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {
                result = new ReportSvc().DeleteReportDetail(id);
                if (string.IsNullOrEmpty(result))
                {
                    T_ReportDetails report = new T_ReportDetails();
                    DataTable dtBanlance = Common.Function.Common.GetTableByEntity<T_ReportDetails>(report);
                    foreach (var item in lstBanlance)
                    {
                        //result = new ReportSvc().CreateReportDetailByBanlance(item, id);
                        //if (!string.IsNullOrEmpty(result))
                        //{
                        //    break;
                        //}
                        addDataToTableByEntity(item, dtBanlance, id);
                    }
                    result = new ReportSvc().CreateReportDetailByBanlance(dtBanlance, typeof(T_ReportDetails).Name);
                }
                if (string.IsNullOrEmpty(result))
                {
                    scope.Complete();
                }
            }
            //string result = new ReportSvc().UpdBalanceSheet(CompanyId(), repDate,type,lstBanlance);

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
        /// <summary>
        /// 结账功能
        /// </summary>
        /// <param name="id"></param>
        /// <param name="repDate"></param>
        /// <param name="type"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public string UpdBalance(string id, string repDate, string type, string status)
        {
            ExceResult res = new ExceResult();
            string result = "";
            ReportSvc svc = new ReportSvc();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {
                if (status == "已结账")
                {

                    T_ReportDetails report = new T_ReportDetails();
                    DataTable dtBanlance = Common.Function.Common.GetTableByEntity<T_ReportDetails>(report);
                    List<T_BalanceSheetTemplate> lstBanlance = new List<T_BalanceSheetTemplate>();
                    lstBanlance = GetBanlance(id, repDate, type, "未结账","");

                    //执行创建资产负债表操作
                    string strReportID = Guid.NewGuid().ToString();
                    string strRepNo = "BS";
                    string strStart = string.Empty;
                    string strEnd = string.Empty;
                    Common.Function.Common.GetReportDateAndRepNo(repDate, type, ref strRepNo, ref strStart, ref strEnd);
                    if (string.IsNullOrEmpty(id))
                    {
                        result = new ReportSvc().CreateReportInfo(CompanyId(), repDate, type, strReportID, strRepNo + repDate);
                        id = strReportID;
                    }
                    else
                    {
                        result = svc.DeleteReportDetail(id);
                    }
                    
                    if (string.IsNullOrEmpty(result))
                    {
                        foreach (var item in lstBanlance)
                        {

                            addDataToTableByEntity(item, dtBanlance, id);
                        }

                        result = new ReportSvc().CreateReportDetailByBanlance(dtBanlance, typeof(T_ReportDetails).Name);
                    }
   
                    if (type == "month")
                    {
                        DateTime dtRep = DateTime.Parse(repDate + "/01");
                        //if (dtRep.Month == 6)
                        if (dtRep.Month == 12)
                        {
                            if (!svc.CreateHisBeginBalance(CompanyId(), dtRep.ToString("yyyy/MM/dd")))
                            {
                                result = "科目初始值更新失败！";
                            }
                        }

                    }
                }
                else
                {
                    if (type == "month")
                    {
                        DateTime dtRep = DateTime.Parse(repDate + "/01");
                        //if (dtRep.Month == 6)
                        if (dtRep.Month == 12)
                        {
                            if (!svc.UpdateHisToBeginBalance(CompanyId(), dtRep.ToString("yyyy/MM/dd")))
                            {
                                result = "科目初始值撤销失败！";
                            }
                        }
                    }
                }

                if (svc.settled(id, status))
                {
                    scope.Complete();
                }
                else
                {
                    result = "状态修改失败";
                }
                
            }
            //string result = new ReportSvc().UpdBalanceSheet(CompanyId(), repDate,type,lstBanlance);

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

        public void addDataToTableByEntity(T_BalanceSheetTemplate temp, DataTable dtNew, string id)
        {
            DataRow dr = dtNew.NewRow();
            dr["GUID"] = Guid.NewGuid().ToString();
            dr["rep_guid"] = id;
            dr["row_no"] = temp.asset_row_no;
            dr["Name"] = temp.asset_item_name;
            if (temp.asset_start_amount != null)
            {
                dr["beginning_amount"] = temp.asset_start_amount;
            }
            if (temp.asset_end_amount != null)
            {
                dr["ending_amount"] = temp.asset_end_amount;
            }

            dtNew.Rows.Add(dr);
            DataRow drSencod = dtNew.NewRow();
            drSencod["GUID"] = Guid.NewGuid().ToString();
            drSencod["rep_guid"] = id;
            drSencod["row_no"] = temp.debt_row_no;
            drSencod["Name"] = temp.debt_item_name;
            if (temp.debt_start_amount != null)
            {
                drSencod["beginning_amount"] = temp.debt_start_amount;
            }
            if (temp.debt_end_amount != null)
            {
                drSencod["ending_amount"] = temp.debt_end_amount;
            }

            dtNew.Rows.Add(drSencod);
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
                new ReportSvc().VaildBeginningBalance(Session["CurrentCompanyGuid"].ToString());
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
                new ReportSvc().GetBeginningBalance(Session["CurrentCompanyGuid"].ToString());
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
        private string GenBeginningBalanceJson(List<T_BeginningBalance> ds, string pid)
        {
            string strFmt = "{{\"ID\":\"{3}\",\"Name\":\"{0}\",\"Money\":{1},\"children\":{2}}},";

            StringBuilder strJson = new StringBuilder("[ ");
            foreach (T_BeginningBalance item in ds.Where(i => i._parentId.Equals(pid)).OrderBy(i => i.Acc_Code))
            {
                strJson.AppendFormat(strFmt, item.Acc_Name, item.Money, GenBeginningBalanceJson(ds, item.Acc_GUID), item.Acc_GUID);
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
            while (beginningBalance.Any(i => i.children.Count > 0))
            {
                currItem = beginningBalance.Where(i => i.children.Count > 0).FirstOrDefault();
                beginningBalance.AddRange(currItem.children);
                currItem.children = new List<T_BeginningBalance>();
            }
            bool result = false;
            if (true)
            {
                result = new ReportSvc().UpdBeginningBalance(beginningBalance, Session["CurrentCompanyGuid"].ToString());
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
