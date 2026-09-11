import { _decorator, instantiate, Label, Node, Prefab, ProgressBar, RichText, Component } from 'cc';
import { BundleManager } from '../bundle/BundleManager';
import { sleep } from '../other/sleep';
const { ccclass, property } = _decorator;

export class Loading {
    public progressBar:Node = null;
    public log:Label = null;

    private isInit = false;
    public _load:Node = null;

    public loadAsset(loading:Node)
    {
        this._load = loading;
        this.initProgressBar();
    }

    public async creatorPanel() {
        let lp = await BundleManager.Instance.loadAssetsFromBundle("loading", "loading") as Prefab;
        this._load = instantiate(lp);
        await this.initProgressBar();
    }

    public async initProgressBar() {
        this.progressBar = this._load.getChildByName("Progress");
        let progressBar = this.progressBar?.getComponent(ProgressBar);
        if (progressBar) {
            progressBar.progress = 0;
        }
        this.log=this._load.getChildByName("Log").getComponent(Label);
        this.log.string="";
    }

    public load(father:Node, desc:boolean = false) : (progress:number) => void 
    {
        console.log("Loading load begin!");

        try {
            if (!this.isInit) {
                father.addChild(this._load);
                this.isInit = true;
            }

            let descTxt = this._load.getChildByPath("Advice/desc");
            descTxt.active = desc;

            let titelTxt = this._load.getChildByPath("Advice/titel");
            titelTxt.active = desc;

            let ageTxt = this._load.getChildByPath("Advice/age");
            ageTxt.active = desc;

            let promptTxt = this._load.getChildByPath("Advice/prompt");
            promptTxt.active = desc;

            let Age = this._load.getChildByPath("Age");
            if (Age) {
                Age.active = desc;
            }

            return (progress:number) => { 
                if (!this.progressBar) {
                    return;
                }
                let progressBar = this.progressBar?.getComponent(ProgressBar);
                if (progressBar) {
                    progressBar.progress = 0 + progress; 
                }
            }
        }
        catch(err) {
            console.error("Loading 下的 Load 错误 err:",err);
        }

        return null;
    }

    public ShowLog(_msg:string,_progress:number)
    {
        switch (_msg)
        {
            case "Sound":
                {
                    this.log.string = "正在加载 音效 " + "(" + _progress + "%)";
                }
                break;
            case "RoleSpine":
                {
                    this.log.string = "正在加载 角色 " + "(" + _progress + "%)";
                }
                break;
            case "EffectSpine":
                {
                    this.log.string = "正在加载 特效 " + "(" + _progress + "%)";
                }
                break;
            case "SceneSpine":
                {
                    this.log.string = "正在加载 场景 " + "(" + _progress + "%)";
                }
                break;
            case "PanelSpine":
                {
                    this.log.string = "正在加载 面板 " + "(" + _progress + "%)";
                }
                break;
            case "SpecialSpine":
                {
                    this.log.string = "正在加载 装饰 " + "(" + _progress + "%)";
                }
                break;
            case "OtherTexture":
                {
                    this.log.string = "正在加载 纹理 " + "(" + _progress + "%)";
                }
                break;
            case "ButtonTexture":
                {
                    this.log.string = "正在加载 图标 " + "(" + _progress + "%)";
                }
                break;
            case "BackGroungTexture":
                {
                    this.log.string = "正在加载 背景 " + "(" + _progress + "%)";
                }
                break;
            case "BoardPrefabs":
                {
                    this.log.string = "正在加载 弹窗 " + "(" + _progress + "%)";
                }
                break;
            case "RolesTexture":
                {
                    this.log.string = "正在加载 角色 " + "(" + _progress + "%)";
                }
                break;
            case "Typeface":
                {
                    this.log.string = "正在加载 字体 " + "(" + _progress + "%)";
                }
                break;
            case "Ready":
                {
                    this.log.string = "正在加载 商店 " + "(" + _progress + "%)";
                }
                break;
            case "MainInterface":
                {
                    this.log.string = "正在加载 主界面 " + "(" + _progress + "%)";
                }
                break;
            case "MainPage":
                {
                    this.log.string = "正在加载 首包 " + "(" + _progress + "%)";
                }
                break;
            case "PanelPrefabs":
                {
                    this.log.string = "正在加载 界面 " + "(" + _progress + "%)";
                }
                break;
            default:
                {
                    this.log.string="正在加载 资源 " + "(" + _progress + "%)";
                }
        }
    }

    public done() {
        if (this._load != null) {
            this._load.active = false;
            this._load.destroy();
            this._load = null;
        }
    }
}