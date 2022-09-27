using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using common;
using TinyFrameWork;
using GameCommon;
namespace Assets.Scripts.GameLogic.socket
{
    public class ChatRPC: imodule
    {
        public ChatRPC()
        {
            reg_event("player_chat_broadcast", player_chat_broadcast);
        }

       // private void 
        public void player_chat_broadcast(ArrayList info)
        {
            Int64 state = (Int64)info[0];
            string msg=(string)info[1];
            string targetUuid = (string)info[2];
            string uuid = (string)info[3];
            ArrayList data = new ArrayList();
            data.Add((ChatState)state);
            data.Add(msg);
            data.Add(targetUuid);
            data.Add(uuid);
            EventDispatcher.GetInstance().MainEventManager.TriggerEvent<ArrayList>(EventId.Server_Chat_msg, data);
        }
    }
}
