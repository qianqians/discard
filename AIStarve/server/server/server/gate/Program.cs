using Abelkhan;
using Service;
using System;
using System.Collections.Generic;
using System.Threading;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace gate_svr
{
    public class RoleCardRequest
    {
        public string UserID = string.Empty;
        public List<string> RoleCards = new List<string>();
    }

    class Program
    {
        //public static RedisHandle RedisHandle;

        static void Main(string[] args)
		{
            var _gate = new Gate.GateService(args[0], args[1]);
            
            Log.Log.trace("gate_svr start ok");

            _gate.run().Wait();
        }
	}
}
