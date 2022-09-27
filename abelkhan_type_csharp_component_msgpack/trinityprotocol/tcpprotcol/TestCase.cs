using MessagePack;

using System.Collections.Generic;

[MessagePackObject]
public class TestCaseByteArray {
    [Key(0)]
    public byte[] a;
    [Key(1)]
    public string b;
}
