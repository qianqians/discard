using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Assets.Scripts;
using TinyFrameWork;
using System.Runtime.InteropServices;
public class UIAccountPanel : MonoBehaviour {

    public List<GameObject> playerView = null;
    public Button closeBtn = null;
    private List<PlayerAccountGrid> gridList;
    public Text roomIDTxt = null;
    public Text playRuleTxt = null;
    public Text baseScoreTxt = null;
    public Text countTxt = null;
    public Text beginTxt = null;
    public Button fenxiangBtn = null;
    private bool isShotComplete;
    private string accountImgRes;
	#if UNITY_IPHONE
	[DllImport("__Internal")]
	private static extern void ShareByIos(string title,string desc,string url);
	#endif // UNITY_IPHONE

	static public void ShareByiOS(string res)
	{
		#if UNITY_IPHONE
			Debug.Log("OnShareToWX");
			ShareByIos("测试分享标题", "测试分享内容", res);             
		#endif
	}

    void Start () {

        closeBtn.onClick.AddListener(delegate ()
        {
            OnClickQuiteBtn(closeBtn);
        });
        fenxiangBtn.onClick.AddListener(delegate ()
        {
				Debug.Log("fenxiangBtn.onClick.AddListener");
            Fenxiang(fenxiangBtn);
        });
        ShowRoomRuleInfo();
    }

    private void Fenxiang(Button btn)
    {
        if (isShotComplete)
        {
            OnShareToWX(accountImgRes);
        }
        else
        {
            NUMessageBox.Show("正在保存战绩！请稍后");
        }
    }

    //private void ScreenShotComplete(Texture2D res,string imgRes)
    //{
    //    OnShareToWX(accountImgRes);
    //}

    private void OnShareToWX(string res)
    {
#if UNITY_EDITOR

#elif UNITY_STANDALONE_WIN
                
#elif UNITY_ANDROID
                AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
                jo.Call("SendPicToWX", res);
#elif UNITY_IPHONE
		ShareByIos("测试分享标题", "测试分享内容", res); 
#else

#endif
    }

    private void ShowRoomRuleInfo()
    {
        List<string>  playRuleList = new List<string> { "无癞", "半癞", "一脚癞油", "油上油" };
        CreatRoomData data = TableController.Instance.creatRoomInfo;
        string times = TableController.Instance.playCount.ToString() + "/" + TableController.Instance.creatRoomInfo.jushu.ToString();
        roomIDTxt.text = SocketClient.Instance.roomID.ToString();
        baseScoreTxt.text = TableController.Instance.creatRoomInfo.baseScore.ToString();
        countTxt.text = times;
    //    playRuleTxt.text = data.playerNum+"人/"+playRuleList[TableController.Instance.creatRoomInfo.playState - 1];
        
        DateTime dt = DateTime.Parse(DateTime.UtcNow.ToString("1970-01-01 00:00:00")).AddMilliseconds(data.roomBeginTime);
        DateTime localdt = dt.ToLocalTime();
        string result = localdt.ToString("yyyy-MM-dd HH:mm");
        beginTxt.text = result;


    }

    private void OnClickQuiteBtn(Button btn)
    {
        // SceneManager.LoadScene("NUMainWindow
        MainManager.Instance.dontDestroyOnLoad.SetLoading(true);
        this.gameObject.SetActive(false);
        MainManager.Instance.dontDestroyOnLoad.StartLoadingScene("NUMainWindow", RoutineJoinAndExitGameRoom);
    }

    private void RoutineJoinAndExitGameRoom()
    {     
        MainManager.Instance.dontDestroyOnLoad.SetLoading(false);
    }
    // Update is called once per frame
    void Update () {
		
	}

    private void SsetGrid()
    {
        GameObject item;
        PlayerAccountGrid grid;
        gridList = new List<PlayerAccountGrid>();
        for (int i = 0; i < playerView.Count; i++)
        {
            item = playerView[i];
            grid = new PlayerAccountGrid(item);
            gridList.Add(grid);
        }
    }

    public void SetPlayerResult(List<AccountData> list)
    {
        PlayerAccountGrid grid;
        AccountData data;
        int imgResIndex;
        if (gridList==null)
        {
            SsetGrid();
        }
        for (int i = 0; i < list.Count; i++)
        {
            data = list[i];
            grid = gridList[i];
            grid.SetInfo(data);
        }

        bool flag = false;
        string[] keysArr = new string[] {"record0", "record1", "record2" };
        string[] imgResKeysArr = new string[] { "recordimg0", "recordimg1", "recordimg2" };
        int rule = TableController.Instance.creatRoomInfo.playState;
        string times = TableController.Instance.playCount.ToString()+"/"+ TableController.Instance.creatRoomInfo.jushu.ToString();
        string str = WarRecordAccessTool.StoreDateforString(list,TableController.Instance.creatRoomInfo.roomBeginTime, rule, times);
        for (int i = 0; i < keysArr.Length; i++)
        {
            if (!PlayerPrefs.HasKey(keysArr[i]))
            {
                PlayerPrefs.SetString(keysArr[i], str);
                KeepAccountImage(imgResKeysArr[i],i);
                PlayerPrefs.SetInt("recordimg", i);
                flag = true;      
                break;
            }
        }
        if (!flag)
        {          
            for (int i = 0; i < keysArr.Length-1; i++)
            {
                if (PlayerPrefs.HasKey(keysArr[i+1]))
                {
                    PlayerPrefs.SetString(keysArr[i], PlayerPrefs.GetString(keysArr[i + 1]));
                    if (PlayerPrefs.HasKey(imgResKeysArr[i + 1]))
                    {
                        PlayerPrefs.SetString(imgResKeysArr[i], PlayerPrefs.GetString(imgResKeysArr[i + 1]));
                    }                 
                }        
            }
            PlayerPrefs.SetString(keysArr[2], str);
            if (PlayerPrefs.HasKey("recordimg"))
            {
                imgResIndex = PlayerPrefs.GetInt("recordimg");
                imgResIndex++;
                if (imgResIndex>=3)
                {
                    imgResIndex = 0;
                }
            }
            else
            {
                imgResIndex = 0;            
            }
            PlayerPrefs.SetInt("recordimg", imgResIndex);
            KeepAccountImage(imgResKeysArr[2], imgResIndex);
        }     
    }

    private string imgKey;
    private void KeepAccountImage(string key,int index)
    {
        GameObject obj = this.gameObject.transform.FindChild("bg").gameObject;
        Vector3[] corners = new Vector3[4];
        obj.GetComponent<RectTransform>().GetWorldCorners(corners);
        Vector3 pos1 = corners[0];
        Vector3 pos = corners[1];
        Rect rct = obj.GetComponent<RectTransform>().rect;
        Rect sreenshotRect = new Rect(pos1.x, pos1.y, corners[3].x - pos.x, pos.y - corners[3].y);
        string screenshotUrl ;
        imgKey = key;
        screenshotUrl = Application.persistentDataPath + "/recordimg" + index.ToString()+ ".png";
        MainManager.Instance.dontDestroyOnLoad.BeginShot(CompleteCallBack, sreenshotRect, screenshotUrl);
    }

    private void CompleteCallBack(Texture2D res,string imgRes)
    {
        isShotComplete = true;
        // string screenshotUrl = Application.persistentDataPath + "/onMobileSavedScreen.png";
        accountImgRes = imgRes;
        PlayerPrefs.SetString(imgKey, imgRes);
    }

    void OnDestroy()
    {
        gridList = null;
        isShotComplete = false;
        closeBtn.onClick.RemoveListener(delegate ()
        {
            OnClickQuiteBtn(closeBtn);
        });
        fenxiangBtn.onClick.RemoveListener(delegate ()
        {
            Fenxiang(fenxiangBtn);
        });
    }
}
