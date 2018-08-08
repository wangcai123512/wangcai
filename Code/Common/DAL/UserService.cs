using System.Collections.Generic;
using System.Data;
using FMS.Model;
namespace Common.DAL
{
    public class UserService
    {
        public List<T_User> GetUsers(string uName = null, string pwd = null)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetUsers";
            dh.AddPare("@LoginName", SqlDbType.NVarChar, 40, uName);
            dh.AddPare("@Pwd", SqlDbType.NVarChar, 40, pwd);
            return dh.Reader<T_User>();
        }

        public bool UpdUser(T_User u)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_UpdUser";
            dh.AddPare("@UserGuid", SqlDbType.NVarChar, 40, u.U_GUID);
            dh.AddPare("@UserName", SqlDbType.NVarChar, 40, u.UserName);
            dh.AddPare("@Password", SqlDbType.NVarChar, 40, u.Password);
            dh.AddPare("@CompanyGuid", SqlDbType.NVarChar, 40, u.C_GUID);
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
        public bool UpdUserState(string  name)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_UpdUserState";
            dh.AddPare("@LoginName", SqlDbType.NVarChar, 40, name);
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

        public bool DelUser(string id)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_DelUser";
            dh.AddPare("@UGuid", SqlDbType.NVarChar, ParameterDirection.Input, 50, id);
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

        public List<string> GetUserRole(string userID)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetUserRole";
            dh.AddPare("@UserGuid", SqlDbType.NVarChar, ParameterDirection.Input, 50, userID);
            return dh.Singel_Colume_Reader<string>("RoleGuid");
        }

        public List<T_Company> GetCompany(string id,string name)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetCompanyInfo";
            dh.AddPare("@id", SqlDbType.NVarChar, 50, id);
            dh.AddPare("@name", SqlDbType.NVarChar, 50, name);
            return dh.Reader<T_Company>();
        }

        public bool UpdUserRole(string userID, List<string> roles)
        {
            DBHelper dh = new DBHelper();
            dh.BeginTran();
            try
            {
                dh.strCmd = "SP_DelUserRole";
                dh.AddPare("@UserID", SqlDbType.NVarChar, ParameterDirection.Input, 100, userID);
                dh.NonQuery();
                dh.strCmd = "SP_AddUserRole";
                dh.AddPare("@RoleID", SqlDbType.NVarChar, ParameterDirection.Input, 50, string.Empty);
                foreach (string role in roles)
                {
                    dh.ChangeParaValue("@RoleID", role);
                    dh.NonQuery();
                }
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
