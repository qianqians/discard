using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace abelkhan.cmd
{
    public class CmdContainer
    {
        private Dictionary<string, Type> CmdDict;

        public CmdContainer(Dictionary<string, Type> _cmdDict) {
            CmdDict = _cmdDict;
        }

        public CmdContainer(): this(new Dictionary<string, Type>()) {
        }

        public T CreateGmCmd<T>(string name)
        {
            // log.trace(new System.Diagnostics.StackFrame(true), timerservice.Tick, "CreateGmCmd:{0}, Key:{1}-{2}", name, key, CmdDict.ContainsKey(name));
            if (CmdDict.ContainsKey(name))
            {
                Type t = CmdDict.GetValueOrDefault(name);
                T cmd = (T)Activator.CreateInstance(t);
                return cmd;
            }
            return default;
        }

        public void AddAll(Dictionary<string, Type> _cmdDict) {
            CmdDict = CmdDict.Concat(_cmdDict).ToDictionary(k => k.Key, v => v.Value);
        }
    }
}
