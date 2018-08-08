using System.Web.Mvc;
using BaseController;

namespace Common.Controllers
{
    public class LocalUserManagementController : UserController
    {
        public LocalUserManagementController() : base("Local_User_Management") { }

        #region User by UAM
        //public ActionResult UserIndex()
        //{
        //    return View("UserIndex");
        //}

        //public ActionResult GetUser(string id = null)
        //{
        //    if (string.IsNullOrEmpty(id))
        //    {
        //        return View("User", new User() { SysName = base.SystemName });
        //    }
        //    else
        //    {
        //        SimpleUserModel user = new UserVerify(this.RMUrl).GetUsers()
        //            .Find(i => i.UserGUID.Equals(id));
        //        //List<User> Users = new RolePermission().GetUsers(base.SystemName);
        //        return View("User", new User()
        //        {
        //            UGUID = user.UserGUID,
        //            UserName = user.UserName,
        //            LoginName = user.UserID,
        //            LoginPwd = string.Empty,
        //            SysName = SystemName
        //        });
        //    }
        //}

        //public ActionResult GetUserRole(string id)
        //{
        //    SimpleUserModel user = new UserVerify(RMUrl).GetUsers()
        //        .Find(i => i.UserGUID.Equals(id));
        //    ViewData["UserGuid"] = user.UserID; ;
        //    ViewData["UserName"] = user.UserName;
        //    //new RolePermission()
        //    //    .GetUsers(base.SystemName)
        //    //    .Find(i => i.UGUID.Equals(id)).UserName;
        //    return View("UserRole");
        //}

        //public string GetUsers()
        //{
        //    StringBuilder strJson = new StringBuilder("[ ");
        //    string strFormatter = "{{\"Name\":\"{0}\",\"Guid\":\"{1}\"}},";
        //    //List<User> users = new RolePermission().GetUsers(base.SystemName);
        //    List<SimpleUserModel> users = new UserVerify(RMUrl).GetUsers();
        //    foreach (SimpleUserModel user in users.OrderBy(u => u.UserName))
        //    {
        //        strJson.AppendFormat(strFormatter, user.UserName, user.UserGUID);
        //    }
        //    strJson.Remove(strJson.Length - 1, 1);
        //    strJson.Append("]");
        //    return strJson.ToString();
        //}

        //public string UpdUser(User u)
        //{
        //    //RolePermission dal = new RolePermission();           
        //    string msg = string.Empty;
        //    bool result = false;
        //    List<SimpleUserModel> users = new UserVerify(RMUrl).GetUsers();
        //    if (users.Any(i => i.UserName.Equals(u.UserName) && !i.UserGUID.Equals(u.UGUID)))
        //    {
        //        msg = Common.Resource.RolePermission.UseNameExist;
        //    }
        //    else if (users.Any(i => i.UserID.Equals(u.LoginName) && !i.UserGUID.Equals(u.UGUID)))
        //    {
        //        msg = Common.Resource.RolePermission.LoginNameExist;
        //    }
        //    else
        //    {
        //        if (string.IsNullOrEmpty(u.UGUID))
        //        {
        //            u.UGUID = Guid.NewGuid().ToString();
        //            result = new UserVerify(RMUrl).AddUser(
        //            new T_User()
        //            {
        //                Password = u.LoginPwd,
        //                Useable = true,
        //                UserGUID = u.UGUID,
        //                UserID = u.LoginName,
        //                UserName = u.UserName,
        //                OriginSystemName = SystemName,
        //                OriginSystemID = SubSystemID
        //            });
        //        }
        //        else
        //        {
        //            result = new UserVerify(RMUrl).UpdUser(
        //            new T_User()
        //            {
        //                Password = u.LoginPwd,
        //                Useable = true,
        //                UserGUID = u.UGUID,
        //                UserID = u.LoginName,
        //                UserName = u.UserName,
        //                OriginSystemName = SystemName,
        //                OriginSystemID = SubSystemID
        //            });
        //        }
        //        //result = dal.UpdUser(u);
        //    }
        //    if (result)
        //    {
        //        msg = General.Resource.Common.Success;
        //    }
        //    else
        //    {
        //        msg = string.Format("{0},{1}", msg, General.Resource.Common.Failed);
        //    }
        //    return string.Format("{{\"Result\":{0},\"Msg\":\"{1}\"}}"
        //        , result.ToString().ToLower(), msg);
        //}

        //public string DelUser(string id)
        //{
        //    //bool result = new RolePermission().DelUser(id);
        //    bool result = new UserVerify(RMUrl).DelUser(id);
        //    string msg = string.Empty;
        //    if (result)
        //    {
        //        msg = General.Resource.Common.Success;
        //    }
        //    else
        //    {
        //        msg = General.Resource.Common.Failed;
        //    }
        //    return string.Format("{{\"Result\":{0},\"Msg\":\"{1}\"}}"
        //        , result.ToString().ToLower(), msg);
        //}

        //public string GetRolesForUser(string id)
        //{
        //    StringBuilder strJson = new StringBuilder("[ ");
        //    string strFormatter = "{{\"Name\":\"{0}\",\"Guid\":\"{1}\",\"Stat\":{2}}},";
        //    //List<string> selectItems = dal.GetUserRole(id);
        //    //List<Role> roles = dal.GetRoles(base.SystemName);
        //    List<string> selectItems = new UserVerify(this.RMUrl).GetUserRoles(id)
        //        .Select(i => i.LocalRoleSymbolID).ToList();
        //    List<T_LocalRole> roles = new UserVerify(this.RMUrl).GetRoles();
        //    foreach (T_LocalRole r in roles)
        //    {
        //        strJson.AppendFormat(strFormatter, r.LocalRoleName, r.LocalRoleSymbolID
        //            , selectItems.Contains(r.LocalRoleSymbolID).ToString().ToLower());
        //    }
        //    strJson.Remove(strJson.Length - 1, 1);
        //    strJson.Append("]");
        //    return strJson.ToString();
        //}

        //public string UpdUserRole(string UserID, string Roles)
        //{
        //    List<string> roles = Roles.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        //    //bool result = new RolePermission().UpdUserRole(UserID, roles);
        //    List<R_LocalRole_User> lus = new List<R_LocalRole_User>();
        //    R_LocalRole_User lu = new R_LocalRole_User();
        //    List<SimpleUserModel> users = new UserVerify(RMUrl).GetUsers();

        //    foreach (string r in roles)
        //    {
        //        lu = new R_LocalRole_User()
        //        {
        //            CreateDate = DateTime.Now,
        //            CreateUTCDate = DateTime.UtcNow,
        //            LocalRoleSymbolID = r,
        //            OriginSystemID = SubSystemID,
        //            SystemID = SubSystemID,
        //            SystemName = SystemName,
        //            UserID = UserID,
        //            UserName = users.Find(i => i.UserGUID.Equals(UserID)).UserName
        //        };
        //        lus.Add(lu);
        //    }
        //    bool result = new UserVerify(RMUrl).UpdUserRoles(UserID, lus);
        //    string msg = string.Empty;
        //    if (result)
        //    {
        //        msg = General.Resource.Common.Success;
        //    }
        //    else
        //    {
        //        msg = General.Resource.Common.Failed;
        //    }
        //    return string.Format("{{\"Result\":{0},\"Msg\":\"{1}\"}}"
        //        , result.ToString().ToLower(), msg);
        //}
        #endregion

        #region User
        public ActionResult UserIndex()
        {
            return View("UserIndex");
        }

        //public ActionResult GetUser(string id = null)
        //{
        //    //if (string.IsNullOrEmpty(id))
        //    //{
        //    //    return View("User", new User());
        //    //}
        //    //else
        //    //{
        //        //SimpleUserModel user = new UserVerify(this.RMUrl).GetUsers()
        //        //    .Find(i => i.UserGUID.Equals(id));
        //        ////List<User> Users = new RolePermission().GetUsers(base.SystemName);
        //        //return View("User", new User()
        //        //{
        //        //    UGUID = user.UserGUID,
        //        //    UserName = user.UserName,
        //        //    LoginName = user.UserID,
        //        //    LoginPwd = string.Empty,
        //        //    SysName = SystemName
        //        //});
        //    //}
        //}

        //public ActionResult GetUserRole(string id)
        //{
        //    SimpleUserModel user = new UserVerify(RMUrl).GetUsers()
        //        .Find(i => i.UserGUID.Equals(id));
        //    ViewData["UserGuid"] = user.UserID; ;
        //    ViewData["UserName"] = user.UserName;
        //    //new RolePermission()
        //    //    .GetUsers(base.SystemName)
        //    //    .Find(i => i.UGUID.Equals(id)).UserName;
        //    return View("UserRole");
        //}

        //public string GetUsers()
        //{
        //    StringBuilder strJson = new StringBuilder("[ ");
        //    string strFormatter = "{{\"Name\":\"{0}\",\"Guid\":\"{1}\"}},";
        //    //List<User> users = new RolePermission().GetUsers(base.SystemName);
        //    List<SimpleUserModel> users = new UserVerify(RMUrl).GetUsers();
        //    foreach (SimpleUserModel user in users.OrderBy(u => u.UserName))
        //    {
        //        strJson.AppendFormat(strFormatter, user.UserName, user.UserGUID);
        //    }
        //    strJson.Remove(strJson.Length - 1, 1);
        //    strJson.Append("]");
        //    return strJson.ToString();
        //}

        //public string UpdUser(User u)
        //{
        //    //RolePermission dal = new RolePermission();           
        //    string msg = string.Empty;
        //    bool result = false;
        //    List<SimpleUserModel> users = new UserVerify(RMUrl).GetUsers();
        //    if (users.Any(i => i.UserName.Equals(u.UserName) && !i.UserGUID.Equals(u.UGUID)))
        //    {
        //        msg = Common.Resource.RolePermission.UseNameExist;
        //    }
        //    else if (users.Any(i => i.UserID.Equals(u.LoginName) && !i.UserGUID.Equals(u.UGUID)))
        //    {
        //        msg = Common.Resource.RolePermission.LoginNameExist;
        //    }
        //    else
        //    {
        //        if (string.IsNullOrEmpty(u.UGUID))
        //        {
        //            u.UGUID = Guid.NewGuid().ToString();
        //            result = new UserVerify(RMUrl).AddUser(
        //            new T_User()
        //            {
        //                Password = u.LoginPwd,
        //                Useable = true,
        //                UserGUID = u.UGUID,
        //                UserID = u.LoginName,
        //                UserName = u.UserName,
        //                OriginSystemName = SystemName,
        //                OriginSystemID = SubSystemID
        //            });
        //        }
        //        else
        //        {
        //            result = new UserVerify(RMUrl).UpdUser(
        //            new T_User()
        //            {
        //                Password = u.LoginPwd,
        //                Useable = true,
        //                UserGUID = u.UGUID,
        //                UserID = u.LoginName,
        //                UserName = u.UserName,
        //                OriginSystemName = SystemName,
        //                OriginSystemID = SubSystemID
        //            });
        //        }
        //        //result = dal.UpdUser(u);
        //    }
        //    if (result)
        //    {
        //        msg = General.Resource.Common.Success;
        //    }
        //    else
        //    {
        //        msg = string.Format("{0},{1}", msg, General.Resource.Common.Failed);
        //    }
        //    return string.Format("{{\"Result\":{0},\"Msg\":\"{1}\"}}"
        //        , result.ToString().ToLower(), msg);
        //}

        //public string DelUser(string id)
        //{
        //    //bool result = new RolePermission().DelUser(id);
        //    bool result = new UserVerify(RMUrl).DelUser(id);
        //    string msg = string.Empty;
        //    if (result)
        //    {
        //        msg = General.Resource.Common.Success;
        //    }
        //    else
        //    {
        //        msg = General.Resource.Common.Failed;
        //    }
        //    return string.Format("{{\"Result\":{0},\"Msg\":\"{1}\"}}"
        //        , result.ToString().ToLower(), msg);
        //}

        //public string GetRolesForUser(string id)
        //{
        //    StringBuilder strJson = new StringBuilder("[ ");
        //    string strFormatter = "{{\"Name\":\"{0}\",\"Guid\":\"{1}\",\"Stat\":{2}}},";
        //    //List<string> selectItems = dal.GetUserRole(id);
        //    //List<Role> roles = dal.GetRoles(base.SystemName);
        //    List<string> selectItems = new UserVerify(this.RMUrl).GetUserRoles(id)
        //        .Select(i => i.LocalRoleSymbolID).ToList();
        //    List<T_LocalRole> roles = new UserVerify(this.RMUrl).GetRoles();
        //    foreach (T_LocalRole r in roles)
        //    {
        //        strJson.AppendFormat(strFormatter, r.LocalRoleName, r.LocalRoleSymbolID
        //            , selectItems.Contains(r.LocalRoleSymbolID).ToString().ToLower());
        //    }
        //    strJson.Remove(strJson.Length - 1, 1);
        //    strJson.Append("]");
        //    return strJson.ToString();
        //}

        //public string UpdUserRole(string UserID, string Roles)
        //{
        //    List<string> roles = Roles.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        //    //bool result = new RolePermission().UpdUserRole(UserID, roles);
        //    List<R_LocalRole_User> lus = new List<R_LocalRole_User>();
        //    R_LocalRole_User lu = new R_LocalRole_User();
        //    List<SimpleUserModel> users = new UserVerify(RMUrl).GetUsers();

        //    foreach (string r in roles)
        //    {
        //        lu = new R_LocalRole_User()
        //        {
        //            CreateDate = DateTime.Now,
        //            CreateUTCDate = DateTime.UtcNow,
        //            LocalRoleSymbolID = r,
        //            OriginSystemID = SubSystemID,
        //            SystemID = SubSystemID,
        //            SystemName = SystemName,
        //            UserID = UserID,
        //            UserName = users.Find(i => i.UserGUID.Equals(UserID)).UserName
        //        };
        //        lus.Add(lu);
        //    }
        //    bool result = new UserVerify(RMUrl).UpdUserRoles(UserID, lus);
        //    string msg = string.Empty;
        //    if (result)
        //    {
        //        msg = General.Resource.Common.Success;
        //    }
        //    else
        //    {
        //        msg = General.Resource.Common.Failed;
        //    }
        //    return string.Format("{{\"Result\":{0},\"Msg\":\"{1}\"}}"
        //        , result.ToString().ToLower(), msg);
        //}
        #endregion
    }
}
