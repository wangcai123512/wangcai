using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using BaseController;
using FMS.DAL;
using FMS.Model;
using System.Web;

namespace FMS.BLL
{
    /// <summary>
    /// 设置商业伙伴
    /// </summary>
    public class BusinessPartnerSettingController : UserController
    {
        public BusinessPartnerSettingController()
            : base("Business_PartnerSetting")
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
        /// 商业伙伴信息页
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult BusinessPartner(string id)
        {
            T_BusinessPartner partner;
            if (string.IsNullOrEmpty(id))
            {
                partner = new T_BusinessPartner() { BP_GUID = string.Empty };
            }
            else
            {
                string C_GUID = Session["CurrentCompanyGuid"].ToString();
                partner = new BusinessPartnerSvc().GetPartners(C_GUID, id).FirstOrDefault();
                Session["IndustryInvolved"] = partner.IndustryInvolved;
                //查询往来公司的银行信息  
                List<T_BankAccount> bankAccount = new BankAccountSvc().GetBankAccount(id);
                ViewBag.BankList = bankAccount;

            }
            return View(partner);
        }

        /// <summary>
        /// 更新商业伙伴信息
        /// </summary>
        /// <param name="partner">商业伙伴对象</param>
        /// <returns></returns>
        public string UpdPartner(T_BusinessPartner partner, List<T_BankAccount> bankItems)
        {           
            partner.C_GUID = Session["CurrentCompanyGuid"].ToString();
            if (string.IsNullOrEmpty(partner.BP_GUID))
            {
                partner.BP_GUID = Guid.NewGuid().ToString();
            }

            bool result = new BusinessPartnerSvc().UpdPartner(partner);
            string msg = string.Empty;
            if (result)
            {
                //增加银行信息
                BankAccountSvc bankAccount = new BankAccountSvc();
                if (bankItems != null)
                {
                    foreach (T_BankAccount account in bankItems)
                    {
                        if (string.IsNullOrEmpty(account.BA_GUID))
                        {
                            account.BA_GUID = Guid.NewGuid().ToString();
                        }
                        account.C_GUID = partner.BP_GUID;
                        account.AccountType = string.Empty;
                        bankAccount.UpdBankAccount(account); 
                    }
                }

                msg = General.Resource.Common.Success;
            }
            else
            {
                msg = General.Resource.Common.Failed;
            }
             ResultModel rs = new ResultModel();

            rs.Result = result.ToString();
            rs.Msg = msg;
            rs.guid = partner.BP_GUID;

            return Newtonsoft.Json.JsonConvert.SerializeObject(rs);

            //return string.Format("{{\"Result\":{0},\"Msg\":\"{1}\"}}"
            //    , result.ToString().ToLower(), msg);
        }

        public string RemoveBankInfo(string id)
        {
            bool result = new BankAccountSvc().DelBankAccount(id);
            string msg = string.Empty;
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
        /// <summary>
        /// 删除商业伙伴信息
        /// </summary>
        /// <param name="id">商业伙伴标识</param>
        /// <returns></returns>
        public string DelPartner(string id)
        {
            bool result = new BusinessPartnerSvc().DelPartner(id);
            string msg = string.Empty;
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

        /// <summary>
        /// 获取商业伙伴列表
        /// </summary>
        /// <returns></returns>
        public JsonResult GetPartners()
        {
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            return Json(new BusinessPartnerSvc().GetPartners(C_GUID));
        }
        /// <summary>
        /// 查询公司列表信息
        /// </summary>
        /// <returns></returns>
        public string GetPart(string Customer, string Supplier, string Partner, string BPName)
        {
            List<T_BusinessPartner> recs = new BusinessPartnerSvc().GetPartner(Session["CurrentCompanyGuid"].ToString(), BPName, Customer, Supplier,Partner);
            string json = new JavaScriptSerializer().Serialize(recs);
            return json;
        }
        /// <summary>
        /// 获取公司列表信息
        /// </summary>
        public string GetPartner()
        {
            List<T_BusinessPartner> recs = new BusinessPartnerSvc().GetPartners(Session["CurrentCompanyGuid"].ToString());
            ViewBag.BusinessPartner = recs;
            string json = new JavaScriptSerializer().Serialize(recs);
            return json;
        }

        public struct ResultModel
        {
            public string Result;
            public string Msg;
            public string guid;
        }
        
    }
}
