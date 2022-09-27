using System;
using System.Collections;
using common;

namespace room
{
    class chat : imodule
    {
        //client call
        public void player_chat(Int64 room_id, Int64 chat_state, string msg, string tag_uuid)
        {
            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "begin player_chat");

            mj_huanghuang_table _table = server.tables.get_mj_huanghuang_table(room_id);
            if (_table != null)
            {
                string client_uuid = hub.hub.gates.current_client_uuid;
                var _proxy = _table.get_player_proxy(client_uuid);

                ArrayList uuids = new ArrayList();
                foreach (var p in _table.players)
                {
                    if (p.Key != _proxy.unionid)
                    {
                        uuids.Add(p.Value.uuid);
                    }
                }
                hub.hub.gates.call_group_client(uuids, "chat", "player_chat_broadcast", chat_state, msg, tag_uuid, _proxy.unionid);
            }

            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "end player_chat");
        }
    }
}
