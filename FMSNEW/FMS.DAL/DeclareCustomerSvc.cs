using System;
using System.Data;
using FMS.Model;
using System.Collections.Generic;

namespace FMS.DAL
{
    public class DeclareCustomerSvc
    {
        public bool UpdReceivablesDeclareCustomer(T_DeclareCustomer rec)
        {
            DBHelper dh = new DBHelper();
            dh.BeginTran();
            try
            {
                dh.strCmd = "SP_UpdReceivablesDeclareCustomer";
                dh.AddPare("@GUID", SqlDbType.NVarChar, 40, rec.GUID);
                dh.AddPare("@VoucherNo", SqlDbType.NVarChar, 40, rec.VoucherNo);
                dh.AddPare("@Business_GUID", SqlDbType.NVarChar, 40, rec.Business_GUID);
                dh.AddPare("@SubBusiness_GUID", SqlDbType.NVarChar, 40, rec.SubBusiness_GUID);
                dh.AddPare("@RPType", SqlDbType.NVarChar, 50, rec.RPType);
                dh.AddPare("@DetailRPType", SqlDbType.NVarChar, 50, rec.DetailRPType);
                dh.AddPare("@InvType", SqlDbType.NVarChar, 40, rec.InvType);
                dh.AddPare("@RPer", SqlDbType.NVarChar, 40, rec.RPer);
                dh.AddPare("@Date", SqlDbType.DateTime, 0, rec.Date);
                dh.AddPare("@Amount", SqlDbType.Decimal, 0, rec.Amount);
                dh.AddPare("@Remark", SqlDbType.NVarChar, 200, rec.Remark);
                dh.AddPare("@Currency", SqlDbType.NVarChar, 20, rec.Currency);
                dh.AddPare("@Profit_Name", SqlDbType.NVarChar, 40, rec.Profit_Name);
                dh.AddPare("@DisAmount", SqlDbType.Decimal, 0, rec.Amount);
                dh.AddPare("@State", SqlDbType.NVarChar, 40, rec.State);
                if (rec.InvType == "短期借款" || rec.InvType == "长期借款")
                {
                    dh.AddPare("@Record", SqlDbType.NVarChar, 40, "未还款");
                }
                else
                {
                    dh.AddPare("@Record", SqlDbType.NVarChar, 40, "未记录");
                }
                
                dh.AddPare("@C_GUID", SqlDbType.NVarChar, 40, rec.C_GUID);
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

        public bool UpdPayDCFL(T_DeclareCustomer rec, string Method) {
            DBHelper dh = new DBHelper();
            dh.BeginTran();
            try
            {
                dh.strCmd = "SP_UpdReceivablesDeclareCustomer";
                dh.AddPare("@GUID", SqlDbType.NVarChar, 40, rec.GUID);
                dh.AddPare("@AccountID", SqlDbType.NVarChar, 40, rec.AccountID);
                dh.AddPare("@Method", SqlDbType.NVarChar, 40, Method);
                dh.AddPare("@VoucherNo", SqlDbType.NVarChar, 40, rec.VoucherNo);
                dh.AddPare("@Business_GUID", SqlDbType.NVarChar, 40, rec.Business_GUID);
                dh.AddPare("@SubBusiness_GUID", SqlDbType.NVarChar, 40, rec.SubBusiness_GUID);
                dh.AddPare("@RPType", SqlDbType.NVarChar, 50, rec.RPType);
                dh.AddPare("@DetailRPType", SqlDbType.NVarChar, 50, rec.DetailRPType);
                dh.AddPare("@InvType", SqlDbType.NVarChar, 40, rec.InvType);
                dh.AddPare("@RPer", SqlDbType.NVarChar, 40, rec.RPer);
                dh.AddPare("@Date", SqlDbType.DateTime, 0, rec.Date);
                dh.AddPare("@Amount", SqlDbType.Decimal, 0, rec.Amount);
                dh.AddPare("@DisAmount", SqlDbType.Decimal, 0, rec.DisAmount);
                dh.AddPare("@Remark", SqlDbType.NVarChar, 200, rec.Remark);
                dh.AddPare("@Currency", SqlDbType.NVarChar, 20, rec.Currency);
                dh.AddPare("@Profit_Name", SqlDbType.NVarChar, 40, rec.Profit_Name);
                dh.AddPare("@State", SqlDbType.NVarChar, 40, rec.State);
                dh.AddPare("@Record", SqlDbType.NVarChar, 40, "未记录");
                dh.AddPare("@C_GUID", SqlDbType.NVarChar, 40, rec.C_GUID);
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

        public bool UpdV(T_IERecord rec) {
            DBHelper dh = new DBHelper();
            dh.BeginTran();
            try
            {
                dh.strCmd = "SP_UpdReceivablesDeclareCustomer";
                dh.AddPare("@GUID", SqlDbType.NVarChar, 40, rec.IE_GUID);
                dh.AddPare("@VoucherNo", SqlDbType.NVarChar, 40, rec.InvNo);
                dh.AddPare("@Business_GUID", SqlDbType.NVarChar, 40, rec.Business_GUID);
                dh.AddPare("@SubBusiness_GUID", SqlDbType.NVarChar, 40, rec.SubBusiness_GUID);
                dh.AddPare("@InvType", SqlDbType.NVarChar, 40, rec.InvType);
                dh.AddPare("@RPer", SqlDbType.NVarChar, 40, rec.RPer);
                dh.AddPare("@Date", SqlDbType.DateTime, 0, rec.Date);
                dh.AddPare("@Amount", SqlDbType.Decimal, 0, rec.SumAmount);
                dh.AddPare("@Remark", SqlDbType.NVarChar, 200, rec.Remark);
                dh.AddPare("@Currency", SqlDbType.NVarChar, 20, rec.Currency);
                dh.AddPare("@Profit_Name", SqlDbType.NVarChar, 40, rec.Profit_Name);
                dh.AddPare("@State", SqlDbType.NVarChar, 40, rec.State);
                if (rec.InvType == "短期借款" || rec.InvType == "长期借款")
                {
                    dh.AddPare("@Record", SqlDbType.NVarChar, 40, "未还款");
                }
                else
                {
                    dh.AddPare("@Record", SqlDbType.NVarChar, 40, "未记录");
                }

                dh.AddPare("@C_GUID", SqlDbType.NVarChar, 40, rec.C_GUID);
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

        public bool UpdDCostSpending(T_DeclareCustomer rec)
        {
            DBHelper dh = new DBHelper();
            dh.BeginTran();
            try
            {
                dh.strCmd = "SP_UpdPaymentDCostSpending";
                dh.AddPare("@GUID", SqlDbType.NVarChar, 40, rec.GUID);
                dh.AddPare("@InvType", SqlDbType.NVarChar, 40, rec.InvType);
                dh.AddPare("@RPer", SqlDbType.NVarChar, 40, rec.RPer);
                if (!rec.Date.Equals(DateTime.MinValue))
                {
                    dh.AddPare("@Date", SqlDbType.DateTime, 0, rec.Date);
                }
                dh.AddPare("@Amount", SqlDbType.Decimal, 0, rec.Amount);
                dh.AddPare("@Remark", SqlDbType.NVarChar, 200, rec.Record);
                dh.AddPare("@Currency", SqlDbType.NVarChar, 20, rec.Currency);
                dh.AddPare("@State", SqlDbType.NVarChar, 40, rec.State);
                dh.AddPare("@Record", SqlDbType.NVarChar, 40, rec.Record);
                dh.AddPare("@C_GUID", SqlDbType.NVarChar, 40, rec.C_GUID);
                dh.AddPare("@VoucherNo", SqlDbType.NVarChar, 40, rec.VoucherNo);
                dh.AddPare("@Profit_GUID", SqlDbType.NVarChar, 40, rec.Profit_GUID);
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

        public List<T_DeclareCustomer> GetReceivablesDeclareCustomerList(string C_GUID, int pageIndex, int pageSize, out int count, string dateBegin, string dateEnd, string customer, string state, string incomeGrp, string currency, string business_GUID, string subBusiness_GUID)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetReceivablesDeclareCustomer";
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
            dh.AddPare("@currency", SqlDbType.NVarChar, 40, currency);
            dh.AddPare("@incomeGrp", SqlDbType.NVarChar, 20, incomeGrp);
            dh.AddPare("@state", SqlDbType.NVarChar, 40, state);
            dh.AddPare("@Business_GUID", SqlDbType.NVarChar, 40, business_GUID);
            dh.AddPare("@SubBusiness_GUID", SqlDbType.NVarChar, 40, subBusiness_GUID);
            List<T_DeclareCustomer> result = new List<T_DeclareCustomer>();
            result = dh.Reader<T_DeclareCustomer>();
            count = dh.GetParaValue<int>("@Count");
            return result;
        }

        public List<T_DeclareCustomer> GetReceivablesDeclareCustomerListTop(string C_GUID, out int count)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetReceivablesDeclareCustomerTop";
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            List<T_DeclareCustomer> result = new List<T_DeclareCustomer>();
            result = dh.Reader<T_DeclareCustomer>();
            count = dh.GetParaValue<int>("@Count");
            return result;
        }
        public List<T_Receivables> GetChooseReceivablesRecord(string id, string C_GUID)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetChooseReceivablesRecord";
            dh.AddPare("@RPer", SqlDbType.NVarChar, 40, id);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 40, C_GUID);
            List<T_Receivables> result = new List<T_Receivables>();
            result = dh.Reader<T_Receivables>();

            return result;
        }
        public List<T_DeclareCustomer> GetReceivablesDeclareCustomerList1(string C_GUID)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetReceivablesDeclareCustomer1";
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            List<T_DeclareCustomer> result = new List<T_DeclareCustomer>();
            result = dh.Reader<T_DeclareCustomer>();
            return result;
        }
        public List<T_DeclareCustomer> GetReceivablesDeclareCustomerList(string C_GUID, string invtype = null, string state = null)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetReceivablesDeclareCustomer";
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            dh.AddPare("@InvType", SqlDbType.NVarChar, 50, invtype);
            dh.AddPare("@State", SqlDbType.NVarChar, 50, state);
            List<T_DeclareCustomer> result = new List<T_DeclareCustomer>();
            result = dh.Reader<T_DeclareCustomer>();
            return result;
        }

        public List<T_DeclareCustomer> ChooseReceivablesDeclareCustomerList(string C_GUID, int pageIndex, int pageSize, out int count, string state, string invtype, string flag, string customer,string record)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_ChooseReceivablesDeclareCustomer";
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@PageIndex", SqlDbType.Int, 0, pageIndex);
            dh.AddPare("@PageSize", SqlDbType.Int, 0, pageSize);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            dh.AddPare("@State", SqlDbType.NVarChar, 40, state);
            dh.AddPare("@InvType", SqlDbType.NVarChar, 40, invtype);
            dh.AddPare("@RP_Flag", SqlDbType.NVarChar, 40, flag);
            dh.AddPare("@customer", SqlDbType.NVarChar, 40, customer);
            dh.AddPare("@record", SqlDbType.NVarChar, 40, record);
            List<T_DeclareCustomer> result = new List<T_DeclareCustomer>();
            result = dh.Reader<T_DeclareCustomer>();
            count = dh.GetParaValue<int>("@Count");
            return result;
        }

        /// <summary>
        /// 更改状态
        /// </summary>
        /// <param name="id">纪录标识</param>
        /// <returns></returns>
        public bool UpdState(string id, string state)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_UpdDeclareCustomerState";
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
        public List<T_DeclareCustomer> GetDCVoucher(int pageIndex, int pageSize, out int count, string id)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetDCVoucher";
            dh.AddPare("@GUID", SqlDbType.NVarChar, 50, id);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            List<T_DeclareCustomer> result = new List<T_DeclareCustomer>();
            result = dh.Reader<T_DeclareCustomer>();
            count = dh.GetParaValue<int>("@Count");
            return result;

        }
        /// <summary>
        /// 删除收入外收款记录
        /// </summary>
        /// <param name="id">纪录标识</param>
        /// <returns></returns>
        /// <summary>
        public bool DelReceivablesDeclareCustomer(string id)
        {
            DBHelper dh = new DBHelper();

            dh.strCmd = "SP_DelReceivablesDeclareCustomer";
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
        /// <summary>
        /// 新增操作
        /// </summary>
        /// <param name="rec"></param>
        /// <returns></returns>

        public bool UpdaDeclareCustomer(T_DeclareCustomer rec)
        {
            DBHelper dh = new DBHelper();
            dh.BeginTran();
            try
            {
                dh.strCmd = "SP_UpdaDeclareCustomer";
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




        //收入外收款
        public List<T_DeclareCustomer> GetRevenueCollectionRecordList(string C_GUID, int page, int rows, out int count, string BA_GUID)
        {
            return GetRevenueCollectionRecordListCount(C_GUID, page, rows, out count, BA_GUID);
        }
        private List<T_DeclareCustomer> GetRevenueCollectionRecordListCount(string C_GUID, int pageIndex, int pageSize, out int count, string BA_GUID)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetRevenueCollectionRecordList";
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@PageSize", SqlDbType.Int, 0, pageSize);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            dh.AddPare("@BA_GUID", SqlDbType.NVarChar, 40, BA_GUID);
            List<T_DeclareCustomer> result = new List<T_DeclareCustomer>();
            result = dh.Reader<T_DeclareCustomer>();
            count = dh.GetParaValue<int>("@Count");
            return result;
        }
    }
}
