using System;
using System.Collections.Generic;
using System.Data;
using FMS.Model;

namespace FMS.DAL
{
    public class CurrencySvc
    {
        /// <summary>
        /// 获取币值
        /// </summary>
        /// <returns></returns>
        public List<T_Currency> GetCurrency(bool? isCommon = null)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetCurrency";
            if (isCommon.HasValue)
            {
                dh.AddPare("@IsCommon", SqlDbType.Bit, 1, isCommon.Value?1:0);
            }
            return dh.Reader<T_Currency>();
        }

        /// <summary>
        /// 获取用户币值
        /// </summary>
        /// <param name="cid"></param>
        /// <returns></returns>
        public List<T_Currency> GetUserCurrency(string cid)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetUserCurrency";
            dh.AddPare("@C_ID", SqlDbType.NVarChar, 40, cid);
            return dh.Reader<T_Currency>();
        }

        public List<T_RateHistory> GetRateHistory(string C_GUID, int pageIndex, int pageSize, out int count, string dateBegin, string dateEnd)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetRateHistory";
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@PageIndex", SqlDbType.Int, 0, pageIndex=-1);
            dh.AddPare("@PageSize", SqlDbType.Int, 0, pageSize=1);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            if (!string.IsNullOrEmpty(dateBegin))
            {
                dh.AddPare("@DateBegin", SqlDbType.DateTime, 0, DateTime.Parse(dateBegin));
            }
            if (!string.IsNullOrEmpty(dateEnd))
            {
                dh.AddPare("@DateEnd", SqlDbType.DateTime, 0, DateTime.Parse(dateEnd));
            }
            List<T_RateHistory> result = new List<T_RateHistory>();
            result = dh.Reader<T_RateHistory>();
            count = dh.GetParaValue<int>("@Count");
            return result;
        }
        public List<T_RateHistory> GetRateHistory(string C_GUID, out int count)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetRateHistory";
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            List<T_RateHistory> result = new List<T_RateHistory>();
            result = dh.Reader<T_RateHistory>();
            count = dh.GetParaValue<int>("@Count");
            return result;
        }

        public bool UpdRateHistory(T_RateHistory rec)
        {
            DBHelper dh = new DBHelper();
            dh.BeginTran();
            try
            {
                dh.strCmd = "SP_UpdRateHistory";
                dh.AddPare("@GUID", SqlDbType.NVarChar, 40, rec.GUID);
                dh.AddPare("@Date", SqlDbType.DateTime, 0, rec.Date);
                dh.AddPare("@FAmount", SqlDbType.Decimal, 0, rec.FAmount);
                dh.AddPare("@FCurrency", SqlDbType.NVarChar, 40, rec.FCurrency);
                dh.AddPare("@TAmount", SqlDbType.Decimal, 0, rec.TAmount);
                dh.AddPare("@TCurrency", SqlDbType.NVarChar, 40, rec.TCurrency);
                dh.AddPare("@C_GUID", SqlDbType.NVarChar, 40, rec.C_GUID);
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
    }
}
