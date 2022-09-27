/*
 * center_msg_handle
 * 2020/6/3
 * qianqians
 */

namespace abelkhan
{
	public class center_msg_handle
	{
		private center_call_server_module _center_call_server_module;
		private closeHandle _closehandle;
		private centerproxy _centerproxy;
		private hubmanager _hubs;

		public center_msg_handle(modulemng modules, hubmanager _hubmanager, closeHandle _closeHandle, centerproxy _proxy)
		{
			_hubs = _hubmanager;
			_closehandle = _closeHandle;
			_centerproxy = _proxy;

			_center_call_server_module = new center_call_server_module(modules);
			_center_call_server_module.onclose_server += close_server;
			_center_call_server_module.onreg_server_sucess += reg_server_sucess;
			_center_call_server_module.onserver_be_close += on_svr_closed;
		}

		public void close_server()
		{
			log.trace(new System.Diagnostics.StackFrame(true), timerservice.Tick, "close_server");
			_closehandle.is_close = true;
		}

		public void reg_server_sucess()
		{
			_centerproxy.is_reg_sucess = true;
		}

		public void on_svr_closed(string type, string name)
        {
			log.trace(new System.Diagnostics.StackFrame(true), timerservice.Tick, "on_svr_closed type:{0}, name:{1}", type, name);
			_hubs.on_hub_closed(name);
		}
	}
}