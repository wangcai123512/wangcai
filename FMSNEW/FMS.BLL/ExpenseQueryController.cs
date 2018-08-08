using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseController;
using System.Web.Mvc;
using FMS.DAL;
using FMS.Model;
using System.Web.Script.Serialization;
using System.Web;
using System.IO;
using Utilitie;
using System.Data;
using Common;
namespace FMS.BLL
{
    /// <summary>
    /// 查询费用
    /// </summary>
    public class ExpenseQueryController : UserController
    {
        public ExpenseQueryController()
            : base("ExpenseQuery")
        {}

        /// <summary>
        /// 列表页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult TaxQuery()
        {
            return View();
        }
        /// <summary>
        /// 费用记录页
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ExpenseRecord(string id)
        {
            return View(new IESvc().GetIE(id, Session["CurrentCompanyGuid"].ToString()));
        }

        public string GetExpenseRecord(string id)
        {
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            T_IERecord rec = new IESvc().GetIE(id, C_GUID);
            string json = new JavaScriptSerializer().Serialize(rec);
            return json;
        }

        public string GetVoucherID(string id)
        {
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            T_IERecord rec = new IESvc().GetVoucherID(id, C_GUID);
            string json = new JavaScriptSerializer().Serialize(rec);
            return json;
        }

        public string GetIEVoucher(string rows, string page, string IE_GUID)
        {
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            int count = 0;
            List<T_IERecord> List = new IESvc().GetVoucher(1, -1, out count, IE_GUID, "E");
            string json = new JavaScriptSerializer().Serialize(List);
            return json;
        }

        public string GetEmployeeSalaryAccount(string strDate,string strType="1")
        { 
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            DateTime dtTime=DateTime.Parse(strDate);
            dtTime = dtTime.AddDays(-dtTime.Day).AddDays(1);
            List<T_Voucher> lstSalary = new IESvc().GetSalaryAccountInfo(dtTime.ToString("yyyy/MM/dd"), C_GUID, strType);
            string json = new JavaScriptSerializer().Serialize(lstSalary);
            return json;
        }
      
        public String GetIERecord(string rows, string page, string id)
        {
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            int count = 0;
            List<T_IERecord> DC = new List<T_IERecord>();
            DC = new IESvc().GetIERecord(1, -1,out count, id, C_GUID);
            return new JavaScriptSerializer().Serialize(DC);  
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
        /// 更新费用纪录
        /// </summary>
        /// <param name="head">费用主数据</param>
        /// <param name="list">费用明细数据</param>
        /// <returns></returns>
        public string UpdExpenseRecord(T_IERecord form)
        {
            bool result = false;
            string msg = string.Empty;
            form.C_GUID = Session["CurrentCompanyGuid"].ToString();
            form.Creator = base.userData.LoginFullName;
            result = new IESvc().UpdExpenseRecord(form);
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
        /// 删除收入费用记录
        /// </summary>
        /// <param name="id">收款纪录标识</param>
        /// <returns></returns>
        public string DelExpenseRecord(string id)
        {
            bool result = new IESvc().DelIERecord(id, "E");
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
        /// 获取费用列表数据
        /// </summary>
        /// <param name="page">页索引</param>
        /// <param name="rows">页大小</param>
        /// <returns></returns>
        //public string GetExpenseList(string page, string rows, string dateBegin, string dateEnd, string customer, string state, string incomeGrp, string IncomeGrpDts)
        //{
        //    int count = 0;
        //    StringBuilder strJson = new StringBuilder();
        //    string strFormatter = "{{\"total\":\"{0}\",\"rows\":{1}}}";
        //    List<T_IERecord> recs = new IESvc().GetAllExpenseList(Session["CurrentCompanyGuid"].ToString(), int.Parse(page), int.Parse(rows), out count, dateBegin, dateEnd, customer, state, incomeGrp, IncomeGrpDts);
        //    strJson.AppendFormat(strFormatter, count, new JavaScriptSerializer().Serialize(recs));
        //    return strJson.ToString();
        //}

        public string GetExpenseList_Bootstraptable(string rows, string page, string dateBegin, string dateEnd, string zdateBegin, string zdateEnd, string customer, string currency, string state, string incomeGrp, string ieGroup, string business_GUID, string subBusiness_GUID, string TaxationGUID, string MounthDate,string taxName)
        {
            int count = 0;
            StringBuilder strJson = new StringBuilder();
            //string strFormatter = "{{\"total\":\"{0}\",\"rows\":{1}}}";
            List<T_IERecord> List = new IESvc().GetAllExpenseList(Session["CurrentCompanyGuid"].ToString(), 1, -1, out count, dateBegin, dateEnd, zdateBegin, zdateEnd, customer, currency, state, incomeGrp, ieGroup, business_GUID, subBusiness_GUID, TaxationGUID, MounthDate,taxName);
            string json = new JavaScriptSerializer().Serialize(List);
            return json;
        }

        /// <summary>
        /// 获取费用明细数据
        /// </summary>
        /// <param name="id">费用标识</param>
        /// <returns></returns>
        public JsonResult GetExpenseDetails(string id)
        {
            return Json(new IESvc().GetIEDetails(id, Session["CurrentCompanyGuid"].ToString()));
        }

        /// <summary>
        /// 将收入列表以Excel形式导出
        /// </summary>
        /// <returns></returns>
        //public FileResult ExportXls(string dateBegin, string dateEnd, string customer, string status, string incomeGrp,string ieGroup)
        //{
        //    int count = 0;
        //    List<T_IERecord> ds = new IESvc().GetAllExpenseList(Session["CurrentCompanyGuid"].ToString(), -1, -1, out count,
        //            dateBegin, dateEnd, customer, status, incomeGrp,ieGroup);
        //    Dictionary<string, string> cfg = new Dictionary<string, string>();
        //    cfg.Add("AffirmDate", "费用确认日期");
        //    cfg.Add("RPerName", "供应商");
        //    cfg.Add("InvNo", "发票/业务单据号");
        //    cfg.Add("SumAmount", "总金额");
        //    cfg.Add("Currency", "货币");
        //    cfg.Add("State", "状态");
        //    cfg.Add("InvType", "费用类别");
        //    cfg.Add("Date", "付款截止日");
        //    cfg.Add("Remark", "备注");

        //    return File(new GenerateXls().GenXls<T_IERecord>("E",ds, cfg), "application/vnd.ms-excel", "费用列表.xls");
        //}

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
        /// 更新附件备注
        /// </summary>
        /// <param name="attachenmt"></param>
        /// <returns></returns>
        public string UpdAttachment(T_Attachment attachenmt)
        {
            string msg = string.Empty;
            bool result = new AttachmentSvc().UpdAttachment(attachenmt.A_GUID, attachenmt.FileName,attachenmt.FileRemark);
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
