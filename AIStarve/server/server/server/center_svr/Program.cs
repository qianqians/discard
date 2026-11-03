using Abelkhan;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Http;
using Service;
using System;
using System.Threading;
using System.Xml.Linq;

namespace center_svr
{
    class Program
    {
        public static RedisHandle _redis_handle;

        static void Main(string[] args)
        {
            var _center = new Abelkhan.Center(args[0], args[1]);
            _center.on_svr_disconnect += (Abelkhan.SvrProxy _proxy) =>
            {
                Log.Log.err("svr:{0},{1} exception!", _proxy.type, _proxy.name);
            };

            if (!_center._root_cfg.has_key("redis_for_mq_pwd"))
            {
                _redis_handle = new RedisHandle(_center._root_cfg.get_value_string("redis_for_cache"), string.Empty);
            }
            else
            {
                _redis_handle = new RedisHandle(_center._root_cfg.get_value_string("redis_for_cache"), _center._root_cfg.get_value_string("redis_for_mq_pwd"));
            }

            Log.Log.trace("Center start ok");

            _center.run();
        }
    }
}
