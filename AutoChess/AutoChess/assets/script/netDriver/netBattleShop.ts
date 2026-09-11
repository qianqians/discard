import * as cli from "../serverSDK/client_handle"
import * as common from "../serverSDK/common"
import * as error from "../serverSDK/error"

import * as player_login from "../serverSDK/ccallplayer"
import * as match from "../serverSDK/ccallmatch"

import * as match_c from "../serverSDK/matchcallc"

import { netSingleton } from "./netSingleton"
import { battle_revive_caller } from "../serverSDK/ccallplayer"

export class netBattleShop {
    public c_match : match.plan_caller;
    public c_revive : battle_revive_caller;

    public constructor() {
        this.c_match = new match.plan_caller(cli.cli_handle);
        this.c_revive = new battle_revive_caller(cli.cli_handle);
    }

    public buy(hub_name:string, shop_index:common.ShopIndex, index:number, role_index:number) {
        return this.c_match.get_hub(hub_name).buy(shop_index, index, role_index);
    }

    public move(hub_name:string, role_index1:number, role_index2:number) {
        return this.c_match.get_hub(hub_name).move(role_index1, role_index2);
    }

    public sale_role(hub_name:string, index:number) {
        return this.c_match.get_hub(hub_name).sale_role(index);
    }
    
    public refresh(hub_name:string) {
        return this.c_match.get_hub(hub_name).refresh();
    }

    public revive(hub_name:string) {
        return this.c_revive.get_hub(hub_name).battle_failed_back();
    }
}