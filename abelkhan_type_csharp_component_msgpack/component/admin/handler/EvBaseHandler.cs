using System;
using System.Collections.Generic;
using System.Reflection;
using EvHttpSharp;
using abelkhan.admin.helper;

namespace abelkhan.admin
{
    public abstract class EvBaseHandler<T> : IEvHttpHandler where T: IResponse
    {
        protected MethodInfo method;
        protected ActionSupport baseType;
        protected List<AbstractActionInterceptor> interceptorList = new List<AbstractActionInterceptor>();

        public bool doInterceptor(EvHttpSessionState session, EventHttpRequest req)
        {
            List<Type> interceptors = ContextLoader.getInterceptors();
            AbstractActionInterceptor interceptor;
            foreach (Type t in interceptors)
            {
                interceptor = (AbstractActionInterceptor)Activator.CreateInstance(t);
                if (!interceptor.intercept(session, req))
                    return false;
                interceptorList.Add(interceptor);
            }
            return true;
        }
        public bool doAfterInterceptor(EvHttpSessionState session, EventHttpRequest req, T resp)
        {
            foreach (AbstractActionInterceptor interceptor in interceptorList)
            {
                if (!interceptor.afterIntercept<T>(session, req, resp))
                    return false;
            }
            return true;
        }


        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(EvHttpSessionState session, EventHttpRequest req)
        {
            if (doInterceptor(session, req) && method != null)
                process(session, req);
        }

        public Dictionary<string, string> getHeaders() {
            return EvHttpHelper.buildCrossHeaders();
        }

        public abstract void process(EvHttpSessionState session, EventHttpRequest req);

        public void ProcessResponse(EvHttpSessionState session, EventHttpRequest req, T resp) {
            if (doAfterInterceptor(session, req, resp))
                response(session, req, resp);
        }
        public abstract void response(EvHttpSessionState session, EventHttpRequest req, T resp);
    }
}
