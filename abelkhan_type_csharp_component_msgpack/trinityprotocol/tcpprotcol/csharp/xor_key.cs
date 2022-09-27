using System;
using System.Collections.Generic;
using System.Collections;
using MessagePack;

namespace abelkhan
{
/*this enum code is codegen by abelkhan codegen for c#*/

/*this struct code is codegen by abelkhan codegen for c#*/
/*this module code is codegen by abelkhan codegen for c#*/
/*this struct code is codegen by abelkhan for c#*/
    [MessagePackObject]
    public class xor_key_refresh_xor_key_struct_ntf
    {
        [Key(0)]
        public UInt32 xor_key;
    }

/*this caller code is codegen by abelkhan codegen for c#*/
/*this cb code is codegen by abelkhan for c#*/
    public class rsp_cb_xor_key : abelkhan.Imodule {
        public readonly MessagePackSerializerOptions op;
        public rsp_cb_xor_key(abelkhan.modulemng modules, MessagePackSerializerOptions _op) : base("rsp_cb_xor_key")
        {
            op = _op;
            modules.reg_module(this);

        }
    }

    public class xor_key_caller : abelkhan.Icaller {
        public static rsp_cb_xor_key rsp_cb_xor_key_handle = null;
        public readonly MessagePackSerializerOptions op;
        public xor_key_caller(abelkhan.Ichannel _ch, abelkhan.modulemng modules, MessagePackSerializerOptions _op) : base("xor_key", _ch)
        {
            op = _op;
            if (rsp_cb_xor_key_handle == null)
            {
                rsp_cb_xor_key_handle = new rsp_cb_xor_key(modules, _op);
            }
        }

        public void refresh_xor_key(UInt32 xor_key){
            var __argv_struct__ = new xor_key_refresh_xor_key_struct_ntf();
            __argv_struct__.xor_key = xor_key;
            var __byte_struct__ = MessagePackSerializer.Serialize(__argv_struct__, op);
            call_module_method("refresh_xor_key", __byte_struct__);
        }

    }
/*this module code is codegen by abelkhan codegen for c#*/
    public class xor_key_module : abelkhan.Imodule {
        private readonly abelkhan.modulemng modules;
        private readonly MessagePackSerializerOptions op;
        public xor_key_module(abelkhan.modulemng _modules, MessagePackSerializerOptions _op) : base("xor_key")
        {
            op = _op;
            modules = _modules;
            modules.reg_module(this);

            reg_method("refresh_xor_key", refresh_xor_key);
        }

        public delegate void cb_refresh_xor_key_handle(UInt32 xor_key);
        public event cb_refresh_xor_key_handle onrefresh_xor_key;

        public void refresh_xor_key(byte[] _event){
            var _struct = MessagePackSerializer.Deserialize<xor_key_refresh_xor_key_struct_ntf>(_event, op);
            if (onrefresh_xor_key != null){
                onrefresh_xor_key(_struct.xor_key);
            }
        }

    }

}