using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using BaseController;
using FMS.DAL;
using FMS.Model;
using System.Text;
using System.Web;
using System.IO;

namespace FMS.BLL
{
    /// <summary>
    /// 记录费用
    /// </summary>
    public class DetailManagementController : UserController
    {
        public DetailManagementController()
            : base("Detail_Record")
        { }

        /// <summary>
        /// 列表页
        /// </summary>
        /// <returns></returns>
        public ActionResult DetailRecord()
        {
            ViewData["GUID"] = Guid.NewGuid().ToString();
            return View();
        }

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="guid">纪录标识</param>
        /// <returns></returns>
        public string DelDetails(string guid)
        {
            bool result = new DetailSvc().DelDetails(guid);
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
        /// 更新类别启用状态
        /// </summary>
        /// <param name="guid">纪录标识</param>
        /// <param name="state">状态</param>
        /// <returns></returns>
        public string UpdDetailstate(string guid, string state)
        {
            bool result = new DetailSvc().UpdDetailstate(guid, state);
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
        /// 新增类别
        /// </summary>
        /// <returns></returns>
        public string UpdDetail(string guid,string name)
        {
            T_DetailedCategories detail=new T_DetailedCategories();
            detail.GUID = guid;
            detail.Name = name;
            detail.State = "启用";
            bool result = new DetailSvc().UpdDetail(detail);
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
        /// 获取费用列表
        /// </summary>
        /// <param name="rows">页大小</param>
        /// <param name="page">页索引</param>
        /// <returns></returns>
        public string GetDetailList(string rows, string page)
        {
            int count = 0;
            string strFormatter = "{{\"total\":\"{0}\",\"rows\":{1}}}";
            StringBuilder strJson = new StringBuilder();
            List<T_DetailedCategories> IERcord = new List<T_DetailedCategories>();
            IERcord = new DetailSvc().GetDetailList(int.Parse(page), int.Parse(rows), out count);
             strJson.AppendFormat(strFormatter, count, new JavaScriptSerializer().Serialize(IERcord));
             return strJson.ToString();
        }
    }
}
