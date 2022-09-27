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
    public class uuid_sync_uuid_struct_req
    {
        [Key(0)]
        public string _cb_uuid;
        [Key(1)]
        public String uuid;
    }

    [MessagePackObject]
    public class uuid_sync_uuid_struct_rsp
    {
        [Key(0)]
        public string _cb_uuid;
    }

    [MessagePackObject]
    public class uuid_sync_uuid_struct_err
    {
        [Key(0)]
        public string _cb_uuid;
        [Key(1)]
        public Int32 errcode;
    }

/*this caller code is codegen by abelkhan codegen for c#*/
    public class cb_sync_uuid
    {
        public delegate void sync_uuid_handle_cb();
        public event sync_uuid_handle_cb onsync_uuid_cb;

        public delegate void sync_uuid_handle_err(Int32 errcode);
        public event sync_uuid_handle_err onsync_uuid_err;

        public void callBack(sync_uuid_handle_cb cb, sync_uuid_handle_err err)
        {
            onsync_uuid_cb += cb;
            onsync_uuid_err += err;
        }

        public void call_cb()
        {
            if (onsync_uuid_cb != null)
            {
                onsync_uuid_cb();
            }
        }

        public void call_err(Int32 errcode)
        {
            if (onsync_uuid_err != null)
            {
                onsync_uuid_err(errcode);
            }
        }

    }

/*this cb code is codegen by abelkhan for c#*/
    public class rsp_cb_uuid : abelkhan.Imodule {
        public Dictionary<string, cb_sync_uuid> map_sync_uuid;
        public rsp_cb_uuid(abelkhan.modulemng modules) : base("rsp_cb_uuid")
        {
            modules.reg_module(this);

            map_sync_uuid = new Dictionary<string, cb_sync_uuid>();
            reg_method("sync_uuid_rsp", sync_uuid_rsp);
            reg_method("sync_uuid_err", sync_uuid_err);
        }
        public void sync_uuid_rsp(byte[] _event){
            var _struct = MessagePackSerializer.Deserialize<uuid_sync_uuid_struct_rsp>(_event);
            var rsp = map_sync_uuid[_struct._cb_uuid];
            rsp.call_cb();
            map_sync_uuid.Remove(_struct._cb_uuid);
        }
        public void sync_uuid_err(byte[] _event){
            var _struct = MessagePackSerializer.Deserialize<uuid_sync_uuid_struct_err>(_event);
            var rsp = map_sync_uuid[_struct._cb_uuid];
            rsp.call_err(_struct.errcode);
            map_sync_uuid.Remove(_struct._cb_uuid);
        }
    }

    public class uuid_caller : abelkhan.Icaller {
        public static rsp_cb_uuid rsp_cb_uuid_handle = null;
        public uuid_caller(abelkhan.Ichannel _ch, abelkhan.modulemng modules) : base("uuid", _ch)
        {
            if (rsp_cb_uuid_handle == null)
            {
                rsp_cb_uuid_handle = new rsp_cb_uuid(modules);
            }
        }

        public cb_sync_uuid sync_uuid(String uuid){
            var __cb_uuid_uuid__ = System.Guid.NewGuid().ToString("N");

            var __argv_struct__ = new uuid_sync_uuid_struct_req();
            __argv_struct__._cb_uuid = __cb_uuid_uuid__;
            __argv_struct__.uuid = uuid;
            var __byte_struct__ = MessagePackSerializer.Serialize(__argv_struct__);
            call_module_method("sync_uuid", __byte_struct__);

            var cb_sync_uuid_obj = new cb_sync_uuid();
            rsp_cb_uuid_handle.map_sync_uuid.Add(__cb_uuid_uuid__, cb_sync_uuid_obj);
            return cb_sync_uuid_obj;
        }

    }
/*this module code is codegen by abelkhan codegen for c#*/
    public class rsp_sync_uuid : abelkhan.Response {
        private string uuid;
        public rsp_sync_uuid(abelkhan.Ichannel _ch, String _uuid) : base("rsp_cb_uuid", _ch)
        {
            uuid = _uuid;
        }

        public void rsp(){
            var __argv_struct__ = new uuid_sync_uuid_struct_rsp();
            __argv_struct__._cb_uuid = uuid;
            var __byte_struct__ = MessagePackSerializer.Serialize(__argv_struct__);            call_module_method("sync_uuid_rsp", __byte_struct__);
        }

        public void err(Int32 errcode){
            var __argv_struct__ = new uuid_sync_uuid_struct_err();
            __argv_struct__._cb_uuid = uuid;
            __argv_struct__.errcode = errcode;
            var __byte_struct__ = MessagePackSerializer.Serialize(__argv_struct__);            call_module_method("sync_uuid_err", __byte_struct__);
        }

    }

    public class uuid_module : abelkhan.Imodule {
        private abelkhan.modulemng modules;
        public uuid_module(abelkhan.modulemng _modules) : base("uuid")
        {
            modules = _modules;
            modules.reg_module(this);

            reg_method("sync_uuid", sync_uuid);
        }

        public delegate void cb_sync_uuid_handle(String uuid);
        public event cb_sync_uuid_handle onsync_uuid;

        public void sync_uuid(byte[] _event){
            var _struct = MessagePackSerializer.Deserialize<uuid_sync_uuid_struct_req>(_event);
            rsp = new rsp_sync_uuid(current_ch, _struct._cb_uuid);
            if (onsync_uuid != null){
                onsync_uuid(_struct.uuid);
            }
            rsp = null;
        }

    }

}
