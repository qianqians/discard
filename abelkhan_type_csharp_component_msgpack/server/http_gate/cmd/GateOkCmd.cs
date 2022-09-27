using abelkhan.cmd;
using System.Threading.Tasks;
using abelkhan;

namespace data
{
    public class GateOkCmd: HBaseCmd
    {
        public override string GetName() {
            return "GateOk";
        }

        public override async Task<string> DoCmd(GmParam param)
        {
            GmParam gmParam = param.parse<GmParam>();
            return await Task.FromResult<string>(GmRespone<string>.Res("Gate OK").Encode());
        }
    }
}