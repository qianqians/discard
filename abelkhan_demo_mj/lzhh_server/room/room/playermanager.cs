using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace room
{
    class playermanager
    {
        public playermanager()
        {
            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "begin playermanager");

            players = new Dictionary<string, playerproxy>();
            players_unionid = new Dictionary<string, playerproxy>();

            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "end playermanager");
        }

        public playerproxy get_playerproxy(string uuid)
        {
            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "begin get_playerproxy");

            if (!players.ContainsKey(uuid))
            {
                return null;
            }

            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "end get_playerproxy");

            return players[uuid];
        }

        public playerproxy get_playerproxy_unionid(string unionid)
        {
            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "begin get_playerproxy_unionid");

            if (!players_unionid.ContainsKey(unionid))
            {
                return null;
            }

            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "end get_playerproxy");

            return players_unionid[unionid];
        }

        public void reg_proxy(string uuid, playerproxy _proxy)
        {
            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "begin reg_proxy");

            if (players.ContainsKey(uuid))
            {
                players[uuid] = _proxy;
            }
            else
            {
                players.Add(uuid, _proxy);
            }

            if (players_unionid.ContainsKey(_proxy.unionid))
            {
                players_unionid[_proxy.unionid] = _proxy;
            }
            else
            {
                players_unionid.Add(_proxy.unionid, _proxy);
            }

            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "end reg_proxy");
        }

        public void unreg_proxy(string uuid)
        {
            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "begin unreg_proxy");

            if (players.ContainsKey(uuid))
            {
                players.Remove(uuid);
            }

            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "end unreg_proxy");
        }
        
        private Dictionary<string, playerproxy> players;
        private Dictionary<string, playerproxy> players_unionid;
    }
}
