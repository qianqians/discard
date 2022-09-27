using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Net;

namespace lobby
{
    public class playerproxy
    {
        public playerproxy(Hashtable _data)
        {
            uuid = "";
            token = "";

            is_inline = false;

            player_info = _data;

            tmp_player_info = new Hashtable();

            room_list = new Hashtable();
        }

        public playerproxy(string client_uuid, string access_token, Hashtable _data)
        {
            uuid = client_uuid;
            token = access_token;

            player_info = _data;

            tmp_player_info = new Hashtable();

            room_list = new Hashtable();

            string uri = String.Format("https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid={1}", access_token, player_info["unionid"]);
            HttpWebRequest request = HttpWebRequest.Create(uri) as HttpWebRequest;
            request.Method = "GET";
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                Stream stream = response.GetResponseStream();
                StreamReader sr = new StreamReader(stream);
                string result = sr.ReadToEnd();
                log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "{0}", result);

                Hashtable o = Json.Jsonparser.unpack(result) as Hashtable;

                nickname = o["nickname"] as string;
                headimg = o["headimgurl"] as string;
                sex = (Int64)o["sex"];

                player_info["nickname"] = nickname;
                update_player_to_db(new List<string> { "nickname" });
            }
        }

        public playerproxy(string client_uuid, string access_token, string nick_name, string _headimg, Int64 _sex, Hashtable _data)
        {
            uuid = client_uuid;
            token = access_token;

            player_info = _data;

            tmp_player_info = new Hashtable();

            room_list = new Hashtable();

            nickname = nick_name;
            headimg = _headimg;
            sex = _sex;
        }

        public string relogin(string client_uuid, string access_token)
        {
            string tmp = uuid;
            uuid = client_uuid;

            token = access_token;

            if (is_inline == false)
            {
                if ((string)player_info["account_type"] == "pc")
                {
                    nickname = "nick";
                    headimg = "";
                    sex = 1;
                }
                else if ((string)player_info["account_type"] == "wechat")
                {
                    string uri = String.Format("https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid={1}", access_token, player_info["unionid"]);
                    HttpWebRequest request = HttpWebRequest.Create(uri) as HttpWebRequest;
                    request.Method = "GET";
                    using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                    {
                        Stream stream = response.GetResponseStream();
                        StreamReader sr = new StreamReader(stream);
                        string result = sr.ReadToEnd();
                        log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "{0}", result);

                        Hashtable o = Json.Jsonparser.unpack(result) as Hashtable;

                        nickname = o["nickname"] as string;
                        headimg = o["headimgurl"] as string;
                        sex = (Int64)o["sex"];

                        player_info["nickname"] = nickname;
                        update_player_to_db(new List<string> { "nickname" });
                    }
                }

                is_inline = true;
            }

            var query = new Hashtable();
            query["unionid"] = player_info["unionid"];
            hub.hub.dbproxy.getCollection("test", "objects").getObjectInfo(query, 
                (ArrayList date_list) =>{
                    player_info = (Hashtable)date_list[0];
                },
                () => { });
            
            return tmp;
        }

        public void update_player_to_db_and_client(List<string> keys)
        {
            var query = new Hashtable();
            query["unionid"] = player_info["unionid"];

            var player_info_db = new Hashtable();
            foreach (var key in keys)
            {
                if (player_info.ContainsKey(key))
                {
                    player_info_db.Add(key, player_info[key]);
                }
            }

            hub.hub.dbproxy.getCollection("test", "objects").updataPersistedObject(query, player_info_db, () => {
                log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "{0}", Json.Jsonparser.pack(player_info_db));

                hub.hub.gates.call_client(uuid, "player_data", "update_player", player_info);
            });
        }

        public void update_player_to_db(List<string> keys)
        {
            var query = new Hashtable();
            query["unionid"] = player_info["unionid"];

            var player_info_db = new Hashtable();
            foreach (var key in keys)
            {
                if (player_info.ContainsKey(key))
                {
                    player_info_db.Add(key, player_info[key]);
                }
            }

            hub.hub.dbproxy.getCollection("test", "objects").updataPersistedObject(query, player_info_db, () => {
                log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "{0}", Json.Jsonparser.pack(player_info_db));
            });
        }

        public Hashtable score_rank_data()
        {
            var score_rank_data = new Hashtable();

            score_rank_data.Add("uuid", player_info["unionid"]);
            score_rank_data.Add("score", player_info["rank_score"]);
            score_rank_data.Add("nickname", nickname);
            score_rank_data.Add("headimg", headimg);
            score_rank_data.Add("sex", sex);

            return score_rank_data;
        }

        public bool is_inline;

        public string uuid;
        public string token;
        public string nickname;
        public string headimg;
        public Int64 sex;

        public Hashtable player_info;
        public Hashtable tmp_player_info;

        public Hashtable room_list;
    }
}
