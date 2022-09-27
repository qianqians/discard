using System;
using common;
using System.Collections;

namespace room
{
    class gm : imodule
    {
        #region lobby gm call;

        //禁止游戏
        public void ban()
        {
            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "begin ban");

            server.disable = true;

            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "end ban");
        }

        //统计在线房间
        public void census_inline_room(string client_uuid)
        {
            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "begin census_inline_room");

            Int64 inline_room = tablemng.create_tables_count - tablemng.free_tables_count;
            hub.hub.gates.call_client(client_uuid, "gm", "census_inline_room", inline_room);

            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "end census_inline_room");
        }

        #endregion

    }
}
