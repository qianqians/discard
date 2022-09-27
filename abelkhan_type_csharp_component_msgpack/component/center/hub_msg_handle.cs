/*
 * hub_msg_handle
 * 2020/6/2
 * qianqians
 */

namespace abelkhan
{
    public class hub_msg_handle
    {
        private hub_call_center_module _hub_call_center_module;
        private svrmanager svrmng;
        private hubmanager hubmng;
        private closehandle _closeHandle;

        public hub_msg_handle(modulemng modules, svrmanager svrs, hubmanager hubs, closehandle _closehandle)
        {
            svrmng = svrs;
            hubmng = hubs;
            _closeHandle = _closehandle;
            _hub_call_center_module = new hub_call_center_module(modules);

            _hub_call_center_module.onclosed += closed;
        }

        private void closed()
        {
            hubmng.hub_closed(_hub_call_center_module.current_ch);
            if (hubmng.check_all_hub_closed())
            {
                svrmng.close_db();
                _closeHandle.is_close = true;
            }
        }
    }
}