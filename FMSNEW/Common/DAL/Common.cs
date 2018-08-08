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
            dh.AddPare("@NickName", SqlDbType.NVarChar, 40, regInfo.User.NickName);
            dh.AddPare("@Password", SqlDbType.NVarChar, 40, regInfo.User.Password);
            dh.AddPare("@LoginName", SqlDbType.NVarChar, 50, regInfo.User.LoginName);
            dh.AddPare("@TelName", SqlDbType.NVarChar, 50, regInfo.User.TelName);
            dh.AddPare("@Language", SqlDbType.NVarChar, 50, regInfo.User.Language);
            dh.AddPare("@MasterCompanyGuid", SqlDbType.NVarChar, 40, regInfo.Company.MasterCompanyGuid);
            dh.AddPare("@CompanyID", SqlDbType.NVarChar, 50, regInfo.Company.CompanyID);
            dh.AddPare("@Taxpayer", SqlDbType.NVarChar, 50, regInfo.Company.Taxpayer);
            dh.AddPare("@CompanyName", SqlDbType.NVarChar, 100, regInfo.Company.Name);
            dh.AddPare("@Address", SqlDbType.NVarChar, 50, regInfo.Company.Address);
            dh.AddPare("@Country", SqlDbType.NVarChar, 50, regInfo.Company.Country);
            dh.AddPare("@Province", SqlDbType.NVarChar, 50, regInfo.Company.Province);
            dh.AddPare("@City", SqlDbType.NVarChar, 50, regInfo.Company.City);
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
            dh.strCmd = "SP_GetThreshold";
            dh.AddPare("@C_ID", SqlDbType.NVarChar, 40, cguid);
            dh.AddPare("@MaxDate", SqlDbType.VarChar,ParameterDirection.Output,20,null);
            dh.NonQuery();
            string maxDate = dh.GetParaValue("@MaxDate").ToString();

            if (!string.IsNullOrEmpty(maxDate))
            {
                //已经按照DESC排序 所以取第一个即最大
                DateTime maxTemp;
                bool temp = DateTime.TryParse(maxDate, out maxTemp);
                if(temp)
                {
                    return maxTemp.AddMonths(1);
                }else
                {
                    return DateTime.MinValue;
                }
               
            }
            else
            {
                return DateTime.MinValue;
            }
        
        }

        
        //保存验证码
        public bool AddValidateCode(string loginName, string validCode)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_AddValidateCode";
            dh.AddPare("@LoginName", SqlDbType.NVarChar, 40, loginName);
            dh.AddPare("@ValidateCode", SqlDbType.NVarChar, 6, validCode);
            try
            {
                dh.NonQuery();
                return true;
            }
            catch (Exception e)
            {
                string x = e.Message;
                return false;
            }
        }
        //验证
        public bool CheckValidateCode(string loginName, string validCode)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetValidateCode";
            dh.AddPare("@LoginName", SqlDbType.NVarChar, 40, loginName);
            dh.AddPare("@ValidCode", SqlDbType.NVarChar, 6, validCode);
            if (dh.Scalar() != null)
            {
                ChangeValidateCode(loginName, validCode);
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool ChangeValidateCode(string loginName, string validCode)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_UpdValidateCode";
            dh.AddPare("@LoginName", SqlDbType.NVarChar, 40, loginName);
            dh.AddPare("@ValidCode", SqlDbType.NVarChar, 6, validCode);
            try
            {
                dh.NonQuery();
                return true;
            }
            catch (Exception e)
            {
                string x = e.Message;
                return false;
            }
        }

        public bool AddCompanyCurrency(string r_guid,string c_guid, string code)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_UpdCompanyCy";
            dh.AddPare("@R_GUID", SqlDbType.NVarChar, 50, r_guid);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, c_guid);
            dh.AddPare("@Code", SqlDbType.NVarChar, 5, code);
            try
            {
                dh.NonQuery();
                return true;
            }
            catch (Exception e)
            {
                string x = e.Message;
                return false;
            }
        }

        public bool AddUser(string u_guid,string c_guid, string loginName, string pwd, string state, string groupcode)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_AddNewUser";
            dh.AddPare("@UserGuid", SqlDbType.NVarChar, 50, u_guid);
            dh.AddPare("@CompanyGuid", SqlDbType.NVarChar, 50, c_guid);
            dh.AddPare("@LoginName", SqlDbType.NVarChar, 50, loginName);
            dh.AddPare("@Pwd", SqlDbType.NVarChar, 50, pwd);
            dh.AddPare("@State", SqlDbType.NVarChar, 50, state);
            dh.AddPare("@GP", SqlDbType.NVarChar, 10, groupcode);
            try
            {
                dh.NonQuery();
                return true;
            }
            catch (Exception e)
            {
                string x = e.Message;
                return false;
            }
        }

       

        public string AddUserCompany(string u_guid, string c_guid,string state,string gp,string date,string nickName)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_AddUserCompany";
            dh.AddPare("@U_GUID", SqlDbType.NVarChar, 50, u_guid);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, c_guid);
            dh.AddPare("@State", SqlDbType.NVarChar, 50, state);
            dh.AddPare("@GP", SqlDbType.NVarChar, 50, gp);
            dh.AddPare("@CreateDate",SqlDbType.NVarChar,50,date);
            dh.AddPare("@NickName", SqlDbType.NVarChar, 50, nickName);
            return (string)dh.Scalar();
        }
        public string AddUserCompanyLogin(string loginName, string c_guid, string state, string gp,string date)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_AddUserCompanyLogin";
            dh.AddPare("@LoginName", SqlDbType.NVarChar, 50, loginName);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, c_guid);
            dh.AddPare("@State", SqlDbType.NVarChar, 50, state);
            dh.AddPare("@GP", SqlDbType.NVarChar, 50, gp);
            dh.AddPare("@CreateDate", SqlDbType.NVarChar, 50, date);

            return (string)dh.Scalar();
        }

        public string GetGroupCode(string c_guid,string u_guid) 
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetGroupCode";
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, c_guid);
            dh.AddPare("@U_GUID", SqlDbType.NVarChar, 50, u_guid);
            return (string)dh.Scalar();
        }
        public string GetCurrencyCode(string c_guid)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetCurrencyCode";
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, c_guid);
            return (string)dh.Scalar();
        }
        public List<R_UserCompany> GetGroup(string c_guid, string u_guid) 
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetGroupCode";
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, c_guid);
            dh.AddPare("@U_GUID", SqlDbType.NVarChar, 50, u_guid);
            return dh.Reader<R_UserCompany>();
        }

        public string GetUserGroup(string c_guid,string u_guid)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetUserGroup";
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, c_guid);
            dh.AddPare("@U_GUID", SqlDbType.NVarChar, 50, u_guid);
            return (string)dh.Scalar();
        }

        public string GetNewUserInfo(string guid)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetNewUserInfo";
            dh.AddPare("@UC_GUID", SqlDbType.NVarChar, 50, guid);
            return (string)dh.Scalar();
        }

        public string GetNewUserPwd(string guid)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetNewUserPwd";
            dh.AddPare("@UC_GUID", SqlDbType.NVarChar, 50, guid);
            return (string)dh.Scalar();
        }

        public string ActivateUser(string guid, string pwd, string nickName,string date)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_ActivateUser";
            dh.AddPare("@UC_Guid", SqlDbType.NVarChar, 40, guid);
            dh.AddPare("@Password", SqlDbType.NVarChar, 40, pwd);
            dh.AddPare("@NickName", SqlDbType.NVarChar, 40, nickName);
            dh.AddPare("@CreateDate",SqlDbType.NVarChar,40, date);
            return (string)dh.Scalar();
        }
        public string ActivateLogin(string guid, string date)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_ActivateLogin";
            dh.AddPare("@UC_Guid", SqlDbType.NVarChar, 40, guid);
            dh.AddPare("@CreateDate", SqlDbType.NVarChar, 40, date);
            return (string)dh.Scalar();
        }
        /// <summary>
        /// 获取已关注和未关注所有列表列表
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// 2017/6/13   hdy
        /// </remarks> 
        public List<R_UserCompany> GetCompanycode(string c_guid, string u_guid)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetUserState";
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, c_guid);
            dh.AddPare("@U_GUID", SqlDbType.NVarChar, 50, u_guid);
            List<R_UserCompany> result = new List<R_UserCompany>();
            result = dh.Reader<R_UserCompany>();
            return result;
        }
    }
}
