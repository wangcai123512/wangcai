using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using FMS.Model;

namespace FMS.DAL
{
    public class RecPayRecordSvc
    {
        /// <summary>
        /// 更新收款纪录
        /// </summary>
        /// <param name="rec">收付款纪录对象</param>
        /// <returns></returns>
        public bool UpdReceivablesRecord(T_RecPayRecord rec)
        {
            rec.RP_Flag = "R";
            return UpdRecPayRecord(rec);
        }

        /// <summary>
        /// 编辑收款纪录
        /// </summary>
        /// <param name="rec">收付款纪录对象</param>
        /// <returns></returns>
        public bool UpEditRecPayRecord(T_RecPayRecord rec)
        {
            rec.RP_Flag = "R";
            return EditRecPayRecord(rec);
        }

        public bool UpEditPayRecord(T_RecPayRecord rec)
        {
            rec.RP_Flag = "P";
            return EditRecPayRecord(rec);
        }
        public bool UpdRecPayFLRecord(T_RecPayRecord rec,string Method)
        {
            DBHelper dh = new DBHelper();
            dh.BeginTran();
            try{
                dh.strCmd = "SP_UpdRecPayRecord";
                dh.AddPare("@ID", SqlDbType.NVarChar, 40, rec.RP_GUID);
                dh.AddPare("@Flag", SqlDbType.NVarChar, 1, rec.RP_Flag);
                dh.AddPare("@Method", SqlDbType.NVarChar, 40, Method);
                dh.AddPare("@AccountID", SqlDbType.NVarChar, 40, rec.AccountID);
                dh.AddPare("@InvType", SqlDbType.NVarChar, 20, rec.InvType);
                dh.AddPare("@InvTypeDts", SqlDbType.NVarChar, 50, rec.InvTypeDts);
                dh.AddPare("@DetailInvType", SqlDbType.NVarChar, 50, rec.DetailInvType);
                dh.AddPare("@ThirdInvType", SqlDbType.NVarChar, 50, rec.ThirdInvType);
                dh.AddPare("@PayCategoryID", SqlDbType.NVarChar, 50, rec.PayCategoryID);
                dh.AddPare("@DetailRPTypeID", SqlDbType.NVarChar, 50, rec.DetailRPTypeID);
                dh.AddPare("@Record", SqlDbType.NVarChar, 50, rec.Record);
                dh.AddPare("@SubjectName", SqlDbType.NVarChar, 50, rec.SubjectName);
                dh.AddPare("@InvNo", SqlDbType.NVarChar, 20, rec.InvNo);
                dh.AddPare("@R_Per", SqlDbType.NVarChar, 40, rec.RPer);
                dh.AddPare("@DLA", SqlDbType.NVarChar, 40, rec.DebitLedgerAccount);
                dh.AddPare("@DDA", SqlDbType.NVarChar, 40, rec.DebitDetailsAccount);
                dh.AddPare("@CLA", SqlDbType.NVarChar, 40, rec.CreditLedgerAccount);
                dh.AddPare("@CDA", SqlDbType.NVarChar, 40, rec.CreditDetailsAccount);
                dh.AddPare("@Amount", SqlDbType.Decimal, 0, rec.SumAmount);
                dh.AddPare("@Mark", SqlDbType.NVarChar, 20, rec.Mark);
                dh.AddPare("@DisAmount", SqlDbType.Decimal, 0, rec.DisAmount1);
                dh.AddPare("@Date", SqlDbType.Date, 0, rec.Date);
                dh.AddPare("@Remark", SqlDbType.NVarChar, 200, rec.Remark);
                dh.AddPare("@Creator", SqlDbType.NVarChar, 40, rec.Creator);
                dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, rec.C_GUID);
                dh.AddPare("@RPable", SqlDbType.NVarChar, 40, rec.RPable);
                dh.AddPare("@Currency", SqlDbType.NVarChar, 5, rec.Currency);
                dh.AddPare("@CFItemGuid", SqlDbType.NVarChar, 40, rec.CFItemGuid);
                dh.AddPare("@CFPItemGuid", SqlDbType.NVarChar, 40, rec.CFPItemGuid);
                dh.AddPare("@B_GUID", SqlDbType.NVarChar, 40, rec.B_GUID);
                dh.AddPare("@BA_GUID", SqlDbType.NVarChar, 40, rec.BA_GUID);
                dh.AddPare("@RPType", SqlDbType.NVarChar, 50, rec.RPType);
                dh.NonQuery();
                dh.CleanPara();
                dh.CommitTran();
                return true;
            }
            catch(Exception ex)
            {
                dh.RollBackTran();
                return false;
            }
        }
        /// <summary>
        /// 更新收款纪录
        /// </summary>
        /// <param name="rec">收付款纪录对象</param>
        /// <returns></returns>
        public bool UpdPaymentRecord(T_RecPayRecord rec, string strSalaryID = "", string strYtTaxID="")
        {
            rec.RP_Flag = "P";
            return UpdRecPayRecord(rec, strSalaryID, strYtTaxID);
        }

        public bool UpdVouIERPRecord(string IE_GUID, string RP_GUID, string mark, string Flag,string C_GUID) { 
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_UpdVouIERPRecord";
            dh.AddPare("@IE_GUID", SqlDbType.NVarChar, 40, IE_GUID);
            dh.AddPare("@RP_GUID", SqlDbType.NVarChar, 40, RP_GUID);
            dh.AddPare("@Flag", SqlDbType.NVarChar, 40, Flag);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 40, C_GUID);
            dh.AddPare("@Mark", SqlDbType.NVarChar, 40, mark);
          
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
        public bool UpdIERP(string IE_GUID, string RP_GUID, string check, string mark, string Flag, string InvTypeDts, string B_GUID = "")
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_UpdIERPRecord";
            dh.AddPare("@IE_GUID", SqlDbType.NVarChar, 40, IE_GUID);
            dh.AddPare("@RP_GUID", SqlDbType.NVarChar, 40, RP_GUID);
            dh.AddPare("@Flag", SqlDbType.NVarChar, 40, Flag);
            dh.AddPare("@Check", SqlDbType.NVarChar, 40, check);
            dh.AddPare("@Mark", SqlDbType.NVarChar, 40, mark);
            dh.AddPare("@InvTypeDts", SqlDbType.NVarChar, 50, InvTypeDts);
            dh.AddPare("@B_GUID", SqlDbType.NVarChar, 40, B_GUID);
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

        public bool UpdVoucherIERP(string IE_GUID, string RP_GUID, Decimal bankAmount, string Flag, string IE_Flag, string RPLA_GUID, string IELA_GUID,string C_GUID)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_UpdVoucherIERP";
            dh.AddPare("@IE_GUID", SqlDbType.NVarChar, 40, IE_GUID);
            dh.AddPare("@RP_GUID", SqlDbType.NVarChar, 40, RP_GUID);
            dh.AddPare("@RPLA_GUID", SqlDbType.NVarChar, 40, RPLA_GUID);
            dh.AddPare("@IELA_GUID", SqlDbType.NVarChar, 40, IELA_GUID);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 40, C_GUID);
            dh.AddPare("@Flag", SqlDbType.NVarChar, 40, Flag);
            dh.AddPare("@IE_Flag", SqlDbType.NVarChar, 40, IE_Flag);
            dh.AddPare("@BankAmount", SqlDbType.NVarChar, 40, bankAmount);
           
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

        public bool UpdIERPMore(T_RecPayRecord rec)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_UpdIERPRecordMore";
            dh.AddPare("@RP_GUID", SqlDbType.NVarChar, 40,rec.RP_GUID);
            dh.AddPare("@Amount", SqlDbType.Decimal, 0, rec.SumAmount);
            dh.AddPare("@DisAmount", SqlDbType.Decimal, 0, rec.DisAmount);
            dh.AddPare("@Mark", SqlDbType.NVarChar, 40, rec.Mark);
            dh.AddPare("@RP_Flag", SqlDbType.NVarChar, 40, rec.RP_Flag);
            dh.AddPare("@B_GUID", SqlDbType.NVarChar, 50, rec.B_GUID);
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
        public bool UpdIERPMore(T_RecPayRecord rec, string SumAmount, string  DisAmount)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_UpdIERPRecordMore";
            dh.AddPare("@RP_GUID", SqlDbType.NVarChar, 40, rec.RP_GUID);
            dh.AddPare("@Amount", SqlDbType.Decimal, 0, SumAmount);
            dh.AddPare("@DisAmount", SqlDbType.Decimal, 0,DisAmount);
            dh.AddPare("@Mark", SqlDbType.NVarChar, 40, rec.Mark);
            dh.AddPare("@RP_Flag", SqlDbType.NVarChar, 40, rec.RP_Flag);
            dh.AddPare("@B_GUID", SqlDbType.NVarChar, 50, rec.B_GUID);
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
        /// 更新收/付款纪录
        /// </summary>
        /// <param name="rec">收付款纪录对象</param>
        /// <returns></returns>
        public bool UpdRecPayRecord(T_RecPayRecord rec, string strSalaryID = "", string strYtTaxID="")
        {
            DBHelper dh = new DBHelper();
            dh.BeginTran();
            try { 
            dh.strCmd = "SP_UpdRecPayRecord";
            switch (rec.InvTypeDts)
            {
                case "支付的各项税费":
                    break;
                case "支付职工薪酬":
                    break;
                default:
                     bool IsCustomer = false;
                    bool IsPartner = false;
                    bool IsSupplier = false;
                    if (rec.RP_Flag == "R")
                    {
                        
                        if (rec.Log_c == "b")
                        {
                            IsCustomer = true;
                            
                            string BP_GUID = rec.RPer;
                            DBHelper da = new DBHelper();
                            da.strCmd = "SP_UpdPartner";
                            da.AddPare("@state", SqlDbType.NVarChar, 40, "customer");
                            da.AddPare("@ID", SqlDbType.NVarChar, 40, BP_GUID);
                            da.AddPare("@Name", SqlDbType.NVarChar, 100, rec.Log);
                            da.AddPare("@IsCustomer", SqlDbType.Bit, 0, IsCustomer);
                            da.AddPare("@C_GUID", SqlDbType.NVarChar, 50, rec.C_GUID);
                            da.NonQuery();
                            da.CleanPara();
                            rec.RPer = BP_GUID;
                        }
                        else if (rec.Log_c == "c")
                        {
                            string BP_GUID = Guid.NewGuid().ToString();
                            IsCustomer = false;
                            IsPartner = true;
                            IsSupplier = false;
                            DBHelper da = new DBHelper();
                            da.strCmd = "SP_UpdPartner";
                            da.AddPare("@ID", SqlDbType.NVarChar, 40, BP_GUID);
                            da.AddPare("@Name", SqlDbType.NVarChar, 100, rec.Log);
                            da.AddPare("@IsCustomer", SqlDbType.Bit, 0, IsCustomer);
                            da.AddPare("@ISSupplier", SqlDbType.Bit, 0, IsSupplier);
                            da.AddPare("@IsPartner", SqlDbType.Bit, 0, IsPartner);
                            da.AddPare("@C_GUID", SqlDbType.NVarChar, 50, rec.C_GUID);
                            da.NonQuery();
                            da.CleanPara();
                            rec.RPer = BP_GUID;

                        } else if (rec.Log_c == "d")
                        {
                            IsCustomer = true;
                            IsPartner = false;
                            IsSupplier = false;
                            string BP_GUID = Guid.NewGuid().ToString();
                            DBHelper da = new DBHelper();
                            da.strCmd = "SP_UpdPartner";
                            da.AddPare("@ID", SqlDbType.NVarChar, 40, BP_GUID);
                            da.AddPare("@Name", SqlDbType.NVarChar, 100, rec.Log);
                            da.AddPare("@IsCustomer", SqlDbType.Bit, 0, IsCustomer);
                            da.AddPare("@ISSupplier", SqlDbType.Bit, 0, IsSupplier);
                            da.AddPare("@IsPartner", SqlDbType.Bit, 0, IsPartner);
                            da.AddPare("@C_GUID", SqlDbType.NVarChar, 50, rec.C_GUID);
                            da.NonQuery();
                            da.CleanPara();
                            rec.RPer = BP_GUID;
                        }
                    }
                    
                    if(rec.RP_Flag =="P")
                    {
                        ///更新为供应商
                        if (rec.Log_c == "b") 
                        {
                            IsSupplier = true;
                            string BP_GUID = rec.RPer;
                            DBHelper da = new DBHelper();
                            da.strCmd = "SP_UpdPartner";
                            da.AddPare("@state", SqlDbType.NVarChar, 40, "supplier");
                            da.AddPare("@ID", SqlDbType.NVarChar, 40, BP_GUID);
                            da.AddPare("@Name", SqlDbType.NVarChar, 100, rec.Log);
                            da.AddPare("@ISSupplier", SqlDbType.Bit, 0, IsSupplier);
                            da.AddPare("@C_GUID", SqlDbType.NVarChar, 50, rec.C_GUID);
                            da.NonQuery();
                            da.CleanPara();
                            rec.RPer = BP_GUID;
                        }
                        ///新增合作伙伴
                        if (rec.Log_c == "c")
                        {
                        IsCustomer = false;
                        IsPartner = true;
                        IsSupplier = false;
                        string BP_GUID = Guid.NewGuid().ToString();
                        DBHelper da = new DBHelper();
                        da.strCmd = "SP_UpdPartner";
                        da.AddPare("@ID", SqlDbType.NVarChar, 40, BP_GUID);
                        da.AddPare("@Name", SqlDbType.NVarChar, 100, rec.Log);
                        da.AddPare("@IsCustomer", SqlDbType.Bit, 0, IsCustomer);
                        da.AddPare("@ISSupplier", SqlDbType.Bit, 0, IsSupplier);
                        da.AddPare("@IsPartner", SqlDbType.Bit, 0, IsPartner);
                        da.AddPare("@C_GUID", SqlDbType.NVarChar, 50, rec.C_GUID);
                        da.NonQuery();
                        da.CleanPara();
                        rec.RPer = BP_GUID;
                      }
                        ///新增供应商
                        if (rec.Log_c == "d")
                        {
                            IsCustomer = false;
                            IsPartner = false;
                            IsSupplier = true;
                            string BP_GUID = Guid.NewGuid().ToString();
                            DBHelper da = new DBHelper();
                            da.strCmd = "SP_UpdPartner";
                            da.AddPare("@ID", SqlDbType.NVarChar, 40, BP_GUID);
                            da.AddPare("@Name", SqlDbType.NVarChar, 100, rec.Log);
                            da.AddPare("@IsCustomer", SqlDbType.Bit, 0, IsCustomer);
                            da.AddPare("@ISSupplier", SqlDbType.Bit, 0, IsSupplier);
                            da.AddPare("@IsPartner", SqlDbType.Bit, 0, IsPartner);
                            da.AddPare("@C_GUID", SqlDbType.NVarChar, 50, rec.C_GUID);
                            da.NonQuery();
                            da.CleanPara();
                            rec.RPer = BP_GUID;
                        }
                   }

                    break;
            }


            dh.AddPare("@Log_c", SqlDbType.NVarChar, 40, rec.Log_c);   
            dh.AddPare("@ID", SqlDbType.NVarChar, 40, rec.RP_GUID);
            dh.AddPare("@Flag", SqlDbType.NVarChar, 1, rec.RP_Flag);
            dh.AddPare("@InvType", SqlDbType.NVarChar, 20, rec.InvType);
            dh.AddPare("@InvTypeDts", SqlDbType.NVarChar, 50, rec.InvTypeDts);
            dh.AddPare("@DetailInvType", SqlDbType.NVarChar, 50, rec.DetailInvType);
            dh.AddPare("@ThirdInvType", SqlDbType.NVarChar, 50, rec.ThirdInvType);
            dh.AddPare("@PayCategoryID", SqlDbType.NVarChar, 50, rec.PayCategoryID);
            dh.AddPare("@DetailRPTypeID", SqlDbType.NVarChar, 50, rec.DetailRPTypeID);
            dh.AddPare("@Record", SqlDbType.NVarChar, 50, rec.Record);
            dh.AddPare("@SubjectName", SqlDbType.NVarChar, 50, rec.SubjectName);
            dh.AddPare("@InvNo", SqlDbType.NVarChar, 20, rec.InvNo);
            dh.AddPare("@R_Per", SqlDbType.NVarChar, 40, rec.RPer);
            dh.AddPare("@DLA", SqlDbType.NVarChar, 40, rec.DebitLedgerAccount);
            dh.AddPare("@DDA", SqlDbType.NVarChar, 40, rec.DebitDetailsAccount);
            dh.AddPare("@CLA", SqlDbType.NVarChar, 40, rec.CreditLedgerAccount);
            dh.AddPare("@CDA", SqlDbType.NVarChar, 40, rec.CreditDetailsAccount);
            dh.AddPare("@Amount", SqlDbType.Decimal, 0, rec.SumAmount);
            dh.AddPare("@Mark", SqlDbType.NVarChar, 20, rec.Mark);
            dh.AddPare("@DisAmount", SqlDbType.Decimal, 0, rec.DisAmount1);
            dh.AddPare("@Date", SqlDbType.Date, 0, rec.Date);
            dh.AddPare("@Remark", SqlDbType.NVarChar, 200, rec.Remark);
            dh.AddPare("@Creator", SqlDbType.NVarChar, 40, rec.Creator);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, rec.C_GUID);
            dh.AddPare("@RPable", SqlDbType.NVarChar, 40, rec.RPable);
            dh.AddPare("@Currency", SqlDbType.NVarChar, 5, rec.Currency);
            dh.AddPare("@CFItemGuid", SqlDbType.NVarChar, 40, rec.CFItemGuid);
            dh.AddPare("@CFPItemGuid", SqlDbType.NVarChar, 40, rec.CFPItemGuid);
            dh.AddPare("@B_GUID", SqlDbType.NVarChar, 40, rec.B_GUID);
            dh.AddPare("@BA_GUID", SqlDbType.NVarChar, 40, rec.BA_GUID);
            dh.AddPare("@RPType", SqlDbType.NVarChar, 50, rec.RPType);
            dh.AddPare("@SalaryID", SqlDbType.NVarChar, 50, strSalaryID);
            dh.AddPare("@YtTaxID", SqlDbType.NVarChar, 50, strYtTaxID);
            dh.AddPare("@IsInit", SqlDbType.Int, 0, rec.IsInit);
                dh.NonQuery();
                dh.CleanPara();
                dh.CommitTran();
            return true;
           }
            
            catch(Exception ex)
            {
                dh.RollBackTran();
                return false;
            }
        }

        /// <summary>
        /// 更新收/付款纪录
        /// </summary>
        /// <param name="rec">收付款纪录对象</param>
        /// <returns></returns>
        private bool EditRecPayRecord(T_RecPayRecord rec)
        {
            DBHelper dh = new DBHelper();
            dh.BeginTran();
            try
            {
                dh.strCmd = "SP_UpdRecPayRecord";
                if (rec.InvTypeDts != "支付的各项税费")
                {
                    if (rec.RPer == null)
                    {
                        bool IsCustomer;
                        bool IsPartner;
                        bool IsSupplier;

                        if (rec.RP_Flag == "R")
                        {
                            IsCustomer = true;
                            IsPartner = false;
                            IsSupplier = false;
                            string BP_GUID = Guid.NewGuid().ToString();
                            DBHelper da = new DBHelper();
                            da.strCmd = "SP_UpdPartner";
                            da.AddPare("@ID", SqlDbType.NVarChar, 40, BP_GUID);
                            da.AddPare("@Name", SqlDbType.NVarChar, 100, rec.Log);
                            da.AddPare("@IsCustomer", SqlDbType.Bit, 0, IsCustomer);
                            da.AddPare("@ISSupplier", SqlDbType.Bit, 0, IsSupplier);
                            da.AddPare("@IsPartner", SqlDbType.Bit, 0, IsPartner);
                            da.AddPare("@C_GUID", SqlDbType.NVarChar, 50, rec.C_GUID);
                            da.NonQuery();
                            da.CleanPara();
                            rec.RPer = BP_GUID;


                        }

                        if (rec.RP_Flag == "P")
                        {
                            IsCustomer = false;
                            IsPartner = false;
                            IsSupplier = true;
                            string BP_GUID = Guid.NewGuid().ToString();
                            DBHelper da = new DBHelper();
                            da.strCmd = "SP_UpdPartner";
                            da.AddPare("@ID", SqlDbType.NVarChar, 40, BP_GUID);
                            da.AddPare("@Name", SqlDbType.NVarChar, 100, rec.Log);
                            da.AddPare("@IsCustomer", SqlDbType.Bit, 0, IsCustomer);
                            da.AddPare("@ISSupplier", SqlDbType.Bit, 0, IsSupplier);
                            da.AddPare("@IsPartner", SqlDbType.Bit, 0, IsPartner);
                            da.AddPare("@C_GUID", SqlDbType.NVarChar, 50, rec.C_GUID);
                            da.NonQuery();
                            da.CleanPara();
                            rec.RPer = BP_GUID;
                        }
                    }
                }
                dh.strCmd = "SP_UpdEdRecPayRecord";
                dh.AddPare("@ID", SqlDbType.NVarChar, 40, rec.RP_GUID);
                dh.AddPare("@Flag", SqlDbType.NVarChar, 1, rec.RP_Flag);
                dh.AddPare("@InvType", SqlDbType.NVarChar, 20, rec.InvType);
                dh.AddPare("@InvTypeDts", SqlDbType.NVarChar, 50, rec.InvTypeDts);
                dh.AddPare("@InvNo", SqlDbType.NVarChar, 20, rec.InvNo);
                dh.AddPare("@R_Per", SqlDbType.NVarChar, 40, rec.RPer);
                dh.AddPare("@DLA", SqlDbType.NVarChar, 40, rec.DebitLedgerAccount);
                dh.AddPare("@DDA", SqlDbType.NVarChar, 40, rec.DebitDetailsAccount);
                dh.AddPare("@CLA", SqlDbType.NVarChar, 40, rec.CreditLedgerAccount);
                dh.AddPare("@CDA", SqlDbType.NVarChar, 40, rec.CreditDetailsAccount);
                dh.AddPare("@Amount", SqlDbType.Decimal, 0, rec.SumAmount);
                dh.AddPare("@Date", SqlDbType.Date, 0, rec.Date);
                dh.AddPare("@Remark", SqlDbType.NVarChar, 200, rec.Remark);
                dh.AddPare("@Creator", SqlDbType.NVarChar, 40, rec.Creator);
                dh.AddPare("@CreateDate", SqlDbType.DateTime, 0, rec.CreateDate);
                dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, rec.C_GUID);
                dh.AddPare("@RPable", SqlDbType.NVarChar, 40, rec.RPable);
                dh.AddPare("@Currency", SqlDbType.NVarChar, 5, rec.Currency);
                dh.AddPare("@CFItemGuid", SqlDbType.NVarChar, 40, rec.CFItemGuid);
                dh.AddPare("@CFPItemGuid", SqlDbType.NVarChar, 40, rec.CFPItemGuid);
                dh.AddPare("@B_GUID", SqlDbType.NVarChar, 40, rec.B_GUID);
                dh.AddPare("@BA_GUID", SqlDbType.NVarChar, 40, rec.BA_GUID);
                dh.AddPare("@IE_GUID", SqlDbType.NVarChar, 40, rec.IE_GUID);
                dh.NonQuery();
                dh.CleanPara();
                dh.CommitTran();
                return true;
            }
            catch (Exception ex)
            {
                dh.RollBackTran();
                return false;
            }
        }

        /// <summary>
        /// 获取收款列表
        /// </summary>
        /// <param name="C_GUID">公司标识</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="count">纪录总数</param>
        /// <returns></returns>
        public List<T_RecPayRecord> GetReceivablesRecord(string C_GUID, int pageIndex, int pageSize, out int count)
        {
            return GetRecPayRecords(pageIndex, pageSize, out count, C_GUID, "R");
        }

        /// <summary>
        /// 获取未归档的收款纪录
        /// </summary>
        /// <param name="C_GUID">公司标识</param>
        /// <returns></returns>
        public List<T_RecPayRecord> GetUnclassifyReceivablesRecord(string C_GUID)
        {
            int count = 0;
            return GetRecPayRecords(0, -1, out count, C_GUID, "R", false);
        }

        /// <summary>
        /// 获取未归档的收款纪录
        /// </summary>
        /// <param name="C_GUID">公司标识</param>
        /// <returns></returns>
        public List<T_RecPayRecord> GetUnclassifyReceivablesRecord(string C_GUID, int page, int rows, out int count, string dateBegin, string dateEnd, string customer, string incomeGrp, string incomeGrpdts=null)
        {
            return GetRecPayRecordsCopy(page, rows, out count, C_GUID,dateBegin,dateEnd,customer,incomeGrp,incomeGrpdts, "R", false);
        }

        public List<T_RecPayRecord> GetReceivablesSelfList(string C_GUID, int page, int rows, out int count, string dateBegin, string dateEnd, string customer, string incomeGrp, string incomeGrpdts = null)
        {
            return GetReceivablesSelfList(page, rows, out count, C_GUID, dateBegin, dateEnd, customer, incomeGrp, incomeGrpdts, "R", false);
        }

        public List<T_RecPayRecord> GetPaymentSelfList(string C_GUID, int page, int rows, out int count, string dateBegin, string dateEnd, string customer, string incomeGrp, string incomeGrpdts = null)
        {
            return GetPaymentSelfList(page, rows, out count, C_GUID, dateBegin, dateEnd, customer, incomeGrp, incomeGrpdts, "P", false);
        }

        public List<T_RecPayRecord> GetPaymentSelfListTwo(string C_GUID, int page, int rows, out int count, string dateBegin, string dateEnd, string customer, string incomeGrp, string incomeGrpdts = null)
        {
            return GetPaymentSelfListTwo(page, rows, out count, C_GUID, dateBegin, dateEnd, customer, incomeGrp, incomeGrpdts, "P", false);
        }

        /// <summary>
        /// 获取所有收款纪录
        /// </summary>
        /// <param name="C_GUID">公司标识</param>
        /// <returns></returns>
        public List<T_RecPayRecord> GetAllReceivablesRecord(string C_GUID)
        {
            return GetAllRecPayRecords(C_GUID, "R");
        }

        /// <summary>
        /// 获取付款纪录
        /// </summary>
        /// <param name="C_GUID">公司标识</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="count">纪录总数</param>
        /// <returns></returns>
        public List<T_RecPayRecord> GetPaymentRecord(string C_GUID, int pageIndex, int pageSize, out int count)
        {
            return GetRecPayRecords(pageIndex, pageSize, out count, C_GUID, "P");
        }
        public List<T_RecPayRecord> GetReceivablesRecord(string C_GUID, int page, int rows, out int count, string record, string dateBegin, string dateEnd, string customer, string incomeGrp,string InvTypeDts)
        {
            return GetAllList("R", C_GUID, page, rows, out count, record, dateBegin, dateEnd, customer, incomeGrp, InvTypeDts);
        }
        public List<T_RecPayRecord> GetPaymentRecord(string C_GUID, int page, int rows, out int count, string record, string dateBegin, string dateEnd, string customer, string incomeGrp, string InvTypeDts)
        {

            return GetAllList("P", C_GUID, page, rows, out count, record, dateBegin, dateEnd, customer, incomeGrp, InvTypeDts);
        }
        /// <summary>
        /// 获取未归档的付款纪录
        /// </summary>
        /// <param name="C_GUID">公司标识</param>
        /// <returns></returns>
        public List<T_RecPayRecord> GetUnclassifyPaymentRecord(string C_GUID)
        {
            int count = 0;
            return GetRecPayRecords(0, -1, out count, C_GUID, "P", false);
        }

        /// <summary>
        /// 获取未归档的付款纪录
        /// </summary>
        /// <param name="C_GUID">公司标识</param>
        /// <returns></returns>
        public List<T_RecPayRecord> GetUnclassifyPaymentRecord(string C_GUID, int page, int rows, out int count, string dateBegin, string dateEnd, string customer, string incomeGrp, string incomeGrpdts=null)
        {
            return GetRecPayRecordsCopy(page, rows, out count, C_GUID, dateBegin, dateEnd, customer, incomeGrp,incomeGrpdts, "P", false);
        }
        /// <summary>
        /// 获取凭证
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <param name="id"></param>
        /// <returns></returns>

        public List<T_RecPayRecord> GetRPVoucher(int pageIndex, int pageSize, out int count, string id, string flag)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetRPVoucher";
            dh.AddPare("@RP_GUID", SqlDbType.NVarChar, 50, id);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            dh.AddPare("@FLAG", SqlDbType.NVarChar, 1, flag);
            List<T_RecPayRecord> result = new List<T_RecPayRecord>();
            result = dh.Reader<T_RecPayRecord>();
            count = dh.GetParaValue<int>("@Count");
            return result;

        }

        public List<RPVoucherInfo> GetRPVoucherInfo(string id)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "GetAccountDetailView";
            dh.AddPare("@RP_GUID", SqlDbType.NVarChar, 50, id);
            List<RPVoucherInfo> result = new List<RPVoucherInfo>();
            result = dh.Reader<RPVoucherInfo>();
            return result;

        }
        /// <summary>
        /// 查看实付或者实收凭证
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<RPVoucherInfo> GetVoucherInfoByRPID(string id)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetAccountDetailByRPID";
            dh.AddPare("@RP_GUID", SqlDbType.NVarChar, 50, id);
            List<RPVoucherInfo> result = new List<RPVoucherInfo>();
            result = dh.Reader<RPVoucherInfo>();
            return result;

        }

        /// <summary>
        /// 转账凭证
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="count"></param>
        /// <param name="id"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public List<T_RecPayRecord> GetRPVoucherNew(int pageIndex, int pageSize, out int count, string id, string flag)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetRPVoucherNew";
            dh.AddPare("@RP_GUID", SqlDbType.NVarChar, 50, id);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            dh.AddPare("@FLAG", SqlDbType.NVarChar, 1, flag);
            List<T_RecPayRecord> result = new List<T_RecPayRecord>();
            result = dh.Reader<T_RecPayRecord>();
            count = dh.GetParaValue<int>("@Count");
            return result;

        }

        /// <summary>
        /// 获取所有付款纪录
        /// </summary>
        /// <param name="C_GUID">公司标识</param>
        /// <returns></returns>
        public List<T_RecPayRecord> GetAllPaymentRecord(string C_GUID)
        {
            return GetAllRecPayRecords(C_GUID, "P");
        }

        /// <summary>
        /// 获取收付款纪录列表
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="count">纪录总数</param>
        /// <param name="C_GUID">公司标识</param>
        /// <param name="flag">收付标识</param>
        /// <param name="classifyFlag">归档标识</param>
        /// <returns></returns>
        private List<T_RecPayRecord> GetRecPayRecords(int pageIndex, int pageSize, out int count, string C_GUID, string flag = null, bool? classifyFlag = null)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetRecPayRecord";
            dh.AddPare("@PageIndex", SqlDbType.Int, 0, pageIndex);
            dh.AddPare("@PageSize", SqlDbType.Int, 0, pageSize);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            dh.AddPare("@Flag", SqlDbType.NVarChar, 1, flag);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@ClassifyFlag", SqlDbType.Bit, 0, classifyFlag);
            List<T_RecPayRecord> result = new List<T_RecPayRecord>();
            result = dh.Reader<T_RecPayRecord>();
            count = dh.GetParaValue<int>("@Count");
            return result;
        }

        private List<T_RecPayRecord> GetAllList(string flag, string C_GUID, int pageIndex, int pageSize, out int count, string record, string dateBegin, string dateEnd, string customer, string incomeGrp, string InvTypeDts)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetPays";
            dh.AddPare("@Flag", SqlDbType.NVarChar, 1, flag);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@PageSize", SqlDbType.Int, 0, pageSize);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            dh.AddPare("@record", SqlDbType.NVarChar, 40, record);
            dh.AddPare("@InvTypeDts", SqlDbType.NVarChar, 40, InvTypeDts);
            if (!string.IsNullOrEmpty(dateBegin))
            {
                dh.AddPare("@dateBegin", SqlDbType.DateTime, 0, DateTime.Parse(dateBegin));
            }
            if (!string.IsNullOrEmpty(dateEnd))
            {
                dh.AddPare("@dateEnd", SqlDbType.DateTime, 0, DateTime.Parse(dateEnd));
            }
            dh.AddPare("@customer", SqlDbType.NVarChar, 40, customer);
           
            dh.AddPare("@ieGroup", SqlDbType.NVarChar, 20, incomeGrp);
            List<T_RecPayRecord> result = new List<T_RecPayRecord>();
            result = dh.Reader<T_RecPayRecord>();
            count = dh.GetParaValue<int>("@Count");
            return result;
        }
        /// <summary>
        /// 获取收付款纪录列表
        /// </summary>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="count">纪录总数</param>
        /// <param name="C_GUID">公司标识</param>
        /// <param name="flag">收付标识</param>
        /// <param name="classifyFlag">归档标识</param>
        /// <returns></returns>
        private List<T_RecPayRecord> GetRecPayRecordsCopy(int pageIndex, int pageSize, out int count, string C_GUID, string dateBegin, string dateEnd, string customer, string incomeGrp,string incomeGrpdts=null, string flag = null, bool? classifyFlag = null)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetRecPayRecordCopy";
            dh.AddPare("@PageIndex", SqlDbType.Int, 0, pageIndex);
            dh.AddPare("@PageSize", SqlDbType.Int, 0, pageSize);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            dh.AddPare("@Flag", SqlDbType.NVarChar, 1, flag);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@ClassifyFlag", SqlDbType.Bit, 0, classifyFlag);
            if (!string.IsNullOrEmpty(dateBegin))
            {
                dh.AddPare("@dateBegin", SqlDbType.DateTime, 0, DateTime.Parse(dateBegin));
            }
            if (!string.IsNullOrEmpty(dateEnd))
            {
                dh.AddPare("@dateEnd", SqlDbType.DateTime, 0, DateTime.Parse(dateEnd));
            }
            dh.AddPare("@customer", SqlDbType.NVarChar, 40, customer);
            dh.AddPare("@incomeGrp", SqlDbType.NVarChar, 20, incomeGrp);
            dh.AddPare("@incomeGrpdts", SqlDbType.NVarChar, 40, incomeGrpdts);
            List<T_RecPayRecord> result = new List<T_RecPayRecord>();
            result = dh.Reader<T_RecPayRecord>();
            count = dh.GetParaValue<int>("@Count");
            return result;
        }
        private List<T_RecPayRecord> GetReceivablesSelfList(int pageIndex, int pageSize, out int count, string C_GUID, string dateBegin, string dateEnd, string customer, string incomeGrp, string incomeGrpdts = null, string flag = null, bool? classifyFlag = null)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetReceivablesSelfList";
            dh.AddPare("@PageIndex", SqlDbType.Int, 0, pageIndex);
            dh.AddPare("@PageSize", SqlDbType.Int, 0, pageSize);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            dh.AddPare("@Flag", SqlDbType.NVarChar, 1, flag);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@ClassifyFlag", SqlDbType.Bit, 0, classifyFlag);
            if (!string.IsNullOrEmpty(dateBegin))
            {
                dh.AddPare("@dateBegin", SqlDbType.DateTime, 0, DateTime.Parse(dateBegin));
            }
            if (!string.IsNullOrEmpty(dateEnd))
            {
                dh.AddPare("@dateEnd", SqlDbType.DateTime, 0, DateTime.Parse(dateEnd));
            }
            dh.AddPare("@customer", SqlDbType.NVarChar, 40, customer);
            dh.AddPare("@incomeGrp", SqlDbType.NVarChar, 20, incomeGrp);
            dh.AddPare("@incomeGrpdts", SqlDbType.NVarChar, 40, incomeGrpdts);
            List<T_RecPayRecord> result = new List<T_RecPayRecord>();
            result = dh.Reader<T_RecPayRecord>();
            count = dh.GetParaValue<int>("@Count");
            return result;
        }

        private List<T_RecPayRecord> GetPaymentSelfList(int pageIndex, int pageSize, out int count, string C_GUID, string dateBegin, string dateEnd, string customer, string incomeGrp, string incomeGrpdts = null, string flag = null, bool? classifyFlag = null)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetReceivablesSelfListTwo";
            dh.AddPare("@PageIndex", SqlDbType.Int, 0, pageIndex);
            dh.AddPare("@PageSize", SqlDbType.Int, 0, pageSize);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            dh.AddPare("@Flag", SqlDbType.NVarChar, 1, flag);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@ClassifyFlag", SqlDbType.Bit, 0, classifyFlag);
            if (!string.IsNullOrEmpty(dateBegin))
            {
                dh.AddPare("@dateBegin", SqlDbType.DateTime, 0, DateTime.Parse(dateBegin));
            }
            if (!string.IsNullOrEmpty(dateEnd))
            {
                dh.AddPare("@dateEnd", SqlDbType.DateTime, 0, DateTime.Parse(dateEnd));
            }
            dh.AddPare("@customer", SqlDbType.NVarChar, 40, customer);
            dh.AddPare("@incomeGrp", SqlDbType.NVarChar, 20, incomeGrp);
            dh.AddPare("@incomeGrpdts", SqlDbType.NVarChar, 40, incomeGrpdts);
            List<T_RecPayRecord> result = new List<T_RecPayRecord>();
            result = dh.Reader<T_RecPayRecord>();
            count = dh.GetParaValue<int>("@Count");
            return result;
        }

        private List<T_RecPayRecord> GetPaymentSelfListTwo(int pageIndex, int pageSize, out int count, string C_GUID, string dateBegin, string dateEnd, string customer, string incomeGrp, string incomeGrpdts = null, string flag = null, bool? classifyFlag = null)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetReceivablesSelfListThree";
            dh.AddPare("@PageIndex", SqlDbType.Int, 0, pageIndex);
            dh.AddPare("@PageSize", SqlDbType.Int, 0, pageSize);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            dh.AddPare("@Flag", SqlDbType.NVarChar, 1, flag);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@ClassifyFlag", SqlDbType.Bit, 0, classifyFlag);
            if (!string.IsNullOrEmpty(dateBegin))
            {
                dh.AddPare("@dateBegin", SqlDbType.DateTime, 0, DateTime.Parse(dateBegin));
            }
            if (!string.IsNullOrEmpty(dateEnd))
            {
                dh.AddPare("@dateEnd", SqlDbType.DateTime, 0, DateTime.Parse(dateEnd));
            }
            dh.AddPare("@customer", SqlDbType.NVarChar, 40, customer);
            dh.AddPare("@incomeGrp", SqlDbType.NVarChar, 20, incomeGrp);
            dh.AddPare("@incomeGrpdts", SqlDbType.NVarChar, 40, incomeGrpdts);
            List<T_RecPayRecord> result = new List<T_RecPayRecord>();
            result = dh.Reader<T_RecPayRecord>();
            count = dh.GetParaValue<int>("@Count");
            return result;
        }

        public List<T_RecPayRecord> GetDeclareCustomer(string flag, string C_GUID, int pageIndex, int pageSize, out int count, string invtypedts)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetDeclareCustomer";
            dh.AddPare("@PageIndex", SqlDbType.Int, 0, pageIndex);
            dh.AddPare("@PageSize", SqlDbType.Int, 0, pageSize);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            dh.AddPare("@Flag", SqlDbType.NVarChar, 1, flag);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@InvTypeDts", SqlDbType.NVarChar, 50, invtypedts);
            List<T_RecPayRecord> result = new List<T_RecPayRecord>();
            result = dh.Reader<T_RecPayRecord>();
            count = dh.GetParaValue<int>("@Count");
            return result;
        }

        public List<T_RecPayRecord> GetPaymentDeclareCostSpending(string flag, string C_GUID, int pageIndex, int pageSize, out int count, string invtypedts)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetPaymentDeclareCostSpendingList";
            dh.AddPare("@PageIndex", SqlDbType.Int, 0, pageIndex);
            dh.AddPare("@PageSize", SqlDbType.Int, 0, pageSize);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            dh.AddPare("@Flag", SqlDbType.NVarChar, 1, flag);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@InvTypeDts", SqlDbType.NVarChar, 50, invtypedts);
            List<T_RecPayRecord> result = new List<T_RecPayRecord>();
            result = dh.Reader<T_RecPayRecord>();
            count = dh.GetParaValue<int>("@Count");
            return result;
        }

        /// <summary>
        /// 获取所有收付款纪录列表
        /// </summary>
        /// <param name="C_GUID">公司标识</param>
        /// <param name="flag">收付标识</param>
        /// <returns></returns>
        private List<T_RecPayRecord> GetAllRecPayRecords(string C_GUID, string flag = null)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetRecPayRecordCopy";
            dh.AddPare("@Flag", SqlDbType.NVarChar, 1, flag);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            //dh.AddPare("@IsAll", SqlDbType.Bit, 0, 1);
            return dh.Reader<T_RecPayRecord>();
        }

        /// <summary>
        /// 获取收款纪录
        /// </summary>
        /// <param name="id">收款纪录标识</param>
        /// <param name="C_GUID">公司标识</param>
        /// <returns></returns>
        public T_RecPayRecord GetReceivablesRecord(string id, string C_GUID)
        {
            return GetRecPayRecord(id, C_GUID);
        }

        /// <summary>
        /// 获取应收纪录列表
        /// </summary>
        /// <param name="id">付款方标识</param>
        /// <param name="C_GUID">公司标识</param>
        /// <returns></returns>
        public List<T_Receivables> GetChooseReceivablesRecord(string id, string C_GUID)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetChooseReceivablesRecord";
            dh.AddPare("@RPer", SqlDbType.NVarChar, 40, id);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 40, C_GUID);
            List<T_Receivables> result = new List<T_Receivables>();
            result = dh.Reader<T_Receivables>();
            return result;
        }

        /// <summary>
        /// 获取已销账纪录列表
        /// </summary>
        /// <param name="id">付款方标识</param>
        /// <param name="C_GUID">公司标识</param>
        /// <returns></returns>
        public List<T_RecPayRecord> GetIEUsed(string C_GUID,int pageIndex, int pageSize, out int count, string id) {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetIEUsed";
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@IE_GUID", SqlDbType.NVarChar, 50, id);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            List<T_RecPayRecord> result = new List<T_RecPayRecord>();
            result = dh.Reader<T_RecPayRecord>();
            count = dh.GetParaValue<int>("@Count");
            return result;
        }


        public List<T_IERecord> GetChoosePayablesList(string C_GUID, int pageIndex, int pageSize, out int count, string state, string customer, string invtype, string remark, string DetailInvtype)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetChoosePayablesList";
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@PageIndex", SqlDbType.Int, 0, pageIndex);
            dh.AddPare("@PageSize", SqlDbType.Int, 0, pageSize);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            dh.AddPare("@State", SqlDbType.NVarChar, 50, state);
            dh.AddPare("@customer", SqlDbType.NVarChar, 50, customer);
            dh.AddPare("@Invtype", SqlDbType.NVarChar, 50, invtype);
            dh.AddPare("@DetailInvtype", SqlDbType.NVarChar, 50, DetailInvtype);
            dh.AddPare("@remark", SqlDbType.NVarChar, 50, remark);
            List<T_IERecord> result = new List<T_IERecord>();
            result = dh.Reader<T_IERecord>();
            count = dh.GetParaValue<int>("@Count");
            return result;
        }

        public List<T_IERecord> GetTaxInfoCollection(string C_GUID)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetTaxInfoCollection";
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            List<T_IERecord> result = new List<T_IERecord>();
            result = dh.Reader<T_IERecord>();
            return result;
        }

        public List<T_IERecord> GetAllTaxInfo(string C_GUID)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetAllTaxInfo";
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            List<T_IERecord> result = new List<T_IERecord>();
            result = dh.Reader<T_IERecord>();
            return result;
        }

        public List<T_IERecord> GetTaxInfo(string C_GUID,string Flag)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetTaxInfo";
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@Flag", SqlDbType.NVarChar, 50, Flag);
            List<T_IERecord> result = new List<T_IERecord>();
            result = dh.Reader<T_IERecord>();
            return result;
        }

        /// <summary>
        /// 合并成本外支行直接、间接物料和成本应付纪录列表
        /// </summary>
        /// <param name="id">收款方标识</param>
        /// <param name="C_GUID">公司标识</param>
        /// <param name="page">页索引</param>
        /// <param name="rows">页大小</param>
        /// <param name="count">纪录总数</param>
        /// <returns></returns>
        public List<T_IERecord> GetUnionDPay(string C_GUID, int pageIndex, int pageSize, out int count, string customer, string InvType) 
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_UnionDPay";
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@PageIndex", SqlDbType.Int, 0, pageIndex);
            dh.AddPare("@PageSize", SqlDbType.Int, 0, pageSize);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            dh.AddPare("@customer", SqlDbType.NVarChar, 50, customer);
            dh.AddPare("@InvType", SqlDbType.NVarChar, 50, InvType);
            List<T_IERecord> result = new List<T_IERecord>();
            result = dh.Reader<T_IERecord>();
            count = dh.GetParaValue<int>("@Count");
            return result;
        }

            

        /// <summary>
        /// 获取应付纪录列表
        /// </summary>
        /// <param name="id">收款方标识</param>
        /// <param name="C_GUID">公司标识</param>
        /// <param name="page">页索引</param>
        /// <param name="rows">页大小</param>
        /// <param name="count">纪录总数</param>
        /// <returns></returns>
        public List<T_Payables> GetChoosePayablesRecord(string id, string C_GUID, int page, int rows, out int count,string iegroup=null)
        {
            DBHelper dh = new DBHelper();
            //dh.strCmd = "SP_GetChoosePayablesRecord";
            dh.strCmd = "SP_GetPayableRecord";
            dh.AddPare("@PageIndex", SqlDbType.Int, 0, page);
            dh.AddPare("@PageSize", SqlDbType.Int, 0, rows);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            dh.AddPare("@RPer", SqlDbType.NVarChar, 40, id);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 40, C_GUID);
            dh.AddPare("@IEGroup", SqlDbType.NVarChar, 40, iegroup);
            List<T_Payables> result = new List<T_Payables>();
            result = dh.Reader<T_Payables>();
            count = dh.GetParaValue<int>("@Count");
            return result;
        }

        /// <summary>
        /// 获取付款纪录
        /// </summary>
        /// <param name="id">付款纪录标识</param>
        /// <param name="C_GUID">公司标识</param>
        /// <returns></returns>
        public T_RecPayRecord GetPaymentRecord(string id, string C_GUID)
        {
            return GetRecPayRecord(id, C_GUID);
        }

        /// <summary>
        /// 获取收付纪录
        /// </summary>
        /// <param name="id">收付纪录标识</param>
        /// <param name="C_GUID">公司标识</param>
        /// <returns></returns>
        private T_RecPayRecord GetRecPayRecord(string id, string C_GUID)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetRecPayRecord";
            dh.AddPare("@ID", SqlDbType.NVarChar, 40, id);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            return dh.Reader<T_RecPayRecord>().FirstOrDefault();
        }
        public List<T_RecPayRecord> GetRecPayRecordD(string id, string C_GUID)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetRecPayRecord";
            dh.AddPare("@ID", SqlDbType.NVarChar, 40, id);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            return dh.Reader<T_RecPayRecord>();
        }
        ///<summary>
        ///销账归类
        ///</summary>

        public bool UpdRecpayType(T_RecPayRecord rec) {


            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_CreatRecord";
            dh.AddPare("@Flag", SqlDbType.NVarChar, 1,rec.RP_Flag);
            dh.AddPare("@ID", SqlDbType.NVarChar, 40, rec.RP_GUID);
            dh.AddPare("@Type", SqlDbType.NVarChar, 50, rec.InvType);
            dh.AddPare("@TypeDts", SqlDbType.NVarChar, 50, rec.InvTypeDts);
            dh.AddPare("@CFItemGuid", SqlDbType.NVarChar, 40,rec.CFItemGuid);
            dh.AddPare("@DisAmount", SqlDbType.Decimal, 0, rec.DisAmount1);
            dh.AddPare("@IE_GUID", SqlDbType.NVarChar, 40, rec.IE_GUID);
            dh.AddPare("@record", SqlDbType.NVarChar, 40, rec.Record);
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
        public bool UpRecPayType(T_RecPayRecord rec) {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_UpdInvType";
            dh.AddPare("@Flag", SqlDbType.NVarChar, 1, rec.RP_Flag);
            dh.AddPare("@ID", SqlDbType.NVarChar, 50, rec.RP_GUID);
            dh.AddPare("@Type", SqlDbType.NVarChar, 100, rec.InvType);
            dh.AddPare("@TypeDts", SqlDbType.NVarChar, 100, rec.InvTypeDts);
            dh.AddPare("@CFItemGuid", SqlDbType.NVarChar, 50, rec.CFItemGuid);
            dh.AddPare("@IE_GUID", SqlDbType.NVarChar, 50, rec.IE_GUID);
            dh.AddPare("@SumAmount", SqlDbType.Decimal, 0, rec.SumAmount);
            dh.AddPare("@record", SqlDbType.NVarChar, 40, rec.Record);
            dh.AddPare("@C_GUID",SqlDbType.NVarChar,50,rec.C_GUID);
            
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
        /// 归类
        /// </summary>
        /// <param name="id">收付纪录标识</param>
        /// <param name="invtype">公司标识</param>
        /// <returns></returns>
        public bool UpdInvType(string flag, string id, string invtype, string typedts, string cfitemguid,string ieguid,string record)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_UpdInvType";
            dh.AddPare("@Flag", SqlDbType.NVarChar, 1, flag);
            dh.AddPare("@ID", SqlDbType.NVarChar, 40, id);
            dh.AddPare("@Type", SqlDbType.NVarChar, 50, invtype);
            dh.AddPare("@TypeDts", SqlDbType.NVarChar, 50, typedts);
            dh.AddPare("@CFItemGuid", SqlDbType.NVarChar, 40, cfitemguid);
            dh.AddPare("@IE_GUID", SqlDbType.NVarChar, 40, ieguid);
            dh.AddPare("@record", SqlDbType.NVarChar, 40, record);
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
        /// 更改状态
        /// </summary>
        /// <param name="id">收入纪录标识</param>
        /// <returns></returns>
        public bool UpdIEState(string flag, string id, string state)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_UpdIEState";
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
        /// 
        /// </summary>
        /// <param name="flag"></param>
        /// <param name="id"></param>
        /// <param name="ieguid"></param>
        /// <param name="invtype"></param>
        /// <param name="invtypedts"></param>
        /// <returns></returns>
        public bool UpdRR(string flag, string id, string ieguid, string invtype, string invtypedts,string cfitemguid)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_UpdRR";
            dh.AddPare("@Flag", SqlDbType.NVarChar, 1, flag);
            dh.AddPare("@ID", SqlDbType.NVarChar, 40, id);
            dh.AddPare("@IE_GUID", SqlDbType.NVarChar, 40, ieguid);
            dh.AddPare("@InvType", SqlDbType.NVarChar, 100, invtype);
            dh.AddPare("@InvTypeDts", SqlDbType.NVarChar, 100, invtypedts);
            dh.AddPare("@CFItemGuid", SqlDbType.NVarChar, 40, cfitemguid);
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
        /// 删除收款纪录
        /// </summary>
        /// <param name="id">收款纪录标识</param>
        /// <returns></returns>
        public bool DelReceivablesRecord(string id)
        {
            return DelRecPayRecord(id);
        }

        /// <summary>
        /// 删除付款纪录
        /// </summary>
        /// <param name="id">付款纪录标识</param>
        /// <returns></returns>
        public bool DelPaymentRecord(string id)
        {
            return DelRecPayRecord(id);
        }

        /// <summary>
        /// 删除收付纪录
        /// </summary>
        /// <param name="id">收付纪录标识</param>
        /// <returns></returns>
        private bool DelRecPayRecord(string id)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_DelRecPayRecord";
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
        ///  主营业务收入、非主营业务收入的应收款列表
        /// </summary>
        /// <param name="id">收款方标识</param>
        /// <param name="C_GUID">公司标识</param>
        /// <param name="page">页索引</param>
        /// <param name="rows">页大小</param>
        /// <param name="count">纪录总数</param>
        /// <returns></returns>
        public List<T_IERecord> GetUnionDRec(string C_GUID, int pageIndex, int pageSize, out int count, string customer, string main, string nomain, string other, string investment)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_UnionDRec";
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@PageIndex", SqlDbType.Int, 0, pageIndex);
            dh.AddPare("@PageSize", SqlDbType.Int, 0, pageSize);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            dh.AddPare("@customer", SqlDbType.NVarChar, 50, customer);
            dh.AddPare("@main", SqlDbType.NVarChar, 50, main);
            dh.AddPare("@nomain", SqlDbType.NVarChar, 50, nomain);
            dh.AddPare("@other", SqlDbType.NVarChar, 50, other);
            dh.AddPare("@investment", SqlDbType.NVarChar, 50, investment);
            List<T_IERecord> result = new List<T_IERecord>();
            result = dh.Reader<T_IERecord>();
            count = dh.GetParaValue<int>("@Count");
            return result;
        }
         /// <summary>
        ///  获取净现金流列表（账户部分）
        /// </summary>
        /// <param name="BA_GUID">账户标识</param>
        /// <param name="C_GUID">公司标识</param>
        /// <param name="page">页索引</param>  
        /// <param name="rows">页大小</param>
        /// <param name="count">纪录总数</param>
        /// <returns></returns>
        public List<T_RecPayRecord> GetNetCashFlowsRecordList(string BA_GUID, string C_GUID, int pageIndex, int pageSize, out int count, string dateBegin, string dateEnd)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetNetCashFlowsRecordList";
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@BA_GUID", SqlDbType.NVarChar, 50, BA_GUID);
            dh.AddPare("@PageIndex", SqlDbType.Int, 0, pageIndex);
            dh.AddPare("@PageSize", SqlDbType.Int, 0, pageSize);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);    
            if (!string.IsNullOrEmpty(dateBegin))
            {
                dh.AddPare("@dateBegin", SqlDbType.DateTime, 0, DateTime.Parse(dateBegin));
            }
            if (!string.IsNullOrEmpty(dateEnd))
            {
                dh.AddPare("@dateEnd", SqlDbType.DateTime, 0, DateTime.Parse(dateEnd));
            }
            List<T_RecPayRecord> result = new List<T_RecPayRecord>();
            result = dh.Reader<T_RecPayRecord>();
            count = dh.GetParaValue<int>("@Count");
            return result;
        }
        /// <summary>
        ///  获取净现金流列表（本币汇总）
        /// </summary>
        /// <param name="BA_GUID">账户标识</param>
        /// <param name="C_GUID">公司标识</param>
        /// <param name="page">页索引</param>  
        /// <param name="rows">页大小</param>
        /// <param name="count">纪录总数</param>
        /// <returns></returns>
        public List<T_RecPayRecord> GetNetCashLocalCurrencyList(string BA_GUID, string C_GUID, int pageIndex, int pageSize, out int count, string dateBegin, string dateEnd)
            {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetNetCashLocalCurrencyList";
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@BA_GUID", SqlDbType.NVarChar, 50, BA_GUID);
            dh.AddPare("@PageIndex", SqlDbType.Int, 0, pageIndex);
            dh.AddPare("@PageSize", SqlDbType.Int, 0, pageSize);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            if (!string.IsNullOrEmpty(dateBegin))
            {
                dh.AddPare("@dateBegin", SqlDbType.DateTime, 0, DateTime.Parse(dateBegin));
            }
            if (!string.IsNullOrEmpty(dateEnd))
            {
                dh.AddPare("@dateEnd", SqlDbType.DateTime, 0, DateTime.Parse(dateEnd));
            }
            List<T_RecPayRecord> result = new List<T_RecPayRecord>();
            result = dh.Reader<T_RecPayRecord>();
            count = dh.GetParaValue<int>("@Count");
            return result;
        }
        /// <summary>
        ///  获取净现金流列表（统计货币汇总）
        /// </summary>
        /// <param name="BA_GUID">账户标识</param>
        /// <param name="C_GUID">公司标识</param>
        /// <param name="page">页索引</param>  
        /// <param name="rows">页大小</param>
        /// <param name="count">纪录总数</param>
        /// <returns></returns>
        public List<T_RecPayRecord> GetNetCashStatisticalCurrencyList( string C_GUID, int pageIndex, int pageSize, out int count, string dateBegin, string dateEnd)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetNetCashStatisticalCurrencyList";
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@PageIndex", SqlDbType.Int, 0, pageIndex);
            dh.AddPare("@PageSize", SqlDbType.Int, 0, pageSize);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            if (!string.IsNullOrEmpty(dateBegin))
            {
                dh.AddPare("@dateBegin", SqlDbType.DateTime, 0, DateTime.Parse(dateBegin));
            }
            if (!string.IsNullOrEmpty(dateEnd))
            {
                dh.AddPare("@dateEnd", SqlDbType.DateTime, 0, DateTime.Parse(dateEnd));
            }
            List<T_RecPayRecord> result = new List<T_RecPayRecord>();
            result = dh.Reader<T_RecPayRecord>();
            count = dh.GetParaValue<int>("@Count");
            return result;
        }
        /// <summary>
        /// 获取净现金流帐记录
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public List<T_RecPayRecord> GetCashInFlowsAccountRecordList(string BA_GUID,string C_GUID, int page, int rows, out int count, string dateBegin, string dateEnd)
        {
            return GetCashInFlowsAccountRecordListCount(page, rows, out count, dateBegin, dateEnd, C_GUID, BA_GUID);
        }
        /// <summary>
        ///  获取净现金流帐记录
        /// </summary>
        /// <param name="BA_GUID">账户标识</param>
        /// <param name="C_GUID">公司标识</param>
        /// <param name="page">页索引</param>  
        /// <param name="rows">页大小</param>
        /// <param name="count">纪录总数</param>
        /// <returns></returns>
        private List<T_RecPayRecord> GetCashInFlowsAccountRecordListCount(int pageIndex, int pageSize, out int count, string dateBegin, string dateEnd, string C_GUID, string BA_GUID)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetCashInFlowsAccountRecordList";
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@PageIndex", SqlDbType.Int, 0, pageIndex);
            dh.AddPare("@PageSize", SqlDbType.Int, 0, pageSize);
            dh.AddPare("@BA_GUID", SqlDbType.NVarChar, 50, BA_GUID);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            if (!string.IsNullOrEmpty(dateBegin))
            {
                dh.AddPare("@dateBegin", SqlDbType.DateTime, 0, DateTime.Parse(dateBegin));
            }
            if (!string.IsNullOrEmpty(dateEnd))
            {
                  dh.AddPare("@dateEnd", SqlDbType.DateTime, 0, DateTime.Parse(dateEnd));
            }
            List<T_RecPayRecord> result = new List<T_RecPayRecord>();
            result = dh.Reader<T_RecPayRecord>();
            count = dh.GetParaValue<int>("@Count");
            return result;
        }

        /// <summary>
        /// 获取净现金流帐记录
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public List<T_RecPayRecord> GetCashFlowsCompareRecordList(string BA_GUID, string C_GUID,string dateBegin, string dateEnd)
        {
            return GetCashFlowsCompareRecordListCount(BA_GUID,C_GUID, dateBegin,dateEnd);
        }
        /// <summary>
        ///  获取净现金流帐记录
        /// </summary>
        /// <param name="BA_GUID">账户标识</param>
        /// <param name="C_GUID">公司标识</param>
        /// <param name="page">页索引</param>  
        /// <param name="rows">页大小</param>
        /// <param name="count">纪录总数</param>
        /// <returns></returns>
        private List<T_RecPayRecord> GetCashFlowsCompareRecordListCount(string BA_GUID, string C_GUID, string dateBegin, string dateEnd)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetCashFlowsCompareRecordList";
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@BA_GUID", SqlDbType.NVarChar, 50, BA_GUID);
            if (!string.IsNullOrEmpty(dateBegin))
            {
                dh.AddPare("@dateBegin", SqlDbType.DateTime, 0, DateTime.Parse(dateBegin));
            }
            if (!string.IsNullOrEmpty(dateEnd))
            {
                dh.AddPare("@dateEnd", SqlDbType.DateTime, 0, DateTime.Parse(dateEnd));
            }
            return dh.Reader<T_RecPayRecord>();
        }
        public List<T_RecPayRecord> GetAccountCashInFlowsRecordList(string C_GUID, int page, int rows, out int count, string dateBegin, string dateEnd, string BA_GUID)
        {
            return GetAccountCashInFlowsRecordListCount("R", C_GUID, page, rows, out count, dateBegin, dateEnd, BA_GUID);
        }
        public List<T_RecPayRecord> GetCustomerCashInFlowsRecordList(string C_GUID, int page, int rows, out int count, string dateBegin, string dateEnd, string BA_GUID)
        {
            return GetCustomerCashInFlowsRecordListCount("R", C_GUID, page, rows, out count, dateBegin, dateEnd, BA_GUID);
        }

        public List<T_RecPayRecord> GetAccountOutFlowCashRecordList(string C_GUID, int page, int rows, out int count, string dateBegin, string dateEnd, string BA_GUID)
        {
            return GetAccountCashInFlowsRecordListCount("P", C_GUID, page, rows, out count, dateBegin, dateEnd, BA_GUID);
        }
        public List<T_RecPayRecord> GetSupplierOutFlowCashRecordList(string C_GUID, int page, int rows, out int count, string dateBegin, string dateEnd, string BA_GUID)
        {
            return GetSupplierOutFlowCashRecordListCount("P", C_GUID, page, rows, out count, dateBegin, dateEnd, BA_GUID);
        }

        private List<T_RecPayRecord> GetAccountCashInFlowsRecordListCount(string flag, string C_GUID, int pageIndex, int pageSize, out int count, string dateBegin, string dateEnd, string BA_GUID)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetAccountCashInFlowsRecordList";
            dh.AddPare("@Flag", SqlDbType.NVarChar, 1, flag);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@PageSize", SqlDbType.Int, 0, pageSize);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            if (!string.IsNullOrEmpty(dateBegin))
            {
                dh.AddPare("@dateBegin", SqlDbType.DateTime, 0, DateTime.Parse(dateBegin));
            }
            if (!string.IsNullOrEmpty(dateEnd))
            {
                dh.AddPare("@dateEnd", SqlDbType.DateTime, 0, DateTime.Parse(dateEnd));
            }
            dh.AddPare("@BA_GUID", SqlDbType.NVarChar, 40, BA_GUID);
            List<T_RecPayRecord> result = new List<T_RecPayRecord>();
            result = dh.Reader<T_RecPayRecord>();
            count = dh.GetParaValue<int>("@Count");
            return result;
        }

        private List<T_RecPayRecord> GetSupplierOutFlowCashRecordListCount(string flag, string C_GUID, int pageIndex, int pageSize, out int count, string dateBegin, string dateEnd, string BA_GUID)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetSupplierOutFlowCashRecordList";    
            dh.AddPare("@Flag", SqlDbType.NVarChar, 1, flag);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@PageSize", SqlDbType.Int, 0, pageSize);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            if (!string.IsNullOrEmpty(dateBegin))
            {
                dh.AddPare("@dateBegin", SqlDbType.DateTime, 0, DateTime.Parse(dateBegin));
            }
            if (!string.IsNullOrEmpty(dateEnd))
            {
                dh.AddPare("@dateEnd", SqlDbType.DateTime, 0, DateTime.Parse(dateEnd));
            }
            dh.AddPare("@BA_GUID", SqlDbType.NVarChar, 40, BA_GUID);
            List<T_RecPayRecord> result = new List<T_RecPayRecord>();
            result = dh.Reader<T_RecPayRecord>();
            count = dh.GetParaValue<int>("@Count");
            return result;
        }


        private List<T_RecPayRecord> GetCustomerCashInFlowsRecordListCount(string flag, string C_GUID, int pageIndex, int pageSize, out int count, string dateBegin, string dateEnd, string BA_GUID)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetCustomerCashInFlowsRecordList";
            dh.AddPare("@Flag", SqlDbType.NVarChar, 1, flag);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@PageSize", SqlDbType.Int, 0, pageSize);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            if (!string.IsNullOrEmpty(dateBegin))
            {
                dh.AddPare("@dateBegin", SqlDbType.DateTime, 0, DateTime.Parse(dateBegin));
            }
            if (!string.IsNullOrEmpty(dateEnd))
            {
                dh.AddPare("@dateEnd", SqlDbType.DateTime, 0, DateTime.Parse(dateEnd));
            }
            dh.AddPare("@BA_GUID", SqlDbType.NVarChar, 40, BA_GUID);
            List<T_RecPayRecord> result = new List<T_RecPayRecord>();
            result = dh.Reader<T_RecPayRecord>();
            count = dh.GetParaValue<int>("@Count");
            return result;
        }
        //流入现金比较
        public List<T_RecPayRecord> GetCashInFlowsCompareRecordList(string BA_GUID, string C_GUID, string dateBegin, string dateEnd)
        {
            return GetCashInFlowsCompareRecordListCount(BA_GUID, C_GUID, dateBegin, dateEnd);
        }
        //流入现金比较
        private List<T_RecPayRecord> GetCashInFlowsCompareRecordListCount(string BA_GUID, string C_GUID, string dateBegin, string dateEnd)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetCashInFlowsCompareRecordList";
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@BA_GUID", SqlDbType.NVarChar, 50, BA_GUID);
            if (!string.IsNullOrEmpty(dateBegin))
            {
                dh.AddPare("@dateBegin", SqlDbType.DateTime, 0, DateTime.Parse(dateBegin));
            }
            if (!string.IsNullOrEmpty(dateEnd))
            {
                dh.AddPare("@dateEnd", SqlDbType.DateTime, 0, DateTime.Parse(dateEnd));
            }
            return dh.Reader<T_RecPayRecord>();
        }
        //流出现金比较
        public List<T_RecPayRecord> GetCompareOutFlowCashRecordList(string BA_GUID, string C_GUID, string dateBegin, string dateEnd)
        {
            return GetCompareOutFlowCashRecordListCount(BA_GUID, C_GUID, dateBegin, dateEnd);
        }
        //流出现金比较
        private List<T_RecPayRecord> GetCompareOutFlowCashRecordListCount(string BA_GUID, string C_GUID, string dateBegin, string dateEnd)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetCompareOutFlowCashRecordList";
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@BA_GUID", SqlDbType.NVarChar, 50, BA_GUID);
            if (!string.IsNullOrEmpty(dateBegin))
            {
                dh.AddPare("@dateBegin", SqlDbType.DateTime, 0, DateTime.Parse(dateBegin));
            }
            if (!string.IsNullOrEmpty(dateEnd))
            {
                dh.AddPare("@dateEnd", SqlDbType.DateTime, 0, DateTime.Parse(dateEnd));
            }
            return dh.Reader<T_RecPayRecord>();
        }




         /// <summary>
        /// 获取净现金流帐记录
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public bool backCancelAccount(string id, string flag)
        {
            return backCancelAccountRP(id,flag);
        }
        
        /// <summary>
        /// 反销账
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// 2017/4/5   sunp   update
        /// </remarks>
        private bool backCancelAccountRP(string id ,string flag)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetBackCancelAccount";
            dh.AddPare("@Flag", SqlDbType.NVarChar, 1, flag);
            dh.AddPare("@RP_GUID", SqlDbType.VarChar, 50, id);
            
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