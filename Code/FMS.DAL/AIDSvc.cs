
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
        /// 更新间接物料采购记录
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public bool UpdIndirectMaterialPurchasingRecord(T_AIDRecord form)
        {
            form.AID_Flag = "I";
            return UpdAIDRecord(form);
        }

        public bool UpdTemporyRecord(T_AIDRecord form)
        {
            form.AID_Flag = "T";
            return UpdAIDRecord(form);
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
            try {
                dh.strCmd = "SP_UpdAIDRecord";
                dh.AddPare("@GUID", SqlDbType.NVarChar, 40, rec.GUID);
                dh.AddPare("@AID_Flag", SqlDbType.NVarChar, 1, rec.AID_Flag);
                dh.AddPare("@InvType", SqlDbType.NVarChar, 40, rec.InvType);
                dh.AddPare("@Description", SqlDbType.NVarChar, 40, rec.Description);
                dh.AddPare("@DepreciationPeriod", SqlDbType.Int, 40, rec.DepreciationPeriod);
                dh.AddPare("@RPer", SqlDbType.NVarChar, 40, rec.RPer);
                dh.AddPare("@C_GUID", SqlDbType.NVarChar, 40, rec.C_GUID);
                if (!rec.Date.Equals(DateTime.MinValue))
                {
                    dh.AddPare("@Date", SqlDbType.DateTime, 0, rec.Date);
                }
                dh.AddPare("@Amount", SqlDbType.Decimal, 0, rec.Amount);
                dh.AddPare("@SurplusValue", SqlDbType.Decimal, 0, rec.SurplusValue);
                dh.AddPare("@Remark", SqlDbType.NVarChar, 200, rec.Remark);
                dh.AddPare("@Currency", SqlDbType.NVarChar, 20, rec.Currency);
                dh.AddPare("@State", SqlDbType.NVarChar, 20, rec.State);
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
                    dh.AddPare("@C_GUID", SqlDbType.NVarChar, 40, rec.C_GUID);
                    dh.NonQuery();
                }
                else if (rec.AID_Flag == "D")
                {
                    dh.strCmd = "SP_UpdPaymentDeclareCostSpending";
                    dh.AddPare("@GUID", SqlDbType.NVarChar, 40, rec.GUID);
                    dh.AddPare("@InvType", SqlDbType.NVarChar, 40, "直接物料采购");
                    dh.AddPare("@RPer", SqlDbType.NVarChar, 40, rec.RPer);
                    if (!rec.Date.Equals(DateTime.MinValue))
                    {
                        dh.AddPare("@Date", SqlDbType.DateTime, 0, rec.Date);
                    }
                    dh.AddPare("@Amount", SqlDbType.Decimal, 0, rec.Amount);
                    dh.AddPare("@Remark", SqlDbType.NVarChar, 200, rec.Remark);
                    dh.AddPare("@Currency", SqlDbType.NVarChar, 20, rec.Currency);
                    dh.AddPare("@State", SqlDbType.NVarChar, 40, "未付");
                    dh.AddPare("@C_GUID", SqlDbType.NVarChar, 40, rec.C_GUID);
                    dh.NonQuery();
                }
                else if (rec.AID_Flag == "I")
                {
                    dh.strCmd = "SP_UpdPaymentDeclareCostSpending";
                    dh.AddPare("@GUID", SqlDbType.NVarChar, 40, rec.GUID);
                    dh.AddPare("@InvType", SqlDbType.NVarChar, 40, "间接物料采购");
                    dh.AddPare("@RPer", SqlDbType.NVarChar, 40, rec.RPer);
                    if (!rec.Date.Equals(DateTime.MinValue))
                    {
                        dh.AddPare("@Date", SqlDbType.DateTime, 0, rec.Date);
                    }
                    dh.AddPare("@Amount", SqlDbType.Decimal, 0, rec.Amount);
                    dh.AddPare("@Remark", SqlDbType.NVarChar, 200, rec.Remark);
                    dh.AddPare("@Currency", SqlDbType.NVarChar, 20, rec.Currency);
                    dh.AddPare("@State", SqlDbType.NVarChar, 40, "未付");
                    dh.AddPare("@C_GUID", SqlDbType.NVarChar, 40, rec.C_GUID);
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
        /// <returns></returns>
        public List<T_AIDRecord> GetAssetPurchaseList(string C_GUID, int page, int rows, out int count, string dateBegin, string dateEnd, string customer, string grp, string state)
        {
            return GetAllList(page, rows, out count, C_GUID, dateBegin, dateEnd, customer, grp, "A", state);
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
        public List<T_AIDRecord> GetIndirectMaterialPurchasingList(string C_GUID, int page, int rows, out int count, string dateBegin, string dateEnd, string customer, string grp, string state)
        {
            return GetAllList(page, rows, out count, C_GUID, dateBegin, dateEnd, customer, grp, "I", state);
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
        public List<T_AIDRecord> GetDirectMaterialPurchasingList(string C_GUID, int page, int rows, out int count, string dateBegin, string dateEnd, string customer, string grp, string state)
        {
            return GetAllList(page, rows, out count, C_GUID, dateBegin, dateEnd, customer, grp, "D", state);
        }

        public List<T_AIDRecord> GetTemporyList(string C_GUID,out int count)
        {
            return GetAllList("T", C_GUID,out count);
        }

        /// <summary>
        /// 获取采购列表
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
        public List<T_AIDRecord> GetAllList(int pageIndex, int pageSize, out int count, string C_GUID, string dateBegin, string dateEnd, string customer, string grp, string flag, string state)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetAIDRecord";
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
            dh.AddPare("@Grp", SqlDbType.NVarChar, 20, grp);
            dh.AddPare("@State", SqlDbType.NVarChar, 40, state);
            List<T_AIDRecord> result = new List<T_AIDRecord>();
            result = dh.Reader<T_AIDRecord>();
            count = dh.GetParaValue<int>("@Count");
            return result;
        }

        private List<T_AIDRecord> GetAllList(string flag,string C_GUID,out int count)
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
            dh.strCmd = "GetAIDRecord";
            dh.AddPare("@ID", SqlDbType.NVarChar, 40, id);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            return dh.Reader<T_AIDRecord>().FirstOrDefault();
        }

        /// <summary>
        /// 更改状态
        /// </summary>
        /// <param name="id">收入纪录标识</param>
        /// <returns></returns>
        public bool UpdAIDState(string flag, string id, string state)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_UpdAIDState";
            dh.AddPare("@Flag", SqlDbType.NVarChar, 1, flag);
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
        public bool DelAIDIERecord(string id)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_DelAIDRecord";
            dh.AddPare("@ID", SqlDbType.NVarChar, 40, id);
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

        public List<T_AIDRecord> GetList(int pageIndex, int pageSize, out int count, string C_GUID, string dateBegin, string dateEnd, string flag)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetAIDRecordUp";
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
            List<T_AIDRecord> result = new List<T_AIDRecord>();
            result = dh.Reader<T_AIDRecord>();
            count = dh.GetParaValue<int>("@Count");
            return result;
        }

    }
}
