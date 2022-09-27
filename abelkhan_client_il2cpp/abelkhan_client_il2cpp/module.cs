using System;
using System.Collections;
using System.Collections.Generic;

namespace common
{
	public class imodule
	{
        private Dictionary<string, Action<ArrayList> > events;

        public imodule()
        {
            events = new Dictionary<string, Action<ArrayList> >();
        }

        public void reg_event(string event_name, Action<ArrayList> method)
        {
            events.Add(event_name, method);
        }

        public Action<ArrayList> get_event(string event_name)
        {
            return events[event_name];
        }
	}
}

