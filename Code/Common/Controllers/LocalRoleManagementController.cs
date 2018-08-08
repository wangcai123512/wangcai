using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using BaseController;
using Common.Models;
using Common.UserIdentityVerify;
using PermissionSys.Models;

namespace Common.Controllers
{
    public class LocalRoleManagementController : UserController
    {
        public LocalRoleManagementController() : base("Local_Role_Management") { }
        #region Role
        public ActionResult RoleIndex()
        {
            return View("RoleIndex");
        }

        public ActionResult GetRole(string id = null)
        {
            if (string.IsNullOrEmpty(id))
            {
                return View("Role", new Role() { SysName = base.SystemName });
            }
            else
            {
                T_LocalRole role = new UserVerify(this.RMUrl).GetRoles()
                    .Find(i => i.LocalRoleSymbolID.Equals(id));
                Role r = new Role()
                {
                    Guid = role.LocalRoleSymbolID,
                    Name = role.LocalRoleName,
                    SysName = role.SystemName,
                };
                return View("Role", r);
            }
        }

        public ActionResult GetRolePermission(string id)
        {
            ViewData["RoleGuid"] = id;
            ViewData["RoleName"] = new UserVerify(this.RMUrl).GetRoles()
                .Find(i => i.LocalRoleSymbolID.Equals(id)).LocalRoleName;
            return View("RolePermission");
        }

        public string GetPermission(string id)
        {
            //List<string> selectedItems = new RolePermission().GetRolePermission(id);
            List<string> selectedItems = new UserVerify(RMUrl).GetRolePermission(id)
                .Select(i => i.SubfunctionID).Distinct().ToList();
            string CultureFlag = System.Threading.Thread.CurrentThread.CurrentCulture.ToString();
            StringBuilder strJson = new StringBuilder("[ ");
            List<string> saasModules = this.HttpContext.Application["SaasModules"] as List<string>;
            List<TreeMenuItem> allMenuNodes = this.HttpContext.Application["Functions"] as List<TreeMenuItem>;
            List<TreeMenuItem> menuLst = allMenuNodes.Where(i => saasModules.Contains(i.GUID) || saasModules.Contains(i.ModuleID))
                .Distinct().ToList();
            menuLst.ForEach(i => i.ModuleID = i.ModuleID ?? string.Empty);
            return GenerateJson(CultureFlag, menuLst, selectedItems);
        }

        private string GenerateJson(string cultureFlag, List<TreeMenuItem> items, List<string> selectedItems)
        {
            string str = GenerateNavigatorTreeNode(cultureFlag,
                items, string.Empty, selectedItems);
            return str;
        }

        private string GenerateNavigatorTreeNode(string cultureFlag, List<TreeMenuItem> items, string pGuid, List<string> selectedItems)
        {
            StringBuilder strBld = new StringBuilder("[ ");
            List<TreeMenuItem> citems = items.Where(i => i.ModuleID.Equals(pGuid)).OrderBy(i => i.OrderNumber).ToList();
            foreach (TreeMenuItem mod in citems)
            {
                strBld.Append("{");
                strBld.AppendFormat(
                    "\"id\":\"{0}\",\"text\":\"{1}\",\"checked\":\"{2}\",\"children\":{3}"
            ,
            mod.GUID,
            cultureFlag.Equals("en-US") ? mod.EnglishName : mod.ChineseName,
            selectedItems.Contains(mod.GUID) ? "Checked" : string.Empty,
            items.Any(i => i.ModuleID.Equals(mod.GUID))
            ? GenerateNavigatorTreeNode(cultureFlag, items, mod.GUID, selectedItems)
            : "[]"
            );
                strBld.Append("},");
            }
            strBld.Remove(strBld.Length - 1, 1);
            strBld.Append("]");
            return strBld.ToString();
        }

        public string GetRoles()
        {
            StringBuilder strJson = new StringBuilder("[ ");
            string strFormatter = "{{\"Name\":\"{0}\",\"Guid\":\"{1}\"}},";
            //List<Role> roles = new RolePermission().GetRoles(base.SystemName);
            List<T_LocalRole> roles = new UserVerify(this.RMUrl).GetRoles();
            foreach (T_LocalRole r in roles)
            {
                strJson.AppendFormat(strFormatter, r.LocalRoleName, r.LocalRoleSymbolID);
            }
            strJson.Remove(strJson.Length - 1, 1);
            strJson.Append("]");
            return strJson.ToString();
        }

        public string DelRole(string id)
        {
            //bool result = new RolePermission().DelRole(id);
            bool result = new UserVerify(RMUrl).DelRole(id);
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

        public string UpdRole(Role r)
        {
            bool result = false;
            if (string.IsNullOrEmpty(r.Guid))
            {
                r.Guid = Guid.NewGuid().ToString();
                result = new UserVerify(this.RMUrl).AddRole(r.Name);
            }
            else
            {
                result = new UserVerify(this.RMUrl).UpdRole(r.Guid, r.Name);
            }

            //new RolePermission().UpdRole(r);
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

        public string UpdRolePermission(string RoleID, string Funs)
        {
            //R_LocalRole_Subfunction
            List<string> funs = Funs.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            //bool result = new RolePermission().UpdRolePermission(RoleID, funs);
            List<R_LocalRole_Subfunction> rfs = new List<R_LocalRole_Subfunction>();
            R_LocalRole_Subfunction item = new R_LocalRole_Subfunction();
            List<TreeMenuItem> menuLst = this.HttpContext.Application["Functions"] as List<TreeMenuItem>;
            TreeMenuItem menuItem = new TreeMenuItem();
            string msg = string.Empty;
            foreach (string fun in funs)
            {
                menuItem = menuLst.Find(i => i.GUID.Equals(fun));
                item = new R_LocalRole_Subfunction()
                {
                    CreateDate = DateTime.Now,
                    CreateUTCDate = DateTime.UtcNow,
                    LocalRoleSymbolID = RoleID,
                    ModuleID = string.IsNullOrEmpty(menuItem.ModuleID) ? fun : menuItem.ModuleID,
                    SubfunctionEnglishName = menuItem.EnglishName,
                    SubfunctionID = fun,
                    SubfunctionName = menuItem.ChineseName,
                    SystemID = SubSystemID,
                    SystemName = SystemName
                };
                rfs.Add(item);
            }
            bool result = new UserVerify(RMUrl).UpdRolePermission(RoleID, rfs);
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
        #endregion
    }
}
