using Assets.Scripts;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using TinyFrameWork;
public class NUTaskPanel : MonoBehaviour {

    public Image barImage;
    private float i = 0.0f;
    private float Gtime = 0.0f;
    public List<Button> getBtnList = null;
    public List<Text> timesList = null;
    public List<Image> boxList = null;
    // Use this for initialization
    void Start () {
        barImage.fillAmount = i;
        foreach (Button btn in getBtnList)
        {
            btn.onClick.AddListener(delegate ()
            {
                OnClickRewardBtn(btn);
            });
        }
        ShowTimes();
        SetBtnState();
        EventDispatcher.GetInstance().MainEventManager.AddEventListener<Hashtable>(EventId.Server_PlayerInfo_Updata, OnSelfInfoUpdata);
    }
	
    void OnDestroy()
    {      
        EventDispatcher.GetInstance().MainEventManager.RemoveEventListener<Hashtable>(EventId.Server_PlayerInfo_Updata, OnSelfInfoUpdata);
    }
    // Update is called once per frame
    void Update () {
        //if (i<1)
        //{
        //    i += 0.01f;
        //    barImage.fillAmount = i;
        //}
        //else
        //{
        //    i = 0.0f;
        //} 
    }

    private void OnSelfInfoUpdata(Hashtable info)
    {
        SetBtnState();
    }

    private void OnClickRewardBtn(Button btn)
    {
        int indx = getBtnList.IndexOf(btn);
        SocketClient.Instance.GetReward((indx+1).ToString());
    }

    private void ShowTimes()
    {
        List<int> times = new List<int> {10,20,30,10,20 };
        Int64 tiems = MainManager.Instance.playerSelfInfo.taskGameCount;
        Int64 winTimes = MainManager.Instance.playerSelfInfo.taskVictoryCount;
        Text txt;
        Int64 temp;
        for (int i = 0; i < timesList.Count; i++)
        {
            txt = timesList[i];
            if (i==3||i==4)
            {
                if (winTimes > times[i])
                {
                    temp = times[i];
                }
                else
                {
                    temp = winTimes;
                }
                txt.text = temp.ToString() + "/" + times[i];
            }
            else
            {
                if (tiems> times[i])
                {
                    temp = times[i];
                }
                else
                {
                    temp = tiems;
                }
                txt.text = temp.ToString() + "/" + times[i];
            }        
        }
    }

    private void SetBtnState()
    {
        string res = "";
        ArrayList getRewardList = MainManager.Instance.playerSelfInfo.hasGetReward;
        Int64 state;
        Button btn;
        Image img;
        for (int i = 0; i < getRewardList.Count; i++)
        {
            state = (Int64)getRewardList[i];
            if (state != 0)
            {
                btn = getBtnList[i];
                btn.gameObject.SetActive(false);
                img = boxList[i];
                res = "box" + (i + 1).ToString() + "_secert";
                img.overrideSprite = Resources.Load("ui/" + res, typeof(Sprite)) as Sprite;
            }
        }
    }
}
