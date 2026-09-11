为当前 Cocos Creator 项目生成 TapTap 小游戏 SDK 适配层代码。

## 你的任务

根据项目结构，在 `assets/scripts/sdk/tap/` 目录下生成以下模块文件，并修改相关管理器文件实现平台切换。

## 前置条件

项目需要有以下基础设施（如果没有请先确认）：
- `Global.ts` 中有全局平台引用导出
- `Debug.ts` 日志工具类
- 广告管理器 `AdMgr.ts`（如有）
- 振动工具 `VibrateUtil.ts`（如有）

## 第一步：平台检测

在 `Global.ts` 中添加：
```typescript
export const tap = globalThis.tap //TapTap
export const TAPTAP: boolean = !!globalThis.tap
```

## 第二步：生成 TapSDK.ts（基础模块）

```typescript
// TapTap 小游戏 SDK 基础功能封装（初始化、登录、系统信息、Toast、振动）
import { tap } from "../../Global"
import { Debug } from "../../util/Debug"

const Tag: string = 'TapSDK'

export class TapSDK {
    private static _sysInfo = null

    public static get StatusBarHeight(): number {
        return this._sysInfo?.statusBarHeight ?? 0
    }

    public static init(): void {
        this._sysInfo = tap.getSystemInfoSync()
        Debug.Log(Tag, '系统信息:', this._sysInfo)
    }

    public static login(): Promise<any> {
        return new Promise((resolve, reject) => {
            tap.login({
                success(res) { Debug.Log(Tag, '登录成功:', res); resolve(res) },
                fail(err) { Debug.Error(Tag, '登录失败:', err); reject(err) }
            })
        })
    }

    public static showToast(tip: string, icon: string = "none", duration: number = 1500): void {
        tap.showToast({ "title": tip, "icon": icon, "duration": duration })
    }

    public static hideToast(): void { tap.hideToast({}) }

    public static vibrate(): void { tap.vibrateShort({ type: 'light' }) }
}
```

## 第三步：生成 TapShare.ts（分享模块）

```typescript
// TapTap 小游戏分享功能封装
import { tap } from "../../Global"
import { Debug } from "../../util/Debug"
import { TapSDK } from "./TapSDK"

const Tag: string = 'TapShare'
const ShareTemplateId: string = '' // TODO: 填入分享模板ID

export class TapShare {
    public static share(succCallBack: Function, failCallBack: Function, data: Object = {}) {
        Debug.Log(Tag, '拉起分享')
        tap.showShareboard({
            templateId: ShareTemplateId,
            success() { Debug.Log(Tag, '分享成功'); TapSDK.showToast("分享成功"); succCallBack && succCallBack() },
            fail(err) { Debug.Log(Tag, '分享失败', err); TapSDK.showToast("分享失败"); failCallBack && failCallBack() }
        })
    }
}
```

## 第四步：生成 TapLeaderboard.ts（排行榜模块）

```typescript
// TapTap 小游戏排行榜功能封装
import { tap } from "../../Global"
import { Debug } from "../../util/Debug"

const Tag: string = 'TapLeaderboard'

export class TapLeaderboard {
    private static _manager = null

    public static init(): void {
        if (!tap.getLeaderboardManager) { Debug.Error(Tag, 'getLeaderboardManager API 不支持'); return }
        this._manager = tap.getLeaderboardManager()
        Debug.Log(Tag, '排行榜管理器初始化完成')
    }

    public static submitScore(leaderboardId: string, score: number): void {
        if (!this._manager) { Debug.Error(Tag, '排行榜管理器未初始化'); return }
        this._manager.submitScores({
            scores: [{ leaderboardId: leaderboardId, score: score }],
            callback: {
                onSuccess(res) { Debug.Log(Tag, '提交分数成功:', res) },
                onFailure(code, message) { Debug.Error(Tag, '提交分数失败:', code, message) }
            }
        })
    }

    public static openLeaderboard(leaderboardId: string): void {
        if (!this._manager) { Debug.Error(Tag, '排行榜管理器未初始化'); return }
        this._manager.openLeaderboard({
            leaderboardId: leaderboardId, collection: 'public',
            callback: {
                onSuccess(res) { Debug.Log(Tag, '打开排行榜成功:', res) },
                onFailure(code, message) { Debug.Error(Tag, '打开排行榜失败:', code, message) }
            }
        })
    }

    public static loadScores(leaderboardId: string): Promise<any[]> {
        return new Promise((resolve, reject) => {
            if (!this._manager) { reject('排行榜管理器未初始化'); return }
            this._manager.loadLeaderboardScores({
                leaderboardId: leaderboardId,
                callback: {
                    onSuccess(res) { Debug.Log(Tag, '加载排行榜数据成功:', res); resolve(res.scores || []) },
                    onFailure(code, message) { Debug.Error(Tag, '加载排行榜数据失败:', code, message); reject(message) }
                }
            })
        })
    }
}
```

## 第五步：生成 TapAchievement.ts（成就模块）

```typescript
// TapTap 小游戏成就系统封装
import { tap } from "../../Global"
import { Debug } from "../../util/Debug"

const Tag: string = 'TapAchievement'

// TODO: 根据游戏设计填入成就ID列表
const AchievementIds: string[] = []

export class TapAchievement {
    private static _manager = null

    public static init(): void {
        this._manager = tap.createAchievementManager({ toastEnable: true })
        Debug.Log(Tag, '成就管理器初始化完成')
    }

    public static unlockAchievement(achievementId: string): void {
        if (!this._manager) { Debug.Error(Tag, '成就管理器未初始化'); return }
        this._manager.unlockAchievement({ achievementId: achievementId })
    }

    public static incrementAchievement(achievementId: string, steps: number): void {
        if (!this._manager) { Debug.Error(Tag, '成就管理器未初始化'); return }
        this._manager.incrementAchievement({ achievementId: achievementId, steps: steps })
    }

    public static showAchievements(): void {
        if (!this._manager) { Debug.Error(Tag, '成就管理器未初始化'); return }
        this._manager.showAchievements()
    }
}
```

## 第六步：修改管理器和初始化

1. **AdMgr.ts**：`init()` 中添加 `if (TAPTAP) this.ad = TapAd`（优先于微信）
2. **VibrateUtil.ts**：添加 `if (TAPTAP) TapSDK.vibrate()` 分支
3. **Load.ts（初始化入口）**：
```typescript
if (TAPTAP) {
    TapSDK.init()
    TapSDK.login()
    TapAchievement.init()
    TapCloudSave.init()
    await TapCloudSave.restore()
}
```

## 生成完成后，提醒用户配置以下 ID

| 配置项 | 文件 | 说明 |
|--------|------|------|
| `ShareTemplateId` | TapShare.ts | 分享模板ID，在 Tap 后台「分享管理」中创建 |
| `LeaderboardId` | 业务代码中使用 | 排行榜ID，在 Tap 后台「排行榜」中创建 |
| 成就ID列表 | TapAchievement.ts | 在 Tap 后台「成就」中创建，ID 使用英文字符 |

排行榜配置建议：排序方式=降序，数据类型=整数，更新策略=取最大值。

## API 注意事项

- 平台检测使用 `!!globalThis.tap` 运行时检测，编辑器下为 false，不会报错
- 排行榜回调格式：`callback: { onSuccess, onFailure }`（不是 success/fail）
- 成就 `showAchievements()` 无参数直接调用
- 成就解锁会自动弹 Toast，无需客户端监听
