using System.Collections;
using TinyFrameWork;
using UnityEngine;
using UnityEngine.UI;

using System.Collections.Generic;
public class UISettingPanel : MonoBehaviour {

    public GameObject SettingPanel = null;
    public Button CloseSettingPanelBtn = null;
    public Slider mainSlider;
    public Slider gameSoundSlider;
    public Text SoundTxt;
   // public Button setBtn = null;
    public List<Toggle> colorToggle = null;
    private string colorIndex;
    // Use this for initialization
    void Start () {
        mainSlider.onValueChanged.AddListener(OnSetSound);
        gameSoundSlider.onValueChanged.AddListener(OnSetGameSound);
        CloseSettingPanelBtn.onClick.AddListener(delegate ()
        {
            OnCloseSettingPanel(gameObject);
        });
        SetColor();
        EventDispatcher.GetInstance().UIFrameWorkEventManager.AddEventListener<bool>(EventId.UIFrameWork_Control_settingPanel, OnSetPanel);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetColor()
    {
        var toggles = FindObjectsOfType<Toggle>();
     //   string name;
        if (toggles != null)
        {
            foreach (var toggle in toggles)
            {
                if (toggle.isOn)
                {
                    colorIndex = (colorToggle.IndexOf(toggle)+1).ToString();
                    PlayerPrefs.SetString("MjBackColor", colorIndex);
                    EventDispatcher.GetInstance().UIFrameWorkEventManager.TriggerEvent<string>(EventId.UIFrameWork_Set_mj_back, colorIndex);
                }
            }
        }
    }

    public void OnCloseSettingPanel(GameObject obj)
    {
        SettingPanel.SetActive(false);
    }

    private void OnSetPanel(bool state)
    {
        SettingPanel.SetActive(state);
    }

    public void OnSetSound(float value)
    {
        SoundManager.Instance.SetVolume(ESoundLayer.Background, value);
        PlayerPrefs.SetFloat("Background_sound", value);
    }

    private void OnSetGameSound(float value)
    {
        SoundManager.Instance.SetVolume(ESoundLayer.Effect, value);
      //  PlayerPrefs.SetFloat("Background_sound", 0.7f);
        PlayerPrefs.SetFloat("Effect_sound", value);
    }
}
