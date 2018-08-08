
namespace FMS.Model
{
    /// <summary>
    /// 合作伙伴
    /// </summary>
    public class T_BusinessPartner
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        public string BP_GUID
        { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        { get; set; }

        /// <summary>
        /// 是否供应商
        /// </summary>
        public bool IsSupplier
        { get; set; }

        /// <summary>
        /// 是否客户
        /// </summary>
        public bool IsCustomer
        { get; set; }

        /// <summary>
        /// 是否商业伙伴
        /// </summary>
        public bool IsPartner
        { get; set; }

        /// <summary>
        /// 当前公司GUID
        /// </summary>
        public string C_GUID
        { get; set; }

        public string ChineseFullName { get; set; }
        public string EnglishFullName { get; set; }
        public string Website { get; set; }
        public string OrganizationCode { get; set; }
        public string IndustryInvolved { get; set; }
        public string RegisteredAddress { get; set; }
        public string Remark { get; set; }
    }
}
