using System.Web.Mvc;
using System.Web.Script.Serialization;
using BaseController;
using FMS.DAL;

namespace FMS.BLL
{
    /// <summary>
    /// 查询付款
    /// </summary>
    public class PaymentQueryController:UserController
    {
        public PaymentQueryController()
            : base("Payment_Query")
        {}

        /// <summary>
        /// 列表页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 付款纪录信息页
        /// </summary>
        /// <param name="id">付款纪录标识</param>
        /// <returns></returns>
        public ActionResult PaymentRecord(string id)
        {
            return View(new RecPayRecordSvc().GetPaymentRecord(id, Session["CurrentCompany"].ToString()));
        }

        /// <summary>
        /// 付款纪录列表数据
        /// </summary>
        /// <returns></returns>
        public string GetPaymentList()
        {
            return new JavaScriptSerializer().Serialize(new RecPayRecordSvc().GetAllPaymentRecord(Session["CurrentCompany"].ToString()));
        }

    }
}
