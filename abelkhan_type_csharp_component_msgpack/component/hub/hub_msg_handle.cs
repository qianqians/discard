/*
 * hub_msg_handle
 * qianqians
 * 2020/6/4
 */

using System;

namespace abelkhan
{
    public class hub_msg_handle
    {
        private hubmanager _hubmng;
        private hub_call_hub_module _module;
        private hub_cmd_dispatcher _cmd_dispatcher;
        public hub_msg_handle(modulemng modules, hubmanager _hubmanager, hub_cmd_dispatcher dispatcher)
        {
            this._hubmng = _hubmanager;
            _cmd_dispatcher = dispatcher;
            _module = new hub_call_hub_module(modules);
            _module.onreg_hub += reg_hub;
            _module.onreq_hub_cmd += do_hub_cmd;
        }

        public event Action<hubproxy> on_hubproxy;
        public void reg_hub(string hub_name, string hub_type)
        {
            var ch = _module.current_ch;
            var rsp = (rsp_reg_hub)(_module.rsp);
            rsp.rsp();

            hubproxy _hubproxy = _hubmng.reg_hub(ch, hub_type, hub_name);
            if (on_hubproxy != null)
            {
                on_hubproxy(_hubproxy);
            }
        }

        private async void do_hub_cmd(string cmd, string param)
        {
            var rsp = (rsp_req_hub_cmd)(_module.rsp);
            string respStr = await _cmd_dispatcher.Dispatch(cmd, param);
            rsp.rsp(respStr);
        }
    }
}