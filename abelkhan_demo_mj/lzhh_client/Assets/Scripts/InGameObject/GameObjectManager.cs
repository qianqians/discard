using System.Collections;
using System.Collections.Generic;
using GameCommon;
using UnityEngine;
using TinyFrameWork;
using System;
using Assets.Scripts;
public class CardObject
{
    public GameObject card;
    public mjCards cardType;
    public void InstanceObject( GameObject parmObject, mjCards parmCardType)
    {
        card = parmObject;
        cardType = parmCardType;
    }
}

/// <summary>
/// 记录 码牌的数据用于显示牌
/// </summary>
public class TableNoShowCardObject
{
    public int index;
    public CharacterType relativePostion;//麻将所在的相对方位 
}

/// <summary>
///  公共牌的数据
/// </summary>
public class TableNOShowCardData
{
    public List<TableNoShowCardObject> cardObjList;
    public int totalCardCount;
    private int playerNum;
    public TableNOShowCardData()
    {
        //_totalCardCount = 108;   
      //  CreatData();
    }

    public void Reset()
    {
        cardObjList.Clear();
        CreatData();
    }

    private void CreatData()
    {
        cardObjList = new List<TableNoShowCardObject>();
        if ((PeopleNum)playerNum == PeopleNum.FourPeople)
        {         
            CreateCardData(26, CharacterType.relative_orignal);
            CreateCardData(28, CharacterType.relative_LeftPostion);
            CreateCardData(26, CharacterType.relative_FrontPostion);
            CreateCardData(28, CharacterType.relative_RightPostion);
        }
        else
        {
            CreateCardData(18, CharacterType.relative_orignal);
            CreateCardData(18, CharacterType.relative_LeftPostion);
            CreateCardData(18, CharacterType.relative_FrontPostion);
            CreateCardData(18, CharacterType.relative_RightPostion);
        }
    }

   // private 
    private void CreateCardData(int count, CharacterType type)
    {
        TableNoShowCardObject cardObj;
        for (int i = 0; i < count; i++)
        {
            cardObj = new TableNoShowCardObject();
            cardObj.relativePostion = type;
            cardObj.index = i;
            cardObjList.Add(cardObj);
        }
    }

    /// <summary>
    /// 根据骰子和牌的数量调整数据
    /// </summary>
    /// <param name="cardCount"></param>
    /// <param name="DiceA"></param>
    /// <param name="DiceB"></param>
    /// <param name="bankerIndex"></param>
    public void Init(int cardCount,Int64 Dice,CharacterType bankerIndex,int playerCount)
    {
        playerNum = playerCount;
        CreatData();
        int DiceA = (int)Dice / 10;
        int DiceB = (int)Dice % 10;
        int minDice = DiceA > DiceB ? DiceB : DiceA;
        int beginIndex = (DiceA + DiceB + (int)bankerIndex - 1) % playerCount;
        int cutOffIndex = 0;
        List<TableNoShowCardObject> list1;
        List<TableNoShowCardObject> list2;
        if ((PeopleNum)playerCount == PeopleNum.FourPeople)
        {
        
            if (beginIndex == 3)
            {
                cutOffIndex = minDice * 2 + 26;
            }
            if (beginIndex == 2)
            {
                cutOffIndex = minDice * 2 + 26 + 28;
            }
            if (beginIndex == 1)
            {
                cutOffIndex = minDice * 2 + 26 + 28 + 26;
            }
            if (beginIndex == 0)
            {
                cutOffIndex = minDice * 2;
            }
            list1 = cardObjList.GetRange(0, cutOffIndex);
            list2 = cardObjList.GetRange(cutOffIndex, cardObjList.Count - cutOffIndex);
            list2.AddRange(list1);
            cardObjList = list2.GetRange(totalCardCount - cardCount, cardCount);
        }
        else
        {
            if (beginIndex == 3)
            {
                cutOffIndex = minDice * 2 + 18;
            }
            if (beginIndex == 2)
            {
                cutOffIndex = minDice * 2 + 18 + 18;
            }
            if (beginIndex == 1)
            {
                cutOffIndex = minDice * 2 + 18 + 18 + 18;
            }
            if (beginIndex == 0)
            {
                cutOffIndex = minDice * 2;
            }
            list1 = cardObjList.GetRange(0, cutOffIndex);
            list2 = cardObjList.GetRange(cutOffIndex, cardObjList.Count - cutOffIndex);
            list2.AddRange(list1);
            cardObjList = list2.GetRange(totalCardCount - cardCount, cardCount);
        }      
    }

    private void AdjustCardListForThreePlayer(int cardCount, Int64 Dice, CharacterType bankerIndex, int playerCount)
    {
        int DiceA = (int)Dice / 10;
        int DiceB = (int)Dice % 10;
        int minDice = DiceA > DiceB ? DiceB : DiceA;
        int beginIndex = (DiceA + DiceB + (int)bankerIndex - 1) % playerCount;
        int cutOffIndex = 0;
        List<TableNoShowCardObject> list1;
        List<TableNoShowCardObject> list2;
        if (beginIndex == 2)
        {
            cutOffIndex = minDice * 2 +24;
        }
        if (beginIndex == 1)
        {
            cutOffIndex = minDice * 2 +48;
        }
        if (beginIndex == 0)
        {
            cutOffIndex = minDice * 2;
        }
        list1 = cardObjList.GetRange(0, cutOffIndex);
        list2 = cardObjList.GetRange(cutOffIndex, cardObjList.Count - cutOffIndex);
        list2.AddRange(list1);
        cardObjList = list2.GetRange(totalCardCount - cardCount, cardCount);
    }
}

class RelationData
{
    public int ColumnNum = 6;
    private int OnTableShowNum = 0;
    public int playerOrderIndex = 0;//玩家的出牌顺序
    public CharacterType playerSeatIndex;//玩家座位编号
    public List<CardObject> CardObjects;//手牌
    public List<GameObject> HandShowCardObjects;
    public List<GameObject> OnTableShowObjects;
    public List<GameObject> LaiziObjects;
    public Vector3 PositionOnTableNoShow;//桌面没有翻开牌的位置
    public Vector3 PositionOnTableShow;//桌面翻开排的位置
    public Vector3 PlayePosition;//桌面出牌的位置
    public Vector3 PositionOnHandShow;//桌面碰过或者杠了的牌的位置
    public Vector3 PositionShowLaizi;//玩家放打出去的癞子位置
    public Vector3 RotationPostion;//根据位置需要旋转的角度
    public Vector3 RotationBackPostion;//根据位置需要 旋转到背面的角度
    public Vector3 Direction;//
   // public Vector2 handPosition;
    public GameObject Container;
    public GameObject TableNoshowContainer;
    public void Init()
    {
        CardObjects = new List<CardObject>();
        HandShowCardObjects = new List<GameObject>();
        OnTableShowObjects = new List<GameObject>();
        LaiziObjects = new List<GameObject>();
        Container = new GameObject();
        TableNoshowContainer = new GameObject();
    }
    public int GetTableShowNum()
    {
        return OnTableShowNum;
    }

    public Vector3 GetShowOnTablePosition(float offsetXTemp, float offsetYTemp)
    {
        Vector3 vc3 = new Vector3(0.0f, 0.0f, 0.0f);
        Vector3 tempVec = Vector3.Cross(new Vector3(0.0f, 1f, 0.0f), Direction);
        OnTableShowNum = OnTableShowObjects.Count+1;
        if (OnTableShowNum > 1)
        {
            vc3 = ((OnTableShowNum - 1) % ColumnNum) * offsetXTemp * Direction + (Mathf.Ceil((OnTableShowNum - 1) / ColumnNum)) * 2 * offsetYTemp* tempVec;
        }
        return vc3;
    }

    public Vector3 GetShowLaiziPosition(float offsetXTemp, float offsetYTemp)
    {
        Vector3 vc3 = new Vector3(0.0f, 0.0f, 0.0f);
        Vector3 tempVec = Vector3.Cross(new Vector3(0.0f, 1f, 0.0f), Direction);
        int num = LaiziObjects.Count;
        if (num >= 1)
        {
            vc3 =(num - 1)  * offsetXTemp * Direction -  2.2f*offsetYTemp * tempVec;
        }
        return vc3;
    }
}

public class GameObjectManager : MonoBehaviour {

    CardPoolManager cardPoolManager;
    public GameObject CameraObject;
    Vector3 Cameraposition;
    Vector3 CameraDirection;
    float DistanceFromEyeZ;
    float DistanceFromEyeY;
    public GameObject RightPosition;//手牌的位置
    public GameObject FrontPosition;
    public GameObject LeftPosition;

    GameObject LeftPositionOnTableNoShow;//桌面没有翻开牌的位置
    GameObject RightPositionOnTableNoShow;
    GameObject DownPositionOnTableNoShow;
    GameObject FrontPositionOnTableNoShow;
    GameObject LeftPositionOnTableShow;//桌面翻开p牌的位置
    GameObject RightPositionOnTableShow;
    GameObject DownPositionOnTableShow;
    GameObject FrontPositionOnTableShow;
    GameObject LeftPlayePosition;//桌面出牌的位置
    GameObject RightPlayePosition;
    GameObject DownPlayePosition;
    GameObject FrontPlayePosition;
    GameObject CameraRotationPosition;//摄像机器盯着的方向

    public GameObject LeftPositionOnHandShow;//桌面碰或者杠了的位置
    public GameObject RightPositionOnHandShow;
    public GameObject DownPositionOnHandShow;
    public GameObject FrontPositionOnHandShow;

    public GameObject LeftPositionLaizi;//桌面放打出了的癞子位置
    public GameObject RightPositionLaizi;
    public GameObject DownPositionLaizi;
    public GameObject FrontPositionLaizi;
    private int _RemoveTableCardparam;//桌面牌减少的时候为了让牌从上往下开始减少的参数；

    float offsetx;
    float offsety;
    List<RelationData> RelationDataArray;//根据玩家记录一些数据
    private List<GameObject> _tableNoShowCardList;
    private GameObject _currentPutOutCard;
    private CharacterType _currentRelative;
    private GameObject _currentClickMj;
   
    private List<PlayerData> _playerInfoList;
    //
    public GameObject TouziMovie;
    public GameObject Touzi0;
    public GameObject Touzi1;
   // public bool isBeginGame;

    //桌面动画
    CameraAnim cameraAnimation;
    // CameraAnim TableAnimation;
    public GameObject TableObject;

    private float SpeedNum;
    private float upNum = -0.155f;
    private bool upMovieFlag;
    public GameObject upMaPai;
    public GameObject TableObj;
    public Animator tableAnim;

    //发牌的参数每次4张牌
    private int _dealPlayerIndex;
    private int _dealNum;

    public GameObject selfHandContinue;
    //桌子上没有翻开的牌 Vector3 StarPosition, Vector3 Direction, Vector3 Rotation
    // Use this for initialization
    public Animation anim;
    private List<Vector3> _diceRotateRadio;

    private TableNOShowCardData _tableNOShowCardData;
    private bool _isGang;//用来改变杠了后从后面拿牌
    private bool delayedFlag;

    public GameObject clickAssistPanel = null;
    /// <summary>
    /// 一把打完后，重置桌面
    /// </summary>
    private void ResetTable()
    {
        List<CardObject> list;
        List<GameObject> arr;
        GameObject obj;
        CardObject cardObj;
        CardData data;
        for (int i = _tableNoShowCardList.Count-1; i >=0; i--)
        {
            obj = _tableNoShowCardList[i];
            _tableNoShowCardList.RemoveAt(i);
            cardPoolManager.ReleaseByGameObject(mjCards.Nodefine, obj);
        }
        try
        {
            for (int i = 0; i < 4; i++)
            {
                //回收手牌
                list = RelationDataArray[i].CardObjects;
                for (int j = list.Count - 1; j >= 0; j--)
                {
                    cardObj = list[j];
                    list.RemoveAt(j);
                    cardPoolManager.ReleaseByGameObject(cardObj.cardType, cardObj.card);
                }

                //碰或者杠了的牌
                arr = RelationDataArray[i].HandShowCardObjects;
                for (int j = arr.Count - 1; j >= 0; j--)
                {
                    obj = arr[j];
                    arr.RemoveAt(j);
                    data = obj.GetComponent<CardData>();
                    cardPoolManager.ReleaseByGameObject(data.CardType, obj);
                }

                //赖子牌
                arr = RelationDataArray[i].LaiziObjects;
                for (int j = arr.Count - 1; j >= 0; j--)
                {
                    obj = arr[j];
                    arr.RemoveAt(j);
                    data = obj.GetComponent<CardData>();
                    cardPoolManager.ReleaseByGameObject(data.CardType, obj);
                }

                //打出去的牌
                arr = RelationDataArray[i].OnTableShowObjects;
                for (int j = arr.Count - 1; j >= 0; j--)
                {
                    obj = arr[j];
                    arr.RemoveAt(j);
                    data = obj.GetComponent<CardData>();
                    cardPoolManager.ReleaseByGameObject(data.CardType, obj);
                }
            }
        }
        catch (Exception e)
        {
            NUMessageBox.Show(e.Message);
        }
      
        _RemoveTableCardparam = 0;
        _tableNOShowCardData.Reset();
        SocketClient.Instance.ReadNextGame();
    }

    void Start() {       
        SpeedNum = 0.032f;
        anim  = TouziMovie.GetComponent<Animation>();
        anim["Take 001"].speed = 8;
        _currState = anim["Take 001"];
       // anim.Play();
        anim.Stop("Take 001");
        _diceRotateRadio = new List<Vector3> { new Vector3(180, 0, 0), new Vector3(270, 0, 0), new Vector3(0, 0, 90), new Vector3(0, 0, 270), new Vector3(90, 0, 0), new Vector3(0, 0, 0) };     
        _tableNOShowCardData = new TableNOShowCardData();//桌面公共牌的数据

        cardPoolManager = new CardPoolManager();
        cardPoolManager.LoadAllCard();
        if (MainManager.Instance.selfMjBackColorState != "")
        {
            OnSetMjBack(MainManager.Instance.selfMjBackColorState);
        }
        anGangMjArr = new List<Int64>();
        otherGangMjArr = new List<Int64>();
        EventDispatcher.GetInstance().UIFrameWorkEventManager.AddEventListener<List<PlayerData>, bool>(EventId.UIFrameWork_Player_Sit_down, OnPlaySitDown);
        EventDispatcher.GetInstance().UIFrameWorkEventManager.AddEventListener<List<PlayerData>>(EventId.UIFrameWork_Deal_Card_First, BeignUpMjMovie);
        EventDispatcher.GetInstance().UIFrameWorkEventManager.AddEventListener<PlayerData,int>(EventId.UIFrameWork_Player_Out_Card, PlayerOutCard);
        EventDispatcher.GetInstance().UIFrameWorkEventManager.AddEventListener<Int64, Int64>(EventId.UIFrameWork_Player_Peng_Card, OnPlayPengCard);
        EventDispatcher.GetInstance().UIFrameWorkEventManager.AddEventListener<Int64, Int64, Int64>(EventId.UIFrameWork_Player_Gang_Card, OnPlaygGangCard);       
        EventDispatcher.GetInstance().UIFrameWorkEventManager.AddEventListener<PlayerData>(EventId.UIFrameWork_Player_Draw_Card, OnAddCard);
        EventDispatcher.GetInstance().UIFrameWorkEventManager.AddEventListener<List<PlayerData>>(EventId.UIFrameWork_Hupai, OnHuPai);
       // EventDispatcher.GetInstance().UIFrameWorkEventManager.AddEventListener<ArrayList>(EventId.UIFrameWork_Game_liuju, OnLiuJu);
        EventDispatcher.GetInstance().UIFrameWorkEventManager.AddEventListener<List<PlayerData>>(EventId.UIFrameWork_Reconnection, OnReconnection);
        EventDispatcher.GetInstance().UIFrameWorkEventManager.AddEventListener<List<PlayerData>>(EventId.UIFrameWork_Reconnection_IsHu, OnReconnectionByIsHu);       
        EventDispatcher.GetInstance().UIFrameWorkEventManager.AddEventListener<List<Int64>>(EventId.UIFrameWork_Putout_Can_Tingpai, OnPutOutCardCanTing);
        EventDispatcher.GetInstance().UIFrameWorkEventManager.AddEventListener(EventId.UIFrameWork_Click_beign_next_game, OnBeginNextGame);
        EventDispatcher.GetInstance().UIFrameWorkEventManager.AddEventListener<string>(EventId.UIFrameWork_Set_mj_back, OnSetMjBack);
        EventDispatcher.GetInstance().UIFrameWorkEventManager.AddEventListener<Int64>(EventId.UIFrameWork_last_mj, OnShowLastMj);
        //RightPosition = GameObject.Find("RightPosition");
        //LeftPosition = GameObject.Find("LeftPosition");
        //FrontPosition = GameObject.Find("FrontPosition
        LeftPositionOnTableNoShow = GameObject.Find("LeftPositionOnTableNoShow");
        FrontPositionOnTableNoShow = GameObject.Find("FrontPositionOnTableNoShow");
        RightPositionOnTableNoShow = GameObject.Find("RightPositionOnTableNoShow");
        DownPositionOnTableNoShow = GameObject.Find("DownPositionOnTableNoShow");

        LeftPositionOnTableShow = GameObject.Find("LeftPositionOnTableShow");
        RightPositionOnTableShow = GameObject.Find("RightPositionOnTableShow");
        DownPositionOnTableShow = GameObject.Find("DownPositionOnTableShow");
        FrontPositionOnTableShow = GameObject.Find("FrontPositionOnTableShow");

        LeftPlayePosition = GameObject.Find("LeftPlayePosition");
        RightPlayePosition = GameObject.Find("RightPlayePosition");
        DownPlayePosition = GameObject.Find("DownPlayePosition");
        FrontPlayePosition = GameObject.Find("FrontPlayePosition");
        CameraRotationPosition = GameObject.Find("CameraRotationPosition");

        Cameraposition = CameraObject.transform.position;
        CameraDirection = CameraObject.transform.forward;
        DistanceFromEyeZ = 3.20f;
        DistanceFromEyeY = 0.51f;

        cameraAnimation = new CameraAnim();
        cameraAnimation.directionObj = TableObject;
        RelationDataArray = new List<RelationData>();
        for (int i = 0; i < (int)CharacterType.relative_Num; i++)
        {
            RelationData tempRelationData = new RelationData();
            tempRelationData.playerSeatIndex = (CharacterType)i;
            switch ( (CharacterType) i)//对坐位写入数据以后好查询不需要写太多if
            {                
                case CharacterType.relative_orignal:            
                    tempRelationData.PositionShowLaizi = DownPositionLaizi.transform.position;
                    tempRelationData.PositionOnTableShow = DownPositionOnTableShow.transform.position;
                    tempRelationData.PositionOnTableNoShow = DownPositionOnTableNoShow.transform.position;
                    tempRelationData.PlayePosition = DownPlayePosition.transform.position;
                    tempRelationData.PositionOnHandShow = DownPositionOnHandShow.transform.position;
                    tempRelationData.RotationPostion = new Vector3(90.0f, 0.0f, 0.0f);
                    tempRelationData.RotationBackPostion = new Vector3(-90,180,0);
                    tempRelationData.Direction = new Vector3(-1.0f, 0.0f, 0.0f);
                    break;
                case CharacterType.relative_LeftPostion:
                    tempRelationData.PositionShowLaizi = LeftPositionLaizi.transform.position;
                    tempRelationData.PositionOnTableShow = LeftPositionOnTableShow.transform.position;
                    tempRelationData.PositionOnTableNoShow = LeftPositionOnTableNoShow.transform.position;
                    tempRelationData.PlayePosition = LeftPlayePosition.transform.position;
                    tempRelationData.PositionOnHandShow = LeftPositionOnHandShow.transform.position;
                    tempRelationData.RotationPostion =  new Vector3(90.0f, 90.0f, 0.0f);
                    tempRelationData.RotationBackPostion = new Vector3(-90, -90, 0);
                    tempRelationData.Direction = new Vector3(0.0f, 0.0f, 1.0f);
                    break;
                case CharacterType.relative_RightPostion:
                    tempRelationData.PositionShowLaizi = RightPositionLaizi.transform.position;
                    tempRelationData.PositionOnTableShow = RightPositionOnTableShow.transform.position;
                    tempRelationData.PositionOnTableNoShow = RightPositionOnTableNoShow.transform.position;
                    tempRelationData.PlayePosition = RightPlayePosition.transform.position;
                    tempRelationData.PositionOnHandShow = RightPositionOnHandShow.transform.position;
                    tempRelationData.RotationPostion = new Vector3(90.0f, -90.0f, 0.0f);
                    tempRelationData.RotationBackPostion = new Vector3(-90, 90, 0);
                    tempRelationData.Direction = new Vector3(0.0f, 0.0f, -1.0f);
                    break;
                case CharacterType.relative_FrontPostion:
                    tempRelationData.PositionShowLaizi = FrontPositionLaizi.transform.position;
                    tempRelationData.PositionOnTableShow = FrontPositionOnTableShow.transform.position;
                    tempRelationData.PositionOnTableNoShow = FrontPositionOnTableNoShow.transform.position;
                    tempRelationData.PlayePosition = FrontPlayePosition.transform.position;
                    tempRelationData.PositionOnHandShow = FrontPositionOnHandShow.transform.position;
                    tempRelationData.RotationPostion = new Vector3(90.0f, 180f, 0.0f);
                    tempRelationData.RotationBackPostion = new Vector3(-90, 0, 0);
                    tempRelationData.Direction = new Vector3(1.0f, 0.0f, 0.0f);
                    break;
            }
         //   GameObject laizi = (GameObject)Instantiate(Resources.Load("Prefabs/Laizi"));
            tempRelationData.Init();
            RelationDataArray.Add(tempRelationData);
        }
       // cameraAnimation.init(CameraObject, 90, CameraRotationPosition.transform.position);
        //TableObject.transform.Rotate(new Vector3(0, 1, 0), 90);
        //CameraObject.transform.RotateAround(CameraRotationPosition.transform.position, new Vector3(0, 1, 0), 90);
        //cameraAnimation.BeginAnim();
        //TableController.Instance.CurrentPlayrInfo.DataList[0].playerOrderIndex;

        //  test codeing begin
        //List<PlayerData> PlayerDataList = new List<PlayerData>();
        //PlayerData Player = new PlayerData(0);
        //Player.playerOrderIndex = 1;
        //Player.playerType = CharacterType.Player;
        //PlayerDataList.Add(Player);
        //Player.HandCardList[0] = (int)mjCards.tiao_1;
        //Player.HandCardList[1] = (int)mjCards.tiao_1;
        //Player.HandCardList[2] = (int)mjCards.tiao_2;
        //Player.HandCardList[3] = (int)mjCards.tiao_3;
        //Player.HandCardList[4] = (int)mjCards.wan_2;
        //Player.HandCardList[5] = (int)mjCards.wan_3;
        //Player.HandCardList[6] = (int)mjCards.wan_4;
        //Player.HandCardList[7] = (int)mjCards.wan_2;
        //Player.HandCardList[8] = (int)mjCards.tong_6;
        //Player.HandCardList[9] = (int)mjCards.tong_7;
        //Player.HandCardList[10] = (int)mjCards.tong_7;
        //Player.HandCardList[11] = (int)mjCards.tong_8;
        //Player.HandCardList[12] = (int)mjCards.tong_8;
        //PlayerData Player1 = new PlayerData(1);
        //Player1.playerType = CharacterType.ComputerTwo;
        //Player1.playerOrderIndex = 2;
        //PlayerDataList.Add(Player1);
        //PlayerData Player2 = new PlayerData(2);
        //Player2.playerOrderIndex = 3;
        //Player2.playerType = CharacterType.ComputerTre;
        //PlayerDataList.Add(Player2);
        //PlayerData Player3 = new PlayerData(3);
        //Player3.playerOrderIndex = 4;
        //Player3.playerType = CharacterType.ComputerFour;
        //PlayerDataList.Add(Player3);
        //OnDealCard(PlayerDataList);
        //    test codeing end
        _tableNoShowCardList = new List<GameObject>();
        if (TableController.Instance.PlayrInfo.selfSitDown)
        {
            ReconnectionSetSelfSit();
        }
    }

    private void OnPutOutCardCanTing(List<Int64> list)
    {
        List<CardObject> cardList= RelationDataArray[(int)CharacterType.relative_orignal].CardObjects;
        Int64 id;
        CardObject obj;
        GameObject gameObj;
        Int64 temp;
        for (int i = 0; i < list.Count; i++)
        {
            id = list[i];
            for (int j = 0; j < cardList.Count; j++)
            {
                obj = cardList[j];
                gameObj = obj.card;
                temp = (Int64)obj.cardType;
                if (temp == id)
                {
                    gameObj.transform.FindChild("back").gameObject.GetComponent<Renderer>().material.color = Color.red;
                  //  gameObj.transform.FindChild("back").gameObject.GetComponent<Renderer>().material.shader = Resources.Load("ShaderFile/biankuan", typeof(Shader)) as Shader;
                }        
            }
        }
    }


    /// <summary>
    /// 断线重连之胡牌状态
    /// </summary>
    /// <param name="list"></param>
    private void OnReconnectionByIsHu(List<PlayerData> list)
    {
        RelationData relationDataTemp;
        OnReconnection(list);
        for (int i = 0; i < RelationDataArray.Count; i++)
        {
            relationDataTemp = RelationDataArray[i];
            FallDownCard(relationDataTemp);
        }
    }

    private void FallDownCard(RelationData data)
    {
        CardObject cardObject;
        GameObject tempcard;
        List<CardObject> list = data.CardObjects;
        CharacterType relativePostionTemp = data.playerSeatIndex;
        Vector3 position;
        for (int i = 0; i < list.Count; i++)
        {
            cardObject = list[i];
            tempcard = cardObject.card;
            position = tempcard.transform.position;
            tempcard.transform.rotation = Quaternion.identity;
            tempcard.transform.Rotate(data.RotationPostion.x, data.RotationPostion.y, data.RotationPostion.z);

            if (relativePostionTemp == CharacterType.relative_RightPostion)
            {
                position.x += 0.23f;
            }
            if (relativePostionTemp == CharacterType.relative_FrontPostion)
            {
                position.z += 0.32f;
            }
            if (relativePostionTemp == CharacterType.relative_LeftPostion)
            {
                position.x -= 0.23f;
            }
            tempcard.transform.position = position;
        }
    }

    /// <summary>
    /// 断线重连
    /// </summary>
    private void OnReconnection(List<PlayerData> list)
    {
        RelationData seatData;
        _playerInfoList = list;
        cardPoolManager.SetLai((int)TableController.Instance.laizi);
        CharacterType bankerSite = GetRelativeEnum((int)TableController.Instance.selfOrderIndex, (int)TableController.Instance.bankerID);
        _tableNOShowCardData.totalCardCount = (int)TableController.Instance.totleCardCountList[(PeopleNum)TableController.Instance.creatRoomInfo.playerNum];
        _tableNOShowCardData.Init((int)TableController.Instance.surplusCardCount, (Int64)TableController.Instance.touziInfo, bankerSite,TableController.Instance.creatRoomInfo.playerNum);
        CreateCardOnTableNoShowByData();
        foreach (PlayerData item in list)
        {
            seatData = RelationDataArray[(int)item.playerType];
            seatData.playerOrderIndex = (int)item.playerOrderIndex;
            ShowHandCardForReconnectionAndHupai(item,true);
            ShowPengAndGangCardForReconnection(item);
            ShowPlayOutCardForReconnection(item);
            ShowPlayOutLaziCardForReconnection(item);
        }

        //调整公共牌的坐标,直接升起来
        foreach (RelationData item in RelationDataArray)
        {
            item.TableNoshowContainer.transform.Translate(Vector3.up * 0.51f);
        }
        SetReconnectionCurrentPutOutCard();
        //   cameraAnimation.rotateOver = true;

        ///断线重连后 调整东南西北方位的角度
       // ReconnectionSetSelfSit();
    }

    private void ReconnectionSetSelfSit()
    {
        float endAngle = (TableController.Instance.selfOrderIndex - 1) * 90;
        if (TableController.Instance.creatRoomInfo.playerNum == (int)PeopleNum.TwoPeople && TableController.Instance.selfOrderIndex != 1)
        {
            endAngle = 180;
        }
        cameraAnimation.directionObj.transform.Rotate(0, endAngle, 0);
        cameraAnimation.isBegin = true;
    }

    /// <summary>
    /// 停在胡牌节点 isReconnection（区分断线重连和正常胡牌）
    /// </summary>
    /// <param name="data"></param>
    /// <param name="isReconnection"></param>
    private void ShowHandCardForReconnectionAndHupai(PlayerData data,bool isReconnection)
    {
        int halfCount = (data.HandCardList.Count - 1) / 2+1;
        CharacterType relativeenum = data.playerType;
        mjCards cardID;
        int count;
        for (int i = 0; i < data.HandCardList.Count; i++)
        {
            cardID = (mjCards)data.HandCardList[i];
            if (data.playerType == CharacterType.relative_orignal)
            {
                if (isReconnection)
                {
                    CreatCardForSelf((mjCards)data.HandCardList[i], halfCount, i);
                }              
            }
            else
            {
                // count  = RelationDataArray[(int)relativeenum].CardObjects.Count + 1; 
                if (data.isHupai && i==0)
                {
                    count = 14;
                }
                else
                {
                    count = 13 - i;
                }
               
                if (relativeenum == CharacterType.relative_RightPostion)
                {
                    CreateHandCard(relativeenum, new Vector3(0.0f, 0.0f, 1.0f), RightPosition.transform.position, data, count, cardID);
                }
                else if (relativeenum == CharacterType.relative_FrontPostion)
                {
                    CreateHandCard(relativeenum, new Vector3(-1.0f, 0.0f, 0.0f), FrontPosition.transform.position, data, count, cardID);
                }
                else if (relativeenum == CharacterType.relative_LeftPostion)
                {
                    CreateHandCard(relativeenum, new Vector3(0.0f, 0.0f, -1.0f), LeftPosition.transform.position, data, count, cardID);
                }
            }
        }       
    }

    private void ShowPengAndGangCardForReconnection(PlayerData data)
    {
        GameObject gameObjectTemp;
        CardData cardata;
        RelationData relationDataTemp = RelationDataArray[(int)data.playerType];
        for (int i = 0; i < data.RevealCardList.Count; i++)
        {
            gameObjectTemp = cardPoolManager.GetGameObjectByType((mjCards)data.RevealCardList[i]);
            cardata = gameObjectTemp.GetComponent<CardData>();
            cardata.CardType = (mjCards)data.RevealCardList[i];
            relationDataTemp.HandShowCardObjects.Add(gameObjectTemp);
        }

        CardRules.SortCardsByGameObject(relationDataTemp.HandShowCardObjects);
        for (int i = 0; i < relationDataTemp.HandShowCardObjects.Count; i++)
        {
            SortHandShowCard(relationDataTemp.HandShowCardObjects[i], relationDataTemp, i);
        }
    }

    /// <summary>
    /// 断线重连,打出去的牌
    /// </summary>
    /// <param name="data"></param>
    private void ShowPlayOutCardForReconnection(PlayerData data)
    {
        CardData cardData;
        for (int i = 0; i < data.playOutCardList.Count; i++)
        {
            GameObject gameObjectTemp = cardPoolManager.GetGameObjectByType((mjCards)data.playOutCardList[i]);
            CharacterType relativePostionTemp = data.playerType;
            RelationData relationDataTemp = RelationDataArray[(int)relativePostionTemp];
            gameObjectTemp.transform.rotation = Quaternion.identity;
            cardData = gameObjectTemp.GetComponent<CardData>();
            cardData.PositionOnTableShow = relationDataTemp.PositionOnTableShow - relationDataTemp.GetShowOnTablePosition(offsetx, offsety);
            relationDataTemp.OnTableShowObjects.Add(gameObjectTemp);
            gameObjectTemp.transform.position = cardData.PositionOnTableShow;
            gameObjectTemp.transform.Rotate(relationDataTemp.RotationPostion.x, relationDataTemp.RotationPostion.y, relationDataTemp.RotationPostion.z);
        }     
    }

    private void ShowPlayOutLaziCardForReconnection(PlayerData data)
    {
        CardData cardData;
        for (int i = 0; i < data.playOutlaiziList.Count; i++)
        {
            GameObject gameObjectTemp = cardPoolManager.GetGameObjectByType((mjCards)data.playOutlaiziList[i]);
            CharacterType relativePostionTemp = data.playerType;
            RelationData relationDataTemp = RelationDataArray[(int)relativePostionTemp];
            gameObjectTemp.transform.rotation = Quaternion.identity;
            cardData = gameObjectTemp.GetComponent<CardData>();
            relationDataTemp.LaiziObjects.Add(gameObjectTemp);
            cardData.PositionOnTableShow = relationDataTemp.PositionShowLaizi - relationDataTemp.GetShowLaiziPosition(offsetx, offsety);
            gameObjectTemp.transform.position = cardData.PositionOnTableShow;
            gameObjectTemp.transform.Rotate(relationDataTemp.RotationPostion.x, relationDataTemp.RotationPostion.y, relationDataTemp.RotationPostion.z);
        }
    }

    private void SetReconnectionCurrentPutOutCard()
    {
        CardData cardData;
        if (TableController.Instance.currentCardID!=0)
        {
            //  currentCardID = data.processer_card;
            CharacterType bankerSite = GetRelativeEnum((int)TableController.Instance.selfOrderIndex, (int)TableController.Instance.playerCardPlayerSeat);
            RelationData relationDataTemp = RelationDataArray[(int)bankerSite];
            GameObject obj;
            for (int i = 0; i < relationDataTemp.OnTableShowObjects.Count; i++)
            {
                obj = relationDataTemp.OnTableShowObjects[i];
                cardData = obj.GetComponent<CardData>();
                if ((int)cardData.CardType == (int)TableController.Instance.currentCardID)
                {
                    _currentPutOutCard = obj;
                }
            }
        }     
    }

    /// <summary>
    /// 获得自己的手牌不显示，先升起桌子
    /// </summary>
    /// <param name="list"></param>
    private void BeignUpMjMovie(List<PlayerData> list)
    {
        _playerInfoList = list;
        upMovieFlag = true;
        InitTableCardNoShow();
        ////0002619934 001049385
        upMaPai.transform.localPosition = new Vector3(-0.0002619934f, -0.173f, -0.001049388f);
        InitTouzi();
        delayedFlag = true;
        // StartCoroutine("PlayerMovieTimeDelaye");
    }

    //延时函数，播放开始动画
    //private IEnumerator PlayerMovieTimeDelaye()
    //{
    //    yield return new WaitForSeconds(0.6f); 
    //    delayedFlag = true;
    //}

    /// <summary>
    /// 码牌
    /// </summary>
    private void InitTableCardNoShow()
    {
        CharacterType bankerSite = GetRelativeEnum((int)TableController.Instance.selfOrderIndex, (int)TableController.Instance.bankerID);
        _tableNOShowCardData.totalCardCount = (int)TableController.Instance.totleCardCountList[(PeopleNum)TableController.Instance.creatRoomInfo.playerNum];
        _tableNOShowCardData.Init(_tableNOShowCardData.totalCardCount, TableController.Instance.touziInfo, bankerSite, TableController.Instance.creatRoomInfo.playerNum);
        CreateCardOnTableNoShowByData();
        cardPoolManager.SetLai((int)TableController.Instance.laizi);
    }

    private void CreateCardOnTableNoShowByData()
    {
        BoxCollider box;
        CardObject CardInstance;
        GameObject cardObject;
        RelationData relationDataTemp;
        Vector3 startPositionTemp;
        Vector3 Rotation;
        Vector3 Direction;
        TableNoShowCardObject cardObj;
        int postionIndex;
        for (int i = 0; i < _tableNOShowCardData.cardObjList.Count; i++)
        {
            CardInstance = new CardObject();
            cardObj = _tableNOShowCardData.cardObjList[i];
            postionIndex = cardObj.index;
            relationDataTemp = RelationDataArray[(int)cardObj.relativePostion];
            startPositionTemp = relationDataTemp.PositionOnTableNoShow;
            Rotation = Quaternion.Euler(90.0f, 0f, 0f) * relationDataTemp.RotationPostion;//倒下来
            Direction = relationDataTemp.Direction;
            cardObject = cardPoolManager.GetGameObjectByType(mjCards.Nodefine);     
            CardInstance.InstanceObject(cardObject, mjCards.Nodefine);
            box = cardObject.GetComponent("BoxCollider") as BoxCollider;
            offsetx = box.bounds.size.x;
            offsety = box.bounds.size.y / 2;
            if (postionIndex % 2 == 0)
            {                
                cardObject.transform.position = startPositionTemp + new Vector3(0.0f, 1.0f, 0.0f) * offsety + Direction * offsetx * (postionIndex / 2);
            }
            else
            {
                cardObject.transform.position = startPositionTemp + Direction * offsetx * (postionIndex / 2);
            }
            cardObject.transform.Rotate(Rotation.x + 180, Rotation.y, Rotation.z);
            cardObject.transform.parent = RelationDataArray[(int)cardObj.relativePostion].TableNoshowContainer.transform;
            _tableNoShowCardList.Add(cardObject);
        }
        _tableNoShowCardList.Reverse();
    }

    /// <summary>
    /// 初始化骰子
    /// </summary>
    private void InitTouzi()
    {       
        Touzi0.SetActive(true);
        Touzi1.SetActive(true);
        Touzi0.transform.rotation = Quaternion.identity;
        Touzi1.transform.rotation = Quaternion.identity;
    }

    /// <summary>
    /// 桌面牌堆减少一张牌
    /// </summary>
    private void RemoveTableNOShowCard(bool flag =false)
    {
        int index =0;
        if (_tableNoShowCardList.Count != 1)
        {
            if (TableController.Instance.currentCardID == TableController.Instance.laizi|| _isGang || flag)
            {
                _isGang = false;
                if (_RemoveTableCardparam == 0)
                {
                    index = 1;
                    _RemoveTableCardparam = 1;
                }
                else
                {
                    _RemoveTableCardparam = 0;
                }
            }
            else
            {             
                index = _tableNoShowCardList.Count - 1;
            }
        }         
        GameObject obj = _tableNoShowCardList[index];
        _tableNoShowCardList.RemoveAt(index);
        cardPoolManager.ReleaseByGameObject(mjCards.Nodefine, obj);
    }

    /// <summary>
    /// 创建其他玩家的手牌
    /// </summary>
    /// <param name="RealtivePositionParm"></param>
    /// <param name="Direction"></param>
    /// <param name="startPosition"></param>
    /// <param name="data"></param>
    /// <param name="index"></param>
    private void CreateHandCard(CharacterType RealtivePositionParm, Vector3 Direction, Vector3 startPosition, PlayerData data, int index, mjCards mjID = mjCards.Nodefine)
    {
        float offset;
        GameObject cardObject = cardPoolManager.GetGameObjectByType(mjID);
        cardObject.name = RealtivePositionParm.ToString();
        CardObject CardInstance = new CardObject();
        CardInstance.InstanceObject(cardObject, mjID);
        RelationDataArray[(int)RealtivePositionParm].CardObjects.Add(CardInstance);
        cardObject.transform.parent = RelationDataArray[(int)RealtivePositionParm].Container.transform;
        cardObject.transform.forward = Vector3.Cross(Direction, new Vector3(0.0f, 1.0f, 0.0f));
        BoxCollider box = cardObject.GetComponent("BoxCollider") as BoxCollider;
        if (RealtivePositionParm == CharacterType.relative_FrontPostion)
        {
            offset = box.bounds.size.x;
        }
        else
        {
            offset = box.bounds.size.z;
        }   
        Vector3 moveVector = new Vector3((float)(startPosition.x), (float)(startPosition.y), (float)(startPosition.z)) - Direction* (offset);
        cardObject.transform.position = Direction * (float)(index * offset) + moveVector;        
    }

    /// <summary>
    ///flag表示别人出牌后我是否能够碰或者杠
    /// </summary>
    /// <param name="parmPlayerData"></param>
    /// <param name="flag"></param>
    private void PlayerOutCard(PlayerData parmPlayerData,int flag)
    {
        Vector3 cardPostion = new Vector3(0,0,0);
        CardData cardData;
        GameObject gameObjectTemp = cardPoolManager.GetGameObjectByType((mjCards)TableController.Instance.currentCardID);
        CharacterType relativePostionTemp = parmPlayerData.playerType;
        RelationData relationDataTemp = RelationDataArray[(int)relativePostionTemp];
        CardObject cardObject;
        gameObjectTemp.transform.rotation = Quaternion.identity;
        cardData = gameObjectTemp.GetComponent<CardData>();
        cardData.CardType = (mjCards)TableController.Instance.currentCardID;
        _currentPutOutCard = gameObjectTemp;
        _currentRelative = parmPlayerData.playerType;
        //int index;
        if (TableController.Instance.currentCardID == TableController.Instance.laizi)
        {
            cardPostion = relationDataTemp.PositionOnHandShow;
            relationDataTemp.LaiziObjects.Add(gameObjectTemp);
            cardData.PositionOnTableShow = relationDataTemp.PositionShowLaizi - relationDataTemp.GetShowLaiziPosition(offsetx, offsety);
            if (relativePostionTemp != CharacterType.relative_orignal)
            {
                cardObject = relationDataTemp.CardObjects[0];
                relationDataTemp.CardObjects.RemoveAt(0);
                cardPoolManager.ReleaseByGameObject(cardObject.cardType, cardObject.card);
                cardObject.card.transform.parent = CameraObject.transform;
            }
        }
        else
        {
            if (relativePostionTemp == CharacterType.relative_orignal)
            {
                cardPostion = relationDataTemp.PositionOnHandShow;  
            }
            else
            {
                try
                {
                    // index = UnityEngine.Random.Range(0, relationDataTemp.CardObjects.Count - 1);
                    if (relationDataTemp.CardObjects.Count>0)
                    {
                        cardObject = relationDataTemp.CardObjects[0];
                        relationDataTemp.CardObjects.RemoveAt(0);
                        cardObject.card.transform.parent = CameraObject.transform;
                        cardPostion = cardObject.card.transform.position;
                        cardPoolManager.ReleaseByGameObject(cardObject.cardType, cardObject.card);
                    }                 
                }
                catch (Exception m)
                {
                    //EventDispatcher.GetInstance().UIFrameWorkEventManager.TriggerEvent<string>(EventId.UIFrameWork_Bug, relationDataTemp.CardObjects.Count+"{addcard}" + m.Message);
                }    
            }
            cardData.interactiveFlag = flag;
            cardData.PositionOnTableShow = relationDataTemp.PositionOnTableShow - relationDataTemp.GetShowOnTablePosition(offsetx, offsety);
            relationDataTemp.OnTableShowObjects.Add(gameObjectTemp);       
        }
        gameObjectTemp.transform.position = cardPostion;      
        cardData.PlayePosition = relationDataTemp.PlayePosition;
        cardData.FinishRotation = relationDataTemp.RotationPostion;
        cardData.sitType = relativePostionTemp;
        cardData.PlayCard();
        //cardData.PlayCardFinsh();
        //cardData.PlayCardOver();

        cardData.OwnerShip = (int)parmPlayerData.playerOrderIndex;
        cardData.JumpMovieOver();

        gameObjectTemp.transform.FindChild("back").gameObject.GetComponent<Renderer>().material.color = TableController.Instance.mjBackColor;//打出去的牌去掉因为听牌检测加上去的颜色
        NeatlyPutHandleCard(relationDataTemp.CardObjects, relativePostionTemp,true);
        mjCards mjName = (mjCards)TableController.Instance.currentCardID;
        if (TableController.Instance.currentCardID == TableController.Instance.laizi)
        {
            PlayCardSound(parmPlayerData.sex, "gang");
        }
        else
        {
            PlayCardSound(parmPlayerData.sex, mjName.ToString());
        }     
    }

    private void PlayCardSound(Int64 sex,string name)
    {
        string res = name;
        //  mjCards mjName = (mjCards)TableController.Instance.currentCardID;
        //  res = mjName.ToString();
        if (sex == 2)
        {
            res = "g_" + name.ToString();
        }
        SoundManager.Instance.Play(ESoundLayer.Effect, res);
    }

    /// <summary>
    /// 起牌 动画
    /// </summary>
    /// <param name="data"></param>
    private void OnAddCard(PlayerData data)
    {
        if (!upMovieFlag)
        {
            int halfCount = -8;
            CharacterType relativeenum = data.playerType;
            if (data.playerType == CharacterType.relative_orignal)
            {
                CreatCardForSelf((mjCards)TableController.Instance.selfGetCardID, halfCount, -1.2f);
            }
            else
            {
                int count = 14;
                if (relativeenum == CharacterType.relative_RightPostion)
                {
                    CreateHandCard(relativeenum, new Vector3(0.0f, 0.0f, 1.0f), RightPosition.transform.position, data, count);
                }
                else if (relativeenum == CharacterType.relative_FrontPostion)
                {
                    CreateHandCard(relativeenum, new Vector3(-1.0f, 0.0f, 0.0f), FrontPosition.transform.position, data, count);
                }
                else if (relativeenum == CharacterType.relative_LeftPostion)
                {
                    CreateHandCard(relativeenum, new Vector3(0.0f, 0.0f, -1.0f), LeftPosition.transform.position, data, count);
                }
            }
            RemoveTableNOShowCard();
           if (data.playerType == CharacterType.relative_orignal)
            {
               // StartCoroutine("robet");
            }
         
        }
    }

    private IEnumerator robet()
    {
        yield return new WaitForSeconds(0.2f);
        RelationData relationDataTemp = RelationDataArray[(int)CharacterType.relative_orignal];
       int index = UnityEngine.Random.Range(0, relationDataTemp.CardObjects.Count - 1);
        CardObject obj = relationDataTemp.CardObjects[index];
        relationDataTemp.CardObjects.Remove(obj);
        SocketClient.Instance.PlayCard((Int64)obj.cardType);
        cardPoolManager.ReleaseByGameObject(obj.cardType, obj.card);
        TableController.Instance.selfGameState = SelfState.Not_playCard;
    }

    private void CreatCardForSelf(mjCards mjId,int halfCount,float index)
    {
        GameObject cardObject = cardPoolManager.GetGameObjectByType(mjId);
        CardObject CardInstance = new CardObject();
        CardInstance.InstanceObject(cardObject, mjId);
        RelationDataArray[(int)CharacterType.relative_orignal].CardObjects.Add(CardInstance);//自己永远在relative_orignal
        BoxCollider box = CardInstance.card.GetComponent("BoxCollider") as BoxCollider;
        float offset = box.bounds.size.x;

        Vector3 moveVector = new Vector3((float)(CameraDirection.x * DistanceFromEyeZ - halfCount * offset), (float)(CameraDirection.y * DistanceFromEyeZ), (float)(CameraDirection.z * DistanceFromEyeZ));
        moveVector.y += CameraDirection.y * DistanceFromEyeY;
        CardInstance.card.transform.position = Cameraposition + moveVector;
        CardInstance.card.transform.position -= CameraObject.transform.up * DistanceFromEyeY;
        CardInstance.card.transform.position += new Vector3((float)(CameraDirection.x + (float)(index * (offset))), 0, 0);
        CardInstance.card.transform.LookAt(CameraObject.transform);
        CardInstance.card.transform.forward = CameraDirection;
    }

    /// <summary>
    ///  cardID 碰的牌，playTypeIndex 座位编号（有玩家碰牌后 删除手上碰的牌，给碰牌数组里加数据）
    /// </summary>
    /// <param name="cardID"></param>
    /// <param name="playTypeIndex"></param>
    private void OnPlayPengCard(Int64 cardID, Int64 playerOrderIndex)
    {
        PlayerData data = TableController.Instance.GetPlayerDatabyIndex((int)playerOrderIndex);
        PlayCardSound(data.sex,"peng");
        SetPengAndGangCard(cardID, data.playerType, 2);
    }

    private List<Int64> anGangMjArr;
    private List<Int64> otherGangMjArr;
    private int setGangcardFlag;
    /// <summary>
    /// flag 杠的类型，4：自笑，1：回头笑，3：点杠
    /// </summary>(0：自笑，1：回头笑，2：点杠
    /// 
    /// <param name="cardID"></param>
    /// <param name="playIndex"></param>
    /// <param name="flag"></param>
    private void OnPlaygGangCard(Int64 cardID, Int64 playerOrder, Int64 flag)
    {
        Hashtable table = TableController.Instance.gangPaiState;
        PlayerData data = TableController.Instance.GetPlayerDatabyIndex((int)playerOrder);
        PlayCardSound(data.sex,"gang");
        if (flag == 4)
        {
            anGangMjArr.Add(cardID);
            table[cardID.ToString()] = (Int64)0;
        }
        else
        {
            otherGangMjArr.Add(cardID);
            table[cardID.ToString()] = (Int64)2;
        }
        SetPengAndGangCard(cardID, data.playerType, flag);
        if (cardID != TableController.Instance.laizipi)
        {
            _isGang = true;
        }    
    }
    
    private void SetPengAndGangCard(Int64 cardID,CharacterType playIndex,Int64 flag)
    {
        RelationData relationDataTemp = RelationDataArray[(int)playIndex];//碰或者杠牌的玩家数据
        RelationData PlayOutCardData = RelationDataArray[(int)_currentRelative];//出牌让人碰或者杠的玩家数据（如果玩家是自笑或者暗杠，则这个数据不处理）
        GameObject gameObjectTemp;
        CardObject cardObject;
        CardData cardata;
        int count = relationDataTemp.CardObjects.Count;
        int maxCount = (int)flag;
        if (flag == 3 || flag== 2)//3 是点杠，2是碰
        {
            relationDataTemp.HandShowCardObjects.Add(_currentPutOutCard);
            PlayOutCardData.OnTableShowObjects.Remove(_currentPutOutCard);
        }
        if (cardID == TableController.Instance.laizipi)
        {
            maxCount--;
        }
        for (int i = count - 1; i >= 0; i--)
        {
            if (CharacterType.relative_orignal == playIndex)
            {
                cardObject = relationDataTemp.CardObjects[i];
                if ((Int64)cardObject.cardType == cardID)
                {
                    gameObjectTemp = cardObject.card;
                    cardata = gameObjectTemp.GetComponent<CardData>();
                    cardata.CardType = cardObject.cardType;
                    relationDataTemp.CardObjects.RemoveAt(i);
                    relationDataTemp.HandShowCardObjects.Add(gameObjectTemp);
                    maxCount--;
                }
            }
            else
            {
                cardObject = relationDataTemp.CardObjects[0];
                relationDataTemp.CardObjects.RemoveAt(0);
                gameObjectTemp = cardPoolManager.GetGameObjectByType((mjCards)cardID);
                cardata = gameObjectTemp.GetComponent<CardData>();
                cardata.CardType = (mjCards)cardID;
                relationDataTemp.HandShowCardObjects.Add(gameObjectTemp);
                cardPoolManager.ReleaseByGameObject(mjCards.Nodefine, cardObject.card);
                maxCount--;
            }
            if (maxCount <= 0)
            {
                break;
            }
        }
        CardRules.SortCardsByGameObject(relationDataTemp.HandShowCardObjects);
        for (int i = 0; i < relationDataTemp.HandShowCardObjects.Count; i++)
        {
            SortHandShowCard(relationDataTemp.HandShowCardObjects[i], relationDataTemp, i);
        }
        NeatlyPutHandleCard(relationDataTemp.CardObjects, playIndex,false);

        if (flag == 2 || cardID == TableController.Instance.laizipi)
        {
            if (playIndex == CharacterType.relative_orignal)
            {
                TableController.Instance.PlayrInfo.CheckSelfPutOutCardCantingTips();
            }
        }
    }

    private void SortHandShowCard(GameObject gameObjectTemp, RelationData relationDataTemp, int index)
    {
        Hashtable table = TableController.Instance.gangPaiState;
        gameObjectTemp.transform.rotation = Quaternion.identity;
        CardData cardata = gameObjectTemp.GetComponent<CardData>();
        Int64 cardID = (Int64)cardata.CardType;
        BoxCollider box = gameObjectTemp.GetComponent("BoxCollider") as BoxCollider;
        float offset = box.bounds.size.x;
        

        bool flag =false;
        if (table.ContainsKey(cardID.ToString()))
        {
            Int64 sxss = (Int64)table[cardID.ToString()];
            if (sxss == 0)
            {
                flag = true;
            }
        }
        if (flag)
        {
            setGangcardFlag++;
            if (setGangcardFlag>1)
            {
                gameObjectTemp.transform.Rotate(relationDataTemp.RotationBackPostion.x, relationDataTemp.RotationBackPostion.y, relationDataTemp.RotationBackPostion.z);
            }
            else
            {
                gameObjectTemp.transform.Rotate(relationDataTemp.RotationPostion.x, relationDataTemp.RotationPostion.y, relationDataTemp.RotationPostion.z);
            }
            if (setGangcardFlag>=4)
            {
                setGangcardFlag = 0;
            }
            if (cardID == TableController.Instance.laizipi&& setGangcardFlag >= 3)
            {
                setGangcardFlag = 0;
            }
        }
        else
        {
            gameObjectTemp.transform.Rotate(relationDataTemp.RotationPostion.x, relationDataTemp.RotationPostion.y, relationDataTemp.RotationPostion.z);
        }

        if (otherGangMjArr.Contains(cardID))
        {
           
        }
        gameObjectTemp.transform.position = relationDataTemp.PositionOnHandShow-relationDataTemp.Direction * (float)(index * offset);
        gameObjectTemp.transform.FindChild("back").gameObject.GetComponent<Renderer>().material.color = TableController.Instance.mjBackColor;
    }

    /// <summary>
    /// 胡牌
    /// </summary>
    /// <param name="list"></param>
    private void OnHuPai(List<PlayerData> list)
    {
        RelationData relationDataTemp;
        CardObject cardObject;
        for (int i = 0; i < RelationDataArray.Count; i++)
        {
            relationDataTemp = RelationDataArray[i];
            if (relationDataTemp.playerSeatIndex != CharacterType.relative_orignal)
            {
                for (int j = relationDataTemp.CardObjects.Count-1; j >=0 ; j--)
                {
                    cardObject = relationDataTemp.CardObjects[j];
                    relationDataTemp.CardObjects.RemoveAt(j);
                    cardPoolManager.ReleaseByGameObject(cardObject.cardType, cardObject.card);
                    cardObject.card.transform.parent = CameraObject.transform;
                }
            }       
        }

        foreach (PlayerData item in list)
        {
            ShowHandCardForReconnectionAndHupai(item,false);
            if (item.isHupai)
            {
                PlayCardSound(item.sex,"hu_zimo");
            }
        }

        for (int i = 0; i < RelationDataArray.Count; i++)
        {
            relationDataTemp = RelationDataArray[i];
            FallDownCard(relationDataTemp);
        }
    }

    private void OnBeginNextGame()
    {
        ResetTable();
    }

    private void OnSetMjBack(string res)
    {
        cardPoolManager.SetMjBackImg(res);
    }

    private IEnumerator TimeDelaye()
    {
        yield return new WaitForSeconds(2);
        ResetTable();
    }

    private void OnShowLastMj(Int64 mj)
    {
        RemoveTableNOShowCard(true);
    }

    /// <summary>
    /// 手牌有变化后整理手牌
    /// </summary>
    private void NeatlyPutHandleCard(List<CardObject> list, CharacterType relativeenum,bool isPutOutCard)
    {
        Vector3 Direction = new Vector3(0,0,0);
        Vector3 startPosition = new Vector3(0, 0, 0);
        CardObject CardInstance;
        int offsetCount;
        if (relativeenum == CharacterType.relative_RightPostion)
        {
            Direction = new Vector3(0.0f, 0.0f, 1.0f);
            startPosition = RightPosition.transform.position;
        }
        else if (relativeenum == CharacterType.relative_FrontPostion)
        {
            Direction = new Vector3(-1.0f, 0.0f, 0.0f);
            startPosition = FrontPosition.transform.position;
        }
        else if (relativeenum == CharacterType.relative_LeftPostion)
        {
            Direction = new Vector3(0.0f, 0.0f, -1.0f);
            startPosition = LeftPosition.transform.position;
        }
        else
        {
            CardRules.SortCards(list, TableController.Instance.laizi);
        }
        
        for (int i = 0; i < list.Count; i++)
        {
            CardInstance = list[i];
            if (relativeenum == CharacterType.relative_orignal)
            {
                Vector3 moveVector = new Vector3((float)(CameraDirection.x * DistanceFromEyeZ + 5 * offsetx), (float)(CameraDirection.y * DistanceFromEyeZ), (float)(CameraDirection.z * DistanceFromEyeZ));
                moveVector.y += CameraDirection.y * DistanceFromEyeY;
                CardInstance.card.transform.position = Cameraposition + moveVector;
                CardInstance.card.transform.position -= CameraObject.transform.up * DistanceFromEyeY;
                CardInstance.card.transform.position += new Vector3((float)(CameraDirection.x - (float)(i * (offsetx))), 0, 0);
                CardInstance.card.transform.LookAt(CameraObject.transform);
                CardInstance.card.transform.forward = CameraDirection;
                CardInstance.card.transform.FindChild("back").gameObject.GetComponent<Renderer>().material.color = TableController.Instance.mjBackColor;                      
            }
            else
            {
                offsetCount = 13 - list.Count;
                SortPutOneCard(CardInstance, Direction, startPosition, offsetCount+i, relativeenum);
            }         
        }   
    }

    /// <summary>
    /// 整理别人的手牌
    /// </summary>
    /// <param name="CardInstance"></param>
    /// <param name="Direction"></param>
    /// <param name="startPosition"></param>
    /// <param name="index"></param>
    /// <param name="relativeenum"></param>
    private void SortPutOneCard(CardObject CardInstance, Vector3 Direction, Vector3 startPosition,int index, CharacterType relativeenum)
    {
        float halfCount = 1.0F;       
        GameObject cardObject;
        BoxCollider box;
        float offset;
        Vector3 moveVector;
        cardObject = CardInstance.card;
        cardObject.transform.forward = Vector3.Cross(Direction, new Vector3(0.0f, 1.0f, 0.0f));
        box = cardObject.GetComponent("BoxCollider") as BoxCollider;
        if (relativeenum == CharacterType.relative_FrontPostion)
        {
            offset = box.bounds.size.x;
        }
        else
        {
            offset = box.bounds.size.z;
        }
        moveVector = new Vector3((float)(startPosition.x), (float)(startPosition.y), (float)(startPosition.z)) - Direction * (halfCount * offset);
        cardObject.transform.position = Direction * (float)(index * offset) + moveVector;       
    }

    private void OnPlaySitDown(List<PlayerData> list,bool isSelf)
    {
        if (isSelf)
        {
            float beginAnger = (TableController.Instance.selfOrderIndex - 1) * 90;
            if (TableController.Instance.creatRoomInfo.playerNum == (int)PeopleNum.TwoPeople&& TableController.Instance.selfOrderIndex != 1)
            {
                beginAnger = 180;
            }
            cameraAnimation.init(CameraObject, beginAnger, CameraRotationPosition.transform.position);        
        }
    }

    /// <summary>
    /// 牌局开始，发手牌
    /// </summary>
    /// <param name="list"></param>
    private void OnDealCard(List<PlayerData> list)
    {
        RelationData seatData;
        _currentClickMj = null;
        PlayerData data;
        for (int i = 0; i < list.Count; i++)
        {
            data = list[i];
            seatData = RelationDataArray[(int)data.playerType];
            seatData.playerOrderIndex = (int)data.playerOrderIndex;
        }
        _dealPlayerIndex = 0;
        _dealNum = 0;
        StartCoroutine("DealCardTimeDelaye");
    }

    /// <summary>
    /// 翻赖子，模拟打出癞子皮
    /// </summary>
    private void  ShowlaiziForBegin()
    {
        CardData cardData;
        CharacterType relativePostionTemp = GetRelativeEnum((int)TableController.Instance.selfOrderIndex, (int)TableController.Instance.bankerID);
        GameObject gameObjectTemp = cardPoolManager.GetGameObjectByType((mjCards)TableController.Instance.laizipi);
        RelationData relationDataTemp = RelationDataArray[(int)relativePostionTemp];
      //  _currentPutOutCard = gameObjectTemp;
        gameObjectTemp.transform.rotation = Quaternion.identity;
        cardData = gameObjectTemp.GetComponent<CardData>();
        cardData.PositionOnTableShow = relationDataTemp.PositionOnTableShow - relationDataTemp.GetShowOnTablePosition(offsetx, offsety);
        relationDataTemp.OnTableShowObjects.Add(gameObjectTemp);
        gameObjectTemp.transform.position = cardData.PositionOnTableShow;
        gameObjectTemp.transform.Rotate(relationDataTemp.RotationPostion.x, relationDataTemp.RotationPostion.y, relationDataTemp.RotationPostion.z);
        RemoveTableNOShowCard();
        _RemoveTableCardparam = 0;
        EventDispatcher.GetInstance().UIFrameWorkEventManager.TriggerEvent(EventId.UIFrameWork_Show_Laizi);
    }

    //延时函数,发票延时
    private IEnumerator DealCardTimeDelaye()
    {
        yield return new WaitForSeconds(0.1f);
        DealCardFun();
    }

    /// <summary>
    /// 递归处理，每次发四张牌
    /// </summary>
    private void DealCardFun()
    {
        PlayerData data;
        int endNum ;
        try
        {
            if (_dealPlayerIndex >= TableController.Instance.creatRoomInfo.playerNum)
            {
                if (_dealNum == 12)
                {
                    ShowlaiziForBegin();
                    return;
                }
                _dealPlayerIndex = 0;
                _dealNum += 4;
            }
            endNum = _dealNum + 4;
            data = _playerInfoList[_dealPlayerIndex];
            CharacterType Playertype = data.playerType;
            int halfCount = (data.HandCardList.Count - 1) / 2+1;
            CharacterType relativeenum = data.playerType;
            if (endNum > 13)
            {
                endNum = data.HandCardList.Count;
            }
            for (int j = _dealNum; j < endNum; j++)
            {
                RemoveTableNOShowCard();
                if (Playertype == CharacterType.relative_orignal)
                {
                    if (j == 13)//如果是自己的庄
                    {
                        CreatCardForSelf((mjCards)data.HandCardList[j], -8, -1.2f);
                        TableController.Instance.PlayrInfo.CheckSelfPutOutCardCantingTips();
                        TableController.Instance.PlayrInfo.CheckSelfAddCardByFirst(TableController.Instance.selfGetCardID);
                      //  StartCoroutine("robet");
                    }
                    else
                    {
                        CreatCardForSelf((mjCards)data.HandCardList[j], halfCount, j);
                    }
                }
                else
                {
                    if (j == 13)
                    {
                        j = 14;
                    }
                    if (relativeenum == CharacterType.relative_RightPostion)
                    {
                        CreateHandCard(relativeenum, new Vector3(0.0f, 0.0f, 1.0f), RightPosition.transform.position, data, j);
                    }
                    else if (relativeenum == CharacterType.relative_FrontPostion)
                    {
                        CreateHandCard(relativeenum, new Vector3(-1.0f, 0.0f, 0.0f), FrontPosition.transform.position, data, j);
                    }
                    else if (relativeenum == CharacterType.relative_LeftPostion)
                    {
                        CreateHandCard(relativeenum, new Vector3(0.0f, 0.0f, -1.0f), LeftPosition.transform.position, data, j);
                    }
                }               
            }
            _dealPlayerIndex++;
            StartCoroutine("DealCardTimeDelaye");
        }
        catch (Exception e)
        {
            EventDispatcher.GetInstance().UIFrameWorkEventManager.TriggerEvent<string>(EventId.UIFrameWork_Bug, "DealCardFun");
        }     
    }

 //   private Vector3 lastClickPoint;
  //  private int loopCount;
    private double time1;
    private double time2;
    private double tempTime;
    private bool isDoubleClick;
    private GameObject hitObj;
    private bool tableUpOver;
    private AnimationState _currState;
    void Update()
    {
        //玩家坐下后旋转桌子动画,就只有坐下来的时候转一次
        if (cameraAnimation != null)
        {
            cameraAnimation.Update();
        }

        //升桌子 每次牌局开始都要升一次
        if (cameraAnimation!=null)
        {
            if (cameraAnimation.rotateOver && delayedFlag)
            {
                if (upMaPai.transform.localPosition.y < upNum)
                {
                    upMaPai.transform.Translate(Vector3.up * SpeedNum);
                    foreach (RelationData item in RelationDataArray)
                    {
                        item.TableNoshowContainer.transform.Translate(Vector3.up * SpeedNum * 2);
                    }
                }
                else
                {
                    //0002619934 001049385
                    upMaPai.transform.localPosition = new Vector3(-0.0002619934f, upNum, -0.001049388f);
                    // EventDispatcher.GetInstance().UIFrameWorkEventManager.TriggerEvent<string>(EventId.UIFrameWork_Bug, "{AAA}");               
                    Touzi0.SetActive(false);
                    Touzi1.SetActive(false);
                    delayedFlag = false;
                    _currState = anim["Take 001"];
                    anim.gameObject.SetActive(true);
                    anim.Play("Take 001");
                    tableUpOver = true;
                }
            }
        }
       
        if (tableUpOver)//麻将桌子升起后开始 播放骰子动画
        {
            if (_currState.normalizedTime >= 0.8f)//骰子动画播放结束后，开始发牌
            {
                anim.Stop("Take 001");
             //   Debug.Log(_currState.normalizedTime);
                anim.gameObject.SetActive(false);
                int nua = (int)TableController.Instance.touziInfo / 10 - 1;
                int nub = (int)TableController.Instance.touziInfo % 10 - 1;
                Touzi0.SetActive(true);
                Touzi1.SetActive(true);
                Touzi0.transform.Rotate(_diceRotateRadio[nua].x, _diceRotateRadio[nua].y, _diceRotateRadio[nua].z);
                Touzi1.transform.Rotate(_diceRotateRadio[nub].x, _diceRotateRadio[nub].y, _diceRotateRadio[nub].z);
                // isBeginGame = false;
                upMovieFlag = false;
                delayedFlag = false;
                tableUpOver = false;
                OnDealCard(_playerInfoList);
               // EventDispatcher.GetInstance().UIFrameWorkEventManager.TriggerEvent<string>(EventId.UIFrameWork_Bug, "{DDD}");
            }       
        }

        if (Input.GetMouseButtonUp(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.transform.gameObject == clickAssistPanel)
                {
                    if (_currentClickMj!=null)
                    {
                            RelationData relationDataTemp = RelationDataArray[(int)CharacterType.relative_orignal];
                            CardObject obj = GetCardObjByGameObj(relationDataTemp.CardObjects, _currentClickMj);
                            relationDataTemp.CardObjects.Remove(obj);
                            SocketClient.Instance.PlayCard((long)obj.cardType);
                            if (TableController.Instance.selfCanChenglaizi)
                            {
                                TableController.Instance.interPanelisShow = false;
                                TableController.Instance.selfCanChenglaizi = false;
                                EventDispatcher.GetInstance().UIFrameWorkEventManager.TriggerEvent(EventId.UIFrameWork_Cheng_laizi);
                            }
                            TableController.Instance.selfGameState = SelfState.Not_playCard;
                            cardPoolManager.ReleaseByGameObject(obj.cardType, obj.card);
                            _currentClickMj = null;
                            time1 = 0;
                            time2 = 0;
                            isDoubleClick = false;                    
                    }
                }
            }
        }

        //麻将点击逻辑 
        if (!TableController.Instance.interPanelisShow || TableController.Instance.selfCanChenglaizi)//如果碰 杠的界面出现了，不让点牌
        {
            if(TableController.Instance.selfGameState == SelfState.Can_playCard && Input.GetMouseButtonUp(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if((TableController.Instance.selfGameState == SelfState.Can_playCard || TableController.Instance.selfCanChenglaizi) && Physics.Raycast(ray, out hit))
                {
                    CardData cardData = hit.collider.transform.gameObject.GetComponent<CardData>();
                    RelationData relationDataTemp = RelationDataArray[(int)CharacterType.relative_orignal];
                  
                    if (!CheckIsClickHandCard(relationDataTemp.CardObjects, hit.collider.transform.gameObject))
                    {
                        return;
                    }
                    if (TableController.Instance.selfCanChenglaizi)
                    {
                        if ((Int64)cardData.CardType != TableController.Instance.laizi)
                        {
                            return;
                        }
                    }
                    if (cardData != null)
                    {
                        time2 = Time.realtimeSinceStartup;      
                        if (time2 - time1 < 0.6)
                        {
                            isDoubleClick = true;
                        }
                        time1 = time2;
                        tempTime = time2;
                        hitObj = hit.collider.transform.gameObject;                     
                    }
                }
            }
        }
    
        if (time2 != 0)
        {
            RelationData relationDataTemp = RelationDataArray[(int)CharacterType.relative_orignal];
            CardState cardState;
            CardData cardData;
          
            tempTime -= 0.1;
            if (time2 - tempTime > 0)//暂时取消双击功能
            {
                cardState = hitObj.GetComponent<CardData>().BeginSelect();
                if (_currentClickMj == hitObj)
                {
                    if (cardState == CardState.Play_State)
                    {
                        CardObject obj = GetCardObjByGameObj(relationDataTemp.CardObjects, hitObj);
                        relationDataTemp.CardObjects.Remove(obj);
                        SocketClient.Instance.PlayCard((long)obj.cardType);
                        if (TableController.Instance.selfCanChenglaizi)
                        {
                            TableController.Instance.interPanelisShow = false;
                            TableController.Instance.selfCanChenglaizi = false;
                            EventDispatcher.GetInstance().UIFrameWorkEventManager.TriggerEvent(EventId.UIFrameWork_Cheng_laizi);
                        }
                        TableController.Instance.selfGameState = SelfState.Not_playCard;
                        cardPoolManager.ReleaseByGameObject(obj.cardType, obj.card);
                        _currentClickMj = null;
                        time1 = 0;
                        time2 = 0;
                        isDoubleClick = false;
                    }
                }
                else
                {
                    if (_currentClickMj != null)
                    {
                        cardData = _currentClickMj.GetComponent<CardData>();
                        cardData.ResetObject();
                    }
                    _currentClickMj = hitObj;
                    cardData = _currentClickMj.GetComponent<CardData>();
                    TableController.Instance.SelfChoseCardCheckTing((Int64)cardData.CardType);
                    time1 = 0;
                    time2 = 0;
                    isDoubleClick = false;
                    hitObj = null;
                }
            }
            else if (isDoubleClick)
            {
                if (_currentClickMj != null)
                {
                    if (_currentClickMj != hitObj)
                    {
                        cardData = _currentClickMj.GetComponent<CardData>();
                        cardData.ResetObject();
                    }
                }

                CardObject obj = GetCardObjByGameObj(relationDataTemp.CardObjects, hitObj);
                relationDataTemp.CardObjects.Remove(obj);
                SocketClient.Instance.PlayCard((Int64)obj.cardType);
                if (TableController.Instance.selfCanChenglaizi)
                {
                    TableController.Instance.interPanelisShow = false;
                    TableController.Instance.selfCanChenglaizi = false;
                    EventDispatcher.GetInstance().UIFrameWorkEventManager.TriggerEvent(EventId.UIFrameWork_Cheng_laizi);
                }
                TableController.Instance.SelfChoseCardCheckTing((Int64)obj.cardType);
                TableController.Instance.selfGameState = SelfState.Not_playCard;
                cardPoolManager.ReleaseByGameObject(obj.cardType, obj.card);
                time1 = 0;
                time2 = 0;
                isDoubleClick = false;
                _currentClickMj = null;
            }
        }
    }

    private CardObject GetCardObjByGameObj(List<CardObject> list, GameObject pre)
    {
        CardObject obj;
        for (int i = 0; i < list.Count; i++)
        {
            obj = list[i];
            if (obj.card == pre)
            {
                return obj;
            }
        }
        return null;
    }

    private bool CheckIsClickHandCard(List<CardObject> list, GameObject pre)
    {
        CardObject obj;
        for (int i = 0; i < list.Count; i++)
        {
            obj = list[i];
            if (obj.card == pre)
            {
                return true;
            }
        }
        return false;
    }

    private CharacterType GetRelativeEnum(int orignalOrder, int Order)//通过相对位置得到所在位置
    {
        int relative = orignalOrder - Order;
        CharacterType tempType = CharacterType.Library;
        if (TableController.Instance.creatRoomInfo.playerNum == (int)PeopleNum.FourPeople)
        {
            if (relative == -1 || relative == 3)
            {
                tempType= CharacterType.relative_RightPostion;
            }
            else if (relative == -2 || relative == 2)
            {
                tempType = CharacterType.relative_FrontPostion;
            }
            else if (relative == 1 || relative == -3)
            {
                tempType = CharacterType.relative_LeftPostion;
            }
            else
            {
                tempType = CharacterType.relative_orignal;
            }
        }
        else if (TableController.Instance.creatRoomInfo.playerNum == (int)PeopleNum.ThreePeople)
        {
            //if (relative == -1 || relative == 2)
            //{
            //    tempType = CharacterType.relative_RightPostion;
            //}
            //else if (relative == 1|| relative == -2)
            //{
            //    tempType = CharacterType.relative_FrontPostion;
            //}
            //else
            //{
            //    tempType = CharacterType.relative_orignal;
            //}
            if (relative == -1 )
            {
                tempType = CharacterType.relative_RightPostion;
            }else if (relative == 1)
            {
                tempType = CharacterType.relative_LeftPostion;
            }
            else if (relative == 2 || relative == -2)
            {
                tempType = CharacterType.relative_FrontPostion;
            }
            else
            {
                tempType = CharacterType.relative_orignal;
            }
        }
        else
        {
            if (relative == 1 || relative == -1)
            {
                tempType = CharacterType.relative_FrontPostion;
            }
            else
            {
                tempType = CharacterType.relative_orignal;
            }
        }
        return tempType;
    }


    void OnDestroy()
    {
        tableUpOver = false;
        _playerInfoList = null;
        EventDispatcher.GetInstance().UIFrameWorkEventManager.RemoveEventListener<List<PlayerData>>(EventId.UIFrameWork_Deal_Card_First, BeignUpMjMovie);
        EventDispatcher.GetInstance().UIFrameWorkEventManager.RemoveEventListener<PlayerData,int>(EventId.UIFrameWork_Player_Out_Card, PlayerOutCard);
        EventDispatcher.GetInstance().UIFrameWorkEventManager.RemoveEventListener<Int64, Int64>(EventId.UIFrameWork_Player_Peng_Card, OnPlayPengCard);
        EventDispatcher.GetInstance().UIFrameWorkEventManager.RemoveEventListener<Int64, Int64, Int64>(EventId.UIFrameWork_Player_Gang_Card, OnPlaygGangCard);
        EventDispatcher.GetInstance().UIFrameWorkEventManager.RemoveEventListener<PlayerData>(EventId.UIFrameWork_Player_Draw_Card, OnAddCard);
        EventDispatcher.GetInstance().UIFrameWorkEventManager.RemoveEventListener<List<PlayerData>>(EventId.UIFrameWork_Hupai, OnHuPai);
      //  EventDispatcher.GetInstance().UIFrameWorkEventManager.RemoveEventListener<ArrayList>(EventId.UIFrameWork_Game_liuju, OnLiuJu);
        EventDispatcher.GetInstance().UIFrameWorkEventManager.RemoveEventListener<List<PlayerData>,bool>(EventId.UIFrameWork_Player_Sit_down, OnPlaySitDown);
        EventDispatcher.GetInstance().UIFrameWorkEventManager.RemoveEventListener<List<PlayerData>>(EventId.UIFrameWork_Reconnection, OnReconnection);
        EventDispatcher.GetInstance().UIFrameWorkEventManager.RemoveEventListener<List<PlayerData>>(EventId.UIFrameWork_Reconnection_IsHu, OnReconnectionByIsHu);
        EventDispatcher.GetInstance().UIFrameWorkEventManager.RemoveEventListener<List<Int64>>(EventId.UIFrameWork_Putout_Can_Tingpai, OnPutOutCardCanTing);
        EventDispatcher.GetInstance().UIFrameWorkEventManager.RemoveEventListener(EventId.UIFrameWork_Click_beign_next_game, OnBeginNextGame);
        EventDispatcher.GetInstance().UIFrameWorkEventManager.RemoveEventListener<Int64>(EventId.UIFrameWork_last_mj, OnShowLastMj);
        EventDispatcher.GetInstance().UIFrameWorkEventManager.RemoveEventListener<string>(EventId.UIFrameWork_Set_mj_back, OnSetMjBack);
    }
}
