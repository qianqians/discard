/*this req file is codegen by abelkhan for c#*/
using System;
using System.Collections;
using System.IO;

namespace req
{
    public class cb_ccallh_func
    {
        public delegate void ccallh_handle_cb(String argv0);
        public event ccallh_handle_cb onccallh_cb;
        public void cb(String argv0)
        {
            if (onccallh_cb != null)
            {
                onccallh_cb(argv0);
            }
        }

        public delegate void ccallh_handle_err();
        public event ccallh_handle_err onccallh_err;
        public void err()
        {
            if (onccallh_err != null)
            {
                onccallh_err();
            }
        }

        public void callBack(ccallh_handle_cb cb, ccallh_handle_err err)
        {
            onccallh_cb += cb;
            onccallh_err += err;
        }

    }

    /*this cb code is codegen by abelkhan for c#*/
    public class cb_ccallh : common.imodule
    {
        public Hashtable map_ccallh = new Hashtable();

        public void ccallh_rsp(ArrayList _events)
        {
            string uuid = (string)_events[0];
            var argv0 = (String)_events[1];
            var rsp = (cb_ccallh_func)map_ccallh[uuid];
            rsp.cb(argv0);
        }

        public void ccallh_err(ArrayList _events)
        {
            string uuid = (string)_events[0];
            var rsp = (cb_ccallh_func)map_ccallh[uuid];
            rsp.err();
        }

        public cb_ccallh()
        {
            reg_event("ccallh_rsp", ccallh_rsp);
            reg_event("ccallh_err", ccallh_err);
        }
    }

    public class ccallh
    {
        private client.client client_handle;
        private cb_ccallh cb_ccallh_handle;

        public ccallh(client.client cli)
        {
            cb_ccallh_handle = new cb_ccallh();
            client_handle = cli;
            client_handle.modulemanager.add_module("ccallh", cb_ccallh_handle);
        }

        public ccallh_hubproxy get_hub(string hub_name)
        {
            return new ccallh_hubproxy(hub_name, client_handle, cb_ccallh_handle);
        }

    }

    public class ccallh_hubproxy
    {
        public string hub_name;
        public cb_ccallh cb_ccallh_handle;
        public client.client client_handle;

        public ccallh_hubproxy(string _hub_name, client.client _client_handle, cb_ccallh _cb_ccallh_handle)
        {
            hub_name = _hub_name;
            client_handle = _client_handle;
            cb_ccallh_handle = _cb_ccallh_handle;
        }

        public cb_ccallh_func ccallh()
        {
            var uuid = System.Guid.NewGuid().ToString();
            client_handle.call_hub(hub_name, "ccallh", "ccallh", uuid);

            var cb_ccallh_obj = new cb_ccallh_func();
            cb_ccallh_handle.map_ccallh.Add(uuid, cb_ccallh_obj);

            return cb_ccallh_obj;
        }

    }
}
