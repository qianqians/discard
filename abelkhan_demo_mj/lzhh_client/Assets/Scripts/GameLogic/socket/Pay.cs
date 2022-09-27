using System;
using System.Collections.Generic;
using System.Linq;
using TinyFrameWork;
using common;
using System.Collections;
using UnityEngine;
public class Pay : imodule
{
    public Pay()
    {
        reg_event("prepay", prepay);
        reg_event("recharge_gold_ok", recharge_gold_ok);
        reg_event("recharge_diamond_ok", recharge_diamond_ok);
    }

    public void prepay(ArrayList data)
    {
        string prepay = (string)data[0];
        EventDispatcher.GetInstance().MainEventManager.TriggerEvent<string>(EventId.Server_Pay_msg, prepay);
    }

    public void recharge_gold_ok(ArrayList data)
    {
        bool flag = (bool)data[0];
        Int64 gold = (Int64)data[1];
        Int64 diamond = (Int64)data[2];
        if (flag)
        {
            NUMessageBox.Show("恭喜你用" + "<Color=#E7B40F><size=30>" + diamond + "</size></Color>" + "颗钻石成功兑换了" + "<Color=#E7B40F><size=30>" + gold + "</size></Color>" + "金币！");
        }
        else
        {
            NUMessageBox.Show("兑换失败！");
        }
    }

    public void recharge_diamond_ok(ArrayList data)
    {
        bool flag = (bool)data[0];
        Int64 gold = (Int64)data[1];
        Int64 diamond = (Int64)data[2];
        if (flag)
        {
            NUMessageBox.Show("恭喜兑换成功，已存入库存!");
        }
        else
        {
            NUMessageBox.Show("兑换失败!");
        }
    }
}

