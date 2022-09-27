using System;
using System.Collections.Generic;
using System.Text;
using EvHttpSharp;

namespace abelkhan.admin
{

    public interface IEvHttpHandler
    {
        bool IsReusable { get; }

        void ProcessRequest(EvHttpSessionState session, EventHttpRequest req);

        Dictionary<string, string> getHeaders();
    }
}
