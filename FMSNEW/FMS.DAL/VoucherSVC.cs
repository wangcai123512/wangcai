using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using FMS.Model;

namespace FMS.DAL
{
    public class VoucherSVC
    {
        public bool UpdVoucher(T_Voucher voucher)
        {
            DBHelper dh = new DBHelper();
            dh.BeginTran();
            try
            {
                dh.strCmd = "SP_UpdVoucher";
                dh.AddPare("@GUID", SqlDbType.NVarChar, 40, voucher.GUID);
                dh.AddPare("@RP_GUID", SqlDbType.NVarChar, 40, voucher.RP_GUID);
                dh.AddPare("@C_GUID", SqlDbType.NVarChar, 40, voucher.C_GUID);
                dh.AddPare("@VoucherFlag", SqlDbType.NVarChar, 40, voucher.VoucherFlag);
                dh.AddPare("@State", SqlDbType.NVarChar, 40, voucher.State);
                dh.AddPare("@DisAmount", SqlDbType.Decimal, 0, voucher.DisAmount);
                dh.AddPare("@Amount", SqlDbType.Decimal, 0, voucher.Amount);
                dh.AddPare("@Date", SqlDbType.DateTime, 0, voucher.Date);
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
        public bool UpdAccountRecord(T_AccountRecord ar)
        {
            DBHelper dh = new DBHelper();
            dh.BeginTran();
            try
            {
                dh.strCmd = "SP_UpdAccountRecord";
                dh.AddPare("@GUID", SqlDbType.NVarChar, 40, ar.GUID);
                dh.AddPare("@IE_GUID", SqlDbType.NVarChar, 40, ar.IE_GUID);
                dh.AddPare("@IELA_GUID", SqlDbType.NVarChar, 40, ar.IELA_GUID);
                dh.AddPare("@Summary",SqlDbType.NVarChar,50,ar.Summary);
                dh.AddPare("@IEDA_GUID", SqlDbType.NVarChar, 40, ar.IEDA_GUID);
                dh.AddPare("@IETh_GUID", SqlDbType.NVarChar, 40, ar.IETh_GUID);
                dh.AddPare("@VoucherFlag", SqlDbType.NVarChar, 40, ar.VoucherFlag);
                dh.AddPare("@State", SqlDbType.NVarChar, 40, ar.State);
                dh.AddPare("@InvType", SqlDbType.NVarChar, 40, ar.InvType);
                dh.AddPare("@VoucherType", SqlDbType.NVarChar, 40, ar.VoucherType);
                dh.AddPare("@AssetAmount", SqlDbType.Decimal, 0, ar.AssetAmount);
                dh.AddPare("@DebtAmount", SqlDbType.Decimal, 0, ar.DebtAmount);
                dh.AddPare("@DisAmount", SqlDbType.Decimal, 0, ar.DisAmount);
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

        public bool UpdPaymethod(T_PayMethod pm)
        {
            DBHelper dh = new DBHelper();
            dh.BeginTran();
            try
            {
                dh.strCmd = "SP_UpdPaymethod";
                dh.AddPare("@GUID", SqlDbType.NVarChar, 40, pm.GUID);
                dh.AddPare("@RP_GUID", SqlDbType.NVarChar, 40, pm.RP_GUID);
                dh.AddPare("@RPDA_GUID", SqlDbType.NVarChar, 40, pm.RPDA_GUID);
                dh.AddPare("@RPLA_GUID", SqlDbType.NVarChar, 40, pm.RPLA_GUID);
                dh.AddPare("@RPTh_GUID", SqlDbType.NVarChar, 40, pm.RPTh_GUID);
                dh.AddPare("@VoucherType", SqlDbType.NVarChar, 40, pm.VoucherType);
                dh.AddPare("@AssetAmount", SqlDbType.Decimal, 0, pm.AssetAmount);
                dh.AddPare("@DebtAmount", SqlDbType.Decimal, 0, pm.DebtAmount);
                dh.AddPare("@Summary", SqlDbType.NVarChar, 40, pm.Summary);
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
    }
}
