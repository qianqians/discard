using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Threading;
using EvHttpSharp;

namespace abelkhan
{
    public class evHttp
    {
        EventHttpMultiworkerListener _listener;
        string _host;
        int _port;

        private Dictionary<string, Action<EventHttpRequest>> callbacks;

        public static void Init()
        {
            LibLocator.Init(null);
        }

        public evHttp(string host, int port, int workers)
        {
            _listener = new EventHttpMultiworkerListener(RequestHandler, workers);
            _host = host;
            _port = port;

            callbacks = new Dictionary<string, Action<EventHttpRequest>>();
        }

        public void Start()
        {
            _listener.Start(_host, (ushort)_port);
        }

        public void post(string uri, Action<EventHttpRequest> callback)
        {
            callbacks.Add(uri, callback);
        }

        private void RequestHandler(EventHttpRequest req)
        {
            ThreadPool.QueueUserWorkItem(_ =>
            {
                try
                {
                    if (!callbacks.TryGetValue(req.Uri, out Action<EventHttpRequest> cb))
                    {
                        log.error(new System.Diagnostics.StackFrame(), timerservice.Tick, "unhandle req exception ip:{0}, uri:{1}", req.UserHostAddress, req.Uri);
                        return;
                    }

                    cb(req);
                }
                catch(System.Exception e)
                {
                    log.error(new System.Diagnostics.StackFrame(), timerservice.Tick, "exception ip:{0}", req.UserHostAddress);
                    log.error(new System.Diagnostics.StackFrame(), timerservice.Tick, "error info:{0}", e.Message);
                }
            });
        }

    }
}
