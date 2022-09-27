/*this rsp file is codegen by abelkhan for c#*/

using System;
using System.Collections;
using System.Collections.Generic;

using abelkhan;

namespace rsp
{
    public class rsp_hcallh : abelkhan.Response
    {
        public string hub_name;
        public string uuid;

        public rsp_hcallh(string _hub_name, string _uuid)
        {
            hub_name = _hub_name;
            uuid = _uuid;
        }

        public void call()
        {
            hub.hub.hubs.call_hub(hub_name, "hcallh", "hcallh_rsp", uuid);
        }
        public void err()
        {
            hub.hub.hubs.call_hub(hub_name, "hcallh", "hcallh_err", uuid);
        }
    }

    public class hcallh_module : abelkhan.Module
    {
        public string module_name;
        public hub.hub hub_handle;
        public hcallh_module(hub.hub _hub)
        {
            module_name = "hcallh";
            hub_handle = _hub;
            hub.hub.modules.add_module("hcallh", this);
        }

        public delegate void hcallhhandle();
        public event hcallhhandle onhcallh;
        public void hcallh(string hub_name, string uuid)
        {
            if(onhcallh == null)
            {
                return;
            }

            rsp = new rsp_hcallh(hub_name, uuid);
            onhcallh();
            rsp = null;
        }

    }
}
