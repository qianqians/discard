using System;
using common;
using TinyFrameWork;
using System.Collections;
using System.Collections.Generic;
namespace Assets.Scripts.GameLogic.socket
{
    public class Login : imodule
    {
       // public static LoginState loginFlag = LoginState.Login_no;
        // public void login_sucess(string unionid, string name, string headimg, Int64 sex, Hashtable playerInfo, string roomName, Int64 roomID
        public Login()
        {
            reg_event("login_sucess", login_sucess);
            reg_event("other_login", other_login);
            reg_event("Set_local_login_info", Set_local_login_info);
            reg_event("Access_token_login", Access_token_login);
        }

        public void login_sucess(ArrayList data)
        {
            string unionid = (string)data[0];
            string name=(string)data[1];
            string headimg= (string)data[2];
            Int64 sex= (Int64)data[3];
            Hashtable playerInfo= (Hashtable)data[4];
            string roomName= (string)data[5];
            Int64 roomID= (Int64)data[6];
            Int64 payRate= (Int64)data[7];
            int[] payRateArr = new int[] {0,1,2,3 };
            Hashtable info = new Hashtable();
            info["unionid"] = unionid;
            info["nickname"] = name;
            info["headimg"] = headimg;
            info["sex"] = sex;
            info["site"] = (Int64)0;
            info["player_info"]= playerInfo;
            info["game_id"] = (Int64)playerInfo["reg_key"];
           
            if (playerInfo.Contains("agent_reg_key"))
            {
                MainManager.Instance.bindingID = (Int64)playerInfo["agent_reg_key"];
                if (MainManager.Instance.bindingID!=0)
                {
                    MainManager.Instance.payRate = payRateArr[1];
                }
                else
                {
                    MainManager.Instance.payRate = payRateArr[payRate];
                }
            }
            else
            {
                MainManager.Instance.payRate = payRateArr[payRate];
            }
            
            EventDispatcher.GetInstance().MainEventManager.TriggerEvent<Hashtable,string,Int64>(EventId.Sever_Login, info, roomName, roomID);
        }

        public void other_login(ArrayList data)
        {
           // NUMessageBox.Show("在别处上线了");
        }   
        
        public void Set_local_login_info(ArrayList data)
        {
            string access_token=(string)data[0];
            string refresh_token = (string)data[1];
            string uiid = (string)data[2];
            string openID= (string)data[3];
            List<string> info = new List<string>() {access_token , refresh_token , uiid , openID };
            EventDispatcher.GetInstance().MainEventManager.TriggerEvent<List<string>>(EventId.Sever_get_access_token, info); 
        }

        public void Access_token_login(ArrayList data)
        {
            bool state = (bool)data[0];
            string token = (string)data[1];
            EventDispatcher.GetInstance().MainEventManager.TriggerEvent<bool,string>(EventId.Sever_access_token_login, state, token); 
        }
    }
}


