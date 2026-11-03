using Abelkhan;
using Newtonsoft.Json;
using SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public class netScene
{
    public static void NpcIntoScene(string scene_guid, NPCInformation info)
    {
#if IS_TEST
        return;
#endif
        netDriver.SceneCaller.get_hub(netDriver.ServerHubName).npc_into_scene(scene_guid, info);
    }

    public static void NpcIntoSubscene(string scene_guid, string npcUUID, string subscene_name)
    {
#if IS_TEST
        return;
#endif
        netDriver.SceneCaller.get_hub(netDriver.ServerHubName).npc_into_subscene(scene_guid, npcUUID, subscene_name);
    }

    public static void NpcMove(string npcUUID, string scene_guid, string subscene_name, Pos start, Abelkhan.Direction dir)
    {
#if IS_TEST
        return;
#endif
        netDriver.SceneCaller.get_hub(netDriver.ServerHubName).npc_move(npcUUID, scene_guid, subscene_name, start, dir);
    }

    public static void RemoveBuilding(List<Building> buildings, List<Cultivated> cultivateds, bool rmFences)
    {
#if IS_TEST
        return;
#endif
        if (netDriver.SceneHubInfo == null)
        {
            Debug.LogWarning("SceneHubInfo is null, skip RemoveBuilding call");
            return;
        }
        netDriver.SceneCaller.get_hub(netDriver.SceneHubInfo.scene_hub_id).remove_building(buildings, cultivateds, rmFences);
    }

    public static void Building(List<Building> buildings, Fence fences)
    {
#if IS_TEST
        return;
#endif
        if (netDriver.SceneHubInfo == null)
        {
            Debug.LogWarning("SceneHubInfo is null, skip Building call");
            return;
        }
        netDriver.SceneCaller.get_hub(netDriver.SceneHubInfo.scene_hub_id).building(buildings, fences);
    }

    public static void Cultivated(List<Cultivated> cultivateds)
    {
#if IS_TEST
        return;
#endif
        if (netDriver.SceneHubInfo == null)
        {
            Debug.LogWarning("SceneHubInfo is null, skip Cultivated call");
            return;
        }
        netDriver.SceneCaller.get_hub(netDriver.SceneHubInfo.scene_hub_id).arable_land(cultivateds);
    }

    public static void Move(Pos start, Abelkhan.Direction dir)
    {
#if IS_TEST
        return;
#endif
        //Debug.LogFormat("netScene Move netDriver.SceneCaller:{0}", netDriver.SceneCaller);
        //Debug.LogFormat("netScene Move netDriver.SceneHubInfo:{0}", netDriver.SceneHubInfo);
        if (netDriver.SceneHubInfo == null)
        {
            return;
        }

        netDriver.SceneCaller.get_hub(netDriver.SceneHubInfo.scene_hub_id).move(start, dir);
    }

    public static void ChangeScene(SceneChange change)
    {
#if IS_TEST
        return;
#endif
        if (netDriver.SceneHubInfo == null)
        {
            Debug.LogWarning("SceneHubInfo is null, skip ChangeScene call");
            return;
        }
        Debug.Log($"netScene ChangeScene netDriver.SceneCaller:{JsonConvert.SerializeObject(change)}");
        netDriver.SceneCaller.get_hub(netDriver.SceneHubInfo.scene_hub_id).scene_change(change);
    }

    public static Task<bag_def> GetBag()
    {
#if IS_TEST
        return null;
#endif

        var _t = new TaskCompletionSource<bag_def>();

        netDriver.BagCaller.get_hub(netDriver.ServerHubName).get_bag().callBack((bag) =>
        {
            _t.SetResult(bag);
        }, () =>
        {
            Log.Log.err("get_bag err");
            _t.SetResult(null);
        }).timeout(3000, () =>
        {
            Log.Log.err("get_bag timeout");
            _t.SetResult(null);
        });

        return _t.Task;
    }

    public static async Task<bool> UseItem(string uuid, string itemAgrv)
    {
#if IS_TEST
        return false;
#endif
        var _t = new TaskCompletionSource<bool>();

        netDriver.BagCaller.get_hub(netDriver.ServerHubName).use_item(uuid, itemAgrv).callBack(() =>
        {
            _t.SetResult(true);
        }, () =>
        {
            Log.Log.err("get_bag err");
            _t.SetResult(false);
        }).timeout(3000, () =>
        {
            Log.Log.err("get_bag timeout");
            _t.SetResult(false);
        });

        return await(_t.Task);
    }

    public static void InitFence(Fence fences, string pwd)
    {
#if IS_TEST
        return ;
#endif
        netDriver.SceneCaller.get_hub(netDriver.SceneHubInfo.scene_hub_id).init_fence(fences, pwd);
    }

    // 导入角色
    public static async Task<List<Role>> ImportRole(Role role)
    {
#if IS_TEST
        return null;
#endif
        var _t = new TaskCompletionSource<List<Role>>();

        netDriver.SceneCaller.get_hub(netDriver.ServerHubName).import_role(role).callBack((List<Role> rs) =>
        {
            _t.SetResult(rs);
        }, (e) =>
        {
            Log.Log.err("import_role err:{0}", e);
            _t.SetResult(null);
        }).timeout(3000, () =>
        {
            Log.Log.err("import_role timeout");
            _t.SetResult(null);
        });

        return await (_t.Task);
    }

    //播种指定的菜到指定的田地
    public static async Task<List<Vegetable>> grow(Vegetable vegetable, Cultivated cultivated)
    {
#if IS_TEST
        return null;
#endif
        Debug.LogFormat("grow vegetable:{0} cultivated:{1}", vegetable.VegetableGuid, cultivated.CultivatedGuid);

        if (netDriver.SceneCaller == null || string.IsNullOrEmpty(netDriver.ServerHubName))
        {
            Debug.LogWarning("SceneCaller or ServerHubName is null, skip grow call");
            return null;
        }
        var _t = new TaskCompletionSource<List<Vegetable>>();

        netDriver.SceneCaller.get_hub(netDriver.ServerHubName).grow(vegetable, cultivated).callBack((List<Vegetable> vs) =>
        {
            _t.SetResult(vs);
        }, (e) =>
        {
            Log.Log.err("grow err:{0}", e);
            _t.SetResult(null);
        }).timeout(3000, () =>
        {
            Log.Log.err("grow timeout");
            _t.SetResult(null);
        });

        return await (_t.Task);
    }

    // 主人收获指定的菜
    public static async Task<bool> harvest(Vegetable vegetable)
    {
#if IS_TEST
        return false;
#endif
        var _t = new TaskCompletionSource<bool>();

        netDriver.SceneCaller.get_hub(netDriver.ServerHubName).harvest(vegetable).callBack(() =>
        {
            _t.SetResult(true);
        }, (e) =>
        {
            Log.Log.err("grow err:{0}", e);
            _t.SetResult(false);
        }).timeout(3000, () =>
        {
            Log.Log.err("grow timeout");
            _t.SetResult(false);
        });

        return await(_t.Task);
    }

    public static async Task<bool> cut_vegetable(Vegetable vegetable)
    {
#if IS_TEST
        return false;
#endif
        var _t = new TaskCompletionSource<bool>();

        netDriver.SceneCaller.get_hub(netDriver.ServerHubName).cut_down(vegetable).callBack(() =>
        {
            _t.SetResult(true);
        }, (e) =>
        {
            Log.Log.err("grow err:{0}", e);
            _t.SetResult(false);
        }).timeout(3000, () =>
        {
            Log.Log.err("grow timeout");
            _t.SetResult(false);
        });

        return await (_t.Task);
    }

    public static async Task<int> sales_vegetable(Vegetable vegetable, int coin)
    {
#if IS_TEST
        return false;
#endif
        var _t = new TaskCompletionSource<int>();

        netDriver.SceneCaller.get_hub(netDriver.ServerHubName).sales_vegetable(vegetable, coin).callBack((c) =>
        {
            _t.SetResult(c);
        }, (e) =>
        {
            Log.Log.err("grow err:{0}", e);
            _t.SetResult(-1);
        }).timeout(3000, () =>
        {
            Log.Log.err("grow timeout");
            _t.SetResult(-1);
        });

        return await (_t.Task);
    }

    // 偷指定的菜
    public static async Task<Vegetable> steal(Vegetable vegetable)
    {
#if IS_TEST
        return null;
#endif
        var _t = new TaskCompletionSource<Vegetable>();

        netDriver.SceneCaller.get_hub(netDriver.ServerHubName).steal(vegetable).callBack((Vegetable v) =>
        {
            _t.SetResult(v);
        }, (e) =>
        {
            Log.Log.err("grow err:{0}", e);
            _t.SetResult(null);
        }).timeout(3000, () =>
        {
            Log.Log.err("grow timeout");
            _t.SetResult(null);
        });

        return await(_t.Task);
    }

    // SceneNtfModule_on_scene_info
    public static void into_subscene(string subscene_name)
    {
        netDriver.SceneCaller.get_hub(netDriver.ServerHubName).into_subscene(subscene_name);
    }
}

public class netSceneModule
{
    public static void Init()
    {
#if IS_TEST
        return;
#endif
        netDriver.SceneNtfModule.on_building_scene_info += SceneNtfModule_on_building_scene_info;
        netDriver.SceneNtfModule.on_remove_scene_info += SceneNtfModule_on_remove_scene_info;
        netDriver.SceneNtfModule.on_arable_scene_info += SceneNtfModule_on_arable_scene_info;
        netDriver.SceneNtfModule.on_scene_change += SceneNtfModule_on_scene_change;
        netDriver.SceneNtfModule.on_scene_info += SceneNtfModule_on_scene_info;
        netDriver.SceneNtfModule.on_move += SceneNtfModule_on_move;
        netDriver.SceneNtfModule.on_player_leave_scene += SceneNtfModule_on_player_leave_scene;
        netDriver.SceneNtfModule.on_time_info += SceneNtfModule_on_time_info;
    }

    private static void SceneNtfModule_on_time_info(TimeInfo _info)
    {
        //Debug.Log($"on_time_info:{JsonConvert.SerializeObject(_info)}");
#if IS_TEST
        return;
#endif
        if (GameSceneManager.Instance != null)
        {
            GameSceneManager.Instance.UpdateServerTime(_info);
        }
    }

    private static void buildUpdate(ref List<Building> buildingArg, bool immedate = false)
    {
#if IS_TEST
        return;
#endif
        if (GameSceneManager.Instance == null)
        {
            Debug.LogError("buildUpdate: GameSceneManager.Instance is null");
            return;
        }
        
        foreach (var building in buildingArg)
        {
            var args = new SceneEventArgs
            {
                ObjectId = building.BuildingGuid,
                EventType = SceneEvent.CreateObject,
                Position = new Vector2(building.topLeft.X, building.topLeft.Y),
                PerfabPathId = building.BuildingId,

            };
            GameSceneManager.Instance.TriggerEvent(args.EventType, args, immedate);
        }
    }
    private static void moveUpdate(ref List<UserMoveInfo> moveInfoArg)
    {
#if IS_TEST
        return;
#endif
        if (GameSceneManager.Instance == null)
        {
            //Debug.LogError("moveUpdate: GameSceneManager.Instance is null");
            return;
        }
        
        //Debug.Log($"moveUpdate called moveInfoArg:{JsonConvert.SerializeObject(moveInfoArg)}");

        foreach (var move in moveInfoArg)
        {
            var args = new SceneEventArgs
            {
                EventType = SceneEvent.CharacterMoved,
                Position = new Vector2(move.Pos.X, move.Pos.Y),
                DirectionArg = (SceneManagement.Direction)move.dir,
                ObjectId = move.UserGuid,
                CharacterType = (CharacterType)move.type
            };
            GameSceneManager.Instance.TriggerEvent(args.EventType, args);
            //Debug.Log($"on_move! x:{move.Pos.X} y:{move.Pos.Y}");
        }
    }

    private static void SceneNtfModule_on_arable_scene_info(List<Cultivated> cultivateds, List<Vegetable> vegetables)
    {
#if IS_TEST
        return;
#endif
        if (GameSceneManager.Instance == null)
        {
            Debug.LogError("SceneNtfModule_on_arable_scene_info: GameSceneManager.Instance is null");
            return;
        }
        
        for( int i = 0; i< cultivateds.Count; i++ )
        {
            Cultivated temp = cultivateds[i];
            var args = new SceneEventArgs
            {
                EventType = SceneEvent.CreateCultivatedLand,
                Position = new Vector2(temp.topLeft.X, temp.topLeft.Y),
                CultivatedData = temp
             };
            GameSceneManager.Instance.TriggerEvent(SceneEvent.CreateCultivatedLand, args, true);
            
        }
        if (vegetables != null)
        {
            foreach (var vegetable in vegetables)
            {
                // 将单元格坐标转换为世界坐标
                var farmingTilemap = GameSceneManager.Instance.GetOrCreateTilemap("FarmingTilemap");
                if (farmingTilemap != null)
                {
                    Vector3Int cellPos = new Vector3Int((int)vegetable.topLeft.X, (int)vegetable.topLeft.Y, 0);
                    Vector3 worldPos = farmingTilemap.GetCellCenterWorld(cellPos);
                    var args = new SceneEventArgs
                    {
                        ObjectId = !string.IsNullOrEmpty(vegetable.VegetableGuid) ? vegetable.VegetableGuid : System.Guid.NewGuid().ToString(),
                        PerfabPathId = vegetable.perfabId,
                        Position = new Vector2(worldPos.x, worldPos.y),
                        GrowiTime = vegetable.growiTime,
                        RipenTime = vegetable.ripenTime
                    };
                    GameSceneManager.Instance.TriggerEvent(SceneEvent.CreateGrowUpObject, args, false);
                }
                else
                {
                    Debug.LogError("FarmingTilemap not found for vegetable creation");
                }
            }
        }
    }

    private static void SceneNtfModule_on_scene_change(SceneChange obj)
    {
#if IS_TEST
        return;
#endif
        Debug.Log($"on_scene_change:{JsonConvert.SerializeObject(obj.add_buildings)}");
        buildUpdate(ref obj.add_buildings,true);
    }

    private static void SceneNtfModule_on_player_leave_scene(string obj)
    {
#if IS_TEST
        return;
#endif
    }

    private static void SceneNtfModule_on_move(List<UserMoveInfo> moveInfo)
    {
#if IS_TEST
        return;
#endif
        moveUpdate(ref moveInfo);

    }

    private static void SceneNtfModule_on_scene_info(SceneInfo sceneInfo)
    {
#if IS_TEST
        return;
#endif
        if (sceneInfo == null)
        {
            Debug.LogWarning("SceneNtfModule_on_scene_info: sceneInfo is null");
            return;
        }
        Debug.Log("on_scene_info:");
        foreach(var c in sceneInfo.Cultivateds)
        {
            Debug.Log($"Cultivateds:{c.CultivatedGuid}");
        }
        foreach (var v in sceneInfo.Vegetables)
        {
            Debug.Log($"Vegetables:{v.VegetableGuid}");
        }
        Debug.Log("on_scene_info end!");

        if (GameSceneManager.Instance == null)
        {
            Debug.LogError("SceneNtfModule_on_scene_info: GameSceneManager.Instance is null");
            return;
        }
        
        // Process fences (log only for now)
        if (sceneInfo.fences != null && sceneInfo.fences.Count > 0)
        {
            Debug.Log($"Scene info contains {sceneInfo.fences.Count} fences (not processed)");
        }
        
        buildUpdate(ref sceneInfo.buildings,false);
        
        // Process cultivated land
        if (sceneInfo.Cultivateds != null)
        {
            foreach (var cultivated in sceneInfo.Cultivateds)
            {
                var args = new SceneEventArgs
                {
                    EventType = SceneEvent.CreateCultivatedLand,
                    Position = new Vector2(cultivated.topLeft.X, cultivated.topLeft.Y),
                    CultivatedData = cultivated
                };
                GameSceneManager.Instance.TriggerEvent(args.EventType, args, true);
                Debug.Log($"Cultivated land created at cell position: {cultivated.topLeft.X}, {cultivated.topLeft.Y}");
            }
        }
        
        // Process vegetables
        if (sceneInfo.Vegetables != null)
        {
            foreach (var vegetable in sceneInfo.Vegetables)
            {
                var farmingTilemap = GameSceneManager.Instance.GetOrCreateTilemap("FarmingTilemap");
                if (farmingTilemap != null)
                {
                    Vector3Int cellPos = new Vector3Int((int)vegetable.topLeft.X, (int)vegetable.topLeft.Y, 0);
                    Vector3 worldPos = farmingTilemap.GetCellCenterWorld(cellPos);
                    var args = new SceneEventArgs
                    {
                        ObjectId = !string.IsNullOrEmpty(vegetable.VegetableGuid) 
                            ? vegetable.VegetableGuid 
                            : System.Guid.NewGuid().ToString(),
                        PerfabPathId = vegetable.perfabId,
                        Position = new Vector2(worldPos.x, worldPos.y),
                        GrowiTime = vegetable.growiTime,
                        RipenTime = vegetable.ripenTime
                    };
                    GameSceneManager.Instance.TriggerEvent(SceneEvent.CreateGrowUpObject, args, true);
                    Debug.Log($"Vegetable created with GUID: {args.ObjectId}, ID: {vegetable.perfabId} at position: {worldPos}");
                }
                else
                {
                    Debug.LogError("FarmingTilemap not found for vegetable creation in scene info");
                }
            }
        }
        
        // Process users
        if (sceneInfo.users != null)
        {
            foreach (var move in sceneInfo.users)
        {
            var args = new SceneEventArgs
            {
                EventType = SceneEvent.CharacterMoved,
                Position = new Vector2(move.info.Pos.X, move.info.Pos.Y),
                DirectionArg = (SceneManagement.Direction)move.info.dir,
                ObjectId = move.info.UserGuid,
                CharacterType = (CharacterType)move.type
            };
            GameSceneManager.Instance.TriggerEvent(args.EventType, args);
            Debug.Log($"on_move! x:{move.info.Pos.X} y:{move.info.Pos.Y}");
            }
        }


    }

    private static void SceneNtfModule_on_remove_scene_info(List<Building> buildings, List<Cultivated> cultivateds, List<Vegetable> vegetables, Fence fences)
    {
#if IS_TEST
        return;
#endif
        Debug.Log($"SceneNtfModule_on_remove_scene_info received - buildings: {buildings?.Count}, cultivateds: {cultivateds?.Count}, vegetables: {vegetables?.Count}, fences: {fences != null}");
        
        // Handle vegetable removal
        if (vegetables != null && vegetables.Count > 0)
        {
            foreach (var vegetable in vegetables)
            {
                if (!string.IsNullOrEmpty(vegetable.VegetableGuid))
                {
                    Debug.Log($"Removing vegetable with GUID: {vegetable.VegetableGuid}");
                    GameSceneManager.Instance.RemoveGrowUpObjectByGuid(vegetable.VegetableGuid);
                }
                else
                {
                    Debug.LogWarning("Vegetable in removal list has empty GUID");
                }
            }
        }
        
        // Note: buildings, cultivateds, and fences removal not implemented yet
        // but could be added here if needed
    }

    private static void SceneNtfModule_on_building_scene_info(List<Building> buildings, Fence fences)
    {
#if IS_TEST
        return;
#endif
    }
}
