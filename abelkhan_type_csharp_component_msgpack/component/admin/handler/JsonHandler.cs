using System;
using System.Collections.Generic;
using System.Text;
using EvHttpSharp;
using abelkhan.admin.helper;
using System.Reflection;

namespace abelkhan.admin
{
    public class JsonHandler : EvBaseHandler<JSON>
    {
        public JsonHandler(MethodInfo method, ActionSupport baseType)
        {
            this.method = method;
            this.baseType = baseType;
        }

        public override void process(EvHttpSessionState session, EventHttpRequest req)
        {
            JSON json = (JSON)method.Invoke(baseType, null);
            ProcessResponse(session, req, json);
            // var headers = getHeaders();
            // string str = JSONHelper.serialize(json.getData());
            // byte[] byteArray = Encoding.UTF8.GetBytes(str);
            // req.Respond(System.Net.HttpStatusCode.OK, headers, byteArray);
        }

        public override void response(EvHttpSessionState session, EventHttpRequest req, JSON json)
        {
            var headers = getHeaders();
            string str = JSONHelper.serialize(json.getData());
            byte[] byteArray = Encoding.UTF8.GetBytes(str);
            req.Respond(System.Net.HttpStatusCode.OK, headers, byteArray);
        }
    }
}
