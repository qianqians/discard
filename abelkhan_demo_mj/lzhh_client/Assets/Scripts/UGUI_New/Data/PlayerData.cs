using System;
using System.Collections.Generic;
using System.Linq;
using GameCommon;
using System.Collections;

public class PlayerData
{
    /// <summary>
    /// 座位编号0,1,2,3（每个人自己的座位号永远是0）
    /// </summary>
    public CharacterType playerType;
    private List<Int64> _handCardList;//自己的牌有数据，别人的牌都用0;
    private List<Int64> _revealCardList;//碰或杠了的牌
    public Int64 playerOrderIndex;//玩家出牌顺序编号1，2，3，4
    public string token;
    public string wechat_name;
    public string headimg;       
    public Int64 score;
    public Int64 integarl;
    public bool IsSelfData;
  //  public Hashtable playBaseInfo;
    public Int64 sex;
    public Directions dir;
    public List<Int64> playOutCardList;
    public List<Int64> playOutlaiziList;
    public Int64 playerID;
    public bool isHupai;
    public PlayerData(Int64 index)
    {
        playerOrderIndex = index+1;
        playerType = (CharacterType)index;
        _handCardList = new List<Int64>() {0,0,0,0,0,0,0,0,0,0,0,0,0};
        _revealCardList = new List<Int64>();
        playOutCardList = new List<Int64>();
        playOutlaiziList = new List<Int64>();

        token = "";
        wechat_name = "";
        headimg = "";
        sex = 0;
        score = 0;
        playerID = 0;
        IsSelfData = false;
        dir = Directions.None;
    }

    public List<Int64> HandCardList
    {
        get
        {
            return _handCardList;
        }
    }

    /// <summary>
    /// 碰或者杠过的牌
    /// </summary>
    public List<Int64> RevealCardList
    {
        get
        {
            return _revealCardList;
        }
    }

    public void AddHandCard(Int64 cardID)
    {
        _handCardList.Add(cardID);
    }

    /// <summary>
    /// 断线重连后设置 别人的牌
    /// </summary>
    /// <param name="count"></param>
    public void SetHandList(int count)
    {
        _handCardList.Clear();
        for (int i = 0; i < count; i++)
        {
            _handCardList.Add((Int64)0);
        }          
    }
    /// <summary>
    /// 断线重连后自己的牌 
    /// </summary>
    /// <param name="arr"></param>
    public void SetHandList(ArrayList arr)
    {
        _handCardList.Clear();
        for (int i = 0; i < arr.Count; i++)
        {
            _handCardList.Add((Int64)arr[i]);
        }
    }

    public void RemoveAppointMj(Int64 cardID)
    {
        _handCardList.Remove(cardID);
    }

    public void AddRevealCard(Int64 cardID)
    {
        _revealCardList.Add(cardID);
    }

    /// <summary>
    ///  断线 重连后设置 flag = true,peng...false gang
    /// </summary>
    /// <param name="list"></param>
    /// <param name="flag"></param>
    public void SetRevealList(ArrayList list,bool flag,Int64 laizipi)
    {
        for (int i = 0; i < list.Count; i++)
        {
            _revealCardList.Add((Int64)list[i]);
            _revealCardList.Add((Int64)list[i]);
            _revealCardList.Add((Int64)list[i]);
            if (!flag && laizipi != (Int64)list[i])
            {
                _revealCardList.Add((Int64)list[i]);
            }
        }
    }

    /// <summary>
    /// 断线重连后 设置打过的牌
    /// </summary>
    /// <param name="list"></param>
    public void SetPlayOutList(ArrayList list,Int64 laizi)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if ((Int64)list[i] == laizi)
            {
                playOutlaiziList.Add((Int64)list[i]);               
            }
            else
            {
                playOutCardList.Add((Int64)list[i]);
            }         
        }

    }

    /// <summary>
    /// 用于下一把,清空牌数据
    /// </summary>
    public void Reset()
    {
        _handCardList = new List<Int64>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        _revealCardList = new List<Int64>();
        playOutCardList = new List<Int64>();
        playOutlaiziList = new List<Int64>();
        isHupai = false;
    }

    public void Dispose()
    {
        playerID = 0;
        token = "";
        wechat_name = "";
        headimg = "";
        score = 0;
        IsSelfData = false;
        dir = (Directions)playerOrderIndex;
        playerType = (CharacterType)(playerOrderIndex - 1);
        Reset();
    }
}

