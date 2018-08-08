using System.Collections.Generic;
using System.Data;
using FMS.Model;

namespace FMS.DAL
{
    public class AccountSvc
    {
        /// <summary>
        /// 获取总账科目
        /// </summary>
        /// <param name="c_id">公司标识</param>
        /// <param name="usable">可用标识</param>
        /// <returns></returns>
        public List<T_GeneralLedgerAccount> GetLedgerAccounts(string c_id, int? usable = null)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetLedgerAccounts";
            if (usable.HasValue)
            {
                dh.AddPare("@stat", SqlDbType.Bit, 0, usable.Value);
            }
            dh.AddPare("@c_id", SqlDbType.NVarChar, 40, c_id);
            return dh.Reader<T_GeneralLedgerAccount>();
        }

        /// <summary>
        /// 获取使用的总账科目
        /// </summary>
        /// <param name="c_id">公司标识</param>
        /// <returns></returns>
        public List<T_GeneralLedgerAccount> GetUserLedgerAccounts(string c_id)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetUserLedgerAccounts";
            dh.AddPare("@c_id", SqlDbType.NVarChar, 40, c_id);
            return dh.Reader<T_GeneralLedgerAccount>();
        }

        /// <summary>
        /// 更新总账科目使用状态
        /// </summary>
        /// <param name="accCodes">总账科目代码串</param>
        /// <param name="c_id">公司标识</param>
        /// <returns></returns>
        public bool UpdUsingLedgerAcc(string accCodes, string c_id)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_UpdUsingLedgerAcc";
            dh.AddPare("@c_id", SqlDbType.NVarChar, 40, c_id);
            dh.AddPare("@AccCodes", SqlDbType.NVarChar, 4000, accCodes);
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
        ///  更新总账科目
        /// </summary>
        /// <param name="acc">总账科目对象</param>
        /// <returns></returns>
        public bool UpdLedgerAcc(T_GeneralLedgerAccount acc)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_UpdLedgerAccount";
            dh.AddPare("@Name", SqlDbType.NVarChar, 100, acc.Name);
            dh.AddPare("@AccGroup", SqlDbType.Int, 0, acc.AccGroup);
            dh.AddPare("@id", SqlDbType.NVarChar, 40, acc.LA_GUID);
            dh.AddPare("@AccCode", SqlDbType.Int, 0, acc.AccCode);
            dh.AddPare("@Useable", SqlDbType.Bit, 0, acc.Useable);
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
        /// 删除总账科目
        /// </summary>
        /// <param name="id">总账科目标识</param>
        /// <returns></returns>
        public bool DelLedgerAcc(string id)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_DelLedgerAccount";
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

        /// <summary>
        /// 获取明细科目列表
        /// </summary>
        /// <param name="c_id">公司标识</param>
        /// <returns></returns>
        public List<T_DetailedAccount> GetDetailsAccs(string c_id)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetDetailedAccounts";
            dh.AddPare("@C_ID", SqlDbType.NVarChar, 40, c_id);
            return dh.Reader<T_DetailedAccount>();
        }

        /// <summary>
        /// 获取明细科目
        /// </summary>
        /// <param name="id">明细科目标识</param>
        /// <param name="c_id">公司标识</param>
        /// <returns></returns>
        public List<T_DetailedAccount> GetDetailsAcc(string id, string c_id)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetDetailedAccounts";
            dh.AddPare("@ID", SqlDbType.NVarChar, 40, id);
            dh.AddPare("@C_ID", SqlDbType.NVarChar, 40, c_id);
            return dh.Reader<T_DetailedAccount>();
        }

        /// <summary>
        /// 获取明细科目
        /// </summary>
        /// <param name="id">明细科目标识</param>
        /// <param name="c_id">公司标识</param>
        /// <returns></returns>
        public List<T_DetailedAccount> GetDetailsAcc(string id)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetDetailedAccountss";
            dh.AddPare("@ID", SqlDbType.NVarChar, 40, id);
            return dh.Reader<T_DetailedAccount>();
        }

        /// <summary>
        /// 获取明细科目
        /// </summary>
        /// <param name="id">上级科目标识</param>
        /// <param name="c_id">公司标识</param>
        /// <returns></returns>
        public List<T_DetailedAccount> GetDetailedAccountsParentAccGuid(string c_id, string id)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetDetailedAccountsParentAccGuid";
            dh.AddPare("@ID", SqlDbType.NVarChar, 40, id);
            dh.AddPare("@C_ID", SqlDbType.NVarChar, 40, c_id);
            return dh.Reader<T_DetailedAccount>();
        }

        /// <summary>
        /// 更新明细科目
        /// </summary>
        /// <param name="acc">明细科目对象</param>
        /// <returns></returns>
        public bool UpdDetailedAccount(T_DetailedAccount acc)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_UpdDetailsAccount";
            dh.AddPare("@ID", SqlDbType.NVarChar, 40, acc.DA_GUID);
            dh.AddPare("@CODE", SqlDbType.Int, 0, acc.AccCode);
            dh.AddPare("@NAME", SqlDbType.NVarChar, 100, acc.Name);
            dh.AddPare("@PID", SqlDbType.NVarChar, 40, acc.ParentAccGuid);
            dh.AddPare("@C_ID", SqlDbType.NVarChar, 40, acc.C_GUID);
            dh.AddPare("@D_ID", SqlDbType.NVarChar, 40, acc.D_GUID);
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
        /// 删除明细科目
        /// </summary>
        /// <param name="id">明细科目标识</param>
        /// <returns></returns>
        public bool DelDetailedAccount(string id)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_DelDetailsAccount";
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
    }
}
