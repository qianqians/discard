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
/*this cb code is codegen by abelkhan for c#*/
    public class scene_client_rsp_cb : Common.IModule {
        public scene_client_rsp_cb() 
        {
        }

    }

    public class scene_client_clientproxy {
        public string client_uuid_e2b3f311_3d11_3958_8f94_f53379eb568e;
        private Int32 uuid_e2b3f311_3d11_3958_8f94_f53379eb568e = (Int32)RandomUUID.random();

        public scene_client_rsp_cb rsp_cb_scene_client_handle;

        public scene_client_clientproxy(scene_client_rsp_cb rsp_cb_scene_client_handle_)
        {
            rsp_cb_scene_client_handle = rsp_cb_scene_client_handle_;
        }

        public void import_dzmm_role_card(List<string> role_card){
            var _argv_8d8b6137_435c_3e28_8678_e4d374730bfd = new ArrayList();
            var _array_4703728c_b992_3b3e_b587_6f80b9c099eb = new ArrayList();
            foreach(var v_a5140999_3ec9_57ee_b859_2ba59ceea4ca in role_card){
                _array_4703728c_b992_3b3e_b587_6f80b9c099eb.Add(v_a5140999_3ec9_57ee_b859_2ba59ceea4ca);
            }
            _argv_8d8b6137_435c_3e28_8678_e4d374730bfd.Add(_array_4703728c_b992_3b3e_b587_6f80b9c099eb);
            Hub.Hub._gates.call_client(client_uuid_e2b3f311_3d11_3958_8f94_f53379eb568e, "scene_client_import_dzmm_role_card", _argv_8d8b6137_435c_3e28_8678_e4d374730bfd);
        }

    }

    public class scene_client_multicast {
        public List<string> client_uuids_e2b3f311_3d11_3958_8f94_f53379eb568e;
        public scene_client_rsp_cb rsp_cb_scene_client_handle;

        public scene_client_multicast(scene_client_rsp_cb rsp_cb_scene_client_handle_)
        {
            rsp_cb_scene_client_handle = rsp_cb_scene_client_handle_;
        }

        public void remove_scene_info(List<Building> buildings, List<Cultivated> cultivateds, List<Vegetable> vegetables, Fence fences){
            var _argv_636b2a7a_df6c_3375_b7c3_981255ddfc2d = new ArrayList();
            var _array_98ba47f5_b9b8_3536_9493_f5e2255b10a9 = new ArrayList();
            foreach(var v_08f36ecb_8228_5356_bb8a_753f8de7b7e3 in buildings){
                _array_98ba47f5_b9b8_3536_9493_f5e2255b10a9.Add(Building.Building_to_protcol(v_08f36ecb_8228_5356_bb8a_753f8de7b7e3));
            }
            _argv_636b2a7a_df6c_3375_b7c3_981255ddfc2d.Add(_array_98ba47f5_b9b8_3536_9493_f5e2255b10a9);
            var _array_ca7f17ea_1480_30b1_a928_770254a9a812 = new ArrayList();
            foreach(var v_c6891ca4_7c79_57d2_a451_0494fcb49a22 in cultivateds){
                _array_ca7f17ea_1480_30b1_a928_770254a9a812.Add(Cultivated.Cultivated_to_protcol(v_c6891ca4_7c79_57d2_a451_0494fcb49a22));
            }
            _argv_636b2a7a_df6c_3375_b7c3_981255ddfc2d.Add(_array_ca7f17ea_1480_30b1_a928_770254a9a812);
            var _array_0b2db772_9803_37db_aa0b_906aa1c698dd = new ArrayList();
            foreach(var v_ba50d5ba_ebdb_582e_80b7_5d9c1177c91f in vegetables){
                _array_0b2db772_9803_37db_aa0b_906aa1c698dd.Add(Vegetable.Vegetable_to_protcol(v_ba50d5ba_ebdb_582e_80b7_5d9c1177c91f));
            }
            _argv_636b2a7a_df6c_3375_b7c3_981255ddfc2d.Add(_array_0b2db772_9803_37db_aa0b_906aa1c698dd);
            _argv_636b2a7a_df6c_3375_b7c3_981255ddfc2d.Add(Fence.Fence_to_protcol(fences));
            Hub.Hub._gates.call_group_client(client_uuids_e2b3f311_3d11_3958_8f94_f53379eb568e, "scene_client_remove_scene_info", _argv_636b2a7a_df6c_3375_b7c3_981255ddfc2d);
        }

        public void building_scene_info(List<Building> buildings, Fence fences){
            var _argv_35b1cb39_ec39_3ac9_80c5_56fa5338db62 = new ArrayList();
            var _array_98ba47f5_b9b8_3536_9493_f5e2255b10a9 = new ArrayList();
            foreach(var v_08f36ecb_8228_5356_bb8a_753f8de7b7e3 in buildings){
                _array_98ba47f5_b9b8_3536_9493_f5e2255b10a9.Add(Building.Building_to_protcol(v_08f36ecb_8228_5356_bb8a_753f8de7b7e3));
            }
            _argv_35b1cb39_ec39_3ac9_80c5_56fa5338db62.Add(_array_98ba47f5_b9b8_3536_9493_f5e2255b10a9);
            _argv_35b1cb39_ec39_3ac9_80c5_56fa5338db62.Add(Fence.Fence_to_protcol(fences));
            Hub.Hub._gates.call_group_client(client_uuids_e2b3f311_3d11_3958_8f94_f53379eb568e, "scene_client_building_scene_info", _argv_35b1cb39_ec39_3ac9_80c5_56fa5338db62);
        }

        public void arable_scene_info(List<Cultivated> cultivateds, List<Vegetable> vegetables){
            var _argv_c4c39bf5_85bc_3b17_b657_41dc0c595928 = new ArrayList();
            var _array_ca7f17ea_1480_30b1_a928_770254a9a812 = new ArrayList();
            foreach(var v_c6891ca4_7c79_57d2_a451_0494fcb49a22 in cultivateds){
                _array_ca7f17ea_1480_30b1_a928_770254a9a812.Add(Cultivated.Cultivated_to_protcol(v_c6891ca4_7c79_57d2_a451_0494fcb49a22));
            }
            _argv_c4c39bf5_85bc_3b17_b657_41dc0c595928.Add(_array_ca7f17ea_1480_30b1_a928_770254a9a812);
            var _array_0b2db772_9803_37db_aa0b_906aa1c698dd = new ArrayList();
            foreach(var v_ba50d5ba_ebdb_582e_80b7_5d9c1177c91f in vegetables){
                _array_0b2db772_9803_37db_aa0b_906aa1c698dd.Add(Vegetable.Vegetable_to_protcol(v_ba50d5ba_ebdb_582e_80b7_5d9c1177c91f));
            }
            _argv_c4c39bf5_85bc_3b17_b657_41dc0c595928.Add(_array_0b2db772_9803_37db_aa0b_906aa1c698dd);
            Hub.Hub._gates.call_group_client(client_uuids_e2b3f311_3d11_3958_8f94_f53379eb568e, "scene_client_arable_scene_info", _argv_c4c39bf5_85bc_3b17_b657_41dc0c595928);
        }

        public void scene_change(SceneChange change){
            var _argv_cca84eac_0cfa_36a4_9e43_af556f6a3c90 = new ArrayList();
            _argv_cca84eac_0cfa_36a4_9e43_af556f6a3c90.Add(SceneChange.SceneChange_to_protcol(change));
            Hub.Hub._gates.call_group_client(client_uuids_e2b3f311_3d11_3958_8f94_f53379eb568e, "scene_client_scene_change", _argv_cca84eac_0cfa_36a4_9e43_af556f6a3c90);
        }

        public void scene_info(SceneInfo info){
            var _argv_eaa1df1b_59b5_3e8d_8e20_b592765a38a8 = new ArrayList();
            _argv_eaa1df1b_59b5_3e8d_8e20_b592765a38a8.Add(SceneInfo.SceneInfo_to_protcol(info));
            Hub.Hub._gates.call_group_client(client_uuids_e2b3f311_3d11_3958_8f94_f53379eb568e, "scene_client_scene_info", _argv_eaa1df1b_59b5_3e8d_8e20_b592765a38a8);
        }

        public void move(List<UserMoveInfo> info){
            var _argv_33efb72e_9227_32af_a058_169be114a277 = new ArrayList();
            var _array_391fd3d4_2d55_3f5e_9223_7f450a814a15 = new ArrayList();
            foreach(var v_0c15545d_d42a_5fe0_bed7_a9496851e88b in info){
                _array_391fd3d4_2d55_3f5e_9223_7f450a814a15.Add(UserMoveInfo.UserMoveInfo_to_protcol(v_0c15545d_d42a_5fe0_bed7_a9496851e88b));
            }
            _argv_33efb72e_9227_32af_a058_169be114a277.Add(_array_391fd3d4_2d55_3f5e_9223_7f450a814a15);
            Hub.Hub._gates.call_group_client(client_uuids_e2b3f311_3d11_3958_8f94_f53379eb568e, "scene_client_move", _argv_33efb72e_9227_32af_a058_169be114a277);
        }

        public void player_leave_scene(string userGuid){
            var _argv_6448d6c0_e269_30cb_8d37_5d709613d7a7 = new ArrayList();
            _argv_6448d6c0_e269_30cb_8d37_5d709613d7a7.Add(userGuid);
            Hub.Hub._gates.call_group_client(client_uuids_e2b3f311_3d11_3958_8f94_f53379eb568e, "scene_client_player_leave_scene", _argv_6448d6c0_e269_30cb_8d37_5d709613d7a7);
        }

        public void time_info(TimeInfo info){
            var _argv_dd6aac94_8175_335e_b366_226615bc0049 = new ArrayList();
            _argv_dd6aac94_8175_335e_b366_226615bc0049.Add(TimeInfo.TimeInfo_to_protcol(info));
            Hub.Hub._gates.call_group_client(client_uuids_e2b3f311_3d11_3958_8f94_f53379eb568e, "scene_client_time_info", _argv_dd6aac94_8175_335e_b366_226615bc0049);
        }

    }

    public class scene_client_broadcast {
        public scene_client_rsp_cb rsp_cb_scene_client_handle;

        public scene_client_broadcast(scene_client_rsp_cb rsp_cb_scene_client_handle_)
        {
            rsp_cb_scene_client_handle = rsp_cb_scene_client_handle_;
        }

    }

    public class scene_client_caller {
        public static scene_client_rsp_cb rsp_cb_scene_client_handle = null;
        private ThreadLocal<scene_client_clientproxy> _clientproxy;
        private ThreadLocal<scene_client_multicast> _multicast;
        private scene_client_broadcast _broadcast;

        public scene_client_caller() 
        {
            if (rsp_cb_scene_client_handle == null)
            {
                rsp_cb_scene_client_handle = new scene_client_rsp_cb();
            }

            _clientproxy = new ThreadLocal<scene_client_clientproxy>();
            _multicast = new ThreadLocal<scene_client_multicast>();
            _broadcast = new scene_client_broadcast(rsp_cb_scene_client_handle);
        }

        public scene_client_clientproxy get_client(string client_uuid) {
            if (_clientproxy.Value == null)
{
                _clientproxy.Value = new scene_client_clientproxy(rsp_cb_scene_client_handle);
            }
            _clientproxy.Value.client_uuid_e2b3f311_3d11_3958_8f94_f53379eb568e = client_uuid;
            return _clientproxy.Value;
        }

        public scene_client_multicast get_multicast(List<string> client_uuids) {
            if (_multicast.Value == null)
{
                _multicast.Value = new scene_client_multicast(rsp_cb_scene_client_handle);
            }
            _multicast.Value.client_uuids_e2b3f311_3d11_3958_8f94_f53379eb568e = client_uuids;
            return _multicast.Value;
        }

        public scene_client_broadcast get_broadcast() {
            return _broadcast;
        }
    }


}
