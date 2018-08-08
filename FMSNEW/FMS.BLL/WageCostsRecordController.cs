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
using Newtonsoft.Json;
using Common.Models;
using Common.DAL;

namespace FMS.BLL
{
    /// <summary>
    /// 
    /// </summary>
    public class WageCostsRecordController : UserController
    {
        public WageCostsRecordController()
            : base("WageCostsRecord")
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
            ViewData["C_GUID"] = Session["CurrentCompanyGuid"].ToString();
            return View("ImportRecord");
        }

        public string UpdWcRecord(string id, decimal total, string Currency, string name, string RPType, string Profit_Name)
        {
            bool result = false;
            ExceResult res = new ExceResult();
            string msg = string.Empty;
            T_WageCost wag = new T_WageCost();
            wag.PayType = RPType;
            wag.C_GUID = Session["CurrentCompanyGuid"].ToString();
            wag.W_GUID = id;
            wag.Total = total;
            wag.Currency = Currency;
            wag.Employee = name;
            wag.Date = DateTime.Now;
            wag.State = "未付";
            wag.Profit_Name = Profit_Name;
            wag.SalaryType = 1;
            string strVouchID = Guid.NewGuid().ToString();
            result = new IESvc().UpdWageCost(wag, strVouchID);
            if (result)
            {
                res.success = true;
                msg = General.Resource.Common.Success;
            }
            else
            {
                res.success = false;
                msg = General.Resource.Common.Failed;
            }
            return JsonConvert.SerializeObject(res);
        }


        public string GetWageCostList(string rows, string page, string state,string id)
        {
            int count = 0;
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            StringBuilder strJson = new StringBuilder();
            List<T_WageCost> List = new List<T_WageCost>();
            List = new IESvc().GetWageCost(C_GUID, 1, -1, out count, state,id);
            return new JavaScriptSerializer().Serialize(List);


        }

        /// <summary>
        /// 20180621 zm添加，获取工资列表
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <param name="state"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetDetailWageCost(string rows, string page, string state, string id)
        {
            int count = 0;
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            StringBuilder strJson = new StringBuilder();
            List<T_DetailSalary> List = new List<T_DetailSalary>();
            List = new IESvc().GetWageCostnew(C_GUID, 1, -1, out count, state, id);
            return new JavaScriptSerializer().Serialize(List);
        }

        /// <summary>
        /// 20180622 zm添加，获取工资明细
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="page"></param>
        /// <param name="state"></param>
        /// <param name="id"></param>
        /// <returns></returns>

        public string GetWageCostInfonew(string SalaryName,string Name)
        {
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            StringBuilder strJson = new StringBuilder();
            List<T_DetailSalary> List = new List<T_DetailSalary>();
            List = new IESvc().GetWageCostInfonew(C_GUID, SalaryName, Name);
            return new JavaScriptSerializer().Serialize(List);
        }

        public string UpdWageCostsRecord(List<T_WageCost> payList,string addstyle)
        {
            bool result = false;
            string msg = string.Empty;
            string strVouchID = Guid.NewGuid().ToString();

            List<T_DetailedAccount> rec = new AccountSvc().GetLAccountByName("职工薪酬", Session["CurrentCompanyGuid"].ToString());
            foreach (T_WageCost wag in payList)
            {
                var query=rec.Where(p => p.ParentName == wag.Profit_Name);
                wag.C_GUID = Session["CurrentCompanyGuid"].ToString();
                T_IERecord Record = new T_IERecord();
                Record.Creator = base.userData.LoginFullName;
                Record.C_GUID = Session["CurrentCompanyGuid"].ToString();
                Record.IE_GUID = wag.W_GUID;
                Record.RPer = "e5e38321-4549-4c24-9f3a-3cd70ee1d591";
                Record.AffirmDate = wag.Date;
                Record.Date = wag.Date;
                Record.SumAmount = wag.Total;
                Record.InvType = wag.InvType;
                if (query.Any())
                {
                    Record.IEGroup = query.FirstOrDefault().DA_GUID;
                }
                Record.Remark = wag.Employee;
                Record.Currency = wag.Currency;
                Record.Amount = wag.Total-wag.PersonalTaxes;
                Record.CreateDate = GetNowDate();
                Record.Profit_Name = wag.Profit_Name;
                Record.TaxationAmount = 0;
                Record.TaxationType = "";
                Record.IE_Flag = "SA";
                //T_GeneralLedgerAccount rec = new AccountSvc().GetLAccount(Record.IEGroup, Session["CurrentCompanyGuid"].ToString(), Record.Profit_Name);
                //Record.IEGroup = rec.Where(p => p.ParentName == wag.Profit_Name).FirstOrDefault().DA_GUID;
                if (addstyle == "1") {
                    Record.State = "已付";
                } else { 
                    Record.State = "应付";
                }
                result = new IESvc().UpdWageCost(wag, strVouchID);
                if (result){
                 DateTime EditThreshold = DateTime.Parse(Session["EditThreshold"].ToString());
                 if (Record.Date <= DateTime.Now && Record.Date >= EditThreshold)
                 {
                     new IESvc().UpdExpenseInfo(Record, strVouchID);
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
                else
                {
                    msg = General.Resource.Common.Failed;
                }
            }
            return string.Format("{{\"Result\":{0},\"Msg\":\"{1}\"}}"
                , result.ToString().ToLower(), msg);
        }

        public string GetSalaryCollectByID(string strPar)
        {
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            List<T_WageCost> wageCollectList = new List<T_WageCost>();
            wageCollectList = new IESvc().GetSalaryCollectById(strPar);
            return new JavaScriptSerializer().Serialize(wageCollectList);
        }

        /// <summary>
        /// 批量导入excel数据
        /// </summary>
        /// 20180514修改对应新的职工薪酬模板
        public ActionResult Upexcel(FormCollection from)
        {
            ExceResult res = new ExceResult();
            res.success = true;
            HttpPostedFileBase file = Request.Files["upload"];
            string result = string.Empty;
            string strVouchID = Guid.NewGuid().ToString();
            if (file == null || file.ContentLength <= 0)
            {
                res.success = false;
                res.msg = "无有效文件";
            }
            else
            {
                try
                {
                    List<T_DetailedAccount> rec = new AccountSvc().GetLAccountByName("职工薪酬", Session["CurrentCompanyGuid"].ToString());
                    Workbook workbook = new Workbook(file.InputStream);
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
                    T_GeneralLedgerAccount gen = new AccountSvc().GetLAByName("银行存款", Session["CurrentCompanyGuid"].ToString());
                    string RpTypeID = string.Empty;
                    if (gen != null)
                    {
                        RpTypeID = gen.LA_GUID;
                    }
                    //按行进行数据存储操作！
                    for (int i = 3; i < dr.Length; i++)
                    {
                        T_WageCost WageCostRecord = new T_WageCost();
                        WageCostRecord.W_GUID = Guid.NewGuid().ToString();
                        WageCostRecord.C_GUID = Session["CurrentCompanyGuid"].ToString();
                        WageCostRecord.State = "未付";
                        WageCostRecord.PayType = RpTypeID;
                        WageCostRecord.Date = Convert.ToDateTime(dr[i][1].ToString());
                        WageCostRecord.Profit_Name = dr[i][2].ToString();
                        WageCostRecord.Employee = dr[i][0].ToString();
                        WageCostRecord.Cash = Convert.ToDecimal(GetValue(dr[i][4].ToString())) + Convert.ToDecimal(GetValue(dr[i][5].ToString())) - Convert.ToDecimal(GetValue(dr[i][6].ToString()));
                        WageCostRecord.Total = Convert.ToDecimal(GetValue(dr[i][23].ToString()));
                        WageCostRecord.PersonalTaxes = Convert.ToDecimal(GetValue(dr[i][12].ToString()));
                        WageCostRecord.BonusAllowance = Convert.ToDecimal(GetValue(dr[i][7].ToString())) + Convert.ToDecimal(GetValue(dr[i][8].ToString())) + Convert.ToDecimal(GetValue(dr[i][9].ToString()));
                        WageCostRecord.EmployeeWelfare = Convert.ToDecimal(GetValue(dr[i][10].ToString()));          
                        WageCostRecord.SocialSecurity = Convert.ToDecimal(GetValue(dr[i][16].ToString()));
                        WageCostRecord.HousingProvident = Convert.ToDecimal(GetValue(dr[i][19].ToString()));
                        WageCostRecord.NonCurrency = Convert.ToDecimal(GetValue(dr[i][22].ToString()));
                        WageCostRecord.StaffEducation = Convert.ToDecimal(GetValue(dr[i][21].ToString()));
                        WageCostRecord.TradeUnion = Convert.ToDecimal(GetValue(dr[i][20].ToString()));
                        WageCostRecord.DismissWelfare = Convert.ToDecimal(GetValue(dr[i][11].ToString()));

                        T_IERecord Record = new T_IERecord();
                        Record.Creator = base.userData.LoginFullName;
                        Record.C_GUID = Session["CurrentCompanyGuid"].ToString();
                        Record.IE_GUID = WageCostRecord.W_GUID;
                        Record.RPer = "e5e38321-4549-4c24-9f3a-3cd70ee1d591";
                        Record.AffirmDate = WageCostRecord.Date;
                        Record.Date = WageCostRecord.Date;
                        Record.SumAmount = WageCostRecord.Total;
                        Record.InvType = WageCostRecord.Profit_Name;
                        if (WageCostRecord.Profit_Name == "营业成本")
                        {
                            WageCostRecord.Profit_Name = "主营业务成本";
                        }
                        var query = rec.Where(p => p.ParentName == WageCostRecord.Profit_Name);
                        if (query.Any())
                        {
                            Record.IEGroup = query.FirstOrDefault().DA_GUID;//工资类别guid
                        }
                        Record.Remark = WageCostRecord.Employee;
                        Record.Currency = dr[i][3].ToString();
                        Record.Amount = WageCostRecord.Total-WageCostRecord.PersonalTaxes;
                        Record.CreateDate = GetNowDate();
                        Record.Profit_Name = WageCostRecord.Profit_Name;
                        
                        Record.TaxationAmount = 0;
                        Record.TaxationType = "";
                        Record.IE_Flag = "SA";
                        Record.State = "应付";

                        if (WageCostRecord.Date.CompareTo(Convert.ToDateTime(GetNowDate())) > 0)
                        {
                            result = "导入失败，时间错误";
                            break;
                        }
                        if (WageCostRecord.Date==null)
                        {
                            result = "导入失败,excel表第二列第" + dr[i] + "行时间没有填写";
                            break;
                        }
                        if (WageCostRecord.Employee == null)
                        {
                            result = "导入失败,excel表第一列第" + dr[i] + "行姓名没有填写";
                            break;
                        }
                        try
                        {
                            string currency = (new CurrencySvc().GetCurrency(dr[i][3].ToString())).ToString();
                            WageCostRecord.Currency = currency;
                        }
                        catch (Exception)
                        {
                            result = "导入失败，无此货币";
                            break;
                        }

                        if (dr[i][2].ToString() != "营业成本" && dr[i][2].ToString() != "销售费用" && dr[i][2].ToString() != "管理费用")
                        {
                            result = "导入失败，类别错误";
                            break;
                        }


                        bool TorF = new IESvc().UpdWageCost(WageCostRecord, strVouchID);
                        
                        if (TorF)
                        {
                            DateTime EditThreshold = DateTime.Parse(Session["EditThreshold"].ToString());
                            if (Record.Date <= DateTime.Now && Record.Date >= EditThreshold)
                            {
                                bool ExpR = new IESvc().UpdExpenseInfo(Record, strVouchID);
                                if (ExpR)
                                {
                                    result = "导入成功！";
                                }
                                else
                                {
                                    result = General.Resource.Common.Failed;
                                }
                            }
                            else
                            {
                                result = FMS.Resource.Finance.Finance.DateError;
                            }
                            
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

       
        /*public ActionResult Upexcel(FormCollection from)
        {
            ExceResult res = new ExceResult();
            res.success = true;
            HttpPostedFileBase file = Request.Files["upload"];
            string result = string.Empty;
            if (file == null || file.ContentLength <= 0)
            {
                res.success = false;
                res.msg = "无有效文件";
            }
            else
            {
                try { 
                    Workbook workbook = new Workbook(file.InputStream);
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
                        T_WageCost WageCostRecord = new T_WageCost();
                        WageCostRecord.W_GUID = Guid.NewGuid().ToString();
                        WageCostRecord.C_GUID = Session["CurrentCompanyGuid"].ToString();
                        WageCostRecord.State = "未付";
                        WageCostRecord.Date = Convert.ToDateTime(dr[i][1].ToString());
                        WageCostRecord.Employee = dr[i][0].ToString();
                        WageCostRecord.Cash = Convert.ToDecimal(dr[i][4].ToString());
                        WageCostRecord.PersonalTaxes = Convert.ToDecimal(dr[i][5].ToString());
                        WageCostRecord.SocialSecurity = Convert.ToDecimal(dr[i][6].ToString());
                        WageCostRecord.Total = Convert.ToDecimal(dr[i][4].ToString())+Convert.ToDecimal(dr[i][5].ToString())+Convert.ToDecimal(dr[i][6].ToString());
                        if (WageCostRecord.Date.CompareTo(Convert.ToDateTime(GetNowDate())) > 0)
                        {
                            result = "导入失败，时间错误";
                            break;
                        }
                        try
                        {
                            string currency = (new CurrencySvc().GetCurrency(dr[i][3].ToString())).ToString();
                            WageCostRecord.Currency = currency;
                        }
                        catch (Exception)
                        {
                            result = "导入失败，无此货币";
                            break;
                        }
                       
                        if (dr[i][2].ToString() != "营业成本" && dr[i][2].ToString() != "销售费用" && dr[i][2].ToString() != "管理费用")
                        {
                            result = "导入失败，类别错误";
                            break;
                        }
                      
                         
                        new IESvc().UpdWageCost(WageCostRecord);
                        //RPer,B_Guid,BA_Guid数据需要比对！
                        string rper = "e5e38321-4549-4c24-9f3a-3cd70ee1d591";

                        T_IERecord record=new T_IERecord();
                        record.IE_GUID = WageCostRecord.W_GUID;
                        record.RPer = rper;
                        record.Date = Convert.ToDateTime(dr[i][1].ToString());
                        record.AffirmDate = Convert.ToDateTime(dr[i][1].ToString());
                        record.Remark = WageCostRecord.Employee;
                        record.State = "应付";
                        record.SumAmount = Convert.ToDecimal(dr[i][4].ToString()) + Convert.ToDecimal(dr[i][5].ToString()) + Convert.ToDecimal(dr[i][6].ToString());
                        record.C_GUID = Session["CurrentCompanyGuid"].ToString();
                        record.Creator = base.userData.LoginFullName;
                        record.CreateDate =GetNowDate();
                        record.Currency = dr[i][3].ToString();
                        record.InvType = dr[i][2].ToString();
                        if(record.InvType == "营业成本"){
                            record.Profit_GUID = "51BFDD3E-2253-4FBF-A946-19C18C25C6FC";
                        }
                        if (record.InvType == "销售费用")
                        {
                            record.Profit_GUID = "DC83D8A5-31F6-4DFE-B093-87F90A234E53";
                        }
                        if (record.InvType == "管理费用")
                        {
                            record.Profit_GUID = "547E5A1A-1C20-4249-92C8-67FFFFBD38E7";
                        }
                        record.IEGroup = "工资";
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
        }*/
        /// <summary>
        /// 判断数值是否为零
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public decimal GetValue(string strValue)
        {
            decimal outPut = 0;
            if (!decimal.TryParse(strValue, out outPut))
            {
                return outPut;
            }
            return outPut;
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
        /// 获取应付薪酬子类别特殊类别
        /// </summary>
        /// <param name="repDate"></param>
        /// <param name="Flag"></param>
        /// <returns></returns>
        public string GetWageRecordDetail(string name)
        {
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            List<T_DetailedAccount> TaxSetList = new List<T_DetailedAccount>();
            TaxSetList = new IESvc().GetWageRecordDetail(C_GUID, name);
            return new JavaScriptSerializer().Serialize(TaxSetList);

        }
       
    }
}
