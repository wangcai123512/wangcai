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
using Newtonsoft.Json;
using Common.Models;
using System.Transactions;

namespace FMS.BLL
{
    /// <summary>
    /// 记录费用
    /// </summary>
    public class TaxProvisionRecordController : UserController
    {
        public TaxProvisionRecordController()
            : base("TaxProvisionRecord")
        { }

        /// <summary>
        /// 列表页
        /// </summary>
        /// <returns></returns>
        public ActionResult TaxProvisionRecord(string reportDate, string type)
        {
            ViewData["IE_GUID"] = Guid.NewGuid().ToString();
            ViewBag.reportDate = reportDate;
            return View();
        }

        public ViewResult Index()
        {
            return View();
        }

        public ActionResult ComTaxRecord()
        {
            return View();
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

        public string CreateTaxSettlement(string repDate, string Amount, string Rep_status,string GUID,string Flag,string State)
        {
            bool result = false;
            ExceResult res = new ExceResult();
            string msg = string.Empty;
            if (string.IsNullOrEmpty(GUID))
            {
                GUID = Guid.NewGuid().ToString();
            }
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            result = new IESvc().CreateTaxSettlement(GUID,repDate, Amount, Rep_status, C_GUID,Flag,State,"非期初");
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
            return Newtonsoft.Json.JsonConvert.SerializeObject(res);
            
        }

        public string CreatTaxSettlement(List<T_TaxSettlement> TaxList)
        {
            bool result = false;
            ExceResult res = new ExceResult();
            string msg = string.Empty;
            string Flag = null;
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {
                foreach (T_TaxSettlement tax in TaxList)
                {
                    if (string.IsNullOrEmpty(tax.GUID))
                    {
                        tax.GUID = Guid.NewGuid().ToString();
                    }
                    switch (tax.Flag)
                    {
                        case "增值税":
                            Flag = "TA";
                            break;
                        case "营业税金及附加":
                            Flag = "YT";
                            break;
                        case "所得税费用":
                            Flag = "CT";
                            break;
                        case "个人所得税":
                            Flag = "SA";
                            break;
                        default:
                            break;

                    }


                    result = new IESvc().CreateTaxSettlement(tax.GUID, tax.Rep_date, tax.Amount.ToString(), tax.Rep_status, C_GUID, Flag, tax.State, "期初");

                }
                if (result)
                {
                    scope.Complete();
                }
                
            }
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
            return Newtonsoft.Json.JsonConvert.SerializeObject(res);
        }

        public string GetTaxSettlement(string repDate,string Flag)
        {
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            List<T_TaxSettlement> TaxSetList = new List<T_TaxSettlement>();
            TaxSetList = new IESvc().GetTaxSettlement(repDate,C_GUID,Flag);
            return new JavaScriptSerializer().Serialize(TaxSetList);
            
        }

        public string GetTaxDetailInfo(string strPar)
        {
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            List<T_DetailTaxSettle> TaxDetailList = new List<T_DetailTaxSettle>();
            TaxDetailList = new IESvc().GetDetailTaxSettlement(strPar);
            return new JavaScriptSerializer().Serialize(TaxDetailList);
        }

        public string GetDetailZZHShui(string strPar) {
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            List<T_DetailTaxSettle> TaxDetailList = new List<T_DetailTaxSettle>();
            TaxDetailList = new IESvc().GetDetailZZHShui(strPar);
            return new JavaScriptSerializer().Serialize(TaxDetailList);
        }

        public string GetTaxDetailInfoByRecID(string strPar,string taskDetailID)
        {
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            List<T_DetailTaxSettle> TaxDetailList = new List<T_DetailTaxSettle>();
            TaxDetailList = new IESvc().GetDetailTaxByRecID(strPar,taskDetailID);
            return new JavaScriptSerializer().Serialize(TaxDetailList);
        }
        
        /// <summary>
        /// 获取增值税子类别特殊类别
        /// </summary>
        /// <param name="repDate"></param>
        /// <param name="Flag"></param>
        /// <returns></returns>
        public string GetZhengZhiDetail(string name)
        {
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            List<T_ThirdAccount> TaxSetList = new List<T_ThirdAccount>();
            TaxSetList = new IESvc().GetZhengZhiDetail(C_GUID, name);
            return new JavaScriptSerializer().Serialize(TaxSetList);

        }

        /// <summary>
        /// 获取营业税子类别特殊类别
        /// </summary>
        /// <param name="repDate"></param>
        /// <param name="Flag"></param>
        /// <returns></returns>
        public string GetSalesTaxDetail(string name)
        {
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            List<T_DetailedAccount> TaxSetList = new List<T_DetailedAccount>();
            TaxSetList = new IESvc().GetSalesTaxDetail(C_GUID, name);
            return new JavaScriptSerializer().Serialize(TaxSetList);

        }
        

        public string GetIETaxList(string Flag,string MounthDate)
        {
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            List<T_IERecord> IERcord = new List<T_IERecord>();
            IERcord = new IESvc().GetIETaxList(Flag,MounthDate,C_GUID);
            return new JavaScriptSerializer().Serialize(IERcord);
        }
        public string GetComIETaxList(string Year, string Quarter)
        {
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            List<T_IERecord> IERcord = new List<T_IERecord>();
            IERcord = new IESvc().GetComIETaxList(Year, Quarter, C_GUID);
            return new JavaScriptSerializer().Serialize(IERcord);
        }

        public string CheckTaxSettlement(string Rep_date)
        {
           
            ExceResult res = new ExceResult();
            res.success = false;
            List<T_TaxSettlement> TaxSetList = new List<T_TaxSettlement>();
            TaxSetList = new IESvc().CheckTaxSettlement(Rep_date);
            int a = TaxSetList.Count();
            if (a <= 0) {
               
                res.success = true;
            }
            return JsonConvert.SerializeObject(res);
        }
        /// <summary>
        /// 获取费用列表
        /// </summary>
        /// <param name="rows">页大小</param>
        /// <param name="page">页索引</param>
        /// <returns></returns>
        public string GetExpenseList(string rows, string page)
        {
            int count = 0;
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            string strFormatter = "{{\"total\":\"{0}\",\"rows\":{1}}}";
            StringBuilder strJson = new StringBuilder();
            List<T_IERecord> IERcord = new List<T_IERecord>();
            IERcord=new IESvc().GetExpenseList(C_GUID,int.Parse(page),int.Parse(rows),out count);
             strJson.AppendFormat(strFormatter, count, new JavaScriptSerializer().Serialize(IERcord));
             return strJson.ToString();


        }

        public string UpdTaxSet(string GUID, string Rep_status)
        {
            bool result = false;
            ExceResult res = new ExceResult();
            string msg = string.Empty;
            result = new IESvc().UpdTaxProvisionRecord(GUID, Rep_status);
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
        /// 更新费用数据
        /// </summary>
        /// <param name="head">费用主数据</param>
        /// <param name="list">费用明细数据</param>
        /// <returns></returns>
        public string UpdTaxProvisionRecord(T_IERecord form)
        {
            bool result = false;
            string msg = string.Empty;
            form.C_GUID = Session["CurrentCompanyGuid"].ToString();
            form.Creator = base.userData.LoginFullName;
            form.CreateDate = GetNowDate();
            if (form.IEGroup == "ac23eded-9c74-4781-8d14-797a5bccdc79" || form.IEGroup == "234218e9-87c5-4854-814a-7d5671bf1fd9" || form.IEGroup == "8e777b91-f3f9-4907-ba1a-2e0842967500")
            {
                form.Profit_GUID = "51BFDD3E-2253-4FBF-A946-19C18C25C6FC";
            }
            if (form.IEGroup == "d136bf9c-c3a3-4f33-ab1e-820526dcbc24")
            {
                form.Profit_GUID = "082CD9EB-9947-43C4-A7C6-F2B7FAB6EE54";
            }
            result = new IESvc().UpdExpenseRecord(form);
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
        /// 删除费用记录
        /// </summary>
        /// <param name="id">费用标识</param>
        /// <returns></returns>
        public string DelExpenseRecord(string id)
        {
            string msg = string.Empty;
            bool result = new IESvc().DelIERecord(id,"E");
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
        /// 添加增值税记录
        /// </summary>
        /// <returns></returns>
        public string AddTaxProvisionRecord(string Amount, string Date, string Name, string inputtax, string outputtax, string exportreduce, string transfertax, string exporttax, string payingtax)
        {
            bool result = false;
            ExceResult res = new ExceResult();
            string msg = string.Empty;
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            result = new IESvc().AddTaxProvisionRecord(Amount, Date, Name, inputtax, outputtax, exportreduce, transfertax, exporttax, payingtax,C_GUID);
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
            return Newtonsoft.Json.JsonConvert.SerializeObject(res);

        }

        /// <summary>
        /// 添加企业所得税记录
        /// </summary>
        /// <returns></returns>
        public string AddCTProvisionRecord(string Amount, string Name,string Date)
        {
            bool result = false;
            ExceResult res = new ExceResult();
            string msg = string.Empty;
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            result = new IESvc().AddCTProvisionRecord(Amount,  Name, C_GUID,Date+"/01");
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
            return Newtonsoft.Json.JsonConvert.SerializeObject(res);

        }
        /// <summary>
        /// 记录营业税金及附加
        /// </summary>
        /// <returns></returns>
        public string AddSalesTaxRecord(string Amount, string Name, string Date, string Excise, string EducationFee, string Sales,
            string UrbanConstruction, string Resource, string LandValue, string UrbanLand, string Property, string VehicleVessel, string MineralResources, string Dischargefee)
        {
            bool result = false;
            ExceResult res = new ExceResult();
            string msg = string.Empty;
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            result = new IESvc().AddSalesTaxRecord(Amount, Name, Date, Excise, EducationFee, Sales, UrbanConstruction, Resource, LandValue, UrbanLand,
                Property, VehicleVessel, MineralResources, Dischargefee,C_GUID);
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
            return Newtonsoft.Json.JsonConvert.SerializeObject(res);

        }

        /// <summary>
        /// 获取增值税
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// </remarks>
        public string GetTaxReportList(string reportDate, string type)
        {
            string CId = CompanyId();

            List<Co_Tr_ThirdAccount> lst = IESvc.GetTaxReportList(CId, reportDate, type);

            return JsonConvert.SerializeObject(lst);
        }

        /// <summary>
        /// 根据时间获取增值税
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// </remarks>

        public string GetTaxReportbyDate(string reportDate, string period, string isend)
        {
            string CId = CompanyId();

            List<Co_Tr_ThirdAccount> lst = IESvc.GetTaxReportbyDate(CId, reportDate, period,isend);

            return JsonConvert.SerializeObject(lst);
        }

        /// <summary>
        /// 获取已结转增值税数据
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// </remarks>

        public string GetTaxReportIsend(string reportDate, string period, string isend)
        {
            string CId = CompanyId();

            List<Co_Tr_ThirdAccount> lst = IESvc.GetTaxReportIsend(CId, reportDate, period, isend);

            return JsonConvert.SerializeObject(lst);
        }



        /// <summary>
        /// 判断是否可以反结账结账
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        ///
        /// </remarks>
        public string isFinish(string repDate, string status)
        {
            int count = 0;
            ExceResult res = new ExceResult();
            string CId = CompanyId();
            List<Co_Tr_ThirdAccount> rep = new List<Co_Tr_ThirdAccount>();
            rep = new IESvc().isFinish(repDate, status, out count,CId);
            string msg = string.Empty;
            bool result = false;
            if (rep.Count == 0)
            {
                msg = General.Resource.Common.Success;
                result = true;
            }
            else
            {
                msg = General.Resource.Common.Failed;
                result = false;
            }

            return string.Format("{{\"Result\":{0},\"Msg\":\"{1}\"}}"
                , result.ToString().ToLower(), msg);

        }

        /// <summary>
        /// 反结账()
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// 2018/07/18   zm
        /// </remarks>
        public string DeleteTaxProvisionRecord(string RepDate, string TaxID)
        {
            bool result = false;
            ExceResult res = new ExceResult();
            string msg = string.Empty;
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            result = new IESvc().DeleteTaxProvisionRecord(RepDate, TaxID, C_GUID);
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
            return Newtonsoft.Json.JsonConvert.SerializeObject(res);

        }

        public string GetDetailTaxList(string AffirmDate, string period)
        {
            int count = 0;
            StringBuilder strJson = new StringBuilder();

            List<T_DetailTaxSettle> List = new IESvc().GetDetailTaxList(Session["CurrentCompanyGuid"].ToString(), 1, -1, out count, AffirmDate, period);
            string json = new JavaScriptSerializer().Serialize(List);

            return json;
        }
    }
}
