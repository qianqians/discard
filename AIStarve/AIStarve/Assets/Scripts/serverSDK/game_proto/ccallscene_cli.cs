using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using MsgPack.Serialization;

namespace Abelkhan
{
/*this enum code is codegen by abelkhan codegen for c#*/

/*this struct code is codegen by abelkhan codegen for c#*/
/*this caller code is codegen by abelkhan codegen for c#*/
    public class scene_mgr_get_scene_list_cb
    {
        private UInt64 cb_uuid;
        private scene_mgr_rsp_cb module_rsp_cb;

        public scene_mgr_get_scene_list_cb(UInt64 _cb_uuid, scene_mgr_rsp_cb _module_rsp_cb)
        {
            cb_uuid = _cb_uuid;
            module_rsp_cb = _module_rsp_cb;
        }

        public event Action<List<scene_hub_info>> on_get_scene_list_cb;
        public event Action<Int32> on_get_scene_list_err;
        public event Action on_get_scene_list_timeout;

        public scene_mgr_get_scene_list_cb callBack(Action<List<scene_hub_info>> cb, Action<Int32> err)
        {
            on_get_scene_list_cb += cb;
            on_get_scene_list_err += err;
            return this;
        }

        public void timeout(UInt64 tick, Action timeout_cb)
        {
            TinyTimer.add_timer(tick, ()=>{
                module_rsp_cb.get_scene_list_timeout(cb_uuid);
            });
            on_get_scene_list_timeout += timeout_cb;
        }

        public void call_cb(List<scene_hub_info> info)
        {
            if (on_get_scene_list_cb != null)
            {
                on_get_scene_list_cb(info);
            }
        }

        public void call_err(Int32 errCode)
        {
            if (on_get_scene_list_err != null)
            {
                on_get_scene_list_err(errCode);
            }
        }

        public void call_timeout()
        {
            if (on_get_scene_list_timeout != null)
            {
                on_get_scene_list_timeout();
            }
        }

    }

    public class scene_mgr_create_scene_cb
    {
        private UInt64 cb_uuid;
        private scene_mgr_rsp_cb module_rsp_cb;

        public scene_mgr_create_scene_cb(UInt64 _cb_uuid, scene_mgr_rsp_cb _module_rsp_cb)
        {
            cb_uuid = _cb_uuid;
            module_rsp_cb = _module_rsp_cb;
        }

        public event Action<scene_hub_info> on_create_scene_cb;
        public event Action<Int32> on_create_scene_err;
        public event Action on_create_scene_timeout;

        public scene_mgr_create_scene_cb callBack(Action<scene_hub_info> cb, Action<Int32> err)
        {
            on_create_scene_cb += cb;
            on_create_scene_err += err;
            return this;
        }

        public void timeout(UInt64 tick, Action timeout_cb)
        {
            TinyTimer.add_timer(tick, ()=>{
                module_rsp_cb.create_scene_timeout(cb_uuid);
            });
            on_create_scene_timeout += timeout_cb;
        }

        public void call_cb(scene_hub_info info)
        {
            if (on_create_scene_cb != null)
            {
                on_create_scene_cb(info);
            }
        }

        public void call_err(Int32 errCode)
        {
            if (on_create_scene_err != null)
            {
                on_create_scene_err(errCode);
            }
        }

        public void call_timeout()
        {
            if (on_create_scene_timeout != null)
            {
                on_create_scene_timeout();
            }
        }

    }

    public class scene_mgr_player_level_scene_cb
    {
        private UInt64 cb_uuid;
        private scene_mgr_rsp_cb module_rsp_cb;

        public scene_mgr_player_level_scene_cb(UInt64 _cb_uuid, scene_mgr_rsp_cb _module_rsp_cb)
        {
            cb_uuid = _cb_uuid;
            module_rsp_cb = _module_rsp_cb;
        }

        public event Action on_player_level_scene_cb;
        public event Action<Int32> on_player_level_scene_err;
        public event Action on_player_level_scene_timeout;

        public scene_mgr_player_level_scene_cb callBack(Action cb, Action<Int32> err)
        {
            on_player_level_scene_cb += cb;
            on_player_level_scene_err += err;
            return this;
        }

        public void timeout(UInt64 tick, Action timeout_cb)
        {
            TinyTimer.add_timer(tick, ()=>{
                module_rsp_cb.player_level_scene_timeout(cb_uuid);
            });
            on_player_level_scene_timeout += timeout_cb;
        }

        public void call_cb()
        {
            if (on_player_level_scene_cb != null)
            {
                on_player_level_scene_cb();
            }
        }

        public void call_err(Int32 errCode)
        {
            if (on_player_level_scene_err != null)
            {
                on_player_level_scene_err(errCode);
            }
        }

        public void call_timeout()
        {
            if (on_player_level_scene_timeout != null)
            {
                on_player_level_scene_timeout();
            }
        }

    }

    public class scene_mgr_player_into_scene_cb
    {
        private UInt64 cb_uuid;
        private scene_mgr_rsp_cb module_rsp_cb;

        public scene_mgr_player_into_scene_cb(UInt64 _cb_uuid, scene_mgr_rsp_cb _module_rsp_cb)
        {
            cb_uuid = _cb_uuid;
            module_rsp_cb = _module_rsp_cb;
        }

        public event Action<SceneInfo> on_player_into_scene_cb;
        public event Action<Int32, scene_hub_info> on_player_into_scene_err;
        public event Action on_player_into_scene_timeout;

        public scene_mgr_player_into_scene_cb callBack(Action<SceneInfo> cb, Action<Int32, scene_hub_info> err)
        {
            on_player_into_scene_cb += cb;
            on_player_into_scene_err += err;
            return this;
        }

        public void timeout(UInt64 tick, Action timeout_cb)
        {
            TinyTimer.add_timer(tick, ()=>{
                module_rsp_cb.player_into_scene_timeout(cb_uuid);
            });
            on_player_into_scene_timeout += timeout_cb;
        }

        public void call_cb(SceneInfo info)
        {
            if (on_player_into_scene_cb != null)
            {
                on_player_into_scene_cb(info);
            }
        }

        public void call_err(Int32 errCode, scene_hub_info hub_info)
        {
            if (on_player_into_scene_err != null)
            {
                on_player_into_scene_err(errCode, hub_info);
            }
        }

        public void call_timeout()
        {
            if (on_player_into_scene_timeout != null)
            {
                on_player_into_scene_timeout();
            }
        }

    }

/*this cb code is codegen by abelkhan for c#*/
    public class scene_mgr_rsp_cb : Common.IModule {
        public Dictionary<UInt64, scene_mgr_get_scene_list_cb> map_get_scene_list;
        public Dictionary<UInt64, scene_mgr_create_scene_cb> map_create_scene;
        public Dictionary<UInt64, scene_mgr_player_level_scene_cb> map_player_level_scene;
        public Dictionary<UInt64, scene_mgr_player_into_scene_cb> map_player_into_scene;
        public scene_mgr_rsp_cb(Common.ModuleManager modules)
        {
            map_get_scene_list = new Dictionary<UInt64, scene_mgr_get_scene_list_cb>();
            modules.add_mothed("scene_mgr_rsp_cb_get_scene_list_rsp", get_scene_list_rsp);
            modules.add_mothed("scene_mgr_rsp_cb_get_scene_list_err", get_scene_list_err);
            map_create_scene = new Dictionary<UInt64, scene_mgr_create_scene_cb>();
            modules.add_mothed("scene_mgr_rsp_cb_create_scene_rsp", create_scene_rsp);
            modules.add_mothed("scene_mgr_rsp_cb_create_scene_err", create_scene_err);
            map_player_level_scene = new Dictionary<UInt64, scene_mgr_player_level_scene_cb>();
            modules.add_mothed("scene_mgr_rsp_cb_player_level_scene_rsp", player_level_scene_rsp);
            modules.add_mothed("scene_mgr_rsp_cb_player_level_scene_err", player_level_scene_err);
            map_player_into_scene = new Dictionary<UInt64, scene_mgr_player_into_scene_cb>();
            modules.add_mothed("scene_mgr_rsp_cb_player_into_scene_rsp", player_into_scene_rsp);
            modules.add_mothed("scene_mgr_rsp_cb_player_into_scene_err", player_into_scene_err);
        }

        public void get_scene_list_rsp(IList<MsgPack.MessagePackObject> inArray){
            var uuid = ((MsgPack.MessagePackObject)inArray[0]).AsUInt64();
            var _info = new List<scene_hub_info>();
            var _protocol_arrayinfo = ((MsgPack.MessagePackObject)inArray[1]).AsList();
            foreach (var v_d856b000_56e0_5f62_a6f3_e6a0c7859745 in _protocol_arrayinfo){
                _info.Add(scene_hub_info.protcol_to_scene_hub_info(((MsgPack.MessagePackObject)v_d856b000_56e0_5f62_a6f3_e6a0c7859745).AsDictionary()));
            }
            var rsp = try_get_and_del_get_scene_list_cb(uuid);
            if (rsp != null)
            {
                rsp.call_cb(_info);
            }
        }

        public void get_scene_list_err(IList<MsgPack.MessagePackObject> inArray){
            var uuid = ((MsgPack.MessagePackObject)inArray[0]).AsUInt64();
            var _errCode = ((MsgPack.MessagePackObject)inArray[1]).AsInt32();
            var rsp = try_get_and_del_get_scene_list_cb(uuid);
            if (rsp != null)
            {
                rsp.call_err(_errCode);
            }
        }

        public void get_scene_list_timeout(UInt64 cb_uuid){
            var rsp = try_get_and_del_get_scene_list_cb(cb_uuid);
            if (rsp != null){
                rsp.call_timeout();
            }
        }

        private scene_mgr_get_scene_list_cb try_get_and_del_get_scene_list_cb(UInt64 uuid){
            lock(map_get_scene_list)
            {
                if (map_get_scene_list.TryGetValue(uuid, out scene_mgr_get_scene_list_cb rsp))
                {
                    map_get_scene_list.Remove(uuid);
                }
                return rsp;
            }
        }

        public void create_scene_rsp(IList<MsgPack.MessagePackObject> inArray){
            var uuid = ((MsgPack.MessagePackObject)inArray[0]).AsUInt64();
            var _info = scene_hub_info.protcol_to_scene_hub_info(((MsgPack.MessagePackObject)inArray[1]).AsDictionary());
            var rsp = try_get_and_del_create_scene_cb(uuid);
            if (rsp != null)
            {
                rsp.call_cb(_info);
            }
        }

        public void create_scene_err(IList<MsgPack.MessagePackObject> inArray){
            var uuid = ((MsgPack.MessagePackObject)inArray[0]).AsUInt64();
            var _errCode = ((MsgPack.MessagePackObject)inArray[1]).AsInt32();
            var rsp = try_get_and_del_create_scene_cb(uuid);
            if (rsp != null)
            {
                rsp.call_err(_errCode);
            }
        }

        public void create_scene_timeout(UInt64 cb_uuid){
            var rsp = try_get_and_del_create_scene_cb(cb_uuid);
            if (rsp != null){
                rsp.call_timeout();
            }
        }

        private scene_mgr_create_scene_cb try_get_and_del_create_scene_cb(UInt64 uuid){
            lock(map_create_scene)
            {
                if (map_create_scene.TryGetValue(uuid, out scene_mgr_create_scene_cb rsp))
                {
                    map_create_scene.Remove(uuid);
                }
                return rsp;
            }
        }

        public void player_level_scene_rsp(IList<MsgPack.MessagePackObject> inArray){
            var uuid = ((MsgPack.MessagePackObject)inArray[0]).AsUInt64();
            var rsp = try_get_and_del_player_level_scene_cb(uuid);
            if (rsp != null)
            {
                rsp.call_cb();
            }
        }

        public void player_level_scene_err(IList<MsgPack.MessagePackObject> inArray){
            var uuid = ((MsgPack.MessagePackObject)inArray[0]).AsUInt64();
            var _errCode = ((MsgPack.MessagePackObject)inArray[1]).AsInt32();
            var rsp = try_get_and_del_player_level_scene_cb(uuid);
            if (rsp != null)
            {
                rsp.call_err(_errCode);
            }
        }

        public void player_level_scene_timeout(UInt64 cb_uuid){
            var rsp = try_get_and_del_player_level_scene_cb(cb_uuid);
            if (rsp != null){
                rsp.call_timeout();
            }
        }

        private scene_mgr_player_level_scene_cb try_get_and_del_player_level_scene_cb(UInt64 uuid){
            lock(map_player_level_scene)
            {
                if (map_player_level_scene.TryGetValue(uuid, out scene_mgr_player_level_scene_cb rsp))
                {
                    map_player_level_scene.Remove(uuid);
                }
                return rsp;
            }
        }

        public void player_into_scene_rsp(IList<MsgPack.MessagePackObject> inArray){
            var uuid = ((MsgPack.MessagePackObject)inArray[0]).AsUInt64();
            var _info = SceneInfo.protcol_to_SceneInfo(((MsgPack.MessagePackObject)inArray[1]).AsDictionary());
            var rsp = try_get_and_del_player_into_scene_cb(uuid);
            if (rsp != null)
            {
                rsp.call_cb(_info);
            }
        }

        public void player_into_scene_err(IList<MsgPack.MessagePackObject> inArray){
            var uuid = ((MsgPack.MessagePackObject)inArray[0]).AsUInt64();
            var _errCode = ((MsgPack.MessagePackObject)inArray[1]).AsInt32();
            var _hub_info = scene_hub_info.protcol_to_scene_hub_info(((MsgPack.MessagePackObject)inArray[2]).AsDictionary());
            var rsp = try_get_and_del_player_into_scene_cb(uuid);
            if (rsp != null)
            {
                rsp.call_err(_errCode, _hub_info);
            }
        }

        public void player_into_scene_timeout(UInt64 cb_uuid){
            var rsp = try_get_and_del_player_into_scene_cb(cb_uuid);
            if (rsp != null){
                rsp.call_timeout();
            }
        }

        private scene_mgr_player_into_scene_cb try_get_and_del_player_into_scene_cb(UInt64 uuid){
            lock(map_player_into_scene)
            {
                if (map_player_into_scene.TryGetValue(uuid, out scene_mgr_player_into_scene_cb rsp))
                {
                    map_player_into_scene.Remove(uuid);
                }
                return rsp;
            }
        }

    }

    public class scene_mgr_caller {
        public static scene_mgr_rsp_cb rsp_cb_scene_mgr_handle = null;
        private ThreadLocal<scene_mgr_hubproxy> _hubproxy;
        public Client.Client _client_handle;
        public scene_mgr_caller(Client.Client client_handle_) 
        {
            _client_handle = client_handle_;
            if (rsp_cb_scene_mgr_handle == null)
            {
                rsp_cb_scene_mgr_handle = new scene_mgr_rsp_cb(_client_handle.modulemanager);
            }

            _hubproxy = new ThreadLocal<scene_mgr_hubproxy>();
        }

        public scene_mgr_hubproxy get_hub(string hub_name)
        {
            if (_hubproxy.Value == null)
{
                _hubproxy.Value = new scene_mgr_hubproxy(_client_handle, rsp_cb_scene_mgr_handle);
            }
            _hubproxy.Value.hub_name_38a10f62_3f3c_3cc5_80b0_664977cfd157 = hub_name;
            return _hubproxy.Value;
        }

    }

    public class scene_mgr_hubproxy {
        public string hub_name_38a10f62_3f3c_3cc5_80b0_664977cfd157;
        private Int32 uuid_38a10f62_3f3c_3cc5_80b0_664977cfd157 = (Int32)RandomUUID.random();

        public Client.Client _client_handle;
        public scene_mgr_rsp_cb rsp_cb_scene_mgr_handle;

        public scene_mgr_hubproxy(Client.Client client_handle_, scene_mgr_rsp_cb rsp_cb_scene_mgr_handle_)
        {
            _client_handle = client_handle_;
            rsp_cb_scene_mgr_handle = rsp_cb_scene_mgr_handle_;
        }

        public scene_mgr_get_scene_list_cb get_scene_list(){
            var uuid_d6c8da40_286e_5202_a23d_8acd3dc18a50 = (UInt64)Interlocked.Increment(ref uuid_38a10f62_3f3c_3cc5_80b0_664977cfd157);

            var _argv_c44290a6_e88a_3536_86ce_be8217f21cdc = new ArrayList();
            _argv_c44290a6_e88a_3536_86ce_be8217f21cdc.Add(uuid_d6c8da40_286e_5202_a23d_8acd3dc18a50);
            _client_handle.call_hub(hub_name_38a10f62_3f3c_3cc5_80b0_664977cfd157, "scene_mgr_get_scene_list", _argv_c44290a6_e88a_3536_86ce_be8217f21cdc);

            var cb_get_scene_list_obj = new scene_mgr_get_scene_list_cb(uuid_d6c8da40_286e_5202_a23d_8acd3dc18a50, rsp_cb_scene_mgr_handle);
            lock(rsp_cb_scene_mgr_handle.map_get_scene_list)
            {                rsp_cb_scene_mgr_handle.map_get_scene_list.Add(uuid_d6c8da40_286e_5202_a23d_8acd3dc18a50, cb_get_scene_list_obj);
            }            return cb_get_scene_list_obj;
        }

        public scene_mgr_create_scene_cb create_scene(string scene_name){
            var uuid_18b2cce0_96f0_5a1b_b662_f2f23c8580ba = (UInt64)Interlocked.Increment(ref uuid_38a10f62_3f3c_3cc5_80b0_664977cfd157);

            var _argv_44a7c36e_64cf_31e1_821c_eb7b2ed69ec7 = new ArrayList();
            _argv_44a7c36e_64cf_31e1_821c_eb7b2ed69ec7.Add(uuid_18b2cce0_96f0_5a1b_b662_f2f23c8580ba);
            _argv_44a7c36e_64cf_31e1_821c_eb7b2ed69ec7.Add(scene_name);
            _client_handle.call_hub(hub_name_38a10f62_3f3c_3cc5_80b0_664977cfd157, "scene_mgr_create_scene", _argv_44a7c36e_64cf_31e1_821c_eb7b2ed69ec7);

            var cb_create_scene_obj = new scene_mgr_create_scene_cb(uuid_18b2cce0_96f0_5a1b_b662_f2f23c8580ba, rsp_cb_scene_mgr_handle);
            lock(rsp_cb_scene_mgr_handle.map_create_scene)
            {                rsp_cb_scene_mgr_handle.map_create_scene.Add(uuid_18b2cce0_96f0_5a1b_b662_f2f23c8580ba, cb_create_scene_obj);
            }            return cb_create_scene_obj;
        }

        public scene_mgr_player_level_scene_cb player_level_scene(string scene_guid){
            var uuid_08c576a4_289e_5738_842a_5e8ce11e0c3d = (UInt64)Interlocked.Increment(ref uuid_38a10f62_3f3c_3cc5_80b0_664977cfd157);

            var _argv_60e2d084_e2d0_346d_9c06_77f5f8acfa40 = new ArrayList();
            _argv_60e2d084_e2d0_346d_9c06_77f5f8acfa40.Add(uuid_08c576a4_289e_5738_842a_5e8ce11e0c3d);
            _argv_60e2d084_e2d0_346d_9c06_77f5f8acfa40.Add(scene_guid);
            _client_handle.call_hub(hub_name_38a10f62_3f3c_3cc5_80b0_664977cfd157, "scene_mgr_player_level_scene", _argv_60e2d084_e2d0_346d_9c06_77f5f8acfa40);

            var cb_player_level_scene_obj = new scene_mgr_player_level_scene_cb(uuid_08c576a4_289e_5738_842a_5e8ce11e0c3d, rsp_cb_scene_mgr_handle);
            lock(rsp_cb_scene_mgr_handle.map_player_level_scene)
            {                rsp_cb_scene_mgr_handle.map_player_level_scene.Add(uuid_08c576a4_289e_5738_842a_5e8ce11e0c3d, cb_player_level_scene_obj);
            }            return cb_player_level_scene_obj;
        }

        public scene_mgr_player_into_scene_cb player_into_scene(string scene_guid, string scene_name, string userGuid, string userName, string userTableID){
            var uuid_eaec9b3f_5343_578b_824c_ade30f7a2362 = (UInt64)Interlocked.Increment(ref uuid_38a10f62_3f3c_3cc5_80b0_664977cfd157);

            var _argv_1d7f9f94_cc7a_356a_b9fb_36951dd62bc4 = new ArrayList();
            _argv_1d7f9f94_cc7a_356a_b9fb_36951dd62bc4.Add(uuid_eaec9b3f_5343_578b_824c_ade30f7a2362);
            _argv_1d7f9f94_cc7a_356a_b9fb_36951dd62bc4.Add(scene_guid);
            _argv_1d7f9f94_cc7a_356a_b9fb_36951dd62bc4.Add(scene_name);
            _argv_1d7f9f94_cc7a_356a_b9fb_36951dd62bc4.Add(userGuid);
            _argv_1d7f9f94_cc7a_356a_b9fb_36951dd62bc4.Add(userName);
            _argv_1d7f9f94_cc7a_356a_b9fb_36951dd62bc4.Add(userTableID);
            _client_handle.call_hub(hub_name_38a10f62_3f3c_3cc5_80b0_664977cfd157, "scene_mgr_player_into_scene", _argv_1d7f9f94_cc7a_356a_b9fb_36951dd62bc4);

            var cb_player_into_scene_obj = new scene_mgr_player_into_scene_cb(uuid_eaec9b3f_5343_578b_824c_ade30f7a2362, rsp_cb_scene_mgr_handle);
            lock(rsp_cb_scene_mgr_handle.map_player_into_scene)
            {                rsp_cb_scene_mgr_handle.map_player_into_scene.Add(uuid_eaec9b3f_5343_578b_824c_ade30f7a2362, cb_player_into_scene_obj);
            }            return cb_player_into_scene_obj;
        }

    }
    public class scene_import_role_cb
    {
        private UInt64 cb_uuid;
        private scene_rsp_cb module_rsp_cb;

        public scene_import_role_cb(UInt64 _cb_uuid, scene_rsp_cb _module_rsp_cb)
        {
            cb_uuid = _cb_uuid;
            module_rsp_cb = _module_rsp_cb;
        }

        public event Action<List<Role>> on_import_role_cb;
        public event Action<Int32> on_import_role_err;
        public event Action on_import_role_timeout;

        public scene_import_role_cb callBack(Action<List<Role>> cb, Action<Int32> err)
        {
            on_import_role_cb += cb;
            on_import_role_err += err;
            return this;
        }

        public void timeout(UInt64 tick, Action timeout_cb)
        {
            TinyTimer.add_timer(tick, ()=>{
                module_rsp_cb.import_role_timeout(cb_uuid);
            });
            on_import_role_timeout += timeout_cb;
        }

        public void call_cb(List<Role> roles)
        {
            if (on_import_role_cb != null)
            {
                on_import_role_cb(roles);
            }
        }

        public void call_err(Int32 errCode)
        {
            if (on_import_role_err != null)
            {
                on_import_role_err(errCode);
            }
        }

        public void call_timeout()
        {
            if (on_import_role_timeout != null)
            {
                on_import_role_timeout();
            }
        }

    }

    public class scene_grow_cb
    {
        private UInt64 cb_uuid;
        private scene_rsp_cb module_rsp_cb;

        public scene_grow_cb(UInt64 _cb_uuid, scene_rsp_cb _module_rsp_cb)
        {
            cb_uuid = _cb_uuid;
            module_rsp_cb = _module_rsp_cb;
        }

        public event Action<List<Vegetable>> on_grow_cb;
        public event Action<Int32> on_grow_err;
        public event Action on_grow_timeout;

        public scene_grow_cb callBack(Action<List<Vegetable>> cb, Action<Int32> err)
        {
            on_grow_cb += cb;
            on_grow_err += err;
            return this;
        }

        public void timeout(UInt64 tick, Action timeout_cb)
        {
            TinyTimer.add_timer(tick, ()=>{
                module_rsp_cb.grow_timeout(cb_uuid);
            });
            on_grow_timeout += timeout_cb;
        }

        public void call_cb(List<Vegetable> vegetables)
        {
            if (on_grow_cb != null)
            {
                on_grow_cb(vegetables);
            }
        }

        public void call_err(Int32 errCode)
        {
            if (on_grow_err != null)
            {
                on_grow_err(errCode);
            }
        }

        public void call_timeout()
        {
            if (on_grow_timeout != null)
            {
                on_grow_timeout();
            }
        }

    }

    public class scene_harvest_cb
    {
        private UInt64 cb_uuid;
        private scene_rsp_cb module_rsp_cb;

        public scene_harvest_cb(UInt64 _cb_uuid, scene_rsp_cb _module_rsp_cb)
        {
            cb_uuid = _cb_uuid;
            module_rsp_cb = _module_rsp_cb;
        }

        public event Action on_harvest_cb;
        public event Action<Int32> on_harvest_err;
        public event Action on_harvest_timeout;

        public scene_harvest_cb callBack(Action cb, Action<Int32> err)
        {
            on_harvest_cb += cb;
            on_harvest_err += err;
            return this;
        }

        public void timeout(UInt64 tick, Action timeout_cb)
        {
            TinyTimer.add_timer(tick, ()=>{
                module_rsp_cb.harvest_timeout(cb_uuid);
            });
            on_harvest_timeout += timeout_cb;
        }

        public void call_cb()
        {
            if (on_harvest_cb != null)
            {
                on_harvest_cb();
            }
        }

        public void call_err(Int32 errCode)
        {
            if (on_harvest_err != null)
            {
                on_harvest_err(errCode);
            }
        }

        public void call_timeout()
        {
            if (on_harvest_timeout != null)
            {
                on_harvest_timeout();
            }
        }

    }

    public class scene_cut_down_cb
    {
        private UInt64 cb_uuid;
        private scene_rsp_cb module_rsp_cb;

        public scene_cut_down_cb(UInt64 _cb_uuid, scene_rsp_cb _module_rsp_cb)
        {
            cb_uuid = _cb_uuid;
            module_rsp_cb = _module_rsp_cb;
        }

        public event Action on_cut_down_cb;
        public event Action<Int32> on_cut_down_err;
        public event Action on_cut_down_timeout;

        public scene_cut_down_cb callBack(Action cb, Action<Int32> err)
        {
            on_cut_down_cb += cb;
            on_cut_down_err += err;
            return this;
        }

        public void timeout(UInt64 tick, Action timeout_cb)
        {
            TinyTimer.add_timer(tick, ()=>{
                module_rsp_cb.cut_down_timeout(cb_uuid);
            });
            on_cut_down_timeout += timeout_cb;
        }

        public void call_cb()
        {
            if (on_cut_down_cb != null)
            {
                on_cut_down_cb();
            }
        }

        public void call_err(Int32 errCode)
        {
            if (on_cut_down_err != null)
            {
                on_cut_down_err(errCode);
            }
        }

        public void call_timeout()
        {
            if (on_cut_down_timeout != null)
            {
                on_cut_down_timeout();
            }
        }

    }

    public class scene_steal_cb
    {
        private UInt64 cb_uuid;
        private scene_rsp_cb module_rsp_cb;

        public scene_steal_cb(UInt64 _cb_uuid, scene_rsp_cb _module_rsp_cb)
        {
            cb_uuid = _cb_uuid;
            module_rsp_cb = _module_rsp_cb;
        }

        public event Action<Vegetable> on_steal_cb;
        public event Action<Int32> on_steal_err;
        public event Action on_steal_timeout;

        public scene_steal_cb callBack(Action<Vegetable> cb, Action<Int32> err)
        {
            on_steal_cb += cb;
            on_steal_err += err;
            return this;
        }

        public void timeout(UInt64 tick, Action timeout_cb)
        {
            TinyTimer.add_timer(tick, ()=>{
                module_rsp_cb.steal_timeout(cb_uuid);
            });
            on_steal_timeout += timeout_cb;
        }

        public void call_cb(Vegetable vegetable)
        {
            if (on_steal_cb != null)
            {
                on_steal_cb(vegetable);
            }
        }

        public void call_err(Int32 errCode)
        {
            if (on_steal_err != null)
            {
                on_steal_err(errCode);
            }
        }

        public void call_timeout()
        {
            if (on_steal_timeout != null)
            {
                on_steal_timeout();
            }
        }

    }

    public class scene_sales_vegetable_cb
    {
        private UInt64 cb_uuid;
        private scene_rsp_cb module_rsp_cb;

        public scene_sales_vegetable_cb(UInt64 _cb_uuid, scene_rsp_cb _module_rsp_cb)
        {
            cb_uuid = _cb_uuid;
            module_rsp_cb = _module_rsp_cb;
        }

        public event Action<Int32> on_sales_vegetable_cb;
        public event Action<Int32> on_sales_vegetable_err;
        public event Action on_sales_vegetable_timeout;

        public scene_sales_vegetable_cb callBack(Action<Int32> cb, Action<Int32> err)
        {
            on_sales_vegetable_cb += cb;
            on_sales_vegetable_err += err;
            return this;
        }

        public void timeout(UInt64 tick, Action timeout_cb)
        {
            TinyTimer.add_timer(tick, ()=>{
                module_rsp_cb.sales_vegetable_timeout(cb_uuid);
            });
            on_sales_vegetable_timeout += timeout_cb;
        }

        public void call_cb(Int32 coin)
        {
            if (on_sales_vegetable_cb != null)
            {
                on_sales_vegetable_cb(coin);
            }
        }

        public void call_err(Int32 errCode)
        {
            if (on_sales_vegetable_err != null)
            {
                on_sales_vegetable_err(errCode);
            }
        }

        public void call_timeout()
        {
            if (on_sales_vegetable_timeout != null)
            {
                on_sales_vegetable_timeout();
            }
        }

    }

/*this cb code is codegen by abelkhan for c#*/
    public class scene_rsp_cb : Common.IModule {
        public Dictionary<UInt64, scene_import_role_cb> map_import_role;
        public Dictionary<UInt64, scene_grow_cb> map_grow;
        public Dictionary<UInt64, scene_harvest_cb> map_harvest;
        public Dictionary<UInt64, scene_cut_down_cb> map_cut_down;
        public Dictionary<UInt64, scene_steal_cb> map_steal;
        public Dictionary<UInt64, scene_sales_vegetable_cb> map_sales_vegetable;
        public scene_rsp_cb(Common.ModuleManager modules)
        {
            map_import_role = new Dictionary<UInt64, scene_import_role_cb>();
            modules.add_mothed("scene_rsp_cb_import_role_rsp", import_role_rsp);
            modules.add_mothed("scene_rsp_cb_import_role_err", import_role_err);
            map_grow = new Dictionary<UInt64, scene_grow_cb>();
            modules.add_mothed("scene_rsp_cb_grow_rsp", grow_rsp);
            modules.add_mothed("scene_rsp_cb_grow_err", grow_err);
            map_harvest = new Dictionary<UInt64, scene_harvest_cb>();
            modules.add_mothed("scene_rsp_cb_harvest_rsp", harvest_rsp);
            modules.add_mothed("scene_rsp_cb_harvest_err", harvest_err);
            map_cut_down = new Dictionary<UInt64, scene_cut_down_cb>();
            modules.add_mothed("scene_rsp_cb_cut_down_rsp", cut_down_rsp);
            modules.add_mothed("scene_rsp_cb_cut_down_err", cut_down_err);
            map_steal = new Dictionary<UInt64, scene_steal_cb>();
            modules.add_mothed("scene_rsp_cb_steal_rsp", steal_rsp);
            modules.add_mothed("scene_rsp_cb_steal_err", steal_err);
            map_sales_vegetable = new Dictionary<UInt64, scene_sales_vegetable_cb>();
            modules.add_mothed("scene_rsp_cb_sales_vegetable_rsp", sales_vegetable_rsp);
            modules.add_mothed("scene_rsp_cb_sales_vegetable_err", sales_vegetable_err);
        }

        public void import_role_rsp(IList<MsgPack.MessagePackObject> inArray){
            var uuid = ((MsgPack.MessagePackObject)inArray[0]).AsUInt64();
            var _roles = new List<Role>();
            var _protocol_arrayroles = ((MsgPack.MessagePackObject)inArray[1]).AsList();
            foreach (var v_7e2e4050_27ab_52a0_b1bd_e4cfab4afda2 in _protocol_arrayroles){
                _roles.Add(Role.protcol_to_Role(((MsgPack.MessagePackObject)v_7e2e4050_27ab_52a0_b1bd_e4cfab4afda2).AsDictionary()));
            }
            var rsp = try_get_and_del_import_role_cb(uuid);
            if (rsp != null)
            {
                rsp.call_cb(_roles);
            }
        }

        public void import_role_err(IList<MsgPack.MessagePackObject> inArray){
            var uuid = ((MsgPack.MessagePackObject)inArray[0]).AsUInt64();
            var _errCode = ((MsgPack.MessagePackObject)inArray[1]).AsInt32();
            var rsp = try_get_and_del_import_role_cb(uuid);
            if (rsp != null)
            {
                rsp.call_err(_errCode);
            }
        }

        public void import_role_timeout(UInt64 cb_uuid){
            var rsp = try_get_and_del_import_role_cb(cb_uuid);
            if (rsp != null){
                rsp.call_timeout();
            }
        }

        private scene_import_role_cb try_get_and_del_import_role_cb(UInt64 uuid){
            lock(map_import_role)
            {
                if (map_import_role.TryGetValue(uuid, out scene_import_role_cb rsp))
                {
                    map_import_role.Remove(uuid);
                }
                return rsp;
            }
        }

        public void grow_rsp(IList<MsgPack.MessagePackObject> inArray){
            var uuid = ((MsgPack.MessagePackObject)inArray[0]).AsUInt64();
            var _vegetables = new List<Vegetable>();
            var _protocol_arrayvegetables = ((MsgPack.MessagePackObject)inArray[1]).AsList();
            foreach (var v_1b631c32_577d_53cf_91e7_c648319a3b24 in _protocol_arrayvegetables){
                _vegetables.Add(Vegetable.protcol_to_Vegetable(((MsgPack.MessagePackObject)v_1b631c32_577d_53cf_91e7_c648319a3b24).AsDictionary()));
            }
            var rsp = try_get_and_del_grow_cb(uuid);
            if (rsp != null)
            {
                rsp.call_cb(_vegetables);
            }
        }

        public void grow_err(IList<MsgPack.MessagePackObject> inArray){
            var uuid = ((MsgPack.MessagePackObject)inArray[0]).AsUInt64();
            var _errCode = ((MsgPack.MessagePackObject)inArray[1]).AsInt32();
            var rsp = try_get_and_del_grow_cb(uuid);
            if (rsp != null)
            {
                rsp.call_err(_errCode);
            }
        }

        public void grow_timeout(UInt64 cb_uuid){
            var rsp = try_get_and_del_grow_cb(cb_uuid);
            if (rsp != null){
                rsp.call_timeout();
            }
        }

        private scene_grow_cb try_get_and_del_grow_cb(UInt64 uuid){
            lock(map_grow)
            {
                if (map_grow.TryGetValue(uuid, out scene_grow_cb rsp))
                {
                    map_grow.Remove(uuid);
                }
                return rsp;
            }
        }

        public void harvest_rsp(IList<MsgPack.MessagePackObject> inArray){
            var uuid = ((MsgPack.MessagePackObject)inArray[0]).AsUInt64();
            var rsp = try_get_and_del_harvest_cb(uuid);
            if (rsp != null)
            {
                rsp.call_cb();
            }
        }

        public void harvest_err(IList<MsgPack.MessagePackObject> inArray){
            var uuid = ((MsgPack.MessagePackObject)inArray[0]).AsUInt64();
            var _errCode = ((MsgPack.MessagePackObject)inArray[1]).AsInt32();
            var rsp = try_get_and_del_harvest_cb(uuid);
            if (rsp != null)
            {
                rsp.call_err(_errCode);
            }
        }

        public void harvest_timeout(UInt64 cb_uuid){
            var rsp = try_get_and_del_harvest_cb(cb_uuid);
            if (rsp != null){
                rsp.call_timeout();
            }
        }

        private scene_harvest_cb try_get_and_del_harvest_cb(UInt64 uuid){
            lock(map_harvest)
            {
                if (map_harvest.TryGetValue(uuid, out scene_harvest_cb rsp))
                {
                    map_harvest.Remove(uuid);
                }
                return rsp;
            }
        }

        public void cut_down_rsp(IList<MsgPack.MessagePackObject> inArray){
            var uuid = ((MsgPack.MessagePackObject)inArray[0]).AsUInt64();
            var rsp = try_get_and_del_cut_down_cb(uuid);
            if (rsp != null)
            {
                rsp.call_cb();
            }
        }

        public void cut_down_err(IList<MsgPack.MessagePackObject> inArray){
            var uuid = ((MsgPack.MessagePackObject)inArray[0]).AsUInt64();
            var _errCode = ((MsgPack.MessagePackObject)inArray[1]).AsInt32();
            var rsp = try_get_and_del_cut_down_cb(uuid);
            if (rsp != null)
            {
                rsp.call_err(_errCode);
            }
        }

        public void cut_down_timeout(UInt64 cb_uuid){
            var rsp = try_get_and_del_cut_down_cb(cb_uuid);
            if (rsp != null){
                rsp.call_timeout();
            }
        }

        private scene_cut_down_cb try_get_and_del_cut_down_cb(UInt64 uuid){
            lock(map_cut_down)
            {
                if (map_cut_down.TryGetValue(uuid, out scene_cut_down_cb rsp))
                {
                    map_cut_down.Remove(uuid);
                }
                return rsp;
            }
        }

        public void steal_rsp(IList<MsgPack.MessagePackObject> inArray){
            var uuid = ((MsgPack.MessagePackObject)inArray[0]).AsUInt64();
            var _vegetable = Vegetable.protcol_to_Vegetable(((MsgPack.MessagePackObject)inArray[1]).AsDictionary());
            var rsp = try_get_and_del_steal_cb(uuid);
            if (rsp != null)
            {
                rsp.call_cb(_vegetable);
            }
        }

        public void steal_err(IList<MsgPack.MessagePackObject> inArray){
            var uuid = ((MsgPack.MessagePackObject)inArray[0]).AsUInt64();
            var _errCode = ((MsgPack.MessagePackObject)inArray[1]).AsInt32();
            var rsp = try_get_and_del_steal_cb(uuid);
            if (rsp != null)
            {
                rsp.call_err(_errCode);
            }
        }

        public void steal_timeout(UInt64 cb_uuid){
            var rsp = try_get_and_del_steal_cb(cb_uuid);
            if (rsp != null){
                rsp.call_timeout();
            }
        }

        private scene_steal_cb try_get_and_del_steal_cb(UInt64 uuid){
            lock(map_steal)
            {
                if (map_steal.TryGetValue(uuid, out scene_steal_cb rsp))
                {
                    map_steal.Remove(uuid);
                }
                return rsp;
            }
        }

        public void sales_vegetable_rsp(IList<MsgPack.MessagePackObject> inArray){
            var uuid = ((MsgPack.MessagePackObject)inArray[0]).AsUInt64();
            var _coin = ((MsgPack.MessagePackObject)inArray[1]).AsInt32();
            var rsp = try_get_and_del_sales_vegetable_cb(uuid);
            if (rsp != null)
            {
                rsp.call_cb(_coin);
            }
        }

        public void sales_vegetable_err(IList<MsgPack.MessagePackObject> inArray){
            var uuid = ((MsgPack.MessagePackObject)inArray[0]).AsUInt64();
            var _errCode = ((MsgPack.MessagePackObject)inArray[1]).AsInt32();
            var rsp = try_get_and_del_sales_vegetable_cb(uuid);
            if (rsp != null)
            {
                rsp.call_err(_errCode);
            }
        }

        public void sales_vegetable_timeout(UInt64 cb_uuid){
            var rsp = try_get_and_del_sales_vegetable_cb(cb_uuid);
            if (rsp != null){
                rsp.call_timeout();
            }
        }

        private scene_sales_vegetable_cb try_get_and_del_sales_vegetable_cb(UInt64 uuid){
            lock(map_sales_vegetable)
            {
                if (map_sales_vegetable.TryGetValue(uuid, out scene_sales_vegetable_cb rsp))
                {
                    map_sales_vegetable.Remove(uuid);
                }
                return rsp;
            }
        }

    }

    public class scene_caller {
        public static scene_rsp_cb rsp_cb_scene_handle = null;
        private ThreadLocal<scene_hubproxy> _hubproxy;
        public Client.Client _client_handle;
        public scene_caller(Client.Client client_handle_) 
        {
            _client_handle = client_handle_;
            if (rsp_cb_scene_handle == null)
            {
                rsp_cb_scene_handle = new scene_rsp_cb(_client_handle.modulemanager);
            }

            _hubproxy = new ThreadLocal<scene_hubproxy>();
        }

        public scene_hubproxy get_hub(string hub_name)
        {
            if (_hubproxy.Value == null)
{
                _hubproxy.Value = new scene_hubproxy(_client_handle, rsp_cb_scene_handle);
            }
            _hubproxy.Value.hub_name_d9ff8761_7787_3ada_910b_8924a79ea4fb = hub_name;
            return _hubproxy.Value;
        }

    }

    public class scene_hubproxy {
        public string hub_name_d9ff8761_7787_3ada_910b_8924a79ea4fb;
        private Int32 uuid_d9ff8761_7787_3ada_910b_8924a79ea4fb = (Int32)RandomUUID.random();

        public Client.Client _client_handle;
        public scene_rsp_cb rsp_cb_scene_handle;

        public scene_hubproxy(Client.Client client_handle_, scene_rsp_cb rsp_cb_scene_handle_)
        {
            _client_handle = client_handle_;
            rsp_cb_scene_handle = rsp_cb_scene_handle_;
        }

        public void move(Pos start, Direction dir){
            var _argv_33efb72e_9227_32af_a058_169be114a277 = new ArrayList();
            _argv_33efb72e_9227_32af_a058_169be114a277.Add(Pos.Pos_to_protcol(start));
            _argv_33efb72e_9227_32af_a058_169be114a277.Add((int)dir);
            _client_handle.call_hub(hub_name_d9ff8761_7787_3ada_910b_8924a79ea4fb, "scene_move", _argv_33efb72e_9227_32af_a058_169be114a277);
        }

        public void remove_building(List<Building> buildings, List<Cultivated> cultivateds, bool rmfences){
            var _argv_902c91ab_d52f_3cce_9a07_6204aa146597 = new ArrayList();
            var _array_98ba47f5_b9b8_3536_9493_f5e2255b10a9 = new ArrayList();
            foreach(var v_08f36ecb_8228_5356_bb8a_753f8de7b7e3 in buildings){
                _array_98ba47f5_b9b8_3536_9493_f5e2255b10a9.Add(Building.Building_to_protcol(v_08f36ecb_8228_5356_bb8a_753f8de7b7e3));
            }
            _argv_902c91ab_d52f_3cce_9a07_6204aa146597.Add(_array_98ba47f5_b9b8_3536_9493_f5e2255b10a9);
            var _array_ca7f17ea_1480_30b1_a928_770254a9a812 = new ArrayList();
            foreach(var v_c6891ca4_7c79_57d2_a451_0494fcb49a22 in cultivateds){
                _array_ca7f17ea_1480_30b1_a928_770254a9a812.Add(Cultivated.Cultivated_to_protcol(v_c6891ca4_7c79_57d2_a451_0494fcb49a22));
            }
            _argv_902c91ab_d52f_3cce_9a07_6204aa146597.Add(_array_ca7f17ea_1480_30b1_a928_770254a9a812);
            _argv_902c91ab_d52f_3cce_9a07_6204aa146597.Add(rmfences);
            _client_handle.call_hub(hub_name_d9ff8761_7787_3ada_910b_8924a79ea4fb, "scene_remove_building", _argv_902c91ab_d52f_3cce_9a07_6204aa146597);
        }

        public void building(List<Building> buildings, Fence fences){
            var _argv_202540e5_3aa7_324c_85c8_f7da9821e7e6 = new ArrayList();
            var _array_98ba47f5_b9b8_3536_9493_f5e2255b10a9 = new ArrayList();
            foreach(var v_08f36ecb_8228_5356_bb8a_753f8de7b7e3 in buildings){
                _array_98ba47f5_b9b8_3536_9493_f5e2255b10a9.Add(Building.Building_to_protcol(v_08f36ecb_8228_5356_bb8a_753f8de7b7e3));
            }
            _argv_202540e5_3aa7_324c_85c8_f7da9821e7e6.Add(_array_98ba47f5_b9b8_3536_9493_f5e2255b10a9);
            _argv_202540e5_3aa7_324c_85c8_f7da9821e7e6.Add(Fence.Fence_to_protcol(fences));
            _client_handle.call_hub(hub_name_d9ff8761_7787_3ada_910b_8924a79ea4fb, "scene_building", _argv_202540e5_3aa7_324c_85c8_f7da9821e7e6);
        }

        public void arable_land(List<Cultivated> cultivateds){
            var _argv_a9d5462f_2c1a_3707_9b7a_9096dc04856d = new ArrayList();
            var _array_ca7f17ea_1480_30b1_a928_770254a9a812 = new ArrayList();
            foreach(var v_c6891ca4_7c79_57d2_a451_0494fcb49a22 in cultivateds){
                _array_ca7f17ea_1480_30b1_a928_770254a9a812.Add(Cultivated.Cultivated_to_protcol(v_c6891ca4_7c79_57d2_a451_0494fcb49a22));
            }
            _argv_a9d5462f_2c1a_3707_9b7a_9096dc04856d.Add(_array_ca7f17ea_1480_30b1_a928_770254a9a812);
            _client_handle.call_hub(hub_name_d9ff8761_7787_3ada_910b_8924a79ea4fb, "scene_arable_land", _argv_a9d5462f_2c1a_3707_9b7a_9096dc04856d);
        }

        public void scene_change(SceneChange change){
            var _argv_cca84eac_0cfa_36a4_9e43_af556f6a3c90 = new ArrayList();
            _argv_cca84eac_0cfa_36a4_9e43_af556f6a3c90.Add(SceneChange.SceneChange_to_protcol(change));
            _client_handle.call_hub(hub_name_d9ff8761_7787_3ada_910b_8924a79ea4fb, "scene_scene_change", _argv_cca84eac_0cfa_36a4_9e43_af556f6a3c90);
        }

        public void into_subscene(string subscene_name){
            var _argv_40a2ffe9_3842_3507_a8ed_ed14adcf405c = new ArrayList();
            _argv_40a2ffe9_3842_3507_a8ed_ed14adcf405c.Add(subscene_name);
            _client_handle.call_hub(hub_name_d9ff8761_7787_3ada_910b_8924a79ea4fb, "scene_into_subscene", _argv_40a2ffe9_3842_3507_a8ed_ed14adcf405c);
        }

        public void init_fence(Fence fences, string pwd){
            var _argv_f1879931_788d_3192_a30c_80e0dfb4644a = new ArrayList();
            _argv_f1879931_788d_3192_a30c_80e0dfb4644a.Add(Fence.Fence_to_protcol(fences));
            _argv_f1879931_788d_3192_a30c_80e0dfb4644a.Add(pwd);
            _client_handle.call_hub(hub_name_d9ff8761_7787_3ada_910b_8924a79ea4fb, "scene_init_fence", _argv_f1879931_788d_3192_a30c_80e0dfb4644a);
        }

        public scene_import_role_cb import_role(Role role){
            var uuid_52e6fe4a_a64a_5899_b104_e0e0dd86016a = (UInt64)Interlocked.Increment(ref uuid_d9ff8761_7787_3ada_910b_8924a79ea4fb);

            var _argv_c230e5af_6701_305e_be75_a718edc8d818 = new ArrayList();
            _argv_c230e5af_6701_305e_be75_a718edc8d818.Add(uuid_52e6fe4a_a64a_5899_b104_e0e0dd86016a);
            _argv_c230e5af_6701_305e_be75_a718edc8d818.Add(Role.Role_to_protcol(role));
            _client_handle.call_hub(hub_name_d9ff8761_7787_3ada_910b_8924a79ea4fb, "scene_import_role", _argv_c230e5af_6701_305e_be75_a718edc8d818);

            var cb_import_role_obj = new scene_import_role_cb(uuid_52e6fe4a_a64a_5899_b104_e0e0dd86016a, rsp_cb_scene_handle);
            lock(rsp_cb_scene_handle.map_import_role)
            {                rsp_cb_scene_handle.map_import_role.Add(uuid_52e6fe4a_a64a_5899_b104_e0e0dd86016a, cb_import_role_obj);
            }            return cb_import_role_obj;
        }

        public scene_grow_cb grow(Vegetable vegetable, Cultivated cultivated){
            var uuid_b69b8879_dfa1_5c57_9211_34f2687260e6 = (UInt64)Interlocked.Increment(ref uuid_d9ff8761_7787_3ada_910b_8924a79ea4fb);

            var _argv_672a87cb_5af2_3344_84fd_fe0130666cba = new ArrayList();
            _argv_672a87cb_5af2_3344_84fd_fe0130666cba.Add(uuid_b69b8879_dfa1_5c57_9211_34f2687260e6);
            _argv_672a87cb_5af2_3344_84fd_fe0130666cba.Add(Vegetable.Vegetable_to_protcol(vegetable));
            _argv_672a87cb_5af2_3344_84fd_fe0130666cba.Add(Cultivated.Cultivated_to_protcol(cultivated));
            _client_handle.call_hub(hub_name_d9ff8761_7787_3ada_910b_8924a79ea4fb, "scene_grow", _argv_672a87cb_5af2_3344_84fd_fe0130666cba);

            var cb_grow_obj = new scene_grow_cb(uuid_b69b8879_dfa1_5c57_9211_34f2687260e6, rsp_cb_scene_handle);
            lock(rsp_cb_scene_handle.map_grow)
            {                rsp_cb_scene_handle.map_grow.Add(uuid_b69b8879_dfa1_5c57_9211_34f2687260e6, cb_grow_obj);
            }            return cb_grow_obj;
        }

        public scene_harvest_cb harvest(Vegetable vegetable){
            var uuid_31c95de5_9b20_51cd_b1cf_7e67250fc830 = (UInt64)Interlocked.Increment(ref uuid_d9ff8761_7787_3ada_910b_8924a79ea4fb);

            var _argv_a180627d_6e3c_3a7b_9a46_391f6553d65b = new ArrayList();
            _argv_a180627d_6e3c_3a7b_9a46_391f6553d65b.Add(uuid_31c95de5_9b20_51cd_b1cf_7e67250fc830);
            _argv_a180627d_6e3c_3a7b_9a46_391f6553d65b.Add(Vegetable.Vegetable_to_protcol(vegetable));
            _client_handle.call_hub(hub_name_d9ff8761_7787_3ada_910b_8924a79ea4fb, "scene_harvest", _argv_a180627d_6e3c_3a7b_9a46_391f6553d65b);

            var cb_harvest_obj = new scene_harvest_cb(uuid_31c95de5_9b20_51cd_b1cf_7e67250fc830, rsp_cb_scene_handle);
            lock(rsp_cb_scene_handle.map_harvest)
            {                rsp_cb_scene_handle.map_harvest.Add(uuid_31c95de5_9b20_51cd_b1cf_7e67250fc830, cb_harvest_obj);
            }            return cb_harvest_obj;
        }

        public scene_cut_down_cb cut_down(Vegetable vegetable){
            var uuid_e2d66685_d715_5bf1_b842_500825e79200 = (UInt64)Interlocked.Increment(ref uuid_d9ff8761_7787_3ada_910b_8924a79ea4fb);

            var _argv_8cccd442_c704_3649_85b1_e458e0261e66 = new ArrayList();
            _argv_8cccd442_c704_3649_85b1_e458e0261e66.Add(uuid_e2d66685_d715_5bf1_b842_500825e79200);
            _argv_8cccd442_c704_3649_85b1_e458e0261e66.Add(Vegetable.Vegetable_to_protcol(vegetable));
            _client_handle.call_hub(hub_name_d9ff8761_7787_3ada_910b_8924a79ea4fb, "scene_cut_down", _argv_8cccd442_c704_3649_85b1_e458e0261e66);

            var cb_cut_down_obj = new scene_cut_down_cb(uuid_e2d66685_d715_5bf1_b842_500825e79200, rsp_cb_scene_handle);
            lock(rsp_cb_scene_handle.map_cut_down)
            {                rsp_cb_scene_handle.map_cut_down.Add(uuid_e2d66685_d715_5bf1_b842_500825e79200, cb_cut_down_obj);
            }            return cb_cut_down_obj;
        }

        public scene_steal_cb steal(Vegetable vegetable){
            var uuid_04333952_49e3_5b20_a0d6_23e9aa7a8fdc = (UInt64)Interlocked.Increment(ref uuid_d9ff8761_7787_3ada_910b_8924a79ea4fb);

            var _argv_211c2516_ca86_3274_9c27_3cd41662932a = new ArrayList();
            _argv_211c2516_ca86_3274_9c27_3cd41662932a.Add(uuid_04333952_49e3_5b20_a0d6_23e9aa7a8fdc);
            _argv_211c2516_ca86_3274_9c27_3cd41662932a.Add(Vegetable.Vegetable_to_protcol(vegetable));
            _client_handle.call_hub(hub_name_d9ff8761_7787_3ada_910b_8924a79ea4fb, "scene_steal", _argv_211c2516_ca86_3274_9c27_3cd41662932a);

            var cb_steal_obj = new scene_steal_cb(uuid_04333952_49e3_5b20_a0d6_23e9aa7a8fdc, rsp_cb_scene_handle);
            lock(rsp_cb_scene_handle.map_steal)
            {                rsp_cb_scene_handle.map_steal.Add(uuid_04333952_49e3_5b20_a0d6_23e9aa7a8fdc, cb_steal_obj);
            }            return cb_steal_obj;
        }

        public scene_sales_vegetable_cb sales_vegetable(Vegetable vegetable, Int32 coin){
            var uuid_08b12878_b956_5307_9325_bbcbfdfde2a5 = (UInt64)Interlocked.Increment(ref uuid_d9ff8761_7787_3ada_910b_8924a79ea4fb);

            var _argv_11b25d19_885a_3403_9235_abd0391f361e = new ArrayList();
            _argv_11b25d19_885a_3403_9235_abd0391f361e.Add(uuid_08b12878_b956_5307_9325_bbcbfdfde2a5);
            _argv_11b25d19_885a_3403_9235_abd0391f361e.Add(Vegetable.Vegetable_to_protcol(vegetable));
            _argv_11b25d19_885a_3403_9235_abd0391f361e.Add(coin);
            _client_handle.call_hub(hub_name_d9ff8761_7787_3ada_910b_8924a79ea4fb, "scene_sales_vegetable", _argv_11b25d19_885a_3403_9235_abd0391f361e);

            var cb_sales_vegetable_obj = new scene_sales_vegetable_cb(uuid_08b12878_b956_5307_9325_bbcbfdfde2a5, rsp_cb_scene_handle);
            lock(rsp_cb_scene_handle.map_sales_vegetable)
            {                rsp_cb_scene_handle.map_sales_vegetable.Add(uuid_08b12878_b956_5307_9325_bbcbfdfde2a5, cb_sales_vegetable_obj);
            }            return cb_sales_vegetable_obj;
        }

        public void npc_into_scene(string scene_guid, NPCInformation info){
            var _argv_dfad8457_dcee_3978_a79e_ed1df565e3ad = new ArrayList();
            _argv_dfad8457_dcee_3978_a79e_ed1df565e3ad.Add(scene_guid);
            _argv_dfad8457_dcee_3978_a79e_ed1df565e3ad.Add(NPCInformation.NPCInformation_to_protcol(info));
            _client_handle.call_hub(hub_name_d9ff8761_7787_3ada_910b_8924a79ea4fb, "scene_npc_into_scene", _argv_dfad8457_dcee_3978_a79e_ed1df565e3ad);
        }

        public void npc_into_subscene(string scene_guid, string npcUUID, string subscene_name){
            var _argv_9de544b1_d073_3ff8_a6d7_6ce70574980a = new ArrayList();
            _argv_9de544b1_d073_3ff8_a6d7_6ce70574980a.Add(scene_guid);
            _argv_9de544b1_d073_3ff8_a6d7_6ce70574980a.Add(npcUUID);
            _argv_9de544b1_d073_3ff8_a6d7_6ce70574980a.Add(subscene_name);
            _client_handle.call_hub(hub_name_d9ff8761_7787_3ada_910b_8924a79ea4fb, "scene_npc_into_subscene", _argv_9de544b1_d073_3ff8_a6d7_6ce70574980a);
        }

        public void npc_move(string npcUUID, string scene_guid, string subscene_name, Pos start, Direction dir){
            var _argv_0b9a1f7a_51d1_39ff_bff1_38cf7425643d = new ArrayList();
            _argv_0b9a1f7a_51d1_39ff_bff1_38cf7425643d.Add(npcUUID);
            _argv_0b9a1f7a_51d1_39ff_bff1_38cf7425643d.Add(scene_guid);
            _argv_0b9a1f7a_51d1_39ff_bff1_38cf7425643d.Add(subscene_name);
            _argv_0b9a1f7a_51d1_39ff_bff1_38cf7425643d.Add(Pos.Pos_to_protcol(start));
            _argv_0b9a1f7a_51d1_39ff_bff1_38cf7425643d.Add((int)dir);
            _client_handle.call_hub(hub_name_d9ff8761_7787_3ada_910b_8924a79ea4fb, "scene_npc_move", _argv_0b9a1f7a_51d1_39ff_bff1_38cf7425643d);
        }

    }

}
