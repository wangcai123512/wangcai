using System;
using System.Web.Mvc;
using BaseController;
using FMS.DAL;
using FMS.Model;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Web.Script.Serialization;
using System.IO;
using System.Web;
using System.Web.UI.WebControls;
using Aspose.Cells;

namespace FMS.BLL
{
    /// <summary>
    /// 记录收款
    /// </summary>
    public class ReceivablesRecordController : UserController
    {
        public ReceivablesRecordController()
            : base("Receivables_Record")
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
                string C_GUID = Session["CurrentCompany"].ToString();
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
            string C_GUID = Session["CurrentCompany"].ToString();
            return View(new RecPayRecordSvc().GetReceivablesRecord(id, C_GUID));
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
        /// 数据导入
        /// </summary>
        /// <returns></returns>
        public ActionResult UpRRFile()
        {
            ViewData["C_GUID"] = Session["CurrentCompany"].ToString();
            return View("UpRRFile");
        }

        /// <summary>
        /// 选择应收页面
        /// </summary>
        /// <param name="id">付款方标识</param>
        /// <returns></returns>
        public ActionResult ChooseReceivablesRecord(string id)
        {
            ViewData["C_GUID"] = Session["CurrentCompany"].ToString();
            ViewData["RPer"] = id;
            return View("ChooseReceivablesRecord");
        }

        public ActionResult GetReceivablesDeclareCustomer()
        {
            ViewData["C_GUID"] = Session["CurrentCompany"].ToString();
            return View("GetReceivablesDeclareCustomer");
        }

        public ActionResult GetReceivablesDeclareDeposit()
        {
            ViewData["C_GUID"] = Session["CurrentCompany"].ToString();
            return View("GetReceivablesDeclareDeposit");
        }

        /// <summary>
        /// 获取应收款列表
        /// </summary>
        /// <param name="rows">页大小</param>
        /// <param name="page">页索引</param>
        /// <param name="id">付款方标识</param>
        /// <returns></returns>
        public string GetChooseReceivablesRecord(string rows, string page, string id)
        {
            int count = 0;
            string C_GUID = Session["CurrentCompany"].ToString();
            string strFormatter = "{{\"total\":\"{0}\",\"rows\":{1}}}";
            StringBuilder strJson = new StringBuilder();
            List<T_Receivables> Receivables = new List<T_Receivables>();
            Receivables = new RecPayRecordSvc().GetChooseReceivablesRecord(id, C_GUID, int.Parse(page), int.Parse(rows), out count);
            strJson.AppendFormat(strFormatter, count, new JavaScriptSerializer().Serialize(Receivables));
            return strJson.ToString();

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
            string C_GUID = Session["CurrentCompany"].ToString();
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
            string C_GUID = Session["CurrentCompany"].ToString();
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
            return View(new RecPayRecordSvc().GetReceivablesRecord(id, Session["CurrentCompany"].ToString()));
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
            rec.C_GUID = Session["CurrentCompany"].ToString();
            DateTime EditThreshold = DateTime.Parse(Session["EditThreshold"].ToString());
            if (rec.Date <= DateTime.Now && rec.Date >= EditThreshold)
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
        public string UpdReceivablesRecordDts(string id, string name, DateTime date, decimal money, string remark, string currency, string bank, string bankaccount, string ieguid, string addstyle)
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
            if (addstyle == "收入获取" || addstyle == "收入计入收款")
            {
                recPayRecord.IE_GUID = ieguid;
                recPayRecord.InvType = "经营活动收款";
                recPayRecord.InvTypeDts = "销售商品/提供服务的收款";
                recPayRecord.CFItemGuid = "97B181C8-D807-4BF0-8D8D-B23273E7FEFE";
            }
            if (addstyle == "直接新增")
            {
                recPayRecord.IE_GUID = null;
                recPayRecord.InvType = "未归账";
            }
            if (addstyle == "预收客户款")
            {
                recPayRecord.IE_GUID = ieguid;
                recPayRecord.InvType = "经营活动收款";
                recPayRecord.InvTypeDts = "收到的其他与经营活动有关的款客户预付、押金返还、暂支还款等;预付客户款";
                recPayRecord.CFItemGuid = "F6330595-F588-46B0-8998-752C7A1D774B";
            }
            if (addstyle == "押金与暂支付款")
            {
                recPayRecord.IE_GUID = ieguid;
                recPayRecord.InvType = "经营活动收款";
                recPayRecord.InvTypeDts = "收到的其他与经营活动有关的款客户预付、押金返还、暂支还款等;押金与暂支付款";
                recPayRecord.CFItemGuid = "F6330595-F588-46B0-8998-752C7A1D774B";
            }
            if (addstyle == "吸取投资")
            {
                recPayRecord.IE_GUID = ieguid;
                recPayRecord.InvType = "筹资活动收款";
                recPayRecord.InvTypeDts = "吸取投资的收款";
                recPayRecord.CFItemGuid = "77A24D5F-3E0C-4211-A552-191FEE0E06FD";
            }
            if (addstyle == "借款")
            {
                recPayRecord.IE_GUID = ieguid;
                recPayRecord.InvType = "筹资活动收款";
                recPayRecord.InvTypeDts = "借款所获得的收款";
                recPayRecord.CFItemGuid = "AD2E5437-0917-43E1-807C-41CA6751360F";
            }
            if (addstyle == "其他与筹资活动有关的收款")
            {
                recPayRecord.IE_GUID = ieguid;
                recPayRecord.InvType = "筹资活动收款";
                recPayRecord.InvTypeDts = "收到的其他与筹资活动有关的款";
                recPayRecord.CFItemGuid = "106B9F2C-24A5-48B5-9621-418D00A7A75A";
            }
            recPayRecord.CreateDate=DateTime.Now;
            DateTime EditThreshold = DateTime.Parse(Session["EditThreshold"].ToString());
            if (recPayRecord.Date <= DateTime.Now && recPayRecord.Date >= EditThreshold)
            {
                result = new RecPayRecordSvc().UpdReceivablesRecord(recPayRecord);
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
                        string baguid = (new BankAccountSvc().GetBankAccountDts(Session["CurrentCompany"].ToString(),bguid, dr[i][5].ToString())).ToString();

                        string cguid = Session["CurrentCompany"].ToString();
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
            jsonresult.Data = RR;
            return jsonresult;
        }
    }
}