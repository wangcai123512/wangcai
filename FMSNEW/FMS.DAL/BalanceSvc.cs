using System.Collections.Generic;
using System.Data;
using FMS.Model;

namespace FMS.DAL
{
    public class BalanceSvc
    {
        public bool UpdInitialBalanceRecord(T_Balance rec)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_UpdInitialBalanceRecord";
            dh.AddPare("@Inital_GUID", SqlDbType.NVarChar, 40, rec.Inital_GUID);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 40, rec.C_GUID);
            dh.AddPare("@MonetaryFundsCurrency", SqlDbType.NVarChar, 40, rec.MonetaryFundsCurrency);
            dh.AddPare("@ShorttermInvestmentsCurrency", SqlDbType.NVarChar, 40, rec.ShorttermInvestmentsCurrency);
            dh.AddPare("@NotesReceivableCurrency", SqlDbType.NVarChar, 40, rec.NotesReceivableCurrency);
            dh.AddPare("@IntangibleAssetsCurrency", SqlDbType.NVarChar, 40, rec.IntangibleAssetsCurrency);
            dh.AddPare("@SubsidiesReceivableCurrency", SqlDbType.NVarChar, 40, rec.SubsidiesReceivableCurrency);
            dh.AddPare("@DividendReceivableCurrency", SqlDbType.NVarChar, 40, rec.DividendReceivableCurrency);
            dh.AddPare("@InventoriesCurrency", SqlDbType.NVarChar, 40, rec.InventoriesCurrency);
            dh.AddPare("@LongtermDebtOneYearCurrency", SqlDbType.NVarChar, 40, rec.LongtermDebtOneYearCurrency);
            dh.AddPare("@LongtermInvestmentsCurrency", SqlDbType.NVarChar, 40, rec.LongtermInvestmentsCurrency);
            dh.AddPare("@OtherCcurrentAssetsCurrency", SqlDbType.NVarChar, 40, rec.OtherCcurrentAssetsCurrency);
            dh.AddPare("@FixedAssetsNBVCurrency", SqlDbType.NVarChar, 40, rec.FixedAssetsNBVCurrency);
            dh.AddPare("@ConstructionInProgressCurrency", SqlDbType.NVarChar, 40, rec.ConstructionInProgressCurrency);
            dh.AddPare("@DeferredCurrency", SqlDbType.NVarChar, 40, rec.DeferredCurrency);
            dh.AddPare("@ShorttermLoansCurrency", SqlDbType.NVarChar, 40, rec.ShorttermLoansCurrency);
            dh.AddPare("@NotesPayableCurrency", SqlDbType.NVarChar, 40, rec.NotesPayableCurrency);
            dh.AddPare("@AccountsPayableCurrency", SqlDbType.NVarChar, 40, rec.AccountsPayableCurrency);
            dh.AddPare("@AdvancesFromCustomersCurrency", SqlDbType.NVarChar, 40, rec.AdvancesFromCustomersCurrency);
            dh.AddPare("@AccruedPayrollCurrency", SqlDbType.NVarChar, 40, rec.AccruedPayrollCurrency);
            dh.AddPare("@TaxesPayableCurrency", SqlDbType.NVarChar, 40, rec.TaxesPayableCurrency);
            dh.AddPare("@LongtermLiabiltiesDueWithinaYearCurrency", SqlDbType.NVarChar, 40, rec.LongtermLiabiltiesDueWithinaYearCurrency);
            dh.AddPare("@OtherCurrentLiabilitiesCurrency", SqlDbType.NVarChar, 40, rec.OtherCurrentLiabilitiesCurrency);
            dh.AddPare("@LongtermBorrowingsCurrency", SqlDbType.NVarChar, 40, rec.LongtermBorrowingsCurrency);
            dh.AddPare("@LongtermPayablesCurrency", SqlDbType.NVarChar, 40, rec.LongtermPayablesCurrency);
            dh.AddPare("@OtherLongtermLliabilitiesCurrency", SqlDbType.NVarChar, 40, rec.OtherLongtermLliabilitiesCurrency);

            //dh.AddPare("@Currency", SqlDbType.NVarChar, 40, rec.Currency);

            dh.AddPare("@MonetaryFunds", SqlDbType.Decimal, 0, rec.MonetaryFunds);
            dh.AddPare("@ShorttermInvestments", SqlDbType.Decimal, 0, rec.ShorttermInvestments);
            dh.AddPare("@NotesReceivable", SqlDbType.Decimal, 0, rec.NotesReceivable);
            dh.AddPare("@SubsidiesReceivable", SqlDbType.Decimal, 0, rec.SubsidiesReceivable);
            dh.AddPare("@DividendReceivable", SqlDbType.Decimal, 0, rec.DividendReceivable);
            dh.AddPare("@Inventories", SqlDbType.Decimal, 0, rec.Inventories);
            dh.AddPare("@LongtermDebtOneYear", SqlDbType.Decimal, 0, rec.LongtermDebtOneYear);
            dh.AddPare("@OtherCcurrentAssets", SqlDbType.Decimal, 0, rec.OtherCcurrentAssets);
            dh.AddPare("@LongtermInvestments", SqlDbType.Decimal, 0, rec.LongtermInvestments);
            dh.AddPare("@FixedAssetsNBV", SqlDbType.Decimal, 0, rec.FixedAssetsNBV);
            dh.AddPare("@ConstructionInProgress", SqlDbType.Decimal, 0, rec.ConstructionInProgress);
            dh.AddPare("@IntangibleAssets", SqlDbType.Decimal, 0, rec.IntangibleAssets);
            dh.AddPare("@Deferred", SqlDbType.Decimal, 0, rec.Deferred);
            dh.AddPare("@ShorttermLoans", SqlDbType.Decimal, 0, rec.ShorttermLoans);
            dh.AddPare("@NotesPayable", SqlDbType.Decimal, 0, rec.NotesPayable);
            dh.AddPare("@AccountsPayable", SqlDbType.Decimal, 0, rec.AccountsPayable);
            dh.AddPare("@AdvancesFromCustomers", SqlDbType.Decimal, 0, rec.AdvancesFromCustomers);
            dh.AddPare("@AccruedPayroll", SqlDbType.Decimal, 0, rec.AccruedPayroll);
            dh.AddPare("@TaxesPayable", SqlDbType.Decimal, 0, rec.TaxesPayable);
            dh.AddPare("@LongtermLiabiltiesDueWithinaYear", SqlDbType.Decimal, 0, rec.LongtermLiabiltiesDueWithinaYear);
            dh.AddPare("@OtherCurrentLiabilities", SqlDbType.Decimal, 0, rec.OtherCurrentLiabilities);
            dh.AddPare("@LongtermBorrowings", SqlDbType.Decimal, 0, rec.LongtermBorrowings);
            dh.AddPare("@LongtermPayables", SqlDbType.Decimal, 0, rec.LongtermPayables);
            dh.AddPare("@OtherLongtermLliabilities", SqlDbType.Decimal, 0, rec.OtherLongtermLliabilities);

            dh.AddPare("@Date", SqlDbType.DateTime, 0, rec.Date);
            //dh.AddPare("@BankAccount1", SqlDbType.NVarChar, 40, rec.BankAccount1);
            //dh.AddPare("@BankAccount1Money", SqlDbType.Decimal, 0, rec.BankAccount1Money);
            //dh.AddPare("@BankAccount2", SqlDbType.NVarChar, 40, rec.BankAccount2);
            //dh.AddPare("@BankAccount2Money", SqlDbType.Decimal, 0, rec.BankAccount2Money);

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

        public bool UpdateInitialBalanceRecord(T_BeginningBalance balence){
             DBHelper dh = new DBHelper();
             dh.strCmd = "SP_UpdateInitialBalanceRecord";
             dh.AddPare("@R_GUID", SqlDbType.NVarChar, 40, balence.R_GUID);
             dh.AddPare("@C_GUID", SqlDbType.NVarChar, 40, balence.C_GUID);
             dh.AddPare("@Acc_Name", SqlDbType.NVarChar, 40, balence.Acc_Name);
             dh.AddPare("@Money", SqlDbType.Decimal, 0, balence.Money);
             dh.AddPare("@InitialDate", SqlDbType.NVarChar, 40, balence.InitialDate);


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

        public bool InsInitialBalanceRecord(T_BeginningBalance balence) {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_InsInitialBalanceRecord";
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 40, balence.C_GUID);
            dh.AddPare("@Money", SqlDbType.Decimal, 0, balence.Money);
            dh.AddPare("@InitialDate", SqlDbType.NVarChar, 40, balence.InitialDate);


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


        public List<T_BeginningBalance> GetInitialBalanceRecord(string C_GUID,string Acc_name)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetInitialBalanceRecord";
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@Acc_name", SqlDbType.NVarChar, 50, Acc_name);
            return dh.Reader<T_BeginningBalance>();
        }

        public List<HisTr_GeneralLedgerAccount> GetGenAccountList(string C_GUID, int pageIndex, int pageSize, out int count, string dateBegin, string dateEnd, string Name)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetHisTrGeneralLedgerAccount";
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@PageIndex", SqlDbType.Int, 0, pageIndex);
            dh.AddPare("@PageSize", SqlDbType.Int, 0, pageSize);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            if (!string.IsNullOrEmpty(dateBegin))
            {
                dh.AddPare("@dateBegin", SqlDbType.DateTime, 0, dateBegin);
            }
            if (!string.IsNullOrEmpty(dateEnd))
            {
                dh.AddPare("@dateEnd", SqlDbType.DateTime, 0, dateEnd);
            }
            dh.AddPare("@Name", SqlDbType.NVarChar, 40, Name);
            List<HisTr_GeneralLedgerAccount> result = new List<HisTr_GeneralLedgerAccount>();
            result = dh.Reader<HisTr_GeneralLedgerAccount>();
            count = dh.GetParaValue<int>("@Count");
            return result;
        }
       /*科目明细*/
        public List<HisTr_GeneralLedgerAccount> GetLDetailAccount(string LA_GUID, string C_GUID)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetLDetailAccount";
            dh.AddPare("@LA_GUID", SqlDbType.NVarChar, 40, LA_GUID);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 40, C_GUID);
            return dh.Reader<HisTr_GeneralLedgerAccount>();

        }

        public List<HisTr_DetailedAccount> GetDAccountAmount(string id,string C_GUID)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetDAccountAmount";
            dh.AddPare("@LA_GUID", SqlDbType.NVarChar, 40, id);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 40, C_GUID);
            return dh.Reader<HisTr_DetailedAccount>();
        }

        public List<HisTr_ThirdAccount> GetThAccountAmount(string id,string C_GUID) {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetThAccountAmount";
            dh.AddPare("@LA_GUID", SqlDbType.NVarChar, 40, id);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 40, C_GUID);
            return dh.Reader<HisTr_ThirdAccount>();
        }

    }
}
