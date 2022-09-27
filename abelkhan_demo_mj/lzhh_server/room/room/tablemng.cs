using System;
using System.Collections.Generic;
using System.Linq;

namespace room
{
    class tablemng
    {
        public tablemng()
        {
            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "begin tablemng");

            mj_huanghuang_tables = new mj_huanghuang_table[500];
            free_mj_huanghuang_tables = new List<Int64>();
            try
            {
                List<int> l = new List<int>();
                for (int i = 0; i < 500; i++)
                {
                    mj_huanghuang_tables[i] = new mj_huanghuang_table();
                    mj_huanghuang_tables[i].init(i);

                    l.Add(i);
                }

                Random ra = new Random();
                int r = ra.Next();

                while (l.Count > 0)
                {
                    int i = r % l.Count;
                    int index = l[i];
                    free_mj_huanghuang_tables.Add(index);

                    l.RemoveAt(i);
                }
            }
            catch (Exception e)
            {
                log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "{0}", e);
            }

            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "end tablemng");
        }

        public bool is_busy()
        {
            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "begin is_busy");

            if (free_mj_huanghuang_tables.Count > 0)
            {
                return false;
            }

            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "end is_busy");

            return true;
        }

        public Int64 create_mj_huanghuang_table()
        {
            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "begin create_mj_huanghuang_table");
            create_tables_count++;
            if (free_mj_huanghuang_tables.Count > 0)
            {
                Int64 room_id = free_mj_huanghuang_tables.ElementAt(0);
                free_mj_huanghuang_tables.Remove(room_id);
                mj_huanghuang_tables[room_id].voting = false;
                mj_huanghuang_tables[room_id].clean();
                mj_huanghuang_tables[room_id].is_free = false;

                return room_id + 100000 * server.room_num;
            }

            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "end create_mj_huanghuang_table");

            return -1;
        }

        public mj_huanghuang_table get_mj_huanghuang_table(Int64 room_id)
        {
            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "begin get_mj_huanghuang_table");

            room_id -= 100000 * server.room_num;

            if (room_id >= 0 && room_id < 500)
            {
                return mj_huanghuang_tables[room_id];
            }

            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "end get_mj_huanghuang_table");

            return null;
        }

        public void free_mj_huanghuang_table(Int64 room_id)
        {
            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "begin free_mj_huanghuang_table");
            free_tables_count++;
            if (room_id < 500)
            {
                mj_huanghuang_tables[room_id].is_free = true;

                if (!free_mj_huanghuang_tables.Contains(room_id))
                {
                    free_mj_huanghuang_tables.Add(room_id);
                }

                log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "3:free_table:{0}", room_id);
            }
            log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "4:free_table:{0}", room_id);

            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "end free_mj_huanghuang_table");
        }

        private mj_huanghuang_table[] mj_huanghuang_tables;
        private List<Int64> free_mj_huanghuang_tables;//空闲麻将桌
        public static Int64 create_tables_count = 0;
        public static Int64 free_tables_count = 0;
    }
}
