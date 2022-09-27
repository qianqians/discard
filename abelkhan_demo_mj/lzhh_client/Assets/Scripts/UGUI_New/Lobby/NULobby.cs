using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class NULobby : MonoBehaviour {

    public static List<GameObject> rankList;


    public static  void RankList(ArrayList rankLisData)
    {
        Debug.Log(""+ rankList.Count);
        for (int i = 0; i < rankList.Count; i++)
        {
            Hashtable newhas = (Hashtable)rankLisData[i];


            rankList[i].transform.FindChild("Name").gameObject.GetComponent<Text>().text = (string)newhas["nickname"];
            rankList[i].transform.FindChild("Points").gameObject.GetComponent<Text>().text = (string)newhas["redpackets_send_perday"];
        }
        OnLoadHeadInfo(rankLisData);
    }

    static int i = 0;
    private static void OnLoadHeadInfo(ArrayList rankLisData)
    {
        GameObject ojb = GameObject.Find("DontDestroyOnLoad");
        HttpUtil Until = ojb.GetComponent<HttpUtil>();

        while (i<9)
        {
            string url = ((Hashtable)(rankLisData[i]))["headimg"] + "";
            string res;

            if (url != "")
            {
                res = Regex.Unescape(url);
                Until.DownloadPicture(res, HeadInfo, i);
            }
            i++;
        }      
    }

    private static void HeadInfo(Texture2D res, int index)
    {
        Sprite spr = Sprite.Create(res, new Rect(0, 0, res.width, res.height), Vector2.zero);
        rankList[i].transform.FindChild("Avatar").gameObject.GetComponent<Image>().overrideSprite = spr;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    

}
