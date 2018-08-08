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
            : base("CurrencyAndExchangeRateManagement ")
        { }

        /// <summary>
        /// 列表页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }


        public string NewGuid()
        {
            string guid = Guid.NewGuid().ToString();
            return guid;
        }

        /// <summary>
        /// 获取费用列表
        /// </summary>
        /// <param name="rows">页大小</param>
        /// <param name="page">页索引</param>
        /// <returns></returns>
        /// 

        public string CheckRate(string TCurrency, string Date)
        {
            bool result = false;
            int count = 0;
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            string FCurrency = Session["Currency"].ToString();
            List<T_RateHistory> List = new List<T_RateHistory>();
            List = new CurrencySvc().CheckRate(C_GUID, out count, FCurrency, TCurrency, Date);
            if (List.Count > 0)
            {
                result = true;
            }
            return new JavaScriptSerializer().Serialize(result);
        }

        public string GetCurrencyAndExchangeRateList(string current)
        {
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            string FCurrency= (new CompanySvc().GetCompanyCurrceny(Session["CurrentCompanyGuid"].ToString()).FirstOrDefault()).Code;
            List<T_RateHistory> List = new List<T_RateHistory>();
            List = new CurrencySvc().GetRateHistory(C_GUID, current, FCurrency);
            for (int i = 0; i < List.Count(); i++)

            {
                if (List[i].FCurrency != null && List[i].FCurrency != "")
                {
                List[i].VarString = List[i].FAmount + List[i].FCurrency + "=" + List[i].TAmount + List[i].TCurrency;
                } else {
                    List[i].VarString = "";
                }
            };
            string json = new JavaScriptSerializer().Serialize(List);
            return json;
        }
        //public string GetCurrencyAndExchangeRateLists()
        //{
        //    int count = 0;
        //    string C_GUID = Session["CurrentCompanyGuid"].ToString();
        //    string json = new JavaScriptSerializer().Serialize(new CurrencySvc().GetRateHistory(C_GUID, out count));
        //    return json;
        //}

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
        public Boolean UpdCurrencyAndExchangeRate(List<T_RateHistory> rateHistory)
        {
            bool result = false;
            string msg = string.Empty;
            //T_RateHistory rateHistroy = new T_RateHistory();
            //rateHistroy.GUID = Guid.NewGuid().ToString();
            //rateHistroy.C_GUID = Session["CurrentCompanyGuid"].ToString();
            //rateHistroy.FCurrency = Session["Currency"].ToString();
            //rateHistroy = rate;

            //T_RateHistory form=new T_RateHistory();
            //form.GUID = guid;
            //form.Date = date;
            //form.FAmount = famount;
            //form.FCurrency = fcurrency;
            //form.TAmount = tamount;
            //form.TCurrency = tcurrency;
            CurrencySvc currencyAccount = new CurrencySvc();
            foreach (T_RateHistory account in rateHistory)
            {
                //account.Currency = Session["Currency"].ToString();
                account.GUID = Guid.NewGuid().ToString();
                account.C_GUID = Session["CurrentCompanyGuid"].ToString();
                account.FCurrency = Session["Currency"].ToString();
               result= new CurrencySvc().UpdRateHistory(account);
               
            }


            return result;
            
        }
    }
}
