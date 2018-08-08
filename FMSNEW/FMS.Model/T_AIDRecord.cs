using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FMS.Model
{
    /// <summary>
    /// 物料，资产对象
    /// </summary>
    public class T_AIDRecord
    {
        /// <summary>
        /// 对象标识
        /// </summary>
        public string GUID { get; set; }

        public string Business_GUID
        { get; set; }

        public string SubBusiness_GUID
        { set; get; }
        /// <summary>
        /// 公司标识
        /// </summary>
        public string C_GUID { get; set; }

        public string BusinessName
        { get; set; }

        public string SubBusinessName
        { get; set; }

        /// <summary>
        /// 购进日期
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        public Decimal Amount { get; set; }

        /// <summary>
        /// 货币
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// 供应商
        /// </summary>
        public string RPer { get; set; }

        /// <summary>
        /// 物料类别
        /// </summary>
        public string InvType { get; set; }

        /// <summary>
        /// 物料描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 折旧周期（月）
        /// </summary>
        public int DepreciationPeriod { get; set; }


        /// <summary>
        /// 折旧年份
        /// </summary>
        public string Depreciation_year
        { get; set; }

        /// <summary>
        /// 资产，直接物料，间接物料判别标志
        /// </summary>
        public string AID_Flag { get; set; }

        /// <summary>
        /// 剩余价值
        /// </summary>
        public Decimal SurplusValue { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string RPerName { get; set; }

        /// <summary>
        /// 附件
        /// </summary>
        public string A_GUID { get; set; }

        /// <summary>
        /// 费用成本类别
        /// </summary>
        public string CostType { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public Decimal OriginalAmount { get; set; }
        /// <summary>
        /// GUID
        /// </summary>
        public string GUIDTW { get; set; }
        /// <summary>
        ///金额
        /// </summary>
        public Decimal Amountsurplus { get; set; }


        /// <summary>
        ///剩余金额
        /// </summary>
        public Decimal ResidualAmount { get; set; }

        /// <summary>
        ///使用金额
        /// </summary>
        public Decimal AmountUsed { get; set; }


        /// <summary>
        ///转售金额
        /// </summary>
        public Decimal ResaleValue { get; set; }

        /// <summary>
        ///税种
        /// </summary>
        public string TaxationType { get; set; }
        /// <summary>
        ///收入状态
        /// </summary>
        public string StateIE { get; set; }
        /// <summary>
        ///税后金额
        /// </summary>
        public Decimal TaxationAmount { get; set; }
        /// <summary>
        ///总金额
        /// </summary>
        public Decimal SumAmount { get; set; }
        /// <summary>
        ///截止日期
        /// </summary>
        public DateTime AffirmDate { get; set; }
        /// <summary>
        /// 物料子类别
        /// </summary>
        public string SonInvType { get; set; }
        /// <summary>
        /// 资产分类
        /// </summary>
        public string AssetType { get; set; }

        /// <summary>
        /// 物料子类别
        /// </summary>
        public string SubType { get; set; }

        //现在有两个子类别  需要合并成一个子类别(后期修改)

        /// <summary>
        /// 制造产品类别
        /// </summary>
        public string ManufacturedType { get; set; }

        /// <summary>
        /// 制造产品子类别
        /// </summary>
        public string ManufacturedTypeSub { get; set; }
       /// <summary>
        /// 物料类别名称
        /// </summary>
        public string AidTypeName { get; set; }

        public string CreateDate { get; set; }
         /// <summary>
        /// 物料子类别名称
        /// </summary>
        public string ASTTypeName { get; set; }
        ///使用金额
        /// </summary>
        public Decimal Use_AID_Amount { get; set; }
        ///使用日期
        /// </summary>
        public string Create_Date { get; set; }
        /// <summary>
        /// 库存金额
        /// </summary>
        public Decimal InventoryAmount { get; set; }
        /// <summary>
        /// 资产分类
        /// </summary>
        public string Asset_class { get; set;}
         /// <summary>
        /// 用户名称
        /// </summary>
        public string Creator { get; set;}
        /// <summary>
        /// 合同/发票/凭证号
        /// </summary>
        public Decimal Pnumber { get; set; }
        /// <summary>
        /// 转售卖出的实际金额（收入金额）
        /// </summary>
        public Decimal ResaleActualAmount { get; set; }
        /// <summary>
        /// 购买物料数量
        /// </summary>
        public string MaterialNumber { get; set; }
        /// <summary>
        /// 物料数量
        /// </summary>
        public int Number { get; set; }
        /// <summary>
        /// 实时库存
        /// </summary>
        public int Inventory_Number { get; set; }
        /// <summary>
        /// 转售数量
        /// </summary>
        public int ResaleNumber { get; set; }
        /// <summary>
        /// 转售到营业成本的类别
        /// </summary>
        public string Detailed_Categories { get; set; }
        /// <summary>
        /// 物料ID
        /// </summary>
        public string MaterielManage { get; set; }
        /// <summary>
        /// 物料名称
        /// </summary>
        public string MM_Name { get; set; }
    }

    public class FixAssetsAmount
    {
        /// <summary>
        /// 公司标识
        /// </summary>
        public string C_GUID { get; set; }
        /// <summary>
        /// 固定资产
        /// </summary>
        public Decimal FixedAssets { get; set; }
        /// <summary>
        /// 累计折旧
        /// </summary>
        public Decimal AccumulatedDepreciation { get; set; }
        /// <summary>
        /// 无形资产
        /// </summary>
        public Decimal IntangibleAssets { get; set; }
        /// <summary>
        /// 累计摊销
        /// </summary>
        public Decimal AccumulatedAmortization { get; set; }
        /// <summary>
        /// 长期待摊费用
        /// </summary>
        public Decimal DeferredExpense { get; set; }
    }
} 
