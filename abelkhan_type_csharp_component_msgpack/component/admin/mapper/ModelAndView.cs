using System;
using System.Collections.Generic;
using System.Reflection;
using System.Collections;

namespace abelkhan.admin
{
    class ModelAndView
    {
        Dictionary<string, object> dict;

        public ModelAndView(string view)
        {
            this.viewName = view;
            dict = new Dictionary<string, object>();
        }

        public string viewName { get; set; }

        public void addObject(string key, object val)
        {
            if (dict.ContainsKey(key))
                dict.Remove(key);
            this.dict.Add(key, val);
        }

        public void addObject(string key, object val, string key2)
        {
            addObject(key, val);
            if (val is ICollection)
            {
                if (dict.ContainsKey(key2))
                    dict.Remove(key2);
                this.dict.Add(key2, ((ICollection)val).Count);
            }
        }


        public void addObject(object val)
        {
            if (val != null)
            {
                PropertyInfo[] fields = val.GetType().GetProperties();
                if (fields.Length > 0)
                {
                    foreach (PropertyInfo field in fields)
                    {
                        addObject(field.Name, field.GetValue(val, null));
                    }
                }
            }
        }

        internal Dictionary<string, object> getParameters()
        {
            return dict;
        }
    }
}
