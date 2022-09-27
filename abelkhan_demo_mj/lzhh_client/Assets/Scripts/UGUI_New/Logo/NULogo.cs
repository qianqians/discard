using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Assets.Scripts;
using TinyFrameWork;
public class NULogo : MonoBehaviour {
    public Button Enter = null;
    // Use this for initialization
    void Start() {
       // EventDispatcher.GetInstance().MainEventManager.AddEventListener(EventId.Sever_Login_Sucess, OnLoginSucess);
    }

    // Update is called once per frame
    void Update() {
		
    }

    void OnDestroy()
    {
       // EventDispatcher.GetInstance().MainEventManager.RemoveEventListener(EventId.Sever_Login_Sucess, OnLoginSucess);
    }

    private void OnLoginSucess()
    {
        // EventDispatcher.GetInstance().UIFrameWorkEventManager.TriggerEvent<string>(EventId.UIFrameWork_Bug, "loginSucess!");
       // MainManager.Instance.dontDestroyOnLoad.StartScene("InGame", ReconnectionLoadGameSceneComplete);
        SceneManager.LoadScene("NULogin");
    }

    //void OnEnter(GameObject obj)
    //{
    //    if (SocketClient.Instance.isGetHub && SocketClient.Instance.isGetRoom1)
    //    {
    //        SceneManager.LoadScene("NULogin");
    //    }
    //    else
    //    {
    //        NUMessageBox.Show("连接有问题");
    //    }
    //}   
}
