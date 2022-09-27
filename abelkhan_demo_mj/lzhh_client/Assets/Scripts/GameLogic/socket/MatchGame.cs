using System;
using common;
using TinyFrameWork;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
public class MatchGame: imodule
{
    public MatchGame()
    {
        reg_event("on_match_mj_huanghuang_room", on_match_mj_huanghuang_room);
        reg_event("begin_match", begin_match);
    }
    /// <summary>
    /// 金币场
    /// </summary>
    public void on_match_mj_huanghuang_room(ArrayList data)
    {
        string hub_name = (string)data[0];
        Int64 room_id = (Int64)data[1];
        TableController.Instance.gameType = GameType.fangkaGame;
        SocketClient.Instance.EnterRoom(hub_name, room_id);
    }

    public void begin_match(ArrayList data)
    {
        MainManager.Instance.dontDestroyOnLoad.StartLoadingScene("InGame");
    }
    
}

