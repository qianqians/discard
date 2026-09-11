"use strict";
/**
 * Tap小游戏构建插件配置
 * 用途：扩展微信小游戏构建，添加Tap小游戏转换功能
 */
Object.defineProperty(exports, "__esModule", { value: true });
exports.assetHandlers = exports.configs = void 0;
exports.configs = {
    'wechatgame': {
        hooks: './hooks',
        options: {
            enableTapConvert: {
                label: '转换为Tap小游戏',
                description: '构建完成后自动转换为Tap小游戏格式',
                default: false,
                render: {
                    ui: 'ui-checkbox',
                },
            },
            tapAppid: {
                label: 'Tap小游戏AppID',
                description: '请输入Tap小游戏的AppID',
                default: '',
                render: {
                    ui: 'ui-input',
                    attributes: {
                        placeholder: '请输入Tap小游戏AppID',
                    },
                },
            },
            gameVersion: {
                label: '游戏版本号',
                description: '游戏版本号（三段式格式，如 1.0.0）',
                default: '0.0.1',
                render: {
                    ui: 'ui-input',
                    attributes: {
                        placeholder: '请输入版本号，如 1.0.0',
                    },
                },
            },
            usePythonScript: {
                label: '强制使用Python脚本',
                description: '跳过TypeScript转换器，直接使用Python脚本（需要本地安装Python 3.6+）',
                default: false,
                render: {
                    ui: 'ui-checkbox',
                },
            },
            deleteWechatBuild: {
                label: '删除微信小游戏包',
                description: '转换完成后自动删除微信小游戏目录（只保留Tap小游戏包）',
                default: false,
                render: {
                    ui: 'ui-checkbox',
                },
            },
        },
    },
};
// Cocos Creator需要的空处理器
exports.assetHandlers = './asset-handlers';
