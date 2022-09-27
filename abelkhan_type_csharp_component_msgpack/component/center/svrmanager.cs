/*
 * svrmanager
 * 2020/6/2
 * qianqians
 */

using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace abelkhan
{
    public class svrproxy
    {
        public string type;
        public string hub_type;
        public string name;
        public string ip;
        public ushort port;
        public abelkhan.Ichannel ch;

        private abelkhan.center_call_server_caller _center_call_server_caller;

        public svrproxy(abelkhan.Ichannel _ch, abelkhan.modulemng modules, string _type, string _hub_type, string _name, string _ip, ushort _port)
        {
            type = _type;
            hub_type = _hub_type;
            name = _name;
            ip = _ip;
            port = _port;
            ch = _ch;

            _center_call_server_caller = new abelkhan.center_call_server_caller(_ch, modules);
        }

        public void reg_server_sucess()
        {
            _center_call_server_caller.reg_server_sucess();
        }

        public event Action<svrproxy> on_svr_close;
        public void close_server()
        {
            _center_call_server_caller.close_server();
            if (on_svr_close != null)
            {
                on_svr_close(this);
            }
        }

        public void server_be_closed(string type, string name)
        {
            _center_call_server_caller.server_be_close(type, name);
        }
    }

    public class svrmanager
    {
        private abelkhan.modulemng modules;
        private List<svrproxy> dbproxys;
        public Dictionary<abelkhan.Ichannel, svrproxy> svrproxys;

        public svrmanager(abelkhan.modulemng _modules)
        {
            modules = _modules;
            dbproxys = new List<svrproxy>();
            svrproxys = new Dictionary<abelkhan.Ichannel, svrproxy>();
            closed_svr_list = new List<svrproxy>();
        }

        public svrproxy reg_svr(abelkhan.Ichannel ch, string type, string hub_type, string name, string ip, ushort port)
        {
            var _svrproxy = new svrproxy(ch, modules, type, hub_type, name, ip, port);
            svrproxys.Add(ch, _svrproxy);
            if (type == "dbproxy")
            {
                dbproxys.Add(_svrproxy);
            }
            _svrproxy.on_svr_close += on_svr_close;
            return _svrproxy;
        }

        public List<svrproxy> closed_svr_list;
        public void remove_closed_svr()
        {
            foreach (var _proxy in closed_svr_list)
            {
                if (svrproxys.ContainsKey(_proxy.ch))
                {
                    svrproxys.Remove(_proxy.ch);
                }

                if (dbproxys.Contains(_proxy))
                {
                    dbproxys.Remove(_proxy);
                }
            }
            closed_svr_list.Clear();
        }

        public void on_svr_close(svrproxy _proxy)
        {
            closed_svr_list.Add(_proxy);
            
            for_each_svr((_proxy_tmp)=> {
                _proxy_tmp.server_be_closed(_proxy.type, _proxy.name);
            });
        }

        public void close_db()
        {
            foreach (var _proxy in dbproxys)
            {
                _proxy.close_server();
            }
        }

        public void for_each_svr(Action<svrproxy> fn){
            foreach (var _proxy in svrproxys.Values)
            {
                fn(_proxy);
            }
        }

        public svrproxy get_svr(abelkhan.Ichannel ch)
        {
            if (svrproxys.TryGetValue(ch, out svrproxy _proxy))
            {
                return _proxy;
            }

            return null;
        }

        public svrproxy find_svr(string name)
        {
            foreach (var _proxy in svrproxys.Values)
            {
                if (_proxy.name.Equals(name)) {
                    return _proxy;
                }
            }

            return null;
        }
    }
}