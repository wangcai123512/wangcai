using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FMS.Model
{
    public class T_DetailTaxSettle
    {
        public T_DetailTaxSettle()
        { }
        #region Model
        private string _guid;
        private string _taxid;
        private string _taxtype;
        private string _taxname;
        private decimal? _taxamount;
        private decimal? _disAmount;
        private string _c_guid;
        private DateTime? _createdate;
        private DateTime? _AffirmDate;
        private string _state;
       
        /// <summary>
        /// 
        /// </summary>
        public string GUID
        {
            set { _guid = value; }
            get { return _guid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string TaxID
        {
            set { _taxid = value; }
            get { return _taxid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string TaxType
        {
            set { _taxtype = value; }
            get { return _taxtype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string TaxName
        {
            set { _taxname = value; }
            get { return _taxname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? TaxAmount
        {
            set { _taxamount = value; }
            get { return _taxamount; }
        }
        public decimal? DisAmount
        {
            set { _disAmount = value; }
            get { return _disAmount; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string C_GUID
        {
            set { _c_guid = value; }
            get { return _c_guid; }
        }

        public string State
        {
            set { _state = value; }
            get { return _state; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? CreateDate
        {
            set { _createdate = value; }
            get { return _createdate; }
        }

        public DateTime? AffirmDate
        {
            set { _AffirmDate = value; }
            get { return _AffirmDate; }
        }

       
        #endregion Model

    }
}
