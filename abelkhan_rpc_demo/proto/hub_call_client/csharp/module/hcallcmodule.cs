/*this imp file is codegen by abelkhan for c#*/

using System;
using System.Collections;
using System.Collections.Generic;

using common;

namespace imp
{
    public class hcallc_module : common.imodule
    {
        public string module_name;

        public delegate void hcallc_handle(String argv0);
        public event hcallc_handle onhcallc;
        void hcallc(ArrayList _event)
        {
            if (onhcallc == null)
            {
                return;
            }

            var argv0 = ((String)_event[0]);

            onhcallc(argv0);
        }

        public hcallc_module(client.client _client)
        {
            module_name = "hcallc";

            reg_event("hcallc", hcallc);

            _client.modulemanager.add_module("hcallc", this);
        }
    }
}
