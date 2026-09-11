import { _decorator, Animation, Component, instantiate, Node, TTFFont, Prefab, RichText } from 'cc';
import { BundleManager } from '../bundle/BundleManager';
import { InfoBoard } from '../secondaryPanel/InfoBoard';
import { SendMessage } from './MessageEvent';
import { Settlement } from '../secondaryPanel/Settlement';
import { UserInfo } from '../secondaryPanel/UserInfo';
import { RankList } from '../secondaryPanel/RankList';
import { AudioManager } from './AudioManager';
import * as enums from '../other/enums';
import { PopUps } from '../secondaryPanel/PopUps';
import { ChooseTag } from '../secondaryPanel/ChooseTag';
import { sleep } from './sleep';
import { LevelInfo } from '../secondaryPanel/LevelInfo';
import { config } from '../battle/AutoChessBattle/config/config';
import { OptionsData, User } from '../login/User';
const { ccclass, property } = _decorator;

@ccclass('GameManager')
export class GameManager extends Component 
{
    private static _instance:GameManager=null;

    public static get Instance()
    {
        return this._instance;
    }
    private typeface: TTFFont=null;
    //升阶界面
    private upStageBoard:Node=null;
    //等待界面
    private waitingPanel:Node=null;
    //侦听计时器
    public listening=null;
    //对象列表
    private boardList:Map<string,Node>=null;
    private panelList:Map<string,Prefab>=null;

    protected onLoad()
    {
        GameManager._instance=this.node.getComponent(GameManager);
        AudioManager.Instance.Init();
    }

    async start() 
    {
        try
        {
           User.OptionsData=new OptionsData();
           this.boardList=new Map<string,Node>();
           this.panelList=new Map<string,Prefab>();
        }
        catch(error)
        {
            console.error("GameManager 下的 start 错误 error: ",error);
        }
    }

    //初始化
    public async Init()
    {
        try
        {
            let tf = BundleManager.Instance.loadAssetsFromBundle("Typeface", "MAOKENASSORTEDSANS");
            let wt=BundleManager.Instance.loadAssetsFromBundle("PanelPrefabs","waiting");
            //加载
            let awaitResult = await Promise.all([tf,wt]);
            this.typeface = awaitResult[0] as TTFFont;
            let tpre=awaitResult[1] as Prefab;
            this.waitingPanel=instantiate(tpre);
            this.waitingPanel.setParent(this.node);
            this.waitingPanel.active=false;

            this.InitEvent();
        }
        catch(error)
        {
            console.error("GameManager 下的 init 错误 error: ",error);
        }
    }

    public async getMainPageBoard(str:string):Promise<Node>
    {
        try
        {
            if (this.boardList.has(str))
            {
                console.log(`getMainPageBoard "${str}": 已缓存, 直接返回`);
                return this.boardList.get(str);
            }
            else
            {
                const t0 = Date.now();
                let prefab = await BundleManager.Instance.loadAssetsFromBundle("MainPage", str) as Prefab;
                const t1 = Date.now();
                let board = instantiate(prefab);
                const t2 = Date.now();
                board.setParent(this.node);
                this.boardList.set(board.name, board);
                console.log(`getMainPageBoard "${str}": load=${t1-t0}ms, instantiate=${t2-t1}ms`);
                return board;
            }
        } catch (error)
        {
            console.error("GameManager 下的 getMainPageBoard 错误 error: ",error);
        }
    }

    /**
     * 查找或加载二级面板
     * @param str 面板名
     * @returns 实例化的面板
     */
    public async getBoard(str:string):Promise<Node>
    {
        try
        {
            if (this.boardList.has(str))
            {
                console.log(`getBoard "${str}": 已缓存, 直接返回`);
                return this.boardList.get(str);
            }
            else
            {
                const t0 = Date.now();
                let prefab = await BundleManager.Instance.loadAssetsFromBundle("BoardPrefabs", str) as Prefab;
                const t1 = Date.now();
                let board = instantiate(prefab);
                const t2 = Date.now();
                board.setParent(this.node);
                this.boardList.set(board.name, board);
                console.log(`getBoard "${str}": load=${t1-t0}ms, instantiate=${t2-t1}ms`);
                return board;
            }
        } catch (error)
        {
            console.error("GameManager 下的 getBoard 错误 error: ",error);
        }
    }

    /**
     * 移除并销毁指定二级面板，释放内存
     * @param str 面板名
     */
    public RemoveBoard(str:string)
    {
        try
        {
            let board = this.boardList.get(str);
            if (board)
            {
                board.destroy();
                this.boardList.delete(str);
            }
        } catch (error)
        {
            console.error("GameManager 下的 RemoveBoard 错误 error: ",error);
        }
    }

    /**
     * 移除并销毁所有二级面板
     */
    public RemoveBoards()
    {
        try
        {
            for (let t of this.boardList.values()) 
            {
                t.destroy();
            }
            this.boardList.clear();
        } catch (error)
        {
            console.error("GameManager 下的 removeBoards 错误 error: ",error);
        }
    }

    public async getMainPagePanel(str:string)
    {
        try
        {
            if (this.panelList.has(str))
            {
                let panel = this.panelList.get(str);
                //panel.active = true;
                return panel;
            }
            else
            {
                let prefab = await BundleManager.Instance.loadAssetsFromBundle("MainPage", str) as Prefab;
                //let panel = instantiate(prefab);
                //panel.setParent(this.node);
                this.panelList.set(str, prefab);
                return prefab;
            }
        } catch (error) {
            console.error("GameManager 下的 getPanel 错误 error: ",error);
        } finally {
        }
    }

    /**
     * 查找或加载界面
     * @param str 界面名
     * @returns 实例化的界面
     */
    public async getPanel(str:string)
    {
        try
        {
            if (this.panelList.has(str))
            {
                let panel = this.panelList.get(str);
                //panel.active = true;
                return panel;
            }
            else
            {
                let prefab = await BundleManager.Instance.loadAssetsFromBundle("PanelPrefabs", str) as Prefab;
                //let panel = instantiate(prefab);
                //panel.setParent(this.node);
                this.panelList.set(str, prefab);
                return prefab;
            }
        } catch (error) {
            console.error("GameManager 下的 getPanel 错误 error: ",error);
        } finally {
        }
    }

    //消息监听
    private InitEvent()
    {
        //打开角色卡信息
        /* 消息来源
         * RoleCard.ts : 第 78 行 
         * 
         * 
         * 
         * 
         * 
         */
        this.node.on(enums.SendMseeageType.OpenCardInfo,async (event:SendMessage)=>
        {
            event.propagationStopped=true;
            let board= await this.getBoard("InformationBoard");
            board?.getComponent(InfoBoard).OpenCardInfo(event.detail.id);
        },this);

        //打开羁绊信息
        /* 消息来源
         * ReadyDis.ts : 第 334 行 
         * 
         * 
         * 
         * 
         * 
         */
        this.node.on(enums.SendMseeageType.OpenFetterInfo,async (event:SendMessage)=>
        {
            event.propagationStopped=true;
            let board = await this.getBoard("InformationBoard");
            board?.getComponent(InfoBoard).OpenFetterInfo(event.detail.id,event.detail.spritePath,event.detail.level);
        },this);

        //打开角色详细信息
        /* 消息来源
         * RoleIcon.ts : 第 453 行 
         * RoleDis.ts : 第 157 行
         * PropIcon.ts
         * 
         * 
         * 
         */
        this.node.on(enums.SendMseeageType.OpenInfoBoard,async (event:SendMessage)=>
        {
            event.propagationStopped=true;
            let board = await this.getBoard("InformationBoard");
            board?.getComponent(InfoBoard).OpenEntityInfo(event.detail.id , event.detail.index , event.detail.role , event.detail.isBuy , event.detail.propType);
        },this);

        //消息提示
        /* 消息来源
         * RoleIcon.ts : 第 215 行 
         * MainInterface.ts : 第 349 行
         * QuestPanel.ts : 第 54 行
         * 
         * 
         * 
         */
        this.node.on(enums.SendMseeageType.ShowTip,(event:SendMessage)=>
        {
            event.propagationStopped=true;
            this.ShowTip(event.detail);
        },this);

        //打开结算面板
        /* 消息来源
         * BattleDis.ts : 第 175、179 行 
         * 
         * 
         * 
         * 
         * 
         */
        this.node.on(enums.SendMseeageType.OpenSettlement,async (event:SendMessage)=>
        {
            event.propagationStopped=true;
            let board = await this.getBoard("SettlementBoard");
            board?.getComponent(Settlement).OpenSettlementBoard(event.detail.outcome, event.detail.GameMode, event.detail.addCoin, event.detail.hpNum, event.detail.isAddTime);
        },this);

        //打开用户信息面板
        /* 消息来源
         *  MainInterface.ts : 第 203 行
         */
        this.node.on(enums.SendMseeageType.OpenUserInfoBoard,async (event:SendMessage)=>
        {
            event.propagationStopped=true;
            let board=await this.getBoard("UserInfoBoard");
            board.getComponent(UserInfo).OpenUserInfoBoard(event.detail);
        },this);

        //打开排行榜
        /* 消息来源
         * MainInterface.ts : 第 221 行
         */
        this.node.on(enums.SendMseeageType.OpenRankListBoard,async (event:SendMessage)=>
        {
            event.propagationStopped=true;
            console.log("OpenRankListBoard event received");
            const t0 = Date.now();
            let board = await this.getMainPageBoard("RankListBoard");
            const t1 = Date.now();
            board?.getComponent(RankList).OpenRankListBoard(event.detail);
            console.log(`OpenRankListBoard total: getBoard=${t1-t0}ms`);
        },this);

        //打开提示弹窗
        /* 消息来源
         * MainInterface.ts : 第 460 行
         * CardEditor.ts : 第 43 行
         */
        this.node.on(enums.SendMseeageType.OpenPopUps,(event:SendMessage)=>
        {
            event.propagationStopped=true;
            this.OpenPopUps(event.detail.type , event.detail.title , event.detail.subheading , event.detail.items , event.callBack)
        },this);

        //打开事件选择面板
        /** 消息来源
         * ReadyDis.ts ：第129行
         */
        this.node.on(enums.SendMseeageType.OpenChooseTag,async (event:SendMessage)=>
        {
            console.log("on message OpenChooseTag");
            event.propagationStopped=true;
            let board = await this.getBoard("ChooseTagBoard");
            await sleep(100);
            console.log("ChooseTags:",event.detail.events);
            board.getComponent(ChooseTag).Open(event.detail.events);
        },this);

        /**打开关卡信息面板 消息来源：
         * VenturePanel.ts ：第 46 行
         */
        this.node.on(enums.SendMseeageType.OpenLevelInfo ,async (event:SendMessage)=>
        {
            event.propagationStopped = true;
            let board = await this.getBoard("LevelInfoBoard");
            board.getComponent(LevelInfo).Open(event.detail.levelId, event.callBack);
        })
    }

    //显示等待
    public Waitting(_flag:boolean)
    {
        if (this.waitingPanel) {
            this.waitingPanel.active=_flag;
            this.waitingPanel.setSiblingIndex(999);
        }
    }

    //显示提示信息
    private async ShowTip(_msg:string)
    {
        try
        {
            let tt = await BundleManager.Instance.loadAssetsFromBundle("TextTipPrefabs", "TextTipBar") as Prefab;
            let tip=instantiate(tt);
            console.log("获取richtext");
            tip.getChildByPath("RichText").getComponent(RichText).string=_msg;
            tip.getChildByPath("RichText").getComponent(RichText).font = this.typeface;
            tip.setParent(this.node);
            console.log("获取anim");
            let anim=tip.getComponent(Animation);
    
            anim.on(Animation.EventType.FINISHED,(event)=>
            {
                tip.destroy();
            });
    
            anim.play();
        }
        catch(error)
        {
            console.error("GameManager 下的 ShowTip 错误 error: ",error);
        }
    }

    //显示弹窗
    private async OpenPopUps(_type:enums.PopUpsType ,_title:string , _subheading:string , _items:Map<string,number> ,  _callBack?:(e?:boolean)=>void)
    {
        console.log("弹窗类型：",_type);
        let board=await this.getBoard("PopUpsBoard");
        board.getComponent(PopUps).title=_title;
        board.getComponent(PopUps).subheading=_subheading;
        board.getComponent(PopUps).Open(_type , _items , _callBack);
    }

    //获取文本
    public GetText(key:string):string
    {
        console.log("GetText config.LanguageConfig:", config.LanguageConfig)
        console.log("GetText key:", key);
        let temp=null;
        switch(User.OptionsData.language)
        {
            case enums.Language.Chinese:
                temp = config.LanguageConfig.get(key).chinese;
                break;
            default:
                temp = config.LanguageConfig.get(key).chinese;
                break;
        }
        if(temp)
        {
            return temp;
        }
        return "null";
    }
}
    
export function ReadConfig(_biomes: enums.Biomes)
{
    let i=100001;
    let cardCfgs = [];
    do
    {
        let rc = config.RoleConfig.get(i);
        if(rc!=null)
        {
            i++;
            if(_biomes == rc.Biomes)
            {
                cardCfgs.push(rc);
            }
        }
        else {
            return cardCfgs;
        }

    } while(true)
}
