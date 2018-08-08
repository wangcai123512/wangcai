using System;
using System.Data;
using FMS.Model;
using System.Collections.Generic;
using System.Linq;

namespace FMS.DAL
{
    public class DeclareCostSpendingSvc
    {
        /// <summary>
        /// 新增操作
        /// </summary>
        /// <param name="rec"></param>
        /// <returns></returns>
        
        public bool UpdaDeclareCostSpending(T_DeclareCostSpending rec){
            DBHelper dh = new DBHelper();
            dh.BeginTran();
            try
            {
                dh.strCmd = "SP_UpdDeclareCostSpending";
                dh.AddPare("@GUID", SqlDbType.NVarChar, 40, rec.GUID);
                dh.AddPare("@InvType", SqlDbType.NVarChar, 40, rec.InvType);
                dh.AddPare("@RPer", SqlDbType.NVarChar, 40, rec.RPer);
                if (!rec.Date.Equals(DateTime.MinValue))
                {
                    dh.AddPare("@Date", SqlDbType.DateTime, 0, rec.Date);
                }
                dh.AddPare("@Amount", SqlDbType.Decimal, 0, rec.Amount);
                dh.AddPare("@Currency", SqlDbType.NVarChar, 20, rec.Currency);
                dh.AddPare("@State", SqlDbType.NVarChar, 40, rec.State);
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
        public bool UpdV(T_IERecord rec)
        {
            DBHelper dh = new DBHelper();
            dh.BeginTran();
            try
            {
                dh.strCmd = "SP_UpdPaymentDeclareCostSpending";
                dh.AddPare("@GUID", SqlDbType.NVarChar, 40, rec.IE_GUID);
                dh.AddPare("@InvType", SqlDbType.NVarChar, 40, rec.InvType);
                dh.AddPare("@RPer", SqlDbType.NVarChar, 40, rec.RPer);
                //if (!rec.Date.Equals(DateTime.MinValue))
                //{
                //    dh.AddPare("@Date", SqlDbType.DateTime, 0, rec.Date);
                //}
                dh.AddPare("@Date", SqlDbType.DateTime, 0, rec.Date);
                dh.AddPare("@Business_GUID", SqlDbType.NVarChar, 40, rec.Business_GUID);
                dh.AddPare("@SubBusiness_GUID", SqlDbType.NVarChar, 40, rec.SubBusiness_GUID);
                dh.AddPare("@Amount", SqlDbType.Decimal, 0, rec.SumAmount);
                dh.AddPare("@Remark", SqlDbType.NVarChar, 200, rec.Remark);
                dh.AddPare("@Currency", SqlDbType.NVarChar, 20, rec.Currency);
                dh.AddPare("@State", SqlDbType.NVarChar, 40, rec.State);
                dh.AddPare("@Record", SqlDbType.NVarChar, 40, rec.Record);
                dh.AddPare("@C_GUID", SqlDbType.NVarChar, 40, rec.C_GUID);
                dh.AddPare("@voucherNo", SqlDbType.NVarChar, 40, rec.InvNo);
                dh.AddPare("@Profit_Name", SqlDbType.NVarChar, 40, rec.Profit_Name);
                dh.AddPare("@Summary", SqlDbType.NVarChar, 40, rec.Summary);
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

        public bool UpdPayDSFL(T_DeclareCostSpending rec,string Method) { 
            DBHelper dh = new DBHelper();
            dh.BeginTran();
            try
            {
                dh.strCmd = "SP_UpdPaymentDeclareCostSpending";
                dh.AddPare("@GUID", SqlDbType.NVarChar, 40, rec.GUID);
                dh.AddPare("@InvType", SqlDbType.NVarChar, 40, rec.InvType);
                dh.AddPare("@DtsInvType", SqlDbType.NVarChar, 40, rec.DtsInvType);
                dh.AddPare("@RPer", SqlDbType.NVarChar, 40, rec.RPer);
                //if (!rec.Date.Equals(DateTime.MinValue))
                //{
                //    dh.AddPare("@Date", SqlDbType.DateTime, 0, rec.Date);
                //}
                dh.AddPare("@Method", SqlDbType.NVarChar, 50, Method);
                dh.AddPare("@AccountID", SqlDbType.NVarChar, 50, rec.AccountID);
                dh.AddPare("@RPType", SqlDbType.NVarChar, 50, rec.RPType);
                dh.AddPare("@DetailRPType", SqlDbType.NVarChar, 50, rec.DetailRPType);
                dh.AddPare("@Date", SqlDbType.DateTime, 0, rec.Date);
                dh.AddPare("@Business_GUID", SqlDbType.NVarChar, 40, rec.Business_GUID);
                dh.AddPare("@SubBusiness_GUID", SqlDbType.NVarChar, 40, rec.SubBusiness_GUID);
                dh.AddPare("@Amount", SqlDbType.Decimal, 0, rec.Amount);
                dh.AddPare("@Remark", SqlDbType.NVarChar, 200, rec.Remark);
                dh.AddPare("@Currency", SqlDbType.NVarChar, 20, rec.Currency);
                dh.AddPare("@DisAmount", SqlDbType.Decimal, 0, rec.DisAmount);
                dh.AddPare("@State", SqlDbType.NVarChar, 40, rec.State);
                dh.AddPare("@Record", SqlDbType.NVarChar, 40, rec.Record);
                dh.AddPare("@C_GUID", SqlDbType.NVarChar, 40, rec.C_GUID);
                dh.AddPare("@voucherNo",SqlDbType.NVarChar,40,rec.VoucherNo);
                dh.AddPare("@Profit_Name", SqlDbType.NVarChar, 40, rec.Profit_Name);
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

        public bool UpdPaymentDeclareCostSpending(T_DeclareCostSpending rec)
        {
            DBHelper dh = new DBHelper();
            dh.BeginTran();
            try
            {
                dh.strCmd = "SP_UpdPaymentDeclareCostSpending";
                dh.AddPare("@GUID", SqlDbType.NVarChar, 40, rec.GUID);
                dh.AddPare("@InvType", SqlDbType.NVarChar, 40, rec.InvType);
                dh.AddPare("@DtsInvType", SqlDbType.NVarChar, 40, rec.DtsInvType);
                dh.AddPare("@RPer", SqlDbType.NVarChar, 40, rec.RPer);
                //if (!rec.Date.Equals(DateTime.MinValue))
                //{
                //    dh.AddPare("@Date", SqlDbType.DateTime, 0, rec.Date);
                //}
                dh.AddPare("@RPType", SqlDbType.NVarChar, 50, rec.RPType);
                dh.AddPare("@DetailRPType", SqlDbType.NVarChar, 50, rec.DetailRPType);
                dh.AddPare("@Date", SqlDbType.DateTime, 0, rec.Date);
                dh.AddPare("@Business_GUID", SqlDbType.NVarChar, 40, rec.Business_GUID);
                dh.AddPare("@SubBusiness_GUID", SqlDbType.NVarChar, 40, rec.SubBusiness_GUID);
                dh.AddPare("@Amount", SqlDbType.Decimal, 0, rec.Amount);
                dh.AddPare("@DisAmount", SqlDbType.Decimal, 0, rec.Amount);
                dh.AddPare("@Remark", SqlDbType.NVarChar, 200, rec.Remark);
                dh.AddPare("@Currency", SqlDbType.NVarChar, 20, rec.Currency);
                dh.AddPare("@State", SqlDbType.NVarChar, 40, rec.State);
                dh.AddPare("@Record", SqlDbType.NVarChar, 40, rec.Record);
                dh.AddPare("@C_GUID", SqlDbType.NVarChar, 40, rec.C_GUID);
                dh.AddPare("@voucherNo",SqlDbType.NVarChar,40,rec.VoucherNo);
                dh.AddPare("@Profit_Name", SqlDbType.NVarChar, 40, rec.Profit_Name);
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

        public List<T_DeclareCostSpending> GetDSVoucher(int pageIndex, int pageSize, out int count, string id)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetDSVoucher";
            dh.AddPare("@GUID", SqlDbType.NVarChar, 50, id);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            List<T_DeclareCostSpending> result = new List<T_DeclareCostSpending>();
            result = dh.Reader<T_DeclareCostSpending>();
            count = dh.GetParaValue<int>("@Count");
            return result;

        }
        public T_DeclareCostSpending GetPaymentDeclareCostSpending(string C_GUID,string id) { 
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetPaymentDeclareCost";
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@ID", SqlDbType.NVarChar, 40, id);
            return dh.Reader<T_DeclareCostSpending>().FirstOrDefault();
        }

        public List<T_DeclareCostSpending> GetPaymentDeclareCostSpendingList(string C_GUID, int pageIndex, int pageSize, out int count, string dateBegin, string dateEnd, string customer, string incomeGrp, string currency, string state, string invtype, string record, string business_GUID, string subBusiness_GUID,string remark)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetPaymentDeclareCostSpending";
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
            dh.AddPare("@incomeGrp", SqlDbType.NVarChar, 20, incomeGrp);
            dh.AddPare("@State", SqlDbType.NVarChar, 50, state);
            dh.AddPare("@currency", SqlDbType.NVarChar, 40, currency);
            dh.AddPare("@InvType", SqlDbType.NVarChar, 50, invtype);
            dh.AddPare("@Record", SqlDbType.NVarChar, 50, record);
            dh.AddPare("@remark", SqlDbType.NVarChar, 50, remark);
            dh.AddPare("@Business_GUID", SqlDbType.NVarChar, 40, business_GUID);
            dh.AddPare("@SubBusiness_GUID", SqlDbType.NVarChar, 40, subBusiness_GUID);
            List<T_DeclareCostSpending> result = new List<T_DeclareCostSpending>();
            result = dh.Reader<T_DeclareCostSpending>();
            count = dh.GetParaValue<int>("@Count");
            return result;
        }

        public List<T_DeclareCostSpending> GetList(int pageIndex, int pageSize, string C_GUID, string dateBegin, string dateEnd, string paymentGrp, string state ,out int count)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetPaymentDeclareCostSpendingAll";
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@PageIndex", SqlDbType.Int, 0, pageIndex);
            dh.AddPare("@PageSize", SqlDbType.Int, 0, pageSize);
            if (!string.IsNullOrEmpty(dateBegin))
            {
                dh.AddPare("@dateBegin", SqlDbType.DateTime, 0, DateTime.Parse(dateBegin));
            }
            if (!string.IsNullOrEmpty(dateEnd))
            {
                dh.AddPare("@dateEnd", SqlDbType.DateTime, 0, DateTime.Parse(dateEnd));
            }
            dh.AddPare("@PaymentGrp", SqlDbType.NVarChar, 50, paymentGrp);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            dh.AddPare("@State", SqlDbType.NVarChar, 50, state);
            List<T_DeclareCostSpending> result = new List<T_DeclareCostSpending>();
            result = dh.Reader<T_DeclareCostSpending>();
            count = dh.GetParaValue<int>("@Count");
            return result;
        }

        public List<T_DeclareCostSpending> ChoosePaymentDeclare(string C_GUID, int pageIndex, int pageSize, out int count, string invtype)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_ChoosePaymentDeclare";
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@PageIndex", SqlDbType.Int, 0, pageIndex);
            dh.AddPare("@PageSize", SqlDbType.Int, 0, pageSize);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            dh.AddPare("@InvType", SqlDbType.NVarChar, 40, invtype);
            List<T_DeclareCostSpending> result = new List<T_DeclareCostSpending>();
            result = dh.Reader<T_DeclareCostSpending>();
            count = dh.GetParaValue<int>("@Count");
            return result;
        }
        /// <summary>
        /// 删除成本外支出记录
        /// </summary>
        /// <param name="id">纪录标识</param>
        /// <returns></returns>
        /// <summary>
        public bool DelPaymentDCostSpending(string id) {
            DBHelper dh = new DBHelper();

            dh.strCmd = "SP_DelPaymentDCostSpending";
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
        /// 更改状态
        /// </summary>
        /// <param name="id">纪录标识</param>
        /// <returns></returns>
        public bool UpdState(string id, string state)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_UpdDeclareCostSpendingState";
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


        //成本外支出
        public List<T_DeclareCostSpending> GetDeclareCostSpendingRecordList(string C_GUID, int page, int rows, out int count, string BA_GUID)
        {
            return GetDeclareCostSpendingRecordListCount(C_GUID, page, rows, out count, BA_GUID);
        }
        private List<T_DeclareCostSpending> GetDeclareCostSpendingRecordListCount(string C_GUID, int pageIndex, int pageSize, out int count, string BA_GUID)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetDeclareCostSpendingRecordCashList";
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@PageSize", SqlDbType.Int, 0, pageSize);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            dh.AddPare("@BA_GUID", SqlDbType.NVarChar, 40, BA_GUID);
            List<T_DeclareCostSpending> result = new List<T_DeclareCostSpending>();
            result = dh.Reader<T_DeclareCostSpending>();
            count = dh.GetParaValue<int>("@Count");
            return result;
        }
    }
}
