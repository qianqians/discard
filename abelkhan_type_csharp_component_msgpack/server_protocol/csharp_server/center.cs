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
    public class center_call_hub_distribute_server_address_struct_ntf
    {
        [Key(0)]
        public String type;
        [Key(1)]
        public String name;
        [Key(2)]
        public String ip;
        [Key(3)]
        public Int32 port;
    }

    [MessagePackObject]
    public class center_call_hub_reload_struct_ntf
    {
    }

/*this struct code is codegen by abelkhan for c#*/
    [MessagePackObject]
    public class center_call_server_reg_server_sucess_struct_ntf
    {
    }

    [MessagePackObject]
    public class center_call_server_close_server_struct_ntf
    {
    }

    [MessagePackObject]
    public class center_call_server_server_be_close_struct_ntf
    {
        [Key(0)]
        public String type;
        [Key(1)]
        public String name;
    }

/*this struct code is codegen by abelkhan for c#*/
    [MessagePackObject]
    public class hub_call_center_closed_struct_ntf
    {
    }

/*this struct code is codegen by abelkhan for c#*/
    [MessagePackObject]
    public class center_reg_server_struct_ntf
    {
        [Key(0)]
        public String type;
        [Key(1)]
        public String hub_type;
        [Key(2)]
        public String name;
        [Key(3)]
        public String ip;
        [Key(4)]
        public Int32 port;
    }

/*this struct code is codegen by abelkhan for c#*/
    [MessagePackObject]
    public class gm_center_confirm_gm_struct_ntf
    {
        [Key(0)]
        public String gm_name;
    }

    [MessagePackObject]
    public class gm_center_close_clutter_struct_ntf
    {
        [Key(0)]
        public String gmname;
    }

    [MessagePackObject]
    public class gm_center_reload_struct_ntf
    {
        [Key(0)]
        public String gmname;
    }

    [MessagePackObject]
    public class gm_center_req_cmd_struct_req
    {
        [Key(0)]
        public string _cb_uuid;
        [Key(1)]
        public String cmd;
        [Key(2)]
        public String param;
    }

    [MessagePackObject]
    public class gm_center_req_cmd_struct_rsp
    {
        [Key(0)]
        public string _cb_uuid;
        [Key(1)]
        public String resp;
    }

    [MessagePackObject]
    public class gm_center_req_cmd_struct_err
    {
        [Key(0)]
        public string _cb_uuid;
    }

/*this caller code is codegen by abelkhan codegen for c#*/
/*this cb code is codegen by abelkhan for c#*/
    public class rsp_cb_center_call_hub : abelkhan.Imodule {
        public rsp_cb_center_call_hub(abelkhan.modulemng modules) : base("rsp_cb_center_call_hub")
        {
            modules.reg_module(this);

        }
    }

    public class center_call_hub_caller : abelkhan.Icaller {
        public static rsp_cb_center_call_hub rsp_cb_center_call_hub_handle = null;
        public center_call_hub_caller(abelkhan.Ichannel _ch, abelkhan.modulemng modules) : base("center_call_hub", _ch)
        {
            if (rsp_cb_center_call_hub_handle == null)
            {
                rsp_cb_center_call_hub_handle = new rsp_cb_center_call_hub(modules);
            }
        }

        public void distribute_server_address(String type, String name, String ip, Int32 port){
            var __argv_struct__ = new center_call_hub_distribute_server_address_struct_ntf();
            __argv_struct__.type = type;
            __argv_struct__.name = name;
            __argv_struct__.ip = ip;
            __argv_struct__.port = port;
            var __byte_struct__ = MessagePackSerializer.Serialize(__argv_struct__);
            call_module_method("distribute_server_address", __byte_struct__);
        }

        public void reload(){
            var __argv_struct__ = new center_call_hub_reload_struct_ntf();
            var __byte_struct__ = MessagePackSerializer.Serialize(__argv_struct__);
            call_module_method("reload", __byte_struct__);
        }

    }
/*this cb code is codegen by abelkhan for c#*/
    public class rsp_cb_center_call_server : abelkhan.Imodule {
        public rsp_cb_center_call_server(abelkhan.modulemng modules) : base("rsp_cb_center_call_server")
        {
            modules.reg_module(this);

        }
    }

    public class center_call_server_caller : abelkhan.Icaller {
        public static rsp_cb_center_call_server rsp_cb_center_call_server_handle = null;
        public center_call_server_caller(abelkhan.Ichannel _ch, abelkhan.modulemng modules) : base("center_call_server", _ch)
        {
            if (rsp_cb_center_call_server_handle == null)
            {
                rsp_cb_center_call_server_handle = new rsp_cb_center_call_server(modules);
            }
        }

        public void reg_server_sucess(){
            var __argv_struct__ = new center_call_server_reg_server_sucess_struct_ntf();
            var __byte_struct__ = MessagePackSerializer.Serialize(__argv_struct__);
            call_module_method("reg_server_sucess", __byte_struct__);
        }

        public void close_server(){
            var __argv_struct__ = new center_call_server_close_server_struct_ntf();
            var __byte_struct__ = MessagePackSerializer.Serialize(__argv_struct__);
            call_module_method("close_server", __byte_struct__);
        }

        public void server_be_close(String type, String name){
            var __argv_struct__ = new center_call_server_server_be_close_struct_ntf();
            __argv_struct__.type = type;
            __argv_struct__.name = name;
            var __byte_struct__ = MessagePackSerializer.Serialize(__argv_struct__);
            call_module_method("server_be_close", __byte_struct__);
        }

    }
/*this cb code is codegen by abelkhan for c#*/
    public class rsp_cb_hub_call_center : abelkhan.Imodule {
        public rsp_cb_hub_call_center(abelkhan.modulemng modules) : base("rsp_cb_hub_call_center")
        {
            modules.reg_module(this);

        }
    }

    public class hub_call_center_caller : abelkhan.Icaller {
        public static rsp_cb_hub_call_center rsp_cb_hub_call_center_handle = null;
        public hub_call_center_caller(abelkhan.Ichannel _ch, abelkhan.modulemng modules) : base("hub_call_center", _ch)
        {
            if (rsp_cb_hub_call_center_handle == null)
            {
                rsp_cb_hub_call_center_handle = new rsp_cb_hub_call_center(modules);
            }
        }

        public void closed(){
            var __argv_struct__ = new hub_call_center_closed_struct_ntf();
            var __byte_struct__ = MessagePackSerializer.Serialize(__argv_struct__);
            call_module_method("closed", __byte_struct__);
        }

    }
/*this cb code is codegen by abelkhan for c#*/
    public class rsp_cb_center : abelkhan.Imodule {
        public rsp_cb_center(abelkhan.modulemng modules) : base("rsp_cb_center")
        {
            modules.reg_module(this);

        }
    }

    public class center_caller : abelkhan.Icaller {
        public static rsp_cb_center rsp_cb_center_handle = null;
        public center_caller(abelkhan.Ichannel _ch, abelkhan.modulemng modules) : base("center", _ch)
        {
            if (rsp_cb_center_handle == null)
            {
                rsp_cb_center_handle = new rsp_cb_center(modules);
            }
        }

        public void reg_server(String type, String hub_type, String name, String ip, Int32 port){
            var __argv_struct__ = new center_reg_server_struct_ntf();
            __argv_struct__.type = type;
            __argv_struct__.hub_type = hub_type;
            __argv_struct__.name = name;
            __argv_struct__.ip = ip;
            __argv_struct__.port = port;
            var __byte_struct__ = MessagePackSerializer.Serialize(__argv_struct__);
            call_module_method("reg_server", __byte_struct__);
        }

    }
    public class cb_req_cmd
    {
        public delegate void req_cmd_handle_cb(String resp);
        public event req_cmd_handle_cb onreq_cmd_cb;

        public delegate void req_cmd_handle_err();
        public event req_cmd_handle_err onreq_cmd_err;

        public void callBack(req_cmd_handle_cb cb, req_cmd_handle_err err)
        {
            onreq_cmd_cb += cb;
            onreq_cmd_err += err;
        }

        public void call_cb(String resp)
        {
            if (onreq_cmd_cb != null)
            {
                onreq_cmd_cb(resp);
            }
        }

        public void call_err()
        {
            if (onreq_cmd_err != null)
            {
                onreq_cmd_err();
            }
        }

    }

/*this cb code is codegen by abelkhan for c#*/
    public class rsp_cb_gm_center : abelkhan.Imodule {
        public Dictionary<string, cb_req_cmd> map_req_cmd;
        public rsp_cb_gm_center(abelkhan.modulemng modules) : base("rsp_cb_gm_center")
        {
            modules.reg_module(this);

            map_req_cmd = new Dictionary<string, cb_req_cmd>();
            reg_method("req_cmd_rsp", req_cmd_rsp);
            reg_method("req_cmd_err", req_cmd_err);
        }
        public void req_cmd_rsp(byte[] _event){
            var _struct = MessagePackSerializer.Deserialize<gm_center_req_cmd_struct_rsp>(_event);
            var rsp = map_req_cmd[_struct._cb_uuid];
            rsp.call_cb(_struct.resp);
            map_req_cmd.Remove(_struct._cb_uuid);
        }
        public void req_cmd_err(byte[] _event){
            var _struct = MessagePackSerializer.Deserialize<gm_center_req_cmd_struct_err>(_event);
            var rsp = map_req_cmd[_struct._cb_uuid];
            rsp.call_err();
            map_req_cmd.Remove(_struct._cb_uuid);
        }
    }

    public class gm_center_caller : abelkhan.Icaller {
        public static rsp_cb_gm_center rsp_cb_gm_center_handle = null;
        public gm_center_caller(abelkhan.Ichannel _ch, abelkhan.modulemng modules) : base("gm_center", _ch)
        {
            if (rsp_cb_gm_center_handle == null)
            {
                rsp_cb_gm_center_handle = new rsp_cb_gm_center(modules);
            }
        }

        public void confirm_gm(String gm_name){
            var __argv_struct__ = new gm_center_confirm_gm_struct_ntf();
            __argv_struct__.gm_name = gm_name;
            var __byte_struct__ = MessagePackSerializer.Serialize(__argv_struct__);
            call_module_method("confirm_gm", __byte_struct__);
        }

        public void close_clutter(String gmname){
            var __argv_struct__ = new gm_center_close_clutter_struct_ntf();
            __argv_struct__.gmname = gmname;
            var __byte_struct__ = MessagePackSerializer.Serialize(__argv_struct__);
            call_module_method("close_clutter", __byte_struct__);
        }

        public void reload(String gmname){
            var __argv_struct__ = new gm_center_reload_struct_ntf();
            __argv_struct__.gmname = gmname;
            var __byte_struct__ = MessagePackSerializer.Serialize(__argv_struct__);
            call_module_method("reload", __byte_struct__);
        }

        public cb_req_cmd req_cmd(String cmd, String param){
            var __cb_uuid_uuid__ = System.Guid.NewGuid().ToString("N");

            var __argv_struct__ = new gm_center_req_cmd_struct_req();
            __argv_struct__._cb_uuid = __cb_uuid_uuid__;
            __argv_struct__.cmd = cmd;
            __argv_struct__.param = param;
            var __byte_struct__ = MessagePackSerializer.Serialize(__argv_struct__);
            call_module_method("req_cmd", __byte_struct__);

            var cb_req_cmd_obj = new cb_req_cmd();
            rsp_cb_gm_center_handle.map_req_cmd.Add(__cb_uuid_uuid__, cb_req_cmd_obj);
            return cb_req_cmd_obj;
        }

    }
/*this module code is codegen by abelkhan codegen for c#*/
    public class center_call_hub_module : abelkhan.Imodule {
        private abelkhan.modulemng modules;
        public center_call_hub_module(abelkhan.modulemng _modules) : base("center_call_hub")
        {
            modules = _modules;
            modules.reg_module(this);

            reg_method("distribute_server_address", distribute_server_address);
            reg_method("reload", reload);
        }

        public delegate void cb_distribute_server_address_handle(String type, String name, String ip, Int32 port);
        public event cb_distribute_server_address_handle ondistribute_server_address;

        public void distribute_server_address(byte[] _event){
            var _struct = MessagePackSerializer.Deserialize<center_call_hub_distribute_server_address_struct_ntf>(_event);
            if (ondistribute_server_address != null){
                ondistribute_server_address(_struct.type, _struct.name, _struct.ip, _struct.port);
            }
        }

        public delegate void cb_reload_handle();
        public event cb_reload_handle onreload;

        public void reload(byte[] _event){
            var _struct = MessagePackSerializer.Deserialize<center_call_hub_reload_struct_ntf>(_event);
            if (onreload != null){
                onreload();
            }
        }

    }
    public class center_call_server_module : abelkhan.Imodule {
        private abelkhan.modulemng modules;
        public center_call_server_module(abelkhan.modulemng _modules) : base("center_call_server")
        {
            modules = _modules;
            modules.reg_module(this);

            reg_method("reg_server_sucess", reg_server_sucess);
            reg_method("close_server", close_server);
            reg_method("server_be_close", server_be_close);
        }

        public delegate void cb_reg_server_sucess_handle();
        public event cb_reg_server_sucess_handle onreg_server_sucess;

        public void reg_server_sucess(byte[] _event){
            var _struct = MessagePackSerializer.Deserialize<center_call_server_reg_server_sucess_struct_ntf>(_event);
            if (onreg_server_sucess != null){
                onreg_server_sucess();
            }
        }

        public delegate void cb_close_server_handle();
        public event cb_close_server_handle onclose_server;

        public void close_server(byte[] _event){
            var _struct = MessagePackSerializer.Deserialize<center_call_server_close_server_struct_ntf>(_event);
            if (onclose_server != null){
                onclose_server();
            }
        }

        public delegate void cb_server_be_close_handle(String type, String name);
        public event cb_server_be_close_handle onserver_be_close;

        public void server_be_close(byte[] _event){
            var _struct = MessagePackSerializer.Deserialize<center_call_server_server_be_close_struct_ntf>(_event);
            if (onserver_be_close != null){
                onserver_be_close(_struct.type, _struct.name);
            }
        }

    }
    public class hub_call_center_module : abelkhan.Imodule {
        private abelkhan.modulemng modules;
        public hub_call_center_module(abelkhan.modulemng _modules) : base("hub_call_center")
        {
            modules = _modules;
            modules.reg_module(this);

            reg_method("closed", closed);
        }

        public delegate void cb_closed_handle();
        public event cb_closed_handle onclosed;

        public void closed(byte[] _event){
            var _struct = MessagePackSerializer.Deserialize<hub_call_center_closed_struct_ntf>(_event);
            if (onclosed != null){
                onclosed();
            }
        }

    }
    public class center_module : abelkhan.Imodule {
        private abelkhan.modulemng modules;
        public center_module(abelkhan.modulemng _modules) : base("center")
        {
            modules = _modules;
            modules.reg_module(this);

            reg_method("reg_server", reg_server);
        }

        public delegate void cb_reg_server_handle(String type, String hub_type, String name, String ip, Int32 port);
        public event cb_reg_server_handle onreg_server;

        public void reg_server(byte[] _event){
            var _struct = MessagePackSerializer.Deserialize<center_reg_server_struct_ntf>(_event);
            if (onreg_server != null){
                onreg_server(_struct.type, _struct.hub_type, _struct.name, _struct.ip, _struct.port);
            }
        }

    }
    public class rsp_req_cmd : abelkhan.Response {
        private string uuid;
        public rsp_req_cmd(abelkhan.Ichannel _ch, String _uuid) : base("rsp_cb_gm_center", _ch)
        {
            uuid = _uuid;
        }

        public void rsp(String resp){
            var __argv_struct__ = new gm_center_req_cmd_struct_rsp();
            __argv_struct__._cb_uuid = uuid;
            __argv_struct__.resp = resp;
            var __byte_struct__ = MessagePackSerializer.Serialize(__argv_struct__);            call_module_method("req_cmd_rsp", __byte_struct__);
        }

        public void err(){
            var __argv_struct__ = new gm_center_req_cmd_struct_err();
            __argv_struct__._cb_uuid = uuid;
            var __byte_struct__ = MessagePackSerializer.Serialize(__argv_struct__);            call_module_method("req_cmd_err", __byte_struct__);
        }

    }

    public class gm_center_module : abelkhan.Imodule {
        private abelkhan.modulemng modules;
        public gm_center_module(abelkhan.modulemng _modules) : base("gm_center")
        {
            modules = _modules;
            modules.reg_module(this);

            reg_method("confirm_gm", confirm_gm);
            reg_method("close_clutter", close_clutter);
            reg_method("reload", reload);
            reg_method("req_cmd", req_cmd);
        }

        public delegate void cb_confirm_gm_handle(String gm_name);
        public event cb_confirm_gm_handle onconfirm_gm;

        public void confirm_gm(byte[] _event){
            var _struct = MessagePackSerializer.Deserialize<gm_center_confirm_gm_struct_ntf>(_event);
            if (onconfirm_gm != null){
                onconfirm_gm(_struct.gm_name);
            }
        }

        public delegate void cb_close_clutter_handle(String gmname);
        public event cb_close_clutter_handle onclose_clutter;

        public void close_clutter(byte[] _event){
            var _struct = MessagePackSerializer.Deserialize<gm_center_close_clutter_struct_ntf>(_event);
            if (onclose_clutter != null){
                onclose_clutter(_struct.gmname);
            }
        }

        public delegate void cb_reload_handle(String gmname);
        public event cb_reload_handle onreload;

        public void reload(byte[] _event){
            var _struct = MessagePackSerializer.Deserialize<gm_center_reload_struct_ntf>(_event);
            if (onreload != null){
                onreload(_struct.gmname);
            }
        }

        public delegate void cb_req_cmd_handle(String cmd, String param);
        public event cb_req_cmd_handle onreq_cmd;

        public void req_cmd(byte[] _event){
            var _struct = MessagePackSerializer.Deserialize<gm_center_req_cmd_struct_req>(_event);
            rsp = new rsp_req_cmd(current_ch, _struct._cb_uuid);
            if (onreq_cmd != null){
                onreq_cmd(_struct.cmd, _struct.param);
            }
            rsp = null;
        }

    }

}
