using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Threading;
using EvHttpSharp;
using System.Text;
using abelkhan.admin.helper;

namespace abelkhan.admin
{
    public class AdminEvHttp : ISessionManager
    {
        EventHttpMultiworkerListener _listener;
        string _host;
        int _port;

        private Dictionary<string, EvHttpSessionState> sessions;

        private Dictionary<string, string> userToSession;

        public static void Init()
        {
            LibLocator.Init(null);
            ContextLoader.init();
        }

        public AdminEvHttp(string host, int port, int workers)
        {
            _listener = new EventHttpMultiworkerListener(RequestHandler, workers);
            _host = host;
            _port = port;

            sessions = new Dictionary<string, EvHttpSessionState>();
            userToSession = new Dictionary<string, string>();
        }

        public void Start()
        {
            sessions = new Dictionary<string, EvHttpSessionState>();

            _listener.Start(_host, (ushort)_port);
            log.trace(new System.Diagnostics.StackFrame(), timerservice.Tick, "Http Start {0}:{1}", _host, _port);
        }

        private void RequestHandler(EventHttpRequest req)
        {
            ThreadPool.QueueUserWorkItem(_ =>
            {
                try
                {
                    string token = EvHttpHelper.TryGetHeader(req.Headers, "X-Token");
                    EvHttpSessionState session = getEvHttpSession(token);
                    // log.error(new System.Diagnostics.StackFrame(), timerservice.Tick, "token info:{0}->{1}", token, session.UserId);
                    IEvHttpHandler handler;
                    if ("options".Equals(req.Method) || "OPTIONS".Equals(req.Method))
                    {
                        handler = new OptionsHandler();
                    }
                    else {
                        ActionMapper action = ContextLoader.getAction(req.Uri);
                        if (action != null)
                        {
                            handler = action.CreateHandler(session, req, ContextLoader.getAutoWired());
                        }
                        else
                        {
                            handler = new NotFoundHandler();
                        }
                    }

                    handler.ProcessRequest(session, req);
                }
                catch (Exception e)
                {
                    log.error(new System.Diagnostics.StackFrame(), timerservice.Tick, "exception ip:{0}", req.UserHostAddress);
                    log.error(new System.Diagnostics.StackFrame(), timerservice.Tick, "error info:{0}", e);
                }
            });
        }

        public EvHttpSessionState getEvHttpSession(string sessionId) {
            return sessions.GetValueOrDefault(sessionId, defaultSessionState());
        }

        public EvHttpSessionState defaultSessionState() {
            return new EvHttpSessionState(System.Guid.NewGuid().ToString("N"), timerservice.Tick, this);
        }

        public void AddSession(EvHttpSessionState session) {
            BindSession(session);
            if (!sessions.ContainsKey(session.SessionId))
                sessions.Add(session.SessionId, session);
        }

        public bool BindSession(EvHttpSessionState session) {
            if (userToSession.ContainsKey(session.UserId))
            {
                return false;
            }
            else
            {
                userToSession.Add(session.UserId, session.SessionId);
                return true;
            }
        }

        public bool IsBindSession(string userId)
        {
            return userToSession.ContainsKey(userId);
        }

        public void RemoveSession(string sessionId)
        {
            sessions.Remove(sessionId);
        }

        public bool KickSession(string userId) {
            if (userToSession.ContainsKey(userId))
            {
                string sessionId = userToSession.GetValueOrDefault(userId);
                userToSession.Remove(userId);
                sessions.Remove(sessionId);
                return true;
            }
            else {
                return false;
            }
        }
    }
}
