/*
 * centerproxy
 * qianqians
 * 2020/6/4
 */

namespace abelkhan
{
    public class centerproxy
    {
        public bool is_reg_center_sucess;
        private center_caller _caller;
        private hub_call_center_caller _hub_call_center_caller;

        public centerproxy(Ichannel ch, modulemng modules)
        {
            is_reg_center_sucess = false;
            _caller = new center_caller(ch, modules);
            _hub_call_center_caller = new hub_call_center_caller(ch, modules);
        }

        public void reg_hub(string name, string hub_type, string ip, ushort port)
        {
            _caller.reg_server("hub", hub_type, name, ip, port);
        }

        public void closed()
        {
            _hub_call_center_caller.closed();
        }
    }
}