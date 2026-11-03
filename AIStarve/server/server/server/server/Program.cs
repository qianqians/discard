using Abelkhan;
using avatar;
using bag;
using Service;
using System.Collections.Generic;

namespace Server
{
    class Server
    {
        public static RedisHandle RedisHandle;
        public static SceneMgr SceneMgr;
        public static AvatarMgr avatarMgr;
        public static Dictionary<string, Hub.HubProxy> HubProxyMap;
        public static TimeInfoMgr timeInfoMgr;

        static void Main(string[] args)
		{
            var _hub = new Hub.Hub(args[0], args[1], "server", "fixed");

            if (!Hub.Hub._root_config.has_key("redis_for_mq_pwd"))
            {
                RedisHandle = new RedisHandle(Hub.Hub._root_config.get_value_string("redis_for_cache"), string.Empty);
            }
            else
            {
                RedisHandle = new RedisHandle(Hub.Hub._root_config.get_value_string("redis_for_cache"), Hub.Hub._root_config.get_value_string("redis_for_mq_pwd"));
            }

            HttpClientWrapper.Init();

            HubProxyMap = new();
            SceneMgr = new SceneMgr();
            avatarMgr = new AvatarMgr();
            timeInfoMgr = new TimeInfoMgr();
            timeInfoMgr.Init(Hub.Hub._config.get_value_bool("time_info_save"));

            BagModdule.init(128, avatarMgr);
            var _login_msg_handle = new login_msg_handle();
            var _scene_mgr_msg_handle = new scene_mgr_msg_handle();
            var _scene_msg_handle = new scene_msg_handle();

            _hub.on_hubproxy += on_hubproxy;
            _hub.on_hubproxy_reconn += on_hubproxy;

            _hub.on_client_disconnect += _hub_on_client_disconnect;

            _hub.onCloseServer += () => {
                _hub.closeSvr();
            };

            Hub.Hub._timer.addticktime(1000, _time_update);

            Log.Log.trace("login start ok");

            _hub.run().Wait();
        }

        private static void _time_update(long tick_time)
        {
            timeInfoMgr.Update(tick_time);
            Hub.Hub._timer.addticktime(1000, _time_update);
        }

        private static void _hub_on_client_disconnect(string uuid)
        {
            SceneMgr.ClientDisconnect(uuid);
        }

        private static void on_hubproxy(Hub.HubProxy _proxy)
        {
            if (_proxy.type == "server")
            {
                HubProxyMap[_proxy.name] = _proxy;
            }
        }
    }
}
