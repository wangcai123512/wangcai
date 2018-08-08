using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using BaseController;
using FMS.DAL;
using FMS.Model;

namespace FMS.BLL
{
    /// <summary>
    /// 注册固定资产
    /// </summary>
    public class FixedAssetsRegisterController : UserController
    {
        public FixedAssetsRegisterController()
            : base("FixedAssetsRegister")
        {
        }

        /// <summary>
        /// 列表页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View("Index");
        }

        /// <summary>
        /// 固定资产信息页
        /// </summary>
        /// <param name="id">固定资产标识</param>
        /// <returns></returns>
        public ActionResult GetAssetsInfo(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return View("AssetsInfo", new T_Assets()
                {
                    A_GUID = Guid.NewGuid().ToString(),
                    PurchaseDate = DateTime.Now,
                    RegisterDate = DateTime.Now,
                    Creator = base.userData.LoginFullName
                });
            }
            else
            {
                string C_GUID = Session["CurrentCompanyGuid"].ToString();
                T_Assets fa = new FixedAssetsSvc().GetAssets(id,C_GUID).FirstOrDefault();
                fa.Creator = base.userData.LoginFullName;
                return View("AssetsInfo", fa);
            }
        }

        /// <summary>
        /// 获取固定资产列表数据
        /// </summary>
        /// <param name="rows">页大小</param>
        /// <param name="page">页索引</param>
        /// <returns></returns>
        public string GetAssetses(string rows, string page)
        {
            int total = 0;
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            string strFormatter = "{{\"total\":\"{0}\",\"rows\":{1}}}";
            StringBuilder strJson = new StringBuilder();
            IEnumerable<T_Assets> Assetses =
                new FixedAssetsSvc().GetAssetses(int.Parse(rows), int.Parse(page), out total, 1,C_GUID);
            strJson.AppendFormat(strFormatter, total, new JavaScriptSerializer().Serialize(Assetses));
            return strJson.ToString();
        }

        /// <summary>
        /// 更新固定资产信息
        /// </summary>
        /// <param name="item">固定资产对象</param>
        /// <returns></returns>
        public string UpdAssets(T_Assets item)
        {
            string msg = string.Empty;
            item.C_GUID = Session["CurrentCompanyGuid"].ToString();
            bool result = new FixedAssetsSvc().UpdAssets(item);
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
        /// 获取固定资产分类
        /// </summary>
        /// <returns></returns>
        public string GetAssetsesGroup()
        {
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            string strFormatter = "{{\"text\":\"{0}\",\"value\":\"{1}\"}},";
            StringBuilder strJson = new StringBuilder("[ ");
            List<T_AssetsGroup> agps = new FixedAssetsSvc().GetAssetsGroups(C_GUID);
            foreach (T_AssetsGroup item in agps)
            {
                strJson.AppendFormat(strFormatter, item.Name, item.AG_GUID);
            }
            strJson.Remove(strJson.Length - 1, 1);
            strJson.Append("]");
            return strJson.ToString();
        }
    }
}