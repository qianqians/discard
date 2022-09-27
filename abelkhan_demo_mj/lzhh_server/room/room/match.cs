using System;
using common;

namespace room
{
    class match : imodule
    {
        public void free_match_room(Int64 room_id)
        {
            server.tables.free_mj_huanghuang_table(room_id);
        }

        public void join_robot(Int64 room_id)
        {
            mj_huanghuang_table _table = server.tables.get_mj_huanghuang_table(room_id);
            if (_table == null)
            {
                return;
            }

            _table.join_robot();
        }
    }
}
