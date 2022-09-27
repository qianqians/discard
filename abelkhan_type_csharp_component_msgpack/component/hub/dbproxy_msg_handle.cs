/*
 * dbproxy_msg_handle
 * qianqians
 * 2020/6/4
 */
using System;

namespace abelkhan
{
	public class dbproxy_msg_handle
	{
		private dbproxyproxy _dbproxyproxy;
		private dbproxy_call_hub_module _dbproxy_call_hub_module;

		public dbproxy_msg_handle(modulemng modules, dbproxyproxy _proxy)
		{
			_dbproxyproxy = _proxy;

			_dbproxy_call_hub_module = new dbproxy_call_hub_module(modules);
			_dbproxy_call_hub_module.onreg_hub_sucess += reg_hub_sucess;
			_dbproxy_call_hub_module.onack_create_persisted_object += ack_create_persisted_object;
			_dbproxy_call_hub_module.onack_updata_persisted_object += ack_updata_persisted_object;
			_dbproxy_call_hub_module.onack_get_object_count += ack_get_object_count;
			_dbproxy_call_hub_module.onack_get_object_info += ack_get_object_info;
			_dbproxy_call_hub_module.onack_get_object_info_end += ack_get_object_info_end;
			_dbproxy_call_hub_module.onack_remove_object += ack_remove_object;
		}

		public event Action on_connect_db;
		public void reg_hub_sucess()
		{
			if (on_connect_db != null)
			{
				on_connect_db();
			}
		}

		public void ack_create_persisted_object(string callbackid, bool is_create_sucess)
		{
			Action<bool> _handle;
			if (_dbproxyproxy.create_callback.Remove(callbackid, out _handle))
			{
				_handle(is_create_sucess);
			}
		}

		public void ack_updata_persisted_object(string callbackid, bool is_updata_sucess)
		{
			Action<bool> _handle;
			if (_dbproxyproxy.update_callback.Remove(callbackid, out _handle))
			{
				_handle(is_updata_sucess);
			}
		}

		public void ack_get_object_count(string callbackid, int count)
		{
			Action<uint> _handle;
			if (_dbproxyproxy.count_callback.Remove(callbackid, out _handle))
			{
				_handle((uint)count);
			}
		}

		public void ack_get_object_info(string callbackid, string json_obejct_array)
		{
			if (_dbproxyproxy.obj_callback.ContainsKey(callbackid))
			{
				var _handle = _dbproxyproxy.obj_callback[callbackid];
				_handle(json_obejct_array);
			}
		}

		public void ack_get_object_info_end(string callbackid)
		{
			_dbproxyproxy.obj_callback.Remove(callbackid);

			Action _end;
			if (_dbproxyproxy.obj_end_callback.Remove(callbackid, out _end))
			{
				_end();
			}
		}

		public void ack_remove_object(string callbackid, bool is_del_sucess)
		{
			Action<bool> _handle;
			if (_dbproxyproxy.remove_callback.Remove(callbackid, out _handle))
			{
				_handle(is_del_sucess);
			}
		}
	}
}