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
        public BalanceSheet GenPerviewBalanceSheet(string c_id)
        {
            BalanceSheet rep = new BalanceSheet();
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GenPerviewBalanceSheet";
            dh.AddPare("@C_ID", SqlDbType.NVarChar, 40, c_id);
            dh.AddPare("@Year", SqlDbType.Int, ParameterDirection.Output, 0, null);
            dh.AddPare("@Month", SqlDbType.Int, ParameterDirection.Output, 0, null);
            dh.AddPare("@RepNo", SqlDbType.NVarChar, ParameterDirection.Output, 40, null);
            rep.Details = dh.Reader<T_ReportDetails>();
            rep.Year = dh.GetParaValue<int>("@Year");
            rep.Month = dh.GetParaValue<int>("@Month");
            rep.RepNo = dh.GetParaValue<string>("@RepNo");
            return rep;
        }

        /// <summary>
        /// 更新资产负债表
        /// </summary>
        /// <param name="c_id">公司标识</param>
        /// <returns></returns>
        public bool UpdBalanceSheet(string c_id)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_UpdBalanceSheet";
            dh.AddPare("@C_ID", SqlDbType.NVarChar, 40, c_id);
            try
            {
                dh.NonQuery();
                return true;
            }
            catch (System.Exception)
            {
                return false;
            }

        }

        /// <summary>
        /// 获取资产负债表列表
        /// </summary>
        /// <param name="c_id">公司标识</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="count">纪录总数</param>
        /// <returns></returns>
        public List<T_Report> GetBalanceSheets(string c_id, int pageIndex, int pageSize, out int count)
        {
            return GetReps(c_id, pageIndex, pageSize, "BS", out count);
        }

        /// <summary>
        /// 获取资产负债表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BalanceSheet GetBalanceSheet(string id)
        {
            DBHelper dh = new DBHelper();
            BalanceSheet rep = new BalanceSheet();
            dh.strCmd = "SP_GetReport";
            dh.AddPare("@RepID", SqlDbType.NVarChar, 40, id);
            rep = dh.Reader<BalanceSheet>().FirstOrDefault();
            dh.strCmd = "SP_GetReportDetails";
            rep.Details = dh.Reader<T_ReportDetails>();
            return rep;
        } 
        #endregion

        #region IncomeStatement
        /// <summary>
        /// 利润表
        /// </summary>
        /// <param name="c_id">公司标识</param>
        /// <returns></returns>
        public IncomeStatement GenPerviewIncomeStatement(string c_id)
        {
            IncomeStatement rep = new IncomeStatement();
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GenPerviewIncomeStatements";
            dh.AddPare("@C_ID", SqlDbType.NVarChar, 40, c_id);
            dh.AddPare("@Year", SqlDbType.Int, ParameterDirection.Output, 0, null);
            dh.AddPare("@Month", SqlDbType.Int, ParameterDirection.Output, 0, null);
            dh.AddPare("@RepNo", SqlDbType.NVarChar, ParameterDirection.Output, 40, null);
            rep.Details = dh.Reader<T_ReportDetails>();
            rep.Year = dh.GetParaValue<int>("@Year");
            rep.Month = dh.GetParaValue<int>("@Month");
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
        public List<T_Report> GetIncomeStatements(string c_id, int pageIndex, int pageSize, out int count)
        {
            return GetReps(c_id, pageIndex, pageSize, "PL", out count);
        }

        /// <summary>
        /// 获取利润表
        /// </summary>
        /// <param name="id">利润表标识</param>
        /// <returns></returns>
        public IncomeStatement GetIncomeStatement(string id)
        {
            DBHelper dh = new DBHelper();
            IncomeStatement rep = new IncomeStatement();
            dh.strCmd = "SP_GetReport";
            dh.AddPare("@RepID", SqlDbType.NVarChar, 40, id);
            rep = dh.Reader<IncomeStatement>().FirstOrDefault();
            dh.strCmd = "SP_GetReportDetails";
            rep.Details = dh.Reader<T_ReportDetails>();
            return rep;
        }

        /// <summary>
        /// 更新利润表
        /// </summary>
        /// <param name="c_id">公司标识</param>
        /// <returns></returns>
        public bool UpdIncomeStatement(string c_id)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_UpdIncomeStatement";
            dh.AddPare("@C_ID", SqlDbType.NVarChar, 40, c_id);
            try
            {
                dh.NonQuery();
                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
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
        public List<T_Report> GetCashFlowStatements(string c_id, int pageIndex, int pageSize, out int count)
        {
            return GetReps(c_id, pageIndex, pageSize, "CF",out count);
        }

        /// <summary>
        /// 获取现金流量预览表
        /// </summary>
        /// <param name="c_id">公司标识</param>
        /// <returns></returns>
        public CashFlowStatement GenPerviewCashFlowStatement(string c_id)
        {
            CashFlowStatement rep = new CashFlowStatement();
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GenPerviewCashFlowStatement";
            dh.AddPare("@C_ID", SqlDbType.NVarChar, 40, c_id);
            dh.AddPare("@Year", SqlDbType.Int, ParameterDirection.Output, 0, null);
            dh.AddPare("@Month", SqlDbType.Int, ParameterDirection.Output, 0, null);
            dh.AddPare("@RepNo", SqlDbType.NVarChar, ParameterDirection.Output, 40, null);
            rep.Details = dh.Reader<T_ReportDetails>();
            rep.Year = dh.GetParaValue<int>("@Year");
            rep.Month = dh.GetParaValue<int>("@Month");
            rep.RepNo = dh.GetParaValue<string>("@RepNo");
            return rep;
        }

        /// <summary>
        /// 获取现金流量表
        /// </summary>
        /// <param name="id">现金流量表标识</param>
        /// <returns></returns>
        public CashFlowStatement GetCashFlowStatement(string id)
        {
            DBHelper dh = new DBHelper();
            CashFlowStatement rep = new CashFlowStatement();
            dh.strCmd = "SP_GetReport";
            dh.AddPare("@RepID", SqlDbType.NVarChar, 40, id);
            rep = dh.Reader<CashFlowStatement>().FirstOrDefault();
            dh.strCmd = "SP_GetReportDetails";
            rep.Details = dh.Reader<T_ReportDetails>();
            return rep;
        }

        /// <summary>
        /// 更新现金流量表
        /// </summary>
        /// <param name="c_id">公司标识</param>
        /// <returns></returns>
        public bool UpdCashFlowStatement(string c_id)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_UpdCashFlowStatement";
            dh.AddPare("@C_ID", SqlDbType.NVarChar, 40, c_id);
            try
            {
                dh.NonQuery();
                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
        }
        #endregion

        #region Common
        /// <summary>
        /// 获取报表
        /// </summary>
        /// <param name="c_id">公司标识</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="type">报表类型</param>
        /// <param name="count">纪录总数</param>
        /// <returns></returns>
        public List<T_Report> GetReps(string c_id, int pageIndex, int pageSize,string type, out int count)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetReports";
            dh.AddPare("@C_ID", SqlDbType.NVarChar, 40, c_id);
            dh.AddPare("@Type", SqlDbType.NVarChar, 5, type);
            dh.AddPare("@PageSize", SqlDbType.Int, 0, pageSize);
            dh.AddPare("@PageIndex", SqlDbType.Int, 0, pageIndex);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            List<T_Report> reps = dh.Reader<T_Report>();
            count = dh.GetParaValue<int>("@Count");
            return reps;
        }
        #endregion

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
            dh.AddPare("@AccID",SqlDbType.NVarChar,40,accid);
            return dh.Reader<T_RecPayRecord>();
        }
        #endregion
    }
}
