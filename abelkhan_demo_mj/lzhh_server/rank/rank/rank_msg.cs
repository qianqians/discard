using System;
using System.Collections;
using common;
using System.Threading;
using System.Timers;

namespace rank
{
    class rank_msg : imodule
    {
        public void clear_rank(string rank_name)
        {
            if (server.ranks.ContainsKey(rank_name))
            {
                server.ranks[rank_name].clear_rank();
            }
            else
            {
                log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "not have the rank :{0}", rank_name);
            }
        }

        public void update_rank(string rank_name, Hashtable entity)
        {
            if (server.ranks.ContainsKey(rank_name))
            {
                var rank = server.ranks[rank_name];

                rank.update_rank(entity);
            }
            else
            {
                log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "not have the rank :{0}", rank_name);
            }
        }

        public void get_rank_entity(string rank_name, int up, int down)
        {
            if (server.ranks.ContainsKey(rank_name))
            {
                var rank = server.ranks[rank_name];

                ArrayList rank_info = new ArrayList();
                for (int i = up; i <= down; i++)
                {
                    rank_info.Add(rank.get_rank(i));
                }
                hub.hub.gates.call_client(hub.hub.gates.current_client_uuid, "rank", "ack_get_rank_entity", rank_info);
            }
            else
            {
                log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "not have the rank :{0}", rank_name);
            }
        }

        public void get_entity_rank(string rank_name, string uuid)
        {
            if (server.ranks.ContainsKey(rank_name))
            {
                var rank = server.ranks[rank_name];

                hub.hub.gates.call_client(hub.hub.gates.current_client_uuid, "rank", "ack_get_entity_rank", rank.get_rank(uuid));
            }
            else
            {
                log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "not have the rank :{0}", rank_name);
            }
        }

        public void in_rank(string rank_name, string uuid)
        {
            if (server.ranks.ContainsKey(rank_name))
            {
                var rank = server.ranks[rank_name];

                hub.hub.gates.call_client(hub.hub.gates.current_client_uuid, "rank", "ack_in_rank", rank.in_rank(uuid));
            }
            else
            {
                log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "not have the rank :{0}", rank_name);
            }
        }
        
        public void count(string rank_name)
        {
            if (server.ranks.ContainsKey(rank_name))
            {
                var rank = server.ranks[rank_name];

                hub.hub.gates.call_client(hub.hub.gates.current_client_uuid, "rank", "ack_rank_count", rank.count());
            }
            else
            {
                log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "not have the rank :{0}", rank_name);
            }
        }
    }
}
