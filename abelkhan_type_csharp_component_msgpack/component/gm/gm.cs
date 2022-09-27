/*
 * gm
 * 2020/6/3
 * qianqians
 */
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Net;
using System.Threading.Tasks;

namespace abelkhan
{
    public class center_proxy
    {
        private gm_center_caller _gm_center_caller;

        public center_proxy(Ichannel ch, modulemng modules)
        {
            _gm_center_caller = new gm_center_caller(ch, modules);
        }

        public void confirm_gm(string gm_name)
        {
            _gm_center_caller.confirm_gm(gm_name);
        }

        public void close_clutter(string gm_name)
        {
            _gm_center_caller.close_clutter(gm_name);
        }

        public void reload(string gm_name)
        {
            _gm_center_caller.reload(gm_name);
        }

        public Task<string> req_cmd(string cmd, string param)
        {
            var t = new TaskCompletionSource<string>();
            _gm_center_caller.req_cmd(cmd, param).callBack((s) => t.TrySetResult(s), () => t.TrySetResult("req_cmd error") );
            return t.Task;
        }
    }

    public class gm
    {
        public string _gm_name;
        public center_proxy _center_proxy;
        private modulemng modules;
        private rawchannel ch;
        private timerservice _timer;
        public gm_cmd_dispatcher _cmd_dispatcher;

        public gm(string cfg_file, string cfg_name, string gm_name)
        {
            _gm_name = gm_name;
            modules = new modulemng();
            _timer = new timerservice();

            var _root_cfg = new config(cfg_file);
            var _config = _root_cfg.get_value_dict(cfg_name);

            var ip = _config.get_value_string("gm_ip");
            var port = _config.get_value_int("gm_port");
            var s = connectservice.connect(IPAddress.Parse(ip), (short)port);
            ch = new rawchannel(s);
            _center_proxy = new center_proxy(ch, modules);
            _center_proxy.confirm_gm(gm_name);

            string hub_type = _config.get_value_string("hub_type");
            if (!string.IsNullOrEmpty(hub_type))
            {
                _cmd_dispatcher = new gm_cmd_dispatcher(this);
                _cmd_dispatcher.StartUp(new List<string>(new string[] { "gm", hub_type }));
            }
        }

        public ICmd GetCmd(string name) {
            return CmdLoader.CreateCmd(name);
        }

        private void output_cmd()
        {
            Console.WriteLine("Enter gm cmd:");
            Console.WriteLine(" close-----close clutter");
            Console.WriteLine(" reload-----reload hub");
            Console.WriteLine(" q----quit");
        }

        public Int64 poll()
        {
            var tick_begin = _timer.refresh();
            try
            {
                _timer.poll();

                while (true)
                {
                    var ev = ch._channel_onrecv.pop();
                    if (ev == null)
                    {
                        break;
                    }

                    modules.process_event(ch, ev);
                }
            }
            catch (AbelkhanException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (System.Exception e)
            {
                Console.WriteLine("{0}", e);
            }

            Int64 tick_end = _timer.refresh();
            return tick_end - tick_begin;
        }

        public static void Main(string[] args)
        {
            if(args.Length <= 0)
            {
                Console.WriteLine("non input start argv");
                return;
            }

            string gm_name = null;
            string cmd = null;
            if (args.Length > 3)
            {
                gm_name = args[2];
                cmd = args[3];
            }
            else
            {
                Console.WriteLine("Enter gm name:");
                gm_name = Console.ReadLine();
            }

            var _gm = new gm(args[0], args[1], gm_name);
            CmdLoader.LoadCmd("gm");

            bool runing = true;
            while (runing)
            {
                var tmp = _gm.poll();

                if (cmd != null)
                {
                    ICmd instance = CmdLoader.CreateCmd(cmd);
                    if (instance != null)
                    {
                        instance.DoCmd(_gm, "");
                    }
                    else
                    {
                        Console.WriteLine("invalid gm cmd!");
                    }

                    System.Threading.Thread.Sleep(1500);
                    runing = false;
                }
                else
                {
                    _gm.output_cmd();

                    string cmd1 = Console.ReadLine();
                    ICmd instance = CmdLoader.CreateCmd(cmd1);
                    if (instance != null)
                    {
                        instance.DoCmd(_gm, "");
                    }
                    else if (cmd1 == "q")
                    {
                        runing = false;
                    }
                    else
                    {
                        Console.WriteLine("invalid gm cmd!");
                    }
                }

                if (tmp < 50)
                {
                    System.Threading.Thread.Sleep(15);
                }
            }
        }
    }
}
