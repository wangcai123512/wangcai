
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using System.Text;

namespace FMS.Model
{
    /// <summary>
    /// 报表对象
    /// </summary>
    public class T_Report<T>
    {
        /// <summary>
        /// 报表标识
        /// </summary>
        public string Rep_GUID { get; set; }
        /// <summary>
        /// 公司标识 
        /// </summary>
        public string C_GUID { get; set; }
        /// <summary>
        /// 报表编号
        /// </summary>
        public string RepNo { get; set; }
        /// <summary>
        /// 报表类型
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 报表年	
        /// </summary>
        public int Year { get; set; }
        /// <summary>
        /// 报表起始月	
        /// </summary>
        public int Month { get; set; }

        public List<T> Details { get; set; }

        public string rep_date { get; set; }

        public string rep_status { get; set; }

        public string period_type { get; set; }
        

        ///// <summary>
        ///// 报表时间
        ///// <remarks>扩展字段</remarks>
        ///// </summary>
        //public DateTime RepDate
        //{
        //    get { return new DateTime(Year, Month, 1); }
        //}


        /// <summary>
        /// 获取报表日期字符串
        /// </summary>
        /// <returns></returns>
        public string GetRepDate()
        {
            return string.Format("{0}.{1}", Year, Month);
        }

    }
    /// <summary>
    /// 利润表
    /// </summary>
    public class IncomeStatement
    {
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns></returns>
        public string GetDetailsJSON_PL()
        {
            return string.Empty;


        }

        /// <summary>
        /// 生成数据json
        /// </summary>
        /// <param name="strFmter">格式字符串</param>
        /// <param name="cfg">配置</param>
        /// <param name="val">递延值</param>
        /// <returns></returns>
        private string GenJson_PL(string strFmter, KeyValuePair<int, List<int>> cfg, ref decimal val)
        {
            return string.Empty;
            //T_ReportDetails curRec = new T_ReportDetails();
            //StringBuilder strJson = new StringBuilder("[ ");
            //foreach (int item in cfg.Value)
            //{
            //    if (Details.Any(i => i.Code.Equals(item.ToString())))
            //    {
            //        curRec = Details.First(i => i.Code.Equals(item.ToString()));
            //        strJson.AppendFormat(strFmter, curRec.Name, "[]", curRec.EndingValue, curRec.Code);
            //        val += curRec.EndingValue;
            //    }
            //}
            //strJson.Remove(strJson.Length - 1, 1);
            //strJson.Append("]");
            //return strJson.ToString();
        }

        /// <summary>
        /// 生成数据json
        /// </summary>
        /// <param name="strFmter">格式字符串</param>
        /// <param name="val">递延值</param>
        /// <returns></returns>
        private string GenJson_PL(string strFmter, ref decimal val)
        {
            return string.Empty;
            //List<int> codes = new List<int>() { 6001, 6401, 6403, 6051, 6402, 6601, 6602, 6603, 6801 };
            //StringBuilder strJson = new StringBuilder("[ ");
            //foreach (var item in
            //    Details.Where(i => !codes.Contains(int.Parse(i.Code))).OrderBy(i => i.AccGrp).ThenBy(i => i.Code))
            //{
            //    strJson.AppendFormat(strFmter, item.Name, "[]", item.EndingValue, item.Code);
            //    val += item.EndingValue;
            //}
            //strJson.Remove(strJson.Length - 1, 1);
            //strJson.Append("]");
            //return strJson.ToString();
        }
    }

}
