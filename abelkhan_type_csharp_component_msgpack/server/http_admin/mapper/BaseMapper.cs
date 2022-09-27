using abelkhan;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Collections;
using abelkhan.admin.helper;

namespace http_admin
{
    public abstract class BaseMapper
    {

        public static string ADMIN_DB = "admin";

        public Collection GetCollection() {
            return http_admin._hub._dbproxyproxy.getCollection(ADMIN_DB, CollectionName());
        }

        public abstract string CollectionName();

        public Task<bool> CreatePersistedObjectAsync(string objectInfo)
        {
            var t = new TaskCompletionSource<bool>();
            GetCollection().createPersistedObject(objectInfo, s => t.TrySetResult(s));
            return t.Task;
        }

        public Task<bool> UpdataPersistedObjectAsync(string query_json, string updata_info)
        {
            var t = new TaskCompletionSource<bool>();
            GetCollection().updataPersistedObject(query_json, updata_info, s => t.TrySetResult(s));
            return t.Task;
        }

        public Task<uint> GetObjectCountAsync(string query_json)
        {
            var t = new TaskCompletionSource<uint>();
            GetCollection().getObjectCount(query_json, s => t.TrySetResult(s));
            return t.Task;
        }

        public Task<ArrayList> GetObjectInfoAsync(string query_json)
        {
            var t = new TaskCompletionSource<ArrayList>();
            ArrayList _alllist = new ArrayList();
            GetCollection().getObjectInfo(query_json, (string str) => {
                ArrayList _datalist = JSONHelper.deserialize<ArrayList>(str);
                _alllist.AddRange(_datalist);
            }, () => t.TrySetResult(_alllist));
            return t.Task;
        }

        public Task<bool> RemoveObjectAsync(string query_json)
        {
            var t = new TaskCompletionSource<bool>();
            GetCollection().removeObject(query_json, s => t.TrySetResult(s));
            return t.Task;
        }

        public Task<List<T>> GetEntityAsync<T>(string query_json)
        {
            var t = new TaskCompletionSource<List<T>>();
            List<T> _alllist = new List<T>();
            GetCollection().getObjectInfo(query_json, (string str) => {
                List<T> _datalist = JSONHelper.deserialize<List<T>>(str);
                _alllist.AddRange(_datalist);
            }, () => t.TrySetResult(_alllist));
            return t.Task;
        }

        public Task<List<T>> GetEntityAsyncEx<T>(string query_json, int skip, int limit)
        {
            var t = new TaskCompletionSource<List<T>>();
            List<T> _alllist = new List<T>();
            GetCollection().getObjectInfoEx(query_json, skip, limit, (string str) => {
                List<T> _datalist = JSONHelper.deserialize<List<T>>(str);
                _alllist.AddRange(_datalist);
            }, () => t.TrySetResult(_alllist));
            return t.Task;
        }
    }
}
