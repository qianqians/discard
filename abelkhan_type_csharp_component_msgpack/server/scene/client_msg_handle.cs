using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;

namespace scene
{
    class client_msg_handle
    {
        private abelkhan.uuid_module _uuid_module;

        public client_msg_handle(abelkhan.modulemng modules)
        {
            _uuid_module = new abelkhan.uuid_module(modules);
            _uuid_module.onsync_uuid += on_sync_uuid;
        }

        private void on_sync_uuid(String uuid)
        {
            var rsp = _uuid_module.rsp as abelkhan.rsp_sync_uuid;

            singleton.clients.reg_client(uuid, _uuid_module.current_ch);

            rsp.rsp();
        }
    }
}
