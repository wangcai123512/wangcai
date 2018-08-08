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
    /// 净现金流
    /// </summary>
    public class NetCashFlowsRecordController : UserController
    {
        public NetCashFlowsRecordController()
            : base("NetCashFlowsRecord")
        { }
        /// <summary>
        /// 列表页
        /// </summary>
        /// <returns></returns>
        public ViewResult Index()
        {
            //获取公司全称
            ViewData["ChineseFullName"] = (new CompanySvc().GetCompanyInformation(Session["CurrentCompanyGuid"].ToString()).FirstOrDefault()).ChineseFullName;
            ViewData["Code"] = (new CompanySvc().GetCompanyCurrceny(Session["CurrentCompanyGuid"].ToString()).FirstOrDefault()).Code;
            return View();
        }
        /// <summary>   
        /// 查询账户下现金流列表(账号信息)
        /// </summary>
        /// <param>hdy 17.3.27</param>
        /// <returns></returns>
        public string GetNetCashFlowsRecordList(string BA_GUID, string dateBegin, string dateEnd, int pageIndex = 1, int pageSize = 10)
        {
            int count = 0;
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            List<T_RecPayRecord> Record = new List<T_RecPayRecord>();
            Record = new RecPayRecordSvc().GetNetCashFlowsRecordList(BA_GUID, C_GUID, pageIndex, -1, out count,
                    dateBegin, dateEnd);
            return new JavaScriptSerializer().Serialize(Record);
        }
        /// <summary>   
        /// 查询账户下现金流列表(本币汇总)
        /// </summary>
        /// <param>hdy 17.3.27</param>
        /// <returns></returns>
        public string GetNetCashLocalCurrencyList(string BA_GUID, string dateBegin, string dateEnd, int pageIndex = 1, int pageSize = 10)
        {
            int count = 0;
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            List<T_RecPayRecord> Record = new List<T_RecPayRecord>();
            Record = new RecPayRecordSvc().GetNetCashLocalCurrencyList(BA_GUID, C_GUID, pageIndex, -1, out count,
                    dateBegin, dateEnd);
            return new JavaScriptSerializer().Serialize(Record);
        }
        /// <summary>   
        /// 查询账户下现金流列表(统计货币汇总)
        /// </summary>
        /// <param>hdy 17.3.28</param>
        /// <returns></returns>
        public string GetNetCashStatisticalCurrencyList(string dateBegin, string dateEnd, int pageIndex = 1, int pageSize = 10)
        {
            int count = 0;
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            List<T_RecPayRecord> Record = new List<T_RecPayRecord>();
            Record = new RecPayRecordSvc().GetNetCashStatisticalCurrencyList(C_GUID, pageIndex, -1, out count,
                    dateBegin, dateEnd);
            return new JavaScriptSerializer().Serialize(Record);
        }

        /// <summary>   
        /// 查询账户下净现金流帐
        /// </summary>
        /// <param>hdy 17.3.31</param>
        /// <returns></returns>
        public string GetCashInFlowsAccountRecordList(string BA_GUID, string dateBegin, string dateEnd, int pageIndex = 1, int pageSize = 10)
        {
            int count = 0;
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            List<T_RecPayRecord> Record = new List<T_RecPayRecord>();
            Record = new RecPayRecordSvc().GetCashInFlowsAccountRecordList(BA_GUID, C_GUID, pageIndex, -1, out count,
                    dateBegin, dateEnd);

            for (int i = 0; i < Record.Count; i++)
            {
                if (i == 0)
                {
                    Record[i].BalanceSumAmount = Record[i].InitialAmount + Record[i].RecSumAmountZ - Record[i].PaySumAmountZ;
                }
                else
                {
                    Record[i].BalanceSumAmount = Record[i - 1].BalanceSumAmount + Record[i].RecSumAmountZ - Record[i].PaySumAmountZ;
                }
                
            }
            return new JavaScriptSerializer().Serialize(Record);
        }
        
        /// <summary>   
        //查询现金流比较（返回月份，金额）
        /// </summary>
        /// <param>hdy 17.3.31</param>
        /// <returns></returns>
        public string CashInFlowsCompareRecordQuery(string BA_GUID, string dateBegin, string dateEnd)
        {
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            List<T_RecPayRecord> Record = new List<T_RecPayRecord>();
            Record = new RecPayRecordSvc().GetCashFlowsCompareRecordList(BA_GUID, C_GUID, dateBegin, dateEnd);
            return new JavaScriptSerializer().Serialize(Record);
        }
     }
}