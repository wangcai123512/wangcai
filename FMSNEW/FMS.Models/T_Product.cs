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
    
    public partial class T_Product
    {
        public string GUID { get; set; }
        public string C_GUID { get; set; }
        public string Create_Date { get; set; }
        public string Currency { get; set; }
        public Nullable<int> stock_count { get; set; }
        public Nullable<int> used_count { get; set; }
        public Nullable<int> saled_count { get; set; }
        public string TypeId { get; set; }
        public string SubTypeId { get; set; }
        public string MaterielManage_GUID { get; set; }
        public string Business_GUID { get; set; }
        public string SubBusiness_GUID { get; set; }
    }
}