using System.Collections.Generic;
using System.Data;
using FMS.Model;
using Common.Models;

namespace Common.DAL
{
    public class UserService
    {
        public List<T_User> GetUsers(string uName, string pwd = null, string c_guid = null)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetUsers";
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 40, c_guid);
            dh.AddPare("@LoginName", SqlDbType.NVarChar, 40, uName);
            dh.AddPare("@Pwd", SqlDbType.NVarChar, 40, pwd);
            return dh.Reader<T_User>();
        }

        public List<R_UserCompany> GetNickName(string u_guid, string c_guid)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetNickName";
            dh.AddPare("@U_GUID", SqlDbType.NVarChar, 40, u_guid);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 40, c_guid);
            return dh.Reader<R_UserCompany>();
        }

        public List<T_User> GetUserss(string u_guid,string c_guid)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetUserss";
            dh.AddPare("@U_GUID", SqlDbType.NVarChar, 40, u_guid);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 40, c_guid);
           
            return dh.Reader<T_User>();
        }

        public List<T_User> GetUserManage(string uName)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetUserManage";
            dh.AddPare("@LoginName", SqlDbType.NVarChar, 40, uName);
            return dh.Reader<T_User>();
        }

        public List<R_UserCompany> GetUser(string uName , string pwd , string uc_guid )
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetUser";
            dh.AddPare("@UC_GUID", SqlDbType.NVarChar, 40, uc_guid);
            dh.AddPare("@LoginName", SqlDbType.NVarChar, 40, uName);
            dh.AddPare("@Pwd", SqlDbType.NVarChar, 40, pwd);
            return dh.Reader<R_UserCompany>();
        }
        public bool UpdaUserInf(string LgName,string C_GUID,string name)
        {

            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_UpdaUserInf";
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 40, C_GUID);
            dh.AddPare("@LoginName", SqlDbType.NVarChar,40, LgName);
            dh.AddPare("@Name", SqlDbType.NVarChar, 40, name);
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
        public string GetUserByCguid(string loginName, string c_guid)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetUserByCguid";
            dh.AddPare("@LoginName", SqlDbType.NVarChar, 40, loginName);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 40, c_guid);
            return (string)dh.Scalar();
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
        public bool UpdUserIfom(T_User u)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_UpdUserInfom";
            dh.AddPare("@U_GUID", SqlDbType.NVarChar, 40, u.U_GUID);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 40, u.C_GUID);
            dh.AddPare("@Password", SqlDbType.NVarChar, 40, u.Password);
            dh.AddPare("@NickName", SqlDbType.NVarChar, 40, u.NickName);
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
        public bool UpdUserInfos(T_User u)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_UpdUserInfos";
            dh.AddPare("@U_GUID", SqlDbType.NVarChar, 40, u.U_GUID);
            dh.AddPare("@UserName", SqlDbType.NVarChar, 40, u.UserName);
            dh.AddPare("@LoginName", SqlDbType.NVarChar, 40, u.LoginName);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 40, u.C_GUID);
            dh.AddPare("@Password", SqlDbType.NVarChar, 40, u.Password);
            dh.AddPare("@State", SqlDbType.NVarChar, 40, u.State);
            dh.AddPare("@EnterC_GUID", SqlDbType.NVarChar, 40, u.EnterC_GUID);
            dh.AddPare("@NickName", SqlDbType.NVarChar, 40, u.NickName);
            dh.AddPare("@CreateDate", SqlDbType.NVarChar, 40, u.CreateDate);
            dh.AddPare("@GroupCode", SqlDbType.NVarChar, 40, u.GroupCode);
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

        public bool UpdUserInf(List<R_UserCompany> u)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_UpdUserInf";
            dh.AddPare("@U_GUID1", SqlDbType.NVarChar, 40, u[0].U_GUID);
            dh.AddPare("@U_GUID2", SqlDbType.NVarChar, 40, u[1].U_GUID);
            dh.AddPare("@C_GUID1", SqlDbType.NVarChar, 40, u[0].C_GUID);
            dh.AddPare("@C_GUID2", SqlDbType.NVarChar, 40, u[1].C_GUID);
            dh.AddPare("@GroupCode1", SqlDbType.NVarChar, 40, u[0].GroupCode);
            dh.AddPare("@GroupCode2", SqlDbType.NVarChar, 40, u[1].GroupCode);
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
        
        public bool DelUserAndModule(string id)
        {
            DBHelper dh = new DBHelper();
            dh.BeginTran();
            try{
                dh.strCmd = "SP_DelUserModule";
                dh.CleanPara();
                dh.AddPare("@UC_GUID", SqlDbType.NVarChar, ParameterDirection.Input, 50, id);

                dh.NonQuery();

                dh.strCmd = "SP_DelUser";
                dh.CleanPara();
                dh.AddPare("@UC_GUID", SqlDbType.NVarChar, ParameterDirection.Input, 50, id);
              
                dh.NonQuery();

                dh.CommitTran();
                return true;
            }
            catch(System.Exception e)
            {
                dh.RollBackTran();
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

        public List<T_Company> GetCompany(string id,string companyID)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetCompanyInfo";
            dh.AddPare("@id", SqlDbType.NVarChar, 50, id);
            dh.AddPare("@CompanyID", SqlDbType.NVarChar, 50, companyID);
            return dh.Reader<T_Company>();
        }

        public List<R_UserCompany> GetCompanyC(string id)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetCompany";
            dh.AddPare("@uc_guid", SqlDbType.NVarChar, 50, id);
            return dh.Reader<R_UserCompany>();
        }

        public List<R_UserCompany> GetUserC(string id)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetUserC";
            dh.AddPare("@uc_guid", SqlDbType.NVarChar, 50, id);
            return dh.Reader<R_UserCompany>();
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

        public List<T_Company> GetCompanyInformation(string id, string name)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetCompanyInfo";
            dh.AddPare("@id", SqlDbType.NVarChar, 50, id);
            dh.AddPare("@CompanyID", SqlDbType.NVarChar, 50, name);
            return dh.Reader<T_Company>();
        }

        public List<T_ModuleList> GetModuleList(string user_guid, string company_guid)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetModuleList";
            dh.AddPare("@U_GUID", SqlDbType.NVarChar, 40, user_guid);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 40, company_guid);
            return dh.Reader<T_ModuleList>();
        }

        public List<R_UserModule> GetUserModules(string guid,string user_guid, string company_guid)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetUserModuleList";
            dh.AddPare("@GUID", SqlDbType.NVarChar, 50,guid);
            dh.AddPare("@U_GUID", SqlDbType.NVarChar, 50, user_guid);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, company_guid);
            return dh.Reader<R_UserModule>();
        }

        public List<string> GetUserStateOneModules(string user_guid,string company_guid)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetUserModuleListStateOne";

            dh.AddPare("@U_GUID", SqlDbType.NVarChar, 40, user_guid);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 40, company_guid);
            List<R_UserModule> recs = dh.Reader<R_UserModule>();
            List<string> result = new List<string>();
            foreach (var rec in recs)
            {
                result.Add(rec.EnglishName);
            }
            return result;
        }

        public bool UpdUserModule(string u_guid,string c_guid,string guid,string strArr,string separetor)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_UpdUserModule";
            dh.AddPare("@U_GUID", SqlDbType.NVarChar, 50, u_guid);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, c_guid);
            dh.AddPare("@GUID", SqlDbType.NVarChar, 50, guid);
            dh.AddPare("@Str", SqlDbType.NVarChar, 4000, strArr);
            dh.AddPare("@Separator", SqlDbType.NVarChar, 50, separetor);
            try
            {
                dh.NonQuery();
                return true;
            }
            catch (System.Exception e)
            {
                return false;
            }
        }
        public bool UpdCompanySetting(string id, string coin) 
        {
            
            DBHelper dh = new DBHelper();
            dh.BeginTran();
             try
            {
                dh.strCmd = "SP_UpdCompanySettings";
                dh.CleanPara();
                dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, id);
                dh.AddPare("@StandardCoin", SqlDbType.NVarChar, 50, coin);
                dh.NonQuery();
             
                dh.strCmd = "SP_UpdCompanyCurrency";
                dh.CleanPara();
                dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, id);
                dh.AddPare("@Code", SqlDbType.NVarChar, 50, coin);
                    dh.NonQuery();

                    dh.CommitTran();
                    return true;
            }
            catch(System.Exception e)
            {
                dh.RollBackTran();
                return false;
            }
        }
       
        /// <summary>
        /// 增加公司设置
        /// </summary>
        /// <param name="id">公司设置标识</param>
        /// <returns></returns>
        public bool AddCompanySetting(string id, string coin = null)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_AddCompanySetting";
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, id);
            dh.AddPare("@StandardCoin", SqlDbType.NVarChar, 50, coin);
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