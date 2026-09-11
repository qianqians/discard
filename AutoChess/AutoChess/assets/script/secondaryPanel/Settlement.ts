/*
 * 新建
 * Settlement.ts 回合结算界面
 * author: hotaru
 * 2024/03/30
 */
import { _decorator, Animation, BlockInputEvents, Button, Component, Node, RichText, sp, Sprite, SpriteFrame } from 'cc';
import { BattleVictory } from '../serverSDK/common';
import { netSingleton } from '../netDriver/netSingleton';
import { AudioManager } from '../other/AudioManager';
import * as enums from '../other/enums';
import { GameManager } from '../other/GameManager';
import { SendMessage } from '../other/MessageEvent';
const { ccclass, property } = _decorator;

@ccclass('Settlement')
export class Settlement extends Component 
{
    @property(SpriteFrame)
    public winBanners:SpriteFrame;
    @property(SpriteFrame)
    public loseBanners:SpriteFrame;
    @property(SpriteFrame)
    public drwaBanners:SpriteFrame;
    //退出按钮
    private exitBtn:Button;
    //显示区域
    private midArea:Node;
    //生命
    private hpGroup:Node;
    //奖杯数
    private cupNum:Node;
    //胜负图标
    private outCome:Node;
    //胜负横幅
    private banners:Node;

    private addTimeBoard:Node;

    private isVictory:BattleVictory;
    private GameMode:enums.GameMode;
    private isAddTime:boolean;
    private addCoin:number;

    protected onLoad(): void 
    {
        this.exitBtn=this.node.getChildByPath("BG").getComponent(Button);
        this.midArea=this.node.getChildByPath("MidArea");
        this.hpGroup=this.node.getChildByPath("MidArea/Hp");
        this.cupNum=this.node.getChildByPath("MidArea/CupNum");
        this.outCome=this.node.getChildByPath("MidArea/Outcome");
        this.banners=this.node.getChildByPath("MidArea/Banners");
        this.Init();
    }

    start() 
    {
        this.exitBtn.node.on(Button.EventType.CLICK,()=>
        {
            console.log("exitboard!");
            this.Exit();
        });
        this.addTimeBoard.active=false;
    }
    
    private Init()
    {
        try
        {
            for (let t of this.hpGroup.children)
            {
                t.getComponent(sp.Skeleton).setAnimation(0, "xin2", true);
            }
        }
        catch (error) 
        {
            console.error("Settlement 下的 Init 错误：", error);
        }
    }

    public OpenSettlementBoard(_isVictory:BattleVictory, GameMode:enums.GameMode, addCoin:number, _hpNum:number,_isAddTime:boolean=false)
    {
        try
        {
            console.log("show settlement");
            //this.node.getComponent(BlockInputEvents).enabled=true;
            this.node.active = true;  // 确保节点激活
            this.node.setSiblingIndex(100);
            this.isVictory = _isVictory;
            this.GameMode = GameMode;
            this.isAddTime = _isAddTime;
            this.addCoin = addCoin;
            this.midArea.active = true;
            console.log(_hpNum);
            for (let i = 0; i < _hpNum; i++)
            {
                this.hpGroup.children[i].getComponent(sp.Skeleton).setAnimation(0, "xin1", true);
            }
            this.midArea.getComponent(Animation).play("PanelAppear");
            switch (_isVictory)
            {
                case BattleVictory.faild:
                    AudioManager.Instance.PlayerOnShot("Sound/battle_failed_01");
                    this.banners.getChildByPath("RichText").getComponent(RichText).string = "<color=#ffffff><outline color=#245998 width=20>失<size=30></size>败</color>";
                    this.cupNum.getChildByPath("RichText").getComponent(RichText).string = "<color=#ffffff>+0</color>"
                    this.banners.getComponent(Sprite).spriteFrame = this.loseBanners;
                    this.outCome.getChildByPath("Spine").getComponent(sp.Skeleton).setAnimation(0, "loss", true);
                    break;
                case BattleVictory.tie:
                    AudioManager.Instance.PlayerOnShot("Sound/battle_draw");
                    this.banners.getChildByPath("RichText").getComponent(RichText).string = "<color=#ffffff><outline color=#2b7c41 width=20>平<size=30></size>局</color>";
                    this.cupNum.getChildByPath("RichText").getComponent(RichText).string = "<color=#ffffff>+0</color>"
                    this.banners.getComponent(Sprite).spriteFrame = this.drwaBanners;
                    this.outCome.getChildByPath("Spine").getComponent(sp.Skeleton).setAnimation(0, "draw", true);
                    break;
                case BattleVictory.victory:
                    AudioManager.Instance.PlayerOnShot("Sound/battle_win_01");
                    this.banners.getChildByPath("RichText").getComponent(RichText).string = "<color=#ffffff><outline color=#f4b428 width=20>胜<size=30></size>利</color>";
                    this.banners.getComponent(Sprite).spriteFrame = this.winBanners;
                    this.outCome.getChildByPath("Spine").getComponent(sp.Skeleton).setAnimation(0, "win", true);
                    break;
            }
            this.midArea.getComponent(Animation).on(Animation.EventType.FINISHED, () =>
            {
                if(_hpNum<5 && BattleVictory.faild==this.isVictory)
                {
                    this.hpGroup.children[_hpNum].getComponent(sp.Skeleton).setAnimation(0, "animation", false);
                    this.hpGroup.children[_hpNum].getComponent(sp.Skeleton).setCompleteListener((trackEntry) =>
                    {
                        if (trackEntry.animation.name === "animation")
                        {
                            this.hpGroup.children[_hpNum].getComponent(sp.Skeleton).setAnimation(0, "xin2", true);
                        }
                    });
                }
            });
        }
        catch (error) 
        {
            console.error("Settlement 下的 OpenSettlementBoard 错误：",error);
        }
    }

    public async Exit()
    {
        this.node.getComponent(BlockInputEvents).enabled=false;
        // 提前捕获网络操作所需数据，避免销毁后无法访问
        const gameMode = this.GameMode;
        const isVictory = this.isVictory;
        const addCoin = this.addCoin;
        this.midArea.getComponent(Animation).on(Animation.EventType.FINISHED, async () =>
        {
            this.node.active=false;
            this.midArea.getComponent(Animation).off(Animation.EventType.FINISHED);
            //返回准备界面代码写这
            if(enums.GameMode.PVP == gameMode)
            {
                const success = await netSingleton.game.confirm_match_round_victory(isVictory, addCoin);
                if (!success) {
                    console.error("Settlement Exit confirm_match_round_victory failed!");
                    this.node.active = true;
                    this.node.getComponent(BlockInputEvents).enabled = true;
                    this.node.dispatchEvent(new SendMessage("ShowTip", true, "<outline color=black width=4>网络连接失败,请重试</outline>"));
                    return;
                }
            }
            else
            {
                await netSingleton.game.confirm_quest_victory(isVictory, addCoin);
            }
            // 网络操作成功后销毁节点释放内存
            GameManager.Instance.RemoveBoard("SettlementBoard");
        });
        this.midArea.getComponent(Animation).play("PanelDisappear");
    }
}


