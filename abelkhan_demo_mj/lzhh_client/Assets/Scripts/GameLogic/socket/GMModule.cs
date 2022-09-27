using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using common;
using TinyFrameWork;
using UnityEngine;
using System.Collections;
public class GMModule:imodule
{
    public GMModule()
    {
        reg_event("notice", notice);
        reg_event("set_pay_rate", set_pay_rate);
    }

    public void notice(ArrayList data)
    {
        EventDispatcher.GetInstance().MainEventManager.TriggerEvent<string>(EventId.Sever_NoticeMsg, (string)data[0]);
    }

    public void set_pay_rate(ArrayList data)
    {
        Int64 rate_index = (Int64)data[0];
        int[] payRateArr = new int[] { 0, 1, 2, 3 };
        MainManager.Instance.payRate = payRateArr[rate_index];
    }
}


