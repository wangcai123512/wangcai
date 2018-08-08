using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseController;
using System.Web;
using System.Web.Mvc;
using FMS.DAL;
using FMS.Model;

namespace FMS.BLL
{
    public class ReceivablesWriteController: UserController
    {
        public ReceivablesWriteController() : base("ReceivablesWrite") { }

        public ActionResult Index()
        {
            return View();
        }
        public string UpdPayInyType(List<T_RecPayRecord> payList, string  SumAmount,string DisAmount)
        {
            //typedts = typedts + ";" + typedtsdts;
            bool result = false;
            string msg = string.Empty;
            foreach (T_RecPayRecord recPayRecord in payList)
            {
                recPayRecord.RP_Flag = "R";
               
               
                    switch (recPayRecord.InvTypeDts) {
                        case "销售商品/提供服务的收款":
                            recPayRecord.CFItemGuid = "97B181C8-D807-4BF0-8D8D-B23273E7FEFE";
                            recPayRecord.SubjectName = "应收账款";
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
                            recPayRecord.CFItemGuid = "97B181C8-D807-4BF0-8D8D-B23273E7FEFE";
                            recPayRecord.SubjectName = "其他应付款";
                            break;
                        case "收到的其它公司支付的押金":
                            recPayRecord.CFItemGuid = "97B181C8-D807-4BF0-8D8D-B23273E7FEFE";
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
                            recPayRecord.CFItemGuid = "97B181C8-D807-4BF0-8D8D-B23273E7FEFE";
                            recPayRecord.SubjectName = "";
                            break;
                        case "营业外收入的收款":
                            recPayRecord.CFItemGuid = "F6330595-F588-46B0-8998-752C7A1D774B";
                            recPayRecord.SubjectName = "应收账款";
                            break;
                        case "其他业务收入的收款":
                            recPayRecord.CFItemGuid = "F6330595-F588-46B0-8998-752C7A1D774B";
                            recPayRecord.SubjectName = "应收账款";
                            break;
                        case "收到的税费返还":
                            recPayRecord.CFItemGuid = "E90ABB77-27D2-48D7-9A20-6F8862F0BE11";
                            recPayRecord.SubjectName = "应收账款";
                            break;



                        case "":
                            break;
                    }
                    string check = null; 
                string[] temp = recPayRecord.IE_GUID.Split(new char[] { ',' });
                if (Convert.ToDecimal(SumAmount) == Convert.ToDecimal(DisAmount))
                    {
                        recPayRecord.Record = "已销账";
                        recPayRecord.DisAmount1 = 0;
                        check = "EQ";
                        foreach (var a in temp)
                        {
                            result = new RecPayRecordSvc().UpdIERP(a, recPayRecord.RP_GUID, check, recPayRecord.Mark, recPayRecord.RP_Flag, recPayRecord.InvTypeDts);
                        }
                    }

                if (Convert.ToDecimal(SumAmount) >Convert.ToDecimal(DisAmount))
                    {
                        recPayRecord.Record = "未销账";
                        recPayRecord.DisAmount1 = Convert.ToDecimal(SumAmount) - Convert.ToDecimal(DisAmount);
                        check = "LESS";
                        foreach (var a in temp)
                        {
                            result = new RecPayRecordSvc().UpdIERP(a, recPayRecord.RP_GUID, check, recPayRecord.Mark, recPayRecord.RP_Flag, recPayRecord.InvTypeDts);
                        }
                     }
                if (Convert.ToDecimal(SumAmount) < Convert.ToDecimal(DisAmount))
                    {
                        recPayRecord.Record = "已销账";
                        recPayRecord.DisAmount1 = 0;
                        recPayRecord.RP_Flag = "R";
                        check = "MORE";
                        foreach (var a in temp)
                        {
                            result = new RecPayRecordSvc().UpdIERP(a, recPayRecord.RP_GUID, check, recPayRecord.Mark, recPayRecord.RP_Flag, recPayRecord.InvTypeDts);
                        }
                        if (result)
                        {
                            result = new RecPayRecordSvc().UpdIERPMore(recPayRecord, SumAmount, DisAmount);
                        }
                    }
          
                result = new RecPayRecordSvc().UpdRecpayType(recPayRecord);

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
              
    }
}
