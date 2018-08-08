
using System;
namespace FMS.Model
{
    /// <summary>
    /// 公司对象
    /// </summary>
    public class T_Company
    {
        /// <summary>
        /// 公司标识
        /// </summary>
        public string C_GUID
        { get; set; }

        public string Count
        { get; set; }

        public string TaxNumber
        { get; set; }
        public string id
        { get; set; }
        /// <summary>
        /// 公司ID
        /// </summary>
        public string CompanyID
        { get; set; }

        /// <summary>
        /// 纳税人
        /// </summary>
        public string Taxpayer
        { get; set; }
        /// <summary>
        /// 主公司标识
        /// </summary>
        public string MasterCompanyGuid
        { get; set; }

        ///
        ///公司归属国 
        /// 
        public string Country
        { get; set; }
        
        ///
        ///公司归属省
        /// 
        public string Province
        { get; set; }

        ///
        ///公司归属市 
        /// 
        public string City
        { get; set; }

        /// <summary>
        /// 公司名称
        /// </summary>
        public string Name
        { get; set; }

        public string LoginName
        { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 公司地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        public string Contacter { get; set; }

        /// <summary>
        /// 联系方式
        /// </summary>
        public string ContactWay { get; set; }

        /// <summary>
        /// 公司类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public string AuditDate { get; set; }

        /// <summary>
        /// 用户权限Code
        /// </summary>
        public string GroupCode { get; set; }
        
        /// <summary>
        /// 用户权限
        /// </summary>
        public string UserGroup { get; set; }

        public string ChineseFullName { get; set; }
        public string EnglishFullName { get; set; }
        public string Website { get; set; }
        public string OrganizationCode { get; set; }
        public string IndustryInvolved { get; set; }
        public string RegisteredAddress { get; set; }
        public string Remark { get; set; }
        public string LOGO { get; set; }
        public string BusinessLicense { get; set; }
    }
}
