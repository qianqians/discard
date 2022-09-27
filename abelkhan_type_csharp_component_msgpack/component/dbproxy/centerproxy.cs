/*
 * centerproxy
 * 2020/6/3
 * qianqians
 */

namespace abelkhan
{
	public class centerproxy
	{
		private center_caller _caller;
		public bool is_reg_sucess;
		public centerproxy(abelkhan.Ichannel ch, modulemng modules)
		{
			is_reg_sucess = false;
			_caller = new center_caller(ch, modules);
		}

		public void reg_dbproxy(string svr_name, string ip, ushort port)
		{
			_caller.reg_server("dbproxy", "dbproxy", svr_name, ip, port);
		}
	}
}