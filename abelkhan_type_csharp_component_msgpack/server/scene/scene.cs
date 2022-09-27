using System;
using System.Threading;

namespace scene
{
    class scene
    {
        static void UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = e.ExceptionObject as Exception;
            abelkhan.log.error(new System.Diagnostics.StackFrame(true), abelkhan.timerservice.Tick, "unhandle exception:{0}", ex.Message);
        }

        public static abelkhan.hub _hub;

        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += UnhandledException;

            _hub = new abelkhan.hub(args[0], args[1]);
            _hub.on_close += () => {
                _hub.close();
            };
            var _client_msg_handle = new client_msg_handle(_hub.modules);
            singleton.Init();

            while (!_hub._closehandle.is_close)
            {
                try
                {
                    var tick = _hub.poll();
                    if (tick < 50)
                    {
                        Thread.Sleep(5);
                    }
                }
                catch (System.Exception e)
                {
                    abelkhan.log.error(new System.Diagnostics.StackFrame(true), _hub._timer.refresh(), "poll error:{0}", e.Message);
                }
            }
        }
    }
}
