using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TinyFrameWork;
using UnityEngine;
using UnityEngine.UI;
public class NULobbyPanel : MonoBehaviour
{

    public Button OpenYijiaoPanel = null;
    public GameObject YijiaoPanel = null;
    public Button butRedBag;
    public Button butcPay;
    public List<GameObject> rankListRed;
    public List<GameObject> rankListPay;
    public GameObject arrRank;
    public GameObject scrollView = null;

    ArrayList arrRad;
    ArrayList arrPay;
    Sprite redBagImg;
    Sprite payImg;
    Sprite kongbai;

    int butkey = 0;
    public GameObject rankPrefab;
    // Use this for initialization
    void Start()
    {

        OpenYijiaoPanel.onClick.AddListener(delegate ()
        {

        });

        rankPrefab = Resources.Load("RankingList/RankFigures") as GameObject;
        redBagImg = Resources.Load("RankingList/redbag", typeof(Sprite)) as Sprite;
        kongbai = Resources.Load("RankingList/kongbai", typeof(Sprite)) as Sprite;
        payImg = Resources.Load("RankingList/pay", typeof(Sprite)) as Sprite;

        EventDispatcher.GetInstance().MainEventManager.AddEventListener<ArrayList, Int64>(EventId.Lobby_RankList_Red_Bag, RankList);
        EventDispatcher.GetInstance().MainEventManager.AddEventListener<ArrayList, Int64>(EventId.Lobby_RankList_Pay, RankList);

       

        butRedBag.onClick.AddListener(delegate ()
        {
            butkey = 0;
            if (arrRad != null)
            {             
                RankListOnRedBag(arrRad);
            }
        });

        butcPay.onClick.AddListener(delegate ()
        {
            butkey = 1;
            if (arrPay != null)
            {
                RankListOnPay(arrPay);
            }
        });
        SocketClient.Instance.GetLoginType();
    }

    void Update()
    {

    }
    void OnDestroy()
    {
        EventDispatcher.GetInstance().MainEventManager.RemoveEventListener<ArrayList, Int64>(EventId.Lobby_RankList_Red_Bag, RankList);
        EventDispatcher.GetInstance().MainEventManager.RemoveEventListener<ArrayList, Int64>(EventId.Lobby_RankList_Pay, RankList);
    }



    public void OnOpenLobbyPanel(GameObject obj)
    {
        this.gameObject.SetActive(false);
        YijiaoPanel.SetActive(true);
    }

    public void RankList(ArrayList rankLisData, Int64 butKey)
    {
        //-----转存数据-----
        if (butKey == 0)
        {
            arrRad = rankLisData;
        }
        else if (butKey == 1)
        {
            arrPay = rankLisData;
        }
        //------------------

        //----------打印数据----------
        if (butKey == butkey)
        {
            if (0 == butKey)
            {
                RankListOnRedBag(arrRad);
            }
            else if (1 == butKey)
            {
                RankListOnPay(arrPay);
            }
        }
        //-----------------------------
    }

    /// <summary>
    /// 土豪榜
    /// </summary>
    /// <param name="rankLisData"></param>
    public void RankListOnPay(ArrayList rankLisData)
    {
        if (payImg != null)
        {
            scrollView.transform.FindChild("RankBackGround").gameObject.GetComponent<Image>().overrideSprite = payImg;
        }

        GameObject item;
        rankListPay.Clear();

        for (int i = 0; i < arrRank.transform.childCount; i++)
        {
            GameObject go = arrRank.transform.GetChild(i).gameObject;
            Destroy(go);
        }

        GameObject ojb = GameObject.Find("DontDestroyOnLoad");
        HttpUtil Until = ojb.GetComponent<HttpUtil>();
        string res;
        Hashtable newhas;


        for (int i = 0; i < 9; i++)
        {
            if (rankLisData.Count>i)
            {
                item = GameObject.Instantiate(rankPrefab);

                newhas = (Hashtable)rankLisData[i];
                res = (string)newhas["headimg"];

                Sprite spr = Resources.Load("Sprite/joinRoom/btn0" + (i + 1), typeof(Sprite)) as Sprite;
                item.transform.FindChild("TopNum").gameObject.GetComponent<Image>().overrideSprite = spr;
                Debug.Log("" + i);
                item.transform.FindChild("Name").gameObject.GetComponent<Text>().text = (string)newhas["nickname"];

                item.transform.FindChild("Points").gameObject.GetComponent<Text>().text = (Int64)newhas["pay_total"]/100+".00";
                rankListPay.Add(item);
                try
                {
                    Until.DownloadPicture(res, HeadInfoOnPay, i);
                }
                catch (Exception e)
                {

                    Debug.Log(e.Message);
                }

                item.transform.SetParent(arrRank.transform, false);
            }           
        }
    }


    /// <summary>
    /// 红包榜
    /// </summary>
    /// <param name="rankLisData"></param>
    public void RankListOnRedBag(ArrayList rankLisData)
    {

        if (redBagImg != null)
        {
            scrollView.transform.FindChild("RankBackGround").gameObject.GetComponent<Image>().overrideSprite = redBagImg;
        }

        GameObject item;
        rankListRed.Clear();

        for (int i = 0; i < arrRank.transform.childCount; i++)
        {
            GameObject go = arrRank.transform.GetChild(i).gameObject;
            Destroy(go);       
        }

        GameObject ojb = GameObject.Find("DontDestroyOnLoad");
        HttpUtil Until = ojb.GetComponent<HttpUtil>();
        string res;
        Hashtable newhas;


        for (int i=0;i<9;i++)
        {

            if (rankLisData.Count>i)
            {
                item = GameObject.Instantiate(rankPrefab);

                newhas = (Hashtable)rankLisData[i];
                res = (string)newhas["headimg"];

                Sprite spr = Resources.Load("Sprite/joinRoom/btn0" + (i + 1), typeof(Sprite)) as Sprite;
                item.transform.FindChild("TopNum").gameObject.GetComponent<Image>().overrideSprite = spr;
                Debug.Log("" + i);
                item.transform.FindChild("Name").gameObject.GetComponent<Text>().text = (string)newhas["nickname"];

                item.transform.FindChild("Points").gameObject.GetComponent<Text>().text =(Int64)newhas["redpackets_send_perday"]+"";
                rankListRed.Add(item);
                try
                {
                    Until.DownloadPicture(res, HeadInfoOnRedBag, i);
                }
                catch (Exception e)
                {

                    Debug.Log(e.Message);
                }

                item.transform.SetParent(arrRank.transform, false);
            }
          
        }
    }


    private void HeadInfoOnRedBag(Texture2D res, int index)
    {
        try
        {
            Sprite spr = Sprite.Create(res, new Rect(0, 0, res.width, res.height), Vector2.zero);
            rankListRed[index].transform.FindChild("Avatar").gameObject.GetComponent<Image>().overrideSprite = spr;
        }
        catch 
        {
        }
            
           

    }
    private void HeadInfoOnPay(Texture2D res, int index)
    {
        try
        {
            Sprite spr = Sprite.Create(res, new Rect(0, 0, res.width, res.height), Vector2.zero);
            rankListPay[index].transform.FindChild("Avatar").gameObject.GetComponent<Image>().overrideSprite = spr;
        }
        catch
        {
        }
            
            
        
    }


}
