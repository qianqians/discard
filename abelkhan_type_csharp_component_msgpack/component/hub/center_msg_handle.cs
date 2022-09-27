/*
 * center_msg_handle
 * qianqians
 * 2020/6/4
 */

using System;
using System.Threading;

namespace abelkhan
{
	public class server_info
	{
		public string type;
		public string name;
		public string ip;
		public ushort port;

		public server_info(string _type, string _name, string _ip, ushort _port)
		{
			type = _type;
			name = _name;
			ip = _ip;
			port = _port;
		}
	}

	public class center_msg_handle
	{
		private closehandle _closehandle;
		private centerproxy _centerproxy;

		private center_call_server_module _center_call_server_module;
		private center_call_hub_module _center_call_hub_module;

		public center_msg_handle(modulemng modules, closehandle _closehandle_, centerproxy _centerproxy_)
		{
			_closehandle = _closehandle_;
			_centerproxy = _centerproxy_;

			_center_call_server_module = new center_call_server_module(modules);
			_center_call_server_module.onreg_server_sucess += reg_server_sucess;
			_center_call_server_module.onclose_server += close_server;
			_center_call_server_module.onserver_be_close += svr_be_closed;

			_center_call_hub_module = new center_call_hub_module(modules);
			_center_call_hub_module.ondistribute_server_address += distribute_server_address;
			_center_call_hub_module.onreload += reload;
		}

		public void reg_server_sucess()
		{
			_centerproxy.is_reg_center_sucess = true;
		}

		public event Action on_close;
		public void close_server()
		{
			log.trace(new System.Diagnostics.StackFrame(true), timerservice.Tick, "close_server");
			if (on_close != null)
			{
				on_close();
			}
		}

		public event Action<server_info> on_svr;
		public void distribute_server_address(string type, string name, string ip, int port)
		{
			if (on_svr != null)
			{
				on_svr(new server_info(type, name, ip, (ushort)port));
			}
		}

		public event Action on_reload;
		public void reload()
		{
			log.trace(new System.Diagnostics.StackFrame(true), timerservice.Tick, "reload");
			if (on_reload != null)
			{
				on_reload();
			}
		}

		public event Action<string, string> on_svr_closed;
		public void svr_be_closed(string type, string name)
        {
			if (on_svr_closed != null)
            {
				on_svr_closed(type, name);
			}
        }
	}
}