using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BaseController;
using FMS.DAL;
using FMS.Model;

namespace FMS.BLL
{
    /// <summary>
    /// 设置商业伙伴
    /// </summary>
    public class CompanyInformationSetController : UserController
    {
        public CompanyInformationSetController()
            : base("CompanyInformation_Set")
        { }
        /// <summary>
        /// 列表页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewData["C_GUID"] = Session["CurrentCompany"].ToString();
            return View(new CompanySvc().GetCompanyInformation(Session["CurrentCompany"].ToString()).FirstOrDefault());
        }

        /// <summary>
        /// 更新商业伙伴信息
        /// </summary>
        /// <param name="Company">商业伙伴对象</param>
        /// <returns></returns>
        public string UpdCompanyInformation(T_Company information)
        {
            bool result = new CompanySvc().UpdCompanyInformation(information);
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

        public string UpdLOGO(string path)
        {
            bool result = new CompanySvc().UpdLOGO(Session["CurrentCompany"].ToString(),path);
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

        public string UpdBusinessLicense(string path)
        {
            bool result = new CompanySvc().UpdBusinessLicense(Session["CurrentCompany"].ToString(), path);
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

        public ActionResult UploadifyFun(HttpPostedFileBase Filedata)
        {
            if (Filedata == null ||
                String.IsNullOrEmpty(Filedata.FileName) ||
                Filedata.ContentLength == 0)
            {
                return this.HttpNotFound();
            }

            string filename = System.IO.Path.GetFileName(Filedata.FileName);
            string fn = Guid.NewGuid().ToString() + "-" + filename;
            string LOGO = "../UploadFile/" + fn;
            UpdLOGO(LOGO);
            string virtualPath = String.Format("~/UploadFile/{0}", fn);

            string path = this.Server.MapPath(virtualPath);
            Filedata.SaveAs(path);
            return this.Json(new object { });
        }

        public ActionResult UploadifyFun1(HttpPostedFileBase Filedata)
        {
            if (Filedata == null ||
                String.IsNullOrEmpty(Filedata.FileName) ||
                Filedata.ContentLength == 0)
            {
                return this.HttpNotFound();
            }
            string filename = System.IO.Path.GetFileName(Filedata.FileName);
            string fn = Guid.NewGuid().ToString() + "-" + filename;
            string BusinessLicense = "../UploadFile/" + fn;
            UpdBusinessLicense(BusinessLicense);
            string virtualPath = String.Format("~/UploadFile/{0}", fn);

            string path = this.Server.MapPath(virtualPath);
            Filedata.SaveAs(path);
            return this.Json(new object { });
        }
    }
}
