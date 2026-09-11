using Abelkhan;
using MongoDB.Bson;
using MsgPack;
using MsgPack.Serialization;
using Service;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rank
{
    public class Rank
    {
        internal string name;
        internal int capacity;
        internal int total_capacity;
        internal long base_score;
        internal SortedList<long, rank_item> rankList = new();
        internal SortedDictionary<long, long> guidRank = new ();

        public Rank(int _capacity)
        {
            capacity = _capacity;
            total_capacity = _capacity + 200;
        }

        public BsonDocument ToBsonDocument()
        {
            var doc = new BsonDocument
            {
                { "name", name },
                { "capacity", capacity }
            };

            var docRankList = new BsonDocument();
            foreach (var it in rankList)
            {
                var docItem = new BsonDocument
                {
                    { "guid", it.Value.guid },
                    { "score", it.Value.score },
                    { "rank", it.Value.rank },
                    { "item", it.Value.item }
                };
                docRankList.Add(it.Key.ToString(), docItem);
            }
            doc.Add("rankList", docRankList);

            var docGuidRank = new BsonDocument();
            foreach(var it in guidRank)
            {
                docGuidRank.Add(it.Key.ToString(), it.Value);
            }
            doc.Add("guidRank", docGuidRank);

            return doc;
        }

        public int UpdateRankItem(rank_item item)
        {
            rank_item oldItem = null;
            if (guidRank.TryGetValue(item.guid, out var oldScore))
            {
                rankList.Remove(oldScore, out oldItem);
            }

            using var st1 = MemoryStreamPool.mstMgr.GetStream(item.item);
            var _serializer1 = MessagePackSerializer.Get<MessagePackObjectDictionary>();
            var it = UserRankInfo.protcol_to_UserRankInfo(_serializer1.Unpack(st1));
            Log.Log.trace("UpdateRankItem it:{0}", it.ToJson());

            if (oldItem == null && it.battle_data == null)
            {
                return -1;
            }

            var score = item.score << 32 | (int.MaxValue - Timerservice.Tick / 1000);
            if (score < base_score)
            {
                return -1;
            }

            if (oldItem != null)
            {
                using var st = MemoryStreamPool.mstMgr.GetStream(oldItem.item);
                var _serializer = MessagePackSerializer.Get<MessagePackObjectDictionary>();
                var oldIt = UserRankInfo.protcol_to_UserRankInfo(_serializer.Unpack(st));
                Log.Log.trace("UpdateRankItem oldIt:{0}", oldIt.ToJson());

                if (it.battle_data != null)
                {
                    it.battle_data.round += oldIt.battle_data.round;
                    it.battle_data.victory += oldIt.battle_data.victory;
                    it.battle_data.faild += oldIt.battle_data.faild;
                }
                else
                {
                    it.battle_data = oldIt.battle_data;
                }
                Log.Log.trace("UpdateRankItem battle_data it:{0}", it.ToJson());

                using var st2 = MemoryStreamPool.mstMgr.GetStream();
                var _serializer2 = MessagePackSerializer.Get<MessagePackObjectDictionary>();
                _serializer2.Pack(st2, UserRankInfo.UserRankInfo_to_protcol(it));
                item.item = st2.ToArray();
            }

            rankList.Add(score, item);
            guidRank[item.guid] = score;
            tidyRank();
            
            item.rank = rankList.IndexOfKey(score) + 1;
            return item.rank;
        }

        private void tidyRank()
        {
            if (rankList.Count > total_capacity)
            {
                var remove = new List<long>();
                var removeGuid = new List<long>();
                for (var i = total_capacity; i < rankList.Count; ++i)
                {
                    remove.Add(rankList.GetKeyAtIndex(i));
                    removeGuid.Add(rankList.GetValueAtIndex(i).guid);
                }
                foreach (var key in remove)
                {
                    rankList.Remove(key);
                }
                foreach (var guid in removeGuid)
                {
                    guidRank.Remove(guid);
                }
                base_score = rankList.GetKeyAtIndex(total_capacity - 1);
            }
        }

        public void ResetRank()
        {
            rankList.Clear();
            guidRank.Clear();

            base_score = 0;
        }

        public rank_item GetRankGuid(long guid)
        {
            Log.Log.trace("GetRankGuid guid:{0} rankDict:{1} {2}", guid, rankList.Count, rankList.Keys.ToJson());

            var rank = new rank_item
            {
                guid = guid
            };

            if (!guidRank.TryGetValue(guid, out var score))
            {
                rank.rank = 20011;
                rank.score = 0;
            }
            else
            {
                if (rankList.TryGetValue(score, out var r))
                {
                    rank.rank = rankList.IndexOfKey(score) + 1;
                    rank.score = score;
                    rank.item = r.item;
                }
            }

            Log.Log.trace("GetRankGuid rank:{0}", rank.ToJson());

            return rank;
        }

        public List<rank_item> GetRankRange(int start, int end)
        {
            var rank = new List<rank_item>();

            var r = start;
            var c = rankList.Count < capacity ? rankList.Count : capacity;
            end = end < c ? end : c;
            for (var i = end; i >= start - 1; i--)
            {
                if (rankList.Count <= i)
                {
                    continue;
                }
                var item = rankList.GetValueAtIndex(i);
                item.rank = r++;
                rank.Add(item);
            }

            return rank;
        }
    }

    public class RankModule
    {
        private static string dbName;
        private static string dbCollection;

        private static Dictionary<string, Rank> rankDict = new();

        private static rank_cli_service_module rank_Cli_Service_Module = new();
        private static rank_svr_service_module rank_svr_Service_Module = new();

        public static Task Init(string _dbName, string _dbCollection, List<string> rankNames)
        {
            var task = new TaskCompletionSource();

            rank_Cli_Service_Module.on_get_rank_guid += Rank_Cli_Service_Module_on_get_rank_guid;
            rank_Cli_Service_Module.on_get_rank_range += Rank_Cli_Service_Module_on_get_rank_range;

            rank_svr_Service_Module.on_get_rank_guid += Rank_svr_Service_Module_on_get_rank_guid;
            rank_svr_Service_Module.on_update_rank_item += Rank_svr_Service_Module_on_update_rank_item;

            dbName = _dbName;
            dbCollection = _dbCollection;

            Log.Log.trace($"rank getObjectInfo {_dbName}_{_dbCollection} {rankNames.ToJson()}");
            var query = new DBQueryHelper();
            query._in("name", new BsonArray(rankNames));
            Hub.Hub.get_random_dbproxyproxy().getCollection(dbName, dbCollection).getObjectInfo(query.query(), (array) =>
            {
                Log.Log.info($"load rank getObjectInfo count:{array.Count}");
                foreach (var rankDoc in array)
                {
                    var rank = new Rank(rankDoc["capacity"].AsInt32);
                    rank.name = rankDoc["name"].AsString;

                    foreach (var it in rankDoc["rankList"].AsBsonDocument)
                    {
                        var item = new rank_item();
                        item.guid = it.Value["guid"].AsInt64;
                        item.score = it.Value["score"].AsInt64;
                        item.rank = it.Value["rank"].AsInt32;
                        item.item = it.Value["item"].AsBsonBinaryData.Bytes;
                        rank.rankList.Add(long.Parse(it.Name), item);
                    }

                    foreach(var it in rankDoc["guidRank"].AsBsonDocument)
                    {
                        rank.guidRank.Add(long.Parse(it.Name), it.Value.AsInt64);
                    }

                    rankDict.Add(rank.name, rank);
                    Log.Log.info($"load rank success:{rank.name} {rank.capacity} {rank.rankList.Count}");
                }
                task.SetResult();
            });

            Hub.Hub._timer.addticktime(60 * 60 * 1000, tick_save_rank);

            return task.Task;
        }

        public static Rank get_rank(string name)
        {
            rankDict.TryGetValue(name, out var rank);
            return rank;
        }

        public static void save_rank()
        {
            try
            {
                foreach (var item in rankDict.Values)
                {
                    var query = new DBQueryHelper();
                    query.condition("name", item.name);
                    var doc = item.ToBsonDocument();
                    var update = new UpdateDataHelper();
                    update.set(doc);
                    Hub.Hub.get_random_dbproxyproxy().getCollection(dbName, dbCollection).updataPersistedObject(query.query(), update.data(), true, (result) =>
                    {
                        if (result != Hub.DBProxyProxy.EM_DB_RESULT.EM_DB_SUCESSED)
                        {
                            Log.Log.err($"tick_save_rank item.name faild:{result}");
                        }
                    });
                }
            }
            catch (System.Exception ex)
            {
                Log.Log.err($"tick_save_rank:{ex}");
            }
        }

        private static void tick_save_rank(long tick)
        {
            save_rank();
            Hub.Hub._timer.addticktime(60 * 60 * 1000, tick_save_rank);
        }

        public static void reset_rank(string rankName)
        {
            if (rankDict.TryGetValue(rankName, out var rankIns))
            {
                rankIns.ResetRank();
            }
        }

        private static void Rank_svr_Service_Module_on_update_rank_item(string rankNmae, rank_item item)
        {
            var rsp = rank_svr_Service_Module.rsp as rank_svr_service_update_rank_item_rsp;

            try
            {
                if (rankDict.TryGetValue(rankNmae, out var rankIns))
                {
                    var rank = rankIns.UpdateRankItem(item);
                    rsp.rsp(rank);
                }
                else
                {
                    rankIns = new Rank(100)
                    {
                        name = rankNmae
                    };
                    rankDict.Add(rankNmae, rankIns);
                    var rank = rankIns.UpdateRankItem(item);
                    rsp.rsp(rank);
                }
                Log.Log.trace("on_update_rank_item rankDict:{0} {1}", rankDict.Count, rankDict.ToJson());
            }
            catch(System.Exception ex)
            {
                Log.Log.err("on_update_rank_item:{0}", ex);
                rsp.err();
            }
        }

        private static void Rank_svr_Service_Module_on_get_rank_guid(string rankNmae, long guid)
        {
            var rsp = rank_svr_Service_Module.rsp as rank_svr_service_get_rank_guid_rsp;

            try
            {
                if (rankDict.TryGetValue(rankNmae, out var rankIns))
                {
                    var rank = rankIns.GetRankGuid(guid);
                    rsp.rsp(rank.rank);
                }
                else
                {
                    rsp.err();
                }
            }
            catch (System.Exception ex)
            {
                Log.Log.err("on_get_rank_guid:{0}", ex);
            }
        }

        private static void Rank_Cli_Service_Module_on_get_rank_range(string rankNmae, int start, int end)
        {
            var rsp = rank_Cli_Service_Module.rsp as rank_cli_service_get_rank_range_rsp;

            try
            {
                Log.Log.trace("on_get_rank_range rankDict:{0} {1}", rankDict.Count, rankDict.ToJson());
                if (rankDict.TryGetValue(rankNmae, out var rankIns))
                {
                    var rank = rankIns.GetRankRange(start, end);
                    rsp.rsp(rank);
                }
                else
                {
                    rsp.err();
                }
            }
            catch (System.Exception ex)
            {
                Log.Log.err("on_get_rank_range:{0}", ex);
            }
        }

        private static void Rank_Cli_Service_Module_on_get_rank_guid(string rankNmae, long guid)
        {
            var rsp = rank_Cli_Service_Module.rsp as rank_cli_service_get_rank_guid_rsp;

            try
            {
                Log.Log.trace("on_get_rank_guid rankDict:{0} {1}", rankDict.Count, rankDict.Keys.ToJson());
                if (rankDict.TryGetValue(rankNmae, out var rankIns))
                {
                    var rank = rankIns.GetRankGuid(guid);
                    rsp.rsp(rank);
                }
                else
                {
                    rsp.err();
                }
            }
            catch (System.Exception ex)
            {
                Log.Log.err("on_get_rank_guid:{0}", ex);
            }
        }
    }
}
