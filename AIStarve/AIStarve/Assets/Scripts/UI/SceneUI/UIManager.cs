using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using SceneManagement;
/// <summary>
/// UI管理系统主控制器，负责UI面板的注册、显示、隐藏和消息传递
/// </summary>
public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance => _instance;

    private Dictionary<string, UIPool> _panelPools = new();
    private Dictionary<string, UIBase> _activePanels = new();
    private Stack<UIBase> _panelStack = new();
    private DropPrefabObject _dropPrefabObject;
    private DropTileBrushObject _dropTileBrushObject; // 新增：TileBrush对象拖放支持

    // 事件处理器字典
    private Dictionary<UIEventType.Event, Action<UIEventType.EventArgs>> _eventHandlers =
        new Dictionary<UIEventType.Event, Action<UIEventType.EventArgs>>();
        
    // 新增：事件缓存队列
    private Queue<(UIEventType.Event, UIEventType.EventArgs)> _eventQueue = new();
    private bool _isProcessingEvents = false;

    /// <summary>
    /// 注册UI事件处理器
    /// </summary>
    public void RegisterEvent(UIEventType.Event eventType, Action<UIEventType.EventArgs> handler)
    {
        if (_eventHandlers.ContainsKey(eventType))
        {
            _eventHandlers[eventType] += handler;
        }
        else
        {
            _eventHandlers[eventType] = handler;
        }
    }

    /// <summary>
    /// 取消注册UI事件处理器
    /// </summary>
    public void UnregisterEvent(UIEventType.Event eventType, Action<UIEventType.EventArgs> handler)
    {
        if (_eventHandlers.ContainsKey(eventType))
        {
            _eventHandlers[eventType] -= handler;
        }
    }

    /// <summary>
    /// 触发UI事件（带缓存功能）
    /// </summary>
    public void TriggerEvent(UIEventType.Event eventType, UIEventType.EventArgs args = null, bool immediate = false)
    {
        if (args == null)
        {
            args = new UIEventType.EventArgs();
        }
        args.EventType = eventType;

        // 如果要求立即处理或者当前状态允许处理
        if (immediate || ShouldProcessEvent(eventType))
        {
            ProcessEvent(eventType, args);
        }
        else
        {
            // 否则加入缓存队列
            _eventQueue.Enqueue((eventType, args));
        }
    }

    // 新增：检查当前状态是否应该处理事件
    private bool ShouldProcessEvent(UIEventType.Event eventType)
    {
        if (GameStateMachine.Instance == null)
            return true;
            
        // 根据事件类型和当前状态决定是否处理
        switch (eventType)
        {
            case UIEventType.Event.PanelOpen:
            case UIEventType.Event.PanelClose:
                
            default:
                return true;
        }
    }

    // 新增：处理单个事件
    private void ProcessEvent(UIEventType.Event eventType, UIEventType.EventArgs args)
    {
        _isProcessingEvents = true;
        
        // 原有事件处理逻辑...
        if (eventType == UIEventType.Event.ButtonClick && !string.IsNullOrEmpty(args.PanelToOpen))
        {
            if (args.PanelToOpen == nameof(SceneObjectSelect))
            {
                if (GameStateMachine.Instance != null)
                {
                    GameStateMachine.Instance.OnButtonClick(args.PanelToOpen);
                }
            }
            else
            {
                ShowPanel(args.PanelToOpen);
            }
        }
        else if (eventType == UIEventType.Event.TilemapToolChanged && (!string.IsNullOrEmpty(args.PrefabPath) || args.PrefabPathId != 0))
        {
            if (GameStateMachine.Instance != null && _dropPrefabObject != null)
            {
                GameStateMachine.Instance.ChangeState(GameStateMachine.PanelState.ObjectSelect);
                if (args.PointerData != null)
                {
                    if (args.ItemType == TilemapItemType.GroundTilemapItem)
                    {
                        if(args.Brush.tile == null)
                        {
                            string stringPrefabPath = GameSceneManager.Instance.SceneDB.GetPrefabPath(args.PrefabPathId);
                            TileBase tile = Resources.Load<TileBase>(stringPrefabPath);
                            if(tile != null)
                            {
                                args.Brush.tile = tile;
                            }
                        }
                        string tilemapTarget = GameSceneManager.Instance.SceneDB.GetTilemapTarget(args.PrefabPathId);
                        GameObject tilemaTarget = GameObject.Find(tilemapTarget);
                        if (tilemaTarget == null)
                        {
                            Debug.LogError("Ground Tilemap not found in scene");
                            return;
                        }

                        Tilemap groundTilemap = tilemaTarget.GetComponent<Tilemap>();
                        if (groundTilemap == null)
                        {
                            Debug.LogError("Ground Tilemap object has no Tilemap component");
                            return;
                        }
                        args.TargetTilemap = groundTilemap;
                        _dropTileBrushObject.OnBeginDrag(args.PointerData);

                        _dropTileBrushObject.StartDrag(args.Brush, args.TargetTilemap, args.PrefabPathId);
                    }else if(args.ItemType == TilemapItemType.ObjectsTilemapItem || args.ItemType == TilemapItemType.ObjectsTilemapGrowUpItem)
                    {
                        string stringPrefabPath = GameSceneManager.Instance.SceneDB.GetPrefabPath(args.PrefabPathId);
                        args.PrefabPath = stringPrefabPath;
                        _dropPrefabObject.OnBeginDrag(args.PointerData);
                        _dropPrefabObject.StartDrag(args.PrefabPath, args.PrefabPathId);
                    }
                    
                }
                else
                {
                    Debug.LogError("PointerData is null!");
                }
            }
           
            else
            {
                Debug.LogError("PanelStateMachine instance or _dropPrefabObject is null!");
            }
        }
        
        else if (eventType == UIEventType.Event.TilemapToolChangedEnd)
        {
            if (args.ItemType == TilemapItemType.ObjectsTilemapItem || args.ItemType == TilemapItemType.ObjectsTilemapGrowUpItem)
            {
                if (_dropPrefabObject != null)
                {
                    // 如果状态机为UIObjectSelect，触发SetButtomImage事件
                    if (GameStateMachine.Instance != null && GameStateMachine.Instance.GetCurState() == GameStateMachine.PanelState.UIObjectSelect)
                    {
                        TriggerEvent(UIEventType.Event.SetButtomImage, new UIEventType.EventArgs { PrefabPathId = args.PrefabPathId },true);
                    }else
                    {
                        _dropPrefabObject.OnEndDrag(args.PointerData);
                    }
                    
                    
                }
               
            }else if(args.ItemType == TilemapItemType.GroundTilemapItem)
            {
                // 新增：处理TileBrush结束拖动
                if (_dropTileBrushObject != null)
                {
                    if (GameStateMachine.Instance != null && GameStateMachine.Instance.GetCurState() == GameStateMachine.PanelState.UIObjectSelect)
                    {
                        TriggerEvent(UIEventType.Event.SetButtomImage, new UIEventType.EventArgs { PrefabPathId = args.PrefabPathId },true);
                    }
                    else
                    {
                        _dropTileBrushObject.OnEndDrag(args.PointerData);
                    }
                }
            }
        }
        else if (eventType == UIEventType.Event.TilemapToolDragging)
        {
            if (args.ItemType == TilemapItemType.ObjectsTilemapItem || args.ItemType == TilemapItemType.ObjectsTilemapGrowUpItem)
            {
                _dropPrefabObject.OnDrag(args.PointerData);
            }
            else if (args.ItemType == TilemapItemType.GroundTilemapItem)
            {
                _dropTileBrushObject.OnDrag(args.PointerData);
            }
        }
        
            
      
        // 其他事件处理...

        if (_eventHandlers.TryGetValue(eventType, out var handler))
        {
            handler?.Invoke(args);
        }
        
        _isProcessingEvents = false;
    }

    // 新增：处理缓存队列中的事件
    public void ProcessQueuedEvents()
    {
        if (_isProcessingEvents || _eventQueue.Count == 0)
            return;
            
        while (_eventQueue.Count > 0)
        {
            var (eventType, args) = _eventQueue.Dequeue();
            if (ShouldProcessEvent(eventType))
            {
                ProcessEvent(eventType, args);
            }
            else
            {
                // 如果还不能处理，重新放回队列头部
                var remainingEvents = new List<(UIEventType.Event, UIEventType.EventArgs)>();
                remainingEvents.Add((eventType, args));
                while (_eventQueue.Count > 0)
                {
                    remainingEvents.Add(_eventQueue.Dequeue());
                }
                foreach (var e in remainingEvents)
                {
                    _eventQueue.Enqueue(e);
                }
                break;
            }
        }
    }

    private void Update()
    {
        // 每帧检查并处理缓存事件
        ProcessQueuedEvents();
    }

    // 其余原有方法保持不变...
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        _dropPrefabObject = gameObject.AddComponent<DropPrefabObject>();
        _dropTileBrushObject = gameObject.AddComponent<DropTileBrushObject>(); // 添加TileBrush支持
        DontDestroyOnLoad(gameObject);
    }

    public void RegisterPanel(string panelName, string prefabPath, int poolSize = 1)
    {
        if (!_panelPools.ContainsKey(panelName))
        {
            _panelPools[panelName] = new UIPool(panelName, prefabPath, poolSize);
        }
    }
    
     /// <summary>
    /// 从对象池获取面板实例
    /// </summary>
    /// <param name="panelName">要获取的面板名称</param>
    /// <returns>面板实例，如果未注册返回null</returns>
    public UIBase GetPanel(string panelName)
    {
        if (_panelPools.TryGetValue(panelName, out var pool))
        {
            return pool.Get();
        }
        return null;
    }

    /// <summary>
    /// 获取已激活的面板实例
    /// </summary>
    /// <param name="panelName">面板名称</param>
    /// <returns>已激活的面板实例，如果未注册或没有激活实例返回null</returns>
    public UIBase GetActivePanel(string panelName)
    {
        if (_panelPools.TryGetValue(panelName, out var pool))
        {
            return pool.GetFromActive();
        }
        return null;
    }

    /// <summary>
    /// 释放面板实例回对象池
    /// </summary>
    /// <param name="panelName">面板名称</param>
    /// <param name="panel">要释放的面板实例</param>
    public void ReleasePanel(string panelName, UIBase panel)
    {
        if (_panelPools.TryGetValue(panelName, out var pool))
        {
            pool.Release(panel);
        }
    }

    /// <summary>
    /// 显示指定面板
    /// </summary>
    /// <param name="panelName">要显示的面板名称</param>
    public void ShowPanel(string panelName)
    {
        Debug.Log($"UIManager.ShowPanel called for: {panelName}");
        
        var panel = GetPanel(panelName);
        if (panel != null)
        {
            Debug.Log($"UIManager.ShowPanel: Got panel {panel.name}, calling OnShow()");
            _panelStack.Push(panel);
            _activePanels[panelName] = panel;
            panel.OnShow();
            TriggerEvent(UIEventType.Event.PanelOpen,
                new UIEventType.EventArgs { PanelName = panelName });
        }
    }

    /// <summary>
    /// 通过类型显示面板
    /// </summary>
    /// <typeparam name="T">面板类型</typeparam>
    public void ShowPanel<T>() where T : UIBase
    {
        ShowPanel(typeof(T).Name);
    }

    /// <summary>
    /// 关闭当前显示的面板
    /// </summary>
    public void CloseCurrentPanel()
    {
        if (_panelStack.Count > 0)
        {
            var panel = _panelStack.Pop();
            panel.OnHide();
            TriggerEvent(UIEventType.Event.PanelClose,
                new UIEventType.EventArgs { PanelName = panel.PanelName });
            ReleasePanel(panel.PanelName, panel);
            _activePanels.Remove(panel.PanelName);

            if (_panelStack.Count > 0)
            {
                _panelStack.Peek().OnShow();
            }
        }
    }
    /// <summary>
    /// 获取指定面板的RectTransform
    /// </summary>
    /// <param name="panelName">面板名称</param>
    /// <returns>面板的RectTransform，如果不存在返回null</returns>
    public RectTransform GetPanelTransform(string panelName)
    {
        if (_activePanels.TryGetValue(panelName, out var panel))
        {
            return panel.GetComponent<RectTransform>();
        }
        return null;
    }
    public void ShowObjectSelection()
    { 
    }
    // 其他原有方法...
}