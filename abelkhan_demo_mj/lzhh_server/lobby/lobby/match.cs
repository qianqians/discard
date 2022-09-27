using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using common;

namespace lobby
{
    class match : imodule
    {
        public match()
        {
            match_list = new List<matchInfo>();

            hub.hub.timer.addticktime(12 * 1000, tickMatch);
        }

        void tickMatch(Int64 tick)
        {
            foreach (var match_room in match_list)
            {
                hub.hub.hubs.call_hub(match_room.hub_name, "match", "join_robot", match_room.room_id);
            }
            match_list.Clear();

            hub.hub.timer.addticktime(12 * 1000, tickMatch);
        }

        public void join_match()
        {
            var client_uuid = hub.hub.gates.current_client_uuid;

            if (server.disable)
            {
                hub.hub.gates.call_client(client_uuid, "room", "disable_game");

                return;
            }

            var _proxy = server.players.get_player_uuid(client_uuid);
            if (_proxy == null)
            {
                log.log.error(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "not exit player:{0}", client_uuid);
                return;
            }

            foreach (var match_room in match_list)
            {
                hub.hub.gates.call_client(_proxy.uuid, "match", "on_match_mj_huanghuang_room", match_room.hub_name, match_room.room_id);

                match_room.player_num++;
                log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "match_room.player_num:{0}", match_room.player_num);

                if (match_room.player_num >= 4)
                {
                    match_list.Remove(match_room);
                }

                return;
            }

            createroomimpl.create_mj_huanghuang_room("nil", 4, (Int64)GameCommon.GameScore.Two, (Int64)GameCommon.GameTimes.unlimited, (Int64)GameCommon.PayRule.MatchPay,
                (string hub_name, Int64 room_id) => {
                    var match_room = new matchInfo(hub_name, room_id);
                    match_room.player_num++;
                    match_list.Add(match_room);

                    hub.hub.gates.call_client(_proxy.uuid, "match", "on_match_mj_huanghuang_room", hub_name, room_id);
                });
        }

        public void wind_up(string hub_name, string unionid, Int64 score)
        {
            var _proxy = server.players.get_player_unionid(unionid);
            if (_proxy == null)
            {
                log.log.error(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "not exit player:{0}", unionid);
                return;
            }

            if (!_proxy.player_info.ContainsKey("rank_score"))
            {
                _proxy.player_info.Add("rank_score", (Int64)0);
            }
            _proxy.player_info["rank_score"] = (Int64)_proxy.player_info["rank_score"] + score;
            _proxy.update_player_to_db_and_client(new List<string> { "rank_score" });

            if ((Int64)_proxy.player_info["rank_score"] > 0)
            {
                hub.hub.hubs.call_hub("rank", "rank_msg", "update_rank", "score", _proxy.score_rank_data());
            }
        }

        public void player_leave_room(string hub_name, Int64 room_id)
        {
            foreach (var match_room in match_list)
            {
                if (match_room.hub_name != hub_name ||
                    match_room.room_id != room_id)
                {
                    continue;
                }

                match_room.player_num--;
                if (match_room.player_num <= 0)
                {
                    match_list.Remove(match_room);
                    hub.hub.hubs.call_hub(match_room.hub_name, "match", "free_match_room", match_room.room_id);
                }

                return;
            }

            var _match_room = new matchInfo(hub_name, room_id);
            _match_room.player_num = 3;
            match_list.Add(_match_room);
        }

        public void on_begin_match_game(string unionid)
        {
            var _proxy = server.players.get_player_unionid(unionid);
            _proxy.player_info["diamond"] = (Int64)_proxy.player_info["diamond"] - 2;
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
        }

        class matchInfo
        {
            public matchInfo(string _hub_name, Int64 _room_id)
            {
                hub_name = _hub_name;
                room_id = _room_id;
                player_num = 0;
            }
            
            public string hub_name;
            public Int64 room_id;
            public Int64 player_num;
        }
        List<matchInfo> match_list;
    }
}
