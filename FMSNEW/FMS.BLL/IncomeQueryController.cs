using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using BaseController;
using FMS.DAL;
using FMS.Model;
using System.Text;
using System.Linq;
using System;
using System.IO;
using System.Web;
using Utilitie;

namespace FMS.BLL
{
    /// <summary>
    /// 收入查询
    /// </summary>
    public class IncomeQueryController:UserController
    {
        public IncomeQueryController()
            : base("IncomeQuery")
        { }

        /// <summary>
        /// 列表页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index() 
        {
            return View();
        }

        ///// <summary>
        ///// 收入纪录页
        ///// </summary>
        ///// <param name="id">收入纪录标识</param>
        ///// <returns></returns>
        //public ActionResult IncomeRecord(string id)
        //{
        //    return View(new IESvc().GetIE(id, Session["CurrentCompanyGuid"].ToString()));
        //}

        /// <summary>
        /// 获取收入纪录列表数据easyui
        /// </summary>
        /// <param name="page">页索引</param>
        /// <param name="rows">页大小</param>
        /// <returns></returns>
        public string GetIncomeList(string page, string rows, string dateBegin, string dateEnd, string customer, string state, string incomeGrp)
        {
            int count = 0;
            StringBuilder strJson = new StringBuilder();
            string strFormatter = "{{\"total\":\"{0}\",\"rows\":{1}}}";//"{{\"count\":{0},\"rows\":{1}}}"
            List<T_IERecord> recs = new IESvc().GetAllExpenseList(Session["CurrentCompanyGuid"].ToString(),int.Parse(page),int.Parse(rows),out count,
                    dateBegin, dateEnd, customer, state, incomeGrp);
            strJson.AppendFormat(strFormatter,count, new JavaScriptSerializer().Serialize(recs));
            return strJson.ToString();
        }

        /// <summary>
        /// 获取收入纪录列表数据Bootstraptable
        /// </summary>
        /// <param name="page">页索引</param>
        /// <param name="rows">页大小</param>
        /// <returns></returns>
        public string GetIncomeList_Bootstraptable(string dateBegin, string dateEnd, string zdateBegin, string zdateEnd, string customer, string state, string incomeGrp, string currency, string business_GUID, string subBusiness_GUID, string TaxationGUID, string MounthDate)
        {
            int count = 0;
            StringBuilder strJson = new StringBuilder();
            //string strFormatter = "{{\"total\":\"{0}\",\"rows\":{1}}}";//"{{\"count\":{0},\"rows\":{1}}}"
            List<T_IERecord> List = new IESvc().GetAllIncomeList(Session["CurrentCompanyGuid"].ToString(), 1, -1, out count, dateBegin, dateEnd, zdateBegin, zdateEnd, customer, state, incomeGrp, currency, business_GUID, subBusiness_GUID, TaxationGUID, MounthDate);
            string json = new JavaScriptSerializer().Serialize(List);
           
            return json;
        }

        /// <summary>
        /// 获取收入纪录列表数据
        /// </summary>
        /// <param name="page">页索引</param>
        /// <param name="rows">页大小</param>
        /// <returns></returns>
        public string CancelIncomeList(string page, string rows, string dateBegin, string dateEnd, string customer, string state, string incomeGrp)
        {
            int count = 0;
            List<T_IERecord> recs = new IESvc().GetAllExpenseList(Session["CurrentCompanyGuid"].ToString(), int.Parse(page), int.Parse(rows), out count,
                dateBegin, dateEnd, customer, state, incomeGrp);
            return string.Format("{{\"count\":{0},\"rows\":{1}}}",
                count, new JavaScriptSerializer().Serialize(recs));
        }

        public ActionResult IncomeRecord(string id)
        {
                string C_GUID = Session["CurrentCompanyGuid"].ToString();
                return View(new IESvc().GetIE(id,C_GUID));
        }

        public string GetIncomeRecord(string id)
        {
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            T_IERecord rec = new IESvc().GetIE(id, C_GUID);
            string json = new JavaScriptSerializer().Serialize(rec);
            return json;
        }

        public ActionResult Attachment(string id)
        {
            return View(new AttachmentSvc().GetAttachmentById(id));
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
        /// 商业伙伴信息页
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult BusinessPartner()
        {
            return View(new T_BusinessPartner() { BP_GUID = Guid.NewGuid().ToString() });
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
        public string DelIERecord(string id)
        {
            bool result = new IESvc().DelIERecord(id,"I");
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
        /// 更新收入纪录
        /// </summary>
        /// <param name="head">收入主数据</param>
        /// <param name="list">收入明细数据</param>
        /// <returns></returns>
        public string UpdIncomeRecord(T_IERecord form)
        {
            bool result = false;
            string msg = string.Empty;
            form.C_GUID = Session["CurrentCompanyGuid"].ToString();
            form.Creator = base.userData.LoginFullName;
            form.CreateDate =GetNowDate();
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
        /// 获取应收款列表数据
        /// </summary>
        /// <param name="page">页索引</param>
        /// <param name="rows">页大小</param>
        /// <returns></returns>
        public string GetReceivableList(string page, string rows)   
        {
             int count = 0;
             string strFormatter = "{{\"total\":\"{0}\",\"rows\":{1}}}";
             StringBuilder strJson = new StringBuilder();
             List<T_IERecord> recs = new IESvc().GetReceivableList(Session["CurrentCompanyGuid"].ToString(),int.Parse(page), int.Parse(rows), out count);
             strJson.AppendFormat(strFormatter, count, new JavaScriptSerializer().Serialize(recs));
             return strJson.ToString();
        }

        /// <summary>
        /// 获取收入明细数据
        /// </summary>
        /// <param name="id">收入纪录标识</param>
        /// <returns></returns>
        public JsonResult GetIncomeDetails(string id)
        {
            return Json(new IESvc().GetIEDetails(id, Session["CurrentCompanyGuid"].ToString()));
        }

        /// <summary>
        /// 将收入列表以Excel形式导出
        /// </summary>
        /// <returns></returns>
        public FileResult ExportXls(string dateBegin, string dateEnd, string customer, string status, string incomeGrp)
        {
            int count = 0;
            List<T_IERecord> ds = new IESvc().GetAllExpenseList(Session["CurrentCompanyGuid"].ToString(),-1,-1, out count,
                    dateBegin, dateEnd, customer, status, incomeGrp);
            Dictionary<string, string> cfg = new Dictionary<string, string>();
            cfg.Add("AffirmDate", "收入确认日期");
            cfg.Add("RPerName", "客户");
            cfg.Add("InvNo", "发票/业务单据号");
            cfg.Add("SumAmount", "总金额");
            cfg.Add("Currency", "货币");
            cfg.Add("State", "状态");
            cfg.Add("InvType", "收入类别");
            cfg.Add("Date", "到账截止日");
            cfg.Add("Remark", "备注");

            return File(new GenerateXls().GenXls<T_IERecord>("I", ds, cfg), "application/vnd.ms-excel", "收入列表.xls");
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
        public string GetIEVoucher(string rows, string page, string IE_GUID)
        {
            int count = 0;
            List<T_IERecord> List = new IESvc().GetVoucher(1, -1, out count, IE_GUID, "I");
            string json = new JavaScriptSerializer().Serialize(List);
            return json;
        }
    }
}
