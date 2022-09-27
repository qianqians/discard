using System;
using System.Collections.Generic;
using System.Text;
using abelkhan.admin;

namespace http_admin
{
    public class BaseController : ActionSupport
    {
        [Post("/")]
        public JSON toWelcome()
        {
            JSON json = new JSON();
            json.Add("time", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            json.Add("server", System.Net.Dns.GetHostName());
            json.Add("version", Environment.Version);
            return json;
        }

        protected JSON getOK(params object[] objs)
        {
            JSON json = new JSON();
            json.Add("success", true);
            string key = "";
            for (int i = 0; i < objs.Length; i++)
            {
                if (i % 2 == 0)
                {
                    key = (string)objs[i];
                }
                else
                {
                    json.Add(key, objs[i]);
                }
            }
            return json;
        }
    }
}
