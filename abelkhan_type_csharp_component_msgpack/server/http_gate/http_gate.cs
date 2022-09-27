using System;
using System.Threading;

namespace http_gate
{
    class http_gate
    {
        static void UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = e.ExceptionObject as Exception;
            abelkhan.log.error(new System.Diagnostics.StackFrame(true), abelkhan.timerservice.Tick, "unhandle exception:{0}", ex.Message);
        }

        public static abelkhan.hub _hub;
        public static http_helper httpHelper;
        
        private static abelkhan.evHttp _http;

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

            //var uuid = Guid.NewGuid().ToString("N");
            //var body = new byte[] { 0xff, 0xfe, 0xfd, 0xfc, 0xfb, 0xfa};
            //var _b = utils.Serialize(uuid, body);
            //var _ret = utils.Deserialize(_b);
            //if (_ret.rawData.Length == 6 &&
            //    body.Length == 6 &&
            //    _ret.rawData[0] == body[0] &&
            //    _ret.rawData[1] == body[1] &&
            //    _ret.rawData[2] == body[2] &&
            //    _ret.rawData[3] == body[3] &&
            //    _ret.rawData[4] == body[4] &&
            //    _ret.rawData[5] == body[5])
            //{
            //    Console.WriteLine("Serialize Deserialize ok!");
            //}
            //else
            //{
            //    Console.WriteLine("Serialize Deserialize faild!");
            //}

            abelkhan.evHttp.Init();
            var _http_out_host = _hub._config.get_value_string("http_out_host");
            var _http_out_port = _hub._config.get_value_int("http_out_port");
            _http = new abelkhan.evHttp(_http_out_host, _http_out_port, 4);
            httpHelper = new http_helper(_http);
            var _protocol = new test_protocol();
            try
            {
                _http.Start();
            }
            catch (System.Exception e)
            {
                abelkhan.log.error(new System.Diagnostics.StackFrame(true), _hub._timer.refresh(), "http error:{0}", e.Message);
            }

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
