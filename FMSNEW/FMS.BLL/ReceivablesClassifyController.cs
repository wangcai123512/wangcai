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
    /// 收款纪录归档
    /// </summary>
    public class ReceivablesClassifyController : UserController
    {
        public ReceivablesClassifyController()
            : base("ReceivablesClassify")
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
                string C_GUID = Session["CurrentCompanyGuid"].ToString();
                return View(new RecPayRecordSvc().GetReceivablesRecord(id, C_GUID));
            }
        }

        /// <summary>
        /// 销账
        /// </summary>
        /// <returns></returns>
        public ActionResult CancelIncomeRecord()
        {
            ViewData["C_GUID"] = Session["CurrentCompanyGuid"].ToString();
            return View("CancelIncomeRecord");
        }

        public ActionResult CancelRecordOne(string Guid, string Name, string Date, string Money, string Cur)
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
        public ActionResult CancelRecordSix()
        {
            ViewData["C_GUID"] = Session["CurrentCompanyGuid"].ToString();
            return View("CancelRecordSix");
        }

        public ActionResult GetReceivablesDeclareCustomer()
        {
            ViewData["C_GUID"] = Session["CurrentCompanyGuid"].ToString();
            return View("GetReceivablesDeclareCustomer");
        }

        public ActionResult GetReceivablesDeclareDeposit()
        {
            ViewData["C_GUID"] = Session["CurrentCompanyGuid"].ToString();
            return View("GetReceivablesDeclareDeposit");
        }

        public ActionResult GetReceivablesDeclareThree()
        {
            ViewData["C_GUID"] = Session["CurrentCompanyGuid"].ToString();
            return View("GetReceivablesDeclareThree");
        }

        public ActionResult GetReceivablesDeclareFour()
        {
            ViewData["C_GUID"] = Session["CurrentCompanyGuid"].ToString();
            return View("GetReceivablesDeclareFour");
        }

        public ActionResult GetReceivablesDeclareFive()
        {
            ViewData["C_GUID"] = Session["CurrentCompanyGuid"].ToString();
            return View("GetReceivablesDeclareFive");
        }
        public string UpRecPayType(string id, string ieguid, string type, Decimal Amount, string detailType)
        {
            bool result = false;
            ExceResult res = new ExceResult();
            string msg = string.Empty;
            T_RecPayRecord rec = new T_RecPayRecord();
            rec.Record = "已销账";
            rec.RP_Flag = "R";
            rec.IE_GUID = ieguid;
            rec.RP_GUID = id;
            rec.SumAmount = Amount;
            switch (type)
            {
                case "主营业务收入":
                    rec.InvType = "经营活动收款";
                    rec.InvTypeDts = "销售商品/提供服务的收款";
                    rec.CFItemGuid = "97B181C8-D807-4BF0-8D8D-B23273E7FEFE";
                    break;
               
                case "营业外收入":
                    rec.InvType = "经营活动收款";
                    rec.InvTypeDts = "营业外收入的收款";
                    rec.CFItemGuid = "E90ABB77-27D2-48D7-9A20-6F8862F0BE11";
                    break;

                case "其他业务收入":
                    rec.InvType = "经营活动收款";
                    rec.InvTypeDts = "其他业务收入的收款";
                    rec.CFItemGuid = "E90ABB77-27D2-48D7-9A20-6F8862F0BE11";
                    break;
                case "投资收益 ":
                    switch (detailType)
                    { 
                        case "利息":
                            rec.InvType = "投资活动收款";
                            rec.InvTypeDts = "取得投资收益的利息的收款";
                            rec.CFItemGuid = "C55B2A4E-129B-407B-AC0B-14C091587D45";
                            break;
                        case "股利":
                            rec.InvType = "投资活动收款";
                            rec.InvTypeDts = "取得投资收益的股利的收款";
                            rec.CFItemGuid = "C55B2A4E-129B-407B-AC0B-14C091587D45";
                            break;
                        default:
                            break;
                    }
                    break;

                case "收回公司支出的暂支借款":
                    rec.InvType = "经营活动收款";
                    rec.InvTypeDts = "收回公司支出的暂支借款";
                    rec.CFItemGuid = "F6330595-F588-46B0-8998-752C7A1D774B";
                    break;
                case "预收客户账款":
                    rec.InvType = "经营活动收款";
                    rec.InvTypeDts = "预收客户账款";
                    rec.CFItemGuid = "F6330595-F588-46B0-8998-752C7A1D774B";
                    break;
                case "收回公司支出的押金":
                    rec.InvType = "经营活动收款";
                    rec.InvTypeDts = "收回公司支出的押金";
                    rec.CFItemGuid = "97B181C8-D807-4BF0-8D8D-B23273E7FEFE";
                    break;
                case "收到的其他公司支付的押金":
                    rec.InvType = "经营活动收款";
                    rec.InvTypeDts = "收到的其它公司支付的押金";
                    rec.CFItemGuid = "97B181C8-D807-4BF0-8D8D-B23273E7FEFE";
                    break;
                case "收取投资款(注册资本金额以内部分)":
                    rec.InvType = "筹资活动收款";
                    rec.InvTypeDts = "吸收投资的收款(注册资本金额以内部分)";
                    rec.CFItemGuid = "77A24D5F-3E0C-4211-A552-191FEE0E06FD";
                    break;
                case "收取投资款(超出注册资本金额部分)":
                    rec.InvType = "筹资活动收款";
                    rec.InvTypeDts = "吸收投资的收款(超出注册资本金额部分)";
                    rec.CFItemGuid = "77A24D5F-3E0C-4211-A552-191FEE0E06FD";
                    break;
                case "短期借款所获得的收款":
                    rec.InvType = "筹资活动收款";
                    rec.InvTypeDts = "短期借款所获得的收款";
                    rec.CFItemGuid = "AD2E5437-0917-43E1-807C-41CA6751360F";
                    break;
                case "长期借款所获得的收款":
                    rec.InvType = "筹资活动收款";
                    rec.InvTypeDts = "长期借款所获得的收款";
                    rec.CFItemGuid = "AD2E5437-0917-43E1-807C-41CA6751360F";
                    break;
                case "收回短期投资的本金金额内的款":
                    rec.InvType = "投资活动收款";
                    rec.InvTypeDts = "收回短期投资的本金金额内的款";
                    rec.CFItemGuid = "496F9D4D-F71B-437A-9EA0-26107D3449C3";
                    break;
                case "收回长期债券投资的本金金额内的款":
                    rec.InvType = "投资活动收款";
                    rec.InvTypeDts = "收回长期债券投资的本金金额内的款";
                    rec.CFItemGuid = "496F9D4D-F71B-437A-9EA0-26107D3449C3";
                    break;
                case "收回长期股权投资的本金金额内的款":
                    rec.InvType = "投资活动收款";
                    rec.InvTypeDts = "收回长期股权投资的本金金额内的款";
                    rec.CFItemGuid = "496F9D4D-F71B-437A-9EA0-26107D3449C3";
                    break;
                default:
                    break;


            }

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
        /// 归类
        /// </summary>
        /// <param name="id">付款纪录标识</param>
        /// <param name="invtype">归类类型</param>
        /// <returns></returns>
        public string UpdPayInyType(string id, string invtype, string typedts, string cfitemguid,string ieguid, string record)
        {
            //typedts = typedts + ";" + typedtsdts;
          
            switch (typedts)
            {
                case "销售商品/提供服务的收款":
                    cfitemguid = "97B181C8-D807-4BF0-8D8D-B23273E7FEFE";
                    break;
                case "押金返还、暂支还款":
                    cfitemguid = "F6330595-F588-46B0-8998-752C7A1D774B";
                    break;
                case "收到经营活动有关的客户预付款":
                    cfitemguid = "F6330595-F588-46B0-8998-752C7A1D774B";
                    break;
                case "收到的税费返还":
                    cfitemguid = "0526C862-F238-4301-A198-E7EC83A6445B";
                    break;
                case "收回投资所收到的款":
                    cfitemguid = "2B86C862-F238-4301-A198-E7EC83A645D5";
                    break;
                case "取得投资收益的收款":
                    cfitemguid = "0526C862-F238-4301-A198-E7EC83A66b8C";
                    break;
                case "处置固定资产、无形资产和其他长期资产所收回的款":
                    cfitemguid = "0526C862-F238-4301-A198-E7EC83A645D5";
                    break;
                case "吸取投资的收款":
                    cfitemguid = "77A24D5F-3E0C-4211-A552-191FEE0E06FD";
                    break;
                case "借款所获得的收款":
                    cfitemguid = "AD2E5437-0917-43E1-807C-41CA6751360F";
                    break;
                case "其他与筹资活动有关的收款":
                    cfitemguid = "106B9F2C-24A5-48B5-9621-418D00A7A75A";
                    break;
                case "":
                    break;

            }

            bool result = new RecPayRecordSvc().UpdInvType("R", id, invtype, typedts, cfitemguid, ieguid, record);
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
        /// 获取收款纪录列表
        /// </summary>
        /// <returns></returns>
        public string GetReceivablesList(string rows, string page, string record, string dateBegin, string dateEnd, string customer, string incomeGrp, string InvTypeDts)
        {
            int count = 0;
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            //string strFormatter = "{{\"total\":\"{0}\",\"rows\":{1}}}";
            StringBuilder strJson = new StringBuilder();
            List<T_RecPayRecord> RecPayRecord = new List<T_RecPayRecord>();
            RecPayRecord = new RecPayRecordSvc().GetReceivablesRecord(C_GUID, 1, -1, out count, record,
                    dateBegin, dateEnd, customer, incomeGrp, InvTypeDts);
            //strJson.AppendFormat(strFormatter, count, new JavaScriptSerializer().Serialize(RecPayRecord));
            //return strJson.ToString();
            string json = new JavaScriptSerializer().Serialize(RecPayRecord);
            return json;
        }

        public string GetReceivablesDeclareCustomerList(string page, string rows)
        {
            int count = 0;
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
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
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
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
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            string strFormatter = "{{\"total\":\"{0}\",\"rows\":{1}}}";
            StringBuilder strJson = new StringBuilder();
            List<T_RecPayRecord> RecPayRecord = new List<T_RecPayRecord>();
            RecPayRecord = new RecPayRecordSvc().GetPaymentSelfList(C_GUID, int.Parse(page), int.Parse(rows), out count,
                    dateBegin, dateEnd, customer, incomeGrp, incomeGrpDts);
            strJson.AppendFormat(strFormatter, count, new JavaScriptSerializer().Serialize(RecPayRecord));
            return strJson.ToString();
        }

        public string GetVoucherInfoByRpID(string RP_GUID)
        {
            List<RPVoucherInfo> List = new RecPayRecordSvc().GetVoucherInfoByRPID(RP_GUID);
            string json = new JavaScriptSerializer().Serialize(List);
            return json;
        }

        public string GetReceivablesRecord(string id)
        {
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            //string strFormatter = "{{\"total\":\"{0}\",\"rows\":{1}}}";
            StringBuilder strJson = new StringBuilder();
            List<T_RecPayRecord> RecPayRecord = new List<T_RecPayRecord>();
            RecPayRecord = new RecPayRecordSvc().GetRecPayRecordD(id, C_GUID);
            //strJson.AppendFormat(strFormatter, 1, new JavaScriptSerializer().Serialize(RecPayRecord));
            //return strJson.ToString();

            string json = new JavaScriptSerializer().Serialize(RecPayRecord);
            return json;
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
            rec.C_GUID = Session["CurrentCompanyGuid"].ToString();
            rec.CreateDate = DateTime.Now.ToString("yyyy-MM-dd");
            DateTime EditThreshold = DateTime.Parse(Session["EditThreshold"].ToString());
            DateTime checkDate;
            DateTime.TryParse(rec.Date, out checkDate);
            if (checkDate <= DateTime.Now && checkDate >= EditThreshold)
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
        /// 更新收款纪录
        /// </summary>
        /// <param name="rec">收款纪录对象</param>
        /// <returns></returns>
        public string UpEditRecPayRecord(T_RecPayRecord rec)
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
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ieguid"></param>
        /// <returns></returns>
        public string UpdRR1(string id, string ieguid)
        {
            bool result = new RecPayRecordSvc().UpdRR("R", id, ieguid, "未归账", null, null);
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
            //int count = 0;
            List<T_RecPayRecord> ds = new RecPayRecordSvc().GetAllReceivablesRecord(Session["CurrentCompanyGuid"].ToString());
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
        public string GetRPVoucher(string rows, string page, string RP_GUID)
        {
            int count = 0;
            List<T_RecPayRecord> List = new RecPayRecordSvc().GetRPVoucher(1, -1, out count, RP_GUID, "R");
            string json = new JavaScriptSerializer().Serialize(List);
            return json;
        }

        /// <summary>
        /// 反销账
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// 2017/4/3   sunp   update
        /// </remarks>
        public string backCancelAccount(string id,string flag)
        {
            ExceResult res = new ExceResult();
            string CId = CompanyId();
            bool result = new RecPayRecordSvc().backCancelAccount(id,flag);
            string msg = string.Empty;
            if (result)
            {
                msg = General.Resource.Common.Success;
                result = true;
            }
            else
            {
                msg = General.Resource.Common.Failed;
                result = false;
            }

            return string.Format("{{\"Result\":{0},\"Msg\":\"{1}\"}}"
                , result.ToString().ToLower(), msg);

        }
    }
}