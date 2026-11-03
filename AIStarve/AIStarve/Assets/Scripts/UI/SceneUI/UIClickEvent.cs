using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
/// <summary>
/// 自动绑定按钮点击事件并触发UI事件系统
/// </summary>
public class UIClickEvent : UIBase
{
    [Header("事件配置")]
    public string eventData = "ButtonClicked";
    [Tooltip("点击后要打开的面板名称")]
    public string panelToOpen;

    private Button _button;

    protected override void Awake()
    {
        base.Awake();
        _button = GetComponent<Button>();

        if (_button != null)
        {
            _button.onClick.AddListener(OnButtonClick);
        }
    }

protected virtual void OnButtonClick()
    {
        UIManager.Instance.TriggerEvent(UIEventType.Event.ButtonClick,
            new UIEventType.EventArgs
            {
                PanelName = this.PanelName,
                PanelToOpen = panelToOpen,
                Data = eventData
            });
    }

    private void OnDestroy()
    {
        if (_button != null)
        {
            _button.onClick.RemoveListener(OnButtonClick);
        }
    }
   
}
