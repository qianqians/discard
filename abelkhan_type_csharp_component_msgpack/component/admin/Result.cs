using System;
using System.ComponentModel;
using System.Reflection;
using abelkhan.admin.helper;

namespace abelkhan.admin
{
    public class Result<T>
    {
        public Code code { set; get; }

        public T data { set; get; }

        public Result(Code _code) {
            this.code = _code;
        }

        public Result(Code _code, T t)
        {
            this.code = _code;
            this.data = t;
        }

        public static Result<T> Success() {
            return new Result<T>(Code.OK);
        }

        public static Result<T> Res(T t)
        {
            return new Result<T>(Code.OK, t);
        }

        public static Result<T> Fail(Code _code)
        {
            return new Result<T>(_code);
        }

        public JSON ToJson() {
            JSON json = new JSON();
            json.Add("code", (int)code);
            json.Add("data", data);
            json.Add("errMessage", EnumHelper.GetDescription(code));
            return json;
        }
    }
    public enum Code : int
    {
        [Description("成功")]
        OK = 0,
        
        [Description("用户已登录无需重复登录")]
        USER_ALREADY_LOGIN = 1,
        [Description("用户未登录")]
        USER_NOT_LOGIN = 2,
        [Description("登录已超时")]
        USER_LOGIN_TIMEOUT = 3,
        [Description("用户删除失败")]
        USER_REMOVE_FAIL = 4,
        [Description("用户已存在")]
        USER_ALREADY_EXISTS = 5,
        [Description("用户不存在")]
        USER_NOT_EXISTS = 6,
        [Description("密码不正确")]
        USER_PASSWORD_ERROR = 7,
        [Description("AdminKey 错误")]
        USER_ADMIN_KEY_ERROR = 8,
        [Description("用户创建失败")]
        USER_CREATE_FAIL = 9,
        [Description("用户更新失败")]
        USER_UPDATE_FAIL = 10,
        [Description("只能修改自己信息")]
        USER_ONLY_UPDATE_SELF = 11,
        [Description("确认密码不相同")]
        USER_CONFIRM_NOT_EQUAL = 12,
        [Description("旧密码不相同")]
        USER_OLD_PWD_NOT_EQUAL = 13,

        // role角色权限 100~200
        [Description("角色权限已存在")]
        ROLE_ALREADY_EXISTS = 100,
        [Description("角色创建失败")]
        ROLE_CREATE_FAIL = 101,
        [Description("角色不存在")]
        ROLE_NOT_EXISTS = 102,
        [Description("角色删除失败")]
        ROLE_REMOVE_FAIL = 103,
        [Description("角色Key不合法")]
        ROLE_KEY_ERROR = 104,
        [Description("权限不足")]
        ROLE_PERMISSION_NOT_ENOUNGH = 105,
        [Description("权限不足-授权不能高于自己")]
        ROLE_PERMISSION_SELF_NOT_ENOUNGH = 105,

        // Gm 201~300
        [Description("Gm命令不存在")]
        GM_CMD_NOT_EXISTS = 201,
        [Description("Gm命令执行失败")]
        GM_CMD_DO_FAIL = 202,
    }
}
