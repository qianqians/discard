using abelkhan.admin;
using abelkhan.admin.helper;
using System.Collections.Generic;
using System;
using System.Linq;
using abelkhan;

namespace http_admin
{
    [Controller("/gm")]
    public class GmController : ActionSupport
    {
        [Post("/cmd")]
        [RequirePermissions(Permission.GM)]
        public JSON Cmd()
        {
            GmParam gmParam = getParams<GmParam>();
            gm _gm = http_admin._gm;
            ICmd cmd = _gm.GetCmd(gmParam.cmdName);
            if (cmd == null) {
                return Result<string>.Fail(Code.GM_CMD_NOT_EXISTS).ToJson();
            }
            string res = cmd.DoCmd(_gm, gmParam.param);

            return Result<string>.Res(res).ToJson();
        }

        [Post("/dispatcher")]
        [RequirePermissions(Permission.GM)]
        public StringWp Dispatcher()
        {
            gm _gm = http_admin._gm;
            string Res = _gm._cmd_dispatcher.Dispatch("TransmitCmd", getParams()).GetAwaiter().GetResult();
            // log.trace(new System.Diagnostics.StackFrame(true), timerservice.Tick, "dispatcher:{0}", Res);
            return StringWp.ValueOf(Res);
        }

        [Post("/dispatcherHub")]
        [RequirePermissions(Permission.GM)]
        public StringWp DispatcherHub()
        {
            gm _gm = http_admin._gm;
            string Res = _gm._cmd_dispatcher.Dispatch("TransmitHubCmd", getParams()).GetAwaiter().GetResult();
            // log.trace(new System.Diagnostics.StackFrame(true), timerservice.Tick, "dispatcher:{0}", Res);
            return StringWp.ValueOf(Res);
        }
    }
}
