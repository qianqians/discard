using System;
using System.Collections;
using System.Collections.Generic;

namespace room
{
    class playerproxy
    {
        public playerproxy(string client_uuid, string _unionid, Int64 game_id, string nickname, string headimg, Int64 sex, Int64 rank_score)
        {
            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "begin playerproxy");

            uuid = client_uuid;
            unionid = _unionid;

            player_info = new Hashtable();
            player_info["unionid"] = unionid;
            player_info["game_id"] = game_id;
            player_info["nickname"] = nickname;
            player_info["headimg"] = headimg;
            player_info["sex"] = sex;
            player_info["rank_score"] = rank_score;

            player_info["score"] = (Int64)0;
            is_robot = false;

            frist_join_match = true;

            disband = GameCommon.roomDisbandVoteState.unvote;

            log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "nickname:{0}, headimg:{1}", nickname, headimg);

            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "end playerproxy");
        }

        public playerproxy()
        {
            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "begin playerproxy");

            uuid = System.Guid.NewGuid().ToString();
            unionid = System.Guid.NewGuid().ToString();

            player_info = new Hashtable();
            player_info["unionid"] = unionid;
            player_info["game_id"] = 10000000; //随机
            player_info["nickname"] = ""; //随机
            player_info["headimg"] = "";//随机
            player_info["sex"] = 0;//随机
            player_info["rank_score"] = 0;

            player_info["score"] = (Int64)0;
            is_robot = true;

            frist_join_match = true;

            disband = GameCommon.roomDisbandVoteState.unvote;

            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "end playerproxy");
        }

        public string relogin(string client_uuid)
        {
            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "begin relogin");

            string tmp = uuid;
            uuid = client_uuid;

            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "end relogin");

            return tmp;
        }

        public Int64 room_id;
        public bool is_robot;
        public string uuid;
        public string unionid;
        public Hashtable player_info;
        public GameCommon.mjPlayerstate state;
        public GameCommon.roomDisbandVoteState disband;
        public bool frist_join_match;
    }
}
