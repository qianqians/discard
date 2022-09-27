using abelkhan.admin;
using System;
using System.Collections.Generic;
using System.Text;

namespace abelkhan.admin
{
    public class NullResponse: IResponse
    {
        public int toCode()
        {
            return 0;
        }
        public string toResp()
        {
            return "";
        }
    }
}
