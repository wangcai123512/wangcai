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
    
    public partial class V_IEDetails
    {
        public string IE_GUID { get; set; }
        public Nullable<decimal> Money { get; set; }
        public string C_GUID { get; set; }
        public string DebitLedgerAccount { get; set; }
        public string DebitDetailsAccount { get; set; }
        public string CreditLedgerAccount { get; set; }
        public string CreditDetailsAccount { get; set; }
        public string Currency { get; set; }
    }
}