using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using BaseController;
using FMS.DAL;
using FMS.Model;
using System.Text;
using System.Web;
using System.IO;
using Aspose.Cells;
using Common.Models;
using Newtonsoft.Json;
namespace FMS.BLL
{
    /// <summary>
    /// 查询付款
    /// </summary>
    public class CashInFlowsRecordController : UserController
    {
        public CashInFlowsRecordController()
            : base("CashInFlowsRecord")
        { }
        /// <summary>
        /// 列表页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewData["ChineseFullName"] = (new CompanySvc().GetCompanyInformation(Session["CurrentCompanyGuid"].ToString()).FirstOrDefault()).ChineseFullName;
            ViewData["Code"] = (new CompanySvc().GetCompanyCurrceny(Session["CurrentCompanyGuid"].ToString()).FirstOrDefault()).Code;
            return View();
        }


        /// <summary>
        /// 获取收款纪录列表
        /// </summary>
        /// <returns></returns>
        public string GetAccountCashInFlowsRecordList(string rows, string page, string dateBegin, string dateEnd, string BA_GUID)
        {
            int count = 0;
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            StringBuilder strJson = new StringBuilder();
            List<T_RecPayRecord> RecPayRecord = new List<T_RecPayRecord>();
            RecPayRecord = new RecPayRecordSvc().GetAccountCashInFlowsRecordList(C_GUID, 1, -1, out count,
                    dateBegin, dateEnd, BA_GUID);
            string json = new JavaScriptSerializer().Serialize(RecPayRecord);
            return json;
        }
        /// <summary>
        /// 获取收款纪录列表
        /// </summary>
        /// <returns></returns>
        public string GetCustomerCashInFlowsRecordList(string rows, string page, string dateBegin, string dateEnd, string BA_GUID)
        {
            int count = 0;
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            StringBuilder strJson = new StringBuilder();
            List<T_RecPayRecord> RecPayRecord = new List<T_RecPayRecord>();
            RecPayRecord = new RecPayRecordSvc().GetCustomerCashInFlowsRecordList(C_GUID, 1, -1, out count,
                    dateBegin, dateEnd, BA_GUID);
            string json = new JavaScriptSerializer().Serialize(RecPayRecord);
            return json;
        }
        /// <summary>   
        //查询现金流比较（返回月份，金额）
        /// </summary>
        /// <param>hdy 17.3.31</param>
        /// <returns></returns>
        public string GetCashInFlowsCompareRecordList(string BA_GUID, string dateBegin, string dateEnd)
        {
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            List<T_RecPayRecord> Record = new List<T_RecPayRecord>();
            Record = new RecPayRecordSvc().GetCashInFlowsCompareRecordList(BA_GUID, C_GUID, dateBegin, dateEnd);
            return new JavaScriptSerializer().Serialize(Record);
        }


    }
}
