using System.Collections;
using TinyFrameWork;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts;
using System;
using System.Collections.Generic;
public class UIRedbagpanel : MonoBehaviour {

    public Button sendBtn = null;
    public GameObject redBagContainer = null;
    public InputField redBagMoney = null;
    public InputField redBagCount = null;
    public InputField sloganTxt =null;
//    public InputField redBagMoney = null
    private List<GameObject> _redPrefabList;
    // Use this for initialization
    private List<RedBagBaseInfo> _sendRedBagList;
    public Text sendListTxt = null;

    public GameObject selfRedBagListView = null;
    public GameObject sendContinaer = null;
    public GameObject snatchContinaer = null;

    public Button openRecordBtn = null;

    public GameObject RedInfoView = null;
    public GameObject RedInfoContinaer = null;

    public Text redBagInfoMoneyTxt = null;
    public Text redBagInfoCountTxt = null;

    private bool isClickBtn;

    //   private List<RedBagDataForRecord> redBagsendRecordList;

    private List<GameObject> _redBagsendRecordList;
    public Text sendRedMoneyTotalTxt = null;
    //   private 
    void Start () {
        _redPrefabList = new List<GameObject>();
        _sendRedBagList = new List<RedBagBaseInfo>();
        _redBagsendRecordList = new List<GameObject>();
        EventDispatcher.GetInstance().MainEventManager.AddEventListener<ArrayList>(EventId.Sever_Can_Rob_List, OnRefreshRedbagList);
        EventDispatcher.GetInstance().MainEventManager.AddEventListener<RedBagBaseInfo>(EventId.Sever_Get_Red_Bag, OnGetRedBagMsg);
        EventDispatcher.GetInstance().MainEventManager.AddEventListener<Hashtable>(EventId.Server_PlayerInfo_Updata, OnSelfInfoUpdata);
        EventDispatcher.GetInstance().MainEventManager.AddEventListener<Hashtable>(EventId.Sever_get_red_Player_Info, OnShowRedBagInfo);
        SocketClient.Instance.RequestRefreshList();
        sendBtn.onClick.AddListener(delegate ()
        {
            SendRedBag();
        });
        selfRedBagListView.SetActive(false);
        openRecordBtn.onClick.AddListener(delegate ()
        {
            selfRedBagListView.SetActive(true);
        });

        selfRedBagListView.GetComponent<Button>().onClick.AddListener(delegate ()
        {
            selfRedBagListView.SetActive(false);
        });
        RedInfoView.SetActive(false);
        RedInfoView.GetComponent<Button>().onClick.AddListener(delegate ()
         {
             RedInfoView.SetActive(false);
         });

        ShowRedBagRecord(MainManager.Instance.playerSelfInfo.sendRedIDList,sendContinaer, "send_id", "send_time");
        ShowRedBagRecord(MainManager.Instance.playerSelfInfo.snatchRedIDList, snatchContinaer, "snatch_id", "snatch_time");
        sendRedMoneyTotalTxt.text = MainManager.Instance.playerSelfInfo.redpacketSendTotal.ToString();
    }

    void OnDestroy()
    {
        EventDispatcher.GetInstance().MainEventManager.RemoveEventListener<ArrayList>(EventId.Sever_Can_Rob_List, OnRefreshRedbagList);
        EventDispatcher.GetInstance().MainEventManager.RemoveEventListener<RedBagBaseInfo>(EventId.Sever_Get_Red_Bag, OnGetRedBagMsg);
        EventDispatcher.GetInstance().MainEventManager.RemoveEventListener<Hashtable>(EventId.Server_PlayerInfo_Updata, OnSelfInfoUpdata);
        EventDispatcher.GetInstance().MainEventManager.RemoveEventListener<Hashtable>(EventId.Sever_get_red_Player_Info, OnShowRedBagInfo);
    }

    private void SendRedBag()
    {
        int money = (int)MainManager.Instance.playerSelfInfo.goldNum;
        int sendMoney = int.Parse(redBagMoney.text);
        int sendCount = int.Parse(redBagCount.text);
        string msg;
        if (sendMoney> money)
        {
            NUMessageBox.Show("金额不足咧！");
        }
        else if (sendMoney < 30000|| sendMoney > 500000 || sendCount<10 || sendCount>50)
        {
            msg = "1:红包金额必须大于3万小于50万\n2:红包个数必须大于10个小于20个";
            NUMessageBox.Show(msg);
        }
        else
        {
            NUMessageBox.Show("你是否确定发红包!", SureSendRedBag);        
        }
    }

    private void SureSendRedBag(NUMessageBox.CallbackType cbt)
    {
        SocketClient.Instance.SendRedBag(int.Parse(redBagMoney.text), int.Parse(redBagCount.text), sloganTxt.text);
    }

	// Update is called once per frame
	void Update () {
     
    }

    public void Init()
    {

    }

    private void OnGetRedBagMsg(RedBagBaseInfo data)
    {
        _sendRedBagList.Add(data);
        ShowSendListTxt();
    }

    private void ShowSendListTxt()
    {
        string str ="";
        RedBagBaseInfo info;
        for (int i = 0; i < _sendRedBagList.Count; i++)
        {
            info = _sendRedBagList[i];
            str += "<Color=#E7B40F><size=20>" + info.sendName + "</size></Color>" + "发了一个金额为" + "<Color=#E7EA09><size=25>" + info.redGold + "</size></Color>" + "的红包\n";         
        }
        sendListTxt.text = str;
    }

    private void OnRefreshRedbagList(ArrayList list)
    {
        GameObject bag;
        RedBagScript code;
        for (int i = _redPrefabList.Count-1; i >=0; i--)
        {
            GameObject go = _redPrefabList[i];           
            Destroy(go);
            _redPrefabList.RemoveAt(i);
        }

        for (int i = 0; i < list.Count; i++)
        {
            bag = GameObject.Instantiate(Resources.Load("UIPrefab/RedBag") as GameObject);
            bag.transform.SetParent(redBagContainer.transform, false); 
            //bag = _redPrefabList[i];
            code = bag.GetComponent<RedBagScript>();
            code.slogan.text = (string)(list[i] as Hashtable)["red_word"];
            code.red_id = (string)(list[i] as Hashtable)["id"];
            code.SetRedBagState(0);
            _redPrefabList.Add(bag);
        }

        SetRedBagStateBySnatchList(MainManager.Instance.playerSelfInfo.snatchRedIDList);
    }


    private void OnSelfInfoUpdata(Hashtable info)
    {
        ArrayList list = (ArrayList)info["snatch_red_id_time"];
        sendRedMoneyTotalTxt.text = (Int64)info["redpackets_send_perday"] +"";
        SetRedBagStateBySnatchList(list);
        ShowRedBagRecord(MainManager.Instance.playerSelfInfo.sendRedIDList, sendContinaer, "send_id", "send_time");
        ShowRedBagRecord(MainManager.Instance.playerSelfInfo.snatchRedIDList, snatchContinaer, "snatch_id", "snatch_time");
    }

    private void SetRedBagStateBySnatchList(ArrayList list)
    {
        string redID;
        string tempRedID;
        GameObject bag;
        RedBagScript code;
        Hashtable table;
        for (int i = 0; i < list.Count; i++)
        {
            table = (Hashtable)list[i];
            redID = (string)table["snatch_id"];
            for (int j = 0; j < _redPrefabList.Count; j++)
            {
                bag = _redPrefabList[j];
                code = bag.GetComponent<RedBagScript>();
                tempRedID = code.red_id;
                if (tempRedID == redID)
                {
                    code.SetRedBagState(1);
                }
            }
        }
    }

    private void ShowRedBagRecord(ArrayList redIDList, GameObject container,string redID,string timeID)
    {
        GameObject bag;
        RedBagInfoItem code;
        bool flag;
        Hashtable redBagTbale;
        Int64 time;
        List<Hashtable> tempList = new List<Hashtable>();

        redIDList.Reverse();
        for (int i = 0; i < redIDList.Count; i++)
        {
            flag = false;
            for (int j = 0; j < container.transform.childCount; j++)
            {
                bag = container.transform.GetChild(j).gameObject;
                code = bag.GetComponent<RedBagInfoItem>();
                redBagTbale = (Hashtable)redIDList[i];
                if (code.redBagID == (string)redBagTbale[redID])
                {
                    flag = true;
                    break;
                }
            }
            if (!flag)
            {
                tempList.Add((Hashtable)redIDList[i]);
            }          
        }
      //  tempList.Reverse();
        foreach (Hashtable item in tempList)
        {
            time = (Int64)item[timeID];
            DateTime dt = DateTime.Parse(DateTime.UtcNow.ToString("1970-01-01 00:00:00")).AddMilliseconds(time);
            DateTime localdt = dt.ToLocalTime();
            string result = localdt.ToString("yyyy-MM-dd HH:mm:ss");
            bag = GameObject.Instantiate(Resources.Load("UIPrefab/RedbagInfo") as GameObject);
            bag.transform.SetParent(container.transform, false);
            code = bag.GetComponent<RedBagInfoItem>();
            code.redBagID = (string)item[redID];
            code.ShowWord(result);
            bag.GetComponent<Button>().onClick.AddListener(delegate ()
            {
                SocketClient.Instance.GetRedBagInfoById((string)item[redID]);
            });
        }
        //ShowRobRedBagRecord();
    }

    private void OnShowRedBagInfo(Hashtable info)
    {
        ArrayList robList = (ArrayList)info["snatch_red_player_info"];      
        GameObject bag;
        Text txt;
        RedBagRobPlayerItem item;          
        RedInfoView.SetActive(true);
        for (int i = 0; i < RedInfoContinaer.transform.childCount; i++)
        {
            GameObject go = RedInfoContinaer.transform.GetChild(i).gameObject;
            Destroy(go);
        }
        redBagInfoMoneyTxt.text = (Int64)info["red_gold"] + "";
        redBagInfoCountTxt.text = (Int64)info["red_count"] + "";
        for (int i = 0; i < robList.Count; i++)
        {
            bag = GameObject.Instantiate(Resources.Load("UIPrefab/RedRobItem") as GameObject);
            bag.transform.SetParent(RedInfoContinaer.transform, false);
            txt = bag.GetComponent<Text>();
            item = new RedBagRobPlayerItem((Hashtable)robList[i]);
            txt.text = item.name + "抢到" + item.money;
        }
        isClickBtn = false;          
    }
}

//public class RedBagDataForRecord
//{
//    public string name;
//    public Int64 money;
//    public string word;
//    public List<RedBagRobPlayerItem> robPlayeInfoList;
//    public string redID;
//    private ArrayList robList;
//    public RedBagDataForRecord(Hashtable info)
//    {
//        name = (string)info["nickname"];
//        money = (Int64)info["money"];
//        robPlayeInfoList = new List<RedBagRobPlayerItem>();
//        robList = (ArrayList)info["snatch_red_player_info"];
//    }
//}

public class RedBagRobPlayerItem
{
    public string name;
    public Int64 money;
    public string unionid;
    public RedBagRobPlayerItem(Hashtable info)
    {
        name = (string)info["nickname"];
        money= (Int64)info["money"];
        unionid = (string)info["unionid"];
    }
}

public class RedBagBaseInfo
{
    public Int64 redGold;
    public Int64 redCount;
    public string sendName;
    public string redWord;
    public string redID;
    public RedBagBaseInfo(Int64 gold, Int64 red_count, string name, string red_id,string word)
    {
        redGold = gold;
        redCount = red_count;
        sendName = name;
        redWord = word;
        redID = red_id;
    } 
}

