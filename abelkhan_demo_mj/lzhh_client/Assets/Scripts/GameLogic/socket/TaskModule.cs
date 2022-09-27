using common;
using System;
using UnityEngine;
using System.Collections;
using TinyFrameWork;
using System.Collections.Generic;
namespace Assets.Scripts.GameLogic.socket
{
    class TaskModule:imodule
    {
        public TaskModule()
        {
            reg_event("task_reward", task_reward);
        }
        public void task_reward(ArrayList data)
        {
            Int64 money= (Int64)data[0];
        }
    }
}
