using System;
using System.Web.Mvc;
using BaseController;
using FMS.DAL;
using FMS.Model;

namespace FMS.BLL
{
    /// <summary>
    /// 固定资产分类
    /// </summary>
    public class FixedAssetsGroupController : UserController
    {
        public FixedAssetsGroupController()
            : base("Fixed_AssetsGroup")
        { }

        /// <summary>
        /// 列表页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View("Index");
        }

        /// <summary>
        /// 固定资产分类页
        /// </summary>
        /// <param name="id">固定资产分类标识</param>
        /// <returns></returns>
        public ActionResult GetAssetsGroup(string id = null)
        {
            if (string.IsNullOrEmpty(id))
            {
                return View("Details", new T_AssetsGroup() { AG_GUID = Guid.NewGuid().ToString() });
            }
            else
            {
                string C_GUID = Session["CurrentCompanyGuid"].ToString();
                return View("Details",
                    new FixedAssetsSvc().GetAssetsGroups(C_GUID).Find(i => i.AG_GUID.Equals(id)));
            }
        }

        /// <summary>
        /// 固定资产分类列表数据
        /// </summary>
        /// <returns></returns>
        public JsonResult GetAssetsGroups()
        {
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            return Json(new FixedAssetsSvc().GetAssetsGroups(C_GUID));
        }

        /// <summary>
        /// 更新固定资产分类
        /// </summary>
        /// <param name="grp">固定资产分类对象</param>
        /// <returns></returns>
        public string UpdAssetsGroup(T_AssetsGroup grp)
        {
            string msg = string.Empty;
            grp.C_GUID = Session["CurrentCompanyGuid"].ToString();
            bool result = new FixedAssetsSvc().UpdAssetsGroup(grp);
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
        /// 删除固定资产分类
        /// </summary>
        /// <param name="id">固定资产分类标识</param>
        /// <returns></returns>
        public string DelAssetsGroup(string id)
        {
            string msg = string.Empty;
            bool result = false;
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
