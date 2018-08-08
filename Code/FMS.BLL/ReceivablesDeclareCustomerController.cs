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
            : base("Receivables_DeclareCustomer")
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
        /// 获取申报
        /// </summary>
        /// <param name="rows">页大小</param>
        /// <param name="page">页索引</param>
        /// <returns></returns>
        public string GetReceivablesDeclareCustomerList(string rows, string page,string invtype=null,string state=null)
        {
            int count = 0;
            string C_GUID = Session["CurrentCompany"].ToString();
            string strFormatter = "{{\"total\":\"{0}\",\"rows\":{1}}}";
            StringBuilder strJson = new StringBuilder();
            List<T_DeclareCustomer> List = new List<T_DeclareCustomer>();
            List = new DeclareCustomerSvc().GetReceivablesDeclareCustomerList(C_GUID, int.Parse(page), int.Parse(rows), out count, invtype, state);
            strJson.AppendFormat(strFormatter, count, new JavaScriptSerializer().Serialize(List));
             return strJson.ToString();
        }

        public string ChooseReceivablesDeclareCustomerList(string rows, string page,string invtype=null)
        {
            int count = 0;
            string C_GUID = Session["CurrentCompany"].ToString();
            string strFormatter = "{{\"total\":\"{0}\",\"rows\":{1}}}";
            StringBuilder strJson = new StringBuilder();
            List<T_DeclareCustomer> List = new List<T_DeclareCustomer>();
            List = new DeclareCustomerSvc().ChooseReceivablesDeclareCustomerList(C_GUID, int.Parse(page), int.Parse(rows), out count, invtype);
            strJson.AppendFormat(strFormatter, count, new JavaScriptSerializer().Serialize(List));
            return strJson.ToString();
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
            form.C_GUID = Session["CurrentCompany"].ToString();
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
    }
}
