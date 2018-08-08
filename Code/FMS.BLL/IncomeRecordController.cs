using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Drawing;
using BaseController;
using FMS.DAL;
using FMS.Model;
using System.Text;
using System.Web;
using System.IO;

namespace FMS.BLL
{
    /// <summary>
    /// 记录收入
    /// </summary>
    public class IncomeRecordController : UserController
    {
        public IncomeRecordController()
            : base("Income_Record")
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
            return View();
        }

        /// <summary>
        /// 收入纪录信息页
        /// </summary>
        /// <param name="id">收入纪录标识</param>
        /// <returns></returns>

        public ActionResult IncomeRecord(string id)
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
                string C_GUID = Session["CurrentCompany"].ToString();
                return View(new IESvc().GetIE(id,C_GUID));
            }
        }

        /// <summary>
        /// 收入纪录信息页（只读）
        /// </summary>
        /// <param name="id">收入纪录标识</param>
        /// <returns></returns>
        public ActionResult CheckIncomeRecord(string id)
        {
            return View(new IESvc().GetIE(id, Session["CurrentCompany"].ToString()));
        }

        /// <summary>
        /// 获取收入列表数据
        /// </summary>
        /// <param name="rows">页大小</param>
        /// <param name="page">页索引</param>
        /// <returns></returns>
        public string GetIncomeList(string rows,string page)
        {
            int count = 0;
            string C_GUID = Session["CurrentCompany"].ToString();
            string strFormatter = "{{\"total\":\"{0}\",\"rows\":{1}}}";
            StringBuilder strJson = new StringBuilder();
            List<T_IERecord> IERcord = new List<T_IERecord>();
            IERcord = new IESvc().GetIncomeList(C_GUID,int.Parse(page),int.Parse(rows),out count,string.Empty,string.Empty,string.Empty,string.Empty,string.Empty);
            strJson.AppendFormat(strFormatter, count, new JavaScriptSerializer().Serialize(IERcord));
            return strJson.ToString();
           
        }

        /// <summary>
        /// 更新收入纪录
        /// </summary>
        /// <param name="head">收入主数据</param>
        /// <param name="list">收入明细数据</param>
        /// <returns></returns>
        public string UpdIncomeRecord(T_IERecord form)
        {
            bool result = false;
            string msg = string.Empty;
            form.C_GUID = Session["CurrentCompany"].ToString();
            form.Creator = base.userData.LoginFullName;
            form.CreateDate = DateTime.Now;
            result = new IESvc().UpdIncomeRecord(form);
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
        /// 获取收入纪录明细
        /// </summary>
        /// <param name="id">收入纪录标识</param>
        /// <returns></returns>
        public JsonResult GetIncomeDetails(string id)
        {
            string C_GUID = Session["CurrentCompany"].ToString();
            return Json(new IESvc().GetIEDetails(id,C_GUID));
        }

        /// <summary>
        /// 商业伙伴信息页
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult BusinessPartner()
        {
                return View(new T_BusinessPartner() { BP_GUID = Guid.NewGuid().ToString() });
        }

        public ActionResult GetReceivablesDeclareCustomer()
        {
            ViewData["C_GUID"] = Session["CurrentCompany"].ToString();
            return View("GetReceivablesDeclareCustomer");
        }

        /// <summary>
        /// 更新商业伙伴信息
        /// </summary>
        /// <param name="partner">商业伙伴对象</param>
        /// <returns></returns>
        public string UpdPartner(T_BusinessPartner partner)
        {
            partner.C_GUID = Session["CurrentCompany"].ToString();
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
        /// 删除收入纪录
        /// </summary>
        /// <param name="id">收入纪录标识</param>
        /// <returns></returns>
        public string DelIncomeRecord(string id)
        {
            string msg = string.Empty;
            bool result = new IESvc().DelIERecord(id,"I");
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
        /// 删除记录的所有附件
        /// </summary>
        /// <param name="id">FR_GUID纪录标识</param>
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
                    entity.FileRemark ="";


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
        /// <param name="fileID">fileID 图片ID</param>
        /// <returns></returns>
        public FileResult DownLoadFile(string fileID)
        {
            AttachmentSvc attSv=new AttachmentSvc();
            var entity = attSv.GetAttachmentById(fileID);
            //从数据库查找
            return File(entity.FlieData,entity.FileType,entity.FileName);
        }

    }
}
