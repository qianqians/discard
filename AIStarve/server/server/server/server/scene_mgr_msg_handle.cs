using System;
using System.Collections.Generic;
using Abelkhan;
using Newtonsoft.Json.Linq;

namespace Server
{
    class scene_mgr_msg_handle
    {
        private scene_mgr_module scene_mgr_Module = new scene_mgr_module();

        public scene_mgr_msg_handle()
        {
            scene_mgr_Module.on_get_scene_list += Scene_mgr_Module_on_get_scene_list;
            scene_mgr_Module.on_create_scene += Scene_mgr_Module_on_create_scene;
            scene_mgr_Module.on_player_level_scene += Scene_mgr_Module_on_player_level_scene;
            scene_mgr_Module.on_player_into_scene += Scene_mgr_Module_on_player_into_scene;
        }

        private async void Scene_mgr_Module_on_player_into_scene(string scene_guid, string scene_name, string userGuid, string userName, string userTableID)
        {
            var uuid = Hub.Hub._gates.current_client_uuid;
            var rsp = scene_mgr_Module.rsp as scene_mgr_player_into_scene_rsp;

            Log.Log.trace("on_player_into_scene begin! uuid:{0} scene_guid:{1}", uuid, scene_guid);
            try
            {
                var scene = await Server.SceneMgr.PlayerIntoScene(scene_guid, userGuid, userName, userTableID, uuid);
                if (scene != null)
                {
                    var info = scene.SceneInfo(userGuid);
                    rsp.rsp(info);
                }
                else
                {
                    var token = Guid.NewGuid().ToString();
                    try
                    {
                        await Server.RedisHandle.Lock(RedisHelp.BuildLoadSceneLockKey(), token, 2000);
                        var _scene_info_list = await Server.RedisHandle.GetData<List<scene_hub_info>>(RedisHelp.BuildSceneInfoListKey());
                        if (_scene_info_list == null)
                        {
                            _scene_info_list = new();
                        }

                        Log.Log.trace("on_player_into_scene begin! uuid:{0} scene_guid:{1} rsp.rsp!", uuid, scene_guid);
                        scene = await Server.SceneMgr.LoadScene(scene_name, scene_guid);
                        Log.Log.trace("on_player_into_scene begin! scene_name:{0}, scene_guid:{1} LoadScenec done!", scene_name, scene_guid);
                        scene = await Server.SceneMgr.PlayerIntoScene(scene_guid, userGuid, userName, userTableID, uuid);
                        Log.Log.trace("on_player_into_scene begin! userName:{0}, scene_guid:{1} PlayerIntoScene done!", userName, scene_guid);
                        
                        bool isNew = true;
                        foreach (var _info in _scene_info_list)
                        {
                            if (_info.scene_guid == scene_guid)
                            {
                                _info.scene_hub_id = Hub.Hub.name;
                                isNew = false;
                            }
                        }
                        if (isNew)
                        {
                            _scene_info_list.Add(new scene_hub_info
                            {
                                scene_name = scene_name,
                                scene_guid = scene.sceneID,
                                scene_hub_id = Hub.Hub.name,
                            });
                        }

                        var info = scene.SceneInfo(userGuid);
                        rsp.rsp(info);
                        Log.Log.trace("on_player_into_scene begin! uuid:{0} scene_guid:{1} rsp.rsp:{2} end!", uuid, scene_guid, info);

                        Log.Log.trace("on_player_into_scene begin! uuid:{0} scene_guid:{1} RedisHandle.SetData!", uuid, scene_guid);
                        await Server.RedisHandle.SetData(RedisHelp.BuildSceneInfoListKey(), _scene_info_list);
                    }
                    catch (System.Exception ex)
                    {
                        Log.Log.err($"{ex}");
                        rsp.err((int)em_error.db_error, new scene_hub_info());
                    }
                    finally
                    {
                        await Server.RedisHandle.UnLock(RedisHelp.BuildLoadSceneLockKey(), token);
                    }
                }
            }
            catch (System.Exception ex)
            {
                Log.Log.err($"{ex}");
                rsp.err((int)em_error.db_error, new scene_hub_info());
            }
        }

        private void Scene_mgr_Module_on_player_level_scene(string scene_guid)
        {
            var uuid = Hub.Hub._gates.current_client_uuid;
            var rsp = scene_mgr_Module.rsp as scene_mgr_player_level_scene_rsp;

            Log.Log.trace("on_player_level_scene begin! uuid:{0} scene_guid:{1}", uuid, scene_guid);
            try
            {
                var scene = Server.SceneMgr.GetScene(scene_guid);
                if (scene != null)
                {
                    scene.PlayerLevelScene(uuid);
                }
                rsp.rsp();
            }
            catch (System.Exception ex)
            {
                Log.Log.err($"{ex}");
                rsp.err((int)em_error.db_error);
            }
        }

        private async void Scene_mgr_Module_on_create_scene(string sceneName)
        {
            var uuid = Hub.Hub._gates.current_client_uuid;
            var rsp = scene_mgr_Module.rsp as scene_mgr_create_scene_rsp;

            Log.Log.trace("on_create_scene begin! uuid:{0} sceneName:{1}", uuid, sceneName);

            var token = Guid.NewGuid().ToString();
            try
            {
                var scene = Server.SceneMgr.CreateScene(sceneName);
                var info = new scene_hub_info
                {
                    scene_name = sceneName,
                    scene_guid = scene.sceneID,
                    scene_hub_id = Hub.Hub.name,
                };
                rsp.rsp(info);

                await Server.RedisHandle.Lock(RedisHelp.BuildLoadSceneLockKey(), token, 2000);
                
                var _scene_info_list = await Server.RedisHandle.GetData<List<scene_hub_info>>(RedisHelp.BuildSceneInfoListKey());
                if (_scene_info_list == null)
                {
                    _scene_info_list = new();
                }
                _scene_info_list.Add(info);
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

        private async void Scene_mgr_Module_on_get_scene_list()
        {
            var uuid = Hub.Hub._gates.current_client_uuid;
            var rsp = scene_mgr_Module.rsp as scene_mgr_get_scene_list_rsp;

            Log.Log.trace("on_get_scene_list begin! uuid:{0}", uuid);
            try
            {
                var _scene_info_list = await Server.RedisHandle.GetData<List<scene_hub_info>>(RedisHelp.BuildSceneInfoListKey());
                if (_scene_info_list == null)
                {
                    _scene_info_list = new();
                }
                rsp.rsp(_scene_info_list);
            }
            catch (System.Exception ex)
            {
                Log.Log.err($"{ex}");
                rsp.err((int)em_error.db_error);
            }
        }
    }
}
