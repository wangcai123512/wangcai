using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Common.BaseControllers;
using FMS.DAL;
using FMS.Model;
using System;
using System.Web.Script.Serialization;
using System.Web;
using System.IO;
using Common.Models;
using System.Data;
using System.Data.Entity;
using EF = FMS.Models;
using FMS.BLL;
using BaseController;
using System.Configuration; 
using System.Globalization;
using Newtonsoft.Json;

namespace FMS.BLL
{
    /// <summary>
    /// 内部数据接口
    /// </summary>
    public class InternalAPIController : APIController
    {
        private const string SelectItemJSONFormart = "{{\"label\":\"{0}\",\"value\":\"{1}\"}},";

        public JsonResult GetInvType()
        {
            return Json(new InvTypeSvc().GetInvType());
        }

        /// <summary>
        /// 获取公司设置
        /// </summary>
        /// <returns></returns>
        public string GetCompanySetting()
        {

            T_CompanySetting setting =
                new CompanySvc().GetCompanySetting(Session["CurrentCompanyGuid"].ToString());
            return new JavaScriptSerializer().Serialize(setting);
        }
        /// <summary>
        /// 付款方
        /// </summary>
        /// <returns></returns>
        public JsonResult GetPayer()
        {
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            return Json(new BusinessPartnerSvc().GetPartners(C_GUID).Where(i => (i.IsCustomer || i.IsPartner)));
        }
        /// <summary>
        /// 客户
        /// </summary>
        /// <returns></returns>
        public string GetCustomer()
        {
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            var cust = new BusinessPartnerSvc().GetPartnersCustomer(C_GUID);
            var strJson = ConvertToSelectJson(cust, "Name", "BP_GUID");
            return strJson.ToString();
        }


        /// <summary>
        /// 所有
        /// </summary>
        /// <returns></returns>
        public string GetPartnersAll()
        {
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            var partnersList = new BusinessPartnerSvc().GetPartnersAll(C_GUID);

            var strJson = ConvertToSelectJson(partnersList, "Name", "BP_GUID");
             return strJson.ToString();
        }

        /// <summary>
        /// 收款方
        /// </summary>
        /// <returns></returns>
        public JsonResult GetPayee()
        {
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            return Json(new BusinessPartnerSvc().GetPartners(C_GUID).Where(i => (i.IsSupplier || i.IsPartner)));
        }

        /// <summary>
        /// 收款方
        /// </summary>
        /// <returns></returns>
        public string GetPayeeJson()
        {
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            var supplierList = new BusinessPartnerSvc().GetSupplier(C_GUID);

            var strJson = ConvertToSelectJson(supplierList, "Name", "BP_GUID");
            return strJson.ToString();
        }

        /// <summary>
        /// 获取币值
        /// </summary>
        /// <returns></returns>
        public string GetCurrency()
        {
            string strFormatter = "{{\"text\":\"{0}\",\"value\":\"{0}\"}},";
            StringBuilder strJson = new StringBuilder("[");
            foreach (T_Currency item in new CurrencySvc().GetCurrency())
            {
                strJson.AppendFormat(strFormatter, item.Code);
            }
            strJson.Remove(strJson.Length - 1, 1);
            strJson.Append("]");
            return strJson.ToString();
        }
        /// <summary>
        /// 获取常用币值
        /// </summary>
        /// <returns></returns>
        public string GetCommonCurrency()
        {
            //string strFormatter = "{{\"label\":\"{0}\",\"value\":\"{1}\"}},";
            //StringBuilder strJson = new StringBuilder("[");
            //foreach (T_Currency item in new CurrencySvc().GetCurrency(true))
            //{
            //    strJson.AppendFormat(strFormatter, item.Code, item.Code);
            //}
            //strJson.AppendFormat(strFormatter, General.Resource.Common.More, -1);
            //strJson.Remove(strJson.Length - 1, 1);
            //strJson.Append("]");
            var currencyList = new CurrencySvc().GetCurrency(true);
            var strJson = ConvertToSelectJson1(currencyList, "Code", "Code");
            return strJson.ToString();
        }

        public string GetSearchCurrency()
        {
            var currencyList = new CurrencySvc().GetCurrency(true);
            var strJson = ConvertToSelectJson(currencyList, "Code", "Code");
            return strJson.ToString();
        }
        /// <summary>
        /// 获取税种
        /// </summary>
        /// <returns></returns>
        public string GetTaxList(string TaxPayer)
        {
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            if(TaxPayer == "1"){
                var taxList = new TaxSvc().GetTax(C_GUID);
                var strJson = ConvertToSelectJson(taxList, "Name", "T_GUID");
                return strJson.ToString();
            }else{
                var taxList = new TaxSvc().GetTax(C_GUID,TaxPayer);
                var strJson = ConvertToSelectJson(taxList, "Name", "T_GUID");
                return strJson.ToString();
            }
            
            
        }
        /// <summary>
        /// 获取单个税种
        /// </summary>
        /// <returns></returns>
        public string GetTaxOne(string value)
        {
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            string strFormatter = "{{\"Rate\":\"{0}\"}}";
            StringBuilder strJson = new StringBuilder("");
            foreach (T_Tax item in new TaxSvc().GetTaxOne(C_GUID, value))
            {
                strJson.AppendFormat(strFormatter, item.Rate);
            }
            return strJson.ToString();
        }
        public string GetCityAddress(string country)
        {

            string strFormatter = "{{\"label\":\"{0}\",\"value\":\"{1}\"}},";
            StringBuilder strJson = new StringBuilder("[");
            foreach (T_Address item in new CompanySvc().GetCityAddress(country))
            {
                strJson.AppendFormat(strFormatter, item.areaValue, item.areaValue);
            }
            
            strJson.Remove(strJson.Length - 1, 1);
            strJson.Append("]");
            return strJson.ToString();
        
        }
        public string GetPAddress() {
            string strFormatter = "{{\"label\":\"{0}\",\"value\":\"{1}\"}},";
            StringBuilder strJson = new StringBuilder("[");
            foreach (T_Address item in new CompanySvc().GetPAddress())
            {
                strJson.AppendFormat(strFormatter, item.areaValue, item.areaValue);
            }
          
            strJson.Remove(strJson.Length - 1, 1);
            strJson.Append("]");
            return strJson.ToString();
        
        }

        public String GetCAddress()
        {
            string strFormatter = "{{\"label\":\"{0}\",\"value\":\"{1}\"}},";
            StringBuilder strJson = new StringBuilder("[");
            foreach (T_Address item in new CompanySvc().GetCAddress())
            {
                strJson.AppendFormat(strFormatter, item.areaValue, item.areaValue);
            }
          
            strJson.Remove(strJson.Length - 1, 1);
            strJson.Append("]");
            return strJson.ToString();

        }

        public string GetCityAddress1(string province) 
        {
            string strFormatter = "{{\"label\":\"{0}\",\"value\":\"{1}\"}},";
            StringBuilder strJson = new StringBuilder("[");

           

            List<T_Address> TAddressList = new CompanySvc().GetCityAddress(province);

            foreach (T_Address item in TAddressList)
            {
                strJson.AppendFormat(strFormatter, item.areaValue, item.areaValue);
            }
        
            strJson.Remove(strJson.Length - 1, 1);
            strJson.Append("]");
            return strJson.ToString();
        
        }


        public string GetAddress()
        {
            string strFormatter = "{{\"label\":\"{0}\",\"value\":\"{1}\"}},";
            StringBuilder strJson = new StringBuilder("[");
            foreach (T_Address item in new CompanySvc().GetAddress())
            {
                strJson.AppendFormat(strFormatter, item.areaValue,item.areaValue);
            }
          
            strJson.Remove(strJson.Length - 1, 1);
            strJson.Append("]");
            return strJson.ToString();
        }

        public string GetAddressNew(string country)
        {
            string strFormatter = "{{\"label\":\"{0}\",\"value\":\"{1}\"}},";
            StringBuilder strJson = new StringBuilder("[");
            foreach (T_Address item in new CompanySvc().GetAddressNew(country))
            {
                strJson.AppendFormat(strFormatter, item.areaValue, item.areaValue);
            }

            strJson.Remove(strJson.Length - 1, 1);
            strJson.Append("]");
            return strJson.ToString();
        }
        /// <summary>
        /// 获取用户币值
        /// </summary>
        /// <returns></returns>
        public string GetUserCurrency()
        {
            //string strFormatter = "{{label:\'{0}\',value:\'{1}\'}},";
            StringBuilder strJson = new StringBuilder("[");
            foreach (T_Currency item in new CurrencySvc().GetUserCurrency(Session["CurrentCompanyGuid"].ToString()))
            {
                //strJson.AppendFormat(strFormatter, item.Code, item.Code);
                strJson.Append("{");
                strJson.AppendFormat("label:\'{0}\',", item.Code);
                strJson.AppendFormat("value:\'{0}\'", item.Code);
                strJson.Append("},");
            }
            strJson.Remove(strJson.Length - 1, 1);
            string json = (strJson.Append("]")).ToString();
            return json;
        }

        public string GetUserBank()
        {
            string strFormatter = "{{\"text\":\"{0}\",\"value\":\"{1}\"}},";
            StringBuilder strJson = new StringBuilder("[");
            foreach (T_BankAccount item in new BankAccountSvc().GetBankAccount(Session["CurrentCompanyGuid"].ToString()))
            {
                strJson.AppendFormat(strFormatter, item.Account, item.BA_GUID);
            }
            strJson.Remove(strJson.Length - 1, 1);
            strJson.Append("]");
            return strJson.ToString();
        }

        /// <summary>
        /// 获取现金流量科目
        /// </summary>
        /// <param name="flag">收付标识</param>
        /// <returns></returns>
        public string GetCashFlowItems(string flag = null)
        {
            return GenCashFlowItemJson(new ReportSvc().GetCashFlowItems(), null, flag);
        }

        /// <summary>
        /// 生成现金流量科目JSON
        /// </summary>
        /// <param name="ds">数据源</param>
        /// <param name="pid">父级标识</param>
        /// <param name="flag">收付标识</param>
        /// <returns></returns>
        private string GenCashFlowItemJson(List<T_CashFlowItem> ds, string pid, string flag)
        {
            string strFmter = "{{\"text\":\"{0}\",\"value\":\"{1}\",\"children\":{2}}},";
            StringBuilder strJson = new StringBuilder("[ ");
            IEnumerable<T_CashFlowItem> tmp = new List<T_CashFlowItem>();
            if (string.IsNullOrEmpty(pid))
            {
                tmp = ds.Where(i => i.PID == pid);
            }
            else
            {
                tmp = ds.Where(i => i.PID == pid && (string.IsNullOrEmpty(flag) || i.RP_Flag.Equals(flag)));
            }
            foreach (T_CashFlowItem item in tmp)
            {
                strJson.AppendFormat(strFmter, item.Name, item.R_GUID, GenCashFlowItemJson(ds, item.R_GUID, flag));
            }
            strJson.Remove(strJson.Length - 1, 1);
            strJson.Append("]");
            return strJson.ToString();
        }

        /// <summary>
        /// 获取总账科目
        /// </summary>
        /// <returns></returns>
        public string GetLedgerAccount()
        {
            string strFormatter = "{{\"text\":\"{0}\",\"value\":\"{1}\",\"Group\":\"{2}\"}},";
            StringBuilder strJson = new StringBuilder("[ ");
            foreach (T_GeneralLedgerAccount item in new AccountSvc().GetLedgerAccounts(Session["CurrentCompanyGuid"].ToString()))
            {
                strJson.AppendFormat(strFormatter, item.Name, item.LA_GUID, item.AccGroup);
            }
            strJson.Remove(strJson.Length - 1, 1);
            strJson.Append("]");
            return strJson.ToString();
        }

        /// <summary>
        /// 获取用户的总账科目
        /// </summary>
        /// <param name="accGrp">科目分组标识</param>
        /// <returns></returns>
        public string GetUserLedgerAccount(string accGrp = null)
        {
            string strFormatter = "{{\"text\":\"{0}\",\"value\":\"{1}\",\"Group\":\"{2}\",\"Code\":\"{3}\"}},";
            StringBuilder strJson = new StringBuilder("[ ");
            List<T_GeneralLedgerAccount> accs =
                new AccountSvc().GetUserLedgerAccounts(Session["CurrentCompanyGuid"].ToString());
            if (!string.IsNullOrEmpty(accGrp))
            {
                IEnumerable<int> accGrps = accGrp.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Cast<int>();
                accs = accs.Where(i => accGrps.Contains(i.AccGroup)).ToList();
            }
            foreach (T_GeneralLedgerAccount item in accs)
            {
                strJson.AppendFormat(strFormatter, item.Name, item.LA_GUID, item.AccGroup, item.AccCode);
            }
            strJson.Remove(strJson.Length - 1, 1);
            strJson.Append("]");
            return strJson.ToString();
        }

        /// <summary>
        /// 获取明细科目
        /// </summary>
        /// <returns></returns>
        public string GetDetailsAccount()
        {
            return GenerateDtlAccsJson(new AccountSvc().GetDetailsAccs(Session["CurrentCompanyGuid"].ToString()), string.Empty);
        }

        /// <summary>
        /// 获取明细科目
        /// </summary>
        /// <param name="id">科目标识</param>
        /// <returns></returns>
        public string GetDetailsAccounts(string id)
        {
            //string s = new JavaScriptSerializer().Serialize(new AccountSvc().GetDetailsAcc(id, Session["CurrentCompanyGuid"].ToString()));
            string s = new JavaScriptSerializer().Serialize(new AccountSvc().GetDetailsAcc(id));
            return s;
            //return new JavaScriptSerializer().Serialize(new AccountSvc().GetDetailsAcc(id, Session["CurrentCompanyGuid"].ToString()));
        }

        /// <summary>
        /// 获取明细科目
        /// </summary>
        /// <param name="pid">上级科目标识</param>
        /// <returns></returns>
        public string GetDetailsAccountParentAccGuid(string pid)
        {
            //string s=new JavaScriptSerializer().Serialize(new AccountSvc().GetDetailedAccountsParentAccGuid(Session["CurrentCompanyGuid"].ToString(), pid));
            string strFormatter = "{{\"label\":\"{0}\",\"value\":\"{1}\"}},";
            StringBuilder strJson = new StringBuilder("[");
            foreach (T_DetailedAccount item in new AccountSvc().GetDetailedAccountsParentAccGuid(Session["CurrentCompanyGuid"].ToString(), pid))
            {
                strJson.AppendFormat(strFormatter, item.Name, item.DA_GUID);
            }
            strJson.AppendFormat(strFormatter, General.Resource.Common.More, -1);
            strJson.Remove(strJson.Length - 1, 1);
            strJson.Append("]");
            return strJson.ToString();  
        }

        public string GetDetailType(string coloum) {
          
            var typeList = new AccountSvc().GetDetailType(Session["CurrentCompanyGuid"].ToString(), coloum);
            var strJson = ConvertToSelectJson(typeList, "ExpenseType", "ExpenseType");
            return strJson.ToString();
        
        }

        public string GetDetailSubject(string Name)
        {
            var  List = new AccountSvc().GetDetailLAByName(Name, Session["CurrentCompanyGuid"].ToString(),"");
            var strJson = ConvertToSelectJson(List, "Name", "DA_GUID");
            return strJson.ToString();
        }

        public string GetAllDetailSubject(string Name)
        {
            var List = new AccountSvc().GetDetailLAByName(Name, Session["CurrentCompanyGuid"].ToString(), "");
            var strJson = new JavaScriptSerializer().Serialize(List);
            return strJson.ToString();
        }

        public string GetThSubject(string Name) {
            var List = new AccountSvc().GetThByName(Name, Session["CurrentCompanyGuid"].ToString());
            var strJson = ConvertToSelectJson(List, "Name", "TDA_GUID");
            return strJson.ToString();
        }

        public string GetALLThSubject(string parentAccGuid)
        {
            var List = new AccountSvc().GetThByID(parentAccGuid, Session["CurrentCompanyGuid"].ToString());
            var strJson = ConvertToSelectJson(List, "Name", "TDA_GUID");
            return strJson.ToString();
        }
        /// <summary>
        /// 获取所有详细分类（包括税费）
        /// </summary>
        /// <returns></returns>
        /// <summary>
        public string GetAllSonType()
        {
            var typeList = new AccountSvc().GetAllSonType(Session["CurrentCompanyGuid"].ToString());
            var strJson = ConvertToSelectJson(typeList, "ExpenseType", "ExpenseType");
            return strJson.ToString();

        }

        /// <summary>
        /// 生成明细科目JSON
        /// </summary>
        /// <param name="ds">数据源</param>
        /// <param name="pid"></param>
        /// <returns></returns>
        private string GenerateDtlAccsJson(List<T_DetailedAccount> ds, string pid)
        {
            StringBuilder strJson = new StringBuilder("[ ");
            string strFormatter = "{{\"id\":\"{0}\",\"text\":\"{1}\",\"children\":{2}}},";
            string strChildren = string.Empty;
            foreach (T_DetailedAccount item in ds.Where(i => i.ParentAccGuid.Equals(pid)).OrderBy(i => i.Name))
            {
                strChildren = GenerateDtlAccsJson(ds, item.DA_GUID);
                strJson.AppendFormat(strFormatter, item.DA_GUID, item.Name,
                    strChildren);
            }
            strJson.Remove(strJson.Length - 1, 1);
            strJson.Append("]");
            return strJson.ToString();
        }

        /// <summary>
        /// 获取银行账户
        /// </summary>
        /// <returns></returns>
        public string GetBankAccounts()
        {
            StringBuilder strJson = new StringBuilder("[ ");
            string strFmt = "{{\"ID\":\"{0}\",\"Name\":\"{1}\",\"children\":{2}}},";
            foreach (T_BankAccount bank in new BankAccountSvc().GetBank(Session["CurrentCompanyGuid"].ToString()))
            {
                strJson.AppendFormat(strFmt, bank.B_GUID, bank.B_GUID, GetJson(bank.BA_GUID));
            }
            strJson.Remove(strJson.Length - 1, 1);
            strJson.Append("]");
            return strJson.ToString();
        }
        public string GetBankAccountss()
        {
            StringBuilder strJson = new StringBuilder("[ ");
            string strFmt = "{{\"label\":\"{0}\",\"value\":\"{1}\"}},";
            foreach (T_BankAccount bank in new BankAccountSvc().GetBank(Session["CurrentCompanyGuid"].ToString()))
            {
                strJson.AppendFormat(strFmt, bank.AccountAbbreviation, bank.BA_GUID);
            }
            //strJson.AppendFormat(strFmt, General.Resource.Common.More, -1);
            strJson.Remove(strJson.Length - 1, 1);
            strJson.Append("]");
            return strJson.ToString();
        }

        public string GetBankAccountsByName(string Type)
        {
            StringBuilder strJson = new StringBuilder("[ ");
            string strFmt = "{{\"label\":\"{0}\",\"value\":\"{1}\"}},";
            foreach (T_BankAccount bank in new BankAccountSvc().GetBankAccountsByName(Session["CurrentCompanyGuid"].ToString(),Type))
            {
                strJson.AppendFormat(strFmt, bank.AccountAbbreviation, bank.BA_GUID);
            }
            //strJson.AppendFormat(strFmt, General.Resource.Common.More, -1);
            strJson.Remove(strJson.Length - 1, 1);
            strJson.Append("]");
            return strJson.ToString();
        }

        public string GetBankAccountsByNameNew()
        {
            StringBuilder strJson = new StringBuilder("[ ");
            string strFmt = "{{\"label\":\"{0}\",\"value\":\"{1}\"}},";
            foreach (T_BankAccount bank in new BankAccountSvc().GetBankAccountsByNameNew(Session["CurrentCompanyGuid"].ToString()))
            {
                strJson.AppendFormat(strFmt, bank.AccountAbbreviation, bank.BA_GUID);
            }
            //strJson.AppendFormat(strFmt, General.Resource.Common.More, -1);
            strJson.Remove(strJson.Length - 1, 1);
            strJson.Append("]");
            return strJson.ToString();
        }


        ///// <summary>
        ///// 获取账号货币
        ///// </summary>
        ///// <returns></returns>
        public string GetBankAccountCurrency(string id)
        {
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            List<T_BankAccount> Record = new List<T_BankAccount>();
            Record = new BankAccountSvc().GetBankCurreny(id, C_GUID);
            return new JavaScriptSerializer().Serialize(Record);
        }

        /// <summary>
        /// 生成银行账户JSON
        /// </summary>
        /// <param name="pid">父级标识即银行标识</param>
        /// <returns></returns>
        private string GetJson(string pid)
        {
            StringBuilder strJson = new StringBuilder("[ ");
            string strFmt = "{{\"ID\":\"{0}\",\"Name\":\"{1}\",\"children\":{2}}},";
            foreach (T_BankAccount acc in new BankAccountSvc().GetBankAccount(Session["CurrentCompanyGuid"].ToString()).Where(i => i.B_GUID.Equals(pid)))
            {
                strJson.AppendFormat(strFmt, acc.BA_GUID, acc.Account, "[]");
            }
            strJson.Remove(strJson.Length - 1, 1);
            strJson.Append("]");
            return strJson.ToString();
        }
        private string GetJsons(string pid)
        {
            StringBuilder strJson = new StringBuilder("[ ");
            string strFmt = "{{\"label\":\"{0}\",\"value\":\"{1}\"}},";
            foreach (T_BankAccount acc in new BankAccountSvc().GetBankAccount(Session["CurrentCompanyGuid"].ToString()).Where(i => i.B_GUID.Equals(pid)))
            {
                strJson.AppendFormat(strFmt, acc.Account, acc.BA_GUID);
            }
            strJson.Remove(strJson.Length - 1, 1);
            strJson.Append("]");
            return strJson.ToString();
        }

        /// <summary>
        /// 获取税种
        /// </summary>
        /// <returns></returns>
        public string GetTax()
        {
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            return new JavaScriptSerializer().Serialize(new TaxSvc().GetTax(C_GUID));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetDetail()
        {
            return new JavaScriptSerializer().Serialize(new DetailSvc().GetAllDetail());
        }

        /// <summary>
        /// 删除记录的所有附件
        /// </summary>
        /// <param name="id">收款纪录标识</param>
        /// <returns></returns>
        private bool DelAttachment(string id)
        {
            return new AttachmentSvc().DelAttachment(id);

        }


        /// <summary>
        /// 上传图片
        /// </summary> 
        /// <param name="frGuid"></param> 
        /// <returns></returns>
        /// <remarks>liujf   2016/05/05   create</remarks>
        public ActionResult FileUpload(string frGuid, string TempFileNameFirst)
        {
            var files = Request.Files;
            ExceResult res = new ExceResult();
            //获取当前时间戳
            string CreateDate = GetDetailDate();
            if (files != null && files.Count > 0)
            {
                ControllerContext.HttpContext.Request.ContentEncoding = Encoding.GetEncoding("UTF-8");
                ControllerContext.HttpContext.Response.ContentEncoding = Encoding.GetEncoding("UTF-8");
                ControllerContext.HttpContext.Response.Charset = "UTF-8";

                //写入数据流
                Stream fileStream = files[0].InputStream;
                byte[] fileDataStream = new byte[files[0].ContentLength];
                fileStream.Read(fileDataStream, 0, files[0].ContentLength);

              

                //写入数据

                for (int iFile = 0; iFile < files.Count; iFile++)
                {
                    T_Attachment entity = new T_Attachment();
                    entity.A_GUID = Guid.NewGuid().ToString();
                    entity.FileName = TempFileNameFirst + CreateDate;
                    entity.FileType = files[0].ContentType;
                    entity.FR_GUID = frGuid;
                    entity.FlieData = fileDataStream;

                    bool rResult = new AttachmentSvc().AddAttachment(entity);
                    if (rResult)
                    {
                        res.success = true;
                    }
                    else
                    {
                        res.success = false;
                        res.msg = "保存附件异常";
                    }
                }
            }

            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(res));

        }

        /// <summary>
        /// 修改附件名称
        /// </summary> 
        /// <param name=""></param> 
        /// <returns></returns>
        /// <remarks>hdy   2017/03/09   create</remarks>
        public string UpdHaveFileUpload(T_Attachment form)
        {
            bool result = false;
            string msg = string.Empty;
            result = new AttachmentSvc().UpdHaveFileUpload(form);
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
        /// <remarks>liujf   2016/05/05   create</remarks>
        public FileResult DownLoadFile(string fileID)
        {
            AttachmentSvc attSv = new AttachmentSvc();
            var entity = attSv.GetAttachmentById(fileID);
            //从数据库查找
            return File(entity.FlieData, entity.FileType, entity.FileName);
        }

        /// <summary>
        /// 修改默认语言
        /// </summary>
        /// <returns></returns>
        public ActionResult ChangeLanguage(string language)
        {
            ExceResult res = new ExceResult();
            string guid = Session["CurrentUserGuid"].ToString();
            bool rResult = new UserSvc().ChangeLanguage(guid, language);
            if (rResult)
            {
                res.success = true;
                Session["Language"] = language;
            }
            else
            {
                res.success = false;
                res.msg = "保存附件异常";
            }
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(res));
        }
        /// <summary>
        /// 获取资产费用与成本类别
        /// </summary>
        /// <returns>JSON字符串（{code:**,name:**}）</returns>
        public ActionResult GetAssetCostType()
        {
            EF.FMS_DevelopEntities entities = new EF.FMS_DevelopEntities();

            var costTypeList = from a in entities.T_AssetCostType
                               select a;
            List<DropDownItem> items = new List<DropDownItem>();
            var index = 0;
            foreach (EF.T_AssetCostType type in costTypeList)
            {
                items.Add(new DropDownItem());
                index = items.Count - 1;

                items[index].label = type.name;

                items[index].value = type.code;

            }
            var costType = Newtonsoft.Json.JsonConvert.SerializeObject(items);
            return Content(costType);
        }
        public string ShowTime(string FR_GUID)
        {
            int count = 0;
            List<T_Attachment> Record = new List<T_Attachment>();
            Record = new AttachmentSvc().ShowTime(out count, FR_GUID);
            return new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue }.Serialize(Record);     
        }
        public string ShowUploadFile(string id, string dateBegin, string dateEnd, int pageIndex = 1, int pageSize = 10)
        {
            int count = 0;
            List<T_Attachment> Record = new List<T_Attachment>();
            Record = new AttachmentSvc().ShowUploadFile(id, pageIndex, -1, out count, dateBegin, dateEnd);
            return new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue }.Serialize(Record);          
        }

        /// <summary>
        /// 删除附件
        /// </summary>
        /// <param name="id">收款纪录标识</param>
        /// <returns></returns> 
        public string DelUploadAttachment(string id)
        {
            bool result = new AttachmentSvc().DelUploadAttachment(id);
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
    
        /// <summary>
        /// 下拉列表
        /// </summary>
        public class DropDownItem
        {
            /// <summary>
            /// 显示内容
            /// </summary>
            public string label { set; get; }

            /// <summary>
            /// 下拉项目的值
            /// </summary>
            public string value { set; get; }

        }
        

        


}
