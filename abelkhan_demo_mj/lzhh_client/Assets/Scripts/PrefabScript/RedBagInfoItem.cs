using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;
using TinyFrameWork;
using Assets.Scripts;

public class RedBagInfoItem : MonoBehaviour {

    public Text wordTxt;
    public string redBagID;
    public bool isSend;
	// Use this for initialization
	void Start () {
        //EventDispatcher.GetInstance().MainEventManager.AddEventListener<Hashtable>(EventId.Sever_get_red_Player_Info, OnShowRedBagInfo);
        //SocketClient.Instance.GetRedBagInfoById(redBagID);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ShowWord(string str)
    {
        wordTxt.text = str;
    }

    private void OnShowRedBagInfo(Hashtable info)
    {
        //string tirms;
        //Int64 money = 0;
       // RedBagRobPlayerItem data;
       // ArrayList robList = (ArrayList)info["snatch_red_player_info"];
       // RedBagRobPlayerItem item;
        //for (int i = 0; i < robList.Count; i++)
        //{
        //    data = new RedBagRobPlayerItem((Hashtable)robList[i]);
        //    if (data.unionid == MainManager.Instance.playerSelfInfo.unionID)
        //    {
        //        money = (Int64)info["money"];
        //        break;
        //    }
        //}

        if (isSend)
        {
            wordTxt.text = (string)info["red_word"];
        }
        else
        {
            wordTxt.text = (string)info["red_word"];
          //  wordTxt.text = "抢了金币"+ money;
        }
        EventDispatcher.GetInstance().MainEventManager.RemoveEventListener<Hashtable>(EventId.Sever_get_red_Player_Info, OnShowRedBagInfo);
        //GameObject bag;
        //Text txt;
        //RedBagRobPlayerItem item;
        //ArrayList robList = (ArrayList)info["snatch_red_player_info"];
        //RedInfoView.SetActive(true);
        //for (int i = 0; i < RedInfoContinaer.transform.childCount; i++)
        //{
        //    GameObject go = RedInfoContinaer.transform.GetChild(i).gameObject;
        //    Destroy(go);
        //}
        //redBagInfoMoneyTxt.text = (Int64)info["red_gold"] + "";
        //redBagInfoCountTxt.text = (Int64)info["red_count"] + "";
        //for (int i = 0; i < robList.Count; i++)
        //{
        //    bag = GameObject.Instantiate(Resources.Load("UIPrefab/RedRobItem") as GameObject);
        //    bag.transform.SetParent(RedInfoContinaer.transform, false);
        //    txt = bag.GetComponent<Text>();
        //    item = new RedBagRobPlayerItem((Hashtable)robList[i]);
        //    txt.text = item.name + "抢到" + item.money;
        //}
    }
}
