using System;
using System.Data;
using FMS.Model;
using System.Collections.Generic;

namespace FMS.DAL
{
    public class DeclareCostSpendingSvc
    {
        /// <summary>
        /// 新增操作
        /// </summary>
        /// <param name="rec"></param>
        /// <returns></returns>
        public bool UpdPaymentDeclareCostSpending(T_DeclareCostSpending rec)
        {
            DBHelper dh = new DBHelper();
            dh.BeginTran();
            try
            {
                dh.strCmd = "SP_UpdPaymentDeclareCostSpending";
                dh.AddPare("@GUID", SqlDbType.NVarChar, 40, rec.GUID);
                dh.AddPare("@InvType", SqlDbType.NVarChar, 40, rec.InvType);
                dh.AddPare("@RPer", SqlDbType.NVarChar, 40, rec.RPer);
                if (!rec.Date.Equals(DateTime.MinValue))
                {
                    dh.AddPare("@Date", SqlDbType.DateTime, 0, rec.Date);
                }
                dh.AddPare("@Amount", SqlDbType.Decimal, 0, rec.Amount);
                dh.AddPare("@Remark", SqlDbType.NVarChar, 200, rec.Remark);
                dh.AddPare("@Currency", SqlDbType.NVarChar, 20, rec.Currency);
                dh.AddPare("@State", SqlDbType.NVarChar, 40, rec.State);
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

        public List<T_DeclareCostSpending> GetPaymentDeclareCostSpendingList(string C_GUID, int pageIndex, int pageSize, out int count,string state,string  invtype)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetPaymentDeclareCostSpending";
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@PageIndex", SqlDbType.Int, 0, pageIndex);
            dh.AddPare("@PageSize", SqlDbType.Int, 0, pageSize);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            dh.AddPare("@State", SqlDbType.NVarChar, 50, state);
            dh.AddPare("@InvType", SqlDbType.NVarChar, 50, invtype);
            List<T_DeclareCostSpending> result = new List<T_DeclareCostSpending>();
            result = dh.Reader<T_DeclareCostSpending>();
            count = dh.GetParaValue<int>("@Count");
            return result;
        }

        public List<T_DeclareCostSpending> GetList(int pageIndex, int pageSize, string C_GUID, string dateBegin, string dateEnd, string paymentGrp, string state ,out int count)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetPaymentDeclareCostSpendingAll";
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@PageIndex", SqlDbType.Int, 0, pageIndex);
            dh.AddPare("@PageSize", SqlDbType.Int, 0, pageSize);
            if (!string.IsNullOrEmpty(dateBegin))
            {
                dh.AddPare("@dateBegin", SqlDbType.DateTime, 0, DateTime.Parse(dateBegin));
            }
            if (!string.IsNullOrEmpty(dateEnd))
            {
                dh.AddPare("@dateEnd", SqlDbType.DateTime, 0, DateTime.Parse(dateEnd));
            }
            dh.AddPare("@PaymentGrp", SqlDbType.NVarChar, 50, paymentGrp);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            dh.AddPare("@State", SqlDbType.NVarChar, 50, state);
            List<T_DeclareCostSpending> result = new List<T_DeclareCostSpending>();
            result = dh.Reader<T_DeclareCostSpending>();
            count = dh.GetParaValue<int>("@Count");
            return result;
        }

        public List<T_DeclareCostSpending> ChoosePaymentDeclare(string C_GUID, int pageIndex, int pageSize, out int count, string invtype)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_ChoosePaymentDeclare";
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@PageIndex", SqlDbType.Int, 0, pageIndex);
            dh.AddPare("@PageSize", SqlDbType.Int, 0, pageSize);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            dh.AddPare("@InvType", SqlDbType.NVarChar, 40, invtype);
            List<T_DeclareCostSpending> result = new List<T_DeclareCostSpending>();
            result = dh.Reader<T_DeclareCostSpending>();
            count = dh.GetParaValue<int>("@Count");
            return result;
        }

        /// <summary>
        /// 更改状态
        /// </summary>
        /// <param name="id">纪录标识</param>
        /// <returns></returns>
        public bool UpdState(string id, string state)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_UpdDeclareCostSpendingState";
            dh.AddPare("@ID", SqlDbType.NVarChar, 40, id);
            dh.AddPare("@State", SqlDbType.NVarChar, 40, state);
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
