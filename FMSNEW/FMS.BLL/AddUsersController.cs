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
    public class AddUsersController : UserController
    {
        public AddUsersController()
            : base("UsersAdd")
        {}
        /// <summary>
        /// 选择公司页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 获取用户列表信息页面
        /// </summary>
        public ActionResult GetUserList()
        {
            return Json(new CompanySvc().GetUserList(Session["CurrentCompanyGuid"].ToString()));
        }

        /// <summary>
        /// 添加用户信息
        /// </summary>
        /// <param name="id">用户标识</param>
        public string UpdateUser(T_User form)
        {
            form.U_GUID = Guid.NewGuid().ToString();
            form.C_GUID = Session["MasterCompanyGuid"].ToString();
            form.Password = "123456";
            form.EnterC_GUID = Session["CurrentCompanyGuid"].ToString();
            form.State = 0;
            bool result = new CompanySvc().UpdUser(form);
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
    }
}
