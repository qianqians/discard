using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts;
using GameCommon;
using System;
public class NUMatchGamePanel : MonoBehaviour {

    public List<Button> joinBtnList=null;
	// Use this for initialization
	void Start () {
        foreach (var btn in joinBtnList)
        {
            btn.onClick.AddListener(delegate ()
            {
                OnClcikBtn(btn);
            });
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnClcikBtn(Button btn)
    {
        SocketClient.Instance.JoinMatchRoom((Int64)GameRule.Laizi,(Int64)GameScore.Five_hundred);
    }
}
