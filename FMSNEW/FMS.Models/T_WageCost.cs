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
    
    public partial class T_WageCost
    {
        public string W_GUID { get; set; }
        public string C_GUID { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string Employee { get; set; }
        public Nullable<decimal> Cash { get; set; }
        public Nullable<decimal> PersonalTaxes { get; set; }
        public Nullable<decimal> SocialSecurity { get; set; }
        public Nullable<decimal> Total { get; set; }
        public string Currency { get; set; }
        public string State { get; set; }
    }
}