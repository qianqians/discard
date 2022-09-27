using System;
using EvHttpSharp;
using abelkhan.admin.helper;
using System.Text;

namespace abelkhan.admin
{
    public class TimeTickInterceptor : AbstractActionInterceptor
    {
        public override bool intercept(EvHttpSessionState session, EventHttpRequest req)
        {
            if (req.Uri == "/" || req.Uri.Contains("/user/login"))
                return true;
            if (session.TimeOut())
            {
                var headers = EvHttpHelper.buildCrossHeaders();
                string str = JSONHelper.serialize(Result<string>.Fail(Code.USER_LOGIN_TIMEOUT).ToJson().getData());
                byte[] byteArray = Encoding.UTF8.GetBytes(str);
                req.Respond(System.Net.HttpStatusCode.OK, headers, byteArray);
                return false;
            }
            return session.RefreshTimeTick();
        }
        
        public override bool afterIntercept<T>(EvHttpSessionState session, EventHttpRequest req, T resp)
        {
            return true;
        }
    }
}
