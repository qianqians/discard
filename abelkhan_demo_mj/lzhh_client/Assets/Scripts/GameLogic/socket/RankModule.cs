using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using common;
using System.Collections;
using UnityEngine;
using TinyFrameWork;


public class RankModule:imodule
{
    public RankModule()
    {
        reg_event("refresh_red_rank_broadcast_TOP", refresh_red_rank_broadcast_TOP);
        reg_event("refresh_pay_rank_broadcast_TOP", refresh_pay_rank_broadcast_TOP);
    }

    public void refresh_red_rank_broadcast_TOP(ArrayList data)
    {
        // Debug.Log(rankLisData.Count + "红包"); 
        ArrayList rankLisData =(ArrayList)data[0];
        EventDispatcher.GetInstance().MainEventManager.TriggerEvent<ArrayList,Int64>(EventId.Lobby_RankList_Red_Bag, rankLisData, (Int64)RankListen.redBag);
    }

    public void refresh_pay_rank_broadcast_TOP(ArrayList data)
    {
        ArrayList rankLisData = (ArrayList)data[0];
        EventDispatcher.GetInstance().MainEventManager.TriggerEvent<ArrayList,Int64>(EventId.Lobby_RankList_Pay, rankLisData, (Int64)RankListen.pay);
    }


    //-------------------------------------------------------------------------------------------
}

