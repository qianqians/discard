using System;
using System.Collections.Generic;
using System.Linq;
using GameCommon;
using System.Collections;
/// <summary>
/// 解析断线重连后的Hashtable数据
/// </summary>
public class ReconnectionData
{
    //其他玩家的手牌数，前端自己算
    public Int64 cardRighter;//谁处理牌
    public Int64 processer;//谁可以碰或者杠
    public Int64 laizi;
    public Int64 dice;//骰子
    public Hashtable playOutCard;//按座位东南西北存入
    public Hashtable pengCard;
    public Hashtable gangCard;
    public Int64 in_game;
    public Int64 play_count;
    public bool is_robot_room;
    public Int64 room_id;
    public Int64 peopleNum;
    public Int64 rule;
    public Int64 score;//底分
    public Int64 times;
    public Int64 payRule;
    public Int64 bankerID;
    public Int64 cardCount;
    public Int64 payState;

    public Int64 processer_card;
    public Int64 playCardPlayerSeat;

    public Int64 hupaiPlayerSeat;
    public ArrayList hupaiCardList;
    public Hashtable playerCardNum;
    public List<HupaiPlayerInfo> hupaiPlayerInfoList;
    public bool isHu;
    public bool isLiuJu;

    private List<PlayerCardInfo> playerCardinfoList;
    public Hashtable gangPaiInfo;
    public ReconnectionData(Hashtable data)
    {
        ArrayList otherPlayerCard;
        ArrayList cardList; ;
        Hashtable OnterInfo;
        HupaiPlayerInfo hupInfo;

        isHu = false;
        hupaiPlayerInfoList = new List<HupaiPlayerInfo>();
        if (data.ContainsKey("play_card_player"))
        {           
            playCardPlayerSeat = (Int64)data["play_card_player"];
            processer_card = (Int64)data["play_card"];
        }
        gangPaiInfo = data["gang_state"] as Hashtable;
        if (data.ContainsKey("is_hu"))
        {
            if ((bool)data["is_hu"])
            {
                isHu = true;
                hupaiPlayerSeat = (Int64)data["hu_player"];
                hupaiCardList = (ArrayList)data["hu_card"];
                otherPlayerCard = (ArrayList)data["other_card"];
                hupInfo = new HupaiPlayerInfo();
                hupInfo.playerSeat = hupaiPlayerSeat;

                for (int j = 0; j < hupaiCardList.Count; j++)
                {
                    hupInfo.cardList.Add((Int64)hupaiCardList[j]);
                }
                hupaiPlayerInfoList.Add(hupInfo);
                for (int i = 0; i < otherPlayerCard.Count; i++)
                {
                    hupInfo = new HupaiPlayerInfo();
                    OnterInfo = (Hashtable)otherPlayerCard[i];
                    hupInfo.playerSeat = (Int64)OnterInfo["player"];
                    cardList = (ArrayList)OnterInfo["pai"];
                    for (int j = 0; j < cardList.Count; j++)
                    {
                        hupInfo.cardList.Add((Int64)cardList[j]);
                    }
                    hupaiPlayerInfoList.Add(hupInfo);
                }
            }
        }

        bankerID = (Int64)data["zhuang"];
        cardCount = (Int64)data["cards_count"];
        dice = (Int64)data["r_saizi"];
        laizi = (Int64)data["laizi"];
        score = (Int64)data["score"];
        payState = (Int64)data["payRule"];
        times = (Int64)data["times"];
        if (data.ContainsKey("card_righter"))
        {
            cardRighter = (Int64)data["card_righter"];
        }
    
      //  processer = (Int64)data["processer"];
        play_count = (Int64)data["play_count"];
        peopleNum = (Int64)data["peopleNum"];       
        Hashtable playout  = data["player_play_cards"] as Hashtable;
        Hashtable pengcard = data["player_peng"] as Hashtable;
        Hashtable gangCard = data["player_gang"] as Hashtable;

        playerCardNum = (Hashtable)data["playerCardNum"];
        PlayerCardInfo info;
        string index;
        playerCardinfoList = new List<PlayerCardInfo>();
        for (int i = 1; i < peopleNum + 1; i++)
        {
            index = i.ToString();
            info = new PlayerCardInfo();
            info.playOutArr = playout[index] as ArrayList;
            info.pengArr = pengcard[index] as ArrayList;
            info.GangArr = gangCard[index] as ArrayList;
            info.handCardNum = (Int64)playerCardNum[index];
            playerCardinfoList.Add(info);
        }

        if (true)
        {
            isLiuJu = true;
        }
    }

    public List<PlayerCardInfo> PlayerCardinfoList
    {
        get
        {
            return playerCardinfoList;
        }
    }
}

public class PlayerCardInfo
{
    public ArrayList playOutArr;
    public ArrayList pengArr;
    public ArrayList GangArr;
    public Int64 handCardNum;
    public PlayerCardInfo()
    {
    }
}


public class HupaiPlayerInfo
{
    public Int64 playerSeat;
    public bool isHupai;
    public List<Int64> cardList;
    public HupaiPlayerInfo()
    {
        cardList = new List<Int64>();
    }
}
