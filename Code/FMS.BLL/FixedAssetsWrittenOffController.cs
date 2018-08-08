using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using BaseController;
using FMS.DAL;
using FMS.Model;

namespace FMS.BLL
{
    /// <summary>
    /// 注销固定资产
    /// </summary>
    public class FixedAssetsWrittenOffController : UserController
    {
        public FixedAssetsWrittenOffController()
            : base("Fixed_Assets_Written_Off")
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
        /// 获取固定资产列表数据
        /// </summary>
        /// <param name="rows">页大小</param>
        /// <param name="page">页索引</param>
        /// <returns></returns>
        public string GetAssetses(string rows, string page)
        {
            int total = 0;
            string C_GUID = Session["CurrentCompany"].ToString();
            string strFormatter = "{{\"total\":\"{0}\",\"rows\":{1}}}";
            StringBuilder strJson = new StringBuilder();
            IEnumerable<T_Assets> Assetses =
                new FixedAssetsSvc().GetAssetses(int.Parse(rows), int.Parse(page), out total, 2,C_GUID);
            strJson.AppendFormat(strFormatter, total, new JavaScriptSerializer().Serialize(Assetses));
            return strJson.ToString();
        }

        /// <summary>
        /// 更新固定资产状态
        /// </summary>
        /// <param name="id">固定资产标识</param>
        /// <param name="flag">固定资产状态（Scrap：报废；Sell：出售）</param>
        /// <returns></returns>
        public string UpdAssetsStat(string id, string flag)
        {
            string msg = string.Empty;
            bool result = new FixedAssetsSvc().UpdAssetsStat(id, flag);
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
