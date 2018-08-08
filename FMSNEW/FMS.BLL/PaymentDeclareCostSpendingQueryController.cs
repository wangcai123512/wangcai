using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseController;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using FMS.DAL;
using FMS.Model;
namespace FMS.BLL

{
   public class PaymentDeclareCostSpendingQueryController:UserController
    {
       public PaymentDeclareCostSpendingQueryController(): base("PaymentDeclareCostSpendingQuery")
       { }
       public ActionResult Query()
       {
           return View();
       }
       public string GetPaymentDeclareCostSpendingList(string rows, string page, string dateBegin, string dateEnd, string customer, string incomeGrp, string currency, string state , string invtype, string record, string business_GUID, string subBusiness_GUID,string remark)
       {
           int count = 0;
           string C_GUID = Session["CurrentCompanyGuid"].ToString();
           // string strFormatter = "{{\"total\":\"{0}\",\"rows\":{1}}}";
           StringBuilder strJson = new StringBuilder();
           List<T_DeclareCostSpending> List = new List<T_DeclareCostSpending>();
           List = new DeclareCostSpendingSvc().GetPaymentDeclareCostSpendingList(C_GUID, 1, -1, out count, dateBegin, dateEnd, customer, incomeGrp, currency, state, invtype, record,business_GUID,subBusiness_GUID,remark);
           strJson.Append(new JavaScriptSerializer().Serialize(List));
           return strJson.ToString();
       }

       public string GetPaymentDeclareCostSpending(string id)
       {
           string C_GUID = Session["CurrentCompanyGuid"].ToString();
           T_DeclareCostSpending rev = new DeclareCostSpendingSvc().GetPaymentDeclareCostSpending(C_GUID, id);
           string json = new JavaScriptSerializer().Serialize(rev);
           return json;
           
       }
       public string GetDSVoucher(string rows, string page,string GUID) {
           int count = 0;
           List<T_DeclareCostSpending> List = new DeclareCostSpendingSvc().GetDSVoucher(1, -1, out count, GUID);
           string json = new JavaScriptSerializer().Serialize(List);
           return json;
       } 
    }
}
