using System;

namespace FMS.Model
{
    public class T_RecPayRecord
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        public string RP_GUID
        { get; set; }

        public string A_GUID
        { get; set; }

        /// <summary>
        /// 公司GUID
        /// </summary>
        public string C_GUID
        { get; set; }

        ///<summary>
        ///资金类别
        /// </summary>
        public string PayCategoryID
        {
            get;
            set;
        }
        public string PayCategory
        {
            get;
            set;
        }
        /// <summary>
        /// 资金子类
        /// </summary>
        public string DetailRPTypeID
        {
            get;
            set;
        }
        public string DetailRPType
        {
            get;
            set;
        }
        /// <summary>
        /// 收/付标识
        /// </summary>
        public string RP_Flag
        { get; set; }

        /// <summary>
        /// 单据类型
        /// </summary>
        public string InvType
        { get; set; }

        /// <summary>
        /// 单据二级类型
        /// </summary>
        public string InvTypeDts
        { get; set; }

        /// <summary>
        /// 单据三级类型
        /// </summary>
        public string DetailInvType
        { get; set; }

public string DetailInvTypeID
        { get; set; } /// <summary>
        /// 单据三级类型
        /// </summary>
        public string ThirdInvType
        { get; set; }
        /// <summary>
        /// 凭证编号
        /// </summary>
        public string AccountID
        { get; set; }
        public string Log
        { get; set; }
        public string Log_c
        { get; set; }
        /// <summary>
        /// 单据号
        /// </summary>
        public string InvNo
        { get; set; }

        /// <summary>
        /// 收/付款方
        /// </summary>
        public string RPer
        { get; set; }

        /// <summary>
        /// 账户简称
        /// </summary>
        public string AccountAbbreviation { get; set; }
        /// <summary>
        /// 银行
        /// </summary>
        public string B_GUID
        { get; set; }

        /// <summary>
        /// 银行账号
        /// </summary>
        public string BA_GUID
        { get; set; }

        /// <summary>
        /// 借方总账科目
        /// </summary>
        public string DebitLedgerAccount
        { get; set; }

        /// <summary>
        /// 借方总账科目名称
        /// <remarks>扩展字段</remarks>
        /// </summary>
        public string DebitLedgerAccountName
        { get; set; }

        /// <summary>
        /// 借方明细科目
        /// </summary>
        public string DebitDetailsAccount
        { get; set; }

        /// <summary>
        /// 借方明细科目名称
        /// <remarks>扩展字段</remarks>
        /// </summary>
        public string DebitDetailsAccountName
        { get; set; }

        /// <summary>
        /// 贷方总账科目
        /// </summary>
        public string CreditLedgerAccount
        { get; set; }

        /// <summary>
        /// 贷方总账科目名称
        /// <remarks>扩展字段</remarks>
        /// </summary>
        public string CreditLedgerAccountName
        { get; set; }

        /// <summary>
        /// 贷方明细科目
        /// </summary>
        public string CreditDetailsAccount
        { get; set; }

        /// <summary>
        /// 贷方明细科目名称
        /// <remarks>扩展字段</remarks>
        /// </summary>
        public string CreditDetailsAccountName
        { get; set; }

        /// <summary>
        /// 发生金额(即是付款金额也是收款金额)
        /// </summary>
        public decimal SumAmount            
        { get; set; }

        public decimal DisAmount { get; set; }

        public decimal DisAmount1 { get; set; }
        /// <summary>
        /// 发生日期
        /// </summary>
        public string Date
        { get; set; }

        public string Mark
        { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string Creator
        { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateDate
        { get; set; }

        /// <summary>
        /// 收/付款方
        /// <remarks>扩展字段</remarks>
        /// </summary>
        public string R_PerName
        { get; set; }

        public string RPerName
        { get; set; }
        ///<summary>
        ///收/付款池
        ///<summary>
        public string RPable
        {
            get;set;
        }
        /// <summary>
        /// 常用币制
        /// </summary>
        public string Currency
        {
            get;set;
        }

        /// <summary>
        /// 单据类型名
        /// <remarks>扩展字段</remarks>
        /// </summary>
        public string TypeName
        {
            get;set;
        }

        /// <summary>
        /// 现金流量项目
        /// </summary>
        public string CFItemGuid
        {
            get;set;
        }

        /// <summary>
        /// 现金流量项目名称
        /// </summary>
        public string CFItemName
        {
            get;
            set;
        }
        /// <summary>
        /// 已销金额
        /// </summary>
        public Decimal Amount_Used { get; set; }
        /// <summary>
        /// 现金流量顶级项目
        /// </summary>
        public string CFPItemGuid
        {
            get;
            set;
        }

        /// <summary>
        /// 现金流量顶级项目名称
        /// </summary>
        public string CFPItemName
        {
            get;
            set;
        }

        /// <summary>
        /// 银行账号
        /// <remarks>扩展字段</remarks>
        /// </summary>
        public string BankAccount
        { get; set; }

        /// <summary>
        /// 唯一标识
        /// </summary>
        public string IE_GUID
        { get; set; }

        /// <summary>
        /// 发生日期string
        /// </summary>
        public string DateS
        { get; set; }
        /// <summary>
        /// 发生日期string
        /// </summary>
        public string Record
        { get; set; }

        /// <summary>
        /// 凭证摘要
        /// </summary>
        public string Summary
        {
            get;
            set;
        }
        /// <summary>
        /// 凭证科目
        /// </summary>
        /// <summary>

        public string Name { get; set; }
        /// 借方金额
        /// </summary>
        public Decimal DebitAmount { get; set; }

        /// <summary>
        /// 贷方金额
        /// </summary>
        public Decimal CreditAmount { get; set; }

        /// <summary>
        /// 查询记录生成GUID
        /// </summary>
        public string TNEWGUID { get; set; }

        /// <summary>
        /// (现金流返回收款金额)
        /// </summary>
        public decimal RecSumAmount
        { get; set; }
        /// <summary>
        /// (现金流返回付款金额)
        /// </summary>
        public decimal PaySumAmount
        { get; set; }
        /// <summary>
        /// 现金流返回余金额（等于收款金额-付款金额）
        /// </summary>
        public decimal BalanceSumAmount
        { get; set; }
        /// <summary>
        /// 期初余额
        /// </summary>
        public decimal InitialAmount
        { get; set; }
        /// <summary>
        /// 收入转
        /// </summary>
        public decimal RecSumAmountZ
        { get; set; }
        /// <summary>
        /// 支出转
        /// </summary>
        public decimal PaySumAmountZ
        { get; set; }

        ///<summary>
        ///支付类型
        ///</summary>
        public string RPType
        { get; set; }
        
        
        ///<summary>
        ///科目名称(对应现金流类别)
        ///</summary>
        public string SubjectName
        { get; set; }

        public int IsInit
        {
            get;
            set;
        }
    }

    public class RPVoucherInfo
    {
        public string InvTypeDts { get; set; }
        public string GeneralName { get; set; }
        public string DetailedName { get; set; }
        public string ThirdName { get; set; }
        public string InvType { get; set; }
        public string IEDA_GUID { get; set; }
        public decimal AssetAmount { get; set; }
        public decimal DebtAmount { get; set; }
    }

}
