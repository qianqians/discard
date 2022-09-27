using abelkhan;
using System;
using System.Threading;

namespace center_server
{
    class center_server
    {
        static void UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = e.ExceptionObject as Exception;
            abelkhan.log.error(new System.Diagnostics.StackFrame(true), timerservice.Tick, "unhandle exception:{0}", ex.Message);
        }

        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += UnhandledException;

            var _center = new abelkhan.center(args[0], args[1]);

            while (!_center._closeHandle.is_close)
            {
                try
                {
                    var tick = _center.poll();
                    if (tick < 50)
                    {
                        Thread.Sleep(5);
                    }
                }
                catch (System.Exception e)
                {
                    abelkhan.log.error(new System.Diagnostics.StackFrame(true), _center._timer.refresh(), "error:{0}", e.Message);
                }
            }
        }
    }
}
