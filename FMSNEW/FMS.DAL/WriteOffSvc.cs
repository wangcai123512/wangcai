using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FMS.Model;
using System.Data;

namespace FMS.DAL
{
    public class WriteOffSvc
    {
        /// <summary>
        /// 获取应收列表
        /// </summary>
        /// <param name="C_GUID">公司标识</param>
        /// <param name="page">页索引</param>
        /// <param name="rows">页大小</param>
        /// <param name="count">纪录总数</param>
        /// <returns></returns>
        public List<T_Receivables> GetReceivablesList(string C_GUID, int page, int rows, out int count)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetChooseReceivablesRecord";
            dh.AddPare("@PageIndex", SqlDbType.Int, 0, page);
            dh.AddPare("@PageSize", SqlDbType.Int, 0, rows);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 40, C_GUID);
            List<T_Receivables> result = new List<T_Receivables>();
            result = dh.Reader<T_Receivables>();
            count = dh.GetParaValue<int>("@Count");
            return result;
        }

        /// <summary>
        /// 获取应付列表
        /// </summary>
        /// <param name="C_GUID">公司标识</param>
        /// <param name="page">页索引</param>
        /// <param name="rows">页大小</param>
        /// <param name="count">纪录总数</param>
        /// <returns></returns>
        public List<T_Payables> GetPayablesList(string C_GUID, int page, int rows, out int count)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetChoosePayablesRecord";
            dh.AddPare("@PageIndex", SqlDbType.Int, 0, page);
            dh.AddPare("@PageSize", SqlDbType.Int, 0, rows);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 40, C_GUID);
            List<T_Payables> result = new List<T_Payables>();
            result = dh.Reader<T_Payables>();
            count = dh.GetParaValue<int>("@Count");
            return result;
        }

        /// <summary>
        /// 获取已销列表
        /// </summary>
        /// <param name="C_GUID">公司标识</param>
        /// <param name="page">页索引</param>
        /// <param name="rows">页大小</param>
        /// <param name="count">纪录总数</param>
        /// <param name="flag">收支标识</param>
        /// <returns></returns>
        public List<T_IEWriteOff> GetIEWriteOffList(string C_GUID, int page, int rows, out int count,string flag)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetIEWriteOffList";
            dh.AddPare("@PageIndex", SqlDbType.Int, 0, page);
            dh.AddPare("@PageSize", SqlDbType.Int, 0, rows);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 40, C_GUID);
            dh.AddPare("@Flag", SqlDbType.NVarChar, 4, flag);
            List<T_IEWriteOff> result = new List<T_IEWriteOff>();
            result = dh.Reader<T_IEWriteOff>();
            count = dh.GetParaValue<int>("@Count");
            return result;
        }

        /// <summary>
        /// 获取应收付纪录
        /// </summary>
        /// <param name="id">应收付纪录标识</param>
        /// <param name="C_GUID">公司标识</param>
        /// <param name="flag">收付标识</param>
        /// <returns></returns>
        public T_Receivables GetRecord(string id, string C_GUID,string flag)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetRecord";
            dh.AddPare("@ID", SqlDbType.NVarChar, 40, id);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@Flag", SqlDbType.NVarChar, 4, flag);
            return dh.Reader<T_Receivables>().FirstOrDefault();
        }

        /// <summary>
        /// 更新核销纪录
        /// </summary>
        /// <param name="rec">核销纪录对象</param>
        /// <returns></returns>
        public bool UpdWriteOffRecord(T_Receivables rec)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_UpdWriteOffRecord";
            dh.AddPare("@R_GUID", SqlDbType.NVarChar, 40, rec.R_GUID);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 40, rec.C_GUID);
            dh.AddPare("@IE_Flag", SqlDbType.NVarChar, 4, rec.IE_Flag);
            dh.AddPare("@DebitLedgerAccount", SqlDbType.NVarChar, 40, rec.DebitLedgerAccount);
            dh.AddPare("@DebitDetailsAccount", SqlDbType.NVarChar, 40, rec.DebitDetailsAccount);
            dh.AddPare("@CreditLedgerAccount", SqlDbType.NVarChar, 40, rec.CreditLedgerAccount);
            dh.AddPare("@CreditDetailsAccount", SqlDbType.NVarChar, 40, rec.CreditDetailsAccount);
            dh.AddPare("@Amount", SqlDbType.Decimal, 0, rec.Money);
            dh.AddPare("@Date", SqlDbType.DateTime, 0, rec.Date);
            dh.AddPare("@Remark", SqlDbType.NVarChar, 200, rec.Remark);
            dh.AddPare("@Creator", SqlDbType.NVarChar, 40,rec.Creator);
            dh.AddPare("@CreateDate", SqlDbType.DateTime, 0, DateTime.Now);
            dh.AddPare("@Currency", SqlDbType.NVarChar, 5, rec.Currency);
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
    }
}
