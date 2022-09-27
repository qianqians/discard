using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;

namespace abelkhan
{
    public class client
    {
        public Queue<ArrayList> que;
        public channel ch;
        public abelkhan.modulemng modules;

        private readonly abelkhan.xor_key_module _xor_key_module;

        public delegate void cb_connect_done(client _client);
        public event cb_connect_done on_connect_done;

        public client(Socket _s, string _host_port, uint xor_key)
        {
            que = new Queue<ArrayList>();
            modules = new abelkhan.modulemng();

            ch = new channel(_s, _host_port, xor_key, que);

            _xor_key_module = new abelkhan.xor_key_module(modules);
            _xor_key_module.onrefresh_xor_key += on_refresh_xor_key;
        }

        private void on_refresh_xor_key(uint xor_key)
        {
            ch.setXorKey(xor_key);
            on_connect_done?.Invoke(this);
        }

        public void poll()
        {
            while (true)
            {
                ArrayList _event = null;

                if (que.Count != 0)
                {
                    lock (que)
                    {
                        _event = que.Dequeue();
                    }
                }


                if (_event == null)
                {
                    break;
                }

                modules.process_event(ch, _event);
            }
        }
    }

    public class clientPool
    {
        private readonly uint xorKey;
        private readonly Dictionary<string, client> clientListByChannel;

        public clientPool(uint xor_key)
        {
            xorKey = xor_key;
            clientListByChannel = new Dictionary<string, client>();
        }

        public void connect(string host, short port, client.cb_connect_done cb)
        {
            System.Net.IPAddress[] address = System.Net.Dns.GetHostAddresses(host);
            var s = connectnetworkservice.connect(address[0], port);

            var c = new client(s, host + ":" + port.ToString(), xorKey);
            c.on_connect_done += cb;
            c.ch.onDisconnect += onDisconnect;
            clientListByChannel.Add(c.ch.host_port, c);
        }

        public void poll()
        {
            foreach (var i in clientListByChannel)
            {
                i.Value.poll();
            }
        }

        private void onDisconnect(channel ch)
        {
            if (clientListByChannel.TryGetValue(ch.host_port, out _))
            {
                clientListByChannel.Remove(ch.host_port);
                Debug.Log("delete client, host_port: " + ch.host_port);
            }
            else
            {
                Debug.LogWarningFormat("delete client failed, can't find {0}", ch.host_port);
            }
        }
    }
}
