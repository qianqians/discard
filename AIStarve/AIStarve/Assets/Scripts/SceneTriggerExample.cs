using UnityEngine;
using UnityEngine.SceneManagement;
using SceneManagement;

/// <summary>
/// 场景触发切换示例脚本
/// 将此脚本附加到带有TriggerArea2D组件的游戏对象上
/// 当玩家进入触发区域时，自动切换到对应的场景
/// </summary>
[RequireComponent(typeof(TriggerArea2D))]
public class SceneTriggerExample : MonoBehaviour
{
    [Tooltip("SceneDataBase实例，如果为空则使用单例或查找")]
    [SerializeField] private SceneDataBase sceneDataBase;

    [Tooltip("是否在进入时立即切换场景")]
    [SerializeField] private bool switchOnEnter = true;

    [Tooltip("切换场景前的延迟时间（秒）")]
    [SerializeField] private float delay = 0f;

    private void Start()
    {
        // 如果没有指定SceneDataBase，尝试查找或使用单例
        if (sceneDataBase == null)
        {
            // 尝试从GameSceneManager获取
            if (GameSceneManager.Instance != null)
            {
                sceneDataBase = GameSceneManager.Instance.SceneDB;
                if (sceneDataBase != null)
                {
                    Debug.Log("SceneDataBase obtained from GameSceneManager.Instance.SceneDB");
                }
            }

            // 如果仍然为null，尝试查找SceneDataBase实例
            if (sceneDataBase == null)
            {
                
                Debug.Log("SceneDataBase found via FindObjectOfType");
               
            }

            if (sceneDataBase == null)
            {
                Debug.LogWarning("SceneDataBase not assigned and could not be found automatically. Scene switching will not work.");
            }
        }

        // 获取TriggerArea2D组件并订阅事件
        TriggerArea2D triggerArea = GetComponent<TriggerArea2D>();
        if (triggerArea != null)
        {
            triggerArea.OnObjectEntered += OnObjectEnteredTriggerArea;
        }
    }

    private void OnObjectEnteredTriggerArea(Collider2D other)
    {
        // 检查进入的对象是否是玩家（根据标签或其他标识）
        if (!other.CompareTag("Player")) return;

        if (switchOnEnter)
        {
            if (delay > 0)
            {
                Invoke(nameof(SwitchScene), delay);
            }
            else
            {
                SwitchScene();
            }
        }
    }

    private void SwitchScene()
    {
        if (sceneDataBase == null)
        {
            // 尝试从GameSceneManager获取
            if (GameSceneManager.Instance != null)
            {
                sceneDataBase = GameSceneManager.Instance.SceneDB;
            }

            

            if (sceneDataBase == null)
            {
                Debug.LogError("SceneDataBase is not assigned and could not be found. Cannot switch scene.");
                return;
            }
        }

        // 使用当前游戏对象的名称查找对应的场景名称
        string sceneName = sceneDataBase.GetSceneNameByGameObjectName(gameObject.name);

        if (!string.IsNullOrEmpty(sceneName))
        {
            // 移除场景名称中的.unity扩展名（如果需要）
            sceneName = System.IO.Path.GetFileNameWithoutExtension(sceneName);
            Debug.Log($"Switching to scene: {sceneName}");
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError($"No scene mapping found for GameObject: {gameObject.name}");
        }
    }

    private void OnDestroy()
    {
        TriggerArea2D triggerArea = GetComponent<TriggerArea2D>();
        if (triggerArea != null)
        {
            triggerArea.OnObjectEntered -= OnObjectEnteredTriggerArea;
        }
    }
}