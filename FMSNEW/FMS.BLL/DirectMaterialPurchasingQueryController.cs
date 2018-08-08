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
    public class DirectMaterialPurchasingQueryController:UserController
    {
        public DirectMaterialPurchasingQueryController()
            : base("DirectMaterialPurchasingQuery")
        {}

        /// <summary>
        /// 列表页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DirectMaterialPurchasingRecord(string id)
        {
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            return View(new AIDSvc().GetAIDRecord(id, C_GUID));
        }

        public ActionResult ExpenseRecord(string id, string date, decimal amount, string rper, string currency)
        {
            ViewData["C_GUID"] = Session["CurrentCompanyGuid"].ToString();
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
            ViewData["C_GUID"] = Session["CurrentCompanyGuid"].ToString();
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
        /// 付款纪录列表数据
        /// </summary>
        /// <returns></returns>
        public string GetDirectMaterialPurchasingList(string dateBegin, string dateEnd, string customer, string Type, string TypeSub,string MaterielManage, string state = null, int pageIndex = 1, int pageSize = 10, string business_GUID=null, string subBusiness_GUID=null)
        {
            int count = 0;
           
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            List<T_AIDRecord> Record = new List<T_AIDRecord>();
            Record = new AIDSvc().GetDirectMaterialPurchasingList(C_GUID, pageIndex, -1, out count,
                    dateBegin, dateEnd, customer, Type, TypeSub,MaterielManage, state, business_GUID, subBusiness_GUID);
            return new JavaScriptSerializer().Serialize(Record);
        }
        ///<summary>
        ///11.16 hudy
        ///获取已转售列表数据
        /// </summary>
        public string GetDirectMaterialResaleValuePurchasingList(string id, string dateBegin, string dateEnd, string customer, string grp, string state = null, int pageIndex = 1, int pageSize = 10)
        {
            int count = 0;

            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            List<T_IERecord> Record = new List<T_IERecord>();
            Record = new IESvc().GetDirectMaterialResaleValuePurchasingList(id, C_GUID, pageIndex, -1, out count,
                    dateBegin, dateEnd, customer, grp, state);
            return new JavaScriptSerializer().Serialize(Record);
        }
        /// <summary>
        /// 资产信息
        /// </summary>
        /// <param name="id">GUID</param>
        /// <returns></returns>
        public string GetAIDRecord(string id)
        {
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            StringBuilder strJson = new StringBuilder();
            T_AIDRecord record = new T_AIDRecord();
            record = new AIDSvc().GetAIDRecord(id, C_GUID);
            strJson.Append(new JavaScriptSerializer().Serialize(record));
            return strJson.ToString();
        }
        /// <summary>
        /// 记录营业外收入的资产信息
        /// </summary>
        /// <param">hdy.16.11.7</param>
        /// <returns></returns>
        public string GetAIDRecordBusinessPartner(string id)
        {
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            StringBuilder strJson = new StringBuilder();
            T_AIDRecord record = new T_AIDRecord();
            record = new AIDSvc().GetAIDRecordBusinessPartner(id, C_GUID);
            strJson.Append(new JavaScriptSerializer().Serialize(record));
            return strJson.ToString();

        }
        /// <summary>
        /// 附件列表数据
        /// </summary>
        /// <returns></returns>
        public string GetAttachList(string id)
        {
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            List<T_Attachment> Record = new List<T_Attachment>();
            Record = new AttachmentSvc().GetAttachment(id);
            return new JavaScriptSerializer().Serialize(Record);
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
        /// 更新费用纪录(转售)
        /// </summary>
        /// <param name="head">费用主数据</param>
        /// <param name="list">费用明细数据</param>
        /// <returns></returns>
        public string UpdResaleExpenseRecord(T_AIDRecord form)
        {
            bool result = false;
            string msg = string.Empty;
            form.C_GUID = Session["CurrentCompanyGuid"].ToString();
            //当前用户 
            form.Creator = base.userData.LoginFullName;
            result = new AIDSvc().UpdResaleDirectMaterialPurchasingRecord(form);
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



     
       /// 
       /// 
       ///
        /// <summary>
        /// 更改状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public string UpdDirectMaterialPurchasingRecordState(string id, string state)
        {
            bool result = new AIDSvc().UpdAIDState(id, state);
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
        /// 获取记录附件
        /// </summary>
        /// <param name="id">FR_GUID 记录标识</param>
        /// <returns></returns>
        public JsonResult GetAttachment(string id)
        {
            return Json(new AttachmentSvc().GetAttachment(id));
        }

        //删除图片文件
        public void DelPic(string id)
        {
            System.IO.File.Delete(System.Web.HttpContext.Current.Request.MapPath("/")+"img/temp/" + id);
        }

        /// <summary>
        /// 下载附件
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string DownLoadFile(string id)
        {
            try
            {
                AttachmentSvc attSv = new AttachmentSvc();
                List<T_Attachment> ent = attSv.GetAttachment(id);
                var entity = ent[0];
                //从数据库查找
                System.IO.File.WriteAllBytes(System.Web.HttpContext.Current.Request.MapPath("/")+"img/temp/" + id + entity.FileName, entity.FlieData);
                return (id + entity.FileName).ToString();
            }
            catch
            {
                return "";
            }
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
        /// 一一删除财务记录下的附件
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
        /// 更新直接物品采购纪录
        /// </summary>
        /// <param name="head">收入主数据</param>
        /// <param name="list">收入明细数据</param>
        /// <returns></returns>
        public string UpdDirectMaterialPurchasingRecord(T_AIDRecord form)
        {
            bool result = false;
            string msg = string.Empty;
            form.C_GUID = Session["CurrentCompanyGuid"].ToString();
           result = new AIDSvc().UpdDirectMaterialPurchasingRecord(form);    
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
        public string DelDirectMaterialPurchasingRecord(string id)
        {
            var result =AIDSvc.DelAIDIERecord(id);
            string msg = string.Empty;
            if (result == 1)
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
