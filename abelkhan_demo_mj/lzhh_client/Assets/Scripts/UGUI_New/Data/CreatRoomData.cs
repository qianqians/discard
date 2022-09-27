using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
public class CreatRoomData
{
    public int playerNum;
    public int playState;
    public int baseScore;
    public int jushu;
    public int payState;
    public int needMoney;
    public Int64 roomBeginTime;
    public string creatRoomPlayerUiid;
    public Int64 currentPlayerNum;
    public string roomID;
    public CreatRoomData()
    {
        playerNum = 0;
        playState = 0;
        baseScore = 0;
        jushu = 0;
        payState = 0;
        roomBeginTime = 0;
        currentPlayerNum = 0;
        roomID = "";
    }
}




