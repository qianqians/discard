using System;
using System.Collections.Generic;
using System.Text;
using abelkhan.cmd;
using System.Threading.Tasks;

namespace abelkhan
{
    public class center_cmd_dispatcher : CmdDispatcher<CBaseCmd, center>
    {
        public center_cmd_dispatcher(center _center): base(_center) { 
        }
    }

    public class CTCode : TCode {
        public CTCode(string name, string des, int value) : base(name, des, value) { }

        /// <summary>
        ///  中心服错误分配范围 31001 ~ 32000
        /// </summary>
        public static TCode CMD_SVR_NOT_EXISTS = new TCode("CMD_SVR_NOT_EXISTS", "服务不存在", 31001);
    }

    public class CBaseCmd : GmBaseCmd<center> {
        public override string GetName() {
            return "center_base";
        }

        public Task<string> Response(GmRespone<string> respone) {
            return Task.FromResult<string>(respone.Encode());
        }
    }

    public class CGmRespone<T> : GmRespone<T> {
        public CGmRespone(TCode _code): base(_code) { 
        }
    }
}
