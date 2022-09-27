using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
//using UnityEditor;
using GameCommon;
public class ViewTingpaiPanel : MonoBehaviour {

    public GameObject Container =null;
    public GameObject totalContainer = null;
    private GameObject tingPrefab;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetTingCard(List<Int64> list)
    {
        Image mjImage;
        GameObject mj;
        GameObject mjFront;
        mjCards mjName;
        float width = 32f;
        PlayerData selfData = TableController.Instance.PlayrInfo.GetSelfData();
        int handShowNum = selfData.RevealCardList.Count;
        float offex = 30;
        totalContainer.transform.localPosition = new Vector3(offex* handShowNum, 20,0);

        for (int j = 0; j < Container.transform.childCount; j++)
        {
            Destroy(Container.transform.GetChild(j).gameObject);
        }

        if (tingPrefab == null)
        {
            tingPrefab = Resources.Load("UIPrefab/TingCard") as GameObject;
        }
        for (int i = 0; i < list.Count; i++)
        {
            mj = GameObject.Instantiate(tingPrefab);
            mj.transform.parent = Container.transform;
            mj.transform.localScale = new Vector3(1,1,1);
            width = mj.GetComponent<RectTransform>().sizeDelta.x;
            mjFront = mj.transform.FindChild("mj").gameObject;
            mjImage = mjFront.GetComponent<Image>();
            mjName = (mjCards)list[i];
            mjImage.overrideSprite = Resources.Load("mjFront/" + mjName.ToString(), typeof(Sprite)) as Sprite;
            mj.transform.localPosition = new Vector3(width * i,0,0);
        }
    }
}
