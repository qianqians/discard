using System;
using System.Collections;
using System.Collections.Generic;
using common;

namespace lobby
{
    class gm : imodule
    {
        public gm()
        {
            //set_pay_rate((Int64)0);
        }
        //公告
        public void notice(String ntf)
        {
            var client_uuid = hub.hub.gates.current_client_uuid;
            var _proxy = server.players.get_player_uuid(client_uuid);

            if ((string)_proxy.player_info["account_type"] != "pc")
            {
                return;
            }

            hub.hub.gates.call_global_client("gm", "notice", ntf);
        }

        //禁止游戏
        public void ban()
        {
            var client_uuid = hub.hub.gates.current_client_uuid;
            var _proxy = server.players.get_player_uuid(client_uuid);

            if ((string)_proxy.player_info["account_type"] != "pc")
            {
                return;
            }

            server.disable = true;

            hub.hub.hubs.call_hub("room1", "gm", "ban");
            hub.hub.hubs.call_hub("room2", "gm", "ban");
            hub.hub.hubs.call_hub("room3", "gm", "ban");
            hub.hub.hubs.call_hub("room4", "gm", "ban");
        }

        //送钻石.GMclient call
        public void give_diamond_someone(Int64 reg_key, Int64 diamond)
        {
            var client_uuid = hub.hub.gates.current_client_uuid;
            var _proxy = server.players.get_player_uuid(client_uuid);

            if ((string)_proxy.player_info["account_type"] != "pc")
            {
                return;
            }

            var _query = new Hashtable();
            _query["reg_key"] = reg_key;
            hub.hub.dbproxy.getCollection("test", "objects").getObjectInfo(_query,
                (ArrayList data_list) =>
                {
                    if (data_list != null && data_list.Count == 1)
                    {
                        Hashtable item = (Hashtable)data_list[0];                          
                        var _player = server.players.get_player_unionid((string)item["unionid"]);
                        if (_player != null)
                        {
                            _player.player_info["diamond"] = (Int64)_player.player_info["diamond"] + diamond;
                            _player.update_player_to_db_and_client(new List<string> { "diamond" });
                        }
                        else
                        {
                            item["diamond"] = (Int64)item["diamond"] + diamond;
                            hub.hub.dbproxy.getCollection("test", "objects").updataPersistedObject(_query, item, () => { });
                        }                        
                        hub.hub.gates.call_client(_proxy.uuid, "gm", "give_diamond", true);
                        log.log.operation(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "DiamondInc:{0}...{1}...{2}", (Int64)item["reg_key"], GameCommon.DiamondInc.Gm, diamond);
                    }
                    else
                    {
                        hub.hub.gates.call_client(_proxy.uuid, "gm", "give_diamond", false);
                        log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "error reg_key");
                    }
                },
                () =>
                {
                });          
        }

        //对某一时间段消费过钻石的玩家送钻石 GMclient call
        public void give_diamond_sometimes(Int64 start_time, Int64 end_time, Int64 diamond)
        {
            string client_uuid = hub.hub.gates.current_client_uuid;
            var proxy = server.players.get_player_uuid(client_uuid);

            if ((string)proxy.player_info["account_type"] != "pc")
            {
                return;
            }

            var _query = new Hashtable();
            _query.Add("object_type", "player");
            hub.hub.dbproxy.getCollection("test", "objects").getObjectInfo(_query,
                (ArrayList data_list) =>
                {
                    if (data_list != null && data_list.Count != 0)
                    {
                        ArrayList consume_time;
                        foreach (Hashtable item in data_list)
                        {
                            if (item.ContainsKey("consume_time"))
                            {
                                consume_time = (ArrayList)item["consume_time"];
                                if (consume_time != null && consume_time.Count != 0)
                                {
                                    foreach (var time in consume_time)
                                    {
                                        if ((Int64)time >= start_time && (Int64)time <= end_time)
                                        {
                                            var _proxy = server.players.get_player_unionid((string)item["unionid"]);

                                            if (_proxy != null)
                                            {
                                                _proxy.player_info["diamond"] = (Int64)_proxy.player_info["diamond"] + diamond;
                                                //hub.hub.gates.call_client(player.uuid, "gm", "give_diamond_sometimes", true);
                                                _proxy.update_player_to_db_and_client(new List<string> { "diamond" });
                                            }
                                            else
                                            {
                                                var query = new Hashtable();
                                                query["unionid"] = item["unionid"];
                                                item["diamond"] = (Int64)item["diamond"] + diamond;
                                                //hub.hub.gates.call_client(player.uuid, "gm", "give_diamond_sometimes", true);
                                                hub.hub.dbproxy.getCollection("test", "objects").updataPersistedObject(query, item, () => { });
                                            }
                                            log.log.operation(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "DiamondInc:{0}...{1}...{2}", (Int64)item["reg_key"], GameCommon.DiamondInc.Gm, diamond);
                                            break;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "赶紧去消费钻石吧骚年:{0}", (Int64)item["reg_key"]);
                            }
                        }
                        hub.hub.gates.call_client(proxy.uuid, "gm", "give_diamond", true);
                    }
                    else
                    {
                        hub.hub.gates.call_client(proxy.uuid, "gm", "give_diamond", false);
                    }
                },
                () =>
                {

                });
        }

        //解散房间. GMclient call
        public void disband_room(Int64 room_id)
        {
            string client_uuid = hub.hub.gates.current_client_uuid;
            var proxy = server.players.get_player_uuid(client_uuid);

            if ((string)proxy.player_info["account_type"] != "pc")
            {
                return;
            }

            hub.hub.hubs.call_hub("room1", "room", "server_disband_room", room_id, client_uuid);
            hub.hub.hubs.call_hub("room2", "room", "server_disband_room", room_id, client_uuid);
            hub.hub.hubs.call_hub("room3", "room", "server_disband_room", room_id, client_uuid);
            hub.hub.hubs.call_hub("room4", "room", "server_disband_room", room_id, client_uuid);
        }

        //统计在线房间
        public void census_inline_room()
        {
            string client_uuid = hub.hub.gates.current_client_uuid;
            var proxy = server.players.get_player_uuid(client_uuid);

            if ((string)proxy.player_info["account_type"] != "pc")
            {
                return;
            }

            hub.hub.hubs.call_hub("room1", "gm", "census_inline_room", client_uuid);
            hub.hub.hubs.call_hub("room2", "gm", "census_inline_room", client_uuid);
            hub.hub.hubs.call_hub("room3", "gm", "census_inline_room", client_uuid);
            hub.hub.hubs.call_hub("room4", "gm", "census_inline_room", client_uuid);
        }

        //每月送5-10名玩家钻石
        public void give_diamond_player_permonth(ArrayList player_regkey, Int64 diamond)
        {
            string client_uuid = hub.hub.gates.current_client_uuid;
            var proxy = server.players.get_player_uuid(client_uuid);

            if ((string)proxy.player_info["account_type"] != "pc")
            {
                return;
            }

            if (player_regkey != null && player_regkey.Count >= 5 && player_regkey.Count <= 10)
            {
                foreach (var item in player_regkey)
                {
                    if ((Int64)item < 10000000)
                    {
                        log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "wrong reg_key");
                        return;
                    }
                }
                foreach (var reg_key in player_regkey)
                {
                    var _query = new Hashtable();
                    _query["reg_key"] = reg_key;
                    hub.hub.dbproxy.getCollection("test", "objects").getObjectInfo(_query,
                    (ArrayList data_list) =>
                    {
                        if (data_list != null && data_list.Count == 1)
                        {
                            Hashtable item = (Hashtable)data_list[0];
                            var _proxy = server.players.get_player_unionid((string)item["unionid"]);
                            if (_proxy != null)
                            {
                                _proxy.player_info["diamond"] = (Int64)_proxy.player_info["diamond"] + diamond;
                                _proxy.update_player_to_db_and_client(new List<string> { "diamond" });
                            }
                            else
                            {
                                item["diamond"] = (Int64)item["diamond"] + diamond;
                                hub.hub.dbproxy.getCollection("test", "objects").updataPersistedObject(_query, item, () => { });
                            }
                        }
                        else
                        {
                            log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "error reg_key");
                        }
                    },
                    () =>
                    {

                    });
                }
            }
            else
            {
                log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "data error!");
            }
        }

        //给所有玩家送钻石
        public void give_diamond_all(Int64 diamond)
        {
            string client_uuid = hub.hub.gates.current_client_uuid;
            var proxy = server.players.get_player_uuid(client_uuid);

            if ((string)proxy.player_info["account_type"] != "pc")
            {
                return;
            }

            ArrayList players = new ArrayList();
            Hashtable query = new Hashtable();
            query.Add("object_type", "player");
            hub.hub.dbproxy.getCollection("test", "objects").getObjectInfo(query,
                (ArrayList data_list) =>
                {
                    players.AddRange(data_list);
                },
                () => 
                {
                    if (players != null && players.Count != 0)
                    {
                        for (int i = players.Count - 1; i >= 0; i--)
                        {
                            Hashtable item = (Hashtable)players[i];
                            var _proxy = server.players.get_player_unionid((string)item["unionid"]);
                            if (_proxy != null)
                            {
                                _proxy.player_info["diamond"] = (Int64)_proxy.player_info["diamond"] + diamond;
                                _proxy.update_player_to_db_and_client(new List<string> { "diamond" });
                            }
                            else
                            {
                                Hashtable _query = new Hashtable();
                                _query["unionid"] = (string)item["unionid"];
                                item["diamond"] = (Int64)item["diamond"] + diamond;
                                hub.hub.dbproxy.getCollection("test", "objects").updataPersistedObject(_query, item, () => { });
                            }
                            log.log.operation(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "DiamondInc:{0}...{1}...{2}", (Int64)item["reg_key"], GameCommon.DiamondInc.Gm, diamond);
                        }
                        hub.hub.gates.call_client(proxy.uuid, "gm", "give_diamond", true);
                    }
                    else
                    {
                        hub.hub.gates.call_client(proxy.uuid, "gm", "give_diamond", false);
                        log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "not find players!");
                    }
                    
                });
        }

        //设置充值比例
        public void set_pay_rate(Int64 _rate_index)
        {
            string client_uuid = hub.hub.gates.current_client_uuid;
            var proxy = server.players.get_player_uuid(client_uuid);

            if ((string)proxy.player_info["account_type"] != "pc")
            {
                return;
            }

            if (_rate_index >= 0 && _rate_index <= 3)
            {
                server.rate_index = (int)_rate_index;
                hub.hub.gates.call_global_client("gm", "set_pay_rate", (Int64)_rate_index);
            }
            else
            {
                log.log.error(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "data error!");
            }
        }

        //强制退出房间
        public void gm_exit_room(Int64 reg_key)
        {
            string client_uuid = hub.hub.gates.current_client_uuid;
            var proxy = server.players.get_player_uuid(client_uuid);

            if ((string)proxy.player_info["account_type"] != "pc")
            {
                return;
            }

            if (reg_key < 10000000)
            {
                hub.hub.gates.call_client(proxy.uuid, "gm", "gm_exit_room", false);
                return;
            }

            var _query = new Hashtable();
            _query["reg_key"] = reg_key;
            hub.hub.dbproxy.getCollection("test", "objects").getObjectInfo(_query,
                    (ArrayList data_list) =>
                    {
                        if (data_list != null && data_list.Count == 1)
                        {
                            Hashtable item = (Hashtable)data_list[0];
                            var _proxy = server.players.get_player_unionid((string)item["unionid"]);
                            if (_proxy != null)
                            {
                                _proxy.player_info["room_name"] = "";
                                _proxy.player_info["in_room"] = (Int64)0;
                                hub.hub.gates.call_client(proxy.uuid, "gm", "gm_exit_room", true);
                            }
                            else
                            {
                                hub.hub.gates.call_client(proxy.uuid, "gm", "gm_exit_room", false);
                            }                        
                        }
                        else
                        {
                            hub.hub.gates.call_client(proxy.uuid, "gm", "gm_exit_room", false);
                            log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "error reg_key");
                        }
                    },
                    () =>
                    {

                    });
        }

        //修改玩家信息某个字段
        public void update_player_info_field(Int64 reg_key, string key, string value)
        {
            string client_uuid = hub.hub.gates.current_client_uuid;
            var proxy = server.players.get_player_uuid(client_uuid);

            if ((string)proxy.player_info["account_type"] != "pc")
            {
                return;
            }

            if (reg_key < 10000000)
            {
                hub.hub.gates.call_client(proxy.uuid, "gm", "give_diamond", false);
                return;
            }

            var _query = new Hashtable();
            _query["reg_key"] = reg_key;
            hub.hub.dbproxy.getCollection("test", "objects").getObjectInfo(_query,
                    (ArrayList data_list) =>
                    {
                        if (data_list != null && data_list.Count == 1)
                        {
                            Hashtable item = (Hashtable)data_list[0];
                            object val = null;
                            if (item[key] is string)
                            {
                                val = value;
                            }
                            else if (item[key] is bool)
                            {
                                val = (bool)Json.Jsonparser.unpack(value);
                            }
                            else if (item[key] is Int64)
                            {
                                val = (Int64)Json.Jsonparser.unpack(value);
                            }
                            else if (item[key] is ArrayList)
                            {
                                val = (ArrayList)Json.Jsonparser.unpack(value);
                            }
                            else
                            {
                                log.log.error(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "error type");
                                return;
                            }

                            if (item.ContainsKey(key) && !string.IsNullOrEmpty(key))
                            {
                                var _proxy = server.players.get_player_unionid((string)item["unionid"]);
                                if (_proxy != null)
                                {
                                    _proxy.player_info[key] = val;
                                    _proxy.update_player_to_db_and_client(new List<string> { key });
                                }
                                else
                                {
                                    item[key] = val;
                                    hub.hub.dbproxy.getCollection("test", "objects").updataPersistedObject(_query, item, () => { });
                                }
                                hub.hub.gates.call_client(proxy.uuid, "gm", "update_player_info_attribute", true);
                            }
                            else
                            {
                                log.log.error(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "error key");
                            }
                        }
                        else
                        {
                            hub.hub.gates.call_client(proxy.uuid, "gm", "update_player_info_attribute", false);
                            log.log.error(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "error reg_key");
                        }
                    },
                    () =>
                    {

                    });
        }
    }
}
