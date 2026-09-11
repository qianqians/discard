# TapTap小游戏构建工具

一键构建微信小游戏并自动转换为Tap小游戏格式

## 兼容性

### 支持的Cocos Creator版本
- ✅ **Cocos Creator 3.8.1** - 已测试
- ✅ **Cocos Creator 3.8.8** - 已测试
- ⚠️  **其他版本** - 理论支持，但未充分测试

### 技术要求
- **Node.js**: 12.0 或更高（Cocos Creator内置）
- **npm**: 6.0 或更高

### 已知兼容性问题
如果遇到以下错误，请参考解决方案：

#### 错误1: `Cannot find module 'node:url'`
**原因**: Cocos Creator内置的Node.js版本过低
**解决方案**:
1. 升级Cocos Creator到3.8.0或更高版本
2. 或联系我们获取兼容版本的插件

#### 错误2: `Babel转换失败`
**原因**: 首次使用需要安装依赖
**解决方案**: 插件会自动重试3次，请耐心等待1-2分钟

### 报告问题
如果在您的Cocos Creator版本上遇到问题，请提供：
- Cocos Creator版本号
- 完整的错误日志
- 操作系统信息。

## 使用方法

### 常规使用（推荐）
1. 打开 **项目 → 构建发布**
2. 选择平台：**微信小游戏**
3. ☑️ 勾选 **"转换为Tap小游戏"**
4. 点击 **构建**

### 测试Python脚本
1. 打开 **项目 → 构建发布**
2. 选择平台：**微信小游戏**
3. ☑️ 勾选 **"转换为Tap小游戏"**
4. ☑️ 勾选 **"强制使用Python脚本"**（需要安装Python 3.6+）
5. 点击 **构建**

## 输出结果

构建完成后生成：
- **微信小游戏**：`build/wechatgame/`
- **Tap小游戏**：`build/TapBuild/game.zip`

最终的 `game.zip` 文件可直接上传到Tap小游戏平台。

## 转换流程

自动完成11个步骤：
1. 检查转换器依赖（已集成，无需安装）
2. 验证路径
3. 复制项目文件
4. 处理配置文件
5. 注入插件
6. Babel代码转换
7. 注入运行时代码
8. WASM兼容性处理
9. Coverview设置
10. 版本检查文件
11. ZIP打包

## 特色功能

### 📦 集成依赖，解压即用
- 插件包已包含完整的Babel转换器依赖（6.6MB）
- **无需npm install**
- **无需网络连接**
- **零配置，零等待**
- 首次使用和后续使用体验完全一致

### 🛡️ 双重保障机制
- **优先**：TypeScript转换器（快速、自动）
- **保底**：Python脚本（需要Python 3.6+）
- 自动降级：TS失败自动切换Python
- 手动选择：可勾选"强制使用Python脚本"

### 🔍 完善的环境检查
- 构建前检查所有必要环境
- 详细的检查日志输出
- 准确的警告和建议

## tap API 类型定义

插件自带完整的 `tap.*` API 类型定义，安装插件后**零配置**即可使用：

- 在任意 `.ts` 文件中输入 `tap.` 即可获得代码补全
- 覆盖所有 API：基础、分享、广告、成就、排行榜、云存档
- 原理：类型定义文件在插件目录中，TypeScript 自动扫描识别

## AI Skills

插件内置 AI Skills 文件，位于 `ai-skills/` 目录，可配合 AI 编程工具快速生成 SDK 适配层代码：

- `tap-sdk-skill.md` → SDK 基础模块（平台检测、登录、分享、排行榜、成就）
- `tap-ad-skill.md` → 广告模块（激励视频、插屏、Banner）
- `tap-cloud-save-skill.md` → 云存档模块

## 插件开发

### 编译

```bash
cd extensions/taptap-minigame-tools
node node_modules/typescript/bin/tsc
```

### 打包发布

```bash
cd extensions/taptap-minigame-tools
bash build.sh
```

自动完成：编译 TypeScript → 删旧 ZIP → 打包新 ZIP。版本号从 `package.json` 自动读取，输出到上级目录。

### 更新类型定义

全量 tap API 类型定义由 Python 脚本生成：

```bash
python3 /Volumes/Q/MiniGame/TapSDK/generate_tap_dts.py
```

生成后复制到插件目录，再执行 `bash build.sh` 打包。

## 技术支持

开发者：TapTap
版本：1.2.0
