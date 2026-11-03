using Abelkhan;
using MongoDB.Bson;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Server
{
    class login_msg_handle
    {
        private login_module login_Module = new login_module();

        public login_msg_handle()
        {
            login_Module.on_player_login_no_token += Login_Module_on_player_login_no_token;
            login_Module.on_combined_user_info += Login_Module_on_combined_user_info;
        }

        private async void Login_Module_on_combined_user_info(string jwt)
        {
            Log.Log.trace("on_combined_user_info begin!");
            var rsp = login_Module.rsp as login_combined_user_info_rsp;

            try
            {
                Log.Log.trace("on_combined_user_info jwt:{0}", jwt);

                /*const string ADMIN_WEB_API = "AS890Snujik29n389jkbjknjk@1";
                var response = await HttpClientWrapper.GetRspAsync(
                    $"https://preview3.xiaoshuoai.com/dzmm/combined-user-info?jwtoken={jwt}",
                    new List<KeyValuePair<string, string>>{
                        new KeyValuePair<string, string>("admin", ADMIN_WEB_API),
                        new KeyValuePair<string, string>("X-API-KEY", ADMIN_WEB_API),
                    });

                response.EnsureSuccessStatusCode();
                string content = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<dynamic>(content);
                string name = data["supabaseUser"]["full_name"];
                if (name == null)
                {
                    name = "testUser";
                }
                string self_id = data["supabaseUser"]["id"];

                rsp.rsp(self_id, name);*/

                rsp.rsp(Guid.NewGuid().ToString(), "testUserLocal");
            }
            catch (System.Net.Http.HttpRequestException) {
                rsp.rsp("6y7yuihkhoy798gygik", "testUserLocal");
            }
            catch (System.Exception ex)
            {
                Log.Log.err($"{ex}");
                rsp.err((int)em_error.db_error);
            }
        }

        private async void Login_Module_on_player_login_no_token(string account)
        {
            var uuid = Hub.Hub._gates.current_client_uuid;
            var rsp = login_Module.rsp as login_player_login_no_token_rsp;

            Log.Log.trace("on_player_login_no_author begin! player account:{0}, uuid:{1}", account, uuid);

            var token = Guid.NewGuid().ToString();
            try
            {
                await Server.RedisHandle.Lock(RedisHelp.BuildLoadSceneLockKey(), token, 2000);

                var _scene_info_list = await Server.RedisHandle.GetData<List<scene_hub_info>>(RedisHelp.BuildSceneInfoListKey());
                if (_scene_info_list == null)
                {
                    _scene_info_list = new();
                }
                foreach(var info in _scene_info_list)
                {
                    if (!Server.HubProxyMap.ContainsKey(info.scene_hub_id))
                    {
                        info.scene_hub_id = Hub.Hub.name;
                    }
                }

                //var guid = await getGuid(account);
                rsp.rsp(account, _scene_info_list);

                await Server.RedisHandle.SetData(RedisHelp.BuildSceneInfoListKey(), _scene_info_list);
            }
            catch (System.Exception ex)
            {
                Log.Log.err($"{ex}");
                rsp.err((int)em_error.db_error);
            }
            finally
            {
                await Server.RedisHandle.UnLock(RedisHelp.BuildLoadSceneLockKey(), token);
            }
        }
    }
}
