using System;
using System.Collections.Generic;
using System.Text;

namespace abelkhan.admin
{
    public interface IResponse
    {
        int toCode();

        string toResp();
    }
}
