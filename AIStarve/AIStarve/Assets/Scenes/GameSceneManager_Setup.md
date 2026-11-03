# GameSceneManager 场景设置说明

## 当前场景
项目包含以下场景：
- **SampleScene.unity** - 默认示例场景
- **login.unity** - 登录场景
- **Field.unity** - 游戏场景

## GameSceneManager 初始化状态

### 问题分析
根据之前的对话，GameSceneManager 可能没有正确初始化。原因可能包括：

1. **GameSceneManager GameObject 不存在于场景中**
2. **GameSceneManager 脚本没有附加到 GameObject**
3. **DontDestroyOnLoad 设置问题**
4. **场景切换时实例丢失**

### 解决方案

#### 方法1：手动添加 GameSceneManager 到场景
1. 在 Unity 编辑器中打开 SampleScene.unity
2. 创建新的 GameObject，命名为 "GameSceneManager"
3. 将 GameSceneManager.cs 脚本附加到该 GameObject
4. 保存场景

#### 方法2：使用 TestGameSceneManager 测试脚本
1. 在 Unity 编辑器中打开任意场景
2. 创建新的 GameObject，命名为 "TestGameSceneManager"
3. 将 TestGameSceneManager.cs 脚本附加到该 GameObject
4. 运行游戏，查看控制台日志

#### 方法3：通过 CubeRotate 测试
1. CubeRotate 脚本已经包含测试方法
2. 在浏览器控制台中执行：
   ```javascript
   SendMessage('CubeRotate', 'TestGameSceneManagerFromJS');
   ```

## 预期日志输出

### 成功初始化：
```
GameSceneManager.Awake() called - Instance check: False
GameSceneManager: Instance set successfully, SceneDB initializing...
GameSceneManager: Registering event handlers...
GameSceneManager: Initialization completed successfully!
GameSceneManager.Start() called - Instance: True, SceneDB: True
CubeRotate.Start(): GameSceneManager.Instance found - testing access
```

### 初始化失败：
```
CubeRotate.Start(): GameSceneManager.Instance is null
TestGameSceneManager: GameSceneManager.Instance is NULL - GameSceneManager not initialized!
```

## 下一步操作

1. **检查当前场景**：确认 GameSceneManager GameObject 是否存在
2. **添加测试组件**：使用 TestGameSceneManager 进行测试
3. **查看控制台日志**：根据日志判断初始化状态
4. **手动修复**：如果 GameSceneManager 不存在，手动添加到场景中

## 重要提醒
GameSceneManager 使用单例模式，需要确保：
- 只有一个 GameSceneManager 实例
- 使用 DontDestroyOnLoad 保持跨场景
- 在 Awake() 方法中正确设置实例