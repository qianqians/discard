import { _decorator, animation, Animation, Button, Color, Component, Node, Sprite, SpriteFrame } from 'cc';
import * as singleton from '../netDriver/netSingleton';
import { GameDifficulty } from '../other/enums';
import { AudioManager } from '../other/AudioManager';
import { GameManager } from '../other/GameManager';
import { User } from "../login/User";
import * as common from "../serverSDK/common"
import { BundleManager } from "../bundle/BundleManager";
import { GuideMask, HighlightType } from "../guide/guideMask";
import { sleep } from '../other/sleep';
import { MainInterface } from './MainInterface';
const { ccclass, property } = _decorator;

@ccclass('StartGame')
export class StartGame extends Component 
{
    //竞技模式
    private athleticsWindow:Node;
    private matchBtn: Node;
    private guideMask: GuideMask;
    private isOpen:boolean = false;

    protected onLoad(): void 
    {
        try
        {
            console.log("StartGamePanel onLoad");
        }
        catch(error)
        {
            console.error('StartGamePanel 下 onLoad 错误 err: ',error);
        }
    }

    start() 
    {
        try
        {
            console.log("StartGamePanel start");
        }
        catch(error)
        {
            console.error('StartGamePanel 下 start 错误 err: ',error);
        }
    }

    public ActiveGuideMask(active:boolean) {
        let mask = this.node.getChildByPath("Mask");
        mask.active = active;
        mask.getChildByPath("DarkLayer").active = active;
      }

    protected onDestroy(): void
    {
        this.destroy();
    }

    private Init() {
        if (!this.athleticsWindow) {
        }
        if (!this.matchBtn) {
        }
        if (!this.guideMask) {
        }
        this.athleticsWindow=this.node.getChildByPath("AthleticsWindow");
        this.matchBtn = this.athleticsWindow.getChildByPath("Button");
        this.guideMask = this.node.getComponent(GuideMask);

        //匹配
        if (!this.matchBtn.hasEventListener(Button.EventType.CLICK)) {
            this.matchBtn.on(Button.EventType.CLICK, async ()=>
            {
                this.node.active = false;

                AudioManager.Instance.PlayerOnShot("Sound/sound_click_wooden_01");
                singleton.netSingleton.game.start_match_battle_ready(common.BattleMod.Battle);
                
                if (!User.UserData?.GuideSteps.includes(common.GuideStep.EnterGameGuide)) {
                    User.UserData?.GuideSteps.push(common.GuideStep.EnterGameGuide);
                    singleton.netSingleton.player.guide_step_ntf(common.GuideStep.EnterGameGuide);
                }

                await sleep(100);
            }, this);
        }
    }

    //竞技模式窗口
    public async OpenAthleticsWindow()
    {
        try
        {
            if (this.isOpen) {
                return;
            }
            this.isOpen = true;

            this.Init();
            this.athleticsWindow=this.node.getChildByPath("AthleticsWindow")
            this.athleticsWindow.active = true;
            this.athleticsWindow.getComponent(Animation).play("PanelAppear");
            
            if (!User.UserData?.GuideSteps.includes(common.GuideStep.EnterGameGuide)) {
                await sleep(300);
                this.ActiveGuideMask(true);
                await sleep(300);

                let alpha = 127;
                let step = 4;
                if (this.matchBtn?.getComponent(Sprite) != null) {
                    console.log("set matchBtn color", this.matchBtn);
                    this.matchBtn.getComponent(Sprite).color = new Color(alpha, alpha, alpha, 255);
                }
                this.guideMask?.highlightNode([[this.matchBtn, HighlightType.CIRCLE]], () => {
                    if (alpha >= 255) {
                        step = -4;
                    } else if (alpha <= 127) {
                        step = 4;
                    }
                    alpha = Math.max(127, Math.min(255, alpha + step));
                    console.log("highlightNode set matchBtn color", this.matchBtn);
                    if (this.matchBtn?.getComponent(Sprite) != null) {
                        this.matchBtn.getComponent(Sprite).color = new Color(alpha, alpha, alpha, 255);
                    }
                });
                this.guideMask?.Redraw();
            }
        }
        catch(error)
        {
            console.error('StartGamePanel 下 OpenAthleticsWindow 错误 err: ',error);
        }
    }
    
    public ClosePanel()
    {
        try
        {
            if (!this.isOpen) {
                return;
            }
            this.isOpen = false;

            AudioManager.Instance.PlayerOnShot("Sound/sound_home_return_feedback_01");

            this.ActiveGuideMask(false);
            this.guideMask.StopHighlight();
        
            this.matchBtn.off(Button.EventType.CLICK);
            if(this.athleticsWindow.active)
            {
                this.athleticsWindow.getComponent(Animation).on(Animation.EventType.FINISHED,()=>
                {
                    this.node.active=false;
                    this.athleticsWindow.getComponent(Animation).off(Animation.EventType.FINISHED);
                });
                this.athleticsWindow.getComponent(Animation).play("PanelDisappear");
            }

            if (!singleton.netSingleton.mainInterface) {
                return;
            }
            singleton.netSingleton.mainInterface.activity=true;
        }
        catch(error)
        {
            console.error('StartGamePanel 下 ClosePanel 错误 err: ',error);
        }
    }
}


