using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using BaseController;
using FMS.DAL;
using FMS.Model;
using System.Text;
using System.Web;
using System.IO;

namespace FMS.BLL
{
    /// <summary>
    /// 记录费用
    /// </summary>
    public class PaymentDeclareCostSpendingController : UserController
    {
        public PaymentDeclareCostSpendingController()
            : base("PaymentDeclareCostSpending")
        { }

        /// <summary>
        /// 列表页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewData["GUID"] = Guid.NewGuid().ToString();
            ViewData["Date"] = DateTime.Now.ToShortDateString();
            return View();
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
        /// 获取费用列表
        /// </summary>
        /// <param name="rows">页大小</param>
        /// <param name="page">页索引</param>
        /// <returns></returns>
        public string GetPaymentDeclareCostSpendingList(string rows, string page, string dateBegin, string dateEnd, string customer, string incomeGrp, string currency , string state , string invtype , string record , string business_GUID, string subBusiness_GUID,string remark)
        {
            int count = 0;
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
           // string strFormatter = "{{\"total\":\"{0}\",\"rows\":{1}}}";
            StringBuilder strJson = new StringBuilder();
            List<T_DeclareCostSpending> List = new List<T_DeclareCostSpending>();
            List = new DeclareCostSpendingSvc().GetPaymentDeclareCostSpendingList(C_GUID, 1, -1, out count, dateBegin, dateEnd, customer, incomeGrp, currency, state, invtype, record, business_GUID, subBusiness_GUID,remark);
            strJson.Append( new JavaScriptSerializer().Serialize(List));
             return strJson.ToString();
        }

        public string ChoosePaymentDeclareSupplierList(string rows, string page, string invtype)
        {
            int count = 0;
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            string strFormatter = "{{\"total\":\"{0}\",\"rows\":{1}}}";
            StringBuilder strJson = new StringBuilder();
            List<T_DeclareCostSpending> List = new List<T_DeclareCostSpending>();
            List = new DeclareCostSpendingSvc().ChoosePaymentDeclare(C_GUID, int.Parse(page), int.Parse(rows), out count, invtype);
            strJson.AppendFormat(strFormatter, count, new JavaScriptSerializer().Serialize(List));
            return strJson.ToString();
        }

        public string ChoosePaymentDeclareDepositList(string rows, string page, string invtype)
        {
            int count = 0;
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            string strFormatter = "{{\"total\":\"{0}\",\"rows\":{1}}}";
            StringBuilder strJson = new StringBuilder();
            List<T_DeclareCostSpending> List = new List<T_DeclareCostSpending>();
            List = new DeclareCostSpendingSvc().ChoosePaymentDeclare(C_GUID, int.Parse(page), int.Parse(rows), out count, invtype);
            strJson.AppendFormat(strFormatter, count, new JavaScriptSerializer().Serialize(List));
            return strJson.ToString();
        }

        public string GetList(string rows, string page, string dateBegin, string dateEnd, string paymentGrp, string state = null)
        {
            int count = 0;
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            string strFormatter = "{{\"total\":\"{0}\",\"rows\":{1}}}";
            StringBuilder strJson = new StringBuilder();
            List<T_DeclareCostSpending> List = new List<T_DeclareCostSpending>();
            List = new DeclareCostSpendingSvc().GetList(int.Parse(page), int.Parse(rows), C_GUID, dateBegin, dateEnd, paymentGrp,state, out count);
            strJson.AppendFormat(strFormatter, count, new JavaScriptSerializer().Serialize(List));
            return strJson.ToString();
        }
        
        
        public string NewGuid()
        {
            string guid = Guid.NewGuid().ToString();
            return guid;
        }
        /// <summary>
        /// 修改申报成本外支出记录
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public string UpdaDeclareCostSpending(T_DeclareCostSpending form)
        {
            bool result = false;
            string msg = string.Empty;
            if (form.InvType == "预付供应商")
            {
                form.Profit_GUID = "88C60FC8-2FCB-41CD-B721-C58A981961B0";
            }
            if (form.InvType == "支付押金和暂支借款")
            {
                form.Profit_GUID = "4F380EB2-C1BC-483C-B229-A7FAEA03D054";
            }
            if (form.InvType == "分配股利、利润")
            {
                form.Profit_GUID = "1F500FDD-1460-45DC-BE8F-39F5ACCE5D95";
            }
            if (form.InvType == "投资支出")
            {
                form.Profit_GUID = "6AE7EF4C-1E46-4839-951A-6514CAF6F6A1";
            }
            if (form.InvType == "直接物料采购" || form.InvType == "间接物料采购")
            {
                form.Profit_GUID = "41C968F2-7D51-4F9F-83B6-EC0F4381ECD0";
            }
            if (form.InvType == "资产采购")
            {
                form.Profit_GUID = "B1F44906-51D6-47F4-B6EC-7B678B5E7CD5";
            }
            result = new DeclareCostSpendingSvc().UpdaDeclareCostSpending(form);
            if (result)
            {
                msg = General.Resource.Common.Success;
            }
            else
            {
                msg = General.Resource.Common.Failed;
            }
            return msg;
        }
        public string CreateIntRecord(List<T_DeclareCostSpending> DeclareCostList) 
        {
            bool result = false;
            string msg = string.Empty;
            foreach (T_DeclareCostSpending form in DeclareCostList)
            {
                form.C_GUID = Session["CurrentCompanyGuid"].ToString();
                form.Currency = Session["Currency"].ToString();
                result = new DeclareCostSpendingSvc().UpdPayDSFL(form,"NV");
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
        /// 新增申报成本外支出记录
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public string UpdPaymentDeclareCostSpending(T_DeclareCostSpending form)
        {
            bool result = false;
            string msg = string.Empty;
            form.C_GUID = Session["CurrentCompanyGuid"].ToString();
            if (form.InvType == "预付供应商账款")
            {
                form.Profit_Name = "预付账款";
            }
            if (form.InvType == "支付押金")
            {
                form.Profit_Name = "其他应收款";
            }
            if (form.InvType == "支付暂支借款")
            {
                form.Profit_Name = "备用金";
            }
            if (form.InvType == "归还短期借款所支付的款")
            {
                form.Profit_Name = "短期借款";
            }
            if (form.InvType == "归还长期借款所支付的款")
            {
                form.Profit_Name = "长期借款";
            }
            if (form.InvType == "归还其它公司支付的押金")
            {
                form.Profit_Name = "其他应付款";
            }
            if (form.InvType == "分配利润、股利所支付的款")
            {
                form.Profit_Name = "应付利润";
            }
            if (form.InvType == "短期投资支出")
            {
                form.Profit_Name = "短期投资支出";
            }
            if (form.InvType == "长期股权投资支出")
            {
                form.Profit_Name = "长期股权投资支出";
            }
            if (form.InvType == "长期债券投资支出")
            {
                form.Profit_Name = "长期债券投资支出";
            }
            if (form.InvType == "直接物料采购" || form.InvType == "间接物料采购")
            {
                form.Profit_Name = "41C968F2-7D51-4F9F-83B6-EC0F4381ECD0";
            }
            if (form.InvType == "资产采购")
            {
                form.Profit_Name = "B1F44906-51D6-47F4-B6EC-7B678B5E7CD5";
            }
            result = new DeclareCostSpendingSvc().UpdPaymentDeclareCostSpending(form);
                if (result)
                {
                    msg = General.Resource.Common.Success;
                }
                else
                {
                    msg = General.Resource.Common.Failed;
                }
                return msg;
        }

        public string CreatPDRecord(List<T_DeclareCostSpending> payList)
        {
            bool result = false;
            string msg = string.Empty;
            foreach (T_DeclareCostSpending dc in payList) {
                dc.C_GUID = Session["CurrentCompanyGuid"].ToString();
                if (dc.InvType == "预付供应商账款")
                {
                    dc.Profit_Name = "预付账款";
                }
                if (dc.InvType == "支付押金")
                {
                    dc.Profit_Name = "其他应收款";
                }
                if (dc.InvType == "支付暂支借款")
                {
                    dc.Profit_GUID = "备用金";
                }
                if (dc.InvType == "归还短期借款所支付的款")
                {
                    dc.Profit_Name = "短期借款";
                }
                if (dc.InvType == "归还长期借款所支付的款")
                {
                    dc.Profit_Name = "长期借款";
                }
                if (dc.InvType == "归还其它公司支付的押金")
                {
                    dc.Profit_Name = "其他应付款";
                }
                if (dc.InvType == "分配利润、股利所支付的款")
                {
                    dc.Profit_Name = "应付利润";
                }
                if (dc.InvType == "短期投资支出")
                {
                    dc.Profit_Name = "短期投资支出";
                }
                if (dc.InvType == "长期股权投资支出")
                {
                    dc.Profit_Name = "长期股权投资支出";
                }
                if (dc.InvType == "长期债券投资支出")
                {
                    dc.Profit_Name = "长期债券投资支出";
                }
                if(dc.InvType == "直接物料采购" || dc.InvType == "间接物料采购"){
                    dc.Profit_GUID = "41C968F2-7D51-4F9F-83B6-EC0F4381ECD0";
                }
                if(dc.InvType == "资产采购"){
                    dc.Profit_GUID = "B1F44906-51D6-47F4-B6EC-7B678B5E7CD5";
                }
                DateTime EditThreshold = DateTime.Parse(Session["EditThreshold"].ToString());
                if (DateTime.Parse(dc.Date) <= DateTime.Now && DateTime.Parse(dc.Date) >= EditThreshold)
                {
                result = new DeclareCostSpendingSvc().UpdPaymentDeclareCostSpending(dc);
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


            }
            return string.Format("{{\"Result\":{0},\"Msg\":\"{1}\"}}"
                   , result.ToString().ToLower(), msg);
        
        }

        /// <summary>
        /// 删除成本外支出记录
        /// </summary>
        /// <param name="id">纪录标识</param>
        /// <returns></returns>
        /// <summary>
        public string DelPaymentDCostSpending(string id) {
            bool result = new DeclareCostSpendingSvc().DelPaymentDCostSpending(id);
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
        /// 更改状态
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string UpdState(string id,string state)
        {
            bool result = new DeclareCostSpendingSvc().UpdState(id, state);
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
    }
}
