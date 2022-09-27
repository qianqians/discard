using System;
using System.Collections.Generic;
using System.Text;
using EvHttpSharp;
using abelkhan.admin.helper;
using System.Reflection;

namespace abelkhan.admin
{
    public class NotFoundHandler : IEvHttpHandler
    {
        public NotFoundHandler()
        {
  
        }

        public bool IsReusable
        {
            get { return false; }
        }

        public  void ProcessRequest(EvHttpSessionState session, EventHttpRequest req)
        {
            var headers = getHeaders();
            byte[] byteArray = Encoding.UTF8.GetBytes("not found!!!");
            req.Respond(System.Net.HttpStatusCode.NotFound, headers, byteArray);
        }

        public Dictionary<string, string> getHeaders()
        {
            return EvHttpHelper.buildCrossHeaders();
        }
    }
}
