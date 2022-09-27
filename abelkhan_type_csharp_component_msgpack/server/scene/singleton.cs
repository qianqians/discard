using System;
using System.Collections.Generic;
using System.Text;

namespace scene
{
    public class singleton
    {
        public static clientmng clients;

        public static void Init()
        {
            clients = new clientmng();
        }
    }
}
