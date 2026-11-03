using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 演示如何通过按钮调用UIManager的示例面板
/// </summary>
public class ExamplePanel : UIBase
{
    [Header("UI References")]
    public Button directCallButton;
    public Button eventCallButton;
    public Button inspectorEventButton; // 新增Inspector配置按钮

    private void Start()
    {
        // 方式1：直接调用UIManager方法
        directCallButton.onClick.AddListener(() => {
            UIManager.Instance.ShowPanel("AnotherPanel");
        });

        // 方式2：通过代码绑定事件系统调用
        eventCallButton.onClick.AddListener(() => {
            TriggerButtonClickEvent();
        });
    }

    // 方式3：Inspector配置按钮事件
    public void OnInspectorButtonClick()
    {
        TriggerButtonClickEvent();
        UIManager.Instance.ShowPanel("SettingsPanel");
    }

    /// <summary>
    /// 触发按钮点击事件的通用方法
    /// </summary>
    private void TriggerButtonClickEvent()
    {
        UIManager.Instance.TriggerEvent(UIEventType.Event.ButtonClick,
            new UIEventType.EventArgs {
                PanelName = this.PanelName,
                Data = "ButtonClicked"
            });
    }

    /// <summary>
    /// 注册事件处理器示例
    /// </summary>
    public override void OnShow()
    {
        base.OnShow();
        UIManager.Instance.RegisterEvent(UIEventType.Event.ButtonClick, OnButtonClickEvent);
    }

    /// <summary>
    /// 取消注册事件处理器示例
    /// </summary>
    public override void OnHide()
    {
        base.OnHide();
        UIManager.Instance.UnregisterEvent(UIEventType.Event.ButtonClick, OnButtonClickEvent);
    }

    private void OnButtonClickEvent(UIEventType.EventArgs args)
    {
        if (args.PanelName == this.PanelName)
        {
            Debug.Log($"收到按钮点击事件，数据: {args.Data}");
            
            // 实际业务逻辑示例：
            if (args.Data.ToString() == "ButtonClicked")
            {
                // 执行按钮点击后的操作
            }
        }
    }

    /// <summary>
    /// 供Inspector调用的公共方法
    /// </summary>
    public void PublicMethodForInspector(string message)
    {
        Debug.Log($"Inspector调用: {message}");
        UIManager.Instance.TriggerEvent(UIEventType.Event.ButtonClick,
            new UIEventType.EventArgs {
                PanelName = this.PanelName,
                Data = message
            });
    }
}