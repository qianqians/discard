# SceneObjectSelect 预制体设置说明

## 在 Unity 编辑器中完成以下步骤：

### 1. 打开预制体
打开 `Assets/Resources/Perfabs/UI/SceneObjectSelect.prefab`

### 2. 添加脚本组件
在预制体的**根节点**（SceneObjectSelect）上：
- 右键 → Add Component → 搜索 "SceneObjectSelect"
- 或者拖拽 `Assets/Scripts/UI/SceneUI/SceneObjectSelect.cs` 脚本到根节点上

### 3. 验证层级结构
确保预制体的层级结构如下：
```
SceneObjectSelect (根节点，带 SceneObjectSelect 脚本)
└── Scroll View (滚动视口)
    └── Viewport
        └── Content (这是放置物品格子的地方)
    └── Scrollbar Vertical
```

### 4. 验证脚本绑定
- 确保 SceneObjectSelect 脚本组件绑定在根节点上（不是子节点）
- `Content` 节点的路径应该是：`Scroll View/Viewport/Content`

### 5. 保存预制体
保存预制体后，场景会自动更新。

## 工作流程

1. 用户点击左下角按钮触发 `GameStateMachine.ChangeState(PanelState.SceneObjectSelect)`
2. `PanelStateMachine` 调用 `HandleSceneObjectSelect()`
3. 注册面板并显示：`UIManager.Instance.ShowPanel<SceneObjectSelect>()`
4. `SceneObjectSelect` 的 `OnShow()` 方法被调用
5. `LoadBagItems()` 异步加载背包数据
6. 物品图标动态添加到 `Content` 节点下

## 调试提示

如果在 Console 中没有看到日志：
- 检查 `GameSceneManager` 是否已初始化
- 检查 `UIManager` 是否已初始化
- 检查预制体是否正确绑定了脚本
- 查看 Console 是否有相关错误信息
