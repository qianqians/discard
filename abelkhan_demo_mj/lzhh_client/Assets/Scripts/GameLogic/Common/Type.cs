using UnityEngine;
using System.Collections;


public enum LoginState
{
    Login_suc,
    Login_no,
    Login_Reque
}
/// <summary>
/// 音效
/// </summary>
public enum ESoundLayer
{
    Background,
    Effect,
    EffectUI
}

/// <summary>
/// 座位编号
/// </summary>
public enum CharacterType
{
   // Library = 0,
    relative_orignal,   
    relative_RightPostion,
    relative_FrontPostion,
    relative_LeftPostion,
    relative_Num,
    Library
    // Desk
}


/// <summary>
/// 花色
/// </summary>
public enum Suits
{
    tong,
    tiao,
    wan,
    None
}



/// <summary>
/// 位置 下右上左
/// </summary>
public enum DirDesk
{
    DownPoint,
    RightPoint,
    UpPoint,  
    LeftPoint,
    None
}

/// <summary>
/// 卡牌权值
/// </summary>
public enum Weight
{
    Three = 0,
    Four,
    Five,
    Six,
    Seven,
    Eight,
    Nine,
    Ten,
    Jack,
    Queen,
    King,
    One,
    Two,
    SJoker,
    LJoker,
}

/// <summary>
/// 身份
/// </summary>
public enum Identity
{
    Farmer,
    Landlord
}


public enum Gender
{
    girl,
    boy
}

/// <summary>
/// 交互类型
/// </summary>
public enum InteractiveType
{
   Pass,
   Hu,
   Peng,
   Gan
}

/// <summary>
/// 存储数据类型
/// </summary>
[System.Serializable]
public class GameData
{
    public int playerIntegration;
    public int computerOneIntegration;
    public int computerTwoIntegration;
    public int computerTreIntegration;
    
}

public enum SelfState
{
    In_lobby,
    Join_room,
    SitDown,
    Can_playCard,
    Can_playLaizi,
    Not_playCard
}

public enum Directions
{
    None,
    East,
    South,
    West,
    North
}

public enum EffectPrompt
{
    hu,
    ruanlaiyou,
    yinglaiyou,
    youshangyou,
    yingshangyou,
    ruanzimo,
    yingzimo,
    lai,
    gang,    
}

public enum InteractivePrompt
{
    guo,
    hu,
    laiyou,
    peng,
    gang
}

public enum RankListen
{
    redBag=0,
    pay=1
}

public enum SceneName
{
    no,
    NULogin,
    NUMainWindow,
    InGame,
}

public enum GameType
{
    fangkaGame,
    goldGame,
    matchGame,
    integarl
}
