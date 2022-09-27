using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hub_server
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

            var ccallh_module = new rsp.ccallh_module(_hub);
            var hcallc_caller = new ntf.hcallc();
            var hcallh_caller = new req.hcallh();

            ccallh_module.onccallh += () =>
            {
                Console.WriteLine("client connect!");

                if (abelkhan.Module.rsp != null)
                {
                    ((rsp.rsp_ccallh)abelkhan.Module.rsp).call("hello world!");
                }

                hcallh_caller.get_hub("hub_server0").hcallh().callBack(() => {
                    Console.WriteLine("ntf hub_server0 client connect!");
                }, () => {
                    Console.WriteLine("error!");
                });

                hcallc_caller.get_client(hub.hub.gates.current_client_uuid).hcallc("again hello world!");
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
