using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FMS.DAL
{
    public class T_AccountRecord
    {
        public string GUID
        { get; set; }

        public string IELA_GUID
        { get; set; }

        public string IEDA_GUID
        { get; set; }

        public string IETh_GUID
        { get; set; }

        public string Summary
        { get; set; }

        public string IE_GUID
        { get; set; }

        public string InvType
        { get; set; }

        public string VoucherType
        { get; set; }

        public decimal AssetAmount
        { get; set; }

        public decimal DebtAmount
        { get; set; }

        public string VoucherFlag
        { get; set; }

        public string State
        { get; set; }

        public Decimal DisAmount
        { get; set; }
        public string DetailInvType
        { get; set; }

        public string ThirdInvType
        { get; set; }
    }
}
