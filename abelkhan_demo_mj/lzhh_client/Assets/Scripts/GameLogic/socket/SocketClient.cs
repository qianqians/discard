using System;
using System.Threading;
using UnityEngine;
using GameCommon;
using Assets.Scripts.GameLogic.UI.Lobby;
using Assets.Scripts.GameLogic.socket;
using TinyFrameWork;
using System.Collections;
using System.Collections.Generic;
using service;
namespace Assets.Scripts
{
    class SocketClient
    {     
        private static SocketClient instance;
        private Int64 _tick;
        private Int64 _tickcount;
        private client.client _client;
        //  private string serverIp = "139.199.11.173";
        //private short portIp = 3236;
        //private short udpPortIp = 3237;139.129.96.47
        //private string serverIp = "139.199.10.158";
        //private string serverIp = "139.129.96.47";
        private string serverIp = "111.230.47.215";
        private short portIp = 3236;
        public Boolean isGetHub;
        public Boolean isGetRoom1;
        public Boolean isGetRoom2;
        public Boolean isGetRoom3;
        public Boolean isGetRoom4;
        public string playToken;
        public Int64 roomID;
        private string hubName;

        private List<short> portIpList = new List<short> { 3236, 3246, 3256, 3266 };
        private List<string> roomList = new List<string> { "room1", "room2", "room3", "room4" };
        public static SocketClient Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SocketClient();
                }
                return instance;
            }
        }

        /// <summary>
        /// client
        /// </summary>
        public client.client NetHandle
        {
            get
            {
                return _client;
            }
        }

        public void LoginByPC(string code)
        {
            playToken = code;
            NetHandle.call_hub("lobby", "login", "player_login_account", playToken);
        }

        public void Login(string code)
        {
          //  _client.disable_heartbeats();
            NetHandle.call_hub("lobby", "login", "player_login", code);
        }
        //access_token
        public void LoginUseAccessToken(string access_token,string refreshToken, string unionid,string openid)
        {
          //  _client.disable_heartbeats();
          //  EventDispatcher.GetInstance().UIFrameWorkEventManager.TriggerEvent<string>(EventId.UIFrameWork_Bug, "disable_heartbeats");
            NetHandle.call_hub("lobby", "login", "player_login_token", access_token, refreshToken, unionid, openid);
        }

        /// <summary>
        /// 创建房间
        /// </summary>
        public void CreateRoom(int num,int rule,int score,int times,int pay)
        {
            MainManager.Instance.dontDestroyOnLoad.SetLoading(true);
            NetHandle.call_hub("lobby", "lobby", "create_mj_huanghuang_room", (Int64)num, (Int64)score, (Int64)times, (Int64)pay);
        }

        public void CreateRoomRobot()
        {
            MainManager.Instance.dontDestroyOnLoad.SetLoading(true);
            NetHandle.call_hub("lobby", "lobby", "create_mj_huanghuang_robot_room");
        }

        public void EnterRoom(string name, Int64 room_id)
        {
            hubName = name;
            roomID = room_id;
            NetHandle.call_hub(name, "room", "enter_mj_huanghuang_room", roomID);
        }

        public void JoinRoom(Int64 room_id)
        {
            Int64 index;
          //  hubName = roomList[(int)index-1];
           // roomID = room_id;
            if (room_id<100000)
            {
                NUMessageBox.Show("输入的房间号应该是6位");
            }
            else
            {
                index = room_id / 100000;
                if (index<= roomList.Count)
                {
                    hubName = roomList[(int)index - 1];
                    roomID = room_id;
                    MainManager.Instance.dontDestroyOnLoad.SetLoading(true);
                    NetHandle.call_hub(hubName, "room", "enter_mj_huanghuang_room", roomID);
                }
                else
                {
                    NUMessageBox.Show("输入的房间号不存在");                
                }
            }         
        }

        public void EnteRoomReconnect(string name,Int64 room_id)
        {
            hubName = name;
            roomID = room_id;
            MainManager.Instance.dontDestroyOnLoad.SetLoading(true);
            NetHandle.call_hub(name, "room", "reconnect_enter_mj_huanghuang_room", room_id); 
        }

        public void ChoseDir(Int64 sit)
        {        
            NetHandle.call_hub(hubName, "room", "mj_huanghuang_occupat_site", roomID, sit);
        }

        public void ExitRoom()
        {
            MainManager.Instance.dontDestroyOnLoad.SetLoading(true);
            NetHandle.call_hub(hubName, "room", "exit_mj_huanghuang_room", roomID);
        }

        public void VoteDisbandRoom(Int64 state)
        {
            NetHandle.call_hub(hubName, "room", "vote_disband_room", roomID,state);
        }

        public void ReqDisbandRoom()
        {
            NetHandle.call_hub(hubName, "room", "req_disband_room", roomID);
        }

        public void PlayerChat(ChatState state, string id,string tag_uuid ="")
        {
            NetHandle.call_hub(hubName, "chat", "player_chat", roomID,(Int64)state,id, tag_uuid);
        }

        //////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// 出牌
        /// </summary>
        /// <param name="card"></param>
        public void PlayCard(Int64 card)
        {
            NetHandle.call_hub(hubName, "mj_huanghuang", "play", roomID, card);
        }

        //public void guo(Int64 room_id)
        public void PassCard()
        {
            NetHandle.call_hub(hubName, "mj_huanghuang", "guo", roomID);
        }

        public void PengCard(Int64 ID)
        {
            NetHandle.call_hub(hubName, "mj_huanghuang", "peng", roomID,ID);
        }

        public void GangCard(Int64 ID)
        {
            NetHandle.call_hub(hubName, "mj_huanghuang", "gang", roomID, ID);
        }
        //public void hu(Int64 room_id)
        //public void Hu()
        //{
        //    NetHandle.call_hub(hubName, "mj_huanghuang", "hu", roomID,TableController.Instance.selfGetCardID);
        //}

        public void SendInteractive(string funcName,Int64 cardID = 0)
        {
            if (cardID==0)
            {
                NetHandle.call_hub(hubName, "mj_huanghuang", funcName, roomID);
            }
            else
            {
                NetHandle.call_hub(hubName, "mj_huanghuang", funcName, roomID, cardID);
            }
        }

        public void ReadNextGame()
        {
            NetHandle.call_hub(hubName, "mj_huanghuang", "read", roomID);
        }

        //支付
        public void PlayerPay(Int64 money)
        {
            NetHandle.call_hub("lobby", "pay", "player_prepay", money, Network.player.ipAddress);
        }

        //-------------


        public void PlayerPayForDiamond(Int64 gold)
        {
            NetHandle.call_hub("lobby", "pay", "gold_for_diamond", gold);
        }

        //-------------

        /// <summary>
        /// 钻石兑金币
        /// </summary>
        /// <param name="money"></param>
        public void PlayerPayForGold(Int64 diamond)
        {
            NetHandle.call_hub("lobby", "pay", "diamond_for_gold", diamond);
        }

        public void PlayerPaySuccess()
        {
            NetHandle.call_hub("lobby", "pay", "player_pay");
        }
        
        //绑定推荐人
        public void BindRecommend(Int64 id)
        {
            NetHandle.call_hub("lobby", "agent", "bind_agent", id);
        }

        //签到
        public void Singn()
        {
            NetHandle.call_hub("lobby", "signin", "player_signin");
        }

        //领取任务
        public void GetReward(string ID)
        {
            NetHandle.call_hub("lobby", "task", "get_reward",ID);           
        }

        //发红包
        public void SendRedBag(int gold,int count,string msg)
        {
            NetHandle.call_hub("lobby", "redpackets", "send_redpackets", (Int64)gold, (Int64)count, msg);
        }

        /// <summary>
        /// 抢红包
        /// </summary>
        /// <param name="str"></param>
        public void RobRedBag(string redid)
        {
            NetHandle.call_hub("lobby", "redpackets", "snatch_redpackets", redid);
        }

        /// <summary>
        /// 拉取当前红包列表，6个
        /// </summary>
        public void RequestRefreshList()
        {
            NetHandle.call_hub("lobby", "redpackets", "request_refresh_list");
        }

        public void GetRedBagInfoById(string redid)
        {
            NetHandle.call_hub("lobby", "redpackets", "send_red_info", redid);
        }

        public void GetLoginType()
        {
            NetHandle.call_hub("lobby", "login", "receive_rank_info");
        }

        public void JoinMatchRoom(Int64 rule, Int64 score)
        {
            NetHandle.call_hub("lobby", "match", "join_match"); 
        }

        public void Destory()
        {
            isGetHub = false;
            isGetRoom1 = false;
            isGetRoom2 = false;
            isGetRoom3 = false;
            isGetRoom4 = false;
            //_client.onConnectHub -= onConnectHub;
            //_client.onConnectGate -= onGeteHandle;
            //  _client = null;
            // _tickcount = 0;
        }

        public void Init()
        {
            _client = new client.client();
            Login _login = new Login();
            Room _room = new Room();
            MJHuan _MJhuan = new MJHuan();
            ChatRPC chat = new ChatRPC();
            PlayerBaseData PlayerData = new PlayerBaseData();
            Pay plPay = new Pay();
            BindAgent bind = new BindAgent();
            Signin sign = new Signin();
            TaskModule task = new TaskModule();
            LobbyModule lobby = new LobbyModule();
            GMModule gmm = new GMModule();
            RankModule rank = new RankModule();
            MatchGame matchGame = new MatchGame();

            _client.modulemanager.add_module("login", _login);
            _client.modulemanager.add_module("room", _room);
            _client.modulemanager.add_module("mj_huanghuang", _MJhuan);
            _client.modulemanager.add_module("player_data", PlayerData);
            _client.modulemanager.add_module("chat", chat);
            _client.modulemanager.add_module("pay", plPay);
            _client.modulemanager.add_module("agent", bind);
            _client.modulemanager.add_module("signin", sign);
            _client.modulemanager.add_module("task", task);
            _client.modulemanager.add_module("redpackets", lobby);
            _client.modulemanager.add_module("gm", gmm);
            _client.modulemanager.add_module("rank_msg", rank);
            _client.modulemanager.add_module("match", matchGame);
            _tick = Environment.TickCount;
            _client.onConnectHub += onConnectHub;
            _client.onConnectGate += onGeteHandle;
            log.log.setLogHandle(brokenLine);

            System.Random ran = new System.Random();
            int index = ran.Next(4);
            short portIP = portIpList[index];
            try
            {
                if (_client.connect_server(serverIp, portIP, serverIp, (short)(portIP+1), _tick))
                {
                   // EventDispatcher.GetInstance().UIFrameWorkEventManager.TriggerEvent<string>(EventId.UIFrameWork_Bug, "connect_server_true");
                }
                else
                {
                   // EventDispatcher.GetInstance().UIFrameWorkEventManager.TriggerEvent<string>(EventId.UIFrameWork_Bug, "connect_server_false");
                } 
            }
            catch (Exception e)
            {
               // EventDispatcher.GetInstance().UIFrameWorkEventManager.TriggerEvent<string>(EventId.UIFrameWork_Bug, e.Message);
            }
            //EventDispatcher.GetInstance().UIFrameWorkEventManager.TriggerEvent<string>(EventId.UIFrameWork_Bug, "connect_server_no_erro!");
            _tickcount = 0;
        }

        private void OnDisCaondctTips()
        {
            MainManager.Instance.dontDestroyOnLoad.BreakOnline();
            //  NUMessageBox.Show("您已经断线了！", OnlineBroken, null, true);
        }

        private void brokenLine(string str)
        {
            EventDispatcher.GetInstance().UIFrameWorkEventManager.TriggerEvent<string>(EventId.UIFrameWork_Bug, str);
            //  MainManager.Instance.dontDestroyOnLoad.debugCallBack(str);
        }
        private void OnlineBroken(NUMessageBox.CallbackType cbt)
        {
          //  EventDispatcher.GetInstance().UIFrameWorkEventManager.TriggerEvent<string>(EventId.UIFrameWork_Bug, "duanxin1");
            MainManager.Instance.dontDestroyOnLoad.BreakOnline();
        }
     
        public void onGeteHandle()
        {
          //  EventDispatcher.GetInstance().UIFrameWorkEventManager.TriggerEvent<string>(EventId.UIFrameWork_Bug, "onGeteHandle");
            try
            {
                _client.connect_hub("lobby");              
            }
            catch (Exception e)
            {
              //  EventDispatcher.GetInstance().UIFrameWorkEventManager.TriggerEvent<string>(EventId.UIFrameWork_Bug, "lobby"+e.Message);
            }

            try
            {
                _client.connect_hub("room1");
                _client.connect_hub("room2");
                _client.connect_hub("room3");
                _client.connect_hub("room4");
            }
            catch (Exception e)
            {
              //  EventDispatcher.GetInstance().UIFrameWorkEventManager.TriggerEvent<string>(EventId.UIFrameWork_Bug, "room1" + e.Message);
            }
            _client.onDisConnect += OnDisCaondctTips;
            _client.enable_heartbeats();
        }

        private void onConnectHub(string hub_name)
        {
            switch (hub_name)
            {
                case "lobby":
                    isGetHub = true;
                    break;
                case "room1":
                    isGetRoom1 = true;
                    break;
                case "room2":
                    isGetRoom2 = true;
                    break;
                case "room3":
                    isGetRoom3 = true;
                    break;
                case "room4":
                    isGetRoom4 = true;
                    break;
            }
          //  EventDispatcher.GetInstance().UIFrameWorkEventManager.TriggerEvent<string>(EventId.UIFrameWork_Bug, hub_name);
            // MainManager.Instance.dontDestroyOnLoad.debugCallBack(hub_name);
            if (isGetHub && isGetRoom1 && isGetRoom2&& isGetRoom3&& isGetRoom4)
            {
                EventDispatcher.GetInstance().MainEventManager.TriggerEvent(EventId.Sever_Login_Sucess);
            }
        }

        /// <summary>
        /// 掉线后再连接服务器
        /// </summary>
        public void OnReconnectServer()
        {
            System.Random ran = new System.Random();
            int index = ran.Next(4);
            short portIP = portIpList[index];
            _tick = service.timerservice.Tick;
            _client.reconnect_server(serverIp, portIP, serverIp, (short)(portIP + 1), _tick);

            //_client.onConnectHub -= onConnectHub;
            //_client.onConnectGate -= onGeteHandle;
            //_client.onDisConnect -= OnDisCaondctTips;
            //_client = null;
            //_tickcount = 0;
            //Init();

        }

        public void Update()
        {
            Int64 tmptick = (Environment.TickCount & UInt32.MaxValue);
            if (tmptick < _tick)
            {
                _tickcount += 1;
                tmptick = tmptick + _tickcount * UInt32.MaxValue;
            }
            _tick = tmptick;

            try
            {
                _client.poll();
               // Debug.Log(_tick+"TINK");
            }
            catch (Exception e)
            {
                EventDispatcher.GetInstance().UIFrameWorkEventManager.TriggerEvent<string>(EventId.UIFrameWork_Bug, e.Message);
            }
           
            tmptick = (Environment.TickCount & UInt32.MaxValue);
            if (tmptick < _tick)
            {
                _tickcount += 1;
                tmptick = tmptick + _tickcount * UInt32.MaxValue;
            }
            Int64 ticktime = (tmptick - _tick);
            _tick = tmptick;

            if (ticktime < 50)
            {
                Thread.Sleep(15);
            }
        }
    }
}
