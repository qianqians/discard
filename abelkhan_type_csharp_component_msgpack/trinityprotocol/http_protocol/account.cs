
using MessagePack;

[MessagePackObject]
public class Account {
    [Key(0)]
    public string UUID;
    [Key(1)]
    public short type;
    [Key(2)]
    public UserCharacter character;
    [Key(3)]
    public UserBag bag;
    [Key(4)]
    public UserScene scene;
}


[MessagePackObject]
public class UserScene {
    [Key(0)]
    public short type;
}

[MessagePackObject]
public class UserBag {
    [Key(0)]
    public short capacity;
}

[MessagePackObject]
public class UserCharacter {
    [Key(0)]
    public short modelType;
}
