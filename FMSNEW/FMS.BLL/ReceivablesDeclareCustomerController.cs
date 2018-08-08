using System;
using System.Collections.Generic;
using System.Globalization;
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
    public class ReceivablesDeclareCustomerController : UserController
    {
        public ReceivablesDeclareCustomerController()
            : base("ReceivablesDeclareCustomer")
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

        public ActionResult Query()
        {
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
        /// 获取申报
        /// </summary>
        /// <param name="rows">页大小</param>
        /// <param name="page">页索引</param>
        /// <returns></returns>
        public string GetReceivablesDeclareCustomerList(string dateBegin, string dateEnd, string customer, string state, string incomeGrp, string currency, string business_GUID, string subBusiness_GUID)
        {
            int count = 0;
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            //string strFormatter = "{{\"total\":\"{0}\",\"rows\":{1}}}";
            StringBuilder strJson = new StringBuilder();
            List<T_DeclareCustomer> List = new List<T_DeclareCustomer>();
            List = new DeclareCustomerSvc().GetReceivablesDeclareCustomerList(C_GUID, 1, -1, out count, dateBegin, dateEnd, customer, state, incomeGrp, currency, business_GUID, subBusiness_GUID);
            string json = new JavaScriptSerializer().Serialize(List);
            //strJson.AppendFormat(strFormatter, count, new JavaScriptSerializer().Serialize(List));
            // return strJson.ToString();
            return json;
        }

        public string GetReceivablesDeclareCustomerListTop()
        {
            int count = 0;
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            //string strFormatter = "{{\"total\":\"{0}\",\"rows\":{1}}}";
            StringBuilder strJson = new StringBuilder();
            List<T_DeclareCustomer> List = new List<T_DeclareCustomer>();
            List = new DeclareCustomerSvc().GetReceivablesDeclareCustomerListTop(C_GUID,out count);
            string json = new JavaScriptSerializer().Serialize(List);
            //strJson.AppendFormat(strFormatter, count, new JavaScriptSerializer().Serialize(List));
            // return strJson.ToString();
            return json;
        }
        
        public string ChooseReceivablesDeclareCustomerList(string rows, string page,string state,string invtype,string flag,string customer,string record)
        {
            int count = 0;
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            //string strFormatter = "{{\"total\":\"{0}\",\"rows\":{1}}}";
            StringBuilder strJson = new StringBuilder();
            List<T_DeclareCustomer> List = new List<T_DeclareCustomer>();
            List = new DeclareCustomerSvc().ChooseReceivablesDeclareCustomerList(C_GUID, 1, -1, out count, state, invtype, flag, customer,record);
            string json = new JavaScriptSerializer().Serialize(List);
            return json;
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

        public string CreateIntRecord(List<T_DeclareCustomer> DCustomerList)
        {
            bool result = false;
            string msg = string.Empty;
            foreach (T_DeclareCustomer form in DCustomerList)
            {
                form.C_GUID = Session["CurrentCompanyGuid"].ToString();
                form.Currency = Session["Currency"].ToString();
                result = new DeclareCustomerSvc().UpdPayDCFL(form,"NV");
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
        /// 新增申报预收客户款记录
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public string UpdReceivablesDeclareCustomer(T_DeclareCustomer form)
        {
            bool result = false;
            string msg = string.Empty;
            form.C_GUID = Session["CurrentCompanyGuid"].ToString();
            if (form.InvType == "预收客户账款")
            {
                form.Profit_Name = "预收账款";
            }
            if (form.InvType == "收取投资款(注册资本金额以内部分)")
            {
                form.Profit_Name = "实收资本";
            }
            if (form.InvType == "收取投资款(超出注册资本金额部分)")
            {
                form.Profit_Name = "资本公积";
            }
            if (form.InvType == "收回短期投资的本金金额内的款")
            {
                form.Profit_Name = "短期投资";
            }
            if (form.InvType == "收回长期债券投资的本金金额内的款")
            {
                form.Profit_Name = "长期债券投资";
            }
            if (form.InvType == "收回长期股权投资的本金金额内的款")
            {
                form.Profit_Name = "长期股权投资";
            }
            if (form.InvType == "收回公司支出的押金")
            {
                form.Profit_Name = "其他应付款";
            }
            if (form.InvType == "收到的其他公司支付的押金")
            {
                form.Profit_Name = "其他应付款";
            }
            if (form.InvType == "收回公司支出的暂支借款")
            {
                form.Profit_Name = "备用金";
            }
            if (form.InvType == "短期借款所获得的收款")
            {
                form.Profit_Name = "短期借款";
            }
            if (form.InvType == "长期借款所获得的收款")
            {
                form.Profit_Name = "长期借款";
            }
            if (form.InvType == "其他与筹资活动有关的收款")
            {
                form.Profit_Name = "其他应付款";
            }
            result = new DeclareCustomerSvc().UpdReceivablesDeclareCustomer(form);
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
        /// 计入借款
        /// </summary>
        /// <param name="payList"></param>
        /// <returns></returns>
        public string CreatPDRecord(List<T_DeclareCustomer> payList)
        {
            bool result = false;
            string msg = string.Empty;
            foreach (T_DeclareCustomer dc in payList)
            {
                dc.C_GUID = Session["CurrentCompanyGuid"].ToString();
                //dc.Profit_GUID = "79C23856-8E3C-4AC8-905C-7681C1D1F565";
                DateTime EditThreshold = DateTime.Parse(Session["EditThreshold"].ToString());
                if (DateTime.Parse(dc.Date) <= DateTime.Now && DateTime.Parse(dc.Date) >= EditThreshold)
                {
                    result = new DeclareCustomerSvc().UpdDCostSpending(dc);
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
        /// 更改状态
        /// </summary>
        /// <param name="id">收入纪录标识</param>
        /// <returns></returns>
        public string UpdState(string id,string state)
        {
            bool result = new DeclareCustomerSvc().UpdState(id, state);
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

        public string CreatRDRecord(List<T_DeclareCustomer> payList)
        {
            bool result = false;
            string msg = string.Empty;
            
            foreach (T_DeclareCustomer dc in payList)
            {
                dc.C_GUID = Session["CurrentCompanyGuid"].ToString();
                if (dc.InvType == "预收客户账款")
                {
                    dc.Profit_Name = "预收账款";
                }
                if (dc.InvType == "收取投资款(注册资本金额以内部分)")
                {
                    dc.Profit_Name = "实收资本";
                }
                if (dc.InvType == "收取投资款(超出注册资本金额部分)")
                {
                    dc.Profit_Name = "资本公积";
                }
                if (dc.InvType == "收回短期投资的本金金额内的款")
                {
                    dc.Profit_Name = "短期投资";
                }
                if (dc.InvType == "收回长期债券投资的本金金额内的款")
                {
                    dc.Profit_Name = "长期债券投资";
                }
                if (dc.InvType == "收回长期股权投资的本金金额内的款")
                {
                    dc.Profit_Name = "长期股权投资";
                }
                if (dc.InvType == "收回公司支出的押金")
                {
                    dc.Profit_Name = "其他应付款";
                }
                if (dc.InvType == "收到的其他公司支付的押金")
                {
                    dc.Profit_Name = "其他应付款";
                }
                if (dc.InvType == "收回公司支出的暂支借款")
                {
                    dc.Profit_Name = "备用金";
                }
                if (dc.InvType == "短期借款所获得的收款")
                {
                    dc.Profit_Name = "短期借款";
                }
                if (dc.InvType == "长期借款所获得的收款")
                {
                    dc.Profit_Name = "长期借款";
                }
                DateTime EditThreshold = DateTime.Parse(Session["EditThreshold"].ToString());
                if (DateTime.Parse(dc.Date) <= DateTime.Now && DateTime.Parse(dc.Date) >= EditThreshold)
                {
                    result = new DeclareCustomerSvc().UpdReceivablesDeclareCustomer(dc);
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
        /// 删除收入外收款记录
        /// </summary>
        /// <param name="id">纪录标识</param>
        /// <returns></returns>
        /// <summary>
        public string DelReceivablesDeclareCustomer(string id)
        {
            bool result = new DeclareCustomerSvc().DelReceivablesDeclareCustomer(id);
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
        /// 修改申报收入外收款记录
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public string UpdaDeclareCustomer(T_DeclareCustomer form)
        {
            bool result = false;
            string msg = string.Empty;
            result = new DeclareCustomerSvc().UpdaDeclareCustomer(form);
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
    }
}
