using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FMS.DAL
{
   public class T_PayMethod
    {
       public string GUID
       { get; set; }

       public string RPLA_GUID
       { get; set; }

       public string RPDA_GUID
       { get; set; }

       public string RPTh_GUID
       { get; set; }

       public string RP_GUID
       { get; set; }

       public string VoucherType
       { get; set; }

       public decimal AssetAmount
       { get; set; }

       public decimal DebtAmount
       { get; set; }

       public string Summary
       { get; set; }
    }
}
