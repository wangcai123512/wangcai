using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Linq;

namespace Common.Function
{
    public class XmlToEntity
    {
        public T ToEntity<T>(XElement xele) where T : new()
        {
            T t = new T();
            PropertyInfo[] propertys = t.GetType().GetProperties();
            object value = null;
            foreach (PropertyInfo pi in propertys)
            {
                if (pi.CanWrite)
                {
                    foreach (XElement item in xele.Elements(pi.Name))
                    {
                        value = item.Value;
                        if (value != null)
                        {
                            pi.SetValue(t, Convert.ChangeType(value, pi.PropertyType), null);
                        }
                    }
                }
            }
            return t;
        }

        public List<T> ToEntities<T>(List<XElement> xeles) where T : new()
        {
            List<T> ts = new List<T>();
            xeles.ForEach(i => ts.Add(ToEntity<T>(i)));
            return ts;
        }
    }
}
