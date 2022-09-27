/*
 * svr_msg_handle
 * 2020/6/2
 * qianqians
 */

namespace abelkhan
{
    public class svr_msg_handle
    {
        private center_module _center_module;
        private svrmanager svrmng;
        private hubmanager hubmng;

        public svr_msg_handle(modulemng modules, svrmanager svrs, hubmanager hubs)
        {
            svrmng = svrs;
            hubmng = hubs;
            _center_module = new center_module(modules);

            _center_module.onreg_server += reg_server;
        }

        private void reg_server(string type, string hub_type, string name, string ip, int port)
        {
            hubmng.for_each_hub((hubproxy _proxy) =>{
                _proxy.distribute_server_address(type, name, ip, (ushort)port);
            });

            if (type == "hub")
            {
                var _hubproxy = hubmng.reg_hub(_center_module.current_ch, type, hub_type, name);

                svrmng.for_each_svr((svrproxy _proxy) =>{
                    _hubproxy.distribute_server_address(_proxy.type, _proxy.name, _proxy.ip, _proxy.port);
                });
            }

            var _svrproxy = svrmng.reg_svr(_center_module.current_ch, type, hub_type, name, ip, (ushort)port);
            _svrproxy.reg_server_sucess();
        }
    }
}