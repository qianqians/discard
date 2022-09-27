using System;
using System.Collections;

namespace abelkhan
{
	public class hub_msg_handle
	{
        private dbevent _dbevent;
        private hubmanager _hubmanager;
        private mongodbproxy _mongodbproxy;
        private hub_call_dbproxy_module _module;

        public hub_msg_handle(modulemng modules, dbevent _dbevent_, hubmanager _hubmanager_, mongodbproxy _mongodbproxy_)
		{
            _dbevent = _dbevent_;
            _hubmanager = _hubmanager_;
			_mongodbproxy = _mongodbproxy_;

            _module = new hub_call_dbproxy_module(modules);
            _module.onreg_hub += reg_hub;
            _module.oncreate_persisted_object += create_persisted_object;
            _module.onupdata_persisted_object += updata_persisted_object;
            _module.onremove_object += remove_object;
            _module.onget_object_count += get_object_count;
            _module.onget_object_info += get_object_info;
            _module.onget_object_infoex += get_object_infoex;
        }

		public void reg_hub(string hub_name)
		{
            log.trace(new System.Diagnostics.StackFrame(true), timerservice.Tick, "hub {0} connected", hub_name);

			hubproxy _hubproxy = _hubmanager.reg_hub(_module.current_ch, hub_name);
			_hubproxy.reg_hub_sucess ();
		}

		public void create_persisted_object(string db, string collection, string object_info, string callbackid)
		{
            log.trace(new System.Diagnostics.StackFrame(true), timerservice.Tick, "begin create_persisted_object");

            hubproxy _hubproxy = _hubmanager.get_hub(_module.current_ch);
            if (_hubproxy == null)
            {
                log.error(new System.Diagnostics.StackFrame(true), timerservice.Tick, "hubproxy is null");
                return;
            }

            _dbevent.push_create_event(new create_event(_mongodbproxy, _hubproxy, db, collection, object_info, callbackid));

            log.trace(new System.Diagnostics.StackFrame(true), timerservice.Tick, "end create_persisted_object");
        }

		public void updata_persisted_object(string db, string collection, string query_json, string object_info, string callbackid)
		{
            log.trace(new System.Diagnostics.StackFrame(true), timerservice.Tick, "begin updata_persisted_object");

            hubproxy _hubproxy = _hubmanager.get_hub(_module.current_ch);
            if (_hubproxy == null)
            {
                log.error(new System.Diagnostics.StackFrame(true), timerservice.Tick, "hubproxy is null");
                return;
            }

            _dbevent.push_updata_event(new update_event(_mongodbproxy, _hubproxy, db, collection, query_json, object_info, callbackid));

            log.trace(new System.Diagnostics.StackFrame(true), timerservice.Tick, "end updata_persisted_object");
        }

        public void remove_object(string db, string collection, string query_json, string callbackid)
        {
            log.trace(new System.Diagnostics.StackFrame(true), timerservice.Tick, "begin remove_object");

            hubproxy _hubproxy = _hubmanager.get_hub(_module.current_ch);
            if (_hubproxy == null)
            {
                log.error(new System.Diagnostics.StackFrame(true), timerservice.Tick, "hubproxy is null");
                return;
            }

            _dbevent.push_remove_event(new remove_event(_mongodbproxy, _hubproxy, db, collection, query_json, callbackid));

            log.trace(new System.Diagnostics.StackFrame(true), timerservice.Tick, "end remove_object");
        }

        public void get_object_count(string db, string collection, string query_json, string callbackid)
        {
            log.trace(new System.Diagnostics.StackFrame(true), timerservice.Tick, "begin get_object_count");

            hubproxy _hubproxy = _hubmanager.get_hub(_module.current_ch);
            if (_hubproxy == null)
            {
                log.error(new System.Diagnostics.StackFrame(true), timerservice.Tick, "hubproxy is null");
                return;
            }

            _dbevent.push_count_event(new count_event(_mongodbproxy, _hubproxy, db, collection, query_json, callbackid));

            log.trace(new System.Diagnostics.StackFrame(true), timerservice.Tick, "end get_object_count");
        }

		public void get_object_info(string db, string collection, string query_json, string callbackid)
        {
            log.trace(new System.Diagnostics.StackFrame(true), timerservice.Tick, "begin get_object_info");

            hubproxy _hubproxy = _hubmanager.get_hub(_module.current_ch);
            if (_hubproxy == null)
            {
                log.error(new System.Diagnostics.StackFrame(true), timerservice.Tick, "hubproxy is null");
                return;
            }

            _dbevent.push_find_event(new find_event(_mongodbproxy, _hubproxy, db, collection, query_json, callbackid));

            log.trace(new System.Diagnostics.StackFrame(true), timerservice.Tick, "end get_object_info");
        }

        public void get_object_infoex(string db, string collection, string query_json, int skip, int limit, string callbackid)
        {
            log.trace(new System.Diagnostics.StackFrame(true), timerservice.Tick, "begin get_object_info");

            hubproxy _hubproxy = _hubmanager.get_hub(_module.current_ch);
            if (_hubproxy == null)
            {
                log.error(new System.Diagnostics.StackFrame(true), timerservice.Tick, "hubproxy is null");
                return;
            }

            _dbevent.push_findex_event(new findex_event(_mongodbproxy, _hubproxy, db, collection, query_json, skip, limit, callbackid));

            log.trace(new System.Diagnostics.StackFrame(true), timerservice.Tick, "end get_object_info");
        }
    }
}

