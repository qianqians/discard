using Abelkhan;
using InfluxData.Net.Kapacitor.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Server
{
    class scene_msg_handle
    {
        private scene_module scene_Module = new scene_module();

        public scene_msg_handle()
        {
            scene_Module.on_into_subscene += Scene_Module_on_into_subscene;
            scene_Module.on_move += Scene_Module_on_move;
            scene_Module.on_remove_building += Scene_Module_on_remove_building;
            scene_Module.on_building += Scene_Module_on_building;
            scene_Module.on_arable_land += Scene_Module_on_arable_land;
            scene_Module.on_scene_change += Scene_Module_on_scene_change;

            scene_Module.on_import_role += Scene_Module_on_import_role;
            scene_Module.on_init_fence += Scene_Module_on_init_fence;
            scene_Module.on_grow += Scene_Module_on_grow;
            scene_Module.on_harvest += Scene_Module_on_harvest;
            scene_Module.on_steal += Scene_Module_on_steal;
            scene_Module.on_cut_down += Scene_Module_on_cut_down;

            scene_Module.on_sales_vegetable += Scene_Module_on_sales_vegetable;

            scene_Module.on_npc_into_scene += Scene_Module_on_npc_into_scene;
            scene_Module.on_npc_into_subscene += Scene_Module_on_npc_into_subscene;
            scene_Module.on_npc_move += Scene_Module_on_npc_move;
        }

        private void Scene_Module_on_npc_move(string npcUUID, string scene_guid, string subscene_name, Pos start, Direction dir)
        {
            var scene = Server.SceneMgr.GetScene(scene_guid);
            if (scene != null)
            {
                if (scene.Npcs.TryGetValue(npcUUID, out var npc))
                {

                    if (subscene_name == "main")
                    {
                        scene.NPCMove(npcUUID, start, dir);
                    }
                    else
                    {
                        if (!scene.subScenes.TryGetValue(subscene_name, out var subScene))
                        {
                            subScene = new SubScene()
                            {
                                subSceneName = subscene_name,
                                SceneClientCaller = Scene.SceneClientCaller,
                            };
                            scene.subScenes.Add(subscene_name, subScene);
                        }
                        subScene.SyncNpcMove(npc, start, dir);
                    }
                }
            }
        }

        private void Scene_Module_on_npc_into_subscene(string scene_guid, string npcUUID, string subscene_name)
        {
            var scene = Server.SceneMgr.GetScene(scene_guid);
            if (scene != null)
            {
                if (scene.Npcs.TryGetValue(npcUUID, out var npc))
                {
                    if (!scene.subScenes.TryGetValue(subscene_name, out var subScene))
                    {
                        subScene = new SubScene()
                        {
                            subSceneName = subscene_name,
                            SceneClientCaller = Scene.SceneClientCaller,
                        };
                        scene.subScenes.Add(subscene_name, subScene);
                    }
                    subScene.AddNpc(npc.npcInformation);
                }
            }
        }

        private void Scene_Module_on_npc_into_scene(string scene_guid, NPCInformation info)
        {

            Server.SceneMgr.GetScene(scene_guid)?.AddNpc(info);
        }

        private void Scene_Module_on_into_subscene(string subSceneName)
        {
            var uuid = Hub.Hub._gates.current_client_uuid;

            Log.Log.trace("on_into_subscene begin! uuid:{0}", uuid);
            try
            {
                var scene = Server.SceneMgr.ClientScenes[uuid];
                if (scene != null)
                {
                    if (subSceneName == "main")
                    {
                        foreach(var subScene1 in scene.subScenes.Values)
                        {
                            foreach(var (netUUID, player1) in subScene1.NetUUIDPlayers)
                            {
                                if (netUUID == uuid)
                                {
                                    scene.PlayerIntoScene(player1);
                                    return;
                                }
                            }
                        }
                    }

                    if (!scene.subScenes.TryGetValue(subSceneName, out var subScene))
                    {
                        subScene = new SubScene()
                        {
                            subSceneName = subSceneName,
                            SceneClientCaller = Scene.SceneClientCaller,
                        };
                        scene.subScenes.Add(subSceneName, subScene);
                    }
                    if (scene.NetUUIDPlayers.TryGetValue(uuid, out var player))
                    {
                        subScene.IntoSubScene(player);
                        scene.NetUUIDPlayers.Remove(uuid);
                        scene.Players.Remove(player.userInformation.UserGuid);

                        foreach (var (_, p) in scene.NetUUIDPlayers)
                        {
                            var info = scene.SceneInfo(p.userInformation.UserGuid);
                            Scene.SceneClientCaller.get_multicast([p.NetUUID]).scene_info(info);
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                Log.Log.err($"{ex}");
            }
        }

        private async void Scene_Module_on_cut_down(Vegetable vegetable)
        {
            var uuid = Hub.Hub._gates.current_client_uuid;

            Log.Log.trace("on_cut_down begin! uuid:{0}", uuid);
            try
            {
                var rsp = (scene_cut_down_rsp)scene_Module.rsp;

                var scene = Server.SceneMgr.ClientScenes[uuid];
                if (scene != null)
                {
                    var ret = await scene.CutVegetable(uuid, vegetable);
                    if (ret != (int)em_error.success)
                    {
                        rsp.err(ret);
                    }
                    else
                    {
                        rsp.rsp();
                        Scene.SceneClientCaller.get_multicast([..scene.NetUUIDPlayers.Keys]).remove_scene_info([], [], [vegetable], null);
                    }
                }
            }
            catch (System.Exception ex)
            {
                Log.Log.err($"{ex}");
            }
        }

        private async void Scene_Module_on_sales_vegetable(Vegetable v, int coin)
        {
            var uuid = Hub.Hub._gates.current_client_uuid;

            Log.Log.trace("on_sales_vegetable begin! uuid:{0}", uuid);
            try
            {
                var rsp = (scene_sales_vegetable_rsp)scene_Module.rsp;

                var scene = Server.SceneMgr.ClientScenes[uuid];
                if (scene != null)
                {
                    var c = await scene.SalesVegetable(uuid, v, coin);
                    if (c > 0)
                    {
                        rsp.rsp(c);
                        Scene.SceneClientCaller.get_multicast([.. scene.NetUUIDPlayers.Keys]).remove_scene_info([], [], [v], null);
                    }
                    else
                    {
                        rsp.err((int)em_error.scene_sales_vegetable_failed);
                    }
                }
            }
            catch (System.Exception ex)
            {
                Log.Log.err($"Scene_Module_on_sales_vegetable {ex}");
            }
        }

        private void Scene_Module_on_arable_land(List<Cultivated> cultivated)
        {
            var uuid = Hub.Hub._gates.current_client_uuid;

            Log.Log.trace("on_building begin! uuid:{0}", uuid);
            try
            {
                var scene = Server.SceneMgr.ClientScenes[uuid];
                if (scene != null)
                {
                    scene.PlayerBuilding(uuid, [], ref cultivated, null);
                    Scene.SceneClientCaller.get_multicast([..scene.NetUUIDPlayers.Keys]).arable_scene_info(cultivated, scene.GetSceneVegetable());
                }
            }
            catch (System.Exception ex)
            {
                Log.Log.err($"Scene_Module_on_arable_land {ex}");
            }
        }

        private void Scene_Module_on_import_role(Role role)
        {
            var uuid = Hub.Hub._gates.current_client_uuid;

            Log.Log.trace("on_harvest begin! uuid:{0}", uuid);
            try
            {
                var rsp = (scene_harvest_rsp)scene_Module.rsp;

                var scene = Server.SceneMgr.ClientScenes[uuid];
                if (scene != null)
                {
                    var ret = scene.ImportRole(uuid, role);
                    if (ret != (int)em_error.success)
                    {
                        rsp.err(ret);
                    }
                    else
                    {
                        rsp.rsp();
                    }
                }
            }
            catch (System.Exception ex)
            {
                Log.Log.err($"Scene_Module_on_import_role {ex}");
            }
        }

        private async void Scene_Module_on_steal(Vegetable vegetable)
        {
            var uuid = Hub.Hub._gates.current_client_uuid;

            Log.Log.trace("on_harvest begin! uuid:{0}", uuid);
            try
            {
                var rsp = (scene_harvest_rsp)scene_Module.rsp;

                var scene = Server.SceneMgr.ClientScenes[uuid];
                if (scene != null)
                {
                    var ret = await scene.StealVegetable(uuid, vegetable);
                    if (ret != (int)em_error.success)
                    {
                        rsp.err(ret);
                    }
                    else
                    {
                        rsp.rsp();
                    }
                }
            }
            catch (System.Exception ex)
            {
                Log.Log.err($"Scene_Module_on_steal {ex}");
            }
        }

        private async void Scene_Module_on_harvest(Vegetable vegetable)
        {
            var uuid = Hub.Hub._gates.current_client_uuid;

            Log.Log.trace("on_harvest begin! uuid:{0}", uuid);
            try
            {
                var rsp = (scene_harvest_rsp)scene_Module.rsp;

                var scene = Server.SceneMgr.ClientScenes[uuid];
                if (scene != null)
                {
                    var ret = await scene.HarvestVegetable(uuid, vegetable);
                    if (ret != (int)em_error.success)
                    {
                        rsp.err(ret);
                    }
                    else
                    {
                        rsp.rsp();
                        Scene.SceneClientCaller.get_multicast([.. scene.NetUUIDPlayers.Keys]).remove_scene_info([], [], [vegetable], null);
                    }
                }
            }
            catch (System.Exception ex)
            {
                Log.Log.err($"{ex}");
            }
        }

        private void Scene_Module_on_grow(Vegetable vegetable, Cultivated cultivated)
        {
            var uuid = Hub.Hub._gates.current_client_uuid;

            Log.Log.trace("on_grow begin! uuid:{0}", uuid);
            try
            {
                var rsp = (scene_grow_rsp)scene_Module.rsp;

                var scene = Server.SceneMgr.ClientScenes[uuid];
                if (scene != null)
                {
                    var (ret, vegetables) = scene.GrowVegetable(uuid, vegetable, cultivated);
                    if (ret != (int)em_error.success)
                    {
                        rsp.err(ret);
                    }
                    else
                    {
                        rsp.rsp(vegetables);
                        Scene.SceneClientCaller.get_multicast([..scene.NetUUIDPlayers.Keys]).arable_scene_info(scene.GetSceneCultivated(), scene.GetSceneVegetable());
                    }
                }
            }
            catch (System.Exception ex)
            {
                Log.Log.err($"{ex}");
            }
        }

        private void Scene_Module_on_init_fence(Fence fence, string pwd)
        {
            var uuid = Hub.Hub._gates.current_client_uuid;

            Log.Log.trace("on_init_fence begin! uuid:{0}", uuid);
            try
            {
                var scene = Server.SceneMgr.ClientScenes[uuid];
                if (scene != null)
                {
                    var ret = scene.PlayerInitFence(uuid, fence, pwd);
                }
            }
            catch (System.Exception ex)
            {
                Log.Log.err($"{ex}");
            }
        }

        private void Scene_Module_on_scene_change(SceneChange change)
        {
            var uuid = Hub.Hub._gates.current_client_uuid;

            Log.Log.trace("on_scene_change begin! uuid:{0}", uuid);
            try
            {
                var scene = Server.SceneMgr.ClientScenes[uuid];
                if (scene != null)
                {
                    bool rmFence = change.new_fences != null;
                    scene.PlayerRemoveBuilding(uuid, change.remove_buildings, change.remove_cultivated, rmFence);
                    scene.PlayerBuilding(uuid, change.add_buildings, ref change.add_cultivated, change.new_fences);

                    Scene.SceneClientCaller.get_multicast([..scene.NetUUIDPlayers.Keys]).scene_change(change);
                }
            }
            catch (System.Exception ex)
            {
                Log.Log.err($"{ex}");
            }
        }

        private void Scene_Module_on_remove_building(List<Building> buildings, List<Cultivated> cultivateds, bool rmfences)
        {
            var uuid = Hub.Hub._gates.current_client_uuid;
            
            Log.Log.trace("on_remove_building begin! uuid:{0}", uuid);
            try
            {
                var scene = Server.SceneMgr.ClientScenes[uuid];
                if (scene != null)
                {
                    var fences = scene.PlayerRemoveBuilding(uuid, buildings, cultivateds, rmfences);
                    Scene.SceneClientCaller.get_multicast([..scene.NetUUIDPlayers.Keys]).remove_scene_info(buildings, cultivateds, [], fences);
                }
            }
            catch (System.Exception ex)
            {
                Log.Log.err($"{ex}");
            }
        }

        private void Scene_Module_on_building(List<Building> buildings, Fence fences)
        {
            var uuid = Hub.Hub._gates.current_client_uuid;

            Log.Log.trace("on_building begin! uuid:{0}", uuid);
            try
            {
                var scene = Server.SceneMgr.ClientScenes[uuid];
                if (scene != null)
                {
                    var cultivateds = new List<Cultivated>();
                    scene.PlayerBuilding(uuid, buildings, ref cultivateds, fences);
                    Scene.SceneClientCaller.get_multicast([..scene.NetUUIDPlayers.Keys]).building_scene_info(buildings, fences);
                }
            }
            catch (System.Exception ex)
            {
                Log.Log.err($"{ex}");
            }
        }

        private void Scene_Module_on_move(Pos pos, Direction dir)
        {
            var uuid = Hub.Hub._gates.current_client_uuid;

            Log.Log.trace("on_move begin! uuid:{0}", uuid);
            try
            {
                var scene = Server.SceneMgr.ClientScenes[uuid];
                if (scene != null)
                {
                    scene.PlayerMove(uuid, pos, dir);
                }
            }
            catch (System.Exception ex)
            {
                Log.Log.err($"{ex}");
            }
        }
    }
}
