
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
	public class T_Report
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

		/// <summary>
		/// 报表明细
		/// <remarks>扩展字段</remarks>
		/// </summary>
		public List<T_ReportDetails> Details { get; set; }

		/// <summary>
		/// 报表时间
		/// <remarks>扩展字段</remarks>
		/// </summary>
		public DateTime RepDate 
		{
			get { return new DateTime(Year, Month, 1); } 
		}

		public T_Report()
		{
			Details = new List<T_ReportDetails>();
		}

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
	/// 资产负债表对象
	/// </summary>
	public class BalanceSheet : T_Report
	{
		/// <summary>
		/// 获取数据
		/// </summary>
		/// <returns></returns>
		public string GetDetailsJSON_BS()
		{
			string strFmt = "{{\"total\":{0},\"rows\":{1},\"footer\":{2}}}";
			string strFooter = "[{{\"Name\":\"资产合计:\",\"EndingValue\":\"{0}\"}},{{\"Name\":\"负债及所有者权益合计:\",\"EndingValue\":\"{1}\"}}]";
			return string.Format(strFmt, Details.Count, new JavaScriptSerializer().Serialize(Details),
				string.Format(strFooter, GetAssetSum(), GetLiabilitySum()
				));
		}

		/// <summary>
		/// 获取资产总值
		/// </summary>
		/// <returns></returns>
		public decimal GetAssetSum()
		{
			return Details.Where(i => i.AccGrp.Equals(1)).Sum(i => i.EndingValue);
		}

		/// <summary>
		/// 获取负债及所有者权益总值
		/// </summary>
		/// <returns></returns>
		public decimal GetLiabilitySum()
		{
			return Details.Where(i => i.AccGrp.Equals(2) || i.AccGrp.Equals(4)).Sum(i => i.EndingValue);
		}

		/// <summary>
		/// 验证
		/// </summary>
		/// <returns></returns>
		public bool Vaild()
		{
			return GetAssetSum() == GetLiabilitySum();
		}
	}

	/// <summary>
	/// 利润表
	/// </summary>
	public class IncomeStatement : T_Report
	{
		/// <summary>
		/// 获取数据
		/// </summary>
		/// <returns></returns>
		public string GetDetailsJSON_PL()
		{
			StringBuilder strJson = new StringBuilder("[");
			T_ReportDetails curRec = new T_ReportDetails();
			Dictionary<int, List<int>> config = new Dictionary<int, List<int>>();
			config.Add(6001, new List<int>() { 6401, 6403 });
			config.Add(1, new List<int>() { 6051, 6402, 6601, 6602, 6603 });
			config.Add(2, new List<int>());
			config.Add(3, new List<int>() { 6801 });
			config.Add(4, new List<int>());

			string strRowFmt = "{{\"Acc_Name\":\"{0}\",\"children\":{1},\"Money\":{2},\"Acc_Code\":\"{3}\"}},";

			if (Details.Any(i => i.Code.Equals("6001")))
			{
				decimal curVal = 0;
				decimal rtVal = 0;
				foreach (KeyValuePair<int, List<int>> kvPair in config)
				{
					switch (kvPair.Key)
					{
						case 6001:
							curRec = Details.First(i => i.Code.Equals("6001"));
							curVal = curRec.EndingValue;
							strJson.AppendFormat(strRowFmt, curRec.Name, GenJson_PL(strRowFmt, kvPair, ref curVal), curRec.EndingValue, curRec.Code);
							break;
						case 1:
							rtVal = curVal;
							strJson.AppendFormat(strRowFmt, "主营业务利润", GenJson_PL(strRowFmt, kvPair, ref curVal), rtVal, "1");
							break;
						case 2:
							rtVal = curVal;
							strJson.AppendFormat(strRowFmt, "营业利润", GenJson_PL(strRowFmt, ref curVal), rtVal, "2");
							break;
						case 3:
							rtVal = curVal;
							strJson.AppendFormat(strRowFmt, "利润总额", GenJson_PL(strRowFmt, kvPair, ref curVal), rtVal, "3");
							break;
						case 4:
							rtVal = curVal;
							strJson.AppendFormat(strRowFmt, "净利润", GenJson_PL(strRowFmt, kvPair, ref curVal), rtVal, "4");
							break;
						default:
							break;
					}
				}
				strJson.Remove(strJson.Length - 1, 1);
				strJson.Append("]");
				return strJson.ToString();
			}
			else
			{
				return "[]";
			}

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
			T_ReportDetails curRec = new T_ReportDetails();
			StringBuilder strJson = new StringBuilder("[ ");
			foreach (int item in cfg.Value)
			{
				if (Details.Any(i => i.Code.Equals(item.ToString())))
				{
					curRec = Details.First(i => i.Code.Equals(item.ToString()));
					strJson.AppendFormat(strFmter, curRec.Name, "[]", curRec.EndingValue, curRec.Code);
					val += curRec.EndingValue;
				}
			}
			strJson.Remove(strJson.Length - 1, 1);
			strJson.Append("]");
			return strJson.ToString();
		}

		/// <summary>
		/// 生成数据json
		/// </summary>
		/// <param name="strFmter">格式字符串</param>
		/// <param name="val">递延值</param>
		/// <returns></returns>
		private string GenJson_PL(string strFmter, ref decimal val)
		{
			List<int> codes = new List<int>() { 6001, 6401, 6403, 6051, 6402, 6601, 6602, 6603, 6801 };
			StringBuilder strJson = new StringBuilder("[ ");
			foreach (var item in
				Details.Where(i => !codes.Contains(int.Parse(i.Code))).OrderBy(i => i.AccGrp).ThenBy(i => i.Code))
			{
				strJson.AppendFormat(strFmter, item.Name, "[]", item.EndingValue, item.Code);
				val += item.EndingValue;
			}
			strJson.Remove(strJson.Length - 1, 1);
			strJson.Append("]");
			return strJson.ToString();
		}
	}

	/// <summary>
	/// 现金流量表
	/// </summary>
	public class CashFlowStatement : T_Report
	{
		/// <summary>
		/// 获取数据
		/// </summary>
		/// <returns></returns>
		public string GetDetailsJSON()
		{
			return GetJson(null);
		}

		/// <summary>
		/// 获取补充材料json
		/// </summary>
		/// <returns></returns>
		public string GetAdditionalJson()
		{
			decimal sum = 0;
			string strFmter = "{{\"Acc_Name\":\"{0}\",\"children\":{1},\"Money\":{2},\"Acc_Code\":\"{3}\"}},";
			StringBuilder strJson = new StringBuilder("[");
			strJson.AppendFormat(strFmter,"将净利润调节为经营活动现金流量",GetJson(strFmter,"6",out sum),sum,"6");
			strJson.AppendFormat(strFmter, "不涉及现金收支的投资和筹资活动",GetJson(strFmter,"7",out sum),string.Empty,"7");
			strJson.AppendFormat(strFmter, "现金及现金等价物净增加情况", GetJson(strFmter, "8", out sum), string.Empty, "8");
			strJson.Remove(strJson.Length - 1, 1);
			strJson.Append("]");
			return strJson.ToString();
		}

		/// <summary>
		/// 生成JSON
		/// </summary>
		/// <param name="pid">父级编号</param>
		/// <returns></returns>
		private string GetJson( string pid)
		{
			string strFmter = "{{\"Acc_Name\":\"{0}\",\"children\":{1},\"Money\":{2},\"Acc_Code\":\"{3}\"}},";
			StringBuilder strJson = new StringBuilder("[ ");
			foreach (T_ReportDetails item in Details.Where(i => i.AccGrp==pid))
			{
				strJson.AppendFormat(strFmter, item.Name, GetJson(item.RGUID), item.EndingValue, item.Code);
			}
			strJson.Remove(strJson.Length - 1, 1);
			strJson.Append("]");
			return strJson.ToString();
		}

		/// <summary>
		/// 生成json
		/// </summary>
		/// <param name="strFmter">格式字符串</param>
		/// <param name="pid">父级编号</param>
		/// <param name="sum">递延和</param>
		/// <returns></returns>
		private string GetJson(string strFmter, string pid,out decimal sum)
		{
			StringBuilder strJson = new StringBuilder("[");
			var dat = Details.Where(i => i.Code.StartsWith(pid));
			foreach (var item in dat)
			{
				strJson.AppendFormat(strFmter, item.Name, "[]", item.EndingValue, item.Code);
			}
			strJson.Remove(strJson.Length - 1, 1);
			strJson.Append("]");
			sum = dat.Sum(i => i.EndingValue);
			return strJson.ToString();
		}
	}
}
