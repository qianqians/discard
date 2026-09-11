import { _decorator, Animation, Button, Component, Label, Node, RichText, Sprite } from 'cc';
import * as singleton from '../netDriver/netSingleton';
import { config } from '../battle/AutoChessBattle/config/config';
import { loadAssets } from '../bundle/LoadAsset';
import { GameManager } from '../other/GameManager';
import { QuestRogueRole } from '../serverSDK/ccallplayer';
const { ccclass, property } = _decorator;

@ccclass('ChooseTag')
export class ChooseTag extends Component 
{   
    private board:Node;

    private tag_1:Node;
    private tag_2:Node;
    private tag_3:Node;

    private tags:QuestRogueRole[]=[];

    private tagnodes=[];

    protected onLoad(): void
    {
        this.board=this.node.getChildByPath("Board");

        this.tag_1=this.node.getChildByPath("Board/Layout/Tag_1");
        this.tag_2=this.node.getChildByPath("Board/Layout/Tag_2");
        this.tag_3=this.node.getChildByPath("Board/Layout/Tag_3");
    }

    start() 
    {
        this.board.active=false;
        this.tagnodes=[this.tag_1,this.tag_2,this.tag_3];
    }

    Init()
    {
        this.tag_1.on(Button.EventType.CLICK, () =>
        {
            console.log(" On ChooseTag: RoleID:",this.tags[0].r.RoleID);
            singleton.netSingleton.ready.readyData.ChooseTag(this.tags[0].index);
            this.Exit();
        }, this);
        this.tag_2.on(Button.EventType.CLICK, () =>
        {
            console.log(" On ChooseTag: RoleID:",this.tags[1].r.RoleID);
            singleton.netSingleton.ready.readyData.ChooseTag(this.tags[1].index);
            this.Exit();
        }, this);
        this.tag_3.on(Button.EventType.CLICK, () =>
        {
            console.log(" On ChooseTag: RoleID:",this.tags[2].r.RoleID);
            singleton.netSingleton.ready.readyData.ChooseTag(this.tags[2].index);
            this.Exit();
        }, this);
    }

    Open(_events:QuestRogueRole[])
    {   
        this.board.active=true;
        console.log("open ChooseTag")
        this.node.setSiblingIndex(100);

        this.board.getComponent(Animation).play("PanelAppear");
        
        this.SetTags(_events);
        this.Init();
    }

    async SetTags(_events:QuestRogueRole[])
    {
        
        for(let i=0;i<_events.length;i++)
        {
            let r = _events[i].r;
            this.tags.push(_events[i]);

            let rolec = config.RoleConfig.get(r.RoleID);
            this.tagnodes[i].getChildByPath("RichText").getComponent(RichText).string = "<color=#000000>" + GameManager.Instance.GetText(rolec.Name) + "</color>";

            this.tagnodes[i].getChildByPath("Avatar/Hp/RichText").
                getComponent(RichText).string = "<color=#9d0c27><outline color=#e93552 width=4>" + r.HP + "</outline></color>";
            this.tagnodes[i].getChildByPath("Avatar/Atk/RichText").
                getComponent(RichText).string = "<color=#f99b08><outline color=#fff457 width=4>" + r.Attack + "</outline></color>";
            this.tagnodes[i].getChildByPath("Avatar/Lvl/RichText").
                getComponent(RichText).string = "<color=1d994f><outline color=74eda5 width=4>"+(r.Level?r.Level:1)+"</outline></color>";


            let skcfg = config.SkillIntroduceConfig.get(rolec.SkillID);
            let skillInfo = "";
            switch (r.Level)
            {
                case 1: skillInfo = GameManager.Instance.GetText(skcfg.Leve1Text); break;
                case 2: skillInfo = GameManager.Instance.GetText(skcfg.Leve2Text); break;
                case 3: skillInfo = GameManager.Instance.GetText(skcfg.Leve3Text); break;
                default:skillInfo = GameManager.Instance.GetText(skcfg.Leve1Text);
            }
            this.tagnodes[i].getChildByPath("RichTextInfo").getComponent(RichText).string = "<color=#9d0c27>" + GameManager.Instance.GetText(skcfg.Timeing_Text) + ":\n" + skillInfo + "</color>";

            let img = await loadAssets.LoadImg(rolec.Avatar);
            if (img)
            {
                this.tagnodes[i].getChildByPath("Avatar/Mask/Sprite").getComponent(Sprite).spriteFrame = img;
            }
            
            let propc=config.EquipConfig.get(r.equipID);
            if (propc) {
                img =await loadAssets.LoadImg(propc.Res);
                if(img)
                {
                    this.tagnodes[i].getChildByPath("Avatar/Equip/Sprite").getComponent(Sprite).spriteFrame = img;
                }
                else
                {
                    this.tagnodes[i].getChildByPath("Avatar/Equip/Sprite").getComponent(Sprite).spriteFrame=null;
                }
            }      
        }
    }

    Exit()
    {
        this.board.getComponent(Animation).on(Animation.EventType.FINISHED,()=>
        {
            // 一次性面板，关闭后销毁节点释放内存
            GameManager.Instance.RemoveBoard("ChooseTagBoard");
        })
        this.board.getComponent(Animation).play("PanelDisappear");
    }
}


