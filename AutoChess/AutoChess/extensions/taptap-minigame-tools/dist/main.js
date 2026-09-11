"use strict";
/**
 * Tap小游戏构建扩展
 * 用途：在Cocos Creator中添加构建Tap小游戏的功能，附带 tap API 类型定义和 AI Skills
 */
Object.defineProperty(exports, "__esModule", { value: true });
exports.methods = void 0;
exports.load = load;
exports.unload = unload;
const TAG = '[Tap小游戏]';
const VERSION = '1.2.0';
function load() {
    console.log(`${TAG} ========================================`);
    console.log(`${TAG} TapTap 小游戏构建插件 v${VERSION}`);
    console.log(`${TAG} ----------------------------------------`);
    console.log(`${TAG} 类型定义已就绪 → 代码中输入 tap. 即可获得提示`);
    console.log(`${TAG} ----------------------------------------`);
    console.log(`${TAG} AI Skills 位于 extensions/taptap-minigame-tools/ai-skills/`);
    console.log(`${TAG}   tap-sdk-skill.md        → SDK 基础模块`);
    console.log(`${TAG}   tap-ad-skill.md         → 广告模块`);
    console.log(`${TAG}   tap-cloud-save-skill.md → 云存档模块`);
    console.log(`${TAG} ========================================`);
}
function unload() {
    console.log(`${TAG} 扩展已卸载`);
}
exports.methods = {
    onBuildTapGame() {
        console.log(`${TAG} 点击了构建Tap小游戏菜单`);
        Editor.Dialog.info('成功', {
            title: 'Tap小游戏',
            detail: '菜单功能正常！下一步将实现构建功能。'
        });
    }
};
