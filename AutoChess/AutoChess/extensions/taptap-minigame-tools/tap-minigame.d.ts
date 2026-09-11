/**
 * TapTap 小游戏全局 API 类型定义
 * 用途：为 tap.* API 提供代码补全和类型检查
 * 自动生成自 TapMiniGame_API_Reference.md + 手工补充
 */

interface TapSystemInfo {
    /** 设备品牌 */
    brand: string;
    /** 设备型号 */
    model: string;
    /** 操作系统及版本 */
    system: string;
    /** 客户端平台 */
    platform: string;
    /** 状态栏高度（px） */
    statusBarHeight: number;
    /** 可使用窗口宽度（px） */
    windowWidth: number;
    /** 可使用窗口高度（px） */
    windowHeight: number;
    /** 屏幕宽度（px） */
    screenWidth: number;
    /** 屏幕高度（px） */
    screenHeight: number;
    /** 设备像素比 */
    pixelRatio: number;
    /** 基础库版本 */
    SDKVersion: string;
}

interface TapCallbackResult {
    errMsg?: string;
    errCode?: number;
    errno?: number;
}

interface TapSuccessFailCallback<T = any> {
    success?: (res: T) => void;
    fail?: (res: TapCallbackResult) => void;
    complete?: () => void;
}

// ============ 广告相关 ============

interface TapRewardedVideoAd {
    load(): Promise<void>;
    show(): Promise<void>;
    onClose(callback: (res: { isEnded: boolean }) => void): void;
    offClose(callback?: Function): void;
    onError(callback: (res: { errMsg: string; errCode: number }) => void): void;
    offError(callback?: Function): void;
    onLoad(callback: () => void): void;
    offLoad(callback?: Function): void;
    destroy(): void;
}

interface TapInterstitialAd {
    load(): Promise<void>;
    show(): Promise<void>;
    onClose(callback: () => void): void;
    offClose(callback?: Function): void;
    onError(callback: (res: { errMsg: string; errCode: number }) => void): void;
    offError(callback?: Function): void;
    onLoad(callback: () => void): void;
    offLoad(callback?: Function): void;
    destroy(): void;
}

interface TapBannerAdStyle {
    width: number;
    left: number;
    top: number;
    realHeight?: number;
}

interface TapBannerAd {
    show(): Promise<void>;
    hide(): void;
    style: TapBannerAdStyle;
    onLoad(callback: () => void): void;
    offLoad(callback?: Function): void;
    onResize(callback: (res: { width: number; height: number }) => void): void;
    offResize(callback?: Function): void;
    onError(callback: (res: { errMsg: string; errCode: number }) => void): void;
    offError(callback?: Function): void;
    destroy(): void;
}

interface TapGridAd {
    show(): Promise<void>;
    hide(): void;
    style: TapBannerAdStyle;
    onLoad(callback: () => void): void;
    offLoad(callback?: Function): void;
    onResize(callback: (res: { width: number; height: number }) => void): void;
    offResize(callback?: Function): void;
    onError(callback: (res: { errMsg: string; errCode: number }) => void): void;
    offError(callback?: Function): void;
    destroy(): void;
}

interface TapCustomAd {
    show(): Promise<void>;
    hide(): void;
    style: TapBannerAdStyle;
    onLoad(callback: () => void): void;
    offLoad(callback?: Function): void;
    onError(callback: (res: { errMsg: string; errCode: number }) => void): void;
    offError(callback?: Function): void;
    destroy(): void;
}

// ============ 成就相关 ============

interface TapAchievementManager {
    unlockAchievement(params: { achievementId: string }): void;
    incrementAchievement(params: { achievementId: string; steps: number }): void;
    showAchievements(): void;
}

// ============ 排行榜相关 ============

interface TapLeaderboardScore {
    leaderboardId: string;
    score: number;
}

interface TapLeaderboardCallbacks {
    callback: {
        onSuccess: (res: any) => void;
        onFailure: (code: number, message: string) => void;
    };
}

interface TapLeaderboardManager {
    submitScores(params: { scores: TapLeaderboardScore[] } & TapLeaderboardCallbacks): void;
    openLeaderboard(params: { leaderboardId: string; collection?: string } & TapLeaderboardCallbacks): void;
    loadLeaderboardScores(params: { leaderboardId: string } & TapLeaderboardCallbacks): void;
}

// ============ 云存档相关 ============

interface TapArchiveMetaData {
    name: string;
    summary?: string;
}

interface TapArchiveItem {
    uuid: string;
    fileId: string;
    name: string;
    summary?: string;
}

interface TapCloudSaveManager {
    getArchiveList(params: TapSuccessFailCallback<{ saves: TapArchiveItem[] }>): void;
    getArchiveData(params: {
        archiveUUID: string;
        archiveFileId: string;
        targetFilePath: string;
    } & TapSuccessFailCallback<{ filePath: string }>): void;
    createArchive(params: {
        archiveMetaData: TapArchiveMetaData;
        archiveFilePath: string;
    } & TapSuccessFailCallback<{ uuid: string; fileId: string }>): void;
    updateArchive(params: {
        archiveUUID: string;
        archiveMetaData: TapArchiveMetaData;
        archiveFilePath: string;
    } & TapSuccessFailCallback<{ fileId: string }>): void;
}

// ============ 文件系统相关 ============

interface TapFileSystemManager {
    readFileSync(filePath: string, encoding?: string): string;
    writeFileSync(filePath: string, data: string, encoding?: string): void;
    readFile(params: { filePath: string; encoding?: string } & TapSuccessFailCallback<{ data: string | ArrayBuffer }>): void;
    writeFile(params: { filePath: string; data: string | ArrayBuffer; encoding?: string } & TapSuccessFailCallback): void;
    appendFile(params: { filePath: string; data: string | ArrayBuffer; encoding?: string } & TapSuccessFailCallback): void;
    appendFileSync(filePath: string, data: string | ArrayBuffer, encoding?: string): void;
    mkdir(params: { dirPath: string; recursive?: boolean } & TapSuccessFailCallback): void;
    mkdirSync(dirPath: string, recursive?: boolean): void;
    rmdir(params: { dirPath: string; recursive?: boolean } & TapSuccessFailCallback): void;
    rmdirSync(dirPath: string, recursive?: boolean): void;
    readdir(params: { dirPath: string } & TapSuccessFailCallback<{ files: string[] }>): void;
    readdirSync(dirPath: string): string[];
    rename(params: { oldPath: string; newPath: string } & TapSuccessFailCallback): void;
    renameSync(oldPath: string, newPath: string): void;
    copyFile(params: { srcPath: string; destPath: string } & TapSuccessFailCallback): void;
    copyFileSync(srcPath: string, destPath: string): void;
    unlink(params: { filePath: string } & TapSuccessFailCallback): void;
    unlinkSync(filePath: string): void;
    stat(params: { path: string; recursive?: boolean } & TapSuccessFailCallback<{ stats: any }>): void;
    statSync(path: string, recursive?: boolean): any;
    access(params: { path: string } & TapSuccessFailCallback): void;
    accessSync(path: string): void;
    saveFile(params: { tempFilePath: string; filePath?: string } & TapSuccessFailCallback<{ savedFilePath: string }>): void;
    saveFileSync(tempFilePath: string, filePath?: string): string;
    removeSavedFile(params: { filePath: string } & TapSuccessFailCallback): void;
    getSavedFileList(params: TapSuccessFailCallback<{ fileList: any[] }>): void;
    getFileInfo(params: { filePath: string } & TapSuccessFailCallback<{ size: number }>): void;
}

// ============ 多人联机相关 ============

interface TapOnlineBattleManager {
    // 多人联机管理器，具体方法参考官方文档
    [key: string]: any;
}

// ============ 音频相关 ============

interface TapInnerAudioContext {
    src: string;
    startTime: number;
    autoplay: boolean;
    loop: boolean;
    obeyMuteSwitch: boolean;
    volume: number;
    playbackRate: number;
    duration: number;
    currentTime: number;
    paused: boolean;
    buffered: number;
    play(): void;
    pause(): void;
    stop(): void;
    seek(position: number): void;
    destroy(): void;
    onCanplay(callback: () => void): void;
    offCanplay(callback?: Function): void;
    onPlay(callback: () => void): void;
    offPlay(callback?: Function): void;
    onPause(callback: () => void): void;
    offPause(callback?: Function): void;
    onStop(callback: () => void): void;
    offStop(callback?: Function): void;
    onEnded(callback: () => void): void;
    offEnded(callback?: Function): void;
    onTimeUpdate(callback: () => void): void;
    offTimeUpdate(callback?: Function): void;
    onError(callback: (res: { errCode: number; errMsg: string }) => void): void;
    offError(callback?: Function): void;
    onWaiting(callback: () => void): void;
    offWaiting(callback?: Function): void;
    onSeeking(callback: () => void): void;
    offSeeking(callback?: Function): void;
    onSeeked(callback: () => void): void;
    offSeeked(callback?: Function): void;
}

// ============ 更新管理相关 ============

interface TapUpdateManager {
    onCheckForUpdate(callback: (res: { hasUpdate: boolean }) => void): void;
    onUpdateReady(callback: () => void): void;
    onUpdateFailed(callback: () => void): void;
    applyUpdate(): void;
}

// ============ 录音管理相关 ============

interface TapRecorderManager {
    start(options?: { duration?: number; sampleRate?: number; numberOfChannels?: number; encodeBitRate?: number; format?: string }): void;
    pause(): void;
    resume(): void;
    stop(): void;
    onStart(callback: () => void): void;
    onPause(callback: () => void): void;
    onResume(callback: () => void): void;
    onStop(callback: (res: { tempFilePath: string; duration: number; fileSize: number }) => void): void;
    onFrameRecorded(callback: (res: { frameBuffer: ArrayBuffer; isLastFrame: boolean }) => void): void;
    onError(callback: (res: { errMsg: string }) => void): void;
    onInterruptionBegin(callback: () => void): void;
    onInterruptionEnd(callback: () => void): void;
}

// ============ 视频相关 ============

interface TapVideo {
    src: string;
    x: number;
    y: number;
    width: number;
    height: number;
    autoplay: boolean;
    loop: boolean;
    muted: boolean;
    play(): void;
    pause(): void;
    stop(): void;
    seek(time: number): void;
    destroy(): void;
    onPlay(callback: () => void): void;
    onPause(callback: () => void): void;
    onEnded(callback: () => void): void;
    onError(callback: (res: { errMsg: string }) => void): void;
    onTimeUpdate(callback: (res: { position: number; duration: number }) => void): void;
    offPlay(callback?: Function): void;
    offPause(callback?: Function): void;
    offEnded(callback?: Function): void;
    offError(callback?: Function): void;
    offTimeUpdate(callback?: Function): void;
}

// ============ WebSocket 相关 ============

interface TapSocketTask {
    send(params: { data: string | ArrayBuffer } & TapSuccessFailCallback): void;
    close(params?: { code?: number; reason?: string } & TapSuccessFailCallback): void;
    onOpen(callback: (res: { header: any }) => void): void;
    onMessage(callback: (res: { data: string | ArrayBuffer }) => void): void;
    onError(callback: (res: { errMsg: string }) => void): void;
    onClose(callback: (res: { code: number; reason: string }) => void): void;
}

// ============ 下载任务相关 ============

interface TapDownloadTask {
    abort(): void;
    onProgressUpdate(callback: (res: { progress: number; totalBytesWritten: number; totalBytesExpectedToWrite: number }) => void): void;
    offProgressUpdate(callback?: Function): void;
    onHeadersReceived(callback: (res: { header: any }) => void): void;
    offHeadersReceived(callback?: Function): void;
}

// ============ 上传任务相关 ============

interface TapUploadTask {
    abort(): void;
    onProgressUpdate(callback: (res: { progress: number; totalBytesSent: number; totalBytesExpectedToSend: number }) => void): void;
    offProgressUpdate(callback?: Function): void;
    onHeadersReceived(callback: (res: { header: any }) => void): void;
    offHeadersReceived(callback?: Function): void;
}

// ============ 请求任务相关 ============

interface TapRequestTask {
    abort(): void;
    onHeadersReceived(callback: (res: { header: any }) => void): void;
    offHeadersReceived(callback?: Function): void;
}

// ============ 触摸事件相关 ============

interface TapTouch {
    identifier: number;
    pageX: number;
    pageY: number;
    clientX: number;
    clientY: number;
}

interface TapTouchEvent {
    touches: TapTouch[];
    changedTouches: TapTouch[];
    timeStamp: number;
}

// ============ 菜单按钮相关 ============

interface TapMenuButtonBoundingClientRect {
    width: number;
    height: number;
    top: number;
    right: number;
    bottom: number;
    left: number;
}

// ============ 全局 tap API ============

/**
 * TapTap 小游戏 API 接口
 * 通过 globalThis.tap 或 import { tap } from "Global" 访问
 */
interface TapAPI {
    /** 环境变量 */
    env: {
        /** 用户数据路径 */
        USER_DATA_PATH: string;
    };

    // -------- 开放接口 - 登录 --------

    /** 调用接口获取登录凭证（code） */
    login(params?: { timeout?: number } & TapSuccessFailCallback<{ code: string }>): void;
    /** 检查登录态是否过期 */
    checkSession(params?: TapSuccessFailCallback): void;

    // -------- 开放接口 - 用户信息 --------

    /** 获取用户信息 */
    getUserInfo(params?: { withCredentials?: boolean; lang?: string } & TapSuccessFailCallback<{ userInfo: any; rawData: string; signature: string; encryptedData: string; iv: string }>): void;
    /** 获取用户手机号 */
    getPhoneNumber(params?: TapSuccessFailCallback<{ encryptedData: string; iv: string }>): void;
    /** 创建用户信息按钮 */
    createUserInfoButton(params: { type: string; text?: string; image?: string; style?: any; withCredentials?: boolean; lang?: string }): any;

    // -------- 开放接口 - 授权 --------

    /** 提前向用户发起授权请求 */
    authorize(params: { scope: string } & TapSuccessFailCallback): void;

    // -------- 开放接口 - 分享 --------

    /** 拉起分享面板 */
    showShareboard(params: { templateId: string } & TapSuccessFailCallback): void;
    /** 监听分享消息事件 */
    onShareMessage(callback: (res: any) => any): void;
    /** 取消监听分享消息事件 */
    offShareMessage(callback?: Function): void;
    /** 设置分享面板隐藏 */
    setShareboardHidden(params: { hidden: boolean } & TapSuccessFailCallback): void;
    /** 打开好友列表 */
    openFriendList(params?: TapSuccessFailCallback): void;

    // -------- 开放接口 - 成就 --------

    /** 创建成就管理器 */
    createAchievementManager(params?: { toastEnable?: boolean }): TapAchievementManager;

    // -------- 开放接口 - 排行榜 --------

    /** 获取排行榜管理器 */
    getLeaderboardManager(): TapLeaderboardManager;

    // -------- 开放接口 - 多人联机 --------

    /** 获取多人联机管理器 */
    getOnlineBattleManager(): TapOnlineBattleManager;

    // -------- 开放接口 - 云存档 --------

    /** 获取云存档管理器（基础库 2.0.0+） */
    getCloudSaveManager(): TapCloudSaveManager;

    // -------- 开放接口 - 设置 --------

    /** 获取用户已授权的设置信息 */
    getSetting(params?: TapSuccessFailCallback<{ authSetting: any }>): void;
    /** 打开设置页面 */
    openSetting(params?: TapSuccessFailCallback<{ authSetting: any }>): void;
    /** 创建打开设置按钮 */
    createOpenSettingButton(params: { type: string; text?: string; image?: string; style?: any }): any;

    // -------- 开放接口 - 隐私 --------

    /** 获取隐私设置 */
    getPrivacySetting(params?: TapSuccessFailCallback<{ needAuthorization: boolean; privacyContractName: string }>): void;
    /** 打开隐私协议页面 */
    openPrivacyContract(params?: TapSuccessFailCallback): void;
    /** 监听隐私授权需求 */
    onNeedPrivacyAuthorization(callback: (resolve: (params: { buttonId?: string; event?: string }) => void) => void): void;
    /** 用户同意隐私协议 */
    requirePrivacyAuthorize(params?: TapSuccessFailCallback): void;

    // -------- 开放接口 - 桌面文件夹 --------

    /** 创建桌面快捷方式 */
    createHomeScreenWidget(params?: TapSuccessFailCallback): void;
    /** 检查桌面快捷方式是否已创建 */
    hasHomeScreenWidgetAndPinned(params?: TapSuccessFailCallback<{ isPinned: boolean }>): void;

    // -------- 开放接口 - 账号信息 --------

    /** 同步获取账号信息 */
    getAccountInfoSync(): { miniGame: { appId: string } };

    // -------- 广告 --------

    /** 创建激励视频广告 */
    createRewardedVideoAd(params: { adUnitId: string }): TapRewardedVideoAd;
    /** 创建插屏广告 */
    createInterstitialAd(params: { adUnitId: string }): TapInterstitialAd;
    /** 创建 Banner 广告 */
    createBannerAd(params: { adUnitId: string; adIntervals?: number; style?: { width?: number; left?: number; top?: number } }): TapBannerAd;
    /** 创建格子广告 */
    createGridAd(params: { adUnitId: string; adTheme?: string; gridCount?: number; style?: { left?: number; top?: number; width?: number; opacity?: number } }): TapGridAd;
    /** 创建原生模板广告 */
    createCustomAd(params: { adUnitId: string; style?: { left?: number; top?: number; fixed?: boolean } }): TapCustomAd;

    // -------- 界面 - 交互 --------

    /** 显示 Toast 提示 */
    showToast(params: { title: string; icon?: string; image?: string; duration?: number; mask?: boolean }): void;
    /** 隐藏 Toast 提示 */
    hideToast(params?: {}): void;
    /** 显示 Loading 提示 */
    showLoading(params: { title: string; mask?: boolean }): void;
    /** 隐藏 Loading 提示 */
    hideLoading(params?: {}): void;
    /** 显示模态对话框 */
    showModal(params: { title?: string; content?: string; showCancel?: boolean; cancelText?: string; cancelColor?: string; confirmText?: string; confirmColor?: string; editable?: boolean; placeholderText?: string } & TapSuccessFailCallback<{ confirm: boolean; cancel: boolean; content?: string }>): void;
    /** 显示操作菜单 */
    showActionSheet(params: { alertText?: string; itemList: string[]; itemColor?: string } & TapSuccessFailCallback<{ tapIndex: number }>): void;

    // -------- 界面 - 菜单 --------

    /** 获取菜单按钮的布局位置信息 */
    getMenuButtonBoundingClientRect(): TapMenuButtonBoundingClientRect;
    /** 设置菜单按钮样式 */
    setMenuStyle(params: { style: string } & TapSuccessFailCallback): void;
    /** 监听菜单按钮布局变化 */
    onMenuButtonBoundingClientRectWeightChange(callback: (res: TapMenuButtonBoundingClientRect) => void): void;
    /** 取消监听菜单按钮布局变化 */
    offMenuButtonBoundingClientRectWeightChange(callback?: Function): void;

    // -------- 界面 - 状态栏 --------

    /** 设置状态栏样式 */
    setStatusBarStyle(params: { style: string } & TapSuccessFailCallback): void;

    // -------- 界面 - 窗口 --------

    /** 监听窗口尺寸变化 */
    onWindowResize(callback: (res: { windowWidth: number; windowHeight: number }) => void): void;
    /** 取消监听窗口尺寸变化 */
    offWindowResize(callback?: Function): void;

    // -------- 数据缓存 --------

    /** 异步存储数据 */
    setStorage(params: { key: string; data: any } & TapSuccessFailCallback): void;
    /** 同步存储数据 */
    setStorageSync(key: string, data: any): void;
    /** 异步读取数据 */
    getStorage(params: { key: string } & TapSuccessFailCallback<{ data: any }>): void;
    /** 同步读取数据 */
    getStorageSync(key: string): any;
    /** 异步删除数据 */
    removeStorage(params: { key: string } & TapSuccessFailCallback): void;
    /** 同步删除数据 */
    removeStorageSync(key: string): void;
    /** 异步清空数据 */
    clearStorage(params?: TapSuccessFailCallback): void;
    /** 同步清空数据 */
    clearStorageSync(): void;
    /** 异步获取存储信息 */
    getStorageInfo(params?: TapSuccessFailCallback<{ keys: string[]; currentSize: number; limitSize: number }>): void;
    /** 同步获取存储信息 */
    getStorageInfoSync(): { keys: string[]; currentSize: number; limitSize: number };

    // -------- 跳转 --------

    /** 跳转到其他小程序 */
    navigateToMiniProgram(params: { appId: string; path?: string; extraData?: any; envVersion?: string } & TapSuccessFailCallback): void;
    /** 返回上一个小程序 */
    navigateBackMiniProgram(params?: { extraData?: any } & TapSuccessFailCallback): void;
    /** 重启小程序 */
    restartMiniProgram(params?: TapSuccessFailCallback): void;

    // -------- Buffer --------

    /** 创建 Buffer URL */
    createBufferURL(buffer: ArrayBuffer): string;
    /** 销毁 Buffer URL */
    revokeBufferURL(url: string): void;

    // -------- 文件系统 --------

    /** 获取文件系统管理器 */
    getFileSystemManager(): TapFileSystemManager;

    // -------- 设备 - 剪贴板 --------

    /** 设置剪贴板内容 */
    setClipboardData(params: { data: string } & TapSuccessFailCallback): void;
    /** 获取剪贴板内容 */
    getClipboardData(params?: TapSuccessFailCallback<{ data: string }>): void;

    // -------- 设备 - 网络状态 --------

    /** 获取网络类型 */
    getNetworkType(params?: TapSuccessFailCallback<{ networkType: string; signalStrength: number; hasSystemProxy: boolean }>): void;
    /** 监听网络状态变化 */
    onNetworkStatusChange(callback: (res: { isConnected: boolean; networkType: string }) => void): void;
    /** 取消监听网络状态变化 */
    offNetworkStatusChange(callback?: Function): void;
    /** 获取本地 IP 地址 */
    getLocalIPAddress(params?: TapSuccessFailCallback<{ localip: string; netmask: string }>): void;
    /** 监听弱网状态变化 */
    onNetworkWeakChange(callback: (res: { weakNet: boolean; networkType: string }) => void): void;
    /** 取消监听弱网状态变化 */
    offNetworkWeakChange(callback?: Function): void;

    // -------- 设备 - 振动 --------

    /** 短振动 */
    vibrateShort(params?: { type?: string } & TapSuccessFailCallback): void;
    /** 长振动 */
    vibrateLong(params?: TapSuccessFailCallback): void;

    // -------- 设备 - 屏幕 --------

    /** 设置常亮 */
    setKeepScreenOn(params: { keepScreenOn: boolean } & TapSuccessFailCallback): void;
    /** 设置屏幕亮度 */
    setScreenBrightness(params: { value: number } & TapSuccessFailCallback): void;
    /** 获取屏幕亮度 */
    getScreenBrightness(params?: TapSuccessFailCallback<{ value: number }>): void;
    /** 监听用户截屏 */
    onUserCaptureScreen(callback: () => void): void;
    /** 取消监听用户截屏 */
    offUserCaptureScreen(callback?: Function): void;
    /** 设置截屏时的视觉效果 */
    setVisualEffectOnCapture(params: { visualEffect: string } & TapSuccessFailCallback): void;
    /** 获取屏幕录制状态 */
    getScreenRecordingState(params?: TapSuccessFailCallback<{ state: string }>): void;
    /** 监听屏幕录制状态变化 */
    onScreenRecordingStateChanged(callback: (res: { state: string }) => void): void;
    /** 取消监听屏幕录制状态变化 */
    offScreenRecordingStateChanged(callback?: Function): void;

    // -------- 设备 - 键盘 --------

    /** 显示键盘 */
    showKeyboard(params: { defaultValue?: string; maxLength?: number; multiple?: boolean; confirmHold?: boolean; confirmType?: string } & TapSuccessFailCallback): void;
    /** 隐藏键盘 */
    hideKeyboard(params?: TapSuccessFailCallback): void;
    /** 更新键盘输入框 */
    updateKeyboard(params: { value: string } & TapSuccessFailCallback): void;
    /** 监听键盘输入 */
    onKeyboardInput(callback: (res: { value: string }) => void): void;
    /** 取消监听键盘输入 */
    offKeyboardInput(callback?: Function): void;
    /** 监听键盘确认 */
    onKeyboardConfirm(callback: (res: { value: string }) => void): void;
    /** 取消监听键盘确认 */
    offKeyboardConfirm(callback?: Function): void;
    /** 监听键盘完成 */
    onKeyboardComplete(callback: (res: { value: string }) => void): void;
    /** 取消监听键盘完成 */
    offKeyboardComplete(callback?: Function): void;
    /** 监听键盘高度变化 */
    onKeyboardHeightChange(callback: (res: { height: number }) => void): void;
    /** 取消监听键盘高度变化 */
    offKeyboardHeightChange(callback?: Function): void;

    // -------- 设备 - 加速度计 --------

    /** 开始监听加速度数据 */
    startAccelerometer(params?: { interval?: string } & TapSuccessFailCallback): void;
    /** 停止监听加速度数据 */
    stopAccelerometer(params?: TapSuccessFailCallback): void;
    /** 监听加速度数据变化 */
    onAccelerometerChange(callback: (res: { x: number; y: number; z: number }) => void): void;
    /** 取消监听加速度数据变化 */
    offAccelerometerChange(callback?: Function): void;

    // -------- 设备 - 电池 --------

    /** 异步获取电池信息 */
    getBatteryInfo(params?: TapSuccessFailCallback<{ level: number; isCharging: boolean }>): void;
    /** 同步获取电池信息 */
    getBatteryInfoSync(): { level: number; isCharging: boolean };

    // -------- 设备 - 内存 --------

    /** 监听内存不足告警 */
    onMemoryWarning(callback: (res: { level: number }) => void): void;
    /** 取消监听内存不足告警 */
    offMemoryWarning(callback?: Function): void;

    // -------- 设备 - 触摸事件 --------

    /** 监听触摸开始 */
    onTouchStart(callback: (res: TapTouchEvent) => void): void;
    /** 取消监听触摸开始 */
    offTouchStart(callback?: Function): void;
    /** 监听触摸移动 */
    onTouchMove(callback: (res: TapTouchEvent) => void): void;
    /** 取消监听触摸移动 */
    offTouchMove(callback?: Function): void;
    /** 监听触摸结束 */
    onTouchEnd(callback: (res: TapTouchEvent) => void): void;
    /** 取消监听触摸结束 */
    offTouchEnd(callback?: Function): void;
    /** 监听触摸取消 */
    onTouchCancel(callback: (res: TapTouchEvent) => void): void;
    /** 取消监听触摸取消 */
    offTouchCancel(callback?: Function): void;

    // -------- 设备 - 扫码 --------

    /** 扫码 */
    scanCode(params?: { onlyFromCamera?: boolean; scanType?: string[] } & TapSuccessFailCallback<{ result: string; scanType: string; charSet: string }>): void;

    // -------- 网络 - 请求 --------

    /** 发起网络请求 */
    request(params: { url: string; data?: any; header?: any; method?: string; dataType?: string; responseType?: string; timeout?: number } & TapSuccessFailCallback<{ data: any; statusCode: number; header: any }>): TapRequestTask;

    // -------- 网络 - 下载 --------

    /** 下载文件 */
    downloadFile(params: { url: string; header?: any; filePath?: string; timeout?: number } & TapSuccessFailCallback<{ tempFilePath: string; filePath: string; statusCode: number }>): TapDownloadTask;

    // -------- 网络 - 上传 --------

    /** 上传文件 */
    uploadFile(params: { url: string; filePath: string; name: string; header?: any; formData?: any; timeout?: number } & TapSuccessFailCallback<{ data: string; statusCode: number }>): TapUploadTask;

    // -------- 网络 - WebSocket --------

    /** 创建 WebSocket 连接 */
    connectSocket(params: { url: string; header?: any; protocols?: string[]; timeout?: number } & TapSuccessFailCallback): TapSocketTask;

    // -------- 基础 - 生命周期 --------

    /** 监听小游戏回到前台 */
    onShow(callback: (res: { query: any; scene: number; shareTicket?: string; referrerInfo?: any }) => void): void;
    /** 取消监听小游戏回到前台 */
    offShow(callback?: Function): void;
    /** 监听小游戏切入后台 */
    onHide(callback: () => void): void;
    /** 取消监听小游戏切入后台 */
    offHide(callback?: Function): void;
    /** 同步获取启动参数 */
    getLaunchOptionsSync(): { query: any; scene: number; shareTicket?: string; referrerInfo?: any };
    /** 同步获取进入参数 */
    getEnterOptionsSync(): { query: any; scene: number; shareTicket?: string; referrerInfo?: any };

    // -------- 基础 - 系统信息 --------

    /** 同步获取系统信息 */
    getSystemInfoSync(): TapSystemInfo;
    /** 异步获取系统信息 */
    getSystemInfo(params?: TapSuccessFailCallback<TapSystemInfo>): void;
    /** 异步获取系统信息（Promise） */
    getSystemInfoAsync(): Promise<TapSystemInfo>;
    /** 获取窗口信息 */
    getWindowInfo(): { pixelRatio: number; screenWidth: number; screenHeight: number; windowWidth: number; windowHeight: number; statusBarHeight: number; safeArea: { left: number; right: number; top: number; bottom: number; width: number; height: number } };
    /** 获取设备信息 */
    getDeviceInfo(): { brand: string; model: string; system: string; platform: string; deviceOrientation: string };
    /** 获取 App 基础信息 */
    getAppBaseInfo(): { SDKVersion: string; enableDebug: boolean; host: any; language: string; version: string; theme: string };
    /** 获取系统设置 */
    getSystemSetting(): { bluetoothEnabled: boolean; locationEnabled: boolean; wifiEnabled: boolean; deviceOrientation: string };
    /** 获取 App 授权设置 */
    getAppAuthorizeSetting(): { albumAuthorized: string; bluetoothAuthorized: string; cameraAuthorized: string; locationAuthorized: string; locationReducedAccuracy: boolean; microphoneAuthorized: string; notificationAuthorized: string };
    /** 获取设备基准信息 */
    getDeviceBenchmarkInfo(): any;
    /** 打开 App 授权设置页 */
    openAppAuthorizeSetting(params?: TapSuccessFailCallback): void;
    /** 打开系统蓝牙设置页 */
    openSystemBluetoothSetting(params?: TapSuccessFailCallback): void;

    // -------- 基础 - 子包加载 --------

    /** 加载子包 */
    loadSubpackage(params: { name: string } & TapSuccessFailCallback): any;
    /** 预下载子包 */
    preDownloadSubpackage(params: { packageName: string } & TapSuccessFailCallback): any;

    // -------- 基础 - 更新管理 --------

    /** 获取更新管理器 */
    getUpdateManager(): TapUpdateManager;
    /** 更新 TapTap 客户端 */
    updateTapTapApp(params?: TapSuccessFailCallback): void;

    // -------- 数据分析 --------

    /** 上报场景数据 */
    reportScene(params: { sceneId: string; costTime?: number; dimension?: any; metric?: any }): void;

    // -------- 渲染 - Canvas --------

    /** 创建 Canvas */
    createCanvas(): any;
    /** 创建 Path2D */
    createPath2D(): any;

    // -------- 渲染 - 字体 --------

    /** 加载自定义字体 */
    loadFont(path: string): string;
    /** 获取文本行高 */
    getTextLineHeight(params: { fontFamily?: string; fontStyle?: string; fontWeight?: string; fontSize: number; text: string }): number;

    // -------- 渲染 - 帧率 --------

    /** 设置首选帧率 */
    setPreferredFramesPerSecond(fps: number): void;

    // -------- 渲染 - 图像 --------

    /** 创建 Image 对象 */
    createImage(): any;
    /** 创建 ImageData 对象 */
    createImageData(width: number, height: number): any;

    // -------- 媒体 - 图片 --------

    /** 选择图片 */
    chooseImage(params?: { count?: number; sizeType?: string[]; sourceType?: string[] } & TapSuccessFailCallback<{ tempFilePaths: string[]; tempFiles: any[] }>): void;
    /** 预览图片 */
    previewImage(params: { urls: string[]; current?: string; showmenu?: boolean } & TapSuccessFailCallback): void;
    /** 预览媒体 */
    previewMedia(params: { sources: any[]; current?: number; showmenu?: boolean } & TapSuccessFailCallback): void;
    /** 保存图片到相册 */
    saveImageToPhotosAlbum(params: { filePath: string } & TapSuccessFailCallback): void;
    /** 压缩图片 */
    compressImage(params: { src: string; quality?: number } & TapSuccessFailCallback<{ tempFilePath: string }>): void;

    // -------- 媒体 - 音频 --------

    /** 创建音频上下文 */
    createInnerAudioContext(): TapInnerAudioContext;

    // -------- 媒体 - 录音 --------

    /** 获取录音管理器 */
    getRecorderManager(): TapRecorderManager;

    // -------- 媒体 - 视频 --------

    /** 创建视频对象 */
    createVideo(params?: { x?: number; y?: number; width?: number; height?: number; src?: string; autoplay?: boolean; loop?: boolean; muted?: boolean }): TapVideo;
    /** 选择媒体文件 */
    chooseMedia(params?: { count?: number; mediaType?: string[]; sourceType?: string[]; maxDuration?: number; sizeType?: string[] } & TapSuccessFailCallback<{ tempFiles: any[]; type: string }>): void;

    // -------- 位置 --------

    /** 获取模糊位置 */
    getFuzzyLocation(params?: { type?: string } & TapSuccessFailCallback<{ latitude: number; longitude: number }>): void;
}

// 声明全局变量 tap，同时让 globalThis.tap 也有类型
// 这样 export const tap = globalThis.tap 导出后 tap.showToast 等方法都有完整提示
declare var tap: TapAPI;
