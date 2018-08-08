using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using BaseController;
using FMS.DAL;
using FMS.Model;

namespace FMS.BLL
{
    public class CompanyController : BasicController
    {
        /// <summary>
        /// 选择公司页面
        /// </summary>
        /// <returns></returns>
        public ActionResult ChooseCompany()
        {
            return View();
        }

        /// <summary>
        /// 获取公司信息页面
        /// </summary>
        /// <param name="id">公司标识</param>
        public ActionResult CompanyInfo(string id)
        {
            if (string.IsNullOrEmpty(id))
            {

                T_Company model = new T_Company();
                return View("CompanyInfo", model);
            }
            else
            {
                ViewData["C_GUID"] = id;
                //ViewData["MasterCompanyGuid"] = "";
                List<T_Company> Info = new List<T_Company>();
                Info = new CompanySvc().GetCompanyInfo(id,"");
                if (Info.Count.Equals(0))
                {
                    T_Company model = new T_Company();
                    return View("CompanyInfo", model);
                }
                else
                {
                    return View("CompanyInfo", Info.FirstOrDefault());
                }

            }
        }

        ///<summary>
        ///检查公司设置
        ///<summary>
        ///<param name='id'>公司标识</param>
        public bool checkCompanySetting(string id)
        {
            bool result = false;
            result = new CompanySvc().checkCompanySetting(id);
            return result;
        }

        /// <summary>
        /// 获取用户列表信息页面
        /// </summary>
        public ActionResult GetUserList()
        {
            return Json(new CompanySvc().GetUserList(Session["MasterCompanyGuid"].ToString()));
        }

        /// <summary>
        /// 获取公司设置信息
        /// </summary>
        /// <param name="id">公司标识</param>
        public ActionResult CompanySetting(string id)
        {
            ViewData["C_GUID"] = id;
            T_CompanySetting Setting = new T_CompanySetting();
            Setting = new CompanySvc().GetCompanySetting(id);
            if (string.IsNullOrEmpty(Setting.R_GUID) == true)
            {
                Setting.Month = 1;
            }
            else
            {
                Setting.Month = Setting.GetReportStartDateMonth();
            }
            Setting.Year = Setting.GetReportStartDateYear();
            List<R_CompanyCurrceny> Currceny = new List<R_CompanyCurrceny>();
            Currceny = new CompanySvc().GetCompanyCurrceny(id);
            Setting.CompanyCy = Currceny.Select(i => i.Code).ToArray();
            return View("CompanySetting", Setting);
        }


        /// <summary>
        /// 获取公司列表信息
        /// </summary>
        public string GetCompanys()
        {
            return GenerateJson(new CompanySvc().GetCompanys(Session["MasterCompanyGuid"].ToString()), string.Empty);
        }

        /// <summary>
        /// 生成公司列表信息的JSON
        /// </summary>
        /// <param name="ds">数据源</param>
        /// <param name="pid">上级标识</param>
        /// <returns></returns>
        private string GenerateJson(List<T_Company> ds, string pid)
        {
            string strFormatter = "{{\"C_GUID\":\"{0}\",\"Name\":\"{1}\",\"children\":{2}}},";
            StringBuilder strJson = new StringBuilder("[ ");
            string strChildren = string.Empty;
            foreach (T_Company item in ds.Where(i => i.MasterCompanyGuid.Equals(pid)).OrderBy(i => i.CreateDate))
            {
                strChildren = GenerateJson(ds, item.C_GUID);
                strJson.AppendFormat(strFormatter, item.C_GUID, item.Name, strChildren);
            }
            strJson.Remove(strJson.Length - 1, 1);
            strJson.Append("]");
            return strJson.ToString();
        }

        /// <summary>
        /// 获取用户信息页面
        /// </summary>
        /// <param name="id">用户标识</param>
        public ActionResult UserInfo(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                ViewData["U_GUID"] = Guid.NewGuid().ToString();
                T_User model = new T_User();
                return View("UserInfo", model);
            }
            else
            {
                ViewData["U_GUID"] = id;
                List<T_User> info = new List<T_User>();
                info = new CompanySvc().GetUserInfo(id,"");
                return View("UserInfo", info.FirstOrDefault());
            }
        }

        /// <summary>
        /// 删除用户信息
        /// </summary>
        /// <param name="id">用户标识</param>
        public string DelUser(string id)
        {
            bool result = new CompanySvc().DelUser(id);
            string msg = string.Empty;
            if (result)
            {
                msg = General.Resource.Common.Success;
            }
            else
            {
                msg = General.Resource.Common.Failed;
            }
            return string.Format("{{\"Result\":{0},\"Msg\":\"{1}\"}}"
                , result.ToString().ToLower(), msg);
        }

        /// <summary>
        /// 保存公司信息
        /// </summary>
        /// <param name="form">公司对象</param>
        public string UpdCompany(T_Company form)
        {
            string msg = string.Empty;
            bool result = false;
            form.AuditDate = DateTime.Now.ToString();
            if (string.IsNullOrEmpty(form.C_GUID) == true)
            {
                form.C_GUID = Guid.NewGuid().ToString();
                form.MasterCompanyGuid = Session["MasterCompanyGuid"].ToString();
            }
            else
            {
                form.MasterCompanyGuid = "";
            }
            if (string.IsNullOrEmpty(form.MasterCompanyGuid) == true)
            {
                form.MasterCompanyGuid = Session["MasterCompanyGuid"].ToString();
            }
            List<T_Company> info = new List<T_Company>();
            info = new CompanySvc().GetCompanyInfo("",form.Name);
            if (info.Count.Equals(0))
            {
                result = new CompanySvc().UpdCompany(form);
                if (result)
                {
                    msg = General.Resource.Common.Success;
                }
                else
                {
                    msg = General.Resource.Common.Failed;
                }
                return string.Format("{{\"Result\":{0},\"Msg\":\"{1}\"}}"
                    , result.ToString().ToLower(), msg);
            }
            else
            {
                if (info.FirstOrDefault().C_GUID == form.C_GUID)
                {
                    result = new CompanySvc().UpdCompany(form);
                    if (result)
                    {
                        msg = General.Resource.Common.Success;
                    }
                    else
                    {
                        msg = General.Resource.Common.Failed;
                    }
                    return string.Format("{{\"Result\":{0},\"Msg\":\"{1}\"}}"
                        , result.ToString().ToLower(), msg);
                }
                result = false;
                msg = General.Resource.Common.CompanyName + General.Resource.Common.Exist;
                return string.Format("{{\"Result\":{0},\"Msg\":\"{1}\"}}",
          result.ToString().ToLower(), msg);
            }
           
        }

        ///<summary>
        ///保存公司设置信息
        ///<summary>
        ///<param name="form">公司设置对象</param>
        public string UpdSetting(T_CompanySetting form)
        {
            bool result = false;
            form.R_GUID = Guid.NewGuid().ToString();
            form.ReportStartDate = new DateTime(form.Year, form.Month, 1);
            form.AuditDate = DateTime.Now.ToString();
            result = new CompanySvc().UpdSetting(form);
            string msg = string.Empty;
            if (result)
            {
                msg = General.Resource.Common.Success;
            }
            else
            {
                msg = General.Resource.Common.Failed;
            }
            return string.Format("{{\"Result\":{0},\"Msg\":\"{1}\"}}"
                , result.ToString().ToLower(), msg);
        }

        /// <summary>
        /// 保存用户信息
        /// </summary>
        /// <param name="form">用户对象</param>
        public string UpdUserInfo(T_User form)
        {
            bool result = false;
            string msg = string.Empty;
            List<T_User> info = new List<T_User>();
            info = new CompanySvc().GetUserInfo("", form.LoginName);
            if (string.IsNullOrEmpty(form.C_GUID))
            {
                form.C_GUID = Session["MasterCompanyGuid"].ToString();
                if (info.Count.Equals(0))
                {
                    result = new CompanySvc().UpdUserInfo(form);
                    if (result)
                    {
                        msg = General.Resource.Common.Success;
                    }
                    else
                    {
                        msg = General.Resource.Common.Failed;
                    }

                }
                else
                {
                    msg = Common.Resource.RolePermission.LoginName + General.Resource.Common.Exist;
                }
                return string.Format("{{\"Result\":{0},\"Msg\":\"{1}\"}}"
                       , result.ToString().ToLower(), msg);
            }
            else
            {
                if (info.Count.Equals(0))
                {
                    result = new CompanySvc().UpdUserInfo(form);
                    if (result)
                    {
                        msg = General.Resource.Common.Success;
                    }
                    else
                    {
                        msg = General.Resource.Common.Failed;
                    }
                    return string.Format("{{\"Result\":{0},\"Msg\":\"{1}\"}}"
                        , result.ToString().ToLower(), msg);
                }
                else
                {
                    if (info.FirstOrDefault().C_GUID == form.C_GUID & info.FirstOrDefault().U_GUID==form.U_GUID)
                    {
                        result = new CompanySvc().UpdUserInfo(form);
                        if (result)
                        {
                            msg = General.Resource.Common.Success;
                        }
                        else
                        {
                            msg = General.Resource.Common.Failed;
                        }
                        return string.Format("{{\"Result\":{0},\"Msg\":\"{1}\"}}"
                            , result.ToString().ToLower(), msg);
                    }
                    result = false;
                    msg = Common.Resource.RolePermission.LoginName + General.Resource.Common.Exist;
                    return string.Format("{{\"Result\":{0},\"Msg\":\"{1}\"}}",
                  result.ToString().ToLower(), msg);
                }
            }
          
        }
    }
}
