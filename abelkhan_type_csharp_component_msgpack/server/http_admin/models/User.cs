using System;
using abelkhan.admin;
using abelkhan;
using System.Collections.Generic;
using System.Linq;
using abelkhan.admin.helper;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace http_admin
{
    public class UserLoginParam {
        public string username { set; get; }
        public string password { set; get; }
    }

    public class RemoveUserParams
    {
        public string uid { set; get; }
    }

    public class UpdatePwdParams
    {

        public string password { set; get; }

        public string oldPassword { set; get; }

        public string confirmPassword { set; get; }
    }

    public class CreateUserParams
    {
        public string uid { set; get; }
        public string name { set; get; }

        public string roles { set; get; }

        public string password { set; get; }

        public string avatar { set; get; }

        public string introduction { set; get; }

        public string adminKey { set; get; }

        public bool locked { set; get; }
    }

    public class UserLoginOK
    {
        public string token { set; get; }

        public UserLoginOK(string token) {
            this.token = token;
        }
    }

    public class UserList {

        public List<UserInfo> list;
        public int total;
        public UserList() {
            list = new List<UserInfo>();
        }
        public void add(UserInfo info) {
            list.Add(info);
            total = list.Count();
        }

        public static UserList valueOf(List<UserInfo> userList) {
            UserList list = new UserList();
            list.list = userList;
            return list;
        }
    }


    public class UserInfo
    {
        public string uid { set; get; }
        public string name { set; get; }

        public string roles { set; get; }

        public string avatar { set; get; }

        public string introduction { set; get; }

        public Int64 createTime { set; get; }

        public Int64 loginTime { set; get; }

        public bool locked { set; get; }
        public List<string> routes { set; get; }

        public static UserInfo valueOf(string name, string roles, string avatar, string introduction) {
            UserInfo info = new UserInfo();
            info.name = name;
            info.roles = roles;
            info.avatar = avatar;
            info.introduction = introduction;
            return info;
        }

        public static UserInfo valueOf(UserEntity entity) {
            UserInfo info = new UserInfo();
            info.name = entity.name;
            info.roles = entity.roles;
            info.avatar = entity.avatar;
            info.introduction = entity.introduction;
            info.createTime = entity.createTime;
            info.loginTime = entity.loginTime;
            info.uid = entity.uid;
            info.locked = entity.locked;
            return info;
        }

        public static UserInfo valueOf(UserEntity entity, List<string> routes)
        {
            UserInfo info = valueOf(entity);
            info.routes = routes;
            return info;
        }
    }
}
