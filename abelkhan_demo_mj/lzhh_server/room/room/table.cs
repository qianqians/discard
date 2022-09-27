using System;
using System.Collections;
using System.Collections.Generic;
using common;

namespace room 
{
    class table
    {
        public table()
        {
            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "begin table");

            players = new Dictionary<string, playerproxy>();
            players_uuid = new Dictionary<string, playerproxy>();
            site = new Dictionary<Int64, playerproxy>();
            matcher = new List<playerproxy>();

            play_count = 0;
            in_game = false;

            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "end table");
        }

        public Hashtable get_room_info()
        {
            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "begin get_room_info");

            Hashtable info = new Hashtable();
            info.Add("in_game", in_game);
            info.Add("play_count", play_count);
            info.Add("is_robot_room", is_robot_room);
            info.Add("room_id", room_id);
            info.Add("peopleNum", peopleNum);
            info.Add("score", score);
            info.Add("times", times);
            info.Add("payRule", payRule);

            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "end get_room_info");

            return info;
        }

        public void init(Int64 _room_id)
        {
            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "begin init");

            room_id = _room_id;

            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "end init");
        }

        public void clean()
        {
            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "begin clean");

            players.Clear();
            players_uuid.Clear();
            site.Clear();
            matcher.Clear();

            owner = "";
            play_count = 0;
            in_game = false;

            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "end clean");
        }

        public void set_room_info(Int64 _peopleNum, Int64 _score, Int64 _times, Int64 _payRule)
        {
            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "begin set_room_info");

            peopleNum = _peopleNum;
            score = _score;
            times = _times;
            payRule = _payRule;

            is_robot_room = false;

            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "end set_room_info");
        }

        public void set_room_owner(string unionid)
        {
            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "begin set_room_owner");

            owner = unionid;

            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "end set_room_owner");
        }

        public void rejoin_table(Int64 room_id, string client_uuid, string unionid, Int64 game_id, string nickname, string headimg, Int64 sex, Int64 rank_score)
        {
            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "begin rejoin_table");

            if (!players.ContainsKey(unionid))
            {
                hub.hub.gates.call_client(client_uuid, "room", "player_not_in_room");
            }

            Int64 time = service.timerservice.Tick;
            broadcast("mj_huanghuang", "player_reconnect", unionid);

            log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "player rejoin table1");

            playerproxy _proxy = players[unionid];
            string old_uuid = _proxy.relogin(client_uuid);
            server.players.reg_proxy(client_uuid, _proxy);

            ArrayList _room_info = new ArrayList();
            foreach (var item in players)
            {
                _room_info.Add(item.Value.player_info);
            }

            log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "player rejoin table2");

            if (players_uuid.ContainsKey(old_uuid))
            {
                players_uuid.Remove(old_uuid);
            }
            players_uuid.Add(client_uuid, _proxy);

            log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "player rejoin table3");

            _proxy.room_id = room_id;

            log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "player rejoin table");
            hub.hub.gates.call_client(client_uuid, "room", "on_enter_mj_huanghuang_room", _room_info, peopleNum, score, times, payRule, time);

            var _room_mj_info = ((mj_huanghuang_table)this).get_room_info();
            hub.hub.gates.call_client(client_uuid, "room", "mj_huanghuang_room_info", _room_mj_info);

            if (((mj_huanghuang_table)this).in_game)
            {
                if (((mj_huanghuang_table)this).card_righter == (Int64)_proxy.player_info["site"] && ((mj_huanghuang_table)this).mopai_state)
                {
                    hub.hub.gates.call_client(client_uuid, "room", "player_mopai", ((mj_huanghuang_table)this).mopai);
                }
                hub.hub.gates.call_client(client_uuid, "room", "player_cards", ((mj_huanghuang_table)this).player_cards[(Int64)_proxy.player_info["site"]]);
            }

            if (((mj_huanghuang_table)this).voting)
            {
                hub.hub.gates.call_client(client_uuid, "room", "req_disband_vote");
                Hashtable vote_info = new Hashtable();
                foreach (var p in ((mj_huanghuang_table)this).players)
                {
                    vote_info.Add(p.Value.player_info["unionid"], (Int64)p.Value.disband);
                }
                hub.hub.gates.call_client(client_uuid, "room", "vote_disband_room_player_state", vote_info);
            }

            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "end rejoin_table");
        }

        public void join_table(Int64 room_id, string client_uuid, string unionid, Int64 game_id, string nickname, string headimg, Int64 sex, Int64 rank_score)
        {
            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "begin join_table");

            log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "player join table {0}", unionid);
            if (is_free)
            {
                log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "please creat table");
                hub.hub.gates.call_client(client_uuid, "room", "exist_room");
                return;
            }

            Int64 time = service.timerservice.Tick;
            if (players.ContainsKey(unionid))
            {
                return;
            }
                
            if (site.Count >= peopleNum)
            {
                log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "table is full peopleNum:{0}", peopleNum);
                hub.hub.gates.call_client(client_uuid, "room", "mj_huanghuang_room_is_full");

                return;
            }
            
            playerproxy _proxy = new playerproxy(client_uuid, unionid, game_id, nickname, headimg, sex, rank_score);
            server.players.reg_proxy(client_uuid, _proxy);

            _proxy.room_id = room_id;

            _proxy.player_info["site"] = (Int64)0;

            ArrayList uuids = new ArrayList();
            ArrayList _room_info = new ArrayList();
            log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "players count:{0}", players.Count);
            foreach (var item in players)
            {
                uuids.Add(item.Value.uuid);
                _room_info.Add(item.Value.player_info);
            }
            log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "send player info to other");
            hub.hub.gates.call_group_client(uuids, "room", "on_player_enter_mj_huanghuang_room", _proxy.player_info);

            log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "send room info to player");
            hub.hub.gates.call_client(client_uuid, "room", "on_enter_mj_huanghuang_room", _room_info, peopleNum, score, times, payRule, time);

            players.Add(unionid, _proxy);
            players_uuid.Add(client_uuid, _proxy);
            matcher.Add(_proxy);

            hub.hub.hubs.call_hub("lobby", "lobby", "enter_room", client_uuid, hub.hub.name, room_id);

            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "end join_table");
        }

        public void join_robot()
        {
            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "begin join_table");
            
            if (is_free)
            {
                log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "please creat table");
                return;
            }

            Int64 time = service.timerservice.Tick;
            
            if (site.Count >= peopleNum)
            {
                log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "table is full peopleNum:{0}", peopleNum);

                return;
            }

            for (int i = site.Count; i < peopleNum; i++)
            {
                playerproxy _proxy = new playerproxy();

                _proxy.room_id = room_id;

                ArrayList uuids = new ArrayList();
                ArrayList _room_info = new ArrayList();
                log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "players count:{0}", players.Count);
                foreach (var item in players)
                {
                    uuids.Add(item.Value.uuid);
                    _room_info.Add(item.Value.player_info);
                }
                log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "send player info to other");
                hub.hub.gates.call_group_client(uuids, "room", "on_player_enter_mj_huanghuang_room", _proxy.player_info);

                players.Add(_proxy.unionid, _proxy);
                players_uuid.Add(_proxy.uuid, _proxy);

                var _site = (int)GameCommon.mjSite.One;
                for(; _site != (int)GameCommon.mjSite.End; _site++)
                {
                    if (!site.ContainsKey(_site))
                    {
                        break;
                    }
                }
                site.Add(_site, _proxy);

                _proxy.player_info["site"] = _site;
                _proxy.state = GameCommon.mjPlayerstate.read;
                broadcast("room", "on_mj_huanghuang_occupat_site", _proxy.player_info["unionid"], _site);
            }

            ((mj_huanghuang_table)this).frist_begin_game();
            ((mj_huanghuang_table)this).begin_game();
            ((mj_huanghuang_table)this).deal();

            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "end join_table");
        }

        public void exit_table(string client_uuid)
        {
            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "begin exit_table");

            log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "exit_table client_uuid:{0}", client_uuid);

            var _proxy = get_player_proxy(client_uuid);
            if (_proxy == null)
            {
                return;
            }
            if (in_game)
            {
                return;
            }
                
            log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick,  "exit_table 1");

            if ((Int64)_proxy.player_info["site"] != (Int64)0)
            {
                site.Remove((Int64)_proxy.player_info["site"]);
                _proxy.player_info["site"] = (Int64)0;

                hub.hub.hubs.call_hub("lobby", "lobby", "exit_table", owner, room_id);
            }
            else
            {
                matcher.Remove(_proxy);
            }
                    
            log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick,  "exit_table 2");

            players_uuid.Remove(client_uuid);
            players.Remove((string)_proxy.player_info["unionid"]);

            log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "on_exit_mj_huanghuang_room client_uuid:{0}", client_uuid);

            broadcast("room", "on_exit_mj_huanghuang_room", _proxy.player_info["unionid"]);

            hub.hub.hubs.call_hub("lobby", "lobby", "exit_room", client_uuid);
            hub.hub.gates.call_client(_proxy.uuid, "room", "on_exit_mj_huanghuang_room", _proxy.player_info["unionid"]);

            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "end exit_table");
        }

        public void disband()
        {
            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "begin disband");

            broadcast("room", "disband");
            foreach (var item in players)
            {
                hub.hub.hubs.call_hub("lobby", "lobby", "exit_room", item.Value.uuid);
            }
            server.tables.free_mj_huanghuang_table(room_id);
            hub.hub.hubs.call_hub("lobby", "lobby", "disband_room", owner, room_id);

            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "end disband");
        }

        public void end_game_disband()
        {
            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "begin end_game_disband");

            foreach (var item in players)
            {
                hub.hub.hubs.call_hub("lobby", "lobby", "exit_room", item.Value.uuid);
            }
            server.tables.free_mj_huanghuang_table(room_id);
            hub.hub.hubs.call_hub("lobby", "lobby", "disband_room", owner, room_id);

            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "end end_game_disband");
        }

        public playerproxy get_player_proxy(string client_uuid)
        {
            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "begin get_player_proxy");

            if (players_uuid.ContainsKey(client_uuid))
            {
                return players_uuid[client_uuid];
            }

            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "end get_player_proxy");

            return null;
        }

        public playerproxy get_player_proxy1(string unionid)
        {
            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "begin get_player_proxy");

            if (players.ContainsKey(unionid))
            {
                return players[unionid];
            }

            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "end get_player_proxy");

            return null;
        }

        public void broadcast(string module, string func, params object[] argvs)
        {
            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "begin broadcast");

            log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "broadcast...1");
            ArrayList uuids = new ArrayList();
            foreach (var item in players)
            {
                if (!item.Value.is_robot)
                {
                    uuids.Add(item.Value.uuid);
                }
            }
            log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "broadcast...2");
            if (uuids.Count > 0)
            {
                hub.hub.gates.call_group_client(uuids, module, func, argvs);
            }

            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "end broadcast");
        }

        public string owner;
        public Dictionary<string, playerproxy> players;
        public Dictionary<string, playerproxy> players_uuid;

        public Dictionary<Int64, playerproxy> site;
        public List<playerproxy> matcher;

        public bool in_game;
        public Int64 play_count;
        public bool is_robot_room;
        public Int64 room_id;
        public Int64 peopleNum;
        public Int64 score;
        public Int64 times;
        public Int64 payRule;

        public bool is_free = true;
    }
}
