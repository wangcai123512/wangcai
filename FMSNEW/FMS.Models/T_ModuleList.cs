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
    
    public partial class T_ModuleList
    {
        public string Guid { get; set; }
        public string ChineseName { get; set; }
        public string EnglishName { get; set; }
        public Nullable<int> OrderNumber { get; set; }
        public string ModuleID { get; set; }
        public Nullable<bool> IsShowTree { get; set; }
        public Nullable<bool> IsLastChild { get; set; }
        public string URL { get; set; }
        public string SubfunctionCode { get; set; }
        public Nullable<int> ModuleState { get; set; }
        public Nullable<int> Level { get; set; }
        public Nullable<int> Block { get; set; }
    }
}
