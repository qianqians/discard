using System;
using EvHttpSharp;
using abelkhan.admin.helper;
using System.Text;

namespace abelkhan.admin
{
    public class LoginInterceptor : AbstractActionInterceptor
    {
        public override bool intercept(EvHttpSessionState session, EventHttpRequest req)
        {
            if (req.Uri == "/" || req.Uri.Contains("/user/login") || req.Uri.Contains("/user/createAdmin"))
                return true;
            if (!session.IsLogin())
            {
                var headers = EvHttpHelper.buildCrossHeaders();
                string str = JSONHelper.serialize(Result<string>.Fail(Code.USER_NOT_LOGIN).ToJson().getData());
                byte[] byteArray = Encoding.UTF8.GetBytes(str);
                req.Respond(System.Net.HttpStatusCode.OK, headers, byteArray);
                return false;
            }
            return true;
        }

        public override bool afterIntercept<T>(EvHttpSessionState session, EventHttpRequest req, T resp)
        {
            return true;
        }
    }
}
