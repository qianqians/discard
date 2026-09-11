import { _decorator, Button, Component, Node } from 'cc';
import { AudioManager } from '../other/AudioManager';
import * as singleton from '../netDriver/netSingleton';
import { SendMessage } from '../other/MessageEvent';
import * as enums from '../other/enums';
import { User } from '../login/User';
import { em_error } from '../serverSDK/error';
import { login } from '../login/login';
import { MainInterface } from '../mainInterface/MainInterface';
const { ccclass, property } = _decorator;

@ccclass('QuestPanel')
export class QuestPanel extends Component 
{
    //退出返回按钮
    private exitBtn:Node;

    private level:Node;

    protected onLoad(): void
    {
        this.exitBtn=this.node.getChildByPath("Exit_Btn");
    }

    start() 
    {
        console.log("QuestPanel start!");

        this.exitBtn.on(Button.EventType.CLICK, async ()=>
        {
            AudioManager.Instance.PlayerOnShot("Sound/sound_click_close_01");
            this.node.active=false;
            this.Exit();
            /*if (!singleton.netSingleton.mainInterface) {
                return;
            }*/
            if (singleton.netSingleton.login){
                await singleton.netSingleton.login.BackMainInterface();
            }
        });

        console.log("QuestPanel end!");
    }

    protected onDestroy(): void
    {
        this.destroy();
    }

    public Exit()
    {
        console.log("QuestPanel Exit!");
        this.node.active=false;
        this.destroy();
        console.log("QuestPanel Exit end!");
    }

    public OnLevelBtnClick(_event, _data)
    {
        let levelId = parseInt(_data);
        console.log("levelId:", levelId);
        console.log("User.UserData.quest:", User.UserData.quest);
        if(User.UserData.quest == levelId)
        {
            singleton.netSingleton.game.start_quest_battle_ready().then(async ()=>
            {
                this.Exit();
            },(err)=>
            {
                if(err == em_error.last_quest)
                {
                    this.node.dispatchEvent(new SendMessage(enums.SendMseeageType.ShowTip, true,"<outline color=black width=4>后续关卡待开放</outline>"));
                }
            });
        }
        else
        {
            singleton.netSingleton.game.start_quest_battle_ready_repeat(levelId).then(async ()=>
            {
                this.Exit();
            },(err)=>
            {
                if(err == em_error.last_quest)
                {
                    this.node.dispatchEvent(new SendMessage(enums.SendMseeageType.ShowTip, true,"<outline color=black width=4>后续关卡待开放</outline>"));
                }
            });
        }
    }

    Open()
    {
        console.log("QuestPanel Open!");

        this.level=this.node.getChildByPath("WorldMap/view/content/Map/Continent/Level");
        
        for(let i=0;i<this.level.children.length;i++)
        {
            let lock=true;
            if(((User.UserData.quest%10000)-1) < i)
            {
                lock=false;
            }
            this.level.children[i].getComponent(Button).interactable=lock;
        }

        console.log("QuestPanel Open end!");
    }
}


