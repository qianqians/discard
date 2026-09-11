using Abelkhan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;

namespace RankSvr
{
    class PlayerProxy
    {
        private Hub.HubProxy proxy;
        private rank_player_caller caller;

        public PlayerProxy(Hub.HubProxy _proxy, rank_player_caller _caller)
        {
            proxy = _proxy;
            caller = _caller;
        }

        public void rank_reward(List<RankPlayer> rewards)
        {
            caller.get_hub(proxy.name).rank_reward(rewards);
        }
    }

    class RankSvr
    {
        private static readonly rank_player_caller caller = new();
        private static Dictionary<string, PlayerProxy> playerProxys = new();

        static void Main(string[] args)
		{
            var _hub = new Hub.Hub(args[0], args[1], "rank", "fixed");

            _hub.on_hubproxy += on_hubproxy;
            _hub.on_hubproxy_reconn += on_hubproxy;

            _hub.onCloseServer += () => {
                _hub.closeSvr();
            };

            _hub.onDBProxyInit += async () =>
            {
                await Rank.RankModule.Init(Constant.Constant.player_db_name, Constant.Constant.player_db_guid_rank, new List<string>() { Constant.Constant.player_rank_name_combat_power });
            };

            Log.Log.trace("rank svr start ok");

            _hub.run().Wait();

            Rank.RankModule.save_rank();
        }

        private static void on_hubproxy(Hub.HubProxy _proxy)
        {
            Log.Log.trace("on_hubproxy type:{0}, name:{1}!", _proxy.type, _proxy.name);

            if (_proxy.type == "player")
            {
                playerProxys[_proxy.name] = new PlayerProxy(_proxy, caller);
            }
        }
    }
}
