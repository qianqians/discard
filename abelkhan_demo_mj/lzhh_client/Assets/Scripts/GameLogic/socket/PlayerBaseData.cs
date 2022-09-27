using System;
using System.Collections.Generic;
using System.Linq;
using TinyFrameWork;
using common;
using System.Collections;
using UnityEngine;
namespace Assets.Scripts.GameLogic.socket
{
    class PlayerBaseData : imodule
    {
        public PlayerBaseData()
        {
            reg_event("update_player", update_player);
        }

        public void update_player(ArrayList data)
        {
            // Int64 gold = (Int64)info["diamond"];
            //   Debug.Log(gold.ToString()+"钻石变化");
            Hashtable info = (Hashtable)data[0];
            MainManager.Instance.playerSelfInfo.UpdataData(info);
            EventDispatcher.GetInstance().MainEventManager.TriggerEvent<Hashtable>(EventId.Server_PlayerInfo_Updata, info);
        }
    }
}
