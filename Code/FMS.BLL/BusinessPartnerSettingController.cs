using System;
using System.Linq;
using System.Web.Mvc;
using BaseController;
using FMS.DAL;
using FMS.Model;

namespace FMS.BLL
{
    /// <summary>
    /// 设置商业伙伴
    /// </summary>
    public class BusinessPartnerSettingController : UserController
    {
        public BusinessPartnerSettingController()
            : base("Business_Partner_Setting")
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
            if (string.IsNullOrEmpty(id))
            {
                return View(new T_BusinessPartner() { BP_GUID = Guid.NewGuid().ToString() });
            }
            else
            {
                string C_GUID = Session["CurrentCompany"].ToString();
                return View(new BusinessPartnerSvc().GetPartners(C_GUID,id).FirstOrDefault());
            }
        }

        /// <summary>
        /// 更新商业伙伴信息
        /// </summary>
        /// <param name="partner">商业伙伴对象</param>
        /// <returns></returns>
        public string UpdPartner(T_BusinessPartner partner)
        {
            partner.C_GUID = Session["CurrentCompany"].ToString();
            bool result = new BusinessPartnerSvc().UpdPartner(partner);
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
            string C_GUID = Session["CurrentCompany"].ToString();
            return Json(new BusinessPartnerSvc().GetPartners(C_GUID));
        }
    }
}
