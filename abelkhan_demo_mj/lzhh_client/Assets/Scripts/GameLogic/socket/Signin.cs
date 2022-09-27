using System;
using common;
using System.Collections;
using TinyFrameWork;
public class Signin:imodule
{
    public Signin()
    {
        reg_event("Sign_success", Sign_success);
    }
    public void Sign_success(ArrayList data)
    {
        Int64 flag = (Int64)data[0];
        EventDispatcher.GetInstance().MainEventManager.TriggerEvent< Int64>(EventId.Server_Daily_Signin, flag);       
    }
}
