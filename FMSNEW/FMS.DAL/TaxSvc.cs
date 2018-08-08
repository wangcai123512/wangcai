using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using FMS.Model;

namespace FMS.DAL
{
    public class TaxSvc
    {
        public List<T_Tax> GetTax(string C_GUID,string TaxPayer)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetTax";
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@TaxPayer", SqlDbType.NVarChar, 50, TaxPayer);
            return dh.Reader<T_Tax>();
        }
        public List<T_Tax> GetTax(string C_GUID)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetTax";
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            return dh.Reader<T_Tax>();
        }
        public List<T_Tax> GetTaxOne(string C_GUID,string value)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetTaxOne";
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@T_GUID", SqlDbType.NVarChar, 50, value);
            return dh.Reader<T_Tax>();
        }
        public List<T_Tax> GetTax(string C_GUID, int pageIndex, int pageSize, out int count)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetTaxNew";
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@PageIndex", SqlDbType.Int, 0, pageIndex);
            dh.AddPare("@PageSize", SqlDbType.Int, 0, pageSize);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            List<T_Tax> result = new List<T_Tax>();
            result = dh.Reader<T_Tax>();
            count = dh.GetParaValue<int>("@Count");
            return result;
        }

        public bool UpdTax(T_Tax rec)
        {
            DBHelper dh = new DBHelper();
            dh.BeginTran();
            try
            {
                dh.strCmd = "SP_UpdTax";
                dh.AddPare("@T_GUID", SqlDbType.NVarChar, 40, rec.T_GUID);
                dh.AddPare("@Type", SqlDbType.NVarChar, 40, rec.Type);
                dh.AddPare("@Name", SqlDbType.NVarChar, 40, rec.Name);
                dh.AddPare("@Rate", SqlDbType.Decimal, 0, rec.Rate);
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

        public bool DelTax(string Tguid)
        {
            DBHelper dh = new DBHelper();
            dh.BeginTran();
            try
            {
                dh.strCmd = "SP_DelTax";
                dh.AddPare("@T_GUID", SqlDbType.NVarChar, 40, Tguid);
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

        public bool InsTax(T_Tax rec)
        {
            DBHelper dh = new DBHelper();
            dh.BeginTran();
            try
            {
                dh.strCmd = "SP_DelTax";
                dh.AddPare("@T_GUID", SqlDbType.NVarChar, 40, rec.T_GUID);
                dh.AddPare("@T_GUID", SqlDbType.NVarChar, 40, rec.T_GUID);
                dh.AddPare("@T_GUID", SqlDbType.NVarChar, 40, rec.T_GUID);
                dh.AddPare("@T_GUID", SqlDbType.NVarChar, 40, rec.T_GUID);
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
