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
    
    public partial class T_AIDRecord
    {
        public string GUID { get; set; }
        public string C_GUID { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public string Currency { get; set; }
        public string RPer { get; set; }
        public string InvType { get; set; }
        public string Description { get; set; }
        public Nullable<int> DepreciationPeriod { get; set; }
        public Nullable<decimal> SurplusValue { get; set; }
        public string State { get; set; }
        public string Remark { get; set; }
        public string CostType { get; set; }
        public string GUID_Parent { get; set; }
        public string SubType { get; set; }
        public Nullable<int> Number { get; set; }
        public Nullable<int> Inventory_Number { get; set; }
        public string Business_GUID { get; set; }
        public string SubBusiness_GUID { get; set; }
        public string MaterielManage_GUID { get; set; }
    }
}