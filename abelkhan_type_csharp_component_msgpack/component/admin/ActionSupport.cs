using System;
using System.Collections.Generic;
using System.Text;
using abelkhan.admin.helper;
using EvHttpSharp;

namespace abelkhan.admin
{
    public class ActionSupport
    {
        public EvHttpSessionState Session { set; get; } 
        public EventHttpRequest Req { set; get; }

        public T getParams<T>()
        {
            string bodyStr = Encoding.UTF8.GetString(Req.RequestBody);
            // abelkhan.log.trace(new System.Diagnostics.StackFrame(true), abelkhan.timerservice.Tick, "user bodyStr- {0}, {1}, {2}, {3}", bodyStr, Req.RequestBody.Length, Req.Uri, Req.Method);
            return JSONHelper.deserialize<T>(bodyStr);
        }

        public string getParams() { 
            return Encoding.UTF8.GetString(Req.RequestBody);
        }
    }
}
