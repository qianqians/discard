using UnityEngine;
using System.Collections.Generic;

public class GameStateMachine : MonoBehaviour
{
    public enum PanelState
    {
        None,
        MainMenu,
        GamePlay,
        SceneObjectSelect,
        ObjectSelect,
        UIObjectSelect
    }

    private static GameStateMachine _instance;
    public static GameStateMachine Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("PanelStateMachine instance is null!");
            }
            return _instance;
        }
    }

    private Dictionary<PanelState, System.Action> _stateHandlers = new();
    private PanelState _currentState = PanelState.None;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Debug.LogWarning("Duplicate PanelStateMachine detected! Destroying duplicate.");
            Destroy(gameObject);
            return;
        }
        
        _instance = this;
        DontDestroyOnLoad(gameObject);
        Debug.Log("PanelStateMachine initialized successfully.");
        // 初始化状态处理器
        _stateHandlers[PanelState.SceneObjectSelect] = HandleSceneObjectSelect;
        _stateHandlers[PanelState.ObjectSelect] = HandleObjectSelect;
    }
    
    public PanelState GetCurState()
    {
        return _currentState;
    }
    public void ChangeState(PanelState newState)
    {
        if (_currentState == newState) return;

        // 退出当前状态
        switch (_currentState)
        {
            case PanelState.SceneObjectSelect:
                if (newState == PanelState.MainMenu)
                {
                    // 隐藏 SceneObjectSelect 面板
                    if (_sceneObjectSelectPanel != null)
                    {
                        var sceneObjectSelect = _sceneObjectSelectPanel.GetComponent<SceneObjectSelect>();
                        if (sceneObjectSelect != null)
                        {
                            sceneObjectSelect.HidePanel();
                        }
                        else
                        {
                            _sceneObjectSelectPanel.SetActive(false);
                        }
                    }
                }
                break;
        }

        _currentState = newState;
        
        // 进入新状态
        if (_stateHandlers.TryGetValue(newState, out var handler))
        {
            handler?.Invoke();
        }
    }

    private GameObject _sceneObjectSelectPanel;
    
    private void HandleSceneObjectSelect()
    {
        Debug.Log("HandleSceneObjectSelect called");
        
        if (_sceneObjectSelectPanel == null)
        {
            Debug.Log("Loading SceneObjectSelect panel...");
            var prefab = Resources.Load<GameObject>(SceneObjectSelect.PrefabPath);
            if (prefab != null)
            {
                _sceneObjectSelectPanel = Instantiate(prefab);
                _sceneObjectSelectPanel.name = "SceneObjectSelect";
                
                // 获取 Canvas 并设置父对象
                var canvas = FindObjectOfType<Canvas>();
                if (canvas != null)
                {
                    _sceneObjectSelectPanel.transform.SetParent(canvas.transform, false);
                }
                
                Debug.Log("SceneObjectSelect panel instantiated");
            }
            else
            {
                Debug.LogError($"Failed to load prefab: {SceneObjectSelect.PrefabPath}");
            }
        }
        
        if (_sceneObjectSelectPanel != null)
        {
            // 显示面板并调用 ShowPanel 方法
            var sceneObjectSelect = _sceneObjectSelectPanel.GetComponent<SceneObjectSelect>();
            if (sceneObjectSelect != null)
            {
                sceneObjectSelect.ShowPanel();
            }
            else
            {
                _sceneObjectSelectPanel.SetActive(true);
            }
        }
    }
    private void HandleObjectSelect()
    {
        ChangeState(PanelState.ObjectSelect);
        UIManager.Instance.ShowObjectSelection();
    }
    public void OnButtonClick(string panelToOpen)
    {
        if (panelToOpen == nameof(SceneObjectSelect))
        {
            // 如果当前已经是SceneObjectSelect状态则关闭面板
            if (_currentState == PanelState.SceneObjectSelect)
            {
                ChangeState(PanelState.MainMenu);
                Debug.Log("Closing SceneObjectSelect panel");
            }
            else
            {
                ChangeState(PanelState.SceneObjectSelect);
                Debug.Log("Opening SceneObjectSelect panel");
            }
        }else if(panelToOpen == nameof(PanelState.ObjectSelect))
        {

        }
    }
}