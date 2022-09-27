using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts;
using System;
using TinyFrameWork;
using System.Text.RegularExpressions;
using GameCommon;
using System.Runtime.InteropServices;
public class NUMainWindow : MonoBehaviour
{
    // 主界面
    public Button ClassGameBtn = null;
    public Button creatRoomBtn = null;
    public Button joinRoomBtn = null;
    public Button bisaiPanelBtn = null;
    public Button bisaiPanelQuit_Btn = null;

    public GameObject LobbyPanel = null;
    public GameObject JoinRoomPanel = null;
    public GameObject biSaiPanel = null;
    public List<Button> MianBtnArray = null;
    public List<GameObject> panelList = null;
    public List<Button> closePanelBtnList = null;
    private List<string> _choseList;

    // 房间界面
    public Button CreateGameRobot = null;
    public Button CreateGameBtn = null; 
    public GameObject CreateGamePanel = null;
    public Text consumeTxt = null;
  //  public List<Button> ToggleBtnList = null;
    public Button CloseLobbyPanelBtn = null;
    public Button CloseCreateGamePanelBtn = null;

    //一脚癞油的大厅界面
    public GameObject YijiaoPanel = null;
    public Button CloseCreateYijiaoPanelBtn = null;

    public Button[] NumberBtnArray = new Button[12];
    public Text RoomNumberText = null;
    // 设置界面
    public GameObject SettingPanel = null;
    public Button SettingBtn = null;
    public Button CloseSettingPanelBtn = null;
    public Slider mainSlider;
    public Slider gameSoundSlider;
    public Text SoundTxt;

    //createroom
    public Button addBtn=null;
    public Button reduceBtn =null;
    public Text DiScore;
    private List<int> scoresNumList = new List<int>() { 2,5,10,20,50};
    private int CurrentScore = 0;
    // Use this for initialization
   // public Button BuyBtn;
    public Image headInfo;
    public Image headSixImage;
    public Text selfName;
    //self info
    public Text diamondTxt =null;
    public Text goldTxt = null;
    public Text IDTxt =null;

    //购买钻石
    public Button openBuyDiamondPanel;
    public Button closeBuyDiamondPanel;
    public GameObject buyDiamondPanel;
   // public List<Button> buyBtnList;

    //绑定界面
    public Button bindBtn;
    public Text bindNumTxt;
    public InputField bindInputNumTxt=null;
    //签到
    public List<Image> signImgeList = null;
    public Button signBtn =null;
    public Text signTxt = null;
    //红包
    public GameObject redBagPanel = null;

	#if UNITY_IPHONE
	[DllImport("__Internal")]
	private static extern void weixinPayByPrepayID(string prepayid);
	#endif // UNITY_IPHONE
    void Start()
    {
        selfName.text = MainManager.Instance.playerSelfInfo.name;
        consumeTxt.text = "2";
        creatRoomBtn.onClick.AddListener(delegate ()
        {
            OnOpenCreateGamePanel(gameObject);
        });

        if (MainManager.Instance.bindingID !=0)
        {
            bindBtn.enabled = false;
            bindBtn.gameObject.SetActive(false);    
            bindInputNumTxt.text = MainManager.Instance.bindingID.ToString();
            bindInputNumTxt.enabled = false;
            // bindNumTxt.text = MainManager.Instance.bindingID.ToString();
            //   bindNumTxt.enabled = false;          
        }
        //绑定按钮
        bindBtn.onClick.AddListener(delegate ()
        {
            //SocketClient.Instance.SendRedBag(1000,10);
            OnClickBindBtn(gameObject);
        });

        joinRoomBtn.onClick.AddListener(delegate ()
        {
            OnOpenJoinRoomPanel(gameObject);
        });

        bisaiPanelBtn.onClick.AddListener(delegate ()
        {
            OnOpenBiSaiPanel(gameObject);
        });

        bisaiPanelQuit_Btn.onClick.AddListener(delegate ()
        {
            OnQuitBisaiPanel(gameObject);
        });

        CreateGameBtn.onClick.AddListener(delegate ()
        {
            OnCreateGame(gameObject);
           // SocketClient.Instance.CreateRoom();
        });
        CreateGameRobot.onClick.AddListener(delegate ()
        {
            OnCreateGameRobot(gameObject);
        });

        //签到
        signBtn.onClick.AddListener(delegate ()
        {
            OnSingn(gameObject);
        });

        //主界面的按钮点击
        foreach (var btn in MianBtnArray)
        {
            btn.onClick.AddListener(delegate ()
            {
                OnMainBtnDown(btn);
            });
        }

        foreach (var btn in closePanelBtnList)
        {
            btn.onClick.AddListener(delegate ()
            {
                OnClosePanelBtnDown(btn);
            });
        }

        foreach (var btn in NumberBtnArray)
        {
            btn.onClick.AddListener(delegate ()
            {
                OnNumberBtnDown(btn);
            });
        }
        CloseLobbyPanelBtn.onClick.AddListener(delegate ()
        {
            OnCloseLobbyPanel(gameObject);
        });
        CloseCreateGamePanelBtn.onClick.AddListener(delegate ()
        {
            OnCloseCreateGamePanel(gameObject);
        });

        SettingBtn.onClick.AddListener(delegate ()
        {
            OnOpenSettingPanel(gameObject);
        });

        addBtn.onClick.AddListener(delegate ()
        {
            OnChangScore(1);
        });
        reduceBtn.onClick.AddListener(delegate ()
        {
            OnChangScore(-1);
        });

        closeBuyDiamondPanel.onClick.AddListener(delegate ()
        {
            buyDiamondPanel.SetActive(false);
        });
      
        RoomNumberText.text = "";
        OnLoadHeadInfo();
      //  Hashtable info = TableController.Instance.SelfInfo["player_info"] as Hashtable;
        OnSelfInfoUpdata();
        IDTxt.text = MainManager.Instance.playerSelfInfo.gameID.ToString();
        EventDispatcher.GetInstance().MainEventManager.AddEventListener<Hashtable>(EventId.Server_PlayerInfo_Updata, OnSelfInfoUpdata);
        EventDispatcher.GetInstance().MainEventManager.AddEventListener<string>(EventId.Server_Pay_msg, OnPay);
        EventDispatcher.GetInstance().MainEventManager.AddEventListener<string>(EventId.Sever_Agent_bind, OnBind);
        EventDispatcher.GetInstance().MainEventManager.AddEventListener<Int64>(EventId.Server_Daily_Signin, OnSigninSuccess);
        EventDispatcher.GetInstance().MainEventManager.AddEventListener<ArrayList>(EventId.Sever_refresh_red_rank_broadcast, OnRefreshRank);
        SetSignDays(MainManager.Instance.playerSelfInfo.signCount);
        signTxt.gameObject.SetActive(false);
        if (MainManager.Instance.playerSelfInfo.isSignToday)
        {
            SetSignStae();
        }
        MainManager.Instance.nowSceneName = SceneName.NUMainWindow;
    }

    private void OnRefreshRank(ArrayList list)
    {

    }

    private void OnSelfInfoUpdata(Hashtable info =null)
    {
        Int64 gold = MainManager.Instance.playerSelfInfo.goldNum;
        Int64 diamond = MainManager.Instance.playerSelfInfo.diamondNum;
        diamondTxt.text = diamond.ToString();
        goldTxt.text = gold.ToString();
    }

    private void OnMainBtnDown(Button btn)
    {
        string name = btn.name;
        // int index = int.Parse((name.Replace("Button", "")));
        int index = MianBtnArray.IndexOf(btn);
        GameObject panle = panelList[index];
        if (panle !=null)
        {
            panle.SetActive(true);
        }  
    }

    private void OnClosePanelBtnDown(Button btn)
    {
      //  string name = btn.name;
        int index = closePanelBtnList.IndexOf(btn);
        GameObject panle = panelList[index];
        panle.SetActive(false);
    }
    
    private void OnPay(String id)
    {
		Debug.Log("OnPay id:" + id);
		#if UNITY_ANDROID
	        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
	        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
	        //调用方法  
	        jo.Call("WXPayByServer", id);
		#elif UNITY_IPHONE
			weixinPayByPrepayID(id);             
		#endif
    }

    private void WXFenxin()
    {
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        //调用方法  
         jo.Call("SendMsgTo","一脚癞油是流行于湖北荆州的一款麻将游戏");
    }

    private void OnBind(string msg)
    {
        if (msg == "ok")
        {
            NUMessageBox.Show("绑定成功");
        }
        else
        {
            NUMessageBox.Show(msg);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    //public void OnClassGame(GameObject obj)
    //{
    //    LobbyPanel.SetActive(true);
    //    RankPanel.SetActive(false);
    //    GamePlay.SetActive(false);
    //    YijiaoPanel.SetActive(true);
    //}

    //绑定
    public void OnClickBindBtn(GameObject obj)
    {
        Int64 id = Int64.Parse(bindNumTxt.text);
        SocketClient.Instance.BindRecommend(id);
    }

    public void OnOpenJoinRoomPanel(GameObject obj)
    {
        JoinRoomPanel.SetActive(true);
    }

    public void OnOpenBiSaiPanel(GameObject obj)
    {
        biSaiPanel.SetActive(true);
        LobbyPanel.SetActive(false);
    }

    public void OnQuitBisaiPanel(GameObject obj)
    {
        biSaiPanel.SetActive(false);
        LobbyPanel.SetActive(true);
    }

    public void OnCloseLobbyPanel(GameObject obj)
    {
        //YijiaoPanel.SetActive(true);
        JoinRoomPanel.SetActive(false);
    }

    public void OnOpenCreateGamePanel(GameObject obj)
    {
        CreateGamePanel.SetActive(true);
    }

    public void OnCloseCreateGamePanel(GameObject obj)
    {
        CreateGamePanel.SetActive(false);
   //     YijiaoPanel.SetActive(true);
    }

    public void OnOpenSettingPanel(GameObject obj)
    {
      //  SettingPanel.SetActive(true);
        EventDispatcher.GetInstance().UIFrameWorkEventManager.TriggerEvent<bool>(EventId.UIFrameWork_Control_settingPanel, true);      
    }

    public void OnChangScore(int num)
    {
        CurrentScore += num;
        if (CurrentScore>=5)
        {
            CurrentScore = 4;
        }
        else if (CurrentScore<=0)
        {
            CurrentScore = 0;
        }
 
        int index = scoresNumList[CurrentScore];
        DiScore.text = index.ToString();
    }

    public void OnCreateGameRobot(GameObject obj)
    {
        SocketClient.Instance.CreateRoomRobot();
    }

    //签到
    private void SetSignStae()
    {
        Image item;
        item = signBtn.gameObject.GetComponent<Image>();
        item.color = new Color(0.5F, 0.5F, 0.5F, 255);
        signBtn.enabled = false;
        signTxt.gameObject.SetActive(true);
    }

    private void OnSingn(GameObject obj)
    {
        SocketClient.Instance.Singn();
    }

    private void OnSigninSuccess(Int64 flag)
    {
        SetSignDays(flag);
        SetSignStae();
    }

    private void SetSignDays(Int64 flag)
    {
        Image item;
        for (int i = 0; i < flag; i++)
        {
            item = signImgeList[i];
            item.color = new Color(0.5F, 0.5F, 0.5F, 255);
        }
    }

    private CreatRoomData _creatRoomdata;
    /// <summary>
    /// 绑定在控件里，别删
    /// </summary>
    public void OnChangeCreatRoomInfo()
    {
        int score = int.Parse(DiScore.text);
        CreatRoomData data = DealCreatRoomData();
        data.baseScore = score;
        ShowConsuma(data);
        _creatRoomdata = data;
    }

    public void OnCreateGame(GameObject obj)
    {
        OnChangeCreatRoomInfo();
        TableController.Instance.creatRoomInfo = _creatRoomdata;
        if (_creatRoomdata.needMoney>MainManager.Instance.playerSelfInfo.diamondNum)
        {
            NUMessageBox.Show("钻石不足！");
        }
        else
        {
            SocketClient.Instance.CreateRoom(_creatRoomdata.playerNum, _creatRoomdata.playState, _creatRoomdata.baseScore, _creatRoomdata.jushu, _creatRoomdata.payState);
        }      
    }

    private void SetCreatRoomState(CreatRoomData data,string name)
    {
        int index;
        List<string> strlist = new List<string> { "playerNum", "playState", "jushu", "payState" };
        string str ;
        List<int> scoreList = new List<int> { 8, 16 };
        for (int i = 0; i < strlist.Count; i++)
        {
            str = strlist[i];
            index = name.IndexOf(str);
            if (index>=0)
            {
                switch (str)
                {
                    case "playerNum":
                        data.playerNum = int.Parse(name.Replace(str, ""));
                        break;
                    case "playState":
                        data.playState = int.Parse(name.Replace(str, ""));
                        break;
                    case "jushu":
                        data.jushu = scoreList[int.Parse(name.Replace(str, ""))];
                        break;
                    case "payState":
                        data.payState = int.Parse(name.Replace(str, ""));
                        break;
                    default:
                        break;
                }
                break;
            }
        }  
    }

    private CreatRoomData DealCreatRoomData()
    {
        CreatRoomData data = new CreatRoomData();
        _choseList = new List<string>();
        string createGameInfo = "";
        var toggles = FindObjectsOfType<Toggle>();
        string name;
        if (toggles != null)
        {
            foreach (var toggle in toggles)
            {
                if (toggle.isOn)
                {
                    createGameInfo +=
                        toggle.gameObject.GetComponentInChildren<Text>().text;
                    _choseList.Add(toggle.gameObject.name);
                    name = toggle.gameObject.name;
                    SetCreatRoomState(data, name);
                }
            }
        }
        return data;
    }

    private void ShowConsuma(CreatRoomData data)
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
        data.needMoney = baseConsuma;
        consumeTxt.text = baseConsuma.ToString();
    }

    public void OnNumberBtnDown(Button btn)
    {
        int num = 0;
        if (int.TryParse(btn.name, out num))
        {
           
            if (RoomNumberText.text.Length < 6)
            {
                RoomNumberText.text += btn.name;
            }
        }
        if (btn.name == "Delete")
        {
            if (RoomNumberText.text.Length > 0)
            {
                RoomNumberText.text =
                RoomNumberText.text.Substring(0, RoomNumberText.text.Length - 1);
            }
        }

        if (btn.name == "Sure")
        {
            SocketClient.Instance.JoinRoom(Int64.Parse(RoomNumberText.text));
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void OnLoadHeadInfo()
    {
        GameObject ojb = GameObject.Find("DontDestroyOnLoad");
        HttpUtil Until = ojb.GetComponent<HttpUtil>();
        //string url = TableController.Instance.SelfInfo["headimg"] as string;
        string url = MainManager.Instance.playerSelfInfo.headUrl;
        string res;
        if (url !="")
        {
            res = Regex.Unescape(url);
            Until.DownloadPicture(res, LoadHeadComplete);
            loadHeadFlagImage();
        }  
    }

    private void LoadHeadComplete(Texture2D res,int index)
    {
        Sprite spr = Sprite.Create(res, new Rect(0, 0, res.width, res.height), Vector2.zero);
        headInfo.sprite = spr;
    }

    private void loadHeadFlagImage()
    {
        Int64 sexNum = (Int64)MainManager.Instance.playerSelfInfo.sex;
        Gender six = (Gender)sexNum;
        headSixImage.overrideSprite = Resources.Load("ui/" + six.ToString(), typeof(Sprite)) as Sprite;
    }

    public void PaySuccess(string flag)
    {
		Debug.Log ("PaySuccess flag:" + flag);
        if (flag == "1")
        {
         //   SocketClient.Instance.PlayerPaySuccess();
        }
        else
        {
          //  NUMessageBox.Show(flag);
        }
       
    }

    void OnDestroy()
    {
        EventDispatcher.GetInstance().MainEventManager.RemoveEventListener<Hashtable>(EventId.Server_PlayerInfo_Updata, OnSelfInfoUpdata);
        EventDispatcher.GetInstance().MainEventManager.RemoveEventListener<string>(EventId.Server_Pay_msg, OnPay);
        EventDispatcher.GetInstance().MainEventManager.RemoveEventListener<string>(EventId.Sever_Agent_bind, OnBind);
        EventDispatcher.GetInstance().MainEventManager.RemoveEventListener<Int64>(EventId.Server_Daily_Signin, OnSigninSuccess);
        EventDispatcher.GetInstance().MainEventManager.RemoveEventListener<ArrayList>(EventId.Sever_refresh_red_rank_broadcast, OnRefreshRank);
    }
 }