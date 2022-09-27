
using MessagePack;

[MessagePackObject]
public class HttpBasePacket {
    [Key(0)]
    public string uuid;
    [Key(1)]
    public ulong timeStamp;
    [Key(2)]
    public byte[] rawData;
}

[MessagePackObject]
public class HttpBaseRsp
{
    [Key(0)]
    public string uuid;
    [Key(1)]
    public ulong timeStamp;
    [Key(2)]
    public byte[] rawData;
}