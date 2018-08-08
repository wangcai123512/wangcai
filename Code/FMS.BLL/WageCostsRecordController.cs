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
    public class WageCostsRecordController : UserController
    {
        public WageCostsRecordController()
            : base("WageCosts_Record")
        { }

        /// <summary>
        /// 列表页
        /// </summary>
        /// <returns></returns>
        public ActionResult WageCostsRecord()
        {
            ViewData["GUID"] = Guid.NewGuid().ToString();
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

        public string UpdWageCostsRecord(string id, string rper, decimal money, decimal money1, decimal money2, decimal money3, string invtype, string remark, string currency, string addstyle, DateTime date)
        {
            bool result = false;
            string msg = string.Empty;
            T_IERecord Record = new T_IERecord();
            Record.Creator = base.userData.LoginFullName;
            Record.C_GUID = Session["CurrentCompany"].ToString();
            Record.IE_GUID = id;
            Record.RPer = rper;
            Record.AffirmDate = date;
            Record.Date = date;
            Record.SumAmount = money;
            Record.InvType = invtype;
            Record.IEGroup = "1544d862-b1ab-42b8-9e97-9c2e1704665c";//工资类别guid
            Record.Remark = remark;
            Record.Currency = currency;
            Record.CreateDate = DateTime.Now;
            Record.TaxationAmount = 0;
            Record.TaxationType = "";
            Record.State = "应付";
            Record.Profit_GUID = "51BFDD3E-2253-4FBF-A946-19C18C25C6FC";

            T_WageCost WageCostRecord = new T_WageCost();
            WageCostRecord.W_GUID = id;
            WageCostRecord.C_GUID = Session["CurrentCompany"].ToString();
            WageCostRecord.Date = date;
            WageCostRecord.Employee = remark;
            WageCostRecord.Cash = money1;
            WageCostRecord.PersonalTaxes = money2;
            WageCostRecord.SocialSecurity = money3;
            WageCostRecord.Total = money1 + money2 + money3;
            new IESvc().UpdWageCost(WageCostRecord);

            DateTime EditThreshold = DateTime.Parse(Session["EditThreshold"].ToString());
            if (Record.Date <= DateTime.Now && Record.Date >= EditThreshold)
            {
                result = new IESvc().UpdExpenseRecord(Record);
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
                        T_WageCost WageCostRecord = new T_WageCost();
                        WageCostRecord.W_GUID = Guid.NewGuid().ToString();
                        WageCostRecord.C_GUID = Session["CurrentCompany"].ToString();
                        WageCostRecord.Date = Convert.ToDateTime(dr[i][1].ToString());
                        WageCostRecord.Employee = dr[i][0].ToString(); ;
                        WageCostRecord.Cash = Convert.ToDecimal(dr[i][2].ToString());
                        WageCostRecord.PersonalTaxes = Convert.ToDecimal(dr[i][3].ToString());
                        WageCostRecord.SocialSecurity = Convert.ToDecimal(dr[i][4].ToString());
                        WageCostRecord.Total = Convert.ToDecimal(dr[i][2].ToString())+Convert.ToDecimal(dr[i][3].ToString())+Convert.ToDecimal(dr[i][4].ToString()); ;
                        new IESvc().UpdWageCost(WageCostRecord);
                        //RPer,B_Guid,BA_Guid数据需要比对！
                        string rper = "b73f1802-4ba4-4873-b423-86ea3d9b723f";

                        T_IERecord record=new T_IERecord();
                        record.IE_GUID = Guid.NewGuid().ToString();
                        record.RPer = rper;

                        DateTime dt;
                        DateTimeFormatInfo dtFormat = new DateTimeFormatInfo();
                        dtFormat.ShortDatePattern = "yyyy/MM/dd";
                        dt = Convert.ToDateTime(dr[i][1].ToString(), dtFormat);
                        record.AffirmDate = dt;
                        record.Date = Convert.ToDateTime(dr[i][1].ToString());
                        record.State = "应付";
                        record.SumAmount = Convert.ToDecimal(dr[i][2].ToString()) + Convert.ToDecimal(dr[i][3].ToString()) + Convert.ToDecimal(dr[i][4].ToString());
                        record.C_GUID = Session["CurrentCompany"].ToString();
                        record.Creator = base.userData.LoginFullName;
                        record.CreateDate = DateTime.Now;
                        record.Currency = dr[i][6].ToString();
                        record.InvType = dr[i][7].ToString();
                        record.IEGroup = "1544d862-b1ab-42b8-9e97-9c2e1704665c";
                        record.Remark = dr[i][0].ToString();
                        record.TaxationAmount = 0;
                        record.TaxationType = "";
                        bool TorF = new IESvc().UpdExpenseRecord(record);
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
    }
}
