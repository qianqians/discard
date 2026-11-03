using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 2D触发区域检测脚本
/// 当有对象进入或离开碰撞器范围时触发相应事件
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class TriggerArea2D : MonoBehaviour
{
    #region 事件定义
    
    /// <summary>
    /// 当有对象进入触发区域时调用
    /// </summary>
    public event System.Action<Collider2D> OnObjectEntered;
    
    /// <summary>
    /// 当有对象离开触发区域时调用
    /// </summary>
    public event System.Action<Collider2D> OnObjectExited;
    
    /// <summary>
    /// 当有对象停留在触发区域内时调用（每帧调用）
    /// </summary>
    public event System.Action<Collider2D> OnObjectStay;
    
    #endregion
    
    #region 序列化字段
    
    [Header("触发区域设置")]
    [Tooltip("是否启用触发检测")]
    [SerializeField] private bool isEnabled = true;
    
    [Tooltip("触发器的标签过滤（为空则检测所有标签）")]
    [SerializeField] private string[] targetTags = new string[0];
    
    [Tooltip("是否在控制台输出调试信息")]
    [SerializeField] private bool debugLog = false;
    
    [Header("碰撞器引用")]
    [Tooltip("用于检测的碰撞器（如果为空则使用GetComponent获取）")]
    [SerializeField] private Collider2D triggerCollider;
    
    #endregion
    
    #region 私有字段
    
    // 当前在区域内的对象列表
    private List<Collider2D> objectsInArea = new List<Collider2D>();
    
    #endregion
    
    #region 属性
    
    /// <summary>
    /// 获取或设置是否启用触发检测
    /// </summary>
    public bool IsEnabled
    {
        get { return isEnabled; }
        set { isEnabled = value; }
    }
    
    /// <summary>
    /// 获取当前在区域内的对象数量
    /// </summary>
    public int ObjectCount
    {
        get { return objectsInArea.Count; }
    }
    
    /// <summary>
    /// 获取当前在区域内的所有对象
    /// </summary>
    public List<Collider2D> ObjectsInArea
    {
        get { return new List<Collider2D>(objectsInArea); }
    }
    
    #endregion
    
    #region Unity生命周期方法
    
    private void Awake()
    {
        // 如果没有指定碰撞器，则尝试获取
        if (triggerCollider == null)
        {
            triggerCollider = GetComponent<Collider2D>();
        }
        
        // 确保碰撞器是触发器
        if (triggerCollider != null)
        {
            triggerCollider.isTrigger = true;
        }
        else
        {
            Debug.LogError($"TriggerArea2D: 在游戏对象 {gameObject.name} 上找不到Collider2D组件！");
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isEnabled) return;
        
        // 检查标签过滤
        if (!IsTagAllowed(other.tag)) return;
        
        // 添加到列表
        if (!objectsInArea.Contains(other))
        {
            objectsInArea.Add(other);
            
            if (debugLog)
            {
                Debug.Log($"TriggerArea2D: 对象 {other.gameObject.name} 进入区域 {gameObject.name}");
            }
            
            // 触发进入事件
            OnObjectEntered?.Invoke(other);
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (!isEnabled) return;
        
        // 检查标签过滤
        if (!IsTagAllowed(other.tag)) return;
        
        // 从列表中移除
        if (objectsInArea.Contains(other))
        {
            objectsInArea.Remove(other);
            
            if (debugLog)
            {
                Debug.Log($"TriggerArea2D: 对象 {other.gameObject.name} 离开区域 {gameObject.name}");
            }
            
            // 触发离开事件
            OnObjectExited?.Invoke(other);
        }
    }
    
    private void OnTriggerStay2D(Collider2D other)
    {
        if (!isEnabled) return;
        
        // 检查标签过滤
        if (!IsTagAllowed(other.tag)) return;
        
        // 触发停留事件
        OnObjectStay?.Invoke(other);
    }
    
    #endregion
    
    #region 公共方法
    
    /// <summary>
    /// 检查指定标签是否在允许的标签列表中
    /// </summary>
    /// <param name="tag">要检查的标签</param>
    /// <returns>如果允许则返回true</returns>
    public bool IsTagAllowed(string tag)
    {
        // 如果没有设置目标标签，则允许所有标签
        if (targetTags == null || targetTags.Length == 0)
            return true;
        
        // 检查标签是否在允许列表中
        foreach (string allowedTag in targetTags)
        {
            if (allowedTag == tag)
                return true;
        }
        
        return false;
    }
    
    /// <summary>
    /// 检查指定游戏对象是否在触发区域内
    /// </summary>
    /// <param name="gameObject">要检查的游戏对象</param>
    /// <returns>如果在区域内则返回true</returns>
    public bool ContainsGameObject(GameObject gameObject)
    {
        if (gameObject == null) return false;
        
        Collider2D collider = gameObject.GetComponent<Collider2D>();
        if (collider == null) return false;
        
        return objectsInArea.Contains(collider);
    }
    
    /// <summary>
    /// 清空区域内的所有对象（不会触发离开事件）
    /// </summary>
    public void ClearObjects()
    {
        objectsInArea.Clear();
    }
    
    /// <summary>
    /// 获取区域内指定类型的组件
    /// </summary>
    /// <typeparam name="T">组件类型</typeparam>
    /// <returns>找到的组件列表</returns>
    public List<T> GetComponentsInArea<T>() where T : Component
    {
        List<T> components = new List<T>();
        
        foreach (Collider2D collider in objectsInArea)
        {
            T component = collider.GetComponent<T>();
            if (component != null)
            {
                components.Add(component);
            }
        }
        
        return components;
    }
    
    /// <summary>
    /// 获取区域内指定标签的游戏对象
    /// </summary>
    /// <param name="tag">标签名称</param>
    /// <returns>找到的游戏对象列表</returns>
    public List<GameObject> GetGameObjectsWithTag(string tag)
    {
        List<GameObject> result = new List<GameObject>();
        
        foreach (Collider2D collider in objectsInArea)
        {
            if (collider.CompareTag(tag))
            {
                result.Add(collider.gameObject);
            }
        }
        
        return result;
    }
    
    #endregion
    
    #region 编辑器方法
    
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (triggerCollider == null)
            triggerCollider = GetComponent<Collider2D>();
        
        if (triggerCollider == null) return;
        
        // 绘制触发区域
        Gizmos.color = new Color(0, 1, 0, 0.3f);
        
        // 根据碰撞器类型绘制不同的形状
        if (triggerCollider is BoxCollider2D boxCollider)
        {
            Gizmos.matrix = Matrix4x4.TRS(
                transform.position + (Vector3)boxCollider.offset,
                transform.rotation,
                transform.lossyScale
            );
            Gizmos.DrawCube(Vector3.zero, boxCollider.size);
        }
        else if (triggerCollider is CircleCollider2D circleCollider)
        {
            Gizmos.matrix = Matrix4x4.TRS(
                transform.position + (Vector3)circleCollider.offset,
                transform.rotation,
                transform.lossyScale
            );
            Gizmos.DrawSphere(Vector3.zero, circleCollider.radius);
        }
        else if (triggerCollider is PolygonCollider2D polygonCollider)
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            Vector3[] vertices = new Vector3[polygonCollider.points.Length];
            for (int i = 0; i < polygonCollider.points.Length; i++)
            {
                vertices[i] = polygonCollider.points[i];
            }
            UnityEditor.Handles.DrawAAConvexPolygon(vertices);
        }
        
        Gizmos.matrix = Matrix4x4.identity;
    }
#endif
    
    #endregion
}