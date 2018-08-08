using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using PermissionSys.Models;

namespace Common.UserIdentityVerify
{
    public class UserVerify
    {
        string originSystemID = Convert.ToString(ConfigurationManager.AppSettings["SubSystemID"]);
        string originSystemName = Convert.ToString(ConfigurationManager.AppSettings["SystemName"]);
        string rmUrl = string.Empty;
        string accID = Convert.ToString(ConfigurationManager.AppSettings["AccountID"]);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url">api host地址</param>
        public UserVerify(string url)
        {
            rmUrl = url;
        }

        #region 角色
        /// <summary>
        /// 新增角色
        /// </summary>
        /// <param name="roleName">角色名称</param>
        /// <returns></returns>
        public bool AddRole(string roleName)
        {
            T_LocalRole role = new T_LocalRole()
            {
                CreateDate = DateTime.Now,
                CreateUTCDate = DateTime.UtcNow,
                LocalRoleName = roleName,
                LocalRoleSymbolID = Guid.NewGuid().ToString(),
                SystemID = this.originSystemID,
                SystemName = this.originSystemName
            };
            return APIRequest<bool>("/api/ApiLocalRole/InsertLocalRole?accountid=" + accID, role, HttpMethod.Post.Method);
        }

        /// <summary>
        /// 更新角色
        /// </summary>
        /// <param name="roleID">角色标识</param>
        /// <param name="roleName">角色名称</param>
        /// <returns></returns>
        public bool UpdRole(string roleID, string roleName)
        {
            T_LocalRole role = new T_LocalRole()
            {
                CreateDate = DateTime.Now,
                CreateUTCDate = DateTime.UtcNow,
                LocalRoleName = roleName,
                LocalRoleSymbolID = roleID,
                SystemID = this.originSystemID,
                SystemName = this.originSystemName
            };
            return APIRequest<bool>("/api/ApiLocalRole/UpdateLocalRole?accountid=" + accID, role, HttpMethod.Post.Method);
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="roleID">角色标识</param>
        /// <returns></returns>
        public bool DelRole(string roleID)
        {
            string apiUrl = string.Format("/api/ApiLocalRole/DeleteLocalRoleAndRelation?LocalRoleSymbolID={0}&SystemID={1}&accountid={2}"
                , roleID, this.originSystemID, accID);
            return APIRequest<bool>(apiUrl, null, HttpMethod.Delete.Method);
        }

        /// <summary>
        /// 获取所有角色
        /// </summary>
        /// <returns></returns>
        public List<T_LocalRole> GetRoles()
        {
            string apiUrl = string.Format("/api/ApiLocalRole/GetLocalRoleBySystemID?SystemID={0}&accountid={1}", originSystemID, accID);
            return APIRequest<List<T_LocalRole>>(apiUrl, null, HttpMethod.Get.Method);
        }

        /// <summary>
        /// 更新角色权限
        /// </summary>
        /// <param name="roleID">角色标识</param>
        /// <param name="rfs">权限集合</param>
        /// <returns></returns>
        public bool UpdRolePermission(string roleID, List<R_LocalRole_Subfunction> rfs)
        {
            string apiUrl = string.Format("/api/ApiLocalRole/DeleteLocalRoleSubFunctionBySymbolIDSystemID?LocalRoleSymbolID={0}&SystemID={1}&accountid={2}"
                , roleID, this.originSystemID, accID);
            bool flag1 = APIRequest<bool>(apiUrl, null, HttpMethod.Delete.Method);
            bool falg2 = false;
            if (rfs.Count == 0)
            {
                falg2 = true;
            }
            else
            {
                falg2 = APIRequest<bool>("/api/ApiLocalRole/InsertMultiLocallRoleSubfunction?accountid=" + accID
                                , rfs, HttpMethod.Post.Method);
            }
            return flag1 & falg2;
        }

        /// <summary>
        /// 获取角色权限
        /// </summary>
        /// <param name="roleID">角色标识</param>
        /// <returns>权限集合</returns>
        public List<R_LocalRole_Subfunction> GetRolePermission(string roleID)
        {
            string apiUrl = string.Format("/api/ApiLocalRole/GetLocalRoleSubfunctionBySystemIDSymbolID?SystemID={0}&LocalRoleSymbolID={1}&accountid={2}"
                , originSystemID, roleID, accID);
            return APIRequest<List<R_LocalRole_Subfunction>>(apiUrl, null, HttpMethod.Get.Method);
        }
        #endregion

        #region 用户
        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="user">用户实体</param>
        /// <returns></returns>
        public bool AddUser(T_User user)
        {
            return APIRequest<bool>("/api/ApiUser/InsertUser?accountid=" + accID
                , user, HttpMethod.Post.Method);
        }

        /// <summary>
        /// 更新用户
        /// </summary>
        /// <param name="roleID">角色标识</param>
        /// <param name="roleName">角色名称</param>
        /// <returns></returns>
        public bool UpdUser(T_User user)
        {
            return APIRequest<bool>("/api/ApiUser/UpdateUser?accountid=" + accID
                , user, HttpMethod.Post.Method);
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="roleID">角色标识</param>
        /// <returns></returns>
        public bool DelUser(string userID)
        {
            string apiUrl = string.Format("/api/ApiUser/DeleteUserByUserGUID?UserGUID={0}&accountid={1}"
                , userID, accID);
            return APIRequest<bool>(apiUrl, null, HttpMethod.Delete.Method);
        }

        /// <summary>
        /// 获取所有本地用户
        /// </summary>
        /// <returns></returns>
        public List<SimpleUserModel> GetUsers()
        {
            string apiUrl = string.Format("/api/ApiUser/GetUsersBySystemID?systemID={0}&accountid={1}", originSystemID, accID);
            return APIRequest<List<SimpleUserModel>>(apiUrl, null, HttpMethod.Get.Method);
        }

        public List<T_LocalRole> GetUserRoles(string userID)
        {
            string apiUrl = string.Format("/api/ApiLocalRole/GetLocalRoleBySystemIDUserID?SystemID={0}&UserID={1}&OriginSystemID={0}&accountid={2}"
                , originSystemID, userID, accID);
            return APIRequest<List<T_LocalRole>>(apiUrl, null, HttpMethod.Get.Method);
        }

        public bool UpdUserRoles(string userID, List<R_LocalRole_User> lus)
        {
            string apiUrl = string.Format("/api/ApiLocalRole/DeleteUserLocalRoleByUserIDSystemID?UserID={0}&SystemID={1}&OriginSystemID={1}&accountid={2}"
                , userID, this.originSystemID, accID);
            bool flag1 = APIRequest<bool>(apiUrl, null, HttpMethod.Delete.Method);
            bool flag2 = false;
            if (lus.Count == 0)
            {
                flag2 = true;
            }
            else
            {
                apiUrl = string.Format("/api/ApiLocalRole/InsertMultiLocalRoleUser?accountid={0}", accID);
                flag2 = APIRequest<bool>(apiUrl, lus, HttpMethod.Post.Method);
            }
            return flag1 & flag2;
        }
        #endregion


        /// <summary>
        /// 插入日志
        /// </summary>
        /// <param name="log">日志对象</param>
        /// <param name="strIP">IP地址</param>
        /// <HttpMethod>POST</HttpMethod>
        /// <returns></returns>
        public bool CreateLog(T_EmployeeLoginOutLog log)
        {
            log.SystemID = originSystemID;
            return APIRequest<bool>("/api/ApiLog/InsertLoginOutLog?accountid=" + accID, log, HttpMethod.Post.Method);
        }

        public SingleSystemLoginModel GetLoginModel(string userID, string pwd)
        {
            string apiUrl = string.Format("/api/ApiUser/GetSingleSystemLoginModelMergeBySaas?userID={0}&password={1}&loginSystemID={2}&accountid={3}"
                , userID, pwd, originSystemID, accID);
            return APIRequest<SingleSystemLoginModel>(apiUrl, null, HttpMethod.Get.Method);
        }

        #region Menu
        //public List<string> GetMenu(string userID)
        //{
        //    string apiUrl = string.Format("/api/ApiRights/GetSubfunctionsBySystemIDangUserID?UserID={0}&SystemID={1}&OriginSystemID={1}&accountid={2}"
        //        , userID, originSystemID, accID);
        //    return APIRequest<List<string>>(apiUrl, null, HttpMethod.Get.Method);
        //}

        //public List<string> GetMenuWithSaas(string userID)
        //{
        //    string apiUrl = string.Format("/api/ApiRights/GetSubfunctionsBySystemIDangUserIDMergeBySaas?UserID={0}&SystemID={1}&OriginSystemID={1}&accountID={2}"
        //        , userID, originSystemID, accID);
        //    return APIRequest<List<string>>(apiUrl, null, HttpMethod.Get.Method);
        //}
        #endregion

        private T APIRequest<T>(string apiUrl, object postValue, string method)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(rmUrl);
            // Add an Accept header for JSON format.
            // 为JSON格式添加一个Accept报头
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = null;
            switch (method)
            {
                case "POST":
                    response = client.PostAsJsonAsync(apiUrl, postValue).Result;
                    break;
                case "DELETE":
                    response = client.DeleteAsync(apiUrl).Result;
                    break;
                case "GET":
                    response = client.GetAsync(apiUrl).Result;
                    break;
                default:
                    break;
            }
            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsAsync<T>().Result;
            }
            else
            {
                throw new Exception(response.StatusCode.ToString());
            }
        }
    }
}