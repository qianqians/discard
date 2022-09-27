using System;
using System.Reflection;
using System.Collections.Generic;
using EvHttpSharp;

namespace abelkhan.admin
{
    public class ActionMapper
    {
        private MethodInfo m_info;

        public ActionMapper(MethodInfo m, string route, Type baseType, Permission permission) : this(m, route, baseType, permission, "Post") { 
        }

        public ActionMapper(MethodInfo m, string route, Type baseType, Permission permission, string method)
        {
            this.m_info = m;
            this.Route = route;
            this.BaseType = baseType;
            this.Method = method;
            this.Permission = permission;
            mapperReturnType(m.ReturnType);
        }

        public string Method { get; set; }

        public string Route { get; set; }

        public Type BaseType { get; set; }

        public string ReturnType { get; set; }

        /// <summary>
        /// 接口权限
        /// </summary>
        public Permission Permission { set; get; }

        internal void mapperReturnType(Type return_type)
        {
            if (return_type.Equals(typeof(ModelAndView)))
                this.ReturnType = "view";
            else if (return_type.Equals(typeof(JSON)))
                this.ReturnType = "json";
            else if (return_type.Equals(typeof(StringWp)))
                this.ReturnType = "StringWp";
            else
                this.ReturnType = "void";
        }


        internal IEvHttpHandler CreateHandler(EvHttpSessionState session, EventHttpRequest req, Dictionary<Type, Type> dictionary)
        {
            ActionSupport obj = (ActionSupport)Activator.CreateInstance(this.BaseType);
            obj.Session = session;
            obj.Req = req;
            session.CurAction = this;
            FieldInfo[] fields = this.BaseType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            if (fields != null && fields.Length > 0)
            {
                object obj2;
                foreach (FieldInfo f in fields)
                {
                    obj2 = getAttribute(f, typeof(AutoWired));
                    if (obj2 != null)
                    {
                        f.SetValue(obj, Activator.CreateInstance(dictionary[f.FieldType]));
                    }
                }
            }
          
            if (this.ReturnType.Equals("json"))
                return new JsonHandler(this.m_info, obj);
            if (this.ReturnType.Equals("void"))
                return new VoidHandler(this.m_info, obj);
            if (this.ReturnType.Equals("StringWp"))
                return new StringHandler(this.m_info, obj);
            return null;
        }

        private static object getAttribute(FieldInfo f, Type type)
        {
            object[] objs = f.GetCustomAttributes(type, false);
            if (objs != null && objs.Length > 0)
            {
                return objs[0];
            }
            return null;
        }
    }
}
