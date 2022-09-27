using System.Collections;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.UI;
using gcloud_voice;
using TinyFrameWork;
using GameCommon;
using System.Collections.Generic;
public class MyGCloudVoice : MonoBehaviour {
    private string m_authkey; /*this key should get from your game svr*/
    private byte[] m_ShareFileID = null; /*when send record file save in svr, we will return a fileid in OnSendFileComplete callback function, you can save it ,and download  record by this fileid*/
    private IGCloudVoice m_voiceengine ;  //engine have init int mainscene start function

    private string m_recordpath;
    private string m_downloadpath;
    private static string m_fileid = "";

    private static string s_strLog;
    private static bool bIsStart = false;
    public Text m_logText;
    public GameObject btn_local = null;
    public GameObject btn_svr = null;
    private bool bIsGetAuthKey = false;

    public bool isCanSay;

    public bool isCanPlayRecordedFile;
    private ArrayList _fileIDArr;
    /// <summary>
    /// 如果录音的过程中有别人发语音过来，，延后处理
    /// </summary>
    private bool _voicePlayerSwitch;
    private Dictionary<string, string> _fileIDDicUUid;
    void OnDestroy()
    {
        if (isCanSay)
        {
            _voicePlayerSwitch = true;
            isCanSay = false;
            m_voiceengine.StopPlayFile();
            removeEvent();
            if (_fileIDArr!=null)
            {
                _fileIDArr.Clear();
            }         
            _fileIDDicUUid = null;
            bIsStart = false;
            EventDispatcher.GetInstance().MainEventManager.RemoveEventListener<ArrayList>(EventId.Server_Chat_msg, OnGetStr);
        }     
    }
    // Use this for initialization
    void Start()
    {
        m_voiceengine = GCloudVoice.GetEngine();
        _voicePlayerSwitch = true;
        _fileIDDicUUid = new Dictionary<string, string>();
        isCanPlayRecordedFile = true;
        isCanSay = true;
        _fileIDArr = new ArrayList();
        EventDispatcher.GetInstance().MainEventManager.AddEventListener<ArrayList>(EventId.Server_Chat_msg, OnGetStr);
        // m_voiceengine.SetMode(GCloudVoiceMode.RealTime);
        if (!bIsStart)
        {
            bIsStart = true;
            m_voiceengine.OnUploadReccordFileComplete += OnUploadReccordFileCompleteHandle;
            m_voiceengine.OnDownloadRecordFileComplete += OnDownloadRecordFileCompleteHandle;
            m_voiceengine.OnPlayRecordFilComplete += OnPlayRecordFilCompleteHandle;
        }
        m_recordpath = Application.persistentDataPath + "/" + "recording.dat";
        m_downloadpath = Application.persistentDataPath + "/" + "download.dat";
    }

    private void removeEvent()
    {
      //  m_voiceengine.OnApplyMessageKeyComplete -= OnApplyMessageKeyCompleteHandle;
        m_voiceengine.OnUploadReccordFileComplete -= OnUploadReccordFileCompleteHandle;
        m_voiceengine.OnDownloadRecordFileComplete -= OnDownloadRecordFileCompleteHandle;
        m_voiceengine.OnPlayRecordFilComplete -= OnPlayRecordFilCompleteHandle;
    }

    private void OnApplyMessageKeyCompleteHandle(IGCloudVoice.GCloudVoiceCompleteCode code)
    {
        Debug.Log("OnApplyMessageKeyComplete c# callback");
        s_strLog += "\r\n" + "OnApplyMessageKeyComplete ret=" + code;
        if (code == IGCloudVoice.GCloudVoiceCompleteCode.GV_ON_MESSAGE_KEY_APPLIED_SUCC)
        {
            bIsGetAuthKey = true;
            Debug.Log("OnApplyMessageKeyComplete succ11");
        }
        else
        {
            Debug.Log("OnApplyMessageKeyComplete error");
        }
       // EventDispatcher.GetInstance().UIFrameWorkEventManager.TriggerEvent<string>(EventId.UIFrameWork_Bug, code.ToString());
    }

    private void OnUploadReccordFileCompleteHandle(IGCloudVoice.GCloudVoiceCompleteCode code, string filepath, string fileid)
    {
        if (code == IGCloudVoice.GCloudVoiceCompleteCode.GV_ON_UPLOAD_RECORD_DONE)
        {
            m_fileid = fileid;
            SocketClient.Instance.PlayerChat(ChatState.Voice, m_fileid);
            s_strLog += "\r\nUpload file to svr succ\r\nstart down record file from svr...";
            //  m_voiceengine.PlayRecordedFile(m_recordpath);
            _fileIDDicUUid.Add(m_fileid, MainManager.Instance.playerSelfInfo.unionID);
            _fileIDArr.Add(m_fileid);
        }
        else
        {
            s_strLog += "OnUploadReccordFileComplete err, filepath:" + filepath + " fileid:" + fileid;
            Debug.Log("OnUploadReccordFileComplete error");
        }
     //   EventDispatcher.GetInstance().UIFrameWorkEventManager.TriggerEvent<string>(EventId.UIFrameWork_Bug, code.ToString() + "OnUploadReccordFileCompleteHandle");
    }

    private void OnDownloadRecordFileCompleteHandle(IGCloudVoice.GCloudVoiceCompleteCode code, string filepath, string fileid)
    {
        Debug.Log("OnDownloadRecordFileComplete c# callback");
        //s_strLog += "\r\n"+"OnDownloadRecordFileComplete ret="+code+" filepath:"+filepath+" fielid:"+fileid;
        if (code == IGCloudVoice.GCloudVoiceCompleteCode.GV_ON_DOWNLOAD_RECORD_DONE)
        {
            Debug.Log("OnDownloadRecordFileComplete succ, filepath:" + filepath + " fileid:" + fileid);
            s_strLog += "\r\nDownload record file from svr succ";
            // btn_svr.SetActive(true);                   
            // Click_btnPlayReocrdFile(fileid);
            _fileIDArr.Add(fileid);
          //  EventDispatcher.GetInstance().UIFrameWorkEventManager.TriggerEvent<string>(EventId.UIFrameWork_Bug, "OnDownloadRecordFileComplete");
        }
        else
        {
            Debug.Log("OnDownloadRecordFileComplete error");
        }
    }

    private void OnPlayRecordFilCompleteHandle(IGCloudVoice.GCloudVoiceCompleteCode code, string filepath)
    {
        Debug.Log("OnPlayRecordFilComplete c# callback");
        s_strLog += "\r\n" + "OnPlayRecordFilComplete ret=" + code + " filepath:" + filepath;
        if (code == IGCloudVoice.GCloudVoiceCompleteCode.GV_ON_PLAYFILE_DONE)
        {
            Debug.Log("OnPlayRecordFilComplete succ, filepath:" + filepath);
        }
        else
        {
            Debug.Log("OnPlayRecordFilComplete error");
        }

        EventDispatcher.GetInstance().UIFrameWorkEventManager.TriggerEvent<string, bool>(EventId.UIFrameWork_Set_Voice_Active, "", false);
        isCanPlayRecordedFile = true;
    }

    private void OnGetStr(ArrayList data)
    {
        ChatState state = (ChatState)data[0];
        string id = (string)data[1];
        string targetUuid = (string)data[2];
        string uuid = (string)data[3];
        if (state == ChatState.Voice)
        {
            _fileIDDicUUid.Add(id, uuid);
            Click_btnDownloadFile(id);
           // EventDispatcher.GetInstance().UIFrameWorkEventManager.TriggerEvent<string>(EventId.UIFrameWork_Bug, state.ToString());
        }
    }

    public void Awake()
    {
        //m_logText = GameObject.Find("Canvas/Panel-GCloudVoice/Text-Log").GetComponentInChildren<Text>();
        //GameObject root = GameObject.Find("Canvas/Panel/Image");               
        //GameObject btn_localfile =  root.transform.Find("Button_LocalFile").gameObject;       
        //btn_localfile.SetActive (false);
        btn_local.SetActive(false);
        btn_svr.SetActive(false);
        /*
        if (btn_local == null)
        {
            btn_local = GameObject.Find("Canvas/Panel-GCloudVoice/Button-Local");//.GetComponent<Button> ();
            btn_local.SetActive(false);
        }
        if (btn_svr == null)
        {
            btn_svr = GameObject.Find("Canvas/Panel-GCloudVoice/Button-FromSvr");//.GetComponent<Button> ();
            btn_svr.SetActive(false);
        }
        */
        //xxxx.SetActive(true);

    }

    // Update is called once per frame
    void Update()
    {
        string fiedID;
        string uuid;
        if (_fileIDArr!=null)
        {
            if (isCanPlayRecordedFile && _fileIDArr.Count > 0 && _voicePlayerSwitch)
            {
                try
                {
                    isCanPlayRecordedFile = false;
                    fiedID = _fileIDArr[0] as string;
                    uuid = _fileIDDicUUid[fiedID];
                    _fileIDArr.RemoveAt(0);
                    _fileIDDicUUid.Remove(uuid);
                    if (uuid == MainManager.Instance.playerSelfInfo.unionID)
                    {
                        m_voiceengine.PlayRecordedFile(m_recordpath);
                    }
                    else
                    {
                        Click_btnPlayReocrdFile(fiedID);
                    }
                   
                    //  EventDispatcher.GetInstance().UIFrameWorkEventManager.TriggerEvent<string>(EventId.UIFrameWork_Bug, "playone");
                    EventDispatcher.GetInstance().UIFrameWorkEventManager.TriggerEvent<string, bool>(EventId.UIFrameWork_Set_Voice_Active, uuid, true);
                }
                catch (System.Exception)
                {
                    EventDispatcher.GetInstance().UIFrameWorkEventManager.TriggerEvent<string>(EventId.UIFrameWork_Bug, "_fileIDDicUUid.erro");
                }

            }
        }
       
        m_logText.text = s_strLog;
    }

    public void Click_btnBack()
    {
        s_strLog = "";
    //    Application.LoadLevel("main");
    }
    public void Click_btnClearLog()
    {
        s_strLog = "";
    }
    public void Click_btnReqAuthKey()
    {
        Debug.Log("ApplyMessageKey btn click");
        m_voiceengine.ApplyMessageKey(15000);
    }

    public void Click_btnStartRecord()
    {
        Debug.Log("startrecord btn click, recordpath=" + m_recordpath);
        int ret = m_voiceengine.StartRecording(m_recordpath);
        s_strLog += "\r\nStartRecording  ret=" + ret;
        btn_local.SetActive(false);
        btn_svr.SetActive(false);
       
        SetOtherSound(false);
        EventDispatcher.GetInstance().UIFrameWorkEventManager.TriggerEvent<bool>(EventId.UIFrameWork_Player_record, true);
    }

    /// <summary>
    /// 录音的时候调小其他音效,false关掉其他音效。。true 打开
    /// </summary>
    /// <param name="flag"></param>
    private void SetOtherSound(bool flag)
    {
        if (flag)
        {
            _voicePlayerSwitch = true;
            SetSound();
        }
        else
        {
            _voicePlayerSwitch = false;
            SoundManager.Instance.SetVolume(ESoundLayer.Background, 0.0f);
            SoundManager.Instance.SetVolume(ESoundLayer.Effect, 0.0f);
            SoundManager.Instance.SetVolume(ESoundLayer.EffectUI, 0.0f);
        }   
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

    public void Click_btnStopRecord()
    {
        Debug.Log("stoprecord btn click");
        int ret = m_voiceengine.StopRecording();
        s_strLog += "\r\nStopRecording  ret=" + ret;
        btn_local.SetActive(false);
        btn_svr.SetActive(false);
        if (ret == 0)
        {
            int ret1 = m_voiceengine.UploadRecordedFile(m_recordpath, 60000);
            s_strLog += "\r\nstart upload record file to svr...ret=" + ret1;
           // EventDispatcher.GetInstance().UIFrameWorkEventManager.TriggerEvent<string>(EventId.UIFrameWork_Bug, ret1.ToString() + "StopRecordret == 0");
        }
        else
        {
            s_strLog += "\r\nStopRecording err, ret=" + ret;
          //  EventDispatcher.GetInstance().UIFrameWorkEventManager.TriggerEvent<string>(EventId.UIFrameWork_Bug, ret.ToString() + "StopRecord");
        }
        SetOtherSound(true);
        EventDispatcher.GetInstance().UIFrameWorkEventManager.TriggerEvent<bool>(EventId.UIFrameWork_Player_record, false);
    }

    public void Click_btnUploadFile()
    {
        //GcloudVoice.GcloudVoiceErrno err;
        //err = m_voiceengine.SendRecordFile (m_recordpath, 15000);
        //PrintLog ("upload file with ret=" + err);
        //s_strLog += "\r\n upload file with ret=="+err;
        int ret1 = m_voiceengine.UploadRecordedFile(m_recordpath, 60000);
        Debug.Log("Click_btnUploadFile file with ret==" + ret1);
    }
    public void Click_btnDownloadFile(string fileid)
    {
        //GcloudVoice.GcloudVoiceErrno err;
        //err = m_voiceengine.DownRecordFile (m_ShareFileID, m_downloadpath, 5000);
        //PrintLog ("download file with ret=" + err);
        //s_strLog += "\r\n download file with ret=="+err;
       // DownloadRecordedFile
        int ret = m_voiceengine.DownloadRecordedFile(fileid, m_downloadpath, 60000);
        s_strLog += "\r\n download file with ret==" + ret + " fileid=" + m_fileid + " downpath" + m_downloadpath;
    }
    public void Click_btnPlayReocrdFile(string id)
    {
        int err;
        if (id == null)
        {
            //UnityEditor.EditorUtility.DisplayDialog("", "you have not download record file ,we will play local record files", "OK");
            err = m_voiceengine.PlayRecordedFile(m_recordpath);
            PrintLog("downloadpath is nill, play local record file with ret=" + err);
            return;
        }        
        m_voiceengine.PlayRecordedFile (m_downloadpath);
    }
    public void Click_btnPlayDownloadFile()
    {
        int err;
        err = m_voiceengine.PlayRecordedFile(m_downloadpath);
        PrintLog("playrecord file with ret=" + err);
    }
    public void Click_btnStopPlayRecordFile()
    {
        //GcloudVoice.GcloudVoiceErrno err;
        //err = m_voiceengine.StopPlayFile ();
        //PrintLog ("stopplay file with ret=" + err);
        m_voiceengine.StopPlayFile();
    }


    public void PrintLog(string str)
    {
        Debug.Log(str);
    }

    public void Click_GetRecFileParam()
    {
        int[] bytes = new int[1];
        bytes[0] = 0;
        float[] seconds = new float[1];
        seconds[0] = 0;
        m_voiceengine.GetFileParam(m_recordpath, bytes, seconds);
        s_strLog += "\r\nfile:" + m_recordpath + "bytes:" + bytes[0] + " seconds:" + seconds[0];
    }
}
