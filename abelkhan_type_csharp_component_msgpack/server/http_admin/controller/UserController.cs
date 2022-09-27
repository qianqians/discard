using System;
using abelkhan.admin;
using abelkhan;
using System.Collections;
using System.Linq;
using abelkhan.admin.helper;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace http_admin
{
    [Controller("/user")]
    public class User : ActionSupport
    {
        [AutoWired]
        private UserMapper userMapper = null;
        [AutoWired]
        private RoleMapper roleMapper = null;

        [Post("/login")]
        public JSON login()
        {
            UserLoginParam param = getParams<UserLoginParam>();
            List<UserEntity> _dataList = userMapper.GetUserEntityAsync("{name: \"" + param.username + "\"}").GetAwaiter().GetResult();
            if (_dataList.Count <= 0) {
                return Result<string>.Fail(Code.USER_NOT_EXISTS).ToJson();
            }

            string md5key = MD5Helper.GetMD5(param.password);
            UserEntity _userEntity = _dataList[0];
            if (!_userEntity.password.Equals(md5key)) {
                return Result<string>.Fail(Code.USER_PASSWORD_ERROR).ToJson();
            }

            _userEntity.loginTime = timerservice.Tick;
            bool isTrue = userMapper.UpdataPersistedObjectAsync("{uid: \"" + _userEntity.uid + "\"}", JSONHelper.serialize(_userEntity)).GetAwaiter().GetResult();
            if (!isTrue)
            {
                return Result<string>.Fail(Code.USER_UPDATE_FAIL).ToJson();
            }

            string token = Session.SessionId;
            Session.Login(param.username);

            List<RoleEntity> _List = roleMapper.GetRoleEntityAsync("{key: \"" + _userEntity.roles + "\"}").GetAwaiter().GetResult();
            Session.Routes = GetRouteList(_List);
            Session.Permissions = GetPermissionList(_List);
            Session.Roles = _userEntity.roles;

            return Result<UserLoginOK>.Res(new UserLoginOK(token)).ToJson();
        }

        public List<string> GetRouteList(List<RoleEntity> _List)
        {
            if (_List.Count > 0)
            {
                RoleEntity roleEntity = _List[0];
                return roleEntity.routes;
            }
            else
            {
                return new List<string>();
            }
        }

        public List<string> GetPermissionList(List<RoleEntity> _List) {
            if (_List.Count > 0)
            {
                RoleEntity roleEntity = _List[0];
                return roleEntity.permissions;
            }
            else
            {
                return new List<string>();
            }
        }

        [Post("/logout")]
        public JSON logout()
        {
            Session.Logout();
            return Result<string>.Success().ToJson();
        }

        [Post("/info")]
        public JSON GetInfo()
        {

            UserLoginParam param = getParams<UserLoginParam>();
            List<UserEntity> _dataList = userMapper.GetUserEntityAsync("{name: \"" + Session.UserId + "\"}").GetAwaiter().GetResult();
            if (_dataList.Count <= 0)
            {
                return Result<string>.Fail(Code.USER_NOT_EXISTS).ToJson();
            }

            UserEntity _userEntity = _dataList[0];
            return Result<UserInfo>.Res(UserInfo.valueOf(_userEntity, Session.Routes)).ToJson();
        }

        [Post("/list")]
        public JSON GetList()
        {
            UserList list = new UserList();
            List<UserEntity> _dataList = userMapper.GetUserEntityAsync("{}").GetAwaiter().GetResult();
            foreach (UserEntity item in _dataList) {
                list.add(UserInfo.valueOf(item));
            }
            return Result<UserList>.Res(list).ToJson();
        }

        [Post("/update")]
        [RequirePermissions(Permission.USER)]
        public JSON UpdateUser()
        {
            CreateUserParams newInfo = getParams<CreateUserParams>();
            if (!Enum.GetNames(typeof(RoleEnum)).Contains(newInfo.roles))
            {
                return Result<string>.Fail(Code.ROLE_KEY_ERROR).ToJson();
            }

            List<UserEntity> _dataList = userMapper.GetUserEntityAsync("{uid: \"" + newInfo.uid + "\"}").GetAwaiter().GetResult();
            if (_dataList.Count <= 0)
            {
                return Result<string>.Fail(Code.USER_NOT_EXISTS).ToJson();
            }

            int roleValue = EnumHelper.getEnumValueByName(typeof(RoleEnum), newInfo.roles);
            int sessionRoleValue = EnumHelper.getEnumValueByName(typeof(RoleEnum), Session.Roles);
            if (sessionRoleValue > roleValue) {
                return Result<string>.Fail(Code.ROLE_PERMISSION_SELF_NOT_ENOUNGH).ToJson();
            }

            UserEntity _userEntity = _dataList[0];
            _userEntity.roles = newInfo.roles;
            _userEntity.introduction = newInfo.introduction;
            _userEntity.avatar = newInfo.avatar;
            _userEntity.locked = newInfo.locked;
            if (newInfo.password != null || "".Equals(newInfo.password))
            {
                string passwordKey = MD5Helper.GetMD5(newInfo.password);
                _userEntity.password = passwordKey;
            }
            
            bool isTrue = userMapper.UpdataPersistedObjectAsync("{uid: \"" + newInfo.uid + "\"}", JSONHelper.serialize(_userEntity)).GetAwaiter().GetResult();
            if (!isTrue)
            {
                return Result<string>.Fail(Code.USER_UPDATE_FAIL).ToJson();
            }
            return Result<string>.Success().ToJson();
        }

        [Post("/updateSelf")]
        public JSON UpdateUserSelf()
        {
            CreateUserParams newInfo = getParams<CreateUserParams>();
            List<UserEntity> _dataList = userMapper.GetUserEntityAsync("{name: \"" + newInfo.name + "\"}").GetAwaiter().GetResult();
            if (_dataList.Count <= 0)
            {
                return Result<string>.Fail(Code.USER_NOT_EXISTS).ToJson();
            }

            UserEntity _userEntity = _dataList[0];
            _userEntity.introduction = newInfo.introduction;
            if (!Session.UserId.Equals(_userEntity.name)) {
                return Result<string>.Fail(Code.USER_ONLY_UPDATE_SELF).ToJson();
            }
         
            bool isTrue = userMapper.UpdataPersistedObjectAsync("{uid: \"" + _userEntity.uid + "\"}", JSONHelper.serialize(_userEntity)).GetAwaiter().GetResult();
            if (!isTrue)
            {
                return Result<string>.Fail(Code.USER_UPDATE_FAIL).ToJson();
            }
            return Result<string>.Success().ToJson();
        }

        [Post("/updatePwd")]
        public JSON UpdatePwd()
        {
            UpdatePwdParams pwdInfo = getParams<UpdatePwdParams>();
            if (!pwdInfo.password.Equals(pwdInfo.confirmPassword)) {
                return Result<string>.Fail(Code.USER_CONFIRM_NOT_EQUAL).ToJson();
            }

            List<UserEntity> _dataList = userMapper.GetUserEntityAsync("{name: \"" + Session.UserId + "\"}").GetAwaiter().GetResult();
            if (_dataList.Count <= 0)
            {
                return Result<string>.Fail(Code.USER_NOT_EXISTS).ToJson();
            }

            UserEntity _userEntity = _dataList[0];
           
            string oldPasswordKey = MD5Helper.GetMD5(pwdInfo.oldPassword);
            if (!oldPasswordKey.Equals(_userEntity.password))
            {
                return Result<string>.Fail(Code.USER_OLD_PWD_NOT_EQUAL).ToJson();
            }
            string passwordKey = MD5Helper.GetMD5(pwdInfo.password);
            _userEntity.password = passwordKey;

            bool isTrue = userMapper.UpdataPersistedObjectAsync("{uid: \"" + _userEntity.uid + "\"}", JSONHelper.serialize(_userEntity)).GetAwaiter().GetResult();
            if (!isTrue)
            {
                return Result<string>.Fail(Code.USER_UPDATE_FAIL).ToJson();
            }
            return Result<string>.Success().ToJson();
        }

        [Post("/create")]
        [RequirePermissions(Permission.USER)]
        public JSON CreateUser()
        {
            CreateUserParams newInfo = getParams<CreateUserParams>();
            if (!Enum.GetNames(typeof(RoleEnum)).Contains(newInfo.roles))
            {
                return Result<string>.Fail(Code.ROLE_KEY_ERROR).ToJson();
            }

            int roleValue = EnumHelper.getEnumValueByName(typeof(RoleEnum), newInfo.roles);
            int sessionRoleValue = EnumHelper.getEnumValueByName(typeof(RoleEnum), Session.Roles);
            if (sessionRoleValue > roleValue)
            {
                return Result<string>.Fail(Code.ROLE_PERMISSION_SELF_NOT_ENOUNGH).ToJson();
            }

            uint count = userMapper.GetObjectCountAsync("{name: \"" + newInfo.name + "\"}").GetAwaiter().GetResult();
            if (count > 0) {
                return Result<string>.Fail(Code.USER_ALREADY_EXISTS).ToJson();
            }

            string uid = userMapper.generateUid();
            string passwordKey = MD5Helper.GetMD5(newInfo.password);
            UserEntity userEntity = UserEntity.valueOf(uid, newInfo.name, newInfo.roles, newInfo.avatar, newInfo.introduction, passwordKey);
            bool isTrue = userMapper.CreatePersistedObjectAsync(JSONHelper.serialize(userEntity)).GetAwaiter().GetResult();
            if (!isTrue)
            {
                return Result<string>.Fail(Code.USER_CREATE_FAIL).ToJson();
            }
            return Result<string>.Success().ToJson();
        }

        [Post("/createAdmin")]
        public JSON CreateUserAdmin()
        {
            CreateUserParams newInfo = getParams<CreateUserParams>();
          
            if (newInfo.adminKey == null || !newInfo.adminKey.Equals(http_admin.adminKey)) {
                return Result<string>.Fail(Code.USER_ADMIN_KEY_ERROR).ToJson();
            }

            uint count = userMapper.GetObjectCountAsync("{name: \"" + "admin" + "\"}").GetAwaiter().GetResult();
            if (count > 0)
            {
                return Result<string>.Fail(Code.USER_ALREADY_EXISTS).ToJson();
            }

            string uid = userMapper.generateUid();
            string passwordKey = MD5Helper.GetMD5(newInfo.password);
            UserEntity userEntity = UserEntity.valueOf(uid, "admin", "admin", "", "管理员", passwordKey);
            bool isTrue = userMapper.CreatePersistedObjectAsync(JSONHelper.serialize(userEntity)).GetAwaiter().GetResult();
            if (!isTrue)
            {
                return Result<string>.Fail(Code.USER_CREATE_FAIL).ToJson();
            }
            return Result<string>.Success().ToJson();
        }

        [Post("/delete")]
        [RequirePermissions(Permission.USER)]
        public JSON DeleteUser()
        {
            RemoveUserParams removeInfo = getParams<RemoveUserParams>();
            bool isTrue = userMapper.RemoveObjectAsync("{uid: \"" + removeInfo.uid + "\"}").GetAwaiter().GetResult();
            if (!isTrue) {
                return Result<string>.Fail(Code.USER_REMOVE_FAIL).ToJson();
            }
            return Result<string>.Success().ToJson();
        }
    }
}
