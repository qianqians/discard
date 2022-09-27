using System;
using System.Collections;
using System.Collections.Generic;
using MessagePack;

[MessagePackObject]
public class MsgPackObj {
    [MessagePackObject]
    public class A {
        [Key(0)]
        public int UserID;
        [MessagePackObject]
        public class TName {
            [Key(0)]
            public string FirstName;
            [Key(1)]
            public string LastName;
        }
        [Key(1)]
        public TName Name;
        [Key(2)]
        public string Email;
    };

    [Key(0)]
    public List<A> a;
}

[MessagePackObject]
public class Pinpang
{
    [Key(0)]
    public Int64 time;
}
