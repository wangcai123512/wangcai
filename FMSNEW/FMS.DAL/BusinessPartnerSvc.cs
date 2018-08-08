using System;
using System.Collections.Generic;
using System.Data;
using FMS.Model;

namespace FMS.DAL
{
    public class BusinessPartnerSvc
    {
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
            dh.AddPare("@TaxNumber", SqlDbType.NVarChar, 50, partner.TaxNumber);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, partner.C_GUID);
            dh.AddPare("@ChineseFullName", SqlDbType.NVarChar, 50, partner.ChineseFullName);
            dh.AddPare("@EnglishFullName", SqlDbType.NVarChar, 50, partner.EnglishFullName);
            dh.AddPare("@Website", SqlDbType.NVarChar, 50, partner.Website);
            dh.AddPare("@OrganizationCode", SqlDbType.NVarChar, 50, partner.OrganizationCode);
            dh.AddPare("@IndustryInvolved", SqlDbType.NVarChar, 50, partner.IndustryInvolved);
            dh.AddPare("@RegisteredAddress", SqlDbType.NVarChar, 50, partner.RegisteredAddress);
            dh.AddPare("@Remark", SqlDbType.NVarChar, 100, partner.Remark);
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

        public bool RemoveBankInfo(string id)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_RemoveBankInfo";
            dh.AddPare("@ID",SqlDbType.NChar,40,id);
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
        /// 删除商业伙伴
        /// </summary>
        /// <param name="id">商业伙伴标识</param>
        /// <returns></returns>
        public bool DelPartner(string id)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_DelPartner";
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
        /// 获取商业伙伴
        /// </summary>
        /// <param name="C_GUID">公司标识</param>
        /// <returns></returns>
        public List<T_BusinessPartner> GetPartners(string C_GUID)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetPartners";
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            return dh.Reader<T_BusinessPartner>();
        }
        /// <summary>
        /// 获取商业伙伴客户
        /// </summary>
        /// <param name="C_GUID">公司标识</param>
        /// <returns></returns>
        public List<T_BusinessPartner> GetPartnersCustomer(string C_GUID)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetPartnersCustomer";
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            return dh.Reader<T_BusinessPartner>();
        }

        /// <summary>
        /// 获取商业伙伴客户
        /// </summary>
        /// <param name="C_GUID">公司标识</param>
        /// <returns></returns>
        public List<T_BusinessPartner> GetSupplier(string C_GUID)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetSupplier";
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            return dh.Reader<T_BusinessPartner>();
        
        }


        /// <summary>
        /// 获取所有公司客户
        /// </summary>
        /// <param name="C_GUID">公司标识</param>
        /// <returns></returns>
        public List<T_BusinessPartner> GetPartnersAll(string C_GUID)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetPartnersAll";
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            return dh.Reader<T_BusinessPartner>();
        }
        public List<T_BusinessPartner> GetPartner(string C_GUID,string a,string Customer, string Supplier,string Partner)
        {
         
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetPartner";
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
             dh.AddPare("@BPName", SqlDbType.NVarChar, 50,a);
             dh.AddPare("@IsCustomer", SqlDbType.NVarChar, 50, Customer);
             dh.AddPare("@IsSupplier", SqlDbType.NVarChar, 50, Supplier);
             dh.AddPare("@IsPartner", SqlDbType.NVarChar, 50, Partner);
            return dh.Reader<T_BusinessPartner>();
         }
        /// <summary>
        /// 获取商业伙伴
        /// </summary>
        /// <param name="C_GUID">公司标识</param>
        /// <param name="id">商业伙伴标识</param>
        /// <returns></returns>
        public List<T_BusinessPartner> GetPartners(string C_GUID,string id)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetPartners";
            dh.AddPare("@ID", SqlDbType.NVarChar, 40, id);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            return dh.Reader<T_BusinessPartner>();
        }
        /// <summary>
        /// 获取同名商业伙伴
        /// </summary>
        /// <param name="C_GUID">公司标识</param>
        /// <param name="BPName">伙伴名</param>
        /// <returns></returns>
        public List<T_BusinessPartner> GetPartnerName(string C_GUID, string BPName)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetPartners";
            dh.AddPare("@BPName", SqlDbType.NVarChar, 40, BPName);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            return dh.Reader<T_BusinessPartner>();
        }
        /// <summary>
        /// 获取商业伙伴
        /// </summary>
        /// <param name="C_GUID">公司标识</param>
        /// <param name="name">商业伙伴名称</param>
        /// <returns></returns>
        public object GetPartnersDts(string C_GUID, string name)
        {
            DBHelper dh = new DBHelper();
            dh.strCmd = "SP_GetPartners";
            dh.AddPare("@BPName", SqlDbType.NVarChar, 40, name);
            dh.AddPare("@C_GUID", SqlDbType.NVarChar, 50, C_GUID);
            Object obj = dh.Scalar();
            return obj;
        }
    }
}
