using System;
using System.Threading;
using System.IO;

namespace lobby
{
    class server
    {
        static void onClientDisconnect(string client_uuid)
        {
            if (players == null)
            {
                return;
            }

            var _proxy = players.get_player_uuid(client_uuid);
            if (_proxy == null)
            {
                return;
            }

            _proxy.is_inline = false;
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

            login _login = new login();
            lobby _lobby = new lobby();
            gm _gm = new gm();
            agent _agent = new agent();
            match _match = new match();
            hub.hub.modules.add_module("login", _login);
            hub.hub.modules.add_module("lobby", _lobby);
            hub.hub.modules.add_module("gm", _gm);
            hub.hub.modules.add_module("agent", _agent);
            hub.hub.modules.add_module("match", _match);

            _hub.onConnectDB += () =>
            {
                players = new playermng();
                
                pay _pay = new pay();
                hub.hub.modules.add_module("pay", _pay);
            };
            
            while (true)
            {
                if (hub.hub.closeHandle.is_close)
                {
                    log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "server closed, hub server {0}", hub.hub.uuid);
                    break;
                }
                
                payUtil.tick_player_pay();

                if (_hub.poll() < 50)
                {
                    Thread.Sleep(15);
                }
            }
        }

        public static playermng players;
        public static bool disable;
        public static int rate_index = 0;

    }
}
