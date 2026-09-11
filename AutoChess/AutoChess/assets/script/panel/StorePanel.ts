/*
 * 修改 StorePanel
 * author：Hotaru
 * 2024/03/18
 * 商店界面
 */
import { _decorator, Button, Component, instantiate, Node, PageView, Prefab, primitives, RichText, Sprite, sys, Toggle } from 'cc';
import * as singleton from '../netDriver/netSingleton';
import { BundleManager } from '../bundle/BundleManager';
import { config } from '../battle/AutoChessBattle/config/config';
import { loadAssets } from '../bundle/LoadAsset';
import { CardPacket } from '../serverSDK/ccallplayer';
import { StorePrompt } from '../secondaryPanel/StorePrompt';
import { InfoBoard } from '../secondaryPanel/InfoBoard';
import { SendMessage } from '../other/MessageEvent';
import { AudioManager } from '../other/AudioManager';
import SdkManager from '../SDK/SdkManager';
import { MainInterface } from '../mainInterface/MainInterface';
const { ccclass, property } = _decorator;

@ccclass('StorePanel')
export class StorePanel extends Component 
{
    private backBtn:Node;
    private pageView:PageView;
    //界面组件预制体
    private storePagePre:Prefab;
    private cardListPre:Prefab;
    private rechargePre:Prefab;
    private roleCardPre:Prefab;
    //分页组件
    private storePage:Node;
    private cardListPage:Node;
    private rechargePage:Node;
    //页签组
    public toggleGroup:Node;
    //二级界面
    //public storePrompt:Node;
    //卡牌列表
    private cards:Node[]=[];

    onLoad()
    {
        try
        {
            this.backBtn=this.node.getChildByPath("Back_Btn");
            this.pageView=this.node.getChildByPath("StoreArea/PageView").getComponent(PageView);
            this.toggleGroup=this.node.getChildByPath("ToggleGroup");
        }
        catch(error)
        {
            console.error("StorePanel 下的 onLoad 错误：",error);
        }

        //二级信息界面
        // this.infoPanel=instantiate(Informationpanel);
        // this.infoPanel.setParent(this.node);
        // this.infoPanel.active=false;
        //商店购买提示框
        // this.storePrompt=instantiate(StorePromptPanelpanel);
        // this.storePrompt.setParent(this.node);
        // this.storePrompt.active=false;

    }

    start() 
    {
        try
        {
            this.backBtn.on(Button.EventType.CLICK,()=>
            {
                AudioManager.Instance.PlayerOnShot("Sound/sound_click_close_01");
                if (singleton.netSingleton.mainInterface) {
                    singleton.netSingleton.mainInterface.panelNode.active=true;
                }
                this.Exit();
            },this);    
        }
        catch(error)
        {
            console.error("StorePanel 下的 start 错误：",error);
        }
    }

    protected onDestroy(): void
    {
        this.destroy();
    }

    public Exit()
    {
        this.ClearPageView();
        this.node.destroy();
    }

    public async CheckStoreToggle(_fromBtn?:boolean)
    {
        try
        {
            AudioManager.Instance.PlayerOnShot("Sound/sound_bookmark_select_01");
            console.log(this.toggleGroup);
            if(!this.toggleGroup.getChildByPath("Store").getComponent(Toggle).isChecked || _fromBtn==true?true:false)
            {
                console.log("CheckStoreToggle!!!");
                this.node.getChildByPath("StoreArea/PageView").getComponent(PageView).removeAllPages();
                this.ClearPageView();
                if (!this.storePagePre) 
                {
                    this.storePagePre = await BundleManager.Instance.loadAssetsFromBundle("PartPrefabs", "StorePage") as Prefab;
                }
                this.storePage = instantiate(this.storePagePre);
                this.pageView.addPage(this.storePage);

                this.InitStore();
            }
            
        }
        catch(error)
        {
            console.error('StorePanel 下 CheckStoreToggle 错误 err: ',error);
        }
        
    }

    public async CheckCardListToggle()
    {
        try
        {
            AudioManager.Instance.PlayerOnShot("Sound/sound_bookmark_select_01");
            if(!this.toggleGroup.getChildByPath("CardList").getComponent(Toggle).isChecked)
            {
                console.log("CheckCardListToggle!!!");
                this.node.getChildByPath("StoreArea/PageView").getComponent(PageView).removeAllPages();
                this.ClearPageView();
                if (!this.cardListPre) {
                    this.cardListPre = await BundleManager.Instance.loadAssetsFromBundle("PartPrefabs", "CardPage") as Prefab;
                }
                this.cardListPage = instantiate(this.cardListPre);
                this.pageView.addPage(this.cardListPage);
                //this.LoadCard();
            }
           
        }
        catch(error)
        {
            console.error('StorePanel 下 CheckCardListToggle 错误 err: ',error);
        }
    }

    public async CheckRechargeToggle()
    {
        try
        {
            let sysinfo=SdkManager.SDK.getSystemInfo().platform;
            
            if(sysinfo === "ios" || sysinfo === "mac")
            {
                this.ClearPageView();
                return;
            }
            AudioManager.Instance.PlayerOnShot("Sound/sound_bookmark_select_01");
            if(!this.toggleGroup.getChildByPath("Recharge").getComponent(Toggle).isChecked)
            {
                console.log("CheckRechargeToggle!!!");
                this.node.getChildByPath("StoreArea/PageView").getComponent(PageView).removeAllPages();
                this.ClearPageView();
                if (!this.rechargePre) {
                    this.rechargePre = await BundleManager.Instance.loadAssetsFromBundle("PartPrefabs", "RechargePage") as Prefab;
                }
                this.rechargePage = instantiate(this.rechargePre);
                this.pageView.addPage(this.rechargePage);
            }
            
        }
        catch(error)
        {
            console.error('StorePanel 下 CheckRechargeToggle 错误 err: ',error);
        }
        
    }

    private ClearPageView()
    {
        for (let node of this.pageView.node.getChildByPath("view/content").children) 
        {
            node.destroy();
        }
    }

    private InitStore()
    {
        //购买卡包
        this.storePage.getChildByPath("Commodity").on(Button.EventType.CLICK,()=>
        {
            singleton.netSingleton.player.buy_card_packet();
        },this);
    }

    public ShowCardPacketContent(_cardPacketInfo:CardPacket)
    {
        //this.storePrompt.getComponent(StorePrompt).ShowPacketItem(_cardPacketInfo);
    }

}


