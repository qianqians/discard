using System;
using System.Collections.Generic;
using System.Text;
using EvHttpSharp;
using abelkhan.admin.helper;
using System.Reflection;

namespace abelkhan.admin
{
    public class StringHandler : EvBaseHandler<StringWp>
    {
        public StringHandler(MethodInfo method, ActionSupport baseType)
        {
            this.method = method;
            this.baseType = baseType;
        }

        public override void process(EvHttpSessionState session, EventHttpRequest req)
        {
            StringWp str = (StringWp)method.Invoke(baseType, null);
            ProcessResponse(session, req, str);
        }

        public override void response(EvHttpSessionState session, EventHttpRequest req, StringWp str)
        {
            var headers = getHeaders();
            byte[] byteArray = Encoding.UTF8.GetBytes(str.toResp());
            req.Respond(System.Net.HttpStatusCode.OK, headers, byteArray);
        }
    }
}
