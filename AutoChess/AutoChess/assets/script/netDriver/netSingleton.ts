import * as player from "./netPlayer";
import * as game from "./netGame";
import * as shop from "./netBattleShop";
import { login } from "../login/login";
import { BattleDis } from "../battle/display/BattleDis";
import { ReadyDis } from "../ready/display/ReadyDis";
import { MainInterface } from "../mainInterface/MainInterface";

import * as cli from "../serverSDK/client_handle";

export let game_url: string[] = ["wss://zzq.ucat.games:3001", "wss://zzq.ucat.games:3002"];

export class netSingleton {
  public static is_conn_gate = false;
  public static player = new player.netPlayer();
  public static game = new game.netGame();
  public static battleshop = new shop.netBattleShop();
  public static battle: BattleDis = null;
  public static ready: ReadyDis = null;
  public static login: login = null;
  public static mainInterface: MainInterface = null;

  public static conn_gate_svr() {
    return new Promise<void>((resolve, reject) => {
      cli.cli_handle.onGateConnect = () => {
        resolve();
      };

      let url_idx = Math.floor(Math.random() * game_url.length);
      cli.cli_handle.connect_gate(game_url[url_idx], () => {
        reject();
      });
    });
  }

  public static async connect_server() {
    await this.conn_gate_svr();

    console.log("conn_gate_svr complete!");
    netSingleton.is_conn_gate = true;
  }
}
