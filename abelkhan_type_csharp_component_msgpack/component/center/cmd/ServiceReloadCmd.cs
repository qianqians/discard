using System;
using abelkhan.cmd;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace abelkhan.cneter.cmd
{
    public class ServiceReloadCmd: CBaseCmd
    {
        public override string GetName() {
            return "ServiceReload";
        }

        public override async Task<string> DoCmd(GmParam gmParam)
        {
            log.trace(new System.Diagnostics.StackFrame(true), timerservice.Tick, "CreateGmCmd:{0}", gmParam.param);
            ReloadParam param = gmParam.parse<ReloadParam>();
            hubmanager hubmng = Server._hubmanager;
            hubproxy _hubproxy = hubmng.find_hub(param.name);
            if (_hubproxy == null)
            {
                return await Response(GmRespone<string>.Fail(CTCode.CMD_SVR_NOT_EXISTS));
            }

            _hubproxy.reload();
            return await Response(GmRespone<string>.Success());
        }
    }

    public class ReloadParam
    {
        public string type;
        public string name;
    }
}
