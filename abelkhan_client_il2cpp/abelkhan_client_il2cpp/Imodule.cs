using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

namespace juggle
{
    public class Imodule
    {
        public delegate void on_event(ArrayList _event);
        protected Dictionary<string, on_event> events;

        public void process_event(Ichannel _ch, ArrayList _event)
		{
			current_ch = _ch;
            try
            {
                String func_name = (String)_event[1];

                if (events.ContainsKey(func_name))
                {
                    on_event method = events[func_name];
                    try
                    {
                        method((ArrayList)_event[2]);
                        current_ch = null;
                    }
                    catch (Exception e)
                    {
                        throw new juggle.Exception(string.Format("function name:{0} System.Exception:{1}", func_name, e));
                    }
                }
                else
                {
                    throw new juggle.Exception(string.Format("do not have a function named::{0}", func_name));
                }
            }
            catch (Exception e)
            {
                throw new juggle.Exception(string.Format("System.Exception:{0}", e));
            }
        }

		public static Ichannel current_ch;
		public String module_name;
    }
}
