using System.Collections.Generic;
using System.Data;
using FMS.Model;
using System.Linq;



namespace FMS.DAL
{
    public class AccountSvc
    {
        ///<summary>
        ///验证科目余额
        /// </summary> 
        public static int CheckSubjectAmount(string RPer, string SubName, string C_GUID, string Amount)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_CheckSubjectAmount";
            dh.AddPare("@RPer", SqlDbType.NVarChar, 50, RPer);
            dh.AddPare("@SubName", SqlDbType.NVarChar, 50, SubName);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@Amount", SqlDbType.Decimal, 0, Amount);
            dh.AddPare("@result", SqlDbType.Int, ParameterDirection.Output, 0, null);
            dh.NonQuery();
            return dh.GetParaValue<int>("@result");
        }

        
         
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

        public List<T_GeneralLedgerAccount> GetLedgerAccount(int code,string C_GUID,string State) {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetLedgerAccount";
            dh.AddPare("@AccGroup", SqlDbType.Int, 0, code);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 40, C_GUID);
            dh.AddPare("@State", SqlDbType.NVarChar, 40, State);
            return dh.Reader<T_GeneralLedgerAccount>();
        }

        public T_GeneralLedgerAccount GetLAccount(string name, string C_GUID, string invtype)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetLAccount";
            dh.AddPare("@Name", SqlDbType.NVarChar, 40, name);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 40, C_GUID);
            dh.AddPare("@InvType", SqlDbType.NVarChar, 40, invtype);
            return dh.Reader<T_GeneralLedgerAccount>().FirstOrDefault();
        }

        public List<T_DetailedAccount> GetLAccountByName(string name, string C_GUID)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetLAccountByName";
            dh.AddPare("@Name", SqlDbType.NVarChar, 40, name);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 40, C_GUID);
            return dh.Reader<T_DetailedAccount>();
        }

        public List<T_GeneralLedgerAccount> GetLedgerAccount(string C_GUID)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetLedgerAccount";
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 40, C_GUID);
            dh.AddPare("@State", SqlDbType.Int, 0, 1);
            return dh.Reader<T_GeneralLedgerAccount>();
        }

        public List<T_GeneralLedgerAccount> GetDetailLedgerAccount(string id,string C_GUID,string state)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetDetailLedgerAccount";
            dh.AddPare("@LA_GUID", SqlDbType.NVarChar, 40, id);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 40, C_GUID);
            dh.AddPare("@State", SqlDbType.NVarChar, 40, state);
            return dh.Reader<T_GeneralLedgerAccount>();
        }

        public List<T_GeneralLedgerAccount> GetDetailLAByName(string name,string C_GUID,string mark) {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetDetailLedgerAccountByName";
            dh.AddPare("@Name", SqlDbType.NVarChar, 40, name);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 40, C_GUID);
            dh.AddPare("@Mark", SqlDbType.NVarChar, 40, mark);
            return dh.Reader<T_GeneralLedgerAccount>();
        }

        public List<T_GeneralLedgerAccount> GetThirdLAByName(string name, string C_GUID, string mark)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetThirdLedgerAccountByName";
            dh.AddPare("@Name", SqlDbType.NVarChar, 40, name);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 40, C_GUID);
            dh.AddPare("@Mark", SqlDbType.NVarChar, 40, mark);
            return dh.Reader<T_GeneralLedgerAccount>();
        }

        public T_GeneralLedgerAccount GetLAByName(string name,string C_GUID)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetLAByName";
            dh.AddPare("@Name", SqlDbType.NVarChar, 40, name);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 40, C_GUID);
           
            return dh.Reader <T_GeneralLedgerAccount>().FirstOrDefault();
        }

        public T_GeneralLedgerAccount GetParentGuid(string id)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetParentGuid";
            dh.AddPare("@ID", SqlDbType.NVarChar, 40, id);
            return dh.Reader <T_GeneralLedgerAccount>().FirstOrDefault();
        }

        public List<T_GeneralLedgerAccount> GetThirdLedgerAccount(string id,string C_GUID,string State) {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetThirdDetailLedgerAccount";
            dh.AddPare("@LA_GUID", SqlDbType.NVarChar, 40, id);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 40, C_GUID);
            dh.AddPare("@State", SqlDbType.NVarChar, 40, State);
            return dh.Reader<T_GeneralLedgerAccount>();
        }

        public List<T_GeneralLedgerAccount> GetThByName(string name, string C_GUID)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetThLedgerAccountByName";
            dh.AddPare("@Name", SqlDbType.NVarChar, 40, name);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 40, C_GUID);
            return dh.Reader<T_GeneralLedgerAccount>();
        }

        public List<T_GeneralLedgerAccount> GetThByID(string parentAccGuid,string C_GUID)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetThLedgerAccountByID";
            dh.AddPare("@ParentAccGuid", SqlDbType.NVarChar, 50, parentAccGuid);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 40, C_GUID);
            return dh.Reader<T_GeneralLedgerAccount>();
        }

        public bool UpdIntAccount(string cguid,string C_GUID)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_UpdIntAccount";
            dh.AddPare("@cguid", SqlDbType.NVarChar, 50, cguid);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
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

        public bool UpdateUsingLA(string AccLaguid, string State,string C_GUID)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_UpdUsingLA";
            dh.AddPare("@AccLaguid", SqlDbType.NVarChar, 50, AccLaguid);
            dh.AddPare("@State", SqlDbType.NVarChar, 50, State);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
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

        public bool UpdateUsingDLA(string DAguid, string Parguid, string State,string C_GUID)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_UpdUsingDLA";
            dh.AddPare("@DAguid", SqlDbType.NVarChar, 50, DAguid);
            dh.AddPare("@Parguid", SqlDbType.NVarChar, 50, Parguid);
            dh.AddPare("@State", SqlDbType.NVarChar, 50, State);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
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

        public bool UpdateUsingThLA(string DAguid, string Parguid, string State,string C_GUID)
        {

            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_UpdateUsingThLA";
            dh.AddPare("@TDAguid", SqlDbType.NVarChar, 50, DAguid);
            dh.AddPare("@Parguid", SqlDbType.NVarChar, 50, Parguid);
            dh.AddPare("@State", SqlDbType.NVarChar, 50, State);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
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

        public bool AddSubject(string strParentID, int AccCode,string Name,string Subject,string C_GUID)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_AddSubject";
            dh.AddPare("@strParentID", SqlDbType.NVarChar, 50, strParentID);
            dh.AddPare("@AccCode", SqlDbType.Int, 0, AccCode);
            dh.AddPare("@Name", SqlDbType.NVarChar, 50, Name);
            dh.AddPare("@Subject", SqlDbType.NVarChar, 50, Subject);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
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

        public bool DeleteAccount(string id)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_DeleteAccount";
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

        public List<T_DetailedAccount> GetDetailedAccountByName(string Name,string c_id)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetDetailedAccountByName";
            dh.AddPare("@Name", SqlDbType.NVarChar, 100, Name);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, c_id);
            return dh.Reader<T_DetailedAccount>();
        }

        public List<T_ExpenseType> GetDetailType(string c_id, string coloum)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetDetailType";
            dh.AddPare("@C_ID", SqlDbType.NVarChar, 40, c_id);
            dh.AddPare("@Coloum", SqlDbType.NVarChar, 40, coloum);
            return dh.Reader<T_ExpenseType>();
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
        /// <summary>
        /// 获取所有详细分类（包括税费）
        /// </summary>
        /// <param name="c_id">公司id</param>
        /// <returns></returns>
        /// <summary>
        public List<T_ExpenseType> GetAllSonType(string c_id)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetAllSonType";
            dh.AddPare("@C_ID", SqlDbType.NVarChar, 40, c_id);
            return dh.Reader<T_ExpenseType>();
        }
    }
}
