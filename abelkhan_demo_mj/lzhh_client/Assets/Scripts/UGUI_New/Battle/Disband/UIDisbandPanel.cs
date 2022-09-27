using TinyFrameWork;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System;
using System.Collections;
public class UIDisbandPanel : MonoBehaviour
{

    public List<GameObject> gridViewList = null;
    private List<DisbandGrid> _gridList;
    private List<AccountData> _playerData;
    public Button closeBtn = null;
    private Hashtable voteState;
    private bool _setVoteFlag;
    public Text timeCountxt = null;
    private bool isShowTimeTxtFlag;
    private int timeCount;
    private bool isStartOver;
    // Use this for initialization
    public bool isExecute;
    void Start()
    {
        closeBtn.onClick.AddListener(delegate ()
        {
            ClosePanel();
        });
        DisbandGrid grid;
        _gridList = new List<DisbandGrid>();
        for (int i = 0; i < gridViewList.Count; i++)
        {
            grid = new DisbandGrid(gridViewList[i]);
            _gridList.Add(grid);
        }
        _setVoteFlag = false;
        ShowViewByData();
        isStartOver = true;
        isExecute = false;
    }

    public void ShowCountDownTime()
    {
        this.CancelInvoke();
        timeCount = 59;
        isShowTimeTxtFlag = true;
    }

    public void HidePanel()
    {
        voteState.Clear();
        Reset();
        this.CancelInvoke();
        timeCountxt.text = "";
        isShowTimeTxtFlag = false;
        this.gameObject.SetActive(false);
    }

    private void setInterval()
    {
        timeCount--;
        timeCountxt.text = timeCount.ToString();
        if (timeCount <= 0)
        {
            isShowTimeTxtFlag = false;
            timeCountxt.gameObject.SetActive(false);
            this.CancelInvoke();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isExecute)
        {
            voteState = TableController.Instance.voteState;
            int count = voteState.Count;
            string[] keys = new string[count];
            this.voteState.Keys.CopyTo(keys, 0);
            foreach (string client in keys)
            {
                OnDisbandgame(client, (Int64)voteState[client]);
            }
            isExecute = true;
        }

        string tempUiid;
        Int64 tempState;
        if (isStartOver)
        {
            if (_setVoteFlag)
            {
                foreach (DisbandGrid item in _gridList)
                {
                    if (item.uiid != "")
                    {
                        tempUiid = item.uiid;
                        if (voteState.ContainsKey(tempUiid))
                        {
                            tempState = (Int64)voteState[tempUiid];
                            if (tempState == 1)
                            {
                                item.AgreeDisband();
                            }
                            else if (tempState == 2)
                            {
                                item.RefuseDisband();
                            }
                            _setVoteFlag = false;
                        }
                    }
                }
            }
        }

        if (isShowTimeTxtFlag)
        {
            this.InvokeRepeating("setInterval", 1.0f, 1.0f);
            isShowTimeTxtFlag = false;
        }
    }

    void OnDestroy()
    {
        this.CancelInvoke();
        isShowTimeTxtFlag = false;
        _playerData = null;
        _gridList = null;
        isStartOver = false;
        if (voteState != null)
        {
            voteState.Clear();
        }
    }

    // private Bool init
    public void Init(List<AccountData> list)
    {
        voteState = TableController.Instance.voteState;
        _playerData = list;
    }

    private void ClosePanel()
    {
        if (voteState.Count == TableController.Instance.creatRoomInfo.playerNum)
        {
            Reset();
            this.CancelInvoke();
            timeCountxt.text = "";
            isShowTimeTxtFlag = false;
            this.gameObject.SetActive(false);
            TableController.Instance.voteState.Clear();
        }
        else
        {
            NUMessageBox.Show("还有玩家没有投票");
        }
    }

    private void ShowViewByData()
    {
        DisbandGrid grid;
        AccountData data;
        for (int i = 0; i < _playerData.Count; i++)
        {
            data = _playerData[i];
            grid = _gridList[i];
            grid.SetInfo(data);
            grid.gridGameObject.SetActive(true);
        }
    }

    private void OnDisbandgame(string uiid, Int64 state)
    {
    //    voteState[uiid] = state;
        _setVoteFlag = true;
        int count = 0;
        foreach (string key in voteState.Keys)
        {
            if ((Int64)voteState[key] != 0)
            {
                count++;
            }
        }
        if (count == TableController.Instance.creatRoomInfo.playerNum)
        {
            if (this.gameObject.activeSelf)
            {
                StartCoroutine("ClosePanelDelay");
            }
        }
    }

    private IEnumerator ClosePanelDelay()
    {
        yield return new WaitForSeconds(1f);
        ClosePanel();
    }

    public void Reset()
    {
        DisbandGrid item;
        for (int i = _gridList.Count - 1; i >= 0; i--)
        {
            item = _gridList[i];
            item.Reset();
            item = null;
        }
    }
}

public class DisbandGrid
{
    private Image _headMc;
    private Text _nameTxt;
    private Image _agreeImage;
    private Image _refuseImage;
    public string uiid;
    public GameObject gridGameObject;
    public DisbandGrid(GameObject grid)
    {
        uiid = "";
        _headMc = grid.transform.FindChild("Head").gameObject.GetComponent<Image>();
        _nameTxt = grid.transform.FindChild("Name").gameObject.GetComponent<Text>();
        _agreeImage = grid.transform.FindChild("Image").gameObject.GetComponent<Image>();
        _refuseImage = grid.transform.FindChild("ImageNO").gameObject.GetComponent<Image>();
        _agreeImage.gameObject.SetActive(false);
        _refuseImage.gameObject.SetActive(false);
        gridGameObject = grid;
        grid.SetActive(false);
    }

    public void Reset()
    {
        _nameTxt.text = "";
        _headMc.overrideSprite = null;
        _agreeImage.gameObject.SetActive(false);
        _refuseImage.gameObject.SetActive(false);
    }

    public void AgreeDisband()
    {
        _agreeImage.gameObject.SetActive(true);
        _refuseImage.gameObject.SetActive(false);
    }

    public void RefuseDisband()
    {
        _agreeImage.gameObject.SetActive(false);
        _refuseImage.gameObject.SetActive(true);
    }

    public void SetInfo(AccountData data)
    {
        _nameTxt.text = data.wechat_name;
        OnLoadHeadInfo(data.headimg);
        uiid = data.token;
    }

    private void OnLoadHeadInfo(string url)
    {
        GameObject ojb = GameObject.Find("DontDestroyOnLoad");
        HttpUtil Until = ojb.GetComponent<HttpUtil>();
        if (url != "")
        {
            string s2 = Regex.Unescape(url);
            Until.DownloadPicture(s2, LoadHeadSuccessCallBack);
        }
    }

    private void LoadHeadSuccessCallBack(Texture2D res, int index)
    {
        Sprite spr = Sprite.Create(res, new Rect(0, 0, res.width, res.height), Vector2.zero);
        if (spr != null)
        {
            _headMc.sprite = spr;
        }
    }
}