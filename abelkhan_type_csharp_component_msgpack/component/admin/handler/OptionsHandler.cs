using System;
using System.Collections.Generic;
using System.Text;
using EvHttpSharp;
using abelkhan.admin.helper;
using System.Reflection;

namespace abelkhan.admin
{
    public class OptionsHandler : IEvHttpHandler
    {
        public OptionsHandler()
        {
  
        }

        public bool IsReusable
        {
            get { return false; }
        }

        public  void ProcessRequest(EvHttpSessionState session, EventHttpRequest req)
        {
            var headers = getHeaders();
            req.Respond(System.Net.HttpStatusCode.OK, headers, Encoding.UTF8.GetBytes(""));
        }

        public Dictionary<string, string> getHeaders()
        {
            return EvHttpHelper.buildCrossHeaders();
        }
    }
}
