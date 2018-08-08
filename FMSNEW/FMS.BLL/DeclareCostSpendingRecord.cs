using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using BaseController;
using FMS.DAL;
using FMS.Model;

namespace FMS.BLL
{
    /// <summary>
    /// 查询付款
    /// </summary>
    public class DeclareCostSpendingRecordController : UserController
    {
        public DeclareCostSpendingRecordController()
            : base("DeclareCostSpendingRecord")
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
        /// 获取收款纪录列表
        /// </summary>
        /// <returns></returns>
        public string GetDeclareCostSpendingRecordList(string rows, string page, string BA_GUID)
        {
            int count = 0;
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            StringBuilder strJson = new StringBuilder();
            List<T_DeclareCostSpending> RecPayRecord = new List<T_DeclareCostSpending>();
            RecPayRecord = new DeclareCostSpendingSvc().GetDeclareCostSpendingRecordList(C_GUID, 1, -1, out count, BA_GUID);
            string json = new JavaScriptSerializer().Serialize(RecPayRecord);
            return json;
        }
    }
}
