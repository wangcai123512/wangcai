using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseController;
using System.Web.Mvc;
using FMS.DAL;
using System.Web.Script.Serialization;
using FMS.Model;

namespace FMS.BLL
{
    /// <summary>
    /// 查询收款
    /// </summary>
    public class ReceivablesQueryController : UserController
    {
        public ReceivablesQueryController()
            : base("Receivables_Query")
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
        /// 收款纪录信息页
        /// </summary>
        /// <param name="id">收款纪录标识</param>
        /// <returns></returns>
        public ActionResult ReceivablesRecord(string id)
        {
            return View(new RecPayRecordSvc().GetReceivablesRecord(id, Session["CurrentCompany"].ToString()));
        }

        /// <summary>
        /// 获取收款纪录列表
        /// </summary>
        /// <param name="rows">页大小</param>
        /// <param name="page">页索引</param>
        /// <returns></returns>
        public string GetReceivablesList(string rows, string page)
        {
            int count = 0;
            string C_GUID = Session["CurrentCompany"].ToString();
            string strFormatter = "{{\"total\":\"{0}\",\"rows\":{1}}}";
            StringBuilder strJson = new StringBuilder();
            List<T_RecPayRecord> RecPayRecord = new List<T_RecPayRecord>();
            RecPayRecord = new RecPayRecordSvc().GetReceivablesRecord(C_GUID, int.Parse(page), int.Parse(rows), out count);
            strJson.AppendFormat(strFormatter, count, new JavaScriptSerializer().Serialize(RecPayRecord));
            return strJson.ToString();
        }
    }
}