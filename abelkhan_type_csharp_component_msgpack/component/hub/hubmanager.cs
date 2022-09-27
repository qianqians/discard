using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace abelkhan
{

    public class hubproxy
    {
        public abelkhan.Ichannel ch;
        public string hub_name;
        public string hub_type;
        public hub_call_hub_caller _hub_caller;

        public hubproxy(abelkhan.Ichannel _ch, modulemng _modules, string _hub_name, string _hub_type)
        {
            ch = _ch;
            hub_name = _hub_name;
            hub_type = _hub_type;
            _hub_caller = new hub_call_hub_caller(ch, _modules);
        }

        public Task<string> req_hub_cmd(string cmd, string param)
        {
            var t = new TaskCompletionSource<string>();
            _hub_caller.req_hub_cmd(cmd, param).callBack((s) => t.TrySetResult(s), () => t.TrySetResult("req_cmd error"));
            return t.Task;
        }
    }

    public class hubmanager
    {
        private modulemng modules;
        private Dictionary<string, hubproxy> hubproxys;
        private Dictionary<string, HashSet<string>> hubTypeToNames;
        public hubmanager(modulemng _modules)
        {
            modules = _modules;
            hubproxys = new Dictionary<string, hubproxy>();
            hubTypeToNames = new Dictionary<string, HashSet<string>>();
        }

        public hubproxy reg_hub(abelkhan.Ichannel ch, string hub_type, string _name)
        {
            var _hubproxy = new hubproxy(ch, modules, hub_type, _name);
            hubproxys.Add(_name, _hubproxy);
            HashSet<string> nameSet = hubTypeToNames.GetValueOrDefault(hub_type, new HashSet<string>());
            nameSet.Add(_name);
            return _hubproxy;
        }

        public void hub_be_closed(string name)
        {
            if (hubproxys.TryGetValue(name, out hubproxy _proxy))
            {
                hubproxys.Remove(name);
                if (hubTypeToNames.TryGetValue(_proxy.hub_type, out HashSet<string> nameSet))
                {
                    if (nameSet != null && nameSet.Contains(name))
                    nameSet.Remove(name);
                }
                _proxy.ch.disconnect();
            }
        }

        public hubproxy get_hub(string name)
        {
            // log.trace(new System.Diagnostics.StackFrame(true), timerservice.Tick, "get_hub:{0}-{1}", name, hubproxys.Count);
            if (hubproxys.TryGetValue(name, out hubproxy _proxy))
            {
                return _proxy;
            }

            return null;
        }

        public hubproxy random_hub(string _hub_type) {
            hubTypeToNames.TryGetValue(_hub_type, out HashSet<string> nameSet);
            if (nameSet == null || nameSet.Count <= 0) {
                return null;
            }
            string name = nameSet.ElementAt<string>(RandomHelper.RandomInt(nameSet.Count));
            return get_hub(name);
        }

        public hubproxy modulo_hub(string _hub_type, int num)
        {
            hubTypeToNames.TryGetValue(_hub_type, out HashSet<string> nameSet);
            if (nameSet == null || nameSet.Count <= 0)
            {
                return null;
            }
            string name = nameSet.ElementAt<string>(num % nameSet.Count);
            return get_hub(name);
        }
    }
}