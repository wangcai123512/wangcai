using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using FMS.Model;

namespace FMS.DAL
{
    public class RecPayRecordSvc
    {
        /// <summary>
        /// 更新收款纪录
        /// </summary>
        /// <param name="rec">收付款纪录对象</param>
        /// <returns></returns>
        public bool UpdReceivablesRecord(T_RecPayRecord rec)
        {
            rec.RP_Flag = "R";
            return UpdRecPayRecord(rec);
        }

        /// <summary>
        /// 更新收款纪录
        /// </summary>
        /// <param name="rec">收付款纪录对象</param>
        /// <returns></returns>
        public bool UpdPaymentRecord(T_RecPayRecord rec)
        {
            rec.RP_Flag = "P";
            return UpdRecPayRecord(rec);
        }

        /// <summary>
        /// 更新收/付款纪录
        /// </summary>
        /// <param name="rec">收付款纪录对象</param>
        /// <returns></returns>
        private bool UpdRecPayRecord(T_RecPayRecord rec)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_UpdRecPayRecord";
            dh.AddPare("@ID", SqlDbType.NVarChar, 40, rec.RP_GUID);
            dh.AddPare("@Flag", SqlDbType.NVarChar, 1, rec.RP_Flag);
            dh.AddPare("@InvType", SqlDbType.NVarChar, 20, rec.InvType);
            dh.AddPare("@InvTypeDts", SqlDbType.NVarChar, 50, rec.InvTypeDts);
            dh.AddPare("@InvNo", SqlDbType.NVarChar, 20, rec.InvNo);
            dh.AddPare("@R_Per", SqlDbType.NVarChar, 40, rec.RPer);
            dh.AddPare("@DLA", SqlDbType.NVarChar, 40, rec.DebitLedgerAccount);
            dh.AddPare("@DDA", SqlDbType.NVarChar, 40, rec.DebitDetailsAccount);
            dh.AddPare("@CLA", SqlDbType.NVarChar, 40, rec.CreditLedgerAccount);
            dh.AddPare("@CDA", SqlDbType.NVarChar, 40, rec.CreditDetailsAccount);
            dh.AddPare("@Amount", SqlDbType.Decimal, 0, rec.SumAmount);
            dh.AddPare("@Date", SqlDbType.Date, 0, rec.Date);
            dh.AddPare("@Remark", SqlDbType.NVarChar, 200, rec.Remark);
            dh.AddPare("@Creator", SqlDbType.NVarChar, 40, rec.Creator);
            dh.AddPare("@CreateDate", SqlDbType.DateTime, 0, rec.CreateDate);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, rec.C_GUID);
            dh.AddPare("@RPable", SqlDbType.NVarChar, 40, rec.RPable);
            dh.AddPare("@Currency", SqlDbType.NVarChar, 5, rec.Currency);
            dh.AddPare("@CFItemGuid", SqlDbType.NVarChar, 40, rec.CFItemGuid);
            dh.AddPare("@CFPItemGuid", SqlDbType.NVarChar, 40, rec.CFPItemGuid);
            dh.AddPare("@B_GUID", SqlDbType.NVarChar, 40, rec.B_GUID);
            dh.AddPare("@BA_GUID", SqlDbType.NVarChar, 40, rec.BA_GUID);
            dh.AddPare("@IE_GUID", SqlDbType.NVarChar, 40, rec.IE_GUID);
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
        /// 获取收款列表
        /// </summary>
        /// <param name="C_GUID">公司标识</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="count">纪录总数</param>
        /// <returns></returns>
        public List<T_RecPayRecord> GetReceivablesRecord(string C_GUID, int pageIndex, int pageSize, out int count)
        {
            return GetRecPayRecords(pageIndex, pageSize, out count, C_GUID, "R");
        }

        /// <summary>
        /// 获取未归档的收款纪录
        /// </summary>
        /// <param name="C_GUID">公司标识</param>
        /// <returns></returns>
        public List<T_RecPayRecord> GetUnclassifyReceivablesRecord(string C_GUID)
        {
            int count = 0;
            return GetRecPayRecords(0, -1, out count, C_GUID, "R", false);
        }

        /// <summary>
        /// 获取未归档的收款纪录
        /// </summary>
        /// <param name="C_GUID">公司标识</param>
        /// <returns></returns>
        public List<T_RecPayRecord> GetUnclassifyReceivablesRecord(string C_GUID, int page, int rows, out int count, string dateBegin, string dateEnd, string customer, string incomeGrp, string incomeGrpdts=null)
        {
            return GetRecPayRecordsCopy(page, rows, out count, C_GUID,dateBegin,dateEnd,customer,incomeGrp,incomeGrpdts, "R", false);
        }

        public List<T_RecPayRecord> GetReceivablesSelfList(string C_GUID, int page, int rows, out int count, string dateBegin, string dateEnd, string customer, string incomeGrp, string incomeGrpdts = null)
        {
            return GetReceivablesSelfList(page, rows, out count, C_GUID, dateBegin, dateEnd, customer, incomeGrp, incomeGrpdts, "R", false);
        }

        public List<T_RecPayRecord> GetPaymentSelfList(string C_GUID, int page, int rows, out int count, string dateBegin, string dateEnd, string customer, string incomeGrp, string incomeGrpdts = null)
        {
            return GetPaymentSelfList(page, rows, out count, C_GUID, dateBegin, dateEnd, customer, incomeGrp, incomeGrpdts, "P", false);
        }

        public List<T_RecPayRecord> GetPaymentSelfListTwo(string C_GUID, int page, int rows, out int count, string dateBegin, string dateEnd, string customer, string incomeGrp, string incomeGrpdts = null)
        {
            return GetPaymentSelfListTwo(page, rows, out count, C_GUID, dateBegin, dateEnd, customer, incomeGrp, incomeGrpdts, "P", false);
        }

        /// <summary>
        /// 获取所有收款纪录
        /// </summary>
        /// <param name="C_GUID">公司标识</param>
        /// <returns></returns>
        public List<T_RecPayRecord> GetAllReceivablesRecord(string C_GUID)
        {
            return GetAllRecPayRecords(C_GUID, "R");
        }

        /// <summary>
        /// 获取付款纪录
        /// </summary>
        /// <param name="C_GUID">公司标识</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="count">纪录总数</param>
        /// <returns></returns>
        public List<T_RecPayRecord> GetPaymentRecord(string C_GUID, int pageIndex, int pageSize, out int count)
        {
            return GetRecPayRecords(pageIndex, pageSize, out count, C_GUID, "P");
        }

        /// <summary>
        /// 获取未归档的付款纪录
        /// </summary>
        /// <param name="C_GUID">公司标识</param>
        /// <returns></returns>
        public List<T_RecPayRecord> GetUnclassifyPaymentRecord(string C_GUID)
        {
            int count = 0;
            return GetRecPayRecords(0, -1, out count, C_GUID, "P", false);
        }

        /// <summary>
        /// 获取未归档的付款纪录
        /// </summary>
        /// <param name="C_GUID">公司标识</param>
        /// <returns></returns>
        public List<T_RecPayRecord> GetUnclassifyPaymentRecord(string C_GUID, int page, int rows, out int count, string dateBegin, string dateEnd, string customer, string incomeGrp, string incomeGrpdts=null)
        {
            return GetRecPayRecordsCopy(page, rows, out count, C_GUID, dateBegin, dateEnd, customer, incomeGrp,incomeGrpdts, "P", false);
        }

        /// <summary>
        /// 获取所有付款纪录
        /// </summary>
        /// <param name="C_GUID">公司标识</param>
        /// <returns></returns>
        public List<T_RecPayRecord> GetAllPaymentRecord(string C_GUID)
        {
            return GetAllRecPayRecords(C_GUID, "P");
        }

        /// <summary>
        /// 获取收付款纪录列表
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="count">纪录总数</param>
        /// <param name="C_GUID">公司标识</param>
        /// <param name="flag">收付标识</param>
        /// <param name="classifyFlag">归档标识</param>
        /// <returns></returns>
        private List<T_RecPayRecord> GetRecPayRecords(int pageIndex, int pageSize, out int count, string C_GUID, string flag = null, bool? classifyFlag = null)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetRecPayRecord";
            dh.AddPare("@PageIndex", SqlDbType.Int, 0, pageIndex);
            dh.AddPare("@PageSize", SqlDbType.Int, 0, pageSize);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            dh.AddPare("@Flag", SqlDbType.NVarChar, 1, flag);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@ClassifyFlag", SqlDbType.Bit, 0, classifyFlag);
            List<T_RecPayRecord> result = new List<T_RecPayRecord>();
            result = dh.Reader<T_RecPayRecord>();
            count = dh.GetParaValue<int>("@Count");
            return result;
        }

        /// <summary>
        /// 获取收付款纪录列表
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="count">纪录总数</param>
        /// <param name="C_GUID">公司标识</param>
        /// <param name="flag">收付标识</param>
        /// <param name="classifyFlag">归档标识</param>
        /// <returns></returns>
        private List<T_RecPayRecord> GetRecPayRecordsCopy(int pageIndex, int pageSize, out int count, string C_GUID, string dateBegin, string dateEnd, string customer, string incomeGrp,string incomeGrpdts=null, string flag = null, bool? classifyFlag = null)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetRecPayRecordCopy";
            dh.AddPare("@PageIndex", SqlDbType.Int, 0, pageIndex);
            dh.AddPare("@PageSize", SqlDbType.Int, 0, pageSize);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            dh.AddPare("@Flag", SqlDbType.NVarChar, 1, flag);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@ClassifyFlag", SqlDbType.Bit, 0, classifyFlag);
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
            dh.AddPare("@incomeGrpdts", SqlDbType.NVarChar, 40, incomeGrpdts);
            List<T_RecPayRecord> result = new List<T_RecPayRecord>();
            result = dh.Reader<T_RecPayRecord>();
            count = dh.GetParaValue<int>("@Count");
            return result;
        }
        private List<T_RecPayRecord> GetReceivablesSelfList(int pageIndex, int pageSize, out int count, string C_GUID, string dateBegin, string dateEnd, string customer, string incomeGrp, string incomeGrpdts = null, string flag = null, bool? classifyFlag = null)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetReceivablesSelfList";
            dh.AddPare("@PageIndex", SqlDbType.Int, 0, pageIndex);
            dh.AddPare("@PageSize", SqlDbType.Int, 0, pageSize);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            dh.AddPare("@Flag", SqlDbType.NVarChar, 1, flag);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@ClassifyFlag", SqlDbType.Bit, 0, classifyFlag);
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
            dh.AddPare("@incomeGrpdts", SqlDbType.NVarChar, 40, incomeGrpdts);
            List<T_RecPayRecord> result = new List<T_RecPayRecord>();
            result = dh.Reader<T_RecPayRecord>();
            count = dh.GetParaValue<int>("@Count");
            return result;
        }

        private List<T_RecPayRecord> GetPaymentSelfList(int pageIndex, int pageSize, out int count, string C_GUID, string dateBegin, string dateEnd, string customer, string incomeGrp, string incomeGrpdts = null, string flag = null, bool? classifyFlag = null)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetReceivablesSelfListTwo";
            dh.AddPare("@PageIndex", SqlDbType.Int, 0, pageIndex);
            dh.AddPare("@PageSize", SqlDbType.Int, 0, pageSize);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            dh.AddPare("@Flag", SqlDbType.NVarChar, 1, flag);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@ClassifyFlag", SqlDbType.Bit, 0, classifyFlag);
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
            dh.AddPare("@incomeGrpdts", SqlDbType.NVarChar, 40, incomeGrpdts);
            List<T_RecPayRecord> result = new List<T_RecPayRecord>();
            result = dh.Reader<T_RecPayRecord>();
            count = dh.GetParaValue<int>("@Count");
            return result;
        }

        private List<T_RecPayRecord> GetPaymentSelfListTwo(int pageIndex, int pageSize, out int count, string C_GUID, string dateBegin, string dateEnd, string customer, string incomeGrp, string incomeGrpdts = null, string flag = null, bool? classifyFlag = null)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetReceivablesSelfListThree";
            dh.AddPare("@PageIndex", SqlDbType.Int, 0, pageIndex);
            dh.AddPare("@PageSize", SqlDbType.Int, 0, pageSize);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            dh.AddPare("@Flag", SqlDbType.NVarChar, 1, flag);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@ClassifyFlag", SqlDbType.Bit, 0, classifyFlag);
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
            dh.AddPare("@incomeGrpdts", SqlDbType.NVarChar, 40, incomeGrpdts);
            List<T_RecPayRecord> result = new List<T_RecPayRecord>();
            result = dh.Reader<T_RecPayRecord>();
            count = dh.GetParaValue<int>("@Count");
            return result;
        }

        public List<T_RecPayRecord> GetDeclareCustomer(string flag, string C_GUID, int pageIndex, int pageSize, out int count, string invtypedts)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetDeclareCustomer";
            dh.AddPare("@PageIndex", SqlDbType.Int, 0, pageIndex);
            dh.AddPare("@PageSize", SqlDbType.Int, 0, pageSize);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            dh.AddPare("@Flag", SqlDbType.NVarChar, 1, flag);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@InvTypeDts", SqlDbType.NVarChar, 50, invtypedts);
            List<T_RecPayRecord> result = new List<T_RecPayRecord>();
            result = dh.Reader<T_RecPayRecord>();
            count = dh.GetParaValue<int>("@Count");
            return result;
        }

        public List<T_RecPayRecord> GetPaymentDeclareCostSpending(string flag, string C_GUID, int pageIndex, int pageSize, out int count, string invtypedts)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetPaymentDeclareCostSpendingList";
            dh.AddPare("@PageIndex", SqlDbType.Int, 0, pageIndex);
            dh.AddPare("@PageSize", SqlDbType.Int, 0, pageSize);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            dh.AddPare("@Flag", SqlDbType.NVarChar, 1, flag);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@InvTypeDts", SqlDbType.NVarChar, 50, invtypedts);
            List<T_RecPayRecord> result = new List<T_RecPayRecord>();
            result = dh.Reader<T_RecPayRecord>();
            count = dh.GetParaValue<int>("@Count");
            return result;
        }

        /// <summary>
        /// 获取所有收付款纪录列表
        /// </summary>
        /// <param name="C_GUID">公司标识</param>
        /// <param name="flag">收付标识</param>
        /// <returns></returns>
        private List<T_RecPayRecord> GetAllRecPayRecords(string C_GUID, string flag = null)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetRecPayRecordCopy";
            dh.AddPare("@Flag", SqlDbType.NVarChar, 1, flag);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            //dh.AddPare("@IsAll", SqlDbType.Bit, 0, 1);
            return dh.Reader<T_RecPayRecord>();
        }

        /// <summary>
        /// 获取收款纪录
        /// </summary>
        /// <param name="id">收款纪录标识</param>
        /// <param name="C_GUID">公司标识</param>
        /// <returns></returns>
        public T_RecPayRecord GetReceivablesRecord(string id, string C_GUID)
        {
            return GetRecPayRecord(id, C_GUID);
        }

        /// <summary>
        /// 获取应收纪录列表
        /// </summary>
        /// <param name="id">付款方标识</param>
        /// <param name="C_GUID">公司标识</param>
        /// <param name="page">页索引</param>
        /// <param name="rows">页大小</param>
        /// <param name="count">纪录总数</param>
        /// <returns></returns>
        public List<T_Receivables> GetChooseReceivablesRecord(string id, string C_GUID, int page, int rows, out int count)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetChooseReceivablesRecord";
            dh.AddPare("@PageIndex", SqlDbType.Int, 0, page);
            dh.AddPare("@PageSize", SqlDbType.Int, 0, rows);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            dh.AddPare("@RPer", SqlDbType.NVarChar, 40, id);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 40, C_GUID);
            List<T_Receivables> result = new List<T_Receivables>();
            result = dh.Reader<T_Receivables>();
            count = dh.GetParaValue<int>("@Count");
            return result;
        }

        /// <summary>
        /// 获取应付纪录列表
        /// </summary>
        /// <param name="id">收款方标识</param>
        /// <param name="C_GUID">公司标识</param>
        /// <param name="page">页索引</param>
        /// <param name="rows">页大小</param>
        /// <param name="count">纪录总数</param>
        /// <returns></returns>
        public List<T_Payables> GetChoosePayablesRecord(string id, string C_GUID, int page, int rows, out int count,string iegroup=null)
        {
            DBHelper dh = new DBHelper();
            //dh.strCmd = "SP_GetChoosePayablesRecord";
            dh.strCmd = "SP_GetPayableRecord";
            dh.AddPare("@PageIndex", SqlDbType.Int, 0, page);
            dh.AddPare("@PageSize", SqlDbType.Int, 0, rows);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            dh.AddPare("@RPer", SqlDbType.NVarChar, 40, id);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 40, C_GUID);
            dh.AddPare("@IEGroup", SqlDbType.NVarChar, 40, iegroup);
            List<T_Payables> result = new List<T_Payables>();
            result = dh.Reader<T_Payables>();
            count = dh.GetParaValue<int>("@Count");
            return result;
        }

        /// <summary>
        /// 获取付款纪录
        /// </summary>
        /// <param name="id">付款纪录标识</param>
        /// <param name="C_GUID">公司标识</param>
        /// <returns></returns>
        public T_RecPayRecord GetPaymentRecord(string id, string C_GUID)
        {
            return GetRecPayRecord(id, C_GUID);
        }

        /// <summary>
        /// 获取收付纪录
        /// </summary>
        /// <param name="id">收付纪录标识</param>
        /// <param name="C_GUID">公司标识</param>
        /// <returns></returns>
        private T_RecPayRecord GetRecPayRecord(string id, string C_GUID)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetRecPayRecord";
            dh.AddPare("@ID", SqlDbType.NVarChar, 40, id);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            return dh.Reader<T_RecPayRecord>().FirstOrDefault();
        }
        public List<T_RecPayRecord> GetRecPayRecordD(string id, string C_GUID)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetRecPayRecord";
            dh.AddPare("@ID", SqlDbType.NVarChar, 40, id);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            return dh.Reader<T_RecPayRecord>();
        }

        /// <summary>
        /// 归类
        /// </summary>
        /// <param name="id">收付纪录标识</param>
        /// <param name="invtype">公司标识</param>
        /// <returns></returns>
        public bool UpdInvType(string flag, string id, string invtype, string typedts, string ieguid, string remark, string cfitemguid)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_UpdInvType";
            dh.AddPare("@Flag", SqlDbType.NVarChar, 1, flag);
            dh.AddPare("@ID", SqlDbType.NVarChar, 40, id);
            dh.AddPare("@Type", SqlDbType.NVarChar, 50, invtype);
            dh.AddPare("@TypeDts", SqlDbType.NVarChar, 50, typedts);
            dh.AddPare("@IE_GUID", SqlDbType.NVarChar, 40, ieguid);
            dh.AddPare("@Remark", SqlDbType.NVarChar, 100, remark);
            dh.AddPare("@CFItemGuid", SqlDbType.NVarChar, 40, cfitemguid);
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
        /// 更改状态
        /// </summary>
        /// <param name="id">收入纪录标识</param>
        /// <returns></returns>
        public bool UpdIEState(string flag, string id, string state)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_UpdIEState";
            dh.AddPare("@Flag", SqlDbType.NVarChar, 1, flag);
            dh.AddPare("@ID", SqlDbType.NVarChar, 40, id);
            dh.AddPare("@State", SqlDbType.NVarChar, 40, state);
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
        /// 
        /// </summary>
        /// <param name="flag"></param>
        /// <param name="id"></param>
        /// <param name="ieguid"></param>
        /// <param name="invtype"></param>
        /// <param name="invtypedts"></param>
        /// <returns></returns>
        public bool UpdRR(string flag, string id, string ieguid, string invtype, string invtypedts,string cfitemguid)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_UpdRR";
            dh.AddPare("@Flag", SqlDbType.NVarChar, 1, flag);
            dh.AddPare("@ID", SqlDbType.NVarChar, 40, id);
            dh.AddPare("@IE_GUID", SqlDbType.NVarChar, 40, ieguid);
            dh.AddPare("@InvType", SqlDbType.NVarChar, 100, invtype);
            dh.AddPare("@InvTypeDts", SqlDbType.NVarChar, 100, invtypedts);
            dh.AddPare("@CFItemGuid", SqlDbType.NVarChar, 40, cfitemguid);
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
        /// 删除收款纪录
        /// </summary>
        /// <param name="id">收款纪录标识</param>
        /// <returns></returns>
        public bool DelReceivablesRecord(string id)
        {
            return DelRecPayRecord(id);
        }

        /// <summary>
        /// 删除付款纪录
        /// </summary>
        /// <param name="id">付款纪录标识</param>
        /// <returns></returns>
        public bool DelPaymentRecord(string id)
        {
            return DelRecPayRecord(id);
        }

        /// <summary>
        /// 删除收付纪录
        /// </summary>
        /// <param name="id">收付纪录标识</param>
        /// <returns></returns>
        private bool DelRecPayRecord(string id)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_DelRecPayRecord";
            dh.AddPare("@ID", SqlDbType.NVarChar, 40, id);
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
    }
}