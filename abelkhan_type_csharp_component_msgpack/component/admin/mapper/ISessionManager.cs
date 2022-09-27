using System;
using System.Collections.Generic;
using System.Text;

namespace abelkhan.admin
{
    public interface ISessionManager
    {
        void AddSession(EvHttpSessionState session);

        void RemoveSession(string sessionId);

        bool BindSession(EvHttpSessionState session);

        bool IsBindSession(string userId);

        bool KickSession(string userId);
    }
}
