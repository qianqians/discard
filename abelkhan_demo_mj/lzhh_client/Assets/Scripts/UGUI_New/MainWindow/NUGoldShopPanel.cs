using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts;


public class NUGoldShopPanel : MonoBehaviour {

    public List<Button> buyDiamondBtnList;

    void Start()
    {


        foreach (var btn in buyDiamondBtnList)
        {
            btn.onClick.AddListener(delegate ()
            {
                OnBuyDiamondBtnDown(btn);
            });
        }
    }

    // Update is called once per frame
    void Update()
    {

    }


    private int _exchange;
    private void OnBuyDiamondBtnDown(Button btn)
    {
        string name = btn.name;

        string butName = btn.transform.FindChild("Text2").gameObject.GetComponent<Text>().text;

        int index = int.Parse((name.Replace("Button", "")));
        List<int> money = new List<int> { 20000, 38000, 90000, 170000 };
        List<int> diam = new List<int> {1,2,5,10 };

        if (money[index] > MainManager.Instance.playerSelfInfo.goldNum)
        {
            NUMessageBox.Show("余额不足！");
        }
        else
        {
            _exchange = money[index];

            string str = string.Format("是否用{0}兑换{1}钻石？", butName, diam[index]);

            NUMessageBox.Show(str, NUMessageCallBack);
        }
    }

    private void NUMessageCallBack(NUMessageBox.CallbackType cbt)
    {
        SocketClient.Instance.PlayerPayForDiamond(_exchange);
    }
}
