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
        public List<T_BankAccount> GetBank(string cid, string id = null)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetBanks";
            dh.AddPare("@ID", SqlDbType.NVarChar, 40, id);
            dh.AddPare("@C_ID", SqlDbType.NVarChar, 40, cid);
            return dh.Reader<T_BankAccount>();
        }

        public List<T_BankAccount> GetBankAccountsByName(string cid,string Type)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetBankAccountsByName";
            dh.AddPare("@Type", SqlDbType.NVarChar, 40, Type);
            dh.AddPare("@C_ID", SqlDbType.NVarChar, 40, cid);
            return dh.Reader<T_BankAccount>();
        }

        /// <summary>
        /// 转账里面获取公司银行信息
        /// </summary>
        /// <param name="cid"></param>
        /// <param name="Type"></param>
        /// <returns></returns>
        public List<T_BankAccount> GetBankAccountsByNameNew(string cid)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetBankAccountsByNameNew";
            dh.AddPare("@C_ID", SqlDbType.NVarChar, 40, cid);
            return dh.Reader<T_BankAccount>();
        }

        /// <summary>
        /// 获取账户货币
        /// </summary>
        /// <param name="cid">公司标识</param>
        /// <param name="id">银行标识</param>
        /// <returns></returns>
        public List<T_BankAccount> GetBankCurreny( string id,string cid)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetBankCurreny";
            dh.AddPare("@ID", SqlDbType.NVarChar, 40, id);
            dh.AddPare("@C_ID", SqlDbType.NVarChar, 40, cid);
            return dh.Reader<T_BankAccount>();
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
        public T_BankAccount GetBankDt(string cid, string name)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetBanks";
            dh.AddPare("@Name", SqlDbType.NVarChar, 40, name);
            dh.AddPare("@C_ID", SqlDbType.NVarChar, 40, cid);
            return dh.Reader<T_BankAccount>().FirstOrDefault();
        }

        /// <summary>
        /// 获取银行账户账户标识
        /// </summary>
        /// <param name="cid">公司标识</param>
        /// <param name="bid">银行标识</param>
        /// <param name="account">银行标识</param>
        /// <returns></returns>
        public object GetBankAccountDts(string cid, string bid, string account)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetBankAccounts";
            dh.AddPare("@B_ID", SqlDbType.NVarChar, 40, bid);
            dh.AddPare("@Account", SqlDbType.NVarChar, 40, account);
            dh.AddPare("@C_ID", SqlDbType.NVarChar, 40, cid);
            return dh.Scalar();
        }

        public T_BankAccount GetBank(string cid, string bid, string baid, string account)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetBankAccounts";
            dh.AddPare("@ID", SqlDbType.NVarChar, 40, baid);
            dh.AddPare("@B_ID", SqlDbType.NVarChar, 40, bid);
            dh.AddPare("@Account", SqlDbType.NVarChar, 40, account);
            dh.AddPare("@C_ID", SqlDbType.NVarChar, 40, cid);
            return dh.Reader<T_BankAccount>().FirstOrDefault();
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
            dh.AddPare("@InitialAmount", SqlDbType.NVarChar, 100, bankAcc.InitialAmount);
            dh.AddPare("@AccountName", SqlDbType.NVarChar, 40, bankAcc.AccountName);
            dh.AddPare("@AccountCurrency", SqlDbType.NVarChar, 40, bankAcc.AccountCurrency);
            dh.AddPare("@AccountAbbreviation", SqlDbType.NVarChar, 40, bankAcc.AccountAbbreviation);
            dh.AddPare("@AccountType", SqlDbType.NVarChar, 40, bankAcc.AccountType);
            dh.AddPare("@BankAddress", SqlDbType.NVarChar, 100, bankAcc.BankAddress);
            dh.AddPare("@SwiftCode", SqlDbType.NVarChar, 40, bankAcc.SwiftCode);
            dh.AddPare("@OtherBank", SqlDbType.NVarChar, 40, bankAcc.OtherBank);
            //dh.AddPare("@Type", SqlDbType.NVarChar, 40, bankAcc.Type);
            //dh.AddPare("@DetailType", SqlDbType.NVarChar, 40, bankAcc.DetailType);
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

        public bool UpdBankAmount(T_BankAccount bankAmount)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_UpdBankAmount";
            dh.AddPare("@BA_GUID", SqlDbType.NVarChar, 40, bankAmount.BA_GUID);
            dh.AddPare("@InitialAmount", SqlDbType.Decimal, 0, bankAmount.InitialAmount);
            //dh.AddPare("@AccountCurrency", SqlDbType.NVarChar, 40, bankAmount.AccountCurrency);
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
        /// <summary>
        /// 转账列表
        /// </summary>
        /// <param name="rec"></param>
        /// <returns></returns>
        /*public bool UpdAccountTransferRecord(T_BankAccount recordList,string outpayType, string InpayType)
        {
            return UpdAccountTransferRecordCount(recordList,outpayType,InpayType);
        }*/
        public bool UpdAccountTransferRecord(string OutBankAccount, string InBankAccount, string outpayType, string InpayType, string OutDate, string OutAmout, string C_GUID)
        {
            return UpdAccountTransferRecordCount(OutBankAccount, InBankAccount, outpayType, InpayType, OutDate, OutAmout, C_GUID);
        }
        /// <summary>
        /// 转账列表
        /// </summary>
        /// <param name="rec"></param>
        /// <returns></returns>
        /*private bool UpdAccountTransferRecordCount(T_BankAccount rec, string outpayType, string InpayType)
        {
             DBHelper dh = new DBHelper();
            dh.strCmd = "SP_UpdAccountTransfer";
            dh.AddPare("@outpayType", SqlDbType.NVarChar, 200, outpayType);
            dh.AddPare("@InpayType", SqlDbType.NVarChar, 200, InpayType);
            dh.AddPare("@OutBankAccount", SqlDbType.NVarChar, 40, rec.OutBankAccount);
            dh.AddPare("@InBankAccount", SqlDbType.NVarChar, 40, rec.InBankAccount);
            dh.AddPare("@OutAmout", SqlDbType.Decimal, 0, rec.OutAmout);
            dh.AddPare("@OutDate", SqlDbType.NVarChar, 40, rec.OutDate);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 40, rec.C_GUID);//
            try
            {
                dh.NonQuery();
                return true;
            }
            catch
            {
                return false;
            }
        }*/
        private bool UpdAccountTransferRecordCount(string OutBankAccount, string InBankAccount, string outpayType, string InpayType, string OutDate, string OutAmout, string C_GUID)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_UpdAccountTransfer";
            dh.AddPare("@outpayType", SqlDbType.NVarChar, 200, outpayType);
            dh.AddPare("@InpayType", SqlDbType.NVarChar, 200, InpayType);
            dh.AddPare("@OutBankAccount", SqlDbType.NVarChar, 40, OutBankAccount);
            dh.AddPare("@InBankAccount", SqlDbType.NVarChar, 40, InBankAccount);
            dh.AddPare("@OutAmout", SqlDbType.Decimal, 0, OutAmout);
            dh.AddPare("@OutDate", SqlDbType.NVarChar, 40, OutDate);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 40, C_GUID);//
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
        /// 获取转出账户转出前账户余额
        /// </summary>
        /// <param name="rec"></param>
        /// <returns></returns>
        public T_BankAccount GetOutBankAmount(string C_GUID, string AccountAbbreviation)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetOutBankAmount";
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 40, C_GUID);
            dh.AddPare("@AccountAbbreviation", SqlDbType.NVarChar, 40, AccountAbbreviation);
            return dh.Reader<T_BankAccount>().FirstOrDefault();
        }

        /// <summary>
        /// 获取转出账户转出前账户库存现金余额
        /// </summary>
        /// <param name="rec"></param>
        /// <returns></returns>
        public T_BankAccount GetOutKuCun(string C_GUID, string AccountAbbreviation)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetOutKuCun";
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 40, C_GUID);
            dh.AddPare("@AccountAbbreviation", SqlDbType.NVarChar, 40, AccountAbbreviation);
            return dh.Reader<T_BankAccount>().FirstOrDefault();
        }
    }
}
