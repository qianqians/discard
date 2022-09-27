using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Security.Cryptography;
using System.Net;
using common;

namespace lobby
{
    class pay : imodule
    {
        public pay()
        {
        }

        public void player_prepay(Int64 total_fee, string ip)
        {
            var client_uuid = hub.hub.gates.current_client_uuid;
            var _proxy = server.players.get_player_uuid(client_uuid);

            var r = new Random();
            var rand = r.Next();
            string out_trade_no = System.Guid.NewGuid().ToString("N");
            string agrv_tmp = string.Format("appid={0}&body={1}&mch_id={2}&nonce_str={3}&notify_url={4}&out_trade_no={5}&spbill_create_ip={6}&total_fee={7}&trade_type=APP&key={8}", 
                global.appid, global.body, global.mch_id, rand, payUtil.notify_url, out_trade_no, ip, total_fee, global.key);
            agrv_tmp = System.Text.Encoding.UTF8.GetString(System.Text.Encoding.Convert(System.Text.Encoding.Default, System.Text.Encoding.UTF8, System.Text.Encoding.Default.GetBytes(agrv_tmp)));
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(agrv_tmp));
            string sign = "";
            for (int i = 0; i < result.Length; i++)
            {
                sign += result[i].ToString("X2").ToUpper();
            }

            string uri = string.Format("https://api.mch.weixin.qq.com/pay/unifiedorder");
            HttpWebRequest request = HttpWebRequest.Create(uri) as HttpWebRequest;
            request.Method = "POST";
            Stream streamRequest = request.GetRequestStream();
            var xml_agrv = new XmlDocument();
            var xmlelem = xml_agrv.CreateElement("", "xml", "");
            xml_agrv.AppendChild(xmlelem);
            XmlNode root = xml_agrv.SelectSingleNode("xml");
            XmlElement xe = xml_agrv.CreateElement("appid");
            xe.InnerText = System.Text.Encoding.UTF8.GetString(System.Text.Encoding.Convert(System.Text.Encoding.Default, System.Text.Encoding.UTF8, System.Text.Encoding.Default.GetBytes(global.appid))); 
            root.AppendChild(xe);
            xe = xml_agrv.CreateElement("body");
            xe.InnerText = System.Text.Encoding.UTF8.GetString(System.Text.Encoding.Convert(System.Text.Encoding.Default, System.Text.Encoding.UTF8, System.Text.Encoding.Default.GetBytes(global.body)));
            root.AppendChild(xe);
            xe = xml_agrv.CreateElement("mch_id");
            xe.InnerText = System.Text.Encoding.UTF8.GetString(System.Text.Encoding.Convert(System.Text.Encoding.Default, System.Text.Encoding.UTF8, System.Text.Encoding.Default.GetBytes(global.mch_id))); 
            root.AppendChild(xe);
            xe = xml_agrv.CreateElement("nonce_str");
            xe.InnerText = System.Text.Encoding.UTF8.GetString(System.Text.Encoding.Convert(System.Text.Encoding.Default, System.Text.Encoding.UTF8, System.Text.Encoding.Default.GetBytes(string.Format("{0}", rand)))); 
            root.AppendChild(xe);
            xe = xml_agrv.CreateElement("notify_url");
            xe.InnerText = System.Text.Encoding.UTF8.GetString(System.Text.Encoding.Convert(System.Text.Encoding.Default, System.Text.Encoding.UTF8, System.Text.Encoding.Default.GetBytes(payUtil.notify_url)));
            root.AppendChild(xe);
            xe = xml_agrv.CreateElement("out_trade_no");
            xe.InnerText = System.Text.Encoding.UTF8.GetString(System.Text.Encoding.Convert(System.Text.Encoding.Default, System.Text.Encoding.UTF8, System.Text.Encoding.Default.GetBytes(out_trade_no))); 
            root.AppendChild(xe);
            xe = xml_agrv.CreateElement("spbill_create_ip");
            xe.InnerText = System.Text.Encoding.UTF8.GetString(System.Text.Encoding.Convert(System.Text.Encoding.Default, System.Text.Encoding.UTF8, System.Text.Encoding.Default.GetBytes(ip))); 
            root.AppendChild(xe);
            xe = xml_agrv.CreateElement("total_fee");
            xe.InnerText = System.Text.Encoding.UTF8.GetString(System.Text.Encoding.Convert(System.Text.Encoding.Default, System.Text.Encoding.UTF8, System.Text.Encoding.Default.GetBytes(string.Format("{0}", total_fee)))); 
            root.AppendChild(xe);
            xe = xml_agrv.CreateElement("trade_type");
            xe.InnerText = System.Text.Encoding.UTF8.GetString(System.Text.Encoding.Convert(System.Text.Encoding.Default, System.Text.Encoding.UTF8, System.Text.Encoding.Default.GetBytes("APP"))); 
            root.AppendChild(xe);
            xe = xml_agrv.CreateElement("sign");
            xe.InnerText = sign;
            root.AppendChild(xe);
            log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "{0}", xml_agrv.OuterXml);
            byte[] bt = System.Text.Encoding.UTF8.GetBytes(xml_agrv.OuterXml);
            streamRequest.Write(bt, 0, bt.Length);
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "request response");

                Stream stream = response.GetResponseStream();
                StreamReader sr = new StreamReader(stream);
                string result1 = sr.ReadToEnd();

                log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "{0}", result1);

                string return_code = "";
                string result_code = "";
                string prepay_id = "";

                XmlDocument x = new XmlDocument();
                x.LoadXml(result1);

                XmlNode xml = x.SelectSingleNode("xml");
                XmlNodeList nodeList = xml.ChildNodes;
                foreach(XmlNode xn in nodeList)
                {
                    XmlElement xe1 = (XmlElement)xn;

                    if (xe1.Name.Equals("return_code"))
                    {
                        return_code = xe1.InnerText;
                    }
                    else if (xe1.Name.Equals("result_code"))
                    {
                        result_code = xe1.InnerText;
                    }
                    else if (xe1.Name.Equals("prepay_id"))
                    {
                        prepay_id = xe1.InnerText;
                    }
                }
                
                log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "return_code={0}, result_code={1}, prepay_id={2}", return_code, result_code, prepay_id);

                if (return_code == "SUCCESS" && result_code == "SUCCESS")
                {
                    _proxy.player_info["out_trade_no"] = out_trade_no;
                    hub.hub.gates.call_client(client_uuid, "pay", "prepay", prepay_id);

                    lock (payUtil.out_trade_no)
                    {
                        payUtil.out_trade_no.Add(out_trade_no, _proxy);
                    }
                }
            }
        }
    }
}
