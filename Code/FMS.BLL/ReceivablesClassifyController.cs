using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using BaseController;
using System.Web.Mvc;
using FMS.Model;
using FMS.DAL;
using System.Web.Script.Serialization;
using Utilitie;
using System.IO;
using System.Drawing;

namespace FMS.BLL
{
    /// <summary>
    /// 收款纪录归档
    /// </summary>
    public class ReceivablesClassifyController : UserController
    {
        public ReceivablesClassifyController()
            : base("Receivables_Classify")
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
        /// 收款纪录信息页
        /// </summary>
        /// <param name="id">收款纪录标识</param>
        /// <returns></returns>
        public ActionResult ReceivablesRecord(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return View(new T_RecPayRecord() { IE_GUID = Guid.NewGuid().ToString() });
            }
            else
            {
                string C_GUID = Session["CurrentCompany"].ToString();
                return View(new RecPayRecordSvc().GetReceivablesRecord(id, C_GUID));
            }
        }

        /// <summary>
        /// 销账
        /// </summary>
        /// <returns></returns>
        public ActionResult CancelIncomeRecord()
        {
            ViewData["C_GUID"] = Session["CurrentCompany"].ToString();
            return View("CancelIncomeRecord");
        }

        public ActionResult CancelRecordOne(string Guid, string Name, string Date, string Money, string Cur)
        {
            ViewData["C_GUID"] = Session["CurrentCompany"].ToString();
            ViewData["GUID"] = Guid;
            ViewData["R_PerName"] = Name;
            ViewData["Date"] = Date;
            ViewData["SumAmount"] = Money;
            ViewData["Currency"] = Cur;
            return View("CancelRecordOne");
        }
        public ActionResult CancelRecordTwo()
        {
            ViewData["C_GUID"] = Session["CurrentCompany"].ToString();
            return View("CancelRecordTwo");
        }

        public ActionResult CancelRecordThree()
        {
            ViewData["C_GUID"] = Session["CurrentCompany"].ToString();
            return View("CancelRecordThree");
        }

        public ActionResult CancelRecordFour()
        {
            ViewData["C_GUID"] = Session["CurrentCompany"].ToString();
            return View("CancelRecordFour");
        }

        public ActionResult CancelRecordFive()
        {
            ViewData["C_GUID"] = Session["CurrentCompany"].ToString();
            return View("CancelRecordFive");
        }
        public ActionResult CancelRecordSix()
        {
            ViewData["C_GUID"] = Session["CurrentCompany"].ToString();
            return View("CancelRecordSix");
        }

        public ActionResult GetReceivablesDeclareCustomer()
        {
            ViewData["C_GUID"] = Session["CurrentCompany"].ToString();
            return View("GetReceivablesDeclareCustomer");
        }

        public ActionResult GetReceivablesDeclareDeposit()
        {
            ViewData["C_GUID"] = Session["CurrentCompany"].ToString();
            return View("GetReceivablesDeclareDeposit");
        }

        public ActionResult GetReceivablesDeclareThree()
        {
            ViewData["C_GUID"] = Session["CurrentCompany"].ToString();
            return View("GetReceivablesDeclareThree");
        }

        public ActionResult GetReceivablesDeclareFour()
        {
            ViewData["C_GUID"] = Session["CurrentCompany"].ToString();
            return View("GetReceivablesDeclareFour");
        }

        public ActionResult GetReceivablesDeclareFive()
        {
            ViewData["C_GUID"] = Session["CurrentCompany"].ToString();
            return View("GetReceivablesDeclareFive");
        }

        /// <summary>
        /// 获取收款纪录列表
        /// </summary>
        /// <returns></returns>
        public string GetReceivablesList(string page, string rows, string dateBegin, string dateEnd, string customer, string incomeGrp, string incomeGrpDts)
        {
            int count = 0;
            string C_GUID = Session["CurrentCompany"].ToString();
            string strFormatter = "{{\"total\":\"{0}\",\"rows\":{1}}}";
            StringBuilder strJson = new StringBuilder();
            List<T_RecPayRecord> RecPayRecord = new List<T_RecPayRecord>();
            RecPayRecord = new RecPayRecordSvc().GetUnclassifyReceivablesRecord(C_GUID, int.Parse(page), int.Parse(rows), out count,
                    dateBegin, dateEnd, customer, incomeGrp, incomeGrpDts);
            strJson.AppendFormat(strFormatter, count, new JavaScriptSerializer().Serialize(RecPayRecord));
            return strJson.ToString();
        }

        public string GetReceivablesDeclareCustomerList(string page, string rows)
        {
            int count = 0;
            string C_GUID = Session["CurrentCompany"].ToString();
            string strFormatter = "{{\"total\":\"{0}\",\"rows\":{1}}}";
            StringBuilder strJson = new StringBuilder();
            List<T_RecPayRecord> RecPayRecord = new List<T_RecPayRecord>();
            RecPayRecord = new RecPayRecordSvc().GetDeclareCustomer("R", C_GUID, int.Parse(page), int.Parse(rows), out count, "收到的其他与经营活动有关的款客户预付、押金返还、暂支还款等;预付客户款");
            strJson.AppendFormat(strFormatter, count, new JavaScriptSerializer().Serialize(RecPayRecord));
            return strJson.ToString();

        }

        public string GetReceivablesSelfList(string page, string rows, string dateBegin, string dateEnd, string customer, string incomeGrp, string incomeGrpDts)
        {
            int count = 0;
            string C_GUID = Session["CurrentCompany"].ToString();
            string strFormatter = "{{\"total\":\"{0}\",\"rows\":{1}}}";
            StringBuilder strJson = new StringBuilder();
            List<T_RecPayRecord> RecPayRecord = new List<T_RecPayRecord>();
            RecPayRecord = new RecPayRecordSvc().GetReceivablesSelfList(C_GUID, int.Parse(page), int.Parse(rows), out count,
                    dateBegin, dateEnd, customer, incomeGrp, incomeGrpDts);
            strJson.AppendFormat(strFormatter, count, new JavaScriptSerializer().Serialize(RecPayRecord));
            return strJson.ToString();
        }

        public string GetReceivablesSelfListTwo(string page, string rows, string dateBegin, string dateEnd, string customer, string incomeGrp, string incomeGrpDts)
        {
            int count = 0;
            string C_GUID = Session["CurrentCompany"].ToString();
            string strFormatter = "{{\"total\":\"{0}\",\"rows\":{1}}}";
            StringBuilder strJson = new StringBuilder();
            List<T_RecPayRecord> RecPayRecord = new List<T_RecPayRecord>();
            RecPayRecord = new RecPayRecordSvc().GetPaymentSelfList(C_GUID, int.Parse(page), int.Parse(rows), out count,
                    dateBegin, dateEnd, customer, incomeGrp, incomeGrpDts);
            strJson.AppendFormat(strFormatter, count, new JavaScriptSerializer().Serialize(RecPayRecord));
            return strJson.ToString();
        }

        public string GetReceivablesRecord(string id)
        {
            string C_GUID = Session["CurrentCompany"].ToString();
            string strFormatter = "{{\"total\":\"{0}\",\"rows\":{1}}}";
            StringBuilder strJson = new StringBuilder();
            List<T_RecPayRecord> RecPayRecord = new List<T_RecPayRecord>();
            RecPayRecord = new RecPayRecordSvc().GetRecPayRecordD(id, C_GUID);
            strJson.AppendFormat(strFormatter, 1, new JavaScriptSerializer().Serialize(RecPayRecord));
            return strJson.ToString();
        }

        /// <summary>
        /// 更新收款纪录
        /// </summary>
        /// <param name="rec">收款纪录对象</param>
        /// <returns></returns>
        public string UpdReceivablesRecord(T_RecPayRecord rec)
        {
            bool result = false;
            string msg = string.Empty;
            rec.Creator = base.userData.LoginFullName;
            rec.C_GUID = Session["CurrentCompany"].ToString();
            DateTime EditThreshold = DateTime.Parse(Session["EditThreshold"].ToString());
            if (rec.Date <= DateTime.Now && rec.Date >= EditThreshold)
            {
                result = new RecPayRecordSvc().UpdReceivablesRecord(rec);
                if (result)
                {
                    msg = General.Resource.Common.Success;
                }
                else
                {
                    msg = General.Resource.Common.Failed;
                }
            }
            else
            {
                result = false;
                msg = FMS.Resource.Finance.Finance.DateError;
            }
            return string.Format("{{\"Result\":{0},\"Msg\":\"{1}\"}}"
                , result.ToString().ToLower(), msg);
        }

        /// <summary>
        /// 归类
        /// </summary>
        /// <param name="id">收款纪录标识</param>
        /// <param name="invtype">归类类型</param>
        /// <returns></returns>
        public string UpdPayInyType(string id, string invtype, string typedts, string typedtsdts, string ieguid, string remark, string cfitemguid)
        {
            typedts = typedts + ";" + typedtsdts;
            bool result = new RecPayRecordSvc().UpdInvType("R", id, invtype, typedts, ieguid, remark, cfitemguid);
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

        public string UpdRecInyType(string id, string invtype, string typedts, string ieguid,string cfitemguid)
        {
            bool result = new RecPayRecordSvc().UpdRR("R", id, ieguid, invtype, typedts, cfitemguid);
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
        /// 更改状态
        /// </summary>
        /// <param name="id">收入纪录标识</param>
        /// <returns></returns>
        public string UpdState(string id)
        {
            bool result = new RecPayRecordSvc().UpdIEState("I", id,"已收");
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
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ieguid"></param>
        /// <returns></returns>
        public string UpdRR(string id, string ieguid)
        {
            bool result = new RecPayRecordSvc().UpdRR("R", id, ieguid, "未归账", null,null);
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
        /// 删除收款纪录
        /// </summary>
        /// <param name="id">收款纪录标识</param>
        /// <returns></returns>
        public string DelReceivablesRecord(string id)
        {
            bool result = new RecPayRecordSvc().DelReceivablesRecord(id);
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
        /// 将收入列表以Excel形式导出
        /// </summary>
        /// <returns></returns>
        public FileResult ExportXls()
        {
            int count = 0;
            List<T_RecPayRecord> ds = new RecPayRecordSvc().GetAllReceivablesRecord(Session["CurrentCompany"].ToString());
            Dictionary<string, string> cfg = new Dictionary<string, string>();
            cfg.Add("Date", "收款日期");
            cfg.Add("R_PerName", "付款方");
            cfg.Add("SumAmount", "总金额");
            cfg.Add("Currency", "货币");
            cfg.Add("InvType", "收款类别");
            cfg.Add("BankAccount", "收款账户");
            cfg.Add("Remark", "备注");

            return File(new GenerateXls().GenXls<T_RecPayRecord>("R",ds, cfg), "application/vnd.ms-excel", "收款列表.xls");
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
        /// 附件信息页
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Attachment(string id)
        {
            return View(new AttachmentSvc().GetAttachmentById(id));
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
    }
}