
using System.Collections.Generic;
namespace FMS.Model
{
    /// <summary>
    /// 期初数对象
    /// </summary>
    public class T_BeginningBalance
    {
        /// <summary>
        /// 纪录唯一标识
        /// </summary>
        public string R_GUID { get; set; }

        /// <summary>
        /// 公司标识
        /// </summary>
        public string C_GUID { get; set; }

        /// <summary>
        /// 科目标识
        /// </summary>
        public string Acc_GUID { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        public decimal Money { get; set; }

        /// <summary>
        /// 科目名称
        /// <remarks>扩展字段</remarks>
        /// </summary>
        public string Acc_Name { get; set; }

        /// <summary>
        /// 科目代码
        /// <remarks>扩展字段</remarks>
        /// </summary>
        public string Acc_Code { get; set; }

        /// <summary>
        /// 上级标识
        /// <remarks>扩展字段</remarks>
        /// </summary>
        public string _parentId { get; set; }

        /// <summary>
        /// 子项
        /// <remarks>扩展字段</remarks>
        /// </summary>
        public List<T_BeginningBalance> children { get; set; }

        public T_BeginningBalance()
        {
            children = new List<T_BeginningBalance>();
        }
    }
}
