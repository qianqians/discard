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
    public class hub_call_hub_reg_hub_struct_req
    {
        [Key(0)]
        public string _cb_uuid;
        [Key(1)]
        public String hub_name;
        [Key(2)]
        public String hub_type;
    }

    [MessagePackObject]
    public class hub_call_hub_reg_hub_struct_rsp
    {
        [Key(0)]
        public string _cb_uuid;
    }

    [MessagePackObject]
    public class hub_call_hub_reg_hub_struct_err
    {
        [Key(0)]
        public string _cb_uuid;
    }

    [MessagePackObject]
    public class hub_call_hub_req_hub_cmd_struct_req
    {
        [Key(0)]
        public string _cb_uuid;
        [Key(1)]
        public String cmd;
        [Key(2)]
        public String param;
    }

    [MessagePackObject]
    public class hub_call_hub_req_hub_cmd_struct_rsp
    {
        [Key(0)]
        public string _cb_uuid;
        [Key(1)]
        public String resp;
    }

    [MessagePackObject]
    public class hub_call_hub_req_hub_cmd_struct_err
    {
        [Key(0)]
        public string _cb_uuid;
    }

/*this caller code is codegen by abelkhan codegen for c#*/
    public class cb_reg_hub
    {
        public delegate void reg_hub_handle_cb();
        public event reg_hub_handle_cb onreg_hub_cb;

        public delegate void reg_hub_handle_err();
        public event reg_hub_handle_err onreg_hub_err;

        public void callBack(reg_hub_handle_cb cb, reg_hub_handle_err err)
        {
            onreg_hub_cb += cb;
            onreg_hub_err += err;
        }

        public void call_cb()
        {
            if (onreg_hub_cb != null)
            {
                onreg_hub_cb();
            }
        }

        public void call_err()
        {
            if (onreg_hub_err != null)
            {
                onreg_hub_err();
            }
        }

    }

    public class cb_req_hub_cmd
    {
        public delegate void req_hub_cmd_handle_cb(String resp);
        public event req_hub_cmd_handle_cb onreq_hub_cmd_cb;

        public delegate void req_hub_cmd_handle_err();
        public event req_hub_cmd_handle_err onreq_hub_cmd_err;

        public void callBack(req_hub_cmd_handle_cb cb, req_hub_cmd_handle_err err)
        {
            onreq_hub_cmd_cb += cb;
            onreq_hub_cmd_err += err;
        }

        public void call_cb(String resp)
        {
            if (onreq_hub_cmd_cb != null)
            {
                onreq_hub_cmd_cb(resp);
            }
        }

        public void call_err()
        {
            if (onreq_hub_cmd_err != null)
            {
                onreq_hub_cmd_err();
            }
        }

    }

/*this cb code is codegen by abelkhan for c#*/
    public class rsp_cb_hub_call_hub : abelkhan.Imodule {
        public Dictionary<string, cb_reg_hub> map_reg_hub;
        public Dictionary<string, cb_req_hub_cmd> map_req_hub_cmd;
        public rsp_cb_hub_call_hub(abelkhan.modulemng modules) : base("rsp_cb_hub_call_hub")
        {
            modules.reg_module(this);

            map_reg_hub = new Dictionary<string, cb_reg_hub>();
            reg_method("reg_hub_rsp", reg_hub_rsp);
            reg_method("reg_hub_err", reg_hub_err);
            map_req_hub_cmd = new Dictionary<string, cb_req_hub_cmd>();
            reg_method("req_hub_cmd_rsp", req_hub_cmd_rsp);
            reg_method("req_hub_cmd_err", req_hub_cmd_err);
        }
        public void reg_hub_rsp(byte[] _event){
            var _struct = MessagePackSerializer.Deserialize<hub_call_hub_reg_hub_struct_rsp>(_event);
            var rsp = map_reg_hub[_struct._cb_uuid];
            rsp.call_cb();
            map_reg_hub.Remove(_struct._cb_uuid);
        }
        public void reg_hub_err(byte[] _event){
            var _struct = MessagePackSerializer.Deserialize<hub_call_hub_reg_hub_struct_err>(_event);
            var rsp = map_reg_hub[_struct._cb_uuid];
            rsp.call_err();
            map_reg_hub.Remove(_struct._cb_uuid);
        }
        public void req_hub_cmd_rsp(byte[] _event){
            var _struct = MessagePackSerializer.Deserialize<hub_call_hub_req_hub_cmd_struct_rsp>(_event);
            var rsp = map_req_hub_cmd[_struct._cb_uuid];
            rsp.call_cb(_struct.resp);
            map_req_hub_cmd.Remove(_struct._cb_uuid);
        }
        public void req_hub_cmd_err(byte[] _event){
            var _struct = MessagePackSerializer.Deserialize<hub_call_hub_req_hub_cmd_struct_err>(_event);
            var rsp = map_req_hub_cmd[_struct._cb_uuid];
            rsp.call_err();
            map_req_hub_cmd.Remove(_struct._cb_uuid);
        }
    }

    public class hub_call_hub_caller : abelkhan.Icaller {
        public static rsp_cb_hub_call_hub rsp_cb_hub_call_hub_handle = null;
        public hub_call_hub_caller(abelkhan.Ichannel _ch, abelkhan.modulemng modules) : base("hub_call_hub", _ch)
        {
            if (rsp_cb_hub_call_hub_handle == null)
            {
                rsp_cb_hub_call_hub_handle = new rsp_cb_hub_call_hub(modules);
            }
        }

        public cb_reg_hub reg_hub(String hub_name, String hub_type){
            var __cb_uuid_uuid__ = System.Guid.NewGuid().ToString("N");

            var __argv_struct__ = new hub_call_hub_reg_hub_struct_req();
            __argv_struct__._cb_uuid = __cb_uuid_uuid__;
            __argv_struct__.hub_name = hub_name;
            __argv_struct__.hub_type = hub_type;
            var __byte_struct__ = MessagePackSerializer.Serialize(__argv_struct__);
            call_module_method("reg_hub", __byte_struct__);

            var cb_reg_hub_obj = new cb_reg_hub();
            rsp_cb_hub_call_hub_handle.map_reg_hub.Add(__cb_uuid_uuid__, cb_reg_hub_obj);
            return cb_reg_hub_obj;
        }

        public cb_req_hub_cmd req_hub_cmd(String cmd, String param){
            var __cb_uuid_uuid__ = System.Guid.NewGuid().ToString("N");

            var __argv_struct__ = new hub_call_hub_req_hub_cmd_struct_req();
            __argv_struct__._cb_uuid = __cb_uuid_uuid__;
            __argv_struct__.cmd = cmd;
            __argv_struct__.param = param;
            var __byte_struct__ = MessagePackSerializer.Serialize(__argv_struct__);
            call_module_method("req_hub_cmd", __byte_struct__);

            var cb_req_hub_cmd_obj = new cb_req_hub_cmd();
            rsp_cb_hub_call_hub_handle.map_req_hub_cmd.Add(__cb_uuid_uuid__, cb_req_hub_cmd_obj);
            return cb_req_hub_cmd_obj;
        }

    }
/*this module code is codegen by abelkhan codegen for c#*/
    public class rsp_reg_hub : abelkhan.Response {
        private string uuid;
        public rsp_reg_hub(abelkhan.Ichannel _ch, String _uuid) : base("rsp_cb_hub_call_hub", _ch)
        {
            uuid = _uuid;
        }

        public void rsp(){
            var __argv_struct__ = new hub_call_hub_reg_hub_struct_rsp();
            __argv_struct__._cb_uuid = uuid;
            var __byte_struct__ = MessagePackSerializer.Serialize(__argv_struct__);            call_module_method("reg_hub_rsp", __byte_struct__);
        }

        public void err(){
            var __argv_struct__ = new hub_call_hub_reg_hub_struct_err();
            __argv_struct__._cb_uuid = uuid;
            var __byte_struct__ = MessagePackSerializer.Serialize(__argv_struct__);            call_module_method("reg_hub_err", __byte_struct__);
        }

    }

    public class rsp_req_hub_cmd : abelkhan.Response {
        private string uuid;
        public rsp_req_hub_cmd(abelkhan.Ichannel _ch, String _uuid) : base("rsp_cb_hub_call_hub", _ch)
        {
            uuid = _uuid;
        }

        public void rsp(String resp){
            var __argv_struct__ = new hub_call_hub_req_hub_cmd_struct_rsp();
            __argv_struct__._cb_uuid = uuid;
            __argv_struct__.resp = resp;
            var __byte_struct__ = MessagePackSerializer.Serialize(__argv_struct__);            call_module_method("req_hub_cmd_rsp", __byte_struct__);
        }

        public void err(){
            var __argv_struct__ = new hub_call_hub_req_hub_cmd_struct_err();
            __argv_struct__._cb_uuid = uuid;
            var __byte_struct__ = MessagePackSerializer.Serialize(__argv_struct__);            call_module_method("req_hub_cmd_err", __byte_struct__);
        }

    }

    public class hub_call_hub_module : abelkhan.Imodule {
        private abelkhan.modulemng modules;
        public hub_call_hub_module(abelkhan.modulemng _modules) : base("hub_call_hub")
        {
            modules = _modules;
            modules.reg_module(this);

            reg_method("reg_hub", reg_hub);
            reg_method("req_hub_cmd", req_hub_cmd);
        }

        public delegate void cb_reg_hub_handle(String hub_name, String hub_type);
        public event cb_reg_hub_handle onreg_hub;

        public void reg_hub(byte[] _event){
            var _struct = MessagePackSerializer.Deserialize<hub_call_hub_reg_hub_struct_req>(_event);
            rsp = new rsp_reg_hub(current_ch, _struct._cb_uuid);
            if (onreg_hub != null){
                onreg_hub(_struct.hub_name, _struct.hub_type);
            }
            rsp = null;
        }

        public delegate void cb_req_hub_cmd_handle(String cmd, String param);
        public event cb_req_hub_cmd_handle onreq_hub_cmd;

        public void req_hub_cmd(byte[] _event){
            var _struct = MessagePackSerializer.Deserialize<hub_call_hub_req_hub_cmd_struct_req>(_event);
            rsp = new rsp_req_hub_cmd(current_ch, _struct._cb_uuid);
            if (onreq_hub_cmd != null){
                onreq_hub_cmd(_struct.cmd, _struct.param);
            }
            rsp = null;
        }

    }

}
