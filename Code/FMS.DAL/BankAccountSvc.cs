using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using FMS.Model;

namespace FMS.DAL
{
    public class BankAccountSvc
    {
        /// <summary>
        /// 获取银行
        /// </summary>
        /// <param name="cid">公司标识</param>
        /// <param name="id">银行标识</param>
        /// <returns></returns>
        public List<T_Bank> GetBank(string cid ,string id = null)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetBanks";
            dh.AddPare("@ID", SqlDbType.NVarChar, 40, id);
            dh.AddPare("@C_ID", SqlDbType.NVarChar, 40, cid);
            return dh.Reader<T_Bank>();
        }

        /// <summary>
        /// 获取银行标识
        /// </summary>
        /// <param name="cid">公司标识</param>
        /// <param name="name">银行标识</param>
        /// <returns></returns>
        public object GetBankDts(string cid, string name)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetBanks";
            dh.AddPare("@Name", SqlDbType.NVarChar, 40, name);
            dh.AddPare("@C_ID", SqlDbType.NVarChar, 40, cid);
            return dh.Scalar();
        }

        /// <summary>
        /// 获取银行账户账户标识
        /// </summary>
        /// <param name="cid">公司标识</param>
        /// <param name="bid">银行标识</param>
        /// <param name="account">银行标识</param>
        /// <returns></returns>
        public object GetBankAccountDts(string cid, string bid,string account)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetBankAccounts";
            dh.AddPare("@B_ID", SqlDbType.NVarChar, 40, bid);
            dh.AddPare("@Account", SqlDbType.NVarChar, 40, account);
            dh.AddPare("@C_ID", SqlDbType.NVarChar, 40, cid);
            return dh.Scalar();
        }

        /// <summary>
        /// 获取银行账户
        /// </summary>
        /// <param name="cid">公司标识</param>
        /// <param name="id">银行账户标识</param>
        /// <returns></returns>
        public List<T_BankAccount> GetBankAccount(string cid, string id = null)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetBankAccounts";
            dh.AddPare("@ID", SqlDbType.NVarChar, 40, id);
            dh.AddPare("@C_ID", SqlDbType.NVarChar, 40, cid);
            return dh.Reader<T_BankAccount>();
        }

        /// <summary>
        /// 更新银行信息
        /// </summary>
        /// <param name="bank">银行对象</param>
        /// <returns></returns>
        public bool UpdBank(T_Bank bank)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_UpdBank";
            dh.AddPare("@B_GUID", SqlDbType.NVarChar, 40, bank.B_GUID);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 40, bank.C_GUID);
            dh.AddPare("@Name", SqlDbType.NVarChar, 100, bank.Name);
            try
            {
                dh.NonQuery();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 更新银行账户信息
        /// </summary>
        /// <param name="bankAcc">银行账户对象</param>
        /// <returns></returns>
        public bool UpdBankAccount(T_BankAccount bankAcc)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_UpdBankAccount";
            dh.AddPare("@BA_GUID", SqlDbType.NVarChar, 40, bankAcc.BA_GUID);
            dh.AddPare("@B_GUID", SqlDbType.NVarChar, 40, bankAcc.B_GUID);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 40, bankAcc.C_GUID);
            dh.AddPare("@Account", SqlDbType.NVarChar, 100, bankAcc.Account);

            dh.AddPare("@AccountName", SqlDbType.NVarChar, 40, bankAcc.AccountName);
            dh.AddPare("@AccountCurrency", SqlDbType.NVarChar, 40, bankAcc.AccountCurrency);
            dh.AddPare("@AccountAbbreviation", SqlDbType.NVarChar, 40, bankAcc.AccountAbbreviation);
            dh.AddPare("@AccountType", SqlDbType.NVarChar, 40, bankAcc.AccountType);
            dh.AddPare("@BankAddress", SqlDbType.NVarChar, 100, bankAcc.BankAddress);
            dh.AddPare("@SwiftCode", SqlDbType.NVarChar, 40, bankAcc.SwiftCode);
            try
            {
                dh.NonQuery();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 删除银行
        /// </summary>
        /// <param name="id">银行标识</param>
        /// <returns></returns>
        public bool DelBank(string id)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_DelBank";
            dh.AddPare("@ID", SqlDbType.NVarChar, 40, id);
            try
            {
                dh.NonQuery();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 删除银行账户
        /// </summary>
        /// <param name="id">银行账户标识</param>
        /// <returns></returns>
        public bool DelBankAccount(string id)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_DelBankAccount";
            dh.AddPare("@ID", SqlDbType.NVarChar, 40, id);
            try
            {
                dh.NonQuery();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
