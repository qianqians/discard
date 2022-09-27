using System;
using System.Text;
using System.Collections.Generic;
using EvHttpSharp;

namespace abelkhan.admin.helper
{
    public class EvHttpHelper
    {
        internal static bool isGZip(EventHttpRequest request)
        {
            bool flag = false;
            IEnumerable<string> encodings;
            request.Headers.TryGetValue("Accept-Encoding", out encodings);
            if (encodings == null) {
                return false;
            }
           foreach (string encoding in encodings)
                if (encoding.IndexOf("gzip") > -1)
                flag = true;
            return flag;
        }


        public static Dictionary<string, string> buildCrossHeaders()
        {
            return new Dictionary<string, string>() { { "Content-Type", "application/json; charset=utf-8" },
                { "Access-Control-Allow-Origin", "*" }, {"Access-Control-Allow-Headers", "X-Token, Content-Type" },
            { "Access-Control-Allow-Methods", "POST, GET, OPTIONS"} };
        }

        public static string TryGetHeader(IDictionary<string, IEnumerable<string>> Headers, string key) {
            IEnumerable<string> en;
            string header = "";
            Headers.TryGetValue("X-Token", out en);
            if (en != null)
            {
                foreach (string s in en)
                {
                    header += s;
                }
            }

            return header;
        }
    }
}
