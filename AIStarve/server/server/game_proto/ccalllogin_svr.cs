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
    public class login_player_login_no_token_rsp : Common.Response {
        private string _client_uuid_3e2e7610_1bd3_3053_a6cb_55c17d6b8ebf;
        private UInt64 uuid_b295d7ce_3d9c_398f_8f6a_ee7a40f01d25;
        public login_player_login_no_token_rsp(string client_uuid, UInt64 _uuid)
        {
            _client_uuid_3e2e7610_1bd3_3053_a6cb_55c17d6b8ebf = client_uuid;
            uuid_b295d7ce_3d9c_398f_8f6a_ee7a40f01d25 = _uuid;
        }

        public void rsp(string userGuid_2b0ccc07_1107_3253_b58d_2ae97933238f, List<scene_hub_info> info_391fd3d4_2d55_3f5e_9223_7f450a814a15){
            var _argv_3e2e7610_1bd3_3053_a6cb_55c17d6b8ebf = new ArrayList();
            _argv_3e2e7610_1bd3_3053_a6cb_55c17d6b8ebf.Add(uuid_b295d7ce_3d9c_398f_8f6a_ee7a40f01d25);
            _argv_3e2e7610_1bd3_3053_a6cb_55c17d6b8ebf.Add(userGuid_2b0ccc07_1107_3253_b58d_2ae97933238f);
            var _array_391fd3d4_2d55_3f5e_9223_7f450a814a15 = new ArrayList();
            foreach(var v_d856b000_56e0_5f62_a6f3_e6a0c7859745 in info_391fd3d4_2d55_3f5e_9223_7f450a814a15){
                _array_391fd3d4_2d55_3f5e_9223_7f450a814a15.Add(scene_hub_info.scene_hub_info_to_protcol(v_d856b000_56e0_5f62_a6f3_e6a0c7859745));
            }
            _argv_3e2e7610_1bd3_3053_a6cb_55c17d6b8ebf.Add(_array_391fd3d4_2d55_3f5e_9223_7f450a814a15);
            Hub.Hub._gates.call_client(_client_uuid_3e2e7610_1bd3_3053_a6cb_55c17d6b8ebf, "login_rsp_cb_player_login_no_token_rsp", _argv_3e2e7610_1bd3_3053_a6cb_55c17d6b8ebf);
        }

        public void err(Int32 err_ad2710a2_3dd2_3a8f_a4c8_a7ebbe1df696){
            var _argv_3e2e7610_1bd3_3053_a6cb_55c17d6b8ebf = new ArrayList();
            _argv_3e2e7610_1bd3_3053_a6cb_55c17d6b8ebf.Add(uuid_b295d7ce_3d9c_398f_8f6a_ee7a40f01d25);
            _argv_3e2e7610_1bd3_3053_a6cb_55c17d6b8ebf.Add(err_ad2710a2_3dd2_3a8f_a4c8_a7ebbe1df696);
            Hub.Hub._gates.call_client(_client_uuid_3e2e7610_1bd3_3053_a6cb_55c17d6b8ebf, "login_rsp_cb_player_login_no_token_err", _argv_3e2e7610_1bd3_3053_a6cb_55c17d6b8ebf);
        }

    }

    public class login_combined_user_info_rsp : Common.Response {
        private string _client_uuid_91d7392f_4967_314b_bc40_481f923020f5;
        private UInt64 uuid_c2b057da_033d_3080_8ec1_73147c716a7d;
        public login_combined_user_info_rsp(string client_uuid, UInt64 _uuid)
        {
            _client_uuid_91d7392f_4967_314b_bc40_481f923020f5 = client_uuid;
            uuid_c2b057da_033d_3080_8ec1_73147c716a7d = _uuid;
        }

        public void rsp(string id_e4a11b9c_a474_3d25_a2f1_d3f536a99026, string name_c6437ef1_5b86_3a4e_a071_c2d4ad414e65){
            var _argv_91d7392f_4967_314b_bc40_481f923020f5 = new ArrayList();
            _argv_91d7392f_4967_314b_bc40_481f923020f5.Add(uuid_c2b057da_033d_3080_8ec1_73147c716a7d);
            _argv_91d7392f_4967_314b_bc40_481f923020f5.Add(id_e4a11b9c_a474_3d25_a2f1_d3f536a99026);
            _argv_91d7392f_4967_314b_bc40_481f923020f5.Add(name_c6437ef1_5b86_3a4e_a071_c2d4ad414e65);
            Hub.Hub._gates.call_client(_client_uuid_91d7392f_4967_314b_bc40_481f923020f5, "login_rsp_cb_combined_user_info_rsp", _argv_91d7392f_4967_314b_bc40_481f923020f5);
        }

        public void err(Int32 err_ad2710a2_3dd2_3a8f_a4c8_a7ebbe1df696){
            var _argv_91d7392f_4967_314b_bc40_481f923020f5 = new ArrayList();
            _argv_91d7392f_4967_314b_bc40_481f923020f5.Add(uuid_c2b057da_033d_3080_8ec1_73147c716a7d);
            _argv_91d7392f_4967_314b_bc40_481f923020f5.Add(err_ad2710a2_3dd2_3a8f_a4c8_a7ebbe1df696);
            Hub.Hub._gates.call_client(_client_uuid_91d7392f_4967_314b_bc40_481f923020f5, "login_rsp_cb_combined_user_info_err", _argv_91d7392f_4967_314b_bc40_481f923020f5);
        }

    }

    public class login_module : Common.IModule {
        public login_module()
        {
            Hub.Hub._modules.add_mothed("login_player_login_no_token", player_login_no_token);
            Hub.Hub._modules.add_mothed("login_combined_user_info", combined_user_info);
        }

        public event Action<string> on_player_login_no_token;
        public void player_login_no_token(IList<MsgPack.MessagePackObject> inArray){
            var _cb_uuid = ((MsgPack.MessagePackObject)inArray[0]).AsUInt64();
            var _account = ((MsgPack.MessagePackObject)inArray[1]).AsString();
            rsp = new login_player_login_no_token_rsp(Hub.Hub._gates.current_client_uuid, _cb_uuid);
            if (on_player_login_no_token != null){
                on_player_login_no_token(_account);
            }
            rsp = null;
        }

        public event Action<string> on_combined_user_info;
        public void combined_user_info(IList<MsgPack.MessagePackObject> inArray){
            var _cb_uuid = ((MsgPack.MessagePackObject)inArray[0]).AsUInt64();
            var _ticket = ((MsgPack.MessagePackObject)inArray[1]).AsString();
            rsp = new login_combined_user_info_rsp(Hub.Hub._gates.current_client_uuid, _cb_uuid);
            if (on_combined_user_info != null){
                on_combined_user_info(_ticket);
            }
            rsp = null;
        }

    }

}
