using System;
using System.Collections.Generic;
using System.Threading;
using abelkhan;

namespace http_admin
{
    public class http_admin
    {

        static void UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = e.ExceptionObject as Exception;
            log.error(new System.Diagnostics.StackFrame(true), abelkhan.timerservice.Tick, "unhandle exception:{0}", ex.Message);
        }

        public static abelkhan.hub _hub;
        public static abelkhan.admin.AdminEvHttp _http;
        public static string adminKey;
        public static abelkhan.gm _gm;
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += UnhandledException;

            _hub = new abelkhan.hub(args[0], args[1]);
            _hub.on_close += () => {
                _hub.close();

                _hub._timer.addticktime(1000, (Int64 tick) =>
                {
                    System.Environment.Exit(0);
                });
            };

            abelkhan.admin.AdminEvHttp.Init();
            var _http_out_host = _hub._config.get_value_string("http_out_host");
            var _http_out_port = _hub._config.get_value_int("http_out_port");
            _http = new abelkhan.admin.AdminEvHttp(_http_out_host, _http_out_port, 4);

            abelkhan.admin.ContextLoader.addAssemblyName("http_admin");
            abelkhan.admin.ContextLoader.Startup();
            adminKey = Guid.NewGuid().ToString("N");
            log.trace(new System.Diagnostics.StackFrame(true), _hub._timer.refresh(), "http adminKey:{0}", adminKey);
            
            _gm = new abelkhan.gm(args[0], args[1], "http_admin");
            CmdLoader.StartUp(new List<string>(new string[] { "gm", "http_admin" }));

            try
            {
                _http.Start();
            }
            catch (System.Exception e)
            {
                log.error(new System.Diagnostics.StackFrame(true), _hub._timer.refresh(), "http error:{0}", e.Message);
            }

            while (!_hub._closehandle.is_close)
            {
                try
                {
                    var gmTick = _gm.poll();
                    var tick = _hub.poll();
                    if (tick < 50 && gmTick < 50)
                    {
                        Thread.Sleep(5);
                    }
                }
                catch (System.Exception e)
                {
                    log.error(new System.Diagnostics.StackFrame(true), _hub._timer.refresh(), "poll error:{0}", e.Message);
                }
            }

        }
    }
}
