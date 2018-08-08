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
    public class CurrencyAndExchangeRateManagementController : UserController
    {
        public CurrencyAndExchangeRateManagementController()
            : base("CurrencyAndExchangeRate_Management ")
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
        /// 获取费用列表
        /// </summary>
        /// <param name="rows">页大小</param>
        /// <param name="page">页索引</param>
        /// <returns></returns>
        public string GetCurrencyAndExchangeRateList(string rows, string page,string dateBegin, string dateEnd)
        {
            int count = 0;
            string C_GUID = Session["CurrentCompany"].ToString();
            string strFormatter = "{{\"total\":\"{0}\",\"rows\":{1}}}";
            StringBuilder strJson = new StringBuilder();
            List<T_RateHistory> List = new List<T_RateHistory>();
            List = new CurrencySvc().GetRateHistory(C_GUID, int.Parse(page), int.Parse(rows), out count,dateBegin,dateEnd);
            strJson.AppendFormat(strFormatter, count, new JavaScriptSerializer().Serialize(List));
             return strJson.ToString();
        }
        public string GetCurrencyAndExchangeRateLists()
        {
            int count = 0;
            string C_GUID = Session["CurrentCompany"].ToString();
            string json = new JavaScriptSerializer().Serialize(new CurrencySvc().GetRateHistory(C_GUID, out count));
            return json;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="date"></param>
        /// <param name="famount"></param>
        /// <param name="fcurrency"></param>
        /// <param name="tamount"></param>
        /// <param name="tcurrency"></param>
        /// <returns></returns>
        public string UpdCurrencyAndExchangeRate(string guid, DateTime date, Decimal famount, string fcurrency, Decimal tamount, string tcurrency)
        {
            bool result = false;
            string msg = string.Empty;
            T_RateHistory form=new T_RateHistory();
            form.GUID = guid;
            form.Date = date;
            form.FAmount = famount;
            form.FCurrency = fcurrency;
            form.TAmount = tamount;
            form.TCurrency = tcurrency;
            form.C_GUID = Session["CurrentCompany"].ToString();
            result = new CurrencySvc().UpdRateHistory(form);
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
