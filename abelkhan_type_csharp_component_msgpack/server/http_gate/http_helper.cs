using System;
using System.Collections.Generic;
using System.Text;

namespace http_gate
{
    public class HttpReq
    {
        public HttpBasePacket packet;
        private EvHttpSharp.EventHttpRequest req;

        public HttpReq(HttpBasePacket _packet, EvHttpSharp.EventHttpRequest _req)
        {
            packet = _packet;
            req = _req;
        }

        public void Respond(byte[] data)
        {
            var headers = new Dictionary<string, string>() { { "Content-Type", "text/json; charset=utf-8" } };
            req.Respond(System.Net.HttpStatusCode.OK, headers, utils.Serialize(packet.uuid, data));
        }
    }

    public class http_helper
    {
        private abelkhan.evHttp http;

        public http_helper(abelkhan.evHttp _http)
        {
            http = _http;
        }

        public void post(string uri, Action<HttpReq> callback)
        {
            http.post(uri, (_req)=> {
                var _packet = utils.Deserialize(_req.RequestBody);
                callback(new HttpReq(_packet, _req));
            });
        }
    }
}
