using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hub_server0
{
    class server
    {
        public static void Main(string[] args)
        {
            if (args.Length <= 0)
            {
                return;
            }

            hub.hub _hub = new hub.hub(args);

            var hcallh_module = new rsp.hcallh_module(_hub);

            hcallh_module.onhcallh += () => {
                if (abelkhan.Module.rsp != null)
                {
                    ((rsp.rsp_hcallh)abelkhan.Module.rsp).call();
                }

                Console.WriteLine("client connect!");
            };

            while (true)
            {
                if (hub.hub.closeHandle.is_close)
                {
                    log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "server closed, hub server " + hub.hub.uuid);
                    break;
                }

                if (_hub.poll() < 50)
                {
                    System.Threading.Thread.Sleep(5);
                }
            }
        }
    }
}
