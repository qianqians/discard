using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;
using UnityEngine;
using System.Text.RegularExpressions;
public class PlayerAccountGrid
{
    private Image _headMc;
    private Text _nameTxt;
    private Text _IDTxt;
    private Text _ScoreTxt;
    private GameObject gridGameObject;
    public PlayerAccountGrid(GameObject grid)
    {
        gridGameObject = grid;
        grid.SetActive(false);
        _headMc = grid.transform.FindChild("Head").gameObject.GetComponent<Image>();
        _nameTxt = grid.transform.FindChild("Name").gameObject.GetComponent<Text>();
        _IDTxt = grid.transform.FindChild("ID").gameObject.GetComponent<Text>();
        _ScoreTxt = grid.transform.FindChild("Score").gameObject.GetComponent<Text>();
    }

    public void SetInfo(AccountData data)
    {
        string scoreNum= data.score.ToString();
        gridGameObject.SetActive(true);
        _nameTxt.text = data.wechat_name;
        if (data.score<0)
        {
            _ScoreTxt.text = "<color=#00FF00>"+ scoreNum+" </color>";
}
        else
        {
            _ScoreTxt.text = scoreNum;
        }
        
        _IDTxt.text = data.ID.ToString();
        OnLoadHeadInfo(data.headimg);
    }

    private void OnLoadHeadInfo(string url)
    {
        GameObject ojb = GameObject.Find("DontDestroyOnLoad");
        HttpUtil Until = ojb.GetComponent<HttpUtil>();
       // string url = TableController.Instance.SelfInfo["headimg"] as string;
        string s2 = Regex.Unescape(url);
        Until.DownloadPicture(s2, LoadHeadSuccessCallBack);
    }

    private void LoadHeadSuccessCallBack(Texture2D res, int index)
    {
        Sprite spr = Sprite.Create(res, new Rect(0, 0, res.width, res.height), Vector2.zero);
        _headMc.overrideSprite = spr;
    }
}

