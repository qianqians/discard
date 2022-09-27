using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace TinyFrameWork
{
    public enum HandleType
    {
        Add = 0,
        Remove = 1,
    }

    public class EventSystemDefine
    {
        public static Dictionary<int, string> dicHandleType = new Dictionary<int, string>()
        {
            { (int)HandleType.Add, "Add"},
            { (int)HandleType.Remove, "Remove"},
        };
    }

    public enum EventId
    {
        None = 0,
        // Test User Input
        TestUserInput,
        // UIFrameWork Event id
        PopRootWindowAdded,
        // Common Event id
        CoinChange,
        DiamondChange,
        // Player Common Event id
        PlayerHitByAI,
        // Net message
        NetUpdateMailContent,


        Sever_Login_Sucess,
        Sever_Login,
        Sever_get_access_token,
        Sever_access_token_login,
        Server_PlayerInfo_Updata,
        //joinRoom
        SelfJoinRoom,
        PlayerExitRoom,
        OtherJoinRoom,
        SitDownError,
        PlaySitDown,
        Server_HandCard,
        Order,
        AddOneCard,
        ShowLaizi,
        PlayCard,
        SelfCanPengOrGang,
        SelfCanPengOrGangbutCardDifference,
        WhoCanPeng,
        Server_PengCard,
        Server_GangCard,
        Server_GangScore,
        Server_HuPai,
        Server_HuPai_other_player_card,
        Server_HuScore,
        Server_draw,
        Server_Next_Game,
        Server_End_Game,
        Server_Disband_Game_Vote,
        Server_Disband_Room_Vote_list,
        Server_Disband_Eng_Game,
        Server_Show_Disband_panel,
        Server_Daily_Signin,
        Server_Chat_msg,
        Server_Pay_msg,
        Sever_Agent_bind,
        Sever_Reconnection,
        Sever_Reconnection_SelfHandcard,
        Sever_Reconnection_selfMopai_Num,
        Sever_NoticeMsg,
        Server_Disable_Join_Game,
        //红包
        Sever_Get_Red_Bag,
        Sever_Can_Rob_List,
        Sever_red_Player_List,//单个红包抢夺玩家列表
        Sever_get_red_Player_Info,//拉取单个红包相信信息
        Sever_refresh_red_rank_broadcast,
        Server_Room_list,      //  房间列表,

        Sever_player_off_Ling,
        UIFrameWork_last_mj,
        //UIFrameWorkEventManager事件参数（和游戏界面相关）
        UIFrameWork_Self_Join_Room,
        UIFrameWork_Player_Exit_Room,
        UIFrameWork_Player_Sit_down,
        UIFrameWork_Set_Voice_Active,
        UIFrameWork_Player_record,
        UIFrameWork_Player_Stand_UP,
        UIFrameWork_Deal_Card_First,
        UIFrameWork_Player_Draw_Card,
        UIFrameWork_Player_Out_Card,
        UIFrameWork_Player_Peng_Card,
        UIFrameWork_Player_Gang_Card,
        UIFrameWork_Change_Score,
        UIFrameWork_Hupai,
        UIFrameWork_Game_liuju,
        UIFrameWork_Game_Animation_Playover,
        UIFrameWork_Game_Current_Card_pos,
        UIFrameWork_Reconnection,
        UIFrameWork_Reconnection_Updata,
        UIFrameWork_Reconnection_IsHu,
        UIFrameWork_Control_settingPanel,
        UIFrameWork_Begin_Shot,
        UIFrameWork_Tingpai,
        UIFrameWork_Putout_Can_Tingpai,

        UIFrameWork_Click_beign_next_game,

        UIFrameWork_Effect_Prompt,
        UIFrameWork_Chose_Card,
        UIFrameWork_Show_Laizi,
        UIFrameWork_Cheng_laizi,

        UIFrameWork_Set_mj_back,
        UIFrameWork_Room_List_Change,
        UIFrameWork_Disband_room_Change,
        UIFrameWork_Bug,

        //游戏内同游戏动画相关
        InGame_SelectCardAnimFinsh,//选牌结束

        Lobby_RankList_Red_Bag,
        Lobby_RankList_Pay,
    }
}
