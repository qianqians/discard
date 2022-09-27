using System;
using System.Collections;

namespace abelkhan
{
/*this enum code is codegen by abelkhan codegen for c#*/

/*this struct code is codegen by abelkhan codegen for c#*/
/*this caller code is codegen by abelkhan codegen for c#*/
/*this cb code is codegen by abelkhan for c#*/
    public class rsp_cb_xor_key : abelkhan.Imodule {
        public rsp_cb_xor_key(abelkhan.modulemng modules) : base("rsp_cb_xor_key")
        {
            modules.reg_module(this);

        }
    }

    public class xor_key_caller : abelkhan.Icaller {
        public rsp_cb_xor_key rsp_cb_xor_key_handle;
        public xor_key_caller(abelkhan.Ichannel _ch, abelkhan.modulemng modules) : base("xor_key", _ch)
        {
            rsp_cb_xor_key_handle = new rsp_cb_xor_key(modules);
        }

        public void refresh_xor_key(UInt32 xor_key){
            var _argv_f4b8900f_a3d1_11ea_aca6_a85e451255ad = new ArrayList();
            _argv_f4b8900f_a3d1_11ea_aca6_a85e451255ad.Add(xor_key);
            call_module_method("refresh_xor_key", _argv_f4b8900f_a3d1_11ea_aca6_a85e451255ad);
        }

    }
/*this module code is codegen by abelkhan codegen for c#*/
    public class xor_key_module : abelkhan.Imodule {
        private abelkhan.modulemng modules;
        public xor_key_module(abelkhan.modulemng _modules) : base("xor_key")
        {
            modules = _modules;
            modules.reg_module(this);

            reg_method("refresh_xor_key", refresh_xor_key);
        }

        public delegate void cb_refresh_xor_key_handle(UInt32 xor_key);
        public event cb_refresh_xor_key_handle onrefresh_xor_key;

        public void refresh_xor_key(ArrayList inArray){
            var _xor_key = (UInt32)inArray[0];
            if (onrefresh_xor_key != null){
                onrefresh_xor_key(_xor_key);
            }
        }

    }

}
