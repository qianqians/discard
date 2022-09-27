using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Assets.Scripts;
using TinyFrameWork;
using GameCommon;
using System.Text.RegularExpressions;
using UnityEngine.SceneManagement;
using meter;
using DG.Tweening;
using System.Runtime.InteropServices;
public class NUReadyPanel : MonoBehaviour
{
    public List<Button> seatBtnArray;
    public List<Text> nameList;
    public GameObject interactiveContainer = null;
    public Button passBtn;
    public Button huBtn;
    public Button pengBtn;
    public Button gangBtn;
    private Int64 _gangID;
    //private Int64 _huCardID;
    public List<GameObject> headInfoList =null;
    public Image laizishow=null;
    public Image laiziBackImage=null;
    public Image gpsImage;

    //设置相关
    public GameObject disbandPanel;
    public List<Text> disbandList;
    public Button disbandBtn;
    public Button quitBtn = null;
    public List<Image> agreeImageList;
    public Button controlBtn = null;
    public Button settingBtn = null;
    public GameObject controlPanel =null;

    public Text roomIDText;
    public Text jushuTxt;
    public Text baseScoreTxt;
    private int jushuCount;
    //分享
    public Button fenxiangBtn=null;
    //表情
    // public List<GameObject> emjList;
    public Button chatBtn;
    public Button emjCloseBtn;

    //互动表情
    public List<GameObject> emojiMoviePosList;
    //玩家信息面板
    public GameObject playerInfoPanel = null;
    //表情动画播放坐标
    public GameObject emojiMoviePlayContainerPanel = null;
    private List<GameObject> _emojiMovieClipList;
    public Button closePlayerContainerPanelBtn = null;
    public Image otherPlayerHead = null;
    public Image otherPlayerSex = null;
    public Text otherPlayerName = null;
    public Text otherPlayerIDText = null;
    public List<Button> emojiMovieBtnList;
    private int _clickPlayerSeatIndex;
   
    public GameObject chatPanel;
    public GameObject emjPanel;
    public GameObject voicePanel;
    public List<Button> emojiBtnList;
    public List<Button> vocieBtnList;
    public Button emojyBtn;
    public Button voiceBtn;

    public GameObject gCloudVoicePanel;
    public List<Image> emojiImgeList;
    //结算
    private List<AccountData> _playerDataList;
    public GameObject accountPanel = null;
    private EmojiPoolManager _emojiPoolManager;
    private GameObject hupaiStateEffectPrompt;

    //语音图标显示
    private GameObject _currentSayPlayer;

    //听牌面板
    public GameObject TingPanel = null;

    public Text surplusCardTipsTxt = null;
    public Text surplusCardCountTxt = null;
    public GameObject surplusCardTipsGameObject = null;
    private int surplusCardCount;

    // public Text droppedText = null;
  //  public List<Text> droppedTextlist = null;
    public List<Image> droppedImagelist = null;
    //玩法显示 戳瞎子半癞
    public Text PlayRuleTxt = null;
    public Text PlayerNumTxt = null;

    private Int64 _inteFlag;
    // private bool _isTianhu;
    private GameObject InteracrivePrefab;

    public Button nextGameBtn =null;

    //GPS小球浮动
    private float radian = 0; // 弧度  
    private float perRadian = 0.3f; // 每次变化的弧度  
    private float radius = 6.4f; // 半径  
    private Vector3 putOutCardpsotion;

    public List<GameObject> dirctionMCList = null;

    private int chatIntervalTime;
    private int intervalTimeMax =50;
    private GameObject mjPrefab2D;
    private GameObject promptPrefab;

    private Tweener noticeTween;
    private bool isHupai;

    public Text timeShowTxt =null;
    public GameObject timeBackGround = null;
    private int _timeCount;

    public GameObject canvas = null;

    public GameObject gameRoomInfo = null;
    //  private Button mjClcik
#if UNITY_IPHONE
	[DllImport("__Internal")]
	private static extern void ShareWebByIos(string title, string desc, string url);
#endif // UNITY_IPHONE
    void Start()
    {
        surplusCardTipsGameObject.SetActive(false);

        disbandPanel.SetActive(false);
        _emojiMovieClipList = new List<GameObject>();
        _clickPlayerSeatIndex = 0;
        _playerDataList = new List<AccountData>();
        for (int i = 0; i < TableController.Instance.creatRoomInfo.playerNum; i++)
        {
            _playerDataList.Add(new AccountData());
        }
        laiziBackImage.gameObject.SetActive(false);
        //   UIDisbandPanel DisbandCode = disbandPanel.GetComponent<UIDisbandPanel>();
        //  DisbandCode.Init();
        //  surplusCardTipsGameObject.SetActive(true);
        disbandBtn.gameObject.SetActive(false);
        gpsImage.gameObject.SetActive(false);
        timeBackGround.SetActive(false);
        EventDispatcher.GetInstance().UIFrameWorkEventManager.AddEventListener<List<PlayerData>,bool>(EventId.UIFrameWork_Player_Sit_down, OnPlaySitDown);
        EventDispatcher.GetInstance().MainEventManager.AddEventListener<bool>(EventId.SitDownError, OnPlaySitDownError);
        EventDispatcher.GetInstance().UIFrameWorkEventManager.AddEventListener<Int64, Int64>(EventId.SelfCanPengOrGang, OnCanPengOrGangCard);
        EventDispatcher.GetInstance().UIFrameWorkEventManager.AddEventListener<List<PlayerData>>(EventId.UIFrameWork_Deal_Card_First, OnDealCardFirst);
        EventDispatcher.GetInstance().UIFrameWorkEventManager.AddEventListener<List<PlayerData>>(EventId.UIFrameWork_Change_Score, OnChangeScore);
        EventDispatcher.GetInstance().UIFrameWorkEventManager.AddEventListener<int>(EventId.UIFrameWork_Player_Exit_Room, OnPlayerExitRoom);
        EventDispatcher.GetInstance().UIFrameWorkEventManager.AddEventListener(EventId.Server_End_Game, OnEndGame);
        EventDispatcher.GetInstance().UIFrameWorkEventManager.AddEventListener<Vector3, CharacterType,int>(EventId.UIFrameWork_Game_Current_Card_pos, OnPutOutCardPos);
        EventDispatcher.GetInstance().MainEventManager.AddEventListener(EventId.Server_Show_Disband_panel, OnShowDisbandChose);
        EventDispatcher.GetInstance().MainEventManager.AddEventListener<ArrayList>(EventId.Server_Chat_msg, OnGetStr);
        EventDispatcher.GetInstance().UIFrameWorkEventManager.AddEventListener<bool>(EventId.UIFrameWork_Reconnection_Updata, OnReconnection);
        EventDispatcher.GetInstance().UIFrameWorkEventManager.AddEventListener(EventId.Server_Disband_Eng_Game, OnDisbandEndGame);
        EventDispatcher.GetInstance().UIFrameWorkEventManager.AddEventListener<string,bool>(EventId.UIFrameWork_Set_Voice_Active, OnSetVoice);
        EventDispatcher.GetInstance().UIFrameWorkEventManager.AddEventListener<List<Int64>>(EventId.UIFrameWork_Tingpai, OnShowtingpai);
        EventDispatcher.GetInstance().UIFrameWorkEventManager.AddEventListener<EffectPrompt>(EventId.UIFrameWork_Effect_Prompt, OnShowEffectPrompt);
        EventDispatcher.GetInstance().UIFrameWorkEventManager.AddEventListener(EventId.UIFrameWork_Show_Laizi, OnShowLaizi);
        EventDispatcher.GetInstance().UIFrameWorkEventManager.AddEventListener<PlayerData>(EventId.UIFrameWork_Player_Draw_Card, OnAddCard);
        EventDispatcher.GetInstance().MainEventManager.AddEventListener<string,bool>(EventId.Sever_player_off_Ling, OnplayerOfline);
        EventDispatcher.GetInstance().UIFrameWorkEventManager.AddEventListener<List<PlayerData>>(EventId.UIFrameWork_Hupai, OnHuPai);
        EventDispatcher.GetInstance().UIFrameWorkEventManager.AddEventListener(EventId.UIFrameWork_Game_liuju, OnLiuJu);
        EventDispatcher.GetInstance().MainEventManager.AddEventListener<Int64>(EventId.Server_Next_Game, OnClickReadOver);

        EventDispatcher.GetInstance().MainEventManager.AddEventListener<Int64>(EventId.Order, OnOrderHandle);
        EventDispatcher.GetInstance().UIFrameWorkEventManager.AddEventListener(EventId.UIFrameWork_Cheng_laizi, OnChenglaizi);

        EventDispatcher.GetInstance().UIFrameWorkEventManager.AddEventListener<Int64>(EventId.UIFrameWork_last_mj, OnShowLastMj);
        EventDispatcher.GetInstance().UIFrameWorkEventManager.AddEventListener<bool>(EventId.UIFrameWork_Player_record, OnShowSelfRecord);

        EventDispatcher.GetInstance().UIFrameWorkEventManager.AddEventListener(EventId.UIFrameWork_Disband_room_Change, OnDisbandRoomChange);

        gCloudVoicePanel.SetActive(false);
        foreach (var btn in seatBtnArray)
        {
            btn.onClick.AddListener(delegate ()
            {
                OnNumberBtnDown(btn);
            });
        }
        HideAllSeatBtn();

        emjPanel.SetActive(false);

        nextGameBtn.gameObject.SetActive(false);
        //下一局
        nextGameBtn.onClick.AddListener(delegate ()
        {
            nextGameBtn.gameObject.SetActive(false);
            surplusCardTipsGameObject.SetActive(false);
            Destroy(hupaiStateEffectPrompt);
            hupaiStateEffectPrompt = null;
            isHupai = false;
        //    this.CancelInvoke();
            timeBackGround.SetActive(false);
            ClearnumEffect();         
            EventDispatcher.GetInstance().UIFrameWorkEventManager.TriggerEvent(EventId.UIFrameWork_Click_beign_next_game);
            if (mjPrefab2D !=null)
            {
                mjPrefab2D.SetActive(false);
            }           
        });

        chatBtn.onClick.AddListener(delegate ()
        {
            chatPanel.SetActive(true);
        });
        emjCloseBtn.onClick.AddListener(delegate ()
        {
            chatPanel.SetActive(false);
        });

        emojyBtn.onClick.AddListener(delegate ()
        {
            emjPanel.SetActive(true);
            voicePanel.SetActive(false);
        });

        voiceBtn.onClick.AddListener(delegate ()
        {
            voicePanel.SetActive(true);
            emjPanel.SetActive(false);
        });

        //固定语音按钮点击
        foreach (var item in vocieBtnList)
        {
            item.onClick.AddListener(delegate ()
            {
                OnClickVocieBtn(item);
            });
        }

        //表情按钮点击
        foreach (var item in emojiBtnList)
        {
            item.onClick.AddListener(delegate ()
            {
                OnClickEmojiBtn(item);
            });
        }

        foreach (Image item in emojiImgeList)
        {
            item.gameObject.SetActive(false);
        }

        //玩家头像点击
        foreach (GameObject item in headInfoList)
        {
            item.SetActive(false);
            item.GetComponent<Button>().onClick.AddListener(delegate ()
            {
                if (headInfoList.IndexOf(item)!=0)
                {
                    OnOpenPlayerInfoPanel(item);
                }               
            });
            item.transform.FindChild("ImageBanker").gameObject.SetActive(false);
            item.transform.FindChild("readImage").gameObject.SetActive(false);
        }

        //点击动画表情按钮
        foreach (Button item in emojiMovieBtnList)
        {
            item.onClick.AddListener(delegate ()
            {
                OnClickEmojiMovieBtn(item);
            });
        }

        //点击控制按钮
        controlBtn.onClick.AddListener(delegate ()
        {
            OnClickControlBtn(controlBtn);
        });

        controlPanel.GetComponent<Button>().onClick.AddListener(delegate () 
        {
            OnClickControlBtn(controlBtn);
        });

        quitBtn.onClick.AddListener(delegate ()
        {
            OnClickQuiteBtn(quitBtn);
        });

        settingBtn.onClick.AddListener(delegate ()
        {
            OnClickSettingBtn(settingBtn);
        });
        
        disbandBtn.onClick.AddListener(delegate ()
        {
            OnClickDisbandBtn(disbandBtn);
        });

        fenxiangBtn.onClick.AddListener(delegate ()
        {
            // ModeEffect();
            OnClickFenxiangBtn(fenxiangBtn);
        });

        closePlayerContainerPanelBtn.onClick.AddListener(delegate ()
        {
            playerInfoPanel.SetActive(false);
        });

        _emojiPoolManager = new EmojiPoolManager();
        _emojiPoolManager.Init();

        laizishow.gameObject.SetActive(true);
        //  ShowAllSitBtn();
        if (TableController.Instance.PlayrInfo.DataList!=null)
        {
            OnSelfJoinRoom(TableController.Instance.PlayrInfo.DataList);
        }
      //  AddBtnClickHandle();
        ShowRoomRuleInfo();

        UIDisbandPanel DisbandCode = disbandPanel.GetComponent<UIDisbandPanel>();
        DisbandCode.Init(_playerDataList);

        MainManager.Instance.nowSceneName = SceneName.InGame;
        numEffectList = new List<GameObject>();
    }

    private bool isfirstOreder;
    private void OnOrderHandle(Int64 orderIndex)
    {
        if (TableController.Instance.creatRoomInfo.playerNum == (int)PeopleNum.TwoPeople)//二人麻将，特殊处理
        {
            if (orderIndex == 2)
            {
                orderIndex = 3;
            }
        }
        GameObject dir = dirctionMCList[(int)orderIndex-1];
        foreach (GameObject item in dirctionMCList)
        {
            item.GetComponent<Renderer>().material.color = Color.gray;
        }
        dir.GetComponent<Renderer>().material.color = Color.green;
        if (isfirstOreder)
        {
            isfirstOreder = false;
        }
        else
        {
            ShowTimeCount();
        }
       
    }

    private void ShowTimeCount()
    {
        _timeCount = 15;
        timeShowTxt.text = _timeCount.ToString();
        this.CancelInvoke();
        this.InvokeRepeating("setInterval", 1.5f, 1.5f);
        timeBackGround.SetActive(true);
    }

    private void setInterval()
    {
        _timeCount--;
        timeShowTxt.text = _timeCount.ToString();
        if (_timeCount <= 0)
        {
            this.CancelInvoke();
        }
    }

    private void SetDirMacColor()
    {
        foreach (GameObject item in dirctionMCList)
        {
            item.GetComponent<Renderer>().material.color = Color.white;
        }
    }

    void Update()
    {
        // gpsImgaRotate -= 10; 
        if (putOutCardpsotion !=null)
        {
            radian += perRadian; // 弧度每次加0.03  
            float dy = Mathf.Cos(radian) * radius; // dy定义的是针对y轴的变量，也可以使用sin，找到一个适合的值就可以  
            gpsImage.transform.position = putOutCardpsotion + new Vector3(0, dy, 0);
        }

        if (chatIntervalTime>0)
        {
            chatIntervalTime--;
        }          
    }
 
    private void OnShowLaizi()
    {
        ShowLaiziMj();
        _timeCount = 15;
        timeShowTxt.text = _timeCount.ToString();
        //  timeBackGround.SetActive(true);
        ShowTimeCount();
    }

    /// <summary>
    /// 打牌特效
    /// </summary>
    /// <param name="state"></param>
    private void OnShowEffectPrompt(EffectPrompt state)
    {
        string str;
        Sprite texturePic;
        if (state == EffectPrompt.hu)
        {
            if (TableController.Instance.PlayrInfo.CheckSelfIsHaveLaizi())
            {
                str = EffectPrompt.ruanzimo.ToString();
            }
            else
            {
                str = EffectPrompt.yingzimo.ToString();
            }          
        }
        else
        {
            str = state.ToString();
        }
        if (promptPrefab ==null)
        {
            promptPrefab = Resources.Load("UIPrefab/EffectPrompt") as GameObject;
        }
        GameObject prompt = GameObject.Instantiate(promptPrefab);
        prompt.transform.parent = this.transform;
        prompt.transform.localPosition = new Vector3(56, -253, 0);
        Image effImage = prompt.GetComponent<Image>();
        texturePic = Resources.Load("Sprite/jiaohu/" + str, typeof(Sprite)) as Sprite;
        effImage.overrideSprite = texturePic;
        prompt.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(texturePic.textureRect.width, texturePic.textureRect.height);
        if (state == EffectPrompt.hu|| state == EffectPrompt.ruanlaiyou || state == EffectPrompt.yinglaiyou || state == EffectPrompt.yingshangyou)
        {
            hupaiStateEffectPrompt = prompt;
        }
        else
        {
            StartCoroutine("EffectPromptPlayOver", prompt);
        }
    }

    private IEnumerator EffectPromptPlayOver(GameObject obj)
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(obj);
    }

    private void OnShowtingpai(List<Int64> list)
    {
        ViewTingpaiPanel code = TingPanel.GetComponent<ViewTingpaiPanel>();
        if (list.Count>0)
        {
            TingPanel.SetActive(true);
            code.SetTingCard(list);
        }
        else
        {
            TingPanel.SetActive(false);
        }
    }

    private void SetBankerImge(List<PlayerData> list)
    {
        GameObject head;
        bool flag = false;
        foreach (PlayerData item in list)
        {
            head = headInfoList[(int)item.playerType];
            if (item.playerOrderIndex == TableController.Instance.bankerID)
            {
                flag = true;
            }
            else
            {
                flag = false;
            }
            head.transform.FindChild("ImageBanker").gameObject.SetActive(flag);
        }  
    }

    private void ShowRoomRuleInfo()
    {
        surplusCardCountTxt.text = surplusCardCount.ToString();
        if (TableController.Instance.creatRoomInfo.payState ==(int)PayRule.OnePay|| TableController.Instance.creatRoomInfo.payState == (int)PayRule.AAPay)
        {
            gameRoomInfo.SetActive(true);       
            roomIDText.text = SocketClient.Instance.roomID.ToString();
            baseScoreTxt.text = TableController.Instance.creatRoomInfo.baseScore.ToString();
            jushuTxt.text = "1/" + TableController.Instance.creatRoomInfo.jushu.ToString();
        }
        else
        {
            gameRoomInfo.SetActive(false);
            //roomIDText.text="";
            //jushuTxt.text = "";
        }
        
    }

    private void OnDisbandEndGame()
    {
        UIDisbandPanel DisbandCode = disbandPanel.GetComponent<UIDisbandPanel>();
        DisbandCode.HidePanel();
        OnEndGame();
    }

    private void OnSetVoice(string str,bool flag)
    {
        GameObject head;
        CharacterType sit;      
        if (flag)
        {
            sit = TableController.Instance.GetPlayerSeatIndexByUuid(str);
            if (sit !=CharacterType.Library)
            {
                head = headInfoList[(int)sit];
                _currentSayPlayer = head.transform.FindChild("Voice").gameObject;
                _currentSayPlayer.SetActive(true);
            }
           
        }
        else
        {
            if (_currentSayPlayer!=null)
            {
                _currentSayPlayer.SetActive(false);
            }
          
        }     
    }

    private void OnDisbandRoomChange()
    {
        UIDisbandPanel DisbandCode;
        if (TableController.Instance.voteState.Count != 0)
        {
            DisbandCode = disbandPanel.GetComponent<UIDisbandPanel>();
            DisbandCode.isExecute = false;
            if (!disbandPanel.activeSelf)
            {
                disbandPanel.SetActive(true);
                DisbandCode.ShowCountDownTime();
            }
        }

        if (TableController.Instance.voteState.ContainsKey(MainManager.Instance.playerSelfInfo.unionID))
        {
            if ((Int64)TableController.Instance.voteState[MainManager.Instance.playerSelfInfo.unionID] != 0)
            {
                NUMessageBox.instance.MessageBoxRoot.SetActive(false);
            }
        }
    }

    private void OnShowSelfRecord(bool flag)
    {
        if (flag)
        {
            PlayLabaMovie();
        }
        else
        {
            PlayLabamovieOver();
        }
    }
    /// <summary>
    /// 断线重连
    /// </summary>
    private void OnReconnection(bool flag)
    {
        SetResultData(TableController.Instance.PlayrInfo.DataList);
        SetBankerImge(TableController.Instance.PlayrInfo.DataList);
        if (TableController.Instance.voteState.ContainsKey(MainManager.Instance.playerSelfInfo.unionID))
        {
            if ((Int64)TableController.Instance.voteState[MainManager.Instance.playerSelfInfo.unionID] == 0)
            {
                NUMessageBox.Show("是否同意解散房间", DisBandRoom, Objection, true);
            }
            OnDisbandRoomChange();
        }
#if UNITY_ANDROID
        {
            gCloudVoicePanel.SetActive(true);
        }
#elif UNITY_IPHONE
        {
         gCloudVoicePanel.SetActive(true);
        }
#endif
        jushuCount = (int)TableController.Instance.reconnectionData.play_count;
        ShowRoomRuleInfo();
        ShowGameInfo(jushuCount);
        ShowLaiziMj();
        ShowPlayerScore(TableController.Instance.PlayrInfo.DataList);
        //UIDisbandPanel DisbandCode = disbandPanel.GetComponent<UIDisbandPanel>();
        //DisbandCode.Init(_playerDataList);
        if (flag)
        {
            nextGameBtn.gameObject.SetActive(true);
        }

        if (TableController.Instance.surplusCardCount == 0)
        {
            nextGameBtn.gameObject.SetActive(true);
        }
        if (TableController.Instance.laizi!=0)
        {
            quitBtn.enabled = false;
            disbandBtn.gameObject.SetActive(true);
            ShowTimeCount();
        }
    }

    private void OnOpenPlayerInfoPanel(GameObject head)
    {
        _clickPlayerSeatIndex = headInfoList.IndexOf(head);
        playerInfoPanel.SetActive(true);
        ShowOtherPlayerInfo();
    }

    private void ShowOtherPlayerInfo()
    {
        PlayerData data = TableController.Instance.PlayrInfo.GetPlayerDataBySeat((CharacterType)_clickPlayerSeatIndex);
        otherPlayerIDText.text = data.playerID.ToString();
        otherPlayerName.text = data.wechat_name;
        Int64 sexNum = (Int64)data.sex;
        Gender six = (Gender)sexNum;
        loadHeadFlagImage(six);
        LoadPlayerHead(data.headimg,0);
    }

    private void loadHeadFlagImage(Gender six)
    {
        otherPlayerSex.overrideSprite = Resources.Load("ui/" + six.ToString(), typeof(Sprite)) as Sprite;
    }

    private void LoadPlayerHead(string url, int index)
    {
        GameObject ojb = GameObject.Find("DontDestroyOnLoad");
        HttpUtil Until = ojb.GetComponent<HttpUtil>();
        if (url != "")
        {
            string img = Regex.Unescape(url);
            Until.DownloadPicture(img, LoadOtherHeadCallBack, index);
        }
    }

    private void LoadOtherHeadCallBack(Texture2D res,int index)
    {
        Sprite spr = Sprite.Create(res, new Rect(0, 0, res.width, res.height), Vector2.zero);
        otherPlayerHead.overrideSprite = spr;
    }

    private void OnDealCardFirst(List<PlayerData> list)
    {
        isfirstOreder = true;
        try
        {
            if (TableController.Instance.gameIsBegin)
            {
                disbandBtn.gameObject.SetActive(true);
            }
            surplusCardCount = (int)TableController.Instance.surplusCardCount;
            surplusCardCountTxt.text = surplusCardCount.ToString();
            HideAllReadImage();       
            gpsImage.gameObject.SetActive(false);   
            SetResultData(list);
           // TingPanel.SetActive(false);
            SetBankerImge(list);
            jushuCount++;     
            ShowGameInfo(jushuCount);         
        }
        catch (Exception e )
        {
            EventDispatcher.GetInstance().UIFrameWorkEventManager.TriggerEvent<string>(EventId.UIFrameWork_Bug, "OnDealCardFirst");
        }
        quitBtn.enabled = false;
    }

    private void ShowGameInfo(int times)
    {
        TableController.Instance.playCount = jushuCount;
        fenxiangBtn.gameObject.SetActive(false);        
        jushuTxt.text = times.ToString() + "/" + TableController.Instance.creatRoomInfo.jushu.ToString();
    }

    private void ShowLaiziMj()
    {   
        laiziBackImage.gameObject.SetActive(true);
        mjCards mj = (mjCards)TableController.Instance.laizi;
        laizishow.overrideSprite = Resources.Load("mjFront/" + mj.ToString(), typeof(Sprite)) as Sprite;
       // laizishow.gameObject.SetActive(true);
    }

    private void OnChangeScore(List<PlayerData> list)
    {
        PlayerScoreChange(list);
        ClearnumEffect();
        for (int i = 0; i < _playerDataList.Count; i++)
        {
            StartCoroutine("DealCardTimeDelaye", i);
        }
    }

    private GameObject numEffectPrefab;
    private List<GameObject> numEffectList;
    private IEnumerator DealCardTimeDelaye(int i)
    {  
        yield return new WaitForSeconds(0.2f);
        AccountData data;
        GameObject effect;
        NumEffect effectCode;
        Vector3 vec;
        int index;
        Text txt;
        data = _playerDataList[i];
        index = (int)data.sitType;
        txt = headInfoList[index].transform.FindChild("ScoreTxt").GetComponent<Text>();
       
        if (TableController.Instance.gameType == GameType.fangkaGame)
        {
            txt.text = data.score.ToString();
        }
        if (TableController.Instance.gameType == GameType.integarl)
        {
            txt.text = (data.score+data.integarl).ToString();
        }
        if ((int)data.changeScore !=0)
        {
            if (numEffectPrefab ==null)
            {
                numEffectPrefab = Resources.Load("UIPrefab/NumEffect") as GameObject;
            }
            effect = GameObject.Instantiate(numEffectPrefab);
            effectCode = effect.GetComponent<NumEffect>();
            effectCode.SetNumShowHaveState((int)data.changeScore);
            effectCode.transform.parent = headInfoList[index].transform;
            vec = effect.transform.position;
            Tween t = DOTween.To(() => vec, x => vec = x, new Vector3(vec.x, 30, 0), 2);
            t.OnUpdate(() => effect.transform.localPosition = vec);
            t.OnComplete(() => OnEffectPlayComplete(effect));             
        }       
    }

    private void ShowPlayerScore(List<PlayerData> list)
    {
        Text txt;
        int index;
        PlayerData data;
        for (int i = 0; i < list.Count; i++)
        {
            data = list[i];
            index = (int)data.playerType;
            txt = headInfoList[index].transform.FindChild("ScoreTxt").GetComponent<Text>();
            if (TableController.Instance.gameType == GameType.fangkaGame)
            {
                txt.text = data.score.ToString();
            }
            if (TableController.Instance.gameType == GameType.integarl)
            {
                txt.text = (data.score + data.integarl).ToString();
            }
        }
    }

    private void OnEffectPlayComplete(GameObject movie)
    {
        if (isHupai)
        {
            numEffectList.Add(movie);
        }
        else
        {
            Destroy(movie);
        }     
    }

    private void ClearnumEffect()
    {
        GameObject obj;
        for (int i = numEffectList.Count-1; i >= 0; i--)
        {
            obj = numEffectList[i];
            Destroy(obj);
        }
    }
    /// <summary>
    /// 初始化各个玩家的数据
    /// </summary>
    /// <param name="list"></param>
    private void SetResultData(List<PlayerData> list)
    {
        int index;
        AccountData data;
        PlayerData item;
        for (int i = 0; i < list.Count; i++)
        {
            item = list[i];
            index = (int)item.playerOrderIndex-1;           
            data = _playerDataList[index];
            data.headimg = item.headimg;
            data.wechat_name = item.wechat_name;
            if (TableController.Instance.creatRoomInfo.payState == (int)PayRule.AAPay || TableController.Instance.creatRoomInfo.payState == (int)PayRule.OnePay)
            {
                data.changeScore = item.score - data.score;
                data.score = item.score;
            }
            else if (TableController.Instance.creatRoomInfo.payState == (int)PayRule.MatchPay)
            {
              //  data.changeScore = MainManager.Instance.playerSelfInfo.integral;
                data.score = item.integarl+item.score;
            }
            data.ID = item.playerID;
            data.sitType = item.playerType;
            data.token = item.token;          
        }
    }

    private void PlayerScoreChange(List<PlayerData> list)
    {
        int index;
        AccountData data;
        PlayerData item;
        for (int i = 0; i < list.Count; i++)
        {
            item = list[i];
            index = (int)item.playerOrderIndex - 1;
            data = _playerDataList[index];

            data.changeScore = item.score - data.score;
            if (TableController.Instance.creatRoomInfo.payState == (int)PayRule.AAPay || TableController.Instance.creatRoomInfo.payState == (int)PayRule.OnePay)
            {
                data.score = item.score;
            }
            else if (TableController.Instance.creatRoomInfo.payState == (int)PayRule.MatchPay)
            {
                data.score = item.score+item.integarl;
            }
           
        }
    }

    private void OnClickControlBtn(Button btn)
    {
        btn.transform.Rotate(0,0,180);
        if (controlPanel.activeSelf)
        {
            controlPanel.SetActive(false);
        }
        else
        {
            controlPanel.SetActive(true);
        }     
    }

    private void OnClickQuiteBtn(Button btn)
    {
        SocketClient.Instance.ExitRoom();
    }

    /// <summary>
    /// //设置按钮
    /// </summary>
    /// <param name="btn"></param>
    private void OnClickSettingBtn(Button btn)
    {
        EventDispatcher.GetInstance().UIFrameWorkEventManager.TriggerEvent<bool>(EventId.UIFrameWork_Control_settingPanel, true);
    }

    private void OnClickDisbandBtn(Button btn)
    {
        if (TableController.Instance.voteState.Count > 0)
        {
            NUMessageBox.Show("请本次投票结束后再发起");
        }
        else
        {
            disbandPanel.SetActive(true);
            UIDisbandPanel DisbandCode = disbandPanel.GetComponent<UIDisbandPanel>();
            DisbandCode.ShowCountDownTime();
            if (TableController.Instance.gameIsBegin)
            {
                SocketClient.Instance.ReqDisbandRoom();
            }
            else
            {
                SocketClient.Instance.ExitRoom();
            }
        }
       
    }

    private void OnClickFenxiangBtn(Button btn)
    {

        //  List<string> jushuList = new List<string> { "8","16"};
        List<string> payList = new List<string> { "房主付费", "AA付费" };
        List<string> playList = new List<string> { "无癞", "半癞", "一脚癞油", "油上油" };
        List<string> playerNUmList = new List<string> { "二人", "三人", "四人" };
        List<string> playNUmList = new List<string> { "对窝子", "戳虾子", "一脚癞油" };
        //调用方法  TableController.Instance.creatRoomInfo. +
        string playRule = playNUmList[TableController.Instance.creatRoomInfo.playerNum - 2];
        string str1 = "房号：" + SocketClient.Instance.roomID.ToString() + "," + "玩法：" + playList[TableController.Instance.creatRoomInfo.playState - 1] + ",";
        string str2 = "人数：" + playerNUmList[TableController.Instance.creatRoomInfo.playerNum - 2] + "," + "局数：" + TableController.Instance.creatRoomInfo.jushu + ",";
        string str3 = "支付方式：" + payList[TableController.Instance.creatRoomInfo.payState - 1] + "," + "底分：" + TableController.Instance.creatRoomInfo.baseScore.ToString() + "\n";
   

#if UNITY_ANDROID
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        jo.Call("SendMsgTo", playRule, str1, str2, str3);
#elif UNITY_IPHONE
			string title = "三袁互娱——"+playRule;
			string description = str1+"\n"+str2+"\n"+str3+"\n";
			string url = "www.sanyuanhy.com/test/fenxiang.html"; 
		ShareWebByIos(title, description, url);
#endif

    }

    private void OnPlayerExitRoom(int index)
    {
        //   NUMessageBox.Show("是否离开游戏", DengLu);
      //  int index = (int)item.playerType;
        Text txt = nameList[index];
        txt.text = "";
        Button btn = seatBtnArray[index];
        headInfoList[index].SetActive(false);
        if (!TableController.Instance.PlayrInfo.selfSitDown)
        {
            btn.gameObject.SetActive(true);
        }
        if (TableController.Instance.selfGameState == SelfState.SitDown)
        {
         //   HideAllSeatBtn();
            //IGCloudVoice m_voiceengine = GCloudVoice.GetEngine();
         //   int rct = m_voiceengine.JoinTeamRoom(SocketClient.Instance.roomID.ToString(), 15000);//离开要退出语音房间
            //m_voiceengine.QuitRoom(SocketClient.Instance.roomID.ToString(), 15000);
            gCloudVoicePanel.SetActive(false);
        }
    }

    //牌局结束
    private void OnEndGame()
    {
        if (mjPrefab2D!=null)
        {         
            StartCoroutine("GameOverDelayShowSccountPanel");
        }
        else
        {
         //   quitBtn.enabled = true;
            accountPanel.SetActive(true);
            UIAccountPanel accountControl = accountPanel.GetComponent<UIAccountPanel>();
            accountControl.SetPlayerResult(_playerDataList);
        }
        surplusCardTipsGameObject.SetActive(false);
        TingPanel.SetActive(false);     
        //UIDisbandPanel DisbandCode = disbandPanel.GetComponent<UIDisbandPanel>();
        //DisbandCode.Reset();    
        ClickInterItemCallback();
    }

    private IEnumerator GameOverDelayShowSccountPanel()
    {
        yield return new WaitForSeconds(3.0f);
      //  quitBtn.enabled = true;
        mjPrefab2D.SetActive(false);
        accountPanel.SetActive(true);
        UIAccountPanel accountControl = accountPanel.GetComponent<UIAccountPanel>();
        accountControl.SetPlayerResult(_playerDataList);
    }

  //  private bool startmovieGps;

    private void OnPutOutCardPos(Vector3 pos, CharacterType type,int cardID)
    {
        if (cardID == TableController.Instance.laizi)
        {
            gpsImage.gameObject.SetActive(false);
        }
        else
        {
            gpsImage.gameObject.SetActive(true);
            putOutCardpsotion = WordToScenePoint(pos, type);
           // gepsMoviePos = new Vector3(putOutCardpsotion.x, putOutCardpsotion.y+8f, putOutCardpsotion.z);
            //gpsImage.transform.position = WordToScenePoint(pos, type);
        }
    }

    private void OnShowDisbandChose()
    {
        disbandPanel.SetActive(true);
        UIDisbandPanel DisbandCode = disbandPanel.GetComponent<UIDisbandPanel>();
        DisbandCode.ShowCountDownTime();
        if (mjPrefab2D != null)
        {
            mjPrefab2D.SetActive(false);
        }
        NUMessageBox.Show("是否同意解散房间", DisBandRoom,Objection,true);
    }

    private void DisBandRoom(NUMessageBox.CallbackType cbt)
    {
        SocketClient.Instance.VoteDisbandRoom((Int64)roomDisbandVoteState.agree);
    }

    private void Objection(NUMessageBox.CallbackType cbt)
    {
        SocketClient.Instance.VoteDisbandRoom((Int64)roomDisbandVoteState.oppose);
    }

    private void OnNumberBtnDown(Button btn)
    {
        string name = btn.name;
        Int64 nun = Int64.Parse(name.Replace("Button", ""));
        CreatRoomData data = TableController.Instance.creatRoomInfo;

        int needMoney;
        if (data.payState == (int)PayRule.OnePay)
        {
            if (data.creatRoomPlayerUiid == MainManager.Instance.playerSelfInfo.unionID)
            {
                needMoney = ShowConsuma(data);
            }
            else
            {
                needMoney = 0;
            }
        }
        else
        {
            needMoney = ShowConsuma(data);
        }

        if (needMoney > MainManager.Instance.playerSelfInfo.diamondNum)
        {
            NUMessageBox.Show("钻石不足！");
        }
        else
        {
            SetSeatBtnEnable(false);
            SocketClient.Instance.ChoseDir(nun + 1);
        }
    }

    /// <summary>
    /// 设置btn点击按钮
    /// </summary>
    private void SetSeatBtnEnable(bool flag)
    {
        foreach (Button item in seatBtnArray)
        {
            item.enabled = flag;
        }
    }

    private int ShowConsuma(CreatRoomData data)
    {
        int baseConsuma = 1;
        if (data.jushu == (int)GameTimes.Sixteen)
        {
            baseConsuma = 2;
        }

        if (data.payState == (int)PayRule.OnePay)
        {
            baseConsuma = baseConsuma * data.playerNum;
        }
       return baseConsuma;
    }

    //语音按钮点击
    private void OnClickVocieBtn(Button btn)
    {
        List<string> yu = new List<string> {"baziying", "chengbudao", "dasalaisa", "dawodui", "denghaha", "duoxie",  "kuaidian", "shangpeng", "tiaoshui", "wangsicheng", "xiaren"};
        string name = btn.name;
        /// int num = int.Parse(name.Replace("Button", ""));
        int num = vocieBtnList.IndexOf(btn);
        if (chatIntervalTime <= 0)
        {
            SoundManager.Instance.Play(ESoundLayer.EffectUI, yu[num]);
            SocketClient.Instance.PlayerChat(ChatState.Sentence, yu[num]);
            chatPanel.SetActive(false);
            chatIntervalTime = intervalTimeMax;
        }
        else
        {
           // NUMessageBox.Show("消息间隔中！请稍后");
        }

    }

    //表情按钮点击
    private void OnClickEmojiBtn(Button btn)
    {     
     //   string name = btn.name;
        int index = emojiBtnList.IndexOf(btn);
        if (chatIntervalTime <= 0)
        {
            SocketClient.Instance.PlayerChat(ChatState.ChatFace, index.ToString());
            PlayBiaoqianMovie(0, index.ToString());
            chatIntervalTime = intervalTimeMax;
        }
        else
        {
            //NUMessageBox.Show("消息间隔中！请稍后");
        }

    }

    /// <summary>
    /// 玩家互动面板点击
    /// </summary>
    /// <param name="btn"></param>
    private void OnClickEmojiMovieBtn(Button btn)
    {
        int index = emojiMovieBtnList.IndexOf(btn);
        SocketClient.Instance.PlayerChat(ChatState.Emoji, index.ToString(), TableController.Instance.GetPlayerUuidBySeat((CharacterType)_clickPlayerSeatIndex));

        playerInfoPanel.SetActive(false);
        PlayEmojiMovie(index,0, _clickPlayerSeatIndex);
        _clickPlayerSeatIndex = 0;

        otherPlayerHead.sprite = null;
        otherPlayerIDText.text = "";
        otherPlayerName.text = "";
        otherPlayerSex.sprite = null;
    }

    private GameObject chatEmojiPrefab;
    private void PlayBiaoqianMovie(int index,string id)
    {
        Image emoji = emojiImgeList[index];
        GameObject objHead = headInfoList[index];
        if (chatEmojiPrefab ==null)
        {
            chatEmojiPrefab = Resources.Load("UIPrefab/ChatEmoji") as GameObject;
        }
        GameObject mc =GameObject.Instantiate(chatEmojiPrefab);
      
        UGUISpriteAnimation code = mc.GetComponent<UGUISpriteAnimation>();
        code.resPath = "ChatEmoji/";
        code.mcName = "chatEmoji_"+ id;
        code.AutoPlay = true;
        mc.transform.SetParent(objHead.transform, false);
        mc.transform.localScale = new Vector3(1.4f, 1.4f, 1.4f);
        chatPanel.SetActive(false);
    }

    private GameObject labaMovie;
    private void PlayLabaMovie()
    {
        UGUISpriteAnimation code;
        if (chatEmojiPrefab == null)
        {
            chatEmojiPrefab = Resources.Load("UIPrefab/ChatEmoji") as GameObject;
        }
        if (labaMovie ==null)
        {
            labaMovie = GameObject.Instantiate(chatEmojiPrefab);
            code = labaMovie.GetComponent<UGUISpriteAnimation>();
            code.resPath = "effects/";
            code.mcName = "laba";
            code.AutoPlay = true;
            code.Loop = true;
            labaMovie.transform.SetParent(canvas.transform, false);
            labaMovie.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
        }
        code = labaMovie.GetComponent<UGUISpriteAnimation>();
        labaMovie.SetActive(true);
        code.Play();
    }

    private void PlayLabamovieOver()
    {
        UGUISpriteAnimation code;
        if (labaMovie != null)
        {
            code = labaMovie.GetComponent<UGUISpriteAnimation>();
            code.Stop();
            labaMovie.SetActive(false);           
        }
    }

    private void PlayEmojiMovie(int index,int playIndex,int targetPlayerSeat)
    {
        Vector2 pos;
        Vector3 vec;
       // List<string> res = new List<string> { "emoji_chui", "emoji_woshou", "emoji_dianzan", "emoji_guokui", "emoji_hua", "emoji_jutai","emoji_mianfen" };

       // List<string> soundRes = new List<string> { "kuaidian", "duoxie", "tiaoshui", "guikuiban", "seeyou", "jutai", "huimian" };
        movie_config movieclip = movie_configs.GetInstance().tables[index];
        SoundManager.Instance.Play(ESoundLayer.EffectUI, movieclip.sayName);
        GameObject moviePrefab = _emojiPoolManager.GetGameObjectByRes(movieclip.prefabName);
        GameObject movie = GameObject.Instantiate(moviePrefab);
        UGUISpriteAnimation code = movie.GetComponent<UGUISpriteAnimation>();
        code.soundName = movieclip.movieSound;

        movie.transform.localScale = new Vector3(0.8f, 0.8f, 0f);
        movie.transform.parent = emojiMoviePlayContainerPanel.transform;
        GameObject tagobject = emojiMoviePosList[targetPlayerSeat];
        pos = tagobject.transform.localPosition;    
        movie.transform.localPosition = emojiMoviePosList[playIndex].transform.localPosition;

        vec = movie.transform.localPosition;
        _emojiMovieClipList.Add(movie);
        Tween t = DOTween.To(() => vec, x => vec = x, new Vector3(pos.x, pos.y, 0), movieclip.middle);
        t.OnUpdate(() => movie.transform.localPosition = vec);     
        t.OnComplete(() => OnMovieComplete(movie));
    }

    //private void OnKillTween()
    //{

    //}

    private void OnUpdate(Transform trans ,Vector3 vecto)
    {
        trans.localPosition = vecto;
    }

    private void OnMovieComplete(GameObject movie)
    {
        UGUISpriteAnimation code = movie.GetComponent<UGUISpriteAnimation>();
        code.Play();
    }

    private void OnGetStr(ArrayList data)
    {
        int index ;
        ChatState state = (ChatState)data[0];
        string id = (string)data[1];
        string targetUuid = (string)data[2];
        string uuid = (string)data[3];
        if (state == ChatState.Sentence)
        {
            SoundManager.Instance.Play(ESoundLayer.EffectUI, id);
            OnSetVoice(uuid,true);
            StartCoroutine("HideVoiceBarDelaye", uuid);
        }
        else if (state == ChatState.ChatFace)
        {
            index = (int)TableController.Instance.GetPlayerSeatIndexByUuid(uuid);
            PlayBiaoqianMovie(index,id);        
        }
        else if (state == ChatState.Emoji)
        {
            int playerSeat = (int)TableController.Instance.GetPlayerSeatIndexByUuid(uuid);
            int targetPlayerSeat = (int)TableController.Instance.GetPlayerSeatIndexByUuid(targetUuid);
            PlayEmojiMovie(int.Parse(id), playerSeat, targetPlayerSeat);
        }
    }

    private IEnumerator HideVoiceBarDelaye(string uuid)
    {
        yield return new WaitForSeconds(1.5f);
        OnSetVoice(uuid, false);
    }
    /// <summary>
    /// 动态表情移动
    /// </summary>
    /// <param name="emoji"></param>
    /// <returns></returns>
    private IEnumerator TimeDelaye(GameObject emoji)
    {
        yield return new WaitForSeconds(1);
        Destroy(emoji);
    }

    private void OnSelfJoinRoom(List<PlayerData> list)
    {
        Text txt;
        int index;
        Button btn;
        string url;    
        foreach (PlayerData item in list)
        {
            index = (int)item.playerType;
            btn = seatBtnArray[index];
            txt = nameList[index];
            txt.text = item.wechat_name;
            if (item.token != "")
            {
                url = item.headimg;
                headInfoList[index].SetActive(true);
                if (url == "")
                {
                    headInfoList[index].transform.FindChild("Image").gameObject.SetActive(false);
                }
                else
                {
                 //   EventDispatcher.GetInstance().UIFrameWorkEventManager.TriggerEvent<string>(EventId.UIFrameWork_Bug, "loadhead|||" + index);
                    LoadHead(url, index);
                }

                if (item.IsSelfData && item.dir != Directions.None)//自己是否坐下
                {
                    TableController.Instance.selfGameState = SelfState.SitDown;
                    //IGCloudVoice m_voiceengine = GCloudVoice.GetEngine();
                    //int rct = m_voiceengine.JoinTeamRoom(SocketClient.Instance.roomID.ToString(), 15000);//离开要退出语音房间，
                    #if UNITY_ANDROID
                    {
                        gCloudVoicePanel.SetActive(true);
                    }
                    #elif UNITY_IPHONE
                    {
                        gCloudVoicePanel.SetActive(true);
                    }
                    #endif
                }
            }
            else
            {
                btn.gameObject.SetActive(true);                         
            }

            if (TableController.Instance.selfGameState == SelfState.SitDown)
            {
                HideAllSeatBtn();
            }
        }

        ShowPlayRule();
    }

    private void ShowPlayRule()
    {
        List<string> playerNum = new List<string> {"对窝子", "戳虾子","晃晃" };
        List<string> rule = new List<string> { "无癞", "半癞", "一脚癞油","油上油" };
        PlayerNumTxt.text = playerNum[TableController.Instance.creatRoomInfo.playerNum - 2];
      //  PlayRuleTxt.text = rule[TableController.Instance.creatRoomInfo.playState-1];
    }

    private void OnPlaySitDown(List<PlayerData> list,bool isSelf)
    {
        Text txt;
        int index;
        Button btn;
        string url;

        foreach (GameObject item in headInfoList)
        {
            Image imge = item.transform.FindChild("Image").GetComponent<Image>();
            item.SetActive(false);
            imge.overrideSprite = null;
        }

        foreach (PlayerData item in list)
        {
            index = (int)item.playerType;
            txt = nameList[index];
            txt.text = item.wechat_name;
            btn = seatBtnArray[index];
            if (item.token != "")
            {
                url = item.headimg;
                headInfoList[index].SetActive(true);
                btn.gameObject.SetActive(false);
                if (url == "")
                {
                    headInfoList[index].transform.FindChild("Image").gameObject.SetActive(false);
                }
                else
                {
                   // EventDispatcher.GetInstance().UIFrameWorkEventManager.TriggerEvent<string>(EventId.UIFrameWork_Bug, index.ToString());
                    LoadHead(url, index);
                }
            }
            else
            {
                btn.gameObject.SetActive(true);
            }
        }

        if (TableController.Instance.selfGameState == SelfState.SitDown)
        {
            HideAllSeatBtn();
         //   IGCloudVoice m_voiceengine = GCloudVoice.GetEngine();
          // int rct = m_voiceengine.JoinTeamRoom(SocketClient.Instance.roomID.ToString(), 15000);//离开要退出语音房间
         //   m_voiceengine.QuitRoom(SocketClient.Instance.roomID.ToString(), 15000);
            gCloudVoicePanel.SetActive(true);
        }
    }

    private void LoadHead(string url, int index)
    {
        GameObject ojb = GameObject.Find("DontDestroyOnLoad");
        HttpUtil Until = ojb.GetComponent<HttpUtil>();
        if (url !="")
        {
            //string img = Regex.Unescape(url);
            Until.DownloadPicture(url, HeadPicLoadCallBack, index);
        }
    }

    private void OnPlaySitDownError(bool flag)
    {
        if (!flag)
        {
            SetSeatBtnEnable(true);
        }
    }

    public void HeadPicLoadCallBack(Texture2D res, int index)
    {
        GameObject head = headInfoList[index];
        Image imge = head.transform.FindChild("Image").GetComponent<Image>();
        Sprite spr = Sprite.Create(res, new Rect(0, 0, res.width, res.height), Vector2.zero);
        imge.overrideSprite = spr;
        head.SetActive(true);
    }

    private void HideAllSeatBtn()
    {
        foreach (Button btn in seatBtnArray)
        {
            btn.gameObject.SetActive(false);
        }
    }

    private void ShowAllSitBtn()
    {
        foreach (Button btn in seatBtnArray)
        {
            btn.gameObject.SetActive(true);
        }
    }

    //private void OnCanHuAndGang(Int64 huCard, Int64 GangCard)
    //{
    //    interactiveContainer.SetActive(true);
    //    TableController.Instance.interPanelisShow = true;
    //    _gangID = GangCard;
    //    _huCardID = huCard;
    //    _inteFlag = 6;
    //    List<InteractivePrompt> showItemlist = new List<InteractivePrompt>();
    //    showItemlist.Add(InteractivePrompt.guo);
    //    showItemlist.Add(InteractivePrompt.gang);
    //    showItemlist.Add(InteractivePrompt.hu);
    //    for (int i = 0; i < showItemlist.Count; i++)
    //    {
    //        AddIneractiveBtn(showItemlist[i], i);
    //    }
    //}

    /// <summary>
    /// 碰 杠 hu
    /// </summary>
    /// <param name="card"></param>
    private void OnCanPengOrGangCard(Int64 flag, Int64 cardID)
    {
        List<InteractivePrompt> showItemlist = new List<InteractivePrompt>();
        TableController.Instance.interPanelisShow = true;
        _gangID = cardID;
       // _inteFlag = flag;
        showItemlist.Add(InteractivePrompt.guo);
        switch (flag)
        {
            case 2:
                showItemlist.Add(InteractivePrompt.peng);
                TableController.Instance.isCanPeng = true;
                break;
            case 3:
                showItemlist.Add(InteractivePrompt.peng);
                showItemlist.Add(InteractivePrompt.gang);
                break;
            case 4:
                showItemlist.Add(InteractivePrompt.gang);
                break;
            case 5:
                showItemlist.Add(InteractivePrompt.hu);
                TableController.Instance.selfCanChenglaizi = true;
                break;
            case 6:
                showItemlist.Add(InteractivePrompt.gang);
                showItemlist.Add(InteractivePrompt.hu);
                TableController.Instance.selfCanChenglaizi = true;
                break;
            case 7:
                showItemlist.Add(InteractivePrompt.peng);
                showItemlist.Add(InteractivePrompt.hu);
                TableController.Instance.selfCanChenglaizi = true;
                break;
            case 8:
                showItemlist.Add(InteractivePrompt.peng);
                showItemlist.Add(InteractivePrompt.gang);
                showItemlist.Add(InteractivePrompt.hu);
                TableController.Instance.selfCanChenglaizi = true;
                break;
            case 9:
                showItemlist.Add(InteractivePrompt.hu);
                TableController.Instance.selfCanChenglaizi = true;
                break;
            default:
                break;
        }
      //  GameObject prompt;
        for (int i = 0; i < showItemlist.Count; i++)
        {
            AddIneractiveBtn(showItemlist[i], i, flag);
        }
        //SocketClient.Instance.SendInteractive("guo", 0);
        //ClickInterItemCallback();
    }

    //private GameObject robetGeme;
    private void AddIneractiveBtn(InteractivePrompt state,int index,Int64 flag)//zhidong
    {
        float wide =140;
        InteracitveItem code;
        if (InteracrivePrefab ==null)
        {
            InteracrivePrefab = Resources.Load("UIPrefab/InteracriveRes") as GameObject;
        }
        GameObject prompt = GameObject.Instantiate(InteracrivePrefab);
        prompt.transform.SetParent(interactiveContainer.transform, false);    
        Image effImage = prompt.GetComponent<Image>();
        code = prompt.GetComponent<InteracitveItem>();
        code.attrs = state;
        code.callback = ClickInterItemCallback;
        Sprite texture;
        if (state == InteractivePrompt.gang)
        {
            code.cardID = _gangID;        
        }
        else if (state == InteractivePrompt.peng)
        {
            code.cardID = TableController.Instance.currentCardID;
        }
        else if (state == InteractivePrompt.hu)
        {
            if (TableController.Instance.lastTimePutOutLaizi)
            {
              //  state = InteractivePrompt.laiyou;
            }
            if (flag== 7|| flag == 8 || flag ==9)
            {
                code.cardID = TableController.Instance.currentCardID;
            }
            else
            {
                code.cardID = TableController.Instance.selfGetCardID;
            }
           
        }
        texture = Resources.Load("Sprite/jiaohu/" + state.ToString(), typeof(Sprite)) as Sprite;
        effImage.overrideSprite = texture;
        prompt.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(texture.textureRect.width, texture.textureRect.height);
        wide = effImage.GetComponent<RectTransform>().sizeDelta.x ;
        effImage.transform.localPosition = new Vector3(effImage.transform.localPosition.x- wide* index, effImage.transform.localPosition.y,0);
    }

    private void ClickInterItemCallback()
    {
        for (int j = interactiveContainer.transform.childCount-1; j >= 0; j--)
        {
            GameObject item = interactiveContainer.transform.GetChild(j).gameObject;
            Destroy(item);
        }
        TableController.Instance.selfCanChenglaizi = false;
        TableController.Instance.isCanPeng = false;
    }

    private void OnChenglaizi()
    {
        for (int j = interactiveContainer.transform.childCount - 1; j >= 0; j--)
        {
            GameObject item = interactiveContainer.transform.GetChild(j).gameObject;
            Destroy(item);
        }
        TableController.Instance.selfCanChenglaizi = false;
    }
    /// <summary>
    /// 出牌标识 定位位置
    /// </summary>
    /// <param name="wordPosition"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public static Vector3 WordToScenePoint(Vector3 wordPosition, CharacterType type)
    {
        GameObject CameraObject = GameObject.Find("Main Camera");
        Camera mainCarema = CameraObject.GetComponent<Camera>();
        Vector3 pos =  mainCarema.WorldToScreenPoint(wordPosition);
        if (type == CharacterType.relative_RightPostion)
        {
            pos.y = pos.y + 35;
            pos.x = pos.x - 30;
        }
        else if (type == CharacterType.relative_LeftPostion)
        {
            pos.y = pos.y + 35;
            pos.x = pos.x + 32;
        }
        else if (type == CharacterType.relative_orignal)
        {
            pos.y = pos.y + 75;
        }
        else
        {
            pos.y = pos.y + 30;
        }        
         return pos;
    }

    /// <summary>
    /// 起牌
    /// </summary>
    /// <param name="data"></param>
    private void OnAddCard(PlayerData data)
    {
        surplusCardCount--;
        if (surplusCardCount<0)
        {
            surplusCardCount = 0;
        }
        if (surplusCardCount<=4 && surplusCardCount> 0)
        {
            surplusCardTipsGameObject.SetActive(true);
            surplusCardTipsTxt.text = "注意噢！\n" + "只剩最后" + surplusCardCount + "张牌了";
        }
        else
        {
            surplusCardTipsGameObject.SetActive(false);
        }
        surplusCardCountTxt.text = surplusCardCount.ToString();
    }

    private void OnHuPai(List<PlayerData> arr)
    {
        gpsImage.gameObject.SetActive(false);
        TingPanel.SetActive(false);

        if (TableController.Instance.gameType == GameType.integarl)
        {
            nextGameBtn.gameObject.SetActive(true);
        }
        if (TableController.Instance.gameType == GameType.fangkaGame)
        {
            if (jushuCount < TableController.Instance.creatRoomInfo.jushu)
            {
                nextGameBtn.gameObject.SetActive(true);
            }
        }    
        SetDirMacColor();
        // laizishow.sprite = null;
        laiziBackImage.gameObject.SetActive(false);
        surplusCardCount = 0;
        surplusCardCountTxt.text = "";
        isHupai = true;
    }

    private void OnLiuJu()
    {
        gpsImage.gameObject.SetActive(false);
        TingPanel.SetActive(false);
        if (jushuCount < TableController.Instance.creatRoomInfo.jushu)
        {
            nextGameBtn.gameObject.SetActive(true);
          //  RobetNextGame();
        }
        SetDirMacColor();
        //laizishow.sprite = null;
        //laizishow.gameObject.SetActive(false);
        laiziBackImage.gameObject.SetActive(false);
    }

    /// <summary>
    /// robetGame
    /// </summary>
    private void RobetNextGame()
    {
        nextGameBtn.gameObject.SetActive(false);
        surplusCardTipsGameObject.SetActive(false);
        Destroy(hupaiStateEffectPrompt);
        hupaiStateEffectPrompt = null;
        EventDispatcher.GetInstance().UIFrameWorkEventManager.TriggerEvent(EventId.UIFrameWork_Click_beign_next_game);
        if (mjPrefab2D != null)
        {
            mjPrefab2D.SetActive(false);
        }
    }

    /// <summary>
    /// 显示最后一张牌
    /// </summary>
    /// <param name="mj"></param>
    private void OnShowLastMj(Int64 mj)
    {
      //  Tweener noticeTween;
        Image img;
        if (mjPrefab2D ==null)
        {
            mjPrefab2D=GameObject.Instantiate(Resources.Load("UIPrefab/MjCard2D") as GameObject);
            mjPrefab2D.transform.SetParent(this.transform, false);
        }
        mjPrefab2D.SetActive(true);
        mjCards card = (mjCards)mj;
        img = mjPrefab2D.transform.FindChild("back").GetComponent<Image>();
        img.overrideSprite = Resources.Load("mjFront/" + card.ToString(), typeof(Sprite)) as Sprite;
        noticeTween = mjPrefab2D.GetComponent<Image>().rectTransform.DOScale(new Vector3(2, 2, 2), 1.5f); 
        noticeTween.OnComplete(() => OnShowLastMjComplete(mjPrefab2D));
    }

    private void OnShowLastMjComplete(GameObject movie)
    {
        mjPrefab2D.transform.localScale= new Vector3(1f,1f,1f);
    }

    /// <summary>
    /// 哪个玩家点击了下一把按钮
    /// </summary>
    /// <param name="playerOrderIndex"></param>
    private void OnClickReadOver(Int64 playerOrderIndex)
    {
        CharacterType seat = TableController.Instance.PlayrInfo.GetPlayerSeatByOrderIndex(playerOrderIndex);
        GameObject head = headInfoList[(int)seat];
        head.transform.FindChild("readImage").gameObject.SetActive(true); 
    }

    private void HideAllReadImage()
    {
        foreach (GameObject item in headInfoList)
        {
            item.transform.FindChild("readImage").gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 玩家掉线
    /// </summary>
    /// <param name="id"></param>
    private void OnplayerOfline(string id,bool flag)
    {
        CharacterType index = TableController.Instance.GetPlayerSeatIndexByUuid(id);
        Image img = droppedImagelist[(int)index-1];
        img.gameObject.SetActive(flag);      
    }

    void OnDestroy()
    {
        foreach (GameObject item in _emojiMovieClipList)
        {
            Destroy(item);
        }
        SetSeatBtnEnable(true);
        Destroy(mjPrefab2D);
        mjPrefab2D = null;
        if (_playerDataList!=null)
        {
            _playerDataList.Clear();
        }
        //DOTween.KillAll();
        EventDispatcher.GetInstance().UIFrameWorkEventManager.RemoveEventListener<List<PlayerData>,bool>(EventId.UIFrameWork_Player_Sit_down, OnPlaySitDown);
        EventDispatcher.GetInstance().MainEventManager.RemoveEventListener<bool>(EventId.SitDownError, OnPlaySitDownError);
        EventDispatcher.GetInstance().UIFrameWorkEventManager.RemoveEventListener<Int64, Int64>(EventId.SelfCanPengOrGang, OnCanPengOrGangCard);
        EventDispatcher.GetInstance().UIFrameWorkEventManager.RemoveEventListener<List<PlayerData>>(EventId.UIFrameWork_Deal_Card_First, OnDealCardFirst);
        EventDispatcher.GetInstance().UIFrameWorkEventManager.RemoveEventListener<List<PlayerData>>(EventId.UIFrameWork_Change_Score, OnChangeScore);
        EventDispatcher.GetInstance().UIFrameWorkEventManager.RemoveEventListener<int>(EventId.UIFrameWork_Player_Exit_Room, OnPlayerExitRoom);
        EventDispatcher.GetInstance().UIFrameWorkEventManager.RemoveEventListener(EventId.Server_End_Game, OnEndGame);
        EventDispatcher.GetInstance().UIFrameWorkEventManager.RemoveEventListener<Vector3, CharacterType,int>(EventId.UIFrameWork_Game_Current_Card_pos, OnPutOutCardPos);
        EventDispatcher.GetInstance().MainEventManager.RemoveEventListener(EventId.Server_Show_Disband_panel, OnShowDisbandChose);
        EventDispatcher.GetInstance().MainEventManager.RemoveEventListener<ArrayList>(EventId.Server_Chat_msg, OnGetStr);
        EventDispatcher.GetInstance().UIFrameWorkEventManager.RemoveEventListener<bool>(EventId.UIFrameWork_Reconnection_Updata, OnReconnection);
        EventDispatcher.GetInstance().UIFrameWorkEventManager.RemoveEventListener(EventId.Server_Disband_Eng_Game, OnDisbandEndGame);
        EventDispatcher.GetInstance().UIFrameWorkEventManager.RemoveEventListener<string, bool>(EventId.UIFrameWork_Set_Voice_Active, OnSetVoice);
        EventDispatcher.GetInstance().UIFrameWorkEventManager.RemoveEventListener<List<Int64>>(EventId.UIFrameWork_Tingpai, OnShowtingpai);
        EventDispatcher.GetInstance().UIFrameWorkEventManager.RemoveEventListener<EffectPrompt>(EventId.UIFrameWork_Effect_Prompt, OnShowEffectPrompt);
        EventDispatcher.GetInstance().UIFrameWorkEventManager.RemoveEventListener(EventId.UIFrameWork_Show_Laizi, OnShowLaizi);
        EventDispatcher.GetInstance().UIFrameWorkEventManager.RemoveEventListener<PlayerData>(EventId.UIFrameWork_Player_Draw_Card, OnAddCard);
        EventDispatcher.GetInstance().MainEventManager.RemoveEventListener<string, bool>(EventId.Sever_player_off_Ling, OnplayerOfline);
        EventDispatcher.GetInstance().UIFrameWorkEventManager.RemoveEventListener<List<PlayerData>>(EventId.UIFrameWork_Hupai, OnHuPai);
        EventDispatcher.GetInstance().UIFrameWorkEventManager.RemoveEventListener(EventId.UIFrameWork_Game_liuju, OnLiuJu);
        EventDispatcher.GetInstance().MainEventManager.RemoveEventListener<Int64>(EventId.Server_Next_Game, OnClickReadOver);

        EventDispatcher.GetInstance().MainEventManager.RemoveEventListener<Int64>(EventId.Order, OnOrderHandle);
        EventDispatcher.GetInstance().UIFrameWorkEventManager.RemoveEventListener(EventId.UIFrameWork_Cheng_laizi, OnChenglaizi);
        EventDispatcher.GetInstance().UIFrameWorkEventManager.RemoveEventListener<Int64>(EventId.UIFrameWork_last_mj, OnShowLastMj);
        EventDispatcher.GetInstance().UIFrameWorkEventManager.RemoveEventListener<bool>(EventId.UIFrameWork_Player_record, OnShowSelfRecord);
        EventDispatcher.GetInstance().UIFrameWorkEventManager.RemoveEventListener(EventId.UIFrameWork_Disband_room_Change, OnDisbandRoomChange);
    }
}
