using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FMS.Model
{
    /// <summary>
    /// T_DetailSalary:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class T_DetailSalary
    {
        public T_DetailSalary()
        { }
        #region Model
        private string _guid;
        private string _salarytype;
        private string _wagecostid;
        private string _salaryname;
        private string _salaryla_guid;
        private string _salaryda_guid;
        private decimal? _salaryamount;
        private decimal? _disamount;
        private string _c_guid;
        private string _state;
        private DateTime? _createdate;
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
        public string WageCostID
        {
            set { _wagecostid = value; }
            get { return _wagecostid; }
        }

        public string BP_GUID
        {
            get;
            set;
        }
        /// <summary>
        /// 
        /// </summary>
        public string SalaryName
        {
            set { _salaryname = value; }
            get { return _salaryname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SalaryLA_GUID
        {
            set { _salaryla_guid = value; }
            get { return _salaryla_guid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SalaryDA_GUID
        {
            set { _salaryda_guid = value; }
            get { return _salaryda_guid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? SalaryAmount
        {
            set { _salaryamount = value; }
            get { return _salaryamount; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? DisAmount
        {
            set { _disamount = value; }
            get { return _disamount; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string C_GUID
        {
            set { _c_guid = value; }
            get { return _c_guid; }
        }
        /// <summary>
        /// 
        /// </summary>
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
        private int salaryType = 0;
        /// <summary>
        /// 工资来源（0为员工职工薪酬，1为其他页面）
        /// </summary>
        public int SalaryType
        {
            get { return salaryType; }
            set { salaryType = value; }
        }
        /// <summary>
        /// 工资标识
        /// </summary>
        public string W_GUID { get; set; }

        /// <summary>
        /// 货币
        /// </summary>
        public string Currency { get; set; }
        /// <summary>
        /// 工资发放日期
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// 职工
        /// </summary>
        public string Profit_GUID { get; set; }

        public string Profit_Name { get; set; }

        public string Employee { get; set; }
        /// <summary>
        /// 类别
        /// </summary>
        public string InvType { get; set; }
        /// <summary>
        /// 金额总数（不包含个税）
        /// </summary>
        public decimal Total { get; set; }
        public string Name { get; set; }
        public string PayType { get; set; }
        #endregion Model
    }
}
