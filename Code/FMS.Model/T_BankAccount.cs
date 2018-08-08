using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FMS.Model
{
    /// <summary>
    /// 银行账户对象
    /// </summary>
    public class T_BankAccount
    {
        /// <summary>
        /// 银行账户标识
        /// </summary>
        public string BA_GUID { get; set; }

        /// <summary>
        /// 银行标识
        /// </summary>
        public string B_GUID { get; set; }

        /// <summary>
        /// 银行账号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 公司标识
        /// </summary>
        public string C_GUID { get; set; }

        /// <summary>
        /// 账户名称
        /// </summary>
        public string AccountName { get; set; }

        /// <summary>
        /// 币种
        /// </summary>
        public string AccountCurrency { get; set; }

        /// <summary>
        /// 账户简称
        /// </summary>
        public string AccountAbbreviation { get; set; }

        /// <summary>
        /// 账号类型
        /// </summary>
        public string AccountType { get; set; }

        /// <summary>
        /// 银行地址
        /// </summary>
        public string BankAddress { get; set; }

        /// <summary>
        /// Swift代码
        /// </summary>
        public string SwiftCode { get; set; }
        
        /// <summary>
        /// 账户余额
        /// </summary>
        public decimal Amount { get; set; }
    }
}
