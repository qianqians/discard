using abelkhan;
using System;

namespace http_admin
{   
    /// <summary>
    /// 用户数据
    /// </summary>
    public class UserEntity
    {
        public string uid { set; get; }

        public string name { set; get; }

        public string roles { set; get; }

        public string avatar { set; get; }

        public string introduction { set; get; }

        public Int64 createTime { set; get; }

        public Int64 loginTime { set; get; }
        
        public string password { set; get; }

        public bool locked { set; get; }

        public static UserEntity valueOf(string uid, string name, string roles, string avatar, string introduction, string password)
        {
            UserEntity info = new UserEntity();
            info.uid = uid;
            info.name = name;
            info.roles = roles;
            info.avatar = avatar;
            info.introduction = introduction;
            info.password = password;
            info.createTime = timerservice.Tick;
            info.loginTime = timerservice.Tick;
            info.locked = false;
            return info;
        }
    }
}
