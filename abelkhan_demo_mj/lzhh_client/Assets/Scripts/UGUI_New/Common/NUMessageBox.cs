using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class NUMessageBox : MonoBehaviour {
    public enum CallbackType
    {
        Close,
        Confirm,
        cancel
    }
    public delegate void MessageBoxCallback(CallbackType cbt);
    public GameObject MessageBoxRoot = null;
    public Text Message = null;
    public Button BGBtn = null;
    public Button ConfirmBtn = null;
    public Button noBtn = null;
    public Text timeCountxt = null;
    private bool ShowMsgflag;
    public static NUMessageBox instance = null;
    MessageBoxCallback callback = null;
    MessageBoxCallback callbackCancel = null;

    private bool isShowTimeTxtFlag;
    private string messg;
    /// <summary>
    /// flag 是否显示倒计时
    /// </summary>
    /// <param name="msg"></param>
    /// <param name="callback"></param>
    /// <param name="callbackCancel"></param>
    /// <param name="flag"></param>
    static public void Show(string msg, MessageBoxCallback callback = null, MessageBoxCallback callbackCancel = null,bool flag = false)
    {
        if (instance != null)
        {
            instance.ShowMessageBox(msg, callback, callbackCancel, flag);
        }
    }
    void Awake()
    {
        instance = this;
    }
    // Use this for initialization
    void Start () {
      //  isShowTimeTxtFlag = false;
        timeCountxt.gameObject.SetActive(false);
        BGBtn.onClick.AddListener(delegate ()
        {
            OnClose(gameObject);
        });
        noBtn.onClick.AddListener(delegate ()
        {
            OnCancel(gameObject);
        });
        ConfirmBtn.onClick.AddListener(delegate ()
        {
            OnConfirm(gameObject);
        });
    }
	
	// Update is called once per frame
	void Update () {
        //if (threadFlag)
        //{
        //    Message.text = messg;
        //    MessageBoxRoot.SetActive(true);
        //    threadFlag = false;
        //    messg = "";
        //}
        if (isShowTimeTxtFlag)
        {
            this.InvokeRepeating("setInterval", 1.0f, 1.0f);
            isShowTimeTxtFlag = false;
        }

        if (ShowMsgflag)
        {
            ShowMsgflag = false;
            Message.text = messg;
            MessageBoxRoot.SetActive(true);
            messg = "";
        }
	}

    private int timeCount;
    private void setInterval()
    {
        timeCount--;
        timeCountxt.text = timeCount.ToString();
        if (timeCount<=0)
        {
            isShowTimeTxtFlag = false;
            timeCountxt.gameObject.SetActive(false);
            this.CancelInvoke();
        }       
    }

    void ShowMessageBox(string msg, MessageBoxCallback callback, MessageBoxCallback callbackCancel,bool flag)
    {
        ShowMsgflag = true;
        isShowTimeTxtFlag = flag;
        if (isShowTimeTxtFlag)
        {
            timeCount = 59;
            timeCountxt.gameObject.SetActive(true);
        }
        messg = msg;
        this.callback = callback;
        this.callbackCancel = callbackCancel;
        timeCountxt.text = "";
    }

    void OnClose(GameObject obj)
    {
        MessageBoxRoot.SetActive(false);
        if (callback != null)
        {
            callback(CallbackType.Close);
        }

        if (timeCount >= 0)
        {
            timeCount = 0;
            isShowTimeTxtFlag = false;
            timeCountxt.gameObject.SetActive(false);
            timeCountxt.text = "";
            this.CancelInvoke();
        }
    }

    void OnConfirm(GameObject obj)
    {
        MessageBoxRoot.SetActive(false);
        if (callback != null)
        {
            callback(CallbackType.Confirm);
        }
        if (timeCount >= 0)
        {
            timeCount = 0;
            isShowTimeTxtFlag = false;
            timeCountxt.text = "";
            timeCountxt.gameObject.SetActive(false);
            this.CancelInvoke();
        }
    }

    void OnCancel(GameObject obj)
    {
        MessageBoxRoot.SetActive(false);
        if (callbackCancel != null)
        {
            callbackCancel(CallbackType.cancel);
        }
        if (timeCount>=0)
        {
            timeCount = 0;
            isShowTimeTxtFlag = false;
            timeCountxt.gameObject.SetActive(false);
            this.CancelInvoke();
        }
    }
}
