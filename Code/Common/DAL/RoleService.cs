using System.Collections.Generic;
using System.Data;
using Common.Models;

namespace Common.DAL
{
    public class RoleService
    {
        public List<Role> GetRoles(string sysName)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetRoles";
            dh.AddPare("@sysName", SqlDbType.NVarChar, ParameterDirection.Input, 10, sysName);
            return dh.Reader<Role>();
        }

        public bool UpdRole(Role r)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_UpdRole";
            dh.AddPare("@RoleGuid", SqlDbType.VarChar, ParameterDirection.Input, 50, r.Guid);
            dh.AddPare("@RoleName", SqlDbType.NVarChar, ParameterDirection.Input, 100, r.Name);
            dh.AddPare("@SysName", SqlDbType.NVarChar, ParameterDirection.Input, 50, r.SysName);
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

        public bool DelRole(string id)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_DelRole";
            dh.AddPare("@rGuid", SqlDbType.NVarChar, ParameterDirection.Input, 50, id);
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

        public bool UpdRolePermission(string roleID, List<string> funs)
        {
            DBHelper dh = new DBHelper();
            dh.BeginTran();
            try
            {
                dh.strCmd = "SP_DelRolePermission";
                dh.AddPare("@RoleID", SqlDbType.NVarChar, ParameterDirection.Input, 100, roleID);
                dh.NonQuery();
                dh.strCmd = "SP_AddRolePermission";
                dh.AddPare("@FunID", SqlDbType.NVarChar, ParameterDirection.Input, 50, string.Empty);
                foreach (string fun in funs)
                {
                    dh.ChangeParaValue("@FunID", fun);
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

        public List<string> GetRolePermission(string roleID)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetRolePermission";
            dh.AddPare("@RoleGuid", SqlDbType.VarChar, ParameterDirection.Input, 50, roleID);
            return dh.Singel_Colume_Reader<string>("FunGuid");
        }
    }
}
