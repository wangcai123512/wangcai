using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FMS.Model
{
    public class T_Voucher
    {
        public string GUID
        { get; set; }

        public string IE_GUID
        { get;set;}

        public string RP_GUID
        { get; set; }

        public string C_GUID
        { get; set; }

        public string VoucherFlag
        { get; set; }

        public string State
        { get; set; }

        public string Date
        { get; set; }

        public string Summary
        { get; set; }

        public Decimal DisAmount
        { get; set; }

        public Decimal Amount
        { get; set; }
    }
}
