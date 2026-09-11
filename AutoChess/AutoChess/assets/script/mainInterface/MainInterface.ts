import {
  _decorator,
  Animation,
  TTFFont,
  assetManager,
  BlockInputEvents,
  Button,
  Component,
  ImageAsset,
  instantiate,
  Node,
  Prefab,
  RichText,
  screen,
  Sprite,
  SpriteFrame,
  sys,
  Texture2D,
  Toggle,
  tween,
  Vec3,
  view,
  Widget,
  Label,
  Color,
} from "cc";
import * as singleton from "../netDriver/netSingleton";
import { StorePanel } from "../panel/StorePanel";
import * as common from "../serverSDK/common";
import { CardPacket } from "../serverSDK/ccallplayer";
import { SendMessage } from "../other/MessageEvent";
import { StartGame } from "./StartGame";
import { AudioManager } from "../other/AudioManager";
import * as enums from "../other/enums";
import { CardEditor } from "../panel/CardEditor";
import { GameManager } from "../other/GameManager";
import SdkManager from "../SDK/SdkManager";
import { login } from "../login/login";
import { AchievePanel } from "../panel/AchievePanel";
import { User } from "../login/User";
import { QuestPanel } from "../panel/QuestPanel";
import { config } from "../battle/AutoChessBattle/config/config";
import { CardLibPanel } from "../panel/CardLibPanel";
import { StartHouseRender } from "../part/StartHouseReader";
import { GuideMask, HighlightType } from "../guide/guideMask";
import { sleep } from "../other/sleep";
import { BundleManager } from "../bundle/BundleManager";
const { ccclass, property } = _decorator;

export class MainInterface {
  //父节点
  public parentNode: Node;
  //主体
  public panelNode: Node;
  //主界面
  public mainPanel: Node;
  //开始游戏
  public startGamePart: Node;
  //各区域按钮
  private startBtn: Node; //匹配按钮
  private cardlibraryBtn: Node; //牌库按钮
  private rankListBtn: Node; //排行榜按钮
  private questBtn: Node; //冒险模式按钮（pve）
  //玩家信息
  private userMoney: Node;
  private userDiamonds: Node;
  private avatarUrl: string;
  public userData: common.UserData;
  //玩家头像
  private userAvatar: Node;

  public activity: boolean = false;

  private guideMask: GuideMask;
  private guideText: string[] = [
    "冒险家你好 欢迎来到魔法密林!", 
    "点击屏幕下方的冒险按钮可以开始探索之旅!",
    "点击匹配按钮可以与其他冒险家展开切磋!",
    "开始你的冒险之旅吧!"];
  private guideIndex: number = 0;

  // 头像纹理引用，用于释放旧纹理避免内存泄漏
  private _avatarSpriteFrame: SpriteFrame | null = null;
  private _avatarTexture: Texture2D | null = null;

  private questPanelPrefab: Promise<Prefab>;

  constructor() {
    this.RegCallBack();
  }
  /*
   * 修改start
   * author：Hotaru
   * 2024/03/07
   * 让加载更平顺
   */
  async start(_father: Node, panelPrefab: Promise<Prefab>, questPanelPrefab: Promise<Prefab>, typeface: TTFFont) {
    try {
      //主界面
      this.parentNode = _father;
      this.panelNode = instantiate(await panelPrefab);
      this.questPanelPrefab = questPanelPrefab;

      //各区域面板
      this.mainPanel = this.panelNode.getChildByPath("MainPanel");
      console.log("start mainPanel:", this.mainPanel);
      this.startGamePart = this.panelNode.getChildByPath("StartGamePanel");
      
      //玩家信息
      this.userMoney = this.panelNode.getChildByPath("MainPanel/UiLayer/TopArea/UserMoney");
      this.userDiamonds = this.panelNode.getChildByPath("MainPanel/UiLayer/TopArea/UserDiamonds");
      this.userAvatar = this.panelNode.getChildByPath("MainPanel/UiLayer/TopArea/UserAvatar");
    
      //冒险模式
      this.questBtn = this.panelNode.getChildByPath("MainPanel/UiLayer/Adventure_Button"); 
      this.questBtn.getChildByPath("Label_Adventure").getComponent(Label).font = typeface;
      //匹配
      this.startBtn = this.panelNode.getChildByPath("MainPanel/UiLayer/Match_Button"); 
      this.startBtn.getChildByPath("Label_Match").getComponent(Label).font = typeface;
      //牌库
      this.cardlibraryBtn = this.panelNode.getChildByPath("MainPanel/UiLayer/Card_Button"); 
      this.cardlibraryBtn.getChildByPath("Label_Card").getComponent(Label).font = typeface;
      //排行榜
      this.rankListBtn = this.panelNode.getChildByPath("MainPanel/UiLayer/Rank_Button"); 
      this.rankListBtn.getChildByPath("Label_Rank").getComponent(Label).font = typeface;

      this.guideMask = this.mainPanel.getComponent(GuideMask);
      //屏幕适配
      this.Adaptation();
      //初始化
      this.initGuideText();

      this.parentNode.addChild(singleton.netSingleton.mainInterface.panelNode);

      console.log("MainInterface start success!");

    } catch (error) {
      console.error("MainInterface 下 Start 错误 err: ", error);
    }
  }

  private initGuideText() {
    if (User.UserData?.GuideSteps.includes(common.GuideStep.EnterGameGuide)) {
      return;
    }

    let guide_text = this.mainPanel.getChildByPath("Mask/guide_role/GuideTxt");
    guide_text.getComponent(Label).string = this.guideText[this.guideIndex];
    this.guideIndex++;
  }

  public destory() 
  {
      if (this.panelNode) {
          AudioManager.Instance.Stop();
          this.panelNode.destroy();
          this.panelNode = null;
      }
      // 清理所有缓存的二级面板
      GameManager.Instance.RemoveBoards();
  }

  /**
   * 显示头像
   * @param _url 路径
   */
  public async ShowAvatar(_url: string) {
    try {
      console.log("尝试加载头像：", _url);
      this.avatarUrl = _url;
      console.log("ShowAvatar mainPanel:", this.mainPanel)
      let node = this.mainPanel.getChildByPath("UiLayer/TopArea/UserAvatar/Mask/Sprite")
      console.log("ShowAvatar node:", node)
      let sprite = node.getComponent(Sprite);
      console.log("ShowAvatar sprite:", sprite)
      await assetManager.loadRemote<ImageAsset>(
        _url,
        { ext: ".png" },
        (_err, image) => {
          // 释放旧纹理避免内存泄漏
          if (this._avatarSpriteFrame) this._avatarSpriteFrame.destroy();
          if (this._avatarTexture) this._avatarTexture.destroy();
          let sp = new SpriteFrame();
          let texture = new Texture2D();
          texture.image = image;
          sp.texture = texture;
          this._avatarSpriteFrame = sp;
          this._avatarTexture = texture;
          sprite.spriteFrame = sp;
        },
      );
    } catch (error) {
      console.error("MainInterface 下 ShowAvatar 错误 err: ", error);
    }
  }
  
  /*
   * 添加Adaptation
   * author：Hotaru
   * 2024/06/1
   * 屏幕适配
   */
  private Adaptation() {
    try {
      let safeHeigh =
        SdkManager.SDK.getSystemInfo().screenHeight -
        SdkManager.SDK.getSystemInfo().safeArea.height;
      let menuBtnHeight = SdkManager.SDK.getSystemInfo().menuBtn.bottom;
      let top = safeHeigh > menuBtnHeight ? safeHeigh : menuBtnHeight;
      this.mainPanel
        .getChildByPath("UiLayer/TopArea")
        .getComponent(Widget).top = top + 10;
    } catch (error) {
      console.error("MainInterface 下 Adaptation 错误 err: ", error);
    }
  }

  public ActiveGuideMask(active:boolean) {
    let mask = this.mainPanel.getChildByPath("Mask");
    mask.active = active;
    mask.getChildByPath("DarkLayer").active = false;

    if (!active) {
      this.guideMask.StopHighlight();
    }
  }

  public HideStartGamePart() {
    this.startGamePart.active = false;
  }

  public async MaskMatchBtn() {
    if (!User.UserData?.GuideSteps.includes(common.GuideStep.EnterGameGuide)) {
      this.ActiveGuideMask(true);
      await sleep(200);

      let alpha = 127;
      let step = 4;
      this.startBtn.getComponent(Sprite).color = new Color(alpha, alpha, alpha, 255);
      this.questBtn.getComponent(Sprite).color = new Color(alpha, alpha, alpha, 255);

      this.guideMask.highlightNode([[this.startBtn, HighlightType.RECT], [this.questBtn, HighlightType.RECT]], () => {
        if (alpha >= 255) {
          step = -4;
        } else if (alpha <= 127) {
          step = 4;
        }
        alpha = Math.max(127, Math.min(255, alpha + step));

        if (this.startBtn.getComponent(Sprite) != null) {
          this.startBtn.getComponent(Sprite).color = new Color(alpha, alpha, alpha, 255);
        }
        if (this.questBtn.getComponent(Sprite) != null) {
          this.questBtn.getComponent(Sprite).color = new Color(alpha, alpha, alpha, 255);
        }
      });
      this.guideMask.Redraw();

      console.log("highlightNode", this.startBtn);
    }
  }

  public Init() {
    try {
      this.RegButton();
      
      this.activity = true;
      AudioManager.Instance.PlaySound("Sound/sound_lobby");
      this.HideStartGamePart();

      this.ActiveGuideMask(false);
      this.MaskMatchBtn();

    } catch (error) {
      console.error("MainInterface 下 Init 错误 err: ", error);
    }
  }

  public rewardedVideoAd: WechatMinigame.RewardedVideoAd = null;
  //注册按钮
  private RegButton() {
    try {
      //打开匹配
      this.startBtn?.on(
        Button.EventType.CLICK,
        async () => {
          if (!User.UserData?.GuideSteps.includes(common.GuideStep.EnterGameGuide)) {
            if (!User.UserData) {
              User.UserData = new common.UserData();
            }
            User.UserData?.GuideSteps.push(common.GuideStep.EnterGameGuide);
            singleton.netSingleton.player.guide_step_ntf(common.GuideStep.EnterGameGuide);
          }
          
          AudioManager.Instance.PlayerOnShot("Sound/sound_base_select_01");

          await sleep(200);
          this.ActiveGuideMask(false);

          console.log("startBtn OpenAthleticsWindow!");
          this.startGamePart.active = true;
          await this.startGamePart.getComponent(StartGame).OpenAthleticsWindow();
        }
      );
      
      //打开牌库界面
      this.cardlibraryBtn?.on(
        Button.EventType.CLICK,
        async () => {
          AudioManager.Instance.PlayerOnShot("Sound/sound_base_select_01");
          let panel = instantiate(await GameManager.Instance.getMainPagePanel("CardLibPanel"));
          panel.getComponent(CardLibPanel).OpenCardLib();
          panel.setParent(GameManager.Instance.node);
          
          this.activity = false;
          this.panelNode.active = this.activity;
        },
        this,
      );
      
      //打开冒险模式界面
      this.questBtn?.on(
        Button.EventType.CLICK,
        async () => {
          new Promise<void>(async (resolve, reject) => {
            if (!User.UserData?.GuideSteps.includes(common.GuideStep.EnterGameGuide)) {
              User.UserData?.GuideSteps.push(common.GuideStep.EnterGameGuide);
              singleton.netSingleton.player.guide_step_ntf(common.GuideStep.EnterGameGuide);
            }

            await sleep(200);
            this.ActiveGuideMask(false);
            AudioManager.Instance.PlayerOnShot("Sound/sound_base_select_01");

            GameManager.Instance.Waitting(true);
            console.log(`QuestPanel instantiate start ${this.questPanelPrefab}!`);
            let panel = instantiate(await this.questPanelPrefab);
            console.log(`QuestPanel instantiate end ${panel}!`);

            panel.getComponent(QuestPanel).Open();
            panel.setParent(GameManager.Instance.node);
            
            this.activity = false;
            this.panelNode.active = this.activity;

            GameManager.Instance.Waitting(false);
            resolve();
          });
        },
        this,
      );
      
      this.panelNode?.on(
        Node.EventType.TOUCH_START,
        () => {
          this.activity = true;
        },
        this,
      );
      
      //打开用户信息
      this.userAvatar?.on(
        Button.EventType.CLICK,
        () => {
          AudioManager.Instance.PlayerOnShot("Sound/sound_player_homepage_01");
          this.panelNode.dispatchEvent(
            new SendMessage("OpenUserInfoBoard", true, this.avatarUrl),
          );
        },
        this,
      );

      //打开排行榜
      this.rankListBtn?.on(
        Button.EventType.CLICK,
        () => {
          AudioManager.Instance.PlayerOnShot("Sound/sound_player_homepage_01");
          this.panelNode.dispatchEvent(
            new SendMessage("OpenRankListBoard", true, User.UserData),
          );
        },
        this,
      );

      this.guideMask.node.on(Node.EventType.TOUCH_END, () => {
        let guide_role = this.mainPanel.getChildByPath("Mask/guide_role");
        if (this.guideIndex >= this.guideText.length) {
          guide_role.active = false;
        }
        else {
          let guide_text = guide_role.getChildByPath("GuideTxt");
          guide_text.getComponent(Label).string = this.guideText[this.guideIndex];
          this.guideIndex++;
        }

      }, this);

      //返回主界面
      let bg = this.mainPanel?.getChildByPath("BG");
      bg.on(Node.EventType.TOUCH_END,()=>
      {
        console.log("bg on CLICK: ", bg);
        this.startGamePart?.getComponent(StartGame)?.ClosePanel();
      },this);
      console.log("bg reg on CLICK: ", bg);

    } catch (error) {
      console.error("MainInterface 下 RegButton 错误 err: ", error);
    }
  }
  /*
   * 修改RegCallBack
   * author：Hotaru
   * 2024/05/13
   * 格式化、回调排行榜周结算奖励
   */
  private RegCallBack() {
    //回调打开弹窗显示获得的卡牌或者碎片
    singleton.netSingleton.player.cb_buy_card_packet = (
      _cardPacketInfo: CardPacket,
      _bagInfo: common.Bag,
    ) => {
      if (_bagInfo && _cardPacketInfo) {
        User.UserData.bag = _bagInfo;
        let panel = this.parentNode
          .getChildByName("StorePanel")
          .getComponent(StorePanel)
          .ShowCardPacketContent(_cardPacketInfo);
        //this.storePanel.getComponent(StorePanel).ShowCardPacketContent(_cardPacketInfo);
      }
    };
    //回调合并碎片后获得卡牌
    singleton.netSingleton.player.cb_buy_card_merge = (
      _roleId: number,
      _playerInfo: common.UserData,
    ) => {};
    
    //回调返回用户信息
    singleton.netSingleton.player.cb_get_user_data = (
      _userData: common.UserData,
      _onLoad: boolean,
    ) => {
      User.UserData = _userData;
      if (_onLoad) {
      }
    };
    //回调任务成就完成
    singleton.netSingleton.player.cb_achievement_complete = (
      achieve: common.UserAchievement,
      wAchieve: common.UserWeekAchievement,
    ) => {
      User.UserData.Achiev = achieve;
      User.UserData.wAchiev = wAchieve;
      
      if (null != singleton.netSingleton.mainInterface) {
        if (null != this.panelNode) {
        }
      }
    };
    //回调领取任务奖励
    singleton.netSingleton.game.cb_check_achievement = (
      _achievementReward: common.AchievementReward,
    ) => {
      //this.userAccount.money+=_achievementReward.gold;
    };
    //回调排行榜周结算奖励
    singleton.netSingleton.player.cb_rank_reward = (
      _reward: common.RankReward,
      _timeDiff,
    ) => {
      if (_timeDiff > 7) {
        singleton.netSingleton.player.get_user_data();
      } else {
        let items: Map<string, number> = new Map();
        items.set("Gold", _reward.gold);
        this.panelNode.dispatchEvent(
          new SendMessage("OpenPopUps", true, {
            type: enums.PopUpsType.Reward,
            title: "获得",
            subheading: "周排行榜奖励",
            items: items,
          }),
        );
      }
    };
  }
}