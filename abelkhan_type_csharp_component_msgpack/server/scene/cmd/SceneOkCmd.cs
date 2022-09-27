using abelkhan.cmd;
using System.Threading.Tasks;
using abelkhan;

namespace data
{
    public class SceneOkCmd: HBaseCmd
    {
        public override string GetName() {
            return "SceneOk";
        }

        public override async Task<string> DoCmd(GmParam param)
        {
            GmParam gmParam = param.parse<GmParam>();
            return await Task.FromResult<string>(GmRespone<string>.Res("Scene OK").Encode());
        }
    }
}