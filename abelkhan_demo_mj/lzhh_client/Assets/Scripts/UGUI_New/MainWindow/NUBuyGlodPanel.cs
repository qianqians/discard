using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts;
/// <summary>
/// 购买金币和钻石的面板
/// </summary>
public class NUBuyGlodPanel : MonoBehaviour {
    public GameObject goldScrollPanel;
    public GameObject diamondScrollPanel;
    public List<Button> buyGoldBtnList;
    public List<Button> buyDiamondBtnList;
    public Button diamondBtn;
    public Button goldBtn;

    private int[] dimanondCountArr;
    public List<Text> diamondTxtLixt = null;

    private int _dalayTime;
    private List<int[]> exchangeList;
    // Use this for initialization
    void Start () {
        Text txt;
        int rate = MainManager.Instance.payRate;
        int[] arr1 = new int[] {6,18,30,68,128 };
        int[] arr2 = new int[] { 7,22,36,82,158 };
        int[] arr3 = new int[] { 8, 24, 40, 90, 170 };
        exchangeList = new List<int[]> { arr1,arr2,arr3 };
        _dalayTime = 0;
        diamondBtn.onClick.AddListener(delegate ()
        {
            OnClickDiamondlabel(diamondBtn);
        });

        goldBtn.onClick.AddListener(delegate ()
        {
            OnClickGoldlabel(goldBtn);
        });

        foreach (var btn in buyGoldBtnList)
        {
            btn.onClick.AddListener(delegate ()
            {
                OnBuyGoldBtnDown(btn);
            });
        }


        foreach (var btn in buyDiamondBtnList)
        {
            btn.onClick.AddListener(delegate ()
            {
                OnBuyDiamondBtnDown(btn);
            });
        }
        
        for (int i = 0; i < diamondTxtLixt.Count; i++)
        {
            txt = diamondTxtLixt[i];
            int[] arr = exchangeList[rate];
            txt.text = arr[i] + " 钻石";
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (_dalayTime>0)
        {
            _dalayTime--;
            if (_dalayTime == 0)
            {
                SetBtnEnable(true);
            }
        }
    }

    private void OnClickGoldlabel(Button btn)
    {
        goldScrollPanel.SetActive(true);
        diamondScrollPanel.SetActive(false);
    }

    private void OnClickDiamondlabel(Button btn)
    {
        goldScrollPanel.SetActive(false);
        diamondScrollPanel.SetActive(true);
    }

    private int _exchange;
    private void OnBuyGoldBtnDown(Button btn)
    {
        string name = btn.name;
        int index = int.Parse((name.Replace("Button", "")));
        List<int> money = new List<int> { 5, 10, 20, 50 };
        if (money[index]>MainManager.Instance.playerSelfInfo.diamondNum)
        {
            NUMessageBox.Show("钻石不足！");
        }
        else
        {
            _exchange = money[index];
            NUMessageBox.Show("是否确定用"+ money[index]+"钻石兑换\n"+ money[index]*10000+"金币", NUMessageCallBack);        
        }
    }

    private void NUMessageCallBack(NUMessageBox.CallbackType cbt)
    {
        SocketClient.Instance.PlayerPayForGold(_exchange);
    }

    private void OnBuyDiamondBtnDown(Button btn)
    {
        string name = btn.name;
        if (_dalayTime <=0)
        {
            _dalayTime = 15;
            SetBtnEnable(false);
            int index = int.Parse((name.Replace("Button", "")));
            List<int> money = new List<int> { 600, 1800, 3000, 6800, 12800 };
            SocketClient.Instance.PlayerPay(money[index]);
        }

    }

    private void SetBtnEnable(bool boo)
    {
        foreach (var btn in buyDiamondBtnList)
        {
            btn.enabled = boo;
        }
    }
}
