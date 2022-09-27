using System;
using common;
using UnityEngine;
using System.Collections;
using TinyFrameWork;

namespace Assets.Scripts.GameLogic.socket
{
   public class MJHuan: imodule
    {
        public MJHuan()
        {
            reg_event("deal", deal);
            reg_event("lazi", lazi);
            reg_event("draw", draw);
            reg_event("right", right);
            reg_event("draw_pai", draw_pai);
            reg_event("play_card", play_card);
            reg_event("pengpai", pengpai);
            reg_event("gangscore", gangscore);
            reg_event("gangpai", gangpai);
            reg_event("hupai", hupai);
            reg_event("otherpai", otherpai);
            reg_event("laiyou", laiyou);
            reg_event("huscore", huscore);
            reg_event("liu_ju", liu_ju);
            reg_event("read", read);
            reg_event("last_card", last_card);
            reg_event("end_game", end_game);
            reg_event("player_disconnect", player_disconnect);
            reg_event("player_reconnect", player_reconnect);
        }

        public void deal(ArrayList data)
        {
            Int64 bankerID = (Int64)data[0];
            Int64 touzi = (Int64)data[1];
            ArrayList list = (ArrayList)data[2];
            EventDispatcher.GetInstance().MainEventManager.TriggerEvent< Int64,Int64,ArrayList > (EventId.Server_HandCard, bankerID, touzi, list);
        }

        //2
        public void lazi(ArrayList data)
        {
            Int64 laizipi = (Int64)data[0];
            Int64 laizi = (Int64)data[1];
            EventDispatcher.GetInstance().MainEventManager.TriggerEvent<Int64,Int64>(EventId.ShowLaizi, laizipi,laizi);
        }

        //1
        public void draw(ArrayList data)
        {
            Int64 mjInfo = (Int64)data[0];
            EventDispatcher.GetInstance().MainEventManager.TriggerEvent<Int64>(EventId.AddOneCard, mjInfo);
        }

        //3
        public void right(ArrayList data)
        {
            Int64 order = (Int64)data[0];
            EventDispatcher.GetInstance().MainEventManager.TriggerEvent<Int64>(EventId.Order, order);
        }

        public void draw_pai(ArrayList data)
        {
            Int64 orderID = (Int64)data[0];
            EventDispatcher.GetInstance().MainEventManager.TriggerEvent<Int64>(EventId.Server_draw, orderID);
        }

        /// <summary>
        /// 出牌广播
        /// </summary>
        /// <param name="card"></param>
        public void play_card(ArrayList data)
        {
            Int64 card = (Int64)data[0];
            EventDispatcher.GetInstance().MainEventManager.TriggerEvent<Int64>(EventId.PlayCard, card);
        }

        //_table.broadcast("mj_huanghuang", "pengpai", _table.processer, card);（碰牌广播）
        public void pengpai(ArrayList data)
        {
            Int64 playerID = (Int64)data[0];
            Int64 card = (Int64)data[1];
            EventDispatcher.GetInstance().UIFrameWorkEventManager.TriggerEvent<string>(EventId.UIFrameWork_Bug, "peng");
            EventDispatcher.GetInstance().MainEventManager.TriggerEvent<Int64,Int64>(EventId.Server_PengCard, playerID,card);
        }

        public void gangscore(ArrayList data)
        {
            Hashtable info = (Hashtable)data[0];
            EventDispatcher.GetInstance().MainEventManager.TriggerEvent<Hashtable>(EventId.Server_GangScore, info);
        }

        //_table.broadcast("mj_huanghuang", "gangpai", _table.processer, card);（杆牌广播）
        public void gangpai(ArrayList data)
        {
            Int64 playerID = (Int64)data[0];
            Int64 card = (Int64)data[1];
            Int64 gangType = (Int64)data[2];
            EventDispatcher.GetInstance().MainEventManager.TriggerEvent<Int64,Int64,Int64>(EventId.Server_GangCard, playerID, card, gangType);
        }
        //_table.broadcast("mj_huanghuang", "hupai", _table.player_cards[_table.card_righter]);（牌组）
        public void hupai(ArrayList data)
        {
            ArrayList list = (ArrayList)data[0];
            Int64 mopai = (Int64)data[1];
            Int64 seat = (Int64)data[2];
            EventDispatcher.GetInstance().MainEventManager.TriggerEvent<ArrayList,Int64, Int64>(EventId.Server_HuPai, list, mopai,seat);
        }

        public void otherpai(ArrayList data)
        {
            ArrayList list = (ArrayList)data[0];
            EventDispatcher.GetInstance().MainEventManager.TriggerEvent<ArrayList>(EventId.Server_HuPai_other_player_card, list);
        }

        public void laiyou(ArrayList data)//seat
        {
            Int64 state = (Int64)data[0];
            Int64 seat = (Int64)data[1];
            if (state == (Int64)EffectPrompt.yingshangyou)
            {
                state = 3;
            }
            if (seat == TableController.Instance.selfOrderIndex)
            {
                EventDispatcher.GetInstance().UIFrameWorkEventManager.TriggerEvent<EffectPrompt>(EventId.UIFrameWork_Effect_Prompt, (EffectPrompt)state);
            }         
        }

        public void huscore(ArrayList data)
        {
            Hashtable info = (Hashtable)data[0];
            EventDispatcher.GetInstance().MainEventManager.TriggerEvent<Hashtable>(EventId.Server_HuScore, info);
        }
        //_table.broadcast("mj_huanghuang", "huscore", score);（胡牌 分数）

        public void liu_ju(ArrayList data)
        {
            EventDispatcher.GetInstance().UIFrameWorkEventManager.TriggerEvent(EventId.UIFrameWork_Game_liuju);
        }

        /// <summary>
        /// 点了下一把的玩家座位号
        /// </summary>
        /// <param name="site"></param>
        public void read(ArrayList data)
        {
            Int64 site = (Int64)data[0];
            EventDispatcher.GetInstance().MainEventManager.TriggerEvent<Int64>(EventId.Server_Next_Game, site);
        }

        public void last_card(ArrayList data)
        {
            Int64 mj = (Int64)data[0];
            EventDispatcher.GetInstance().UIFrameWorkEventManager.TriggerEvent<Int64 >(EventId.UIFrameWork_last_mj,mj);
        }

        public void end_game(ArrayList data)
        {
            EventDispatcher.GetInstance().UIFrameWorkEventManager.TriggerEvent(EventId.Server_End_Game);
        }

        //掉线
        public void player_disconnect(ArrayList data)
        {
            string unionid = (string)data[0];
            EventDispatcher.GetInstance().MainEventManager.TriggerEvent<string,bool>(EventId.Sever_player_off_Ling, unionid,true);
        }
     //   player_disconnect
        //上线
        public void player_reconnect(ArrayList data)
        {
            string unionid = (string)data[0];
            EventDispatcher.GetInstance().MainEventManager.TriggerEvent<string,bool>(EventId.Sever_player_off_Ling, unionid,false);
        }

    }
}
