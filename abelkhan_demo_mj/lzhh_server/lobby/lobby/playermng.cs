using System;
using System.Collections;
using System.Collections.Generic;

namespace lobby
{
    class playermng
    {
        public playermng()
        {
            players = new Dictionary<string, playerproxy>();
            players_uuid = new Dictionary<string, playerproxy>();

            bInit = false;

            Hashtable _query = new Hashtable();
            _query.Add("object_type", "player");
            hub.hub.dbproxy.getCollection("test", "objects").getObjectCount(_query, (Int64 _count) => {
                count = _count;
            });
        }

        public playerproxy reg_player(string uuid, string token, string openid, Hashtable _data)
        {
            if (!players.ContainsKey((string)_data["unionid"]) && !players_uuid.ContainsKey(uuid))
            {
                playerproxy _proxy = new playerproxy(uuid, token, _data);

                if (!_proxy.player_info.ContainsKey("openid"))
                {
                    _proxy.player_info["openid"] = openid;
                    _proxy.update_player_to_db(new List<string> { "openid" });
                }

                players.Add((string)_data["unionid"], _proxy);
                players_uuid.Add(uuid, _proxy);

                return _proxy;
            }

            return null;
        }

        public playerproxy reg_player_account(string uuid, string token, Hashtable _data)
        {
            if (!players.ContainsKey((string)_data["unionid"]) && !players_uuid.ContainsKey(uuid))
            {
                playerproxy _proxy = new playerproxy(uuid, token, "nick", "", 1, _data);

                players.Add((string)_data["unionid"], _proxy);
                players_uuid.Add(uuid, _proxy);

                return _proxy;
            }

            log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "error player register info");

            return null;
        }

        public string relogin(string unionid, string token, string openid, string client_uuid)
        {
            playerproxy _proxy = get_player_unionid(unionid);

            if (_proxy != null)
            {
                string old_uuid = _proxy.relogin(client_uuid, token);

                if (!_proxy.player_info.ContainsKey("openid"))
                {
                    _proxy.player_info["openid"] = openid;
                    _proxy.update_player_to_db(new List<string> { "openid" });
                }

                if (players_uuid.ContainsKey(old_uuid))
                {
                    players_uuid.Remove(old_uuid);
                }

                players_uuid.Add(client_uuid, _proxy);

                return old_uuid;
            }

            return "";
        }

        public bool has_player(string token)
        {
            if (players.ContainsKey(token))
            {
                return true;
            }

            return false;
        }

        public playerproxy get_player_unionid(string unionid)
        {
            if (players.ContainsKey(unionid))
            {
                return players[unionid];
            }

            return null;
        }

        public playerproxy get_player_uuid(string uuid)
        {
            foreach(var item in players_uuid)
            {
                log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "{0}:{1}",item.Key, item.Value);
            }
            if (players_uuid.ContainsKey(uuid))
            {
                return players_uuid[uuid];
            }

            return null;
        }

        public bool bInit;

        public Int64 count;
        private Dictionary<string, playerproxy> players;

        private Dictionary<string, playerproxy> players_uuid;

        public Dictionary<string, playerproxy> Players
        {
            get { return players; }
        }

        public Dictionary<string, playerproxy> Players_uuid
        {
            get { return players_uuid; }
        }
    }
}
