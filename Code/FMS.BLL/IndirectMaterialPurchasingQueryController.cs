using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using BaseController;
using FMS.DAL;
using FMS.Model;

namespace FMS.BLL
{
    /// <summary>
    /// 查询付款
    /// </summary>
    public class IndirectMaterialPurchasingQueryController:UserController
    {
        public IndirectMaterialPurchasingQueryController()
            : base("IndirectMaterialPurchasing_Query")
        {}

        /// <summary>
        /// 列表页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult IndirectMaterialPurchasingRecord(string id)
        {
            string C_GUID = Session["CurrentCompany"].ToString();
            return View(new AIDSvc().GetAIDRecord(id, C_GUID));
        }

        public ActionResult ExpenseRecord(string id,string date,decimal  amount,string rper,string currency)
        {
            ViewData["C_GUID"] = Session["CurrentCompany"].ToString();
            ViewData["IE_GUID"] = Guid.NewGuid().ToString();
            ViewData["RP_GUID"] = id;
            ViewData["AffirmDate"] = date;
            ViewData["Date"] = date;
            ViewData["Amount"] = amount;
            ViewData["RPer"] = rper;
            ViewData["Currency"] = currency;
            return View("ExpenseRecord");
        }

        public ActionResult IncomeRecord(string id, string date, decimal amount, string rper, string currency)
        {
            ViewData["C_GUID"] = Session["CurrentCompany"].ToString();
            ViewData["IE_GUID"] = Guid.NewGuid().ToString();
            ViewData["RP_GUID"] = id;
            ViewData["AffirmDate"] = date;
            ViewData["Date"] = date;
            ViewData["Amount"] = amount;
            ViewData["RPer"] = rper;
            ViewData["Currency"] = currency;
            return View("IncomeRecord");
        }

        /// <summary>
        /// 附件信息页
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Attachment(string id)
        {
            return View(new AttachmentSvc().GetAttachmentById(id));
        }

        /// <summary>
        /// 间接物料采购纪录列表数据
        /// </summary>
        /// <returns></returns>
        public string GetIndirectMaterialPurchasingList(string page, string rows, string dateBegin, string dateEnd, string customer, string grp, string state = null)
        {
            int count = 0;
            string C_GUID = Session["CurrentCompany"].ToString();
            string strFormatter = "{{\"total\":\"{0}\",\"rows\":{1}}}";
            StringBuilder strJson = new StringBuilder();
            List<T_AIDRecord> Record = new List<T_AIDRecord>();
            Record = new AIDSvc().GetIndirectMaterialPurchasingList(C_GUID, int.Parse(page), int.Parse(rows), out count,
                    dateBegin, dateEnd, customer, grp, state);
            strJson.AppendFormat(strFormatter, count, new JavaScriptSerializer().Serialize(Record));
            return strJson.ToString();
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="fileData">上传文件</param>
        /// <param name="guid"></param>
        /// <param name="folder"></param>
        /// <returns></returns>
        //[AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Upload(HttpPostedFileBase fileData, string guid, string folder)
        {
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
                    entity.FileRemark = "";


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
        /// 获取记录附件
        /// </summary>
        /// <param name="id">FR_GUID 记录标识</param>
        /// <returns></returns>
        public JsonResult GetAttachment(string id)
        {
            return Json(new AttachmentSvc().GetAttachment(id));
        }

        /// <summary>
        /// 下载附件
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public FileResult DownLoadFile(string id)
        {
            AttachmentSvc attSv = new AttachmentSvc();
            var entity = attSv.GetAttachmentById(id);
            //从数据库查找
            return File(entity.FlieData, entity.FileType, entity.FileName);
        }

        /// <summary>
        /// 更新附件备注
        /// </summary>
        /// <param name="attachenmt"></param>
        /// <returns></returns>
        public string UpdAttachment(T_Attachment attachenmt)
        {
            string msg = string.Empty;
            bool result = new AttachmentSvc().UpdAttachment(attachenmt.A_GUID, attachenmt.FileName, attachenmt.FileRemark);
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
        /// 一一删除记录的每个附件
        /// </summary>
        /// <param name="id">A_GUID纪录标识</param>
        /// <returns></returns>
        public string DelEveryAttachment(string id)
        {
            bool result = new AttachmentSvc().DelEveryAttachment(id);
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
        /// 更新间接物品采购纪录
        /// </summary>
        /// <param name="head">收入主数据</param>
        /// <param name="list">收入明细数据</param>
        /// <returns></returns>
        public string UpdIndirectMaterialPurchasingRecord(T_AIDRecord form)
        {
            bool result = false;
            string msg = string.Empty;
            form.C_GUID = Session["CurrentCompany"].ToString();
            result = new AIDSvc().UpdIndirectMaterialPurchasingRecord(form);
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
        /// 更改状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public string UpdIndirectMaterialPurchasingRecordState(string id, string state)
        {
            bool result = new AIDSvc().UpdAIDState("I", id, state);
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
        /// 删除采购记录
        /// </summary>
        /// <param name="id">收款纪录标识</param>
        /// <returns></returns>
        public string DelIndirectMaterialPurchasingRecord(string id)
        {
            bool result = new AIDSvc().DelAIDIERecord(id);
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
    }
}
