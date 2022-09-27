/*this req file is codegen by abelkhan for c#*/
using System;
using System.Collections;
using System.IO;

namespace req
{
    public class cb_hcallh_func
    {
        public delegate void hcallh_handle_cb();
        public event hcallh_handle_cb onhcallh_cb;
        public void cb()
        {
            if (onhcallh_cb != null)
            {
                onhcallh_cb();
            }
        }

        public delegate void hcallh_handle_err();
        public event hcallh_handle_err onhcallh_err;
        public void err()
        {
            if (onhcallh_err != null)
            {
                onhcallh_err();
            }
        }

        public void callBack(hcallh_handle_cb cb, hcallh_handle_err err)
        {
            onhcallh_cb += cb;
            onhcallh_err += err;
        }

    }

    /*this cb code is codegen by abelkhan for c#*/
    public class cb_hcallh : common.imodule
    {
        public Hashtable map_hcallh = new Hashtable();
        public void hcallh_rsp(string uuid)
        {
            var rsp = (cb_hcallh_func)map_hcallh[uuid];
            rsp.cb();
        }

        public void hcallh_err(string uuid)
        {
            var rsp = (cb_hcallh_func)map_hcallh[uuid];
            rsp.err();
        }

    }

    public class hcallh
    {
        private cb_hcallh cb_hcallh_handle;

        public hcallh()
        {
            cb_hcallh_handle = new cb_hcallh();
            hub.hub.modules.add_module("hcallh", cb_hcallh_handle);
        }

        public hcallh_hubproxy get_hub(string hub_name)
        {
            return new hcallh_hubproxy(hub_name, cb_hcallh_handle);
        }

    }

    public class hcallh_hubproxy
    {
        public cb_hcallh cb_hcallh_handle;
        public string hub_name;

        public hcallh_hubproxy(string _hub_name, cb_hcallh _cb_hcallh_handle)
        {
            cb_hcallh_handle = _cb_hcallh_handle;
            hub_name = _hub_name;
        }

        public cb_hcallh_func hcallh()
        {
            var uuid = System.Guid.NewGuid().ToString();
            hub.hub.hubs.call_hub(hub_name, "hcallh", "hcallh", hub.hub.name, uuid);

            var cb_hcallh_obj = new cb_hcallh_func();
            cb_hcallh_handle.map_hcallh.Add(uuid, cb_hcallh_obj);

            return cb_hcallh_obj;
        }

    }
}
