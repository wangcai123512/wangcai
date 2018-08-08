using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FMS.Model
{
    public class Co_Tr_ThirdAccount
    {

        public string TDA_GUID
        { get; set; }


        public string State
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
        public string ParentAccGuid
        { get; set; }

        public string C_GUID
        { get; set; }

        public string Level
        { get; set; }

        public string Mark
        { get; set; }

        public decimal Amount
        { get; set; }

        public decimal OldAmount
        { get; set; }

        public decimal diffAmount
        { get; set; }

        public string createDate
        { get; set; }

        public decimal XiaoAmount
        { get; set; }

        public decimal JingAmount
        { get; set; }

        public decimal YiJiaoAmount
        { get; set; }

        public string isend
        { get; set; }

        public string rownumber
        { get; set; }

        public string isfanjie
        { get; set; }

        public string TaxID
        { get; set; }

        public string period_type
        { get; set; }

        public decimal ChuNeiAmount
        { get; set; }

        public decimal XiaoChuAmount
        { get; set; }

        public decimal ChuKouAmount
        { get; set; }
    }
}
