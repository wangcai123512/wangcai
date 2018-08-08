using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseController;
using System.Web.Mvc;
using FMS.Model;
using FMS.DAL;
using System.Web.Script.Serialization;

namespace FMS.BLL
{
    public class ReceivablesDeclareCustomerQueryController:UserController
    {
        public ReceivablesDeclareCustomerQueryController()
            : base("ReceivablesDeclareCustomerQuery")
        { }
        public ActionResult Query()
        {
            return View();
        }
        public string GetReceivablesDeclareCustomerList(string dateBegin, string dateEnd, string customer, string state, string incomeGrp, string currency ,string business_GUID, string subBusiness_GUID)
        {
            int count = 0;
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            //string strFormatter = "{{\"total\":\"{0}\",\"rows\":{1}}}";
            StringBuilder strJson = new StringBuilder();
            List<T_DeclareCustomer> List = new List<T_DeclareCustomer>();
            List = new DeclareCustomerSvc().GetReceivablesDeclareCustomerList(C_GUID, 1, -1, out count, dateBegin, dateEnd, customer, state, incomeGrp, currency, business_GUID, subBusiness_GUID);
            string json = new JavaScriptSerializer().Serialize(List);
            //strJson.AppendFormat(strFormatter, count, new JavaScriptSerializer().Serialize(List));
            // return strJson.ToString();
            return json;
        }
        public string GetDCVoucher(string rows, string page, string GUID)
        {
            int count = 0;
            List<T_DeclareCustomer> List = new DeclareCustomerSvc().GetDCVoucher(1, -1, out count, GUID);
            string json = new JavaScriptSerializer().Serialize(List);
            return json;
        } 
    }
}
