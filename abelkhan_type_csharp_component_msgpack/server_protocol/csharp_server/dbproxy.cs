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
    public class dbproxy_call_hub_reg_hub_sucess_struct_ntf
    {
    }

    [MessagePackObject]
    public class dbproxy_call_hub_ack_create_persisted_object_struct_ntf
    {
        [Key(0)]
        public String callbackid;
        [Key(1)]
        public Boolean is_create_sucessed;
    }

    [MessagePackObject]
    public class dbproxy_call_hub_ack_updata_persisted_object_struct_ntf
    {
        [Key(0)]
        public String callbackid;
        [Key(1)]
        public Boolean is_update_sucessed;
    }

    [MessagePackObject]
    public class dbproxy_call_hub_ack_get_object_count_struct_ntf
    {
        [Key(0)]
        public String callbackid;
        [Key(1)]
        public Int32 count;
    }

    [MessagePackObject]
    public class dbproxy_call_hub_ack_get_object_info_struct_ntf
    {
        [Key(0)]
        public String callbackid;
        [Key(1)]
        public String object_info;
    }

    [MessagePackObject]
    public class dbproxy_call_hub_ack_get_object_info_end_struct_ntf
    {
        [Key(0)]
        public String callbackid;
    }

    [MessagePackObject]
    public class dbproxy_call_hub_ack_remove_object_struct_ntf
    {
        [Key(0)]
        public String callbackid;
        [Key(1)]
        public Boolean is_remove_sucessed;
    }

/*this struct code is codegen by abelkhan for c#*/
    [MessagePackObject]
    public class hub_call_dbproxy_reg_hub_struct_ntf
    {
        [Key(0)]
        public String hub_name;
    }

    [MessagePackObject]
    public class hub_call_dbproxy_create_persisted_object_struct_ntf
    {
        [Key(0)]
        public String db;
        [Key(1)]
        public String collection;
        [Key(2)]
        public String object_info;
        [Key(3)]
        public String callbackid;
    }

    [MessagePackObject]
    public class hub_call_dbproxy_updata_persisted_object_struct_ntf
    {
        [Key(0)]
        public String db;
        [Key(1)]
        public String collection;
        [Key(2)]
        public String query_json;
        [Key(3)]
        public String object_info;
        [Key(4)]
        public String callbackid;
    }

    [MessagePackObject]
    public class hub_call_dbproxy_get_object_count_struct_ntf
    {
        [Key(0)]
        public String db;
        [Key(1)]
        public String collection;
        [Key(2)]
        public String query_json;
        [Key(3)]
        public String callbackid;
    }

    [MessagePackObject]
    public class hub_call_dbproxy_get_object_info_struct_ntf
    {
        [Key(0)]
        public String db;
        [Key(1)]
        public String collection;
        [Key(2)]
        public String query_json;
        [Key(3)]
        public String callbackid;
    }

    [MessagePackObject]
    public class hub_call_dbproxy_get_object_infoex_struct_ntf
    {
        [Key(0)]
        public String db;
        [Key(1)]
        public String collection;
        [Key(2)]
        public String query_json;
        [Key(3)]
        public Int32 skip;
        [Key(4)]
        public Int32 limit;
        [Key(5)]
        public String callbackid;
    }

    [MessagePackObject]
    public class hub_call_dbproxy_remove_object_struct_ntf
    {
        [Key(0)]
        public String db;
        [Key(1)]
        public String collection;
        [Key(2)]
        public String query_json;
        [Key(3)]
        public String callbackid;
    }

/*this caller code is codegen by abelkhan codegen for c#*/
/*this cb code is codegen by abelkhan for c#*/
    public class rsp_cb_dbproxy_call_hub : abelkhan.Imodule {
        public rsp_cb_dbproxy_call_hub(abelkhan.modulemng modules) : base("rsp_cb_dbproxy_call_hub")
        {
            modules.reg_module(this);

        }
    }

    public class dbproxy_call_hub_caller : abelkhan.Icaller {
        public static rsp_cb_dbproxy_call_hub rsp_cb_dbproxy_call_hub_handle = null;
        public dbproxy_call_hub_caller(abelkhan.Ichannel _ch, abelkhan.modulemng modules) : base("dbproxy_call_hub", _ch)
        {
            if (rsp_cb_dbproxy_call_hub_handle == null)
            {
                rsp_cb_dbproxy_call_hub_handle = new rsp_cb_dbproxy_call_hub(modules);
            }
        }

        public void reg_hub_sucess(){
            var __argv_struct__ = new dbproxy_call_hub_reg_hub_sucess_struct_ntf();
            var __byte_struct__ = MessagePackSerializer.Serialize(__argv_struct__);
            call_module_method("reg_hub_sucess", __byte_struct__);
        }

        public void ack_create_persisted_object(String callbackid, Boolean is_create_sucessed){
            var __argv_struct__ = new dbproxy_call_hub_ack_create_persisted_object_struct_ntf();
            __argv_struct__.callbackid = callbackid;
            __argv_struct__.is_create_sucessed = is_create_sucessed;
            var __byte_struct__ = MessagePackSerializer.Serialize(__argv_struct__);
            call_module_method("ack_create_persisted_object", __byte_struct__);
        }

        public void ack_updata_persisted_object(String callbackid, Boolean is_update_sucessed){
            var __argv_struct__ = new dbproxy_call_hub_ack_updata_persisted_object_struct_ntf();
            __argv_struct__.callbackid = callbackid;
            __argv_struct__.is_update_sucessed = is_update_sucessed;
            var __byte_struct__ = MessagePackSerializer.Serialize(__argv_struct__);
            call_module_method("ack_updata_persisted_object", __byte_struct__);
        }

        public void ack_get_object_count(String callbackid, Int32 count){
            var __argv_struct__ = new dbproxy_call_hub_ack_get_object_count_struct_ntf();
            __argv_struct__.callbackid = callbackid;
            __argv_struct__.count = count;
            var __byte_struct__ = MessagePackSerializer.Serialize(__argv_struct__);
            call_module_method("ack_get_object_count", __byte_struct__);
        }

        public void ack_get_object_info(String callbackid, String object_info){
            var __argv_struct__ = new dbproxy_call_hub_ack_get_object_info_struct_ntf();
            __argv_struct__.callbackid = callbackid;
            __argv_struct__.object_info = object_info;
            var __byte_struct__ = MessagePackSerializer.Serialize(__argv_struct__);
            call_module_method("ack_get_object_info", __byte_struct__);
        }

        public void ack_get_object_info_end(String callbackid){
            var __argv_struct__ = new dbproxy_call_hub_ack_get_object_info_end_struct_ntf();
            __argv_struct__.callbackid = callbackid;
            var __byte_struct__ = MessagePackSerializer.Serialize(__argv_struct__);
            call_module_method("ack_get_object_info_end", __byte_struct__);
        }

        public void ack_remove_object(String callbackid, Boolean is_remove_sucessed){
            var __argv_struct__ = new dbproxy_call_hub_ack_remove_object_struct_ntf();
            __argv_struct__.callbackid = callbackid;
            __argv_struct__.is_remove_sucessed = is_remove_sucessed;
            var __byte_struct__ = MessagePackSerializer.Serialize(__argv_struct__);
            call_module_method("ack_remove_object", __byte_struct__);
        }

    }
/*this cb code is codegen by abelkhan for c#*/
    public class rsp_cb_hub_call_dbproxy : abelkhan.Imodule {
        public rsp_cb_hub_call_dbproxy(abelkhan.modulemng modules) : base("rsp_cb_hub_call_dbproxy")
        {
            modules.reg_module(this);

        }
    }

    public class hub_call_dbproxy_caller : abelkhan.Icaller {
        public static rsp_cb_hub_call_dbproxy rsp_cb_hub_call_dbproxy_handle = null;
        public hub_call_dbproxy_caller(abelkhan.Ichannel _ch, abelkhan.modulemng modules) : base("hub_call_dbproxy", _ch)
        {
            if (rsp_cb_hub_call_dbproxy_handle == null)
            {
                rsp_cb_hub_call_dbproxy_handle = new rsp_cb_hub_call_dbproxy(modules);
            }
        }

        public void reg_hub(String hub_name){
            var __argv_struct__ = new hub_call_dbproxy_reg_hub_struct_ntf();
            __argv_struct__.hub_name = hub_name;
            var __byte_struct__ = MessagePackSerializer.Serialize(__argv_struct__);
            call_module_method("reg_hub", __byte_struct__);
        }

        public void create_persisted_object(String db, String collection, String object_info, String callbackid){
            var __argv_struct__ = new hub_call_dbproxy_create_persisted_object_struct_ntf();
            __argv_struct__.db = db;
            __argv_struct__.collection = collection;
            __argv_struct__.object_info = object_info;
            __argv_struct__.callbackid = callbackid;
            var __byte_struct__ = MessagePackSerializer.Serialize(__argv_struct__);
            call_module_method("create_persisted_object", __byte_struct__);
        }

        public void updata_persisted_object(String db, String collection, String query_json, String object_info, String callbackid){
            var __argv_struct__ = new hub_call_dbproxy_updata_persisted_object_struct_ntf();
            __argv_struct__.db = db;
            __argv_struct__.collection = collection;
            __argv_struct__.query_json = query_json;
            __argv_struct__.object_info = object_info;
            __argv_struct__.callbackid = callbackid;
            var __byte_struct__ = MessagePackSerializer.Serialize(__argv_struct__);
            call_module_method("updata_persisted_object", __byte_struct__);
        }

        public void get_object_count(String db, String collection, String query_json, String callbackid){
            var __argv_struct__ = new hub_call_dbproxy_get_object_count_struct_ntf();
            __argv_struct__.db = db;
            __argv_struct__.collection = collection;
            __argv_struct__.query_json = query_json;
            __argv_struct__.callbackid = callbackid;
            var __byte_struct__ = MessagePackSerializer.Serialize(__argv_struct__);
            call_module_method("get_object_count", __byte_struct__);
        }

        public void get_object_info(String db, String collection, String query_json, String callbackid){
            var __argv_struct__ = new hub_call_dbproxy_get_object_info_struct_ntf();
            __argv_struct__.db = db;
            __argv_struct__.collection = collection;
            __argv_struct__.query_json = query_json;
            __argv_struct__.callbackid = callbackid;
            var __byte_struct__ = MessagePackSerializer.Serialize(__argv_struct__);
            call_module_method("get_object_info", __byte_struct__);
        }

        public void get_object_infoex(String db, String collection, String query_json, Int32 skip, Int32 limit, String callbackid){
            var __argv_struct__ = new hub_call_dbproxy_get_object_infoex_struct_ntf();
            __argv_struct__.db = db;
            __argv_struct__.collection = collection;
            __argv_struct__.query_json = query_json;
            __argv_struct__.skip = skip;
            __argv_struct__.limit = limit;
            __argv_struct__.callbackid = callbackid;
            var __byte_struct__ = MessagePackSerializer.Serialize(__argv_struct__);
            call_module_method("get_object_infoex", __byte_struct__);
        }

        public void remove_object(String db, String collection, String query_json, String callbackid){
            var __argv_struct__ = new hub_call_dbproxy_remove_object_struct_ntf();
            __argv_struct__.db = db;
            __argv_struct__.collection = collection;
            __argv_struct__.query_json = query_json;
            __argv_struct__.callbackid = callbackid;
            var __byte_struct__ = MessagePackSerializer.Serialize(__argv_struct__);
            call_module_method("remove_object", __byte_struct__);
        }

    }
/*this module code is codegen by abelkhan codegen for c#*/
    public class dbproxy_call_hub_module : abelkhan.Imodule {
        private abelkhan.modulemng modules;
        public dbproxy_call_hub_module(abelkhan.modulemng _modules) : base("dbproxy_call_hub")
        {
            modules = _modules;
            modules.reg_module(this);

            reg_method("reg_hub_sucess", reg_hub_sucess);
            reg_method("ack_create_persisted_object", ack_create_persisted_object);
            reg_method("ack_updata_persisted_object", ack_updata_persisted_object);
            reg_method("ack_get_object_count", ack_get_object_count);
            reg_method("ack_get_object_info", ack_get_object_info);
            reg_method("ack_get_object_info_end", ack_get_object_info_end);
            reg_method("ack_remove_object", ack_remove_object);
        }

        public delegate void cb_reg_hub_sucess_handle();
        public event cb_reg_hub_sucess_handle onreg_hub_sucess;

        public void reg_hub_sucess(byte[] _event){
            var _struct = MessagePackSerializer.Deserialize<dbproxy_call_hub_reg_hub_sucess_struct_ntf>(_event);
            if (onreg_hub_sucess != null){
                onreg_hub_sucess();
            }
        }

        public delegate void cb_ack_create_persisted_object_handle(String callbackid, Boolean is_create_sucessed);
        public event cb_ack_create_persisted_object_handle onack_create_persisted_object;

        public void ack_create_persisted_object(byte[] _event){
            var _struct = MessagePackSerializer.Deserialize<dbproxy_call_hub_ack_create_persisted_object_struct_ntf>(_event);
            if (onack_create_persisted_object != null){
                onack_create_persisted_object(_struct.callbackid, _struct.is_create_sucessed);
            }
        }

        public delegate void cb_ack_updata_persisted_object_handle(String callbackid, Boolean is_update_sucessed);
        public event cb_ack_updata_persisted_object_handle onack_updata_persisted_object;

        public void ack_updata_persisted_object(byte[] _event){
            var _struct = MessagePackSerializer.Deserialize<dbproxy_call_hub_ack_updata_persisted_object_struct_ntf>(_event);
            if (onack_updata_persisted_object != null){
                onack_updata_persisted_object(_struct.callbackid, _struct.is_update_sucessed);
            }
        }

        public delegate void cb_ack_get_object_count_handle(String callbackid, Int32 count);
        public event cb_ack_get_object_count_handle onack_get_object_count;

        public void ack_get_object_count(byte[] _event){
            var _struct = MessagePackSerializer.Deserialize<dbproxy_call_hub_ack_get_object_count_struct_ntf>(_event);
            if (onack_get_object_count != null){
                onack_get_object_count(_struct.callbackid, _struct.count);
            }
        }

        public delegate void cb_ack_get_object_info_handle(String callbackid, String object_info);
        public event cb_ack_get_object_info_handle onack_get_object_info;

        public void ack_get_object_info(byte[] _event){
            var _struct = MessagePackSerializer.Deserialize<dbproxy_call_hub_ack_get_object_info_struct_ntf>(_event);
            if (onack_get_object_info != null){
                onack_get_object_info(_struct.callbackid, _struct.object_info);
            }
        }

        public delegate void cb_ack_get_object_info_end_handle(String callbackid);
        public event cb_ack_get_object_info_end_handle onack_get_object_info_end;

        public void ack_get_object_info_end(byte[] _event){
            var _struct = MessagePackSerializer.Deserialize<dbproxy_call_hub_ack_get_object_info_end_struct_ntf>(_event);
            if (onack_get_object_info_end != null){
                onack_get_object_info_end(_struct.callbackid);
            }
        }

        public delegate void cb_ack_remove_object_handle(String callbackid, Boolean is_remove_sucessed);
        public event cb_ack_remove_object_handle onack_remove_object;

        public void ack_remove_object(byte[] _event){
            var _struct = MessagePackSerializer.Deserialize<dbproxy_call_hub_ack_remove_object_struct_ntf>(_event);
            if (onack_remove_object != null){
                onack_remove_object(_struct.callbackid, _struct.is_remove_sucessed);
            }
        }

    }
    public class hub_call_dbproxy_module : abelkhan.Imodule {
        private abelkhan.modulemng modules;
        public hub_call_dbproxy_module(abelkhan.modulemng _modules) : base("hub_call_dbproxy")
        {
            modules = _modules;
            modules.reg_module(this);

            reg_method("reg_hub", reg_hub);
            reg_method("create_persisted_object", create_persisted_object);
            reg_method("updata_persisted_object", updata_persisted_object);
            reg_method("get_object_count", get_object_count);
            reg_method("get_object_info", get_object_info);
            reg_method("get_object_infoex", get_object_infoex);
            reg_method("remove_object", remove_object);
        }

        public delegate void cb_reg_hub_handle(String hub_name);
        public event cb_reg_hub_handle onreg_hub;

        public void reg_hub(byte[] _event){
            var _struct = MessagePackSerializer.Deserialize<hub_call_dbproxy_reg_hub_struct_ntf>(_event);
            if (onreg_hub != null){
                onreg_hub(_struct.hub_name);
            }
        }

        public delegate void cb_create_persisted_object_handle(String db, String collection, String object_info, String callbackid);
        public event cb_create_persisted_object_handle oncreate_persisted_object;

        public void create_persisted_object(byte[] _event){
            var _struct = MessagePackSerializer.Deserialize<hub_call_dbproxy_create_persisted_object_struct_ntf>(_event);
            if (oncreate_persisted_object != null){
                oncreate_persisted_object(_struct.db, _struct.collection, _struct.object_info, _struct.callbackid);
            }
        }

        public delegate void cb_updata_persisted_object_handle(String db, String collection, String query_json, String object_info, String callbackid);
        public event cb_updata_persisted_object_handle onupdata_persisted_object;

        public void updata_persisted_object(byte[] _event){
            var _struct = MessagePackSerializer.Deserialize<hub_call_dbproxy_updata_persisted_object_struct_ntf>(_event);
            if (onupdata_persisted_object != null){
                onupdata_persisted_object(_struct.db, _struct.collection, _struct.query_json, _struct.object_info, _struct.callbackid);
            }
        }

        public delegate void cb_get_object_count_handle(String db, String collection, String query_json, String callbackid);
        public event cb_get_object_count_handle onget_object_count;

        public void get_object_count(byte[] _event){
            var _struct = MessagePackSerializer.Deserialize<hub_call_dbproxy_get_object_count_struct_ntf>(_event);
            if (onget_object_count != null){
                onget_object_count(_struct.db, _struct.collection, _struct.query_json, _struct.callbackid);
            }
        }

        public delegate void cb_get_object_info_handle(String db, String collection, String query_json, String callbackid);
        public event cb_get_object_info_handle onget_object_info;

        public void get_object_info(byte[] _event){
            var _struct = MessagePackSerializer.Deserialize<hub_call_dbproxy_get_object_info_struct_ntf>(_event);
            if (onget_object_info != null){
                onget_object_info(_struct.db, _struct.collection, _struct.query_json, _struct.callbackid);
            }
        }

        public delegate void cb_get_object_infoex_handle(String db, String collection, String query_json, Int32 skip, Int32 limit, String callbackid);
        public event cb_get_object_infoex_handle onget_object_infoex;

        public void get_object_infoex(byte[] _event){
            var _struct = MessagePackSerializer.Deserialize<hub_call_dbproxy_get_object_infoex_struct_ntf>(_event);
            if (onget_object_infoex != null){
                onget_object_infoex(_struct.db, _struct.collection, _struct.query_json, _struct.skip, _struct.limit, _struct.callbackid);
            }
        }

        public delegate void cb_remove_object_handle(String db, String collection, String query_json, String callbackid);
        public event cb_remove_object_handle onremove_object;

        public void remove_object(byte[] _event){
            var _struct = MessagePackSerializer.Deserialize<hub_call_dbproxy_remove_object_struct_ntf>(_event);
            if (onremove_object != null){
                onremove_object(_struct.db, _struct.collection, _struct.query_json, _struct.callbackid);
            }
        }

    }

}
