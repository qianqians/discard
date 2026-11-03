using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LeftBottomPanel : UIBase
{
    public const string PrefabPath = "Perfabs/UI/LeftBottomPanel"; // 直接放在Resources根目录
    private RectTransform _rectTransform = null;

    public override void Start()
    {
        base.Start();
        var panel = UIManager.Instance.GetActivePanel("LeftBottomPanel");
        _rectTransform = panel?.GetComponent<RectTransform>();
        
        OnWindowSizeChanged();

    }
    public override void OnCreate()
    {
        base.OnCreate();
        
    }

    private float _lastUpdateTime;
    private Vector2 _lastWindowSize;

    private void UpdatePanelPosition()
    {
        // 限制执行频率：每0.5秒最多执行一次
        if (Time.time - _lastUpdateTime < 0.5f)
        {
            return;
        }

        // 检查窗口尺寸是否真的变化了
        Vector2 currentWindowSize = new Vector2(Screen.width, Screen.height);
        if (currentWindowSize == _lastWindowSize)
        {
            return;
        }

        if (_rectTransform != null)
        {
            // 获取窗口尺寸
            float windowHeight = currentWindowSize.y;
            
            _rectTransform.offsetMin = new Vector2(0, 50); // left, bottom
            _rectTransform.offsetMax = new Vector2(-322, -(windowHeight /2) -50); // right, top

            _lastWindowSize = currentWindowSize;
            _lastUpdateTime = Time.time;
        }
    }
    private Coroutine _resizeCoroutine;
    
    protected override void OnWindowSizeChanged()
    {
        // 如果已有延迟协程在运行，先停止它
        if (_resizeCoroutine != null)
        {
            StopCoroutine(_resizeCoroutine);
        }
        
        // 启动新的延迟协程
        _resizeCoroutine = StartCoroutine(DelayedUpdatePosition());
    }
    
    private IEnumerator DelayedUpdatePosition()
    {
        // 等待0.5秒没有新的尺寸变化后再执行
        yield return new WaitForSeconds(0.5f);
        UpdatePanelPosition();
        _resizeCoroutine = null;
    }

}