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

namespace FMS.BLL
{
    /// <summary>
    /// 记录付款
    /// </summary>
    public class PaymentRecordController : UserController
    {
        public PaymentRecordController()
            : base("Payment_Record")
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

                string C_GUID = Session["CurrentCompany"].ToString();
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
            string C_GUID = Session["CurrentCompany"].ToString();
            return View(new RecPayRecordSvc().GetPaymentRecord(id, C_GUID));
        }

        /// <summary>
        /// 选择应付纪录页面
        /// </summary>
        /// <param name="id">收款方标识</param>
        /// <returns></returns>
        public ActionResult ChoosePayablesRecord(string id)
        {
            ViewData["C_GUID"] = Session["CurrentCompany"].ToString();
            ViewData["RPer"] = id;
            return View("ChoosePayablesRecord");
        }

        public ActionResult ChooseWCRecord(string id)
        {
            ViewData["C_GUID"] = Session["CurrentCompany"].ToString();
            ViewData["RPer"] = id;
            return View("ChooseWCRecord");
        }

        public ActionResult GetPaymentDeclareCostSpending()
        {
            ViewData["C_GUID"] = Session["CurrentCompany"].ToString();
            return View("GetPaymentDeclareCostSpending");
        }

        /// <summary>
        /// 数据导入
        /// </summary>
        /// <returns></returns>
        public ActionResult UpPRFile()
        {
            ViewData["C_GUID"] = Session["CurrentCompany"].ToString();
            return View("UpPRFile");
        }

        /// <summary>
        /// 数据导入
        /// </summary>
        /// <returns></returns>
        public ActionResult ImportRecord()
        {
            ViewData["C_GUID"] = Session["CurrentCompany"].ToString();
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

        /// <summary>
        /// 应付纪录列表
        /// </summary>
        /// <param name="rows">页大小</param>
        /// <param name="page">页索引</param>
        /// <param name="id">收款方标识</param>
        /// <returns></returns>
        public string GetChoosePayablesRecord(string rows, string page, string id,string iegroup=null)
        {
            int count = 0;
            string C_GUID = Session["CurrentCompany"].ToString();
            string strFormatter = "{{\"total\":\"{0}\",\"rows\":{1}}}";
            StringBuilder strJson = new StringBuilder();
            List<T_Payables> Payables = new List<T_Payables>();
            Payables = new RecPayRecordSvc().GetChoosePayablesRecord(id, C_GUID, int.Parse(page), int.Parse(rows), out count, iegroup);
            strJson.AppendFormat(strFormatter, count, new JavaScriptSerializer().Serialize(Payables));
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
            string C_GUID = Session["CurrentCompany"].ToString();
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
            return View(new RecPayRecordSvc().GetPaymentRecord(id, Session["CurrentCompany"].ToString()));
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
            rec.C_GUID = Session["CurrentCompany"].ToString();
            DateTime EditThreshold = DateTime.Parse(Session["EditThreshold"].ToString());
            if (rec.Date <= DateTime.Now && rec.Date >= EditThreshold)
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
            recPayRecord.C_GUID = Session["CurrentCompany"].ToString();
            recPayRecord.RP_GUID = id;
            //recPayRecord.RPer = (new BusinessPartnerSvc().GetPartnersDts(Session["CurrentCompany"].ToString(), name)).ToString();
            recPayRecord.RPer = name;
            recPayRecord.Date = date.Date;
            recPayRecord.SumAmount = money;
            recPayRecord.Remark = remark;
            recPayRecord.Currency = currency;
            recPayRecord.B_GUID = bank;
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
            recPayRecord.CreateDate = DateTime.Now;
            DateTime EditThreshold = DateTime.Parse(Session["EditThreshold"].ToString());
            if (recPayRecord.Date <= DateTime.Now && recPayRecord.Date >= EditThreshold)
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
                        string rper = (new BusinessPartnerSvc().GetPartnersDts(Session["CurrentCompany"].ToString(), dr[i][0].ToString())).ToString();
                        string bguid = (new BankAccountSvc().GetBankDts(Session["CurrentCompany"].ToString(), dr[i][4].ToString())).ToString();
                        string baguid = (new BankAccountSvc().GetBankAccountDts(Session["CurrentCompany"].ToString(), bguid, dr[i][5].ToString())).ToString();

                        string cguid = Session["CurrentCompany"].ToString();
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
    }
}