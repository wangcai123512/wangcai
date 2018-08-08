
using System;
namespace FMS.Model
{
    /// <summary>
    /// 公司设置
    /// </summary>
    public class T_CompanySetting
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        public string R_GUID { get; set; }

        /// <summary>
        /// 公司标识
        /// </summary>
        public string C_GUID { get; set; }

        ///<summary>
        ///本位币
        ///<summary>
        public string StandardCoin
        {
            get;
            set;
        }

        ///<summary>
        ///报表周期
        ///<summary>
        public string ReportPeriod
        {
            get;
            set;
        }

        ///<summary>
        ///报表起始日期
        ///<summary>
        public DateTime ReportStartDate
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string AuditDate
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int Year
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int Month
        {
            get;
            set;
        }

        /// <summary>
        /// 获取报表起始日期年
        /// </summary>
        /// <returns></returns>
        public int GetReportStartDateYear()
        { return ReportStartDate.Year; }

        /// <summary>
        /// 获取报表起始日期月
        /// </summary>
        /// <returns></returns>
        public int GetReportStartDateMonth()
        {
            return ReportStartDate.Month;
        }

        /// <summary>
        /// 常用币制
        /// <remarks>扩展字段</remarks>
        /// </summary>
        public string[] CompanyCy
        {get;set;}

    }
}
