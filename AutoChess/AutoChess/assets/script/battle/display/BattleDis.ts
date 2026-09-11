/*
 * BattleDis.ts
 * author: Hotaru
 * 2023/10/12
 * 战斗展示类
 */
import {
  _decorator,
  instantiate,
  Node,
  Prefab,
  Label,
  Button,
  UITransform,
  Vec3,
  sp,
  game,
  Game,
  error,
} from "cc";
import { login } from '../../login/login';
import { Queue } from "./Queue";
import { Battle } from "../AutoChessBattle/battle";
import * as skill from "../AutoChessBattle/skill/skill_base";
import * as BattleEnums from "../AutoChessBattle/BattleEnums";
import { delay, sleep } from "../../other/sleep";
import { RoleDis } from "./RoleDis";
import { BundleManager } from "../../bundle/BundleManager";
import { hub_call_gate_reverse_reg_client_hub_rsp } from "../../serverSDK/gate";
import { Role as rRole } from "../AutoChessBattle/role";
import { netSingleton } from "../../netDriver/netSingleton";
import * as singleton from "../../netDriver/netSingleton";
import { BattleVictory, Role } from "../../serverSDK/common";
import { Team } from "../AutoChessBattle/team";
import { GameManager } from "../../other/GameManager";
import { SendMessage } from "../../other/MessageEvent";
import { AudioManager } from "../../other/AudioManager";
import { config } from '../AutoChessBattle/config/config';
import * as enums from "../../other/enums";
import * as common from "../../serverSDK/common";
import { GameMode } from "../../other/enums";
const { ccclass, property } = _decorator;

export class BattleDis {
  //摄像机
  private cameraNode: Node;
  private originCameraPos: Vec3;
  //父级对象
  public father: Node;
  //顶级面板
  public panelNode: Node;
  //战斗效果
  private battleEffectImg: Node;
  private launchSkillEffect: Node;
  //敌我开场介绍面板
  private selfBeginEffect: Node;
  private enemyBeginEffect: Node;
  //敌我队列
  public selfQueue: Queue;
  public enemyQueue: Queue;

  public BattleFailed: Node;
  public backMainBtn: Button;
  private isAdRevive: boolean = false;
  private BackMainPage: () => void = null;
  public reviveBtn: Button;

  private btnGMWin: Node;

  public rewardedVideoAd: WechatMinigame.RewardedVideoAd = null;

  public puase: boolean = false;

  //所有需要并行执行的事件队列,分己方和敌人方，己方先执行
  private selfParallelList: Promise<void>[] = [];
  private enemyParallelList: Promise<void>[] = [];

  //战斗系统类
  public battleCentre: Battle = null;

  public constructor(battle: Battle) {
    this.battleCentre = battle;

    this.battleCentre.onPlaySound = (sound: string) => {
      AudioManager.Instance.PlaySound(sound);
    };
    this.battleCentre.onPlayerOnShot = (sound: string) => {
      AudioManager.Instance.PlayerOnShot(sound);
    };
    this.battleCentre.onKillRole = (r: Role) => {
      singleton.netSingleton.game.kill_Role_ntf(r);
    };

    this.onEvent();
  }

  public destory() {
    AudioManager.Instance.Stop();

    this.selfQueue.destroy();
    this.enemyQueue.destroy();

    this.panelNode.destroy();
  }

  private async pullBattleRoleSpine() {
    for (let role of this.battleCentre.GetSelfTeam().GetRoles()) {
      let rc = config.RoleConfig.get(role.id);
      let ads = rc.Skel.split('/');
      await BundleManager.Instance.PreLoadBundleDir(ads[0], ads[1], true);
    }
    for (let role of this.battleCentre.GetEnemyTeam().GetRoles()) {
      let rc = config.RoleConfig.get(role.id);
      let ads = rc.Skel.split('/');
      await BundleManager.Instance.PreLoadBundleDir(ads[0], ads[1], true);
    }
  }

  /*
   * 修改start
   * author：Hotaru
   * 2024/03/07
   * 让加载更平顺
   */
  public async Start(father: Node, _callBack: (e?: () => void) => void) {
    try {
      console.log("battledis start");
      this.panelNode = instantiate(await GameManager.Instance.getPanel("BattlePanel"));
      this.panelNode.setParent(GameManager.Instance.node);

      this.selfQueue = this.panelNode.getChildByName("Self_Queue").getComponent(Queue);
      this.enemyQueue = this.panelNode.getChildByName("Enemy_Queue").getComponent(Queue);

      this.battleEffectImg = this.panelNode.getChildByName("BattleEffectImg");
      this.battleEffectImg.active = false;

      this.selfBeginEffect = this.panelNode.getChildByName("SelfBeginEffect");
      this.selfBeginEffect.active = false;

      this.enemyBeginEffect = this.panelNode.getChildByName("EnemyBeginEffect");
      this.enemyBeginEffect.active = false;

      this.launchSkillEffect = this.panelNode.getChildByName("LaunchSkillEffect");
      this.launchSkillEffect.active = false;

      this.btnGMWin = this.panelNode.getChildByPath("BtnGMWin");
      this.BattleFailed = this.panelNode.getChildByPath("BattleFailed");
      this.BattleFailed.active = false;
      this.backMainBtn = this.BattleFailed.getChildByName("back").getComponent(Button);
      this.reviveBtn = this.BattleFailed.getChildByName("revive").getComponent(Button);
      this.registerEvent();

      let victory = this.panelNode.getChildByName("victory");
      if (victory) {
        victory.active = false;
      }

      console.log("PutRole start");
      await this.PutRole();
      console.log("PutRole end");
      this.father = father;

      await this.pullBattleRoleSpine();
      AudioManager.Instance.PlaySound("Sound/sound_battle");

      _callBack(() => {
        setTimeout(this.TickBattle.bind(this), 500);
      });
    } catch (error) {
      console.error("BattleDis 下的 Start 错误 err:", error);
    }
  }

  private registerEvent() {
    this.btnGMWin.on(Button.EventType.CLICK, async () => {
      if(enums.GameMode.PVP == this.battleCentre.gamemode)
      {
        await netSingleton.game.confirm_match_round_victory(BattleVictory.victory, this.battleCentre.addCoin);
        //netSingleton.game.confirm_peak_strength_victory(BattleVictory.victory, this.battleCentre.addCoin);
      }
      else
      {
        await netSingleton.game.confirm_quest_victory(BattleVictory.victory, this.battleCentre.addCoin);
      }
      this.puase = true;
    });

    this.backMainBtn.node.on(Button.EventType.CLICK, () => {
      this.BattleFailed.active = false;
      this.BackMainPage();
    });

    this.reviveBtn.node.on(Button.EventType.CLICK, async () => {
      this.BattleFailed.active = false;
      if (this.isAdRevive) {
        // 创建激励视频广告实例，提前初始化
        if (!this.rewardedVideoAd) {
          this.rewardedVideoAd = wx.createRewardedVideoAd({
            adUnitId: 'adunit-f680ffa7a8a528b8'
          })
          this.rewardedVideoAd.onError((err) => {
            console.error("激励视频广告错误 err:", err);
            this.rewardedVideoAd = null; // 销毁广告实例，避免重复错误
          });
          this.rewardedVideoAd.onLoad(() => {
            console.log("激励视频广告加载成功!");
          });
          this.rewardedVideoAd.onClose( async (res) => {
            if (res && res.isEnded || res === undefined) {
              // 正常播放结束，可以下发奖励
              console.log("激励视频广告正常播放结束，发放奖励!");
              this.Revive();  
            } else {
              // 播放中途退出，不下发奖励
              console.log("激励视频广告播放中途退出，不发放奖励");
              this.BattleFailed.setSiblingIndex(999);
              this.BattleFailed.active = true;
            }
          });
          await this.rewardedVideoAd.load();
        }
        await this.rewardedVideoAd.show();
      } else {
        let sharedTitle = [ '一起来冒险吧!' , '我在萌萌自走棋等你哦!' , '快来和我一起玩萌萌自走棋吧!' , '萌萌自走棋好玩又刺激，快来和我一起玩吧!' , '萌萌自走棋等你来战!' ];
        wx.shareAppMessage({
          //imageUrl: "shared_bk.jpg",
          title: sharedTitle[Math.floor(Math.random() * sharedTitle.length)],
        });
        this.Revive();
      }
    });
  }

  private Revive() {
    try {
      if(enums.GameMode.PVP == this.battleCentre.gamemode) {
        singleton.netSingleton.game.match_revive_battle().callBack((data, shop) => {
          singleton.netSingleton.game.cb_start_match_battle_ready(data, shop, null);
        }, (err) => {
          console.error("PVP 复活失败 err:", err);
        });
      } else if (enums.GameMode.PVE == this.battleCentre.gamemode) {
        singleton.netSingleton.game.quest_revive_battle().callBack((data, shop) => {
          singleton.netSingleton.game.cb_start_quest_battle_ready(data, shop, null);
        }, (err) => {
          console.error("PVE 复活失败 err:", err);
        });
      }
    }
    catch (err) {
      console.error("Revive 复活失败 err:", err);
    }
  }

  async TickBattle() {
    try {
      while (!this.battleCentre.CheckEndBattle()) {
        await this.battleCentre.TickBattle();

        while (this.puase) {
          await sleep(200);
        }
      }

      let is_victory = BattleVictory.victory;
      if (this.battleCentre.GetWinCamp() == BattleEnums.Camp.Self) {
        is_victory = BattleVictory.victory;
      } else if (this.battleCentre.GetWinCamp() == BattleEnums.Camp.Enemy) {
        is_victory = BattleVictory.faild;
      }

      let addTime = false;
      let settlement = false;
      console.log("准备战斗结算");
      if (this.battleCentre.round <= 15 && 10 == this.battleCentre.victory) {
        addTime = true;
      }
      if (is_victory == BattleVictory.victory || (is_victory == BattleVictory.faild && this.battleCentre.faild - 1 > 0)) {
        let heath = is_victory == BattleVictory.victory ? this.battleCentre.faild : this.battleCentre.faild - 1;
        settlement = true;
        this.panelNode.dispatchEvent(
          new SendMessage("OpenSettlement", true, {
            outcome: is_victory,
            GameMode: this.battleCentre.gamemode,
            addCoin: this.battleCentre.addCoin,
            hpNum: heath,
            isAddTime: addTime,
          }),
        );
      } 
      
      if (!settlement) {
        console.log("TickBattle settlement false");

        await sleep(500);
        if(enums.GameMode.PVP == this.battleCentre.gamemode)
        {
          await this.confirmWithRetry(is_victory);
        }
        else
        {
          await netSingleton.game.confirm_quest_victory(is_victory, this.battleCentre.addCoin);
        }
        this.battleCentre.addCoin = 0;      
      }

    } catch (error) {
      console.error("BattleDis 下的 TickBattle 错误 err:", error);
    }
  }

  private async confirmWithRetry(is_victory: BattleVictory) {
    const maxRetries = 3;
    for (let attempt = 1; attempt <= maxRetries; attempt++) {
      const success = await netSingleton.game.confirm_match_round_victory(is_victory, this.battleCentre.addCoin);
      if (success) {
        return;
      }

      if (attempt < maxRetries) {
        console.log(`confirmWithRetry attempt ${attempt} failed, retrying...`);
        this.panelNode.dispatchEvent(new SendMessage("ShowTip", true, `<outline color=black width=4>网络连接失败，正在重试(${attempt}/${maxRetries})...</outline>`));
        await sleep(1000);
      } else {
        this.panelNode.dispatchEvent(new SendMessage("ShowTip", true, "<outline color=black width=4>网络连接失败，请检查网络后重试</outline>"));
      }
    }
  }

  async SetGameVictory(is_victory: boolean, backMainPage: () => void) {
    if (is_victory) {
      let msg = "游戏胜利!";
      let text = "<outline color=black width=4>" + msg + "</outline>";
      this.panelNode.dispatchEvent(new SendMessage("ShowTip", true, text));

      await sleep(333);
      backMainPage();
    }
    else {
      this.BackMainPage = backMainPage;
      this.BattleFailed.setSiblingIndex(999);
      this.BattleFailed.active = true;
      if (Math.random() < 0.5) {
        this.isAdRevive = true;
        this.reviveBtn.node.getChildByName("Label").getComponent(Label).string = "看广告复活一次!";
      } else {
        this.isAdRevive = false;
        this.reviveBtn.node.getChildByName("Label").getComponent(Label).string = "邀请好友复活一次!";
      }
    }
  }

  async PutRole() {
    try {
      let waits = [];

      let selfRoles = this.battleCentre.GetSelfTeam().GetRoles();
      waits.push(this.selfQueue.InitRole(selfRoles));

      let enemyRoles = this.battleCentre.GetEnemyTeam().GetRoles();
      waits.push(this.enemyQueue.InitRole(enemyRoles));

      await Promise.all(waits);
    } catch (error) {
      console.error("BattleDis 下的 PutRole 错误 err:", error);
    }
  }

  ShowBattleEffect() {
    this.battleEffectImg.active = true;
    this.battleEffectImg.setSiblingIndex(99);
    this.battleEffectImg
      .getComponent(sp.Skeleton)
      .setAnimation(0, "animation", false);
    delay(500, () => {
      this.battleEffectImg.active = false;
    });
  }

  private showLaunchSkillEffect(isShow: boolean = false) {
    if (!isShow) {
      return delay(0, () => {});
    }
    this.launchSkillEffect.active = true;

    this.launchSkillEffect
      .getChildByPath("BottomImg")
      .getComponent(sp.Skeleton).animation = "a2";
    this.launchSkillEffect
      .getChildByPath("RoleImg")
      .getComponent(sp.Skeleton).animation = "a";
    this.launchSkillEffect
      .getChildByPath("TopImg")
      .getComponent(sp.Skeleton).animation = "a";

    return delay(2000, () => {
      this.launchSkillEffect.active = false;
    });
  }

  private showLaunchFettersEffect() {
    return delay(2000, () => {
      this.launchSkillEffect.active = false;
    });
  }

  private shakeScreen(duration: number, magnitude: number) {
    // 获取摄像机组件
    // 保存原始位置，以便震动后可以恢复

    // 震动效果
    if (null == this.cameraNode) return;
    let startTime = Date.now();
    const durationMs = duration * 1000;
    const shake = () => {
      const elapsed = Date.now() - startTime;
      if (elapsed < durationMs) {
        // 随机确定摄像机震动的新位置
        const randomX = Math.random() * magnitude * 2 - magnitude;
        const randomY = Math.random() * magnitude * 2 - magnitude;
        this.cameraNode.setPosition(
          this.originCameraPos.x + randomX,
          this.originCameraPos.y + randomY,
        );

        // 请求下一帧继续执行震动
        requestAnimationFrame(shake);
      } else {
        // 震动结束，恢复摄像机的原始位置
        this.cameraNode.setPosition(this.originCameraPos);
      }
    };

    // 开始震动
    shake();
  }
  /*
   *
   * 以下为事件响应函数
   *
   */
  //战斗开始阶段展示双方名牌
  private async CheckBeginBattle(evs: skill.Event[]) {
    try {
      for (let ev of evs) {
        //console.log("checkAttackEvent ev:", ev)
        if (BattleEnums.EventType.BattleBegin != ev.type) {
          continue;
        }

        this.selfBeginEffect.active = true;
        this.enemyBeginEffect.active = true;

        this.selfBeginEffect.setSiblingIndex(100);
        this.enemyBeginEffect.setSiblingIndex(100);

        this.selfBeginEffect
          .getChildByPath("BottomImg")
          .getComponent(sp.Skeleton).animation = "a2";
        this.selfBeginEffect
          .getChildByPath("UserName")
          .getComponent(Label).string = "己方队伍";
        this.selfBeginEffect
          .getChildByPath("TopImg")
          .getComponent(sp.Skeleton).animation = "a";

        this.enemyBeginEffect
          .getChildByPath("BottomImg")
          .getComponent(sp.Skeleton).animation = "a2";
        this.enemyBeginEffect
          .getChildByPath("UserName")
          .getComponent(Label).string = "敌方队伍";
        this.enemyBeginEffect
          .getChildByPath("TopImg")
          .getComponent(sp.Skeleton).animation = "a";

        return delay(1000, () => {
          this.selfBeginEffect.active = false;
          this.enemyBeginEffect.active = false;
        });
      }
    } catch (error) {
      console.error("BattleDis 下的 CheckBeginBattle 错误 err:", error);
    }
  }

  //普通攻击
  private async CheckAttackEvent(evs: skill.Event[]) {
    try {
      let evs_floating: skill.Event[] = [];
      let selfAttack = false;
      let enemyAttack = false;
      let selfRoleNodeRoleDis: RoleDis = null;
      let enemyRoleNodeRoleDis: RoleDis = null;
      for (let ev of evs) {
        if (
          BattleEnums.EventType.AttackInjured == ev.type ||
          BattleEnums.EventType.TransferInjured == ev.type
        ) {
          evs_floating.push(ev);
        }
        if (BattleEnums.EventType.AttackInjured == ev.type) {
          if (BattleEnums.Camp.Self == ev.spellcaster.camp) {
            if (!selfAttack) {
              for (let r of ev.recipient) {
                if (this.battleCentre.GetEnemyTeam().GetRole(r.index).CheckDead())
                {}
              }
              let roleNode = this.selfQueue.roleNodes[ev.spellcaster.index];
              if (roleNode) {
                selfRoleNodeRoleDis = roleNode.getComponent(RoleDis);
                if (selfRoleNodeRoleDis) {
                  selfAttack = true;
                }
              }
            }
          } else if (BattleEnums.Camp.Enemy == ev.spellcaster.camp) {
            if (!enemyAttack) {
              let roleNode = this.enemyQueue.roleNodes[ev.spellcaster.index];
              if (roleNode) {
                enemyRoleNodeRoleDis = roleNode.getComponent(RoleDis);
                if (enemyRoleNodeRoleDis) {
                  enemyAttack = true;
                }
              }
            }
          }
        }
      }

      if (selfAttack && enemyAttack) {
        console.log("CheckAttackEvent begin!");
        console.log(
          "CheckAttackEvent selfRoleNodeRoleDis:",
          selfRoleNodeRoleDis,
        );
        console.log(
          "CheckAttackEvent enemyRoleNodeRoleDis:",
          enemyRoleNodeRoleDis,
        );

        let allAwait: Promise<void>[] = [];

        allAwait.push(
          selfRoleNodeRoleDis.Attack(
            this.selfQueue.readyLocation.worldPosition,
            this.selfQueue.battleLocation.worldPosition,
            BattleEnums.Camp.Self,
          ),
        );
        allAwait.push(
          enemyRoleNodeRoleDis.Attack(
            this.enemyQueue.readyLocation.worldPosition,
            this.enemyQueue.battleLocation.worldPosition,
            BattleEnums.Camp.Enemy,
          ),
        );

        await Promise.all(allAwait);
        allAwait = [];

        //震动部分
        if (null == this.cameraNode) {
          this.cameraNode = this.panelNode.parent.getChildByName("Camera");
          if (!this.cameraNode) {
            console.error("Camera node not found,shake screen failed");
          } else {
            this.originCameraPos = this.cameraNode.getPosition();
          }
        }
        this.shakeScreen(0.5, 10);
        await this.OnBehurted(evs_floating);

        console.log("CheckAttackEvent end!");
      }
    } catch (error) {
      console.error("BattleDis 下的 CheckAttackEvent 错误 err:", error);
    }
  }

  //受击
  private async OnBehurted(evs: skill.Event[]) {
    try {
      let allAwait = [];
      let r: Node = null;
      for (let ev of evs) {
        if (
          BattleEnums.EventType.AttackInjured == ev.type ||
          BattleEnums.EventType.TransferInjured == ev.type
        ) {
          if (ev.is_trigger_floating) {
            continue;
          }
          ev.is_trigger_floating = true;

          if (BattleEnums.Camp.Self == ev.spellcaster.camp) {
            r = this.enemyQueue.roleNodes[ev.recipient[0].index];
            if (r) {
              allAwait.push(r.getComponent(RoleDis).BeHurted(ev));
              allAwait.push(r.getComponent(RoleDis).ChangeAtt());
            }
          }
          if (BattleEnums.Camp.Enemy == ev.spellcaster.camp) {
            r = this.selfQueue.roleNodes[ev.recipient[0].index];
            if (r) {
              allAwait.push(r.getComponent(RoleDis).BeHurted(ev));
              allAwait.push(r.getComponent(RoleDis).ChangeAtt());
            }
          }
        }
      }
      console.log("OnBehurted allAwait:", allAwait);
      await Promise.all(allAwait);
    } catch (error) {
      console.error("BattleDis 下的 OnBehurted 错误 err:", error);
    }
  }

  //远程攻击技能
  private async CheckRemoteInjured(evs: skill.Event[]) {
    try {
      let allAwait = [];
      for (let ev of evs) {
        if (BattleEnums.EventType.RemoteInjured != ev.type) {
          continue;
        }

        let spList = BattleEnums.Camp.Self == ev.spellcaster.camp ?
          this.selfQueue : this.enemyQueue;
        if (!spList.roleNodes || spList.roleNodes.length <= ev.spellcaster.index) {
          continue;
        }
        let self = spList.roleNodes[ev.spellcaster.index];

        if (ev.isParallel) {
          if (BattleEnums.Camp.Self == ev.spellcaster.camp) {
            this.selfParallelList.push(self.getComponent(RoleDis).UseSkill(ev));
          } else {
            this.enemyParallelList.push(
              self.getComponent(RoleDis).UseSkill(ev),
            );
          }
        } else {
          allAwait.push(self.getComponent(RoleDis).UseSkill(ev));
        }
      }
      await Promise.all(allAwait);
    } catch (error) {
      console.error("BattleDis 下的 CheckRemoteInjured 错误 err:", error);
    }
  }

  //召唤技能
  private async CheckSummonEvent(evs: skill.Event[]) {
    try {
      let allAwait = [];
      for (let ev of evs) {
        if (BattleEnums.EventType.Summon == ev.type) {
          let spList =
            BattleEnums.Camp.Self == ev.spellcaster.camp
              ? this.selfQueue
              : this.enemyQueue;
          let self = spList.roleNodes[ev.spellcaster.index];
          if (ev.isParallel) {
            if (BattleEnums.Camp.Self == ev.spellcaster.camp) {
              this.selfParallelList.push(
                self.getComponent(RoleDis).UseSkill(ev),
              );
            } else {
              this.enemyParallelList.push(
                self.getComponent(RoleDis).UseSkill(ev),
              );
            }
          } else {
            allAwait.push(self.getComponent(RoleDis).UseSkill(ev));
          }
        }
      }
      await Promise.all(allAwait);
    } catch (error) {
      console.error("BattleDis 下的 CheckSummon 错误 err:", error);
    }
  }

  /**
   * 检测队伍增益技能
   * @param evs 事件流
   */
  private async CheckAttGainEvent(evs: skill.Event[]) {
    try {
      let allAwait = [];
      for (let ev of evs) {
        if (
          BattleEnums.EventType.IntensifierProperties == ev.type ||
          BattleEnums.EventType.IntensifierExp == ev.type
        ) {
          if (ev.recipient.length <= 0) continue;
          console.log("检测到加属性事件");
          let spList =
            BattleEnums.Camp.Self == ev.spellcaster.camp
              ? this.selfQueue
              : this.enemyQueue;
          let self = spList.roleNodes[ev.spellcaster.index];

          if (ev.isParallel) {
            if (BattleEnums.Camp.Self == ev.spellcaster.camp) {
              this.selfParallelList.push(
                self.getComponent(RoleDis).UseSkill(ev),
              );
            } else {
              this.enemyParallelList.push(
                self.getComponent(RoleDis).UseSkill(ev),
              );
            }
          } else {
            allAwait.push(self.getComponent(RoleDis).UseSkill(ev));
          }
        }
      }
      await Promise.all(allAwait);
    } catch (error) {
      console.error("BattleDis 下的 CheckAttGainEvent 错误 err:", error);
    }
  }

  //加buff或者护盾
  private async CheckAddBuff(evs: skill.Event[]) {
    try {
      let allAwait = [];
      for (let ev of evs) {
        if (
          BattleEnums.EventType.GiveShields == ev.type ||
          BattleEnums.EventType.AddBuff == ev.type
        ) {
          let spList =
            BattleEnums.Camp.Self == ev.spellcaster.camp
              ? this.selfQueue
              : this.enemyQueue;
          let self = spList.roleNodes[ev.spellcaster.index];

          allAwait.push(self.getComponent(RoleDis).UseSkill(ev));
        }
      }
      await Promise.all(allAwait);
    } catch (error) {
      console.error("BattleDis 下的 CheckAddBuff 错误 err:", error);
    }
  }

  //换位
  private async CheckTransPosition(evs: skill.Event[]) {
    try {
      let allAwait = [];
      for (let ev of evs) {
        if (BattleEnums.EventType.ChangeLocation == ev.type) {
          let spList =
            BattleEnums.Camp.Self == ev.spellcaster.camp
              ? this.selfQueue
              : this.enemyQueue;
          let self = spList.roleNodes[ev.spellcaster.index];

          allAwait.push(self.getComponent(RoleDis).UseSkill(ev));
        }
      }
      await Promise.all(allAwait);
    } catch (error) {
      console.error("BattleDis 下的 CheckTransPosition 错误 err:", error);
    }
  }

  //属性交换
  private async CheckSwapProperties(evs: skill.Event[]) {
    try {
      let allAwait = [];
      for (let ev of evs) {
        if (BattleEnums.EventType.SwapProperties == ev.type) {
          let spList =
            BattleEnums.Camp.Self == ev.spellcaster.camp
              ? this.selfQueue
              : this.enemyQueue;
          let self = spList.roleNodes[ev.spellcaster.index];

          allAwait.push(self.getComponent(RoleDis).UseSkill(ev));
        }
      }
      await Promise.all(allAwait);
    } catch (error) {
      console.error("BattleDis 下的 CheckSwapProperties 错误 err:", error);
    }
  }

  //抗伤
  private async CheckSubstitute(evs: skill.Event[]) {
    try {
      let allAwait = [];
      let evs_floating: skill.Event[] = [];
      for (let ev of evs) {
        if (BattleEnums.EventType.SubstituteDamage == ev.type) {
          let spList =
            BattleEnums.Camp.Self == ev.spellcaster.camp
              ? this.selfQueue
              : this.enemyQueue;
          let self = spList.roleNodes[ev.spellcaster.index];

          allAwait.push(self.getComponent(RoleDis).UseSkill(ev));
        }
      }
      await Promise.all(allAwait);
      //await this.OnBehurted(evs_floating);
    } catch (error) {
      console.error("BattleDis 下的 CheckSubstitute 错误 err:", error);
    }
  }

  //离场
  private async CheckExitEvent(evs: skill.Event[]) {
    try {
      let allAwait = [];
      for (let ev of evs) {
        if (BattleEnums.EventType.Exit != ev.type) {
          continue;
        }

        if (BattleEnums.Camp.Self == ev.spellcaster.camp) {
          allAwait.push(this.selfQueue.RemoveRole(ev.spellcaster.index));
        } else if (BattleEnums.Camp.Enemy == ev.spellcaster.camp) {
          allAwait.push(this.enemyQueue.RemoveRole(ev.spellcaster.index));
        }
      }
      await Promise.all(allAwait);
    } catch (error) {
      console.error("BattleDis 下的 CheckExitEvent 错误 err:", error);
    }
  }

  onEvent() {
    this.battleCentre.on_event = async (evs) => {
      try {
        console.log("onEvent evs:", evs);

        await this.CheckBeginBattle(evs);
        await this.CheckSwapProperties(evs);
        await this.CheckTransPosition(evs);
        await this.CheckRemoteInjured(evs);
        await this.CheckAddBuff(evs);
        await this.CheckSummonEvent(evs);
        await this.CheckAttGainEvent(evs);
        if (
          this.selfParallelList.length > 0 ||
          this.enemyParallelList.length > 0
        ) {
          await Promise.all(this.selfParallelList);
          await Promise.all(this.enemyParallelList);
        }
        this.selfParallelList = [];
        this.enemyParallelList = [];
        await this.CheckAttackEvent(evs);
        await this.CheckSubstitute(evs);
        await this.CheckExitEvent(evs);
      } catch (error) {
        console.error("BattleDis 下的 on_event 错误 err:", error);
      }
    };
  }
}
