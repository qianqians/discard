using abelkhan;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Collections;
using abelkhan.admin.helper;


namespace http_admin
{

    public class UserMapper : BaseMapper
    {
        public override string CollectionName()
        {
            return "admin_user";
        }
        public UserMapper() { 
        }

        public string generateUid() {
            return System.Guid.NewGuid().ToString("N");
        }

        public Task<List<UserEntity>> GetUserEntityAsync(string query_json)
        {
            var t = new TaskCompletionSource<List<UserEntity>>();
            List<UserEntity> _alllist = new List<UserEntity>();
            GetCollection().getObjectInfo(query_json, (string str) => {
                List<UserEntity> _datalist = JSONHelper.deserialize<List<UserEntity>>(str);
                _alllist.AddRange(_datalist);
            }, () => t.TrySetResult(_alllist));
            return t.Task;
        }
    }
}
