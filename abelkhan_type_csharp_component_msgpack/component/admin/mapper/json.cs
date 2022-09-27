using System;
using System.Collections.Generic;
using System.Text;
using abelkhan.admin.helper;

namespace abelkhan.admin
{
    public class JSON : IResponse
    {
        Dictionary<string, object> dict;

        public JSON()
        {
            dict = new Dictionary<string, object>();
        }

        public void Add(string key, object obj)
        {
            dict.Add(key, obj);
        }

        internal Dictionary<string, object> getData()
        {
            return dict;
        }

        public int toCode() {
            return (int)dict.GetValueOrDefault("code");
        }
        public string toResp()
        {
            return JSONHelper.serialize(getData());
        }
    }
}
