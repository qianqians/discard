using System;
using System.Net;
using System.Collections.Generic;
using System.Text;
using abelkhan;
using MessagePack;
using System.IO;

namespace http_gate
{
    class test_protocol
    {
        public test_protocol()
        {
            //http_gate._http.post("/", (req) => {
            //    var req_data = MessagePackSerializer.Deserialize<MsgPackObj>(req.RequestBody);
            //    req_data.a[0].Email = "wuming@xxx.com";
            //    req_data.a[0].Name.FirstName = "wu";
            //    req_data.a[0].Name.LastName = "ming";

            //    var headers = new Dictionary<string, string>() { { "Content-Type", "text/json; charset=utf-8" } };
            //    var msg = MessagePackSerializer.Serialize(req_data);
            //    req.Respond(System.Net.HttpStatusCode.OK, headers, msg);
            //});

            http_gate.httpHelper.post("/testTime", (req) => {
                var _Pinpang = MessagePackSerializer.Deserialize<Pinpang>(req.packet.rawData);
                _Pinpang.time = timerservice.Tick;

                req.Respond(MessagePackSerializer.Serialize(_Pinpang));
            });
        }
    }
}
