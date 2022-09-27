using System;
using EvHttpSharp;
using abelkhan.admin.helper;
using System.Text;

namespace abelkhan.admin
{
    public class PermissionInterceptor : AbstractActionInterceptor
    {
        public override bool intercept(EvHttpSessionState session, EventHttpRequest req)
        {
            ActionMapper action = session.CurAction;
            if (action.Permission == Permission.BASE || RoleEnum.admin.ToString().Equals(session.Roles)) {
                return true;
            }
            if (!session.Permissions.Contains(action.Permission.ToString())) {
                var headers = EvHttpHelper.buildCrossHeaders();
                string str = JSONHelper.serialize(Result<string>.Fail(Code.ROLE_PERMISSION_NOT_ENOUNGH).ToJson().getData());
                byte[] byteArray = Encoding.UTF8.GetBytes(str);
                // log.trace(new System.Diagnostics.StackFrame(), timerservice.Tick, "PermissionInterceptor :{0}", str);
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
