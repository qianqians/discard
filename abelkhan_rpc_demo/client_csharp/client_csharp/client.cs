using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace client_csharp
{
    class ccallh : common.imodule
    {
        public void ccallh_rsp(string uuid, string str)
        {
            Console.WriteLine(str);
        }
    }

    class hcallc_module : common.imodule
    {
        public void hcallc(string str)
        {
            Console.WriteLine(str);
        }
    }

    class client_csharp
    {
        public static void Main()
        {
            var _client = new client.client(123456);

            _client.modulemanager.add_module("ccallh", new ccallh());
            _client.modulemanager.add_module("hcallc", new hcallc_module());

            if (_client.connect_server("127.0.0.1", 3236))
            {
                _client.onConnectGate += ()=>
                {
                    _client.connect_hub("hub_server");
                    _client.connect_hub("hub_server0");
                };

                List<string> hubs = new List<string>();
                _client.onConnectHub += (string hub_name) =>
                {
                    hubs.Add(hub_name);
                    if (hubs.Contains("hub_server") && hubs.Contains("hub_server0"))
                    {
                        _client.call_hub("hub_server", "ccallh", "ccallh", System.Guid.NewGuid().ToString());
                    }
                };
            }

            while(true)
            {
                if (_client.poll() < 50)
                {
                    System.Threading.Thread.Sleep(5);
                }
            }
        }
    }
}
