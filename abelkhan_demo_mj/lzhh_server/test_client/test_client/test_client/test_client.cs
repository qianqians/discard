﻿using System;
using System.Threading;


namespace client
{
    class test_client
    {
        static client _client;

        static void Main(string[] args)
        {
            _client = new client();

            login _login = new login();
            //signin _signin = new signin();
            _client.modulemanager.add_module("login", _login);
            //_client.modulemanager.add_module("signin", _signin);

            Int64 tick = Environment.TickCount;
            _client.connect_server("127.0.0.105", 3236, tick);

            _client.onConnectGate += onGeteHandle;
            _client.onConnectHub += onConnectHub;
            Int64 tickcount = 0;
            while (true)
            {
                Int64 tmptick = (Environment.TickCount & UInt32.MaxValue);
                if (tmptick < tick)
                {
                    tickcount += 1;
                    tmptick = tmptick + tickcount * UInt32.MaxValue;
                }
                tick = tmptick;

                _client.poll();

                tmptick = (Environment.TickCount & UInt32.MaxValue);
                if (tmptick < tick)
                {
                    tickcount += 1;
                    tmptick = tmptick + tickcount * UInt32.MaxValue;
                }
                Int64 ticktime = (tmptick - tick);
                tick = tmptick;

                if (ticktime < 50)
                {
                    Thread.Sleep(15);
                }
            } 
        }

        private static void onGeteHandle()
        {
            Console.WriteLine("11111111111");
            _client.connect_hub("lobby");
        }

        private static void onConnectHub(string hub_name)
        {
            Console.WriteLine("2222222222222");
            _client.call_hub("lobby", "login", "player_login", "qianqians");
        }


    }
}
