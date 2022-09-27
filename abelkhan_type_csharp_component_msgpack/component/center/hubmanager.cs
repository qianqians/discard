/*
 * hubmanager
 * 2020/6/2
 * qianqians
 */

using System;
using System.Collections.Generic;

namespace abelkhan
{
    public class hubproxy
    {
        public string type;
        public string hub_type;
        public string name;
        public bool is_closed;
        private abelkhan.center_call_hub_caller _center_call_hub_caller;

        public hubproxy(abelkhan.Ichannel ch, abelkhan.modulemng modules, string _type, string _hub_type, string _name)
        {
            type = _type;
            hub_type = _hub_type;
            name = _name;
            _center_call_hub_caller = new abelkhan.center_call_hub_caller(ch, modules);
        }

        public void distribute_server_address(string type, string name, string ip, ushort port)
        {
            _center_call_hub_caller.distribute_server_address(type, name, ip, port);
        }

        public void reload()
        {
            _center_call_hub_caller.reload();
        }
    }

    public class hubmanager
    {
        private abelkhan.modulemng modules;
        private Dictionary<abelkhan.Ichannel, hubproxy> hubproxys;

        public hubmanager(abelkhan.modulemng _modules)
        {
            modules = _modules;
            hubproxys = new Dictionary<abelkhan.Ichannel, hubproxy>();
        }

        public hubproxy reg_hub(abelkhan.Ichannel ch, string _type, string _hub_type, string _name)
        {
            var _hubproxy = new hubproxy(ch, modules, _type, _hub_type, _name);
            hubproxys.Add(ch, _hubproxy);
            return _hubproxy;
        }

        public hubproxy get_hub(abelkhan.Ichannel ch)
        {
            if (hubproxys.TryGetValue(ch, out hubproxy _proxy))
            {
                return _proxy;
            }

            return null;
        }

        public hubproxy find_hub(string name)
        {
            foreach (hubproxy _proxy in hubproxys.Values)
            {
                if (_proxy.name.Equals(name)) {
                    return _proxy;
                }
            }

            return null;
        }

        public void for_each_hub(Action<hubproxy> fn)
        {
            foreach (var _proxy in hubproxys.Values)
            {
                fn(_proxy);
            }
        }

        public void hub_closed(abelkhan.Ichannel ch)
        {
            if (hubproxys.TryGetValue(ch, out hubproxy _proxy))
            {
                _proxy.is_closed = true;
            }
        }

        public bool check_all_hub_closed()
        {
            bool _all_closed = true;
            foreach (var _proxy in hubproxys.Values)
            {
                if (!_proxy.is_closed)
                {
                    _all_closed = false;
                }
            }
            return _all_closed;
        }
    }
}
