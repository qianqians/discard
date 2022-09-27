using System;
using System.Collections;
using System.Collections.Generic;
using StackExchange.Redis;

namespace lobby
{
    public class payUtil
    {
        public static string notify_url = "http://111.230.47.215:5000/api/pay";

        public static Dictionary<string, playerproxy> out_trade_no = new Dictionary<string, playerproxy>();
        public static ConnectionMultiplexer redis_proxy = ConnectionMultiplexer.Connect("127.0.0.1:6479");

        public static void tick_player_pay()
        {
            var str_json_out_trade_no = redis_proxy.GetDatabase().ListLeftPop("out_trade_no").ToString();
            if (str_json_out_trade_no == null)
            {
                return;
            }

            Hashtable json_out_trade_no = Json.Jsonparser.unpack(str_json_out_trade_no) as Hashtable;
            string str_out_trade_no = json_out_trade_no["out_trade_no"] as string;
            Int64 total_fee = (Int64)json_out_trade_no["total_fee"];

            playerproxy _proxy = null;
            lock(out_trade_no)
            {
                if (out_trade_no.ContainsKey(str_out_trade_no))
                {
                    _proxy = out_trade_no[str_out_trade_no];
                    out_trade_no.Remove(str_out_trade_no);
                }
            }

            if (_proxy == null)
            {
                log.log.error(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "out_trade_no:{0}", str_out_trade_no);
                return;
            }
            
            Hashtable _data = new Hashtable();
            _data.Add("total_fee", (Int64)total_fee);
            _data.Add("player_reg_key", (Int64)_proxy.player_info["reg_key"]);
            _data.Add("player_nickname", _proxy.nickname);
            if (_proxy.player_info.ContainsKey("agent_reg_key"))
            {
                _data.Add("agent_reg_key", (Int64)_proxy.player_info["agent_reg_key"]);
            }
            _data.Add("object_type", "pay_record");
            _data.Add("account_type", "wechat");
            _data.Add("time", (Int64)service.timerservice.Tick);
            hub.hub.dbproxy.getCollection("test", "objects").createPersistedObject(_data, () => { });

                   
            Int64 diamond = 0;
            if (server.rate_index == 2)
            {
                diamond = calc_pay_discount(total_fee);
            }
            else
            {
                if (_proxy.player_info.ContainsKey("agent_reg_key"))
                {
                    diamond = calc_pay_rate_agent(total_fee);
                }
                else
                {
                    diamond = calc_pay_rate(total_fee);
                }
            }
            _proxy.player_info["diamond"] = (Int64)_proxy.player_info["diamond"] + diamond;
            _proxy.player_info["pay_total"] = (Int64)_proxy.player_info["pay_total"] + (Int64)total_fee;
            _proxy.update_player_to_db_and_client(new List<string> { "diamond", "pay_total" });
            log.log.operation(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "DiamondInc:{0}...{1}...{2}", (Int64)_proxy.player_info["reg_key"], GameCommon.DiamondInc.Pay, (Int64)diamond);
        }

        //计算绑定代理充值比率
        public static Int64 calc_pay_rate_agent(Int64 total_fee)
        {
            switch (total_fee)
            {
                case 600:
                    return 7;
                case 1800:
                    return 22;
                case 3000:
                    return 36;
                case 6800:
                    return 82;
                case 12800:
                    return 158;
                default:
                    return 0;
            }
        }

        //计算充值比率
        public static Int64 calc_pay_rate(Int64 total_fee)
        {
            switch (total_fee)
            {
                case 600:
                    return 6;
                case 1800:
                    return 18;
                case 3000:
                    return 30;
                case 6800:
                    return 68;
                case 12800:
                    return 128;
                default:
                    return 0;
            }
        }

        //活动充值比率
        public static Int64 calc_pay_discount(Int64 total_fee)
        {
            switch (total_fee)
            {
                case 600:
                    return 8;
                case 1800:
                    return 24;
                case 3000:
                    return 40;
                case 6800:
                    return 90;
                case 12800:
                    return 170;
                default:
                    return 0;
            }
        }
    }
}
