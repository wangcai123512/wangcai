using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FMS.Model
{
    public class T_ModuleList
    {
        public string Guid
        { get; set; }

        public string ChineseName
        { get; set; }

        public string EnglishName
        { get; set; }
       
        public string OrderNumber
        { get; set; }

        public string ModuleID
        { get; set; }

        public string IsShowTree
        { get; set; }

         public string IsLastChild
        { get; set; }

         public string URL
        { get; set; }

         public string SubfunctionCode
        { get; set; }

         public string ModuleState
        { get; set; }

         public string Block
        { get; set; }

    }
}
