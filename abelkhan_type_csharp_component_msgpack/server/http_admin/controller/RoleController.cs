using abelkhan.admin;
using abelkhan.admin.helper;
using System.Collections.Generic;
using System;
using System.Linq;

namespace http_admin
{
    [Controller("/role")]
    public class RoleController : ActionSupport
    {
        [AutoWired]
        private RoleMapper roleMapper = null;

        [Post("/permissions")]
        public JSON GetPermissions()
        {
            PermissionList _list = new PermissionList();
            foreach (Permission eP in Enum.GetValues(typeof(Permission)))
            {
                _list.add(PermissionInfo.ValueOf(eP.ToString(), EnumHelper.GetDescription(eP)));
            }
            return Result<PermissionList>.Res(_list).ToJson();
        }

        [Post("/routes")]
        public JSON GetRoutes()
        {
            UserList list = new UserList();
            return Result<UserList>.Res(list).ToJson();
        }

        [Post("/list")]
        public JSON GetList()
        {
            RoleList list = new RoleList();
            List<RoleEntity> _dataList = roleMapper.GetRoleEntityAsync("{}").GetAwaiter().GetResult();
            foreach (RoleEntity item in _dataList)
            {
                list.add(RoleInfo.ValueOf(item));
            }
            return Result<RoleList>.Res(list).ToJson();
        }

        [Post("/add")]
        [RequirePermissions(Permission.ROLE)]
        public JSON AddRole()
        {
            RoleInfo newInfo = getParams<RoleInfo>();
            if (!Enum.GetNames(typeof(RoleEnum)).Contains(newInfo.key)) {
                return Result<string>.Fail(Code.ROLE_KEY_ERROR).ToJson();
            }
            
            uint count = roleMapper.GetObjectCountAsync("{key: \"" + newInfo.key + "\"}").GetAwaiter().GetResult();
            if (count > 0)
            {
                return Result<string>.Fail(Code.ROLE_ALREADY_EXISTS).ToJson();
            }

            RoleEntity roleEntity = RoleEntity.ValueOf(newInfo);
            bool isTrue = roleMapper.CreatePersistedObjectAsync(JSONHelper.serialize(roleEntity)).GetAwaiter().GetResult();
            if (!isTrue)
            {
                return Result<string>.Fail(Code.ROLE_CREATE_FAIL).ToJson();
            }
            return Result<string>.Success().ToJson();
        }

        [Post("/update")]
        [RequirePermissions(Permission.ROLE)]
        public JSON UpdateRole()
        {
            RoleInfo newInfo = getParams<RoleInfo>();
            uint count = roleMapper.GetObjectCountAsync("{key: \"" + newInfo.key + "\"}").GetAwaiter().GetResult();
            if (count <= 0)
            {
                return Result<string>.Fail(Code.ROLE_NOT_EXISTS).ToJson();
            }

            RoleEntity roleEntity = RoleEntity.ValueOf(newInfo);
            bool isTrue = roleMapper.UpdataPersistedObjectAsync("{key: \"" + newInfo.key + "\"}", JSONHelper.serialize(roleEntity)).GetAwaiter().GetResult();
            if (!isTrue)
            {
                return Result<string>.Fail(Code.ROLE_CREATE_FAIL).ToJson();
            }
            return Result<string>.Success().ToJson();
        }

        [Post("/delete")]
        [RequirePermissions(Permission.ROLE)]
        public JSON DeleteRole()
        {
            RemoveRoleParams removeInfo = getParams<RemoveRoleParams>();
            bool isTrue = roleMapper.RemoveObjectAsync("{key: \"" + removeInfo.key + "\"}").GetAwaiter().GetResult();
            if (!isTrue)
            {
                return Result<string>.Fail(Code.ROLE_REMOVE_FAIL).ToJson();
            }
            return Result<string>.Success().ToJson();
        }
    }
}
