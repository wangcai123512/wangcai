using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FMS.Model
{
   public class HisTr_GeneralLedgerAccount
    {
        public string LA_GUID
        { get; set; }

    
        public string State
        {
            get;
            set;
        }
        public string V_GUID
        {
            get;
            set;
        }
        /// <summary>
        /// 科目代码
        /// </summary>
        public int AccCode
        { get; set; }

        /// <summary>
        /// 科目名称
        /// </summary>
        public string Name
        { get; set; }

        
        /// <summary>
        /// 科目类别
        /// </summary>
        public int AccGroup
        { get; set; }

        public decimal Amount
        { get; set; }

        public decimal OldAmount
        { get; set; }

        public decimal diffAmount
        { get; set; }

        public string createDate
        { get; set; }
      
    }
}
