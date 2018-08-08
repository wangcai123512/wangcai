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
    /// 查询固定资产
    /// </summary>
    public class FixedAssetsQueryController : UserController
    {
        public FixedAssetsQueryController()
            : base("Fixed_AssetsQuery")
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
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            string strFormatter = "{{\"total\":\"{0}\",\"rows\":{1}}}";
            StringBuilder strJson = new StringBuilder();
            IEnumerable<T_Assets> Assetses =
                new FixedAssetsSvc().GetAssetses(int.Parse(rows), int.Parse(page), out total, 0,C_GUID);
            strJson.AppendFormat(strFormatter, total, new JavaScriptSerializer().Serialize(Assetses));
            return strJson.ToString();
        }
    }
}
