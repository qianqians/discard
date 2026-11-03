using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using MsgPack.Serialization;

namespace Abelkhan
{
/*this enum code is codegen by abelkhan codegen for c#*/

/*this struct code is codegen by abelkhan codegen for c#*/
/*this module code is codegen by abelkhan codegen for c#*/
    public class scene_mgr_get_scene_list_rsp : Common.Response {
        private string _client_uuid_c44290a6_e88a_3536_86ce_be8217f21cdc;
        private UInt64 uuid_29b8fbfc_39fb_308d_937c_5a53271a0473;
        public scene_mgr_get_scene_list_rsp(string client_uuid, UInt64 _uuid)
        {
            _client_uuid_c44290a6_e88a_3536_86ce_be8217f21cdc = client_uuid;
            uuid_29b8fbfc_39fb_308d_937c_5a53271a0473 = _uuid;
        }

        public void rsp(List<scene_hub_info> info_391fd3d4_2d55_3f5e_9223_7f450a814a15){
            var _argv_c44290a6_e88a_3536_86ce_be8217f21cdc = new ArrayList();
            _argv_c44290a6_e88a_3536_86ce_be8217f21cdc.Add(uuid_29b8fbfc_39fb_308d_937c_5a53271a0473);
            var _array_391fd3d4_2d55_3f5e_9223_7f450a814a15 = new ArrayList();
            foreach(var v_d856b000_56e0_5f62_a6f3_e6a0c7859745 in info_391fd3d4_2d55_3f5e_9223_7f450a814a15){
                _array_391fd3d4_2d55_3f5e_9223_7f450a814a15.Add(scene_hub_info.scene_hub_info_to_protcol(v_d856b000_56e0_5f62_a6f3_e6a0c7859745));
            }
            _argv_c44290a6_e88a_3536_86ce_be8217f21cdc.Add(_array_391fd3d4_2d55_3f5e_9223_7f450a814a15);
            Hub.Hub._gates.call_client(_client_uuid_c44290a6_e88a_3536_86ce_be8217f21cdc, "scene_mgr_rsp_cb_get_scene_list_rsp", _argv_c44290a6_e88a_3536_86ce_be8217f21cdc);
        }

        public void err(Int32 errCode_c2a4463a_f4dd_322e_bbf5_24a0acf9d2ec){
            var _argv_c44290a6_e88a_3536_86ce_be8217f21cdc = new ArrayList();
            _argv_c44290a6_e88a_3536_86ce_be8217f21cdc.Add(uuid_29b8fbfc_39fb_308d_937c_5a53271a0473);
            _argv_c44290a6_e88a_3536_86ce_be8217f21cdc.Add(errCode_c2a4463a_f4dd_322e_bbf5_24a0acf9d2ec);
            Hub.Hub._gates.call_client(_client_uuid_c44290a6_e88a_3536_86ce_be8217f21cdc, "scene_mgr_rsp_cb_get_scene_list_err", _argv_c44290a6_e88a_3536_86ce_be8217f21cdc);
        }

    }

    public class scene_mgr_create_scene_rsp : Common.Response {
        private string _client_uuid_44a7c36e_64cf_31e1_821c_eb7b2ed69ec7;
        private UInt64 uuid_aedeae4e_6650_3d43_ad1f_28f39ad880fa;
        public scene_mgr_create_scene_rsp(string client_uuid, UInt64 _uuid)
        {
            _client_uuid_44a7c36e_64cf_31e1_821c_eb7b2ed69ec7 = client_uuid;
            uuid_aedeae4e_6650_3d43_ad1f_28f39ad880fa = _uuid;
        }

        public void rsp(scene_hub_info info_391fd3d4_2d55_3f5e_9223_7f450a814a15){
            var _argv_44a7c36e_64cf_31e1_821c_eb7b2ed69ec7 = new ArrayList();
            _argv_44a7c36e_64cf_31e1_821c_eb7b2ed69ec7.Add(uuid_aedeae4e_6650_3d43_ad1f_28f39ad880fa);
            _argv_44a7c36e_64cf_31e1_821c_eb7b2ed69ec7.Add(scene_hub_info.scene_hub_info_to_protcol(info_391fd3d4_2d55_3f5e_9223_7f450a814a15));
            Hub.Hub._gates.call_client(_client_uuid_44a7c36e_64cf_31e1_821c_eb7b2ed69ec7, "scene_mgr_rsp_cb_create_scene_rsp", _argv_44a7c36e_64cf_31e1_821c_eb7b2ed69ec7);
        }

        public void err(Int32 errCode_c2a4463a_f4dd_322e_bbf5_24a0acf9d2ec){
            var _argv_44a7c36e_64cf_31e1_821c_eb7b2ed69ec7 = new ArrayList();
            _argv_44a7c36e_64cf_31e1_821c_eb7b2ed69ec7.Add(uuid_aedeae4e_6650_3d43_ad1f_28f39ad880fa);
            _argv_44a7c36e_64cf_31e1_821c_eb7b2ed69ec7.Add(errCode_c2a4463a_f4dd_322e_bbf5_24a0acf9d2ec);
            Hub.Hub._gates.call_client(_client_uuid_44a7c36e_64cf_31e1_821c_eb7b2ed69ec7, "scene_mgr_rsp_cb_create_scene_err", _argv_44a7c36e_64cf_31e1_821c_eb7b2ed69ec7);
        }

    }

    public class scene_mgr_player_level_scene_rsp : Common.Response {
        private string _client_uuid_60e2d084_e2d0_346d_9c06_77f5f8acfa40;
        private UInt64 uuid_db8dc61f_ed2e_315f_a4bd_1818a35596c2;
        public scene_mgr_player_level_scene_rsp(string client_uuid, UInt64 _uuid)
        {
            _client_uuid_60e2d084_e2d0_346d_9c06_77f5f8acfa40 = client_uuid;
            uuid_db8dc61f_ed2e_315f_a4bd_1818a35596c2 = _uuid;
        }

        public void rsp(){
            var _argv_60e2d084_e2d0_346d_9c06_77f5f8acfa40 = new ArrayList();
            _argv_60e2d084_e2d0_346d_9c06_77f5f8acfa40.Add(uuid_db8dc61f_ed2e_315f_a4bd_1818a35596c2);
            Hub.Hub._gates.call_client(_client_uuid_60e2d084_e2d0_346d_9c06_77f5f8acfa40, "scene_mgr_rsp_cb_player_level_scene_rsp", _argv_60e2d084_e2d0_346d_9c06_77f5f8acfa40);
        }

        public void err(Int32 errCode_c2a4463a_f4dd_322e_bbf5_24a0acf9d2ec){
            var _argv_60e2d084_e2d0_346d_9c06_77f5f8acfa40 = new ArrayList();
            _argv_60e2d084_e2d0_346d_9c06_77f5f8acfa40.Add(uuid_db8dc61f_ed2e_315f_a4bd_1818a35596c2);
            _argv_60e2d084_e2d0_346d_9c06_77f5f8acfa40.Add(errCode_c2a4463a_f4dd_322e_bbf5_24a0acf9d2ec);
            Hub.Hub._gates.call_client(_client_uuid_60e2d084_e2d0_346d_9c06_77f5f8acfa40, "scene_mgr_rsp_cb_player_level_scene_err", _argv_60e2d084_e2d0_346d_9c06_77f5f8acfa40);
        }

    }

    public class scene_mgr_player_into_scene_rsp : Common.Response {
        private string _client_uuid_1d7f9f94_cc7a_356a_b9fb_36951dd62bc4;
        private UInt64 uuid_2cc6284e_8b8f_32ec_954c_b6da4b42082c;
        public scene_mgr_player_into_scene_rsp(string client_uuid, UInt64 _uuid)
        {
            _client_uuid_1d7f9f94_cc7a_356a_b9fb_36951dd62bc4 = client_uuid;
            uuid_2cc6284e_8b8f_32ec_954c_b6da4b42082c = _uuid;
        }

        public void rsp(SceneInfo info_391fd3d4_2d55_3f5e_9223_7f450a814a15){
            var _argv_1d7f9f94_cc7a_356a_b9fb_36951dd62bc4 = new ArrayList();
            _argv_1d7f9f94_cc7a_356a_b9fb_36951dd62bc4.Add(uuid_2cc6284e_8b8f_32ec_954c_b6da4b42082c);
            _argv_1d7f9f94_cc7a_356a_b9fb_36951dd62bc4.Add(SceneInfo.SceneInfo_to_protcol(info_391fd3d4_2d55_3f5e_9223_7f450a814a15));
            Hub.Hub._gates.call_client(_client_uuid_1d7f9f94_cc7a_356a_b9fb_36951dd62bc4, "scene_mgr_rsp_cb_player_into_scene_rsp", _argv_1d7f9f94_cc7a_356a_b9fb_36951dd62bc4);
        }

        public void err(Int32 errCode_c2a4463a_f4dd_322e_bbf5_24a0acf9d2ec, scene_hub_info hub_info_4ca94c1e_3083_3fe9_a4f0_b4f03b01b0f2){
            var _argv_1d7f9f94_cc7a_356a_b9fb_36951dd62bc4 = new ArrayList();
            _argv_1d7f9f94_cc7a_356a_b9fb_36951dd62bc4.Add(uuid_2cc6284e_8b8f_32ec_954c_b6da4b42082c);
            _argv_1d7f9f94_cc7a_356a_b9fb_36951dd62bc4.Add(errCode_c2a4463a_f4dd_322e_bbf5_24a0acf9d2ec);
            _argv_1d7f9f94_cc7a_356a_b9fb_36951dd62bc4.Add(scene_hub_info.scene_hub_info_to_protcol(hub_info_4ca94c1e_3083_3fe9_a4f0_b4f03b01b0f2));
            Hub.Hub._gates.call_client(_client_uuid_1d7f9f94_cc7a_356a_b9fb_36951dd62bc4, "scene_mgr_rsp_cb_player_into_scene_err", _argv_1d7f9f94_cc7a_356a_b9fb_36951dd62bc4);
        }

    }

    public class scene_mgr_module : Common.IModule {
        public scene_mgr_module()
        {
            Hub.Hub._modules.add_mothed("scene_mgr_get_scene_list", get_scene_list);
            Hub.Hub._modules.add_mothed("scene_mgr_create_scene", create_scene);
            Hub.Hub._modules.add_mothed("scene_mgr_player_level_scene", player_level_scene);
            Hub.Hub._modules.add_mothed("scene_mgr_player_into_scene", player_into_scene);
        }

        public event Action on_get_scene_list;
        public void get_scene_list(IList<MsgPack.MessagePackObject> inArray){
            var _cb_uuid = ((MsgPack.MessagePackObject)inArray[0]).AsUInt64();
            rsp = new scene_mgr_get_scene_list_rsp(Hub.Hub._gates.current_client_uuid, _cb_uuid);
            if (on_get_scene_list != null){
                on_get_scene_list();
            }
            rsp = null;
        }

        public event Action<string> on_create_scene;
        public void create_scene(IList<MsgPack.MessagePackObject> inArray){
            var _cb_uuid = ((MsgPack.MessagePackObject)inArray[0]).AsUInt64();
            var _scene_name = ((MsgPack.MessagePackObject)inArray[1]).AsString();
            rsp = new scene_mgr_create_scene_rsp(Hub.Hub._gates.current_client_uuid, _cb_uuid);
            if (on_create_scene != null){
                on_create_scene(_scene_name);
            }
            rsp = null;
        }

        public event Action<string> on_player_level_scene;
        public void player_level_scene(IList<MsgPack.MessagePackObject> inArray){
            var _cb_uuid = ((MsgPack.MessagePackObject)inArray[0]).AsUInt64();
            var _scene_guid = ((MsgPack.MessagePackObject)inArray[1]).AsString();
            rsp = new scene_mgr_player_level_scene_rsp(Hub.Hub._gates.current_client_uuid, _cb_uuid);
            if (on_player_level_scene != null){
                on_player_level_scene(_scene_guid);
            }
            rsp = null;
        }

        public event Action<string, string, string, string, string> on_player_into_scene;
        public void player_into_scene(IList<MsgPack.MessagePackObject> inArray){
            var _cb_uuid = ((MsgPack.MessagePackObject)inArray[0]).AsUInt64();
            var _scene_guid = ((MsgPack.MessagePackObject)inArray[1]).AsString();
            var _scene_name = ((MsgPack.MessagePackObject)inArray[2]).AsString();
            var _userGuid = ((MsgPack.MessagePackObject)inArray[3]).AsString();
            var _userName = ((MsgPack.MessagePackObject)inArray[4]).AsString();
            var _userTableID = ((MsgPack.MessagePackObject)inArray[5]).AsString();
            rsp = new scene_mgr_player_into_scene_rsp(Hub.Hub._gates.current_client_uuid, _cb_uuid);
            if (on_player_into_scene != null){
                on_player_into_scene(_scene_guid, _scene_name, _userGuid, _userName, _userTableID);
            }
            rsp = null;
        }

    }
    public class scene_import_role_rsp : Common.Response {
        private string _client_uuid_c230e5af_6701_305e_be75_a718edc8d818;
        private UInt64 uuid_ce4937ed_df79_3445_b84b_8122980631f7;
        public scene_import_role_rsp(string client_uuid, UInt64 _uuid)
        {
            _client_uuid_c230e5af_6701_305e_be75_a718edc8d818 = client_uuid;
            uuid_ce4937ed_df79_3445_b84b_8122980631f7 = _uuid;
        }

        public void rsp(List<Role> roles_5fea3b35_c7ed_34c2_b4d8_f143b1af844a){
            var _argv_c230e5af_6701_305e_be75_a718edc8d818 = new ArrayList();
            _argv_c230e5af_6701_305e_be75_a718edc8d818.Add(uuid_ce4937ed_df79_3445_b84b_8122980631f7);
            var _array_5fea3b35_c7ed_34c2_b4d8_f143b1af844a = new ArrayList();
            foreach(var v_7e2e4050_27ab_52a0_b1bd_e4cfab4afda2 in roles_5fea3b35_c7ed_34c2_b4d8_f143b1af844a){
                _array_5fea3b35_c7ed_34c2_b4d8_f143b1af844a.Add(Role.Role_to_protcol(v_7e2e4050_27ab_52a0_b1bd_e4cfab4afda2));
            }
            _argv_c230e5af_6701_305e_be75_a718edc8d818.Add(_array_5fea3b35_c7ed_34c2_b4d8_f143b1af844a);
            Hub.Hub._gates.call_client(_client_uuid_c230e5af_6701_305e_be75_a718edc8d818, "scene_rsp_cb_import_role_rsp", _argv_c230e5af_6701_305e_be75_a718edc8d818);
        }

        public void err(Int32 errCode_c2a4463a_f4dd_322e_bbf5_24a0acf9d2ec){
            var _argv_c230e5af_6701_305e_be75_a718edc8d818 = new ArrayList();
            _argv_c230e5af_6701_305e_be75_a718edc8d818.Add(uuid_ce4937ed_df79_3445_b84b_8122980631f7);
            _argv_c230e5af_6701_305e_be75_a718edc8d818.Add(errCode_c2a4463a_f4dd_322e_bbf5_24a0acf9d2ec);
            Hub.Hub._gates.call_client(_client_uuid_c230e5af_6701_305e_be75_a718edc8d818, "scene_rsp_cb_import_role_err", _argv_c230e5af_6701_305e_be75_a718edc8d818);
        }

    }

    public class scene_grow_rsp : Common.Response {
        private string _client_uuid_672a87cb_5af2_3344_84fd_fe0130666cba;
        private UInt64 uuid_ff52c9ac_dca0_3f9a_a9e3_e843fbf593cb;
        public scene_grow_rsp(string client_uuid, UInt64 _uuid)
        {
            _client_uuid_672a87cb_5af2_3344_84fd_fe0130666cba = client_uuid;
            uuid_ff52c9ac_dca0_3f9a_a9e3_e843fbf593cb = _uuid;
        }

        public void rsp(List<Vegetable> vegetables_0b2db772_9803_37db_aa0b_906aa1c698dd){
            var _argv_672a87cb_5af2_3344_84fd_fe0130666cba = new ArrayList();
            _argv_672a87cb_5af2_3344_84fd_fe0130666cba.Add(uuid_ff52c9ac_dca0_3f9a_a9e3_e843fbf593cb);
            var _array_0b2db772_9803_37db_aa0b_906aa1c698dd = new ArrayList();
            foreach(var v_1b631c32_577d_53cf_91e7_c648319a3b24 in vegetables_0b2db772_9803_37db_aa0b_906aa1c698dd){
                _array_0b2db772_9803_37db_aa0b_906aa1c698dd.Add(Vegetable.Vegetable_to_protcol(v_1b631c32_577d_53cf_91e7_c648319a3b24));
            }
            _argv_672a87cb_5af2_3344_84fd_fe0130666cba.Add(_array_0b2db772_9803_37db_aa0b_906aa1c698dd);
            Hub.Hub._gates.call_client(_client_uuid_672a87cb_5af2_3344_84fd_fe0130666cba, "scene_rsp_cb_grow_rsp", _argv_672a87cb_5af2_3344_84fd_fe0130666cba);
        }

        public void err(Int32 errCode_c2a4463a_f4dd_322e_bbf5_24a0acf9d2ec){
            var _argv_672a87cb_5af2_3344_84fd_fe0130666cba = new ArrayList();
            _argv_672a87cb_5af2_3344_84fd_fe0130666cba.Add(uuid_ff52c9ac_dca0_3f9a_a9e3_e843fbf593cb);
            _argv_672a87cb_5af2_3344_84fd_fe0130666cba.Add(errCode_c2a4463a_f4dd_322e_bbf5_24a0acf9d2ec);
            Hub.Hub._gates.call_client(_client_uuid_672a87cb_5af2_3344_84fd_fe0130666cba, "scene_rsp_cb_grow_err", _argv_672a87cb_5af2_3344_84fd_fe0130666cba);
        }

    }

    public class scene_harvest_rsp : Common.Response {
        private string _client_uuid_a180627d_6e3c_3a7b_9a46_391f6553d65b;
        private UInt64 uuid_4c95fb59_53ba_3193_8a78_72c13c0ae260;
        public scene_harvest_rsp(string client_uuid, UInt64 _uuid)
        {
            _client_uuid_a180627d_6e3c_3a7b_9a46_391f6553d65b = client_uuid;
            uuid_4c95fb59_53ba_3193_8a78_72c13c0ae260 = _uuid;
        }

        public void rsp(){
            var _argv_a180627d_6e3c_3a7b_9a46_391f6553d65b = new ArrayList();
            _argv_a180627d_6e3c_3a7b_9a46_391f6553d65b.Add(uuid_4c95fb59_53ba_3193_8a78_72c13c0ae260);
            Hub.Hub._gates.call_client(_client_uuid_a180627d_6e3c_3a7b_9a46_391f6553d65b, "scene_rsp_cb_harvest_rsp", _argv_a180627d_6e3c_3a7b_9a46_391f6553d65b);
        }

        public void err(Int32 errCode_c2a4463a_f4dd_322e_bbf5_24a0acf9d2ec){
            var _argv_a180627d_6e3c_3a7b_9a46_391f6553d65b = new ArrayList();
            _argv_a180627d_6e3c_3a7b_9a46_391f6553d65b.Add(uuid_4c95fb59_53ba_3193_8a78_72c13c0ae260);
            _argv_a180627d_6e3c_3a7b_9a46_391f6553d65b.Add(errCode_c2a4463a_f4dd_322e_bbf5_24a0acf9d2ec);
            Hub.Hub._gates.call_client(_client_uuid_a180627d_6e3c_3a7b_9a46_391f6553d65b, "scene_rsp_cb_harvest_err", _argv_a180627d_6e3c_3a7b_9a46_391f6553d65b);
        }

    }

    public class scene_cut_down_rsp : Common.Response {
        private string _client_uuid_8cccd442_c704_3649_85b1_e458e0261e66;
        private UInt64 uuid_80272c48_d7c2_30f0_9450_74dbd2a537ce;
        public scene_cut_down_rsp(string client_uuid, UInt64 _uuid)
        {
            _client_uuid_8cccd442_c704_3649_85b1_e458e0261e66 = client_uuid;
            uuid_80272c48_d7c2_30f0_9450_74dbd2a537ce = _uuid;
        }

        public void rsp(){
            var _argv_8cccd442_c704_3649_85b1_e458e0261e66 = new ArrayList();
            _argv_8cccd442_c704_3649_85b1_e458e0261e66.Add(uuid_80272c48_d7c2_30f0_9450_74dbd2a537ce);
            Hub.Hub._gates.call_client(_client_uuid_8cccd442_c704_3649_85b1_e458e0261e66, "scene_rsp_cb_cut_down_rsp", _argv_8cccd442_c704_3649_85b1_e458e0261e66);
        }

        public void err(Int32 errCode_c2a4463a_f4dd_322e_bbf5_24a0acf9d2ec){
            var _argv_8cccd442_c704_3649_85b1_e458e0261e66 = new ArrayList();
            _argv_8cccd442_c704_3649_85b1_e458e0261e66.Add(uuid_80272c48_d7c2_30f0_9450_74dbd2a537ce);
            _argv_8cccd442_c704_3649_85b1_e458e0261e66.Add(errCode_c2a4463a_f4dd_322e_bbf5_24a0acf9d2ec);
            Hub.Hub._gates.call_client(_client_uuid_8cccd442_c704_3649_85b1_e458e0261e66, "scene_rsp_cb_cut_down_err", _argv_8cccd442_c704_3649_85b1_e458e0261e66);
        }

    }

    public class scene_steal_rsp : Common.Response {
        private string _client_uuid_211c2516_ca86_3274_9c27_3cd41662932a;
        private UInt64 uuid_23120493_1e9d_3ad3_a967_5065a02f62f9;
        public scene_steal_rsp(string client_uuid, UInt64 _uuid)
        {
            _client_uuid_211c2516_ca86_3274_9c27_3cd41662932a = client_uuid;
            uuid_23120493_1e9d_3ad3_a967_5065a02f62f9 = _uuid;
        }

        public void rsp(Vegetable vegetable_8c681d72_2f53_3eb5_a996_87b18f440696){
            var _argv_211c2516_ca86_3274_9c27_3cd41662932a = new ArrayList();
            _argv_211c2516_ca86_3274_9c27_3cd41662932a.Add(uuid_23120493_1e9d_3ad3_a967_5065a02f62f9);
            _argv_211c2516_ca86_3274_9c27_3cd41662932a.Add(Vegetable.Vegetable_to_protcol(vegetable_8c681d72_2f53_3eb5_a996_87b18f440696));
            Hub.Hub._gates.call_client(_client_uuid_211c2516_ca86_3274_9c27_3cd41662932a, "scene_rsp_cb_steal_rsp", _argv_211c2516_ca86_3274_9c27_3cd41662932a);
        }

        public void err(Int32 errCode_c2a4463a_f4dd_322e_bbf5_24a0acf9d2ec){
            var _argv_211c2516_ca86_3274_9c27_3cd41662932a = new ArrayList();
            _argv_211c2516_ca86_3274_9c27_3cd41662932a.Add(uuid_23120493_1e9d_3ad3_a967_5065a02f62f9);
            _argv_211c2516_ca86_3274_9c27_3cd41662932a.Add(errCode_c2a4463a_f4dd_322e_bbf5_24a0acf9d2ec);
            Hub.Hub._gates.call_client(_client_uuid_211c2516_ca86_3274_9c27_3cd41662932a, "scene_rsp_cb_steal_err", _argv_211c2516_ca86_3274_9c27_3cd41662932a);
        }

    }

    public class scene_sales_vegetable_rsp : Common.Response {
        private string _client_uuid_11b25d19_885a_3403_9235_abd0391f361e;
        private UInt64 uuid_fd54d56e_c946_3b8c_81c4_9b7760545f61;
        public scene_sales_vegetable_rsp(string client_uuid, UInt64 _uuid)
        {
            _client_uuid_11b25d19_885a_3403_9235_abd0391f361e = client_uuid;
            uuid_fd54d56e_c946_3b8c_81c4_9b7760545f61 = _uuid;
        }

        public void rsp(Int32 coin_bf39414a_d47b_3564_87aa_8965f9ca95df){
            var _argv_11b25d19_885a_3403_9235_abd0391f361e = new ArrayList();
            _argv_11b25d19_885a_3403_9235_abd0391f361e.Add(uuid_fd54d56e_c946_3b8c_81c4_9b7760545f61);
            _argv_11b25d19_885a_3403_9235_abd0391f361e.Add(coin_bf39414a_d47b_3564_87aa_8965f9ca95df);
            Hub.Hub._gates.call_client(_client_uuid_11b25d19_885a_3403_9235_abd0391f361e, "scene_rsp_cb_sales_vegetable_rsp", _argv_11b25d19_885a_3403_9235_abd0391f361e);
        }

        public void err(Int32 errCode_c2a4463a_f4dd_322e_bbf5_24a0acf9d2ec){
            var _argv_11b25d19_885a_3403_9235_abd0391f361e = new ArrayList();
            _argv_11b25d19_885a_3403_9235_abd0391f361e.Add(uuid_fd54d56e_c946_3b8c_81c4_9b7760545f61);
            _argv_11b25d19_885a_3403_9235_abd0391f361e.Add(errCode_c2a4463a_f4dd_322e_bbf5_24a0acf9d2ec);
            Hub.Hub._gates.call_client(_client_uuid_11b25d19_885a_3403_9235_abd0391f361e, "scene_rsp_cb_sales_vegetable_err", _argv_11b25d19_885a_3403_9235_abd0391f361e);
        }

    }

    public class scene_module : Common.IModule {
        public scene_module()
        {
            Hub.Hub._modules.add_mothed("scene_move", move);
            Hub.Hub._modules.add_mothed("scene_remove_building", remove_building);
            Hub.Hub._modules.add_mothed("scene_building", building);
            Hub.Hub._modules.add_mothed("scene_arable_land", arable_land);
            Hub.Hub._modules.add_mothed("scene_scene_change", scene_change);
            Hub.Hub._modules.add_mothed("scene_into_subscene", into_subscene);
            Hub.Hub._modules.add_mothed("scene_init_fence", init_fence);
            Hub.Hub._modules.add_mothed("scene_import_role", import_role);
            Hub.Hub._modules.add_mothed("scene_grow", grow);
            Hub.Hub._modules.add_mothed("scene_harvest", harvest);
            Hub.Hub._modules.add_mothed("scene_cut_down", cut_down);
            Hub.Hub._modules.add_mothed("scene_steal", steal);
            Hub.Hub._modules.add_mothed("scene_sales_vegetable", sales_vegetable);
            Hub.Hub._modules.add_mothed("scene_npc_into_scene", npc_into_scene);
            Hub.Hub._modules.add_mothed("scene_npc_into_subscene", npc_into_subscene);
            Hub.Hub._modules.add_mothed("scene_npc_move", npc_move);
        }

        public event Action<Pos, Direction> on_move;
        public void move(IList<MsgPack.MessagePackObject> inArray){
            var _start = Pos.protcol_to_Pos(((MsgPack.MessagePackObject)inArray[0]).AsDictionary());
            var _dir = (Direction)((MsgPack.MessagePackObject)inArray[1]).AsInt32();
            if (on_move != null){
                on_move(_start, _dir);
            }
        }

        public event Action<List<Building>, List<Cultivated>, bool> on_remove_building;
        public void remove_building(IList<MsgPack.MessagePackObject> inArray){
            var _buildings = new List<Building>();
            var _protocol_arraybuildings = ((MsgPack.MessagePackObject)inArray[0]).AsList();
            foreach (var v_4435b032_5f03_5920_bcc2_f3356d041cc1 in _protocol_arraybuildings){
                _buildings.Add(Building.protcol_to_Building(((MsgPack.MessagePackObject)v_4435b032_5f03_5920_bcc2_f3356d041cc1).AsDictionary()));
            }
            var _cultivateds = new List<Cultivated>();
            var _protocol_arraycultivateds = ((MsgPack.MessagePackObject)inArray[1]).AsList();
            foreach (var v_49c1caf0_a8a7_5ea3_bcc2_4d1826995f0d in _protocol_arraycultivateds){
                _cultivateds.Add(Cultivated.protcol_to_Cultivated(((MsgPack.MessagePackObject)v_49c1caf0_a8a7_5ea3_bcc2_4d1826995f0d).AsDictionary()));
            }
            var _rmfences = ((MsgPack.MessagePackObject)inArray[2]).AsBoolean();
            if (on_remove_building != null){
                on_remove_building(_buildings, _cultivateds, _rmfences);
            }
        }

        public event Action<List<Building>, Fence> on_building;
        public void building(IList<MsgPack.MessagePackObject> inArray){
            var _buildings = new List<Building>();
            var _protocol_arraybuildings = ((MsgPack.MessagePackObject)inArray[0]).AsList();
            foreach (var v_4435b032_5f03_5920_bcc2_f3356d041cc1 in _protocol_arraybuildings){
                _buildings.Add(Building.protcol_to_Building(((MsgPack.MessagePackObject)v_4435b032_5f03_5920_bcc2_f3356d041cc1).AsDictionary()));
            }
            var _fences = Fence.protcol_to_Fence(((MsgPack.MessagePackObject)inArray[1]).AsDictionary());
            if (on_building != null){
                on_building(_buildings, _fences);
            }
        }

        public event Action<List<Cultivated>> on_arable_land;
        public void arable_land(IList<MsgPack.MessagePackObject> inArray){
            var _cultivateds = new List<Cultivated>();
            var _protocol_arraycultivateds = ((MsgPack.MessagePackObject)inArray[0]).AsList();
            foreach (var v_49c1caf0_a8a7_5ea3_bcc2_4d1826995f0d in _protocol_arraycultivateds){
                _cultivateds.Add(Cultivated.protcol_to_Cultivated(((MsgPack.MessagePackObject)v_49c1caf0_a8a7_5ea3_bcc2_4d1826995f0d).AsDictionary()));
            }
            if (on_arable_land != null){
                on_arable_land(_cultivateds);
            }
        }

        public event Action<SceneChange> on_scene_change;
        public void scene_change(IList<MsgPack.MessagePackObject> inArray){
            var _change = SceneChange.protcol_to_SceneChange(((MsgPack.MessagePackObject)inArray[0]).AsDictionary());
            if (on_scene_change != null){
                on_scene_change(_change);
            }
        }

        public event Action<string> on_into_subscene;
        public void into_subscene(IList<MsgPack.MessagePackObject> inArray){
            var _subscene_name = ((MsgPack.MessagePackObject)inArray[0]).AsString();
            if (on_into_subscene != null){
                on_into_subscene(_subscene_name);
            }
        }

        public event Action<Fence, string> on_init_fence;
        public void init_fence(IList<MsgPack.MessagePackObject> inArray){
            var _fences = Fence.protcol_to_Fence(((MsgPack.MessagePackObject)inArray[0]).AsDictionary());
            var _pwd = ((MsgPack.MessagePackObject)inArray[1]).AsString();
            if (on_init_fence != null){
                on_init_fence(_fences, _pwd);
            }
        }

        public event Action<Role> on_import_role;
        public void import_role(IList<MsgPack.MessagePackObject> inArray){
            var _cb_uuid = ((MsgPack.MessagePackObject)inArray[0]).AsUInt64();
            var _role = Role.protcol_to_Role(((MsgPack.MessagePackObject)inArray[1]).AsDictionary());
            rsp = new scene_import_role_rsp(Hub.Hub._gates.current_client_uuid, _cb_uuid);
            if (on_import_role != null){
                on_import_role(_role);
            }
            rsp = null;
        }

        public event Action<Vegetable, Cultivated> on_grow;
        public void grow(IList<MsgPack.MessagePackObject> inArray){
            var _cb_uuid = ((MsgPack.MessagePackObject)inArray[0]).AsUInt64();
            var _vegetable = Vegetable.protcol_to_Vegetable(((MsgPack.MessagePackObject)inArray[1]).AsDictionary());
            var _cultivated = Cultivated.protcol_to_Cultivated(((MsgPack.MessagePackObject)inArray[2]).AsDictionary());
            rsp = new scene_grow_rsp(Hub.Hub._gates.current_client_uuid, _cb_uuid);
            if (on_grow != null){
                on_grow(_vegetable, _cultivated);
            }
            rsp = null;
        }

        public event Action<Vegetable> on_harvest;
        public void harvest(IList<MsgPack.MessagePackObject> inArray){
            var _cb_uuid = ((MsgPack.MessagePackObject)inArray[0]).AsUInt64();
            var _vegetable = Vegetable.protcol_to_Vegetable(((MsgPack.MessagePackObject)inArray[1]).AsDictionary());
            rsp = new scene_harvest_rsp(Hub.Hub._gates.current_client_uuid, _cb_uuid);
            if (on_harvest != null){
                on_harvest(_vegetable);
            }
            rsp = null;
        }

        public event Action<Vegetable> on_cut_down;
        public void cut_down(IList<MsgPack.MessagePackObject> inArray){
            var _cb_uuid = ((MsgPack.MessagePackObject)inArray[0]).AsUInt64();
            var _vegetable = Vegetable.protcol_to_Vegetable(((MsgPack.MessagePackObject)inArray[1]).AsDictionary());
            rsp = new scene_cut_down_rsp(Hub.Hub._gates.current_client_uuid, _cb_uuid);
            if (on_cut_down != null){
                on_cut_down(_vegetable);
            }
            rsp = null;
        }

        public event Action<Vegetable> on_steal;
        public void steal(IList<MsgPack.MessagePackObject> inArray){
            var _cb_uuid = ((MsgPack.MessagePackObject)inArray[0]).AsUInt64();
            var _vegetable = Vegetable.protcol_to_Vegetable(((MsgPack.MessagePackObject)inArray[1]).AsDictionary());
            rsp = new scene_steal_rsp(Hub.Hub._gates.current_client_uuid, _cb_uuid);
            if (on_steal != null){
                on_steal(_vegetable);
            }
            rsp = null;
        }

        public event Action<Vegetable, Int32> on_sales_vegetable;
        public void sales_vegetable(IList<MsgPack.MessagePackObject> inArray){
            var _cb_uuid = ((MsgPack.MessagePackObject)inArray[0]).AsUInt64();
            var _vegetable = Vegetable.protcol_to_Vegetable(((MsgPack.MessagePackObject)inArray[1]).AsDictionary());
            var _coin = ((MsgPack.MessagePackObject)inArray[2]).AsInt32();
            rsp = new scene_sales_vegetable_rsp(Hub.Hub._gates.current_client_uuid, _cb_uuid);
            if (on_sales_vegetable != null){
                on_sales_vegetable(_vegetable, _coin);
            }
            rsp = null;
        }

        public event Action<string, NPCInformation> on_npc_into_scene;
        public void npc_into_scene(IList<MsgPack.MessagePackObject> inArray){
            var _scene_guid = ((MsgPack.MessagePackObject)inArray[0]).AsString();
            var _info = NPCInformation.protcol_to_NPCInformation(((MsgPack.MessagePackObject)inArray[1]).AsDictionary());
            if (on_npc_into_scene != null){
                on_npc_into_scene(_scene_guid, _info);
            }
        }

        public event Action<string, string, string> on_npc_into_subscene;
        public void npc_into_subscene(IList<MsgPack.MessagePackObject> inArray){
            var _scene_guid = ((MsgPack.MessagePackObject)inArray[0]).AsString();
            var _npcUUID = ((MsgPack.MessagePackObject)inArray[1]).AsString();
            var _subscene_name = ((MsgPack.MessagePackObject)inArray[2]).AsString();
            if (on_npc_into_subscene != null){
                on_npc_into_subscene(_scene_guid, _npcUUID, _subscene_name);
            }
        }

        public event Action<string, string, string, Pos, Direction> on_npc_move;
        public void npc_move(IList<MsgPack.MessagePackObject> inArray){
            var _npcUUID = ((MsgPack.MessagePackObject)inArray[0]).AsString();
            var _scene_guid = ((MsgPack.MessagePackObject)inArray[1]).AsString();
            var _subscene_name = ((MsgPack.MessagePackObject)inArray[2]).AsString();
            var _start = Pos.protcol_to_Pos(((MsgPack.MessagePackObject)inArray[3]).AsDictionary());
            var _dir = (Direction)((MsgPack.MessagePackObject)inArray[4]).AsInt32();
            if (on_npc_move != null){
                on_npc_move(_npcUUID, _scene_guid, _subscene_name, _start, _dir);
            }
        }

    }

}
