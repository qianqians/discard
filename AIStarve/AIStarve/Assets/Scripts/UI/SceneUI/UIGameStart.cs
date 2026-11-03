using UnityEngine;
using SceneManagement;
public class UIGameStart : MonoBehaviour
{
    void Start()
    {
        // 确保UIManager实例存在
        if (UIManager.Instance == null)
        {
            var go = new GameObject("UIManager");
            go.AddComponent<UIManager>();
        }
        
        // 注册并显示左下角面板
        try
        {
            GameStateMachine.Instance.ChangeState(GameStateMachine.PanelState.MainMenu);
            GameSceneManager.Instance.ProcessQueuedEvents();
            var prefab = Resources.Load<GameObject>(LeftBottomPanel.PrefabPath);
            if (prefab == null)
            {
                Debug.LogError($"Prefab not found at path: {LeftBottomPanel.PrefabPath}");
                Debug.LogError("请确保：\n1. Prefab路径正确\n2. Prefab放在Resources文件夹内");
                return;
            }

            UIManager.Instance.RegisterPanel(
                nameof(LeftBottomPanel),
                LeftBottomPanel.PrefabPath,
                1
            );
            UIManager.Instance.ShowPanel<LeftBottomPanel>();
        }
        catch (System.Exception e)
        {
            Debug.LogError($"UI初始化失败: {e.Message}");
        }
    }
}