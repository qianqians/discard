using Assets.Scripts.GameLogic.socket;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TinyFrameWork;
using gcloud_voice;
using System;
using System.Runtime.InteropServices;
namespace Assets.Scripts
{
    public class NULogin : MonoBehaviour
    {
		#if UNITY_IPHONE
		[DllImport("__Internal")]
		private static extern void weixinLoginByIos();
		#endif // UNITY_IPHONE
        public InputField UserName = null;
        public InputField Password = null;
        public Button LoginBtn = null;
        private string codestr;

        public Image ceshiPic = null;
        private string VersionID = "1.0.0.9";
        // Use this for initialization
        void Start()
        {         
            EventDispatcher.GetInstance().MainEventManager.AddEventListener<Hashtable,string,Int64>(EventId.Sever_Login, OnSelfLogin);
            EventDispatcher.GetInstance().MainEventManager.AddEventListener<List<string>>(EventId.Sever_get_access_token, OnGetAccessToken);
            EventDispatcher.GetInstance().MainEventManager.AddEventListener<bool,string>(EventId.Sever_access_token_login, OnAccessTokenLogin);                
            LoginBtn.onClick.AddListener(delegate ()
            {
                OnLogin();
                //MainManager.Instance.dontDestroyOnLoad.BeginShot(ScreenshotComplete);
            });
            string webRes = "http://www.sanyuanhy.com/download/VersionNumber.txt";
			#if UNITY_IPHONE
				webRes = "http://www.sanyuanhy.com/download/VersionNumberIos.txt";
			#endif

            StartCoroutine(GETTexture(webRes));
            if (PlayerPrefs.HasKey("Account"))
            {
                UserName.text = PlayerPrefs.GetString("Account");
            }
            MainManager.Instance.nowSceneName = SceneName.NULogin;
        }

        IEnumerator GETTexture(string webRes)
        { 
            WWW wwwTexture = new WWW(webRes);
            yield return wwwTexture;
            if (wwwTexture.error != null)
            {
                //GET请求失败  
            }
            else
            {
                string ver = wwwTexture.text;
                if (ver == VersionID)
                {
                    if (PlayerPrefs.HasKey("access_token") && PlayerPrefs.HasKey("refresh_token"))
                    {
                        OnLogin();
                    }
                }
                else
                {
                    NUMessageBox.Show("有新版本，请更新",NUMessageCallBack, NUMessageNOCallBack);
                }
               
            }
        }

        private void NUMessageCallBack(NUMessageBox.CallbackType cbt)
        {
            Application.OpenURL("http://www.sanyuanhy.com/test/fenxiang.html");
        }

        private void NUMessageNOCallBack(NUMessageBox.CallbackType cbt)
        {
            if (PlayerPrefs.HasKey("access_token") && PlayerPrefs.HasKey("refresh_token"))
            {
                OnLogin();
            }
        }

        void OnDestroy()
        {
            EventDispatcher.GetInstance().MainEventManager.RemoveEventListener<Hashtable, string, Int64>(EventId.Sever_Login, OnSelfLogin);
            EventDispatcher.GetInstance().MainEventManager.RemoveEventListener<List<string>>(EventId.Sever_get_access_token, OnGetAccessToken);
            EventDispatcher.GetInstance().MainEventManager.RemoveEventListener<bool,string>(EventId.Sever_access_token_login, OnAccessTokenLogin);
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnAccessTokenLogin(bool state,string token)
        {
           // EventDispatcher.GetInstance().UIFrameWorkEventManager.TriggerEvent<string>(EventId.UIFrameWork_Bug, state.ToString());
            if (state)
            {
                if (token!="")
                {
                    PlayerPrefs.SetString("access_token", token);
                }         
            }
            else
            {
                LoginBtn.gameObject.SetActive(true);
                if (PlayerPrefs.HasKey("access_token"))
                {
                    PlayerPrefs.DeleteKey("access_token");
                }
                if (PlayerPrefs.HasKey("refresh_token"))
                {
                    PlayerPrefs.DeleteKey("refresh_token");
                }
            }
        }

        private void OnGetAccessToken(List<string> data)
        {
            PlayerPrefs.SetString("access_token", data[0]);
            PlayerPrefs.SetString("refresh_token", data[1]);
            PlayerPrefs.SetString("unionid", data[2]);
            PlayerPrefs.SetString("openID", data[3]);
            SetVicoeInit(data[3]);
        }

        private void SetVicoeInit(string openID)
        {
    #if UNITY_ANDROID
                if (MainManager.Instance.m_voiceengine  == null)
                {
                    MainManager.Instance.m_voiceengine = GCloudVoice.GetEngine();
                    MainManager.Instance.m_voiceengine.SetAppInfo("1690546381", "babfc5e6dfaa15e4e8aa3d772dfcfe7b", openID);
                    MainManager.Instance.m_voiceengine.Init();
                    MainManager.Instance.m_voiceengine.SetMode(GCloudVoiceMode.Messages);
                    MainManager.Instance.m_voiceengine.OnApplyMessageKeyComplete += OnApplyMessageKeyCompleteHandle;
                }
    #elif UNITY_IPHONE
                    if (MainManager.Instance.m_voiceengine  == null)
                    {
                        MainManager.Instance.m_voiceengine = GCloudVoice.GetEngine();
                        MainManager.Instance.m_voiceengine.SetAppInfo("1690546381", "babfc5e6dfaa15e4e8aa3d772dfcfe7b", openID);
                        MainManager.Instance.m_voiceengine.Init();
                        MainManager.Instance.m_voiceengine.SetMode(GCloudVoiceMode.Messages);
                        MainManager.Instance.m_voiceengine.OnApplyMessageKeyComplete += OnApplyMessageKeyCompleteHandle;          
                    }           
    #endif
            if (!MainManager.Instance.isGetAuthKey)
            {
                int rct;
                if (MainManager.Instance.m_voiceengine != null)
                {
                    MainManager.Instance.isGetAuthKey = true;
                    rct = MainManager.Instance.m_voiceengine.ApplyMessageKey(6000);
                }
            }
        }

        private void OnApplyMessageKeyCompleteHandle(IGCloudVoice.GCloudVoiceCompleteCode code)
        {
            if (code == IGCloudVoice.GCloudVoiceCompleteCode.GV_ON_MESSAGE_KEY_APPLIED_SUCC)
            {
                Debug.Log("OnApplyMessageKeyComplete succ11");
            }
            else
            {
                Debug.Log("OnApplyMessageKeyComplete error");
            }
        }

        private void OnSelfLogin(Hashtable info,string roomName, Int64 roomID)
        {
            SocketClient.Instance.playToken = (string)info["unionid"];
            TableController.Instance.SelfInfo = info;
            SelfBaseData data = new SelfBaseData(info);
            MainManager.Instance.playerSelfInfo = data;
            if (roomID!=0 && roomName != "")//判断是断线重连还是正常登录
            {
                SocketClient.Instance.EnteRoomReconnect(roomName, roomID);
            }
            else
            {
                MainManager.Instance.dontDestroyOnLoad.StartLoadingScene("NUMainWindow", ReconnectionLoadGameSceneComplete);
            }
          //  SocketClient.Instance.NetHandle.enable_heartbeats();
        }

        private void ReconnectionLoadGameSceneComplete()
        {
            //EventDispatcher.GetInstance().UIFrameWorkEventManager.TriggerEvent<string>(EventId.UIFrameWork_Bug, "ReconnectionLoadGameSceneComplete");
           // SocketClient.Instance.NetHandle.enable_heartbeats();
           // MainManager.Instance.dontDestroyOnLoad.SetLoading(false);
        }

        //public void InitScene()
        //{
        //    string fileName = "";
        //    if (Application.platform == RuntimePlatform.Android)
        //    {
        //        fileName = Application.persistentDataPath;
        //    }
        //    else
        //    {
        //        fileName = Application.dataPath;
        //    }
        //}


        public void GetCode(string code)
        {
            codestr = code;
            SocketClient.Instance.Login(codestr);
        }

        void OnLogin()
        {
            string code;
            string refreshToken;
            string uiid;
            string openid;
            LoginBtn.gameObject.SetActive(false);
            MainManager.Instance.dontDestroyOnLoad.SetLoading(true);
            if (PlayerPrefs.HasKey("access_token") && PlayerPrefs.HasKey("refresh_token"))
            {
                code = PlayerPrefs.GetString("access_token");
                refreshToken = PlayerPrefs.GetString("refresh_token");
                uiid = PlayerPrefs.GetString("unionid");
                openid = PlayerPrefs.GetString("openID");
                SetVicoeInit(openid);
                SocketClient.Instance.LoginUseAccessToken(code, refreshToken, uiid, openid);
                return;
            }

            #if UNITY_EDITOR
            PlayerPrefs.SetString("Account", UserName.text);
            SocketClient.Instance.LoginByPC(UserName.text);
            #elif UNITY_STANDALONE_WIN
            PlayerPrefs.SetString("Account", UserName.text);
            SocketClient.Instance.LoginByPC(UserName.text);
            #elif UNITY_ANDROID
            LoginByLocal();
            //AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            //AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
			//调用方法  
			//jo.Call("weiLogin");
            #elif UNITY_IPHONE
			weixinLoginByIos();
            #else
			// TODO:PC平台登录
			//Debug.Log("PC平台登录暂未实现");
            #endif // UNITY_ANDROID
            //   LoginBtn.GetComponent<Button>().enabled = false;
        }

        public void LoginByLocal()
        {
            if (!PlayerPrefs.HasKey("refresh_token"))
            {
                AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
                jo.Call("weiLogin");
            }
        } 

        public void login_sucess()
        {
            Debug.Log(Network.player.ipAddress+ "Network.player.ipAddress");
        }
    }
}
