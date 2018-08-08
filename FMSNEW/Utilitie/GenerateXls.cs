using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aspose.Cells;
using System.Reflection;
using System.IO;

namespace Utilitie
{
    public class GenerateXls
    {
        public byte[] GenXls<T>(string flag,IEnumerable<T> ds, Dictionary<string, string> cfg) where T : new()
        {
            Workbook wb = new Workbook();
            Worksheet ws = wb.Worksheets[0];
            int rowNo = 0;
            int colNo = 0;
            T t = new T();
            PropertyInfo[] propertys = t.GetType().GetProperties();
            foreach (KeyValuePair<string, string> kv in cfg)
            {
                    ws.Cells[rowNo, colNo].Value = kv.Value;
                    colNo++;
            }
            rowNo++;
            colNo = 0;
            foreach (T item in ds)
            {
                foreach (KeyValuePair<string, string> kv in cfg)
                {
                     if (propertys.FirstOrDefault(i => i.Name.Equals(kv.Key)) != null)
                    {
                         if (flag == "I")
                         {
                             if (kv.Key == "Status") {
                                 Object obj = propertys.FirstOrDefault(i => i.Name.Equals(kv.Key)).GetValue(item, null);
                                 string str = obj.ToString();
                                 if (str == "0")
                                 {
                                     ws.Cells[rowNo, colNo].Value = "应收";
                                 }
                                 else { ws.Cells[rowNo, colNo].Value = "已收";}
                             }
                             else if (kv.Key == "AffirmDate" || kv.Key == "Date")
                             {
                                 Object obj = propertys.FirstOrDefault(i => i.Name.Equals(kv.Key)).GetValue(item, null);
                                 string str = obj.ToString();
                                 ws.Cells[rowNo, colNo].Value = DateTime.Parse(str).ToString("yyyy-MM-dd");
                             }
                             else
                             {
                                 ws.Cells[rowNo, colNo].Value = propertys.FirstOrDefault(i => i.Name.Equals(kv.Key)).GetValue(item, null);
                             }
                         }
                         if (flag == "E")
                         {
                             if (kv.Key == "Status")
                             {
                                 Object obj = propertys.FirstOrDefault(i => i.Name.Equals(kv.Key)).GetValue(item, null);
                                 string str = obj.ToString();
                                 if (str == "0")
                                 {
                                     ws.Cells[rowNo, colNo].Value = "应付";
                                 }
                                 else { ws.Cells[rowNo, colNo].Value = "已付"; }
                             }
                             else if (kv.Key == "AffirmDate" || kv.Key == "Date")
                             {
                                 Object obj = propertys.FirstOrDefault(i => i.Name.Equals(kv.Key)).GetValue(item, null);
                                 string str = obj.ToString();
                                 ws.Cells[rowNo, colNo].Value = DateTime.Parse(str).ToString("yyyy-MM-dd");
                             }
                             else
                             {
                                 ws.Cells[rowNo, colNo].Value = propertys.FirstOrDefault(i => i.Name.Equals(kv.Key)).GetValue(item, null);
                             }
                         }
                         if (flag == "R")
                         {
                             if (kv.Key == "AffirmDate" || kv.Key == "Date")
                             {
                                 Object obj = propertys.FirstOrDefault(i => i.Name.Equals(kv.Key)).GetValue(item, null);
                                 string str = obj.ToString();
                                 ws.Cells[rowNo, colNo].Value = DateTime.Parse(str).ToString("yyyy-MM-dd");
                             }
                             else
                             {
                                 ws.Cells[rowNo, colNo].Value = propertys.FirstOrDefault(i => i.Name.Equals(kv.Key)).GetValue(item, null);
                             }
                         }
                         if (flag == "P")
                         {
                             if (kv.Key == "AffirmDate" || kv.Key == "Date")
                             {
                                 Object obj = propertys.FirstOrDefault(i => i.Name.Equals(kv.Key)).GetValue(item, null);
                                 string str = obj.ToString();
                                 ws.Cells[rowNo, colNo].Value = DateTime.Parse(str).ToString("yyyy-MM-dd");
                             }
                             else
                             {
                                 ws.Cells[rowNo, colNo].Value = propertys.FirstOrDefault(i => i.Name.Equals(kv.Key)).GetValue(item, null);
                             }
                         }
                         colNo++;
                    }
                }
                rowNo++;
                colNo = 0;
            }
            ws.AutoFitColumns();
            return wb.SaveToStream().ToArray();
        }
    }
}
