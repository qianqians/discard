"use strict";
/**
 * Tap小游戏转换器 - TypeScript版本
 * 用途：替代Python脚本，实现微信小游戏到Tap小游戏的转换
 * 无需Python环境，完全基于Node.js
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
var __importDefault = (this && this.__importDefault) || function (mod) {
    return (mod && mod.__esModule) ? mod : { "default": mod };
};
Object.defineProperty(exports, "__esModule", { value: true });
exports.convertWechatToTap = convertWechatToTap;
const fs = __importStar(require("fs-extra"));
const path = __importStar(require("path"));
const archiver_1 = __importDefault(require("archiver"));
const child_process_1 = require("child_process");
const util_1 = require("util");
const execAsync = (0, util_1.promisify)(child_process_1.exec);
const CONVERTER_VERSION = "2.0.4-ts";
const DEFAULT_COMPANY = "DefaultCompany";
const DEFAULT_PRODUCT = "My Project";
const DEFAULT_VERSION = "0.1";
let COVERVIEW_CUSTOMIZED = false;
/**
 * 步骤2: 验证源路径
 */
function validateSourcePath(sourcePath) {
    console.log('[Tap小游戏] 正在验证微信小游戏源路径...');
    if (!fs.existsSync(sourcePath)) {
        console.log('[Tap小游戏] ❌ 源路径不存在:', sourcePath);
        throw new Error(`源路径不存在: ${sourcePath}`);
    }
    console.log('[Tap小游戏] ✓ 源路径验证通过');
    console.log('[Tap小游戏] 源路径:', sourcePath);
}
/**
 * 步骤2: 创建目标路径
 */
function ensureTargetPath(targetPath) {
    const targetParent = targetPath;
    const gameDir = path.join(targetParent, 'game');
    console.log('[Tap小游戏] 正在准备输出目录...');
    // 如果目标目录存在且不为空，先删除
    if (fs.existsSync(targetParent)) {
        console.log('[Tap小游戏] 删除旧的目标目录...');
        fs.removeSync(targetParent);
    }
    // 创建目录
    fs.ensureDirSync(gameDir);
    console.log('[Tap小游戏] ✓ 目标目录创建成功');
    console.log('[Tap小游戏] 输出目录:', gameDir);
    return gameDir;
}
/**
 * 步骤3: 复制文件（排除特定文件）
 */
function copyAssets(source, target) {
    console.log('[Tap小游戏] 正在复制微信小游戏文件...');
    console.log('[Tap小游戏] 源目录:', source);
    console.log('[Tap小游戏] 目标目录:', target);
    // 复制文件，排除 .* __pycache__ *.meta *.bak
    fs.copySync(source, target, {
        filter: (src) => {
            const basename = path.basename(src);
            // 排除隐藏文件、meta文件、备份文件、缓存目录
            if (basename.startsWith('.'))
                return false;
            if (basename === '__pycache__')
                return false;
            if (basename.endsWith('.meta'))
                return false;
            if (basename.endsWith('.bak'))
                return false;
            return true;
        }
    });
    console.log('[Tap小游戏] ✓ 文件复制完成');
}
/**
 * 步骤4: 处理game.json配置
 */
function handleGameConfig(targetFolder, gameVersion) {
    console.log('[Tap小游戏] 处理game.json配置...');
    let configPath = path.join(targetFolder, 'game.json');
    // 如果game.json不存在，尝试manifest.json
    if (!fs.existsSync(configPath)) {
        configPath = path.join(targetFolder, 'manifest.json');
        if (!fs.existsSync(configPath)) {
            throw new Error('缺少game.json或manifest.json配置文件');
        }
    }
    // 读取配置
    const config = fs.readJsonSync(configPath);
    // 更新配置（保留原始的 companyName 和 productName）
    config.productVersion = gameVersion || DEFAULT_VERSION;
    config.convertScriptVersion = CONVERTER_VERSION;
    // 记录coverviewCustomized设置
    COVERVIEW_CUSTOMIZED = config.coverviewCustomized || false;
    // 处理orientation字段
    if (config.orientation) {
        config.deviceOrientation = config.orientation;
        delete config.orientation;
    }
    // 写回配置
    fs.writeJsonSync(configPath, config, { spaces: 2, encoding: 'utf-8' });
    console.log('[Tap小游戏] 游戏配置:');
    console.log('  companyName:', config.companyName);
    console.log('  productName:', config.productName);
    console.log('  productVersion:', config.productVersion);
    console.log('[Tap小游戏] ✓ 配置处理完成');
    return config;
}
/**
 * 步骤5: 复制插件文件
 */
function copyPlugins(targetFolder, config, converterDir) {
    console.log('[Tap小游戏] 复制插件文件...');
    const plugins = config.plugins || {};
    const cacheDir = path.join(converterDir, 'wx_unity_converter');
    for (const [pluginName, pluginInfo] of Object.entries(plugins)) {
        const version = pluginInfo.version;
        const provider = pluginInfo.provider;
        // 处理UnityPlugin
        if (pluginName === 'UnityPlugin') {
            handleUnityPlugin(targetFolder, converterDir);
            continue;
        }
        // 处理特殊插件（不需要版本号）
        let pluginPath;
        if (['MinigameLoading', 'MiniGameCommon', 'MiniGameCenter'].includes(pluginName)) {
            pluginPath = path.join(cacheDir, pluginName);
        }
        else {
            pluginPath = path.join(cacheDir, `${pluginName}-${version}`);
        }
        // 复制插件
        if (fs.existsSync(pluginPath)) {
            const targetPluginDir = path.join(targetFolder, 'cachedPlugin', pluginName);
            copyCachedPlugin(pluginPath, targetPluginDir);
            console.log('  ✓ 复制插件:', pluginName);
        }
        else {
            console.log('  - 跳过不支持的插件:', pluginName);
        }
    }
    console.log('[Tap小游戏] ✓ 插件复制完成');
}
/**
 * 复制缓存的插件目录
 */
function copyCachedPlugin(source, target) {
    if (fs.existsSync(target)) {
        fs.removeSync(target);
    }
    fs.copySync(source, target);
}
/**
 * 处理UnityPlugin
 */
function handleUnityPlugin(targetFolder, converterDir) {
    const pluginDir = path.join(targetFolder, 'cachedPlugin', 'UnityPlugin');
    fs.ensureDirSync(pluginDir);
    const defaultPlugin = path.join(converterDir, 'libs', 'UnityPlugin', 'dist', 'index.js');
    if (!fs.existsSync(defaultPlugin)) {
        throw new Error('UnityPlugin不存在: ' + defaultPlugin);
    }
    fs.copySync(defaultPlugin, path.join(pluginDir, 'index.js'));
    console.log('  ✓ 复制UnityPlugin');
}
/**
 * 检查Node.js版本兼容性
 */
function checkNodeVersion() {
    const nodeVersion = process.version; // e.g., "v14.17.0"
    const majorVersion = parseInt(nodeVersion.split('.')[0].substring(1));
    console.log('[Tap小游戏] Node.js版本:', nodeVersion);
    if (majorVersion < 12) {
        console.log('[Tap小游戏] ⚠️  警告：Node.js版本过低');
        console.log('[Tap小游戏] 当前版本:', nodeVersion);
        console.log('[Tap小游戏] 最低要求: Node.js 12+');
        console.log('[Tap小游戏] 建议升级Cocos Creator到最新版本');
    }
    else {
        console.log('[Tap小游戏] ✓ Node.js版本兼容');
    }
}
/**
 * 检查并安装converter依赖（带重试机制）
 */
async function ensureConverterDependencies(converterDir) {
    const nodeModulesPath = path.join(converterDir, 'node_modules');
    const packageJsonPath = path.join(converterDir, 'package.json');
    console.log('========================================');
    console.log('[Tap小游戏] 步骤1: 检查转换器依赖');
    console.log('========================================');
    // 检查Node.js版本
    checkNodeVersion();
    // 检查node_modules是否存在
    if (fs.existsSync(nodeModulesPath)) {
        console.log('[Tap小游戏] ✓ 转换器依赖已安装');
        console.log('[Tap小游戏] 依赖路径:', nodeModulesPath);
        return;
    }
    // 检查package.json是否存在
    if (!fs.existsSync(packageJsonPath)) {
        console.log('[Tap小游戏] ❌ 未找到converter/package.json');
        console.log('[Tap小游戏] 预期路径:', packageJsonPath);
        throw new Error('转换器配置文件不存在，插件可能未正确安装');
    }
    console.log('[Tap小游戏] ⚠️  检测到首次使用，需要安装转换器依赖');
    console.log('[Tap小游戏] 安装目录:', converterDir);
    console.log('[Tap小游戏] 这可能需要1-2分钟，请耐心等待...');
    console.log('');
    // 最多重试3次
    const maxRetries = 3;
    let lastError = null;
    for (let attempt = 1; attempt <= maxRetries; attempt++) {
        try {
            console.log(`[Tap小游戏] 尝试安装依赖 (${attempt}/${maxRetries})...`);
            const { stdout, stderr } = await execAsync('npm install', {
                cwd: converterDir,
                maxBuffer: 10 * 1024 * 1024, // 10MB buffer
                timeout: 120000 // 2分钟超时
            });
            // 输出安装日志（简化版）
            if (stdout) {
                const lines = stdout.split('\n').filter(line => line.trim());
                if (lines.length > 0) {
                    console.log('[npm]', lines[lines.length - 1]); // 只显示最后一行
                }
            }
            // 验证安装是否成功
            if (fs.existsSync(nodeModulesPath)) {
                console.log('[Tap小游戏] ✓ 依赖安装成功！');
                console.log('[Tap小游戏] 已安装路径:', nodeModulesPath);
                return;
            }
            else {
                throw new Error('安装完成但node_modules目录未生成');
            }
        }
        catch (error) {
            lastError = error;
            console.log(`[Tap小游戏] ✗ 第${attempt}次安装失败`);
            if (error.code === 'ETIMEDOUT') {
                console.log('[Tap小游戏] 原因: 安装超时（网络可能较慢）');
            }
            else if (error.message) {
                console.log('[Tap小游戏] 原因:', error.message.split('\n')[0]); // 只显示第一行错误
            }
            // 如果还有重试机会，等待后重试
            if (attempt < maxRetries) {
                console.log('[Tap小游戏] 等待5秒后重试...');
                await new Promise(resolve => setTimeout(resolve, 5000));
            }
        }
    }
    // 所有重试都失败
    console.log('');
    console.log('========================================');
    console.log('[Tap小游戏] ❌ 依赖安装失败（已重试3次）');
    console.log('========================================');
    console.log('[Tap小游戏] 错误详情:', lastError?.message || '未知错误');
    console.log('[Tap小游戏] ');
    console.log('[Tap小游戏] 解决方案：');
    console.log('[Tap小游戏] 1. 检查网络连接是否正常');
    console.log('[Tap小游戏] 2. 手动在以下目录执行 npm install：');
    console.log('[Tap小游戏]    ', converterDir);
    console.log('[Tap小游戏] 3. 或者尝试切换npm镜像源：');
    console.log('[Tap小游戏]    npm config set registry https://registry.npmmirror.com');
    console.log('========================================');
    console.log('');
    throw new Error(`转换器依赖安装失败（已重试${maxRetries}次）。请检查网络连接或手动安装依赖。`);
}
/**
 * 递归收集目录下所有 .js 文件
 */
function collectJsFiles(dir) {
    const results = [];
    const entries = fs.readdirSync(dir);
    for (const entry of entries) {
        const fullPath = path.join(dir, entry);
        const stat = fs.statSync(fullPath);
        if (stat.isDirectory()) {
            results.push(...collectJsFiles(fullPath));
        }
        else if (entry.endsWith('.js')) {
            results.push(fullPath);
        }
    }
    return results;
}
/**
 * 步骤6: 运行Babel转换（使用 @babel/core Node.js API）
 */
async function runBabelTransform(targetFolder, converterDir) {
    console.log('[Tap小游戏] 正在进行JavaScript兼容性转换...');
    // 检查是否已经转换过
    const babelDir = path.join(targetFolder, '@babel');
    if (fs.existsSync(babelDir)) {
        console.log('[Tap小游戏] ✓ Babel已转换，跳过');
        return;
    }
    // 加载 @babel/core
    const babelCorePath = path.join(converterDir, 'node_modules', '@babel', 'core');
    let babel;
    try {
        babel = require(babelCorePath);
    }
    catch (error) {
        throw new Error(`加载 @babel/core 失败: ${error.message}\n请在 ${converterDir} 目录下执行 npm install`);
    }
    // 读取 .babelrc 配置
    const babelrcPath = path.join(converterDir, '.babelrc');
    console.log('[Tap小游戏] Babel配置:', babelrcPath);
    console.log('[Tap小游戏] 转换目录:', targetFolder);
    let babelConfig;
    try {
        babelConfig = fs.readJsonSync(babelrcPath);
    }
    catch (error) {
        throw new Error(`读取 .babelrc 失败: ${error.message}`);
    }
    // 从 .babelrc 中提取 ignore 列表
    const ignoreFiles = babelConfig.ignore || [];
    // 构建 transformFileSync 的配置（解析 preset 路径到 converter 的 node_modules 下）
    const transformConfig = {
        sourceType: babelConfig.sourceType || 'unambiguous',
        presets: (babelConfig.presets || []).map((preset) => {
            if (typeof preset === 'string') {
                // preset 可能是完整名称如 "@babel/preset-env"，也可能是短名如 "env"
                const presetName = preset.startsWith('@babel/') ? preset : `@babel/preset-${preset}`;
                return require.resolve(presetName, { paths: [converterDir] });
            }
            return preset;
        }),
    };
    // 收集所有 .js 文件
    const jsFiles = collectJsFiles(targetFolder);
    console.log(`[Tap小游戏] 发现 ${jsFiles.length} 个JS文件需要转换`);
    let successCount = 0;
    let skipCount = 0;
    let errorCount = 0;
    for (const filePath of jsFiles) {
        const fileName = path.basename(filePath);
        // 跳过 ignore 列表中的文件
        if (ignoreFiles.includes(fileName)) {
            skipCount++;
            console.log(`[Babel] 跳过: ${fileName}`);
            continue;
        }
        // 跳过 Cocos 引擎文件（cc.HASH.js），这些文件很大且不需要转换
        if (/^cc\.[a-f0-9]+\.js$/.test(fileName)) {
            skipCount++;
            console.log(`[Babel] 跳过引擎文件: ${fileName}`);
            continue;
        }
        try {
            const result = babel.transformFileSync(filePath, transformConfig);
            if (result && result.code != null) {
                fs.writeFileSync(filePath, result.code, 'utf-8');
                successCount++;
            }
        }
        catch (error) {
            errorCount++;
            console.log(`[Babel] 转换失败: ${fileName} - ${error.message}`);
        }
    }
    console.log(`[Tap小游戏] Babel转换完成: 成功 ${successCount}, 跳过 ${skipCount}, 失败 ${errorCount}`);
    if (errorCount > 0 && successCount === 0) {
        throw new Error(`Babel转换全部失败（${errorCount}个文件）`);
    }
    console.log('[Tap小游戏] ✓ Babel转换完成');
}
/**
 * 步骤8: 注入运行时代码到game.js
 */
function injectRuntimeCode(targetFolder, converterDir) {
    console.log('[Tap小游戏] 注入运行时代码...');
    const gameJsPath = path.join(targetFolder, 'game.js');
    if (!fs.existsSync(gameJsPath)) {
        throw new Error('game.js不存在');
    }
    const injectionFile = path.join(converterDir, 'wx_unity_converter', 'wx_unity.js');
    if (!fs.existsSync(injectionFile)) {
        console.log('[Tap小游戏] ⚠️ wx_unity.js不存在，跳过注入');
        return;
    }
    const injectionCode = fs.readFileSync(injectionFile, 'utf-8');
    const originalContent = fs.readFileSync(gameJsPath, 'utf-8');
    const newContent = `/* Unity Converter Injection */\n${injectionCode}\n${originalContent}`;
    fs.writeFileSync(gameJsPath, newContent, 'utf-8');
    console.log('[Tap小游戏] ✓ 运行时代码注入完成');
}
/**
 * 步骤9: 处理wasm-split.js
 */
function handleWasmSplit(targetFolder) {
    console.log('[Tap小游戏] 处理WASM兼容性...');
    const wasmSplitPath = path.join(targetFolder, 'wasm-split.js');
    if (!fs.existsSync(wasmSplitPath)) {
        console.log('[Tap小游戏] - wasm-split.js不存在，跳过');
        return;
    }
    console.log('[Tap小游戏] 处理wasm-split.js替换...');
    let content = fs.readFileSync(wasmSplitPath, 'utf-8');
    // 替换字符串
    const replacements = [
        { old: 'GameGlobal.isIOSHighPerformanceMode', new: '__rep_isIOSHighPerformanceMode' },
        { old: 'GameGlobal.canUseH5Renderer', new: '__rep_canUseH5Renderer' }
    ];
    let replacedCount = 0;
    let prependCode = '';
    for (const { old, new: newStr } of replacements) {
        if (content.includes(old)) {
            content = content.replace(new RegExp(old, 'g'), newStr);
            prependCode += `var ${newStr} = false;\n`;
            replacedCount++;
            console.log(`  - 替换: ${old} -> ${newStr}`);
        }
    }
    // 如果有替换，添加变量声明到文件开头
    if (replacedCount > 0) {
        content = prependCode + content;
        fs.writeFileSync(wasmSplitPath, content, 'utf-8');
    }
    console.log(`[Tap小游戏] ✓ WASM处理完成，替换了${replacedCount}个模式`);
}
/**
 * 步骤10: 处理coverviewCustomized
 */
function handleCustomizedCoverview(targetFolder) {
    console.log('[Tap小游戏] 处理coverviewCustomized设置...');
    const indexJsPath = path.join(targetFolder, 'cachedPlugin', 'UnityPlugin', 'index.js');
    if (!fs.existsSync(indexJsPath)) {
        console.log('[Tap小游戏] - UnityPlugin/index.js不存在，跳过');
        return;
    }
    const coverviewValue = COVERVIEW_CUSTOMIZED ? 'true' : 'false';
    const coverviewCode = `GameGlobal.pluginEnv.coverviewCustomized = ${coverviewValue};`;
    const originalContent = fs.readFileSync(indexJsPath, 'utf-8');
    const newContent = `${coverviewCode}\n${originalContent}`;
    fs.writeFileSync(indexJsPath, newContent, 'utf-8');
    console.log('[Tap小游戏] ✓ coverviewCustomized设置完成');
}
/**
 * 步骤11: 复制check-version.js
 */
function copyVersionChecker(targetFolder, converterDir) {
    console.log('[Tap小游戏] 复制版本检查文件...');
    const sourceFile = path.join(converterDir, 'wx_unity_converter', 'check-version.js');
    const targetFile = path.join(targetFolder, 'check-version.js');
    if (fs.existsSync(sourceFile)) {
        fs.copySync(sourceFile, targetFile);
        console.log('[Tap小游戏] ✓ 版本检查文件复制完成');
    }
    else {
        console.log('[Tap小游戏] - check-version.js不存在，跳过');
    }
}
/**
 * 步骤12: 打包成ZIP
 */
async function packGame(targetFolder, config, useSubpackage) {
    console.log('[Tap小游戏] 创建ZIP包...');
    const targetParent = path.dirname(targetFolder);
    const mainZipPath = path.join(targetParent, 'game.zip');
    // 删除旧的zip文件
    if (fs.existsSync(mainZipPath)) {
        fs.removeSync(mainZipPath);
    }
    return new Promise((resolve, reject) => {
        const output = fs.createWriteStream(mainZipPath);
        const archive = (0, archiver_1.default)('zip', {
            zlib: { level: 9 } // 最大压缩
        });
        output.on('close', () => {
            const sizeBytes = archive.pointer();
            const sizeMB = (sizeBytes / 1024 / 1024).toFixed(2);
            console.log(`[Tap小游戏] ✓ game.zip创建完成: ${sizeMB} MB`);
            resolve();
        });
        archive.on('error', (err) => {
            reject(err);
        });
        archive.pipe(output);
        // 添加game目录下的所有文件
        archive.directory(targetFolder, false);
        archive.finalize();
    });
}
/**
 * 主转换流程
 */
async function convertWechatToTap(options) {
    console.log('\n=== Tap小游戏转换器 (TypeScript版本) ===\n');
    const converterDir = path.join(__dirname, '..', 'converter');
    try {
        // 1. 检查并安装转换器依赖（必须第一步）
        await ensureConverterDependencies(converterDir);
        // 2. 验证路径
        console.log('\n========================================');
        console.log('[Tap小游戏] 步骤2: 验证源路径');
        console.log('========================================');
        validateSourcePath(options.source);
        const targetFolder = ensureTargetPath(options.target);
        // 3. 复制文件
        console.log('\n========================================');
        console.log('[Tap小游戏] 步骤3: 复制项目文件');
        console.log('========================================');
        copyAssets(options.source, targetFolder);
        // 4. 处理配置
        console.log('\n========================================');
        console.log('[Tap小游戏] 步骤4: 处理game.json配置');
        console.log('========================================');
        const config = handleGameConfig(targetFolder, options.gameVersion);
        // 5. 复制插件
        console.log('\n========================================');
        console.log('[Tap小游戏] 步骤5: 注入插件');
        console.log('========================================');
        copyPlugins(targetFolder, config, converterDir);
        // 6. Babel转换
        console.log('\n========================================');
        console.log('[Tap小游戏] 步骤6: 运行Babel转换');
        console.log('========================================');
        await runBabelTransform(targetFolder, converterDir);
        // 7. 注入运行时代码
        console.log('\n========================================');
        console.log('[Tap小游戏] 步骤7: 注入运行时代码');
        console.log('========================================');
        injectRuntimeCode(targetFolder, converterDir);
        // 8. 处理WASM
        console.log('\n========================================');
        console.log('[Tap小游戏] 步骤8: 处理WASM兼容性');
        console.log('========================================');
        handleWasmSplit(targetFolder);
        // 9. 处理Coverview
        console.log('\n========================================');
        console.log('[Tap小游戏] 步骤9: 处理coverviewCustomized设置');
        console.log('========================================');
        handleCustomizedCoverview(targetFolder);
        // 10. 复制版本检查
        console.log('\n========================================');
        console.log('[Tap小游戏] 步骤10: 复制版本检查文件');
        console.log('========================================');
        copyVersionChecker(targetFolder, converterDir);
        // 11. 打包ZIP
        console.log('\n========================================');
        console.log('[Tap小游戏] 步骤11: 创建分发包');
        console.log('========================================');
        await packGame(targetFolder, config, options.useSubpackage || false);
        console.log('\n========================================');
        console.log('[Tap小游戏] ✅ 转换完成！');
        console.log('========================================');
        console.log('[Tap小游戏] 输出目录:', options.target);
        console.log('[Tap小游戏] game.zip已生成，可以上传到TapTap开发者中心');
        console.log('========================================\n');
    }
    catch (error) {
        console.log('\n========================================');
        console.log('[Tap小游戏] ❌ 转换失败');
        console.log('========================================');
        console.log('[Tap小游戏] 错误信息:', error.message);
        console.log('========================================\n');
        throw error;
    }
}
