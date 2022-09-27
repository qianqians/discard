using abelkhan.admin;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using abelkhan;

namespace abelkhan.admin
{
    public class ContextLoader
    {
        // 保存PageAction的字典
        private static Dictionary<string, ActionMapper> s_PageActionDict;
        private static List<Type> s_Interceptors;
        private static List<Type> s_AllField;
        private static List<Type> s_AllClass;
        private static Dictionary<Type, Type> s_AutoWired;
        private static List<string> s_AssemblyName;

        public static void init() {
            if (s_PageActionDict == null)
                s_PageActionDict = new Dictionary<string, ActionMapper>();
            if (s_Interceptors == null)
                s_Interceptors = new List<Type>();
            if (s_AllField == null)
                s_AllField = new List<Type>();
            if (s_AllClass == null)
                s_AllClass = new List<Type>();
            if (s_AutoWired == null)
                s_AutoWired = new Dictionary<Type, Type>();

            if (s_AssemblyName == null)
            {
                s_AssemblyName = new List<string>();
                s_AssemblyName.Add("admin");
            }
        }

        public static void Startup()
        {
            init();

            foreach (string assemblyName in s_AssemblyName) {
                loadWithAssemblyName(assemblyName);
            }
            
            loadAutoWired();
            log.trace(new System.Diagnostics.StackFrame(true), timerservice.Tick, "started...  Actions => " + s_PageActionDict.Count);
        }

        public static void addAssemblyName(string assemblyName) {
            if (s_AssemblyName == null)
            {
                s_AssemblyName = new List<string>();
                s_AssemblyName.Add("admin");
            }

            s_AssemblyName.Add(assemblyName);
        }

        public static void loadAutoWired()
        {
            foreach (Type t in s_AllClass)
            {
                foreach (Type i in s_AllField)
                {
                    if (i.IsAssignableFrom(t))
                    {
                        s_AutoWired.Add(i, t);
                        break;
                    }
                }
            }
        }

        public static void loadWithAssemblyName(string assemblyName) {
            try
            {
                foreach (Type t in Assembly.Load(assemblyName).GetTypes())
                {
                    if (t.IsSubclassOf(typeof(ActionSupport)))
                    {
                        getActionMapper(t);
                    }
                    else if (t.IsSubclassOf(typeof(AbstractActionInterceptor)))
                    {
                        s_Interceptors.Add(t);
                    }
                    else
                    {
                        if (!t.IsInterface)
                            s_AllClass.Add(t);
                    }
                }
            }
            catch (System.Exception e)
            {
                log.error(new System.Diagnostics.StackFrame(true), timerservice.Tick, "loadWithAssemblyName->{0} error->{1}", assemblyName, e);
                throw e;
            }
        }

        private static void getActionMapper(Type t)
        {
            string package = "";
            object obj = getAttribute(t, typeof(Controller));
            if (obj != null)
            {
                package = ((Controller)obj).route;
            }

            MethodInfo[] methods = t.GetMethods();
            if (methods != null && methods.Length > 0)
            {
                object obj2;
                string url;
                foreach (MethodInfo m in methods)
                {
                    obj2 = getAttribute2(m, typeof(Post));
                    if (obj2 != null)
                    {
                        url = ((Post)obj2).route;
                        if (url.StartsWith("~"))
                            url = url.Substring(1);
                        else
                            url = package + url;

                        Permission permission;
                        object objPm = getAttribute2(m, typeof(RequirePermissions));
                        if (objPm != null)
                        {
                            permission = ((RequirePermissions)objPm).Permission;
                        }
                        else {
                            permission = Permission.BASE;
                        }

                        s_PageActionDict.Add(url, new ActionMapper(m, url, t, permission));
                    }
                }
            }
            FieldInfo[] fields = t.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            if (fields != null && fields.Length > 0)
            {
                object obj2;
                foreach (FieldInfo f in fields)
                {
                    obj2 = getAttribute(f, typeof(AutoWired));
                    if (obj2 != null)
                    {
                        s_AllField.Add(f.FieldType);
                    }
                }
            }
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

        private static object getAttribute2(MemberInfo m, Type type)
        {
            object[] objs = m.GetCustomAttributes(type, false);
            if (objs != null && objs.Length > 0)
            {
                return objs[0];
            }
            return null;
        }

        private static object getAttribute(Type t, Type type)
        {
            object[] objs = t.GetCustomAttributes(type, false);
            if (objs != null && objs.Length > 0)
            {
                return objs[0];
            }
            return null;
        }

        internal static ActionMapper getAction(string url)
        {
            if (s_PageActionDict.ContainsKey(url))
                return s_PageActionDict[url];
            return null;
        }

        internal static Dictionary<Type, Type> getAutoWired()
        {
            return s_AutoWired;
        }

        internal static List<Type> getInterceptors()
        {
            return s_Interceptors;
        }
    }
}
