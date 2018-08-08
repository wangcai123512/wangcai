using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FMS.Model
{
    public class T_BusinessType
    {
        public string GUID
        { get; set; }

        public string BusinessName
        { get; set; }

        public string C_GUID
        { get; set; }

        public string Sub_GUID
        { get; set; }

        public string SubBusinessName
        { get; set; }

        public string Parent_GUID
        { get; set; }

        public string Remark
        { get; set; }

    }
}
