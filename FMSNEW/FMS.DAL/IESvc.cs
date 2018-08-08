
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using FMS.Model;

namespace FMS.DAL
{
    public class IESvc
    {
        /// <summary>
        /// 更新收入纪录
        /// </summary>
        /// <param name="head">收入概要</param>
        /// <param name="list">收入明细</param>
        /// <returns></returns>
        public bool UpdIncomeRecord(T_IERecord form)
        {
            form.IE_Flag = "I";
            return UpdIERecord(form);
        }

        /// <summary>
        /// 更新费用纪录
        /// </summary>
        /// <param name="head">费用概要</param>
        /// <param name="list">费用明细</param>
        /// <returns></returns>
        public bool UpdExpenseRecord(T_IERecord form)
        {
            form.IE_Flag = "E";
            return UpdIERecord(form);
        }

        public bool UpdExpenseInfo(T_IERecord form,string vouchid="")
        {
            return UpdIERecord(form, vouchid);
        }
        /// <summary>
        /// 凭证入流水
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public bool UpdVoucherFL(T_IERecord rec,string Method)
        {
            DBHelper dh = new DBHelper();
            dh.BeginTran();
            try
            {
                dh.strCmd = "SP_UpdIERecord";
                dh.AddPare("@RPer", SqlDbType.NVarChar, 40, rec.RPer);
                dh.AddPare("@IE_GUID", SqlDbType.NVarChar, 40, rec.IE_GUID);
                dh.AddPare("@IE_Flag", SqlDbType.NVarChar, 10, rec.IE_Flag);
                dh.AddPare("@InvType", SqlDbType.NVarChar, 40, rec.InvType);
                dh.AddPare("@IEGroup", SqlDbType.NVarChar, 40, rec.IEGroup);
                dh.AddPare("@DetailInvtype", SqlDbType.NVarChar, 40, rec.DetailInvtype);
                dh.AddPare("@Creator", SqlDbType.NVarChar, 40, rec.Creator);
                dh.AddPare("@Method", SqlDbType.NVarChar, 40, Method);
                dh.AddPare("@C_GUID", SqlDbType.NVarChar, 40, rec.C_GUID);
                dh.AddPare("@SumAmount", SqlDbType.Decimal, 0, rec.SumAmount);
                dh.AddPare("@Amount", SqlDbType.Decimal, 0, rec.Amount);
                dh.AddPare("@Currency", SqlDbType.NVarChar, 20, rec.Currency);
                dh.AddPare("@State", SqlDbType.NVarChar, 40, rec.State);
                dh.AddPare("@AccountID", SqlDbType.NVarChar, 40, rec.AccountID);
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
        /// 更新收入/费用纪录
        /// </summary>
        /// <param name="head">概要</param>
        /// <param name="list">明细</param>
        /// <returns></returns>
        private bool UpdIERecord(T_IERecord rec, string vouchid = "")
        {

            DBHelper dh = new DBHelper();
            dh.BeginTran();
            try
            {
                dh.strCmd = "SP_UpdIERecord";
                if (rec.InvType != "税费计提" && rec.InvType != "应交税金")
                {
                    bool IsCustomer = false;
                    bool IsPartner = false;
                    bool IsSupplier = false;
                    if (rec.IE_Flag == "I")
                    {
                        string BP_GUID = Guid.NewGuid().ToString();
                        string State = null;
                        if (rec.Log_c == "b")
                        {
                            IsCustomer = true;
                            BP_GUID = rec.RPer;
                            State = "customer";
                            DBHelper da = new DBHelper();
                            da.strCmd = "SP_UpdPartner";
                            da.AddPare("@State", SqlDbType.NVarChar, 40, State);
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
                            IsCustomer = true;
                            IsPartner = false;
                            IsSupplier = false;
                            State = "新增";
                            DBHelper da = new DBHelper();
                            da.strCmd = "SP_UpdPartner";
                            da.AddPare("@State", SqlDbType.NVarChar, 40, State);
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



                    if (rec.IE_Flag == "E" || rec.IE_Flag=="SA")
                    {
                        string BP_GUID = Guid.NewGuid().ToString();
                        if (rec.Log_c == "b")
                        {
                            IsSupplier = true;
                            BP_GUID = rec.RPer;
                            DBHelper da = new DBHelper();
                            da.strCmd = "SP_UpdPartner";
                            da.AddPare("@State", SqlDbType.NVarChar, 40, "supplier");
                            da.AddPare("@ID", SqlDbType.NVarChar, 40, BP_GUID);
                            da.AddPare("@Name", SqlDbType.NVarChar, 100, rec.Log);
                            da.AddPare("@ISSupplier", SqlDbType.Bit, 0, IsSupplier);
                            da.AddPare("@C_GUID", SqlDbType.NVarChar, 50, rec.C_GUID);
                            da.NonQuery();
                            da.CleanPara();
                            rec.RPer = BP_GUID;
                        }
                        else if (rec.Log_c == "c")
                        {
                            IsCustomer = false;
                            IsPartner = false;
                            IsSupplier = true;
                            DBHelper da = new DBHelper();
                            da.strCmd = "SP_UpdPartner";
                            da.AddPare("@State", SqlDbType.NVarChar, 40, "更新");
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

                dh.AddPare("@RPer", SqlDbType.NVarChar, 40, rec.RPer);
                dh.AddPare("@IE_GUID", SqlDbType.NVarChar, 40, rec.IE_GUID);
                dh.AddPare("@IE_Flag", SqlDbType.NVarChar, 10, rec.IE_Flag);
                dh.AddPare("@InvType", SqlDbType.NVarChar, 40, rec.InvType);
                dh.AddPare("@IEGroup", SqlDbType.NVarChar, 40, rec.IEGroup);
                dh.AddPare("@IEDescription", SqlDbType.NVarChar, 40, rec.IEDescription);
                dh.AddPare("@InvNo", SqlDbType.NVarChar, 20, rec.InvNo);
                dh.AddPare("@Creator", SqlDbType.NVarChar, 40, rec.Creator);
                dh.AddPare("@C_GUID", SqlDbType.NVarChar, 40, rec.C_GUID);
                if (!rec.AffirmDate.Equals(DateTime.MinValue))
                {
                    dh.AddPare("@AffirmDate", SqlDbType.DateTime, 0, rec.AffirmDate);
                }
                if (!rec.Date.Equals(DateTime.MinValue))
                {
                    dh.AddPare("@Date", SqlDbType.DateTime, 0, rec.Date);
                }
                dh.AddPare("@Business_GUID", SqlDbType.NVarChar, 40, rec.Business_GUID);
                dh.AddPare("@SubBusiness_GUID", SqlDbType.NVarChar, 40, rec.SubBusiness_GUID);
                dh.AddPare("@Amount", SqlDbType.Decimal, 0, rec.Amount);
                //此处应该有对转售的修改，金额是否小于之前的金额
                //add by huangj 2018/05/28
                dh.AddPare("@AccountID", SqlDbType.NVarChar, 50, vouchid);
                dh.AddPare("@RPType", SqlDbType.NVarChar, 50, rec.RPType);
                dh.AddPare("@DetailRPType",SqlDbType.NVarChar,50,rec.DetailRPType);
                dh.AddPare("@IELA_GUID", SqlDbType.NVarChar, 50, rec.IELA_GUID);
                dh.AddPare("@DetailInvtype", SqlDbType.NVarChar, 50, rec.DetailInvtype);
                dh.AddPare("@TaxationAmount", SqlDbType.Decimal, 0, rec.TaxationAmount);
                dh.AddPare("@TaxationType", SqlDbType.NVarChar, 40, rec.TaxationType);
                dh.AddPare("@SumAmount", SqlDbType.Decimal, 0, rec.SumAmount);
                dh.AddPare("@Remark", SqlDbType.NVarChar, 200, rec.Remark);
                dh.AddPare("@Currency", SqlDbType.NVarChar, 20, rec.Currency);
                dh.AddPare("@B_GUID", SqlDbType.NVarChar, 40, rec.B_GUID);
                dh.AddPare("@BA_GUID", SqlDbType.NVarChar, 40, rec.BA_GUID);
                dh.AddPare("@Profit_Name", SqlDbType.NVarChar, 40, rec.Profit_Name);
                dh.AddPare("@State", SqlDbType.NVarChar, 40, rec.State);
                dh.AddPare("@Summary",SqlDbType.NVarChar, 40,rec.Summary);
                dh.NonQuery();
                dh.CleanPara();
                if (rec.IE_Flag == "I")
                {
                    dh.strCmd = "SP_UpdReceivables";
                    dh.AddPare("@R_GUID", SqlDbType.NVarChar, 40, rec.IE_GUID);
                    dh.AddPare("@C_GUID", SqlDbType.NVarChar, 40, rec.C_GUID);
                    dh.AddPare("@Payer", SqlDbType.NVarChar, 40, rec.RPer);
                    if (!rec.AffirmDate.Equals(DateTime.MinValue))
                    {
                        dh.AddPare("@Date", SqlDbType.DateTime, 0, rec.AffirmDate);
                    }
                    dh.AddPare("@InvType", SqlDbType.NVarChar, 40, rec.InvType);
                    dh.AddPare("@InvNo", SqlDbType.NVarChar, 40, rec.InvNo);
                    dh.AddPare("@B_GUID", SqlDbType.NVarChar, 40, rec.B_GUID);
                    dh.AddPare("@BA_GUID", SqlDbType.NVarChar, 40, rec.BA_GUID);
                    dh.AddPare("@Money", SqlDbType.Decimal, 0, rec.Amount + rec.TaxationAmount);
                    dh.AddPare("@Currency", SqlDbType.NVarChar, 5, rec.Currency);
                    dh.NonQuery();

                    DBHelper dc = new DBHelper();
                    string state = "关闭";
                    dc.strCmd = "SP_UpdDeclareCustomerState";
                    dc.AddPare("@ID", SqlDbType.NVarChar, 40, rec.IE_GUID);
                    dc.AddPare("@State", SqlDbType.NVarChar, 40, state);
                    dc.NonQuery();
                    dc.CleanPara();
                }
                else if (rec.IE_Flag == "E" || rec.IE_Flag == "SA")
                {
                    dh.strCmd = "SP_UpdPayable";
                    dh.AddPare("@R_GUID", SqlDbType.NVarChar, 40, rec.IE_GUID);
                    dh.AddPare("@C_GUID", SqlDbType.NVarChar, 40, rec.C_GUID);
                    dh.AddPare("@Payee", SqlDbType.NVarChar, 40, rec.RPer);
                    if (!rec.AffirmDate.Equals(DateTime.MinValue))
                    {
                        dh.AddPare("@Date", SqlDbType.DateTime, 0, rec.AffirmDate);
                    }
                    dh.AddPare("@InvType", SqlDbType.NVarChar, 40, rec.InvType);
                    dh.AddPare("@InvNo", SqlDbType.NVarChar, 40, rec.InvNo);
                    dh.AddPare("@B_GUID", SqlDbType.NVarChar, 40, rec.B_GUID);
                    dh.AddPare("@BA_GUID", SqlDbType.NVarChar, 40, rec.BA_GUID);
                    dh.AddPare("@Money", SqlDbType.Decimal, 0, rec.Amount + rec.TaxationAmount);
                    dh.AddPare("@Currency", SqlDbType.NVarChar, 5, rec.Currency);
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

         public bool UpdRecPayRecord(T_IERecord rec) {
            DBHelper dh = new DBHelper();
            dh.BeginTran();
            try
            {
              dh.strCmd = "SP_UpdRecPayRecord";
            dh.AddPare("@ID", SqlDbType.NVarChar, 40, rec.RP_GUID);
            dh.AddPare("@Flag", SqlDbType.NVarChar, 1, rec.RP_Flag);
            dh.AddPare("@InvType", SqlDbType.NVarChar, 20, rec.RPInvType);
            dh.AddPare("@InvTypeDts", SqlDbType.NVarChar, 50, rec.InvTypeDts);
            dh.AddPare("@Record", SqlDbType.NVarChar, 50, rec.Record);
            dh.AddPare("@InvNo", SqlDbType.NVarChar, 20, rec.InvNo);
            dh.AddPare("@R_Per", SqlDbType.NVarChar, 40, rec.RPer);
            dh.AddPare("@DisAmount", SqlDbType.Decimal, 0, rec.DisAmount);
            dh.AddPare("@Amount", SqlDbType.Decimal, 0, rec.bankAmount);
            dh.AddPare("@Date", SqlDbType.Date, 0, rec.AffirmDate);
            dh.AddPare("@Remark", SqlDbType.NVarChar, 200, rec.Remark);
            dh.AddPare("@Creator", SqlDbType.NVarChar, 40, rec.Creator);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, rec.C_GUID);
            dh.AddPare("@Currency", SqlDbType.NVarChar, 5, rec.Currency);
            dh.AddPare("@CFItemGuid", SqlDbType.NVarChar, 40, rec.CFItemGuid);
            dh.AddPare("@B_GUID", SqlDbType.NVarChar, 40, rec.B_GUID);
            dh.AddPare("@BA_GUID", SqlDbType.NVarChar, 40, rec.BA_GUID);
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


        public List<T_WageCost> GetWageCost(string C_GUID, int pageIndex, int pageSize, out int count, string state, string id)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetWageCostRecord";
            dh.AddPare("@W_GUID", SqlDbType.NVarChar, 50, id);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@PageIndex", SqlDbType.Int, 0, pageIndex);
            dh.AddPare("@PageSize", SqlDbType.Int, 0, pageSize);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            dh.AddPare("@State", SqlDbType.NVarChar, 50, state);
            List<T_WageCost> result = new List<T_WageCost>();
            result = dh.Reader<T_WageCost>();
            count = result.Count;
            return result;
        }

        public List<T_DetailSalary> GetDetailWageCost(string C_GUID, int pageIndex, int pageSize, out int count, string state, string id)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetDetailWageCostRecord";
            dh.AddPare("@W_GUID", SqlDbType.NVarChar, 50, id);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@PageIndex", SqlDbType.Int, 0, pageIndex);
            dh.AddPare("@PageSize", SqlDbType.Int, 0, pageSize);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            dh.AddPare("@State", SqlDbType.NVarChar, 50, state);
            List<T_DetailSalary> result = new List<T_DetailSalary>();
            result = dh.Reader<T_DetailSalary>();
            count = result.Count;
            return result;
        }

        /// <summary>
        /// 20180621添加  获取工资列表
        /// </summary>
        /// <param name="rec"></param>
        /// <param name="strVouchID"></param>
        /// <returns></returns>
        public List<T_DetailSalary> GetWageCostnew(string C_GUID, int pageIndex, int pageSize, out int count, string state, string id)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetWageCost";
            dh.AddPare("@W_GUID", SqlDbType.NVarChar, 50, id);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@PageIndex", SqlDbType.Int, 0, pageIndex);
            dh.AddPare("@PageSize", SqlDbType.Int, 0, pageSize);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            dh.AddPare("@State", SqlDbType.NVarChar, 50, state);
            List<T_DetailSalary> result = new List<T_DetailSalary>();
            result = dh.Reader<T_DetailSalary>();
            count = result.Count;
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rec"></param>
        /// <param name="strVouchID"></param>
        /// <returns></returns>
        public List<T_DetailSalary> GetWageCostInfonew(string C_GUID, string SalaryName, string Name)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetWageCostInfonew";
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@SalaryName", SqlDbType.NVarChar, 50, SalaryName);
            dh.AddPare("@Name", SqlDbType.NVarChar, 50, Name);
            List<T_DetailSalary> result = new List<T_DetailSalary>();
            result = dh.Reader<T_DetailSalary>();
            return result;
        }

        public bool UpdWageCost(T_WageCost rec, string strVouchID)
        {
            DBHelper dh = new DBHelper();
            dh.BeginTran();
            try
            {
                dh.strCmd = "SP_UpdWageCost";
                dh.AddPare("@W_GUID", SqlDbType.NVarChar, 40, rec.W_GUID);
                dh.AddPare("@C_GUID", SqlDbType.NVarChar, 40, rec.C_GUID);
                dh.AddPare("@Employee", SqlDbType.NVarChar, 40, rec.Employee);
                dh.AddPare("@Currency", SqlDbType.NVarChar, 0, rec.Currency);
                dh.AddPare("@State", SqlDbType.NVarChar, 0, rec.State);
                dh.AddPare("@Cash", SqlDbType.Decimal, 0, rec.Cash);
                dh.AddPare("@PersonalTaxes", SqlDbType.Decimal, 0, rec.PersonalTaxes);
                dh.AddPare("@Profit_Name", SqlDbType.NVarChar, 40, rec.Profit_Name);
                dh.AddPare("@SocialSecurity", SqlDbType.Decimal, 0, rec.SocialSecurity);
                dh.AddPare("@Total", SqlDbType.Decimal, 0, rec.Total);
                dh.AddPare("@PayType", SqlDbType.NVarChar, 40, rec.PayType);
                dh.AddPare("@EmployeeWelfare", SqlDbType.Decimal, 0, rec.EmployeeWelfare);
                dh.AddPare("@HousingProvident", SqlDbType.Decimal, 0, rec.HousingProvident);
                dh.AddPare("@TradeUnion", SqlDbType.Decimal, 0, rec.TradeUnion);
                dh.AddPare("@StaffEducation", SqlDbType.Decimal, 0, rec.StaffEducation);
                dh.AddPare("@NonCurrency", SqlDbType.Decimal, 0, rec.NonCurrency);
                dh.AddPare("@DismissWelfare", SqlDbType.Decimal, 0, rec.DismissWelfare);
                dh.AddPare("@SalaryType", SqlDbType.NVarChar, 50, rec.SalaryType);
                dh.AddPare("@BonusAllowance", SqlDbType.Decimal, 0, rec.BonusAllowance);
                dh.AddPare("@VouchID", SqlDbType.NVarChar, 50, strVouchID);
                if (!rec.Date.Equals(DateTime.MinValue))
                {
                    dh.AddPare("@Date", SqlDbType.DateTime, 0, rec.Date);
                }
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

        public bool UpdTaxProvisionRecord(string GUID, string Rep_status)
        {
            DBHelper dh = new DBHelper();
            dh.BeginTran();
            try
            {
                dh.strCmd = "SP_UpdTaxProvisionRecord";
                dh.AddPare("@GUID", SqlDbType.NVarChar, 40, GUID);
                dh.AddPare("@Rep_status", SqlDbType.NVarChar, 40, Rep_status);
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

        public bool UpdVouchWageCost(T_WageCost rec, string strMethod, string vouchID)
        {
            DBHelper dh = new DBHelper();
            dh.BeginTran();
            try
            {
                dh.strCmd = "SP_UpdWageCost";
                dh.AddPare("@W_GUID", SqlDbType.NVarChar, 40, rec.W_GUID);
                dh.AddPare("@C_GUID", SqlDbType.NVarChar, 40, rec.C_GUID);
                dh.AddPare("@Employee", SqlDbType.NVarChar, 40, rec.Employee);
                dh.AddPare("@Currency", SqlDbType.NVarChar, 0, rec.Currency);
                dh.AddPare("@State", SqlDbType.NVarChar, 0, rec.State);
                dh.AddPare("@Cash", SqlDbType.Decimal, 0, rec.Cash);
                dh.AddPare("@PersonalTaxes", SqlDbType.Decimal, 0, rec.PersonalTaxes);
                dh.AddPare("@Profit_Name", SqlDbType.NVarChar, 40, rec.Profit_Name);
                dh.AddPare("@SocialSecurity", SqlDbType.Decimal, 0, rec.SocialSecurity);
                dh.AddPare("@Total", SqlDbType.Decimal, 0, rec.Total);
                dh.AddPare("@PayType", SqlDbType.NVarChar, 40, rec.PayType);
                dh.AddPare("@EmployeeWelfare", SqlDbType.Decimal, 0, rec.EmployeeWelfare);
                dh.AddPare("@HousingProvident", SqlDbType.Decimal, 0, rec.HousingProvident);
                dh.AddPare("@TradeUnion", SqlDbType.Decimal, 0, rec.TradeUnion);
                dh.AddPare("@StaffEducation", SqlDbType.Decimal, 0, rec.StaffEducation);
                dh.AddPare("@NonCurrency", SqlDbType.Decimal, 0, rec.NonCurrency);
                dh.AddPare("@DismissWelfare", SqlDbType.Decimal, 0, rec.DismissWelfare);
                dh.AddPare("@SalaryType", SqlDbType.NVarChar, 50, rec.SalaryType);
                dh.AddPare("@BonusAllowance", SqlDbType.Decimal, 0, rec.BonusAllowance);
                dh.AddPare("@Method", SqlDbType.NVarChar, 50, strMethod);
                dh.AddPare("@VouchID", SqlDbType.NVarChar, 50, vouchID);

                if (!rec.Date.Equals(DateTime.MinValue))
                {
                    dh.AddPare("@Date", SqlDbType.DateTime, 0, rec.Date);
                }
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
        /// 获取收入列表
        /// </summary>
        /// <param name="C_GUID">公司标识</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="count">纪录总数</param>
        /// <returns></returns>
        public List<T_IERecord> GetIncomeList(string C_GUID, int pageIndex, int pageSize, out int count, string dateBegin, string dateEnd, string customer, string status, string incomeGrp)
        {
            return GetIEList("I", C_GUID, pageIndex, pageSize, out count, dateBegin, dateEnd, customer, status, incomeGrp);
        }

        /// <summary>
        /// 获取应收款列表
        /// </summary>
        /// <param name="C_GUID">公司标识</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="count">纪录总数</param>
        /// <returns></returns>
        public List<T_IERecord> GetReceivableList(string C_GUID, int pageIndex, int pageSize, out int count)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetChooseReceivablesRecord";
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@PageIndex", SqlDbType.Int, 0, pageIndex);
            dh.AddPare("@PageSize", SqlDbType.Int, 0, pageSize);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            List<T_IERecord> result = new List<T_IERecord>();
            result = dh.Reader<T_IERecord>();
            count = dh.GetParaValue<int>("@Count");
            return result;
        }

        /// <summary>
        /// 获取应付款列表
        /// </summary>
        /// <param name="C_GUID">公司标识</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="count">纪录总数</param>
        /// <returns></returns>
        public List<T_IERecord> GetPayableList(string C_GUID, int pageIndex, int pageSize, out int count)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetChoosePayablesRecord";
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@PageIndex", SqlDbType.Int, 0, pageIndex);
            dh.AddPare("@PageSize", SqlDbType.Int, 0, pageSize);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            List<T_IERecord> result = new List<T_IERecord>();
            result = dh.Reader<T_IERecord>();
            count = dh.GetParaValue<int>("@Count");
            return result;
        }

        /// <summary>
        /// 获取所有收入列表
        /// </summary>
        /// <param name="C_GUID">公司标识</param>
        /// <returns></returns>
        public List<T_IERecord> GetAllIncomeList(string C_GUID, int page, int rows, out int count, string dateBegin, string dateEnd, string customer, string state, string incomeGrp, string business_GUID, string subBusiness_GUID, string TaxationGUID, string MounthDate)
        {
            return GetAllIEList("I", C_GUID, page, rows, out count, dateBegin, dateEnd, null, null, customer, state, incomeGrp, null, business_GUID, subBusiness_GUID, TaxationGUID, MounthDate);
        }

        public List<T_IERecord> GetAllIncomeList(string C_GUID, int page, int rows, out int count, string dateBegin, string dateEnd, string zdateBegin, string zdateEnd, string customer, string state, string incomeGrp, string currency, string business_GUID, string subBusiness_GUID, string TaxationGUID, string MounthDate)
        {
            return GetAllIEList("I", C_GUID, 1, -1, out count, dateBegin, dateEnd, zdateBegin, zdateEnd, customer, state, incomeGrp, currency, business_GUID, subBusiness_GUID, TaxationGUID, MounthDate);
        }

        /// <summary>
        /// 获取费用列表
        /// </summary>
        /// <param name="C_GUID">公司标识</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="count">纪录总数</param>
        /// <returns></returns>
        public List<T_IERecord> GetExpenseList(string C_GUID, int pageIndex, int pageSize, out int count)
        {
            return GetIEList("E", C_GUID, pageIndex, pageSize, out count, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
        }

        public List<T_IERecord> GetAllExpenseList(string C_GUID, int page, int rows, out int count, string dateBegin, string dateEnd, string customer, string state, string incomeGrp)
        {
            return GetAllIEList("I", C_GUID, page, rows, out count, dateBegin, dateEnd, null, null, customer, state, incomeGrp, null, null, null,null,null);
        }
        /// <summary>
        ///  获取所有费用列表
        /// </summary>
        /// <param name="C_GUID">公司标识</param>
        /// <returns></returns>
        public List<T_IERecord> GetAllExpenseList(string C_GUID, int page, int rows, out int count, string dateBegin, string dateEnd, string zdateBegin, string zdateEnd, string customer, string currency, string state, string incomeGrp, string ieGroup, string business_GUID, string subBusiness_GUID, string TaxationGUID, string MounthDate,string taxName)
        {
            return GetAllIEList("E", C_GUID, page, rows, out count, dateBegin, dateEnd, zdateBegin, zdateEnd, customer, currency, state, incomeGrp, ieGroup, business_GUID, subBusiness_GUID, TaxationGUID,MounthDate,taxName);
        }

        public List<T_TaxSettlement> GetTaxSettlement(string repDate, string C_GUID,string Flag)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetTaxSettlement";
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@repDate", SqlDbType.NVarChar, 50, repDate);
            dh.AddPare("@Flag", SqlDbType.NVarChar, 50, Flag);
            List<T_TaxSettlement> result = new List<T_TaxSettlement>();
            result = dh.Reader<T_TaxSettlement>();
            return result;
        }

        public List<T_DetailTaxSettle> GetDetailTaxSettlement(string param)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetDetailTaxSettlement";
            dh.AddPare("@TaxID", SqlDbType.VarChar, 3000, param);
            List<T_DetailTaxSettle> result = new List<T_DetailTaxSettle>();
            result = dh.Reader<T_DetailTaxSettle>();
            return result;
        }

        public List<T_DetailTaxSettle> GetDetailZZHShui(string TaxID)
            {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetDetailZZHShui";
            dh.AddPare("@TaxID", SqlDbType.VarChar, 3000, TaxID);
            List<T_DetailTaxSettle> result = new List<T_DetailTaxSettle>();
            result = dh.Reader<T_DetailTaxSettle>();
            return result;
        }

        public List<T_DetailTaxSettle> GetDetailTaxByRecID(string param,string taskDetailID)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetDetailTaxByRecID";
            dh.AddPare("@RP_GUID", SqlDbType.NVarChar, 50, param);
            dh.AddPare("@TaskDetailID", SqlDbType.VarChar, 3000, taskDetailID);
            List<T_DetailTaxSettle> result = new List<T_DetailTaxSettle>();
            result = dh.Reader<T_DetailTaxSettle>();
            return result;
        }

        public List<T_WageCost> GetSalaryCollectById(string param)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetWageCostInfo";
            dh.AddPare("@W_GUID", SqlDbType.VarChar, 3000, param);
            List<T_WageCost> result = new List<T_WageCost>();
            result = dh.Reader<T_WageCost>();
            return result;
        } 

        /// <summary>
        /// 获取增值税子类别特殊类别
        /// </summary>
        /// <param name="repDate"></param>
        /// <param name="C_GUID"></param>
        /// <param name="Flag"></param>
        /// <returns></returns>
        public List<T_ThirdAccount> GetZhengZhiDetail(string C_GUID, string name)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetZhengZhiDetail";
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@name", SqlDbType.NVarChar, 50, name);
            List<T_ThirdAccount> result = new List<T_ThirdAccount>();
            result = dh.Reader<T_ThirdAccount>();
            return result;
        }

        /// <summary>
        /// 获取营业税子类别特殊类别
        /// </summary>
        /// <param name="repDate"></param>
        /// <param name="C_GUID"></param>
        /// <param name="Flag"></param>
        /// <returns></returns>
        public List<T_DetailedAccount> GetSalesTaxDetail(string C_GUID, string name)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetSalesTaxDetail";
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@name", SqlDbType.NVarChar, 50, name);
            List<T_DetailedAccount> result = new List<T_DetailedAccount>();
            result = dh.Reader<T_DetailedAccount>();
            return result;
        }

        /// <summary>
        /// 获取应付薪酬子类别特殊类别
        /// </summary>
        /// <param name="repDate"></param>
        /// <param name="C_GUID"></param>
        /// <param name="Flag"></param>
        /// <returns></returns>
        public List<T_DetailedAccount> GetWageRecordDetail(string C_GUID, string name)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetWageRecordDetail";
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@name", SqlDbType.NVarChar, 50, name);
            List<T_DetailedAccount> result = new List<T_DetailedAccount>();
            result = dh.Reader<T_DetailedAccount>();
            return result;
        }

        public List<T_TaxSettlement> CheckTaxSettlement(string repDate)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_CheckTaxSettlement";
            dh.AddPare("@repDate", SqlDbType.NVarChar, 50, repDate);
            List<T_TaxSettlement> result = new List<T_TaxSettlement>();
            result = dh.Reader<T_TaxSettlement>();
            return result;
        }
        public List<T_IERecord> GetIETaxList(string flag,string MounthDate,string C_GUID)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetIETaxList";
            dh.AddPare("@Flag", SqlDbType.NVarChar, 1, flag);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@MounthDate", SqlDbType.NVarChar, 50, MounthDate);
            List<T_IERecord> result = new List<T_IERecord>();
            result = dh.Reader<T_IERecord>();
            return result;

        }

        public List<T_IERecord> GetComIETaxList(string Year, string Quarter, string C_GUID)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetComIETaxList";
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@Year", SqlDbType.NVarChar, 50, Year);
            dh.AddPare("@Quarter", SqlDbType.NVarChar, 50, Quarter);
            List<T_IERecord> result = new List<T_IERecord>();
            result = dh.Reader<T_IERecord>();
            return result;


        }
        /// <summary>
        /// 获取收入\费用列表
        /// </summary>
        /// <param name="flag">收入\费用标识</param>
        /// <param name="C_GUID">公司标识</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="count">纪录总数</param>
        /// <returns></returns>
        private List<T_IERecord> GetIEList(string flag, string C_GUID, int pageIndex, int pageSize, out int count, string dateBegin, string dateEnd, string customer, string status, string incomeGrp)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetIEs";
            dh.AddPare("@Flag", SqlDbType.NVarChar, 1, flag);
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
            dh.AddPare("@customer", SqlDbType.NVarChar, 40, customer);
            dh.AddPare("@status", SqlDbType.Bit, 0, status.Equals("1") ? true : false);
            dh.AddPare("@incomeGrp", SqlDbType.NVarChar, 20, incomeGrp);
            List<T_IERecord> result = new List<T_IERecord>();
            result = dh.Reader<T_IERecord>();
            count = dh.GetParaValue<int>("@Count");
            return result;
        }

        /// <summary>
        /// 获取所有收入\费用列表(包括历史数据)
        /// </summary>
        /// <param name="flag">收入\费用标识</param>
        /// <param name="C_GUID">公司标识</param>
        /// <returns></returns>
        private List<T_IERecord> GetAllIEList(string flag, string C_GUID, int pageIndex, int pageSize, out int count, string dateBegin, string dateEnd, string zdateBegin, string zdateEnd, string customer, string state, string incomeGrp, string currency, string business_GUID, string subBusiness_GUID, string TaxationGUID, string MounthDate)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetIEs";
            dh.AddPare("@Flag", SqlDbType.NVarChar, 1, flag);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            //dh.AddPare("@IsAll", SqlDbType.Bit, 0, true);
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
            if (!string.IsNullOrEmpty(zdateBegin))
            {
                dh.AddPare("@zdateBegin", SqlDbType.DateTime, 0, DateTime.Parse(zdateBegin));
            }
            if (!string.IsNullOrEmpty(zdateEnd))
            {
                dh.AddPare("@zdateEnd", SqlDbType.DateTime, 0, DateTime.Parse(zdateEnd));
            }
            dh.AddPare("@customer", SqlDbType.NVarChar, 40, customer);
            dh.AddPare("@MounthDate", SqlDbType.NVarChar, 40, MounthDate);
            dh.AddPare("@TaxationGUID", SqlDbType.NVarChar, 40, TaxationGUID);
            dh.AddPare("@incomeGrp", SqlDbType.NVarChar, 20, incomeGrp);
            dh.AddPare("@state", SqlDbType.NVarChar, 40, state);
            dh.AddPare("@currency", SqlDbType.NVarChar, 40, currency);
            dh.AddPare("@Business_GUID", SqlDbType.NVarChar, 40, business_GUID);
            dh.AddPare("@SubBusiness_GUID", SqlDbType.NVarChar, 40, subBusiness_GUID);
            List<T_IERecord> result = new List<T_IERecord>();
            result = dh.Reader<T_IERecord>();
            count = dh.GetParaValue<int>("@Count");
            return result;
        }

        private List<T_IERecord> GetAllIEList(string flag, string C_GUID, int pageIndex, int pageSize, out int count, string dateBegin, string dateEnd, string zdateBegin, string zdateEnd, string customer, string currency, string state, string incomeGrp, string ieGroup, string business_GUID, string subBusiness_GUID, string TaxationGUID, string MounthDate,string taxName)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetIEs";
            dh.AddPare("@Flag", SqlDbType.NVarChar, 1, flag);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            //dh.AddPare("@IsAll", SqlDbType.Bit, 0, true);
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
            if (!string.IsNullOrEmpty(zdateBegin))
            {
                dh.AddPare("@zdateBegin", SqlDbType.DateTime, 0, DateTime.Parse(zdateBegin));
            }
            if (!string.IsNullOrEmpty(zdateEnd))
            {
                dh.AddPare("@zdateEnd", SqlDbType.DateTime, 0, DateTime.Parse(zdateEnd));
            }
            dh.AddPare("@customer", SqlDbType.NVarChar, 40, customer);
            dh.AddPare("@MounthDate", SqlDbType.NVarChar, 40, MounthDate);
            dh.AddPare("@TaxationGUID", SqlDbType.NVarChar, 40, TaxationGUID);
            dh.AddPare("@incomeGrp", SqlDbType.NVarChar, 20, incomeGrp);
            dh.AddPare("@ieGroup", SqlDbType.NVarChar, 50, ieGroup);
            dh.AddPare("@currency", SqlDbType.NVarChar, 50, currency);
            dh.AddPare("@state", SqlDbType.NVarChar, 40, state);
            dh.AddPare("@Business_GUID", SqlDbType.NVarChar, 40, business_GUID);
            dh.AddPare("@SubBusiness_GUID", SqlDbType.NVarChar, 40, subBusiness_GUID);
            dh.AddPare("@TaxName", SqlDbType.NVarChar, 40, taxName);
            List<T_IERecord> result = new List<T_IERecord>();
            result = dh.Reader<T_IERecord>();
            count = dh.GetParaValue<int>("@Count");
            return result;
        }

        ///<summary>
        ///计算税费
        ///</summary>

        public bool CreateTaxSettlement(string GUID,string repDate, string Amount, string Rep_status,string C_GUID,string Flag,string State,string TaxName)
        {
            DBHelper dh = new DBHelper();
            dh.BeginTran();
            try
            {
                dh.strCmd = "SP_UpdTaxSettlement";
                dh.AddPare("@GUID", SqlDbType.NVarChar, 40, GUID);
                dh.AddPare("@repDate", SqlDbType.NVarChar, 40,repDate);
                dh.AddPare("@Amount", SqlDbType.Decimal, 0, Amount);
                dh.AddPare("@Rep_status", SqlDbType.NVarChar, 40, Rep_status);
                dh.AddPare("@C_GUID", SqlDbType.NVarChar, 40, C_GUID);
                dh.AddPare("@Flag", SqlDbType.NVarChar, 40, Flag);
                dh.AddPare("@State", SqlDbType.NVarChar, 40, State);
                dh.AddPare("@TaxName", SqlDbType.NVarChar, 40, TaxName);
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
        /// 获取收入\费用纪录
        /// </summary>
        /// <param name="id">收入\费用纪录标识</param>
        /// <param name="C_GUID">公司标识</param>
        /// <returns></returns>
        public T_IERecord GetIE(string id, string C_GUID)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetIE";
            dh.AddPare("@ID", SqlDbType.NVarChar, 40, id);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            return dh.Reader<T_IERecord>().FirstOrDefault();
        }

        public T_IERecord GetVoucherID(string id, string C_GUID)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetVoucherID";
            dh.AddPare("@ID", SqlDbType.NVarChar, 40, id);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            return dh.Reader<T_IERecord>().FirstOrDefault();
        }
        public List<T_IERecord> GetVoucher(int pageIndex, int pageSize, out int count, string id, string flag)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetIEVoucher";
            dh.AddPare("@IE_GUID", SqlDbType.NVarChar, 50, id);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            dh.AddPare("@FLAG", SqlDbType.NVarChar, 1, flag);
            List<T_IERecord> result = new List<T_IERecord>();
            result = dh.Reader<T_IERecord>();
            count = dh.GetParaValue<int>("@Count");
            return result;

        }

        public List<T_Voucher> GetSalaryAccountInfo(string date, string C_GUID, string VourchTyp)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetEmployeeAccountInfo";
            dh.AddPare("@Date", SqlDbType.VarChar, 10, date);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@VourchType", SqlDbType.Int,0,VourchTyp);
            List<T_Voucher> result = new List<T_Voucher>();
            result = dh.Reader<T_Voucher>();
            return result;
        }

        public List<T_IERecord> GetAllVoucher(string date, string C_GUID,string M_GUID,string VouchGuid)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetAllVoucher";
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@M_GUID", SqlDbType.NVarChar, 50, M_GUID);
            dh.AddPare("@MounthDate", SqlDbType.NVarChar, 50, date);
            dh.AddPare("@VoucherID", SqlDbType.NVarChar, 50, VouchGuid);
            List<T_IERecord> result = new List<T_IERecord>();
            result = dh.Reader<T_IERecord>();
            return result;
        }

        public List<T_IERecord> GetIERecord(int pageIndex, int pageSize, out int count, string id, string C_GUID)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetIERecord";
            dh.AddPare("@PageSize", SqlDbType.Int, 0, pageSize);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            dh.AddPare("@ID", SqlDbType.NVarChar, 40, id);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            List<T_IERecord> result = new List<T_IERecord>();
            result = dh.Reader<T_IERecord>();
            count = dh.GetParaValue<int>("@Count");
            return result;
        }
        /// <summary>
        /// 更新商业伙伴
        /// </summary>
        /// <param name="partner">商业伙伴对象</param>
        /// <returns></returns>
        public bool UpdPartner(T_BusinessPartner partner)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_UpdPartner";
            dh.AddPare("@ID", SqlDbType.NVarChar, 40, partner.BP_GUID);
            dh.AddPare("@Name", SqlDbType.NVarChar, 100, partner.Name);
            dh.AddPare("@IsCustomer", SqlDbType.Bit, 0, partner.IsCustomer);
            dh.AddPare("@ISSupplier", SqlDbType.Bit, 0, partner.IsSupplier);
            dh.AddPare("@IsPartner", SqlDbType.Bit, 0, partner.IsPartner);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, partner.C_GUID);
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
        /// 获取收入\费用纪录明细
        /// </summary>
        /// <param name="id">收入\费用纪录标识</param>
        /// <param name="C_GUID">公司标识</param>
        /// <returns></returns>
        public List<T_IEDetails> GetIEDetails(string id, string C_GUID)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetIEDetails";
            dh.AddPare("@IE_ID", SqlDbType.NVarChar, 40, id);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            return dh.Reader<T_IEDetails>();
        }

        /// <summary>
        /// 删除收入\费用纪录
        /// </summary>
        /// <param name="id">收入\费用纪录标识</param>
        /// <param name="flag">收入\费用标识</param>
        /// <returns></returns>
        public bool DelIERecord(string id, string flag)
        {
            DBHelper dh = new DBHelper();

            dh.strCmd = "SP_DelIERecord";
            dh.AddPare("@ID", SqlDbType.NVarChar, 40, id);
            dh.AddPare("@Flag", SqlDbType.NVarChar, 4, flag);
            try
            {
                dh.NonQuery();
                return true;
            }
            catch
            {
                return false;
            }
            //dh.BeginTran();
            //try
            //{
            //    dh.strCmd = "SP_DelIERecord";
            //    dh.CleanPara();
            //    dh.AddPare("@ID", SqlDbType.NVarChar, 40, id);
            //    dh.AddPare("@Flag", SqlDbType.NVarChar, 4, flag);
            //    dh.NonQuery();

            //    dh.strCmd = "SP_DelAttachment";
            //    dh.CleanPara();
            //    dh.AddPare("@FR_ID", SqlDbType.NVarChar, 50, id);
            //    dh.NonQuery();
            //    dh.CommitTran();
            //    return true;
            //}
            //catch (System.Exception e)
            //{
            //    dh.RollBackTran();
            //    return false;
            //}
        }

        /// <summary>
        /// 获取收入/费用
        /// </summary>
        /// <param name="id">IE_id</param>
        /// <param name="C_GUID">公司标识</param>
        /// <param name="flag">收入\费用标识</param>
        /// <returns></returns>
        public List<T_IERecord> GetIncomeToReceivables(string id, string C_GUID, string flag)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetIEs";
            dh.AddPare("@Flag", SqlDbType.NVarChar, 1, flag);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@ID", SqlDbType.NVarChar, 40, id);
            List<T_IERecord> result = new List<T_IERecord>();
            result = dh.Reader<T_IERecord>();
            return result;
        }

        /// <summary>
        /// 直接物料
        /// </summary>
        /// <param name="ID"></param>
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

        public List<T_IERecord> GetDirectMaterialResaleValuePurchasingList(string id, string C_GUID, int page, int rows, out int count, string dateBegin, string dateEnd, string customer, string grp, string state)
        {
            return GetResaleValueAllList(id, page, rows, out count, C_GUID, dateBegin, dateEnd, customer, grp, "D", state);
        }
        /// <summary>
        /// 间接物料
        /// </summary>
        /// <param name="ID"></param>
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

        public List<T_IERecord> GetIndirectMaterialResaleValuePurchasingList(string id, string C_GUID, int page, int rows, out int count, string dateBegin, string dateEnd, string customer, string grp, string state)
        {
            return GetResaleValueAllList(id, page, rows, out count, C_GUID, dateBegin, dateEnd, customer, grp, "I", state);
        }
        /// <summary>
        /// 间接物料
        /// </summary>
        /// <param name="ID"></param>
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

        public List<T_IERecord> GetAssetPurchaseRecordResaleValuePurchasingList(string id, string C_GUID, int page, int rows, out int count, string dateBegin, string dateEnd, string customer, string grp, string state)
        {
            return GetResaleValueAllList(id, page, rows, out count, C_GUID, dateBegin, dateEnd, customer, grp, "A", state);
        }
        /// <summary>
        /// 获取已转售的列表
        /// </summary>
        /// <param name="id"></param>
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
        public List<T_IERecord> GetResaleValueAllList(string id, int pageIndex, int pageSize, out int count, string C_GUID, string dateBegin, string dateEnd, string customer, string grp, string flag, string state)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetIEPersonRecord";
            dh.AddPare("@ID", SqlDbType.NVarChar, 40, id);
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
            List<T_IERecord> result = new List<T_IERecord>();
            result = dh.Reader<T_IERecord>();
            count = dh.GetParaValue<int>("@Count");
            return result;
        }
        /// <summary>
        /// 获取营业外收入列表
        /// </summary>
        /// <param name="C_GUID">公司标识</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="count">纪录总数</param>
        /// <returns></returns>
        public List<T_IERecord> GetNonoperatingIncomeList(string C_GUID, int pageIndex, int pageSize, out int count)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetNonoperatingIncomeList";
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@PageIndex", SqlDbType.Int, 0, pageIndex);
            dh.AddPare("@PageSize", SqlDbType.Int, 0, pageSize);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            List<T_IERecord> result = new List<T_IERecord>();
            result = dh.Reader<T_IERecord>();
            count = dh.GetParaValue<int>("@Count");
            return result;
        }
        /// <summary>
        /// 获取应收款列表(总金额汇总)
        /// </summary>
        /// <param name="C_GUID">公司标识</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="count">纪录总数</param>
        /// <returns></returns>
        public List<T_IERecord> GetAllAmountReceivablesList(string C_GUID, int page, int rows, out int count)
        {
            return GetAllAmountReceivablesListCount(C_GUID, page, rows, out count);
        }
        /// <summary>
        /// 获取应收款列表(总金额汇总)
        /// </summary>
        /// <param name="C_GUID">公司标识</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="count">纪录总数</param>
        /// <returns></returns>
        public List<T_IERecord> GetAllAmountReceivablesListCount(string C_GUID, int pageIndex, int pageSize, out int count)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetAllAmountReceivablesListCount";
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@PageIndex", SqlDbType.Int, 0, pageIndex);
            dh.AddPare("@PageSize", SqlDbType.Int, 0, pageSize);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            List<T_IERecord> result = new List<T_IERecord>();
            result = dh.Reader<T_IERecord>();
            count = dh.GetParaValue<int>("@Count");
            return result;
        }
        /// <summary>
        /// 获取应收款列表
        /// </summary>
        /// <param name="C_GUID"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="count"></param>
        /// <param name="RPer"></param>
        /// <returns></returns>

        public List<T_IERecord> GetTotalAmountReceivablesList(string RPer, string C_GUID, int page, int rows, out int count)
        {
            return GetTotalAmountReceivablesListCount(RPer, C_GUID, page, rows, out count);
        }
        /// <summary>
        /// 获取应收款列表
        /// </summary>
        /// <param name="C_GUID">公司标识</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="count">纪录总数</param>
        /// <returns></returns>
        public List<T_IERecord> GetTotalAmountReceivablesListCount(string RPer, string C_GUID, int pageIndex, int pageSize, out int count)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetTotalAmountReceivablesListCount";
            dh.AddPare("@RPer", SqlDbType.NVarChar, 50, RPer);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@PageIndex", SqlDbType.Int, 0, pageIndex);
            dh.AddPare("@PageSize", SqlDbType.Int, 0, pageSize);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            List<T_IERecord> result = new List<T_IERecord>();
            result = dh.Reader<T_IERecord>();
            count = dh.GetParaValue<int>("@Count");
            return result;
        }
        /// <summary>
        /// 获取客户逾期应收款（汇总）
        /// </summary>
        /// <param name="C_GUID"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="count"></param>
        /// <param name="RPer"></param>
        /// <returns></returns>

        public List<T_IERecord> GetAllAmountOverdueRList(string C_GUID, int page, int rows, out int count)
        {
            return GetAllAmountOverdueRListCount(C_GUID, page, rows, out count);
        }
        /// <summary>
        /// 获取客户逾期应收款（汇总）
        /// </summary>
        /// <param name="C_GUID">公司标识</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="count">纪录总数</param>
        /// <returns></returns>
        public List<T_IERecord> GetAllAmountOverdueRListCount(string C_GUID, int pageIndex, int pageSize, out int count)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetAllAmountOverdueRListCount";
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@PageIndex", SqlDbType.Int, 0, pageIndex);
            dh.AddPare("@PageSize", SqlDbType.Int, 0, pageSize);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            List<T_IERecord> result = new List<T_IERecord>();
            result = dh.Reader<T_IERecord>();
            count = dh.GetParaValue<int>("@Count");
            return result;
        }
        /// <summary>
        /// 获取客户逾期应收款
        /// </summary>
        /// <param name="C_GUID"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="count"></param>
        /// <param name="RPer"></param>
        /// <returns></returns>

        public List<T_IERecord> GetTotalAmountOverdueRList(string RPer, string C_GUID, int page, int rows, out int count)
        {
            return GetTotalAmountOverdueRListCount(RPer, C_GUID, page, rows, out count);
        }
        /// <summary>
        /// 获取客户逾期应收款
        /// </summary>
        /// <param name="C_GUID">公司标识</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="count">纪录总数</param>
        /// <returns></returns>
        public List<T_IERecord> GetTotalAmountOverdueRListCount(string RPer, string C_GUID, int pageIndex, int pageSize, out int count)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetTotalAmountOverdueRListCount";
            dh.AddPare("@RPer", SqlDbType.NVarChar, 50, RPer);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@PageIndex", SqlDbType.Int, 0, pageIndex);
            dh.AddPare("@PageSize", SqlDbType.Int, 0, pageSize);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            List<T_IERecord> result = new List<T_IERecord>();
            result = dh.Reader<T_IERecord>();
            count = dh.GetParaValue<int>("@Count");
            return result;
        }
        /// <summary>
        /// 获取客户m天到n天逾期应收款总金额
        /// </summary>
        /// <param name="C_GUID"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="count"></param>
        /// <param name="RPer"></param>
        /// <returns></returns>

        public List<T_IERecord> GetTotalTodayAmountOverdueRList(string dateBegin, string dateEnd, string RPer, string C_GUID, int page, int rows, out int count)
        {
            return GetTotalTodayAmountOverdueRListCount(dateBegin, dateEnd, RPer, C_GUID, page, rows, out count);
        }
        /// <summary>
        /// 获取客户m天到n天逾期应收款总金额
        /// </summary>
        /// <param name="C_GUID">公司标识</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="count">纪录总数</param>
        /// <returns></returns>
        public List<T_IERecord> GetTotalTodayAmountOverdueRListCount(string dateBegin, string dateEnd, string RPer, string C_GUID, int pageIndex, int pageSize, out int count)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetTotalTodayAmountOverdueRListCount";
            dh.AddPare("@RPer", SqlDbType.NVarChar, 50, RPer);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@PageIndex", SqlDbType.Int, 0, pageIndex);
            dh.AddPare("@PageSize", SqlDbType.Int, 0, pageSize);
            dh.AddPare("@dateBegin", SqlDbType.NVarChar, 50, dateBegin);
            dh.AddPare("@dateEnd", SqlDbType.NVarChar, 50, dateEnd);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            List<T_IERecord> result = new List<T_IERecord>();
            result = dh.Reader<T_IERecord>();
            count = dh.GetParaValue<int>("@Count");
            return result;
        }
        /// <summary>
        /// 获取一级费用科目分类的总成本与费用汇总行
        /// </summary>
        /// <param name="C_GUID"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<T_IERecord> GetOnceTotalCollectList(string C_GUID,int page,int rows,out int count) 
        {
            return GetOnceTotalCollectListCount(C_GUID,page,rows,out count);
        }

        /// <summary>
        /// 查询获取一级费用科目分类的总成本与费用汇总行
        /// </summary>
        /// <param name="C_GUID"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private List<T_IERecord> GetOnceTotalCollectListCount(string C_GUID,int pageIndex, int pageSize, out int count)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetOnceTotalCollectListCount";
            dh.AddPare("@pageIndex", SqlDbType.Int, 0, pageIndex);
            dh.AddPare("@pageSize", SqlDbType.Int, 0, pageSize);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            List<T_IERecord> result = new List<T_IERecord>();
            result = dh.Reader<T_IERecord>();
            count = dh.GetParaValue<int>("@Count");
            return result;
        }
        /// <summary>
        /// 获取一级费用科目分类的总成本与费用列表
        /// </summary>
        /// <param name="C_GUID"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<T_IERecord> GetOnceTotalList( string dateBegin, string dateEnd,string C_GUID, int page, int rows, out int count)
        {
            return GetOnceTotalListCount( dateBegin, dateEnd,C_GUID, page, rows, out count);
        }

        /// <summary>
        /// 查询获取一级费用科目分类的总成本与费用列表
        /// </summary>
        /// <param name="C_GUID"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="count"></param>
        /// <param name="datebegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        private List<T_IERecord> GetOnceTotalListCount( string dateBegin, string dateEnd,string C_GUID, int pageIndex, int pageSize, out int count)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetOnceTotalListCount";
            dh.AddPare("@pageIndex", SqlDbType.Int, 0, pageIndex);
            dh.AddPare("@pageSize", SqlDbType.Int, 0, pageSize);
            dh.AddPare("@dateBegin", SqlDbType.DateTime, 0, DateTime.Parse(dateBegin));
            dh.AddPare("@dateEnd", SqlDbType.DateTime, 0, DateTime.Parse(dateEnd));
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            List<T_IERecord> result = new List<T_IERecord>();
            result = dh.Reader<T_IERecord>();
            count = dh.GetParaValue<int>("@Count");
            return result;
        }
        /// <summary>
        /// 获取二级费用科目分类的总成本与费用列表
        /// </summary>
        /// <param name="C_GUID"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<T_IERecord> GetSecondTotalList(string dateBegin, string dateEnd, string C_GUID, int page, int rows, out int count)
        {
            return GetSecondTotalListCount(dateBegin, dateEnd, C_GUID, page, rows, out count);
        }

        /// <summary>
        /// 查询获取二级费用科目分类的总成本与费用列表
        /// </summary>
        /// <param name="C_GUID"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="count"></param>
        /// <param name="datebegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        private List<T_IERecord> GetSecondTotalListCount(string dateBegin, string dateEnd, string C_GUID, int pageIndex, int pageSize, out int count)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetSecondTotalListCount";
            dh.AddPare("@pageIndex", SqlDbType.Int, 0, pageIndex);
            dh.AddPare("@pageSize", SqlDbType.Int, 0, pageSize);
            dh.AddPare("@dateBegin", SqlDbType.DateTime, 0, DateTime.Parse(dateBegin));
            dh.AddPare("@dateEnd", SqlDbType.DateTime, 0, DateTime.Parse(dateEnd));
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            List<T_IERecord> result = new List<T_IERecord>();
            result = dh.Reader<T_IERecord>();
            count = dh.GetParaValue<int>("@Count");
            return result;
        }
        /// <summary>
        /// 获取总成本与费用的比较
        /// </summary>
        /// <param name="C_GUID"></param>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        public List<T_IERecord> GetCompareTotalList(string C_GUID,string dateBegin, string dateEnd)
        {
            return GetCompareTotalListCount(C_GUID,dateBegin, dateEnd);
        }
        /// <summary>
        /// 获取总成本与费用的比较
        /// </summary>
        /// <param name="C_GUID"></param>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        private List<T_IERecord> GetCompareTotalListCount(string C_GUID,string dateBegin, string dateEnd)
        { 
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetCompareTotalListCount";
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            if (!string.IsNullOrEmpty(dateBegin))
            {
                dh.AddPare("@dateBegin", SqlDbType.DateTime, 0, DateTime.Parse(dateBegin));
            }
            if (!string.IsNullOrEmpty(dateEnd))
            {
                dh.AddPare("@dateEnd", SqlDbType.DateTime, 0, DateTime.Parse(dateEnd));
            }
            return dh.Reader<T_IERecord>();        
        }
        /// <summary>
        /// 获取一级费用科目分类的总成本与费用汇总行
        /// </summary>
        /// <param name="C_GUID"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<T_IERecord> GetOnceClassifyTotalCollectList(string InvType,string dateBegin,string dateEnd,string C_GUID, int page, int rows, out int count)
        {
            return GetOnceClassifyTotalCollectListCount(InvType,dateBegin,dateEnd,C_GUID, page, rows, out count);
        }

        /// <summary>
        /// 查询获取一级费用科目分类的总成本与费用汇总行
        /// </summary>
        /// <param name="C_GUID"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private List<T_IERecord> GetOnceClassifyTotalCollectListCount(string InvType,string dateBegin,string dateEnd, string C_GUID, int pageIndex, int pageSize, out int count)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetOnceClassifyTotalCollectListCount";
            dh.AddPare("@pageIndex", SqlDbType.Int, 0, pageIndex);
            dh.AddPare("@pageSize", SqlDbType.Int, 0, pageSize);
            dh.AddPare("@dateBegin", SqlDbType.DateTime, 0, DateTime.Parse(dateBegin));
            dh.AddPare("@dateEnd", SqlDbType.DateTime, 0, DateTime.Parse(dateEnd));
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@InvType", SqlDbType.NVarChar, 50, InvType);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            List<T_IERecord> result = new List<T_IERecord>();
            result = dh.Reader<T_IERecord>();
            count = dh.GetParaValue<int>("@Count");
            return result;
        }
        /// <summary>
        /// 获取一级费用科目分类的总成本与费用列表
        /// </summary>
        /// <param name="C_GUID"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<T_IERecord> GetOnceClassifyTotalList(string InvType, string dateBegin, string dateEnd, string C_GUID, int page, int rows, out int count)
        {
            return GetOnceClassifyTotalListCount(InvType, dateBegin, dateEnd, C_GUID, page, rows, out count);
        }

        /// <summary>
        /// 查询获取一级费用科目分类的总成本与费用列表
        /// </summary>
        /// <param name="C_GUID"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="count"></param>
        /// <param name="datebegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        private List<T_IERecord> GetOnceClassifyTotalListCount(string InvType, string dateBegin, string dateEnd, string C_GUID, int pageIndex, int pageSize, out int count)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetOnceClassifyTotalListCount";
            dh.AddPare("@pageIndex", SqlDbType.Int, 0, pageIndex);
            dh.AddPare("@pageSize", SqlDbType.Int, 0, pageSize);
            dh.AddPare("@dateBegin", SqlDbType.DateTime, 0, DateTime.Parse(dateBegin));
            dh.AddPare("@dateEnd", SqlDbType.DateTime, 0, DateTime.Parse(dateEnd));
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@InvType", SqlDbType.NVarChar, 50, InvType);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            List<T_IERecord> result = new List<T_IERecord>();
            result = dh.Reader<T_IERecord>();
            count = dh.GetParaValue<int>("@Count");
            return result;
        }
        /// <summary>
        /// 获取一级费用科目分类的成本与费用比较
        /// </summary>
        /// <param name="C_GUID"></param>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        public List<T_IERecord> GetOnceClassifyCompareList(string InvType,string C_GUID, string dateBegin, string dateEnd)
        {
            return GetOnceClassifyCompareListCount(InvType, C_GUID, dateBegin, dateEnd);
        }
        /// <summary>
        /// 获取一级费用科目分类的成本与费用比较
        /// </summary>
        /// <param name="C_GUID"></param>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        private List<T_IERecord> GetOnceClassifyCompareListCount(string InvType, string C_GUID, string dateBegin, string dateEnd)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetOnceClassifyCompareListCount";
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@InvType", SqlDbType.NVarChar, 50, InvType);
            if (!string.IsNullOrEmpty(dateBegin))
            {
                dh.AddPare("@dateBegin", SqlDbType.DateTime, 0, DateTime.Parse(dateBegin));
            }
            if (!string.IsNullOrEmpty(dateEnd))
            {
                dh.AddPare("@dateEnd", SqlDbType.DateTime, 0, DateTime.Parse(dateEnd));
            }
            return dh.Reader<T_IERecord>();
        }
        /// <summary>
        /// 获取二级费用科目分类的成本与费用汇总行
        /// </summary>
        /// <param name="C_GUID"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<T_IERecord> GetSecondClassifyTotalCollectList(string IEGroup, string dateBegin, string dateEnd,string C_GUID, int page, int rows, out int count)
        {
            return GetSecondClassifyTotalCollectListCount(IEGroup,dateBegin,dateEnd,C_GUID, page, rows, out count);
        }

        /// <summary>
        /// 查询获取二级费用科目分类的成本与费用汇总行
        /// </summary>
        /// <param name="C_GUID"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private List<T_IERecord> GetSecondClassifyTotalCollectListCount(string IEGroup, string dateBegin, string dateEnd, string C_GUID, int pageIndex, int pageSize, out int count)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetSecondClassifyTotalCollectListCount";
            dh.AddPare("@pageIndex", SqlDbType.Int, 0, pageIndex);
            dh.AddPare("@pageSize", SqlDbType.Int, 0, pageSize);
            dh.AddPare("@dateBegin", SqlDbType.DateTime, 0, DateTime.Parse(dateBegin));
            dh.AddPare("@dateEnd", SqlDbType.DateTime, 0, DateTime.Parse(dateEnd));
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@IEGroup", SqlDbType.NVarChar, 50, IEGroup);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            List<T_IERecord> result = new List<T_IERecord>();
            result = dh.Reader<T_IERecord>();
            count = dh.GetParaValue<int>("@Count"); 
            return result;
        }
        /// <summary>
        /// 获取二级费用科目分类的成本与费用列表
        /// </summary>
        /// <param name="C_GUID"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<T_IERecord> GetSecondClassifyTotalList(string IEGroup, string dateBegin, string dateEnd, string C_GUID, int page, int rows, out int count)
        {
            return GetSecondClassifyTotalListCount(IEGroup, dateBegin, dateEnd, C_GUID, page, rows, out count);
        }

        /// <summary>
        /// 查询获取二级费用科目分类的成本与费用列表
        /// </summary>
        /// <param name="C_GUID"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="count"></param>
        /// <param name="datebegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        private List<T_IERecord> GetSecondClassifyTotalListCount(string IEGroup, string dateBegin, string dateEnd, string C_GUID, int pageIndex, int pageSize, out int count)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetSecondClassifyTotalListCount";
            dh.AddPare("@pageIndex", SqlDbType.Int, 0, pageIndex);
            dh.AddPare("@pageSize", SqlDbType.Int, 0, pageSize);
            dh.AddPare("@dateBegin", SqlDbType.DateTime, 0, DateTime.Parse(dateBegin));
            dh.AddPare("@dateEnd", SqlDbType.DateTime, 0, DateTime.Parse(dateEnd));
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@IEGroup", SqlDbType.NVarChar, 50, IEGroup);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            List<T_IERecord> result = new List<T_IERecord>();
            result = dh.Reader<T_IERecord>();
            count = dh.GetParaValue<int>("@Count");
            return result;
        }
        /// <summary>
        /// 获取一级费用科目分类的成本与费用比较
        /// </summary>
        /// <param name="C_GUID"></param>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        public List<T_IERecord> GetSecondClassifyCompareList(string IEGroup, string C_GUID, string dateBegin, string dateEnd)
        {
            return GetSecondClassifyCompareListCount(IEGroup, C_GUID, dateBegin, dateEnd);
        }
        /// <summary>
        /// 获取一级费用科目分类的成本与费用比较
        /// </summary>
        /// <param name="C_GUID"></param>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        private List<T_IERecord> GetSecondClassifyCompareListCount(string IEGroup, string C_GUID, string dateBegin, string dateEnd)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetSecondClassifyCompareListCount";
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@IEGroup", SqlDbType.NVarChar, 50, IEGroup);
            if (!string.IsNullOrEmpty(dateBegin))
            {
                dh.AddPare("@dateBegin", SqlDbType.DateTime, 0, DateTime.Parse(dateBegin));
            }
            if (!string.IsNullOrEmpty(dateEnd))
            {
                dh.AddPare("@dateEnd", SqlDbType.DateTime, 0, DateTime.Parse(dateEnd));
            }
            return dh.Reader<T_IERecord>();
        }
        /// <summary>
        /// 获取一级分类下面二级费用科目分类的成本与费用汇总行
        /// </summary>
        /// <param name="C_GUID"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<T_IERecord> GetOnceSonClassifyTotalCollectList(string InvType,string IEGroup, string dateBegin, string dateEnd, string C_GUID, int page, int rows, out int count)
        {
            return GetOnceSonClassifyTotalCollectListCount(InvType,IEGroup, dateBegin, dateEnd, C_GUID, page, rows, out count);
        }
        /// <summary>
        /// 查询获取一级分类下面二级费用科目分类的成本与费用汇总行
        /// </summary>
        /// <param name="C_GUID"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private List<T_IERecord> GetOnceSonClassifyTotalCollectListCount(string InvType,string IEGroup, string dateBegin, string dateEnd, string C_GUID, int pageIndex, int pageSize, out int count)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetOnceSonClassifyTotalCollectListCount";   
            dh.AddPare("@pageIndex", SqlDbType.Int, 0, pageIndex);
            dh.AddPare("@pageSize", SqlDbType.Int, 0, pageSize);
            dh.AddPare("@dateBegin", SqlDbType.DateTime, 0, DateTime.Parse(dateBegin));
            dh.AddPare("@dateEnd", SqlDbType.DateTime, 0, DateTime.Parse(dateEnd));
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@InvType", SqlDbType.NVarChar, 50, InvType);
            dh.AddPare("@IEGroup", SqlDbType.NVarChar, 50, IEGroup);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            List<T_IERecord> result = new List<T_IERecord>();
            result = dh.Reader<T_IERecord>();
            count = dh.GetParaValue<int>("@Count");
            return result;
        }
        /// <summary>
        /// 获取一级分类下面二级费用科目分类的成本与费用列表
        /// </summary>
        /// <param name="C_GUID"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<T_IERecord> GetOnceSonClassifyTotalList(string InvType,string IEGroup, string dateBegin, string dateEnd, string C_GUID, int page, int rows, out int count)
        {
            return GetOnceSonClassifyTotalListCount(InvType, IEGroup, dateBegin, dateEnd, C_GUID, page, rows, out count);
        }

        /// <summary>
        /// 获取一级分类下面二级费用科目分类的成本与费用列表
        /// </summary>
        /// <param name="C_GUID"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="count"></param>
        /// <param name="datebegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        private List<T_IERecord> GetOnceSonClassifyTotalListCount(string InvType, string IEGroup, string dateBegin, string dateEnd, string C_GUID, int pageIndex, int pageSize, out int count)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetOnceSonClassifyTotalListCount";
            dh.AddPare("@pageIndex", SqlDbType.Int, 0, pageIndex);
            dh.AddPare("@pageSize", SqlDbType.Int, 0, pageSize);
            dh.AddPare("@dateBegin", SqlDbType.DateTime, 0, DateTime.Parse(dateBegin));
            dh.AddPare("@dateEnd", SqlDbType.DateTime, 0, DateTime.Parse(dateEnd));
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@InvType", SqlDbType.NVarChar, 50, InvType);
            dh.AddPare("@IEGroup", SqlDbType.NVarChar, 50, IEGroup);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            List<T_IERecord> result = new List<T_IERecord>();
            result = dh.Reader<T_IERecord>();
            count = dh.GetParaValue<int>("@Count");
            return result;
        }
        /// <summary>
        /// 查询供应商成本与费用汇总
        /// </summary>
        /// <param name="C_GUID"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<T_IERecord> GetSupplierTotalCollectList( string C_GUID,int page, int rows, out int count)
        {
            return GetSupplierTotalCollectListCount( C_GUID,page, rows, out count);
        }
        /// <summary>
        /// 查询供应商成本与费用汇总
        /// </summary>
        /// <param name="C_GUID"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private List<T_IERecord> GetSupplierTotalCollectListCount(string C_GUID,int pageIndex, int pageSize, out int count)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetSupplierTotalCollectListCount";
            dh.AddPare("@pageIndex", SqlDbType.Int, 0, pageIndex);
            dh.AddPare("@pageSize", SqlDbType.Int, 0, pageSize);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            List<T_IERecord> result = new List<T_IERecord>();
            result = dh.Reader<T_IERecord>();
            count = dh.GetParaValue<int>("@Count");
            return result;
        }
        /// <summary>
        /// 查询供应商成本与费用列表
        /// </summary>
        /// <param name="C_GUID"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<T_IERecord> GetSupplierTotalList(string RPer, string C_GUID, string dateBegin, string dateEnd, int page, int rows, out int count)
        {
            return GetSupplierTotalListCount(RPer, dateBegin, dateEnd, C_GUID, page, rows, out count);
        }

        /// <summary>
        /// 获取供应商成本与费用列表
        /// </summary>
        /// <param name="C_GUID"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="count"></param>
        /// <param name="datebegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        private List<T_IERecord> GetSupplierTotalListCount(string RPer, string dateBegin, string dateEnd, string C_GUID, int pageIndex, int pageSize, out int count)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetSupplierTotalListCount";
            dh.AddPare("@pageIndex", SqlDbType.Int, 0, pageIndex);
            dh.AddPare("@pageSize", SqlDbType.Int, 0, pageSize);
            dh.AddPare("@dateBegin", SqlDbType.DateTime, 0, DateTime.Parse(dateBegin));
            dh.AddPare("@dateEnd", SqlDbType.DateTime, 0, DateTime.Parse(dateEnd));
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@RPer", SqlDbType.NVarChar, 50, RPer);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            List<T_IERecord> result = new List<T_IERecord>();
            result = dh.Reader<T_IERecord>();
            count = dh.GetParaValue<int>("@Count");
            return result;
        }
        /// <summary>
        /// 获取一级分类下面二级分类费用科目分类的成本与费用比较
        /// </summary>
        /// <param name="C_GUID"></param>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        public List<T_IERecord> GetOnceSonClassifyCompareList(string InvType,string IEGroup, string C_GUID, string dateBegin, string dateEnd)
        {
            return GetOnceSonClassifyCompareListCount(InvType,IEGroup, C_GUID, dateBegin, dateEnd);
        }
        /// <summary>
        /// 获取一级分类下面二级分类费用科目分类的成本与费用比较
        /// </summary>
        /// <param name="C_GUID"></param>
        /// <param name="dateBegin"></param>
        /// <param name="dateEnd"></param>
        /// <returns></returns>
        private List<T_IERecord> GetOnceSonClassifyCompareListCount(string Invtype,string IEGroup, string C_GUID, string dateBegin, string dateEnd)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetOnceSonClassifyCompareListCount";
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@Invtype", SqlDbType.NVarChar, 50, Invtype);
            dh.AddPare("@IEGroup", SqlDbType.NVarChar, 50, IEGroup);
            if (!string.IsNullOrEmpty(dateBegin))
            {
                dh.AddPare("@dateBegin", SqlDbType.DateTime, 0, DateTime.Parse(dateBegin));
            }
            if (!string.IsNullOrEmpty(dateEnd))
            {
                dh.AddPare("@dateEnd", SqlDbType.DateTime, 0, DateTime.Parse(dateEnd));
            }
            return dh.Reader<T_IERecord>();
        }

        /// <summary>
        /// 增值税
        /// </summary>
        ///
        /// <returns></returns>
        public bool AddTaxProvisionRecord(string Amount, string Date, string Name,string inputtax, string outputtax, string exportreduce, string transfertax, string exporttax, string payingtax, string C_GUID)
        {
            DBHelper dh = new DBHelper();
            dh.BeginTran();
            try
            {
                dh.strCmd = "SP_AddTaxProvisionRecord";
                dh.AddPare("@Amount", SqlDbType.Decimal, 0, Amount);
                dh.AddPare("@Date", SqlDbType.NVarChar, 50, Date);
                dh.AddPare("@Name", SqlDbType.NVarChar, 50, Name);
                dh.AddPare("@inputtax", SqlDbType.Decimal, 0, inputtax);
                dh.AddPare("@outputtax", SqlDbType.Decimal, 0, outputtax);
                dh.AddPare("@exportreduce", SqlDbType.Decimal, 0, exportreduce);
                dh.AddPare("@transfertax", SqlDbType.Decimal, 0, transfertax);
                dh.AddPare("@exporttax", SqlDbType.Decimal, 0, exporttax);
                dh.AddPare("@payingtax", SqlDbType.Decimal, 0, payingtax);
                dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
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

        public bool AddVTFLAddSalesTaxRecord(T_TaxSettlement record, string Method)
        {
            DBHelper dh = new DBHelper();
            dh.BeginTran();
            try
            {
                dh.strCmd = "SP_AddSalesTaxRecord";
                dh.AddPare("@Amount", SqlDbType.Decimal, 0, record.Amount);
                dh.AddPare("@Excise", SqlDbType.Decimal, 0, record.Excise);
                dh.AddPare("@EducationFee", SqlDbType.Decimal, 0, record.EducationFee);
                dh.AddPare("@Sales", SqlDbType.Decimal, 0, record.Sales);
                dh.AddPare("@UrbanConstruction", SqlDbType.Decimal, 0, record.UrbanConstruction);
                dh.AddPare("@Resource", SqlDbType.Decimal, 0, record.Resource);
                dh.AddPare("@LandValue", SqlDbType.Decimal, 0, record.LandValue);
                dh.AddPare("@UrbanLand", SqlDbType.Decimal, 0, record.UrbanLand);
                dh.AddPare("@Property", SqlDbType.Decimal, 0, record.Property);
                dh.AddPare("@VehicleVessel", SqlDbType.Decimal, 0, record.VehicleVessel);
                dh.AddPare("@MineralResources", SqlDbType.Decimal, 0, record.MineralResources);
                dh.AddPare("@Dischargefee", SqlDbType.Decimal, 0, record.Dischargefee);
                dh.AddPare("@Method", SqlDbType.NVarChar, 50, Method);
                dh.AddPare("@DisAmount", SqlDbType.Decimal, 0, record.DisAmount);
                dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, record.C_GUID);
                dh.AddPare("@GUID", SqlDbType.NVarChar, 50, record.GUID);
                dh.AddPare("@AccountID", SqlDbType.NVarChar, 50, record.AccountID);
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


        public bool AddATFCTProvisionRecord(T_TaxSettlement record,string Method) {
            DBHelper dh = new DBHelper();
            dh.BeginTran();
            try
            {
                dh.strCmd = "SP_AddCTProvisionRecord";
                dh.AddPare("@Amount", SqlDbType.Decimal, 0, record.Amount);
                dh.AddPare("@DisAmount", SqlDbType.Decimal, 0, record.DisAmount);
                dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, record.C_GUID);
                dh.AddPare("@GUID", SqlDbType.NVarChar, 50, record.GUID);
                dh.AddPare("@AccountID", SqlDbType.NVarChar, 50, record.AccountID);
                dh.AddPare("@Method", SqlDbType.NVarChar, 50, Method);
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
        /// 增值税专用
        /// </summary>
        /// <param name="record"></param>
        /// <param name="Method"></param>
        /// <returns></returns>
        public bool AddOtherTaxInfo(T_TaxSettlement record)
        {
            DBHelper dh = new DBHelper();
            dh.BeginTran();
            try
            {
                dh.strCmd = "SP_CreateOtherTax";
                dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, record.C_GUID);
                dh.AddPare("@TaxName", SqlDbType.NVarChar, 50, record.TaxName);
                dh.AddPare("@Voucher_GUID", SqlDbType.NVarChar, 50, record.AccountID);
                dh.AddPare("@TaxationAmount", SqlDbType.Decimal, 0, record.Amount);
                dh.AddPare("@AffirmDate_tax", SqlDbType.DateTime, 0, record.Date);
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
        /// 企业所得税记录
        /// </summary>
        ///         
        /// <returns></returns>
        public bool AddCTProvisionRecord(string Amount,  string Name,string C_GUID,string Date)
        {
            DBHelper dh = new DBHelper();
            dh.BeginTran();
            try
            {
                dh.strCmd = "SP_AddCTProvisionRecord";
                dh.AddPare("@Amount", SqlDbType.Decimal, 0, Amount); 
                dh.AddPare("@Name", SqlDbType.NVarChar, 50, Name);
                dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
                dh.AddPare("@Date", SqlDbType.NVarChar, 50, Date);
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
        /// 记录营业税金及附加
        /// </summary>
        ///
        /// <returns></returns>
        public bool AddSalesTaxRecord(string Amount, string Name,string Date,string Excise, string EducationFee, string Sales,
            string UrbanConstruction, string Resource, string LandValue, string UrbanLand, string Property, string VehicleVessel, string MineralResources, string Dischargefee, string C_GUID)
        {
            DBHelper dh = new DBHelper();
            dh.BeginTran();
            try
            {
                dh.strCmd = "SP_AddSalesTaxRecord";
                dh.AddPare("@Amount", SqlDbType.Decimal, 0, Amount);
                dh.AddPare("@Name", SqlDbType.NVarChar, 50, Name);
                dh.AddPare("@Date", SqlDbType.NVarChar, 50, Date);
                dh.AddPare("@Excise", SqlDbType.Decimal, 0, Excise);
                dh.AddPare("@EducationFee", SqlDbType.Decimal, 0, EducationFee);
                dh.AddPare("@Sales", SqlDbType.Decimal, 0, Sales);
                dh.AddPare("@UrbanConstruction", SqlDbType.Decimal, 0, UrbanConstruction);
                dh.AddPare("@Resource", SqlDbType.Decimal, 0, Resource);
                dh.AddPare("@LandValue", SqlDbType.Decimal, 0, LandValue);
                dh.AddPare("@UrbanLand", SqlDbType.Decimal, 0, UrbanLand);
                dh.AddPare("@Property", SqlDbType.Decimal, 0, Property);
                dh.AddPare("@VehicleVessel", SqlDbType.Decimal, 0, VehicleVessel);
                dh.AddPare("@MineralResources", SqlDbType.Decimal, 0, MineralResources);
                dh.AddPare("@Dischargefee", SqlDbType.Decimal, 0, Dischargefee);
                dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
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
        /// 获取增值税列表
        /// </summary>
        /// <param name="companyId">公司ID</param>
        /// <returns>
        /// </returns>
        public static List<Co_Tr_ThirdAccount> GetTaxReportList(string companyId, string reportDate, string type)
        {
            DBHelper dh = new DBHelper();

            dh = GetTaxReportLists(companyId,reportDate, type);

            List<Co_Tr_ThirdAccount> reps = dh.Reader<Co_Tr_ThirdAccount>();

            return reps;
        }

        private static DBHelper GetTaxReportLists(string companyId, string reportDate, string periodType)
        {

            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetTaxReportList";
            dh.AddPare("@C_ID", SqlDbType.VarChar, 40, companyId);
            //if (reportDate == "")
            //{
            //    DateTime now = DateTime.Now;
            //    dh.AddPare("@report_year", SqlDbType.VarChar, 10, now.Year);
            //}
            dh.AddPare("@report_date", SqlDbType.VarChar, 10, reportDate);
            dh.AddPare("@period_type", SqlDbType.VarChar, 50, periodType);
            return dh;
        }

        public static List<Co_Tr_ThirdAccount> GetTaxReportbyDate(string companyId, string reportDate, string period,string isend)
        {
            DBHelper dh = new DBHelper();

            dh = GetTaxReportbyDates(companyId, reportDate, period,isend);

            List<Co_Tr_ThirdAccount> reps = dh.Reader<Co_Tr_ThirdAccount>();

            return reps;
        }

        private static DBHelper GetTaxReportbyDates(string companyId, string reportDate, string period, string isend)
        {

            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetTaxReportbyDate";
            dh.AddPare("@C_ID", SqlDbType.VarChar, 40, companyId);
            dh.AddPare("@report_date", SqlDbType.VarChar, 10, reportDate);
            dh.AddPare("@isend", SqlDbType.VarChar, 10, isend);
            dh.AddPare("@period", SqlDbType.VarChar, 40, period);
            return dh;
        }

        public static List<Co_Tr_ThirdAccount> GetTaxReportIsend(string companyId, string reportDate, string period, string isend)
        {
            DBHelper dh = new DBHelper();

            dh = GetTaxReportIsends(companyId, reportDate, period, isend);

            List<Co_Tr_ThirdAccount> reps = dh.Reader<Co_Tr_ThirdAccount>();

            return reps;
        }

        private static DBHelper GetTaxReportIsends(string companyId, string reportDate, string period, string isend)
        {

            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetTaxReportIsend";
            dh.AddPare("@C_ID", SqlDbType.VarChar, 40, companyId);
            dh.AddPare("@report_date", SqlDbType.VarChar, 10, reportDate);
            dh.AddPare("@isend", SqlDbType.VarChar, 10, isend);
            dh.AddPare("@period", SqlDbType.VarChar, 40, period);
            return dh;
        }
        /// <summary>
        /// 判断是否可以反结账结账
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// 2018/7/18   ZM   update
        /// </remarks>
        public List<Co_Tr_ThirdAccount> isFinish(string repDate, string status, out int count, string CId)
        {
            List<Co_Tr_ThirdAccount> rep = new List<Co_Tr_ThirdAccount>();
            //string[] date = repDate.Split('/');
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetIsTaxReport";
            dh.AddPare("@repDate", SqlDbType.VarChar, 50, repDate);
            //dh.AddPare("@report_year", SqlDbType.VarChar, 10, date[0]);
            //dh.AddPare("@report_month", SqlDbType.VarChar, 10, date[1]);
            //dh.AddPare("@type", SqlDbType.VarChar, 50, type);
            //dh.AddPare("@period", SqlDbType.VarChar, 50, period);
            dh.AddPare("@C_GUID", SqlDbType.VarChar, 50, CId);
            dh.AddPare("@REP_STATUS", SqlDbType.VarChar, 50, status);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            rep = dh.Reader<Co_Tr_ThirdAccount>();
            count = dh.GetParaValue<int>("@Count");
            return rep;
        }


        public bool DeleteTaxProvisionRecord(string RepDate, string TaxID, string C_GUID)
        {
            DBHelper dh = new DBHelper();
            dh.BeginTran();
            try
            {
                dh.strCmd = "SP_DeleteTaxProvisionRecord";
                dh.AddPare("@Date", SqlDbType.NVarChar, 50, RepDate);
                dh.AddPare("@TaxID", SqlDbType.NVarChar, 50, TaxID);
                dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
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

        public List<T_DetailTaxSettle> GetDetailTaxList(string C_GUID, int pageIndex, int pageSize, out int count, string AffirmDate, string period)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetDetailTaxList";
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            dh.AddPare("@PageIndex", SqlDbType.Int, 0, pageIndex);
            dh.AddPare("@PageSize", SqlDbType.Int, 0, pageSize);
            dh.AddPare("@Count", SqlDbType.Int, ParameterDirection.Output, 0, null);
            dh.AddPare("@AffirmDate", SqlDbType.NVarChar,50, AffirmDate);
            dh.AddPare("@period", SqlDbType.NVarChar, 50, period);
            List<T_DetailTaxSettle> result = new List<T_DetailTaxSettle>();
            result = dh.Reader<T_DetailTaxSettle>();
            count = dh.GetParaValue<int>("@Count");
            return result;
        }

        
    }
}