为当前 Cocos Creator 项目生成 TapTap 小游戏广告模块代码。

## 你的任务

在 `assets/scripts/sdk/tap/` 目录下生成 `TapAd.ts`，封装激励视频、插屏、Banner 三种广告。

## 前置条件

- 已有 `TapSDK.ts` 基础模块（提供 showToast）
- 已有 `Global.ts` 中的 `tap` 和 `TAPTAP` 导出
- 已有广告管理器 `AdMgr.ts`

## 生成 TapAd.ts

```typescript
// TapTap 小游戏广告管理封装（激励视频、插屏、Banner）
import { tap } from "../../Global"
import { Debug } from "../../util/Debug"
import { TapSDK } from "./TapSDK"

const Tag: string = 'TapAd'

const RewardedVideoAdId: string = '' // TODO: 填入激励视频广告ID
const BannerAdId: string = ''        // TODO: 填入Banner广告ID
const InterstitialId: string = ''    // TODO: 填入插屏广告ID

export class TapAd {
    private static rewardedVideoAd
    private static _bannerAd
    private static _windowWidth: number
    private static _windowHeight: number

    public static init() {
        let sysInfo = tap.getSystemInfoSync()
        this._windowWidth = sysInfo.windowWidth
        this._windowHeight = sysInfo.windowHeight
        if (RewardedVideoAdId) this.preloadRewardedVideo()
        if (BannerAdId) this.createBanner()
    }

    private static preloadRewardedVideo() {
        this.rewardedVideoAd = tap.createRewardedVideoAd({ adUnitId: RewardedVideoAdId })
        this.rewardedVideoAd.load().catch((err) => {
            Debug.Error(Tag, '激励视频预加载失败:', err)
        })
    }

    public static showRewardedVideo(onSuccess: Function, onFail: Function = null, onError: Function = null): void {
        if (!this.rewardedVideoAd) {
            TapSDK.showToast("广告未就绪")
            onError && onError()
            return
        }

        this.rewardedVideoAd.offError()
        this.rewardedVideoAd.onError((res) => {
            Debug.Error(Tag, '激励视频播放失败:', res.errMsg)
            TapSDK.showToast("视频拉取失败")
            onError && onError()
        })

        this.rewardedVideoAd.offClose()
        this.rewardedVideoAd.onClose((res) => {
            if (res && res.isEnded || res === undefined) {
                onSuccess && onSuccess()
            } else {
                TapSDK.showToast("中途退出，不下发奖励")
                onFail && onFail()
            }
        })

        this.loadAndShow(0, onError)
    }

    private static loadAndShow(retryCount: number, onError: Function): void {
        this.rewardedVideoAd.show().catch(() => {
            this.rewardedVideoAd.load().then(() => {
                this.rewardedVideoAd.show()
            }).catch((err) => {
                if (retryCount < 2) {
                    Debug.Log(Tag, `激励视频加载失败，重试第${retryCount + 1}次`)
                    this.loadAndShow(retryCount + 1, onError)
                } else {
                    Debug.Error(Tag, '激励视频加载失败:', err)
                    TapSDK.showToast("暂时没有合适的广告")
                    onError && onError()
                }
            })
        })
    }

    public static showInterstitialAd(): void {
        if (!tap || !InterstitialId) return
        const ad = tap.createInterstitialAd({ adUnitId: InterstitialId });
        ad.load().then(() => {
            ad.show().then(() => { Debug.Log(Tag, '插屏展示成功') });
        }).catch((err) => { Debug.Error(Tag, '插屏展示失败', err) })
    }

    public static createBanner(autoShow?: boolean): void {
        this._bannerAd = tap.createBannerAd({
            adUnitId: BannerAdId, adIntervals: 60,
            style: { width: this._windowWidth, left: 0, top: 0 }
        })
        if (!this._bannerAd) { Debug.Error(Tag, 'Banner创建失败'); return }
        this._bannerAd.onLoad(() => { autoShow && this.showBanner() })
        this._bannerAd.onResize(res => {
            this._bannerAd.style.top = this._windowHeight - this._bannerAd.style.realHeight;
        })
        this._bannerAd.onError((e) => { Debug.Error(Tag, 'Banner报错 ', e.errCode, ' ', e.errMsg) })
    }

    public static showBanner(): void {
        if (!this._bannerAd) { Debug.Error(Tag, 'Banner暂未创建，无法展示'); return }
        this._bannerAd.show().then(() => { Debug.Log(Tag, 'Banner展示成功') })
            .catch((err) => { Debug.Error(Tag, 'Banner展示失败', err) })
    }

    public static hideBanner(): void {
        if (!this._bannerAd) { Debug.Error(Tag, 'Banner暂未创建，无法隐藏'); return }
        this._bannerAd.hide()
        Debug.Log(Tag, 'Banner隐藏成功')
    }
}
```

## 接入 AdMgr

在 `AdMgr.ts` 的 `init()` 中添加 TAPTAP 分支（优先于微信）：
```typescript
import { TAPTAP } from "../Global";
import { TapAd } from "../sdk/tap/TapAd";

public static init() {
    if (TAPTAP) this.ad = TapAd
    else if (WECHAT) this.ad = WxAd
    this.ad?.init()
}
```

## 生成完成后，提醒用户配置以下 ID

| 配置项 | 常量名 | 说明 |
|--------|--------|------|
| 激励视频广告ID | `RewardedVideoAdId` | 在 Tap 后台「流量变现 - 广告位管理」中创建，注意选择竖屏/横屏 |
| Banner广告ID | `BannerAdId` | 同上，可选 |
| 插屏广告ID | `InterstitialId` | 同上，可选 |

## 广告机制说明

- 激励视频是**单例**模式，`createRewardedVideoAd` 多次调用返回同一实例
- 激励视频广告会**自动 load**，关闭后不需要手动重新 load
- load 失败最多重试 3 次（首次 + 2次重试），最终失败 Toast 提示
- `onClose` 回调中 `res.isEnded` 判断是否完整观看
- `offError()`/`offClose()` 不传参数时移除所有监听
- 广告 ID 为空时不会创建广告实例，不会报错
