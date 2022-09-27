using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using common;
using TinyFrameWork;
namespace Assets.Scripts.GameLogic.socket
{
    public class BindAgent: imodule
    {
        public BindAgent()
        {
            reg_event("bind_sucess", bind_sucess);
            reg_event("bind_faild", bind_faild);
        }

        public void bind_sucess(ArrayList data)
        {
            string msg = "ok";
            EventDispatcher.GetInstance().MainEventManager.TriggerEvent<string>(EventId.Sever_Agent_bind, msg);
        }

        public void bind_faild(ArrayList data)
        {
            string msg = (string)data[0];
            EventDispatcher.GetInstance().MainEventManager.TriggerEvent<string>(EventId.Sever_Agent_bind, "已绑定成功，不能重复绑定");
        }
    }
}
