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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FMS.BLL
{
    /// <summary>
    /// 记录收入
    /// </summary>
    public class IncomeRecordController : UserController
    {
        public IncomeRecordController()
            : base("IncomeRecord")
        { }


        /// <summary>
        /// 列表页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        public string GetUserCurrency()
        {
            //string strFormatter = "{{label:\'{0}\',value:\'{1}\'}},";
            StringBuilder strJson = new StringBuilder("[");
            foreach (T_Currency item in new CurrencySvc().GetUserCurrency(Session["CurrentCompanyGuid"].ToString()))
            {
                //strJson.AppendFormat(strFormatter, item.Code, item.Code);
                strJson.Append("{");
                strJson.AppendFormat("label:\'{0}\',", item.Code);
                strJson.AppendFormat("value:\'{0}\'", item.Code);
                strJson.Append("},");
            }
            strJson.Remove(strJson.Length - 1, 1);
            string json = (strJson.Append("]")).ToString();
            return json;
        }

        public ActionResult Temporary()
        {
            ViewData["IE_GUID"] = Guid.NewGuid().ToString();
            ViewData["DateNow"] =GetNowDate();
            ViewData["product"] = GetUserCurrency();
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
                string C_GUID = Session["CurrentCompanyGuid"].ToString();
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
            return View(new IESvc().GetIE(id, Session["CurrentCompanyGuid"].ToString()));
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
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            string strFormatter = "{{\"total\":\"{0}\",\"rows\":{1}}}";
            StringBuilder strJson = new StringBuilder();
            List<T_IERecord> IERcord = new List<T_IERecord>();
            IERcord = new IESvc().GetIncomeList(C_GUID,int.Parse(page),int.Parse(rows),out count,string.Empty,string.Empty,string.Empty,string.Empty,string.Empty);
            strJson.AppendFormat(strFormatter, count, new JavaScriptSerializer().Serialize(IERcord));
            return strJson.ToString();
           
        }
        public string UpdInCome(List<T_IERecord> IEList) {
            bool result = false;
            string msg = string.Empty;
            foreach (T_IERecord ieRecord in IEList)
            {
                ieRecord.C_GUID = Session["CurrentCompanyGuid"].ToString();
                ieRecord.Creator = base.userData.LoginFullName;
                ieRecord.Currency = Session["Currency"].ToString();
                switch (ieRecord.InvType)
                {
                    case "主营业务收入":
                    ieRecord.Profit_Name = "主营业务收入";
                    ieRecord.RPInvType = "经营活动收款";
                    ieRecord.InvTypeDts = "销售商品/提供服务的收款";
                    ieRecord.CFItemGuid = "97B181C8-D807-4BF0-8D8D-B23273E7FEFE";
                    break;
                    case "其他业务收入":
                    ieRecord.Profit_Name = "其他业务收入";
                    ieRecord.RPInvType = "经营活动收款";
                    ieRecord.InvTypeDts = "其他业务收入的收款";
                    ieRecord.CFItemGuid = "F6330595-F588-46B0-8998-752C7A1D774B";
                    break;
                    case "营业外收入":
                    ieRecord.Profit_Name = "营业外收入";
                    ieRecord.RPInvType = "经营活动收款";
                    ieRecord.InvTypeDts = "营业外收入的收款";
                    ieRecord.CFItemGuid = "F6330595-F588-46B0-8998-752C7A1D774B";
                    break;
                    case "预收客户款":
                    ieRecord.Profit_Name = "预收账款";
                    ieRecord.RPInvType = "经营活动收款";
                    ieRecord.InvTypeDts = "预收客户账款";
                    ieRecord.CFItemGuid = "F6330595-F588-46B0-8998-752C7A1D774B";
                    break;
                    case "收回公司支出的暂支借款":
                    ieRecord.Profit_Name = "备用金";
                    ieRecord.RPInvType = "经营活动收款";
                    ieRecord.InvTypeDts = "收回公司支出的暂支借款";
                    ieRecord.CFItemGuid = "F6330595-F588-46B0-8998-752C7A1D774B";
                    break;
                    case "收回公司支出的押金":
                    ieRecord.Profit_Name = "其他应收款";
                    ieRecord.RPInvType = "经营活动收款";
                    ieRecord.InvTypeDts = "收回公司支出的押金";
                    ieRecord.CFItemGuid = "F6330595-F588-46B0-8998-752C7A1D774B";
                    break;
                    case "收到的其它公司支付的押金":
                    ieRecord.Profit_Name = "其他应付款";
                    ieRecord.RPInvType = "经营活动收款";
                    ieRecord.InvTypeDts = "收到的其它公司支付的押金";
                    ieRecord.CFItemGuid = "F6330595-F588-46B0-8998-752C7A1D774B";
                    break;
                    case "投资收益":
                    ieRecord.Profit_Name = "投资收益";
                    ieRecord.RPInvType = "投资活动收款";
                    if (ieRecord.DetailInvtype == "利息") { ieRecord.InvTypeDts = "取得投资收益的利息的收款"; }
                    if (ieRecord.DetailInvtype == "股利") { ieRecord.InvTypeDts = "取得投资收益的股利的收款"; }
                    ieRecord.CFItemGuid = "C55B2A4E-129B-407B-AC0B-14C091587D45";
                    break;
                    case "主营业务成本":
                    ieRecord.Profit_Name = "主营业务成本";
                    ieRecord.InvType = "营业成本";   
                    ieRecord.RPInvType = "经营活动付款";
                    ieRecord.InvTypeDts = "购买商品、接受服务所支付的款";
                    ieRecord.CFItemGuid = "0526C862-F238-4301-A198-E7EC83A645D5";
                    break;
                    case "销售费用":
                    ieRecord.Profit_Name = "销售费用";
                    ieRecord.RPInvType = "经营活动付款";
                    ieRecord.InvTypeDts = "支付销售费用";
                    ieRecord.CFItemGuid = "DE7D81B9-680B-4011-A771-C8B327A549E7";
                    break;
                    case "财务费用":
                    ieRecord.Profit_Name = "财务费用";
                    ieRecord.RPInvType = "经营活动付款";
                    ieRecord.InvTypeDts = "支付财务费用";
                    ieRecord.CFItemGuid = "DE7D81B9-680B-4011-A771-C8B327A549E7";
                    break;
                    case "管理费用":
                    ieRecord.Profit_Name = "管理费用";
                    ieRecord.RPInvType = "经营活动付款";
                    ieRecord.InvTypeDts = "支付管理费用";
                    ieRecord.CFItemGuid = "DE7D81B9-680B-4011-A771-C8B327A549E7";
                    break;
                    case "其他业务成本":
                    ieRecord.Profit_Name = "其他业务成本";
                    ieRecord.RPInvType = "经营活动付款";
                    ieRecord.InvTypeDts = "支付其他业务成本";
                    ieRecord.CFItemGuid = "DE7D81B9-680B-4011-A771-C8B327A549E7";
                    break;
                    case "营业外支出":
                    ieRecord.Profit_Name = "营业外支出";
                    ieRecord.RPInvType = "经营活动付款";
                    ieRecord.InvTypeDts = "支付营业外支出";
                    ieRecord.CFItemGuid = "DE7D81B9-680B-4011-A771-C8B327A549E7";
                    break;
                    case "预付供应商":
                    ieRecord.Profit_Name = "预付账款";
                    ieRecord.RPInvType = "经营活动付款";
                    ieRecord.InvTypeDts = "预付供应商账款";
                    ieRecord.CFItemGuid = "DE7D81B9-680B-4011-A771-C8B327A549E7";
                    break;
                    case "支付押金":
                    ieRecord.Profit_Name = "其他应收款";
                    ieRecord.RPInvType = "经营活动付款";
                    ieRecord.InvTypeDts = "支付押金";
                    ieRecord.CFItemGuid = "DE7D81B9-680B-4011-A771-C8B327A549E7";
                    break;
                    case "支付暂支借款":
                    ieRecord.Profit_Name = "备用金";
                    ieRecord.RPInvType = "经营活动付款";
                    ieRecord.InvTypeDts = "支付暂支借款";
                    ieRecord.CFItemGuid = "DE7D81B9-680B-4011-A771-C8B327A549E7";
                    break;
                    case "归还其它公司支付的押金":
                    ieRecord.Profit_Name = "其他应付款";
                    ieRecord.RPInvType = "经营活动付款";
                    ieRecord.InvTypeDts = "归还其它公司支付的押金";
                    ieRecord.CFItemGuid = "DE7D81B9-680B-4011-A771-C8B327A549E7";
                    break;
                    case "分配股利、利润":
                    ieRecord.Profit_Name = "预付账款";
                    ieRecord.RPInvType = "经营活动付款";
                    ieRecord.InvTypeDts = "支付其他与经营活动有关的款";
                    ieRecord.CFItemGuid = "DE7D81B9-680B-4011-A771-C8B327A549E7";
                    break;
                    case "短期投资支出":
                    ieRecord.Profit_Name = "短期投资";
                    ieRecord.RPInvType = "投资活动付款";
                    ieRecord.InvTypeDts = "短期投资支出";
                    ieRecord.CFItemGuid = "049F1C6D-49EA-4E2D-93FD-2DABEBED666C";
                    break;
                    case "收回短期投资的本金金额内的款":
                    ieRecord.Profit_Name = "短期投资";
                    ieRecord.RPInvType = "投资活动收款";
                    ieRecord.InvTypeDts = "收回短期投资的本金金额内的款";
                    ieRecord.CFItemGuid = "496F9D4D-F71B-437A-9EA0-26107D3449C3";
                    break;
                    case "长期股权投资支出":
                    ieRecord.Profit_Name = "长期股权投资";
                    ieRecord.RPInvType = "投资活动付款";
                    ieRecord.InvTypeDts = "投资所支付款";
                    ieRecord.CFItemGuid = "049F1C6D-49EA-4E2D-93FD-2DABEBED666C";
                    break;
                    case "长期债权投资支出":
                    ieRecord.Profit_Name = "长期债权投资";
                    ieRecord.RPInvType = "投资活动付款";
                    ieRecord.InvTypeDts = "长期债权投资支出";
                    ieRecord.CFItemGuid = "049F1C6D-49EA-4E2D-93FD-2DABEBED666C";
                    break;
                    case "收回长期债券投资的本金金额内的款":
                    ieRecord.Profit_Name = "长期债权投资";
                    ieRecord.RPInvType = "投资活动收款款";
                    ieRecord.InvTypeDts = "收回长期债券投资的本金金额内的款";
                    ieRecord.CFItemGuid = "496F9D4D-F71B-437A-9EA0-26107D3449C3";
                    break;

                    case "归还短期借款所支付的款":
                    ieRecord.Profit_Name = "短期借款";
                    ieRecord.RPInvType = "筹资活动付款";
                    ieRecord.InvTypeDts = "归还短期借款所支付的款";
                    ieRecord.CFItemGuid = "DD7BCD86-150E-4E62-B6CC-21EF341B41F1";
                    break;
                    case "归还长期借款所支付的款":
                    ieRecord.Profit_Name = "长期借款";
                    ieRecord.RPInvType = "筹资活动付款";
                    ieRecord.InvTypeDts = "归还长期借款所支付的款";
                    ieRecord.CFItemGuid = "DD7BCD86-150E-4E62-B6CC-21EF341B41F1";
                    break;

                    case "收取投资款(注册资本金额以内部分)":
                    ieRecord.Profit_Name = "实收资本";
                    ieRecord.RPInvType = "筹资活动收款";
                    ieRecord.InvTypeDts = "吸收投资的收款(注册资本金额以内部分)";
                    ieRecord.CFItemGuid = "77A24D5F-3E0C-4211-A552-191FEE0E06FD";
                    break;
                    case "收取投资款(超出注册资本金额部分)":
                    ieRecord.Profit_Name = "资本公积";
                    ieRecord.RPInvType = "筹资活动收款";
                    ieRecord.InvTypeDts = "吸收投资的收款(超出注册资本金额部分)";
                    ieRecord.CFItemGuid = "77A24D5F-3E0C-4211-A552-191FEE0E06FD";
                    break;
                    case "短期借款所获得的收款":
                    ieRecord.Profit_Name = "短期借款";
                    ieRecord.RPInvType = "筹资活动收款";
                    ieRecord.InvTypeDts = "短期借款所获得的收款";
                    ieRecord.CFItemGuid = "AD2E5437-0917-43E1-807C-41CA6751360F";
                    break;
                    case "长期借款所获得的收款":
                    ieRecord.Profit_Name = "长期借款";
                    ieRecord.RPInvType = "筹资活动收款";
                    ieRecord.InvTypeDts = "长期借款所获得的收款";
                    ieRecord.CFItemGuid = "AD2E5437-0917-43E1-807C-41CA6751360F";
                    break;
                   
                    case "其他与筹资活动有关的收款":
                    ieRecord.Profit_Name = "其他应付款";
                    ieRecord.RPInvType = "筹资活动收款";
                    ieRecord.InvTypeDts = "其他与筹资活动有关的收款";
                    ieRecord.CFItemGuid = "106B9F2C-24A5-48B5-9621-418D00A7A75A";
                    break;
                   
                   
                        default:
                        break;
                }
               
                

                if (string.IsNullOrEmpty(ieRecord.RP_GUID))
                {
                    if (ieRecord.IE_Flag == "I") {
                        result = new IESvc().UpdIncomeRecord(ieRecord);
                    }
                    if (ieRecord.IE_Flag == "E")
                    {
                        result = new IESvc().UpdExpenseRecord(ieRecord);
                    }
                    if (ieRecord.IE_Flag == "DS")
                    {
                        ieRecord.Record = "未记录";
                        result = new DeclareCostSpendingSvc().UpdV(ieRecord);
                    }
                    if (ieRecord.IE_Flag == "DC")
                    {
                        if (ieRecord.Profit_Name == "长期借款" || ieRecord.Profit_Name == "短期借款") {
                            ieRecord.Record = "未还款";
                        }
                        else {
                            ieRecord.Record = "未记录";
                        }
                        result = new DeclareCustomerSvc().UpdV(ieRecord);
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
                else {
                    if (ieRecord.IE_Flag == "I")
                    {
                        result = new IESvc().UpdIncomeRecord(ieRecord);
                    }
                    if(ieRecord.IE_Flag == "E")
                    {
                        result = new IESvc().UpdExpenseRecord(ieRecord);
                    }
                    if (ieRecord.IE_Flag == "DS")
                    {
                        result = new DeclareCostSpendingSvc().UpdV(ieRecord);
                    }
                    if (ieRecord.IE_Flag == "DC")
                    {
                        result = new DeclareCustomerSvc().UpdV(ieRecord);
                    }
                    if (result) {
                        ieRecord.Record = "已销账";
                        ieRecord.DisAmount = 0;
                        if (ieRecord.IE_Flag == "I" || ieRecord.IE_Flag == "DC") { ieRecord.RP_Flag = "R"; }
                        if (ieRecord.IE_Flag == "E" || ieRecord.IE_Flag == "DS") { ieRecord.RP_Flag = "P"; }
                       
                        ieRecord.Creator = base.userData.LoginFullName;
                        ieRecord.C_GUID = Session["CurrentCompanyGuid"].ToString();
                        if (!string.IsNullOrEmpty(ieRecord.BA_GUID))
                        {
                            if (ieRecord.BA_GUID !="1")
                            {
                            T_BankAccount ba = new BankAccountSvc().GetBank(Session["CurrentCompanyGuid"].ToString(), null, ieRecord.BA_GUID, null);
                            ieRecord.B_GUID = ba.B_GUID;
                            ieRecord.BA_GUID = ba.BA_GUID;
                                }
                        }
                        result = new RecPayRecordSvc().UpdVoucherIERP(ieRecord.IE_GUID, ieRecord.RP_GUID, ieRecord.bankAmount, ieRecord.RP_Flag, ieRecord.IE_Flag, ieRecord.RPLA_GUID.Trim(), ieRecord.IELA_GUID, ieRecord.C_GUID);
                        if (result) {

                            result = new IESvc().UpdRecPayRecord(ieRecord);
                        if (result)
                        {
                            msg = General.Resource.Common.Success;
                        }
                        else
                        {
                            msg = General.Resource.Common.Failed;
                        }
                        }
                    }
                    
                
                }
                
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

            if (form.InvType == "主营业务收入")
            {
                form.Profit_Name = "主营业务收入";
            }
            if (form.InvType == "其他业务收入")
            {
                form.Profit_Name = "其他业务收入";
                form.DetailInvtype = form.DetailInvtype1;
            }
            if (form.InvType == "营业外收入")
            {
                form.Profit_Name = "营业外收入";
                form.DetailInvtype = form.DetailInvtype1;
            }
            if (form.InvType == "投资收益")
            {
                form.Profit_Name = "投资收益";
            }

            if (form.Pagename == "DirectMaterialPurchasingQuery/Index")
            {
                result = new IESvc().UpdIncomeRecord(form);



            }else{
                result = new IESvc().UpdIncomeRecord(form);
            }
            
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
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
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
            ViewData["C_GUID"] = Session["CurrentCompanyGuid"].ToString();
            return View("GetReceivablesDeclareCustomer");
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
        public string UpdCustomer(T_BusinessPartner partner)
        {
            partner.C_GUID = Session["CurrentCompanyGuid"].ToString();
            partner.IsCustomer = true;
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

        public ActionResult NewUpload(HttpPostedFileBase fileData, string guid, string number)
        {
            try
            {
                var files = Request.Files;
                if (files != null && files.Count > 0)
                {
                    
                    Stream fileStream = Request.Files[0].InputStream;
                    byte[] fileDataStream = new byte[Request.Files[0].ContentLength];
                    fileStream.Read(fileDataStream, 0, Request.Files[0].ContentLength);
                    T_Attachment entity = new T_Attachment();
                    entity.A_GUID = Guid.NewGuid().ToString();
                    entity.FileName = Request.Files[0].FileName;    
                    entity.FileType = Request.Files[0].ContentType;
                    entity.FR_GUID = guid;
                    entity.FlieData = fileDataStream;
                    entity.Number = number;
                    entity.FileRemark = "";

                    bool rResult = new AttachmentSvc().AddAttachment(entity);       
                    return Content(rResult.ToString());
                }
                else
                {
                    return Content("false");
                }
            }
            catch (Exception ex)
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

        public string GetAttachmentRecord(string id)
        {
            List<T_Attachment> rec = new AttachmentSvc().GetAttachment(id);
            //string json = new JavaScriptSerializer().Serialize(rec);
            string json = JsonConvert.SerializeObject(rec);
            return json;
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
