
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

        /// <summary>
        /// 主公司标识
        /// </summary>
        public string MasterCompanyGuid
        { get; set; }

        /// <summary>
        /// 公司名称
        /// </summary>
        public string Name
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
