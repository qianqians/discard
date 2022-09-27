using System;
using System.Collections;
using System.Collections.Generic;
using GameCommon;
using TinyFrameWork;
using UnityEngine;
using Assets.Scripts;
public class PlayerInfo
{
    private List<PlayerData> _infoList;
    private int _playerCount;//玩家数量
    public Int64 selfOrderIndex;//自己的出牌顺序号  
    public bool selfSitDown;
    public Int64 laizi;
    public GameRule gameRule;
    //自己打了多少癞子
    public int selfPutOutLaiziNum;
    public PlayerInfo(int playerCount)
    {
        laizi = 0;
        selfPutOutLaiziNum = 0;
        _playerCount = playerCount;
        _infoList = new List<PlayerData>();
        for (int i = 0; i < _playerCount; i++)
        {
            _infoList.Add(new PlayerData(i));
        }
    }

    public List<PlayerData> DataList
    {
        get
        {
            return _infoList;
        }
    }

    public PlayerData GetSelfData()
    {
        return _infoList[(int)selfOrderIndex-1];
    }

    public CharacterType GetPlayerSeatByUuid(string uuid)
    {
        CharacterType temp = CharacterType.Library;
        foreach (PlayerData item in _infoList)
        {
            if (item.token == uuid)
            {
                return item.playerType;
            }
        }
        return temp;       
    }

    public String GetPlayerfUuidIndexBySeat(CharacterType temp)
    {
        string index = "";
        foreach (PlayerData item in _infoList)
        {
            if (item.playerType == temp)
            {
                return item.token;
            }
        }
        return index;
    }

    public CharacterType GetPlayerSeatByOrderIndex(Int64 OrderIndex)
    {
        CharacterType index = CharacterType.Library;
        foreach (PlayerData item in _infoList)
        {
            if (item.playerOrderIndex == OrderIndex)
            {
                return item.playerType;
            }
        }
        return index;
    }

    public PlayerData GetPlayerDataBySeat(CharacterType seat)
    {
        foreach (PlayerData item in _infoList)
        {
            if (item.playerType == seat)
            {
                return item;
            }
        }
        return null;
    }

    public void SelfJoinRoom(ArrayList list)
    {
        PlayerData data;
        foreach (Hashtable item in list)
        {
            Int64 index = (Int64)item["site"];
            if (index != 0)
            {
                data = _infoList[(int)index - 1];
                data.token = (string)item["unionid"];
                data.wechat_name = (string)item["nickname"];
                data.playerOrderIndex = index;
                data.headimg = (string)item["headimg"];
                data.sex = (Int64)item["sex"];
               // data.playBaseInfo = item["player_info"] as Hashtable;
                data.dir = (Directions)index;
                data.playerID = (Int64)item["game_id"];
                data.score = (Int64)item["score"];
                data.integarl = (Int64)item["rank_score"];
                //断线重连的时候会用到
                if (data.token == SocketClient.Instance.playToken)
                {
                    selfSitDown = true;
                    selfOrderIndex = index;
                    data.IsSelfData = true;
                    ChangeSeatIndex(index);                
                    data.playerOrderIndex = index;
                    data.dir = (Directions)index;
                }
            }            
        }
    }

    public void PlayerSitDown(Hashtable table)
    {
        bool ifSelfSitDown = false;
        Int64 index = (Int64)table["site"];
        PlayerData data = _infoList[(int)index-1];
        data.token = (string)table["unionid"];
        data.wechat_name = (string)table["nickname"];
        data.headimg = (string)table["headimg"];
        data.sex = (Int64)table["sex"];
       // data.playBaseInfo = table["player_info"] as Hashtable;
        data.playerID = (Int64)table["game_id"];
        //当自己坐下去的时候改变座位编号
        if (data.token == SocketClient.Instance.playToken)
        {
            ifSelfSitDown = true;
            selfSitDown = true;
            selfOrderIndex = index;
            data.IsSelfData = true;
            ChangeSeatIndex(index);          
            data.dir = (Directions)index;        
        }
        EventDispatcher.GetInstance().UIFrameWorkEventManager.TriggerEvent<List<PlayerData>, bool>(EventId.UIFrameWork_Player_Sit_down, _infoList, ifSelfSitDown);
    }


    /// <summary>
    /// 重连
    /// </summary>
    /// <param name="data"></param>
    public void Reconnection(ReconnectionData data)
    {
        List<PlayerCardInfo> playerCardinfoList = data.PlayerCardinfoList;
        int index;
        PlayerCardInfo info;
        foreach (PlayerData item in _infoList)
        {
            index = (int)item.dir;
            info = playerCardinfoList[index-1];
            if (data.bankerID == index)//如果是庄家则 把第一次翻的癞子皮放到他打过的牌里
            {
                info.playOutArr.Insert( 0,GetLaizipi(data.laizi));
            }
            item.SetRevealList(info.pengArr,true, GetLaizipi(data.laizi));
            item.SetRevealList(info.GangArr,false, GetLaizipi(data.laizi));
            item.SetPlayOutList(info.playOutArr, data.laizi);
            if (item.IsSelfData)
            {
                selfPutOutLaiziNum = item.playOutlaiziList.Count;
            }
            item.SetHandList((int)info.handCardNum);//还要后台给吧，前端算有问题
        }
        EventDispatcher.GetInstance().UIFrameWorkEventManager.TriggerEvent<bool>(EventId.UIFrameWork_Reconnection_Updata,data.isHu);
    }

    private Int64 GetLaizipi(Int64 laizi)
    {
        Int64 laizipi;
        laizipi = laizi - 1;
        if (laizipi % 10 == 0)
        {
            laizipi = laizi + 8;
        }
        return laizipi;
    }

    public void Reconnection_SelfHandCard(ArrayList arr)
    {
        int index = (int)selfOrderIndex;
        PlayerData item = _infoList[index-1];
        item.SetHandList(arr);
       
    }

    /// <summary>
    /// 断线重连的时候，自己的手牌里面包括当前摸的牌，为了走摸牌的流程，把自己当前的摸牌从手牌中剔除
    /// </summary>
    public void ReconnectionRemoveMopai(Int64 mopai)
    {
        int index = (int)selfOrderIndex;
        PlayerData item = _infoList[index - 1];
        item.RemoveAppointMj(mopai);
        CardRules.SortCardsForInt(item.HandCardList, TableController.Instance.laizi);
        item.HandCardList.Reverse();
        EventDispatcher.GetInstance().UIFrameWorkEventManager.TriggerEvent<List<PlayerData>>(EventId.UIFrameWork_Reconnection, _infoList);
    }

    public void ReconnectionShowSelfCard(Int64 index)
    {
        PlayerData data = _infoList[(Int32)index - 1];
        CheckIsTingPai(data.HandCardList,0);
        CardRules.SortCardsForInt(data.HandCardList, TableController.Instance.laizi);
        data.HandCardList.Reverse();
        EventDispatcher.GetInstance().UIFrameWorkEventManager.TriggerEvent<List<PlayerData>>(EventId.UIFrameWork_Reconnection, _infoList);
    }

    public void ReconnectionCheckSelfCanInter(Int64 processer_card, Int64 laizipi, Int64 processer)
    {
        if (processer_card !=0)//等于0表示当前没人出牌
        {
            int interactiveFlag = CheckOtherPlayCard(processer_card, laizipi);
            if (processer == selfOrderIndex)
            {
                EventDispatcher.GetInstance().UIFrameWorkEventManager.TriggerEvent<int, int>(EventId.UIFrameWork_Game_Animation_Playover, (int)processer_card, interactiveFlag);
            }
        }
        
    }

    public void ReconnectionShowByIsHu(List<HupaiPlayerInfo> list)
    {
        HupaiPlayerInfo recDate;
        PlayerData data;
        Int64 index;
        for (int i = 0; i < list.Count; i++)
        {
            recDate = list[i];
            index = recDate.playerSeat;
            data = _infoList[(int)index - 1];
            data.HandCardList.Clear();
            recDate.cardList.Sort();
            for (int j = 0; j < recDate.cardList.Count; j++)
            {
                data.AddHandCard(recDate.cardList[j]);
            }
        }
        EventDispatcher.GetInstance().UIFrameWorkEventManager.TriggerEvent<List<PlayerData>>(EventId.UIFrameWork_Reconnection_IsHu, _infoList); 
    }

    private void ChangeSeatIndex(Int64 seatNum)
    {
        if (TableController.Instance.creatRoomInfo.playerNum == (int)PeopleNum.TwoPeople)
        {
            foreach (PlayerData data in _infoList)
            {
                if (data.IsSelfData)
                {
                    data.playerType = CharacterType.relative_orignal;
                }
                else
                {
                    data.playerType = CharacterType.relative_FrontPostion;
                }
            }
        }
        else if (TableController.Instance.creatRoomInfo.playerNum == (int)PeopleNum.ThreePeople)
        {
            if (seatNum != 1)
            {
                Int64 ratio = seatNum - 1;
                foreach (PlayerData data in _infoList)
                {
                    //if (ratio = )
                    //{

                    //}
                    if (data.playerOrderIndex-selfOrderIndex == -1)
                    {
                        data.playerType = CharacterType.relative_LeftPostion;
                    }
                    else if (data.playerOrderIndex - selfOrderIndex == -2)
                    {
                        data.playerType = CharacterType.relative_FrontPostion;
                    }
                    else if (data.playerOrderIndex - selfOrderIndex == 1)
                    {
                        data.playerType = CharacterType.relative_RightPostion;
                    }
                    else
                    {
                        data.playerType = CharacterType.relative_orignal;
                    }
                    //data.playerType = (CharacterType)Convert((Int64)data.playerType - ratio);
                }
            }
        }
        else
        {
            //自己的默认座位号是1      
            if (seatNum != 1)
            {
                Int64 ratio = seatNum - 1;
                foreach (PlayerData data in _infoList)
                {
                    data.playerType = (CharacterType)Convert((Int64)data.playerType - ratio);
                }
            }
        }
    }

    private Int64 Convert(Int64 num)
    {
        Int64 newNum;
        if (num < 0)
        {
            newNum = num + _playerCount;
        }
        else
        {
            newNum = num;
        }
        return newNum;
    }

    public void PlayerUpOutRoom(Hashtable table)
    {
        Int64 index = (Int64)table["site"];
        PlayerData data = _infoList[(Int32)index - 1];
        int sitType = (int)data.playerType;
        EventDispatcher.GetInstance().UIFrameWorkEventManager.TriggerEvent<int>(EventId.UIFrameWork_Player_Exit_Room, sitType);
        data.Dispose();
    }

    /// <summary>
    /// 给手牌添加数据
    /// </summary>
    /// <param name="list"></param>
    public void DealCard(ArrayList list)
    {
        PlayerData data = _infoList[(int)selfOrderIndex - 1];
        for (int i = 0; i < list.Count; i++)
        {
            data.HandCardList[i] = (Int64)list[i];
        }
    }

    /// <summary>
    /// 起牌
    /// </summary>
    /// <param name="cardID"></param>
    public void AddCard(Int64 cardID, Int64 index)
    {
        PlayerData data = _infoList[(Int32)index - 1];
        data.AddHandCard(cardID);
        EventDispatcher.GetInstance().UIFrameWorkEventManager.TriggerEvent<PlayerData>(EventId.UIFrameWork_Player_Draw_Card, data);
        if (index == selfOrderIndex)
        {
            List<Int64> tingPaiList;
            tingPaiList = CardRules.CheckPutOutCanTingpai(data.HandCardList, laizi, gameRule);
            if (tingPaiList.Count>0)
            {
                EventDispatcher.GetInstance().UIFrameWorkEventManager.TriggerEvent<List<Int64>>(EventId.UIFrameWork_Putout_Can_Tingpai, tingPaiList);
            }      
        }
    }

    /// <summary>
    /// 如果赖子没有做配牌使用，也是硬胡，那么也就是没赖子
    /// </summary>
    /// <returns></returns>
    public bool CheckSelfIsHaveLaizi()
    {
        int index = (int)selfOrderIndex;
        PlayerData item = _infoList[index - 1];
        if (CardRules.CheckIsYingHu(item.HandCardList))
        {
            return false;
        }     
        return item.HandCardList.Contains(laizi);
    }

    /// <summary>
    /// 别人出牌后，检测自己是否可以碰或者点杠
    /// </summary>
    public int CheckOtherPlayCard(Int64 cardID, Int64 laizipi)
    {
        int temp = 0;
        int flag = 0; ;
        PlayerData data;
        data = _infoList[(Int32)selfOrderIndex - 1];
        if (CardRules.PopEnable(data.HandCardList, cardID, laizipi) == 2)
        {
            flag = 2;
        }
        else if (CardRules.PopEnable(data.HandCardList, cardID, laizipi) == 3)
        {
            flag = 3;
            if (cardID == laizipi)
            {
                flag = 4;
            }
            // EventDispatcher.GetInstance().UIFrameWorkEventManager.TriggerEvent<Int64, Int64>(EventId.SelfCanPengOrGang, 3, cardID);
        }
        if (CardRules.IsHupai(data.HandCardList, laizi, cardID, gameRule))
        {
           // temp = 5;
            if (flag ==2)
            {
                flag = 7;
            }
            else if (flag==3)
            {
                flag = 8;
            }
            else if (flag == 0)
            {
                flag = 9;
            }           
        }
      
       // CheckSelfAddCard(cardID, laizi, laizipi);
        //  CheckSelfByAddCard();
        return flag;
    }

    /// <summary>
    /// 检测第一次发牌，并且是自己摸牌的情况
    /// </summary>
    /// <param name="cardID"></param>
    public void CheckSelfAddCardByFirst(Int64 cardID)
    {
        PlayerData data = _infoList[(int)selfOrderIndex - 1];
        List<Int64> list = new List<Int64>(data.HandCardList);
        list.Remove(cardID);

        CheckSelfAddCardFunc(list, data.RevealCardList, cardID);
    }

    /// <summary>
    /// 自己摸牌后检测 num=6可以杠并且可以胡
    /// </summary>
    /// <param name="cardID"></param>
    /// <param name="laizi"></param>
    public void CheckSelfAddCard(Int64 cardID, Int64 laizi,Int64 laizipi)
    {
        PlayerData data = _infoList[(int)selfOrderIndex - 1];
        CheckSelfAddCardFunc(data.HandCardList,data.RevealCardList, cardID);
    }

    /// <summary>
    /// 自己碰了后检测自己是否可以杠
    /// </summary>
    public void CheckSelfCanAnGang()
    {
        PlayerData data = _infoList[(int)selfOrderIndex - 1];
        int canGangCard = CardRules.CheckBySelfAddCardIsCanGang(data.HandCardList, 0, laizi, GetLaizipi(laizi));
        if (canGangCard != 0)
        {
            EventDispatcher.GetInstance().UIFrameWorkEventManager.TriggerEvent<Int64, Int64>(EventId.SelfCanPengOrGang, 4, canGangCard);
        }
    }

    private void CheckSelfAddCardFunc(List<Int64> list, List<Int64> RevealCardList,Int64 mopai)
    {
        Int64 cardID =0;
        int num = 3;
        int canGangCard = CardRules.CheckBySelfAddCardIsCanGang(list, mopai, laizi, GetLaizipi(laizi));
        if (canGangCard != 0)
        {
            num++;
            cardID = canGangCard;
        }
      
        if (CardRules.PopEnable(RevealCardList, mopai, GetLaizipi(laizi)) == 3)//检测回头笑的情况
        {
            num++;
            cardID = mopai;
        }

        if (CardRules.IsHupai(list, laizi, mopai, gameRule))
        {
            num += 2; ;
        }
        if (num != 3)
        {
            EventDispatcher.GetInstance().UIFrameWorkEventManager.TriggerEvent<Int64, Int64>(EventId.SelfCanPengOrGang, num, cardID);//自己摸牌判断是否可以用杠
        }
    }

    public void PengCard(Int64 cardID, Int64 index)
    {
        int count = 2;
        PlayerData data = _infoList[(int)index-1];
        for (int i = 0; i < 3; i++)
        {
            data.AddRevealCard(cardID);
        }

        if (data.playerOrderIndex == selfOrderIndex)
        {
            for (int j = data.HandCardList.Count - 1; j >= 0; j--)
            {
                if (data.HandCardList[j] == cardID)
                {
                    data.HandCardList.RemoveAt(j);
                    count--;
                    if (count ==0)
                    {
                        break;
                    }
                }
            }
        }
        else
        {
            data.HandCardList.RemoveAt(1);
            data.HandCardList.RemoveAt(0);           
        }     
        EventDispatcher.GetInstance().UIFrameWorkEventManager.TriggerEvent<Int64,Int64>(EventId.UIFrameWork_Player_Peng_Card, cardID, (Int64)data.playerOrderIndex);
    }

    /// <summary>
    /// 检测自己杠或者碰了以后，自己打什么牌可以听
    /// </summary>
    public void CheckSelfPutOutCardCantingTips()
    {
        PlayerData data = _infoList[(int)selfOrderIndex - 1];
        List<Int64> tingPaiList = CardRules.CheckPutOutCanTingpai(data.HandCardList, laizi, gameRule);
        if (tingPaiList.Count > 0)
        {
            EventDispatcher.GetInstance().UIFrameWorkEventManager.TriggerEvent<List<Int64>>(EventId.UIFrameWork_Putout_Can_Tingpai, tingPaiList);
        }
    }

    /// <summary>
    /// cardID 杠的牌，flag 杠的类型，0：自笑，1：回头笑，2：点杠
    /// </summary>
    /// <param name="cardID"></param>
    /// <param name="flag"></param>
    public void GangCard(Int64 cardID, Int64 index, Int64 flag)
    {
        int num = 0;
        int temp = 0;
        PlayerData data = _infoList[(Int32)index - 1];
        if (flag == 0)
        {
            num = 4;
            temp = 4;
            if (cardID == GetLaizipi(laizi))
            {
                temp--;
            }
            if (data.playerOrderIndex == selfOrderIndex)
            {
                for (int j = data.HandCardList.Count - 1; j >= 0; j--)
                {
                    if (data.HandCardList[j] == cardID)
                    {
                        data.HandCardList.RemoveAt(j);
                    }
                }
            }
            else
            {
                for (int i = temp-1; i >=0 ; i--)
                {
                    data.HandCardList.RemoveAt(i);
                }           
            }

            for (int i = 0; i < temp; i++)
            {
                data.AddRevealCard(cardID);
            }
        }
        else if (flag == 1)
        {
            num = 1;
            if (data.playerOrderIndex == selfOrderIndex)
            {
                for (int j = data.HandCardList.Count - 1; j >= 0; j--)
                {
                    if (data.HandCardList[j] == cardID)
                    {
                        data.HandCardList.RemoveAt(j);
                    }
                }                    
            }
            else
            {
                data.HandCardList.RemoveAt(0);
            }
            data.AddRevealCard(cardID);
        }
        else if (flag == 2)
        {
            num = 3;
            temp = 3;
            if (cardID == GetLaizipi(laizi))//因为赖子皮只有三张，碰就是杠
            {
                temp--;
            }
            if (data.playerOrderIndex == selfOrderIndex)
            {
                for (int j = data.HandCardList.Count - 1; j >= 0; j--)
                {
                    if (data.HandCardList[j] == cardID)
                    {
                        data.HandCardList.RemoveAt(j);
                    }
                }
            }
            else
            {
                for (int i = temp-1; i >=0; i--)
                {
                    data.HandCardList.RemoveAt(i);
                }
            }

            for (int i = 0; i < temp; i++)
            {
                data.AddRevealCard(cardID);
            }
        }
        if (data.playerOrderIndex == selfOrderIndex)
        {
            EventDispatcher.GetInstance().UIFrameWorkEventManager.TriggerEvent<EffectPrompt>(EventId.UIFrameWork_Effect_Prompt, EffectPrompt.gang);
        }
        EventDispatcher.GetInstance().UIFrameWorkEventManager.TriggerEvent<Int64, Int64,Int64>(EventId.UIFrameWork_Player_Gang_Card, cardID, (Int64)data.playerOrderIndex, num);
    }

    public void CheckIsTingByChoseCard(Int64 choseCardID, Int64 laizi)
    {
       // List<Int64> tingPaiList;
        PlayerData data = _infoList[(int)selfOrderIndex - 1];
        List<Int64> tempList = new List<Int64>(data.HandCardList);
        tempList.Remove(choseCardID);
        CheckIsTingPai(tempList, choseCardID);
    }

    private void CheckIsTingPai(List<Int64> list,Int64 choseCardID)
    {
        List<Int64> tingPaiList;
        tingPaiList = CardRules.CheckIsTing(list, laizi,gameRule, choseCardID);
        EventDispatcher.GetInstance().UIFrameWorkEventManager.TriggerEvent<List<Int64>>(EventId.UIFrameWork_Tingpai, tingPaiList);
    }

    /// <summary>
    /// 出牌
    /// </summary>
    /// <param name="cardID"></param>
    public void PopCard(Int64 cardID, Int64 index,Int64 laizipi,Int64 laizi)
    {
        List<Int64> tingPaiList;
        int flag =0;
        PlayerData data = _infoList[(Int32)index - 1];
        if (index == selfOrderIndex)
        {
            if (cardID == laizi)
            {
                selfPutOutLaiziNum++;
            }
            data.HandCardList.Remove(cardID);
            if (laizi == cardID)
            {
                EventDispatcher.GetInstance().UIFrameWorkEventManager.TriggerEvent<EffectPrompt>(EventId.UIFrameWork_Effect_Prompt, EffectPrompt.lai);
            }
        }
        else
        {
            data.HandCardList.Remove(0);
        }

        if (index != selfOrderIndex)
        {
            if (!TableController.Instance.canNotPengMjList.Contains(cardID))
            {
                // flag = CheckOtherPlayCard(cardID, laizipi
                flag= CheckOtherPlayCard(cardID, laizipi);
            }                 
        }
        else
        {
            tingPaiList = CardRules.CheckIsTing(data.HandCardList,laizi,gameRule);
            if (tingPaiList.Count>0)
            {
                EventDispatcher.GetInstance().UIFrameWorkEventManager.TriggerEvent<List<Int64>>(EventId.UIFrameWork_Tingpai, tingPaiList); 
            }
        }
        EventDispatcher.GetInstance().UIFrameWorkEventManager.TriggerEvent<PlayerData, int>(EventId.UIFrameWork_Player_Out_Card, data, flag);
    }

    /// <summary>
    /// 排序 只需要给自己的排序
    /// </summary>
    /// <param name="index"></param>
    public void SortCard()
    {      
        PlayerData data = _infoList[(Int32)selfOrderIndex - 1];
        CardRules.SortCardsForInt(data.HandCardList,laizi);
        data.HandCardList.Reverse();
        EventDispatcher.GetInstance().UIFrameWorkEventManager.TriggerEvent<List<PlayerData>>(EventId.UIFrameWork_Deal_Card_First, _infoList);                    
    }

    public void ChangePlayerSocre(Hashtable info)
    {
        Hashtable data = info;
        Int64 score;
        foreach (PlayerData item in _infoList)
        {
            foreach (DictionaryEntry it in info)
            {
                if (it.Key.ToString() == item.token)
                {
                    score = Int64.Parse(it.Value.ToString());
                    item.score += score;
                }
            }
        }
        EventDispatcher.GetInstance().UIFrameWorkEventManager.TriggerEvent<List<PlayerData>>(EventId.UIFrameWork_Change_Score, _infoList);
    }

    /// <summary>
    /// 已经知道胡牌人的index。。要修改了
    /// </summary>
    /// <param name="list"></param>
    /// <param name="info"></param>
    public void TreatedHu( Hashtable info)
    {
        PlayerData data;
        CharacterType  hupaiPlayerSeat = CharacterType.Library;
        foreach(DictionaryEntry item in info)
        {
            for (int i = 0; i < _infoList.Count; i++)
            {
                data = _infoList[i];
                if ((string)item.Key == data.token)
                {
                    data.score += (Int64)item.Value;
                    if((Int64)item.Value >0)
                    {
                        hupaiPlayerSeat = data.playerType;
                    }
                }
            }
        }

        EventDispatcher.GetInstance().UIFrameWorkEventManager.TriggerEvent<List<PlayerData>>(EventId.UIFrameWork_Change_Score, _infoList);
        if (hupaiPlayerSeat == CharacterType.Library)
        {
            EventDispatcher.GetInstance().UIFrameWorkEventManager.TriggerEvent<string>(EventId.UIFrameWork_Bug, "TreatedHu里位置为空");
        }
        else
        {
            //EventDispatcher.GetInstance().UIFrameWorkEventManager.TriggerEvent<ArrayList, CharacterType>(EventId.UIFrameWork_Hupai, list, hupaiPlayerSeat);
        }
    }

    /// <summary>
    /// 胡牌后把其他玩家的手牌信息写入数据
    /// </summary>
    /// <param name="list"></param>
    public void ReplacePlayerHandCardDataByHupai(List<HupaiPlayerInfo> list)
    {
        HupaiPlayerInfo recDate;
        PlayerData data;
        Int64 index;
        for (int i = 0; i < list.Count; i++)
        {
            recDate = list[i];
            index = recDate.playerSeat;
            data = _infoList[(int)index - 1];
            data.HandCardList.Clear();
          //  recDate.cardList.Sort();
            for (int j = 0; j < recDate.cardList.Count; j++)
            {
                data.AddHandCard(recDate.cardList[j]);
            }
            data.isHupai = recDate.isHupai;
        }
        EventDispatcher.GetInstance().UIFrameWorkEventManager.TriggerEvent<List<PlayerData>>(EventId.UIFrameWork_Hupai, _infoList);
    }

    public void Reset()
    {
        laizi = 0;
        selfPutOutLaiziNum = 0;
        foreach (PlayerData item in _infoList)
        {
            item.Reset();
        }
    }

    public void Dispose()
    {
       selfSitDown = false;
       Reset();      
    }
}

