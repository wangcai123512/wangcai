using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using FMS.Model;

namespace FMS.DAL
{
    public class CompanySvc
    {
        /// <summary>
        /// 获取公司列表
        /// </summary>
        /// <param name="masterCompanyGuid">主公司标识</param>
        /// <returns></returns>
        public List<T_Company> GetCompanys(string masterCompanyGuid)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetCompany";
            dh.AddPare("@MasterCompanyGuid", SqlDbType.NVarChar, 40, masterCompanyGuid);
            return dh.Reader<T_Company>();
        }
        
        /// <summary>
        /// 获取用户列表信息
        /// </summary>
        /// <param name="C_GUID">公司标识</param>
        /// <returns></returns>
        public List<T_User> GetUserList(string C_GUID)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetUserList";
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            return dh.Reader<T_User>();
        }

        /// <summary>
        /// 公司信息
        /// </summary>
        /// <param name="form">公司对象</param>
        /// <returns></returns>
        public bool UpdCompany(T_Company form)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_UpdCompany";
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, form.C_GUID);
            dh.AddPare("@Name", SqlDbType.NVarChar, 50, form.Name);
            dh.AddPare("@Address", SqlDbType.NVarChar, 50, form.Address);
            dh.AddPare("@Contacter", SqlDbType.NVarChar, 50, form.Contacter);
            dh.AddPare("@ContactWay", SqlDbType.NVarChar, 50, form.ContactWay);
            dh.AddPare("@Type", SqlDbType.NVarChar, 50, form.Type);
            dh.AddPare("@AuditDate", SqlDbType.DateTime,0, form.AuditDate);
            dh.AddPare("@MasterCompanyGuid", SqlDbType.NVarChar, 50, form.MasterCompanyGuid);
            try
            {
                dh.NonQuery();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool UpdCompanyInformation(T_Company form)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_UpdCompanyInformation";
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, form.C_GUID);
            dh.AddPare("@ChineseFullName", SqlDbType.NVarChar, 50, form.ChineseFullName);
            dh.AddPare("@EnglishFullName", SqlDbType.NVarChar, 50, form.EnglishFullName);
            dh.AddPare("@Website", SqlDbType.NVarChar, 50, form.Website);
            dh.AddPare("@OrganizationCode", SqlDbType.NVarChar, 50, form.OrganizationCode);
            dh.AddPare("@IndustryInvolved", SqlDbType.NVarChar, 50, form.IndustryInvolved);
            dh.AddPare("@RegisteredAddress", SqlDbType.NVarChar,50, form.RegisteredAddress);
            dh.AddPare("@LOGO", SqlDbType.NVarChar, 400, form.LOGO);
            dh.AddPare("@Remark", SqlDbType.NVarChar, 50, form.Remark);
            dh.AddPare("@BusinessLicense", SqlDbType.NVarChar, 400, form.BusinessLicense);
            try
            {
                dh.NonQuery();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool UpdLOGO(string C_GUID, string path)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_UpdLOGO";
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@LOGO", SqlDbType.NVarChar, 400, path);
            try
            {
                dh.NonQuery();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool UpdBusinessLicense(string C_GUID, string path)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_UpdBusinessLicense";
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@BusinessLicense", SqlDbType.NVarChar, 400, path);
            try
            {
                dh.NonQuery();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 检查公司设置
        /// </summary>
        /// <param name="id">公司标识</param>
        /// <returns></returns>
        public bool checkCompanySetting(string id)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetCompanySetting";
            dh.AddPare("@ID", SqlDbType.NVarChar, 50, id);
            if (dh.Scalar() != null)
            {
                return true;
            }
            else
            {
                return false;
            }
                
        }

        /// <summary>
        /// 保存用户信息
        /// </summary>
        /// <param name="form">用户对象</param>
        /// <returns></returns>
        public bool UpdUserInfo(T_User form)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_UpdUserInfo";
            dh.AddPare("@U_GUID", SqlDbType.NVarChar, 50, form.U_GUID);
            dh.AddPare("@UserName", SqlDbType.NVarChar, 50, form.UserName);
            dh.AddPare("@LoginName", SqlDbType.NVarChar, 50, form.LoginName);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, form.C_GUID);
            dh.AddPare("@Password", SqlDbType.NVarChar, 50, form.Password);
            try
            {
                dh.NonQuery();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 删除用户信息
        /// </summary>
        /// <param name="id">用户标识</param>
        /// <returns></returns>
        public bool DelUser(string id)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_DelUser";
            dh.AddPare("@U_GUID", SqlDbType.NVarChar, 50, id);
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

        /// <summary>
        /// 保存公司设置信息
        /// </summary>
        /// <param name="form">公司设置对象</param>
        /// <returns></returns>
        public bool UpdSetting(T_CompanySetting form)
        {
            DBHelper dh = new DBHelper();
            dh.BeginTran();
            dh.strCmd = "SP_UpdCompanySetting";
            dh.AddPare("@R_GUID", SqlDbType.NVarChar, 50, form.R_GUID);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, form.C_GUID);
            dh.AddPare("@StandardCoin", SqlDbType.NVarChar, 50, form.StandardCoin);
            dh.AddPare("@ReportStartDate", SqlDbType.DateTime, 0, form.ReportStartDate);
            dh.AddPare("@AuditDate", SqlDbType.DateTime,0, form.AuditDate);
            try
            {
                dh.NonQuery();
            }
            catch (Exception ex)
            {
                dh.RollBackTran();
                return false;
            }
            dh.strCmd = "SP_DelCompanyCy";
            dh.CleanPara();
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, form.C_GUID);
            try
            {
                dh.NonQuery();
            }
            catch (Exception ex)
            {
                dh.RollBackTran();
                return false;
            }
            dh.strCmd = "SP_UpdCompanyCy";
            foreach (string item in form.CompanyCy)
            {
                dh.CleanPara();
                string R_GUID=Guid.NewGuid().ToString();
                dh.AddPare("@R_GUID", SqlDbType.NVarChar, 50,R_GUID);
                dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, form.C_GUID);
                dh.AddPare("@Code", SqlDbType.NVarChar, 50, item);
                try
                {
                    dh.NonQuery();
                }
                catch (Exception ex)
                {
                    dh.RollBackTran();
                    return false;
                }
            }
            dh.CommitTran();
            return true;
            
        }

        /// <summary>
        /// 获取公司设置
        /// </summary>
        /// <param name="id">公司设置标识</param>
        /// <returns></returns>
        public T_CompanySetting GetCompanySetting(string id)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetCompanySetting";
            dh.AddPare("@ID", SqlDbType.NVarChar, 50, id);
            return dh.Reader<T_CompanySetting>().FirstOrDefault() ?? new T_CompanySetting() { ReportStartDate=DateTime.Now};
        }

        /// <summary>
        /// 获取常用币制
        /// </summary>
        /// <param name="id">公司标识</param>
        /// <returns></returns>
        public List<R_CompanyCurrceny> GetCompanyCurrceny(string id)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetCompanyCurrceny";
            dh.AddPare("@ID", SqlDbType.NVarChar, 50, id);
            return dh.Reader<R_CompanyCurrceny>();
        }

        /// <summary>
        /// 获取公司信息
        /// </summary>
        /// <param name="id">公司标识</param>
        /// <param name="name">公司名称</param>
        /// <returns></returns>
        public List<T_Company> GetCompanyInfo(string id,string name)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetCompanyInfo";
            dh.AddPare("@id", SqlDbType.NVarChar, 50, id);
            dh.AddPare("@Name", SqlDbType.NVarChar, 50, name);
            return dh.Reader<T_Company>();
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="id">用户标识</param>
        /// <param name="name">用户登陆名</param>
        /// <returns></returns>
        public List<T_User> GetUserInfo(string id,string name)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd="SP_GetUserInfo";
            dh.AddPare("@U_GUID",SqlDbType.NVarChar,50,id);
            dh.AddPare("@Name", SqlDbType.NVarChar, 50, name);
            return dh.Reader<T_User>();
        }

        public List<T_Company> GetCompanyInformation(string cid)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetCompanyInformation";
            dh.AddPare("@C_ID", SqlDbType.NVarChar, 40, cid);
            return dh.Reader<T_Company>();
        }
    }
}
