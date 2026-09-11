import {
  _decorator,
  Component,
  Node,
  Canvas,
  instantiate,
  sys,
  Prefab,
  TTFFont,
} from "cc";

const { ccclass, property } = _decorator;

import * as common from "../serverSDK/common";

import * as singleton from "../netDriver/netSingleton";
import * as load from "../loading/load";

import { Battle } from "../battle/AutoChessBattle/battle";
import { BattleDis } from "../battle/display/BattleDis";
import * as config from "../battle/AutoChessBattle/config/config";
import { BundleManager } from "../bundle/BundleManager";
import { ReadyData } from "../ready/ReadyData";
import { ReadyDis } from "../ready/display/ReadyDis";
import { MainInterface } from "../mainInterface/MainInterface";
import { sleep } from "../other/sleep";
import { GameManager, ReadConfig } from "../other/GameManager";
import * as enums from "../other/enums";
import SdkManager from "../SDK/SdkManager";
import * as player_login from "../serverSDK/ccallplayer";
import { QuestPanel } from "../panel/QuestPanel";
import { User } from "./User";
import { SendMessage } from "../other/MessageEvent";
import { Loading } from "../loading/load";
import "minigame-api-typings";

@ccclass("login")
export class login extends Component {
  @property(Node)
  netNode: Node = null;
  @property(Canvas)
  bk: Canvas = null;
  @property(Canvas)
  ld: Canvas = null;
  @property(Node)
  nodeLoading: Node = null;

  private progressBar: Node = null;

  private interval;

  private _loading: Loading = null;
  private _setProgress: (progress: number) => void = null;
  private _progress = 0.1;

  private nick_name: string = null;
  private avatar_url: string = null;

  private isNetSuccess: boolean = false;

  private mainInterfacePanelPrefab: Promise<Prefab>;
  private questPanelPrefab: Promise<Prefab>;

  private AutoUpdata(): Promise<void> {
    return new Promise((resolve, reject) => {
      const updateManager = wx.getUpdateManager();

      updateManager.onCheckForUpdate(function (res) {
        // 请求完新版本信息的回调
        console.log(res.hasUpdate);
        if (!res.hasUpdate) {
          resolve();
        }
      });

      updateManager.onUpdateReady(() => {
        wx.showModal({
          title: "更新提示",
          content: "新版本已经准备好，是否重启应用？",
          success: (res) => {
            if (res.confirm) {
              // 新的版本已经下载好，调用 applyUpdate 应用新版本并重启
              updateManager.applyUpdate();
              resolve();
            }
          },
        });
      });

      updateManager.onUpdateFailed(() => {
        // 新版本下载失败
         wx.showModal({
          title: "更新提示",
          content: "下载失败，请检查网络？",
          success: (_) => {
            reject();
          },
        });
      });
    });
  }

  protected onLoad(): void {}

  private initLoading() {
//初始化加载界面
    this._loading = new load.Loading();
    this._loading.loadAsset(this.nodeLoading);
    this._setProgress = this._loading.load(this.ld.node, true);

    this.progressBar = this._loading.progressBar;
    this.progressBar.active = true;
    this._loading.ShowLog("", 0);
    this._progress = 0.11;
    this._setProgress.call(this, this._progress)
  }

  private async loadBundle() {
    let _callBack = (bundleName, progress) => {
      this._loading.ShowLog(bundleName, progress);
      this._progress = progress/100;
      this._setProgress?.call(this, this._progress);
    };
    let _complete = () => {
      this._progress = 1.0;
      this._setProgress?.call(this, this._progress);
    };

    let cardCfgs = ReadConfig(enums.Biomes.Jungle);
    let waitLoad = [];
    for (let j = 0; j < cardCfgs.length; j++)
    {
        let ads = cardCfgs[j].Skel.split('/');
        waitLoad.push(BundleManager.Instance.PreLoadBundleDir(ads[0], ads[1], false, _callBack, _complete));
        let res = cardCfgs[j].Res.split('/');
        waitLoad.push(BundleManager.Instance.PreLoadBundleDir(res[0], res[1], false, _callBack, _complete));
    }
    await Promise.all(waitLoad);

    await BundleManager.Instance.PreLoadBundleDir("Ready","", false, _callBack, _complete);
    await BundleManager.Instance.PreLoadBundleDir("FrameTexture","", false, _callBack, _complete);
    await BundleManager.Instance.PreLoadBundleDir("OtherTexture", "", false, _callBack, _complete);
    await BundleManager.Instance.PreLoadBundleDir("PanelTexture", "", false, _callBack, _complete);
    await BundleManager.Instance.PreLoadBundleDir("PropsTexture","", false, _callBack, _complete);
    await BundleManager.Instance.PreLoadBundleDir("BackGroungTexture", "", false, _callBack, _complete);
    await BundleManager.Instance.PreLoadBundleDir("RoleAvatarTexture","", false, _callBack, _complete);
    await BundleManager.Instance.PreLoadBundleDir("IconTexture","", false, _callBack, _complete);
    await BundleManager.Instance.PreLoadBundleDir("ButtonTexture", "", false, _callBack, _complete);
    await BundleManager.Instance.PreLoadBundleDir("CardLib","",  false, _callBack, _complete);
    await BundleManager.Instance.PreLoadBundleDir("SpecialSpine","", false, _callBack, _complete);
    await BundleManager.Instance.PreLoadBundleDir("SceneSpine", "", false, _callBack, _complete);
    await BundleManager.Instance.PreLoadBundleDir("PanelSpine", "QuestMap", false, _callBack, _complete);
    await BundleManager.Instance.PreLoadBundleDir("Typeface", "MAOKENASSORTEDSANS", false, _callBack, _complete);
    await BundleManager.Instance.PreLoadBundleDir("MainInterface", "", false, _callBack, _complete);
    await BundleManager.Instance.PreLoadBundleDir("BoardPrefabs", "UserInfoBoard", false, _callBack, _complete);
    await BundleManager.Instance.PreLoadBundleDir("PanelPrefabs", "waiting", false, _callBack, _complete);
    await BundleManager.Instance.PreLoadBundleDir("PartPrefabs", "RankFarme", false, _callBack, _complete);
    await BundleManager.Instance.PreLoadBundleDir("MainPage", "", false, _callBack, _complete);

    BundleManager.Instance.PreLoadBundleDir("EffectSpine", "skill_0006_2", true);
  }

  private randLoading(_progress:number) {
    this._loading.ShowLog("", _progress);
    this._progress = _progress / 100;
    this._setProgress?.call(this, this._progress);
  }

  private async preloadMasterBundle() {
    //预加载
    await this.loadBundle();
    
    this.mainInterfacePanelPrefab = GameManager.Instance.getMainPagePanel("MainInterface");
    this.questPanelPrefab = GameManager.Instance.getMainPagePanel("QuestPanel");

    this.randLoading(31);
  }

  private regLoginNonAccount() {
    //连接
    singleton.netSingleton.player.cb_player_login_non_account = (
      code: string,
    ) => {
      this._progress += 0.01;
      this._setProgress?.call(this, this._progress);

      console.log("login non_account create role");
      this._loading.ShowLog("尝试连接服务器...", 0);
      singleton.netSingleton.player.create_role(
        code,
        SdkManager.SDK.getUserInfo().nickName,
        SdkManager.SDK.getUserInfo().nickName,
        SdkManager.SDK.getUserInfo().avatarUrl,
      );
    };
  }

  private regReconnect() {
    //重连
    this.netNode.on("reconnect", () => {
      console.log("on net reconnect!");

      if (!singleton.netSingleton.player.UserData) {
        this._progress += 0.3;
        this._setProgress?.call(this, this._progress);
        //this.wxLogin();
        SdkManager.SDK.login(async (e: boolean) => {
          if (e != null) {
            this._loading.progressBar.active = e;
            this._loading.log.node.active = e;
          }
        }, null);
        return;
      }

      singleton.netSingleton.player
        .reconnect(singleton.netSingleton.player.UserData.User.UserGuid)
        .callBack(
          async (info, match_name) => {
            singleton.netSingleton.player.UserData = info;
            this._loading.ShowLog("正在连接中...", 0);
            if (match_name != "") {
              singleton.netSingleton.game.match_name = match_name;
              if (singleton.netSingleton.ready) {
                singleton.netSingleton.game
                  .get_match_battle_data()
                  .callBack(
                    (battle_info, shop_info, fetters_info) => {
                      singleton.netSingleton.ready.Restore(battle_info);
                    },
                    () => {
                      console.log("on net reconnect get_battle_data error!");
                    },
                  )
                  .timeout(3000, () => {
                    console.log("on net reconnect get_battle_data timeout!");
                    this.node.dispatchEvent(
                      new SendMessage("OpenPopUps", true, {
                        type: enums.PopUpsType.Other,
                        title: "",
                        subheading: "服务器连接超时",
                      }),
                    );
                  });
              }
            } else {
              await this.BackMainInterface();
            }
          },
          async (err) => {
            if (singleton.netSingleton.ready) {
              singleton.netSingleton.ready.destory();
              singleton.netSingleton.ready = null;
            }
            if (singleton.netSingleton.battle) {
              singleton.netSingleton.battle.destory();
              singleton.netSingleton.battle = null;
              console.log("regReconnect .battle = null!");
            }

            this._loading = new load.Loading();
            await this._loading.creatorPanel();
            this._setProgress = this._loading.load(this.ld.node);

            this.interval = setInterval(() => {
              this._progress += 0.001;
              this._setProgress?.call(this, this._progress);
            }, 100);

            SdkManager.SDK.login(async (e: boolean) => {
              if (e != null) {
                this._loading.progressBar.active = e;
                this._loading.log.node.active = e;
              }
            }, null);
          },
        );
    });
  }

  private enterMainInterface() {
    //登录进入主界面
    let that = this;
    singleton.netSingleton.player.cb_player_login_sucess = async () => {
      console.log("login sucess callback 00000");

      while (!that.isNetSuccess) {
        await sleep(500);
      }
      console.log("login sucess callback 11111");

      that._progress += 0.1;
      that._setProgress?.call(that, that._progress);

      // 等待主界面需要的资源加载完成
      BundleManager.Instance.WaitLoadMust();
      BundleManager.Instance.WaitLoadBundle();
      console.log("login sucess callback: must + wait bundles loaded");

      if (!singleton.netSingleton.mainInterface) {
        singleton.netSingleton.mainInterface = new MainInterface();
      }

      console.log("login sucess callback check new mainInterface");

      await singleton.netSingleton.player.get_user_data(true, (_steps) => {
        console.log("get_user_data guide step:", _steps);
      });

      that.interval = setInterval(() => {
        that._progress += 0.001;
        that._setProgress?.call(that, that._progress);
      }, 100);

      console.log(`login sucess callback load assert start ${Date.now()}, ${that.mainInterfacePanelPrefab}, ${that.questPanelPrefab}`);
      let typeface = (await BundleManager.Instance.loadAssetsFromBundle("Typeface", "MAOKENASSORTEDSANS",)) as TTFFont;
      console.log(`login sucess callback load assert end ${Date.now()}`);

      console.log("login sucess callback check mainInterface start");
      await singleton.netSingleton.mainInterface.start(that.bk.node, that.mainInterfacePanelPrefab, that.questPanelPrefab, typeface);

      singleton.netSingleton.mainInterface.Init();
      singleton.netSingleton.mainInterface.ShowAvatar(SdkManager.SDK.getUserInfo().avatarUrl);
      console.log("login sucess callback check mainInterface init");

      that.clearInterval();
      this.doneLoading();

      singleton.netSingleton.login = that;
    };
  }

  clearInterval() {
    if (this.interval) {
      clearInterval(this.interval);
      this.interval = null;
    }
  }
  
  doneLoading() {
    this._setProgress?.call(this, 1.0);
    this._setProgress = null;
    this._loading?.done();
  }

  async start() {
    if (sys.platform === sys.Platform.WECHAT_GAME) {
      SdkManager.SetPlatform(enums.SDK_TYPE.WX);
    } else {
      SdkManager.SetPlatform(enums.SDK_TYPE.Default);
    }

    await this.AutoUpdata();
    await config.config.load();

    //注册回调
    this.RegGameCallBack();
    this.regLoginNonAccount();
    this.regReconnect();
    this.enterMainInterface();

    this.initLoading();
    await this.preloadMasterBundle();
    BundleManager.Instance.PreloadMustBundle(true);
    BundleManager.Instance.PreloadWaitBundle(true);

    console.log("login start!");
    let wait = singleton.netSingleton.connect_server();
    SdkManager.SDK.login(async (_) => {
      this._progress += 0.15;
      this._setProgress?.call(this, this._progress);
      this._loading.ShowLog("", Math.floor(this._progress*100));

      await wait;
      this.isNetSuccess = true;
      this._progress += 0.15;
      this._setProgress?.call(this, this._progress);
      this._loading.ShowLog("", Math.floor(this._progress*100));
    }, null);
    
    await GameManager.Instance.Init();
  }

  private RegGameCallBack() {
    //pvp准备阶段
    singleton.netSingleton.game.cb_start_match_battle_ready = (
      battle_info: common.UserBattleData, shop_info: common.ShopData, fetters_info: common.Fetters[]) => 
    {
      this.GameStart(enums.GameMode.PVP, battle_info, shop_info, fetters_info);
    };

    //pvp战斗阶段
    singleton.netSingleton.game.cb_start_match_battle = (
      self: common.UserBattleData,
      target: common.UserBattleData,
    ) => {
      this.BattleStart(self, target, enums.GameMode.PVP);
    };

    //pvp结算
    singleton.netSingleton.player.cb_match_settlement = () => {};

    //pvp游戏结束
    singleton.netSingleton.game.cb_battle_victory = async (
      mod: common.BattleMod,
      is_victory: boolean,
    ) => {
      console.log("战斗结束");
      let isBackMain = true;
      if (singleton.netSingleton.battle) {
        await singleton.netSingleton.battle.SetGameVictory(is_victory, async () => {
          console.log("返回主界面");
          if (isBackMain) {
            await this.BackMainInterface();
          }
        });
      }
    };

    //pve准备阶段
    singleton.netSingleton.game.cb_start_quest_battle_ready = (
      battle_info, shop_info, events, fetters_info) => 
    {
      this.GameStart(enums.GameMode.PVE, battle_info, shop_info, fetters_info, events);
    };

    //pve战斗阶段
    singleton.netSingleton.game.cb_start_quest_battle = (_self, _target) => {
      this.BattleStart(_self, _target, enums.GameMode.PVE);
    };

    singleton.netSingleton.game.cb_confirm_quest_victory = async (state) => {
      console.log("cb_confirm_quest_victory:", singleton.netSingleton.battle);
      if (singleton.netSingleton.battle) {
        if (player_login.em_quest_state.failed == state) {
          await singleton.netSingleton.battle.SetGameVictory(false, async () => {
            console.log("返回pve场景");
            await this.BackMainInterface("VenturePanel");
          });
        }
        else if (player_login.em_quest_state.finish_tmp_level == state) {
          await singleton.netSingleton.battle.SetGameVictory(true, async () => {
            console.log("返回pve场景");
            await this.BackMainInterface("VenturePanel");
          });
        }
        else if (player_login.em_quest_state.next_quest == state) {
          await singleton.netSingleton.battle.SetGameVictory(true, async () => {
            console.log("返回pve场景");
            User.UserData.quest++;
            await this.BackMainInterface("VenturePanel");
          });
        }
        else if (player_login.em_quest_state.finish_pve_level == state) {
          await singleton.netSingleton.battle.SetGameVictory(true, async () => {
            console.log("返回pve场景");
            User.UserData.quest++;
            await this.BackMainInterface("VenturePanel");
          });
        }
      }
    };
  }

  /*public async WaitLoadBundle() {
    this._loading = new load.Loading();
    await this._loading.creatorPanel();
    this._setProgress = this._loading.load(this.ld.node);
    await sleep(600); // 等待加载界面显示
    
    let _callBack = (bundleName, progress) => {
      this._loading.ShowLog(bundleName, progress);
      this._progress = progress/100;
      this._setProgress?.call(this, this._progress);
    };
    let _complete = () => {
      this._progress = 1.0
      this._setProgress?.call(this, this._progress);
    };

    //await BundleManager.Instance.PreLoadBundleDir("RoleSpine", "", false, _callBack, _complete);
    //await BundleManager.Instance.PreLoadBundleDir("EffectSpine", "", false, _callBack, _complete);
    //await sleep(600);

    this.randLoading(30);
    this.interval = setInterval(() => {
      this._progress += 0.001;
      this._setProgress?.call(this, this._progress);
      this._loading?.ShowLog("", Math.floor(this._progress*100));
    }, 100);

    await BundleManager.Instance.WaitLoadBundle();
    await BundleManager.Instance.WaitLoadMust();

    BundleManager.Instance._battleBundlesReleased = false;

    return () => {
      this.clearInterval();
      this.doneLoading();
    }
  }*/

  private async GameStart(
    _gamemode: enums.GameMode,
    _battle_info: common.UserBattleData,
    _shop_info: common.ShopData,
    _fetters_info?: common.Fetters[],
    events?: player_login.QuestRogueRole[],
  ) {
    console.log("start game!");
    
    let _readyData = new ReadyData(
      _battle_info,
      _shop_info,
      _gamemode,
      _fetters_info,
      events,
    );
    if (!singleton.netSingleton.ready) {
      singleton.netSingleton.ready = new ReadyDis(_readyData);
    }
    console.log("singleton.netSingleton.ready!");
    
    await singleton.netSingleton.ready.start(this.bk.node, _battle_info, async (_event) => {
      console.log("Start Ready callback!");
      
      if (singleton.netSingleton.battle) {
        singleton.netSingleton.battle.destory();
        singleton.netSingleton.battle = null;
        console.log("GameStart.battle = null!");
        // 释放战斗相关 bundle 资源
        //BundleManager.Instance.ReleaseBattleBundles();
      }

      if (singleton.netSingleton.mainInterface) {
        singleton.netSingleton.mainInterface.HideStartGamePart();
        singleton.netSingleton.mainInterface.destory();
        singleton.netSingleton.mainInterface = null;
      }

      await sleep(10); //不知道为啥必须等待0.01秒，商店物品的位置才不会错
      await _event();
      this._setProgress?.call(this, 1.0);
      this._setProgress = null;
      this._loading?.done();
      this._loading = null;
      console.log("GameStart sucess!");
    });

    console.log("start singleton.netSingleton.ready!");
  }

  private async BattleStart(
    _self: common.UserBattleData,
    _target: common.UserBattleData,
    _gamemode: enums.GameMode,
  ) {
    console.log(`cb_battle start round! self:${JSON.stringify(_self)} target:${JSON.stringify(_target)}`);
    
    //let isLoadBundle = BundleManager.Instance.NeedLoadBundle();
    //if (isLoadBundle) {
    //  await this.WaitLoadBundle();
    //}

    let _battle = new Battle(_self, _target, _gamemode);
    singleton.netSingleton.battle = new BattleDis(_battle);
    await singleton.netSingleton.battle.Start(this.bk.node, async (event) => {
      await sleep(111);

      if (this._loading != null) {
        this._setProgress?.call(this, 1.0);
        this._setProgress = null;
        this._loading?.done();
        this._loading = null;
      }
      
      if (singleton.netSingleton.ready) {
        singleton.netSingleton.ready.destory();
        singleton.netSingleton.ready = null;
      }

      this.bk.node.addChild(singleton.netSingleton.battle.panelNode);
      singleton.netSingleton.battle.battleCentre.StartBattle();
      if (event) {
        event();
      }
    });
  }

  public async BackMainInterface(panelName?: string) {
    console.log("BackMainInterface begin!");

    if (panelName == "VenturePanel") {
      //let vt = (await BundleManager.Instance.loadAssetsFromBundle("PanelPrefabs", "QuestPanel")) as Prefab;
      let panel = instantiate(await this.questPanelPrefab);
      panel.setParent(this.node);
      panel.getComponent(QuestPanel)?.Open();

      if (singleton.netSingleton.mainInterface) {
        singleton.netSingleton.mainInterface.panelNode.active = false;
      }
      return;
    }

    if (!singleton.netSingleton.mainInterface) {
      singleton.netSingleton.mainInterface = new MainInterface();
    }
    let typeface = (await BundleManager.Instance.loadAssetsFromBundle("Typeface", "MAOKENASSORTEDSANS",)) as TTFFont;
    await singleton.netSingleton.mainInterface.start(this.bk.node, this.mainInterfacePanelPrefab, this.questPanelPrefab, typeface);

    /*let isLoadBundle = BundleManager.Instance.NeedLoadBundle();
    if (isLoadBundle) {
      await this.WaitLoadBundle();
    } else {
      await sleep(111);
    }*/

    await singleton.netSingleton.player.get_user_data(true, (_) => {
      this._setProgress?.call(this, 1.0);
      this._setProgress = null;
      this._loading?.done();
      this._loading = null;
    });
    await singleton.netSingleton.mainInterface.ShowAvatar(SdkManager.SDK.getUserInfo().avatarUrl);
    singleton.netSingleton.mainInterface.Init();

    if (singleton.netSingleton.battle) {  
      singleton.netSingleton.battle.destory();
      singleton.netSingleton.battle = null;
      console.log("BackMainInterface .battle = null!");
      // 释放战斗相关 bundle 资源
      BundleManager.Instance.ReleaseBattleBundles();
    }
    if (singleton.netSingleton.ready) {
      singleton.netSingleton.ready.destory();
      singleton.netSingleton.ready = null;
    }

    console.log("BackMainInterface end!");
  }
}
