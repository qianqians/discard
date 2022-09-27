using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class RecordItem : MonoBehaviour {

    public List<Text> infoList = null;
    public Text beginTimeTxt=null;
    public Text playTxt = null;
    public Text timesTxt = null;
    private List<string> playRuleList;
    public Button shareBtn = null;
    private string imgRes;
	// Use this for initialization
	void Start () {
        shareBtn.onClick.AddListener(delegate ()
        {
            OnshareImg();
        });
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnshareImg()
    {
        if (imgRes!="")
        {
            OnShareToWX(imgRes);
        }
    }

    private void OnShareToWX(string res)
    {
        #if UNITY_EDITOR

        #elif UNITY_STANDALONE_WIN
                
        #elif UNITY_ANDROID
                AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
                jo.Call("SendPicToWX", res);
        #elif UNITY_IPHONE
			UIAccountPanel.ShareByiOS(res);             
        #else

        #endif
    }

    public void Show(object[] arr,string res)
    {
        string nameStr;
        playRuleList = new List<string> { "无癞", "半癞", "一脚癞油", "油上油" };
        AccountData data;
        Text txt;
        List<AccountData> list = (List<AccountData>)arr[3];
        string time = (string)arr[0];
        int rule = int.Parse((string)arr[1]);
        for (int i = 0; i < list.Count; i++)
        {
            data = list[i];
            txt = infoList[i];
            nameStr = data.wechat_name;
            if (nameStr.Length>13)
            {
                nameStr = nameStr.Substring(0, 12);
            }
            txt.text = nameStr + "\n" + data.ID + "\n" + data.score;
        }

        if (list.Count<4)
        {
            infoList[3].gameObject.SetActive(false);
        }
        if (list.Count < 3)
        {
            infoList[2].gameObject.SetActive(false);
        }

        DateTime dt = DateTime.Parse(DateTime.UtcNow.ToString("1970-01-01 00:00:00")).AddMilliseconds(Int64.Parse(time));
        DateTime localdt = dt.ToLocalTime();
        string result = localdt.ToString("yyyy-MM-dd HH:mm");
        beginTimeTxt.text = result;

        playTxt.text = playRuleList[rule-1];
        timesTxt.text = (string)arr[2];
        imgRes = res;
    }
}
