// Cocos Creator 编辑器全局类型声明
declare namespace Editor {
    namespace App {
        const version: string;
    }
    namespace Project {
        const path: string;
    }
    namespace Dialog {
        function info(message: string, options?: {
            title?: string;
            detail?: string;
            buttons?: string[];
        }): void;
        function warn(message: string, options?: {
            title?: string;
            detail?: string;
            buttons?: string[];
            default?: number;
            cancel?: number;
        }): Promise<{ response: number }>;
        function error(message: string, options?: {
            title?: string;
            detail?: string;
            buttons?: string[];
        }): void;
    }
}
