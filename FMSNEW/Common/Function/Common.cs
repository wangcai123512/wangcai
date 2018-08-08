using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
namespace Common.Function
{
    public static class Common
    {
        public static string DataTableToJson(DataTable table)
        {
            var JsonString = new StringBuilder();
            if (table.Rows.Count > 0)
            {
                JsonString.Append("[");
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    JsonString.Append("{");
                    for (int j = 0; j < table.Columns.Count; j++)
                    {
                        if (j < table.Columns.Count - 1)
                        {
                            JsonString.Append("\"" + table.Columns[j].ColumnName.ToString() + "\":" + "\"" + table.Rows[i][j].ToString() + "\",");
                        }
                        else if (j == table.Columns.Count - 1)
                        {
                            JsonString.Append("\"" + table.Columns[j].ColumnName.ToString() + "\":" + "\"" + table.Rows[i][j].ToString() + "\"");
                        }
                    }
                    if (i == table.Rows.Count - 1)
                    {
                        JsonString.Append("}");
                    }
                    else
                    {
                        JsonString.Append("},");
                    }
                }
                JsonString.Append("]");
            }
            return JsonString.ToString();
        }

        public static void GetReportDateAndRepNo(string report_date, string period_type, ref string report_fix, ref string begin_date, ref string end_date)
        {
            DateTime dtReport = new DateTime();
            switch (period_type)
            {
                case "year":
                    dtReport = DateTime.Parse(report_date + "/01/01");
                    begin_date = dtReport.ToString("yyyy/MM/dd");
                    end_date = dtReport.AddYears(1).AddDays(-1).ToString("yyyy/MM/dd");
                    report_fix = report_fix + 'Y';
                    break;
                case "month":
                    dtReport = DateTime.Parse(report_date + "/01");
                    begin_date = dtReport.ToString("yyyy/MM/dd");
                    end_date = dtReport.AddMonths(1).AddDays(-1).ToString("yyyy/MM/dd");
                    report_fix = report_fix + 'M';
                    break;
                case "quarter":
                    dtReport = DateTime.Parse(report_date + "/01");
                    DateTime dtQuarter = DateTime.Parse(dtReport.Year + "/01/01");
                    int quarty = dtReport.Month;
                    begin_date = dtQuarter.AddMonths((quarty-1)*3).ToString("yyyy/MM/dd");
                    end_date = dtQuarter.AddMonths(quarty * 3).AddDays(-1).ToString("yyyy/MM/dd");
                    report_fix = report_fix + 'Q';
                    break;
                default:
                    break;
            }

        }

        public static decimal GetAmountValue(string strValue)
        {
            decimal returnValue = 0;
            if (!string.IsNullOrEmpty(strValue))
            {
                if (decimal.TryParse(strValue,out returnValue))
                {
                    returnValue = decimal.Parse(strValue);
                }
            }
                
            return returnValue;
        }

        /// <summary>
        /// 通过类获取表格
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static DataTable GetTableByEntity<T>(T t)
        {
            Type modelType = typeof(T);
            DataTable dtNew = new DataTable();
             var pro=  modelType.GetProperties();
             for (int i = 0; i < pro.Length; i++)
            {
                var colType=pro[i].PropertyType;
                if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                {
                    colType = colType.GetGenericArguments()[0];
                }
                dtNew.Columns.Add(new DataColumn(pro[i].Name, colType)); 
            }
             return dtNew;
        }
    }
}
