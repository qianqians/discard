using System;
using System.Collections.Generic;
using System.Text;

namespace http_admin
{
    public class RemoveRoleParams
    {
        public string key { set; get; }
    }

    public class PermissionInfo
    {
        public string des { set; get; }
        public string name { set; get; }

        public static PermissionInfo ValueOf(string name, string des)
        {
            PermissionInfo info = new PermissionInfo();
            info.des = des;
            info.name = name;
            return info;
        }
    }

    public class PermissionList
    {
        public List<PermissionInfo> list;

        public int total;
        public PermissionList()
        {
            list = new List<PermissionInfo>();
        }
        public void add(PermissionInfo info)
        {
            list.Add(info);
            total = list.Count;
        }

        public static PermissionList valueOf(List<PermissionInfo> _list)
        {
            PermissionList list = new PermissionList();
            list.list = _list;
            return list;
        }
    }

    public class RoleInfo
    {
        public string key { set; get; }

        public string name { set; get; }

        public string description { set; get; }

        public List<string> routes { set; get; }

        public List<string> permissions { set; get; }

        public static RoleInfo ValueOf(RoleEntity roleEntity)
        {
            RoleInfo roleInfo = new RoleInfo();
            roleInfo.key = roleEntity.key;
            roleInfo.name = roleEntity.name;
            roleInfo.description = roleEntity.description;
            roleInfo.routes = roleEntity.routes;
            roleInfo.permissions = roleEntity.permissions;
            return roleInfo;
        }
    }

    public class RoleList
    {
        public List<RoleInfo> list;

        public int total;
        public RoleList()
        {
            list = new List<RoleInfo>();
        }
        public void add(RoleInfo info)
        {
            list.Add(info);
            total = list.Count;
        }

        public static RoleList valueOf(List<RoleInfo> userList)
        {
            RoleList list = new RoleList();
            list.list = userList;
            return list;
        }
    }
}
