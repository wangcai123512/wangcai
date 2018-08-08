using System;
using System.Configuration;
using System.Web.Mvc;
using System.Web.Routing;
using System.Collections.Generic;
using System.Reflection;
using Common.Models;
using Newtonsoft.Json;




namespace Common.BaseControllers
{
    public class APIController : Controller
    {
        protected string SubSystemID = Convert.ToString(ConfigurationManager.AppSettings["SubSystemID"]);
        protected string AccountID = Convert.ToString(ConfigurationManager.AppSettings["AccountID"]);
        protected string SystemName = Convert.ToString(ConfigurationManager.AppSettings["SystemName"]);
        protected string RMUrl = string.Empty;
        protected bool SSO = bool.Parse(ConfigurationManager.AppSettings["SSO"]);

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
        }
        protected string ConvertToSelectJson<T>(List<T> list, string labelField, string valueField)
        {
            List<MultiSelect> selectList = new List<MultiSelect>();

            foreach (T t in list)
            {
                PropertyInfo[] propertys = t.GetType().GetProperties();

                MultiSelect select = new MultiSelect();
                foreach (PropertyInfo pi in propertys)
                {
                    if (pi.Name == labelField)
                    {
                        select.label = pi.GetValue(t, null).ToString();

                    }
                    else if (pi.Name == valueField)
                    {
                        select.value = pi.GetValue(t, null).ToString();
                    }

                }
                selectList.Add(select);
            }
            if (selectList.Count > 0)
            {
                MultiSelect all = new MultiSelect();
                all.label = "全部";
                all.value = string.Empty;
                selectList.Insert(0, all);
            }
            return JsonConvert.SerializeObject(selectList); ;
        }

        protected string ConvertToSelectJson1<T>(List<T> list, string labelField, string valueField)
        {
            List<MultiSelect> selectList = new List<MultiSelect>();

            foreach (T t in list)
            {
                PropertyInfo[] propertys = t.GetType().GetProperties();

                MultiSelect select = new MultiSelect();
                foreach (PropertyInfo pi in propertys)
                {
                    if (pi.Name == labelField)
                    {
                        select.label = pi.GetValue(t, null).ToString();

                    }
                    else if (pi.Name == valueField)
                    {
                        select.value = pi.GetValue(t, null).ToString();
                    }

                }
                selectList.Add(select);
            }
            
            return JsonConvert.SerializeObject(selectList); ;
        }

        /// <summary>
        /// 获取当前日期
        /// </summary>
        /// <returns></returns>

        protected string GetDetailDate()
        {
            ////返回时间戳
            //return (datetime.ToUniversalTime().Ticks - new DateTime(1970, 1, 1).Ticks)
            //return DateTime.Now.ToLocalTime().ToString();
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalMilliseconds).ToString();
        }
    }
}
