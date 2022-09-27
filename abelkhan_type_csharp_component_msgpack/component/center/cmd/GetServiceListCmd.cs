using System;
using abelkhan.cmd;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace abelkhan.cneter.cmd
{
    public class GetServiceListCmd: CBaseCmd
    {
        public override string GetName() {
            return "GetServiceList";
        }

        public override async Task<string> DoCmd(GmParam param)
        {
            List<cmd_server_info> _list = new List<cmd_server_info>();
            foreach (var _svrproxy in Server._svrmanager.svrproxys.Values) { 
                _list.Add(new cmd_server_info(_svrproxy.type, _svrproxy.hub_type, _svrproxy.name, _svrproxy.ip, _svrproxy.port));
            };
            return await Task.FromResult<string>(CGmRespone<List<cmd_server_info>>.Res(_list).Encode());
        }
    }

    public class cmd_server_info
    {
        public string type;
        public string hub_type;
        public string name;
        public string ip;
        public ushort port;

        public cmd_server_info(string _type, string _hub_type, string _name, string _ip, ushort _port)
        {
            type = _type;
            name = _name;
            hub_type = _hub_type;
            ip = _ip;
            port = _port;
        }
    }
}
