using System;
using EvHttpSharp;
using abelkhan.admin.helper;
using System.Text;
using http_admin;

namespace abelkhan.admin
{
    public class OperatorLogInterceptor : AbstractActionInterceptor
    {
        private OpLogMapper opLogMapper = new OpLogMapper();

        public override bool intercept(EvHttpSessionState session, EventHttpRequest req)
        {
            return true;
        }

        public override bool afterIntercept<T>(EvHttpSessionState session, EventHttpRequest req, T resp)
        {
            if (req.Uri.Contains("/user/info") || req.Uri.Contains("list"))
                return true;

            string bodyStr = Encoding.UTF8.GetString(req.RequestBody);
            if (req.Uri.Contains("/user/create") || req.Uri.Contains("/user/update") || req.Uri.Contains("/user/createAdmin") || req.Uri.Contains("/user/updateSelf"))
            {
                CreateUserParams newInfo = JSONHelper.deserialize<CreateUserParams>(bodyStr);
                newInfo.password = "***";
                bodyStr = JSONHelper.serialize(newInfo);
            }
            if (req.Uri.Contains("/user/login"))
            {
                UserLoginParam newInfo = JSONHelper.deserialize<UserLoginParam>(bodyStr);
                newInfo.password = "***";
                bodyStr = JSONHelper.serialize(newInfo);
            }
            if (req.Uri.Contains("/user/updatePwd"))
            {
                UpdatePwdParams newInfo = JSONHelper.deserialize<UpdatePwdParams>(bodyStr);
                newInfo.password = "***";
                newInfo.oldPassword = "***";
                newInfo.confirmPassword = "***";
                bodyStr = JSONHelper.serialize(newInfo);
            }
            OpLogEntity entiry = OpLogEntity.ValueOf(session.UserId, req.Uri, req.UserHostAddress, bodyStr, timerservice.Tick, resp.toCode().ToString());
            opLogMapper.CreatePersistedObjectAsync(JSONHelper.serialize(entiry));
            return true;
        }
    }
}
