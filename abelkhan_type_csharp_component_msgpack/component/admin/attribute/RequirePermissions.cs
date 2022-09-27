using System;
using System.ComponentModel;
using System.Reflection;

namespace abelkhan.admin
{
    public class RequirePermissions : Attribute
    {
        public RequirePermissions(Permission _permission)
        {
            Permission = _permission;
        }

        /// <summary>
        ///  权限枚举
        /// </summary>
        public Permission Permission { set; get; }
    }

    public enum Permission {
        [Description("基础权限")]
        BASE = 0,
        [Description("用户管理")]
        USER = 1,
        [Description("权限管理")]
        ROLE = 2,
        [Description("操作日志")]
        OP_LOG = 3,
        [Description("gm操作")]
        GM = 4,
    }

    public enum RoleEnum {
        [Description("管理员")]
        admin = 0,
        [Description("运营主管")]
        mgr = 1,
        [Description("运营")]
        optor = 2,
        [Description("编辑")]
        editor = 3,
        [Description("访客")]
        visitor = 4,
    }
}
