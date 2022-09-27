using abelkhan.cmd;
using System.Threading.Tasks;

namespace abelkhan
{
    public class TransmitCmd: GBaseCmd
    {
        public override string GetName() {
            return "TransmitCmd";
        }

        public override async Task<string> DoCmd(GmParam param)
        {

            GmParam gmParam = param.parse<GmParam>();
            string res = await Server._center_proxy.req_cmd(gmParam.cmdName, gmParam.param);
            // log.trace(new System.Diagnostics.StackFrame(true), timerservice.Tick, "TransmitCmd:{0} -> {1} {2} suces", gmParam.cmdName, gmParam.param, res);
            return await Task.FromResult<string>(res);
        }
    }
}