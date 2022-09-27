using System;
using System.Collections.Generic;
using System.Text;
using abelkhan.admin.helper;

namespace abelkhan.admin
{
    public class StringWp : IResponse
    {
        
        public string Res { set; get; }


        public int toCode() {
            return 0;
        }
        public string toResp()
        {
            return Res;
        }

        public static StringWp ValueOf(string _res) {
            StringWp stringWp = new StringWp();
            stringWp.Res = _res;
            return stringWp;
        }
    }
}
