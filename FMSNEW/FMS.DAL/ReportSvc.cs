using System.Collections.Generic;
using System.Data;
using System.Linq;
using FMS.Model;
using System;

namespace FMS.DAL
{
    public class ReportSvc
    {
        #region BalanceSheet
        /// <summary>
        /// 验证期初数
        /// </summary>
        /// <param name="c_id">公司标识</param>
        /// <returns></returns>
        public List<T_BeginningBalance> VaildBeginningBalance(string c_id)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_VaildBeginningBalance";
            dh.AddPare("@C_ID", SqlDbType.NVarChar, 40, c_id);
            return dh.Reader<T_BeginningBalance>();
        }
        /// <summary>
        /// 获取期初数
        /// </summary>
        /// <param name="c_id">公司标识</param>
        /// <returns></returns>
        public List<T_BeginningBalance> GetBeginningBalance(string c_id)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetBeginningBalance";
            dh.AddPare("@C_ID", SqlDbType.NVarChar, 40, c_id);
            return dh.Reader<T_BeginningBalance>();
        }

        /// <summary>
        /// 更新期初数
        /// </summary>
        /// <param name="bbs">期初数</param>
        /// <param name="c_id">公司标识</param>
        /// <returns></returns>
        public bool UpdBeginningBalance(List<T_BeginningBalance> bbs, string c_id)
        {
            DBHelper dh = new DBHelper();
            dh.BeginTran();
            try
            {
                dh.strCmd = "SP_DelBeginningBalance";
                dh.AddPare("@C_ID", SqlDbType.NVarChar, 40, c_id);
                dh.NonQuery();
                dh.strCmd = "SP_UpdBeginningBalance";
                dh.AddPare("@LA_ID", SqlDbType.NVarChar, 40, null);
                dh.AddPare("@Money", SqlDbType.Decimal, 18, null);
                foreach (T_BeginningBalance item in bbs)
                {
                    dh.ChangeParaValue("@LA_ID", item.Acc_GUID);
                    dh.ChangeParaValue("@Money", item.Money);
                    dh.NonQuery();
                }
                dh.CommitTran();
                return true;
            }
            catch (System.Exception)
            {
                dh.RollBackTran();
                return false;
            }
        }

        /// <summary>
        /// 生成资产负债预览表
        /// </summary>
        /// <param name="c_id">公司标识</param>
        /// <returns></returns>
        public T_Report<T_BalanceSheetTemplate> GenPerviewBalanceSheet(string c_id, string reportDate, string type)
        {
            T_Report<T_BalanceSheetTemplate> rep = new T_Report<T_BalanceSheetTemplate>();
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GenPerviewBalanceSheet";
            dh.AddPare("@C_ID", SqlDbType.NVarChar, 40, c_id);
            dh.AddPare("@report_date", SqlDbType.VarChar, 10, reportDate);
            dh.AddPare("@period_type", SqlDbType.VarChar, 50, type);
            dh.AddPare("@RepNo", SqlDbType.NVarChar, ParameterDirection.Output, 40, null);
            rep.Details = dh.Reader<T_BalanceSheetTemplate>();
            rep.RepNo = dh.GetParaValue<string>("@RepNo");
            return rep;
        }

        public DataTable GetBanlanceAmount(string c_id, string startDate, string endDate)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetBanlanceAmount";
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, c_id);
            dh.AddPare("@StartDate", SqlDbType.VarChar, 10, startDate);
            dh.AddPare("@EndDate", SqlDbType.VarChar, 10, endDate);
            DataTable dtBanlance = new DataTable();
            dtBanlance = dh.Query().Tables[0];

            return dtBanlance;
        }

        /// <summary>
        /// 更新资产负债表
        /// </summary>
        /// <param name="c_id">公司标识</param>
        /// <returns>执行结果，正常返回空，否则返回异常信息</returns>
        public string UpdBalanceSheet(string c_id, string reportDate, string type)
        {
            string[] b = reportDate.Split('/'); //Split()方法的参数为分隔符
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_UpdBalanceSheet";
            dh.AddPare("@C_ID", SqlDbType.VarChar, 40, c_id);
            dh.AddPare("@report_date", SqlDbType.VarChar, 10, reportDate);
            dh.AddPare("@period_type", SqlDbType.VarChar, 50, type);
            dh.AddPare("@report_year", SqlDbType.VarChar, 10, b[0]);
            dh.AddPare("@report_month", SqlDbType.VarChar, 10, b[1]);
            dh.AddPare("@error_msg", SqlDbType.VarChar, ParameterDirection.Output, 500, null);
            dh.NonQuery();
            return dh.GetParaValue<string>("@error_msg");
        }

        public string CreateReportInfo(string c_id, string reportDate, string type, string rep_guid, string RepNo)
        {
            string[] b = reportDate.Split('/'); //Split()方法的参数为分隔符
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_CreateReportInfo";
            dh.AddPare("@C_ID", SqlDbType.VarChar, 40, c_id);
            dh.AddPare("@rep_guid", SqlDbType.VarChar, 50, rep_guid);
            dh.AddPare("@RepNo", SqlDbType.VarChar, 40, RepNo);
            dh.AddPare("@report_date", SqlDbType.VarChar, 10, reportDate);
            dh.AddPare("@period_type", SqlDbType.VarChar, 50, type);
            dh.AddPare("@report_year", SqlDbType.VarChar, 10, b[0]);
            if (b.Length > 1)
            {
                dh.AddPare("@report_month", SqlDbType.VarChar, 10, b[1]);
            }
            dh.AddPare("@error_msg", SqlDbType.VarChar, ParameterDirection.Output, 500, null);
            //dh.AddPare("@status", SqlDbType.VarChar, 50, status);
            dh.NonQuery();
            return dh.GetParaValue<string>("@error_msg");
        }

        public string DeleteReportDetail(string rep_guid)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_DelReportDetail";
            dh.AddPare("@rep_guid", SqlDbType.VarChar, 50, rep_guid);
            dh.AddPare("@error_msg", SqlDbType.VarChar, ParameterDirection.Output, 500, null);
            dh.NonQuery();
            return dh.GetParaValue<string>("@error_msg");
        }

        //public string CreateReportDetailByBanlance(T_BalanceSheetTemplate balance, string rep_guid)
        //{
        //    DBHelper dh = new DBHelper();
        //    dh.strCmd = "SP_CreateReportDetails";
        //    dh.AddPare("@rep_guid", SqlDbType.VarChar, 50, rep_guid);
        //    dh.AddPare("@asset_row_no", SqlDbType.Int, 0, balance.asset_row_no);
        //    dh.AddPare("@asset_item_name", SqlDbType.VarChar, 500, balance.asset_item_name);
        //    dh.AddPare("@asset_start_amount", SqlDbType.Decimal, 0, balance.asset_start_amount);
        //    dh.AddPare("@asset_end_amount", SqlDbType.Decimal, 0, balance.asset_end_amount);
        //    dh.AddPare("@debt_row_no", SqlDbType.Int, 0, balance.debt_row_no);
        //    dh.AddPare("@debt_item_name", SqlDbType.VarChar, 500, balance.debt_item_name);
        //    dh.AddPare("@debt_start_amount", SqlDbType.Decimal, 0, balance.debt_start_amount);
        //    dh.AddPare("@debt_end_amount", SqlDbType.Decimal, 0, balance.debt_end_amount);
        //    dh.AddPare("@error_msg", SqlDbType.VarChar, ParameterDirection.Output, 500, null);
        //    dh.NonQuery();
        //    return dh.GetParaValue<string>("@error_msg");
        //}

        public string CreateReportDetailByBanlance(DataTable dt, string strTableName)
        {
            DBHelper dh = new DBHelper();
            return dh.BulkCopyByDataTable(dt, strTableName);
        }


        public decimal GetProfitYearAmount(string C_ID, string CurrentRepBeginDate, string CurrentRepEndDate)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetProfitbyYear";
            dh.AddPare("@C_ID", SqlDbType.VarChar, 50, C_ID);
            dh.AddPare("@CurrentRepBeginDate", SqlDbType.VarChar, 10, CurrentRepBeginDate);
            dh.AddPare("@CurrentRepEndDate", SqlDbType.VarChar, 10, CurrentRepEndDate);
            dh.AddDecimalPare("@ProfitTotalYear", SqlDbType.Decimal, ParameterDirection.Output,18, 2, null);
            dh.NonQuery();
            return dh.GetParaValue<decimal>("@ProfitTotalYear");
        }

        public bool CreateHisBeginBalance(string C_ID, string Date)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "Sp_CreateHisBeginBalance";
            dh.AddPare("@C_GUID", SqlDbType.VarChar, 50, C_ID);
            dh.AddPare("@Date", SqlDbType.VarChar, 10, Date);
            dh.NonQuery();
            return true;
        }

        public bool UpdateHisToBeginBalance(string C_ID, string Date)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "Sp_UpdateHisToBeginBalance";
            dh.AddPare("@C_GUID", SqlDbType.VarChar, 50, C_ID);
            dh.AddPare("@Date", SqlDbType.VarChar, 10, Date);
            dh.NonQuery();
            return true;
        }

        /// <summary>
        /// 获取资产负债表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T_Report<T_BalanceSheetTemplate> GetBalanceSheet(string id)
        {
            DBHelper dh = new DBHelper();
            T_Report<T_BalanceSheetTemplate> rep = new T_Report<T_BalanceSheetTemplate>();
            dh.strCmd = "[SP_GetReportDetails]";
            dh.AddPare("@RepID", SqlDbType.NVarChar, 40, id);
            rep.Details = dh.Reader<T_BalanceSheetTemplate>();

            return rep;
        }
        #endregion

        #region IncomeStatement
        /// <summary>
        /// 利润表
        /// </summary>
        /// <param name="c_id">公司标识</param>
        /// <param name="reportDate">报告日期</param>
        /// <param name="type">日期类型</param>
        /// <returns></returns>
        public T_Report<T_IncomeStatementTemplate> GenPerviewIncomeStatement(string c_id, string reportDate, string type)
        {
            T_Report<T_IncomeStatementTemplate> rep = new T_Report<T_IncomeStatementTemplate>();
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GenPerviewIncomeStatement";
            dh.AddPare("@C_ID", SqlDbType.NVarChar, 40, c_id);
            dh.AddPare("@report_date", SqlDbType.VarChar, 10, reportDate);
            dh.AddPare("@period_type", SqlDbType.VarChar, 50, type);
            dh.AddPare("@RepNo", SqlDbType.NVarChar, ParameterDirection.Output, 40, null);

            rep.Details = dh.Reader<T_IncomeStatementTemplate>();

            rep.RepNo = dh.GetParaValue<string>("@RepNo");
            return rep;
        }

        /// <summary>
        /// 获取利润表列表
        /// </summary>
        /// <param name="c_id">公司标识</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="count">纪录总数</param>
        /// <returns></returns>
        public List<T_Report<T_CashFlowItemTemplate>> GetIncomeStatements(string c_id)
        {
            // return GetReps(c_id, pageIndex, pageSize, "PL", out count);
            return null;
        }

        /// <summary>
        /// 获取利润表
        /// </summary>
        /// <param name="id">利润表标识</param>
        /// <returns></returns>
        /// <remarks>
        /// 2017/1/4    liujf   update
        /// </remarks>
        public T_Report<T_IncomeStatementTemplate> GetIncomeStatement(string id)
        {

            DBHelper dh = new DBHelper();
            T_Report<T_IncomeStatementTemplate> rep = new T_Report<T_IncomeStatementTemplate>();
            dh.strCmd = "[SP_GetReportDetails]";
            dh.AddPare("@RepID", SqlDbType.VarChar, 40, id);
            rep.Details = dh.Reader<T_IncomeStatementTemplate>();

            return rep;
        }

        /// <summary>
        /// 更新利润表
        /// </summary>
        /// <param name="c_id">公司标识</param>
        /// <returns></returns>
        /// <remarks>
        /// 2017/1/4    liujf   update
        /// </remarks>
        public string CreateIncomeStatement(string c_id, string reportDate, string type)
        {

            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_UpdIncomeStatement";
            dh.AddPare("@C_ID", SqlDbType.VarChar, 40, c_id);
            dh.AddPare("@period_type", SqlDbType.VarChar, 50, type);
            dh.AddPare("@report_date", SqlDbType.VarChar, 10, reportDate);
            if (type == "month" || type == "quarter")
            {
                string[] b = reportDate.Split('/'); //Split()方法的参数为分隔符
                dh.AddPare("@report_year", SqlDbType.VarChar, 10, b[0]);
                dh.AddPare("@report_month", SqlDbType.VarChar, 10, b[1]);
            }
            else
            {
                dh.AddPare("@report_year", SqlDbType.VarChar, 10, reportDate);
                dh.AddPare("@report_month", SqlDbType.VarChar, 10, null);
            }
            dh.AddPare("@error_msg", SqlDbType.VarChar, ParameterDirection.Output, 500, null);
            dh.NonQuery();
            return dh.GetParaValue<string>("@error_msg");
        }
        #endregion

        #region CashFlowStatement
        /// <summary>
        /// 获取现金流量表项目
        /// </summary>
        /// <returns></returns>
        public List<T_CashFlowItem> GetCashFlowItems()
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetCashFlowItems";
            return dh.Reader<T_CashFlowItem>();
        }

        /// <summary>
        /// 获取现金流量表列表
        /// </summary>
        /// <param name="c_id">公司标识</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="count">纪录总数</param>
        /// <returns></returns>
        public List<T_Report<T_CashFlowItemTemplate>> GetCashFlowStatements(string c_id)
        {
            //return GetReps(c_id, pageIndex, pageSize, "CF",out count);
            return null;
        }

        /// <summary>
        /// 获取现金流量预览表
        /// </summary>
        /// <param name="c_id">公司标识</param>
        /// <returns></returns>
        public T_Report<T_CashFlowItemTemplate> GenPerviewCashFlowStatement(string c_id, string reportDate, string type)
        {
            T_Report<T_CashFlowItemTemplate> rep = new T_Report<T_CashFlowItemTemplate>();
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GenPerviewCashFlowStatement";
            dh.AddPare("@C_ID", SqlDbType.NVarChar, 40, c_id);
            dh.AddPare("@report_date", SqlDbType.VarChar, 10, reportDate);
            dh.AddPare("@period_type", SqlDbType.VarChar, 50, type);
            dh.AddPare("@RepNo", SqlDbType.NVarChar, ParameterDirection.Output, 40, null);
            rep.Details = dh.Reader<T_CashFlowItemTemplate>();

            rep.RepNo = dh.GetParaValue<string>("@RepNo");
            return rep;
        }

        /// <summary>
        /// 获取现金流量表明细
        /// </summary>
        /// <param name="id">现金流量表标识</param>
        /// <returns></returns>
        public T_Report<T_CashFlowItemTemplate> GetCashFlowStatement(string id)
        {
            DBHelper dh = new DBHelper();
            T_Report<T_CashFlowItemTemplate> rep = new T_Report<T_CashFlowItemTemplate>();
            dh.strCmd = "[SP_GetReportDetails]";
            dh.AddPare("@RepID", SqlDbType.VarChar, 40, id);
            rep.Details = dh.Reader<T_CashFlowItemTemplate>();

            return rep;
        }

        /// <summary>
        /// 更新现金流量表
        /// </summary>
        /// <param name="c_id">公司标识</param>
        /// <returns></returns>
        public string UpdCashFlowStatement(string c_id, string reportDate, string type)
        {
            string[] b = reportDate.Split('/'); //Split()方法的参数为分隔符
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_UpdCashFlowStatement";
            dh.AddPare("@C_ID", SqlDbType.VarChar, 40, c_id);
            dh.AddPare("@report_date", SqlDbType.VarChar, 10, reportDate);
            dh.AddPare("@period_type", SqlDbType.VarChar, 50, type);
            dh.AddPare("@report_year", SqlDbType.VarChar, 10, b[0]);
            dh.AddPare("@report_month", SqlDbType.VarChar, 10, b[1]);
            dh.AddPare("@error_msg", SqlDbType.VarChar, ParameterDirection.Output, 500, null);
            dh.NonQuery();
            return dh.GetParaValue<string>("@error_msg");
        }
        #endregion



        /// <summary>
        /// 获取资产负债表列表
        /// </summary>
        /// <param name="companyId">公司ID</param>
        /// <returns>
        /// 2017/1/3   liujf   create
        /// </returns>
        public static List<T_Report<T_BalanceSheetTemplate>> GetBalanceSheetList(string companyId, string reportDate, string type)
        {
            DBHelper dh = new DBHelper();

            dh = GetReportData(companyId, "BS", reportDate, type);

            List<T_Report<T_BalanceSheetTemplate>> reps = dh.Reader<T_Report<T_BalanceSheetTemplate>>();

            return reps;
        }

        public static List<T_Report<T>> GetReportDateList<T>(string companyId, string reportDate, string type)
        {
            DBHelper dh = new DBHelper();

            dh = GetBeginingDate(companyId);
            T_BeginningBalance beginDate = dh.Reader<T_BeginningBalance>().FirstOrDefault();
            Type typeClass = typeof(T);
            string strContent = string.Empty;
            switch (typeClass.Name)
            {
                case "T_BalanceSheetTemplate":
                    strContent = "BS";
                    break;
                case "T_CashFlowItemTemplate":
                    strContent = "CF";
                    break;
                case "T_IncomeStatementTemplate":
                    strContent = "PL";
                    break;
                default:
                    strContent = "BS";
                    break;
            }
            dh = GetReportDateList(companyId, strContent, reportDate, type);

            List<T_Report<T>> reps = dh.Reader<T_Report<T>>();
            List<T_Report<T>> repsDetail = new List<T_Report<T>>();
            DateTime dtNow = DateTime.Now;
            DateTime dtBeingin = new DateTime();
            int length = 0;
            if (type == "month")
            {
                if (beginDate != null && beginDate.InitialDate != null)
                {
                    dtBeingin = DateTime.Parse(beginDate.InitialDate);

                    if (dtBeingin.Year == dtNow.Year)
                    {
                        length = dtNow.Month - dtBeingin.Month;
                    }
                    else
                    {
                        dtBeingin = DateTime.Parse(dtNow.ToString("yyyy") + "/01/01");
                        length = dtNow.Month - 1;
                    }
                }
                else
                {
                    length = dtNow.Month - 1;
                    dtBeingin = DateTime.Parse(dtNow.ToString("yyyy") + "/01/01");
                }
                for (int i = 0; i < length; i++)
                {
                    T_Report<T> reportDetail = new T_Report<T>();
                    reportDetail.rep_date = dtBeingin.AddMonths(i).ToString("yyyy/M");
                    reportDetail.rep_status = "未结账";
                    reportDetail.Type = strContent;
                    reportDetail.period_type = type;
                    repsDetail.Add(reportDetail);
                }
            }
            if (type == "quarter")
            {
                if (beginDate != null && beginDate.InitialDate != null)
                {
                    dtBeingin = DateTime.Parse(beginDate.InitialDate);
                    dtBeingin = dtBeingin.AddMonths(0 - (dtBeingin.Month - 1) % 3).AddDays(1 - dtBeingin.Day);
                    dtNow = dtNow.AddMonths(0 - (dtNow.Month - 1) % 3).AddDays(1 - dtNow.Day);
                    if (dtBeingin.Year == dtNow.Year)
                    {
                        length = (dtNow.Month - dtBeingin.Month) / 3;
                    }
                    else
                    {
                        dtBeingin = DateTime.Parse(dtNow.ToString("yyyy") + "/01/01");
                        length = (dtNow.Month / 3);
                    }
                }
                else
                {
                    length = dtNow.Month;
                    dtBeingin = DateTime.Parse(dtNow.ToString("yyyy") + "/01/01");
                }
                for (int i = 0; i < length; i++)
                {
                    T_Report<T> reportDetail = new T_Report<T>();
                    reportDetail.rep_date = dtBeingin.ToString("yyyy/")+(dtBeingin.AddMonths(i * 3).Month/3+1);
                    reportDetail.rep_status = "未结账";
                    reportDetail.Type = strContent;
                    reportDetail.period_type = type;
                    repsDetail.Add(reportDetail);
                }
            }
            if (type == "year")
            {
                if (beginDate != null && beginDate.InitialDate != null)
                {
                    dtBeingin = DateTime.Parse(beginDate.InitialDate);
                    if (dtBeingin.Year == dtNow.Year)
                    {
                        length = dtNow.Year - dtBeingin.Year;
                    }
                    else
                    {
                        length = dtNow.Year - dtBeingin.Year;
                        length = length > 2 ? 2 : length;
                    }
                }
                for (int i = length; i > 0; i--)
                {
                    T_Report<T> reportDetail = new T_Report<T>();
                    reportDetail.rep_date = dtNow.AddYears(-i).ToString("yyyy");
                    reportDetail.rep_status = "未结账";
                    reportDetail.Type = strContent;
                    reportDetail.period_type = type;
                    repsDetail.Add(reportDetail);
                }
            }
            foreach (var item in repsDetail)
            {
                var query = reps.Where(p => p.rep_date == item.rep_date);
                if (!query.Any())
                {
                    reps.Add(item);
                }
            }

            return reps.OrderByDescending(p => p.rep_date).ToList();
        }

        /// <summary>
        /// 获取现金流量表列表
        /// </summary>
        /// <param name="companyId">公司ID</param>
        /// <returns>
        /// 2017/1/3   liujf   create
        /// </returns>
        public static List<T_Report<T_CashFlowItemTemplate>> GetCashFlowList(string companyId, string reportDate, string type)
        {
            DBHelper dh = new DBHelper();

            dh = GetReportData(companyId, "CF", reportDate, type);

            List<T_Report<T_CashFlowItemTemplate>> reps = dh.Reader<T_Report<T_CashFlowItemTemplate>>();

            return reps;
        }

        /// <summary>
        /// 获取利润表列表
        /// </summary>
        /// <param name="companyId">公司ID</param>
        /// <returns>
        /// 2017/1/4   liujf   create
        /// </returns>
        public static List<T_Report<T_IncomeStatementTemplate>> GetIncomeStatementList(string companyId, string reportDate, string type)
        {
            DBHelper dh = new DBHelper();

            dh = GetReportData(companyId, "PL", reportDate, type);

            List<T_Report<T_IncomeStatementTemplate>> reps = dh.Reader<T_Report<T_IncomeStatementTemplate>>();

            return reps;
        }

        private static DBHelper GetReportData(string companyId, string type, string reportDate, string periodType)
        {

            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetReportList";
            dh.AddPare("@C_ID", SqlDbType.VarChar, 40, companyId);
            dh.AddPare("@Type", SqlDbType.VarChar, 5, type);
            if (reportDate == "")
            {
                DateTime now = DateTime.Now;
                dh.AddPare("@report_year", SqlDbType.VarChar, 10, now.Year);
            }
            dh.AddPare("@report_date", SqlDbType.VarChar, 10, reportDate);
            dh.AddPare("@period_type", SqlDbType.VarChar, 50, periodType);
            return dh;
        }

        private static DBHelper GetReportDateList(string companyId, string type, string reportDate, string periodType)
        {

            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetReportDateList";
            dh.AddPare("@C_ID", SqlDbType.VarChar, 40, companyId);
            dh.AddPare("@Type", SqlDbType.VarChar, 5, type);
            if (reportDate == "")
            {
                DateTime now = DateTime.Now;
                dh.AddPare("@report_year", SqlDbType.VarChar, 10, now.Year);
            }
            dh.AddPare("@report_date", SqlDbType.VarChar, 10, reportDate);
            dh.AddPare("@period_type", SqlDbType.VarChar, 50, periodType);
            return dh;
        }

        private static DBHelper GetBeginingDate(string companyId)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetBeginningDate";
            dh.AddPare("@C_ID", SqlDbType.VarChar, 40, companyId);
            return dh;
        }
        /// <summary>
        /// 更新利润表
        /// </summary>
        /// <param name="c_id">公司标识</param>
        /// <returns></returns>
        /// <remarks>
        /// 2017/3/29   sunp   update
        /// </remarks>
        public bool settled(string id, string status)
        {

            try
            {
                DBHelper dh = new DBHelper();
                dh.strCmd = "SP_UpdReport";
                dh.AddPare("@ID", SqlDbType.VarChar, 40, id);
                dh.AddPare("@REP_STATUS", SqlDbType.VarChar, 50, status);
                dh.NonQuery();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool settledTest(string CId, string id, string status, string RepDate, string period)
        {

            try
            {
                DBHelper dh = new DBHelper();
                dh.strCmd = "SP_UpdReportTest";
                dh.AddPare("@C_ID", SqlDbType.VarChar, 40, CId);
                dh.AddPare("@ID", SqlDbType.VarChar, 40, id);
                dh.AddPare("@REP_STATUS", SqlDbType.VarChar, 50, status);
                dh.AddPare("@report_date", SqlDbType.VarChar, 10, RepDate);
                if (period == "month" || period == "quarter")
                {
                    string[] b = RepDate.Split('/'); //Split()方法的参数为分隔符
                    dh.AddPare("@report_year", SqlDbType.VarChar, 10, b[0]);
                    dh.AddPare("@report_month", SqlDbType.VarChar, 10, b[1]);
                }
                else
                {
                    dh.AddPare("@report_year", SqlDbType.VarChar, 10, RepDate);
                    dh.AddPare("@report_month", SqlDbType.VarChar, 10, null);
                }
                dh.AddPare("@period_type", SqlDbType.VarChar, 10, period);
                dh.NonQuery();
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 更新利润表——new
        /// </summary>
        /// <param name="c_id">公司标识</param>
        /// <returns></returns>
        /// <remarks>
        /// 2018/06/07  zm
        /// </remarks>
        public bool settledprofit(string id, string status, string CId, string repDate, string period)
        {

            try
            {
                DBHelper dh = new DBHelper();
                dh.strCmd = "SP_UpdReportprofit";
                dh.AddPare("@ID", SqlDbType.VarChar, 40, id);
                dh.AddPare("@REP_STATUS", SqlDbType.VarChar, 50, status);
                dh.AddPare("@C_ID", SqlDbType.VarChar, 40, CId);
                dh.AddPare("@report_date", SqlDbType.VarChar, 10, repDate);
                dh.AddPare("@period_type", SqlDbType.VarChar, 50, period);
                dh.NonQuery();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 更新现金流量表
        /// </summary>
        /// <param name="c_id">公司标识</param>
        /// <returns></returns>
        /// <remarks>
        /// 2018/06/07  zm
        /// </remarks>
        public bool settledcash(string id, string status, string CId, string repDate, string period)
        {

            try
            {
                
                DBHelper dh = new DBHelper();
                dh.strCmd = "SP_UpdReportcash";
                dh.AddPare("@ID", SqlDbType.VarChar, 40, id);
                dh.AddPare("@REP_STATUS", SqlDbType.VarChar, 50, status);
                dh.AddPare("@C_ID", SqlDbType.VarChar, 40, CId);
                dh.AddPare("@report_date", SqlDbType.VarChar, 10, repDate);
                dh.AddPare("@period_type", SqlDbType.VarChar, 50, period);
                if (period == "month" || period == "quarter")
                {
                    string[] b = repDate.Split('/'); //Split()方法的参数为分隔符
                    dh.AddPare("@report_year", SqlDbType.VarChar, 10, b[0]);
                    dh.AddPare("@report_month", SqlDbType.VarChar, 10, b[1]);
                }
                else
                {
                    dh.AddPare("@report_year", SqlDbType.VarChar, 10, repDate);
                    dh.AddPare("@report_month", SqlDbType.VarChar, 10, null);
                }
                dh.NonQuery();
                return true;
            }
            catch
            {
                return false;
            }
        }
        
        /// <summary>
        /// 判断是否可以反结账结账
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// 2017/3/29   sunp   update
        /// </remarks>
        public List<T_Report<IncomeStatement>> isFinish(string repDate, string status, out int count, string type, string period, string CId)
        {
            List<T_Report<IncomeStatement>> rep = new List<T_Report<IncomeStatement>>();
            string[] date = repDate.Split('/');
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetIsReport";
            dh.AddPare("@report_year", SqlDbType.VarChar, 10, date[0]);
            dh.AddPare("@report_month", SqlDbType.VarChar, 10, date[1]);
            dh.AddPare("@type", SqlDbType.VarChar, 50, type);
            dh.AddPare("@period", SqlDbType.VarChar, 50, period);
            dh.AddPare("@C_GUID", SqlDbType.VarChar, 50, CId);
            dh.AddPare("@REP_STATUS", SqlDbType.VarChar, 50, status);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            rep = dh.Reader<T_Report<IncomeStatement>>();
            count = dh.GetParaValue<int>("@Count");
            return rep;
        }

        /// <summary>
        /// 判断流水账部分编辑修改，新增是否已结账
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// 2017/4/5   sunp   update
        /// </remarks>
        public List<T_Report<IncomeStatement>> isCheckout(string repDate, string status, out int count, string CId)
        {
            List<T_Report<IncomeStatement>> rep = new List<T_Report<IncomeStatement>>();
            string[] date = repDate.Split('/');
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetIsCheckout";
            dh.AddPare("@report_year", SqlDbType.VarChar, 10, date[0]);
            dh.AddPare("@report_month", SqlDbType.VarChar, 10, date[1]);
            dh.AddPare("@C_GUID", SqlDbType.VarChar, 50, CId);
            dh.AddPare("@REP_STATUS", SqlDbType.VarChar, 50, status);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            rep = dh.Reader<T_Report<IncomeStatement>>();
            count = dh.GetParaValue<int>("@Count");
            return rep;
        }

        /// <summary>
        /// 判断增值税是否结算
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// 2018/7/23   zm   update
        /// </remarks>
        public int isZenZhiCheckout(string repDate, out int count, string CId, string TaxName, string TaxationType)
        {
            string[] date = repDate.Split('/');
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetIsZenZhiCheckout";
            dh.AddPare("@report_year", SqlDbType.VarChar, 10, date[0]);
            dh.AddPare("@report_month", SqlDbType.VarChar, 10, date[1]);
            dh.AddPare("@C_GUID", SqlDbType.VarChar, 50, CId);
            dh.AddPare("@TaxName", SqlDbType.VarChar, 50, TaxName);
            //dh.AddPare("@c_TaxationType", SqlDbType.VarChar, 50, c_TaxationType);
            dh.AddPare("@TaxationType", SqlDbType.VarChar, 50, TaxationType);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            dh.NonQuery();
            count = dh.GetParaValue<int>("@Count");
            return count;
        }

        public T_Report<T_IncomeStatementTemplate> GetVoucher(int pageIndex, int pageSize, out int count, string id, string flag)
        {
            T_Report<T_IncomeStatementTemplate> result = new T_Report<T_IncomeStatementTemplate>();
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetIEVoucher";
            dh.AddPare("@IE_GUID", SqlDbType.NVarChar, 50, id);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            dh.AddPare("@FLAG", SqlDbType.NVarChar, 1, flag);
            result.Details = dh.Reader<T_IncomeStatementTemplate>();
            count = dh.GetParaValue<int>("@Count");
            return result;

        }
        public T_Report<T_IncomeStatementTemplate> GetRPVoucher(string c_id, string flag, string reportDate, string type)
        {
            T_Report<T_IncomeStatementTemplate> rep = new T_Report<T_IncomeStatementTemplate>();
            rep = new ReportSvc().GenPerviewIncomeStatement(c_id, reportDate, type);
            for (int i = 0; i < rep.Details.Count; i++)
            {
                if (rep.Details[i].row_no == 1 || rep.Details[i].row_no == 4 || rep.Details[i].row_no == 5 || rep.Details[i].row_no == 10 || rep.Details[i].row_no == 11 || rep.Details[i].row_no == 12 || rep.Details[i].row_no == 13 || rep.Details[i].row_no == 15 || rep.Details[i].row_no == 18)
                {
                    rep.Details[i].Summary = "借";
                    rep.Details[i].item_name = rep.Details[i].item_name;
                    rep.Details[i].amount_r = rep.Details[i].amount;
                }
                if (rep.Details[i].row_no == 2 || rep.Details[i].row_no == 3 || rep.Details[i].row_no == 6 || rep.Details[i].row_no == 7 || rep.Details[i].row_no == 8 || rep.Details[i].row_no == 9 || rep.Details[i].row_no == 14 || rep.Details[i].row_no == 16 || rep.Details[i].row_no == 17)
                {
                    rep.Details[i].Summary = "贷";
                    rep.Details[i].item_name = rep.Details[i].item_name;
                    rep.Details[i].amount_p = rep.Details[i].amount;
                }

            }
            return rep;
        }
        #region AccountSummary
        /// <summary>
        /// 获取科目汇总信息
        /// </summary>
        /// <param name="cid">公司标识</param>
        /// <returns></returns>
        public List<T_BeginningBalance> GetAccountSummary(string cid)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetAccountSummary";
            dh.AddPare("@C_ID", SqlDbType.NVarChar, 40, cid);
            return dh.Reader<T_BeginningBalance>();
        }

        /// <summary>
        /// 获取科目汇总明细
        /// </summary>
        /// <param name="cid">公司标识</param>
        /// <param name="accid">科目标识</param>
        /// <returns></returns>
        public List<T_RecPayRecord> GetAccountSummaryDetails(string cid, string accid)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetAccountSummaryDetails";
            dh.AddPare("@C_ID", SqlDbType.NVarChar, 40, cid);
            dh.AddPare("@AccID", SqlDbType.NVarChar, 40, accid);
            return dh.Reader<T_RecPayRecord>();
        }
        #endregion

        /// <summary>
        /// 查询快速关注模板
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// 2017/6/1   hdy
        /// </remarks>
        public List<T_QuickAttention> GetQuickAttentionModel(string c_guid) //
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetQuickAttentionModel";
            dh.AddPare("@c_guid", SqlDbType.NVarChar, 50, c_guid);
            List<T_QuickAttention> result = new List<T_QuickAttention>();
            result = dh.Reader<T_QuickAttention>();
            return result;
        }
        /// <summary>
        /// 新增所属公司快速关注列表
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// 2017/6/1   hdy
        /// </remarks>
        public bool UpdQuickAttention(T_QuickAttention rec)
        {
            DBHelper dh = new DBHelper();
            dh.BeginTran();
            try
            {
                dh.strCmd = "SP_UpdQuickAttention";
                dh.AddPare("@id", SqlDbType.NVarChar, 40, rec.id);
                dh.AddPare("@c_guid", SqlDbType.NVarChar, 40, rec.c_guid);
                dh.AddPare("@attention_type", SqlDbType.NVarChar, 40, rec.attention_type);
                if (!rec.statistical_time.Equals(DateTime.MinValue))
                {
                    dh.AddPare("@statistical_time", SqlDbType.DateTime, 0, rec.statistical_time);
                }
                dh.AddPare("@attention_type_amount", SqlDbType.Decimal, 0, rec.attention_type_amount);
                dh.AddPare("@statistical_currency", SqlDbType.NVarChar, 40, rec.statistical_currency);
                dh.AddPare("@attention_state", SqlDbType.NVarChar, 40, rec.attention_state);
                dh.AddPare("@push_account", SqlDbType.NVarChar, 40, rec.push_account);
                dh.AddPare("@push_frequency", SqlDbType.NVarChar, 40, rec.push_frequency);
                dh.AddPare("@company_name", SqlDbType.NVarChar, 40, rec.company_name);
                dh.NonQuery();
                dh.CleanPara();
                dh.CommitTran();
                return true;
            }
            catch
            {
                dh.RollBackTran();
                return false;
            }
        }
        /// <summary>
        /// 更新快速关注的净现金流
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// 2017/6/5   hdy
        /// </remarks>        
        public bool UpdYearFromNeCashFlows(string c_guid, string dateBegin, string dateEnd, string attention_type)
        {
            DBHelper db = new DBHelper();
            db.strCmd = "SP_UpdYearFromNeCashFlows";
            db.AddPare("@c_guid", SqlDbType.NVarChar, 40, c_guid);
            if (!string.IsNullOrEmpty(dateBegin))
            {
                db.AddPare("@DateBegin", SqlDbType.DateTime, 0, DateTime.Parse(dateBegin));
            }
            if (!string.IsNullOrEmpty(dateEnd))
            {
                db.AddPare("@DateEnd", SqlDbType.DateTime, 0, DateTime.Parse(dateEnd));
            }
            db.AddPare("@attention_type", SqlDbType.NVarChar, 40, attention_type);
            try
            {
                db.NonQuery();
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 更新快速关注的应收款
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// 2017/6/5   hdy
        /// </remarks>        
        public bool UpdQAAccounts(string c_guid)
        {
            DBHelper db = new DBHelper();
            db.strCmd = "SP_UpdQAAccounts";
            db.AddPare("@c_guid", SqlDbType.NVarChar, 40, c_guid);
            try
            {
                db.NonQuery();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 获取已关注的快速关注列表
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// 2017/6/6   hdy
        /// </remarks> 
        public List<T_QuickAttention> GetQuickAttentionList(string c_guid)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetQuickAttentionList";
            dh.AddPare("@c_guid", SqlDbType.NVarChar, 40, c_guid);
            List<T_QuickAttention> result = new List<T_QuickAttention>();
            result = dh.Reader<T_QuickAttention>();
            return result;
        }
        /// <summary>
        /// 获取已关注和未关注所有列表列表
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// 2017/6/13   hdy
        /// </remarks> 
        public List<T_QuickAttention> GetAllAttentionList(string c_guid)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetAllAttentionList";
            dh.AddPare("@c_guid", SqlDbType.NVarChar, 40, c_guid);
            List<T_QuickAttention> result = new List<T_QuickAttention>();
            result = dh.Reader<T_QuickAttention>();
            return result;
        }
        /// <summary>
        /// 更新快速关注状态为0
        /// </summary>
        /// <param name="rec"></param>
        /// <returns></returns>
        public bool UpdAttentionState(string c_guid)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_UpdAttentionState";
            dh.AddPare("@c_guid", SqlDbType.NVarChar, 40, c_guid);
            try
            {
                dh.NonQuery();
                return true;
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// 更新快速关注状态为0以及更改账号和频率
        /// </summary>
        /// <param name="rec"></param>
        /// <returns></returns>
        public bool UpdateQuickAList(List<T_QuickAttention> recordList, string c_guid)
        {

            DBHelper dh = new DBHelper();
            try
            {
                dh.strCmd = "SP_UpdAttentionState";
                dh.AddPare("@c_guid", SqlDbType.NVarChar, 40, c_guid);
                dh.NonQuery();
                dh.CleanPara();
                for (int i = 0; i < recordList.Count; i++)
                {
                    dh.strCmd = "SP_UpdateQuickH";
                    dh.AddPare("@c_guid", SqlDbType.NVarChar, 40, recordList[i].c_guid);
                    dh.AddPare("@attention_type", SqlDbType.NVarChar, 40, recordList[i].attention_type);
                    dh.AddPare("@push_account", SqlDbType.NVarChar, 40, recordList[i].push_account);
                    dh.AddPare("@push_frequency", SqlDbType.NVarChar, 40, recordList[i].push_frequency);
                    dh.NonQuery();
                    dh.CleanPara();
                }
                return true;
            }
            catch (System.Exception e)
            {

                return false;
            }

        }

        public List<T_QuickAttention> GetAutoCheckQuickList(string push_isselect, out int count, int pageIndex, int pageSize)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetAutoCheckQuickList";
            dh.AddPare("@push_isselect", SqlDbType.NVarChar, 40, push_isselect);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            dh.AddPare("@PageIndex", SqlDbType.Int, 0, pageIndex);
            dh.AddPare("@PageSize", SqlDbType.Int, 0, pageSize);
            List<T_QuickAttention> result = new List<T_QuickAttention>();
            result = dh.Reader<T_QuickAttention>();
            count = dh.GetParaValue<int>("@Count");
            return result;
        }
    }
}
