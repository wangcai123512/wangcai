using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using BaseController;
using FMS.Model;
using FMS.DAL;
using System.Web.Script.Serialization;
using System.Transactions;
namespace FMS.BLL
{
    public class VoucherQueryController : UserController
    {
        public VoucherQueryController()
            : base("VoucherQuery")
        { }

        public ActionResult Index()
        {
            return View();
        }

        public string GetALLVoucher(string Date, string M_GUID, string VouchGuid)
        {
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            List<T_IERecord> List = new IESvc().GetAllVoucher(Date, C_GUID, M_GUID, VouchGuid);
            string json = new JavaScriptSerializer().Serialize(List);
            return json;
        }

        public string GetVoucherInfo(string Date, string VouchID)
        {
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            List<T_IERecord> List = new IESvc().GetAllVoucher(Date, C_GUID, null, VouchGuid: VouchID);
            string json = new JavaScriptSerializer().Serialize(List);
            return json;
        }

        public ActionResult VoucherRecord()
        {
            return View();
        }

        public string UpdVoucher(List<T_Voucher> VoucherList)
        {
            bool result = false;
            string msg = string.Empty;
            foreach (T_Voucher voucher in VoucherList)
            {
                voucher.C_GUID = Session["CurrentCompanyGuid"].ToString();
                result = new VoucherSVC().UpdVoucher(voucher);
            }
            if (result)
            {
                msg = General.Resource.Common.Success;
            }
            else
            {
                msg = General.Resource.Common.Failed;
            }
            return string.Format("{{\"Result\":{0},\"Msg\":\"{1}\"}}"
               , result.ToString().ToLower(), msg);
        }

        public string UpdAccountRecord(List<T_AccountRecord> RecordList, List<T_Voucher> VoucherList, string Date)
        {
            bool result = false;
            bool result1 = true;
            string msg = string.Empty;
            string Flag = null;
            string RPer = null;
            string record = null;
            string PayCategoryID = null;
            string DetailRPTypeID = null;
            string DetailInvtype = null;
            string AccountID = null;
            string BA_GUID = null;
            string RPInvType = null;
            string InvTypeDts = null;
            string CFItemGuid = null;
            string SubjectName = null;
            string VoucherFlag = null;
            string C_GUID = Session["CurrentCompanyGuid"].ToString();
            string Creator = base.userData.LoginFullName;
            string Currency = Session["Currency"].ToString();
            string[] strs1 = new string[] { "预付账款", "备用金", "其他应收款", "其他应付款", "短期投资", "长期债券投资", "长期股权投资", "固定资产", "无形资产", "短期借款", "长期借款", "应付利润", "应付利息" };
            string[] strs2 = new string[] { "预收账款", "实收资本", "资本公积", "短期投资", "长期债券投资", "长期股权投资", "其他应收款", "其他应付款", "短期借款", "长期借款" };
            List<T_IERecord> IEList = new List<T_IERecord>();
            List<T_RecPayRecord> RPList = new List<T_RecPayRecord>();
            T_WageCost WageCostRecord = new T_WageCost();
            List<T_WageCost> wageList = new List<T_WageCost>();
            List<T_DeclareCostSpending> DSList = new List<T_DeclareCostSpending>();
            List<T_DeclareCustomer> DCList = new List<T_DeclareCustomer>();
            List<T_TaxSettlement> TaxList = new List<T_TaxSettlement>();
            //增值税使用
            List<T_TaxSettlement> OtherTaxList = new List<T_TaxSettlement>();
            T_TaxSettlement tax = new T_TaxSettlement();
            string EmployeeContent = string.Empty;
            T_GeneralLedgerAccount gen = new AccountSvc().GetLAByName("银行存款", Session["CurrentCompanyGuid"].ToString());
            string RpTypeID = string.Empty;
            decimal totolAmount = 0;
            decimal totolTaxAmount = 0;
            if (gen != null)
            {
                RpTypeID = gen.LA_GUID;
            }
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {
                foreach (T_Voucher voucher in VoucherList)
                {
                    voucher.C_GUID = Session["CurrentCompanyGuid"].ToString();
                    result = new VoucherSVC().UpdVoucher(voucher);
                }
                if (result)
                {
                    foreach (T_AccountRecord accountRecord in RecordList)
                    {
                        T_RecPayRecord RP = new T_RecPayRecord();
                        T_IERecord IE = new T_IERecord();
                        T_DeclareCostSpending DS = new T_DeclareCostSpending();
                        T_DeclareCustomer DC = new T_DeclareCustomer();
                        AccountID = accountRecord.IE_GUID;
                        Flag = accountRecord.VoucherFlag;
                        switch (accountRecord.VoucherFlag)
                        {
                            case "TS":
                                if (accountRecord.InvType == "主营业务成本" || accountRecord.InvType == "其他业务成本" || accountRecord.InvType == "销售费用" || accountRecord.InvType == "管理费用" || accountRecord.InvType == "财务费用" || accountRecord.InvType == "营业外支出")
                                {
                                    IE.Amount = accountRecord.AssetAmount;
                                    IE.SumAmount = accountRecord.AssetAmount;
                                    IE.DisAmount = 0;
                                    IE.RPer = accountRecord.IETh_GUID;
                                    IE.IE_Flag = "E";
                                    IE.C_GUID = C_GUID;
                                    IE.Creator = Creator;
                                    IE.Currency = Currency;
                                    IE.InvType = accountRecord.InvType;
                                    IE.IEGroup = accountRecord.IEDA_GUID;
                                    IE.IE_GUID = Guid.NewGuid().ToString();
                                    IE.AccountID = accountRecord.IE_GUID;
                                    IE.State = "已付";
                                    IEList.Add(IE);
                                }
                                break;
                            case "CT":
                                VoucherFlag = accountRecord.VoucherFlag;
                                if (accountRecord.InvType == "所得税费用")
                                {
                                    tax.GUID = Guid.NewGuid().ToString();
                                    tax.Flag = accountRecord.InvType;
                                    tax.C_GUID = C_GUID;
                                    tax.Amount = accountRecord.AssetAmount;
                                    tax.DisAmount = accountRecord.AssetAmount;
                                    tax.AccountID = accountRecord.IE_GUID;
                                }
                                break;
                            case "YT":
                                VoucherFlag = accountRecord.VoucherFlag;
                                if (accountRecord.InvType == "营业税金及附加")
                                {
                                    tax.GUID = Guid.NewGuid().ToString();
                                    tax.Flag = accountRecord.InvType;
                                    tax.C_GUID = C_GUID;
                                    tax.Amount = accountRecord.AssetAmount;
                                    tax.DisAmount = accountRecord.AssetAmount;
                                    tax.AccountID = accountRecord.IE_GUID;
                                }
                                if (accountRecord.InvType == "应交税费")
                                {
                                    switch (accountRecord.DetailInvType)
                                    {
                                        case "消费税":
                                            tax.Excise = accountRecord.DebtAmount;
                                            break;
                                        case "教育费附加":
                                            tax.EducationFee = accountRecord.DebtAmount;
                                            break;
                                        case "营业税":
                                            tax.Sales = accountRecord.DebtAmount;
                                            break;
                                        case "城市维护建设税":
                                            tax.UrbanConstruction = accountRecord.DebtAmount;

                                            break;
                                        case "资源税":
                                            tax.Resource = accountRecord.DebtAmount;
                                            break;
                                        case "土地增值税":
                                            tax.LandValue = accountRecord.DebtAmount;
                                            break;
                                        case "城镇土地使用税":
                                            tax.UrbanLand = accountRecord.DebtAmount;
                                            break;
                                        case "房产税":
                                            tax.Property = accountRecord.DebtAmount;
                                            break;
                                        case "车船税":
                                            tax.VehicleVessel = accountRecord.DebtAmount;
                                            break;
                                        case "矿产资源补偿费":
                                            tax.MineralResources = accountRecord.DebtAmount;
                                            break;
                                        case "排污费":
                                            tax.Dischargefee = accountRecord.DebtAmount;
                                            break;
                                        default:
                                            break;
                                    }
                                }

                                break;
                            case "I":
                                if (accountRecord.InvType != "应收账款" && accountRecord.InvType != "应收利息" && accountRecord.InvType != "应收股利")
                                {
                                    IE.Amount = accountRecord.DebtAmount;
                                    IE.SumAmount = accountRecord.DebtAmount;
                                    IE.DisAmount = IE.SumAmount;
                                    totolAmount += accountRecord.DebtAmount;
                                    IE.RPer = accountRecord.IETh_GUID;
                                    IE.IE_Flag = "I";
                                    IE.C_GUID = C_GUID;
                                    IE.Creator = Creator;
                                    IE.Currency = Currency;
                                    IE.AccountID = accountRecord.IE_GUID;
                                    IE.IE_GUID = Guid.NewGuid().ToString();
                                    IE.State = accountRecord.State;
                                }
                                switch (accountRecord.InvType)
                                {
                                    case "主营业务收入":
                                        IE.InvType = "主营业务收入";
                                        IEList.Add(IE);
                                        break;
                                    case "其他业务收入":
                                        IE.InvType = accountRecord.InvType;
                                        IE.DetailInvtype = accountRecord.IEDA_GUID;
                                        IEList.Add(IE);
                                        break;
                                    case "营业外收入":
                                        IE.InvType = accountRecord.InvType;
                                        IE.DetailInvtype = accountRecord.IEDA_GUID;
                                        IEList.Add(IE);
                                        break;
                                    case "投资收益":
                                        IE.InvType = accountRecord.InvType;
                                        IEList.Add(IE);
                                        break;
                                    case "应收账款":
                                        RPer = accountRecord.IETh_GUID;
                                        break;
                                    case "应收股利":
                                        RPer = accountRecord.IETh_GUID;
                                        DetailInvtype = "股利";
                                        break;
                                    case "应收利息":
                                        RPer = accountRecord.IETh_GUID;
                                        DetailInvtype = "利息";
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            case "E":
                                if (accountRecord.InvType != "应付账款")
                                {
                                    IE.Amount = accountRecord.AssetAmount;
                                    IE.SumAmount = accountRecord.AssetAmount;
                                    IE.DisAmount = IE.SumAmount;
                                    totolAmount += accountRecord.AssetAmount;
                                    IE.RPer = accountRecord.IETh_GUID;
                                    IE.IE_Flag = "E";
                                    IE.C_GUID = C_GUID;
                                    IE.Creator = Creator;
                                    IE.Currency = Currency;
                                    IE.IE_GUID = Guid.NewGuid().ToString();
                                    IE.AccountID = accountRecord.IE_GUID;
                                    IE.State = accountRecord.State;
                                }
                                switch (accountRecord.InvType)
                                {
                                    case "主营业务成本":
                                        IE.InvType = "营业成本";
                                        IEList.Add(IE);
                                        break;
                                    case "销售费用":
                                        IE.InvType = accountRecord.InvType;
                                        IE.IEGroup = accountRecord.IEDA_GUID;
                                        IEList.Add(IE);
                                        break;
                                    case "财务费用":
                                        IE.InvType = accountRecord.InvType;
                                        IE.IEGroup = accountRecord.IEDA_GUID;
                                        IEList.Add(IE);
                                        break;
                                    case "管理费用":
                                        IE.InvType = accountRecord.InvType;
                                        IE.IEGroup = accountRecord.IEDA_GUID;
                                        IEList.Add(IE);
                                        break;
                                    case "其他业务成本":
                                        IE.InvType = accountRecord.InvType;
                                        IE.IEGroup = accountRecord.IEDA_GUID;
                                        IEList.Add(IE);
                                        break;
                                    case "营业外支出":
                                        IE.InvType = accountRecord.InvType;
                                        IE.IEGroup = accountRecord.IEDA_GUID;
                                        IEList.Add(IE);
                                        break;
                                    case "应付账款":
                                        RPer = accountRecord.IETh_GUID;
                                        break;
                                    default:
                                        break;
                                }

                                break;
                            case "SA":
                                if (EmployeeContent != accountRecord.InvType && accountRecord.InvType != "应交税费" && EmployeeContent != "应交税费")
                                {
                                    if (WageCostRecord.W_GUID != null)
                                    {
                                        wageList.Add(WageCostRecord);
                                        WageCostRecord = null;
                                    }
                                    WageCostRecord = new T_WageCost();
                                    WageCostRecord.W_GUID = Guid.NewGuid().ToString();
                                    WageCostRecord.C_GUID = Session["CurrentCompanyGuid"].ToString();
                                    WageCostRecord.State = "未付";
                                    WageCostRecord.PayType = RpTypeID;
                                    WageCostRecord.Date = DateTime.Parse(Date);
                                    WageCostRecord.Profit_Name = EmployeeContent;
                                    WageCostRecord.Employee = "员工";
                                    WageCostRecord.Currency = Currency;
                                }

                                if (accountRecord.DetailInvType == "职工工资")
                                {
                                    WageCostRecord.Cash = accountRecord.DebtAmount;
                                }
                                if (accountRecord.DetailInvType == "奖金、津贴和补贴")
                                {
                                    WageCostRecord.BonusAllowance = accountRecord.DebtAmount;
                                }
                                if (accountRecord.DetailInvType == "职工福利费")
                                {
                                    WageCostRecord.EmployeeWelfare = accountRecord.DebtAmount;
                                }
                                if (accountRecord.DetailInvType == "社会保险费")
                                {
                                    WageCostRecord.SocialSecurity = accountRecord.DebtAmount;
                                }
                                if (accountRecord.DetailInvType == "住房公积金")
                                {
                                    WageCostRecord.HousingProvident = accountRecord.DebtAmount;
                                }
                                if (accountRecord.DetailInvType == "工会经费")
                                {
                                    WageCostRecord.TradeUnion = accountRecord.DebtAmount;
                                }
                                if (accountRecord.DetailInvType == "职工教育经费")
                                {
                                    WageCostRecord.StaffEducation = accountRecord.DebtAmount;
                                }
                                if (accountRecord.DetailInvType == "非货币性福利")
                                {
                                    WageCostRecord.NonCurrency = accountRecord.DebtAmount;
                                }
                                if (accountRecord.DetailInvType == "辞退福利")
                                {
                                    WageCostRecord.DismissWelfare = accountRecord.DebtAmount;
                                }
                                if (accountRecord.DetailInvType == "个人所得税")
                                {
                                    WageCostRecord.PersonalTaxes = accountRecord.DebtAmount;
                                }

                                break;
                            case "TA":
                                tax = new T_TaxSettlement();
                                tax.TaxName = accountRecord.ThirdInvType;
                                if (accountRecord.ThirdInvType == "进项税额")
                                {
                                    totolTaxAmount += accountRecord.AssetAmount;
                                    tax.Amount = accountRecord.AssetAmount;
                                }
                                else
                                {
                                    totolTaxAmount += accountRecord.DebtAmount;
                                    tax.Amount = accountRecord.DebtAmount;
                                    tax.TaxName = "销项税额";
                                }
                                tax.C_GUID = Session["CurrentCompanyGuid"].ToString();
                                tax.AccountID = accountRecord.IE_GUID;
                                tax.Date = DateTime.Parse(Date);
                                OtherTaxList.Add(tax);
                                break;
                            case "TR":
                                if (accountRecord.InvType == "银行存款")
                                {
                                    if (accountRecord.DebtAmount > 0)
                                    {
                                        PayCategoryID = accountRecord.IELA_GUID;
                                        DetailRPTypeID = accountRecord.IEDA_GUID;
                                        BA_GUID = accountRecord.IEDA_GUID;
                                        SubjectName = "内部转账";
                                        RP.RP_GUID = Guid.NewGuid().ToString();
                                        RP.AccountID = accountRecord.IE_GUID;
                                        RP.DisAmount = 0;
                                        RP.SumAmount = accountRecord.DebtAmount;
                                        RP.RP_Flag = "P";
                                        RP.Record = "已销账";
                                        RP.C_GUID = C_GUID;
                                        RP.InvType = "内部转账";
                                        RP.Creator = Creator;
                                        RP.Currency = Currency;
                                        RP.AccountID = accountRecord.IE_GUID;
                                        RPer = accountRecord.IETh_GUID;
                                        RPList.Add(RP);
                                    }

                                    if (accountRecord.AssetAmount > 0)
                                    {
                                        RP.RP_GUID = Guid.NewGuid().ToString();
                                        RP.AccountID = accountRecord.IE_GUID;
                                        RP.DisAmount = 0;
                                        RP.SumAmount = accountRecord.AssetAmount;
                                        RP.RP_Flag = "R";
                                        RP.Record = "已销账";
                                        RP.C_GUID = C_GUID;
                                        RP.Creator = Creator;
                                        RP.Currency = Currency;
                                        RP.InvType = "内部转账";
                                        RP.AccountID = accountRecord.IE_GUID;
                                        RPer = accountRecord.IETh_GUID;
                                        RPList.Add(RP);
                                    }
                                }
                                break;
                            case "R":
                                if (accountRecord.InvType == "银行存款" || accountRecord.InvType == "库存现金" || accountRecord.InvType == "其他货币资金")
                                {
                                    PayCategoryID = accountRecord.IELA_GUID;
                                    DetailRPTypeID = accountRecord.IEDA_GUID;




                                    if (accountRecord.InvType == "银行存款")
                                    {
                                        BA_GUID = accountRecord.IEDA_GUID;
                                        SubjectName = "应收账款";
                                    }
                                    if (accountRecord.InvType == "库存现金")
                                    {
                                        SubjectName = "应收账款";
                                    }
                                    if (accountRecord.InvType == "其他货币资金")
                                    {
                                        SubjectName = "应收票据";
                                    }
                                }
                                if (accountRecord.InvType == "应交税费")
                                {
                                    tax = new T_TaxSettlement();
                                    totolTaxAmount += accountRecord.DebtAmount;
                                    tax.Amount = accountRecord.DebtAmount;
                                    tax.C_GUID = Session["CurrentCompanyGuid"].ToString();
                                    tax.TaxName = "销项税额";
                                    tax.AccountID = accountRecord.IE_GUID;
                                    tax.Date = DateTime.Parse(Date);
                                    OtherTaxList.Add(tax);
                                }
                                if (accountRecord.InvType == "应收账款" || accountRecord.InvType == "应收股利" || accountRecord.InvType == "应收利息" || accountRecord.InvType == "应收票据")
                                {
                                    RP.RP_Flag = "R";
                                    RP.C_GUID = C_GUID;
                                    RP.Creator = Creator;
                                    RP.Currency = Currency;
                                    RP.AccountID = accountRecord.IE_GUID;
                                    RPer = accountRecord.IETh_GUID;
                                    switch (accountRecord.InvType)
                                    {
                                        case "应收账款":
                                            RP.Record = "未销账";
                                            RP.InvType = "经营活动收款";
                                            RP.CFItemGuid = "97B181C8-D807-4BF0-8D8D-B23273E7FEFE";
                                            RP.DisAmount = accountRecord.DebtAmount;
                                            RP.SumAmount = accountRecord.DebtAmount;
                                            RP.RP_GUID = Guid.NewGuid().ToString();
                                            break;
                                        case "应收股利":
                                            RP.Record = "未销账";
                                            RP.InvType = "投资活动收款";
                                            RP.InvTypeDts = "取得投资收益的股利的收款";
                                            RP.CFItemGuid = "C55B2A4E-129B-407B-AC0B-14C091587D45";
                                            RP.DisAmount = accountRecord.DebtAmount;
                                            RP.SumAmount = accountRecord.DebtAmount;
                                            RP.RP_GUID = Guid.NewGuid().ToString();
                                            DetailInvtype = "股利";
                                            break;
                                        case "应收利息":
                                            RP.Record = "未销账";
                                            RP.InvType = "投资活动收款";
                                            RP.InvTypeDts = "取得投资收益的利息的收款";
                                            RP.CFItemGuid = "C55B2A4E-129B-407B-AC0B-14C091587D45";
                                            RP.DisAmount = accountRecord.DebtAmount;
                                            RP.SumAmount = accountRecord.DebtAmount;
                                            RP.RP_GUID = Guid.NewGuid().ToString();
                                            DetailInvtype = "利息";
                                            break;
                                        case "应收票据":
                                            break;
                                    }
                                    RPList.Add(RP);
                                }
                                if (accountRecord.InvType == "主营业务收入" || accountRecord.InvType == "其他业务收入" || accountRecord.InvType == "营业外收入" || accountRecord.InvType == "投资收益")
                                {
                                    IE.Amount = accountRecord.DebtAmount;
                                    IE.SumAmount = accountRecord.DebtAmount;
                                    IE.InvType = accountRecord.InvType;
                                    IE.DetailInvtype = accountRecord.IEDA_GUID;
                                    IE.DisAmount = 0;
                                    RPer = accountRecord.IETh_GUID;
                                    IE.IE_Flag = "I";
                                    totolAmount += accountRecord.DebtAmount;
                                    IE.C_GUID = C_GUID;
                                    IE.Creator = Creator;
                                    IE.Currency = Currency;
                                    IE.IE_GUID = Guid.NewGuid().ToString();
                                    IE.AccountID = accountRecord.IE_GUID;
                                    IE.State = "已收";
                                    RP.SumAmount = accountRecord.DebtAmount;
                                    RP.DisAmount = 0;
                                    RP.RP_Flag = "R";
                                    RP.C_GUID = C_GUID;
                                    RP.Creator = Creator;
                                    RP.Currency = Currency;
                                    RP.RP_GUID = Guid.NewGuid().ToString();
                                    RP.AccountID = accountRecord.IE_GUID;
                                    IE.RP_GUID = RP.RP_GUID;
                                    switch (accountRecord.InvType)
                                    {
                                        case "主营业务收入":
                                            RP.Record = "已销账";
                                            RP.InvType = "经营活动收款";
                                            RP.InvTypeDts = "销售商品/提供服务的收款";
                                            RP.CFItemGuid = "97B181C8-D807-4BF0-8D8D-B23273E7FEFE";
                                            break;
                                        case "其他业务收入":
                                            RP.Record = "已销账";
                                            RP.InvType = "经营活动收款";
                                            RP.InvTypeDts = "其他业务收入的收款";
                                            RP.CFItemGuid = "F6330595-F588-46B0-8998-752C7A1D774B";
                                            break;
                                        case "营业外收入":
                                            RP.Record = "已销账";
                                            RP.InvType = "经营活动收款";
                                            RP.InvTypeDts = "营业外收入的收款";
                                            RP.CFItemGuid = "F6330595-F588-46B0-8998-752C7A1D774B";
                                            break;
                                        case "投资收益":
                                            RP.Record = "已销账";
                                            RP.InvType = "投资活动收款";
                                            RP.CFItemGuid = "C55B2A4E-129B-407B-AC0B-14C091587D45";
                                            break;
                                    }
                                    result1 = new RecPayRecordSvc().UpdVouIERPRecord(IE.IE_GUID, IE.RP_GUID, RP.Record, RP.RP_Flag, IE.C_GUID);
                                    if (result1)
                                    {
                                        RPList.Add(RP);
                                        IEList.Add(IE);
                                    }
                                }
                                if (strs2.Contains(accountRecord.InvType))
                                {
                                    DC.Amount = accountRecord.DebtAmount;
                                    DC.DisAmount = 0;
                                    RPer = accountRecord.IETh_GUID;
                                    DC.C_GUID = C_GUID;
                                    DC.Currency = Currency;
                                    DC.GUID = Guid.NewGuid().ToString();
                                    DC.AccountID = accountRecord.IE_GUID;
                                    DC.State = "已收";
                                    RP.SumAmount = accountRecord.DebtAmount;
                                    RP.DisAmount = 0;
                                    RP.RP_Flag = "R";
                                    RP.C_GUID = C_GUID;
                                    RP.Creator = Creator;
                                    RP.Currency = Currency;
                                    RP.RP_GUID = Guid.NewGuid().ToString();
                                    RP.AccountID = accountRecord.IE_GUID;
                                    switch (accountRecord.InvType)
                                    {
                                        case "预收账款":
                                            DC.InvType = "预收客户账款";
                                            RP.Record = "已销账";
                                            RP.InvType = "经营活动收款";
                                            RP.InvTypeDts = "预收客户账款";
                                            RP.CFItemGuid = "F6330595-F588-46B0-8998-752C7A1D774B";
                                            break;
                                        case "实收资本":
                                            DC.InvType = "收取投资款(注册资本金额以内部分)";
                                            RP.Record = "已销账";
                                            RP.InvType = "筹资活动收款";
                                            RP.InvTypeDts = "收取投资款(注册资本金额以内部分)";
                                            RP.CFItemGuid = "77A24D5F-3E0C-4211-A552-191FEE0E06FD";
                                            break;
                                        case "资本公积":
                                            DC.InvType = "收取投资的收款(超出注册资本金额部分)";
                                            RP.Record = "已销账";
                                            RP.InvType = "筹资活动收款";
                                            RP.InvTypeDts = "收取投资的收款(超出注册资本金额部分)";
                                            RP.CFItemGuid = "77A24D5F-3E0C-4211-A552-191FEE0E06FD";
                                            break;
                                        case "短期投资":
                                            DC.InvType = "收回短期投资的本金金额内的款";
                                            RP.Record = "已销账";
                                            RP.InvType = "投资活动收款";
                                            RP.InvTypeDts = "收回短期投资的本金金额内的款";
                                            RP.CFItemGuid = "496F9D4D-F71B-437A-9EA0-26107D3449C3";
                                            break;
                                        case "长期债券投资":
                                            DC.InvType = "收回长期债券投资的本金金额内的款";
                                            RP.Record = "已销账";
                                            RP.InvType = "投资活动收款";
                                            RP.InvTypeDts = "收回长期债券投资的本金金额内的款";
                                            RP.CFItemGuid = "496F9D4D-F71B-437A-9EA0-26107D3449C3";
                                            break;
                                        case "长期股权投资":
                                            DC.InvType = "收回长期股权投资的本金金额内的款";
                                            RP.Record = "已销账";
                                            RP.InvType = "投资活动收款";
                                            RP.InvTypeDts = "收回长期股权投资的本金金额内的款";
                                            RP.CFItemGuid = "496F9D4D-F71B-437A-9EA0-26107D3449C3";
                                            break;
                                        case "其他应收款":
                                            DC.InvType = "收回公司支出的押金";
                                            RP.Record = "已销账";
                                            RP.InvType = "经营活动收款";
                                            RP.InvTypeDts = "收回公司支出的押金";
                                            RP.CFItemGuid = "97B181C8-D807-4BF0-8D8D-B23273E7FEFE";
                                            break;
                                        case "其他应付款":
                                            DC.InvType = "收到的其他公司支付的押金";
                                            RP.Record = "已销账";
                                            RP.InvType = "经营活动收款";
                                            RP.InvTypeDts = "收到的其它公司支付的押金";
                                            RP.CFItemGuid = "97B181C8-D807-4BF0-8D8D-B23273E7FEFE";
                                            break;
                                        case "短期借款":
                                            DC.InvType = "短期借款所获得的收款";
                                            RP.Record = "已销账";
                                            RP.InvType = "筹资活动收款";
                                            RP.InvTypeDts = "短期借款所获得的收款";
                                            RP.CFItemGuid = "AD2E5437-0917-43E1-807C-41CA6751360F";
                                            break;
                                        case "长期借款":
                                            DC.InvType = "长期借款所获得的收款";
                                            RP.Record = "已销账";
                                            RP.InvType = "筹资活动收款";
                                            RP.InvTypeDts = "长期借款所获得的收款";
                                            RP.CFItemGuid = "AD2E5437-0917-43E1-807C-41CA6751360F";
                                            break;

                                    }
                                    result1 = new RecPayRecordSvc().UpdVouIERPRecord(DC.GUID, RP.RP_GUID, RP.Record, RP.RP_Flag, DC.C_GUID);
                                    if (result1)
                                    {
                                        RPList.Add(RP);
                                        DCList.Add(DC);
                                    }
                                }
                                break;
                            case "P": if (accountRecord.InvType == "银行存款" || accountRecord.InvType == "库存现金" || accountRecord.InvType == "其他货币资金")
                                {
                                    PayCategoryID = accountRecord.IELA_GUID;
                                    DetailRPTypeID = accountRecord.IEDA_GUID;




                                    if (accountRecord.InvType == "银行存款")
                                    {
                                        BA_GUID = accountRecord.IEDA_GUID;
                                        SubjectName = "应付账款";
                                    }
                                    if (accountRecord.InvType == "库存现金")
                                    {
                                        SubjectName = "应付账款";
                                    }
                                    if (accountRecord.InvType == "其他货币资金")
                                    {
                                        SubjectName = "应付票据";
                                    }
                                }
                                if (accountRecord.ThirdInvType == "进项税额")
                                {
                                    tax = new T_TaxSettlement();
                                    totolTaxAmount += accountRecord.AssetAmount;
                                    tax.Amount = accountRecord.AssetAmount;
                                    tax.C_GUID = Session["CurrentCompanyGuid"].ToString();
                                    tax.TaxName = accountRecord.ThirdInvType;
                                    tax.AccountID = accountRecord.IE_GUID;
                                    tax.Date = DateTime.Parse(Date);
                                    OtherTaxList.Add(tax);
                                }
                                if (accountRecord.InvType == "应付账款" || accountRecord.InvType == "应付票据" || accountRecord.InvType == "应付利润" || accountRecord.InvType == "应付利息" || (accountRecord.InvType == "应交税费" && accountRecord.ThirdInvType != "进项税额") || accountRecord.InvType == "应付职工薪酬")
                                {
                                    RP.RP_Flag = "P";
                                    RP.C_GUID = C_GUID;
                                    RP.Creator = Creator;
                                    RP.Currency = Currency;
                                    RP.AccountID = accountRecord.IE_GUID;
                                    RPer = accountRecord.IETh_GUID;
                                    RP.DetailInvType = accountRecord.IEDA_GUID;
                                    switch (accountRecord.InvType)
                                    {
                                        case "应付账款":
                                            RP.Record = "未销账";
                                            RP.InvType = "经营活动付款";
                                            RP.CFItemGuid = "0526C862-F238-4301-A198-E7EC83A645D5";
                                            RP.DisAmount = accountRecord.AssetAmount;
                                            RP.SumAmount = accountRecord.AssetAmount;
                                            RP.RP_GUID = Guid.NewGuid().ToString();
                                            break;
                                        case "应付票据":
                                            break;
                                        case "应付利润":
                                            RP.Record = "未销账";
                                            RP.InvType = "筹资活动付款";
                                            RP.InvTypeDts = "分配利润、股利所支付的款";
                                            RP.CFItemGuid = "5BDE7476-F268-4A62-8CC3-D426D51E253D";
                                            RP.DisAmount = accountRecord.AssetAmount;
                                            RP.SumAmount = accountRecord.AssetAmount;
                                            RP.RP_GUID = Guid.NewGuid().ToString();
                                            break;
                                        case "应付利息":
                                            RP.Record = "未销账";
                                            RP.InvType = "筹资活动付款";
                                            RP.InvTypeDts = "偿付利息所支付的款";
                                            RP.CFItemGuid = "5BDE7476-F268-4A62-8CC3-D426D51E253D";
                                            RP.DisAmount = accountRecord.AssetAmount;
                                            RP.SumAmount = accountRecord.AssetAmount;
                                            RP.RP_GUID = Guid.NewGuid().ToString();
                                            break;
                                        case "应付职工薪酬":
                                            RP.Record = "未销账";
                                            RP.InvType = "经营活动付款";
                                            RP.InvTypeDts = "支付职工薪酬";
                                            RP.CFItemGuid = "70765251-FA58-432F-BCC5-122EF3581102";
                                            RP.DisAmount = accountRecord.AssetAmount;
                                            RP.SumAmount = accountRecord.AssetAmount;
                                            RP.RP_GUID = Guid.NewGuid().ToString();
                                            break;
                                        case "应交税费":
                                            RP.Record = "未销账";
                                            RP.InvType = "经营活动付款";
                                            RP.InvTypeDts = "支付的各项税费";
                                            RP.CFItemGuid = "E4F16AB4-8DFE-42E1-8A7F-0CB342CF9C73";
                                            RP.DisAmount = accountRecord.AssetAmount;
                                            RP.SumAmount = accountRecord.AssetAmount;
                                            RP.RP_GUID = Guid.NewGuid().ToString();
                                            if (accountRecord.DetailInvType == "增值税")
                                            {
                                                if (accountRecord.ThirdInvType == "未交税金")
                                                {
                                                    RP.DetailInvType = accountRecord.IETh_GUID;
                                                    RP.ThirdInvType = accountRecord.DetailInvType;
                                                }
                                            }

                                            if (accountRecord.ThirdInvType == "企业所得税")
                                            {
                                                RP.ThirdInvType = "所得税费用";
                                            }

                                            if (accountRecord.DetailInvType == "个人所得税")
                                            {
                                                RP.ThirdInvType = "个人所得税";
                                            }

                                            if (accountRecord.DetailInvType == "消费税" || accountRecord.DetailInvType == "营业税" || accountRecord.DetailInvType == "城市维护建设税"
                                                || accountRecord.DetailInvType == "资源税" || accountRecord.DetailInvType == "土地增值税" || accountRecord.DetailInvType == "城镇土地使用税"
                                                || accountRecord.DetailInvType == "房产税" || accountRecord.DetailInvType == "车船税" || accountRecord.DetailInvType == "教育费附加" || accountRecord.DetailInvType == "排污费")
                                            {
                                                RP.ThirdInvType = "营业税金及附加";
                                            }
                                            break;
                                    }
                                    RPList.Add(RP);
                                }
                                if (accountRecord.InvType == "主营业务成本" || accountRecord.InvType == "其他业务成本" || accountRecord.InvType == "销售费用" || accountRecord.InvType == "管理费用" || accountRecord.InvType == "财务费用" || accountRecord.InvType == "营业外支出")
                                {
                                    IE.Amount = accountRecord.AssetAmount;
                                    IE.SumAmount = accountRecord.AssetAmount;
                                    IE.InvType = accountRecord.InvType;
                                    IE.IEGroup = accountRecord.IEDA_GUID;
                                    IE.DisAmount = 0;
                                    totolAmount += accountRecord.AssetAmount;
                                    RPer = accountRecord.IETh_GUID;
                                    IE.IE_Flag = "E";
                                    IE.C_GUID = C_GUID;
                                    IE.Creator = Creator;
                                    IE.Currency = Currency;
                                    IE.IE_GUID = Guid.NewGuid().ToString();
                                    IE.AccountID = accountRecord.IE_GUID;
                                    IE.State = "已付";
                                    RP.SumAmount = accountRecord.AssetAmount;
                                    RP.DisAmount = 0;
                                    RP.RP_Flag = "P";
                                    RP.C_GUID = C_GUID;
                                    RP.Creator = Creator;
                                    RP.Currency = Currency;
                                    RP.RP_GUID = Guid.NewGuid().ToString();
                                    RP.AccountID = accountRecord.IE_GUID;
                                    RP.DetailInvType = accountRecord.IEDA_GUID;
                                    IE.RP_GUID = RP.RP_GUID;
                                    switch (accountRecord.InvType)
                                    {
                                        case "主营业务成本":
                                            if (accountRecord.DetailInvType!=null && accountRecord.DetailInvType.IndexOf("职工") != -1)
                                            {
                                                RP.Record = "已销账";
                                                RP.InvType = "经营活动付款";
                                                RP.InvTypeDts = "支付职工薪酬";
                                                RP.CFItemGuid = "70765251-FA58-432F-BCC5-122EF3581102";
                                            }
                                            else
                                            {
                                                RP.Record = "已销账";
                                                RP.InvType = "经营活动付款";
                                                RP.InvTypeDts = "购买商品、接受服务所支付的款";
                                                RP.CFItemGuid = "0526C862-F238-4301-A198-E7EC83A645D5";
                                            }
                                            break;
                                        case "其他业务成本":
                                            RP.Record = "已销账";
                                            RP.InvType = "经营活动付款";
                                            RP.InvTypeDts = "支付其他业务成本";
                                            RP.CFItemGuid = "DE7D81B9-680B-4011-A771-C8B327A549E7";
                                            break;
                                        case "销售费用":
                                            if (accountRecord.DetailInvType == "职工薪酬")
                                            {
                                                RP.Record = "已销账";
                                                RP.InvType = "经营活动付款";
                                                RP.InvTypeDts = "支付职工薪酬";
                                                RP.CFItemGuid = "70765251-FA58-432F-BCC5-122EF3581102";
                                            }
                                            else
                                            {
                                                RP.Record = "已销账";
                                                RP.InvType = "经营活动付款";
                                                RP.InvTypeDts = "支付销售费用";
                                                RP.CFItemGuid = "DE7D81B9-680B-4011-A771-C8B327A549E7";
                                            }
                                            break;
                                        case "管理费用":
                                            if (accountRecord.DetailInvType == "职工薪酬")
                                            {
                                                RP.Record = "已销账";
                                                RP.InvType = "经营活动付款";
                                                RP.InvTypeDts = "支付职工薪酬";
                                                RP.CFItemGuid = "70765251-FA58-432F-BCC5-122EF3581102";
                                            }
                                            else
                                            {
                                                RP.Record = "已销账";
                                                RP.InvType = "经营活动付款";
                                                RP.InvTypeDts = "支付管理费用";
                                                RP.CFItemGuid = "DE7D81B9-680B-4011-A771-C8B327A549E7";
                                            }
                                            break;
                                        case "财务费用":
                                            RP.Record = "已销账";
                                            RP.InvType = "经营活动付款";
                                            RP.InvTypeDts = "支付财务费用";
                                            RP.CFItemGuid = "DE7D81B9-680B-4011-A771-C8B327A549E7";
                                            break;
                                        case "营业外支出":
                                            RP.Record = "已销账";
                                            RP.InvType = "经营活动付款";
                                            RP.InvTypeDts = "支付营业外支出";
                                            RP.CFItemGuid = "DE7D81B9-680B-4011-A771-C8B327A549E7";
                                            break;
                                    }
                                    if (accountRecord.DetailInvType !=null && (accountRecord.DetailInvType == "职工薪酬" || accountRecord.DetailInvType.IndexOf("职工") != -1))
                                    {
                                        WageCostRecord = new T_WageCost();
                                        WageCostRecord.W_GUID = Guid.NewGuid().ToString();
                                        WageCostRecord.C_GUID = Session["CurrentCompanyGuid"].ToString();
                                        WageCostRecord.State = "已付";
                                        WageCostRecord.PayType = RpTypeID;
                                        WageCostRecord.Date = DateTime.Parse(Date);
                                        WageCostRecord.Profit_Name = EmployeeContent;
                                        WageCostRecord.Employee = "员工";
                                        WageCostRecord.Currency = Currency;
                                        WageCostRecord.Total = accountRecord.AssetAmount;
                                        WageCostRecord.Cash = accountRecord.AssetAmount;
                                        wageList.Add(WageCostRecord);
                                    }

                                    result1 = new RecPayRecordSvc().UpdVouIERPRecord(IE.IE_GUID, IE.RP_GUID, RP.Record, RP.RP_Flag, IE.C_GUID);
                                    if (result1)
                                    {
                                        RPList.Add(RP);
                                        IEList.Add(IE);
                                    }
                                }
                                if (strs1.Contains(accountRecord.InvType))
                                {
                                    DS.Amount = accountRecord.AssetAmount;
                                    DS.DisAmount = 0;
                                    RPer = accountRecord.IETh_GUID;
                                    DS.C_GUID = C_GUID;
                                    DS.Currency = Currency;
                                    DS.GUID = Guid.NewGuid().ToString();
                                    DS.AccountID = accountRecord.IE_GUID;
                                    DS.State = "已付";
                                    RP.SumAmount = accountRecord.AssetAmount;
                                    RP.DisAmount = 0;
                                    RP.RP_Flag = "P";
                                    RP.C_GUID = C_GUID;
                                    RP.Creator = Creator;
                                    RP.Currency = Currency;
                                    RP.RP_GUID = Guid.NewGuid().ToString();
                                    RP.AccountID = accountRecord.IE_GUID;
                                    switch (accountRecord.InvType)
                                    {

                                        case "预付账款":
                                            DS.InvType = "预付供应商账款";
                                            RP.Record = "已销账";
                                            RP.InvType = "经营活动付款";
                                            RP.InvTypeDts = "预付供应商账款";
                                            RP.CFItemGuid = "DE7D81B9-680B-4011-A771-C8B327A549E7";
                                            break;
                                        case "备用金":
                                            DS.InvType = "支付暂支借款";
                                            RP.Record = "已销账";
                                            RP.InvType = "经营活动付款";
                                            RP.InvTypeDts = "支付暂支借款";
                                            RP.CFItemGuid = "DE7D81B9-680B-4011-A771-C8B327A549E7";
                                            break;
                                        case "其他应收款":
                                            DS.InvType = "支付押金";
                                            RP.Record = "已销账";
                                            RP.InvType = "经营活动付款";
                                            RP.InvTypeDts = "支付押金";
                                            RP.CFItemGuid = "DE7D81B9-680B-4011-A771-C8B327A549E7";
                                            break;
                                        case "其他应付款":
                                            DS.InvType = "归还其它公司支付的押金";
                                            RP.Record = "已销账";
                                            RP.InvType = "经营活动付款";
                                            RP.InvTypeDts = "归还其它公司支付的押金";
                                            RP.CFItemGuid = "DE7D81B9-680B-4011-A771-C8B327A549E7";
                                            break;
                                        case "短期投资":
                                            DS.InvType = "短期投资支出";
                                            RP.Record = "已销账";
                                            RP.InvType = "投资活动付款";
                                            RP.InvTypeDts = "短期投资支出";
                                            RP.CFItemGuid = "049F1C6D-49EA-4E2D-93FD-2DABEBED666C";
                                            break;
                                        case "长期债券投资":
                                            DS.InvType = "长期债券投资支出";
                                            RP.Record = "已销账";
                                            RP.InvType = "投资活动付款";
                                            RP.InvTypeDts = "长期债券投资支出";
                                            RP.CFItemGuid = "049F1C6D-49EA-4E2D-93FD-2DABEBED666C";
                                            break;
                                        case "长期股权投资":
                                            DS.InvType = "长期股权投资支出";
                                            RP.Record = "已销账";
                                            RP.InvType = "投资活动付款";
                                            RP.InvTypeDts = "长期股权投资支出";
                                            RP.CFItemGuid = "049F1C6D-49EA-4E2D-93FD-2DABEBED666C";
                                            break;
                                        case "固定资产":
                                            DS.InvType = "购买固定资产所支付的款";
                                            RP.Record = "已销账";
                                            RP.InvType = "投资活动付款";
                                            RP.InvTypeDts = "购买固定资产所支付的款";
                                            RP.CFItemGuid = "EA46CFA0-5D41-4FF1-9BF8-9A36CB8F1F11";
                                            break;
                                        case "无形资产":
                                            DS.InvType = "购买无形资产所支付的款";
                                            RP.Record = "已销账";
                                            RP.InvType = "投资活动付款";
                                            RP.InvTypeDts = "购买无形资产所支付的款";
                                            RP.CFItemGuid = "EA46CFA0-5D41-4FF1-9BF8-9A36CB8F1F11";
                                            break;
                                        case "短期借款":
                                            DS.InvType = "归还短期借款所支付的款";
                                            RP.Record = "已销账";
                                            RP.InvType = "筹资活动付款";
                                            RP.InvTypeDts = "归还短期借款所支付的款";
                                            RP.CFItemGuid = "DD7BCD86-150E-4E62-B6CC-21EF341B41F1";
                                            break;
                                        case "长期借款":
                                            DS.InvType = "归还长期借款所支付的款";
                                            RP.Record = "已销账";
                                            RP.InvType = "筹资活动付款";
                                            RP.InvTypeDts = "归还长期借款所支付的款";
                                            RP.CFItemGuid = "DD7BCD86-150E-4E62-B6CC-21EF341B41F1";
                                            break;

                                    }
                                    result1 = new RecPayRecordSvc().UpdVouIERPRecord(DS.GUID, RP.RP_GUID, RP.Record, RP.RP_Flag, DS.C_GUID);
                                    if (result1)
                                    {
                                        RPList.Add(RP);
                                        DSList.Add(DS);
                                    }
                                }
                                break;

                            default:
                                break;
                        }


                        //switch (accountRecord.InvType) {
                        //    case "主营业务成本":
                        //    IE.InvType = "营业成本";   
                        //    IE.RPInvType = "经营活动付款";
                        //    IE.InvTypeDts = "购买商品、接受服务所支付的款";
                        //    IE.CFItemGuid = "0526C862-F238-4301-A198-E7EC83A645D5";
                        //        break;
                        //    case "销售费用":
                        //        IE.Profit_Name = "销售费用";
                        //        IE.RPInvType = "经营活动付款";
                        //        IE.InvTypeDts = "支付销售费用";
                        //        IE.CFItemGuid = "DE7D81B9-680B-4011-A771-C8B327A549E7";
                        //        break;
                        //    case "财务费用":
                        //        IE.Profit_Name = "财务费用";
                        //        IE.RPInvType = "经营活动付款";
                        //        IE.InvTypeDts = "支付财务费用";
                        //        IE.CFItemGuid = "DE7D81B9-680B-4011-A771-C8B327A549E7";
                        //        break;
                        //    case "管理费用":
                        //        IE.Profit_Name = "管理费用";
                        //        IE.RPInvType = "经营活动付款";
                        //        IE.InvTypeDts = "支付管理费用";
                        //        IE.CFItemGuid = "DE7D81B9-680B-4011-A771-C8B327A549E7";
                        //        break;
                        //    case "其他业务成本":
                        //        IE.Profit_Name = "其他业务成本";
                        //        IE.RPInvType = "经营活动付款";
                        //        IE.InvTypeDts = "支付其他业务成本";
                        //        IE.CFItemGuid = "DE7D81B9-680B-4011-A771-C8B327A549E7";
                        //        break;
                        //    case "营业外支出":
                        //        IE.Profit_Name = "营业外支出";
                        //        IE.RPInvType = "经营活动付款";
                        //        IE.InvTypeDts = "支付营业外支出";
                        //        IE.CFItemGuid = "DE7D81B9-680B-4011-A771-C8B327A549E7";
                        //        break;
                        //    case "应付账款":
                        //        break;
                        //    case "银行存款":
                        //        break;
                        //    case "库存现金":
                        //        break;
                        //    case "其他货币资金":
                        //        break;
                        //    default:
                        ////        break;
                        //}
                        EmployeeContent = accountRecord.InvType;
                        if (result1)
                            result = new VoucherSVC().UpdAccountRecord(accountRecord);
                    }
                }
                if (VoucherFlag == "YT" || VoucherFlag == "CT")
                {
                    TaxList.Add(tax);
                }
                if (!string.IsNullOrEmpty(WageCostRecord.W_GUID))
                {
                    wageList.Add(WageCostRecord);
                    WageCostRecord = null;
                }
                if (IEList.Count > 0)
                {
                    if (result)
                    {

                        foreach (T_IERecord ierecord in IEList)
                        {
                            if (totolTaxAmount != 0)
                            {
                                ierecord.SumAmount += Decimal.Parse(string.Format("{0:F2}", (ierecord.SumAmount / totolAmount * totolTaxAmount).ToString()));
                                ierecord.DisAmount = ierecord.SumAmount;
                            }
                            if (ierecord.IE_Flag == "E")
                            {
                                if (!string.IsNullOrEmpty(RPer))
                                {
                                    ierecord.RPer = RPer;
                                }
                                result = new IESvc().UpdVoucherFL(ierecord, "VTF");
                            }
                            if (ierecord.IE_Flag == "I")
                            {
                                if (!string.IsNullOrEmpty(RPer))
                                {
                                    ierecord.RPer = RPer;
                                }
                                if (!string.IsNullOrEmpty(DetailInvtype))
                                {
                                    ierecord.DetailInvtype = DetailInvtype;
                                }
                                result = new IESvc().UpdVoucherFL(ierecord, "VTF");
                            }
                        }
                    }
                }
                if (RPList.Count > 0)
                {
                    if (result)
                    {
                        foreach (T_RecPayRecord rprecord in RPList)
                        {
                            if (totolTaxAmount != 0)
                            {
                                rprecord.SumAmount += Decimal.Parse(string.Format("{0:F2}", (rprecord.SumAmount / totolAmount * totolTaxAmount).ToString()));
                            }
                            rprecord.PayCategoryID = PayCategoryID;
                            rprecord.DetailRPTypeID = DetailRPTypeID;
                            rprecord.BA_GUID = BA_GUID;
                            rprecord.SubjectName = SubjectName;
                            if (!string.IsNullOrEmpty(RPer))
                            {
                                rprecord.RPer = RPer;
                            }
                            result = new RecPayRecordSvc().UpdRecPayFLRecord(rprecord, "VTF");
                        }
                    }
                }
                if (DSList.Count > 0)
                {
                    if (result)
                    {
                        foreach (T_DeclareCostSpending dsrecord in DSList)
                        {

                            if (!string.IsNullOrEmpty(RPer))
                            {
                                dsrecord.RPer = RPer;
                            }
                            result = new DeclareCostSpendingSvc().UpdPayDSFL(dsrecord, "VTF");
                        }
                    }
                }
                if (DCList.Count > 0)
                {
                    if (result)
                    {
                        foreach (T_DeclareCustomer dcrecord in DCList)
                        {

                            if (!string.IsNullOrEmpty(RPer))
                            {
                                dcrecord.RPer = RPer;
                            }
                            result = new DeclareCustomerSvc().UpdPayDCFL(dcrecord, "VTF");
                        }
                    }
                }
                if (wageList.Any())
                {
                    if (result)
                    {
                        foreach (T_WageCost dcrecord in wageList)
                        {
                            result = new IESvc().UpdVouchWageCost(dcrecord, "VTF", AccountID);
                        }
                    }
                }
                if (TaxList.Count > 0)
                {
                    if (result)
                    {
                        foreach (T_TaxSettlement taxrecord in TaxList)
                        {
                            if (VoucherFlag == "YT") { result = new IESvc().AddVTFLAddSalesTaxRecord(taxrecord, "VTF"); }
                            if (VoucherFlag == "CT") { result = new IESvc().AddATFCTProvisionRecord(taxrecord, "VTF"); }

                        }
                    }
                }
                if (OtherTaxList.Any())
                {
                    if (result)
                    {
                        foreach (T_TaxSettlement taxrecord in OtherTaxList)
                        {
                            result = new IESvc().AddOtherTaxInfo(taxrecord);
                        }
                    }
                }
                if (result)
                {
                    scope.Complete();
                }
            }
            if (result)
            {
                msg = General.Resource.Common.Success;
            }
            else
            {
                msg = General.Resource.Common.Failed;
            }
            return string.Format("{{\"Result\":{0},\"Msg\":\"{1}\"}}"
               , result.ToString().ToLower(), msg);

        }

        public string UpdPaymethod(List<T_PayMethod> PayMethodList)
        {
            bool result = false;
            string msg = string.Empty;
            foreach (T_PayMethod payMethod in PayMethodList)
            {
                result = new VoucherSVC().UpdPaymethod(payMethod);
            }
            if (result)
            {
                msg = General.Resource.Common.Success;
            }
            else
            {
                msg = General.Resource.Common.Failed;
            }
            return string.Format("{{\"Result\":{0},\"Msg\":\"{1}\"}}"
               , result.ToString().ToLower(), msg);
        }
    }
}
