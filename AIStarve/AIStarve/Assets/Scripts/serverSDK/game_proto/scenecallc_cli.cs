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
    public class scene_client_module : Common.IModule {
        public Client.Client _client_handle;
        public scene_client_module(Client.Client client_handle_) 
        {
            _client_handle = client_handle_;
            _client_handle.modulemanager.add_mothed("scene_client_import_dzmm_role_card", import_dzmm_role_card);
            _client_handle.modulemanager.add_mothed("scene_client_remove_scene_info", remove_scene_info);
            _client_handle.modulemanager.add_mothed("scene_client_building_scene_info", building_scene_info);
            _client_handle.modulemanager.add_mothed("scene_client_arable_scene_info", arable_scene_info);
            _client_handle.modulemanager.add_mothed("scene_client_scene_change", scene_change);
            _client_handle.modulemanager.add_mothed("scene_client_scene_info", scene_info);
            _client_handle.modulemanager.add_mothed("scene_client_move", move);
            _client_handle.modulemanager.add_mothed("scene_client_player_leave_scene", player_leave_scene);
            _client_handle.modulemanager.add_mothed("scene_client_time_info", time_info);
        }

        public event Action<List<string>> on_import_dzmm_role_card;
        public void import_dzmm_role_card(IList<MsgPack.MessagePackObject> inArray){
            var _role_card = new List<string>();
            var _protocol_arrayrole_card = ((MsgPack.MessagePackObject)inArray[0]).AsList();
            foreach (var v_89ed315e_e460_5c3a_b4e4_ccd62cf2e468 in _protocol_arrayrole_card){
                _role_card.Add(((MsgPack.MessagePackObject)v_89ed315e_e460_5c3a_b4e4_ccd62cf2e468).AsString());
            }
            if (on_import_dzmm_role_card != null){
                on_import_dzmm_role_card(_role_card);
            }
        }

        public event Action<List<Building>, List<Cultivated>, List<Vegetable>, Fence> on_remove_scene_info;
        public void remove_scene_info(IList<MsgPack.MessagePackObject> inArray){
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
            var _vegetables = new List<Vegetable>();
            var _protocol_arrayvegetables = ((MsgPack.MessagePackObject)inArray[2]).AsList();
            foreach (var v_1b631c32_577d_53cf_91e7_c648319a3b24 in _protocol_arrayvegetables){
                _vegetables.Add(Vegetable.protcol_to_Vegetable(((MsgPack.MessagePackObject)v_1b631c32_577d_53cf_91e7_c648319a3b24).AsDictionary()));
            }
            var _fences = Fence.protcol_to_Fence(((MsgPack.MessagePackObject)inArray[3]).AsDictionary());
            if (on_remove_scene_info != null){
                on_remove_scene_info(_buildings, _cultivateds, _vegetables, _fences);
            }
        }

        public event Action<List<Building>, Fence> on_building_scene_info;
        public void building_scene_info(IList<MsgPack.MessagePackObject> inArray){
            var _buildings = new List<Building>();
            var _protocol_arraybuildings = ((MsgPack.MessagePackObject)inArray[0]).AsList();
            foreach (var v_4435b032_5f03_5920_bcc2_f3356d041cc1 in _protocol_arraybuildings){
                _buildings.Add(Building.protcol_to_Building(((MsgPack.MessagePackObject)v_4435b032_5f03_5920_bcc2_f3356d041cc1).AsDictionary()));
            }
            var _fences = Fence.protcol_to_Fence(((MsgPack.MessagePackObject)inArray[1]).AsDictionary());
            if (on_building_scene_info != null){
                on_building_scene_info(_buildings, _fences);
            }
        }

        public event Action<List<Cultivated>, List<Vegetable>> on_arable_scene_info;
        public void arable_scene_info(IList<MsgPack.MessagePackObject> inArray){
            var _cultivateds = new List<Cultivated>();
            var _protocol_arraycultivateds = ((MsgPack.MessagePackObject)inArray[0]).AsList();
            foreach (var v_49c1caf0_a8a7_5ea3_bcc2_4d1826995f0d in _protocol_arraycultivateds){
                _cultivateds.Add(Cultivated.protcol_to_Cultivated(((MsgPack.MessagePackObject)v_49c1caf0_a8a7_5ea3_bcc2_4d1826995f0d).AsDictionary()));
            }
            var _vegetables = new List<Vegetable>();
            var _protocol_arrayvegetables = ((MsgPack.MessagePackObject)inArray[1]).AsList();
            foreach (var v_1b631c32_577d_53cf_91e7_c648319a3b24 in _protocol_arrayvegetables){
                _vegetables.Add(Vegetable.protcol_to_Vegetable(((MsgPack.MessagePackObject)v_1b631c32_577d_53cf_91e7_c648319a3b24).AsDictionary()));
            }
            if (on_arable_scene_info != null){
                on_arable_scene_info(_cultivateds, _vegetables);
            }
        }

        public event Action<SceneChange> on_scene_change;
        public void scene_change(IList<MsgPack.MessagePackObject> inArray){
            var _change = SceneChange.protcol_to_SceneChange(((MsgPack.MessagePackObject)inArray[0]).AsDictionary());
            if (on_scene_change != null){
                on_scene_change(_change);
            }
        }

        public event Action<SceneInfo> on_scene_info;
        public void scene_info(IList<MsgPack.MessagePackObject> inArray){
            var _info = SceneInfo.protcol_to_SceneInfo(((MsgPack.MessagePackObject)inArray[0]).AsDictionary());
            if (on_scene_info != null){
                on_scene_info(_info);
            }
        }

        public event Action<List<UserMoveInfo>> on_move;
        public void move(IList<MsgPack.MessagePackObject> inArray){
            var _info = new List<UserMoveInfo>();
            var _protocol_arrayinfo = ((MsgPack.MessagePackObject)inArray[0]).AsList();
            foreach (var v_d856b000_56e0_5f62_a6f3_e6a0c7859745 in _protocol_arrayinfo){
                _info.Add(UserMoveInfo.protcol_to_UserMoveInfo(((MsgPack.MessagePackObject)v_d856b000_56e0_5f62_a6f3_e6a0c7859745).AsDictionary()));
            }
            if (on_move != null){
                on_move(_info);
            }
        }

        public event Action<string> on_player_leave_scene;
        public void player_leave_scene(IList<MsgPack.MessagePackObject> inArray){
            var _userGuid = ((MsgPack.MessagePackObject)inArray[0]).AsString();
            if (on_player_leave_scene != null){
                on_player_leave_scene(_userGuid);
            }
        }

        public event Action<TimeInfo> on_time_info;
        public void time_info(IList<MsgPack.MessagePackObject> inArray){
            var _info = TimeInfo.protcol_to_TimeInfo(((MsgPack.MessagePackObject)inArray[0]).AsDictionary());
            if (on_time_info != null){
                on_time_info(_info);
            }
        }

    }

}
