﻿using System;
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
using Common.Models;
using Newtonsoft.Json;

namespace FMS.BLL
{
    /// <summary>
    /// 
    /// </summary>
    public class IndirectMaterialPurchasingRecordController : UserController
    {
        public IndirectMaterialPurchasingRecordController()
            : base("IndirectMaterialPurchasingRecord")
        { }

        /// <summary>
        /// 列表页
        /// </summary>
        /// <returns></returns>
        public ActionResult IndirectMaterialPurchasingRecord()
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
            ViewData["C_GUID"] = Session["CurrentCompanyGuid"].ToString();
            return View("ImportRecord");
        }

        /// <summary>
        /// 数据导入
        /// </summary>
        /// <returns></returns>
        public ActionResult UpIMPFile()
        {
            ViewData["C_GUID"] = Session["CurrentCompanyGuid"].ToString();
            return View("UpIMPFile");
        }

        /// <summary>
        /// 保存间接物料
        /// </summary>
        /// <param name="recordList">间接物料列表</param>
        /// <returns></returns>
        public string UpdateRecord(List<T_AIDRecord> recordList)
        {
            bool saved = false;
         
            //日期必须在Now 和 EditThreshold之间
            var dateCheck = DateCheck(recordList);
            ExceResult res = new ExceResult();

            //无符合的日期返回日期异常提示
            if (dateCheck.Count == 0)
            {
                res.success = false;
                res.msg = FMS.Resource.Finance.Finance.DateError;
                return JsonConvert.SerializeObject(res);
            }

            foreach (T_AIDRecord aid in recordList)
            {
                aid.C_GUID = Session["CurrentCompanyGuid"].ToString();
                aid.State = "库存";

                saved = new AIDSvc().UpdIndirectMaterialPurchasingRecord(aid);
                if (saved)
                {
                    res.success = true;
                    res.msg = General.Resource.Common.Success;
                }
                else
                {
                    res.success = false;
                    res.msg = General.Resource.Common.Failed;
                    break;
                }
            }

            return JsonConvert.SerializeObject(res);
        }

        public string UpdIndirectMaterialPurchasingRecord(string id, string rper, decimal amount, string invtype, string remark, string currency, string addstyle, string date, string description, string state)
        {
            bool result = false;
            string msg = string.Empty;
            T_AIDRecord Record = new T_AIDRecord();
            Record.C_GUID = Session["CurrentCompanyGuid"].ToString();
            Record.GUID = id;
            Record.RPer = rper;
            Record.Date = date;
            Record.Amount = amount;
            Record.InvType = invtype;
            Record.Remark = remark;
            Record.Currency = currency;
            Record.Description = description;
            Record.State = state;

            DateTime EditThreshold = DateTime.Parse(Session["EditThreshold"].ToString());

            if (DateCheck(Record.Date))
            {
                result = new AIDSvc().UpdIndirectMaterialPurchasingRecord(Record);
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
        public ActionResult ImportExcel()
        {
            ExceResult res = new ExceResult();
            res.success = true;

            var file = Request.Files;
            string result = string.Empty;
            if (file == null || file.Count <= 0)
            {
                res.success = false;
                res.msg = "无有效文件";
            }
            else
            {
                try
                {
                    Workbook workbook = new Workbook(file[0].InputStream);
                    Cells cells = workbook.Worksheets[0].Cells;
                    DataTable tab = cells.ExportDataTable(0, 0, cells.Rows.Count, cells.MaxDisplayRange.ColumnCount);
                    int rowsnum = tab.Rows.Count;
                    if (rowsnum == 0)
                    {
                        res.success = false;
                        result = "Excel表为空!请重新导入！"; //当Excel表为空时，对用户进行提示
                    }
                    //数据表一共多少行！
                    DataRow[] dr = tab.Select();
                    //按行进行数据存储操作！
                    for (int i = 1; i < dr.Length; i++) 
                    {
                        //RPer,B_Guid,BA_Guid数据需要比对！                     
                        string SonInvType = (new AIDSvc().GetSubTypeCatId(Session["CurrentCompanyGuid"].ToString(), dr[i][5].ToString(), dr[i][4].ToString())).ToString();
                        T_AIDRecord record = new T_AIDRecord();
                        record.C_GUID = Session["CurrentCompanyGuid"].ToString();
                        record.GUID = Guid.NewGuid().ToString();
                        record.Date =  dr[i][0].ToString();
                        if (record.Date.CompareTo(GetNowDate()) > 0)
                        {
                            result = "导入失败，购入日期错误";
                            break;
                        }
                        record.Amount = Convert.ToDecimal(dr[i][1].ToString());
                        try
                        {
                            string currency = (new CurrencySvc().GetCurrency(dr[i][2].ToString())).ToString();
                            record.Currency = currency;
                        }
                        catch (Exception)
                        {
                            result = "导入失败，无此货币";
                            break;
                        }
                        try
                        {
                            string rper = (new BusinessPartnerSvc().GetPartnersDts(Session["CurrentCompanyGuid"].ToString(), dr[i][3].ToString())).ToString();
                            record.RPer = rper;
                        }
                        catch (Exception)
                        {
                            result = "导入失败，无此供应商";
                            break;
                        }
                        try
                        {
                            string InvType = (new AIDSvc().GetTypeCatId(Session["CurrentCompanyGuid"].ToString(), dr[i][4].ToString())).ToString();
                            record.InvType = InvType;
                        }
                        catch (Exception)
                        {
                            result = "导入失败，无此物料类别";
                            break;
                        }
                        try
                        {
                            string InvType = (new AIDSvc().GetTypeCatId(Session["CurrentCompanyGuid"].ToString(),dr[i][4].ToString())).ToString();
                            record.SonInvType = SonInvType;
                        }
                        catch (Exception)
                        {
                            result = "导入失败，无此物料子类别";
                            break;
                        }
                        record.State = "存货";
        
                        bool TorF = new AIDSvc().UpdIndirectMaterialPurchasingRecord(record);
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
