using System;
using System.IO;
using System.Collections;
using System.Net;
using common;

namespace lobby
{
    class login : imodule
    {
        //pc端登录
        public void player_login_account(string account_id)
        {
            var client_uuid = hub.hub.gates.current_client_uuid;
            log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "player_login {0}",client_uuid);

            //占线
            if (server.players.has_player(account_id))
            {
				log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "relogin");
                
                string old_uuid = server.players.relogin(account_id, "123", "nil", client_uuid);
                var _proxy = server.players.get_player_unionid(account_id);
                if (_proxy.tmp_player_info["in_room"] != null && (Int64)_proxy.tmp_player_info["in_room"] != 0) //如果玩家在游戏中
                {
                    hub.hub.gates.call_client(client_uuid, "login", "login_sucess", 
                        _proxy.player_info["unionid"], 
                        _proxy.nickname, 
                        _proxy.headimg, 
                        _proxy.sex, 
                        _proxy.player_info, 
                        (string)_proxy.tmp_player_info["room_name"], 
                        (Int64)_proxy.tmp_player_info["in_room"],
                        (Int64)server.rate_index);
                    
                }
                else
                {
                    hub.hub.gates.call_client(client_uuid, "login", "login_sucess", _proxy.player_info["unionid"], _proxy.nickname, _proxy.headimg, _proxy.sex, _proxy.player_info, "", 0, (Int64)server.rate_index);
                }
                
                if (_proxy.room_list.Count > 0)
                {
                    hub.hub.gates.call_client(client_uuid, "room", "room_list", _proxy.room_list);
                }

                hub.hub.gates.call_client(old_uuid, "login", "other_login");
            }
            else
            {
                log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "getObjectInfo");

                Hashtable _query = new Hashtable();
                _query.Add("unionid", account_id);
                hub.hub.dbproxy.getCollection("test", "objects").getObjectInfo(_query, (ArrayList date_list) => { query_player_info_account(client_uuid, "123", account_id, date_list); }, ()=> { });
            }
        }

        //pc端登录
        void query_player_info_account(string uuid, string token, string unionid, ArrayList date_list)
        {
            log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "db rsp {0} : {1} : {2}", uuid, token, unionid);

            if (date_list.Count > 1)
            {
				log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "error: repeate token");
            }

            if (date_list.Count == 0)
            {
                Hashtable _data = new Hashtable();
                _data.Add("unionid", unionid);
                _data.Add("diamond", (Int64)12);
                _data.Add("object_type", "player");
                _data.Add("account_type", "pc");

                _data.Add("rank_score", (Int64)0);

                //充值
                _data.Add("pay_total", (Int64)0);

                //消费时间
                ArrayList consume_time = new ArrayList();               
                _data.Add("consume_time", (ArrayList)consume_time);

                _data.Add("reg_key", (Int64)(10000000 + server.players.count++));
                log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "注册账号");
                hub.hub.dbproxy.getCollection("test", "objects").createPersistedObject(_data, () => { create_player_account(uuid, token, unionid, _data); });
            }
            else
            {
                reg_client_info_account(uuid, token, unionid, (Hashtable)date_list[0]);
            }

        }

        void create_player_account(string uuid, string token, string unionid, Hashtable _data)
        {
            reg_client_info_account(uuid, token, unionid, _data);
        }

        void reg_client_info_account(string uuid, string token, string unionid, Hashtable _data)
        {
            var _proxy = server.players.reg_player_account(uuid, token, _data);
            if (_proxy == null)
            {
                log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "error login");
                return;
            }
            
            hub.hub.gates.call_client(uuid, "login", "login_sucess", _proxy.player_info["unionid"], _proxy.nickname, _proxy.headimg, _proxy.sex, _proxy.player_info, "", 0, (Int64)server.rate_index);
            log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "rsp client {0}", uuid);
        }

        //微信端登录
        public void player_login(string code)
        {
            var client_uuid = hub.hub.gates.current_client_uuid;
            log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "player_login {0}", client_uuid);

            string uri = String.Format("https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code", global.appid, global.secret, code);
            HttpWebRequest request = HttpWebRequest.Create(uri) as HttpWebRequest;
            request.Method = "GET";
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "get response");

                Stream stream = response.GetResponseStream();
                StreamReader sr = new StreamReader(stream);
                string result = sr.ReadToEnd();

                log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "result");

                Hashtable o = Json.Jsonparser.unpack(result) as Hashtable;

                string token = o["access_token"] as string;
                string unionid = o["unionid"] as string;
                string openid = o["openid"] as string;
                string refresh_token = o["refresh_token"] as string;

                hub.hub.gates.call_client(client_uuid, "login", "Set_local_login_info", token, refresh_token, unionid, openid);

                
                if (server.players.has_player(unionid))
                {
					log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "relogin");

                    string old_uuid = server.players.relogin(unionid, token, openid, client_uuid);
                    var _proxy = server.players.get_player_unionid(unionid);
                    if (_proxy.tmp_player_info["in_room"] != null && (Int64)_proxy.tmp_player_info["in_room"] != 0) //如果玩家在游戏中
                    {
                        hub.hub.gates.call_client(client_uuid, "login", "login_sucess", 
                            _proxy.player_info["unionid"], 
                            _proxy.nickname, 
                            _proxy.headimg, 
                            _proxy.sex, 
                            _proxy.player_info, 
                            (string)_proxy.tmp_player_info["room_name"], 
                            (Int64)_proxy.tmp_player_info["in_room"],
                            (Int64)server.rate_index);
                        
                    }
                    else
                    {
                        hub.hub.gates.call_client(client_uuid, "login", "login_sucess", _proxy.player_info["unionid"], _proxy.nickname, _proxy.headimg, _proxy.sex, _proxy.player_info, "", 0, (Int64)server.rate_index);
                    }

                    if (_proxy.room_list.Count > 0)
                    {
                        hub.hub.gates.call_client(client_uuid, "room", "room_list", _proxy.room_list);
                    }

                    hub.hub.gates.call_client(old_uuid, "login", "other_login");
                }
                else
                {
                    log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "getObjectInfo");

                    Hashtable _query = new Hashtable();
                    _query.Add("unionid", unionid);
                    hub.hub.dbproxy.getCollection("test", "objects").getObjectInfo(_query, (ArrayList date_list) => { query_player_info(client_uuid, token, unionid, openid, date_list); }, ()=> { });
                }
            }
        }

        public void player_login_token(string token, string refreshToken, string unionid, string openid)
        {
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(unionid) || string.IsNullOrEmpty(openid) || string.IsNullOrEmpty(refreshToken))
            {
                return;
            }

            log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "一:{0}   二:{1}", token, refreshToken);
            var client_uuid = hub.hub.gates.current_client_uuid;
            string uri = string.Format("https://api.weixin.qq.com/sns/oauth2/refresh_token?appid={0}&grant_type=refresh_token&refresh_token={1}", global.appid, refreshToken);
            HttpWebRequest request = HttpWebRequest.Create(uri) as HttpWebRequest;
            request.Method = "GET";
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "get response");

                Stream stream = response.GetResponseStream();
                StreamReader sr = new StreamReader(stream);
                string result = sr.ReadToEnd();

                log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "{0}", result);


                Hashtable o = Json.Jsonparser.unpack(result) as Hashtable;
                string access_token = (string)o["access_token"];
                string refresh_token = (string)o["refresh_token"];

                if (string.IsNullOrEmpty(access_token) == false && o.Contains("errmsg") == false)
                {
                    string url = string.Format("https://api.weixin.qq.com/sns/auth?access_token={0}&openid={1}", access_token, openid);
                    HttpWebRequest req = HttpWebRequest.Create(url) as HttpWebRequest;
                    req.Method = "GET";
                    using (HttpWebResponse respon = req.GetResponse() as HttpWebResponse)
                    {
                        Stream st = respon.GetResponseStream();
                        StreamReader streamreader = new StreamReader(st);
                        string res = streamreader.ReadToEnd();

                        Hashtable obj = Json.Jsonparser.unpack(res) as Hashtable;
                        Int64 err_code = (Int64)obj["errcode"];
                        if (err_code == 0)
                        {
                            if (server.players.has_player(unionid))
                            {
                                log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "relogin");


                                string old_uuid = server.players.relogin(unionid, access_token, openid, client_uuid);

                                var _proxy = server.players.get_player_unionid(unionid);

                                if (_proxy.tmp_player_info["in_room"] != null && (Int64)_proxy.tmp_player_info["in_room"] != 0) //如果玩家在游戏中
                                {
                                    hub.hub.gates.call_client(client_uuid, "login", "login_sucess",
                                        _proxy.player_info["unionid"],
                                        _proxy.nickname,
                                        _proxy.headimg,
                                        _proxy.sex,
                                        _proxy.player_info,
                                        (string)_proxy.tmp_player_info["room_name"],
                                        (Int64)_proxy.tmp_player_info["in_room"],
                                        (Int64)server.rate_index);

                                }
                                else
                                {
                                    hub.hub.gates.call_client(client_uuid, "login", "login_sucess", _proxy.player_info["unionid"], _proxy.nickname, _proxy.headimg, _proxy.sex, _proxy.player_info, "", 0, (Int64)server.rate_index);
                                }

                                if (_proxy.room_list.Count > 0)
                                {
                                    hub.hub.gates.call_client(client_uuid, "room", "room_list", _proxy.room_list);
                                }

                                hub.hub.gates.call_client(old_uuid, "login", "other_login");
                            }
                            else
                            {
                                log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "getObjectInfo");

                                Hashtable _query = new Hashtable();
                                _query.Add("unionid", unionid);
                                hub.hub.dbproxy.getCollection("test", "objects").getObjectInfo(_query, (ArrayList date_list) => { query_player_info(client_uuid, access_token, unionid, openid, date_list); }, () => { });
                            }
                            hub.hub.gates.call_client(client_uuid, "login", "Access_token_login", true, access_token);
                        }
                        else
                        {
                            log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "刷新token失败");
                            hub.hub.gates.call_client(client_uuid, "login", "Access_token_login", false, "");
                        }
                    }
                }
                else
                {
                    hub.hub.gates.call_client(client_uuid, "login", "Access_token_login", false, "");
                }
            }
        }

        //微信登陆时查询
        void query_player_info(string uuid, string token, string unionid, string openid, ArrayList date_list)
        {
			log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "db rsp{0}:{1}:{2} ", uuid, token, unionid);


            if (date_list.Count > 1)
            {
                log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "error: repeate token");
            }

            if (date_list.Count == 0)
            {
                Hashtable _data = new Hashtable();
                
                _data.Add("unionid", unionid);
                _data.Add("openid", openid);
                _data.Add("diamond", (Int64)12);
                _data.Add("object_type", "player");
                _data.Add("account_type", "wechat");

                _data.Add("rank_score", (Int64)0);

                //充值
                _data.Add("pay_total", (Int64)0);

                //消费时间
                ArrayList consume_time = new ArrayList();
                _data.Add("consume_time", (ArrayList)consume_time);

                _data.Add("reg_key", (Int64)(10000000 + server.players.count++));
                log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "注册账号");
                hub.hub.dbproxy.getCollection("test", "objects").createPersistedObject(_data, ()=> { create_player(uuid, token, unionid, openid, _data); });
            }
            else
            {
                reg_client_info(uuid, token, unionid, openid, (Hashtable)date_list[0]);
            }

        }

        void create_player(string uuid, string token, string unionid, string openid, Hashtable _data)
        {
            reg_client_info(uuid, token, unionid, openid, _data);
        }

        void reg_client_info(string uuid, string token, string unionid, string openid, Hashtable _data)
        {
            var _proxy = server.players.reg_player(uuid, token, openid, _data);
            if (_proxy == null)
            {
                log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "error login");
                return;
            }

            hub.hub.gates.call_client(uuid, "login", "login_sucess", _proxy.player_info["unionid"], _proxy.nickname, _proxy.headimg, _proxy.sex, _proxy.player_info, "", 0, (Int64)server.rate_index);
            log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "rsp client headimg:{0}", _proxy.headimg);
        }
    }
}
