using System;
using common;
using System.Collections;
using TinyFrameWork;
using System.Collections.Generic;
namespace Assets.Scripts.GameLogic.UI.Lobby
{
    class Room: imodule
    {
        public Room()
        {
            reg_event("on_create_mj_huanghuang_room", on_create_mj_huanghuang_room);
            reg_event("on_player_enter_mj_huanghuang_room", on_player_enter_mj_huanghuang_room);
            reg_event("on_enter_mj_huanghuang_room", on_enter_mj_huanghuang_room);
            reg_event("on_exit_mj_huanghuang_room", on_exit_mj_huanghuang_room);
            reg_event("vote_disband_room_state", vote_disband_room_state);
            reg_event("req_disband_vote", req_disband_vote);
            reg_event("disband", disband);
            reg_event("on_mj_huanghuang_occupat_site", on_mj_huanghuang_occupat_site);
            reg_event("mj_huanghuang_room_info", mj_huanghuang_room_info);
            reg_event("player_cards", player_cards);
            reg_event("player_mopai", player_mopai);
            reg_event("mj_huanghuang_room_is_full", mj_huanghuang_room_is_full);
            reg_event("exist_room", exist_room);
            reg_event("occupat_site", occupat_site);
            reg_event("player_not_in_room", player_not_in_room);
            reg_event("exit_room", exit_room);
            reg_event("disable_game", disable_game);
            reg_event("vote_disband_room_player_state", vote_disband_room_player_state);
         //   reg_event("on_match_mj_huanghuang_room", on_match_mj_huanghuang_room);
        }

        //private void on_match_mj_huanghuang_room(ArrayList data)
        //{
        //    string name = (string)data[0];
        //    Int64 room_id = (Int64)data[1];
        //    Debug.Log(name + "roomNam");          
        //}

        public void on_create_mj_huanghuang_room(ArrayList data)
        {
            string name = (string)data[0];
            Int64 room_id = (Int64)data[1];         
            SocketClient.Instance.EnterRoom(name, room_id);
        }

        public void on_player_enter_mj_huanghuang_room(ArrayList data)
        {
            Hashtable Hashtab = (Hashtable)data[0];
            EventDispatcher.GetInstance().MainEventManager.TriggerEvent<Hashtable>(EventId.OtherJoinRoom, Hashtab);
        }

        public void on_enter_mj_huanghuang_room(ArrayList data)
        {
            ArrayList list = (ArrayList)data[0];
            Int64 peopleNum = (Int64)data[1];
           // Int64 rule = (Int64)data[2];
            Int64 score = (Int64)data[2];
            Int64 times = (Int64)data[3];
            Int64 payRule = (Int64)data[4];
            Int64 beginTime = (Int64)data[5];
            CreatRoomData creatRoomInfo = new CreatRoomData();
            creatRoomInfo.playerNum = (int)peopleNum;
          //  creatRoomInfo.playState = (int)rule;
            creatRoomInfo.baseScore = (int)score;
            creatRoomInfo.jushu = (int)times;
            creatRoomInfo.payState = (int)payRule;
            creatRoomInfo.roomBeginTime = beginTime;
            EventDispatcher.GetInstance().MainEventManager.TriggerEvent<ArrayList, CreatRoomData>(EventId.SelfJoinRoom,list, creatRoomInfo);       
        }

        public void on_exit_mj_huanghuang_room(ArrayList data)
        {
            string token = (string)data[0];
            EventDispatcher.GetInstance().MainEventManager.TriggerEvent<string>(EventId.PlayerExitRoom, token);
        }
     
        public void vote_disband_room_state(ArrayList data)
        {
            string unionid = (string)data[0];
            Int64 state = (Int64)data[1];
            EventDispatcher.GetInstance().MainEventManager.TriggerEvent<string,Int64>(EventId.Server_Disband_Game_Vote, unionid, state);
        }

        public void req_disband_vote(ArrayList data)
        {
            EventDispatcher.GetInstance().MainEventManager.TriggerEvent(EventId.Server_Show_Disband_panel);
        }

        private void vote_disband_room_player_state(ArrayList data)
        {
            Hashtable room_info = (Hashtable)data[0];
            EventDispatcher.GetInstance().MainEventManager.TriggerEvent<Hashtable>(EventId.Server_Disband_Room_Vote_list, room_info);
        }

        public void disband(ArrayList data)
        {
            // Debug.Log("disband");
            //  EventDispatcher.GetInstance().MainEventManager.TriggerEvent<string>(EventId.PlayerExitRoom, token);
            EventDispatcher.GetInstance().UIFrameWorkEventManager.TriggerEvent(EventId.Server_Disband_Eng_Game);
        }
        
        public void on_mj_huanghuang_occupat_site(ArrayList data)
        {
            string token = (string)data[0];
            Int64 id = (Int64)data[1];
            EventDispatcher.GetInstance().MainEventManager.TriggerEvent<KeyValuePair<string, Int64>>(EventId.PlaySitDown, new KeyValuePair<string, Int64>(token, id));
        }

        //o
        public void mj_huanghuang_room_info(ArrayList data)
        {
            Hashtable room_mj_info = (Hashtable)data[0];
            ReconnectionData info = new ReconnectionData(room_mj_info);
            EventDispatcher.GetInstance().MainEventManager.TriggerEvent<ReconnectionData>(EventId.Sever_Reconnection, info);
        }

        /// <summary>
        /// 自己的手牌
        /// </summary>
        /// <param name="list"></param>
        public void player_cards(ArrayList data)
        {
            ArrayList list = (ArrayList)data[0];
            EventDispatcher.GetInstance().MainEventManager.TriggerEvent<ArrayList>(EventId.Sever_Reconnection_SelfHandcard, list);            
        }

        public void player_mopai(ArrayList data)
        {
            Int64 mjNum = (Int64)data[0];
            EventDispatcher.GetInstance().MainEventManager.TriggerEvent<Int64>(EventId.Sever_Reconnection_selfMopai_Num, mjNum);
        }              

        public void mj_huanghuang_room_is_full(ArrayList data)
        {
            MainManager.Instance.dontDestroyOnLoad.SetLoading(false);
            NUMessageBox.Show("房间已满");
        }

        public void exist_room(ArrayList data)
        {
            MainManager.Instance.dontDestroyOnLoad.SetLoading(false);

            if (MainManager.Instance.nowSceneName == SceneName.NUMainWindow)
            {
                NUMessageBox.Show("房间号不存在");
            }
            else
            {
                MainManager.Instance.dontDestroyOnLoad.StartLoadingScene("NUMainWindow");
            }                   
        }

        public void occupat_site(ArrayList data)
        {
            bool state = (bool)data[0];
            EventDispatcher.GetInstance().MainEventManager.TriggerEvent<bool>(EventId.SitDownError, state); 
        }

        public void player_not_in_room(ArrayList data)
        {
           // EventDispatcher.GetInstance().UIFrameWorkEventManager.TriggerEvent<string>(EventId.UIFrameWork_Bug, "player_not_in_room");
            MainManager.Instance.dontDestroyOnLoad.StartLoadingScene("NUMainWindow");
        }

        public void exit_room(ArrayList data)
        {
           // EventDispatcher.GetInstance().UIFrameWorkEventManager.TriggerEvent<string>(EventId.UIFrameWork_Bug, "exit_room");
            MainManager.Instance.dontDestroyOnLoad.StartLoadingScene("NUMainWindow");
        }

        /// <summary>
        /// 禁止进入房间
        /// </summary>
        public void disable_game(ArrayList data)
        {
            MainManager.Instance.dontDestroyOnLoad.SetLoading(false);
            NUMessageBox.Show("服务器即将重启，禁止创建游戏和进入房间！");
        }
      
    }
}
