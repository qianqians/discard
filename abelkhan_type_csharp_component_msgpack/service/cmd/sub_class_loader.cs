using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace abelkhan.cmd
{
    public class SubClassLoader
    {
        public static Dictionary<string, Type> LoadClassBasisOfSub<T>(string _assemblyName) where T : IName
        {
            try
            {
                Dictionary<string, Type> CmdDict = new Dictionary<string, Type>();
                foreach (Type t in Assembly.Load(_assemblyName).GetTypes())
                {
                    if (t.IsSubclassOf(typeof(T)))
                    {
                        T cmd = (T)Activator.CreateInstance(t);
                        CmdDict.Add(cmd.GetName(), t);
                    }
                }

                return CmdDict;
            }
            catch (Exception e)
            {
                log.error(new System.Diagnostics.StackFrame(true), timerservice.Tick, "LoadClassBasisOfSub->{0} error->{1}", _assemblyName, e);
                throw e;
            }
        }
        public static Dictionary<string, Type> LoadClassBasisOfSub<T>(List<string> _assemblyNames) where T : IName
        {
            Dictionary<string, Type> CmdDict = new Dictionary<string, Type>();
            foreach (string name in _assemblyNames)
            {
                Dictionary<string, Type>  _cmdDict = LoadClassBasisOfSub<T>(name);
                CmdDict = CmdDict.Concat(_cmdDict).ToDictionary(k => k.Key, v => v.Value);
            }

            return CmdDict;
        }
    }

    public interface IName
    {
        string GetName();
    }
}
