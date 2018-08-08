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
    public class TaxAndTaxRateSetController : UserController
    {
        public TaxAndTaxRateSetController()
            : base("TaxAndTaxRate_Set")
        { }

        /// <summary>
        /// 列表页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewData["T_GUID"] = Guid.NewGuid().ToString();
            return View();
        }


        /// <summary>
        /// 获取费用列表
        /// </summary>
        /// <param name="rows">页大小</param>
        /// <param name="page">页索引</param>
        /// <returns></returns>
        public string GetTaxAndTaxRateList(string rows, string page)
        {
            int count = 0;
            string C_GUID = Session["CurrentCompany"].ToString();
            string strFormatter = "{{\"total\":\"{0}\",\"rows\":{1}}}";
            StringBuilder strJson = new StringBuilder();
            List<T_Tax> List = new List<T_Tax>();
            List = new TaxSvc().GetTax(C_GUID, int.Parse(page), int.Parse(rows), out count);
            strJson.AppendFormat(strFormatter, count, new JavaScriptSerializer().Serialize(List));
             return strJson.ToString();
        }

        /// <summary>
        /// 新增申报预收客户款记录
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public string UpdTaxAndTaxRate(T_Tax form)
        {
            bool result = false;
            string msg = string.Empty;
            form.C_GUID = Session["CurrentCompany"].ToString();
            result = new TaxSvc().UpdTax(form);
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
