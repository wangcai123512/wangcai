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
using Newtonsoft.Json;
using Aspose.Cells;
using Common.Models;

namespace FMS.BLL
{
    /// <summary>
    /// 记录费用
    /// </summary>
    public class ExpenseRecordController : UserController
    {
        public ExpenseRecordController()
            : base("ExpenseRecord")
        { }

        /// <summary>
        /// 列表页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Temporary()
        {
            ViewData["IE_GUID"] = Guid.NewGuid().ToString();
            ViewData["DateNow"] = GetNowDate();
            return View();
        }

        public ActionResult BusinessPartner()
        {
            return View(new T_BusinessPartner() { BP_GUID = Guid.NewGuid().ToString() });
        }

        public ActionResult GetPaymentDeclareCostSpending()
        {
            ViewData["C_GUID"] = Session["CurrentCompanyGuid"].ToString();
            return View("GetPaymentDeclareCostSpending");
        }

        /// <summary>
        /// 删除记录的所有附件
        /// </summary>
        /// <param name="id">收款纪录标识</param>
        /// <returns></returns>
        public string DelAttachment(string id)
        {
            bool result = new AttachmentSvc().DelAttachment(id);
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
        /// 更新商业伙伴信息
        /// </summary>
        /// <param name="partner">商业伙伴对象</param>
        /// <returns></returns>
        public string UpdPartner(T_BusinessPartner partner)
        {
            partner.C_GUID = Session["CurrentCompanyGuid"].ToString();
            bool result = new IESvc().UpdPartner(partner);
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
        public string UpdSupplier(T_BusinessPartner partner)
        {
            partner.C_GUID = Session["CurrentCompanyGuid"].ToString();
            partner.IsSupplier = true;
            bool result = new IESvc().UpdPartner(partner);
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
        /// 上传图片
        /// </summary>
        /// <param name="fileData">上传文件</param>
        /// <param name="guid"></param>
        /// <param name="folder"></param>
        /// <returns></returns>
        //[AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Upload(HttpPostedFileBase fileData, string guid, string folder)
        {
            DelAttachment(guid);
            if (fileData != null)
            {
                try
                {
                    ControllerContext.HttpContext.Request.ContentEncoding = Encoding.GetEncoding("UTF-8");
                    ControllerContext.HttpContext.Response.ContentEncoding = Encoding.GetEncoding("UTF-8");
                    ControllerContext.HttpContext.Response.Charset = "UTF-8";

                    //写入数据流
                    Stream fileStream = fileData.InputStream;
                    byte[] fileDataStream = new byte[fileData.ContentLength];
                    fileStream.Read(fileDataStream, 0, fileData.ContentLength);
                    //写入数据
                    T_Attachment entity = new T_Attachment();
                    entity.A_GUID = Guid.NewGuid().ToString();
                    entity.FileName = fileData.FileName;
                    entity.FileType = fileData.ContentType;
                    entity.FR_GUID = guid;
                    entity.FlieData = fileDataStream;


                    bool rResult = new AttachmentSvc().AddAttachment(entity);
                    return Content(rResult.ToString());
                }
                catch (Exception ex)
                {
                    return Content("false");
                }
            }
            else
            {
                return Content("false");
            }
        }

        /// <summary>
        /// 费用记录页
        /// </summary>
        /// <param name="id">费用标识</param>
        /// <returns></returns>
        public ActionResult ExpenseRecord(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return View(new T_IERecord()
                {
                    IE_GUID = Guid.NewGuid().ToString(),
                });
            }
            else
            {
                string C_GUID = Session["CurrentCompanyGuid"].ToString();
                return View(new IESvc().GetIE(id,C_GUID));
            }
        }

        /// <summary>
        /// 获取费用列表
        /// </summary>
        /// <param name="rows">页大小</param>
        /// <param name="page">页索引</param>
        /// <returns></returns>
        public string GetExpenseList(string rows, string page)
        {
            int count = 0;
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            string strFormatter = "{{\"total\":\"{0}\",\"rows\":{1}}}";
            StringBuilder strJson = new StringBuilder();
            List<T_IERecord> IERcord = new List<T_IERecord>();
            IERcord=new IESvc().GetExpenseList(C_GUID,int.Parse(page),int.Parse(rows),out count);
             strJson.AppendFormat(strFormatter, count, new JavaScriptSerializer().Serialize(IERcord));
             return strJson.ToString();


        }

        /// <summary>
        /// 更新费用数据
        /// </summary>
        /// <param name="head">费用主数据</param>
        /// <param name="list">费用明细数据</param>
        /// <returns></returns>
        public string UpdExpenseRecord(T_IERecord form)
        {
            bool result = false;
            ExceResult res = new ExceResult();
            string msg = string.Empty;
            form.C_GUID = Session["CurrentCompanyGuid"].ToString();
            form.Creator = base.userData.LoginFullName;
            result = new IESvc().UpdExpenseRecord(form);
                if (result)
                {
                    res.success = true;
                    msg = General.Resource.Common.Success;
                }
                else
                {
                    res.success = false;
                    msg = General.Resource.Common.Failed;
                }
                return JsonConvert.SerializeObject(res);
        }

        public String CreateTaxRecord(List<T_IERecord> TaxList) {
            bool result = false;
            ExceResult res = new ExceResult();
            string msg = string.Empty;
            foreach (T_IERecord form in TaxList){
                form.C_GUID = Session["CurrentCompanyGuid"].ToString();
                form.Creator = base.userData.LoginFullName;
                form.Currency = Session["Currency"].ToString();
                if(form.IEGroup =="增值税"){
                    form.IE_Flag = "E";
                }
                
                    result = new IESvc().UpdVoucherFL(form,"NV");
                
                if (result)
                {
                    msg = General.Resource.Common.Success;
                }
                else
                {
                    msg = General.Resource.Common.Failed;
                }
            }

            return string.Format("{{\"Result\":{0},\"Msg\":\"{1}\"}}"
               , result.ToString().ToLower(), msg);
        
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult CheckExpenseRecord(string id)
        {
            return View(new IESvc().GetIE(id, Session["CurrentCompanyGuid"].ToString()));
        }

        /// <summary>
        /// 费用记录页（只读）
        /// </summary>
        /// <param name="id">费用标识</param>
        /// <returns></returns>
        public JsonResult GetExpenseDetails(string id)
        {
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            return Json(new IESvc().GetIEDetails(id,C_GUID));
        }

        /// <summary>
        /// 删除费用记录
        /// </summary>
        /// <param name="id">费用标识</param>
        /// <returns></returns>
        public string DelExpenseRecord(string id)
        {
            string msg = string.Empty;
            bool result = new IESvc().DelIERecord(id,"E");
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
