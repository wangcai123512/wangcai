using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FMS.Model
{
    /// <summary>
    /// 快速关注
    /// </summary>
    public class T_QuickAttention
    {
        /// <summary>
        /// 关注标识
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 所属公司
        /// </summary>
        public string c_guid { get; set; }
        /// <summary>
        /// 关注类型
        /// </summary>
        public string attention_type { get; set; }
        /// <summary>
        /// 关注类型金额
        /// </summary>
        public Decimal attention_type_amount { get; set; }
        /// <summary>
        /// 统计时间
        /// </summary>
        public DateTime statistical_time { get; set; }
        /// <summary>
        /// 统计货币
        /// </summary>
        public string statistical_currency { get; set; }
        /// <summary>
        /// 关注状态
        /// </summary>
        public int attention_state { get; set; }
        /// <summary>
        /// 推送账号
        /// </summary>
        public string push_account { get; set; }
        /// <summary>
        /// 推送频率
        /// </summary>
        public string push_frequency { get; set; }
        /// <summary>
        /// 推送类型的选择（日，周，月）
        /// </summary>
        public string push_isselect { get; set; }
        /// <summary>
        /// 推送类型的选择（日，周，月）
        /// </summary>
        public string push_content { get; set; }
        /// <summary>
        /// 推送公司名称
        /// </summary>
        public string company_name { get; set; }

    }
}
