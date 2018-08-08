using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FMS.Model
{
    /// <summary>
    /// 银行对象
    /// </summary>
    public class T_Balance
    {
        /// <summary>
        /// 期初余额对象标识
        /// </summary>
        public string Inital_GUID { get; set; }

        /// <summary>
        /// 公司标识
        /// </summary>
        public string C_GUID { get; set; }

        /// <summary>
        /// 备用现金余额
        /// </summary>
        public decimal MonetaryFunds { get; set; }

        /// <summary>
        /// 短期投资余额
        /// </summary>
        public decimal ShorttermInvestments { get; set; }

        /// <summary>
        /// 应收票据余额
        /// </summary>
        public decimal NotesReceivable { get; set; }

        /// <summary>
        /// 应收补贴余额
        /// </summary>
        public decimal SubsidiesReceivable { get; set; }

        /// <summary>
        /// 应收股利余额
        /// </summary>
        public decimal DividendReceivable { get; set; }

        /// <summary>
        /// 存货余额
        /// </summary>
        public decimal Inventories { get; set; }

        /// <summary>
        /// 一年内到期的长期债券投资余额
        /// </summary>
        public decimal LongtermDebtOneYear { get; set; }

        /// <summary>
        /// 其他流动资产余额
        /// </summary>
        public decimal OtherCcurrentAssets { get; set; }

        /// <summary>
        /// 长期投资余额
        /// </summary>
        public decimal LongtermInvestments { get; set; }

        /// <summary>
        /// 固定资产余额
        /// </summary>
        public decimal FixedAssetsNBV { get; set; }

        /// <summary>
        /// 在建工程余额
        /// </summary>
        public decimal ConstructionInProgress { get; set; }

        /// <summary>
        /// 无形资产余额
        /// </summary>
        public decimal IntangibleAssets { get; set; }

        /// <summary>
        /// 延递资产余额
        /// </summary>
        public decimal Deferred { get; set; }



        /// <summary>
        /// 短期借款余额
        /// </summary>
        public decimal ShorttermLoans { get; set; }

        /// <summary>
        /// 应付票据余额
        /// </summary>
        public decimal NotesPayable { get; set; }

        /// <summary>
        /// 应付账款余额
        /// </summary>
        public decimal AccountsPayable { get; set; }

        /// <summary>
        /// 预收账款余额
        /// </summary>
        public decimal AdvancesFromCustomers { get; set; }

        /// <summary>
        /// 应付工资余额
        /// </summary>
        public decimal AccruedPayroll { get; set; }

        /// <summary>
        /// 应交税金
        /// </summary>
        public decimal TaxesPayable { get; set; }

        /// <summary>
        /// 一年内到期的长期负债余额
        /// </summary>
        public decimal LongtermLiabiltiesDueWithinaYear { get; set; }

        /// <summary>
        /// 其他流动负债余额
        /// </summary>
        public decimal OtherCurrentLiabilities { get; set; }

        /// <summary>
        /// 长期借款余额
        /// </summary>
        public decimal LongtermBorrowings { get; set; }

        /// <summary>
        /// 长期应付款余额
        /// </summary>
        public decimal LongtermPayables { get; set; }

        /// <summary>
        /// 其他长期负债余额
        /// </summary>
        public decimal OtherLongtermLliabilities { get; set; }


        public decimal Amount { get; set; }

        /// <summary>
        /// 货币
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Flag { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        public DateTime Date { get; set; }

        public string MonetaryFundsCurrency { get; set; }
        public string ShorttermInvestmentsCurrency { get; set; }
        public string NotesReceivableCurrency { get; set; }
        public string IntangibleAssetsCurrency { get; set; }
        public string SubsidiesReceivableCurrency { get; set; }
        public string DividendReceivableCurrency { get; set; }
        public string InventoriesCurrency { get; set; }
        public string LongtermDebtOneYearCurrency { get; set; }
        public string LongtermInvestmentsCurrency { get; set; }
        public string OtherCcurrentAssetsCurrency { get; set; }
        public string FixedAssetsNBVCurrency { get; set; }
        public string ConstructionInProgressCurrency { get; set; }
        public string DeferredCurrency { get; set; }
        public string ShorttermLoansCurrency { get; set; }
        public string NotesPayableCurrency { get; set; }
        public string AccountsPayableCurrency { get; set; }
        public string AdvancesFromCustomersCurrency { get; set; }
        public string AccruedPayrollCurrency { get; set; }
        public string TaxesPayableCurrency { get; set; }
        public string LongtermLiabiltiesDueWithinaYearCurrency { get; set; }
        public string OtherCurrentLiabilitiesCurrency { get; set; }
        public string LongtermBorrowingsCurrency { get; set; }
        public string LongtermPayablesCurrency { get; set; }
        public string OtherLongtermLliabilitiesCurrency { get; set; }


       
        public string BankAccount1 { get; set; }
        public decimal BankAccount1Money { get; set; }
        public string BankAccount2 { get; set; }
        public decimal BankAccount2Money { get; set; }
    }
}
