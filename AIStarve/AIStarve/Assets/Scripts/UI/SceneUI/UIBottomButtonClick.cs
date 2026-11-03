using SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using Abelkhan;

/// <summary>
/// 底部按钮点击事件类，继承UIClickEvent并实现拖拽和指针进入事件
/// </summary>
public class UIBottomButtonClick : UIClickEvent,   IPointerEnterHandler, IPointerExitHandler
{
    [Header("按钮配置")]
    [Tooltip("按钮ID，用于从SceneDataBase读取数据")]
    public int buttonId = 0;
    
    
private GameStateMachine _stateMachine;
    private bool _isDragging = false;
    [SerializeField]
    public Transform _iconChild;
    private Image _iconImage;
    private bool bactive = false;
    private item_def _currentItem;
    private TextMeshProUGUI _textCount;

protected override void Awake()
    {
        base.Awake();
        
        _stateMachine = GameStateMachine.Instance;
       
        _iconImage = _iconChild.GetComponent<Image>();
        if (_iconImage == null)
        {
            Debug.LogWarning("UIBottomButtonClick: 未找到Image组件，请手动分配iconImage或确保GameObject或其子对象包含Image组件。");
        }

UIManager.Instance.RegisterEvent(UIEventType.Event.SetButtomImage, OnEndDrag);
        UIManager.Instance.RegisterEvent(UIEventType.Event.KeyboardMappingUpdated, OnKeyboardMappingUpdated);
    }
    
    private void Start()
    {
        InitializeTextCount();
        
        var sceneManager = GameSceneManager.Instance;
        if (sceneManager != null && sceneManager.SceneDB != null)
        {
            LoadIconByButtonId();
        }
        else
        {
            GameSceneManager.Instance.RegisterEvent(SceneEvent.DBInitFinish, OnDBInit);
        }
    }
    
    public void OnDBInit(SceneEventArgs args)
    {
        LoadIconByButtonId();
    }
    
private void InitializeTextCount()
    {
        Transform textCountTransform = FindChildRecursive(transform, "TextCount");
        if (textCountTransform != null)
        {
            _textCount = textCountTransform.GetComponent<TextMeshProUGUI>();
        }
    }
    
    private Transform FindChildRecursive(Transform parent, string name)
    {
        foreach (Transform child in parent)
        {
            if (child.name == name) return child;
            var found = FindChildRecursive(child, name);
            if (found != null) return found;
        }
        return null;
    }

/// <summary>
    /// 根据buttonId从键盘映射加载贴图
    /// </summary>
    private void LoadIconByButtonId()
    {
        if (buttonId <= 0) return;
        var sceneManager = GameSceneManager.Instance;
        if (sceneManager == null) return;
        
        var mapping = sceneManager.SceneDB.GetKeyboardMapping(buttonId.ToString());
        if (mapping.HasValue && mapping.Value.prefabId > 0)
        {
            LoadIconFromSceneDataBase(mapping.Value.prefabId);
            UpdateAmountDisplay(mapping.Value.amount);
        }
        else
        {
            UpdateAmountDisplay(0);
        }
    }

    /// <summary>
    /// 指针进入事件处理
    /// </summary>
    public  void OnPointerEnter(PointerEventData eventData)
    {
        
        // 设置状态机为UIObjectSelect
        if (_stateMachine != null)
        {
            _stateMachine.ChangeState(GameStateMachine.PanelState.UIObjectSelect);
            bactive = true;
            //Debug.Log($"UIBottomButtonClick: 状态机设置为 UIObjectSelect, 按钮ID: {buttonId}");
        }
        else
        {
            //Debug.LogWarning("UIBottomButtonClick: 状态机实例为空");
        }
    }

    /// <summary>
    /// 拖拽事件处理
    /// </summary>
    public void OnDragEnd(PointerEventData eventData)
    {
        _isDragging = true;
    }

/// <summary>
    /// 拖拽结束事件处理
    /// </summary>
    public void OnEndDrag(UIEventType.EventArgs args)
    {
        if (args.TargetButtonId != buttonId) return;
        if (args.PrefabPathId <= 0) return;
        
        int prefabId = args.PrefabPathId;
        LoadIconFromSceneDataBase(prefabId);
        
        var sceneManager = GameSceneManager.Instance;
        if (sceneManager != null)
        {
            if (args.ItemData != null)
            {
                _currentItem = args.ItemData;
                sceneManager.SceneDB.UpdateKeyboardMapping(
                    buttonId.ToString(),
                    prefabId,
                    args.ItemData.uuid,
                    (int)args.ItemData.amount);
                Debug.Log($"Saved keyboard mapping: {buttonId} -> prefabId:{prefabId}, uuid:{args.ItemData.uuid}, amount:{args.ItemData.amount}");
                UpdateAmountDisplay((int)args.ItemData.amount);
            }
            else
            {
                sceneManager.SceneDB.UpdateKeyboardMapping(buttonId.ToString(), prefabId);
                Debug.Log($"Saved keyboard mapping: {buttonId} -> {prefabId}");
                UpdateAmountDisplay(0);
            }
        }
        else
        {
            Debug.LogWarning("GameSceneManager instance is null, cannot update keyboard mapping.");
        }
    }
    
private void UpdateAmountDisplay(int amount)
    {
        if (_textCount == null)
        {
            InitializeTextCount();
        }
        
        if (_textCount == null) return;
        
        if (amount > 1)
        {
            _textCount.text = amount.ToString();
            _textCount.gameObject.SetActive(true);
        }
        else
        {
            _textCount.gameObject.SetActive(false);
        }
    }
    
    private void OnKeyboardMappingUpdated(UIEventType.EventArgs args)
    {
        if (args.KeyIndex != buttonId) return;
        
        var sceneManager = GameSceneManager.Instance;
        if (sceneManager == null) return;
        
        var mapping = sceneManager.SceneDB.GetKeyboardMapping(buttonId.ToString());
        if (mapping.HasValue)
        {
            UpdateAmountDisplay(mapping.Value.amount);
        }
    }
    
    protected override void OnButtonClick()
    {
        var sceneManager = GameSceneManager.Instance;
        if (sceneManager == null || sceneManager.SceneDB == null)
        {
            base.OnButtonClick();
            return;
        }
        
        var mapping = sceneManager.SceneDB.GetKeyboardMapping(buttonId.ToString());
        if (!mapping.HasValue || mapping.Value.prefabId <= 0)
        {
            base.OnButtonClick();
            return;
        }
        
        int prefabId = mapping.Value.prefabId;
        int prefabType = sceneManager.SceneDB.GetPrefabType(prefabId);
        
        if (prefabType == (int)TilemapItemType.ObjectsTilemapGrowUpItem ||
            prefabType == (int)TilemapItemType.ObjectsTilemapItem)
        {
            UIManager.Instance.TriggerEvent(UIEventType.Event.ToolKeyPressed,
                new UIEventType.EventArgs { KeyIndex = buttonId }, true);
        }
        else
        {
            base.OnButtonClick();
        }
    }
    
private void LoadFromKeyboardMapping()
    {
        if (buttonId <= 0) return;
        var sceneManager = GameSceneManager.Instance;
        if (sceneManager == null) return;
        
        var mapping = sceneManager.SceneDB.GetKeyboardMapping(buttonId.ToString());
        if (mapping.HasValue && mapping.Value.prefabId > 0)
        {
            LoadIconFromSceneDataBase(mapping.Value.prefabId);
            UpdateAmountDisplay(mapping.Value.amount);
        }
        else
        {
            UpdateAmountDisplay(0);
        }
    }

/// <summary>
    /// 从路径加载精灵，支持 Sprite 贴图或 Prefab
    /// </summary>
    private Sprite LoadSpriteFromPath(string iconPath)
    {
        // 1. 尝试直接加载 Sprite
        Sprite sprite = Resources.Load<Sprite>(iconPath);
        if (sprite != null)
        {
            return sprite;
        }
        
        // 2. 尝试加载 Prefab，获取其 SpriteRenderer.sprite
        GameObject prefab = Resources.Load<GameObject>(iconPath);
        if (prefab != null)
        {
            SpriteRenderer sr = prefab.GetComponent<SpriteRenderer>();
            if (sr != null && sr.sprite != null)
            {
                return sr.sprite;
            }
        }
        
        // 未找到
        Debug.LogWarning($"LoadSpriteFromPath: 无法加载精灵，路径: {iconPath}");
        return null;
    }

    /// <summary>
    /// 从SceneDataBase加载图标贴图
    /// </summary>
    private void LoadIconFromSceneDataBase(int perfabID)
    {
        if (perfabID <= 0)
        {
            Debug.LogWarning("UIBottomButtonClick: 按钮ID无效，无法从SceneDataBase读取数据");
            return;
        }

        // 获取GameSceneManager实例
        var sceneManager = GameSceneManager.Instance;
        if (sceneManager == null)
        {
            Debug.LogError("UIBottomButtonClick: GameSceneManager实例为空");
            return;
        }

// 从SceneDataBase获取贴图路径
        string iconPath = sceneManager.SceneDB.GetIconPath(perfabID);
        if (string.IsNullOrEmpty(iconPath))
        {
            Debug.LogError($"UIBottomButtonClick: 无法从SceneDataBase获取ID为 {buttonId} 的贴图路径");
            return;
        }

        // 加载贴图资源
        Sprite iconSprite = LoadSpriteFromPath(iconPath);
        if (iconSprite == null)
        {
            Debug.LogError($"UIBottomButtonClick: 无法加载贴图资源，路径: {iconPath}");
            return;
        }

        // 设置贴图到Image组件
        if (_iconImage != null)
        {
            _iconImage.sprite = iconSprite;
            Debug.Log($"UIBottomButtonClick: 成功加载并设置贴图，路径: {iconPath}");
        }
        else
        {
            Debug.LogError("UIBottomButtonClick: iconImage组件为空，无法设置贴图");
        }
    }

    /// <summary>
    /// 手动设置按钮ID并立即加载图标
    /// </summary>
    public void SetButtonId(int id)
    {
        buttonId = id;
        
    }

    /// <summary>
    /// 获取当前按钮ID
    /// </summary>
    public int GetButtonId()
    {
        return buttonId;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        bactive = false;
    }
    /// <summary>
    /// 销毁时取消注册事件
    /// </summary>
private void OnDestroy()
    {
        UIManager.Instance.UnregisterEvent(UIEventType.Event.SetButtomImage, OnEndDrag);
        UIManager.Instance.UnregisterEvent(UIEventType.Event.KeyboardMappingUpdated, OnKeyboardMappingUpdated);
    }
}