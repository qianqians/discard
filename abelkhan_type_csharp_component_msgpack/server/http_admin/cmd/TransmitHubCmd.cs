using abelkhan.cmd;
using System.Threading.Tasks;
using abelkhan;

namespace http_admin
{
    public class TransmitHubCmd: GBaseCmd
    {
        public override string GetName() {
            return "TransmitHubCmd";
        }

        public override async Task<string> DoCmd(abelkhan.cmd.GmParam param)
        {
            abelkhan.cmd.GmParam gmParam = param.parse<abelkhan.cmd.GmParam>();
            hubproxy _proxy = http_admin._hub._hubmanager.get_hub(gmParam.svrName);
            if (_proxy == null) {
                return await Response(GmRespone<string>.Fail(HTCode.CMD_HUB_NOT_EXISTS));
            }
            string res = await _proxy.req_hub_cmd(gmParam.cmdName, gmParam.param);
            // log.trace(new System.Diagnostics.StackFrame(true), timerservice.Tick, "TransmitCmd:{0} -> {1} {2} suces", gmParam.cmdName, gmParam.param, res);
            return await Task.FromResult<string>(res);
        }
    }
}