using System;
using System.Collections;
using System.Collections.Generic;
using MessagePack;

namespace abelkhan
{
    [MessagePackObject]
    public class ProtoRoot
    {
        [Key(0)]
        public string module_name;
        [Key(1)]
        public string method_name;
        [Key(2)]
        public byte[] argvs;
    }
}