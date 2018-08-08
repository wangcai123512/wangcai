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

using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace FMS.BLL
{
    /// <summary>
    /// 记录收款
    /// </summary>
    public class ReceivablesRecordController : UserController
    {
        public ReceivablesRecordController()
            : base("ReceivablesRecord")
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
        /// 收款信息页
        /// </summary>
        /// <param name="id">收款纪录标识</param>
        /// <returns></returns>
        public ActionResult ReceivablesRecord(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return View(new T_RecPayRecord() { RP_GUID = Guid.NewGuid().ToString() });
            }
            else
            {
                string C_GUID = Session["CurrentCompanyGuid"].ToString();
                return View(new RecPayRecordSvc().GetReceivablesRecord(id, C_GUID));
            }
        }

        public string NewGuid()
        {
            string guid = Guid.NewGuid().ToString();
            return guid;
        }

        /// <summary>
        /// 归档页
        /// </summary>
        /// <param name="id">收款纪录标识</param>
        /// <returns></returns>
        public ActionResult Pigeonhole(string id)
        {
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            return View(new RecPayRecordSvc().GetReceivablesRecord(id, C_GUID));
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
        /// 数据导入
        /// </summary>
        /// <returns></returns>
        public ActionResult UpRRFile()
        {
            ViewData["C_GUID"] = Session["CurrentCompanyGuid"].ToString();
            return View("UpRRFile");
        }

        /// <summary>
        /// 选择应收页面
        /// </summary>
        /// <param name="id">付款方标识</param>
        /// <returns></returns>
        public ActionResult ChooseReceivablesRecord(string id)
        {
            ViewData["C_GUID"] = Session["CurrentCompanyGuid"].ToString();
            ViewData["RPer"] = id;
            return View("ChooseReceivablesRecord");
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

        /// <summary>
        /// 获取应收款列表
        /// </summary>
        /// <param name="id">付款方标识</param>
        /// <returns></returns>
        public string GetChooseReceivablesRecord(string id)
        {
            //int count = 0;
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            //string strFormatter = "{{\"total\":\"{0}\",\"rows\":{1}}}";
            StringBuilder strJson = new StringBuilder();
            List<T_Receivables> Receivables = new List<T_Receivables>();
            Receivables = new RecPayRecordSvc().GetChooseReceivablesRecord(id, C_GUID);
            string json = new JavaScriptSerializer().Serialize(Receivables);
            return json;
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="rows">行数组</param>
        /// <returns></returns>
        public string GetIncomeToReceivables(string rows)
        {
            int count = 0;
            string str = rows;
            string[] sArray = str.Split(','); 
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            string strFormatter = "{{\"total\":\"{0}\",\"rows\":{1}}}";
            StringBuilder strJson = new StringBuilder();
            List<T_IERecord> IERecords = new List<T_IERecord>();
            for (int i = 0; i < sArray.Length; i++)
            {
                IERecords = new IESvc().GetIncomeToReceivables(sArray[i], C_GUID, "I");
                count++;
            }
            strJson.AppendFormat(strFormatter, count, new JavaScriptSerializer().Serialize(IERecords));
            return strJson.ToString();

        }

        /// <summary>
        /// 获取付款纪录列表
        /// </summary>
        /// <param name="rows">页大小</param>
        /// <param name="page">页索引</param>
        /// <returns></returns>
        public string GetReceivablesList(string rows, string page)
        {
            int count = 0;
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            string strFormatter = "{{\"total\":\"{0}\",\"rows\":{1}}}";
            StringBuilder strJson = new StringBuilder();
            List<T_RecPayRecord> RecPayRecord = new List<T_RecPayRecord>();
            RecPayRecord = new RecPayRecordSvc().GetReceivablesRecord(C_GUID, int.Parse(page), int.Parse(rows), out count);
            strJson.AppendFormat(strFormatter, count, new JavaScriptSerializer().Serialize(RecPayRecord));
            return strJson.ToString();

        }

        /// <summary>
        /// 付款纪录页（只读）
        /// </summary>
        /// <param name="id">收款纪录标识</param>
        /// <returns></returns>
        public ActionResult CheckReceivablesRecord(string id)
        {
            return View(new RecPayRecordSvc().GetReceivablesRecord(id, Session["CurrentCompanyGuid"].ToString()));
        }

        /// <summary>
        /// 更新付款纪录
        /// </summary>
        /// <param name="rec">付款纪录对象</param>
        /// <returns></returns>
        public string UpdReceivablesRecord(T_RecPayRecord rec)
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
        /// 更改状态
        /// </summary>
        /// <param name="id">收入纪录标识</param>
        /// <returns></returns>
        public string UpdState(string id)
        {
            bool result = new DeclareCustomerSvc().UpdState(id, "已收");
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
        /// 更新付款纪录
        /// </summary>
        /// <param name="rec">付款纪录对象</param>
        /// <returns></returns>
        public string UpdReceivablesRecordDts(List<T_RecPayRecord> payList, string addstyle)
        {
            bool result = false;
            string msg = string.Empty;
            foreach (T_RecPayRecord recPayRecord in payList)
            {
                if (recPayRecord.BA_GUID == null)
                {
                    recPayRecord.Creator = base.userData.LoginFullName;
                    recPayRecord.C_GUID = Session["CurrentCompanyGuid"].ToString();
                    recPayRecord.B_GUID = "";
                    recPayRecord.BA_GUID = "";
                    recPayRecord.RP_Flag = "R";
                }
                else {
                    recPayRecord.Creator = base.userData.LoginFullName;
                    recPayRecord.C_GUID = Session["CurrentCompanyGuid"].ToString();
                    T_BankAccount ba = new BankAccountSvc().GetBank(Session["CurrentCompanyGuid"].ToString(), null, recPayRecord.BA_GUID, null);
                    recPayRecord.B_GUID = ba.B_GUID;
                    recPayRecord.BA_GUID = ba.BA_GUID;
                    recPayRecord.RP_Flag = "R";
                }

                //if (recPayRecord.RPType == "0F15C212-A858-4AD5-9B13-E51502994818")
                //{
                //    recPayRecord.RPType = "库存现金";
                //}
                //else if (recPayRecord.RPType == "6704EDDF-9D23-41FC-A870-827CE2F4D5DB")
                //{
                //    recPayRecord.RPType = "银行存款";
                //}
                //else
                //{
                //    recPayRecord.RPType = "其他货币资金";
                //}
               
                //归账和销账
                if (addstyle == "直接新增")
                {
                    switch (recPayRecord.InvTypeDts) {
                        case "销售商品/提供服务的收款":
                            recPayRecord.CFItemGuid = "97B181C8-D807-4BF0-8D8D-B23273E7FEFE";
                            switch (recPayRecord.PayCategory)
                            {
                                case "银行存款":
                                    recPayRecord.SubjectName = "应收账款";
                                    break;
                                case "库存现金":
                                    recPayRecord.SubjectName = "应收账款";
                                    break;
                                case "其他货币资金":
                                    recPayRecord.SubjectName = "应收票据";
                                    break;
                                case "":
                                    break;
                            }
                        break;
                        case "预收客户账款":
                        recPayRecord.CFItemGuid = "F6330595-F588-46B0-8998-752C7A1D774B";
                        recPayRecord.SubjectName = "预收账款";
                        break;
                        case "收回公司支出的暂支借款":
                        recPayRecord.CFItemGuid = "F6330595-F588-46B0-8998-752C7A1D774B";
                        recPayRecord.SubjectName = "备用金";
                        break;
                        case "收回公司支出的押金":
                        recPayRecord.CFItemGuid = "F6330595-F588-46B0-8998-752C7A1D774B";
                        recPayRecord.SubjectName = "其他应付款";
                        break;
                        case "收到的其它公司支付的押金":
                        recPayRecord.CFItemGuid = "F6330595-F588-46B0-8998-752C7A1D774B";
                        recPayRecord.SubjectName = "其他应付款";
                        break;
                        case "取得投资收益的利息的收款":
                        recPayRecord.CFItemGuid = "C55B2A4E-129B-407B-AC0B-14C091587D45";
                        recPayRecord.SubjectName = "应收利息";
                        break;
                        case "取得投资收益的股利的收款":
                        recPayRecord.CFItemGuid = "C55B2A4E-129B-407B-AC0B-14C091587D45";
                        recPayRecord.SubjectName = "应收股利";
                        break;
                        case "收回短期投资的本金金额内的款":
                        recPayRecord.CFItemGuid = "496F9D4D-F71B-437A-9EA0-26107D3449C3";
                        recPayRecord.SubjectName = "短期投资";
                        break;
                        case "收回长期债券投资的本金金额内的款":
                        recPayRecord.CFItemGuid = "496F9D4D-F71B-437A-9EA0-26107D3449C3";
                        recPayRecord.SubjectName = "长期债券投资";
                        break;
                        case "收回长期股权投资的本金金额内的款":
                        recPayRecord.CFItemGuid = "496F9D4D-F71B-437A-9EA0-26107D3449C3";
                        recPayRecord.SubjectName = "长期股权投资";
                        break;
                        case "处置固定资产所收回的款":
                        recPayRecord.CFItemGuid = "56B5FE80-EE8D-4E52-A2E8-8EE9A5F5BB73";
                        recPayRecord.SubjectName = "固定资产";
                        break;
                        case "处置无形资产所收回的款":
                        recPayRecord.CFItemGuid = "56B5FE80-EE8D-4E52-A2E8-8EE9A5F5BB73";
                        recPayRecord.SubjectName = "无形资产";
                        break;
                        case "处置其他长期资产所收回的款":
                        recPayRecord.CFItemGuid = "56B5FE80-EE8D-4E52-A2E8-8EE9A5F5BB73";
                        recPayRecord.SubjectName = "其他长期资产";
                        break;
                        case "收到的其他与投资活动有关的款":
                        recPayRecord.CFItemGuid = "0D3AED4A-0904-450B-9919-A952CD2E9534";
                        recPayRecord.SubjectName = "";
                        break;
                        case "吸收投资的收款(注册资本金额以内部分)":
                        recPayRecord.CFItemGuid = "77A24D5F-3E0C-4211-A552-191FEE0E06FD";
                        recPayRecord.SubjectName = "实收资本";
                        break;
                        case "吸收投资的收款(超出注册资本金额部分)":
                        recPayRecord.CFItemGuid = "77A24D5F-3E0C-4211-A552-191FEE0E06FD";
                        recPayRecord.SubjectName = "资本公积";
                        break;
                        case "短期借款所获得的收款":
                        recPayRecord.CFItemGuid = "AD2E5437-0917-43E1-807C-41CA6751360F";
                        recPayRecord.SubjectName = "短期借款";
                        break;
                        case "长期借款所获得的收款":
                        recPayRecord.CFItemGuid = "AD2E5437-0917-43E1-807C-41CA6751360F";
                        recPayRecord.SubjectName = "长期借款";
                        break;
                        case "收到的其他与筹资活动有关的款":
                        //recPayRecord.CFItemGuid = "97B181C8-D807-4BF0-8D8D-B23273E7FEFE";
                        recPayRecord.CFItemGuid = "106B9F2C-24A5-48B5-9621-418D00A7A75A";
                        recPayRecord.SubjectName = "";
                        break;
                        case "营业外收入的收款":
                        recPayRecord.CFItemGuid = "F6330595-F588-46B0-8998-752C7A1D774B";
                        switch (recPayRecord.PayCategory)
                        {
                            case "银行存款":
                                recPayRecord.SubjectName = "应收账款";
                                break;
                            case "库存现金":
                                recPayRecord.SubjectName = "应收账款";
                                break;
                            case "其他货币资金":
                                recPayRecord.SubjectName = "应收票据";
                                break;
                            case "":
                                break;
                        }
                        break;
                        case "其他业务收入的收款":
                        recPayRecord.CFItemGuid = "F6330595-F588-46B0-8998-752C7A1D774B";
                        switch (recPayRecord.PayCategory)
                        {
                            case "银行存款":
                                recPayRecord.SubjectName = "应收账款";
                                break;
                            case "库存现金":
                                recPayRecord.SubjectName = "应收账款";
                                break;
                            case "其他货币资金":
                                recPayRecord.SubjectName = "应收票据";
                                break;
                            case "":
                                break;
                        }
                        break;
                        case "收到的税费返还":
                        recPayRecord.CFItemGuid = "E90ABB77-27D2-48D7-9A20-6F8862F0BE11";
                        //recPayRecord.SubjectName = "应收账款";
                        switch (recPayRecord.PayCategory)
                        {
                            case "银行存款":
                                recPayRecord.SubjectName = "应收账款";
                                break;
                            case "库存现金":
                                recPayRecord.SubjectName = "应收账款";
                                break;
                            case "其他货币资金":
                                recPayRecord.SubjectName = "应收票据";
                                break;
                            case "":
                                break;
                        }
                        break;
                       
                       
                       
                        case "":
                        break;
                    
                    }
                    
                }
                if (addstyle == "收入获取")
                {
                    switch (recPayRecord.InvTypeDts)
                    {
                        case "销售商品/提供服务的收款":
                            recPayRecord.CFItemGuid = "97B181C8-D807-4BF0-8D8D-B23273E7FEFE";
                            switch (recPayRecord.PayCategory)
                            {
                                case "银行存款":
                                    recPayRecord.SubjectName = "应收账款";
                                    break;
                                case "库存现金":
                                    recPayRecord.SubjectName = "应收账款";
                                    break;
                                case "其他货币资金":
                                    recPayRecord.SubjectName = "应收票据";
                                    break;
                                case "":
                                    break;
                            }
                            break;
                        case "营业外收入的收款":
                            recPayRecord.CFItemGuid = "F6330595-F588-46B0-8998-752C7A1D774B";
                            switch (recPayRecord.PayCategory)
                            {
                                case "银行存款":
                                    recPayRecord.SubjectName = "应收账款";
                                    break;
                                case "库存现金":
                                    recPayRecord.SubjectName = "应收账款";
                                    break;
                                case "其他货币资金":
                                    recPayRecord.SubjectName = "应收票据";
                                    break;
                                case "":
                                    break;
                            }
                            break;
                        case "其他业务收入的收款":
                            recPayRecord.CFItemGuid = "F6330595-F588-46B0-8998-752C7A1D774B";
                            switch (recPayRecord.PayCategory)
                            {
                                case "银行存款":
                                    recPayRecord.SubjectName = "应收账款";
                                    break;
                                case "库存现金":
                                    recPayRecord.SubjectName = "应收账款";
                                    break;
                                case "其他货币资金":
                                    recPayRecord.SubjectName = "应收票据";
                                    break;
                                case "":
                                    break;
                            }
                            break;
                        case "取得投资收益的利息的收款":
                            recPayRecord.CFItemGuid = "C55B2A4E-129B-407B-AC0B-14C091587D45";
                            recPayRecord.SubjectName = "应收利息";
                            break;
                        case "取得投资收益的股利的收款":
                            recPayRecord.CFItemGuid = "C55B2A4E-129B-407B-AC0B-14C091587D45";
                            recPayRecord.SubjectName = "应收股利";
                            break;
                        case "收到的税费返还":
                            recPayRecord.CFItemGuid = "E90ABB77-27D2-48D7-9A20-6F8862F0BE11";
                            switch (recPayRecord.PayCategory)
                            {
                                case "银行存款":
                                    recPayRecord.SubjectName = "应收账款";
                                    break;
                                case "库存现金":
                                    recPayRecord.SubjectName = "应收账款";
                                    break;
                                case "其他货币资金":
                                    recPayRecord.SubjectName = "应收票据";
                                    break;
                                case "":
                                    break;
                            }
                            break;
                    } 
                   
                }
                if (addstyle == "收入计入收款")
                {
                   
                    recPayRecord.InvType = "经营活动收款";
                    recPayRecord.InvTypeDts = "销售商品/提供服务的收款";
                    recPayRecord.CFItemGuid = "97B181C8-D807-4BF0-8D8D-B23273E7FEFE";

                    switch (recPayRecord.PayCategory)
                    {
                        case "银行存款":
                            recPayRecord.SubjectName = "应收账款";
                            break;
                        case "库存现金":
                            recPayRecord.SubjectName = "应收账款";
                            break;
                        case "其他货币资金":
                            recPayRecord.SubjectName = "应收票据";
                            break;
                        case "":
                            break;
                    }
                }
                if (addstyle == "预收客户账款")
                {
                  recPayRecord.SubjectName = "预收账款";
                  recPayRecord.CFItemGuid = "F6330595-F588-46B0-8998-752C7A1D774B";
                }
                if (addstyle == "收取投资款(注册资本金额以内部分)" || addstyle == "收取投资的收款(超出注册资本金额部分)")
                {
                    recPayRecord.CFItemGuid = "77A24D5F-3E0C-4211-A552-191FEE0E06FD";
                    switch (addstyle)
                    {
                        case "收取投资款(注册资本金额以内部分)":
                            recPayRecord.SubjectName = "实收资本";
                            break;
                        case "收取投资的收款(超出注册资本金额部分)":
                            recPayRecord.SubjectName = "资本公积";
                            break;
                    }
                }

                if (addstyle == "收回短期投资的本金金额内的款" || addstyle == "收回长期债券投资的本金金额内的款" || addstyle == "收回长期股权投资的本金金额内的款")
                {

                    recPayRecord.CFItemGuid = "496F9D4D-F71B-437A-9EA0-26107D3449C3";
                    switch (addstyle)
                    {
                        case "收回短期投资的本金金额内的款":
                            recPayRecord.SubjectName = "短期投资";
                            break;
                        case "收回长期债券投资的本金金额内的款":
                            recPayRecord.SubjectName = "长期债券投资";
                            break;
                        case "收回长期股权投资的本金金额内的款":
                            recPayRecord.SubjectName = "长期股权投资";
                            break;
                    }
                }
                if (addstyle == "收回公司支出的押金" || addstyle == "收到的其他公司支付的押金" || addstyle == "收回公司支出的暂支借款")
                {

                    recPayRecord.CFItemGuid = "F6330595-F588-46B0-8998-752C7A1D774B";
                    switch (addstyle) {
                        case "收回公司支出的押金":
                            recPayRecord.SubjectName = "其他应收款";
                            break;
                        case "收到的其他公司支付的押金":
                            recPayRecord.SubjectName = "其他应付款";
                            break;
                        case "收回公司支出的暂支借款":
                            recPayRecord.SubjectName = "备用金";
                            break;
                    }
                }
                if (addstyle == "短期借款所获得的收款" || addstyle == "长期借款所获得的收款")
                {
                    switch (addstyle)
                    {
                        case "短期借款所获得的收款":
                            recPayRecord.SubjectName = "短期借款";
                            break;
                        case "长期借款所获得的收款":
                            recPayRecord.SubjectName = "长期借款";
                            break;
                       }
                    recPayRecord.CFItemGuid = "AD2E5437-0917-43E1-807C-41CA6751360F";
                }

                //if (addstyle == "吸取投资")
                //{
                    
                  
                //    recPayRecord.CFItemGuid = "77A24D5F-3E0C-4211-A552-191FEE0E06FD";
                //}
                //if (addstyle == "借款")
                //{

                //    recPayRecord.CFItemGuid = "AD2E5437-0917-43E1-807C-41CA6751360F";
                //}
                if (addstyle == "其他与筹资活动有关的收款")
                {



                    recPayRecord.CFItemGuid = "106B9F2C-24A5-48B5-9621-418D00A7A75A";
                }
                recPayRecord.CreateDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                DateTime EditThreshold = DateTime.Parse(Session["EditThreshold"].ToString());
                DateTime checkDate;
                DateTime.TryParse(recPayRecord.Date, out checkDate);
                if (checkDate <= DateTime.Now && checkDate >= EditThreshold)
                {
                    string check = null;
                    if (recPayRecord.IE_GUID != null)
                    {
                        string[] temp = recPayRecord.IE_GUID.Split(new char[] { ',' });
                        if (recPayRecord.SumAmount == recPayRecord.DisAmount)
                        {
                            recPayRecord.Record = "已销账";
                            recPayRecord.DisAmount1 = 0;
                            check = "EQ";
                            foreach (var a in temp)
                            {
                                result = new RecPayRecordSvc().UpdIERP(a, recPayRecord.RP_GUID, check, recPayRecord.Mark, recPayRecord.RP_Flag, recPayRecord.InvTypeDts);
                            }
                        }

                        if (recPayRecord.SumAmount > recPayRecord.DisAmount)
                        {
                            recPayRecord.Record = "未销账";
                            recPayRecord.DisAmount1 = recPayRecord.SumAmount - recPayRecord.DisAmount;
                            check = "LESS";
                            foreach (var a in temp)
                            {
                                result = new RecPayRecordSvc().UpdIERP(a, recPayRecord.RP_GUID, check, recPayRecord.Mark, recPayRecord.RP_Flag, recPayRecord.InvTypeDts);
                            }
                        }
                        if (recPayRecord.SumAmount < recPayRecord.DisAmount)
                        {
                            recPayRecord.Record = "已销账";
                            recPayRecord.RP_Flag = "R";
                            check = "MORE";
                            recPayRecord.DisAmount1 = 0;
                            foreach (var a in temp)
                            {
                                result = new RecPayRecordSvc().UpdIERP(a, recPayRecord.RP_GUID, check, recPayRecord.Mark, recPayRecord.RP_Flag, recPayRecord.InvTypeDts);
                            }
                            if (result)
                            {
                                result = new RecPayRecordSvc().UpdIERPMore(recPayRecord);
                            }
                        }
                        if (result)
                        {
                            result = new RecPayRecordSvc().UpdReceivablesRecord(recPayRecord);
                        }
                    }
                    else {
                        recPayRecord.DisAmount1 = recPayRecord.SumAmount;
                        result = new RecPayRecordSvc().UpdReceivablesRecord(recPayRecord);
                    }
                    if (result)
                    {
                        msg = General.Resource.Common.Success;
                        //if (addstyle == "收入获取")
                        //{
                        //    UpdIncomeState(recPayRecord.IE_GUID);
                        //}
                        //else {
                        //    UpdState(recPayRecord.IE_GUID);
                        //}
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
            }
            return string.Format("{{\"Result\":{0},\"Msg\":\"{1}\"}}"
                , result.ToString().ToLower(), msg);
        }
      

        public string UpdIncomeState(string id)
        {
            bool result = new RecPayRecordSvc().UpdIEState("I", id, "已收");
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

        public ActionResult NewUpload(HttpPostedFileBase fileData, string guid, string number=null)
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
        /// 删除付款纪录
        /// </summary>
        /// <param name="id">付款纪录标识</param>
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
        /// 下载附件
        /// </summary>
        /// <param name="fileID">fileID 图片ID</param>
        /// <returns></returns>
        public FileResult DownLoadFile(string fileID)
        {
            AttachmentSvc attSv = new AttachmentSvc();
            var entity = attSv.GetAttachmentById(fileID);
            //从数据库查找
            return File(entity.FlieData, entity.FileType, entity.FileName);
        }

        /// <summary>
        /// 批量导入excel数据
        /// </summary>
         public ActionResult Upexcel(FormCollection from)
        {
            // var fl = Request.Files;
            // Stream file = Request.Files[0].InputStream;
            //if (files != null && files.Count > 0)
            //{

            //    Stream fileStream = Request.Files[0].InputStream;
            //    byte[] fileDataStream = new byte[Request.Files[0].ContentLength];
            //    fileStream.Read(fileDataStream, 0, Request.Files[0].ContentLength);
            //}
            HttpPostedFileBase file = Request.Files["upload"];
            string result = string.Empty;
            if (file == null || file.ContentLength <= 0){
                
            }
            else{
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
                        string baguid = (new BankAccountSvc().GetBankAccountDts(Session["CurrentCompanyGuid"].ToString(),bguid, dr[i][5].ToString())).ToString();

                        string cguid = Session["CurrentCompanyGuid"].ToString();
                        string creator = base.userData.LoginFullName;
                        DateTime createdate = DateTime.Now;

                        DBHelper dh = new DBHelper();
                        dh.strCmd = "SP_UpdRecPayRecord";
                        dh.AddPare("@ID", SqlDbType.NVarChar, 40, Guid.NewGuid().ToString());
                        dh.AddPare("@Flag", SqlDbType.NVarChar, 1, "R");
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
                catch (Exception)
                {
                    result = "导入失败，请检查EXCEL格式是否错误！";
                }
            }
            JsonResult json = new JsonResult();
            json.Data = result;
            return json;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
         public ActionResult UpexcelToDatagrid()
        {
            HttpPostedFileBase file = Request.Files["upload"];
            string result = string.Empty;
            DataTable RR = new DataTable();
            RR.Columns.Add("RP_GUID");
            RR.Columns.Add("RPerName");
            RR.Columns.Add("Money");
            RR.Columns.Add("Date");
            RR.Columns.Add("Remark");
            RR.Columns.Add("AddStyle");
            if (file == null || file.ContentLength <= 0)
            {
                result = "请选择你要导入的Excel文件";
            }
            else
            {
                try
                {
                    Workbook workbook = new Workbook(file.InputStream);
                    Cells cells = workbook.Worksheets[0].Cells;
                    DataTable tab = cells.ExportDataTable(1, 0, cells.Rows.Count - 1, cells.MaxDisplayRange.ColumnCount);
                    int rowsnum = tab.Rows.Count;
                    if (rowsnum == 0)
                    {
                        result = "导入的Excel为空请重新选择!";
                    }
                    for (int i = 0; i < rowsnum; i++)
                    {
                        RR.Rows.Add(RR.NewRow());
                        RR.Rows[i]["RP_GUID"] = Guid.NewGuid().ToString();
                        RR.Rows[i]["RPerName"] = tab.Rows[i][0].ToString();
                        RR.Rows[i]["Date"] = tab.Rows[i][1].ToString();
                        RR.Rows[i]["Money"] = tab.Rows[i][2].ToString();
                        RR.Rows[i]["Remark"] = tab.Rows[i][6].ToString();
                    }
                }
                catch (Exception)
                {
                    result = "导入失败，请检查EXCEL格式是否错误！";
                }
            }
            JsonResult jsonresult = new JsonResult();
            //jsonresult.Data = RR;
            jsonresult.Data = result;
            return jsonresult;
        }

         /// <summary>
         /// 主营业务收入、非主营业务收入的应收款列表
         /// </summary>
         /// <param name="rows">页大小</param>
         /// <param name="page">页索引</param>
         /// <param name="id">收款方标识</param>
         /// <returns></returns>
         public string GetUnionList(string rows, string page, string customer, string main, string nomain, string other, string investment)
         {
             int count = 0;
             string C_GUID = Session["CurrentCompanyGuid"].ToString();
             StringBuilder strJson = new StringBuilder();
             List<T_IERecord> List = new List<T_IERecord>();
             List = new RecPayRecordSvc().GetUnionDRec(C_GUID, 1, -1, out count, customer,main, nomain, other, investment);
             return new JavaScriptSerializer().Serialize(List);

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
                try { 
                   ba = new BankAccountSvc().GetBankDt(Session["CurrentCompanyGuid"].ToString(), dr[i][0].ToString());
                }
                catch {
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
                    result = "导入失败，无此客户";
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
                    
                    case "销售商品/提供服务的收款":
                        CFItemGuid = "97B181C8-D807-4BF0-8D8D-B23273E7FEFE";
                    break;
                    case "押金返还、暂支还款":
                        CFItemGuid = "F6330595-F588-46B0-8998-752C7A1D735A";
                    break;
                    case "收到经营活动有关的客户预付款":
                        CFItemGuid = "F6330595-F588-46B0-8998-752C7A1D774B";
                    break;
                    case "收到的税费返还":
                        CFItemGuid = "E90ABB77-27D2-48D7-9A20-6F8862F0BE11";
                    break;
                    case "收回投资所收到的款":
                        CFItemGuid = "496F9D4D-F71B-437A-9EA0-26107D3449C3";
                    break;
                    case "取得投资收益的收款":
                        CFItemGuid = "C55B2A4E-129B-407B-AC0B-14C091587D45";
                    break;
                    case "处置固定资产、无形资产和其他长期资产所收回的款":
                        CFItemGuid = "56B5FE80-EE8D-4E52-A2E8-8EE9A5F5BB73";
                    break;
                    case "吸取投资的收款":
                        CFItemGuid = "77A24D5F-3E0C-4211-A552-191FEE0E06FD";
                    break;
                    case "借款所获得的收款":
                        CFItemGuid = "AD2E5437-0917-43E1-807C-41CA6751360F";
                    break;
                    case "其他与筹资活动有关的收款":
                        CFItemGuid = "106B9F2C-24A5-48B5-9621-418D00A7A75A";
                    break;
                    case "":
                    break;

                }
                DBHelper dh = new DBHelper();
                dh.strCmd = "SP_UpdRecPayRecord";
                dh.AddPare("@ID", SqlDbType.NVarChar, 40, Guid.NewGuid().ToString());
                dh.AddPare("@Flag", SqlDbType.NVarChar, 1, "R");
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
    }
}
