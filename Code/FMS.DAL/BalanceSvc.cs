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
            dh.AddPare("@Currency", SqlDbType.NVarChar, 40, rec.Currency);

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
            dh.AddPare("@BankAccount1", SqlDbType.NVarChar, 40, rec.BankAccount1);
            dh.AddPare("@BankAccount1Money", SqlDbType.Decimal, 0, rec.BankAccount1Money);
            dh.AddPare("@BankAccount2", SqlDbType.NVarChar, 40, rec.BankAccount2);
            dh.AddPare("@BankAccount2Money", SqlDbType.Decimal, 0, rec.BankAccount2Money);

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

        public List<T_Balance> GetInitialBalanceRecord(string C_GUID)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetInitialBalanceRecord";
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            return dh.Reader<T_Balance>();
        }
    }
}
