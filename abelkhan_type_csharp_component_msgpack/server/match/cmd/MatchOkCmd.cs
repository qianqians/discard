using abelkhan.cmd;
using System.Threading.Tasks;
using abelkhan;

namespace data
{
    public class MatchOkCmd: HBaseCmd
    {
        public override string GetName() {
            return "MatchOk";
        }

        public override async Task<string> DoCmd(GmParam param)
        {
            GmParam gmParam = param.parse<GmParam>();
            return await Task.FromResult<string>(GmRespone<string>.Res("Match OK").Encode());
        }
    }
}