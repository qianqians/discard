/*
 * hubmanager
 * 2020/6/3
 * qianqians
 */

using System.Collections.Generic;
using System.Reflection;

namespace abelkhan
{
	public class hubproxy
	{
		public abelkhan.Ichannel ch;

		private string hub_name;
		private dbproxy_call_hub_caller _caller;

		public hubproxy(Ichannel _ch, string _hub_name, modulemng modules)
		{
			ch = _ch;
			hub_name = _hub_name;
			_caller = new dbproxy_call_hub_caller(_ch, modules);
		}

		public void reg_hub_sucess()
		{
			_caller.reg_hub_sucess();
		}

		public void ack_create_persisted_object(string cbid, bool is_create_sucess)
		{
			_caller.ack_create_persisted_object(cbid, is_create_sucess);
		}

		public void ack_updata_persisted_object(string callbackid, bool is_update_sucess)
		{
			_caller.ack_updata_persisted_object(callbackid, is_update_sucess);
		}

		public void ack_get_object_count(string callbackid, int count)
		{
			_caller.ack_get_object_count(callbackid, count);
		}
		
		public void ack_get_object_info(string callbackid, string json_str_object_info_list)
		{
			_caller.ack_get_object_info(callbackid, json_str_object_info_list);
		}

		public void ack_get_object_info_end(string callbackid)
		{
			_caller.ack_get_object_info_end(callbackid);
		}

		public void ack_remove_object(string callbackid, bool is_remove_sucess)
		{
			_caller.ack_remove_object(callbackid, is_remove_sucess);
		}
	}

	public class hubmanager
	{
		private modulemng modules;
		private Dictionary<Ichannel, hubproxy> hubproxys;
		private Dictionary<string, hubproxy> hubproxy_names;

		public hubmanager(modulemng _modules)
		{
			modules = _modules;
			hubproxys = new Dictionary<Ichannel, hubproxy>();
			hubproxy_names = new Dictionary<string, hubproxy>();
		}

		public hubproxy reg_hub(Ichannel ch, string svr_name)
		{
			var _proxy = new hubproxy(ch, svr_name, modules);
			hubproxys.Add(ch, _proxy);
			hubproxy_names.Add(svr_name, _proxy);
			return _proxy;
		}

		public void on_hub_closed(string name)
        {
			if (hubproxy_names.TryGetValue(name, out hubproxy _proxy))
            {
				hubproxy_names.Remove(name);
				hubproxys.Remove(_proxy.ch);
			}
		}

		public hubproxy get_hub(Ichannel ch)
		{
			if (hubproxys.TryGetValue(ch, out hubproxy _proxy))
			{
				return _proxy;
			}

			return null;
		}
	}
}