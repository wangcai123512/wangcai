using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FMS.Model
{
    /// <summary>
    /// 税费对象
    /// </summary>
    public class T_WageCost
    {
        /// <summary>
        /// 工资标识
        /// </summary>
        public string W_GUID { get; set; }

        /// <summary>
        /// 所属公司
        /// </summary>
        public string C_GUID { get; set; }
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


        public string Profit_GUID { get; set;}

        public string Profit_Name { get; set; }

        public string Employee { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public string State { get; set; }
        /// <summary>
        /// 类别
        /// </summary>
        public string InvType { get; set; }

        /// <summary>
        /// 基本工资==》职工工资
        /// </summary>
        public decimal Cash { get; set; }
        /// <summary>
        /// 已销金额
        /// </summary>
        public Decimal Amount_Used { get; set; }

        /// <summary>
        /// 未销金额
        /// </summary>
        public Decimal DisAmount { get; set; }
        /// <summary>
        /// 个人所得税
        /// </summary>
        public decimal PersonalTaxes { get; set; }

        /// <summary>
        /// 社保福利
        /// </summary>
        public decimal SocialSecurity { get; set; }

        /// <summary>
        /// 个人总数
        /// </summary>
        public decimal Total { get; set; }

        /// <summary>
        ///
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 支付类型
        /// </summary>
        public string PayType { get; set; }

        /// <summary>
        /// 职工福利费
        /// </summary>
        public decimal EmployeeWelfare { get; set; }

        /// <summary>
        /// 奖金、津贴和补贴
        /// </summary>
        public decimal BonusAllowance { get; set; }

        /// <summary>
        /// 住房公积金
        /// </summary>
        public decimal HousingProvident { get; set; }

        /// <summary>
        /// 工会经费
        /// </summary>
        public decimal TradeUnion{ get; set; }

        /// <summary>
        /// 职工教育经费
        /// </summary>
        public decimal StaffEducation { get; set; }

        /// <summary>
        /// 非货币性福利
        /// </summary>
        public decimal NonCurrency { get; set; }

        /// <summary>
        /// 辞退福利
        /// </summary>
        public decimal DismissWelfare { get; set; }
        private int salaryType = 0;
        /// <summary>
        /// 工资来源（0为员工职工薪酬，1为其他页面）
        /// </summary>
        public int SalaryType { get { return salaryType; } 
            set { salaryType = value; } }
        public string Name { get; set; }
    }
}
