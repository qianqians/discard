using System;
using System.Collections;
using System.Collections.Generic;
using TinyFrameWork;
using UnityEngine.SceneManagement;
using Assets.Scripts;
using GameCommon;
using UnityEngine;
public class TableController
{
    private static TableController instance;
    public bool gameIsBegin;//牌局是否开始
    private PlayerInfo _playersInfo;
    private ArrayList _PlayerInfoList;//当前房间玩家列表
    public Int64 laizi;
    public Int64 laizipi;
    public Int64 currentPlayerID;
    public Int64 currentCardID;
    public Int64 selfOrderIndex;
    public Int64 selfGetCardID;
    public Int64 ReconnectionProcesserID;
    public Hashtable SelfInfo;
    public SelfState selfGameState;
    public bool interPanelisShow;
    public Int64 bankerID;//记录的是玩家的出牌顺序
    public CharacterType bankerSeatID;//庄家的是玩家的座位
    public Int64 touziInfo;
    // private ArrayList _huipaiList;
    public bool lastTimePutOutLaizi;//自己上一次打出的牌是否是赖子
    //断线重连相关
    public Int64 surplusCardCount;
    public ReconnectionData reconnectionData;
    private ReconnectionData _reconnectionData;
    private ArrayList _reconnectionSelfHandcardCard;
    public Int64 playerCardPlayerSeat;
    /// <summary>
    /// 当前房间牌局信息
    /// </summary>
    public CreatRoomData creatRoomInfo;
    public int playCount;
    public Hashtable totleCardCountList;
    public Color mjBackColor;
    public bool selfCanChenglaizi;
    private bool _isLiuju;
    // public 
    /// <summary>
    /// 是不是 发牌
    /// </summary>
  //  public bool isFirstAddCard;

    /// <summary>
    /// 胡牌后。每个玩家的手牌
    /// </summary>
    public List<HupaiPlayerInfo> hupaiCompletePlayerHandcardSlist;

    public Hashtable gangPaiState;

    public bool disbandIsOpen;

    //为了处理热碰的情况
    public List<Int64> canNotPengMjList;
    public bool isCanPeng;

    //游戏类型
    public GameType gameType;

    public List<CreatRoomData> selfRoomList;

    /// <summary>
    /// 解散房间状态
    /// </summary>
    public Hashtable voteState;
    public static TableController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new TableController();
            }
            return instance;
        }
    }

    private void Reset()
    {
        laizi = 0;
        laizipi = 0;
        currentPlayerID = 0;
        currentCardID = 0;
        selfGetCardID = 0;
        interPanelisShow = false;
        bankerID = 0;
        lastTimePutOutLaizi = false;
        selfCanChenglaizi = false;
        gangPaiState.Clear();
        canNotPengMjList.Clear();
    }

    public PlayerInfo PlayrInfo
    {
        get
        {
            return _playersInfo;
        }
    }

    public PlayerData GetPlayerDatabyIndex(int index)
    {
        return _playersInfo.DataList[index - 1];
    }

    public CharacterType GetPlayerSeatIndexByUuid(string uuid)
    {
        return _playersInfo.GetPlayerSeatByUuid(uuid);
    }

    public string GetPlayerUuidBySeat(CharacterType seat)
    {
        return _playersInfo.GetPlayerfUuidIndexBySeat(seat);
    }

    public void Init()
    {
        voteState = new Hashtable();
        selfRoomList = new List<CreatRoomData>();
       // gameType = GameType.fangkaGame;
        canNotPengMjList = new List<Int64>();
        gangPaiState = new Hashtable();
        creatRoomInfo = new CreatRoomData();
        selfGameState = SelfState.In_lobby;
        _PlayerInfoList = new ArrayList();
        totleCardCountList = new Hashtable();
        totleCardCountList[PeopleNum.TwoPeople] = 72;
        totleCardCountList[PeopleNum.ThreePeople] = 72;
        totleCardCountList[PeopleNum.FourPeople] = 108;
        hupaiCompletePlayerHandcardSlist = new List<HupaiPlayerInfo>();
        EventDispatcher.GetInstance().MainEventManager.AddEventListener<Hashtable>(EventId.OtherJoinRoom, OnOtherJoinRoom);
        EventDispatcher.GetInstance().MainEventManager.AddEventListener<ArrayList, CreatRoomData>(EventId.SelfJoinRoom, onSelfJoinRoom);
        EventDispatcher.GetInstance().MainEventManager.AddEventListener<KeyValuePair<string, Int64>>(EventId.PlaySitDown, OnPlayerSitDown);

        EventDispatcher.GetInstance().MainEventManager.AddEventListener<Int64, Int64, ArrayList>(EventId.Server_HandCard, OnBeginGame);
        EventDispatcher.GetInstance().MainEventManager.AddEventListener<Int64, Int64>(EventId.ShowLaizi, OnShowLaizi);
        EventDispatcher.GetInstance().MainEventManager.AddEventListener<Int64>(EventId.Order, OnOrderHandle);

        EventDispatcher.GetInstance().MainEventManager.AddEventListener<Int64>(EventId.AddOneCard, OnAddOneHandle);//第一次发牌，如果是自己的庄，则先收到自己摸的牌，再收到摸赖子消息
        EventDispatcher.GetInstance().MainEventManager.AddEventListener<Int64>(EventId.Server_draw, OnPlayerDrawCard);

        EventDispatcher.GetInstance().MainEventManager.AddEventListener<Int64>(EventId.PlayCard, OnPlayPutOutCard);
        EventDispatcher.GetInstance().MainEventManager.AddEventListener<Int64, Int64>(EventId.Server_PengCard, OnPengCard);
        EventDispatcher.GetInstance().MainEventManager.AddEventListener<Int64, Int64, Int64>(EventId.Server_GangCard, OnGangCardHandle);
        EventDispatcher.GetInstance().MainEventManager.AddEventListener<Hashtable>(EventId.Server_GangScore, OnGangScore);
        EventDispatcher.GetInstance().MainEventManager.AddEventListener<ArrayList, Int64, Int64>(EventId.Server_HuPai, OnHupai);
        EventDispatcher.GetInstance().MainEventManager.AddEventListener<ArrayList>(EventId.Server_HuPai_other_player_card, OnOtherPlayerCards);

        EventDispatcher.GetInstance().MainEventManager.AddEventListener<Hashtable>(EventId.Server_HuScore, OnHuScore);
        EventDispatcher.GetInstance().UIFrameWorkEventManager.AddEventListener(EventId.UIFrameWork_Game_liuju, GameLiuJu);
        EventDispatcher.GetInstance().MainEventManager.AddEventListener<string>(EventId.PlayerExitRoom, OnPlayerExitRoom);
        EventDispatcher.GetInstance().UIFrameWorkEventManager.AddEventListener<int, int>(EventId.UIFrameWork_Game_Animation_Playover, OnAnimationPlayover);
        EventDispatcher.GetInstance().UIFrameWorkEventManager.AddEventListener(EventId.Server_End_Game, OnEndGame);
        EventDispatcher.GetInstance().UIFrameWorkEventManager.AddEventListener(EventId.Server_Disband_Eng_Game, OnDisbandEndGame);
        EventDispatcher.GetInstance().MainEventManager.AddEventListener<ReconnectionData>(EventId.Sever_Reconnection, OnReconnection);
        EventDispatcher.GetInstance().MainEventManager.AddEventListener<ArrayList>(EventId.Sever_Reconnection_SelfHandcard, OnReconnectionSelfHandcard);
        EventDispatcher.GetInstance().MainEventManager.AddEventListener<Int64>(EventId.Sever_Reconnection_selfMopai_Num, OnReconnectionSelfMopaiNum);
        EventDispatcher.GetInstance().MainEventManager.AddEventListener<Hashtable>(EventId.Server_Room_list, OnChangRoomList);

        EventDispatcher.GetInstance().MainEventManager.AddEventListener<string, Int64>(EventId.Server_Disband_Game_Vote, OnDisbandgame);
        EventDispatcher.GetInstance().MainEventManager.AddEventListener<Hashtable>(EventId.Server_Disband_Room_Vote_list, OnReconnectionDisbandList);

    }

    private void OnDisbandgame(string uiid, Int64 state)
    {
        voteState[uiid] = state;
        EventDispatcher.GetInstance().UIFrameWorkEventManager.TriggerEvent(EventId.UIFrameWork_Disband_room_Change);
    }

    private void OnReconnectionDisbandList(Hashtable table)
    {
        voteState = table;
    }

    private void OnChangRoomList(Hashtable table)
    {
        Hashtable value;
        CreatRoomData roomInfo;
        // List<string> playerNum = new List<string> { "对窝子", "戳虾子", "晃晃" };
        selfRoomList.Clear();
        foreach (string item in table.Keys)
        {
            roomInfo = new CreatRoomData();
            value = (Hashtable)table[item];
            roomInfo.roomID = item;
            roomInfo.playerNum = (int)(Int64)value["peopleNum"];
            roomInfo.currentPlayerNum = (Int64)value["playerNum"];
            roomInfo.playState = (int)(Int64)value["gameRule"];
            roomInfo.baseScore = (int)(Int64)value["gameScore"];
            roomInfo.jushu = (int)(Int64)value["gameTimes"];
            selfRoomList.Add(roomInfo);
        }

        EventDispatcher.GetInstance().UIFrameWorkEventManager.TriggerEvent(EventId.UIFrameWork_Room_List_Change);
        Debug.Log(table.Count);
    }

    /// <summary>
    /// 断线重连
    /// </summary>
    /// <param name="data"></param>
    private void OnReconnection(ReconnectionData data)
    {
        _reconnectionData = data;
        //  selfGetCardID = _reconnectionData.curr
    }

    private void InitReconnectionData(ReconnectionData data)
    {
        touziInfo = data.dice;
        bankerID = data.bankerID;
        surplusCardCount = data.cardCount;
        laizi = data.laizi;
        gangPaiState = data.gangPaiInfo;
        if (laizi != 0)
        {
            gameIsBegin = true;
        }
        SetLaizipi();
        reconnectionData = data;
        currentPlayerID = data.cardRighter;

        playerCardPlayerSeat = data.playCardPlayerSeat;
        currentCardID = data.processer_card;

        creatRoomInfo.baseScore = (int)data.score;
        creatRoomInfo.jushu = (int)data.times;

        ReconnectionProcesserID = data.processer;
        _playersInfo.laizi = data.laizi;
        if (data.cardRighter == selfOrderIndex)
        {
            selfGameState = SelfState.Can_playCard;
        }
        if (data.laizi != 0)
        {
            _playersInfo.Reconnection(data);
        }
    }

    /// <summary>
    /// 如果 断线的时候自己摸牌，则收到自己摸的是什么牌
    /// </summary>
    /// <param name="arr"></param>
    private void OnReconnectionSelfMopaiNum(Int64 mopai)
    {
        selfGetCardID = mopai;
    }

    /// <summary>
    /// 断线 自己的手牌
    /// </summary>
    /// <param name="arr"></param>
    private void OnReconnectionSelfHandcard(ArrayList arr)
    {
        _reconnectionSelfHandcardCard = arr;
    }

    private void ReconnectionSelfHandcard(ArrayList arr)
    {
        if (arr != null)
        {
            if (arr.Count > 0)
            {
                _playersInfo.Reconnection_SelfHandCard(arr);

                if (_reconnectionData.isHu)
                {
                    _playersInfo.ReconnectionShowByIsHu(_reconnectionData.hupaiPlayerInfoList);
                }
                else
                {
                    if (selfGetCardID != 0)
                    {
                        _playersInfo.ReconnectionRemoveMopai(selfGetCardID);//踢掉摸的牌然后显示自己的手牌
                        CheckSelfByAddCard(selfGetCardID);
                        _playersInfo.AddCard(selfGetCardID, selfOrderIndex);
                    }
                    else
                    {
                        _playersInfo.ReconnectionShowSelfCard(selfOrderIndex);//显示自己的手牌
                        _playersInfo.ReconnectionCheckSelfCanInter(currentCardID, laizipi, currentPlayerID);
                        if (playerCardPlayerSeat != 0)
                        {
                            //   OnPlayPutOutCard(currentCardID);
                            _playersInfo.PopCard(currentCardID, playerCardPlayerSeat, laizipi, laizi);
                        }
                    }
                }
            }
        }

    }

    private void OnEndGame()
    {
        gameIsBegin = false;
        DisPoseData();
        SelfInfo["site"] = (Int64)0;
        _PlayerInfoList.Clear();
    }

    private void OnDisbandEndGame()
    {
        OnEndGame();
    }

    private void OnAnimationPlayover(int cardID, int flag)
    {
        EventDispatcher.GetInstance().UIFrameWorkEventManager.TriggerEvent<Int64, Int64>(EventId.SelfCanPengOrGang, flag, cardID); //别人出牌判断自己是否可以碰或者杠          
    }

    /// <summary>
    /// 自己进入房间
    /// </summary>
    /// <param name="list"></param>
    private void onSelfJoinRoom(ArrayList list, CreatRoomData info)
    {
        creatRoomInfo = info;
        _playersInfo = new PlayerInfo(creatRoomInfo.playerNum);
        _playersInfo.gameRule = (GameRule)creatRoomInfo.playState;
        _PlayerInfoList = list;
        selfGameState = SelfState.Join_room;

        if (creatRoomInfo.payState == (int)PayRule.AAPay || creatRoomInfo.payState == (int)PayRule.OnePay)
        {
            gameType = GameType.fangkaGame;
        }
        else if (creatRoomInfo.payState == (int)PayRule.MatchPay)
        {
            gameType = GameType.integarl;
        }
        if (!CheckIsHaveSelf(SelfInfo))//区分断线重连
        {
            _PlayerInfoList.Add(SelfInfo);
            _playersInfo.SelfJoinRoom(_PlayerInfoList);
            selfOrderIndex = _playersInfo.selfOrderIndex;
            // SceneManager.LoadScene("InGame");
            MainManager.Instance.dontDestroyOnLoad.StartLoadingScene("InGame", RoutineJoinAndExitGameRoom);
        }
        else
        {
            _playersInfo.SelfJoinRoom(_PlayerInfoList);
            selfOrderIndex = _playersInfo.selfOrderIndex;
            MainManager.Instance.dontDestroyOnLoad.StartLoadingScene("InGame", ReconnectionLoadGameSceneComplete);
        }
    }

    private void RoutineJoinAndExitGameRoom()
    {
        MainManager.Instance.dontDestroyOnLoad.SetLoading(false);
    }

    private void ReconnectionLoadGameSceneComplete()
    {
        if (_reconnectionData != null)
        {
            InitReconnectionData(_reconnectionData);
            ReconnectionSelfHandcard(_reconnectionSelfHandcardCard);
            MainManager.Instance.dontDestroyOnLoad.SetLoading(false);
        }
    }

    private bool CheckIsHaveSelf(Hashtable selfInfo)
    {
        foreach (Hashtable item in _PlayerInfoList)
        {
            if ((string)item["unionid"] == (string)selfInfo["unionid"])
            {
                return true;
            }
        }
        return false;
    }

    private void OnOtherJoinRoom(Hashtable tab)
    {
        _PlayerInfoList.Add(tab);
    }

    private void OnPlayerSitDown(KeyValuePair<string, Int64> kvp)
    {
        Hashtable item;
        for (int i = 0; i < _PlayerInfoList.Count; i++)
        {
            item = (Hashtable)_PlayerInfoList[i];
            if ((string)item["unionid"] == kvp.Key)
            {
                if (kvp.Key == SocketClient.Instance.playToken)
                {
                    selfGameState = SelfState.SitDown;
                    selfOrderIndex = kvp.Value;
                }
                item["site"] = kvp.Value;
                _playersInfo.PlayerSitDown(item);
            }
        }
    }

    private void CheckSelfByAddCard(Int64 mj)
    {
        if (laizi != 0)
        {
            _playersInfo.CheckSelfAddCard(mj, laizi, laizipi);
        }
    }

    //第一次发牌，只能收到自己的牌
    private void OnBeginGame(Int64 bankerid, Int64 touzi, ArrayList list)
    {
        gameIsBegin = true;
        _playersInfo.Reset();
        //  isFirstAddCard = true;
        lastTimePutOutLaizi = false;
        playCount = 0;
        bankerID = bankerid;
        touziInfo = touzi;
        surplusCardCount = (int)TableController.Instance.totleCardCountList[(PeopleNum)TableController.Instance.creatRoomInfo.playerNum] - TableController.Instance.creatRoomInfo.playerNum * 13 - 1;
        _playersInfo.DealCard(list);
    }

    private void OnShowLaizi(Int64 lzp, Int64 lz)
    {
        laizi = lz;
        laizipi = lzp;
        _playersInfo.laizi = laizi;
        _playersInfo.SortCard();

        //   _playersInfo.CheckSelfAddCardByFirst(selfGetCardID);
    }

    private void OnOrderHandle(Int64 orderIndex)
    {
        currentPlayerID = orderIndex;
        if (currentPlayerID == selfOrderIndex)
        {
            selfGameState = SelfState.Can_playCard;
            TableController.Instance.canNotPengMjList.Clear();
        }
        else
        {
            selfGameState = SelfState.Not_playCard;
        }
    }

    /// <summary>
    /// 玩家出牌
    /// </summary>
    /// <param name="cardID"></param>
    public void OnPlayPutOutCard(Int64 cardID)
    {
        currentCardID = cardID;
        if (currentPlayerID == selfOrderIndex && laizi == cardID)
        {
            lastTimePutOutLaizi = true;
        }
        else
        {
            lastTimePutOutLaizi = false;
        }
        _playersInfo.PopCard(currentCardID, currentPlayerID, laizipi, laizi);
    }

    /// <summary>
    /// 有人碰牌了，广播
    /// </summary>
    /// <param name="card"></param>
    /// <param name="cardID"></param>
    public void OnPengCard(Int64 index, Int64 cardID)
    {
        _playersInfo.PengCard(cardID, index);
        if (index == selfOrderIndex)
        {
            _playersInfo.CheckSelfCanAnGang();
        }
    }

    public void OnGangCardHandle(Int64 index, Int64 cardID, Int64 gangType)
    {
        _playersInfo.GangCard(cardID, index, gangType);
        if (index == selfOrderIndex)
        {
            lastTimePutOutLaizi = false;
            _playersInfo.CheckSelfCanAnGang();
        }
    }

    /// <summary>
    /// 添加手牌,只有自己摸牌的时候才会收到
    /// </summary>
    private void OnAddOneHandle(Int64 mjInfo)
    {
        selfGetCardID = mjInfo;
        CheckSelfByAddCard(mjInfo);
    }

    /// <summary>
    /// 玩家摸牌（通知客户端播放摸牌的动画）
    /// </summary>
    /// <param name="playerID"></param>
    public void OnPlayerDrawCard(Int64 playerID)
    {
        if (playerID == selfOrderIndex)
        {
            _playersInfo.AddCard(selfGetCardID, playerID);
        }
        else
        {
            _playersInfo.AddCard(0, playerID);
        }
    }

    public void SelfChoseCardCheckTing(Int64 cardID)
    {
        _playersInfo.CheckIsTingByChoseCard(cardID, laizi);
    }

    /// <summary>
    /// 杠牌后，改变玩家分数
    /// </summary>
    /// <param name="info"></param>
    private void OnGangScore(Hashtable info)
    {
        _playersInfo.ChangePlayerSocre(info);
    }

    private void OnHupai(ArrayList list, Int64 mopai, Int64 seat)
    {
        HupaiPlayerInfo hupInfo;
        hupInfo = new HupaiPlayerInfo();
        for (int j = 0; j < list.Count; j++)
        {
            hupInfo.cardList.Add((Int64)list[j]);
        }
        hupInfo.cardList.Sort();
        hupInfo.cardList.Remove(mopai);
        hupInfo.cardList.Add(mopai);
        // CardRules.SortCardsForInt(hupInfo.cardList, mopai);
        hupInfo.cardList.Reverse();
        hupInfo.playerSeat = seat;
        hupInfo.isHupai = true;
        hupaiCompletePlayerHandcardSlist.Add(hupInfo);
        Reset();
    }

    private void OnOtherPlayerCards(ArrayList otherPlayerCard)
    {
        HupaiPlayerInfo hupInfo;
        Hashtable OnterInfo;
        ArrayList cardList;
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
            hupInfo.cardList.Sort();
            hupaiCompletePlayerHandcardSlist.Add(hupInfo);
        }

        if (_isLiuju)
        {
            _isLiuju = false;
            selfGameState = SelfState.Not_playCard;
            _playersInfo.ReplacePlayerHandCardDataByHupai(hupaiCompletePlayerHandcardSlist);
            hupaiCompletePlayerHandcardSlist.Clear();
        }
    }

    private void OnHuScore(Hashtable info)
    {
        selfGameState = SelfState.Not_playCard;
        _playersInfo.TreatedHu(info);
        _playersInfo.ReplacePlayerHandCardDataByHupai(hupaiCompletePlayerHandcardSlist);
        hupaiCompletePlayerHandcardSlist.Clear();
    }

    private void GameLiuJu()
    {
        _isLiuju = true;
        Reset();
        //selfGameState = SelfState.Not_playCard;
        //_playersInfo.ReplacePlayerHandCardDataByHupai(hupaiCompletePlayerHandcardSlist);
    }

    //private void resetGame()
    //{
    //    SocketClient.Instance.ReadNextGame();
    //}

    private void OnPlayerExitRoom(string token)
    {
        if (PlayerExitRoom(token))
        {
            //退出游戏界面，进入大厅
            OnEndGame();
            //  SceneManager.LoadScene("NUMainWindow");
            MainManager.Instance.dontDestroyOnLoad.StartLoadingScene("NUMainWindow", RoutineJoinAndExitGameRoom);
        }
    }

    /// <summary>
    /// 有玩家离开
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    private bool PlayerExitRoom(string token)
    {
        Hashtable item;
        for (int i = _PlayerInfoList.Count - 1; i >= 0; i--)
        {
            item = (Hashtable)_PlayerInfoList[i];
            if ((string)item["unionid"] == token)
            {
                _PlayerInfoList.RemoveAt(i);
                if ((Int64)item["site"] != 0)
                {
                    _playersInfo.PlayerUpOutRoom(item);
                }
                if (token == SocketClient.Instance.playToken)
                {
                    return true;
                }
                break;
            }
        }
        return false;
    }

    private void DisPoseData()
    {
        if (_playersInfo != null)
        {
            _playersInfo.Dispose();
        }
        selfOrderIndex = 0;
        selfGameState = SelfState.In_lobby;
        selfGetCardID = 0;
    }

    private void SetLaizipi()
    {
        laizipi = laizi - 1;
        if (laizipi % 10 == 0)
        {
            laizipi = laizi + 8;
        }
    }

    public void RemoveEventListener()
    {
        if (voteState != null)
        {
            voteState.Clear();
        }
        if (selfRoomList != null)
        {
            selfRoomList.Clear();
        }
        gangPaiState = null;
        DisPoseData();
        _reconnectionData = null;
        reconnectionData = null;
        _reconnectionSelfHandcardCard = null;
        EventDispatcher.GetInstance().MainEventManager.RemoveEventListener<Hashtable>(EventId.OtherJoinRoom, OnOtherJoinRoom);
        EventDispatcher.GetInstance().MainEventManager.RemoveEventListener<ArrayList, CreatRoomData>(EventId.SelfJoinRoom, onSelfJoinRoom);
        EventDispatcher.GetInstance().MainEventManager.RemoveEventListener<KeyValuePair<string, Int64>>(EventId.PlaySitDown, OnPlayerSitDown);
        EventDispatcher.GetInstance().MainEventManager.RemoveEventListener<Int64, Int64, ArrayList>(EventId.Server_HandCard, OnBeginGame);
        EventDispatcher.GetInstance().MainEventManager.RemoveEventListener<Int64, Int64>(EventId.ShowLaizi, OnShowLaizi);
        EventDispatcher.GetInstance().MainEventManager.RemoveEventListener<Int64>(EventId.Order, OnOrderHandle);
        EventDispatcher.GetInstance().MainEventManager.RemoveEventListener<Int64>(EventId.AddOneCard, OnAddOneHandle);
        EventDispatcher.GetInstance().MainEventManager.RemoveEventListener<ArrayList>(EventId.Server_HuPai_other_player_card, OnOtherPlayerCards);

        EventDispatcher.GetInstance().MainEventManager.RemoveEventListener<Int64>(EventId.Server_draw, OnPlayerDrawCard);
        EventDispatcher.GetInstance().MainEventManager.RemoveEventListener<Int64>(EventId.PlayCard, OnPlayPutOutCard);
        EventDispatcher.GetInstance().MainEventManager.RemoveEventListener<Int64, Int64>(EventId.Server_PengCard, OnPengCard);
        EventDispatcher.GetInstance().MainEventManager.RemoveEventListener<Int64, Int64, Int64>(EventId.Server_GangCard, OnGangCardHandle);
        EventDispatcher.GetInstance().MainEventManager.RemoveEventListener<Hashtable>(EventId.Server_GangScore, OnGangScore);
        EventDispatcher.GetInstance().MainEventManager.RemoveEventListener<ArrayList, Int64, Int64>(EventId.Server_HuPai, OnHupai);
        EventDispatcher.GetInstance().MainEventManager.RemoveEventListener<Hashtable>(EventId.Server_HuScore, OnHuScore);
        EventDispatcher.GetInstance().UIFrameWorkEventManager.RemoveEventListener(EventId.UIFrameWork_Game_liuju, GameLiuJu);
        EventDispatcher.GetInstance().MainEventManager.RemoveEventListener<string>(EventId.PlayerExitRoom, OnPlayerExitRoom);
        EventDispatcher.GetInstance().UIFrameWorkEventManager.RemoveEventListener<int, int>(EventId.UIFrameWork_Game_Animation_Playover, OnAnimationPlayover);
        EventDispatcher.GetInstance().UIFrameWorkEventManager.RemoveEventListener(EventId.Server_End_Game, OnEndGame);
        EventDispatcher.GetInstance().UIFrameWorkEventManager.RemoveEventListener(EventId.Server_Disband_Eng_Game, OnDisbandEndGame);
        EventDispatcher.GetInstance().MainEventManager.RemoveEventListener<ReconnectionData>(EventId.Sever_Reconnection, OnReconnection);
        EventDispatcher.GetInstance().MainEventManager.RemoveEventListener<ArrayList>(EventId.Sever_Reconnection_SelfHandcard, OnReconnectionSelfHandcard);
        EventDispatcher.GetInstance().MainEventManager.RemoveEventListener<Int64>(EventId.Sever_Reconnection_selfMopai_Num, OnReconnectionSelfMopaiNum);
        EventDispatcher.GetInstance().MainEventManager.RemoveEventListener<Hashtable>(EventId.Server_Room_list, OnChangRoomList);

        EventDispatcher.GetInstance().MainEventManager.RemoveEventListener<string, Int64>(EventId.Server_Disband_Game_Vote, OnDisbandgame);
        EventDispatcher.GetInstance().MainEventManager.RemoveEventListener<Hashtable>(EventId.Server_Disband_Room_Vote_list, OnReconnectionDisbandList);
    }
}

