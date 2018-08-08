using System;
using System.Collections.Generic;
using System.Data;
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
using Aspose.Cells;

namespace FMS.BLL
{
    /// <summary>
    /// 
    /// </summary>
    public class AssetPurchaseRecordController : UserController
    {
        public AssetPurchaseRecordController()
            : base("AssetPurchase_Record")
        { }

        /// <summary>
        /// 列表页
        /// </summary>
        /// <returns></returns>
        public ActionResult AssetPurchaseRecord()
        {
            //ViewData["GUID"] = Guid.NewGuid().ToString();
            return View();
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
        public ActionResult UpAPFile()
        {
            ViewData["C_GUID"] = Session["CurrentCompany"].ToString();
            return View("UpAPFile");
        }

        public string UpdAssetPurchaseRecord(string id, string rper, decimal amount, string invtype, string remark, string currency, string addstyle, DateTime date, int depreciationperiod, string description, decimal surplusvalue,string state)
        {
            bool result = false;
            string msg = string.Empty;
            T_AIDRecord Record = new T_AIDRecord();
            Record.C_GUID = Session["CurrentCompany"].ToString();
            Record.GUID = id;
            Record.RPer = rper;
            Record.Date = date;
            Record.Amount = amount;
            Record.InvType = invtype;
            Record.Remark = remark;
            Record.Currency = currency;
            Record.Description = description;
            Record.DepreciationPeriod = depreciationperiod;
            Record.SurplusValue = surplusvalue;
            Record.State = state;

            DateTime EditThreshold = DateTime.Parse(Session["EditThreshold"].ToString());
            if (Record.Date <= DateTime.Now && Record.Date >= EditThreshold)
            {
                result = new AIDSvc().UpdAssetPurchaseRecord(Record);
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
                        string rper = (new BusinessPartnerSvc().GetPartnersDts(Session["CurrentCompany"].ToString(), dr[i][3].ToString())).ToString();

                        T_AIDRecord record = new T_AIDRecord();
                        record.C_GUID = Session["CurrentCompany"].ToString();
                        record.GUID = Guid.NewGuid().ToString();
                        record.Date = Convert.ToDateTime(dr[i][0].ToString());
                        record.Amount = Convert.ToDecimal(dr[i][1].ToString());
                        record.Currency = dr[i][2].ToString();
                        record.RPer = rper; 
                        record.InvType = dr[i][4].ToString();
                        record.Description = dr[i][5].ToString();
                        record.DepreciationPeriod = Convert.ToInt32(dr[i][6].ToString()); 
                        record.Remark = dr[i][7].ToString();
                        record.SurplusValue = Convert.ToDecimal(dr[i][1].ToString());
                        record.State = "折旧中";

                        bool TorF = new AIDSvc().UpdAssetPurchaseRecord(record);
                        if (TorF)
                        {
                            result = "导入成功！";
                        }
                        else
                        {
                            result = "导入失败！";
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
        /// 上传附件
        /// </summary>
        /// <param name="fileData">上传附件</param>
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
        /// 删除记录的所有附件
        /// </summary>
        /// <param name="id">附件纪录标识</param>
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
    }
  }
