using System;
using System.Web.Mvc;
using BaseController;
using FMS.DAL;
using FMS.Model;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Web.Script.Serialization;
using System.Web;
using System.IO;
using Aspose.Cells;
using Newtonsoft.Json;
using System.Globalization;
using System.Linq;
using Common.Models;

namespace FMS.BLL
{
    /// <summary>
    /// 记录付款
    /// </summary>
    public class PaymentRecordController : UserController
    {
        public PaymentRecordController()
            : base("PaymentRecord")
        {
        }

        /// <summary>
        /// 列表页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 付款纪录页
        /// </summary>
        /// <param name="id">付款纪录标识</param>
        /// <returns></returns>
        public ActionResult PaymentRecord(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return View(new T_RecPayRecord() { RP_GUID = Guid.NewGuid().ToString() });
            }
            else
            {

                string C_GUID = Session["CurrentCompanyGuid"].ToString();
                return View(new RecPayRecordSvc().GetPaymentRecord(id, C_GUID));
            }
        }

        /// <summary>
        /// 归档页
        /// </summary>
        /// <param name="id">付款纪录标识</param>
        /// <returns></returns>
        public ActionResult Pigeonhole(string id)
        {
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            return View(new RecPayRecordSvc().GetPaymentRecord(id, C_GUID));
        }

        /// <summary>
        /// 选择应付纪录页面
        /// </summary>
        /// <param name="id">收款方标识</param>
        /// <returns></returns>
        public ActionResult ChoosePayablesRecord(string id)
        {
            ViewData["C_GUID"] = Session["CurrentCompanyGuid"].ToString();
            ViewData["RPer"] = id;
            return View("ChoosePayablesRecord");
        }

        public ActionResult ChooseWCRecord(string id)
        {
            ViewData["C_GUID"] = Session["CurrentCompanyGuid"].ToString();
            ViewData["RPer"] = id;
            return View("ChooseWCRecord");
        }

        public ActionResult GetPaymentDeclareCostSpending()
        {
            ViewData["C_GUID"] = Session["CurrentCompanyGuid"].ToString();
            return View("GetPaymentDeclareCostSpending");
        }

        /// <summary>
        /// 数据导入
        /// </summary>
        /// <returns></returns>
        public ActionResult UpPRFile()
        {
            ViewData["C_GUID"] = Session["CurrentCompanyGuid"].ToString();
            return View("UpPRFile");
        }

        /// <summary>
        /// 数据导入
        /// </summary>
        /// <returns></returns>
        public ActionResult ImportRecord()
        {
            ViewData["C_GUID"] = Session["CurrentCompanyGuid"].ToString();
            return View("ImportRecord");
        }

        /// <summary>
        /// 更改状态
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string UpdState(string id)
        {
            bool result = new DeclareCostSpendingSvc().UpdState(id, "已付");
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
                    entity.FileType = fileData.FileName.Substring(fileData.FileName.LastIndexOf(".") + 1);
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

        public string GetIEUsed(string rows, string page, string id)
        {
            int count = 0;
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            StringBuilder strJson = new StringBuilder();
            List<T_RecPayRecord> List = new List<T_RecPayRecord>();
            List = new RecPayRecordSvc().GetIEUsed(C_GUID, 1, -1, out count, id);
            return new JavaScriptSerializer().Serialize(List);

        }

        public string GetChoosePayablesList(string rows, string page, string state, string customer, string invtype, string remark, string DetailInvtype)
        {
            int count = 0;
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            StringBuilder strJson = new StringBuilder();
            List<T_IERecord> List = new List<T_IERecord>();
            List = new RecPayRecordSvc().GetChoosePayablesList(C_GUID, 1, -1, out count, state, customer, invtype, remark, DetailInvtype);
            return new JavaScriptSerializer().Serialize(List);


        }
        public string GetTaxInfoCollection()
        {
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            StringBuilder strJson = new StringBuilder();
            List<T_IERecord> List = new List<T_IERecord>();
            List = new RecPayRecordSvc().GetTaxInfoCollection(C_GUID);
            T_IERecord detailTax = new T_IERecord();
            GetTaxDetail(ref detailTax);
            if (detailTax != null && detailTax.IEGroup!=null)
            {
                List.Insert(0, detailTax);
            }
            return new JavaScriptSerializer().Serialize(List);
        }

        public List<T_IERecord> GetTaxDetail(ref T_IERecord iecord)
        {
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            List<T_IERecord> List = new List<T_IERecord>();
            List<T_IERecord> lstDetail = new List<T_IERecord>();
            List = new RecPayRecordSvc().GetAllTaxInfo(C_GUID);
            decimal fistAmount = 0;
            for (int i = 0; i < List.Count; i++)
            {
                T_IERecord record = new T_IERecord();
                if (i == 0)
                {
                    if (List[i].DisAmount > 0)
                    {
                        record.IE_GUID = List[i].IE_GUID;
                        record.IEGroup = List[i].IEGroup;
                        record.InvType = List[i].InvType;
                        record.Amount = List[i].DisAmount;
                        record.DisAmount = List[i].DisAmount;
                        record.State = List[i].State;
                        record.AffirmDate = List[i].AffirmDate;
                        iecord.IEGroup = List[i].IEGroup;
                        iecord.InvType = List[i].InvType;
                        iecord.Amount = List[i].DisAmount;
                        iecord.DisAmount = List[i].DisAmount;
                        iecord.State = List[i].State;
                        lstDetail.Add(record);
                    }
                    else
                    {
                        fistAmount = List[i].DisAmount;
                    }
                }
                else
                {
                    if (List[i].DisAmount + fistAmount <= 0)
                    {
                        fistAmount = List[i].DisAmount + fistAmount;
                    }
                    else
                    {
                        record.IEGroup = List[i].IEGroup;
                        record.IE_GUID = List[i].IE_GUID;
                        record.InvType = List[i].InvType;
                        iecord.Amount += List[i].DisAmount + fistAmount;
                        record.Amount = List[i].DisAmount + fistAmount;
                        iecord.DisAmount += List[i].DisAmount + fistAmount;
                        record.DisAmount = List[i].DisAmount + fistAmount;
                        record.State = List[i].State;
                        record.AffirmDate = List[i].AffirmDate;
                        lstDetail.Add(record);
                    }
                }
            }
            return lstDetail.OrderByDescending(p=>p.AffirmDate).ToList();
            //return new JavaScriptSerializer().Serialize(List);
        }

        public string GetTaxInfo(string Flag)
        {
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            StringBuilder strJson = new StringBuilder();
            List<T_IERecord> List = new List<T_IERecord>();
            if (Flag == "TA")
            {
                T_IERecord iercord = new T_IERecord();
                List = GetTaxDetail(ref iercord);
            }
            else
            {
               List = new RecPayRecordSvc().GetTaxInfo(C_GUID, Flag);
            }
            return new JavaScriptSerializer().Serialize(List);
        }

        /// <summary>
        /// 合并成本外支行直接、间接物料和成本应付纪录列表
        /// </summary>
        /// <param name="rows">页大小</param>
        /// <param name="page">页索引</param>
        /// <param name="id">收款方标识</param>
        /// <returns></returns>
        public string GetUnionList(string rows, string page, string customer, string InvType)
        {
            int count = 0;
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            StringBuilder strJson = new StringBuilder();
            List<T_IERecord> List = new List<T_IERecord>();
            List = new RecPayRecordSvc().GetUnionDPay(C_GUID, 1, -1, out count, customer, InvType);
            return new JavaScriptSerializer().Serialize(List);

        }
        /// <summary>
        /// 应付纪录列表
        /// </summary>
        /// <param name="rows">页大小</param>
        /// <param name="page">页索引</param>
        /// <param name="id">收款方标识</param>
        /// <returns></returns>
        public string GetChoosePayablesRecord(string rows, string page, string id, string iegroup = null)
        {
            int count = 0;
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            //string strFormatter = "{{\"total\":\"{0}\",\"rows\":{1}}}";
            StringBuilder strJson = new StringBuilder();
            List<T_Payables> Payables = new List<T_Payables>();
            Payables = new RecPayRecordSvc().GetChoosePayablesRecord(id, C_GUID, 1, -1, out count, iegroup);
            //strJson.AppendFormat(strFormatter, count, new JavaScriptSerializer().Serialize(Payables));
            strJson.Append(new JavaScriptSerializer().Serialize(Payables));

            return strJson.ToString();

        }

        /// <summary>
        /// 付款纪录列表
        /// </summary>
        /// <param name="rows">页大小</param>
        /// <param name="page">页索引</param>
        /// <returns></returns>
        public string GetPaymentList(string rows, string page)
        {
            int count = 0;
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            string strFormatter = "{{\"total\":\"{0}\",\"rows\":{1}}}";
            StringBuilder strJson = new StringBuilder();
            List<T_RecPayRecord> RecPayRecord = new List<T_RecPayRecord>();
            RecPayRecord = new RecPayRecordSvc().GetPaymentRecord(C_GUID, int.Parse(page), int.Parse(rows), out count);
            strJson.AppendFormat(strFormatter, count, new JavaScriptSerializer().Serialize(RecPayRecord));
            return strJson.ToString();
        }

        /// <summary>
        /// 付款纪录页面（只读）
        /// </summary>
        /// <param name="id">付款纪录标识</param>
        /// <returns></returns>
        public ActionResult CheckPaymentRecord(string id)
        {
            return View(new RecPayRecordSvc().GetPaymentRecord(id, Session["CurrentCompanyGuid"].ToString()));
        }


        /// <summary>
        /// 更新付款纪录
        /// </summary>
        /// <param name="rec">更新付款对象</param>
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
        /// 新增付款记录
        /// </summary>
        /// <param name="paramList">付款记录列表</param>
        /// <returns></returns>
        [HttpPost]
        public string CreatePaymentRecord(List<T_RecPayRecord> payList, string addstyle)
        {
            bool result = false;
            string msg = string.Empty;
            string strSalaryID = Guid.NewGuid().ToString();
            string strYtTaxID = Guid.NewGuid().ToString();
            //DateTime EditThreshold = DateTime.Parse(Session["EditThreshold"].ToString());

            //var errorPay = payList.FindAll(i => i.Date > DateTime.Now || i.Date < EditThreshold);
            //if (errorPay.Count > 0)
            //{
            //    result = false;
            //    msg = FMS.Resource.Finance.Finance.DateError;

            //    return string.Format("{{\"Result\":{0},\"Msg\":\"{1}\"}}"
            //        , result.ToString().ToLower(), msg);
            //}
            //根据银行账户ID获取对应的银行ID


            foreach (T_RecPayRecord recPayRecord in payList)
            {
                if (recPayRecord.BA_GUID == null)
                {
                    recPayRecord.C_GUID = Session["CurrentCompanyGuid"].ToString();
                    recPayRecord.Creator = base.userData.LoginFullName;
                    recPayRecord.B_GUID = "";
                    recPayRecord.BA_GUID = "";
                    recPayRecord.RP_Flag = "P";
                }
                else
                {
                    T_BankAccount ba = new BankAccountSvc().GetBank(Session["CurrentCompanyGuid"].ToString(), null, recPayRecord.BA_GUID, null);
                    var bankGuid = ba.B_GUID;
                    recPayRecord.C_GUID = Session["CurrentCompanyGuid"].ToString();
                    recPayRecord.Creator = base.userData.LoginFullName;
                    recPayRecord.B_GUID = bankGuid;
                    recPayRecord.BA_GUID = ba.BA_GUID;
                    recPayRecord.RP_Flag = "P";
                }

                //if(recPayRecord.RPType=="0F15C212-A858-4AD5-9B13-E51502994818"){
                //recPayRecord.RPType = "库存现金";
                //}
                //else if (recPayRecord.RPType == "6704EDDF-9D23-41FC-A870-827CE2F4D5DB")
                //{
                //    recPayRecord.RPType = "银行存款";
                //}else{
                //    recPayRecord.RPType = "其他货币资金";
                //}

                switch (recPayRecord.PayCategory)
                {
                    case "银行存款":
                        recPayRecord.SubjectName = "应付账款";
                        break;
                    case "库存现金":
                        recPayRecord.SubjectName = "应付账款";
                        break;
                    case "其他货币资金":
                        recPayRecord.SubjectName = "应付票据";
                        break;
                    case "":
                        break;
                }
                switch (addstyle)
                {
                    case "费用获取":
                        switch (recPayRecord.InvTypeDts)
                        {
                            case "购买商品、接受服务所支付的款":
                                recPayRecord.CFItemGuid = "0526C862-F238-4301-A198-E7EC83A645D5";
                                break;
                            case "支付销售费用":
                                recPayRecord.CFItemGuid = "DE7D81B9-680B-4011-A771-C8B327A549E7";
                                break;
                            case "支付管理费用":
                                recPayRecord.CFItemGuid = "DE7D81B9-680B-4011-A771-C8B327A549E7";
                                break;
                            case "支付财务费用":
                                recPayRecord.CFItemGuid = "DE7D81B9-680B-4011-A771-C8B327A549E7";
                                break;
                            case "支付其他业务成本":
                                recPayRecord.CFItemGuid = "DE7D81B9-680B-4011-A771-C8B327A549E7";
                                break;
                            case "支付营业外支出":
                                recPayRecord.CFItemGuid = "DE7D81B9-680B-4011-A771-C8B327A549E7";
                                break;
                        }
                        break;
                    case "工资表获取":
                        if (recPayRecord.InvTypeDts == "支付职工薪酬")
                        {
                            recPayRecord.CFItemGuid = "70765251-FA58-432F-BCC5-122EF3581102";
                            recPayRecord.SubjectName = "应付职工薪酬";
                        }
                        else if (recPayRecord.InvTypeDts == "支付的各项税费")
                        {
                            recPayRecord.CFItemGuid = "E4F16AB4-8DFE-42E1-8A7F-0CB342CF9C73";
                            recPayRecord.SubjectName = "应交税费";
                        }
                        break;
                    case "直接新增":
                        switch (recPayRecord.InvTypeDts)
                        {
                            case "购买商品、接受服务所支付的款":
                                recPayRecord.CFItemGuid = "0526C862-F238-4301-A198-E7EC83A645D5";
                                break;
                            case "支付销售费用":
                                recPayRecord.CFItemGuid = "DE7D81B9-680B-4011-A771-C8B327A549E7";
                                break;
                            case "支付管理费用":
                                recPayRecord.CFItemGuid = "DE7D81B9-680B-4011-A771-C8B327A549E7";
                                break;
                            case "支付财务费用":
                                recPayRecord.CFItemGuid = "DE7D81B9-680B-4011-A771-C8B327A549E7";
                                break;
                            case "预付供应商账款":
                                recPayRecord.CFItemGuid = "DE7D81B9-680B-4011-A771-C8B327A549E7";
                                recPayRecord.SubjectName = "预付账款";
                                break;
                            case "支付其他业务成本":
                                recPayRecord.CFItemGuid = "DE7D81B9-680B-4011-A771-C8B327A549E7";
                                break;
                            case "支付营业外支出":
                                recPayRecord.CFItemGuid = "DE7D81B9-680B-4011-A771-C8B327A549E7";
                                break;
                            case "支付暂支借款":
                                recPayRecord.CFItemGuid = "DE7D81B9-680B-4011-A771-C8B327A549E7";
                                recPayRecord.SubjectName = "备用金";
                                break;
                            case "支付押金":
                                recPayRecord.CFItemGuid = "DE7D81B9-680B-4011-A771-C8B327A549E7";
                                recPayRecord.SubjectName = "其他应收款";
                                break;
                            case "归还其它公司支付的押金":
                                recPayRecord.CFItemGuid = "DE7D81B9-680B-4011-A771-C8B327A549E7";
                                recPayRecord.SubjectName = "其他应付款";
                                break;
                            case "短期投资支出":
                                recPayRecord.CFItemGuid = "049F1C6D-49EA-4E2D-93FD-2DABEBED666C";
                                recPayRecord.SubjectName = "短期投资";
                                break;
                            case "长期债券投资支出":
                                recPayRecord.CFItemGuid = "049F1C6D-49EA-4E2D-93FD-2DABEBED666C";
                                recPayRecord.SubjectName = "长期债券投资";
                                break;
                            case "长期股权投资支出":
                                recPayRecord.CFItemGuid = "049F1C6D-49EA-4E2D-93FD-2DABEBED666C";
                                recPayRecord.SubjectName = "长期股权投资";
                                break;
                            case "支付的其他与投资活动有关的现金":
                                recPayRecord.CFItemGuid = "53B21A65-9723-4F7D-B5BF-61490B7BD71D";
                                recPayRecord.SubjectName = "支付其他投资";
                                break;
                            case "购买固定资产所支付的款":
                                recPayRecord.CFItemGuid = "EA46CFA0-5D41-4FF1-9BF8-9A36CB8F1F11";
                                recPayRecord.SubjectName = "固定资产";
                                break;
                            case "购买无形资产所支付的款":
                                recPayRecord.CFItemGuid = "EA46CFA0-5D41-4FF1-9BF8-9A36CB8F1F11";
                                recPayRecord.SubjectName = "无形资产";
                                break;
                            case "购买其他长期资产所支付的款":
                                recPayRecord.CFItemGuid = "EA46CFA0-5D41-4FF1-9BF8-9A36CB8F1F11";
                                recPayRecord.SubjectName = "";
                                break;
                            case "归还短期借款所支付的款":
                                recPayRecord.CFItemGuid = "DD7BCD86-150E-4E62-B6CC-21EF341B41F1";
                                recPayRecord.SubjectName = "短期借款";
                                break;
                            case "归还长期借款所支付的款":
                                recPayRecord.CFItemGuid = "DD7BCD86-150E-4E62-B6CC-21EF341B41F1";
                                recPayRecord.SubjectName = "长期借款";
                                break;
                            case "分配利润、股利所支付的款":
                                recPayRecord.CFItemGuid = "5BDE7476-F268-4A62-8CC3-D426D51E253D";
                                recPayRecord.SubjectName = "应付利润";
                                break;
                            case "偿付利息所支付的款":
                                recPayRecord.CFItemGuid = "5BDE7476-F268-4A62-8CC3-D426D51E253D";
                                recPayRecord.SubjectName = "应付利息";
                                break;
                            case "支付的其他与筹资活动有关的现金":
                                recPayRecord.CFItemGuid = "8254B1DA-91CC-4CA5-B7F0-9AC5D2653865";
                                recPayRecord.SubjectName = "支付其他筹资";
                                break;
                            case "支付职工薪酬":
                                recPayRecord.CFItemGuid = "70765251-FA58-432F-BCC5-122EF3581102";
                                recPayRecord.SubjectName = "应付职工薪酬";
                                break;
                            case "支付的各项税费":
                                recPayRecord.CFItemGuid = "E4F16AB4-8DFE-42E1-8A7F-0CB342CF9C73";
                                recPayRecord.SubjectName = "应交税费";
                                break;
                            case "支付其他与筹资活动有关的款":
                                recPayRecord.CFItemGuid = "8254B1DA-91CC-4CA5-B7F0-9AC5D2653865";
                                break;
                            //case "暂支借款":
                            //    recPayRecord.CFItemGuid = "DE7D81B9-680B-4011-A771-C8B327A549E7";
                            //    break;
                            case "":
                                break;

                        }
                        break;
                    case "预付供应商账款":
                        recPayRecord.CFItemGuid = "DE7D81B9-680B-4011-A771-C8B327A549E7";
                        recPayRecord.SubjectName = "预付账款";
                        break;
                    case "支付暂支借款":
                        recPayRecord.CFItemGuid = "DE7D81B9-680B-4011-A771-C8B327A549E7";
                        recPayRecord.SubjectName = "备用金";
                        break;
                    case "支付押金":
                        recPayRecord.CFItemGuid = "DE7D81B9-680B-4011-A771-C8B327A549E7";
                        recPayRecord.SubjectName = "其他应收款";
                        break;
                    case "投资支出":
                        recPayRecord.InvType = "投资活动付款";
                        recPayRecord.InvTypeDts = "投资所支付的款";
                        recPayRecord.CFItemGuid = "049F1C6D-49EA-4E2D-93FD-2DABEBED666C";
                        break;
                    case "直接物料采购":
                        recPayRecord.CFItemGuid = "0526C862-F238-4301-A198-E7EC83A645D5";
                        break;
                    case "间接物料采购":
                        recPayRecord.CFItemGuid = "0526C862-F238-4301-A198-E7EC83A645D5";
                        break;
                    case "资产采购":
                        recPayRecord.CFItemGuid = "EA46CFA0-5D41-4FF1-9BF8-9A36CB8F1F11";
                        break;
                    case "归还其它公司支付的押金":
                        recPayRecord.CFItemGuid = "DE7D81B9-680B-4011-A771-C8B327A549E7";
                        recPayRecord.SubjectName = "其他应付款";
                        break;
                    case "短期投资支出":
                        recPayRecord.CFItemGuid = "049F1C6D-49EA-4E2D-93FD-2DABEBED666C";
                        recPayRecord.SubjectName = "短期投资";
                        break;
                    case "长期债券投资支出":
                        recPayRecord.CFItemGuid = "049F1C6D-49EA-4E2D-93FD-2DABEBED666C";
                        recPayRecord.SubjectName = "长期债券投资";
                        break;
                    case "长期股权投资支出":
                        recPayRecord.CFItemGuid = "049F1C6D-49EA-4E2D-93FD-2DABEBED666C";
                        recPayRecord.SubjectName = "长期股权投资";
                        break;
                    case "支付的其他与投资活动有关的现金":
                        recPayRecord.CFItemGuid = "53B21A65-9723-4F7D-B5BF-61490B7BD71D";
                        recPayRecord.SubjectName = "支付其他投资";
                        break;
                    case "购买固定资产所支付的款":
                        recPayRecord.CFItemGuid = "EA46CFA0-5D41-4FF1-9BF8-9A36CB8F1F11";
                        recPayRecord.SubjectName = "固定资产";
                        break;
                    case "购买无形资产所支付的款":
                        recPayRecord.CFItemGuid = "EA46CFA0-5D41-4FF1-9BF8-9A36CB8F1F11";
                        recPayRecord.SubjectName = "无形资产";
                        break;
                    case "购买其他长期资产所支付的款":
                        recPayRecord.CFItemGuid = "EA46CFA0-5D41-4FF1-9BF8-9A36CB8F1F11";
                        recPayRecord.SubjectName = "";
                        break;
                    case "归还短期借款所支付的款":
                        recPayRecord.CFItemGuid = "DD7BCD86-150E-4E62-B6CC-21EF341B41F1";
                        recPayRecord.SubjectName = "短期借款";
                        break;
                    case "归还长期借款所支付的款":
                        recPayRecord.CFItemGuid = "DD7BCD86-150E-4E62-B6CC-21EF341B41F1";
                        recPayRecord.SubjectName = "长期借款";
                        break;
                    case "分配利润、股利所支付的款":
                        recPayRecord.CFItemGuid = "5BDE7476-F268-4A62-8CC3-D426D51E253D";
                        recPayRecord.SubjectName = "应付利润";
                        break;
                    case "偿付利息所支付的款":
                        recPayRecord.CFItemGuid = "5BDE7476-F268-4A62-8CC3-D426D51E253D";
                        recPayRecord.SubjectName = "应付利息";
                        break;
                    case "支付职工薪酬":
                        recPayRecord.CFItemGuid = "70765251-FA58-432F-BCC5-122EF3581102";
                        recPayRecord.SubjectName = "应付职工薪酬";
                        break;
                    case "支付的各项税费":
                        recPayRecord.CFItemGuid = "E4F16AB4-8DFE-42E1-8A7F-0CB342CF9C73";
                        recPayRecord.SubjectName = "应交税费";
                        break;
                    case "支付其他与筹资活动有关的款":
                        recPayRecord.CFItemGuid = "8254B1DA-91CC-4CA5-B7F0-9AC5D2653865";
                        break;
                    //case "暂支借款":
                    //    recPayRecord.CFItemGuid = "DE7D81B9-680B-4011-A771-C8B327A549E7";
                    //    break;
                    case "":
                        break;
                    default:
                        break;
                }
                if (recPayRecord.IE_GUID != null)
                {
                    string check = null;
                    string[] temp = recPayRecord.IE_GUID.Split(new char[] { ',' });
                    if (recPayRecord.SumAmount == recPayRecord.DisAmount)
                    {
                        recPayRecord.Record = "已销账";
                        recPayRecord.DisAmount1 = 0;
                        check = "EQ";
                        foreach (var a in temp)
                        {
                            result = new RecPayRecordSvc().UpdIERP(a, recPayRecord.RP_GUID, check, recPayRecord.Mark, recPayRecord.RP_Flag, recPayRecord.InvTypeDts, recPayRecord.B_GUID);
                        }
                    }

                    if (recPayRecord.SumAmount > recPayRecord.DisAmount)
                    {
                        recPayRecord.Record = "未销账";
                        recPayRecord.DisAmount1 = recPayRecord.SumAmount - recPayRecord.DisAmount;
                        check = "LESS";
                        foreach (var a in temp)
                        {
                            result = new RecPayRecordSvc().UpdIERP(a, recPayRecord.RP_GUID, check, recPayRecord.Mark, recPayRecord.RP_Flag, recPayRecord.InvTypeDts, recPayRecord.B_GUID);
                        }
                    }
                    if (recPayRecord.SumAmount < recPayRecord.DisAmount)
                    {
                        recPayRecord.Record = "已销账";

                        check = "MORE";
                        recPayRecord.DisAmount1 = 0;
                        foreach (var a in temp)
                        {
                            result = new RecPayRecordSvc().UpdIERP(a, recPayRecord.RP_GUID, check, recPayRecord.Mark, recPayRecord.RP_Flag, recPayRecord.InvTypeDts, recPayRecord.B_GUID);
                        }
                        if (result)
                        {
                            result = new RecPayRecordSvc().UpdIERPMore(recPayRecord);
                        }
                    }
                    if (result)
                    {
                        result = new RecPayRecordSvc().UpdPaymentRecord(recPayRecord, strSalaryID, strYtTaxID);
                    }
                }
                else
                {
                    recPayRecord.DisAmount1 = recPayRecord.SumAmount;
                    result = new RecPayRecordSvc().UpdPaymentRecord(recPayRecord, strSalaryID, strYtTaxID);
                }


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
        /// 更新付款纪录
        /// </summary>
        /// <param name="rec">付款纪录对象</param>
        /// <returns></returns>
        public string UpdPaymentRecordDts(string id, string name, DateTime date, decimal money, string remark, string currency, string bank, string bankaccount, string ieguid, string addstyle)
        {
            bool result = false;
            string msg = string.Empty;
            T_RecPayRecord recPayRecord = new T_RecPayRecord();
            recPayRecord.Creator = base.userData.LoginFullName;
            recPayRecord.C_GUID = Session["CurrentCompanyGuid"].ToString();
            recPayRecord.RP_GUID = id;
            //recPayRecord.RPer = (new BusinessPartnerSvc().GetPartnersDts(Session["CurrentCompanyGuid"].ToString(), name)).ToString();
            recPayRecord.RPer = name;
            recPayRecord.Date = DateTime.Now.ToString("yyyy-MM-dd");
            recPayRecord.SumAmount = money;
            recPayRecord.Remark = remark;
            recPayRecord.Currency = currency;

            T_BankAccount ba = new BankAccountSvc().GetBank(Session["CurrentCompanyGuid"].ToString(), null, bankaccount, null);
            recPayRecord.B_GUID = ba.B_GUID;

            //recPayRecord.B_GUID = bank;
            recPayRecord.BA_GUID = bankaccount;
            if (addstyle == "费用获取")
            {
                recPayRecord.IE_GUID = ieguid;
                recPayRecord.InvType = "经营活动付款";
                recPayRecord.InvTypeDts = "购买商品、接受服务所支付的款";
                recPayRecord.CFItemGuid = "0526C862-F238-4301-A198-E7EC83A645D5";
            }
            if (addstyle == "工资表获取")
            {
                recPayRecord.IE_GUID = ieguid;
                recPayRecord.InvType = "经营活动付款";
                recPayRecord.InvTypeDts = "支付职工或为职工支付的款";
                recPayRecord.CFItemGuid = "70765251-FA58-432F-BCC5-122EF3581102";
            }
            if (addstyle == "直接新增")
            {
                recPayRecord.IE_GUID = null;
                recPayRecord.InvType = "未归账";
            }
            if (addstyle == "预付供应商" || addstyle == "支付押金和暂支借款")
            {
                recPayRecord.IE_GUID = ieguid;
                recPayRecord.InvType = "经营活动付款";
                recPayRecord.InvTypeDts = "支付的其他与经营活动有关的款预付供应商、支付押金、暂支款等";
                recPayRecord.CFItemGuid = "DE7D81B9-680B-4011-A771-C8B327A549E7";
            }
            if (addstyle == "投资支出")
            {
                recPayRecord.IE_GUID = ieguid;
                recPayRecord.InvType = "投资活动付款";
                recPayRecord.InvTypeDts = "投资所支付的款";
                recPayRecord.CFItemGuid = "049F1C6D-49EA-4E2D-93FD-2DABEBED666C";
            }
            if (addstyle == "直接物料采购" || addstyle == "间接物料采购" || addstyle == "资产采购")
            {
                recPayRecord.IE_GUID = ieguid;
                recPayRecord.InvType = "投资活动付款";
                recPayRecord.InvTypeDts = "购买固定资产、无形资产和其他长期资产所支付的款";
                recPayRecord.CFItemGuid = "EA46CFA0-5D41-4FF1-9BF8-9A36CB8F1F11";
            }
            recPayRecord.CreateDate = DateTime.Now.ToString("yyyy-MM-dd");
            DateTime EditThreshold = DateTime.Parse(Session["EditThreshold"].ToString());
            DateTime checkDate;
            DateTime.TryParse(recPayRecord.Date, out checkDate);
            if (checkDate <= DateTime.Now && checkDate >= EditThreshold)
            {
                result = new RecPayRecordSvc().UpdPaymentRecord(recPayRecord);
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
        /// 下载附件
        /// </summary>
        /// <param name="fileID">fileID 图片ID</param>
        /// <returns></returns>
        public FileResult DownLoadFile(string fileID)
        {
            Encoding encoding;
            string outputFileName = null;
            AttachmentSvc attSv = new AttachmentSvc();
            var entity = attSv.GetAttachmentById(fileID);
            HttpBrowserCapabilitiesBase bc = Request.Browser;
            string browser = bc.Browser.ToUpper();
            if (browser.Contains("MS") == true && browser.Contains("IE") == true)
            {
                outputFileName = HttpUtility.UrlEncode(entity.FileName);
                encoding = System.Text.Encoding.Default;
            }
            else if (browser.Contains("FIREFOX") == true)
            {
                outputFileName = entity.FileName;
                encoding = System.Text.Encoding.GetEncoding("GB2312");
            }
            else
            {
                outputFileName = HttpUtility.UrlEncode(entity.FileName);
                encoding = System.Text.Encoding.Default;
            }

            //从数据库查找
            return File(entity.FlieData, entity.FileType, outputFileName);
        }

        /// <summary>
        /// 批量导入excel数据
        /// </summary>
        public ActionResult Upexcel(FormCollection from)
        {
            HttpPostedFileBase file = Request.Files["upload"];
            string result = string.Empty;
            if (file == null || file.ContentLength <= 0)
            {

            }
            else
            {
                try
                {
                    Workbook workbook = new Workbook(file.InputStream);
                    Cells cells = workbook.Worksheets[0].Cells;
                    DataTable tab = cells.ExportDataTable(0, 0, cells.Rows.Count, cells.MaxDisplayRange.ColumnCount);
                    int rowsnum = tab.Rows.Count;
                    if (rowsnum == 0)
                    {
                        result = "Excel表为空!请重新导入！"; //当Excel表为空时，对用户进行提示
                    }
                    //数据表一共多少行！
                    DataRow[] dr = tab.Select();
                    //按行进行数据存储操作！
                    for (int i = 1; i < dr.Length; i++)
                    {
                        //RPer,B_Guid,BA_Guid数据需要比对！
                        string rper = (new BusinessPartnerSvc().GetPartnersDts(Session["CurrentCompanyGuid"].ToString(), dr[i][0].ToString())).ToString();
                        string bguid = (new BankAccountSvc().GetBankDts(Session["CurrentCompanyGuid"].ToString(), dr[i][4].ToString())).ToString();
                        string baguid = (new BankAccountSvc().GetBankAccountDts(Session["CurrentCompanyGuid"].ToString(), bguid, dr[i][5].ToString())).ToString();

                        string cguid = Session["CurrentCompanyGuid"].ToString();
                        string creator = base.userData.LoginFullName;
                        DateTime createdate = DateTime.Now;

                        DBHelper dh = new DBHelper();
                        dh.strCmd = "SP_UpdRecPayRecord";
                        dh.AddPare("@ID", SqlDbType.NVarChar, 40, Guid.NewGuid().ToString());
                        dh.AddPare("@Flag", SqlDbType.NVarChar, 1, "P");
                        dh.AddPare("@R_Per", SqlDbType.NVarChar, 40, rper);
                        dh.AddPare("@Date", SqlDbType.Date, 0, dr[i][1].ToString());
                        dh.AddPare("@Amount", SqlDbType.Decimal, 0, dr[i][2].ToString());
                        dh.AddPare("@Currency", SqlDbType.NVarChar, 40, dr[i][3].ToString());
                        dh.AddPare("@B_Guid", SqlDbType.NVarChar, 40, bguid);
                        dh.AddPare("@BA_Guid", SqlDbType.NVarChar, 40, baguid);
                        dh.AddPare("@Remark", SqlDbType.NVarChar, 40, dr[i][6].ToString());

                        dh.AddPare("@Creator", SqlDbType.NVarChar, 40, creator);
                        dh.AddPare("@CreateDate", SqlDbType.DateTime, 0, createdate);
                        dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, cguid);
                        dh.AddPare("@InvType", SqlDbType.NVarChar, 50, "未归账");
                        try
                        {
                            dh.NonQuery();
                            result = "导入成功！";
                        }
                        catch
                        {
                            result = "导入数据部分错误！";
                        }
                    }
                }
                catch (Exception ex)
                {
                    result = "导入失败！";
                }
            }
            JsonResult json = new JsonResult();
            json.Data = result;
            return json;
        }

        public ActionResult ImportExecl(HttpPostedFileBase fileData, string guid, string number = null)
        {
            List<T_RecPayRecord> payList = new List<T_RecPayRecord>();
            T_RecPayRecord rec = new T_RecPayRecord();
            ExceResult res = new ExceResult();
            res.success = true;
            var files = Request.Files;
            string result = string.Empty;
            JsonResult json = new JsonResult();

            if (files == null && files.Count == 0)
            {
                res.success = false;
                res.msg = "无有效文件";
            }

            Workbook workbook = new Workbook(Request.Files[0].InputStream);
            Cells cells = workbook.Worksheets[0].Cells;
            DataTable tab = cells.ExportDataTable(0, 0, cells.Rows.Count, cells.MaxDisplayRange.ColumnCount);
            int rowsnum = tab.Rows.Count;
            if (rowsnum == 0)
            {
                res.success = false;
                result = "Excel表为空!请重新导入！";//当Excel表为空时，对用户进行提示
            }
            //数据表一共多少行！
            DataRow[] dr = tab.Select();
            //按行进行数据存储操作！
            for (int i = 1; i < dr.Length; i++)
            {


                T_BankAccount ba = new T_BankAccount();
                string rper;
                //RPer,B_Guid,BA_Guid数据需要比对！
                try
                {
                    ba = new BankAccountSvc().GetBankDt(Session["CurrentCompanyGuid"].ToString(), dr[i][0].ToString());
                }
                catch
                {
                    result = "导入失败，无此银行账号";
                    break;
                }
                //string baguid = (new BankAccountSvc().GetBankAccountDts(Session["CurrentCompanyGuid"].ToString(), bguid, dr[i][5].ToString())).ToString();
                try
                {
                    rper = (new BusinessPartnerSvc().GetPartnersDts(Session["CurrentCompanyGuid"].ToString(), dr[i][2].ToString())).ToString();
                }
                catch
                {
                    result = "导入失败，无此供应商";
                    break;
                }
                try
                {
                    string currency = (new CurrencySvc().GetCurrency(dr[i][1].ToString())).ToString();
                }
                catch (Exception)
                {
                    result = "导入失败，无此货币";
                    break;
                }

                if (dr[i][4].ToString().CompareTo(GetNowDate()) > 0)
                {
                    result = "导入失败，时间错误";
                    break;
                }

                //rec.RP_GUID = Guid.NewGuid().ToString();
                //rec.RP_Flag = "P";
                //rec.R_PerName = dr[i][2].ToString();
                //rec.B_GUID = dr[i][0].ToString();
                //rec.C_GUID = Session["CurrentCompanyGuid"].ToString();
                //rec.InvType = dr[i][5].ToString();
                //rec.InvTypeDts = dr[i][6].ToString();
                //rec.CreateDate = GetDetailDate();
                //rec.Creator = base.userData.LoginFullName;
                //rec.Currency = dr[i][1].ToString();
                //rec.Record = "未归账";
                //payList.Add(rec);


                //ViewBag.payList = payList;
                //result = "导入成功！";


                string CFItemGuid = null;
                string cguid = Session["CurrentCompanyGuid"].ToString();
                string creator = base.userData.LoginFullName;
                DateTime createdate = DateTime.Now;
                switch (dr[i][6].ToString())
                {
                    case "购买商品、接受服务所支付的款":
                        CFItemGuid = "0526C862-F238-4301-A198-E7EC83A645D5";
                        break;
                    case "支付其他与经营活动有关的款":
                        CFItemGuid = "DE7D81B9-680B-4011-A771-C8B327A549E7";
                        break;
                    case "投资所支付款":
                        CFItemGuid = "049F1C6D-49EA-4E2D-93FD-2DABEBED666C";
                        break;
                    case "购买固定资产、无形资产和其他长期资产所支付的款":
                        CFItemGuid = "EA46CFA0-5D41-4FF1-9BF8-9A36CB8F1F11";
                        break;
                    case "归还债务所支付的款":
                        CFItemGuid = "DD7BCD86-150E-4E62-B6CC-21EF341B41F1";
                        break;
                    case "分配股利、利润的款":
                        CFItemGuid = "5BDE7476-F268-4A62-8CC3-D426D51E253D";
                        break;
                    case "偿付利息所支付的款":
                        CFItemGuid = "5BDE7476-F268-4A62-8CC3-D426D51E253D";
                        break;
                    case "支付职工或为职工支付的款":
                        CFItemGuid = "70765251-FA58-432F-BCC5-122EF3581102";
                        break;
                    case "支付的各项税费":
                        CFItemGuid = "E4F16AB4-8DFE-42E1-8A7F-0CB342CF9C73";
                        break;
                    case "支付其他与筹资活动有关的款":
                        CFItemGuid = "8254B1DA-91CC-4CA5-B7F0-9AC5D2653865";
                        break;
                    case "支付押金、暂支款":
                        CFItemGuid = "DE7D81B9-680B-4011-A771-C8B327A549E7";
                        break;
                    case "":
                        break;

                }
                DBHelper dh = new DBHelper();
                dh.strCmd = "SP_UpdRecPayRecord";
                dh.AddPare("@ID", SqlDbType.NVarChar, 40, Guid.NewGuid().ToString());
                dh.AddPare("@Flag", SqlDbType.NVarChar, 1, "P");
                dh.AddPare("@R_Per", SqlDbType.NVarChar, 40, rper);
                dh.AddPare("@Date", SqlDbType.Date, 0, dr[i][4].ToString());
                dh.AddPare("@Amount", SqlDbType.Decimal, 0, dr[i][3].ToString());
                dh.AddPare("@Currency", SqlDbType.NVarChar, 40, dr[i][1].ToString());
                dh.AddPare("@B_Guid", SqlDbType.NVarChar, 40, ba.B_GUID);
                dh.AddPare("@BA_Guid", SqlDbType.NVarChar, 40, ba.BA_GUID);
                dh.AddPare("@Remark", SqlDbType.NVarChar, 40, null);
                dh.AddPare("@CFItemGuid", SqlDbType.NVarChar, 40, CFItemGuid);
                dh.AddPare("@Creator", SqlDbType.NVarChar, 40, creator);
                dh.AddPare("@CreateDate", SqlDbType.DateTime, 0, createdate);
                dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, cguid);
                dh.AddPare("@InvType", SqlDbType.NVarChar, 20, dr[i][5].ToString());
                dh.AddPare("@InvTypeDts", SqlDbType.NVarChar, 50, dr[i][6].ToString());
                dh.AddPare("@Record", SqlDbType.NVarChar, 50, "未销账");
                try
                {
                    dh.NonQuery();
                    result = "导入成功！";
                }
                catch (Exception)
                {
                    result = "导入失败，请检查EXCEL格式是否错误！";
                }

            }
            string pay = new JavaScriptSerializer().Serialize(payList);
            json.Data = result;

            return json;
        }
        /// <summary>
        /// 提交内部转账
        /// </summary>
        /// <param name="recordList">账户表</param>
        /// <returns></returns>
        /// 20180620注释
        /*public string UpdAccountTransferRecord(T_BankAccount recordList, string outpayType, string InpayType)
        {
            bool result = false;
            string msg = string.Empty;
            recordList.C_GUID = Session["CurrentCompanyGuid"].ToString();
            //当前用户 
            result = new BankAccountSvc().UpdAccountTransferRecord(recordList, outpayType, InpayType);
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
            
        }*/
        public string UpdAccountTransferRecord(string OutBankAccount, string InBankAccount, string outpayType, string InpayType, string OutDate, string OutAmout)
        {
            bool result = false;
            string msg = string.Empty;
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            //当前用户 
            result = new BankAccountSvc().UpdAccountTransferRecord(OutBankAccount, InBankAccount, outpayType, InpayType, OutDate, OutAmout, C_GUID);
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
        /// <param name="recordList"></param>
        /// <returns></returns>
        public string GetOutBankAmount(string AccountAbbreviation)
        {
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            T_BankAccount rec = new BankAccountSvc().GetOutBankAmount(C_GUID, AccountAbbreviation);
            string json = new JavaScriptSerializer().Serialize(rec);
            //string result = json.Remove(0, 193);
            //return result;
            return json;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="recordList"></param>
        /// <returns></returns>
        public string GetOutKuCun(string AccountAbbreviation)
        {
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            T_BankAccount rec = new BankAccountSvc().GetOutKuCun(C_GUID, AccountAbbreviation);
            string json = new JavaScriptSerializer().Serialize(rec);
            //string result = json.Remove(0, 193);
            //return result;
            return json;
        }

        /*public string GetVoucherID(string id)
       {
           string C_GUID = Session["CurrentCompanyGuid"].ToString();
           T_IERecord rec = new IESvc().GetVoucherID(id, C_GUID);
           string json = new JavaScriptSerializer().Serialize(rec);
           return json;
       }*/

    }
}