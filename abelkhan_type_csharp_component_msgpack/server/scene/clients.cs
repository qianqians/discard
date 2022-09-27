using System;
using System.Collections.Generic;
using System.Text;

namespace scene
{
    public class clientproxy
    {
        public clientproxy(abelkhan.Ichannel ch)
        {
        }
    }

    public class clientmng
    {
        private Dictionary<abelkhan.Ichannel, string> client_ch_uuid;
        private Dictionary<abelkhan.Ichannel, clientproxy> client_ch_proxy;
        private Dictionary<string, clientproxy> clients;

        public clientmng()
        {
            client_ch_uuid = new Dictionary<abelkhan.Ichannel, string>();
            client_ch_proxy = new Dictionary<abelkhan.Ichannel, clientproxy>();
            clients = new Dictionary<string, clientproxy>();
        }

        public clientproxy reg_client(string uuid, abelkhan.Ichannel ch)
        {
            var _proxy = new clientproxy(ch);
            client_ch_uuid.Add(ch, uuid);
            client_ch_proxy.Add(ch, _proxy);
            clients.Add(uuid, _proxy);

            return _proxy;
        }

        public void unreg_client(abelkhan.Ichannel ch)
        {
            client_ch_proxy.Remove(ch);
            if (client_ch_uuid.Remove(ch, out string uuid)) {
                clients.Remove(uuid);
            }
        }

        public clientproxy get_client(abelkhan.Ichannel ch)
        {
            return client_ch_proxy.GetValueOrDefault(ch, null);
        }

        public clientproxy get_client(string uuid)
        {
            return clients.GetValueOrDefault(uuid, null);
        }
    }
}
