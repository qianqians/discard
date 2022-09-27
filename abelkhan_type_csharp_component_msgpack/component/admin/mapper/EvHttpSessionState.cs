using System;
using System.Collections.Generic;
using System.Text;

namespace abelkhan.admin
{
    public class EvHttpSessionState
    {
        public string SessionId { set; get; }

        public string Token { set; get; }

        public string UserId { set; get; }

        public Int64 TimeTick { set; get; }

        public string Roles { set; get; }

        public ActionMapper CurAction { set; get; }

        public Dictionary<string, object> keys;

        public ISessionManager sessionManager;
        
        public List<string> Permissions { set; get; }

        public List<string> Routes { set; get; }

        public Int64 TIME_INTERVAL = 60 * TimeSpan.TicksPerMinute / TimeSpan.TicksPerMillisecond;

        public void setKey(string key, object obj) {
            keys.Add(key, obj);
        }

        public T getObj<T>(string key) {
            return (T)keys.GetValueOrDefault(key);
        }

        public EvHttpSessionState(string token, Int64 timeTick) {
            this.Token = token;
            this.TimeTick = timeTick;
        }

        public EvHttpSessionState(string sessionId, Int64 timeTick, ISessionManager _sessionManager)
        {
            this.TimeTick = timeTick;
            this.SessionId = sessionId;
            this.sessionManager = _sessionManager;
        }

        public void Login(string username) {
            if (sessionManager.IsBindSession(username)) {
                sessionManager.KickSession(username);
            }

            UserId = username;
            this.TimeTick = timerservice.Tick;
            sessionManager.AddSession(this);
        }

        public void Logout()
        {
            if (sessionManager.IsBindSession(UserId))
            {
                sessionManager.KickSession(UserId);
            }

            UserId = null;
            sessionManager.RemoveSession(SessionId);
        }

        public bool TimeOut() {
            return timerservice.Tick - TimeTick > TIME_INTERVAL;
        }

        public bool RefreshTimeTick()
        {
            TimeTick = timerservice.Tick;
            return true;
        }

        public bool IsLogin() {
            return UserId != null && !"".Equals(UserId) && sessionManager.IsBindSession(UserId);
        }
    }
}
