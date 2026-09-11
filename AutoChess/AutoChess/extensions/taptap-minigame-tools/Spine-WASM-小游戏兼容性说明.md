# Spine WASM 小游戏兼容性说明

> 本文档供 Unity 转换团队参考，记录 Cocos Creator 小游戏场景中 Spine WASM 的兼容性问题及已知方案。

## 问题描述

Cocos Creator 3.8.0 ~ 3.8.2 版本中，Spine 动画使用 WebAssembly (WASM) 模块实现高性能渲染。但在小游戏环境中，WASM 模块的加载方式与浏览器不同，可能导致以下问题：

- **加载失败**：小游戏环境不支持标准的 `WebAssembly.instantiateStreaming`
- **运行时崩溃**：WASM 内存模型与小游戏沙箱存在冲突
- **iOS 兼容性问题**：iOS 小游戏容器对 WASM 支持不完整

## iOS / Android 平台差异

| 平台 | WASM 支持 | 推荐方案 |
|------|-----------|----------|
| **Android** | 原生 WebAssembly 可用 | 直接使用 WASM 模块 |
| **iOS** | 部分环境不支持或性能差 | 降级为 ASM.js 模式 |

### iOS 降级策略

iOS 设备需要在运行时检测平台并切换到 ASM.js 版本：
- 检测 `navigator.platform` 或小游戏平台 API 判断是否为 iOS
- iOS 环境下加载 `.asm.js` 文件替代 `.wasm` 文件
- 需要构建时同时输出 WASM 和 ASM.js 两套产物

## Bilibili 小游戏方案摘要

Bilibili 小游戏插件采用了以下技术方案处理 Spine WASM 兼容性：

1. **构建时 AST 注入**：在构建阶段通过 AST 解析，注入平台检测代码
2. **自定义加载器**：替换默认的 WASM 加载器，使用小游戏专用的文件系统 API 加载
3. **条件编译**：根据目标平台自动选择 WASM 或 ASM.js 版本

核心思路：拦截 `WebAssembly.instantiate` 调用，替换为小游戏环境兼容的加载方式。

## 与 TapTap 小游戏的关系

当前 TapTap 小游戏转换脚本（`converter-ts.ts`）已处理了部分 WASM 兼容性问题：
- `handleWasmSplit()` 函数替换了 `GameGlobal.isIOSHighPerformanceMode` 等平台相关调用
- 但尚未处理 Spine 专用 WASM 模块的加载问题

**需要确认的关键问题**：TapTap 小游戏运行时环境是否存在与微信小游戏类似的 WASM 限制。

## 建议讨论的问题清单

1. **TapTap 运行时 WASM 支持**：TapTap 小游戏容器对 WebAssembly 的支持程度如何？是否与微信小游戏一致？
2. **iOS 降级必要性**：TapTap 小游戏在 iOS 上是否需要 ASM.js 降级方案？
3. **Spine 版本兼容**：当前转换脚本支持的 Spine 运行时版本范围是什么？
4. **性能影响评估**：ASM.js 降级后对 Spine 动画性能的影响程度？是否可接受？
5. **Unity 侧方案**：Unity 转换脚本中 Spine 是否也使用 WASM？如有，是否需要类似的兼容处理？
6. **Cocos 3.8.3+ 修复**：Cocos Creator 3.8.3 及以上版本是否已修复 Spine WASM 小游戏兼容问题？如已修复，是否可以将最低版本要求提升到 3.8.3？
