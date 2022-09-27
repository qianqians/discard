using common;
using System.Collections;
using System;
using System.Collections;
using TinyFrameWork;
namespace Assets.Scripts.GameLogic.socket
{
    class LobbyModule:imodule
    {
        public LobbyModule()
        {
            reg_event("red_packet_error", red_packet_error);
            reg_event("snatch_redpackets_broadcast", snatch_redpackets_broadcast);
            reg_event("snatch_redpackets_ok", snatch_redpackets_ok);
            reg_event("refresh_red_list", refresh_red_list);
            reg_event("redquest_red_info", redquest_red_info);
            reg_event("refresh_red_rank_broadcast", refresh_red_rank_broadcast);           
        }

        public void red_packet_error(ArrayList data)
        {
            Int64 info = (Int64)data[0];
            if (info == 2)
            {
                NUMessageBox.Show("发送红包有5秒间隔！");
            }
            else
            {
                NUMessageBox.Show("ERROR  编号:" + info.ToString());
            }
        }



        public void snatch_redpackets_broadcast(ArrayList data)
        {
            Int64 gold = (Int64)data[0];
            Int64 red_count = (Int64)data[1];
            string name = (string)data[2];
            string red_id = (string)data[3];
            string msg = (string)data[4];
            RedBagBaseInfo info = new RedBagBaseInfo(gold, red_count, name, red_id, msg);
            EventDispatcher.GetInstance().MainEventManager.TriggerEvent<RedBagBaseInfo>(EventId.Sever_Get_Red_Bag, info);
        }

        public void snatch_redpackets_ok(ArrayList data)
        {
            Int64 num = (Int64)data[0];
            ArrayList list = (ArrayList)data[1];
            if (num != 0)
            {
                NUMessageBox.Show("抢了" + num.ToString() + "金币，运气不错哦！");
            }
            EventDispatcher.GetInstance().MainEventManager.TriggerEvent<ArrayList>(EventId.Sever_red_Player_List, list);
        }

        /// <summary>
        /// 可抢红包列表
        /// </summary>
        /// <param name="list"></param>
        public void refresh_red_list(ArrayList data)
        {
            ArrayList list = (ArrayList)data[0];
            EventDispatcher.GetInstance().MainEventManager.TriggerEvent<ArrayList>(EventId.Sever_Can_Rob_List, list);
        }

        public void redquest_red_info(ArrayList data)
        {
            Hashtable red = (Hashtable)data[0];
            EventDispatcher.GetInstance().MainEventManager.TriggerEvent<Hashtable>(EventId.Sever_get_red_Player_Info, red); 
        }

        //
        public void refresh_red_rank_broadcast(ArrayList data)
        {
            ArrayList list = (ArrayList)data[0];
            EventDispatcher.GetInstance().MainEventManager.TriggerEvent<ArrayList>(EventId.Sever_refresh_red_rank_broadcast, list); 
        }
    }
}
