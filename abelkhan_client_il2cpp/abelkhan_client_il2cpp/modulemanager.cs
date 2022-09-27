using System;
using System.Collections;

namespace common
{
	public class modulemanager
	{
		public modulemanager()
		{
			modules = new Hashtable();
		}

		public void add_module(String module_name, imodule _module)
		{
			modules.Add(module_name, _module);
		}

		public void process_module_mothed(String module_name, String func_name, ArrayList argvs)
		{
            if (modules.ContainsKey(module_name))
			{
				imodule _module = (imodule)modules[module_name];
				try
				{
                    var method = _module.get_event(func_name);
                    method(argvs);
				}
				catch (Exception e)
                {
                    log.log.error(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "call rpc error, function name:{0} System.Exception:{1}, agrv:{2}", func_name, e, Json.Jsonparser.pack(argvs));
				}
			}
			else
            {
                log.log.error(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "do not have a module name:{0}", module_name);
			}
		}

		private Hashtable modules;
	}
}

