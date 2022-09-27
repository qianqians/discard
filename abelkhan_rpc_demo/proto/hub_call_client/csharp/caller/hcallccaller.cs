/*this caller file is codegen by abelkhan for c#*/
using System;
using System.Collections;
using System.IO;

namespace ntf
{
    public class hcallc
    {
        public hcallc()
        {
        }

        public hcallc_cliproxy get_client(string uuid)
        {
            return new hcallc_cliproxy(uuid);
        }

        public hcallc_cliproxy_multi get_multicast(ArrayList uuids)
        {
            return new hcallc_cliproxy_multi(uuids);
        }

        public hcallc_broadcast get_broadcast()
        {
            return new hcallc_broadcast();
        }

    }

    public class hcallc_cliproxy
    {
        private string uuid;

        public hcallc_cliproxy(string _uuid)
        {
            uuid = _uuid;
        }

        public void hcallc(String argv0)
        {
            hub.hub.gates.call_client(uuid, "hcallc", "hcallc", argv0);
        }

    }

    public class hcallc_cliproxy_multi
    {
        private ArrayList uuids;

        public hcallc_cliproxy_multi(ArrayList _uuids)
        {
            uuids = _uuids;
        }

    }

    public class hcallc_broadcast
    {
        public hcallc_broadcast()
        {
        }

    }

}
