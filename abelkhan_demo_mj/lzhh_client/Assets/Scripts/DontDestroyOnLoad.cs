using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using System;
using gcloud_voice;
using TinyFrameWork;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;
using DG.Tweening;
using System.Text.RegularExpressions;
using System.Text;
using System.Threading;

namespace Assets.Scripts
{
    public class DontDestroyOnLoad : MonoBehaviour
    {
        public Text debugTxt;
        public Image scen = null;
        public GameObject loadingObject = null;
        public GameObject loadingPanel = null;
        public float rotateSpeed = 50;
        private bool _isShowLoading;
        private AsyncOperation async_operation;

        //切换账号
        public Button changeAccountBtn = null;
        public GameObject setPanel = null;
        private Action loadingCompleteFunc;

        public GameObject noticebar = null;
        public Text noticeText = null;
        private Tweener noticeTween;
        private bool debugFlag;
        private string debugMsg;
      //  private List<string> debugArr;
   //     private int countNum;

        //截屏的路径
        private string url = "/onMobileSavedScreen.png";
        private string ScreenShotpath;
        private int CountDebug;

        private bool iscanbuhh;
        private StringBuilder debugStr;
        //    public Action<string> debugCallBack;
        void Start()
        {
            DontDestroyOnLoad(this.gameObject);
          //  debugArr = new List<string>();
          //  debugArr.Capacity = 105;
            ScreenShotpath = Application.persistentDataPath;
            //  ScreenShotpath = Application.dataPath;
            debugTxt.gameObject.SetActive(true);
            noticebar.gameObject.SetActive(false);
            debugStr = new StringBuilder();
            EventDispatcher.GetInstance().UIFrameWorkEventManager.AddEventListener<string>(EventId.UIFrameWork_Bug, Ondebug);
            EventDispatcher.GetInstance().MainEventManager.AddEventListener(EventId.Sever_Login_Sucess, OnLogoSucess);
            EventDispatcher.GetInstance().MainEventManager.AddEventListener<string>(EventId.Sever_NoticeMsg, OnShowNotice);
            OnShowNotice("游戏测试阶段如有问题可联系上级代理或联系客服微信 sanyuanxy 进行处理");
            _isShowLoading = false;      
            SoundManager.Instance.Create();
            SoundManager.Instance.Play(ESoundLayer.Background, "bgjn");
            SetSound();
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
           
            OnLoadingLogoComplete(false);
            //if (PlayerPrefs.HasKey("MjBackColor"))
            //{
            //    MainManager.Instance.selfMjBackColorState = PlayerPrefs.GetString("MjBackColor");
            //}
            changeAccountBtn.onClick.AddListener(delegate ()
            {
                setPanel.SetActive(false);
                Reset();
            });
        }

        /// <summary>
        /// 断线后点确定重连
        /// </summary>
        public void BreakOnline()
        {
            OnLoadingLogoComplete(true);
        }

        private void SetSound()
        {
            float backgroundSound;
            float effectSound;
            float effectUISound;
            if (PlayerPrefs.HasKey("Background_sound"))
            {
                backgroundSound = PlayerPrefs.GetFloat("Background_sound");
            }
            else
            {
                backgroundSound = 0.02f;
                PlayerPrefs.SetFloat("Background_sound", 0.02f);

            }

            if (PlayerPrefs.HasKey("Effect_sound"))
            {
                effectSound = PlayerPrefs.GetFloat("Effect_sound");
            }
            else
            {
                effectSound = 0.7f;
                PlayerPrefs.SetFloat("Effect_sound", 0.7f);
            }

            if (PlayerPrefs.HasKey("Effect_UI"))
            {
                effectUISound = PlayerPrefs.GetFloat("Effect_sound");
            }
            else
            {
                effectUISound = 0.5f;
                PlayerPrefs.SetFloat("Effect_UI", 0.5f);
            }

            SoundManager.Instance.SetVolume(ESoundLayer.Background, backgroundSound);
            SoundManager.Instance.SetVolume(ESoundLayer.Effect, effectSound);
            SoundManager.Instance.SetVolume(ESoundLayer.EffectUI, effectUISound);
        }

        private void OnShowNotice(string str)
        {     
            noticeText.text = str;
            if (noticeTween == null)
            {              
                noticebar.gameObject.SetActive(true);
                NoticeMovie();
            }
            else
            {
                noticeTween.Kill();
                noticeTween = null;
                NoticeMovie();
            }
        }

        private bool loadOver;
        private void OnLoadingLogoComplete(bool isReconnect)
        {
            MainManager.Instance.Init();
            loadOver = true;
            MainManager.Instance.dontDestroyOnLoad = this;
            TableController.Instance.RemoveEventListener();
            TableController.Instance.Init();
            SetLoading(true);
            SocketClient.Instance.Destory();
            if (isReconnect)
            {
                SocketClient.Instance.OnReconnectServer();
            }
            else
            {
                SocketClient.Instance.Init();
            }    
        }

        private void Ondebug(string str)
        {
            debugFlag = false;
          //  Debug.Log(str);
            if (debugFlag)
            {
                //if (str.Length > 100)
                //{
                //    debugStr.AppendLine(str.Substring(0, 100));
                //}
                //else
                //{
                //    debugStr.AppendLine(str.Substring(0, str.Length));
                //}
                debugStr.AppendLine(str);

                iscanbuhh = true;
            }         
        }

        public  List<string> SplitLength(string SourceString, int Length)
        {
            List<string> list = new List<string>();
            int startIndex = 0;
            int endIndex = 100;
            for (int i = 0; i < Length; i++)
            {
                list.Add(SourceString.Substring(startIndex, endIndex));
                startIndex = endIndex;
                if (endIndex+100> SourceString.Length)
                {
                    endIndex = SourceString.Length - startIndex;
                }
                else
                {
                    endIndex = endIndex + 100;
                }
            }
            return list;
        }


        void Update()
        {         
            if (iscanbuhh)
            {
                debugTxt.text = debugStr.ToString();
                iscanbuhh = false;
            }

            if (_isShowLoading)
            {
                loadingPanel.SetActive(true);
                Loading();
            }
            else
            {
                if (loadingPanel.activeSelf)
                {
                    loadingPanel.SetActive(false);
                }            
            }

            if (async_operation != null)
            {
                if (async_operation.progress == 1 && _isShowLoading)
                {
                    _isShowLoading = false;
                    async_operation = null;
                    if (loadingCompleteFunc != null)
                    {                      
                        loadingCompleteFunc();                     
                        loadingCompleteFunc = null;
                    }
                }
            }

            if (loadOver)
            {
                if (PlayerPrefs.HasKey("MjBackColor"))
                {
                    MainManager.Instance.selfMjBackColorState = PlayerPrefs.GetString("MjBackColor");
                    loadOver = false;
                }
            }

            if (Input.GetKey(KeyCode.Escape))
            {

                #if UNITY_EDITOR

                #elif UNITY_ANDROID
                        //这个地方可以写“再按一次退出”的提示           
                            //escapeTimes++;
                                NUMessageBox.Show("确定退出游戏？",SureEndGame);
                            //StartCoroutine("resetTimes");
                            //if (escapeTimes > 1)
                            //{
                            //    Application.Quit();
                            //}                      
                #endif
            }
            if (MainManager.Instance.m_voiceengine == null)
            {
              //  Debug.Log("m_voiceengine is null");
            }
            else
            {
                try
                {
                    MainManager.Instance.m_voiceengine.Poll();
                }
                catch (System.Exception e)
                {
                    EventDispatcher.GetInstance().UIFrameWorkEventManager.TriggerEvent<string>(EventId.UIFrameWork_Bug, e.Message);
                }
            }
            SocketClient.Instance.Update();
        }

        private string tempStr;
        private IEnumerator HideVoiceBarDelaye(string str)
        {
            yield return new WaitForSeconds(0.5f);
            Monitor.Enter(this);
            debugStr.Append(str);
            iscanbuhh = true;
            Monitor.Exit(this);//取消锁定
            Thread.Sleep(5);
        }

        private void NoticeMovie()
        {
            float offet;
            float moviceTime;
            Transform aa = noticeText.gameObject.transform;
            int num = noticeText.text.Length;
            int chinaCount = 0;
            for (int i = 0; i < noticeText.text.Length; i++)
            {
                if ((int)noticeText.text[i] > 127)
                {
                    chinaCount++;
                }         
            }
            offet = chinaCount * 29 + (num - chinaCount) * 19;
            noticeText.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(offet, 4.95f);
            if (num>15)
            {
                moviceTime = num * 1.4f;
            }
            else
            {
                moviceTime = 18;
            }
            noticeTween = aa.DOLocalMoveX(-offet- 820, moviceTime).SetRelative().SetLoops(-1, LoopType.Restart);
            noticeTween.SetEase(Ease.Linear);
        }

        private void SureEndGame(NUMessageBox.CallbackType cbt)
        {
            Application.Quit();
        }

        /// <summary>
        /// 加载登录界面
        /// </summary>
        private void OnLogoSucess()
        {
            StartLoadingScene("NULogin", LoadNULoginComplete);
        }

        private void LoadNULoginComplete()
        {
            SetLoading(false);
        }
  
        //private void FenxiangPic()
        //{
        //    AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        //    AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        //    jo.Call("SendPicToWX", "aoke");
        //}

        public void Reset()
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.DeleteKey("access_token");
            PlayerPrefs.DeleteKey("refresh_token");
            PlayerPrefs.DeleteKey("unionid");
            PlayerPrefs.DeleteKey("openID");
            OnLoadingLogoComplete(false);         
        }

        public void StartLoadingScene(string scene_name,Action complete=null)
        {
            loadingCompleteFunc = complete;
            SetLoading(true);
            StartCoroutine("LoadScene", scene_name);
         //   SceneManager.LoadScene();
        }
        
        //加载场景  
        IEnumerator LoadScene(string scene_name)
        {
            async_operation = SceneManager.LoadSceneAsync(scene_name);
            yield return async_operation;        
        }

        public void SetLoading(bool flag)
        {
            _isShowLoading = flag;
        }

        public void Loading()
        {
            loadingObject.transform.Rotate(-Vector3.forward * rotateSpeed * Time.deltaTime);
        }
 
       /// <summary>
       /// 回掉函数，rect 要截屏的区域
       /// </summary>
       /// <param name="callback"></param>
       /// <param name="rect"></param>
        public void BeginShot(Action<Texture2D,string> callback, Rect rect,string imgRes)
        {
            StartCoroutine(Screenshot(callback, rect,imgRes));
        }

        //开启一个携程进行截屏
        IEnumerator Screenshot(Action<Texture2D, string> callback, Rect rect,string imgRes)
        {
            //等待帧结束
            yield return new WaitForEndOfFrame();
            //获取到屏幕的宽高，进行全屏截图
            //int width = Screen.width;
            //int height = Screen.height;
            int width  = (int)rect.width;
            int height = (int)rect.height;
            Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);
            tex.ReadPixels(rect, 0, 0);
            tex.Apply();
            yield return 0;
            //将图片转成byte数据
            byte[] bytes = tex.EncodeToPNG();
            Destroy(tex);
            File.WriteAllBytes(imgRes, bytes);
            callback(tex, imgRes);
        }

        public void GetText(Action<Texture2D> callback)
        {
            StartCoroutine(GetPic(callback));
        }

        public IEnumerator GetPic(Action<Texture2D> callback)
        {
            string filePath = "file://" + Application.dataPath + url;
            WWW www = new WWW(filePath);
            yield return www;
            callback(www.texture);
        }
    }
}
