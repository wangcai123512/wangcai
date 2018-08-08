
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using FMS.Model;

namespace FMS.DAL
{
    public class AIDSvc
    {
        /// <summary>
        /// 更新资产采购记录
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public bool UpdAssetPurchaseRecord(T_AIDRecord form)
        {
            form.AID_Flag = "A";
            return UpdAIDRecord(form);
        }

        public bool UpdateFixAssetAmount(FixAssetsAmount fixAmount)
        {
            return UpdateFixedAssets(fixAmount);
        }
        /// <summary>
        /// 转售资产采购记录
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public bool UpdResaleAssetPurchaseRecord(T_AIDRecord form)
        {
            form.AID_Flag = "A";
            return UpdResaleAIDRecord(form);
        }

        /// <summary>
        /// 更新直接物料采购记录
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public bool UpdDirectMaterialPurchasingRecord(T_AIDRecord form)
        {
            form.AID_Flag = "D";
            return UpdAIDRecord(form);
        }
        /// <summary>
        /// 转售直接物料采购记录
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public bool UpdResaleDirectMaterialPurchasingRecord(T_AIDRecord form)
        {
            form.AID_Flag = "D";
            return UpdResaleAIDRecord(form);
        }

        /// <summary>
        /// 更新间接物料采购记录
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public bool UpdIndirectMaterialPurchasingRecord(T_AIDRecord form)
        {
            form.AID_Flag = "I";
            return UpdAIDRecord(form);
        }

        /// <summary>
        /// 转售物料采购记录
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public bool UpdResaleIndirectMaterialPurchasingRecord(T_AIDRecord form)
        {
            form.AID_Flag = "I";
            return UpdResaleAIDRecord(form);
        }
        public bool UpdTemporyRecord(T_AIDRecord form)
        {
            form.AID_Flag = "T";
            return UpdAIDRecord(form);
        }

        private bool UpdateFixedAssets(FixAssetsAmount rec)
        {
            DBHelper dh = new DBHelper();
            dh.BeginTran();
            try
            {
                dh.strCmd = "SP_UpdFixAmount";
                dh.AddPare("@C_GUID", SqlDbType.NVarChar, 40, rec.C_GUID);
                dh.AddPare("@FixedAssets", SqlDbType.Decimal, 0, rec.FixedAssets);
                dh.AddPare("@AccumulatedDepreciation", SqlDbType.Decimal, 0, rec.AccumulatedDepreciation);
                dh.AddPare("@IntangibleAssets", SqlDbType.Decimal, 0, rec.IntangibleAssets);
                dh.AddPare("@AccumulatedAmortization", SqlDbType.Decimal, 0, rec.AccumulatedAmortization);
                dh.AddPare("@DeferredExpense", SqlDbType.Decimal, 0, rec.DeferredExpense);
                dh.NonQuery();
                dh.CleanPara();

                dh.CommitTran();
                return true;
            }
            catch
            {
                dh.RollBackTran();
                return false;
            }

        }
        /// <summary>
        /// 更新采购记录
        /// </summary>
        /// <param name="rec"></param>
        /// <returns></returns>
        private bool UpdAIDRecord(T_AIDRecord rec)
        {
            DBHelper dh = new DBHelper();
            dh.BeginTran();
            try
            {
                dh.strCmd = "SP_UpdAIDRecord";
                dh.AddPare("@GUID", SqlDbType.NVarChar, 40, rec.GUID);
                
                dh.AddPare("@Description", SqlDbType.NVarChar, 100, rec.Description);
                //dh.AddPare("@DepreciationPeriod", SqlDbType.Int, 40, rec.DepreciationPeriod);
                dh.AddPare("@RPer", SqlDbType.NVarChar, 40, rec.RPer);
                //添加资产分类和物料类别的子类别
                dh.AddPare("@InvType", SqlDbType.VarChar, 50, rec.InvType);//类别
                dh.AddPare("@SonInvType", SqlDbType.VarChar, 50, rec.SonInvType);//子类别
                dh.AddPare("@MaterielManage", SqlDbType.VarChar, 50, rec.MaterielManage);//子类别
                dh.AddPare("@C_GUID", SqlDbType.NVarChar, 40, rec.C_GUID);
                if (!rec.Date.Equals(DateTime.MinValue))
                {
                    dh.AddPare("@Date", SqlDbType.DateTime, 0, rec.Date);
                }
                dh.AddPare("@Amount", SqlDbType.Decimal, 0, rec.Amount);
                dh.AddPare("@MaterialNumber", SqlDbType.NVarChar, 40, rec.MaterialNumber);
                dh.AddPare("@SurplusValue", SqlDbType.Decimal, 0, rec.SurplusValue);
                dh.AddPare("@Remark", SqlDbType.NVarChar, 200, rec.Remark);
                dh.AddPare("@Currency", SqlDbType.NVarChar, 40, rec.Currency);
                dh.AddPare("@State", SqlDbType.NVarChar, 40, rec.State);
                dh.AddPare("@CostType", SqlDbType.VarChar, 1, rec.CostType);
                dh.AddPare("@Business_GUID", SqlDbType.NVarChar, 40, rec.Business_GUID);
                dh.AddPare("@SubBusiness_GUID", SqlDbType.NVarChar, 40, rec.SubBusiness_GUID);
              
               
                dh.NonQuery();
                dh.CleanPara();
                if (rec.AID_Flag == "A")
                {
                    dh.strCmd = "SP_UpdPaymentDeclareCostSpending";
                    dh.AddPare("@GUID", SqlDbType.NVarChar, 40, rec.GUID);
                    dh.AddPare("@InvType", SqlDbType.NVarChar, 40, "资产采购");
                    dh.AddPare("@RPer", SqlDbType.NVarChar, 40, rec.RPer);
                    if (!rec.Date.Equals(DateTime.MinValue))
                    {
                        dh.AddPare("@Date", SqlDbType.DateTime, 0, rec.Date);
                    }
                    dh.AddPare("@Amount", SqlDbType.Decimal, 0, rec.Amount);
                    dh.AddPare("@Remark", SqlDbType.NVarChar, 200, rec.Remark);
                    dh.AddPare("@Currency", SqlDbType.NVarChar, 20, rec.Currency);
                    dh.AddPare("@State", SqlDbType.NVarChar, 40, "未付");
                    dh.AddPare("@Record", SqlDbType.NVarChar, 40, "未记录");
                    dh.AddPare("@C_GUID", SqlDbType.NVarChar, 40, rec.C_GUID);
                    dh.AddPare("@Profit_GUID", SqlDbType.NVarChar, 40, "B1F44906-51D6-47F4-B6EC-7B678B5E7CD5");
                    dh.AddPare("@Business_GUID", SqlDbType.NVarChar, 40, rec.Business_GUID);
                    dh.AddPare("@SubBusiness_GUID", SqlDbType.NVarChar, 40, rec.SubBusiness_GUID);
                    dh.NonQuery();
                }
                else if (rec.AID_Flag == "D")
                {
                    dh.strCmd = "SP_UpdPaymentDeclareCostSpending";
                    dh.AddPare("@GUID", SqlDbType.NVarChar, 40, rec.GUID);
                    dh.AddPare("@InvType", SqlDbType.NVarChar, 40, "物料采购");
                    dh.AddPare("@RPer", SqlDbType.NVarChar, 40, rec.RPer);
                    if (!rec.Date.Equals(DateTime.MinValue))
                    {
                        dh.AddPare("@Date", SqlDbType.DateTime, 0, rec.Date);
                    }
                    dh.AddPare("@Amount", SqlDbType.Decimal, 0, rec.Amount);
                    dh.AddPare("@Remark", SqlDbType.NVarChar, 200, rec.Remark);
                    dh.AddPare("@Currency", SqlDbType.NVarChar, 20, rec.Currency);
                    dh.AddPare("@State", SqlDbType.NVarChar, 40, "未付");
                    dh.AddPare("@Record", SqlDbType.NVarChar, 40, "未记录");
                    dh.AddPare("@C_GUID", SqlDbType.NVarChar, 40, rec.C_GUID);
                    dh.AddPare("@voucherNo", SqlDbType.NVarChar, 40, rec.Remark);
                    dh.AddPare("@Profit_GUID", SqlDbType.NVarChar, 40, "41C968F2-7D51-4F9F-83B6-EC0F4381ECD0");
                    dh.AddPare("@Business_GUID", SqlDbType.NVarChar, 40, rec.Business_GUID);
                    dh.AddPare("@SubBusiness_GUID", SqlDbType.NVarChar, 40, rec.SubBusiness_GUID);
                    dh.NonQuery();
                }
                else if (rec.AID_Flag == "I")
                {
                    dh.strCmd = "SP_UpdPaymentDeclareCostSpending";
                    dh.AddPare("@GUID", SqlDbType.NVarChar, 40, rec.GUID);
                    dh.AddPare("@InvType", SqlDbType.NVarChar, 40, "物料采购");
                    dh.AddPare("@RPer", SqlDbType.NVarChar, 40, rec.RPer);
                    if (!rec.Date.Equals(DateTime.MinValue))
                    {
                        dh.AddPare("@Date", SqlDbType.DateTime, 0, rec.Date);
                    }
                    dh.AddPare("@Amount", SqlDbType.Decimal, 0, rec.Amount);
                    dh.AddPare("@Remark", SqlDbType.NVarChar, 200, rec.Remark);
                    dh.AddPare("@Currency", SqlDbType.NVarChar, 20, rec.Currency);
                    dh.AddPare("@State", SqlDbType.NVarChar, 40, "未付");
                    dh.AddPare("@Record", SqlDbType.NVarChar, 40, "未记录");
                    dh.AddPare("@C_GUID", SqlDbType.NVarChar, 40, rec.C_GUID);
                    dh.AddPare("@Profit_GUID", SqlDbType.NVarChar, 40, "41C968F2-7D51-4F9F-83B6-EC0F4381ECD0");
                    dh.AddPare("@Business_GUID", SqlDbType.NVarChar, 40, rec.Business_GUID);
                    dh.AddPare("@SubBusiness_GUID", SqlDbType.NVarChar, 40, rec.SubBusiness_GUID);
                    dh.NonQuery();
                }
                dh.CommitTran();
                return true;
            }
            catch
            {
                dh.RollBackTran();
                return false;
            }
        }

        /// <summary>
        /// 转售采购记录
        /// </summary>
        /// <param name="rec"></param>
        /// <returns></returns>
        private bool UpdResaleAIDRecord(T_AIDRecord rec)
        {
            DBHelper dh = new DBHelper();
            dh.BeginTran();
            try
            {
                dh.strCmd = "SP_UpdResaleAIDRecord";        
                dh.AddPare("@GUID", SqlDbType.NVarChar, 40, rec.GUID);
                dh.AddPare("@C_GUID", SqlDbType.NVarChar, 40, rec.C_GUID);
                //转售的实际金额(收入金额)
                dh.AddPare("@ResaleActualAmount", SqlDbType.Decimal, 0, rec.ResaleActualAmount);
                //这里的AID——Flag是指IE_Record表中的IE_Flag
                dh.AddPare("@AID_Flag", SqlDbType.NVarChar, 1, "I");
                dh.AddPare("@InvType", SqlDbType.NVarChar, 40, rec.InvType);
                dh.AddPare("@RPer", SqlDbType.NVarChar, 40, rec.RPer);
                //当库存金额等于使用金额的时候状态修改为已使用
                dh.AddPare("@Amount", SqlDbType.Decimal, 0, rec.Amount);//使用金额
                dh.AddPare("@InventoryAmount", SqlDbType.Decimal, 0, rec.InventoryAmount);//库存金额
                dh.AddPare("@Business_GUID", SqlDbType.NVarChar, 40, rec.Business_GUID);
                dh.AddPare("@SubBusiness_GUID", SqlDbType.NVarChar, 40, rec.SubBusiness_GUID);
                
                if (rec.Amount >= rec.InventoryAmount)
                {
                    dh.AddPare("@State", SqlDbType.NVarChar, 40, "已转售");
                }
                else
                {
                    dh.AddPare("@State", SqlDbType.NVarChar, 40, "库存");
                }           
                string GUIDTW = Guid.NewGuid().ToString();
                dh.AddPare("@GUIDTW", SqlDbType.NVarChar, 40, GUIDTW);
                dh.AddPare("@ResaleNumber", SqlDbType.NVarChar, 40, rec.ResaleNumber);
                //dh.AddPare("@IE_GUID", SqlDbType.NVarChar, 40, rec.IE_GUID);
                dh.AddPare("@Inventory_Number", SqlDbType.NVarChar, 40, rec.Inventory_Number);
                dh.AddPare("@Creator", SqlDbType.NVarChar, 40, rec.Creator);
                dh.AddPare("@AffirmDate", SqlDbType.DateTime, 0, rec.AffirmDate);//收入确认日期
                dh.AddPare("@Date", SqlDbType.DateTime, 0, rec.Date);//收入截止日期
                dh.AddPare("@Pnumber", SqlDbType.NVarChar, 40, rec.Pnumber);
                dh.AddPare("@Currency", SqlDbType.NVarChar, 40, rec.Currency);
                dh.AddPare("@OriginalAmount", SqlDbType.Decimal, 0, rec.OriginalAmount);
                dh.AddPare("@TaxationType", SqlDbType.NVarChar, 40, rec.TaxationType);
                dh.AddPare("@TaxationAmount", SqlDbType.Decimal, 0, rec.TaxationAmount);
                dh.AddPare("@SumAmount", SqlDbType.Decimal, 0, rec.ResaleActualAmount + rec.TaxationAmount);
                dh.AddPare("@Remark", SqlDbType.NVarChar, 200, rec.Remark);
                dh.AddPare("@Description", SqlDbType.NVarChar, 100, rec.Description);
                dh.AddPare("@States", SqlDbType.NVarChar, 40, rec.StateIE);
                dh.AddPare("@CostType", SqlDbType.VarChar, 1, rec.CostType);
                dh.AddPare("@Detailed_Categories", SqlDbType.NVarChar, 40, rec.Detailed_Categories);
                dh.NonQuery();
                dh.CleanPara();

                dh.CommitTran();
                return true;
            }
            catch
            {
                dh.RollBackTran();
                return false;
            }
        }


        /// <summary>
        /// 获取资产采购列表
        /// </summary>
        /// <param name="C_GUID"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="count"></param>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <param name="customer"></param>
        /// <param name="grp"></param>
        /// /// <param name="grp"></param>
        /// <returns></returns>
        public List<T_AIDRecord> GetAssetPurchaseList(string C_GUID, int page, int rows, out int count, string dateBegin, string dateEnd, string customer, string AssetType, string Type, string TypeSub, string MaterielManage, string state, string business_GUID, string subBusiness_GUID)
        {
            return GetAssetAllList(page, rows, out count, C_GUID, dateBegin, dateEnd, customer, AssetType, Type, TypeSub, MaterielManage,"A", state, business_GUID, subBusiness_GUID);
            //获取资产列表和获取直接物料与间接物料不同

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="C_GUID"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="count"></param>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <param name="customer"></param>
        /// <param name="grp"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public List<T_AIDRecord> GetIndirectMaterialPurchasingList(string C_GUID, int page, int rows, out int count, string dateBegin, string dateEnd, string customer, string Type, string TypeSub, string MaterielManage, string state, string business_GUID, string subBusiness_GUID)
        {
            return GetAllList(page, rows, out count, C_GUID, dateBegin, dateEnd, customer, Type, TypeSub ,MaterielManage,"I", state, business_GUID, subBusiness_GUID);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="C_GUID"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="count"></param>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <param name="customer"></param>
        /// <param name="grp"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public List<T_AIDRecord> GetDirectMaterialPurchasingList(string C_GUID, int page, int rows, out int count, string dateBegin, string dateEnd, string customer, string Type, string TypeSub,string MaterielManage, string state, string business_GUID, string subBusiness_GUID)
        {
            return GetAllList(page, rows, out count, C_GUID, dateBegin, dateEnd, customer, Type, TypeSub, MaterielManage,"D", state, business_GUID, subBusiness_GUID);
        }
        public List<T_AIDRecord> GetTemporyList(string C_GUID, out int count)
        {
            return GetAllList("T", C_GUID, out count);
        }

        /// <summary>
        /// 获取直接和间接采购列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <param name="C_GUID"></param>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <param name="customer"></param>
        /// <param name="grp"></param>
        /// <param name="flag"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public List<T_AIDRecord> GetAllList(int pageIndex, int pageSize, out int count, string C_GUID, string dateBegin, string dateEnd, string customer, string Type, string TypeSub,string MaterielManage, string flag, string state, string business_GUID, string subBusiness_GUID)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetAIDRecord";
            //Flag本为AID_Record的Aid_Flag  现在为类别表中的Flag
            dh.AddPare("@Flag", SqlDbType.NVarChar, 1, flag); 

            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@PageIndex", SqlDbType.Int, 0, pageIndex);
            dh.AddPare("@PageSize", SqlDbType.Int, 0, pageSize);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            if (!string.IsNullOrEmpty(dateBegin))
            {
                dh.AddPare("@DateBegin", SqlDbType.DateTime, 0, DateTime.Parse(dateBegin));
            }
            if (!string.IsNullOrEmpty(dateEnd))
            {
                dh.AddPare("@DateEnd", SqlDbType.DateTime, 0, DateTime.Parse(dateEnd));
            }
            dh.AddPare("@Customer", SqlDbType.NVarChar, 40, customer);
            dh.AddPare("@Type", SqlDbType.NVarChar, 40, Type);
            dh.AddPare("@TypeSub", SqlDbType.NVarChar, 40, TypeSub);
            dh.AddPare("@MaterielManage", SqlDbType.NVarChar, 40, MaterielManage);
            dh.AddPare("@State", SqlDbType.NVarChar, 40, state);
            dh.AddPare("@Business_GUID", SqlDbType.NVarChar, 40, business_GUID);
            dh.AddPare("@SubBusiness_GUID", SqlDbType.NVarChar, 40, subBusiness_GUID);
            List<T_AIDRecord> result = new List<T_AIDRecord>();
            result = dh.Reader<T_AIDRecord>();
            count = dh.GetParaValue<int>("@Count");
            return result;
        }
        /// <summary>
        /// 获取资产采购列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <param name="C_GUID"></param>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <param name="customer"></param>
        /// <param name="AssetType"></param>
        /// <param name="Type"></param>
        /// <param name="TypeSub"></param>
        /// <param name="flag"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public List<T_AIDRecord> GetAssetAllList(int pageIndex, int pageSize, out int count, string C_GUID, string dateBegin, string dateEnd, string customer, string AssetType, string Type, string TypeSub, string MaterielManage, string flag, string state, string business_GUID, string subBusiness_GUID)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetAssetAIDRecord";

            //Flag本为AID_Record的Aid_Flag  现在为类别表中的Flag
            dh.AddPare("@Flag", SqlDbType.NVarChar, 1, flag);          
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@PageIndex", SqlDbType.Int, 0, pageIndex);
            dh.AddPare("@PageSize", SqlDbType.Int, 0, pageSize);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            if (!string.IsNullOrEmpty(dateBegin))
            {
                dh.AddPare("@DateBegin", SqlDbType.DateTime, 0, DateTime.Parse(dateBegin));
            }
            if (!string.IsNullOrEmpty(dateEnd))
            {
                dh.AddPare("@DateEnd", SqlDbType.DateTime, 0, DateTime.Parse(dateEnd));
            }
            dh.AddPare("@Customer", SqlDbType.NVarChar, 40, customer);
            //现在应该读取类别表中类别
            dh.AddPare("@AssetType", SqlDbType.NVarChar, 40, AssetType);
            dh.AddPare("@Type", SqlDbType.NVarChar, 40, Type);
            dh.AddPare("@TypeSub", SqlDbType.NVarChar, 40, TypeSub);
            dh.AddPare("@MaterielManage", SqlDbType.NVarChar, 40, MaterielManage);
            
            dh.AddPare("@State", SqlDbType.NVarChar, 40, state);
            dh.AddPare("@Business_GUID", SqlDbType.NVarChar, 40, business_GUID);
            dh.AddPare("@SubBusiness_GUID", SqlDbType.NVarChar, 40, subBusiness_GUID);
            List<T_AIDRecord> result = new List<T_AIDRecord>();
            result = dh.Reader<T_AIDRecord>();
            count = dh.GetParaValue<int>("@Count");
            return result;
        }

        private List<T_AIDRecord> GetAllList(string flag, string C_GUID, out int count)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetAIDRecord";
            dh.AddPare("@Flag", SqlDbType.NVarChar, 1, flag);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            List<T_AIDRecord> result = new List<T_AIDRecord>();
            result = dh.Reader<T_AIDRecord>();
            count = dh.GetParaValue<int>("@Count");
            return result;
        }

        /// <summary>
        /// 获取收入\费用纪录
        /// </summary>
        /// <param name="id">收入\费用纪录标识</param>
        /// <param name="C_GUID">公司标识</param>
        /// <returns></returns>
        public T_AIDRecord GetAIDRecord(string id, string C_GUID)
        {
            DBHelper dh = new DBHelper();
            //dh.strCmd = "GetAIDRecord";
            dh.strCmd = "SP_GetAIDRecordDetail";
            dh.AddPare("@ID", SqlDbType.NVarChar, 40, id);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            return dh.Reader<T_AIDRecord>().FirstOrDefault();
        }
        /// <summary>
        /// 获取收入\费用纪录(包括公司信息)
        /// </summary>
        /// <param name="id">收入\费用纪录标识</param>
        /// <param name="C_GUID">公司标识</param>
        /// <returns>11.7 hdy新增</returns>
        public T_AIDRecord GetAIDRecordBusinessPartner(string id, string C_GUID)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetAIDRecordBusinessPartner";
            dh.AddPare("@ID", SqlDbType.NVarChar, 40, id);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            return dh.Reader<T_AIDRecord>().FirstOrDefault();
        }

        /// <summary>
        /// 更改状态
        /// </summary>
        /// <param name="id">收入纪录标识</param>
        /// <returns></returns>
        public bool UpdAIDState(string id, string state)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_UpdAIDState";
            dh.AddPare("@ID", SqlDbType.NVarChar, 40, id);
            dh.AddPare("@State", SqlDbType.NVarChar, 40, state);
            try
            {
                dh.NonQuery();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 删除采购记录
        /// </summary>
        /// <param name="id">收入\费用纪录标识</param>
        /// <param name="flag">收入\费用标识</param>
        /// <returns></returns>
        /// 

        public static int DelAIDIERecord(string id)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_DelAIDRecord";
            dh.AddPare("@ID", SqlDbType.NVarChar, 40, id);
            dh.AddPare("@result", SqlDbType.Int, ParameterDirection.Output, 0, null);

            dh.NonQuery();
            return dh.GetParaValue<int>("@result");
        }

        public List<T_AIDRecord> GetList(int pageIndex, int pageSize, out int count, string C_GUID, string dateBegin, string dateEnd, string flag)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetAIDRecordUp";
            dh.AddPare("@Flag", SqlDbType.NVarChar, 1, flag);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@PageIndex", SqlDbType.Int, 0, pageIndex);
            dh.AddPare("@PageSize", SqlDbType.Int, 0, pageSize);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            if (!string.IsNullOrEmpty(dateBegin))
            {
                dh.AddPare("@DateBegin", SqlDbType.DateTime, 0, DateTime.Parse(dateBegin));
            }
            if (!string.IsNullOrEmpty(dateEnd))
            {
                dh.AddPare("@DateEnd", SqlDbType.DateTime, 0, DateTime.Parse(dateEnd));
            }
            List<T_AIDRecord> result = new List<T_AIDRecord>();
            result = dh.Reader<T_AIDRecord>();
            count = dh.GetParaValue<int>("@Count");
            return result;
        }

        /// <summary>
        /// 获取类别列表
        /// </summary>
        /// <returns></returns>
        /// 
        public List<T_PTR> GetPurchasingTypeRecordList(int pageIndex, int pageSize, out int count, string C_GUID, string AID_Flag)
        {
            return GetPurchasingTypeRecordLists(out count, C_GUID, AID_Flag);

        }
        /// <summary>
        /// 获取物料列表
        /// </summary>
        /// <returns></returns>
        /// 
        public List<T_PTR> GetMaterielManageList(int pageIndex, int pageSize, out int count, string C_GUID, string AID_Flag)
        {
            return GetMaterielManageLists(out count, C_GUID, AID_Flag);

        }
        /// <summary>
        /// 获取类别列表
        /// </summary>
        /// <returns></returns>
        /// 
        public List<T_PTR> GetMaterielManageLists(out int count, string C_GUID, string AID_FLAG)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetMML";
            dh.AddPare("@Flag", SqlDbType.NVarChar, 1, AID_FLAG);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            List<T_PTR> result = new List<T_PTR>();
            result = dh.Reader<T_PTR>();

            count = dh.GetParaValue<int>("@Count");
            return result;
        }
        /// <summary>
        /// 获取类别列表
        /// </summary>
        /// <returns></returns>
        /// 
        public List<T_PTR> GetPurchasingTypeRecordLists(out int count, string C_GUID, string AID_FLAG)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetPTR";
            dh.AddPare("@Flag", SqlDbType.NVarChar, 1, AID_FLAG);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            List<T_PTR> result = new List<T_PTR>();
            result = dh.Reader<T_PTR>();

            count = dh.GetParaValue<int>("@Count");
            return result;
        }

        public bool UpdPurchasingTypeRecord(T_PTR ptr)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_UpdPTR";
            dh.AddPare("@AT_GUID", SqlDbType.NVarChar, 50, ptr.AT_GUID);
            dh.AddPare("@Flag", SqlDbType.NVarChar, 1, ptr.AID_FLAG);
            dh.AddPare("@AidTypeName", SqlDbType.NVarChar, 50, ptr.AidTypeName);
            dh.AddPare("@Asset_class", SqlDbType.NVarChar, 50, ptr.Asset_class);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, ptr.C_GUID);
            try
            {
                dh.NonQuery();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdPurchasingSubTypeRecord(T_PTR ptr)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_UpdPSTR";
            dh.AddPare("@AST_GUID", SqlDbType.NVarChar, 50, ptr.AST_GUID);
            dh.AddPare("@Depreciation_year", SqlDbType.NVarChar, 50, ptr.Depreciation_year);
            dh.AddPare("@AST_ParentAidType", SqlDbType.NVarChar, 50, ptr.AST_ParentAidType);
            dh.AddPare("@Remark", SqlDbType.NVarChar, 50, ptr.Remark);
            dh.AddPare("@ASTTypeName", SqlDbType.NVarChar, 50, ptr.ASTTypeName);
            dh.AddPare("@AID_FLAG", SqlDbType.NVarChar, 50, ptr.AID_FLAG);
            try
            {
                dh.NonQuery();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdMaterielManage(T_PTR ptr)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_UpdMM";
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, ptr.C_GUID);
            dh.AddPare("@MM_GUID", SqlDbType.NVarChar, 50, ptr.MM_GUID);
            dh.AddPare("@Parent", SqlDbType.NVarChar, 50, ptr.Parent);
            dh.AddPare("@Depreciation_year", SqlDbType.NVarChar, 50, ptr.Depreciation_year);
            dh.AddPare("@MM_Name", SqlDbType.NVarChar, 50, ptr.MM_Name);
            dh.AddPare("@Remark", SqlDbType.NVarChar, 50, ptr.Remark);
            dh.AddPare("@Asset_class", SqlDbType.NVarChar, 50, ptr.Asset_class);
            dh.AddPare("@MM_FLAG", SqlDbType.NVarChar, 50, ptr.MM_FLAG);
            try
            {
                dh.NonQuery();
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 删除类别前查询
        /// </summary>
        /// <returns></returns>
        public List<T_AIDRecord> GetDelPurchasingTypeRecord(out int count, string C_GUID, string AST_GUID, string AST_ParentAidType, string AID_FLAG, string AidTypeName)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetDPTR";
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@AST_GUID", SqlDbType.NVarChar, 50, AST_GUID);
            dh.AddPare("@AST_ParentAidType", SqlDbType.NVarChar, 50, AST_ParentAidType);
            dh.AddPare("@AID_FLAG", SqlDbType.NVarChar, 50, AID_FLAG);
            dh.AddPare("@AidTypeName", SqlDbType.NVarChar, 50, AidTypeName);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            List<T_AIDRecord> result = new List<T_AIDRecord>();
            result = dh.Reader<T_AIDRecord>();
            count = dh.GetParaValue<int>("@Count");
            return result;
        }
        /// <summary>
        /// 删除物料前查询
        /// </summary>
        /// <returns></returns>
        public List<T_AIDRecord> GetDelMaterielManage(out int count, string C_GUID, string MM_GUID, string MM_FLAG)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetDMM";
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@MM_GUID", SqlDbType.NVarChar, 50, MM_GUID);
            dh.AddPare("@MM_FLAG", SqlDbType.NVarChar, 50, MM_FLAG);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            List<T_AIDRecord> result = new List<T_AIDRecord>();
            result = dh.Reader<T_AIDRecord>();
            count = dh.GetParaValue<int>("@Count");
            return result;
        }
        /// <summary>
        /// 更新父类别前查询
        /// </summary>
        /// <returns></returns>
        public List<T_AIDRecord> GetUpdPurchasingTypeRecord(out int count, string AidTypeName, string AID_FLAG, string C_GUID)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetUPTR";
            dh.AddPare("@AidTypeName", SqlDbType.NVarChar, 50, AidTypeName);
            dh.AddPare("@AID_FLAG", SqlDbType.NVarChar, 50, AID_FLAG);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            List<T_AIDRecord> result = new List<T_AIDRecord>();
            result = dh.Reader<T_AIDRecord>();


            count = dh.GetParaValue<int>("@Count");
            return result;
        }
        /// <summary>
        /// 更新子类别前查询
        /// </summary>
        /// <returns></returns>
        public List<T_AIDRecord> GetUpdPurchasingSonTypeRecord(out int count, string AT_GUID, string AID_FLAG, string C_GUID, string ASTTypeName)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetSonPTR";
            dh.AddPare("@AT_GUID", SqlDbType.NVarChar, 50, AT_GUID);
            dh.AddPare("@ASTTypeName", SqlDbType.NVarChar, 50, ASTTypeName);
            dh.AddPare("@AID_FLAG", SqlDbType.NVarChar, 50, AID_FLAG);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            List<T_AIDRecord> result = new List<T_AIDRecord>();
            result = dh.Reader<T_AIDRecord>();
            count = dh.GetParaValue<int>("@Count");
            return result;
        }
        /// <summary>
        /// 更新物料前查询
        /// </summary>
        /// <returns></returns>
        public List<T_AIDRecord> GetUpdMaterielManage(out int count, string MM_FLAG, string C_GUID, string MM_Name)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetUpdMM";
            dh.AddPare("@MM_Name", SqlDbType.NVarChar, 50, MM_Name);
            dh.AddPare("@MM_FLAG", SqlDbType.NVarChar, 50, MM_FLAG);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            List<T_AIDRecord> result = new List<T_AIDRecord>();
            result = dh.Reader<T_AIDRecord>();
            count = dh.GetParaValue<int>("@Count");
            return result;
        }
        /// <summary>
        /// 添加Bom结构前查询
        /// </summary>
        /// <returns></returns>
        public List<T_ProductBom> GetUpdPurchasingBomTypeRecord(out int count, string C_GUID, string nodes, string subnodes, int nodelevel, string nodesid, string MaterielManage_GUID)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetBomPTR";
            dh.AddPare("@nodesid", SqlDbType.NVarChar, 50, nodesid);
            dh.AddPare("@nodes", SqlDbType.NVarChar, 50, nodes);
            dh.AddPare("@subnodes", SqlDbType.NVarChar, 50, subnodes);
            dh.AddPare("@nodelevel", SqlDbType.Int, 5, nodelevel);
            dh.AddPare("@MaterielManage_GUID", SqlDbType.NVarChar, 50, MaterielManage_GUID);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            List<T_ProductBom> result = new List<T_ProductBom>();
            result = dh.Reader<T_ProductBom>();
            count = dh.GetParaValue<int>("@Count");
            return result;
        }
        /// <summary>
        /// 删除类别
        /// </summary>
        /// <returns></returns>
        public bool DelPurchasingTypeRecord(T_PTR ptr)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_DelPTR";
            dh.AddPare("@AT_GUID", SqlDbType.NVarChar, 50, ptr.AT_GUID);
            dh.AddPare("@Flag", SqlDbType.NVarChar, 1, ptr.AID_FLAG);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, ptr.C_GUID);
            try
            {
                dh.NonQuery();
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 删除物料
        /// </summary>
        /// <returns></returns>
        public bool DelMaterielManage(T_PTR ptr)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_DelMM";
            dh.AddPare("@MM_GUID", SqlDbType.NVarChar, 50, ptr.MM_GUID);
            dh.AddPare("@Flag", SqlDbType.NVarChar, 1, ptr.MM_FLAG);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, ptr.C_GUID);
            try
            {
                dh.NonQuery();
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 获取父类
        /// </summary>
        /// <returns></returns>
        public List<T_PTR> GetParentAidType(T_PTR ptr)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetParentAidType";
            dh.AddPare("@Flag", SqlDbType.NVarChar, 1, ptr.AID_FLAG);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, ptr.C_GUID);
            return dh.Reader<T_PTR>();
        }
        /// <summary>
        /// 获取父类
        /// </summary>
        /// <returns></returns>
        public List<T_PTR> GetSubAidType(T_PTR ptr)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetSubAidType";
            dh.AddPare("@Parent", SqlDbType.NVarChar, 50, ptr.Parent);
            return dh.Reader<T_PTR>();
        }
        /// <summary>
        /// 获取公共父类
        /// </summary>
        /// <returns></returns>
        public List<T_PTR> GetComParentAidType(T_PTR ptr)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetComParentAidType";
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, ptr.C_GUID);
            return dh.Reader<T_PTR>();
        }
        /// <summary>
        /// 获取子类
        /// </summary>
        /// <returns></returns>
        public List<T_PTR> GetSonAidType(string parentId)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetSonAidType";
            dh.AddPare("@Parent_ID", SqlDbType.NVarChar, 50, parentId);

            return dh.Reader<T_PTR>();
        }
        /// <summary>
        /// 获取子类
        /// </summary>
        /// <returns></returns>
        public List<T_PTR> GetMMType(string C_GUID, string parentId)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetMMType";
            if (parentId == "null")
            {
                dh.AddPare("@Parent_ID", SqlDbType.NVarChar, 50, null);
            }else{
                dh.AddPare("@Parent_ID", SqlDbType.NVarChar, 50, parentId);
            }
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            return dh.Reader<T_PTR>();
        }
        /// <summary>
        /// 获取子类A
        /// </summary>
        /// <returns></returns>
        public List<T_PTR> GetMMTypeA(string C_GUID, string parentId)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetMMTypeA";
            if (parentId == "null")
            {
                dh.AddPare("@Parent_ID", SqlDbType.NVarChar, 50, null);
            }
            else
            {
                dh.AddPare("@Parent_ID", SqlDbType.NVarChar, 50, parentId);
            }
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            return dh.Reader<T_PTR>();
        }
        /// <summary>
        /// 获取子类NA
        /// </summary>
        /// <returns></returns>
        public List<T_PTR> GetMMTypeNA(string C_GUID, string parentId)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetMMTypeNA";
            if (parentId == "null")
            {
                dh.AddPare("@Parent_ID", SqlDbType.NVarChar, 50, null);
            }
            else
            {
                dh.AddPare("@Parent_ID", SqlDbType.NVarChar, 50, parentId);
            }
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            return dh.Reader<T_PTR>();
        }
        /// <summary>
        /// 通过子类获取分期
        /// </summary>
        /// <returns></returns>
        public List<T_PTR> GetPeriodAidType(string parentId)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetPeriodAidType";
            dh.AddPare("@Parent_ID", SqlDbType.NVarChar, 50, parentId);

            return dh.Reader<T_PTR>();



        }
        /// <summary>
        /// 获取资产分类
        /// </summary>
        /// <returns></returns>
        public List<T_PTR> GetAssetsAidType(string assetType,string companyId)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetAssetsAidType";
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, companyId);
            dh.AddPare("@Asset_class", SqlDbType.VarChar, 40, assetType);//资产分类类型
            
            return dh.Reader<T_PTR>();
        }


        /// <summary>
        /// 获取父类别标识
        /// </summary>
        /// <param name="name">类别名称</param>
        /// <returns></returns>
        public object GetTypeCatId(string C_GUID, string InvType)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetTypeCatId";
            dh.AddPare("@InvType", SqlDbType.NVarChar, 40, InvType);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            Object obj = dh.Scalar();
            return obj;
        }


        /// <summary>
        /// 获取资产父类别标识
        /// </summary>
        /// <param name="name">类别名称</param>
        /// <returns></returns>
        public object GetAssetTypeCatId(string C_GUID, string InvType, string AssetType)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetAssetTypeCatId";
            dh.AddPare("@InvType", SqlDbType.NVarChar, 40, InvType);
            dh.AddPare("@AssetType", SqlDbType.NVarChar, 40, AssetType);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            Object obj = dh.Scalar();
            return obj;
        }
        /// <summary>
        /// 获取子类别标识
        /// </summary>
        /// <param name="name">类别名称</param>
        /// <returns></returns>
        public object GetSubTypeCatId(string C_GUID, string SonInvType, string InvType)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetSubTypeCatId";
            dh.AddPare("@InvType", SqlDbType.NVarChar, 40, InvType);
            dh.AddPare("@SonInvType", SqlDbType.NVarChar, 40, SonInvType);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            Object obj = dh.Scalar();
            return obj;
        }

        /// <summary>
        /// 查询产品结构信息
        /// </summary>
        /// <param name="subTypeId">产品子类ID</param>
        /// <returns></returns>
        /// 
        public static DataTable GetProductBomList(string subTypeId)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetProductBom";
            dh.AddPare("@product_subtype_id", SqlDbType.VarChar, 40, subTypeId);
            DataSet ds = dh.Query();
            if (ds != null && ds.Tables.Count > 0)
            {
                return ds.Tables[0];
            }
            return null;
        }
        public List<T_PTR> GetParentnodesId(string AT_GUID)
        {
            DBHelper dh =new DBHelper();
            dh.strCmd = "SP_GetGetParentnodes";
            dh.AddPare("@Son_ID", SqlDbType.NVarChar, 50, AT_GUID);
            return dh.Reader<T_PTR>();
        
        }
        ///// <summary>
        ///// 更新物料数量
        ///// </summary>
        ///// <returns></returns>
        public bool UpdProductBomRecord(T_ProductBom form)
        {
            DBHelper dh = new DBHelper();
                dh.strCmd = "SP_UpdProductBom";
                dh.AddPare("@nodesid", SqlDbType.NVarChar, 40, form.nodesid);
                dh.AddPare("@nodes", SqlDbType.NVarChar, 40, form.nodes);
                dh.AddPare("@nodelevel", SqlDbType.NVarChar, 40, form.nodelevel);
                dh.AddPare("@tags", SqlDbType.Int, 0, form.tags);
                try
                {
                    dh.NonQuery();
                    return true;
                }
                catch
                {
                    return false;
                }

        }
        ///// <summary>
        ///// 提交节点
        ///// </summary>
        ///// <returns></returns>
        public bool SubmitProductBomRecord(T_ProductBom form)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_UpdProductBomDeatil";
            dh.AddPare("@MaterielManage_GUID", SqlDbType.NVarChar, 40, form.MaterielManage_GUID);
            dh.AddPare("@nodesid", SqlDbType.NVarChar, 40, form.nodesid);
            dh.AddPare("@subnodes", SqlDbType.NVarChar, 40, form.MaterielManage_GUID);
            dh.AddPare("@tags", SqlDbType.Int, 0, form.tags);
            try
            {
                dh.NonQuery();
                return true;
            }
            catch
            {
                return false;
            }

        }
        /// <summary>
        /// 获取ProductBom纪录
        /// </summary>
        /// <param name="id">标识</param>     
        /// <returns></returns>
        public T_ProductBom GetQueryBom(string nodesid)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetProductBomDetail";
            dh.AddPare("@ID", SqlDbType.NVarChar, 40, nodesid);
            return dh.Reader<T_ProductBom>().FirstOrDefault();
        }
        /// <summary>
        /// 删除ProductBom记录
        /// </summary>
        /// <param name="id">标识</param>
        /// <returns></returns>
        /// 

        public bool DelProductBomDetail(string id, int nodelevel)
        {
            DBHelper db = new DBHelper();
            db.strCmd = "SP_DelProductBomDetail";
            db.AddPare("@ID", SqlDbType.NVarChar, 40, id);
            db.AddPare("@nodelevel", SqlDbType.Int, 0, nodelevel);
            try
            {
                db.NonQuery();
                return true;
            }
            catch
            {
                return false;
            }
        }

    }

}