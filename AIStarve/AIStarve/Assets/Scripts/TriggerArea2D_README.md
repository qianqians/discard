# TriggerArea2D 使用说明

## 概述

`TriggerArea2D` 是一个Unity 2D触发区域检测脚本，用于检测对象进入和离开碰撞器范围的事件。它提供了事件订阅、标签过滤、调试信息等功能。

## 文件说明

1. **TriggerArea2D.cs** - 主脚本，核心触发检测功能
2. **TriggerAreaExample.cs** - 使用示例，展示如何订阅事件
3. **SimpleTriggerUsage.cs** - 简单使用示例，展示在其他脚本中如何使用

## 使用方法

### 1. 基本设置

1. 在Unity编辑器中，创建一个带有2D碰撞器的游戏对象
2. 添加 `TriggerArea2D` 组件
3. 确保碰撞器的 `Is Trigger` 属性被勾选

### 2. 配置选项

- **Is Enabled**: 启用/禁用触发检测
- **Target Tags**: 指定要检测的标签（为空则检测所有标签）
- **Debug Log**: 在控制台输出调试信息
- **Trigger Collider**: 指定用于检测的碰撞器（如果为空则使用GetComponent获取）

### 3. 事件订阅

在需要响应触发事件的脚本中，订阅 `TriggerArea2D` 的事件：

```csharp
public class MyScript : MonoBehaviour
{
    [SerializeField] private TriggerArea2D triggerArea;
    
    private void Start()
    {
        if (triggerArea != null)
        {
            triggerArea.OnObjectEntered += OnObjectEntered;
            triggerArea.OnObjectExited += OnObjectExited;
            triggerArea.OnObjectStay += OnObjectStay;
        }
    }
    
    private void OnDestroy()
    {
        if (triggerArea != null)
        {
            triggerArea.OnObjectEntered -= OnObjectEntered;
            triggerArea.OnObjectExited -= OnObjectExited;
            triggerArea.OnObjectStay -= OnObjectStay;
        }
    }
    
    private void OnObjectEntered(Collider2D other)
    {
        Debug.Log($"对象进入: {other.gameObject.name}");
        // 处理进入逻辑
    }
    
    private void OnObjectExited(Collider2D other)
    {
        Debug.Log($"对象离开: {other.gameObject.name}");
        // 处理离开逻辑
    }
    
    private void OnObjectStay(Collider2D other)
    {
        // 对象停留在区域内（每帧调用）
        // 处理持续逻辑
    }
}
```

### 4. 实用方法

`TriggerArea2D` 提供了多个实用方法：

```csharp
// 检查特定游戏对象是否在区域内
bool isInArea = triggerArea.ContainsGameObject(targetObject);

// 获取区域内所有指定标签的游戏对象
List<GameObject> players = triggerArea.GetGameObjectsWithTag("Player");

// 获取区域内指定类型的组件
List<Rigidbody2D> rigidbodies = triggerArea.GetComponentsInArea<Rigidbody2D>();

// 检查标签是否被允许
bool isAllowed = triggerArea.IsTagAllowed("Enemy");

// 获取区域内对象数量
int count = triggerArea.ObjectCount;

// 启用/禁用检测
triggerArea.IsEnabled = false;

// 清空区域内的对象（不会触发离开事件）
triggerArea.ClearObjects();
```

## 示例场景

### 示例1：检测玩家进入区域

```csharp
public class PlayerDetector : MonoBehaviour
{
    [SerializeField] private TriggerArea2D detectionArea;
    
    private void Start()
    {
        detectionArea.OnObjectEntered += (collider) => 
        {
            if (collider.CompareTag("Player"))
            {
                Debug.Log("玩家进入区域！");
                // 显示提示、播放音效等
            }
        };
        
        detectionArea.OnObjectExited += (collider) => 
        {
            if (collider.CompareTag("Player"))
            {
                Debug.Log("玩家离开区域！");
                // 隐藏提示等
            }
        };
    }
}
```

### 示例2：区域伤害

```csharp
public class AreaDamage : MonoBehaviour
{
    [SerializeField] private TriggerArea2D damageArea;
    [SerializeField] private float damagePerSecond = 10f;
    
    private void Update()
    {
        // 获取区域内所有敌人
        var enemies = damageArea.GetGameObjectsWithTag("Enemy");
        
        foreach (var enemy in enemies)
        {
            // 对每个敌人造成伤害
            Health health = enemy.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(damagePerSecond * Time.deltaTime);
            }
        }
    }
}
```

### 示例3：交互区域

```csharp
public class InteractionZone : MonoBehaviour
{
    [SerializeField] private TriggerArea2D interactionArea;
    [SerializeField] private GameObject interactionPrompt;
    
    private bool playerInZone = false;
    
    private void Start()
    {
        interactionArea.OnObjectEntered += (collider) => 
        {
            if (collider.CompareTag("Player"))
            {
                playerInZone = true;
                interactionPrompt.SetActive(true);
            }
        };
        
        interactionArea.OnObjectExited += (collider) => 
        {
            if (collider.CompareTag("Player"))
            {
                playerInZone = false;
                interactionPrompt.SetActive(false);
            }
        };
    }
    
    private void Update()
    {
        if (playerInZone && Input.GetKeyDown(KeyCode.E))
        {
            // 玩家按下交互键
            Interact();
        }
    }
    
    private void Interact()
    {
        Debug.Log("与对象交互！");
        // 执行交互逻辑
    }
}
```

## 注意事项

1. **碰撞器要求**: 确保附加了2D碰撞器组件（BoxCollider2D、CircleCollider2D、PolygonCollider2D等）
2. **触发器设置**: 碰撞器必须设置为触发器（Is Trigger = true）
3. **标签过滤**: 使用Target Tags可以限制只检测特定标签的对象
4. **性能优化**: 对于大量对象，考虑使用对象池或优化检测逻辑
5. **事件取消订阅**: 在OnDestroy中取消订阅事件，避免内存泄漏

## 编辑器功能

在Unity编辑器中，选中带有 `TriggerArea2D` 组件的游戏对象时：
- 会显示触发区域的绿色轮廓（仅在选中时可见）
- 支持各种2D碰撞器形状的绘制

## 扩展建议

可以根据需要扩展 `TriggerArea2D` 脚本，例如：
- 添加距离检测
- 添加视线检测（Raycast）
- 添加多层检测（不同层有不同的处理）
- 添加延迟触发/离开
- 添加触发次数限制

## 常见问题

**Q: 为什么事件没有被触发？**
A: 检查以下事项：
1. 碰撞器是否设置为触发器
2. 目标对象是否有碰撞器
3. 标签过滤设置是否正确
4. Is Enabled 是否被启用

**Q: 如何检测特定类型的对象？**
A: 使用 `GetComponentsInArea<T>()` 方法，例如：
```csharp
List<Enemy> enemies = triggerArea.GetComponentsInArea<Enemy>();
```

**Q: 如何临时禁用触发检测？**
A: 设置 `triggerArea.IsEnabled = false;`

**Q: 如何获取区域内所有对象？**
A: 使用 `triggerArea.ObjectsInArea` 属性