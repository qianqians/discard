using System;
using System.Collections.Generic;
using System.Text;

namespace http_admin
{
    /// <summary>
    /// 权限角色
    /// </summary>
    public class RoleEntity
    {
        public string key { set; get; }

        public string name { set; get; }

        public string description { set; get; }

        public List<string> routes { set; get; }

        public List<string> permissions { set; get; }

        public static RoleEntity ValueOf(RoleInfo roleInfo) {
            RoleEntity roleEntity = new RoleEntity();
            roleEntity.key = roleInfo.key;
            roleEntity.name = roleInfo.name;
            roleEntity.description = roleInfo.description;
            roleEntity.routes = roleInfo.routes;
            roleEntity.permissions = roleInfo.permissions;
            return roleEntity;
        }
    }
}
