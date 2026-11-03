using Abelkhan;
using avatar;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Server
{
    public class PlayerFromDBError : System.Exception
    {
        public string Error;
        public PlayerFromDBError(string err)
        {
            Error = err;
        }
    }

    public class Player
    {
        public UserInformation userInformation = null;
        public Fence fences = null;
        public List<Building> buildings = [];
        public List<Cultivated> cultivateds = [];
        public List<Role> roles = [];
        public List<Vegetable> vegetables = [];
        public List<Vegetable> ripe_vegetables = [];
        public List<Npc> npcs = [];

        private string netUUID = string.Empty;
        public string NetUUID
        {
            get {
                return netUUID;
            }

            set {
                netUUID = value;
            }
        }

        public bool IsOnline
        {
            get
            {
                return netUUID != string.Empty;
            }

            private set {}
        }

        private Hub.DBProxyProxy _dbProxy;

        public void SetDBProxy(Hub.DBProxyProxy dbProxy)
        {
            _dbProxy = dbProxy;
        }

        public Player(string userGuid)
        {
            userInformation = new UserInformation();
            userInformation.UserGuid = userGuid;
        }

        public async Task Init()
        {
            Server.avatarMgr.init_data_db_opt((opt) =>
            {
                opt.DBName = Constant.Constant.player_db_name;
                opt.DBCollection = Constant.Constant.player_db_collection;
                opt.GuidCollection = Constant.Constant.player_db_guid_collection;
                opt.StoreDelayTime = 10 * 60 * 1000;
            });
            var avatar = await Server.avatarMgr.load_or_create(userInformation.UserGuid, NetUUID);

            var bag = avatar.get_real_hosting_data<bag.Bag>();
            bag.Data.onUseItem += (string itemGuid, string agvr) =>
            {
                var it = bag.Data.get_item(itemGuid);
                if (it != null)
                {
                    switch (it.desc_id)
                    {
                        case 1001:
                            {
                                var cultivated = Newtonsoft.Json.JsonConvert.DeserializeObject<Cultivated>(agvr);
                                var scene = Server.SceneMgr.ClientScenes[NetUUID];
                                if (scene != null)
                                {
                                    var cultivateds = new List<Cultivated>() { cultivated };
                                    scene.PlayerBuilding(NetUUID, [], ref cultivateds, null);
                                    Scene.SceneClientCaller.get_multicast([.. scene.NetUUIDPlayers.Keys]).arable_scene_info(cultivateds, scene.GetSceneVegetable());
                                }
                            }
                            break;
                    }
                }
                return false;
            };


            if (bag.Data.Count() <= 0)
            {
                var item = new item_def()
                {
                    uuid = Guid.NewGuid().ToString(),
                    desc_id = 19,
                    perfab_id = 19,
                    type = 1,
                    subtype = 1,
                    limit_amount = 1,
                    amount = 1,
                };
                bag.Data.add_item(ref item);

                for (var i = 24; i < 40; i++)
                {
                    item = new item_def()
                    {
                        uuid = Guid.NewGuid().ToString(),
                        desc_id = i,
                        perfab_id = i,
                        type = 4,
                        subtype = 1,
                        limit_amount = 100,
                        amount = 50,
                    };
                    bag.Data.add_item(ref item);
                }
            }
        }

        static private Task<UserInformation> loadUserInformation(Hub.DBProxyProxy _dbProxy, string sceneID, string userGuid)
        {
            var _t = new TaskCompletionSource<UserInformation>();

            var query = new DBQueryHelper();
            query.condition("data_type", "UserInformation");
            query.condition("SceneID", sceneID);
            query.condition("UserGuid", userGuid);
            _dbProxy.getCollection("aiStarve", "game").getObjectInfo(query.query(), (value) => {
                if (value.Count > 1)
                {
                    throw new PlayerFromDBError($"repeated sceneID:{sceneID} userGuid:{userGuid}");
                }
                else if (value.Count == 1)
                {
                    var doc = value[0] as BsonDocument;

                    var _info = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<UserInformation>(doc);
                    _t.SetResult(_info);
                }
                else
                {
                    _t.SetResult(null);
                }
            }, ()=>{ });

            return _t.Task;
        }

        private Task loadFence(string SceneID, string userGuid)
        {
            var _t = new TaskCompletionSource();

            var query = new DBQueryHelper();
            query.condition("data_type", "UserFence");
            query.condition("SceneID", SceneID);
            query.condition("UserGuid", userGuid);
            _dbProxy.getCollection("aiStarve", "game").getObjectInfo(query.query(), (value) => {
                if (value.Count > 0)
                {
                    var _fence = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<Fence>(value[0] as BsonDocument);
                    fences = _fence;
                }
                else
                {
                }
            }, () => {
                _t.SetResult();
            });

            return _t.Task;
        }

        private Task loadBuilding(string SceneID, string userGuid)
        {
            Log.Log.trace($"loadBuilding SceneID:{SceneID} userGuid:{userGuid}");

            var _t = new TaskCompletionSource();

            var query = new DBQueryHelper();
            query.condition("data_type", "UserBuilding");
            query.condition("SceneID", SceneID);
            query.condition("UserGuid", userGuid);
            _dbProxy.getCollection("aiStarve", "game").getObjectInfo(query.query(), (value) => {
                Log.Log.trace("loadBuilding value:{0}", value);
                if (value.Count > 0)
                {
                    foreach (var doc in value)
                    {
                        var _building = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<Building>(doc as BsonDocument);
                        buildings.Add(_building);
                    }
                }
                else
                {
                }
            }, () => {
                _t.SetResult();
            });

            return _t.Task;
        }

        private Task loadCultivated(string SceneID, string userGuid)
        {
            Log.Log.trace($"loadCultivated SceneID:{SceneID} userGuid:{userGuid}");

            var _t = new TaskCompletionSource();

            var query = new DBQueryHelper();
            query.condition("data_type", "UserCultivated");
            query.condition("SceneID", SceneID);
            query.condition("UserGuid", userGuid);
            _dbProxy.getCollection("aiStarve", "game").getObjectInfo(query.query(), (value) => {
                Log.Log.trace("loadCultivated value:{0}", value);
                if (value.Count > 0)
                {
                    foreach (var doc in value)
                    {
                        var _cultivated = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<Cultivated>(doc as BsonDocument);
                        cultivateds.Add(_cultivated);
                    }
                }
                else
                {
                }
            }, () => {
                _t.SetResult();
            });

            return _t.Task;
        }

        private Task loadVegetable(string SceneID, string userGuid)
        {
            Log.Log.trace($"loadVegetable SceneID:{SceneID} userGuid:{userGuid}");

            var _t = new TaskCompletionSource();

            var query = new DBQueryHelper();
            query.condition("data_type", "UserVegetable");
            query.condition("SceneID", SceneID);
            query.condition("UserGuid", userGuid);
            _dbProxy.getCollection("aiStarve", "game").getObjectInfo(query.query(), (value) => {
                Log.Log.trace("loadVegetable value:{0}", value);
                if (value.Count > 0)
                {
                    foreach (var doc in value)
                    {
                        var _vegetable = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<Vegetable>(doc as BsonDocument);
                        vegetables.Add(_vegetable);
                    }
                }
                else
                {
                }
            }, () => {
                _t.SetResult();
            });

            return _t.Task;
        }

        public void SavePlayer()
        {
            var query = new DBQueryHelper();
            query.condition("data_type", "UserInformation");
            query.condition("SceneID", userInformation.SceneID);
            query.condition("UserGuid", userInformation.UserGuid);
            var data = new UpdateDataHelper();
            data.set(userInformation);
            _dbProxy.getCollection("aiStarve", "game").updataPersistedObject(query.query(), data.data(), true, (ret) =>
            {
                Log.Log.info($"SavePlayer result:{ret}");
            });
        }

        public static async Task<Player> LoadPlayer(Hub.DBProxyProxy _dbProxy, string sceneID, string userGuid)
        {
            Log.Log.trace("LoadPlayer begin userGuid:{0}", userGuid);

            var userInfo = await loadUserInformation(_dbProxy, sceneID, userGuid);
            if (userInfo == null)
            {
                Log.Log.trace("LoadPlayer end userInfo == null userGuid:{0}", userGuid);
                return null;
            }

            var player = new Player(userInfo.UserGuid);
            player._dbProxy = _dbProxy;
            player.userInformation = userInfo;

            await player.Init();
            await player.loadFence(sceneID, userGuid);
            await player.loadBuilding(sceneID, userGuid);
            await player.loadCultivated(sceneID, userGuid);
            await player.loadVegetable(sceneID, userGuid);

            Log.Log.trace("LoadPlayer end userGuid:{0}", userGuid);
            return player;
        }

        public bool checkCultivatedIsExit(Cultivated cultivated)
        { 
            foreach (var c in cultivateds)
            {
                if (c.bottomRight.X == cultivated.bottomRight.X && c.bottomRight.Y == cultivated.bottomRight.Y &&
                    c.topLeft.X == cultivated.topLeft.X && c.topLeft.Y == cultivated.topLeft.Y)
                {
                    return false;
                }

                if (c.bottomRight.X == cultivated.topLeft.X && c.bottomRight.Y == cultivated.topLeft.Y &&
                    c.topLeft.X == cultivated.bottomRight.X && c.topLeft.Y == cultivated.bottomRight.Y)
                {
                    return false;
                }
            }

            return true;
        }


        public bool CheckInFence(Pos pos)
        {
            if (fences == null)
            {
                return false;
            }

            float x_min = int.MaxValue, y_min = int.MaxValue, x_max = int.MinValue, y_max = int.MinValue;
            var f = fences;
            if (f.topLeft.X < x_min)
            {
                x_min = f.topLeft.X;
            }
            if (f.topLeft.X < x_min)
            {
                x_min = f.topLeft.X;
            }
            if (f.topLeft.Y < y_min)
            {
                y_min = f.topLeft.Y;
            }
            if (f.topLeft.Y < y_min)
            {
                y_min = f.topLeft.Y;
            }

            if (f.bottomRight.X > x_max)
            {
                x_max = f.bottomRight.X;
            }
            if (f.bottomRight.X > x_max)
            {
                x_max = f.bottomRight.X;
            }
            if (f.bottomRight.Y > y_max)
            {
                y_max = f.bottomRight.Y;
            }
            if (f.bottomRight.Y > y_max)
            {
                y_max = f.bottomRight.Y;
            }

            if (pos.X > x_min && pos.X < x_max &&
                pos.Y > y_min && pos.Y < y_max)
            {
                return true;
            }

            return false;
        }
    }
}