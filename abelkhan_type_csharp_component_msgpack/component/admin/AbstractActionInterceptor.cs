using System;
using System.Collections.Generic;
using System.Text;
using EvHttpSharp;

namespace abelkhan.admin
{
    public abstract class AbstractActionInterceptor
    {
        public abstract bool intercept(EvHttpSessionState session, EventHttpRequest req);

        public abstract bool afterIntercept<T>(EvHttpSessionState session, EventHttpRequest req, T resp) where T: IResponse;
    }
}
