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
using Common.Models;
using Newtonsoft.Json;


namespace FMS.BLL
{
    /// <summary>
    /// 付款纪录归档
    /// </summary>
    public class PaymentClassifyController : UserController
    {
        public PaymentClassifyController()
            : base("PaymentClassify")
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
        /// 付款纪录信息页
        /// </summary>
        /// <param name="id">付款纪录标识</param>
        /// <returns></returns>
        public ActionResult PaymentRecord(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return View(new T_RecPayRecord() { IE_GUID = Guid.NewGuid().ToString() });
            }
            else
            {
                string C_GUID = Session["CurrentCompanyGuid"].ToString();
                return View(new RecPayRecordSvc().GetPaymentRecord(id, C_GUID));
            }
        }

        /// <summary>
        /// 销账
        /// </summary>
        /// <returns></returns>
        public ActionResult CancelExpenseRecord()
        {
            ViewData["C_GUID"] = Session["CurrentCompanyGuid"].ToString();
            return View("CancelExpenseRecord");
        }

        public ActionResult CancelRecordOne(string Guid,string Name,string Date,string Money,string Cur)
        {
            ViewData["C_GUID"] = Session["CurrentCompanyGuid"].ToString();
            ViewData["GUID"] = Guid;
            ViewData["R_PerName"] = Name;
            ViewData["Date"] = Date;
            ViewData["SumAmount"] = Money;
            ViewData["Currency"] = Cur;
            return View("CancelRecordOne");
        }
        public ActionResult CancelRecordTwo()
        {
            ViewData["C_GUID"] = Session["CurrentCompanyGuid"].ToString();
            return View("CancelRecordTwo");
        }

        public ActionResult CancelRecordThree()
        {
            ViewData["C_GUID"] = Session["CurrentCompanyGuid"].ToString();
            return View("CancelRecordThree");
        }

        public ActionResult CancelRecordFour()
        {
            ViewData["C_GUID"] = Session["CurrentCompanyGuid"].ToString();
            return View("CancelRecordFour");
        }

        public ActionResult CancelRecordFive()
        {
            ViewData["C_GUID"] = Session["CurrentCompanyGuid"].ToString();
            return View("CancelRecordFive");
        }

        public ActionResult CancelRecordFives()
        {
            ViewData["C_GUID"] = Session["CurrentCompanyGuid"].ToString();
            return View("CancelRecordFives");
        }

        public ActionResult CancelRecordSix()
        {
            ViewData["C_GUID"] = Session["CurrentCompanyGuid"].ToString();
            return View("CancelRecordSix");
        }

        public ActionResult CancelRecordSeven(string Guid, string Name, string Date, string Money, string Cur)
        {
            ViewData["C_GUID"] = Session["CurrentCompanyGuid"].ToString();
            ViewData["GUID"] = Guid;
            ViewData["R_PerName"] = Name;
            ViewData["Date"] = Date;
            ViewData["SumAmount"] = Money;
            ViewData["Currency"] = Cur;
            return View("CancelRecordSeven");
        }

        public ActionResult CancelRecordSevens()
        {
            ViewData["C_GUID"] = Session["CurrentCompanyGuid"].ToString();
            return View("CancelRecordSevens");
        }

        public ActionResult CancelRecordEight()
        {
            ViewData["C_GUID"] = Session["CurrentCompanyGuid"].ToString();
            return View("CancelRecordEight");
        }

        public ActionResult GetPaymentDeclareSupplier()
        {
            ViewData["C_GUID"] = Session["CurrentCompanyGuid"].ToString();
            return View("GetPaymentDeclareSupplier");
        }

        public ActionResult GetPaymentDeclareDeposit()
        {
            ViewData["C_GUID"] = Session["CurrentCompanyGuid"].ToString();
            return View("GetPaymentDeclareDeposit");
        }

        /// <summary>
        /// 更新付款纪录
        /// </summary>
        /// <param name="rec">付款纪录对象</param>
        /// <returns></returns>
        public string UpdPaymentRecord(T_RecPayRecord rec)
        {
            bool result = false;
            string msg = string.Empty;
            rec.Creator = base.userData.LoginFullName;
            rec.C_GUID = Session["CurrentCompanyGuid"].ToString();
            DateTime EditThreshold = DateTime.Parse(Session["EditThreshold"].ToString());
            DateTime checkDate;
            DateTime.TryParse(rec.Date, out checkDate);
            if (checkDate <= DateTime.Now && checkDate >= EditThreshold)
            {
                result = new RecPayRecordSvc().UpdPaymentRecord(rec);

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
        /// 未归档付款纪录列表
        /// </summary>
        /// <param name="pageSize">页大小</param>
        /// <param name="pageIndex">页索引</param>
        /// <returns></returns>
        public string GetPaymentList(string rows, string page, string record, string dateBegin, string dateEnd, string customer, string incomeGrp, string InvTypeDts)
        {
            int count = 0;
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            //string strFormatter = "{{\"total\":\"{0}\",\"rows\":{1}}}";
            //StringBuilder strJson = new StringBuilder();
            List<T_RecPayRecord> RecPayRecord = new List<T_RecPayRecord>();
            RecPayRecord = new RecPayRecordSvc().GetPaymentRecord(C_GUID, 1, -1, out count, record,
                    dateBegin, dateEnd, customer, incomeGrp, InvTypeDts);
            //strJson.AppendFormat(strFormatter, count, new JavaScriptSerializer().Serialize(RecPayRecord));
            return new JavaScriptSerializer().Serialize(RecPayRecord);
        }

        public string GetPaymentDeclareCostSpendingList(string page, string rows)
        {
            int count = 0;
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            string strFormatter = "{{\"total\":\"{0}\",\"rows\":{1}}}";
            StringBuilder strJson = new StringBuilder();
            List<T_RecPayRecord> RecPayRecord = new List<T_RecPayRecord>();
            RecPayRecord = new RecPayRecordSvc().GetPaymentDeclareCostSpending("P", C_GUID, int.Parse(page), int.Parse(rows), out count, "支付的其他与经营活动有关的款预付供应商、支付押金、暂支款等;预付供应商");
            strJson.AppendFormat(strFormatter, count, new JavaScriptSerializer().Serialize(RecPayRecord));
            return strJson.ToString();

        }

        public string GetRPVoucher(string rows, string page, string RP_GUID)
        {
            int count = 0;
            List<T_RecPayRecord> List = new RecPayRecordSvc().GetRPVoucher(1, -1, out count, RP_GUID,"P");
            string json = new JavaScriptSerializer().Serialize(List);
            return json;
        }

        public string GetRPVoucherNew(string rows, string page, string RP_GUID)
        {
            int count = 0;
            List<T_RecPayRecord> List = new RecPayRecordSvc().GetRPVoucherNew(1, -1, out count, RP_GUID, "P");
            string json = new JavaScriptSerializer().Serialize(List);
            return json;
        }


        public string GetRPVoucherByRpID(string RP_GUID)
        {
            List<RPVoucherInfo> List = new RecPayRecordSvc().GetRPVoucherInfo(RP_GUID);
            string json = new JavaScriptSerializer().Serialize(List);
            return json;
        }

        public string GetPaymentSelfList(string page, string rows, string dateBegin, string dateEnd, string customer, string incomeGrp, string incomeGrpDts)
        {
            int count = 0;
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            string strFormatter = "{{\"total\":\"{0}\",\"rows\":{1}}}";
            StringBuilder strJson = new StringBuilder();
            List<T_RecPayRecord> RecPayRecord = new List<T_RecPayRecord>();
            RecPayRecord = new RecPayRecordSvc().GetPaymentSelfListTwo(C_GUID, int.Parse(page), int.Parse(rows), out count,
                    dateBegin, dateEnd, customer, incomeGrp, incomeGrpDts);
            strJson.AppendFormat(strFormatter, count, new JavaScriptSerializer().Serialize(RecPayRecord));
            return strJson.ToString();
        }

        public string GetPaymentRecord(string id)
        {
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            string strFormatter = "{{\"total\":\"{0}\",\"rows\":{1}}}";
            StringBuilder strJson = new StringBuilder();
            List<T_RecPayRecord> RecPayRecord = new List<T_RecPayRecord>();
            RecPayRecord = new RecPayRecordSvc().GetRecPayRecordD(id, C_GUID);
            strJson.AppendFormat(strFormatter, 1, new JavaScriptSerializer().Serialize(RecPayRecord));
            return strJson.ToString();
        }

        public string UpEditPayRecord(T_RecPayRecord rec)
        {
            bool result = false;
            string msg = string.Empty;
            rec.Creator = base.userData.LoginFullName;
            rec.C_GUID = Session["CurrentCompanyGuid"].ToString();
            rec.CreateDate = DateTime.Now.ToString("yyyy-MM-dd");
            DateTime EditThreshold = DateTime.Parse(Session["EditThreshold"].ToString());
            DateTime checkDate;
            DateTime.TryParse(rec.Date, out checkDate);
            if (checkDate <= DateTime.Now && checkDate >= EditThreshold)
            {
                result = new RecPayRecordSvc().UpEditPayRecord(rec);
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

        public string UpRecPayType(string id, string ieguid, string type,Decimal SumAmount)
        {
            bool result = false;
            ExceResult res = new ExceResult();
            string msg = string.Empty;
            T_RecPayRecord rec = new T_RecPayRecord();
             rec.Record = "已销账";
             rec.RP_Flag = "P";
             rec.IE_GUID = ieguid;
             rec.RP_GUID = id;
             rec.SumAmount = SumAmount;
             rec.C_GUID = Session["CurrentCompanyGuid"].ToString();
             switch (type)
             {
                 case "归还短期借款所支付的款":
                    rec.InvType = "筹资活动付款";
                    rec.InvTypeDts = "归还短期借款所支付的款";
                    rec.CFItemGuid = "DD7BCD86-150E-4E62-B6CC-21EF341B41F1";
                     
                    break;
                 case "归还长期借款所支付的款":
                    rec.InvType = "筹资活动付款";
                    rec.InvTypeDts = "归还长期借款所支付的款";
                    rec.CFItemGuid = "DD7BCD86-150E-4E62-B6CC-21EF341B41F1";
                    break;
                case"营业成本":
                    rec.InvType = "经营活动付款";
                    rec.InvTypeDts = "购买商品、接受服务所支付的款";   
                    rec.CFItemGuid = "0526C862-F238-4301-A198-E7EC83A645D5";
                    
                    break;
                case"销售费用":
                    rec.InvType = "经营活动付款";
                    rec.InvTypeDts = "支付销售费用";
                    rec.CFItemGuid = "DE7D81B9-680B-4011-A771-C8B327A549E7";
                    
                    break;
                case"管理费用":
                    rec.InvType = "经营活动付款";
                    rec.InvTypeDts = "支付管理费用";
                    rec.CFItemGuid = "DE7D81B9-680B-4011-A771-C8B327A549E7";
                   
                    break;
                case"财务费用":
                    rec.InvType = "经营活动付款";
                    rec.InvTypeDts = "支付财务费用";
                    rec.CFItemGuid = "DE7D81B9-680B-4011-A771-C8B327A549E7";
                    
                    break;
                case"工资成本":
                    rec.InvType = "经营活动付款";
                    rec.InvTypeDts = "支付职工或为职工支付的款";
                    rec.CFItemGuid = "70765251-FA58-432F-BCC5-122EF3581102";
                    
                    break;
                case"税费":
                    rec.InvType = "经营活动付款";
                    rec.InvTypeDts = "支付的各项税费";
                    rec.CFItemGuid = "E4F16AB4-8DFE-42E1-8A7F-0CB342CF9C73";
                    
                    break;
                case "预付供应商账款":
                      rec.InvType = "经营活动付款";
                      rec.InvTypeDts = "预付供应商账款";
                      rec.CFItemGuid = "DE7D81B9-680B-4011-A771-C8B327A549E7";
                      
                    break;
                case "支付押金":
                      rec.InvType = "经营活动付款";
                      rec.InvTypeDts = "支付押金";
                      rec.CFItemGuid = "DE7D81B9-680B-4011-A771-C8B327A549E7";
                    break;
                case "支付暂支借款":
                    rec.InvType = "经营活动付款";
                    rec.InvTypeDts = "支付暂支借款";
                    rec.CFItemGuid = "DE7D81B9-680B-4011-A771-C8B327A549E7";
                    break;
                case "短期投资支出":
                      rec.InvType = "投资活动付款";
                      rec.InvTypeDts = "短期投资支出";
                      rec.CFItemGuid = "049F1C6D-49EA-4E2D-93FD-2DABEBED666C";
                    break;
                case "长期债券投资支出":
                    rec.InvType = "投资活动付款";
                    rec.InvTypeDts = "长期债券投资支出";
                    rec.CFItemGuid = "049F1C6D-49EA-4E2D-93FD-2DABEBED666C";
                    break;
                case "长期股权投资支出":
                    rec.InvType = "投资活动付款";
                    rec.InvTypeDts = "长期股权投资支出";
                    rec.CFItemGuid = "049F1C6D-49EA-4E2D-93FD-2DABEBED666C";
                    break;
                case "购买固定资产所支付的款":
                    rec.InvType = "投资活动付款";
                    rec.InvTypeDts = "购买固定资产所支付的款";
                    rec.CFItemGuid = "EA46CFA0-5D41-4FF1-9BF8-9A36CB8F1F11";
                    break;
                case "购买无形资产所支付的款":
                    rec.InvType = "投资活动付款";
                    rec.InvTypeDts = "购买无形资产所支付的款";
                    rec.CFItemGuid = "EA46CFA0-5D41-4FF1-9BF8-9A36CB8F1F11";
                    break;
                case "购买其他长期资产所支付的款":
                    rec.InvType = "投资活动付款";
                    rec.InvTypeDts = "购买其他长期资产所支付的款";
                    rec.CFItemGuid = "EA46CFA0-5D41-4FF1-9BF8-9A36CB8F1F11";
                    break;
                case "直接物料采购":
                      rec.InvType = "经营活动付款";
                      rec.InvTypeDts = "购买商品、接受服务所支付的款";
                      rec.CFItemGuid = "0526C862-F238-4301-A198-E7EC83A645D5";
                    break;
                
                 case "间接物料采购":
                      rec.InvType = "经营活动付款";
                      rec.InvTypeDts = "购买商品、接受服务所支付的款";
                      rec.CFItemGuid = "0526C862-F238-4301-A198-E7EC83A645D5";
                    break;
                 case "分配股利、利润":
                      rec.InvType = "筹资活动付款";
                      rec.InvTypeDts = "分配股利、利润的款";
                      rec.CFItemGuid = "5BDE7476-F268-4A62-8CC3-D426D51E253D";
                    break;
                     
                      default:
                        break;

            
            }
             //if (type == "税费") {
             //    result = new RecPayRecordSvc().UpdRecpayType(rec);
             //}
            
                 result = new RecPayRecordSvc().UpRecPayType(rec);
                
                     
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
        /// <summary>
        /// 计入收入
        /// </summary>
        /// <returns></returns>
        

       
        /// <summary>
        /// 归类
        /// </summary>
        /// <param name="id">付款纪录标识</param>
        /// <param name="invtype">归类类型</param>
        /// <returns></returns>
        public string UpdPayInyType(string id, string invtype,string typedts,string cfitemguid,string ieguid,string record)
        {
            //typedts = typedts + ";" + typedtsdts;
            bool result = new RecPayRecordSvc().UpdInvType("P", id, invtype, typedts, cfitemguid, ieguid, record);
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

        public string UpdInyType(string id, string invtype, string typedts, string cfitemguid,string ieguid,string record)
        {
            bool result = new RecPayRecordSvc().UpdInvType("P", id, invtype, typedts,cfitemguid,ieguid,record);
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
            bool result = new RecPayRecordSvc().UpdIEState("E", id,"已付");
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
        public string UpdPR(string id, string ieguid)
        {
            bool result = new RecPayRecordSvc().UpdRR("P", id, ieguid, "经营活动付款", "销售商品/提供服务的收款", "97B181C8-D807-4BF0-8D8D-B23273E7FEFE");
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
        /// 删除付款纪录
        /// </summary>
        /// <param name="id">付款纪录标识</param>
        /// <returns></returns>
        public string DelPaymentRecord(string id)
        {
            bool result = new RecPayRecordSvc().DelPaymentRecord(id);
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
            //int count = 0;
            List<T_RecPayRecord> ds = new RecPayRecordSvc().GetAllPaymentRecord(Session["CurrentCompanyGuid"].ToString());
            Dictionary<string, string> cfg = new Dictionary<string, string>();
            cfg.Add("Date", "付款日期");
            cfg.Add("R_PerName", "收款方");
            cfg.Add("SumAmount", "总金额");
            cfg.Add("Currency", "货币");
            cfg.Add("InvType", "付款类别");
            cfg.Add("BankAccount", "付款账户");
            cfg.Add("Remark", "备注");

            return File(new GenerateXls().GenXls<T_RecPayRecord>("P",ds, cfg), "application/vnd.ms-excel", "付款列表.xls");
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

        public string GetAIDList(string page, string rows, string dateBegin, string dateEnd, string aidFlag)
        {
            int count = 0;
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            string strFormatter = "{{\"total\":\"{0}\",\"rows\":{1}}}";
            StringBuilder strJson = new StringBuilder();
            List<T_AIDRecord> AIDRecord = new List<T_AIDRecord>();
            AIDRecord = new AIDSvc().GetList(int.Parse(page), int.Parse(rows), out count, C_GUID,dateBegin, dateEnd,aidFlag);
            strJson.AppendFormat(strFormatter, count, new JavaScriptSerializer().Serialize(AIDRecord));
            return strJson.ToString();
        }
     }
   }