
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using FMS.Model;

namespace FMS.DAL
{
    public class IESvc
    {
        /// <summary>
        /// 更新收入纪录
        /// </summary>
        /// <param name="head">收入概要</param>
        /// <param name="list">收入明细</param>
        /// <returns></returns>
        public bool UpdIncomeRecord(T_IERecord form)
        {
            form.IE_Flag = "I";
            return UpdIERecord(form);
        }

        /// <summary>
        /// 更新费用纪录
        /// </summary>
        /// <param name="head">费用概要</param>
        /// <param name="list">费用明细</param>
        /// <returns></returns>
        public bool UpdExpenseRecord(T_IERecord form)
        {
            form.IE_Flag = "E";
            return UpdIERecord(form);
        }

        /// <summary>
        /// 更新收入/费用纪录
        /// </summary>
        /// <param name="head">概要</param>
        /// <param name="list">明细</param>
        /// <returns></returns>
        private bool UpdIERecord(T_IERecord rec)
        {
            DBHelper dh = new DBHelper();
            dh.BeginTran();
            try {
                dh.strCmd = "SP_UpdIERecord";
                dh.AddPare("@IE_GUID", SqlDbType.NVarChar, 40, rec.IE_GUID);
                dh.AddPare("@IE_Flag", SqlDbType.NVarChar, 1, rec.IE_Flag);
                dh.AddPare("@InvType", SqlDbType.NVarChar, 40, rec.InvType);
                dh.AddPare("@IEGroup", SqlDbType.NVarChar, 40, rec.IEGroup);
                dh.AddPare("@IEDescription", SqlDbType.NVarChar, 40, rec.IEDescription);
                dh.AddPare("@InvNo", SqlDbType.NVarChar, 20, rec.InvNo);
                dh.AddPare("@RPer", SqlDbType.NVarChar, 40, rec.RPer);
                dh.AddPare("@Creator", SqlDbType.NVarChar, 40, rec.Creator);
                dh.AddPare("@CreateDate", SqlDbType.DateTime, 0, rec.CreateDate);
                dh.AddPare("@C_GUID", SqlDbType.NVarChar, 40, rec.C_GUID);
                if (!rec.AffirmDate.Equals(DateTime.MinValue))
                {
                   dh.AddPare("@AffirmDate", SqlDbType.DateTime, 0, rec.AffirmDate); 
                }
                if (!rec.Date.Equals(DateTime.MinValue))
                {
                    dh.AddPare("@Date", SqlDbType.DateTime, 0, rec.Date);
                }
                dh.AddPare("@Amount", SqlDbType.Decimal, 0, rec.Amount);
                dh.AddPare("@TaxationAmount", SqlDbType.Decimal, 0, rec.TaxationAmount);
                dh.AddPare("@TaxationType", SqlDbType.NVarChar, 40, rec.TaxationType);
                dh.AddPare("@SumAmount", SqlDbType.Decimal, 0, rec.SumAmount == 0 ? (rec.TaxationAmount + rec.Amount) : rec.SumAmount);
                dh.AddPare("@Remark", SqlDbType.NVarChar, 200, rec.Remark);
                dh.AddPare("@Currency", SqlDbType.NVarChar, 20, rec.Currency);
                dh.AddPare("@B_GUID", SqlDbType.NVarChar, 40, rec.B_GUID);
                dh.AddPare("@BA_GUID", SqlDbType.NVarChar, 40, rec.BA_GUID);
                dh.AddPare("@Profit_GUID", SqlDbType.NVarChar, 40, rec.Profit_GUID);
                dh.AddPare("@State", SqlDbType.NVarChar, 40, rec.State);
                dh.NonQuery();
                dh.CleanPara();
                if (rec.IE_Flag == "I")
                {
                    dh.strCmd = "SP_UpdReceivables";
                    dh.AddPare("@R_GUID", SqlDbType.NVarChar, 40, rec.IE_GUID);
                    dh.AddPare("@C_GUID", SqlDbType.NVarChar, 40, rec.C_GUID);
                    dh.AddPare("@Payer", SqlDbType.NVarChar, 40, rec.RPer);
                    if (!rec.AffirmDate.Equals(DateTime.MinValue))
                    {
                        dh.AddPare("@Date", SqlDbType.DateTime, 0, rec.AffirmDate);
                    }
                    dh.AddPare("@InvType", SqlDbType.NVarChar, 40, rec.InvType);
                    dh.AddPare("@InvNo", SqlDbType.NVarChar, 40, rec.InvNo);
                    dh.AddPare("@B_GUID", SqlDbType.NVarChar, 40, rec.B_GUID);
                    dh.AddPare("@BA_GUID", SqlDbType.NVarChar, 40, rec.BA_GUID);
                    dh.AddPare("@Money", SqlDbType.Decimal, 0, rec.Amount + rec.TaxationAmount);
                    dh.AddPare("@Currency", SqlDbType.NVarChar, 5, rec.Currency);
                    dh.NonQuery();
                }
                else if (rec.IE_Flag == "E")
                {
                    dh.strCmd = "SP_UpdPayable";
                    dh.AddPare("@R_GUID", SqlDbType.NVarChar, 40, rec.IE_GUID);
                    dh.AddPare("@C_GUID", SqlDbType.NVarChar, 40, rec.C_GUID);
                    dh.AddPare("@Payee", SqlDbType.NVarChar, 40, rec.RPer);
                    if (!rec.AffirmDate.Equals(DateTime.MinValue))
                    {
                        dh.AddPare("@Date", SqlDbType.DateTime, 0, rec.AffirmDate);
                    }
                    dh.AddPare("@InvType", SqlDbType.NVarChar, 40, rec.InvType);
                    dh.AddPare("@InvNo", SqlDbType.NVarChar, 40, rec.InvNo);
                    dh.AddPare("@B_GUID", SqlDbType.NVarChar, 40, rec.B_GUID);
                    dh.AddPare("@BA_GUID", SqlDbType.NVarChar, 40, rec.BA_GUID);
                    dh.AddPare("@Money", SqlDbType.Decimal, 0, rec.Amount + rec.TaxationAmount);
                    dh.AddPare("@Currency", SqlDbType.NVarChar, 5, rec.Currency);
                    dh.NonQuery();
                }
                dh.CommitTran();
                return true;
            }

            catch
            {
                dh.RollBackTran();
                return false;
            }
        }

        public bool UpdWageCost(T_WageCost rec)
        {
            DBHelper dh = new DBHelper();
            dh.BeginTran();
            try
            {
                dh.strCmd = "SP_UpdWageCost";
                dh.AddPare("@W_GUID", SqlDbType.NVarChar, 40, rec.W_GUID);
                dh.AddPare("@C_GUID", SqlDbType.NVarChar, 40, rec.C_GUID);
                dh.AddPare("@Employee", SqlDbType.NVarChar, 40, rec.Employee);
                dh.AddPare("@Cash", SqlDbType.Decimal, 0, rec.Cash);
                dh.AddPare("@PersonalTaxes", SqlDbType.Decimal, 0, rec.PersonalTaxes);
                dh.AddPare("@SocialSecurity", SqlDbType.Decimal, 0, rec.SocialSecurity);
                dh.AddPare("@Total", SqlDbType.Decimal, 0, rec.Total);
                if (!rec.Date.Equals(DateTime.MinValue))
                {
                    dh.AddPare("@Date", SqlDbType.DateTime, 0, rec.Date);
                }
                dh.NonQuery();
                dh.CleanPara();
                dh.CommitTran();
                return true;
            }
            catch
            {
                dh.RollBackTran();
                return false;
            }
        }

        /// <summary>
        /// 获取收入列表
        /// </summary>
        /// <param name="C_GUID">公司标识</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="count">纪录总数</param>
        /// <returns></returns>
        public List<T_IERecord> GetIncomeList(string C_GUID, int pageIndex, int pageSize, out int count, string dateBegin, string dateEnd, string customer, string status, string incomeGrp)
        {
            return GetIEList("I", C_GUID, pageIndex, pageSize, out count, dateBegin, dateEnd, customer, status, incomeGrp);
        }

        /// <summary>
        /// 获取应收款列表
        /// </summary>
        /// <param name="C_GUID">公司标识</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="count">纪录总数</param>
        /// <returns></returns>
        public List<T_IERecord> GetReceivableList(string C_GUID, int pageIndex, int pageSize, out int count)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetChooseReceivablesRecord";
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@PageIndex", SqlDbType.Int, 0, pageIndex);
            dh.AddPare("@PageSize", SqlDbType.Int, 0, pageSize);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            List<T_IERecord> result = new List<T_IERecord>();
            result = dh.Reader<T_IERecord>();
            count = dh.GetParaValue<int>("@Count");
            return result;
        }

        /// <summary>
        /// 获取应付款列表
        /// </summary>
        /// <param name="C_GUID">公司标识</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="count">纪录总数</param>
        /// <returns></returns>
        public List<T_IERecord> GetPayableList(string C_GUID, int pageIndex, int pageSize, out int count)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetChoosePayablesRecord";
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@PageIndex", SqlDbType.Int, 0, pageIndex);
            dh.AddPare("@PageSize", SqlDbType.Int, 0, pageSize);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            List<T_IERecord> result = new List<T_IERecord>();
            result = dh.Reader<T_IERecord>();
            count = dh.GetParaValue<int>("@Count");
            return result;
        }

        /// <summary>
        /// 获取所有收入列表
        /// </summary>
        /// <param name="C_GUID">公司标识</param>
        /// <returns></returns>
        public List<T_IERecord> GetAllIncomeList(string C_GUID, int page, int rows, out int count, string dateBegin, string dateEnd, string customer, string state, string incomeGrp)
        {
            return GetAllIEList("I", C_GUID, page, rows, out count, dateBegin, dateEnd, customer, state, incomeGrp);
        }

        /// <summary>
        /// 获取费用列表
        /// </summary>
        /// <param name="C_GUID">公司标识</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="count">纪录总数</param>
        /// <returns></returns>
        public List<T_IERecord> GetExpenseList(string C_GUID, int pageIndex, int pageSize, out int count)
        {
            return GetIEList("E", C_GUID, pageIndex, pageSize, out count, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
        }

        /// <summary>
        ///  获取所有费用列表
        /// </summary>
        /// <param name="C_GUID">公司标识</param>
        /// <returns></returns>
        public List<T_IERecord> GetAllExpenseList(string C_GUID, int page, int rows, out int count, string dateBegin, string dateEnd, string customer, string state, string incomeGrp, string IncomeGrpDts)
        {
            return GetAllIEList("E", C_GUID, page, rows, out count, dateBegin, dateEnd, customer, state, incomeGrp, IncomeGrpDts);
        }

        /// <summary>
        /// 获取收入\费用列表
        /// </summary>
        /// <param name="flag">收入\费用标识</param>
        /// <param name="C_GUID">公司标识</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="count">纪录总数</param>
        /// <returns></returns>
        private List<T_IERecord> GetIEList(string flag, string C_GUID, int pageIndex, int pageSize, out int count, string dateBegin, string dateEnd, string customer, string status, string incomeGrp)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetIEs";
            dh.AddPare("@Flag", SqlDbType.NVarChar, 1, flag);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@PageIndex", SqlDbType.Int, 0, pageIndex);
            dh.AddPare("@PageSize", SqlDbType.Int, 0, pageSize);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            if (!string.IsNullOrEmpty(dateBegin))
            {
                dh.AddPare("@dateBegin", SqlDbType.DateTime, 0, DateTime.Parse(dateBegin));
            }
            if (!string.IsNullOrEmpty(dateEnd))
            {
                dh.AddPare("@dateEnd", SqlDbType.DateTime, 0, DateTime.Parse(dateEnd));
            }
            dh.AddPare("@customer", SqlDbType.NVarChar, 40, customer);
            dh.AddPare("@status", SqlDbType.Bit, 0, status.Equals("1")?true:false);
            dh.AddPare("@incomeGrp", SqlDbType.NVarChar, 20, incomeGrp);
            List<T_IERecord> result = new List<T_IERecord>();
            result = dh.Reader<T_IERecord>();
            count = dh.GetParaValue<int>("@Count");
            return result;
        }

        /// <summary>
        /// 获取所有收入\费用列表(包括历史数据)
        /// </summary>
        /// <param name="flag">收入\费用标识</param>
        /// <param name="C_GUID">公司标识</param>
        /// <returns></returns>
        private List<T_IERecord> GetAllIEList(string flag, string C_GUID, int pageIndex, int pageSize, out int count, string dateBegin, string dateEnd, string customer, string state, string incomeGrp)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetIEs";
            dh.AddPare("@Flag", SqlDbType.NVarChar, 1, flag);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@IsAll", SqlDbType.Bit, 0, true);
            dh.AddPare("@PageIndex", SqlDbType.Int, 0, pageIndex);
            dh.AddPare("@PageSize", SqlDbType.Int, 0, pageSize);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            if (!string.IsNullOrEmpty(dateBegin))
            {
                dh.AddPare("@dateBegin", SqlDbType.DateTime, 0, DateTime.Parse(dateBegin));
            }
            if (!string.IsNullOrEmpty(dateEnd))
            {
                dh.AddPare("@dateEnd", SqlDbType.DateTime, 0, DateTime.Parse(dateEnd));
            }
            dh.AddPare("@customer", SqlDbType.NVarChar, 40, customer);
            dh.AddPare("@incomeGrp", SqlDbType.NVarChar, 20, incomeGrp);
            dh.AddPare("@state", SqlDbType.NVarChar, 40, state);
            List<T_IERecord> result = new List<T_IERecord>();
            result = dh.Reader<T_IERecord>();
            count = dh.GetParaValue<int>("@Count");
            return result;
        }

        private List<T_IERecord> GetAllIEList(string flag, string C_GUID, int pageIndex, int pageSize, out int count, string dateBegin, string dateEnd, string customer, string state, string incomeGrp, string IncomeGrpDts)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetIEs";
            dh.AddPare("@Flag", SqlDbType.NVarChar, 1, flag);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@IsAll", SqlDbType.Bit, 0, true);
            dh.AddPare("@PageIndex", SqlDbType.Int, 0, pageIndex);
            dh.AddPare("@PageSize", SqlDbType.Int, 0, pageSize);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            if (!string.IsNullOrEmpty(dateBegin))
            {
                dh.AddPare("@dateBegin", SqlDbType.DateTime, 0, DateTime.Parse(dateBegin));
            }
            if (!string.IsNullOrEmpty(dateEnd))
            {
                dh.AddPare("@dateEnd", SqlDbType.DateTime, 0, DateTime.Parse(dateEnd));
            }
            dh.AddPare("@customer", SqlDbType.NVarChar, 40, customer);
            dh.AddPare("@incomeGrp", SqlDbType.NVarChar, 20, incomeGrp);
            dh.AddPare("@IncomeGrpDts", SqlDbType.NVarChar, 50, IncomeGrpDts);
            dh.AddPare("@state", SqlDbType.NVarChar, 40, state);
            List<T_IERecord> result = new List<T_IERecord>();
            result = dh.Reader<T_IERecord>();
            count = dh.GetParaValue<int>("@Count");
            return result;
        }
        /// <summary>
        /// 获取收入\费用纪录
        /// </summary>
        /// <param name="id">收入\费用纪录标识</param>
        /// <param name="C_GUID">公司标识</param>
        /// <returns></returns>
        public T_IERecord GetIE(string id, string C_GUID)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetIEs";
            dh.AddPare("@ID", SqlDbType.NVarChar, 40, id);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            return dh.Reader<T_IERecord>().FirstOrDefault();
        }

        /// <summary>
        /// 更新商业伙伴
        /// </summary>
        /// <param name="partner">商业伙伴对象</param>
        /// <returns></returns>
        public bool UpdPartner(T_BusinessPartner partner)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_UpdPartner";
            dh.AddPare("@ID", SqlDbType.NVarChar, 40, partner.BP_GUID);
            dh.AddPare("@Name", SqlDbType.NVarChar, 100, partner.Name);
            dh.AddPare("@IsCustomer", SqlDbType.Bit, 0, partner.IsCustomer);
            dh.AddPare("@ISSupplier", SqlDbType.Bit, 0, partner.IsSupplier);
            dh.AddPare("@IsPartner", SqlDbType.Bit, 0, partner.IsPartner);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, partner.C_GUID);
            try
            {
                dh.NonQuery();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 获取收入\费用纪录明细
        /// </summary>
        /// <param name="id">收入\费用纪录标识</param>
        /// <param name="C_GUID">公司标识</param>
        /// <returns></returns>
        public List<T_IEDetails> GetIEDetails(string id, string C_GUID)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetIEDetails";
            dh.AddPare("@IE_ID", SqlDbType.NVarChar, 40, id);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            return dh.Reader<T_IEDetails>();
        }

        /// <summary>
        /// 删除收入\费用纪录
        /// </summary>
        /// <param name="id">收入\费用纪录标识</param>
        /// <param name="flag">收入\费用标识</param>
        /// <returns></returns>
        public bool DelIERecord(string id, string flag)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_DelIERecord";
            dh.AddPare("@ID", SqlDbType.NVarChar, 40, id);
            dh.AddPare("@Flag", SqlDbType.NVarChar, 4, flag);
            try
            {
                dh.NonQuery();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 获取收入/费用
        /// </summary>
        /// <param name="id">IE_id</param>
        /// <param name="C_GUID">公司标识</param>
        /// <param name="flag">收入\费用标识</param>
        /// <returns></returns>
        public List<T_IERecord> GetIncomeToReceivables(string id, string C_GUID, string flag)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetIEs";
            dh.AddPare("@Flag", SqlDbType.NVarChar, 1, flag);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@ID", SqlDbType.NVarChar, 40, id);
            List<T_IERecord> result = new List<T_IERecord>();
            result = dh.Reader<T_IERecord>();
            return result;
        }
    }
}
