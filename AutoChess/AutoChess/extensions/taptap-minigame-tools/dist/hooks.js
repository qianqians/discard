"use strict";
/**
 * Tap小游戏构建钩子
 * 用途：在构建完成后自动调用TypeScript转换器
 */
var __createBinding = (this && this.__createBinding) || (Object.create ? (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    var desc = Object.getOwnPropertyDescriptor(m, k);
    if (!desc || ("get" in desc ? !m.__esModule : desc.writable || desc.configurable)) {
      desc = { enumerable: true, get: function() { return m[k]; } };
    }
    Object.defineProperty(o, k2, desc);
}) : (function(o, m, k, k2) {
    if (k2 === undefined) k2 = k;
    o[k2] = m[k];
}));
var __setModuleDefault = (this && this.__setModuleDefault) || (Object.create ? (function(o, v) {
    Object.defineProperty(o, "default", { enumerable: true, value: v });
}) : function(o, v) {
    o["default"] = v;
});
var __importStar = (this && this.__importStar) || (function () {
    var ownKeys = function(o) {
        ownKeys = Object.getOwnPropertyNames || function (o) {
            var ar = [];
            for (var k in o) if (Object.prototype.hasOwnProperty.call(o, k)) ar[ar.length] = k;
            return ar;
        };
        return ownKeys(o);
    };
    return function (mod) {
        if (mod && mod.__esModule) return mod;
        var result = {};
        if (mod != null) for (var k = ownKeys(mod), i = 0; i < k.length; i++) if (k[i] !== "default") __createBinding(result, mod, k[i]);
        __setModuleDefault(result, mod);
        return result;
    };
})();
Object.defineProperty(exports, "__esModule", { value: true });
exports.throwError = void 0;
exports.onBeforeBuild = onBeforeBuild;
exports.onAfterBuild = onAfterBuild;
const fs = __importStar(require("fs"));
const path = __importStar(require("path"));
const converter_ts_1 = require("./converter-ts");
const child_process_1 = require("child_process");
/**
 * 检查Node.js版本是否兼容
 */
function checkNodeVersionCompatibility() {
    const nodeVersion = process.version; // e.g., "v14.17.0"
    const majorVersion = parseInt(nodeVersion.split('.')[0].substring(1));
    if (majorVersion < 12) {
        return {
            compatible: false,
            version: nodeVersion,
            message: `Node.js版本过低（${nodeVersion}），需要12.0或更高版本。\n\n请升级Cocos Creator到3.8.0或更高版本。`
        };
    }
    return {
        compatible: true,
        version: nodeVersion,
        message: `Node.js版本兼容（${nodeVersion}）`
    };
}
/**
 * 检查Python环境是否可用
 */
async function checkPythonAvailable() {
    return new Promise((resolve) => {
        try {
            const python = process.platform === 'win32' ? 'python' : 'python3';
            const child = (0, child_process_1.spawn)(python, ['--version'], { stdio: 'pipe' });
            let output = '';
            let resolved = false;
            child.stdout.on('data', (data) => { output += data.toString(); });
            child.stderr.on('data', (data) => { output += data.toString(); });
            child.on('close', (code) => {
                if (resolved)
                    return;
                resolved = true;
                if (code === 0 && output.includes('Python')) {
                    resolve({ available: true, version: output.trim() });
                }
                else {
                    resolve({ available: false, version: '' });
                }
            });
            child.on('error', (error) => {
                if (resolved)
                    return;
                resolved = true;
                console.log('[Tap小游戏] Python检测出错:', error.message);
                resolve({ available: false, version: '' });
            });
            // 超时处理
            setTimeout(() => {
                if (resolved)
                    return;
                resolved = true;
                try {
                    child.kill();
                }
                catch (e) {
                    // 忽略kill错误
                }
                resolve({ available: false, version: '' });
            }, 3000);
        }
        catch (error) {
            // spawn本身可能失败
            console.log('[Tap小游戏] Python检测失败:', error.message);
            resolve({ available: false, version: '' });
        }
    });
}
/**
 * 检查转换器依赖是否完整
 */
function checkConverterDependencies() {
    // 修正路径：__dirname在编译后指向dist/，需要回到上层
    const converterDir = path.join(__dirname, '..', 'converter');
    const nodeModulesPath = path.join(converterDir, 'node_modules');
    const packageJsonPath = path.join(converterDir, 'package.json');
    const babelCorePath = path.join(nodeModulesPath, '@babel', 'core', 'lib', 'index.js');
    console.log('[Tap小游戏] 检查转换器依赖...');
    console.log('[Tap小游戏] converter目录:', converterDir);
    // 检查package.json
    if (!fs.existsSync(packageJsonPath)) {
        console.log('[Tap小游戏] ✗ package.json不存在');
        return {
            ok: false,
            message: '插件安装不完整，缺少converter/package.json。\n请重新安装插件。'
        };
    }
    console.log('[Tap小游戏] ✓ package.json存在');
    // 检查node_modules
    if (!fs.existsSync(nodeModulesPath)) {
        console.log('[Tap小游戏] ✗ node_modules不存在');
        return {
            ok: false,
            message: '转换器依赖未安装。\n\n首次使用会自动安装，请稍候1-2分钟。\n如果安装失败，请检查网络连接。'
        };
    }
    console.log('[Tap小游戏] ✓ node_modules存在');
    // 检查 @babel/core 模块是否安装
    if (!fs.existsSync(babelCorePath)) {
        console.log('[Tap小游戏] ✗ @babel/core未安装:', babelCorePath);
        return {
            ok: false,
            message: '@babel/core未安装。\n\n首次使用会自动安装，请稍候1-2分钟。'
        };
    }
    console.log('[Tap小游戏] ✓ @babel/core已安装');
    return {
        ok: true,
        message: '转换器依赖完整'
    };
}
/**
 * 构建前环境检查（完整版）
 */
async function checkEnvironment() {
    console.log('[Tap小游戏] ========================================');
    console.log('[Tap小游戏] 🔍 开始环境检查...');
    console.log('[Tap小游戏] ========================================');
    // 1. 检查Node.js版本
    const nodeCheck = checkNodeVersionCompatibility();
    console.log('[Tap小游戏] Node.js版本:', nodeCheck.version);
    // 2. 检查转换器依赖
    const depsCheck = checkConverterDependencies();
    // 3. 检查Python保底方案
    console.log('[Tap小游戏] 检查Python保底方案...');
    const pythonCheck = await checkPythonAvailable();
    if (pythonCheck.available) {
        console.log('[Tap小游戏] ✓ 检测到Python环境:', pythonCheck.version);
    }
    else {
        console.log('[Tap小游戏] - 未检测到Python环境');
    }
    // 综合判断
    if (!nodeCheck.compatible) {
        console.log('[Tap小游戏] ⚠️  Node.js版本较低（建议12.0+）');
        if (pythonCheck.available) {
            console.log('[Tap小游戏] 如果TypeScript转换失败，将自动使用Python保底方案');
            return {
                ok: true,
                message: `Node.js版本较低，但检测到${pythonCheck.version}作为保底方案。`,
                hasPythonFallback: true
            };
        }
        else {
            return {
                ok: true, // 仍然让它继续，转换时可能成功
                message: nodeCheck.message,
                hasPythonFallback: false
            };
        }
    }
    if (!depsCheck.ok) {
        console.log('[Tap小游戏] ⚠️  转换器依赖不完整');
        if (pythonCheck.available) {
            console.log('[Tap小游戏] 检测到Python环境，可作为保底方案');
            return {
                ok: true,
                message: `转换器依赖不完整，但检测到${pythonCheck.version}作为保底方案。\n\n${depsCheck.message}`,
                hasPythonFallback: true
            };
        }
        else {
            return {
                ok: true, // 让它继续，首次使用会自动安装依赖
                message: depsCheck.message,
                hasPythonFallback: false
            };
        }
    }
    console.log('[Tap小游戏] ✓ Node.js版本兼容');
    console.log('[Tap小游戏] ✓ 转换器依赖完整');
    console.log('[Tap小游戏] ========================================');
    console.log('[Tap小游戏] ✅ 环境检查通过');
    console.log('[Tap小游戏] ========================================');
    return {
        ok: true,
        message: '环境检查通过',
        hasPythonFallback: pythonCheck.available
    };
}
async function onBeforeBuild(options) {
    console.log('[Tap小游戏] 准备构建微信小游戏');
    // 检查是否启用了Tap转换，未启用则跳过后续检查
    const tapOptions = options.packages?.['taptap-minigame-tools'];
    if (!tapOptions || !tapOptions.enableTapConvert) {
        return;
    }
    // 引擎版本检查
    try {
        const editorVersion = Editor.App.version; // e.g., "3.8.4"
        console.log('[Tap小游戏] Cocos Creator版本:', editorVersion);
        const parts = editorVersion.split('.');
        const major = parseInt(parts[0]) || 0;
        const minor = parseInt(parts[1]) || 0;
        if (major < 3 || (major === 3 && minor < 8)) {
            console.log('[Tap小游戏] ⚠️  检测到低版本Cocos Creator:', editorVersion);
            const result = await Editor.Dialog.warn(`当前Cocos Creator版本为 ${editorVersion}，本插件推荐 3.8.0 及以上版本。\n\n低版本可能存在兼容性问题，是否继续构建？`, {
                title: 'Tap小游戏 - 版本提示',
                buttons: ['继续构建', '取消构建'],
                default: 1,
                cancel: 1,
            });
            // result.response: 0=继续构建, 1=取消构建
            if (result.response === 1) {
                throw new Error('用户取消构建：Cocos Creator版本过低');
            }
            console.log('[Tap小游戏] 用户选择继续构建');
        }
    }
    catch (error) {
        // 如果是用户主动取消，直接抛出
        if (error.message?.includes('用户取消构建')) {
            throw error;
        }
        // 其他错误（如 Editor.App.version 不存在）只记录日志
        console.log('[Tap小游戏] 版本检查跳过:', error.message);
    }
}
// Python相关代码已完全移除
// 只保留强制使用Python的选项（用户主动勾选时才使用）
async function onAfterBuild(options, result) {
    console.log('[Tap小游戏] 微信小游戏构建完成！');
    // 检查是否启用了Tap转换
    const tapOptions = options.packages?.['taptap-minigame-tools'];
    if (!tapOptions || !tapOptions.enableTapConvert) {
        console.log('[Tap小游戏] 未启用Tap转换，跳过');
        console.log('[Tap小游戏] 调试信息 - packages:', Object.keys(options.packages || {}));
        return;
    }
    console.log('[Tap小游戏] 开始转换为Tap小游戏...');
    try {
        // 获取微信小游戏构建路径
        const wechatBuildPath = result.dest;
        console.log('[Tap小游戏] 微信小游戏路径:', wechatBuildPath);
        // 创建TapBuild目录（与微信小游戏目录同级）
        const buildDir = path.dirname(wechatBuildPath);
        const tapBuildPath = path.join(buildDir, 'TapBuild');
        // 如果TapBuild目录存在，先删除
        if (fs.existsSync(tapBuildPath)) {
            console.log('[Tap小游戏] 删除旧的TapBuild目录...');
            fs.rmSync(tapBuildPath, { recursive: true, force: true });
        }
        // 创建新的TapBuild目录
        fs.mkdirSync(tapBuildPath, { recursive: true });
        console.log('[Tap小游戏] 创建TapBuild目录:', tapBuildPath);
        // 注意："强制使用Python脚本"选项已移除
        // 只使用TypeScript转换器
        console.log('[Tap小游戏] ========================================');
        console.log('[Tap小游戏] 📦 使用TypeScript转换器');
        console.log('[Tap小游戏] ========================================');
        // 版本号三段式格式校验
        let gameVersion = tapOptions.gameVersion || '0.0.1';
        if (!/^\d+\.\d+\.\d+$/.test(gameVersion)) {
            console.log(`[Tap小游戏] ⚠️  版本号格式不正确: "${gameVersion}"，已使用默认值 0.0.1`);
            gameVersion = '0.0.1';
        }
        await (0, converter_ts_1.convertWechatToTap)({
            source: wechatBuildPath,
            target: tapBuildPath,
            useSubpackage: false,
            gameVersion: gameVersion,
        });
        console.log('[Tap小游戏] ✅ TypeScript转换器执行成功');
        // 验证zip文件是否生成
        console.log('[Tap小游戏] ========================================');
        console.log('[Tap小游戏] ✅ 转换完成！');
        console.log('[Tap小游戏] ========================================');
        const gameZipPath = path.join(tapBuildPath, 'game.zip');
        if (fs.existsSync(gameZipPath)) {
            const stats = fs.statSync(gameZipPath);
            const sizeMB = (stats.size / 1024 / 1024).toFixed(2);
            console.log('[Tap小游戏] game.zip已生成:', gameZipPath);
            console.log('[Tap小游戏] 文件大小:', sizeMB, 'MB');
        }
        else {
            throw new Error('game.zip未生成，转换可能已失败，请检查上方日志');
        }
        // 检查是否需要删除微信小游戏包
        if (tapOptions.deleteWechatBuild) {
            console.log('[Tap小游戏] ========================================');
            console.log('[Tap小游戏] 🗑️  删除微信小游戏包...');
            console.log('[Tap小游戏] ========================================');
            try {
                if (fs.existsSync(wechatBuildPath)) {
                    console.log('[Tap小游戏] 删除目录:', wechatBuildPath);
                    fs.rmSync(wechatBuildPath, { recursive: true, force: true });
                    console.log('[Tap小游戏] ✅ 微信小游戏包已删除');
                }
                else {
                    console.log('[Tap小游戏] ⚠️  微信小游戏目录不存在，跳过删除');
                }
            }
            catch (deleteError) {
                console.log('[Tap小游戏] ⚠️  删除微信小游戏包失败:', deleteError.message);
                console.log('[Tap小游戏] 可以手动删除目录:', wechatBuildPath);
            }
        }
    }
    catch (error) {
        console.log('[Tap小游戏] ========================================');
        console.log('[Tap小游戏] ❌ 转换失败');
        console.log('[Tap小游戏] ========================================');
        console.log('[Tap小游戏] 错误详情:', error.message);
        // 弹出错误提示
        Editor.Dialog.error('转换失败，请查看控制台日志获取详细信息。\n\n' + error.message, {
            title: 'Tap小游戏',
            buttons: ['确定']
        });
        throw error;
    }
}
/**
 * 告知Cocos构建系统：hook抛出异常时立即停止构建
 */
exports.throwError = true;
