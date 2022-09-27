using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace client_il2cpp
{
    class client_il2cpp
    {
        public static void Main()
        {
            var _client = new client.client(123456);

            var ccallh_handle = new req.ccallh(_client);
            var hcallc_handle = new imp.hcallc_module(_client);
            hcallc_handle.onhcallc += (string text) =>
            {
                Console.WriteLine(text);
            };

            if (_client.connect_server("127.0.0.1", 3236))
            {
                _client.onConnectGate += () =>
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
                        ccallh_handle.get_hub("hub_server").ccallh().callBack((string text) => {
                            Console.WriteLine(text);
                        }, () => {
                            Console.WriteLine("error");
                        });
                    }
                };
            }

            while (true)
            {
                if (_client.poll() < 50)
                {
                    System.Threading.Thread.Sleep(5);
                }
            }
        }
    }
}
