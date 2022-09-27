using System;
using abelkhan.cmd;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace abelkhan.cneter.cmd
{
    public class ServiceCloseCmd: CBaseCmd
    {
        public override string GetName() {
            return "ServiceClose";
        }

        public override async Task<string> DoCmd(GmParam gmParam)
        {
            CloseParam param = gmParam.parse<CloseParam>();

            svrmanager svrmng = Server._svrmanager;
            hubmanager hubmng = Server._hubmanager;
            svrproxy _svrproxy = svrmng.find_svr(param.name);
            if (_svrproxy == null){
                return await Response(GmRespone<string>.Fail(CTCode.CMD_SVR_NOT_EXISTS));
            }
            _svrproxy.close_server();
            if (hubmng.check_all_hub_closed())
            {
                svrmng.close_db();
                Server._closeHandle.is_close = true;
            }

            return await Response(GmRespone<string>.Success());
        }
    }

    public class CloseParam
    {
        public string type;
        public string name;
    }
}
