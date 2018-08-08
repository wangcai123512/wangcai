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
    /// <summary>
    /// 明细科目
    /// </summary>
    public class DetailedAccountController : UserController
    {
        public DetailedAccountController()
            : base("DetailedAccount")
        { }

        /// <summary>
        /// 列表页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 明细科目信息页
        /// </summary>
        /// <param name="id">明细科目标识</param>
        /// <param name="pid">明细科目父级标识</param>
        /// <returns></returns>
        public ActionResult DetailedAccount(string id, string pid = null,string flag=null)
        {
            ViewData["flag"] = flag;
            if (string.IsNullOrEmpty(id))
            {
                return View(new T_DetailedAccount() { DA_GUID = Guid.NewGuid().ToString(), ParentAccGuid = pid });
            }
            else
            {
                return View(new AccountSvc().GetDetailsAcc(id, Session["CurrentCompanyGuid"].ToString()).FirstOrDefault());
            }
        }

        /// <summary>
        /// 明细科目列表数据
        /// </summary>
        /// <returns></returns>
        public string GetDetails()
        {
            return GenerateDtlAccsJson(new AccountSvc().GetDetailsAccs(Session["CurrentCompanyGuid"].ToString()), string.Empty);
        }

        /// <summary>
        /// 明细科目列表数据json
        /// </summary>
        /// <param name="ds">数据源</param>
        /// <param name="pid">明细科目父级标识</param>
        /// <returns></returns>
        private string GenerateDtlAccsJson(List<T_DetailedAccount> ds, string pid)
        {
            StringBuilder strJson = new StringBuilder("[ ");
            string strFormatter = "{{\"DA_GUID\":\"{0}\",\"Name\":\"{1}\",\"children\":{2},\"IsRoot\":{3},\"AccCode\":\"{4}\"{5}}},";
            string strChildren = string.Empty;
            foreach (T_DetailedAccount item in ds.Where(i => i.ParentAccGuid.Equals(pid)).OrderBy(i => i.AccCode))
            {
                strChildren = GenerateDtlAccsJson(ds, item.DA_GUID);
                strJson.AppendFormat(strFormatter, item.DA_GUID, item.Name,
                    strChildren, string.IsNullOrEmpty(item.ParentAccGuid).ToString().ToLower(), item.AccCode,
                    string.IsNullOrWhiteSpace(strChildren.Replace('[', ' ').Replace(']', ' ')) ? string.Empty : ",\"state\":\"closed\"");
            }
            strJson.Remove(strJson.Length - 1, 1);
            strJson.Append("]");
            return strJson.ToString();
        }

        /// <summary>
        /// 更新明细科目
        /// </summary>
        /// <param name="acc">明细科目对象</param>
        /// <returns></returns>
        public string UpdDetailedAccount(T_DetailedAccount acc,string flag,string cname)
        {
            if (flag=="1")
            {
                acc.D_GUID = acc.Name;
                acc.Name = cname;
            }
            AccountSvc svc = new AccountSvc();
            bool result = false;
            string msg = string.Empty;
            List<T_DetailedAccount> accs = svc.GetDetailsAccs(Session["CurrentCompanyGuid"].ToString());
            if (accs.Any(i => !i.DA_GUID.Equals(acc.DA_GUID) && i.AccCode.Equals(acc.AccCode)))
            {
                msg = FMS.Resource.Account.Account.AccExisted;
            }
            else
            {
                result = svc.UpdDetailedAccount(acc);
                msg = result ? General.Resource.Common.Success : General.Resource.Common.Failed;
            }
            return string.Format("{{\"Result\":{0},\"Msg\":\"{1}\"}}"
                , result.ToString().ToLower(), msg);
        }

        /// <summary>
        /// 删除明细科目
        /// </summary>
        /// <param name="id">明细科目标识</param>
        /// <returns></returns>
        public string DelDetailedAccount(string id)
        {
            bool result = new AccountSvc().DelDetailedAccount(id);
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
    }
}
