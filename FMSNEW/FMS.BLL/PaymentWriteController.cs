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
    public class PaymentWriteController: UserController 
    {
        public PaymentWriteController()
            : base("PaymentWrite") { }

        /// <summary>
        /// 列表页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }
        public string UpdPayInyType(List<T_RecPayRecord> payList,string  SumAmount,string DisAmount)
        {
            //typedts = typedts + ";" + typedtsdts;
            bool result = false;
            string msg = string.Empty;
            foreach (T_RecPayRecord recPayRecord in payList) {
                recPayRecord.RP_Flag = "P";
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
                        break;
                    case "支付其他业务成本":
                        recPayRecord.CFItemGuid = "DE7D81B9-680B-4011-A771-C8B327A549E7";
                        break;
                    case "支付营业外支出":
                        recPayRecord.CFItemGuid = "DE7D81B9-680B-4011-A771-C8B327A549E7";
                        break;
                    case "支付暂支借款":
                        recPayRecord.CFItemGuid = "DE7D81B9-680B-4011-A771-C8B327A549E7";
                        break;
                    case "支付押金":
                        recPayRecord.CFItemGuid = "DE7D81B9-680B-4011-A771-C8B327A549E7";
                        break;
                    case "归还其它公司支付的押金":
                        recPayRecord.CFItemGuid = "DE7D81B9-680B-4011-A771-C8B327A549E7";
                        break;
                    case "短期投资支出":
                        recPayRecord.CFItemGuid = "049F1C6D-49EA-4E2D-93FD-2DABEBED666C";
                        break;
                    case "长期债券投资支出":
                        recPayRecord.CFItemGuid = "049F1C6D-49EA-4E2D-93FD-2DABEBED666C";
                        break;
                    case "长期股权投资支出":
                        recPayRecord.CFItemGuid = "049F1C6D-49EA-4E2D-93FD-2DABEBED666C";
                        break;
                    case "购买固定资产所支付的款":
                        recPayRecord.CFItemGuid = "EA46CFA0-5D41-4FF1-9BF8-9A36CB8F1F11";
                        break;
                    case "购买无形资产所支付的款":
                        recPayRecord.CFItemGuid = "EA46CFA0-5D41-4FF1-9BF8-9A36CB8F1F11";
                        break;
                    case "购买其他长期资产所支付的款":
                        recPayRecord.CFItemGuid = "EA46CFA0-5D41-4FF1-9BF8-9A36CB8F1F11";
                        break;
                    case "归还短期借款所支付的款":
                        recPayRecord.CFItemGuid = "DD7BCD86-150E-4E62-B6CC-21EF341B41F1";
                        break;
                    case "归还长期借款所支付的款":
                        recPayRecord.CFItemGuid = "DD7BCD86-150E-4E62-B6CC-21EF341B41F1";
                        break;
                    case "分配利润、股利所支付的款":
                        recPayRecord.CFItemGuid = "5BDE7476-F268-4A62-8CC3-D426D51E253D";
                        break;
                    case "偿付利息所支付的款":
                        recPayRecord.CFItemGuid = "5BDE7476-F268-4A62-8CC3-D426D51E253D";
                        break;
                    case "支付职工薪酬":
                        recPayRecord.CFItemGuid = "70765251-FA58-432F-BCC5-122EF3581102";
                        break;
                    case "支付的各项税费":
                        recPayRecord.CFItemGuid = "E4F16AB4-8DFE-42E1-8A7F-0CB342CF9C73";
                        break;
                    case "支付其他与筹资活动有关的款":
                        recPayRecord.CFItemGuid = "8254B1DA-91CC-4CA5-B7F0-9AC5D2653865";
                        break;
                    case "暂支借款":
                        recPayRecord.CFItemGuid = "DE7D81B9-680B-4011-A771-C8B327A549E7";
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

                if (Convert.ToDecimal(SumAmount) > Convert.ToDecimal(DisAmount))
                {
                    recPayRecord.Record = "未销账";
                    recPayRecord.DisAmount1 = Convert.ToDecimal(SumAmount) - Convert.ToDecimal(DisAmount); ;
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
                    check = "MORE";
                    foreach (var a in temp)
                    {
                        result = new RecPayRecordSvc().UpdIERP(a, recPayRecord.RP_GUID, check, recPayRecord.Mark, recPayRecord.RP_Flag, recPayRecord.InvTypeDts);
                    }
                    if (result)
                    {
                        result = new RecPayRecordSvc().UpdIERPMore(recPayRecord,SumAmount, DisAmount);
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
