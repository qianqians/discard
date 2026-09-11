为当前 Cocos Creator 项目生成 TapTap 小游戏云存档模块代码。

## 你的任务

在 `assets/scripts/sdk/tap/` 目录下生成 `TapCloudSave.ts`，实现本地优先 + 云端备份的存档同步方案。

## 前置条件

- 已有 `Global.ts` 中的 `tap` 和 `TAPTAP` 导出
- 已有 `StorageUtil.ts` 统一存储工具类（使用 `sys.localStorage`）
- 需要先分析项目的 StorageUtil 使用情况，确定需要同步的 key 列表

## 设计方案

- **本地优先**：本地有存档就以本地为主，不会被云端覆盖
- **云端备份**：本地无存档时尝试拉取云端存档恢复
- **定时同步**：每 3 分钟检查本地存档是否有变化，有变化则全量上传云端
- **频次安全**：云端 create/update 限制一分钟最多一次，3分钟间隔远低于限制

## 第一步：分析项目存储 key

先读取项目中所有调用 `StorageUtil.setItem` / `StorageUtil.setObj` 的地方，整理出需要云同步的 key 列表。通常包括：
- 游戏进度（关卡数、金币等）
- 物品数据
- 设置项
- 奖励领取记录

排除不需要同步的 key（如假排行榜数据、已废弃功能的数据）。

## 第二步：生成 TapCloudSave.ts

```typescript
// TapTap 小游戏云存档功能封装
// 策略：本地存档优先，本地无存档时尝试拉取云存档
// 定时检查（3分钟）本地存档是否有变化，有变化则全量同步到云端
// 云端频次限制：createArchive 和 updateArchive 一分钟最多一次

import { sys } from "cc"
import { tap } from "../../Global"
import { Debug } from "../../util/Debug"

const Tag: string = 'TapCloudSave'
const ArchiveName: string = 'save_main'
const ArchiveFilePath = () => `${tap.env.USER_DATA_PATH}/cloud_save.json`
const SyncInterval: number = 180000 // 3分钟检查一次

// TODO: 根据项目实际情况填入需要云同步的 localStorage key 列表（不含前缀）
const SyncKeys: string[] = [
    // 'curLevel',
    // 'itemDic',
    // ...
]

// TODO: 根据项目的 StorageUtil 前缀修改
const KeyPrefix: string = ''

export class TapCloudSave {
    private static _manager = null
    private static _fs = null
    private static _archiveUUID: string = ''
    private static _archiveFileId: string = ''
    private static _lastSnapshot: string = ''
    private static _syncing: boolean = false
    private static _timer: any = null

    public static init(): void {
        if (!tap.getCloudSaveManager) {
            Debug.Error(Tag, 'getCloudSaveManager API 不支持')
            return
        }
        this._manager = tap.getCloudSaveManager()
        this._fs = tap.getFileSystemManager()
        Debug.Log(Tag, '云存档管理器初始化完成')
    }

    public static async restore(): Promise<void> {
        if (!this._manager) return

        const hasLocal = this.hasLocalSave()
        if (hasLocal) {
            Debug.Log(Tag, '本地存在存档，以本地为主')
            this._lastSnapshot = this.takeSnapshot()
            await this.fetchArchiveInfo()
            this.startSyncTimer()
            return
        }

        Debug.Log(Tag, '本地无存档，尝试拉取云存档')
        try {
            const hasCloud = await this.fetchArchiveInfo()
            if (hasCloud) {
                await this.downloadAndApply()
                Debug.Log(Tag, '云存档拉取成功，已写入本地')
            } else {
                Debug.Log(Tag, '云端也无存档，使用默认数据')
            }
        } catch (err) {
            Debug.Error(Tag, '拉取云存档失败，使用默认数据:', err)
        }

        this._lastSnapshot = this.takeSnapshot()
        this.startSyncTimer()
    }

    // TODO: 修改检测逻辑，使用项目中代表"有存档"的核心 key
    private static hasLocalSave(): boolean {
        const val = sys.localStorage.getItem(`${KeyPrefix}curLevel`)
        return !!val
    }

    private static takeSnapshot(): string {
        const data = {}
        for (const key of SyncKeys) {
            const fullKey = `${KeyPrefix}${key}`
            const val = sys.localStorage.getItem(fullKey)
            if (val !== null && val !== undefined) {
                data[key] = val
            }
        }
        return JSON.stringify(data)
    }

    private static fetchArchiveInfo(): Promise<boolean> {
        return new Promise((resolve) => {
            this._manager.getArchiveList({
                success: (res) => {
                    const saves = res.saves || []
                    const target = saves.find(s => s.name === ArchiveName)
                    if (target) {
                        this._archiveUUID = target.uuid
                        this._archiveFileId = target.fileId
                        Debug.Log(Tag, '找到云存档:', this._archiveUUID)
                        resolve(true)
                    } else {
                        Debug.Log(Tag, '云端无存档')
                        resolve(false)
                    }
                },
                fail: (res) => {
                    Debug.Error(Tag, '获取存档列表失败:', res.errMsg)
                    resolve(false)
                }
            })
        })
    }

    private static downloadAndApply(): Promise<void> {
        return new Promise((resolve, reject) => {
            const targetPath = ArchiveFilePath()
            this._manager.getArchiveData({
                archiveUUID: this._archiveUUID,
                archiveFileId: this._archiveFileId,
                targetFilePath: targetPath,
                success: (res) => {
                    try {
                        const content = this._fs.readFileSync(res.filePath, 'utf8')
                        const data = JSON.parse(content)
                        for (const key in data) {
                            const fullKey = `${KeyPrefix}${key}`
                            sys.localStorage.setItem(fullKey, data[key])
                        }
                        Debug.Log(Tag, '云存档已写入本地')
                        resolve()
                    } catch (err) {
                        Debug.Error(Tag, '解析云存档文件失败:', err)
                        reject(err)
                    }
                },
                fail: (res) => {
                    Debug.Error(Tag, '下载云存档失败:', res.errMsg)
                    reject(res.errMsg)
                }
            })
        })
    }

    private static startSyncTimer(): void {
        if (this._timer) return
        this._timer = setInterval(() => { this.checkAndSync() }, SyncInterval)
        Debug.Log(Tag, '云存档定时同步已启动，间隔:', SyncInterval / 1000, '秒')
    }

    private static checkAndSync(): void {
        if (!this._manager || this._syncing) return
        const currentSnapshot = this.takeSnapshot()
        if (currentSnapshot === this._lastSnapshot) { return }
        Debug.Log(Tag, '检测到本地存档变化，开始同步到云端')
        this._lastSnapshot = currentSnapshot
        this.uploadToCloud(currentSnapshot)
    }

    private static uploadToCloud(snapshotJson: string): void {
        this._syncing = true
        try {
            const filePath = ArchiveFilePath()
            this._fs.writeFileSync(filePath, snapshotJson, 'utf8')

            if (this._archiveUUID) {
                this._manager.updateArchive({
                    archiveUUID: this._archiveUUID,
                    archiveMetaData: { name: ArchiveName, summary: `auto sync` },
                    archiveFilePath: filePath,
                    success: (res) => { this._archiveFileId = res.fileId; Debug.Log(Tag, '云存档更新成功'); this._syncing = false },
                    fail: (res) => { Debug.Error(Tag, '云存档更新失败:', res.errMsg, res.errno); this._syncing = false }
                })
            } else {
                this._manager.createArchive({
                    archiveMetaData: { name: ArchiveName, summary: `auto sync` },
                    archiveFilePath: filePath,
                    success: (res) => { this._archiveUUID = res.uuid; this._archiveFileId = res.fileId; Debug.Log(Tag, '云存档创建成功:', this._archiveUUID); this._syncing = false },
                    fail: (res) => { Debug.Error(Tag, '云存档创建失败:', res.errMsg, res.errno); this._syncing = false }
                })
            }
        } catch (err) {
            Debug.Error(Tag, '写入存档文件失败:', err)
            this._syncing = false
        }
    }
}
```

## 第三步：接入初始化流程

在游戏初始化入口（如 `Load.ts`）中，**必须在各 Manager.init() 之前**调用：
```typescript
if (TAPTAP) {
    TapCloudSave.init()
    await TapCloudSave.restore()  // 需要 await，确保云存档写入 localStorage 后再 init 各 Manager
}
// 然后才是各 Manager.init()
```

## 生成完成后，提醒用户配置以下内容

| 配置项 | 位置 | 说明 |
|--------|------|------|
| `SyncKeys` | TapCloudSave.ts | 需要云同步的 localStorage key 列表（不含前缀），根据项目 StorageUtil 使用情况填写 |
| `KeyPrefix` | TapCloudSave.ts | StorageUtil 的 key 前缀，需要与项目一致 |
| `ArchiveName` | TapCloudSave.ts | 云存档名称，60字节以内，**不允许汉字** |
| `hasLocalSave()` | TapCloudSave.ts | 判断本地是否有存档的核心 key，根据项目修改 |

## API 注意事项

- `tap.getCloudSaveManager()` 基础库 2.0.0 起支持
- `createArchive` 和 `updateArchive` **一分钟最多调用一次**
- 存档文件需要先写入本地文件系统，再上传。路径使用 `${tap.env.USER_DATA_PATH}/xxx.json`
- `archiveMetaData.name` **不允许汉字**，60字节以内
- 存档文件大小不超过 10MB
- `getArchiveList` / `getArchiveData` 回调使用 `success/fail` 格式
- `createArchive` / `updateArchive` 也使用 `success/fail` 格式
- 云存档失败不应阻塞游戏，所有操作需要有降级处理
