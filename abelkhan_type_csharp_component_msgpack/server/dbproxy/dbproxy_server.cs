using System;
using System.Threading;

namespace dbproxy_server
{
    class dbproxy_server
    {
        static void UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = e.ExceptionObject as Exception;
            abelkhan.log.error(new System.Diagnostics.StackFrame(true), abelkhan.timerservice.Tick, "unhandle exception:{0}", ex.Message);
        }

        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += UnhandledException;

            var _dbproxy = new abelkhan.dbproxy(args[0], args[1]);

            while (!_dbproxy._closeHandle.is_close)
            {
                try
                {
                
                    var tick = _dbproxy.poll();
                    if (tick < 50)
                    {
                        Thread.Sleep(5);
                    }
                }
                catch (System.Exception e)
                {
                    abelkhan.log.error(new System.Diagnostics.StackFrame(true), _dbproxy._timer.refresh(), "error:{0}", e.Message);
                }
            }
        }
    }
}
