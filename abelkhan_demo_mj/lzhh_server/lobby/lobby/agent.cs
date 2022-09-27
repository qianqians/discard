using System;
using System.Collections;
using System.Collections.Generic;
using common;

namespace lobby
{
    class agent : imodule
    {
        public void bind_agent(Int64 agent_id)
        {
            var client_uuid = hub.hub.gates.current_client_uuid;
            var _proxy = server.players.get_player_uuid(client_uuid);

            if (!_proxy.player_info.ContainsKey("agent_reg_key"))
            {
                Hashtable query = new Hashtable();
                query.Add("key", agent_id.ToString());
                hub.hub.dbproxy.getCollection("agent", "agent").getObjectInfo(query, 
                    (ArrayList date_list) => {
                        if (date_list.Count == 1)
                        {
                            _proxy.player_info["agent_reg_key"] = agent_id;
                            _proxy.player_info["diamond"] = (Int64)_proxy.player_info["diamond"] + (Int64)10;
                            _proxy.update_player_to_db_and_client(new List<string> { "agent_reg_key", "diamond" } );

                            hub.hub.gates.call_client(client_uuid, "agent", "bind_sucess");

                            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "bind_success");
                            log.log.operation(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "DiamondInc:{0}...{1}...{2}", (Int64)_proxy.player_info["reg_key"], GameCommon.DiamondInc.Agent, 10);
                        }
                        else
                        {
                            hub.hub.gates.call_client(client_uuid, "agent", "bind_faild", "wrong agent id");
                            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "wrong agent id");
                        }
                    }, 
                    ()=> { });
            }
            else
            {
                hub.hub.gates.call_client(client_uuid, "agent", "bind_faild", "repeated bind");
                log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "repeated bind");
            }
        }
    }
}
