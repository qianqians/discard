# Cocos Creator 项目 Bundle 依赖关系报告

## 概览
本报告详细记录了 `BoardPrefabs`、`PanelPrefabs` 和 `MainInterface` 三个 bundle 的配置、内容和依赖关系。

---

## 1. BoardPrefabs Bundle

### 配置信息
- **位置**: `assets/prefab/BoardPrefabs/`
- **配置文件**: `assets/prefab/BoardPrefabs.meta`
- **Bundle 类型**: 本地 Bundle
- **优先级**: 2
- **UUID**: `b0bed0de-3825-411a-8b68-c3543e8831d1`

### 包含资源
包含 10 个预制体（Board类型面板）：
1. `ChooseTagBoard.prefab` - 标签选择面板
2. `InformationBoard.prefab` - 信息展示面板
3. `LevelInfoBoard.prefab` - 关卡信息面板
4. `PopUpsBoard.prefab` - 弹出框面板
5. `RankListBoard.prefab` - 排行榜面板
6. `SettlementBoard.prefab` - 结算面板
7. `StorePromptBoard.prefab` - 商店提示面板
8. `TaskAchieveBoard.prefab` - 任务成就面板
9. `UpStageBoard.prefab` - 升阶面板
10. `UserInfoBoard.prefab` - 用户信息面板

### 依赖的其他 Bundle
**直接依赖**:
- `PanelPrefabs` - 部分Board可能包含Panel预制体的引用
- 可能依赖的纹理 Bundle：
  - `PanelTexture` - 面板贴图
  - `OtherTexture` - 其他贴图
  - `RolesTexture` - 角色贴图

### 代码中的使用方式
**在 GameManager.ts 中使用**:
```typescript
public async getBoard(str:string):Promise<Node> {
    // 从 BoardPrefabs bundle 中加载预制体
    let prefab = await BundleManager.Instance.loadAssetsFromBundle("BoardPrefabs", str) as Prefab;
    let board = instantiate(prefab);
    board.setParent(this.node);
    return board;
}
```

**在 login.ts 中预加载**:
```typescript
await BundleManager.Instance.PreLoadBundleDir("BoardPrefabs", "", false, _callBack, _complete);
```

**消息系统触发加载**:
```typescript
// 打开卡牌信息
SendMseeageType.OpenCardInfo → getBoard("InformationBoard")
// 打开排行榜
SendMseeageType.OpenRankList → getBoard("RankListBoard")
// 打开结算面板
SendMseeageType.SettlementEnd → getBoard("SettlementBoard")
```

### 加载策略
- **预加载时机**: 登录成功后预加载（在 `login.ts` 的 `loadBundle()` 方法中）
- **加载类型**: 全目录加载（`loadDir`）
- **缓存机制**: 在 `GameManager` 中使用 `Map<string, Node>` 缓存已加载的 Board 实例

---

## 2. PanelPrefabs Bundle

### 配置信息
- **位置**: `assets/prefab/PanelPrefabs/`
- **配置文件**: `assets/prefab/PanelPrefabs.meta`
- **Bundle 类型**: 本地 Bundle
- **优先级**: 2
- **UUID**: `247b72d6-ae2d-4702-aca3-19fc3b33b261`

### 包含资源
包含 10 个预制体（Panel类型界面）：
1. `AchievePanel.prefab` - 成就面板
2. `BattlePanel.prefab` - 战斗面板
3. `CardLibPanel.prefab` - 卡牌库面板
4. `MainInterface.prefab` - **主界面** ⭐
5. `PlotPanel.prefab` - 剧情面板
6. `QuestPanel.prefab` - 冒险任务面板
7. `ReadyPanel.prefab` - 准备面板
8. `StorePanel.prefab` - 商店面板
9. `gm.prefab` - 调试面板
10. `waiting.prefab` - 等待面板

### 依赖的其他 Bundle
**直接依赖**:
- `Typeface` - 字体 Bundle（在 MainInterface.prefab 中大量使用）
- `MainInterface` (texture) - UI纹理资源
- `PanelTexture` - 面板通用纹理
- `IconTexture` - 图标纹理
- `RolesTexture` - 角色纹理

**间接依赖**（通过 Panel 内部脚本）:
- `PanelSpine` - 面板动画
- `RoleSpine` - 角色 Spine 动画
- `Sound` - 音效

### 代码中的使用方式
**在 GameManager.ts 中使用**:
```typescript
public async getPanel(str:string) {
    // 从 PanelPrefabs bundle 中加载预制体
    let prefab = await BundleManager.Instance.loadAssetsFromBundle("PanelPrefabs", str) as Prefab;
    this.panelList.set(str, prefab);
    return prefab;
}
```

**在 login.ts 中**:
```typescript
// 预加载阶段 1 - 预加载（preload）
BundleManager.Instance.PreLoadBundleDir("MainInterface","", true);

// 预加载阶段 2 - 完整加载（load）
await BundleManager.Instance.PreLoadBundleDir("PanelPrefabs", "", false, _callBack, _complete);
```

**在 MainInterface.ts 中加载子 Panel**:
```typescript
let panel = instantiate(await GameManager.Instance.getPanel("CardLibPanel"));
```

**获取特定 Panel 的预制体**:
```typescript
// 在 login.ts 中
this.mainInterfacePanelPrefab = GameManager.Instance.getPanel("MainInterface");
```

### 加载策略
- **预加载时机**: 登录成功后立即预加载
- **加载类型**: 全目录加载
- **缓存机制**: 在 `GameManager` 中使用 `Map<string, Prefab>` 缓存预制体
- **实例化**: 按需实例化（`instantiate`）

---

## 3. MainInterface Bundle

### 注意：存在两个 MainInterface 相关的资源

#### 3a. MainInterface (Texture Bundle) ⭐ 这是真正的 Bundle

### 配置信息
- **位置**: `assets/texture/MainInterface/`
- **配置文件**: `assets/texture/MainInterface.meta`
- **Bundle 类型**: 本地 Bundle
- **优先级**: 10 （高优先级）
- **UUID**: `5c5c385f-2709-43de-9517-0515ae96e2ae`

### 包含资源
包含大量 UI 纹理资源（36+ 个 PNG 文件）：
- **头像相关**: `AvatarBG.png`, `AvatarFrames.png`, `AvatarMask.png`
- **按钮相关**: `MainBtn.png`, `MainFocusBtn.png`, `BtnListBG.png`
- **背景相关**: `SkyBg.png`, `GroundBG.png`, `Foreground_1.png`, `Foreground_2.png`
- **装饰相关**: `Ornament/` （子目录）
- **资源相关**: `Coin.png`, `diamond.png`
- **其他**: `economyBG.png`, `UserNameBG.png`, `bottom.png` 等

### 依赖的其他 Bundle
**被依赖的 Bundle**:
- `PanelPrefabs` - MainInterface.prefab 中的所有纹理引用
- `MainInterface` (script) - MainInterface.ts 类定义

**可能的间接依赖**:
- 任何引用这些纹理的 Panel

### 代码中的使用方式
**在 login.ts 中预加载**:
```typescript
// 预加载阶段 1 - 预加载
BundleManager.Instance.PreLoadBundleDir("MainInterface","", true);

// 预加载阶段 2 - 完整加载
await BundleManager.Instance.PreLoadBundleDir("MainInterface", "", false, _callBack, _complete);
```

**间接使用** - 通过 MainInterface.prefab 中的 Sprite 组件引用

### 加载策略
- **预加载时机**: 登录成功后，最优先级预加载（在 `preloadBundle()` 中）
- **加载类型**: 全目录预加载 (`preloadDir`)
- **优先级**: 10（在所有 Bundle 中优先级最高）

---

#### 3b. MainInterface (Script) - 这不是 Bundle，而是脚本类

### 配置信息
- **位置**: `assets/script/mainInterface/MainInterface.ts`
- **类型**: TypeScript 脚本类
- **用途**: 主界面逻辑控制类

### 导出的资源
脚本目录 `assets/script/mainInterface/` 包含：
- `MainInterface.ts` - 主界面控制脚本
- `Commodity.ts` - 商品逻辑
- `InterfaceDrag.ts` - 拖拽逻辑
- `StartGame.ts` - 游戏启动逻辑

---

## 4. Bundle 间的依赖关系图

```
┌─────────────────────────────────────────────────────────────┐
│                      Login 阶段                             │
│  preloadBundle() → 预加载基础资源                            │
└─────────────────────────────────────────────────────────────┘
                          ↓
┌─────────────────────────────────────────────────────────────┐
│                   预加载顺序 (preload)                       │
├─────────────────────────────────────────────────────────────┤
│ 1. SceneSpine, PanelSpine, RoleSpine                        │
│ 2. ButtonTexture, BackGroungTexture, RoleAvatarTexture     │
│ 3. PropsTexture, OtherTexture, Typeface, RolesTexture      │
│ 4. PanelTexture, Ready                                      │
│ 5. MainInterface (Texture) ⭐ 优先级最高                    │
│ 6. FrameTexture, IconTexture, SpecialSpine, CardLib        │
└─────────────────────────────────────────────────────────────┘
                          ↓
┌─────────────────────────────────────────────────────────────┐
│                   完整加载阶段 (load)                        │
├─────────────────────────────────────────────────────────────┤
│ 1. SceneSpine, PanelSpine, RoleSpine...（同上）             │
│ 2. ✓ BoardPrefabs ────────┐                                 │
│ 3. ✓ PanelPrefabs ────────┤→ 依赖关系:                       │
│ 4. ✓ MainInterface(Tex) ──┘   PanelPrefabs                  │
│                                ├─ MainInterface.prefab      │
│                                │  └─ 依赖 MainInterface(Tex)│
│                                └─ 其他 Panel prefabs        │
│                                   ├─ BattlePanel           │
│                                   ├─ CardLibPanel          │
│                                   ├─ QuestPanel            │
│                                   └─ ...                   │
└─────────────────────────────────────────────────────────────┘
                          ↓
┌─────────────────────────────────────────────────────────────┐
│               运行时按需加载（在 GameManager 中）            │
├─────────────────────────────────────────────────────────────┤
│ • GameManager.getPanel("PanelName")                         │
│   └─ 从 PanelPrefabs 中加载并缓存                           │
│                                                             │
│ • GameManager.getBoard("BoardName")                         │
│   └─ 从 BoardPrefabs 中加载并缓存                           │
│                                                             │
│ • MainInterface 的各子 Panel 加载                           │
│   └─ 如: CardLibPanel, QuestPanel 等                        │
└─────────────────────────────────────────────────────────────┘
```

---

## 5. 详细的依赖关系表

### 依赖关系矩阵

| 依赖源 | 依赖目标 | 依赖类型 | 用途 |
|-------|--------|--------|------|
| BoardPrefabs | PanelPrefabs | 可能 | Board 内部可能包含 Panel 引用 |
| BoardPrefabs | 纹理 Bundles | 间接 | 通过 UI 组件显示贴图 |
| PanelPrefabs | MainInterface(Tex) | 直接 | MainInterface.prefab 中的 Sprite 组件 |
| PanelPrefabs | Typeface | 直接 | 所有 Panel 使用字体 |
| PanelPrefabs | PanelTexture | 直接 | Panel UI 贴图 |
| PanelPrefabs | IconTexture | 直接 | 图标显示 |
| MainInterface(Tex) | PanelPrefabs | 被依赖 | 纹理被 MainInterface.prefab 使用 |
| MainInterface.ts | PanelPrefabs | 代码 | 加载 CardLibPanel 等子面板 |
| MainInterface.ts | Typeface | 代码 | 动态设置标签字体 |

---

## 6. 资源统计

### Bundle 大小估计
- **BoardPrefabs**: 10 个预制体（相对较小）
- **PanelPrefabs**: 10 个预制体（包含主界面，较大）
- **MainInterface(Texture)**: 36+ 个纹理资源（中等大小）

### 预制体总数
- Board 类预制体: **10 个**
- Panel 类预制体: **10 个**
- 特殊预制体: **MainInterface.prefab** (位于 PanelPrefabs)

---

## 7. 加载流程时序图

```
用户登录
   ↓
login.preloadBundle() 
   ├─→ PreLoadBundleDir("MainInterface", "", true) ⭐
   ├─→ PreLoadBundleDir("SceneSpine", "", true)
   ├─→ ... (其他基础资源)
   └─→ 等待预加载完成
   ↓
login.loadBundle()
   ├─→ PreLoadBundleDir("SceneSpine", "", false)
   ├─→ PreLoadBundleDir("PanelSpine", "", false)
   ├─→ PreLoadBundleDir("RoleSpine", "", false)
   ├─→ ... (其他资源)
   ├─→ PreLoadBundleDir("BoardPrefabs", "", false) 
   ├─→ PreLoadBundleDir("PanelPrefabs", "", false)
   ├─→ PreLoadBundleDir("MainInterface", "", false) ⭐
   └─→ 等待所有加载完成
   ↓
GameManager.Init()
   ├─→ 加载字体 (Typeface)
   ├─→ 加载等待 Panel (waiting from PanelPrefabs)
   └─→ 初始化完成
   ↓
MainInterface.start()
   ├─→ 实例化 MainInterface.prefab
   ├─→ 设置各按钮字体
   └─→ 显示主界面
   ↓
用户交互 (运行时)
   ├─→ 点击按钮触发消息
   ├─→ GameManager.getBoard() → 从 BoardPrefabs 加载
   ├─→ GameManager.getPanel() → 从 PanelPrefabs 加载
   └─→ 显示相应 UI
```

---

## 8. 关键代码位置汇总

| 功能 | 文件位置 | 行号范围 |
|------|---------|---------|
| Bundle 管理器 | `assets/script/bundle/BundleManager.ts` | 1-400 |
| PreloadMustBundle | `assets/script/bundle/BundleManager.ts` | 175-190 |
| PreLoadBundleDir | `assets/script/bundle/BundleManager.ts` | 250-310 |
| 登录预加载 | `assets/script/login/login.ts` | 118-176 |
| GameManager | `assets/script/other/GameManager.ts` | 1-300 |
| getPanel 方法 | `assets/script/other/GameManager.ts` | 156-176 |
| getBoard 方法 | `assets/script/other/GameManager.ts` | 90-115 |
| MainInterface | `assets/script/mainInterface/MainInterface.ts` | 1-150+ |

---

## 9. 总结

### 三个 Bundle 的核心区别

1. **BoardPrefabs**
   - 性质：二级面板集合
   - 大小：中等
   - 加载时机：登录后预加载
   - 使用方式：按需从 GameManager 获取

2. **PanelPrefabs**
   - 性质：主要 UI Panel 集合（包含主界面）
   - 大小：较大
   - 加载时机：登录后立即加载
   - 使用方式：通过 GameManager 缓存管理

3. **MainInterface (Texture)**
   - 性质：主界面 UI 纹理资源
   - 大小：中等
   - 加载时机：**最优先** - 在登录预加载阶段首先加载
   - 使用方式：被 PanelPrefabs 中的 MainInterface.prefab 引用

### 关键发现

- ✅ PanelPrefabs 强依赖 MainInterface(Texture)
- ✅ MainInterface 是优先级最高的 Bundle（priority: 10）
- ✅ 所有 Panel 都依赖 Typeface Bundle
- ✅ 存在两个 "MainInterface" - 一个是纹理 Bundle，一个是脚本类
- ✅ 采用了三层加载策略：预加载 → 完整加载 → 按需加载
