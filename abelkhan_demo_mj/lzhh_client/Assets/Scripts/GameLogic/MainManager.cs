using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TinyFrameWork;
using gcloud_voice;
using Assets.Scripts;
public class MainManager
{
    private static MainManager instance;
    public string redBagID;
    public SelfBaseData playerSelfInfo;
    public DontDestroyOnLoad dontDestroyOnLoad;
    public bool isGetAuthKey;
    public bool isStartGvoice;
    public string selfMjBackColorState;
    public int payRate;
    public Int64 bindingID;
    public IGCloudVoice m_voiceengine = null;
    public SceneName nowSceneName;
    public static MainManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new MainManager();
            }
            return instance;
        }
    }

    public void Init()
    {
        selfMjBackColorState = "";
        payRate = 0;
        bindingID = 0;
        nowSceneName = SceneName.no;
        redBagID = "";
    }

  
}

