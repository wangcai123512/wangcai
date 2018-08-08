using System;
using System.Data;
using FMS.Model;
using System.Collections.Generic;

namespace FMS.DAL
{
    public class DeclareCustomerSvc
    {
        public bool UpdReceivablesDeclareCustomer(T_DeclareCustomer rec)
        {
            DBHelper dh = new DBHelper();
            dh.BeginTran();
            try
            {
                dh.strCmd = "SP_UpdReceivablesDeclareCustomer";
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

        public List<T_DeclareCustomer> GetReceivablesDeclareCustomerList(string C_GUID, int pageIndex, int pageSize, out int count,string invtype=null,string state=null)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetReceivablesDeclareCustomer";
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@PageIndex", SqlDbType.Int, 0, pageIndex);
            dh.AddPare("@PageSize", SqlDbType.Int, 0, pageSize);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            dh.AddPare("@InvType", SqlDbType.NVarChar, 50, invtype);
            dh.AddPare("@State", SqlDbType.NVarChar, 50, state);
            List<T_DeclareCustomer> result = new List<T_DeclareCustomer>();
            result = dh.Reader<T_DeclareCustomer>();
            count = dh.GetParaValue<int>("@Count");
            return result;
        }

        public List<T_DeclareCustomer> ChooseReceivablesDeclareCustomerList(string C_GUID, int pageIndex, int pageSize, out int count, string invtype=null)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_ChooseReceivablesDeclareCustomer";
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@PageIndex", SqlDbType.Int, 0, pageIndex);
            dh.AddPare("@PageSize", SqlDbType.Int, 0, pageSize);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            dh.AddPare("@InvType", SqlDbType.NVarChar, 40, invtype);
            List<T_DeclareCustomer> result = new List<T_DeclareCustomer>();
            result = dh.Reader<T_DeclareCustomer>();
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
            dh.strCmd = "SP_UpdDeclareCustomerState";
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
