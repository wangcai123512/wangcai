using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Common.BaseControllers;
using FMS.DAL;
using FMS.Model;
using System;
using System.Web.Script.Serialization;

namespace FMS.BLL
{
	/// <summary>
	/// 内部数据接口
	/// </summary>
	public class InternalAPIController : APIController
	{
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
				new CompanySvc().GetCompanySetting(Session["CurrentCompany"].ToString());
			return new JavaScriptSerializer().Serialize(setting);
		}
		/// <summary>
		/// 付款方
		/// </summary>
		/// <returns></returns>
		public JsonResult GetPayer()
		{
			string C_GUID = Session["CurrentCompany"].ToString();
			return Json(new BusinessPartnerSvc().GetPartners(C_GUID).Where(i => (i.IsCustomer || i.IsPartner)));
		}

		/// <summary>
		/// 收款方
		/// </summary>
		/// <returns></returns>
		public JsonResult GetPayee()
		{
			string C_GUID = Session["CurrentCompany"].ToString();
			return Json(new BusinessPartnerSvc().GetPartners(C_GUID).Where(i => (i.IsSupplier || i.IsPartner)));
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
			string strFormatter = "{{\"text\":\"{0}\",\"value\":\"{1}\"}},";
			StringBuilder strJson = new StringBuilder("[");
			foreach (T_Currency item in new CurrencySvc().GetCurrency(true))
			{
				strJson.AppendFormat(strFormatter, item.Code, item.Code);
			}
			strJson.AppendFormat(strFormatter, General.Resource.Common.More, -1);
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
			string strFormatter = "{{\"text\":\"{0}\",\"value\":\"{1}\"}},";
			StringBuilder strJson = new StringBuilder("[");
			foreach (T_Currency item in new CurrencySvc().GetUserCurrency(Session["CurrentCompany"].ToString()))
			{
				strJson.AppendFormat(strFormatter, item.Code, item.Code);
			}
			strJson.Remove(strJson.Length - 1, 1);
			strJson.Append("]");
			return strJson.ToString();
		}

        public string GetUserBank()
        {
            string strFormatter = "{{\"text\":\"{0}\",\"value\":\"{1}\"}},";
            StringBuilder strJson = new StringBuilder("[");
            foreach (T_BankAccount item in new BankAccountSvc().GetBankAccount(Session["CurrentCompany"].ToString()))
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
			return GenCashFlowItemJson(new ReportSvc().GetCashFlowItems(), null,flag);
		}

		/// <summary>
		/// 生成现金流量科目JSON
		/// </summary>
		/// <param name="ds">数据源</param>
		/// <param name="pid">父级标识</param>
		/// <param name="flag">收付标识</param>
		/// <returns></returns>
		private string GenCashFlowItemJson(List<T_CashFlowItem> ds,string pid,string flag)
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
				tmp = ds.Where(i => i.PID == pid && ( string.IsNullOrEmpty(flag) ||i.RP_Flag.Equals(flag) ));
			}
			foreach (T_CashFlowItem item in tmp )
			{
				strJson.AppendFormat(strFmter,item.Name,item.R_GUID, GenCashFlowItemJson(ds,item.R_GUID,flag));
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
			foreach (T_GeneralLedgerAccount item in new AccountSvc().GetLedgerAccounts(Session["CurrentCompany"].ToString()))
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
				new AccountSvc().GetUserLedgerAccounts(Session["CurrentCompany"].ToString());
			if (!string.IsNullOrEmpty(accGrp))
			{
				IEnumerable<int> accGrps = accGrp.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Cast<int>();
				accs = accs.Where(i => accGrps.Contains(i.AccGroup)).ToList();
			}
			foreach (T_GeneralLedgerAccount item in accs)
			{
				strJson.AppendFormat(strFormatter, item.Name, item.LA_GUID, item.AccGroup,item.AccCode);
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
			return GenerateDtlAccsJson(new AccountSvc().GetDetailsAccs(Session["CurrentCompany"].ToString()), string.Empty);
		}

        /// <summary>
        /// 获取明细科目
        /// </summary>
        /// <param name="id">科目标识</param>
        /// <returns></returns>
        public string GetDetailsAccounts(string id)
        {
            //string s = new JavaScriptSerializer().Serialize(new AccountSvc().GetDetailsAcc(id, Session["CurrentCompany"].ToString()));
            string s = new JavaScriptSerializer().Serialize(new AccountSvc().GetDetailsAcc(id));
            return s;
            //return new JavaScriptSerializer().Serialize(new AccountSvc().GetDetailsAcc(id, Session["CurrentCompany"].ToString()));
        }

        /// <summary>
        /// 获取明细科目
        /// </summary>
        /// <param name="pid">上级科目标识</param>
        /// <returns></returns>
        public string GetDetailsAccountParentAccGuid(string pid)
        {
            string s=new JavaScriptSerializer().Serialize(new AccountSvc().GetDetailedAccountsParentAccGuid(Session["CurrentCompany"].ToString(), pid));
            return s;
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
			foreach (T_Bank bank in new BankAccountSvc().GetBank(Session["CurrentCompany"].ToString()))
			{
				strJson.AppendFormat(strFmt, bank.B_GUID, bank.Name, GetJson(bank.B_GUID));
			}
			strJson.Remove(strJson.Length - 1, 1);
			strJson.Append("]");
			return strJson.ToString();
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
			foreach (T_BankAccount acc in new BankAccountSvc().GetBankAccount(Session["CurrentCompany"].ToString()).Where(i => i.B_GUID.Equals(pid)))
			{
				strJson.AppendFormat(strFmt, acc.BA_GUID, acc.Account, "[]");
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
            string C_GUID = Session["CurrentCompany"].ToString();
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
	}
}
