using System;
using System.Collections.Generic;
using TinyFrameWork;
using System.Collections;
public class SelfBaseData
{
    public ArrayList hasGetReward;
    public Int64 signCount;
    public Int64 taskVictoryCount;
    public Int64 redpacketsSendMoeny;
    public Int64 taskGameCount;
    public Int64 goldNum;
    public Int64 diamondNum;
    public ArrayList snatchRedIDList;
    public ArrayList sendRedIDList;
    public bool isSignToday;
    public string unionID;
    public Int64 redpacketSendTotal;
    public string name;
    public Int64 gameID;
    public Int64 sex;
    public string headUrl;
    public Int64 integral;
    public SelfBaseData(Hashtable table)
    {
        name = (string)table["nickname"];
        headUrl = (string)table["headimg"];
        sex = (Int64)table["sex"];
        UpdataData((Hashtable)table["player_info"]);
    }

    public void UpdataData(Hashtable table)
    {
        try
        {
            if (table.ContainsKey("has_get_reward"))
            {
                hasGetReward = (ArrayList)table["has_get_reward"];
            }
            if (table.ContainsKey("sign_count"))
            {
                signCount = (Int64)table["sign_count"];
            }
            if (table.ContainsKey("task_victory_count"))
            {
                taskVictoryCount = (Int64)table["task_victory_count"];
            }
            if (table.ContainsKey("task_game_count"))
            {
                taskGameCount = (Int64)table["task_game_count"];
            }
            if (table.ContainsKey("diamond"))
            {
                diamondNum = (Int64)table["diamond"];
            }
            if (table.ContainsKey("gold"))
            {
                goldNum = (Int64)table["gold"];
            }
            if (table.ContainsKey("snatch_red_id_time"))
            {
                snatchRedIDList = (ArrayList)table["snatch_red_id_time"];
            }
            if (table.ContainsKey("send_red_id_time"))
            {
                sendRedIDList = (ArrayList)table["send_red_id_time"];
            }
            if (table.ContainsKey("has_sign"))
            {
                isSignToday = (bool)table["has_sign"];
            }
            if (table.ContainsKey("unionid"))
            {
                unionID = (string)table["unionid"];
            }
            if (table.ContainsKey("redpackets_send_perday"))
            {
                redpacketSendTotal = (Int64)table["redpackets_send_perday"];
            }
            if (table.ContainsKey("reg_key"))
            {
                gameID = (Int64)table["reg_key"];
            }
            if (table.ContainsKey("rank_score"))
            {
                integral = (Int64)table["rank_score"];
            }
            // redpacketsSendMoeny = (Int64)table["redpackets_send_total"];
            // redpacketsSendMoeny = (Int64)table["redpackets_send_perday"];    

        }
        catch (Exception e)
        {
            EventDispatcher.GetInstance().UIFrameWorkEventManager.TriggerEvent<string>(EventId.UIFrameWork_Bug, e.Message);
        }
       
    }
}

