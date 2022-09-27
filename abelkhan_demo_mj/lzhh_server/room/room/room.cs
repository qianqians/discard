using System;
using System.Collections;
using common;
using System.Threading;
using System.Collections.Generic;

namespace room
{
    class room : imodule
    {
        // lobby hub call
        public void could_create_mj_huanghuang_room(string callback_id)
        {
            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "begin could_create_mj_huanghuang_room");

            // call hub lobby
            hub.hub.hubs.call_hub("lobby", "lobby", "on_could_create_mj_huanghuang_room", !server.tables.is_busy(), callback_id, hub.hub.name);

            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "end could_create_mj_huanghuang_room");
        }

        // lobby hub call
        public void create_mj_huanghuang_room(string unionid, Int64 peopleNum, Int64 score, Int64 times, Int64 payRule, string callback_id)
        {
            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "begin create_mj_huanghuang_room");

            if (!server.tables.is_busy())
            {
                log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "peopleNum:{0}, score:{1}, times:{2}, payRule:{3}", peopleNum, score, times, payRule);

                Int64 room_id = server.tables.create_mj_huanghuang_table();
                table _table = server.tables.get_mj_huanghuang_table(room_id);
                _table.set_room_info(peopleNum, score, times, payRule);
                _table.set_room_owner(unionid);
                hub.hub.hubs.call_hub("lobby", "lobby", "on_create_mj_huanghuang_room_real", room_id, callback_id);

                hub.hub.timer.addticktime(60 * 1000, (Int64 tick) => {
                    if (!_table.is_free && !_table.in_game)
                    {
                        if (_table.site.Count <= 0)
                        {
                            ArrayList _uuids = new ArrayList();
                            foreach (var item in _table.players_uuid)
                            {
                                _uuids.Add(item.Key);
                            }
                            for (int i = _table.players_uuid.Count - 1; i >= 0; i--)
                            {
                                _table.exit_table((string)_uuids[i]);
                            }

                            hub.hub.hubs.call_hub("lobby", "lobby", "disband_room", _table.owner, room_id);

                            server.tables.free_mj_huanghuang_table(room_id);
                        }
                    }
                    else
                    {
                        log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "桌子为空");
                    }
                });
            }

            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "end create_mj_huanghuang_room");
        }

        // client call 
        public void enter_mj_huanghuang_room(Int64 room_id)
        {
            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "begin enter_mj_huanghuang_room");

            log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "enter_mj_huanghuang_room");
            var client_uuid = hub.hub.gates.current_client_uuid;

            table _table = server.tables.get_mj_huanghuang_table(room_id);
            if (_table == null)
            {
                hub.hub.gates.call_client(client_uuid, "room", "exist_room");
                return;
            }
            else
            {
                if (_table.is_free)
                {
                    hub.hub.gates.call_client(client_uuid, "room", "exist_room");
                    return;
                } 
            }
            

            if (server.disable)
            {
                hub.hub.gates.call_client(client_uuid, "room", "disable_game");

                return;
            }
            hub.hub.hubs.call_hub("lobby", "lobby", "get_player_info_enter_room", hub.hub.name, client_uuid, room_id, false);

            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "end enter_mj_huanghuang_room");
        }

        // client call
        public void reconnect_enter_mj_huanghuang_room(Int64 room_id)
        {
            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "begin reconnect_enter_mj_huanghuang_room");

            log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "reconnect_enter_mj_huanghuang_room");

            var client_uuid = hub.hub.gates.current_client_uuid;
            hub.hub.hubs.call_hub("lobby", "lobby", "get_player_info_enter_room", hub.hub.name, client_uuid, room_id, true);

            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "end reconnect_enter_mj_huanghuang_room");
        }

        public void player_info_enter_room(Int64 room_id, string uuid, string unionid, Int64 game_id, string nickname, string headimg, Int64 sex, Int64 rank_score, bool is_renconnect)
        {
            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "begin player_info_enter_room");

            log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "player_info_enter_room:{0}", room_id);
            table _table = server.tables.get_mj_huanghuang_table(room_id);
            if (_table != null && !_table.is_free)
            {
                if (!is_renconnect)
                {
                    log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "join_table");
                    _table.join_table(room_id, uuid, unionid, game_id, nickname, headimg, sex, rank_score);
                }
                else
                {
                    log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "rejoin_table");
                    _table.rejoin_table(room_id, uuid, unionid, game_id, nickname, headimg, sex, rank_score);
                }
            }
            else
            {
                log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "exist_room");
                hub.hub.gates.call_client(uuid, "room", "exist_room");
            }

            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "end player_info_enter_room");
        }

        // client call
        public void exit_mj_huanghuang_room(Int64 room_id)
        {
            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "begin exit_mj_huanghuang_room");

            log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "exit_mj_huanghuang_room");
            table _table = server.tables.get_mj_huanghuang_table(room_id);
            var client_uuid = hub.hub.gates.current_client_uuid;
            if (_table != null && !_table.in_game)
            {
                _table.exit_table((string)client_uuid);
            }

            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "end exit_mj_huanghuang_room");
        }

        // client call 
        public void mj_huanghuang_occupat_site(Int64 room_id, Int64 site)
        {
            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "begin mj_huanghuang_occupat_site");

            mj_huanghuang_table _table = server.tables.get_mj_huanghuang_table(room_id);
            if (_table != null)
            {
                log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "occupat_site");

                if (!_table.site.ContainsKey(site) && site < (int)GameCommon.mjSite.End)
                {
                    string client_uuid = hub.hub.gates.current_client_uuid;

                    var _proxy = _table.get_player_proxy(client_uuid);
                    _table.site.Add(site, _proxy);
                    _table.matcher.Remove(_proxy);

                    _proxy.player_info["site"] = site;
                    _proxy.state = GameCommon.mjPlayerstate.read;
                    _table.broadcast("room", "on_mj_huanghuang_occupat_site", _proxy.player_info["unionid"], site);

                    hub.hub.hubs.call_hub("lobby", "lobby", "occupat_site", _table.owner, room_id);
                }
                else
                {
                    string client_uuid = hub.hub.gates.current_client_uuid;
                    hub.hub.gates.call_client(client_uuid, "room", "occupat_site", false);
                    return;
                }

                
                if (_table.site.Count == _table.peopleNum)
                {
                    //坐满了则将没坐下的踢出去
                    if (_table != null)
                    {
                        ArrayList _proxys = new ArrayList();
                        ArrayList _uuids = new ArrayList();
                        
                        foreach (var item in _table.players_uuid)
                        {
                            _proxys.Add(item.Value);
                            _uuids.Add(item.Key);
                        }
                        for (int i = _table.players_uuid.Count - 1 ; i >= 0; i--)
                        {
                            if (!_table.site.ContainsValue((playerproxy)_proxys[i]))
                            {
                                _table.exit_table((string)_uuids[i]);
                            }
                        }
                    }

                    hub.hub.hubs.call_hub("lobby", "lobby", "begin_game", _table.owner, room_id);

                    //开始游戏
                    log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "1:start game !");
                    _table.frist_begin_game();
                    _table.begin_game();
                    _table.deal();
                }
            }

            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "end mj_huanghuang_occupat_site");
        }

        //client call
        public void req_disband_room(Int64 room_id)
        {
            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "begin req_disband_room");

            mj_huanghuang_table _table = server.tables.get_mj_huanghuang_table(room_id);
            if (_table != null && _table.in_game)
            {
                string client_uuid = hub.hub.gates.current_client_uuid;
                var _proxy = _table.get_player_proxy(client_uuid);
                if (!_table.voting)
                {
                    log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "req_disband_room");
                    _table.voting = true;

                    _proxy.disband = GameCommon.roomDisbandVoteState.agree;

                    ArrayList uuids = new ArrayList();
                    foreach (var p in _table.players)
                    {
                        if (p.Key != _proxy.unionid)
                        {
                            p.Value.disband = GameCommon.roomDisbandVoteState.unvote;//赋初值

                            uuids.Add(p.Value.uuid);
                        }
                    }
                    log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "broadcast req_disband_vote");
                    hub.hub.gates.call_group_client(uuids, "room", "req_disband_vote");

                    _proxy.disband = GameCommon.roomDisbandVoteState.agree;
                    _table.broadcast("room", "vote_disband_room_state", _proxy.player_info["unionid"], (Int64)_proxy.disband);

                    hub.hub.timer.addticktime(60 * 1000, (Int64 tick) =>
                    {
                        if (_table.is_free)
                        {
                            return;
                        }

                        if (!_table.voting)
                        {
                            return;
                        }
                        _table.voting = false;

                        foreach (var item in _table.players)
                        {
                            if (item.Value.disband == GameCommon.roomDisbandVoteState.unvote)
                            {
                                item.Value.disband = GameCommon.roomDisbandVoteState.agree;

                                _table.broadcast("room", "vote_disband_room_state", item.Value.player_info["unionid"], (Int64)item.Value.disband);
                            }
                            log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "{0}", item.Key);
                        }

                        bool b_disband = true;
                        foreach (var item in _table.players)
                        {
                            if (item.Value.disband != GameCommon.roomDisbandVoteState.agree)
                            {
                                b_disband = false;
                                log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "not_disband_room");
                            }
                        }
                        if (b_disband)
                        {
                            //解散房间表明一轮结束，计算输赢,桌上玩家游戏任务加一
                            if (_table.play_count > 1)
                            {
                                foreach (var item in _table.players)
                                {
                                    if ((Int64)item.Value.player_info["score"] > 0 && item.Value.is_robot != true)
                                    {
                                        hub.hub.hubs.call_hub("lobby", "lobby", "get_victory_count", item.Value.player_info["unionid"]);
                                    }
                                    if (item.Value.is_robot != true)
                                    {
                                        hub.hub.hubs.call_hub("lobby", "lobby", "get_game_count", item.Value.player_info["unionid"]);
                                    }
                                }
                            }

                            log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "disband_room");
                            _table.disband();
                        }
                    });
                }
            }
            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "end req_disband_room");
        }

        //client call
        public void vote_disband_room(Int64 room_id, Int64 disband)
        {
            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "begin vote_disband_room");

            mj_huanghuang_table _table = server.tables.get_mj_huanghuang_table(room_id);
            if (_table != null)
            {
                log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "vote_disband_room");

                string client_uuid = hub.hub.gates.current_client_uuid;
                var _proxy = _table.get_player_proxy(client_uuid);

                _proxy.disband = (GameCommon.roomDisbandVoteState)disband;
                _table.broadcast("room", "vote_disband_room_state", _proxy.player_info["unionid"], (Int64)_proxy.disband);

                bool b_disband = true;
                int vote_count = 0;
                foreach (var item in _table.players)
                {
                    if (item.Value.disband != GameCommon.roomDisbandVoteState.agree)
                    {
                        b_disband = false;
                        log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "not_disband_room");
                    }
                    if (item.Value.disband != GameCommon.roomDisbandVoteState.unvote)
                    {
                        vote_count++;
                    }
                }
                if (vote_count >= _table.peopleNum)
                {
                    _table.voting = false;
                }
                if (b_disband)
                {
                    //解散房间表明一轮结束，计算输赢,桌上玩家游戏任务加一
                    if (_table.play_count > 1)
                    {
                        foreach (var item in _table.players)
                        {
                            if ((Int64)item.Value.player_info["score"] > 0 && item.Value.is_robot != true)
                            {
                                hub.hub.hubs.call_hub("lobby", "lobby", "get_victory_count", item.Value.player_info["unionid"]);
                            }
                            if (item.Value.is_robot != true)
                            {
                                hub.hub.hubs.call_hub("lobby", "lobby", "get_game_count", item.Value.player_info["unionid"]);
                            }
                        }
                    }

                    _table.voting = false;
                    log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "disband_room");
                    _table.disband();
                }
            }

            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "end vote_disband_room");
        }

        //lobby hub gm module call
        public void server_disband_room(Int64 room_id ,string client_uuid)
        {
            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "begin server_disband_room");

            mj_huanghuang_table _table = server.tables.get_mj_huanghuang_table(room_id);
            if (_table != null)
            {
                log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick,  "gm_disband_room");

                if (_table.play_count > 1)
                {
                    foreach (var item in _table.players)
                    {
                        if ((Int64)item.Value.player_info["score"] > 0 && item.Value.is_robot != true)
                        {
                            hub.hub.hubs.call_hub("lobby", "lobby", "get_victory_count", item.Value.player_info["unionid"]);
                        }
                        if (item.Value.is_robot != true)
                        {
                            hub.hub.hubs.call_hub("lobby", "lobby", "get_game_count", item.Value.player_info["unionid"]);
                        }
                    }
                }
                _table.disband();
                hub.hub.gates.call_client(client_uuid, "gm", "server_disband_room", true);
            }
            else
            {
                hub.hub.gates.call_client(client_uuid, "gm", "server_disband_room", false);
                log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "room_id error");
            }

            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "end server_disband_room");
        }

    }
}
