using System;
using System.Collections.Generic;
using System.Text;
using EvHttpSharp;
using System.Reflection;

namespace abelkhan.admin
{
    public class VoidHandler : EvBaseHandler<NullResponse>
    {
        public VoidHandler(MethodInfo method, ActionSupport baseType)
        {
            this.method = method;
            this.baseType = baseType;
        }

        public override void process(EvHttpSessionState session, EventHttpRequest req)
        {
            method.Invoke(baseType, null);
            ProcessResponse(session, req, new NullResponse());
        }

        public override void response(EvHttpSessionState session, EventHttpRequest req, NullResponse resp)
        {
            var headers = getHeaders();
            byte[] byteArray = Encoding.UTF8.GetBytes(resp.toResp());
            req.Respond(System.Net.HttpStatusCode.OK, headers, byteArray);
        }
    }
}
