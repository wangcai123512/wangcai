//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace FMS.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class T_RecPayHistoryRecord
    {
        public string RP_GUID { get; set; }
        public string RP_Flag { get; set; }
        public string C_GUID { get; set; }
        public string InvType { get; set; }
        public string InvNo { get; set; }
        public string R_Per { get; set; }
        public string DebitLedgerAccount { get; set; }
        public string DebitDetailsAccount { get; set; }
        public string CreditLedgerAccount { get; set; }
        public string CreditDetailsAccount { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string Remark { get; set; }
        public string Creator { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public string Currency { get; set; }
        public string CFItem { get; set; }
        public string CFPItem { get; set; }
        public string Bank { get; set; }
        public string BankAccount { get; set; }
    }
}
