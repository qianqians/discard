/*this rsp file is codegen by abelkhan for c#*/

using System;
using System.Collections;
using System.Collections.Generic;

using abelkhan;

namespace rsp
{
    public class rsp_ccallh : abelkhan.Response
    {
        public string uuid;

        public rsp_ccallh(string _uuid)
        {
            uuid = _uuid;
        }

        public void call(String argv0)
        {
            hub.hub.gates.call_client(hub.hub.gates.current_client_uuid, "ccallh", "ccallh_rsp", uuid, argv0);
        }
        public void err()
        {
            hub.hub.gates.call_client(hub.hub.gates.current_client_uuid, "ccallh", "ccallh_err", uuid);
        }
    }

    public class ccallh_module : abelkhan.Module
    {
        public string module_name;
        public hub.hub hub_handle;
        public ccallh_module(hub.hub _hub)
        {
            module_name = "ccallh";
            hub_handle = _hub;
            hub.hub.modules.add_module("ccallh", this);
        }

        public delegate void ccallhhandle();
        public event ccallhhandle onccallh;
        public void ccallh(string uuid)
        {
            if(onccallh != null)
            {
                rsp = new rsp_ccallh(uuid);
                onccallh();
                rsp = null;
            }
        }

    }
}
