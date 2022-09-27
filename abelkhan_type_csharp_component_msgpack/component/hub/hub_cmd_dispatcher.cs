using System;
using System.Collections.Generic;
using System.Text;
using abelkhan.cmd;
using System.Threading.Tasks;

namespace abelkhan
{
    public class hub_cmd_dispatcher : CmdDispatcher<HBaseCmd, hub>
    {
        public hub_cmd_dispatcher(hub _hub): base(_hub) { 
        }
    }

    public class HTCode : TCode {
        public HTCode(string name, string des, int value) : base(name, des, value) { }

        /// <summary>
        ///  hub服错误分配范围 32001 ~ 40000
        /// </summary>
        public static TCode CMD_HUB_NOT_EXISTS = new TCode("CMD_HUB_NOT_EXISTS", "Hub服务不存在", 32001);
    }

    public class HBaseCmd : GmBaseCmd<hub> {
        public override string GetName() {
            return "center_base";
        }

        public Task<string> Response(GmRespone<string> respone) {
            return Task.FromResult<string>(respone.Encode());
        }
    }

    public class HGmRespone<T> : GmRespone<T> {
        public HGmRespone(TCode _code): base(_code) { 
        }
    }
}
