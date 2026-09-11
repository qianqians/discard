import {
  _decorator,
  Animation,
  assetManager,
  BlockInputEvents,
  Button,
  color,
  Component,
  ImageAsset,
  instantiate,
  Label,
  Node,
  Prefab,
  RichText,
  ScrollView,
  Sprite,
  SpriteFrame,
  Texture2D,
  Vec3,
} from "cc";
import { AudioManager } from "../other/AudioManager";
import { rank_item } from "../serverSDK/rank_comm";
import * as common from "../serverSDK/common";
import { decode } from "../serverSDK/@msgpack/msgpack";
import * as singleton from "../netDriver/netSingleton";
import { BundleManager } from "../bundle/BundleManager";
import { Role, UserData, UserRankInfo } from "../serverSDK/common";
import SdkManager from "../SDK/SdkManager";
import { loadAssets } from "../bundle/LoadAsset";
import { config } from "../battle/AutoChessBattle/config/config";
import { netSingleton } from "../netDriver/netSingleton";
import { User } from "../login/User";
import { MainInterface } from "../mainInterface/MainInterface";
import { GameManager } from "../other/GameManager";
const { ccclass, property } = _decorator;

@ccclass("RankList")
export class RankList extends Component {
  private closeBtn: Node;
  private board: Node;
  private listcontent: Node;
  private selfRank: Node;
  private displayFarme: Node;

  //预制体
  private rankFarmePre: Prefab;
  // 头像纹理引用，用于释放旧纹理避免内存泄漏
  private _avatarTextures: { sf: SpriteFrame; tex: Texture2D }[] = [];

  private pointer: number = 0;
  private listStartY: number = 0;

  protected onLoad(): void {
    this.closeBtn = this.node.getChildByPath("Board/Close_Btn");
    this.board = this.node.getChildByPath("Board");
    this.listcontent = this.board.getChildByPath("RankView/view/content");
    this.selfRank = this.board.getChildByPath("SelfRank/SelfRank");
    this.displayFarme = this.board.getChildByPath("DisplayFarme");

    this.Init();
  }

  start() {
    this.closeBtn.on(
      Button.EventType.CLICK,
      () => {
        AudioManager.Instance.PlayerOnShot("Sound/sound_click_close_01");
        this.node.getComponent(BlockInputEvents).enabled = false;
        this.Close();
      },
      this,
    );

    this.node.on(
      Button.EventType.CLICK,
      () => {
        this.node.getComponent(BlockInputEvents).enabled = false;
        this.Close();
      },
      this,
    );

    this.listStartY = this.listcontent.position.y;
  }

  private Close() {
    try {
      if (singleton.netSingleton.mainInterface) {
        singleton.netSingleton.mainInterface.activity = true;
      }
      this.board
        .getComponent(Animation)
        .on(Animation.EventType.FINISHED, () => {
          this.board.getComponent(Animation).off(Animation.EventType.FINISHED);
          // 动画结束后销毁子节点和纹理
          this.listcontent.destroyAllChildren();
          for (let t of this._avatarTextures) {
              if (t.sf) t.sf.destroy();
              if (t.tex) t.tex.destroy();
          }
          this._avatarTextures = [];
          // 关闭后销毁节点释放内存
          GameManager.Instance.RemoveBoard("RankListBoard");
        });
      this.board.getComponent(Animation).play("PanelDisappear");
    } catch (error) {
      console.error("RankList 下的 Close 错误 : ", error);
    }
  }

  private async Init() {
    this.RegCallBack();
    const t0 = Date.now();
    this.rankFarmePre = (await BundleManager.Instance.loadAssetsFromBundle(
      "PartPrefabs",
      "RankFarme",
    )) as Prefab;
    console.log(`RankList Init: loadAssetsFromBundle("PartPrefabs", "RankFarme") = ${Date.now()-t0}ms`);
  }

  private RegCallBack() {
    singleton.netSingleton.player.cb_get_rank_guid = (_rank, info) => {
      //获取个人排行、展示代码写这
      this.ShowSelfData(_rank, info);
    };

    singleton.netSingleton.player.cb_get_rank_range = async (_rank_info) => {
      //获取区间排行、展示代码写这
      await this.ShowRankList(_rank_info);
    };
  }

  protected update(dt: number): void {
    this.OnDragEvent();
  }

  //列表采用动态加载，节省空间
  private OnDragEvent() {
    //100记得改成字典的长度
    if (
      this.pointer + 24 < 100 &&
      this.listcontent.position.y >= this.listStartY + 16 * 120
    ) {
      if (
        this.board
          .getChildByPath("RankView")
          .getComponent(ScrollView)
          .isAutoScrolling()
      ) {
        return;
      }
      let downLoad = 8;
      this.pointer += downLoad;

      if (this.pointer + 24 > 100) //100记得改成字典的长度
      {
        let outLen = this.pointer + 24 - 100; //100记得改成字典的长度
        downLoad -= outLen;
        this.pointer -= downLoad;
      }
      //此处更新物体信息
      //.....
      this.listcontent.setPosition(
        new Vec3(0, this.listcontent.position.y - downLoad * 120, 0),
      );
      return;
    }

    if (this.pointer > 0 && this.listcontent.position.y <= this.listStartY) {
      if (
        this.board
          .getChildByPath("RankView")
          .getComponent(ScrollView)
          .isAutoScrolling()
      ) {
        return;
      }
      let upLoad = 8;
      this.pointer -= upLoad;
      if (this.pointer < 0) {
        upLoad += this.pointer;
        this.pointer = 0;
      }
      //此处更新物体信息
      //.....
      this.listcontent.setPosition(
        new Vec3(0, this.listcontent.position.y + upLoad * 120, 0),
      );
    }
  }

  public OpenRankListBoard(_userInfo: UserData) {
    try {
      console.log("OpenRankListBoard begin!");
      singleton.netSingleton.player.get_rank_guid("rank", _userInfo.User.UserGuid);
      singleton.netSingleton.player.get_rank_range("rank", 1, 99);
      AudioManager.Instance.PlayerOnShot("Sound/sound_home_return_feedback_01");
      this.node.getComponent(BlockInputEvents).enabled = true;
      this.node.setSiblingIndex(98);
      this.node.active = true;
      this.board.getComponent(Animation).play("PanelAppear");
    } catch (error) {
      console.error("RankList 下的 OpenRankListBoard 错误 : ", error);
    }
  }

  private async ShowSelfData(key: rank_item, rank: UserRankInfo) {
    try {
      console.log("ShowSelfData rank:", JSON.stringify(rank));

      if (key.rank <= 3) {
        switch (key.rank) {
          case 1:
            this.selfRank.getChildByPath("RankFlag").children[0].active = true;
            break;
          case 2:
            this.selfRank.getChildByPath("RankFlag").children[1].active = true;
            break;
          case 3:
            this.selfRank.getChildByPath("RankFlag").children[2].active = true;
            break;
        }
      } else {
        if (key.rank >= 100) {
          this.selfRank.getChildByPath("RankNmber").getComponent(Label).string = "99+";
        } else {
          this.selfRank.getChildByPath("RankNmber").getComponent(Label).string = "" + key.rank;
        }
      }

      if (rank != null) {
        this.selfRank.getChildByPath("UserName").getComponent(Label).string = "" + rank.nick_name;
        this.selfRank.getChildByPath("huihe/Label").getComponent(Label).string = "" + rank.battle_data.round;
        this.selfRank.getChildByPath("HPIcon/Label").getComponent(Label).string = "" + rank.battle_data.faild;
        this.selfRank.getChildByPath("jiangbei/Label").getComponent(Label).string = "" + rank.battle_data.victory;
        
        this.ShowAvatar(rank.avatar, this.selfRank.getChildByPath("UserAvatar/Mask/Sprite"));
      }
      else {
        this.selfRank.getChildByPath("UserName").getComponent(Label).string = "" + User.UserData.User.UserName;
        this.selfRank.getChildByPath("huihe/Label").getComponent(Label).string = "0";
        this.selfRank.getChildByPath("HPIcon/Label").getComponent(Label).string = "0";
        this.selfRank.getChildByPath("jiangbei/Label").getComponent(Label).string = "0";
        
        this.ShowAvatar(User.UserData.User.Avatar, this.selfRank.getChildByPath("UserAvatar/Mask/Sprite"));
      }

    } catch (error) {
      console.error("UserInfo 下 ShowUserData 错误 err: ", error);
    }
  }

  private async ShowAvatar(_url: string, _avatar: Node) {
    try {
      //console.log("尝试加载头像：", _url);
      let sprite = _avatar.getComponent(Sprite);
      await assetManager.loadRemote<ImageAsset>(
        _url,
        { ext: ".png" },
        (_err, image) => {
          let sp = new SpriteFrame();
          let texture = new Texture2D();
          texture.image = image;
          sp.texture = texture;
          this._avatarTextures.push({ sf: sp, tex: texture });
          sprite.spriteFrame = sp;
        },
      );
    } catch (error) {
      console.error("UserInfo 下 ShowAvatar 错误 err: ", error);
    }
  }

  private async ShowRankList(_rank_Info: rank_item[]) {
    let i = 1;
    for (let _info of _rank_Info) {
      if (i > 24) {
        break;
      }

      let value = common.protcol_to_UserRankInfo(decode(_info.item));
      console.log("ShowRankList _rank_Info:", JSON.stringify(value));
      if (!value.battle_data) {
        continue;
      }

      if (i <= 1) {
        let t = this.board.getChildByName("UserAvatarNum1")
        t.getChildByPath("UserName").getComponent(RichText).string = 
          `<color=#ffffff><outline color=#000000 width=5>${value.nick_name}</outline></color>`;
        t.getChildByPath("LevelFrame/Level").getComponent(Label).string = "1";
        this.ShowAvatar(value.avatar, t.getChildByPath("Mask/Sprite"));

        this.displayFarme.getChildByPath("huihe/Label").getComponent(Label).string = "" + value.battle_data.round;
        this.displayFarme.getChildByPath("HPIcon/Label").getComponent(Label).string = "" + value.battle_data.faild;
        this.displayFarme.getChildByPath("jiangbei/Label").getComponent(Label).string = "" + value.battle_data.victory;

        if (value.battle_data.User.UserGuid == User.UserData.User.UserGuid) {
          this.ShowSelfData(_info, value);
        }

      } else {
        let t = instantiate(this.rankFarmePre);
        if (_info.rank <= 3) {
          switch (_info.rank) {
            case 1:
              t.getChildByPath("RankFlag").children[0].active = true;
              break;
            case 2:
              t.getChildByPath("RankFlag").children[1].active = true;
              break;
            case 3:
              t.getChildByPath("RankFlag").children[2].active = true;
              break;
          }
        } else {
          t.getChildByPath("RankNmber").getComponent(Label).string = "" + i; //_info.rank;
        }
        t.getChildByPath("UserName").getComponent(Label).string = "" + value.nick_name;

        t.getChildByPath("huihe/Label").getComponent(Label).string = "" + value.battle_data.round;
        t.getChildByPath("HPIcon/Label").getComponent(Label).string = "" + value.battle_data.faild;
        t.getChildByPath("jiangbei/Label").getComponent(Label).string = "" + value.battle_data.victory;
        
        this.ShowAvatar(value.avatar, t.getChildByPath("UserAvatar/Mask/Sprite"));

        if (value.battle_data.User.UserGuid == User.UserData.User.UserGuid) {
          this.ShowSelfData(_info, value);
        }

        this.listcontent.addChild(t);
      }
      i++;
    }
  }
}
