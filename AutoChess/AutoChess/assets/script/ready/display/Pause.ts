import { _decorator, Animation, Button, Component, Node, Prefab, instantiate } from 'cc';
import * as singleton from '../../netDriver/netSingleton';
import { login } from '../../login/login';
import * as enmus from '../../other/enums';
import { AudioManager } from '../../other/AudioManager';
import { QuestPanel } from "../../panel/QuestPanel";
import { BundleManager } from '../../bundle/BundleManager';
import { ReadyDis } from './ReadyDis';
const { ccclass, property } = _decorator;

@ccclass('Pause')
export class Pause extends Component 
{
    private board:Node;

    protected onLoad(): void
    {
        this.board=this.node.getChildByPath("Board");
    }

    start() 
    {
        this.board.getChildByPath("Exit_Btn").on(Button.EventType.CLICK, async () =>
        {
            AudioManager.Instance.PlayerOnShot("Sound/sound_click_wooden_01");
            if (enmus.GameMode.PVE == singleton.netSingleton.ready.readyData.gameMode)
            {
                await singleton.netSingleton.ready.parent.getComponent(login).BackMainInterface("VenturePanel");
            }
            else
            {
                await singleton.netSingleton.ready.parent.getComponent(login).BackMainInterface();
            }
            this.node.active = false;
            if (singleton.netSingleton.ready) {
                singleton.netSingleton.ready.destory();
                singleton.netSingleton.ready = null;
            }
        });
        this.board.getChildByPath("Back_Btn").on(Button.EventType.CLICK, () =>
        {
            this.board.getComponent(Animation)?.on(Animation.EventType.FINISHED, () =>
            {
                singleton.netSingleton.ready.activity=true;
                AudioManager.Instance.PlayerOnShot("Sound/sound_click_wooden_01");
                this.board.active = false;
                this.node.active = false;
                this.board.getComponent(Animation)?.off(Animation.EventType.FINISHED);
            })
            this.board.getComponent(Animation)?.play("PanelMid2Bottom");
        });
    }

    protected onDestroy(): void
    {
    }

    public Open()
    {    
        this.node.setSiblingIndex(110);
        this.board.active=true;
        this.board.getComponent(Animation).play("PanelTop2Mid");
    }
}


