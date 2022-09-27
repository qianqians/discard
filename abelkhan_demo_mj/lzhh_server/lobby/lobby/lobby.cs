using System;
using System.Collections.Generic;
using System.Collections;
using common;

namespace lobby
{
    class lobby : imodule
    {
        public lobby()
        {
            could_create_room_callback = new Dictionary<string, create_mj_huanghuang_room_real_handle>();
            create_room_real_callback = new Dictionary<string, create_mj_huanghuang_room_callback_client_handle>();
        }
        
        // client call
        public void create_mj_huanghuang_room(Int64 peopleNum, Int64 score, Int64 times, Int64 payRule)
        {
            var client_uuid = hub.hub.gates.current_client_uuid;

            if (server.disable)
            {
                hub.hub.gates.call_client(client_uuid, "room", "disable_game");

                return;
            }

            var _proxy = server.players.get_player_uuid(client_uuid);
            if (_proxy != null)
            {
                createroomimpl.create_mj_huanghuang_room((string)_proxy.player_info["unionid"], peopleNum, score, times, payRule, 
                    (string hub_name, Int64 room_id)=> { create_mj_huanghuang_room_callback_client(_proxy, hub_name, room_id, peopleNum, score, times, payRule); });
            }
            else
            {
                log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "not exit player:{0}", client_uuid);
            }
        }

        void create_mj_huanghuang_room_callback_client(playerproxy _proxy, string hub_name, Int64 room_id, Int64 peopleNum, Int64 gameScore, Int64 gameTimes, Int64 payRule)
        {
            log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "creat_room_callback_client,hub_name:{0}, room_id:{1}", hub_name, room_id);
            hub.hub.gates.call_client(_proxy.uuid, "room", "on_create_mj_huanghuang_room", hub_name, room_id);

            _proxy.room_list.Add(room_id.ToString(), new Hashtable() { { "gameScore", gameScore }, { "gameTimes", gameTimes }, { "payRule", payRule }, { "peopleNum", peopleNum }, { "playerNum", (Int64)0 }, { "inGame", false } });
            hub.hub.gates.call_client(_proxy.uuid, "room", "room_list", _proxy.room_list);
        }

        // room hub call
        public void on_could_create_mj_huanghuang_room(bool coulded, string callback_id, string hub_name)
        {
            if (coulded)
            {
                if (could_create_room_callback.ContainsKey(callback_id))
                {
                    could_create_room_callback[callback_id](hub_name);
                    could_create_room_callback.Remove(callback_id);
                }
            }
        }

        // room hub call
        public void on_create_mj_huanghuang_room_real(Int64 room_id, string callback_id)
        {
            if (create_room_real_callback.ContainsKey(callback_id))
            {
                create_room_real_callback[callback_id](room_id);
                create_room_real_callback.Remove(callback_id);
            }
        }

        //room hub call
        public void get_victory_count(string unionid)
        {
            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "get_victory_count :{0}", unionid);
            var _proxy = server.players.get_player_unionid(unionid);
            if (_proxy == null)
            {
                log.log.error(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "wrong unionid:{0}", unionid);
                hub.hub.gates.disconnect_client(_proxy.uuid);
                return;
            }

            if (_proxy.player_info["task_victory_count"] == null)
            {
                _proxy.player_info["task_victory_count"] = (Int64)0;
            }
            _proxy.player_info["task_victory_count"] = (Int64)_proxy.player_info["task_victory_count"] + 1;
            _proxy.update_player_to_db_and_client(new List<string> { "task_victory_count" });
            log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "task_victory_count:{0}", _proxy.player_info["task_victory_count"]);
        }

        //room hub call
        public void get_game_count(string unionid)
        {
            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "get_game_count :{0}", unionid);
            var _proxy = server.players.get_player_unionid(unionid);
            if (_proxy == null)
            {
                log.log.error(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "wrong unionid:{0}", unionid);
                hub.hub.gates.disconnect_client(_proxy.uuid);
                return;
            }

            if (_proxy.player_info["task_game_count"] == null)
            {
                _proxy.player_info["task_game_count"] = (Int64)0;
            }
            _proxy.player_info["task_game_count"] = (Int64)_proxy.player_info["task_game_count"] + 1;
            _proxy.update_player_to_db_and_client(new List<string> { "task_game_count" });
            log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "task_game_count:{0}", _proxy.player_info["task_game_count"]);
        }

        public void get_player_info_enter_room(string hub_name, string client_uuid, Int64 room_id, bool is_renconnect)
        {
            var _proxy = server.players.get_player_uuid(client_uuid);
            if (_proxy != null)
            {
                hub.hub.hubs.call_hub(hub_name, "room", "player_info_enter_room", room_id, _proxy.uuid, _proxy.player_info["unionid"], _proxy.player_info["reg_key"], _proxy.nickname, _proxy.headimg, _proxy.sex, _proxy.player_info["rank_score"], is_renconnect);
            }
        }

        public void enter_room(string client_uuid, string hub_name, Int64 room_id)
        {
            var _proxy = server.players.get_player_uuid(client_uuid);
            if (_proxy != null)
            {
                _proxy.tmp_player_info["room_name"] = hub_name;
                _proxy.tmp_player_info["in_room"] = room_id;
            }
        }

        public void exit_room(string client_uuid)
        {
            var _proxy = server.players.get_player_uuid(client_uuid);
            if (_proxy != null)
            {
                _proxy.tmp_player_info["room_name"] = "";
                _proxy.tmp_player_info["in_room"] = (Int64)0;
            }
        }
        
        public void occupat_site(string unionid, Int64 room_id)
        {
            var _proxy = server.players.get_player_unionid(unionid);
            if (_proxy == null)
            {
                return;
            }

            var roominfo = _proxy.room_list[room_id.ToString()] as Hashtable;
            roominfo["playerNum"] = (Int64)roominfo["playerNum"] + 1;
            hub.hub.gates.call_client(_proxy.uuid, "room", "room_list", _proxy.room_list);
        }

        public void exit_table(string unionid, Int64 room_id)
        {
            var _proxy = server.players.get_player_unionid(unionid);
            if (_proxy == null)
            {
                return;
            }

            var roominfo = _proxy.room_list[room_id.ToString()] as Hashtable;
            roominfo["playerNum"] = (Int64)roominfo["playerNum"] - 1;
            hub.hub.gates.call_client(_proxy.uuid, "room", "room_list", _proxy.room_list);
        }

        public void begin_game(string unionid, Int64 room_id)
        {
            var _proxy = server.players.get_player_unionid(unionid);
            if (_proxy == null)
            {
                return;
            }

            var roominfo = _proxy.room_list[room_id.ToString()] as Hashtable;
            roominfo["inGame"] = true;
            hub.hub.gates.call_client(_proxy.uuid, "room", "room_list", _proxy.room_list);
        }

        public void disband_room(string unionid, Int64 room_id)
        {
            var _proxy = server.players.get_player_unionid(unionid);
            if (_proxy == null)
            {
                return;
            }

            _proxy.room_list.Remove(room_id.ToString());
            hub.hub.gates.call_client(_proxy.uuid, "room", "room_list", _proxy.room_list);
        }
        
        public void on_pay_diamond(System.String unionid, System.Int64 times, System.Int64 payRule, System.Int64 peopleNum)
        {
            log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "on_pay_diamond times:{0}, payRule:{1}, peopleNum:{2}", times, payRule, peopleNum);
            //扣费 预留
            for (int i = 0; i < meter.room_configs.GetInstance().tables.Count; i++)
            {
                if (meter.room_configs.GetInstance().tables[i].times == times &&
                    meter.room_configs.GetInstance().tables[i].payRule == payRule &&
                    meter.room_configs.GetInstance().tables[i].playerNum == peopleNum)
                {
                    log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "on_pay_diamond 1");

                    var _proxy = server.players.get_player_unionid(unionid);
                    log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "on_pay_diamond 3, {0}", _proxy.player_info["diamond"].GetType());
                    _proxy.player_info["diamond"] = (Int64)_proxy.player_info["diamond"] - meter.room_configs.GetInstance().tables[i].pay;
                    if (_proxy.player_info.ContainsKey("consume_time"))
                    {
                        ArrayList consume_time = (ArrayList)_proxy.player_info["consume_time"];
                        consume_time.Add((Int64)service.timerservice.Tick);
                    }
                    else
                    {
                        ArrayList consume_time = new ArrayList();
                        consume_time.Add((Int64)service.timerservice.Tick);
                        _proxy.player_info.Add("consume_time", (ArrayList)consume_time);
                    }
                    _proxy.update_player_to_db_and_client(new List<string> { "diamond", "consume_time" });

                    log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "on_pay_diamond 2");
                }
            }
        }

        public delegate void create_mj_huanghuang_room_real_handle(string hub_name);
        public delegate void create_mj_huanghuang_room_callback_client_handle(Int64 room_id);

        static public Dictionary<string, create_mj_huanghuang_room_real_handle> could_create_room_callback;
        static public Dictionary<string, create_mj_huanghuang_room_callback_client_handle> create_room_real_callback;
    }
}
