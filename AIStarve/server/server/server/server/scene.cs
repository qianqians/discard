using Abelkhan;
using MongoDB.Bson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Server
{
    public class Npc
    {
        public NPCInformation npcInformation;
    }

    public class SubScene
    {
        private Dictionary<string, Player> players = new();
        public Dictionary<string, Player> Players
        {
            get
            {
                return players;
            }
        }

        private Dictionary<string, Npc> npcs = new();
        public Dictionary<string, Npc> Npcs
        {
            get
            {
                return npcs;
            }
        }

        private Dictionary<string, Player> netUUIDPlayers = new();
        public Dictionary<string, Player> NetUUIDPlayers
        {
            get
            {
                return netUUIDPlayers;
            }
        }

        public string subSceneName;

        public scene_client_caller SceneClientCaller;

        public SceneInfo SceneInfo()
        {
            var info = new SceneInfo();
            info.users = new List<UserSceneInfo>();
            info.fences = new List<Fence>();
            info.Cultivateds = new List<Cultivated>();
            info.Vegetables = new List<Vegetable>();
            info.buildings = new List<Building>();

            foreach (var player in players.Values)
            {
                if (player.IsOnline)
                {
                    var type = AvatarType.Friend;
                    info.users.Add(new UserSceneInfo()
                    {
                        type = type,
                        info = player.userInformation
                    });
                }
            }

            foreach (var npc in Npcs.Values)
            {
                var type = AvatarType.Friend;
                info.npcs.Add(new NPCSceneInfo()
                {
                    type = type,
                    info = npc.npcInformation
                });
            }

            return info;
        }

        public void AddNpc(NPCInformation info)
        {
            var npc = new Npc()
            {
                npcInformation = info
            };
            Npcs[info.NPCGuid] = npc;

            SyncSubScene();
        }

        public void SyncMove(Player player, Pos pos, Direction dir)
        {
            player.userInformation.Pos = pos;
            player.userInformation.dir = dir;

            foreach (var item in netUUIDPlayers)
            {
                var type = AvatarType.Friend;
                if (player.userInformation.UserGuid == item.Value.userInformation.UserGuid)
                {
                    type = AvatarType.Self;
                }

                var info = new UserMoveInfo() {
                    type = type,
                    UserGuid = player.userInformation.UserGuid,
                    Pos = player.userInformation.Pos,
                    dir = player.userInformation.dir,
                };

                SceneClientCaller.get_multicast(new() { item.Key }).move(new List<UserMoveInfo> { info });
            }
        }

        public void SyncNpcMove(Npc npc, Pos pos, Direction dir)
        {
            npc.npcInformation.Pos = pos;
            npc.npcInformation.dir = dir;

            foreach (var item in netUUIDPlayers)
            {
                var type = AvatarType.Friend;
                if (npc.npcInformation.ownerGuid == item.Value.userInformation.UserGuid)
                {
                    type = AvatarType.Self;
                }

                var info = new UserMoveInfo()
                {
                    type = type,
                    UserGuid = npc.npcInformation.NPCGuid,
                    NPCTableID = npc.npcInformation.NPCTableID,
                    Pos = npc.npcInformation.Pos,
                    dir = npc.npcInformation.dir,
                };

                SceneClientCaller.get_multicast(new() { item.Key }).move(new List<UserMoveInfo> { info });
            }
        }

        public void SyncSubScene()
        {
            var info = SceneInfo();
            foreach (var (_, p) in NetUUIDPlayers)
            {
                foreach (var _player in info.users)
                {
                    if (_player.info.UserGuid == p.userInformation.UserGuid)
                    {
                        _player.type = AvatarType.Self;
                        break;
                    }
                }

                foreach (var npc in info.npcs)
                {
                    if (npc.info.ownerGuid == p.userInformation.UserGuid)
                    {
                        npc.type = AvatarType.Self;
                        break;
                    }
                }

                Scene.SceneClientCaller.get_multicast([p.NetUUID]).scene_info(info);
            }
        }

        public void IntoSubScene(Player player)
        {
            Players[player.userInformation.UserGuid] = player;
            NetUUIDPlayers[player.NetUUID] = player;
            SyncSubScene();
        }

        public void LeaveSubScene(string uuid)
        {
            if (NetUUIDPlayers.TryGetValue(uuid, out var p))
            {
                NetUUIDPlayers.Remove(uuid);
                Players.Remove(p.userInformation.UserGuid);
                SyncSubScene();
            }
        }
    }

    public class Scene
    {
        public static int Length
        {
            get {
                return 1024;
            }
            private set {}
        }

        public static int Width
        {
            get
            {
                return 1024;
            }
            private set { }
        }

        public static scene_client_caller SceneClientCaller = new scene_client_caller();

        public static async Task<Scene> LoadScene(string _sceneName, string _sceneID)
        {
            Log.Log.trace("LoadScene sceneName:{0} sceneID:{1}!", _sceneName, _sceneID);

            var scene = new Scene();
            scene.dbProxy = Hub.Hub.get_random_dbproxyproxy();
            scene.sceneName = _sceneName;
            scene.sceneID = _sceneID;

            var playerList = await scene.loadPlayers();
            Log.Log.trace("LoadScene playerList: {0}", playerList.ToJson());
            if (playerList != null)
            {
                Log.Log.trace("LoadScene playerList:{0}!", playerList.ToJson());
                foreach (var playerGuid in playerList)
                {
                    Log.Log.trace("LoadScene LoadPlayer playerGuid:{0}!", playerGuid);
                    var player = await Player.LoadPlayer(scene.dbProxy, scene.sceneID, playerGuid);
                    if (player != null)
                    {
                        Log.Log.trace("LoadScene LoadPlayer playerGuid:{0} comelete!", playerGuid);
                        scene.Players.Add(playerGuid, player);
                    }
                    else
                    {
                        Log.Log.trace("LoadScene LoadPlayer playerGuid:{0} is null!", playerGuid);
                    }
                }
                Log.Log.trace("LoadScene LoadPlayer playerList:{0}!", playerList.ToJson());
            }

            Hub.Hub._timer.addticktime(5 * 60 * 1000, scene.saveScene);

            Log.Log.trace("LoadScene sceneName:{0} sceneID:{1} end!", _sceneName, _sceneID);
            return scene;
        }

        public static Scene CreateScene(string _sceneName)
        {
            var scene = new Scene();
            scene.dbProxy = Hub.Hub.get_random_dbproxyproxy();
            scene.sceneName = _sceneName;
            scene.sceneID = Guid.NewGuid().ToString();

            Hub.Hub._timer.addticktime(5 * 60 * 1000, scene.saveScene);

            return scene;
        }

        public Dictionary<string, SubScene> subScenes = new();

        private Dictionary<string, Player> players = new();
        public Dictionary<string, Player> Players { 
            get { 
                return players; 
            } 
        }

        private Dictionary<string, Npc> npcs = new();
        public Dictionary<string, Npc> Npcs
        {
            get
            {
                return npcs;
            }
        }

        private Dictionary<string, Player> netUUIDPlayers = new();
        public Dictionary<string, Player> NetUUIDPlayers
        {
            get
            {
                return netUUIDPlayers;
            }
        }

        private Hub.DBProxyProxy dbProxy;
        public string sceneName;
        public string sceneID;
        private Task<List<string>> loadPlayers()
        {
            Log.Log.trace("loadPlayers getObjectInfo begin1!");

            var _t = new TaskCompletionSource<List<string>>();

            Log.Log.trace("loadPlayers getObjectInfo begin2!");

            var query = new DBQueryHelper();
            query.condition("data_type", "PlayerList");
            query.condition("SceneName", sceneName);
            query.condition("SceneID", sceneID);
            dbProxy.getCollection("aiStarve", "game").getObjectInfo(query.query(), (value) => {
                Log.Log.trace("loadPlayers getObjectInfo callback:{0}", value);

                if (value.Count > 1)
                {
                    throw new PlayerFromDBError($"repeated PlayerList sceneName:{sceneName}");
                }
                if (value.Count == 1)
                {
                    var doc = value[0] as BsonDocument;
                    Log.Log.trace("loadPlayers getObjectInfo doc:{0}", doc);
                    var plist = doc["_list"];

                    if (plist == null)
                    {
                        _t.SetResult([]);
                    }
                    else
                    {
                        Log.Log.trace("loadPlayers getObjectInfo plist:{0}", plist);
                        var list = plist.AsBsonArray.Select(v => v.AsString).ToList();
                        _t.SetResult(list);
                    }
                }
                else
                {
                    _t.SetResult([]);
                }
            }, () => {
                Log.Log.trace("loadPlayers getObjectInfo end");
            });

            return _t.Task;
        }

        private Scene()
        {
            Hub.Hub._timer.addticktime(5 * 60 * 1000, savePlayer);
        }

        private void savePlayer(long _)
        {
            foreach (var player in players.Values)
            {
                player.SavePlayer();
            }

            Hub.Hub._timer.addticktime(5 * 60 * 1000, savePlayer);
        }

        private void saveScene(long _)
        {
            var query = new DBQueryHelper();
            query.condition("data_type", "PlayerList");
            query.condition("SceneName", sceneName);
            query.condition("SceneID", sceneID);
            var data = new UpdateDataHelper();
            data.set(new Dictionary<string, List<string>>{ { "_list", players.Keys.ToList() } });
            dbProxy.getCollection("aiStarve", "game").updataPersistedObject(query.query(), data.data(), true, (ret) => {
                Log.Log.info($"Scene saveScene updataPersistedObject result:{ret}");
            });

            Hub.Hub._timer.addticktime(5 * 60 * 1000, saveScene);
        }

        public SceneInfo SceneInfo(string userGuid)
        {
            var info = new SceneInfo();
            info.users = new List<UserSceneInfo>();
            info.fences = new List<Fence>();
            info.Cultivateds = new List<Cultivated>();
            info.Vegetables = new List<Vegetable>();
            info.buildings = new List<Building>();

            foreach(var player in players.Values)
            {
                if (player.IsOnline)
                {
                    var type = AvatarType.Friend;
                    if (player.userInformation.UserGuid == userGuid)
                    {
                        type = AvatarType.Self;
                    }
                    info.users.Add(new UserSceneInfo()
                    {
                        type = type,
                        info = player.userInformation
                    });
                }

                if (player.fences != null)
                {
                    info.fences.Add(player.fences);
                }

                if (player.cultivateds != null)
                {
                    info.Cultivateds.AddRange(player.cultivateds);
                }

                if (player.vegetables != null)
                {
                    info.Vegetables.AddRange(player.vegetables);
                }

                info.buildings.AddRange(player.buildings);
            }
            return info;
        }

        public List<UserMoveInfo> MoveInfo(string userGuid)
        {
            var list = new List<UserMoveInfo>();

            foreach (var p in players.Values)
            {
                if (!p.IsOnline)
                {
                    continue;
                }
                
                var type = AvatarType.Friend;
                if (p.userInformation.UserGuid == userGuid)
                {
                    type = AvatarType.Self;
                }

                var info = new UserMoveInfo();
                info.type = type;
                info.UserGuid = p.userInformation.UserGuid;
                info.Pos = p.userInformation.Pos;
                info.dir = p.userInformation.dir;
                list.Add(info);
            }

            foreach (var p in Npcs.Values)
            {
                var type = AvatarType.Friend;
                if (p.npcInformation.ownerGuid == userGuid)
                {
                    type = AvatarType.Self;
                }

                var info = new UserMoveInfo();
                info.type = type;
                info.UserGuid = p.npcInformation.NPCGuid;
                info.NPCTableID = p.npcInformation.NPCTableID;
                info.Pos = p.npcInformation.Pos;
                info.dir = p.npcInformation.dir;
                list.Add(info);
            }

            return list;
        }

        public void AddNpc(NPCInformation info)
        {
            var npc = new Npc()
            {
                npcInformation = info
            };
            Npcs[info.NPCGuid] = npc;

            NtfSceneInfo();
        }

        public bool CheckInFence(Pos pos)
        {
            foreach (var player in players.Values)
            {
                if (player.CheckInFence(pos))
                {
                    return true;
                }
            }
            return false;
        }

        public async Task PlayerIntoScene(string userGuid, string userName, string userTableID, string userNetUUID)
        {
            var player = players.GetValueOrDefault(userGuid, null);
            if (player == null)
            {
                player = new Player(userGuid);
                player.userInformation.UserName = userName;
                player.userInformation.UserTableID = userTableID;
                player.userInformation.Pos = new();
                player.userInformation.Avatar = "";

                do
                {
                    var y = RandomHelper.RandomInt(Length);
                    var x = RandomHelper.RandomInt(Width);
                    player.userInformation.Pos.X = 0; // x;
                    player.userInformation.Pos.Y = 0; // y;
                } while (CheckInFence(player.userInformation.Pos));

                player.SetDBProxy(dbProxy);
                players.Add(userGuid, player);
            }
            player.NetUUID = userNetUUID;
            player.userInformation.SceneID = sceneID;
            netUUIDPlayers.Add(userNetUUID, player);

            NtfSceneInfo();

            await player.Init();
            await Server.RedisHandle.SetStrData(RedisHelp.BuildPlayerSceneHubKey(userGuid), Hub.Hub.name);
        }

        public void PlayerLevelScene(string userNetUUID)
        {
            var player = netUUIDPlayers.GetValueOrDefault(userNetUUID, null);
            if (player != null)
            {
                Server.RedisHandle.DelData(RedisHelp.BuildPlayerSceneHubKey(player.userInformation.UserGuid));

                player.NetUUID = string.Empty;
                player.userInformation.dir = Direction.None;
                netUUIDPlayers.Remove(userNetUUID);

                SceneClientCaller.get_multicast([..netUUIDPlayers.Keys]).player_leave_scene(player.userInformation.UserGuid);
            }
        }

        public async void PlayerIntoScene(Player player)
        {
            netUUIDPlayers.Add(player.NetUUID, player);

            NtfSceneInfo();
            await Server.RedisHandle.SetStrData(RedisHelp.BuildPlayerSceneHubKey(player.userInformation.UserGuid), Hub.Hub.name);
        }

        public void NPCMove(string npcGUID, Pos pos, Direction dir)
        {
            Log.Log.trace("NPCMove ");

            var npc = npcs.GetValueOrDefault(npcGUID, null);
            if (npc != null)
            {
                npc.npcInformation.Pos = pos;
                npc.npcInformation.dir = dir;

                foreach (var item in netUUIDPlayers)
                {
                    var type = AvatarType.Friend;
                    if (npc.npcInformation.ownerGuid == item.Value.userInformation.UserGuid)
                    {
                        type = AvatarType.Self;
                    }

                    var info = new UserMoveInfo();
                    info.type = type;
                    info.UserGuid = npc.npcInformation.NPCGuid;
                    info.NPCTableID = npc.npcInformation.NPCTableID;
                    info.Pos = npc.npcInformation.Pos;
                    info.dir = npc.npcInformation.dir;

                    SceneClientCaller.get_multicast(new() { item.Key }).move(new List<UserMoveInfo> { info });
                }
                Log.Log.trace("PlayerMove move {0}", netUUIDPlayers.Keys.ToList().ToJson());
            }
        }

        public void PlayerMove(string userNetUUID, Pos pos, Direction dir)
        {
            Log.Log.trace("PlayerMove ");

            var player = netUUIDPlayers.GetValueOrDefault(userNetUUID, null);
            if (player != null)
            {
                player.userInformation.Pos = pos;
                player.userInformation.dir = dir;

                foreach(var item in netUUIDPlayers)
                {
                    var type = AvatarType.Friend;
                    if (player.userInformation.UserGuid == item.Value.userInformation.UserGuid)
                    {
                        type = AvatarType.Self;
                    }

                    var info = new UserMoveInfo();
                    info.type = type;
                    info.UserGuid = player.userInformation.UserGuid;
                    info.Pos = player.userInformation.Pos;
                    info.dir = player.userInformation.dir;

                    SceneClientCaller.get_multicast(new() { item.Key }).move(new List<UserMoveInfo> { info });
                }
                Log.Log.trace("PlayerMove move {0}", netUUIDPlayers.Keys.ToList().ToJson());
            }
            else
            {
                foreach (var subScene in subScenes.Values)
                {
                    foreach (var (netUUID, player1) in subScene.NetUUIDPlayers)
                    {
                        if (netUUID == userNetUUID)
                        {
                            subScene.SyncMove(player1, pos, dir);
                            return;
                        }
                    }
                }
            }
        }

        public Fence PlayerRemoveBuilding(string userNetUUID, List<Building> buildings, List<Cultivated> cultivateds, bool rmfences)
        {
            Fence rm_fence = null;

            var player = netUUIDPlayers.GetValueOrDefault(userNetUUID, null);
            if (player == null)
            {
                return null;
            }
            
            if (buildings == null)
            {
                return null;
            }

            foreach (var building in buildings)
            {
                foreach (var i in player.buildings)
                {
                    if (building.BuildingGuid == i.BuildingGuid)
                    {
                        player.buildings.Remove(i);

                        var query = new DBQueryHelper();
                        query.condition("data_type", "UserBuilding");
                        query.condition("SceneID", sceneID);
                        query.condition("BuildingGuid", building.BuildingGuid);
                        dbProxy.getCollection("aiStarve", "game").removeObject(query.query(), (ret) => {
                            Log.Log.info($"Scene PlayerRemoveBuilding UserBuilding removeObject result:{ret}");
                        });

                        break;
                    }
                }
            }

            foreach (var cultivated in cultivateds)
            {
                foreach (var i in player.cultivateds)
                {
                    if (cultivated.CultivatedGuid == i.CultivatedGuid)
                    {
                        player.cultivateds.Remove(i);

                        var query = new DBQueryHelper();
                        query.condition("data_type", "UserCultivated");
                        query.condition("SceneID", sceneID);
                        query.condition("CultivatedGuid", cultivated.CultivatedGuid);
                        dbProxy.getCollection("aiStarve", "game").removeObject(query.query(), (ret) => {
                            Log.Log.info($"Scene PlayerRemoveBuilding UserCultivated removeObject result:{ret}");
                        });

                        break;
                    }
                }
            }

            if (rmfences)
            {
                rm_fence = player.fences;
                player.fences = null;
                var query = new DBQueryHelper();
                query.condition("data_type", "UserFence");
                query.condition("SceneID", sceneID);
                query.condition("FenceGuid", player.fences.FenceGuid);
                dbProxy.getCollection("aiStarve", "game").removeObject(query.query(), (ret) => {
                    Log.Log.info($"Scene PlayerRemoveBuilding UserFence removeObject result:{ret}");
                });
            }

            return rm_fence;
        }

        public int ImportRole(string userNetUUID, Role role)
        {
            var player = netUUIDPlayers.GetValueOrDefault(userNetUUID, null);
            if (player != null)
            {
                foreach (var r in player.roles)
                {
                    if (r.RoleId == role.RoleId)
                    {
                        return (int)em_error.role_is_import;
                    }
                }
            }
            player.roles.Add(role);
            return 0;
        }

        public async Task<int> StealVegetable(string userNetUUID, Vegetable vegetable)
        {
            var player = players.GetValueOrDefault(vegetable.UserGuid, null);
            if (player != null)
            {
                if (player.roles.Count > 0)
                {
                    return (int)em_error.have_role_protect;
                }

                foreach (var v in player.vegetables)
                {
                    if (v.VegetableGuid == vegetable.VegetableGuid)
                    {
                        v.count -= 3;
                        if (v.count <= 0)
                        {
                            foreach (var c in player.cultivateds)
                            {
                                if (c.VegetableGuid == v.VegetableGuid)
                                {
                                    c.VegetableGuid = string.Empty;
                                    break;
                                }
                            }
                            player.vegetables.Remove(v);
                        }

                        var stealer = netUUIDPlayers.GetValueOrDefault(userNetUUID, null);
                        var avatar = await Server.avatarMgr.load_or_create(stealer.userInformation.UserGuid, userNetUUID);
                        if (avatar != null)
                        {
                            var item = new item_def()
                            { 
                                uuid = v.VegetableGuid,
                                desc_id = v.perfabId,
                                perfab_id = v.perfabId,
                                subtype = v.subtype,
                                limit_amount = 999,
                                amount = 3,
                            };
                            avatar.get_real_hosting_data<bag.Bag>().Data.add_item(ref item);
                        }
                        return (int)em_error.success;
                    }
                }
            }
            return (int)em_error.db_error;
        }

        public async Task<int> SalesVegetable(string userNetUUID, Vegetable vegetable, int coin)
        {
            var player = netUUIDPlayers.GetValueOrDefault(userNetUUID, null);
            if (player != null)
            {
                foreach (var v in player.vegetables)
                {
                    if (v.VegetableGuid == vegetable.VegetableGuid)
                    {
                        v.count -= vegetable.count;
                        if (v.count <= 0)
                        {
                            foreach (var c in player.cultivateds)
                            {
                                if (c.VegetableGuid == v.VegetableGuid)
                                {
                                    c.VegetableGuid = string.Empty;
                                    break;
                                }
                            }
                            player.vegetables.Remove(v);
                        }
                        var avatar = await Server.avatarMgr.load(player.userInformation.UserGuid);
                        if (avatar != null)
                        {
                            player.userInformation.coin += coin;
                        }
                        return player.userInformation.coin;
                    }
                }
            }
            return -1;
        }

        public async Task<int> HarvestVegetable(string userNetUUID, Vegetable vegetable)
        {
            var player = netUUIDPlayers.GetValueOrDefault(userNetUUID, null);
            if (player != null)
            {
                foreach (var v in player.vegetables)
                {
                    if (v.VegetableGuid == vegetable.VegetableGuid)
                    {
                        player.ripe_vegetables.Add(v);
                        player.vegetables.Remove(v);

                        foreach (var c in player.cultivateds)
                        {
                            if (c.VegetableGuid == v.VegetableGuid)
                            {
                                c.VegetableGuid = string.Empty;
                                break;
                            }
                        }

                        var avatar = await Server.avatarMgr.load_or_create(player.userInformation.UserGuid, userNetUUID);
                        if (avatar != null)
                        {
                            var item = new item_def()
                            {
                                uuid = v.VegetableGuid,
                                desc_id = v.perfabId,
                                perfab_id = v.perfabId,
                                subtype = v.subtype,
                                limit_amount = 999,
                                amount = 3,
                            };
                            avatar.get_real_hosting_data<bag.Bag>().Data.add_item(ref item);
                        }

                        return (int)em_error.success;
                    }
                }
            }
            return (int)em_error.db_error;
        }

        public async Task<int> CutVegetable(string userNetUUID, Vegetable vegetable)
        {
            var player = netUUIDPlayers.GetValueOrDefault(userNetUUID, null);
            if (player != null)
            {
                foreach (var v in player.vegetables)
                {
                    if (v.VegetableGuid == vegetable.VegetableGuid)
                    {
                        player.ripe_vegetables.Add(v);
                        player.vegetables.Remove(v);

                        foreach (var c in player.cultivateds)
                        {
                            if (c.VegetableGuid == v.VegetableGuid)
                            {
                                c.VegetableGuid = string.Empty;
                                break;
                            }
                        }
                        return (int)em_error.success;
                    }
                }
            }
            return (int)em_error.db_error;
        }

        public Tuple<int, List<Vegetable>> GrowVegetable(string userNetUUID, Vegetable vegetable, Cultivated cultivated)
        {
            Log.Log.trace("GrowVegetable userNetUUID:{0} vegetable:{1} cultivated:{2}", userNetUUID, vegetable.ToJson(), cultivated.ToJson());

            var isNew = true;
            var player = netUUIDPlayers.GetValueOrDefault(userNetUUID, null);
            if (player != null)
            {
                foreach (var v in player.vegetables)
                {
                    if (v.VegetableGuid == vegetable.VegetableGuid)
                    {
                        isNew = false;
                        break;
                    }
                }
                if (isNew)
                {
                    vegetable.UserGuid = player.userInformation.UserGuid;
                    vegetable.VegetableGuid = Guid.NewGuid().ToString();
                    vegetable.growiTime = Server.timeInfoMgr.TimeInfo();
                    vegetable.SceneID = sceneID;
                }

                foreach (var c in player.cultivateds)
                {
                    if (c.CultivatedGuid == cultivated.CultivatedGuid)
                    {
                        if (!string.IsNullOrEmpty(c.VegetableGuid))
                        {
                            return Tuple.Create((int)em_error.cultivated_is_grow, player.vegetables);
                        }
                        else
                        {
                            if (isNew)
                            {
                                c.VegetableGuid = vegetable.VegetableGuid;

                                var query = new DBQueryHelper();
                                query.condition("data_type", "UserCultivated");
                                query.condition("SceneID", sceneID);
                                query.condition("UserGuid", player.userInformation.UserGuid);
                                query.condition("CultivatedGuid", c.CultivatedGuid);
                                var data = new UpdateDataHelper();
                                data.set(c);
                                dbProxy.getCollection("aiStarve", "game").updataPersistedObject(query.query(), data.data(), true, (ret) =>
                                {
                                    Log.Log.info($"Scene PlayerBuilding createPersistedObject result:{ret}");
                                });
                            }
                            break;
                        }
                    }
                }

                if (isNew)
                {
                    player.vegetables.Add(vegetable);
                    dbProxy.getCollection("aiStarve", "game").createPersistedObject(vegetable.ToBsonDocument(), (ret) => {
                        Log.Log.info($"Scene PlayerBuilding createPersistedObject result:{ret}");
                    });
                }

                NtfSceneInfo();
            }
            return Tuple.Create((int)em_error.success, player.vegetables);
        }

        public void NtfSceneInfo()
        {
            foreach (var u in netUUIDPlayers.Values)
            {
                var info = SceneInfo(u.userInformation.UserGuid);
                SceneClientCaller.get_multicast([u.NetUUID]).scene_info(info);
            }
        }

        public List<Vegetable> GetSceneVegetable()
        {
            var vegetables = new List<Vegetable>();
            foreach (var player in players.Values)
            {
                vegetables.AddRange(player.vegetables);
            }
            return vegetables;
        }

        public List<Cultivated> GetSceneCultivated()
        {
            var cultivateds = new List<Cultivated>();
            foreach (var player in players.Values)
            {
                cultivateds.AddRange(player.cultivateds);
            }
            return cultivateds;
        }

        public int PlayerInitFence(string userNetUUID, Fence new_fence, string pwd)
        {
            Fence old_fence = null;
            var player = netUUIDPlayers.GetValueOrDefault(userNetUUID, null);
            if (player != null)
            {
                if (player.fences != null)
                {
                    return (int)em_error.fence_is_exist;
                }

                old_fence = player.fences;
                player.fences = new_fence;
                player.fences.pwd = pwd;
            }

            Scene.SceneClientCaller.get_multicast([..NetUUIDPlayers.Keys]).scene_change(new SceneChange
            {
                remove_buildings = new(),
                add_buildings = new(),
                old_fences = old_fence,
                new_fences = new_fence,
            });

            return (int)em_error.success;
        }

        public void PlayerBuilding(string userNetUUID, List<Building> buildings, ref List<Cultivated> cultivateds, Fence fences)
        {
            var player = netUUIDPlayers.GetValueOrDefault(userNetUUID, null);
            if (player == null)
            {
                return;
            }

            if (buildings != null)
            {
                foreach (var building in buildings)
                {
                    building.BuildingGuid = Guid.NewGuid().ToString();
                    building.UserGuid = player.userInformation.UserGuid;
                    building.SceneID = sceneID;
                    player.buildings.Add(building);

                    dbProxy.getCollection("aiStarve", "game").createPersistedObject(building.ToBsonDocument(), (ret) => {
                        Log.Log.info($"Scene PlayerBuilding createPersistedObject result:{ret}");
                    });
                }
            }

            if (cultivateds != null)
            {
                foreach (var cultivated in cultivateds)
                {
                    if (!player.checkCultivatedIsExit(cultivated))
                    {
                        continue;
                    }

                    cultivated.CultivatedGuid = Guid.NewGuid().ToString();
                    cultivated.UserGuid = player.userInformation.UserGuid;
                    cultivated.SceneID = sceneID;
                    player.cultivateds.Add(cultivated);

                    dbProxy.getCollection("aiStarve", "game").createPersistedObject(cultivated.ToBsonDocument(), (ret) =>
                    {
                        Log.Log.info($"Scene PlayerBuilding createPersistedObject result:{ret}");
                    });
                }
            }

            if (fences != null)
            {
                fences.FenceGuid = Guid.NewGuid().ToString();
                fences.UserGuid = player.userInformation.UserGuid;
                fences.SceneID = sceneID;
                player.fences = fences;

                dbProxy.getCollection("aiStarve", "game").createPersistedObject(fences.ToBsonDocument(), (ret) => {
                    Log.Log.info($"Scene PlayerBuilding createPersistedObject result:{ret}");
                });
            }
        }

        public void NtfTimeInfo(TimeInfo info)
        {
            SceneClientCaller.get_multicast([..netUUIDPlayers.Keys]).time_info(info);
            Log.Log.trace("Scene NtfTimeInfo info:{0}", info.ToJson());
        }
    }
}