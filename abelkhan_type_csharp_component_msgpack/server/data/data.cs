using abelkhan;
using System;
using System.Threading;

namespace data
{
    class data
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
            _hub.on_connect_db += () => {
                //_hub._dbproxyproxy.getCollection("trinityserver", "role").createPersistedObject("{\"roleId\":1, \"lv\":1}", (is_save_sucess)=> {
                //    log.trace(new System.Diagnostics.StackFrame(), timerservice.Tick, "createPersistedObject sucess");
                //});
                _hub._dbproxyproxy.getCollection("trinityserver", "role").getObjectInfoEx("{\"roleId\":1}", 1, 2, (str)=> {
                    log.trace(new System.Diagnostics.StackFrame(), timerservice.Tick, "object info:{0}", str);
                }, ()=> { 
                });
            };

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
