/*
 * gm_msg_handle
 * 2020/6/2
 * qianqians
 */

namespace abelkhan
{
    public class gm_msg_handle
    {
        private abelkhan.gm_center_module gm_center_module;
        private svrmanager svrmng;
        private hubmanager hubmng;
        private gmmanager gmmng;
        private closehandle closeHandle;
        private center_cmd_dispatcher _cmd_dispatcher;

        public gm_msg_handle(abelkhan.modulemng modules, svrmanager svrs, hubmanager hubs, gmmanager gms, closehandle _closeHandle, center_cmd_dispatcher _Dispatcher)
        {
            svrmng = svrs;
            hubmng = hubs;
            gmmng = gms;
            closeHandle = _closeHandle;
            gm_center_module = new abelkhan.gm_center_module(modules);
            _cmd_dispatcher = _Dispatcher;

            gm_center_module.onconfirm_gm += confirm_gm;
            gm_center_module.onclose_clutter += close_clutter;
            gm_center_module.onreload += reload;
            gm_center_module.onreq_cmd += do_cmd;
        }

        private void confirm_gm(string gm_name)
        {
            gmmng.reg_gm(gm_name, gm_center_module.current_ch);
        }

        private void close_clutter(string gmname)
        {
            if (gmmng.check_gm(gmname, gm_center_module.current_ch))
            {
                log.trace(new System.Diagnostics.StackFrame(true), timerservice.Tick, "close_clutter {0}", gmname);

                closeHandle.is_closing = true;
                svrmng.for_each_svr((svrproxy _svrproxy) => {
                    if (_svrproxy.type != "dbproxy")
                    {
                        _svrproxy.close_server();
                    }
                });

                if (hubmng.check_all_hub_closed())
                {
                    svrmng.close_db();
                    closeHandle.is_close = true;
                }
            }
        }

        private void reload(string gmname)
        {
            if (gmmng.check_gm(gmname, gm_center_module.current_ch))
            {
                hubmng.for_each_hub((hubproxy _proxy) => {
                    _proxy.reload();
                });
            }
        }

        private async void do_cmd(string cmd, string param)
        {
            var rsp = (rsp_req_cmd)(gm_center_module.rsp);
            string respStr = await _cmd_dispatcher.Dispatch(cmd, param);
            rsp.rsp(respStr);
        }
    }
}