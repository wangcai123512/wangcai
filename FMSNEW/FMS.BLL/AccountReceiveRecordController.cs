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
    public class AccountReceiveRecordController : UserController
    {
        public AccountReceiveRecordController()
            : base("AccountReceiveRecord")
        { }
        /// <summary>
        /// 列表页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewData["Code"] = (new CompanySvc().GetCompanyCurrceny(Session["CurrentCompanyGuid"].ToString()).FirstOrDefault()).Code;
            return View();
        }
        ///// 获取客户应收款总金额（汇总）
        ///// </summary>
        ///// <returns></returns>
        public string GetAllAmountReceivablesList(int pageIndex = 1, int pageSize = 10)
        {
            int count = 0;
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            List<T_IERecord> Record = new List<T_IERecord>();
            Record = new IESvc().GetAllAmountReceivablesList(C_GUID, pageIndex, -1, out count);
            return new JavaScriptSerializer().Serialize(Record);
        }
        /// 获取客户应收款总金额
        /// </summary>
        /// <returns></returns>
        public string GetTotalAmountReceivablesList(string RPerS, int pageIndex = 1, int pageSize = 10)
        {
            int count = 0;
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            string[] RPerSA = RPerS.Split(',');
            List<T_IERecord> RecordCount = new List<T_IERecord>();
            for (int i = 0; i < RPerSA.Length; i++)
            {
                string RPer = RPerSA[i].ToString();
                List<T_IERecord> Record = new List<T_IERecord>();
                Record = new IESvc().GetTotalAmountReceivablesList(RPer, C_GUID, pageIndex, -1, out count);
                if (Record.Count > 0)
                {
                    for (int a = 0; a < Record.Count; a++) { 
                        RecordCount.Add(Record[a]);
                    }
                }
            }
         return new JavaScriptSerializer().Serialize(RecordCount);
        }
        ///// 获取客户逾期应收款总金额（汇总）
        ///// </summary>
        ///// <returns></returns>
        public string GetAllAmountOverdueRList(int pageIndex = 1, int pageSize = 10)
        {
            int count = 0;
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            List<T_IERecord> Record = new List<T_IERecord>();
            Record = new IESvc().GetAllAmountOverdueRList(C_GUID, pageIndex, -1, out count);
            return new JavaScriptSerializer().Serialize(Record);
        }
        /// 获取客户逾期应收款总金额
        /// </summary>
        /// <returns></returns>
        public string GetTotalAmountOverdueRList(string RPerS, int pageIndex = 1, int pageSize = 10)
        {
            int count = 0;
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            string[] RPerSA = RPerS.Split(',');
            List<T_IERecord> RecordCount = new List<T_IERecord>();
            for (int i = 0; i < RPerSA.Length; i++)
            {
                string RPer = RPerSA[i].ToString();
                List<T_IERecord> Record = new List<T_IERecord>();
                Record = new IESvc().GetTotalAmountOverdueRList(RPer, C_GUID, pageIndex, -1, out count);
                if (Record.Count > 0)
                {
                    for (int a = 0; a < Record.Count; a++)
                    {
                        RecordCount.Add(Record[a]);
                    }
                }
            }
            return new JavaScriptSerializer().Serialize(RecordCount);
        }
        // 获取客户m天到n天逾期应收款总金额
        /// </summary>
        /// <returns></returns>
        public string GetTotalTodayAmountOverdueRList(string dateBegin, string dateEnd, string RPerS, int pageIndex = 1, int pageSize = 10)
        {
            int count = 0;
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            string[] RPerSA = RPerS.Split(',');
            List<T_IERecord> RecordCount = new List<T_IERecord>();
            for (int i = 0; i < RPerSA.Length; i++)
            {
                string RPer = RPerSA[i].ToString();
                List<T_IERecord> Record = new List<T_IERecord>();
                Record = new IESvc().GetTotalTodayAmountOverdueRList(dateBegin, dateEnd, RPer, C_GUID, pageIndex, -1, out count);
                if (Record.Count > 0)
                {
                    for (int a = 0; a < Record.Count; a++)
                    {
                        RecordCount.Add(Record[a]);
                    }
                }
            }
            return new JavaScriptSerializer().Serialize(RecordCount);
        }
    }
}
