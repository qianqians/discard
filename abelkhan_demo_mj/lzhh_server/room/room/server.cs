using System;
using System.Threading;
using System.Collections;
using hub;

namespace room
{
    class server
    {
        static void onClientDisconnect(string client_uuid)
        {
            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "begin onClientDisconnect");

            if (players == null)
            {
                return;
            }

            var _proxy = players.get_playerproxy(client_uuid);
            if (_proxy == null)
            {
                return;
            }

            table _table = server.tables.get_mj_huanghuang_table(_proxy.room_id);
            if (_table == null)
            {
                return;
            }

            if (_table.in_game)
            {
                _table.broadcast("mj_huanghuang", "player_disconnect", _proxy.unionid);
            }

            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "end onClientDisconnect");
        }

        static void Main(string[] args)
        {
            if (args.Length <= 0)
            {
                return;
            }

            disable = false;

            hub.hub _hub = new hub.hub(args);

            hub.hub.gates.clientDisconnect += onClientDisconnect;

            room_num = hub.hub.config.get_value_int("room_num");
            players = new playermanager();
            tables = new tablemng();

            room _room = new room();
            mj_huanghuang _mj_huanghuang = new mj_huanghuang();
            chat _chat = new chat();
            gm _gm = new gm();
            match _match = new match();
            hub.hub.modules.add_module("room", _room);
            hub.hub.modules.add_module("mj_huanghuang", _mj_huanghuang);
            hub.hub.modules.add_module("chat", _chat);
            hub.hub.modules.add_module("gm", _gm);
            hub.hub.modules.add_module("match", _match);

            while (true)
            {
                if (hub.hub.closeHandle.is_close)
                {
                    log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "server closed, hub server " + hub.hub.uuid);
                    break;
                }
                
                if (_hub.poll() < 50)
                {
                    Thread.Sleep(15);
                }
            }
        }

        public static Int64 room_num;
        public static playermanager players;
        public static tablemng tables;
        public static bool disable;


    }
}
