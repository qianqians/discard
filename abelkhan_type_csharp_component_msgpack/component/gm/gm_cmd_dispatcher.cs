using Newtonsoft.Json;
using abelkhan.cmd;
using System.Threading.Tasks;

namespace abelkhan
{
    public class gm_cmd_dispatcher : CmdDispatcher<GBaseCmd, gm>
    {
        public gm_cmd_dispatcher(gm _gm) : base(_gm) { }
    }

    public class GTCode : TCode {
        public GTCode(string name, string des, int value) : base(name, des, value) { }

    }

    public class GBaseCmd : GmBaseCmd<gm> {
        public override string GetName() {
            return "center_base";
        }

        public Task<string> Response(GmRespone<string> respone) {
            return Task.FromResult<string>(respone.Encode());
        }
    }

    public class GGmRespone<T> : GmRespone<T> {
        public GGmRespone(TCode _code): base(_code) { 
        }
    }
}
