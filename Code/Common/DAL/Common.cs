using System.Collections.Generic;
using System.Data;
using Common.Models;
using System;
using FMS.Model;
using System.Linq;

namespace Common.DAL
{
    public class Common
    {
        public List<TreeMenuItem> GetTreeMenuItem()
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetTreeMenuItem";
            //dh.AddPare("@sysName", SqlDbType.NVarChar, ParameterDirection.Input, 10, sysName);
            return dh.Reader<TreeMenuItem>();
        }

        public bool UpdRegInfo(RegInfo regInfo)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_UpdRegInfo";
            dh.AddPare("@U_GUID", SqlDbType.NVarChar, 40, regInfo.User.U_GUID);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 40, regInfo.User.C_GUID);
            dh.AddPare("@UserName", SqlDbType.NVarChar, 40, regInfo.User.UserName);
            dh.AddPare("@Password", SqlDbType.NVarChar, 40, regInfo.User.Password);
            dh.AddPare("@LoginName", SqlDbType.NVarChar, 50, regInfo.User.LoginName);

            dh.AddPare("@MasterCompanyGuid", SqlDbType.NVarChar, 40, regInfo.Company.MasterCompanyGuid);
            dh.AddPare("@CompanyName", SqlDbType.NVarChar, 100, regInfo.Company.Name);
            dh.AddPare("@Address", SqlDbType.NVarChar, 50, regInfo.Company.Address);
            dh.AddPare("@Contacter", SqlDbType.NVarChar, 50, regInfo.Company.Contacter);
            dh.AddPare("@ContactWay", SqlDbType.NVarChar, 50, regInfo.Company.ContactWay);
            dh.AddPare("@Type", SqlDbType.NVarChar, 50, regInfo.Company.Type);
            dh.AddPare("@AuditDate", SqlDbType.DateTime, 0, regInfo.Company.AuditDate);
            dh.AddPare("@State", SqlDbType.Int, 0, regInfo.User.State);
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

        public DateTime GetEditThreshold(string cguid)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetReports";
            dh.AddPare("@C_ID", SqlDbType.NVarChar, 40, cguid);
            List<T_Report> result = dh.Reader<T_Report>();
            if (result.Any())
            {
                return result.Max(i => i.RepDate).AddMonths(1);
            }
            else
            {
                return DateTime.MinValue;
            }
        }
    }
}
