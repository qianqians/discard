/*
 * dbproxyproxy
 * qianqians
 * 2020/6/4
 */

using System;
using System.Collections.Generic;

namespace abelkhan
{
    public class dbproxyproxy
    {
        public hub_call_dbproxy_caller _hub_call_dbproxy_caller;
        
        public Dictionary<string, Action<bool> > create_callback;
        public Dictionary<string, Action<bool> > update_callback;
        public Dictionary<string, Action<uint> > count_callback;
        public Dictionary<string, Action<string> > obj_callback;
        public Dictionary<string, Action> obj_end_callback;
        public Dictionary<string, Action<bool> > remove_callback;

        private Collection _collection;

        public dbproxyproxy(abelkhan.Ichannel ch, modulemng modules)
        {
            _hub_call_dbproxy_caller = new hub_call_dbproxy_caller(ch, modules);
            _collection = new Collection("_", "_", this);

            create_callback = new Dictionary<string, Action<bool> >();
            update_callback = new Dictionary<string, Action<bool> >();
            count_callback = new Dictionary<string, Action<uint> >();
            obj_callback = new Dictionary<string, Action<string> >();
            obj_end_callback = new Dictionary<string, Action>();
            remove_callback = new Dictionary<string, Action<bool> >();
        }

        public void on_closed()
        {
            _hub_call_dbproxy_caller = null;
        }

        public void reset(abelkhan.Ichannel ch, modulemng modules)
        {
            _hub_call_dbproxy_caller = new hub_call_dbproxy_caller(ch, modules);
            _collection.on_reset_dbproxy();
        }

        public void reg_hub(string name)
        {
            _hub_call_dbproxy_caller.reg_hub(name);
        }

        public Collection getCollection(string db, string collection)
        {
            _collection.set_db_collection(db, collection);
            return _collection;
        }
    }

    public class Collection
    {
        private string _db;
        private string _collection;
        private dbproxyproxy _dbproxy;

        public Collection(string db, string collection, dbproxyproxy dbproxy)
        {
            _db = db;
            _collection = collection;
            _dbproxy = dbproxy;

            create_obj_list = new List<create_obj>();
        }

        public void set_db_collection(string db, string collection)
        {
            _db = db;
            _collection = collection;
        }

        public void on_reset_dbproxy()
        {
            foreach (var obj in create_obj_list)
            {
                _dbproxy._hub_call_dbproxy_caller.create_persisted_object(obj.db, obj.collection, obj.object_info, obj.callbackid);
            }

            foreach(var obj in updat_obj_list)
            {
                _dbproxy._hub_call_dbproxy_caller.updata_persisted_object(obj.db, obj.collection, obj.query_json, obj.updata_info, obj.callbackid);
            }

            foreach(var obj in count_obj_list)
            {
                _dbproxy._hub_call_dbproxy_caller.get_object_count(obj.db, obj.collection, obj.query_json, obj.callbackid);
            }

            foreach (var obj in get_obj_obj_list)
            {
                _dbproxy._hub_call_dbproxy_caller.get_object_info(obj.db, obj.collection, obj.query_json, obj.callbackid);
            }

            foreach (var obj in get_objex_obj_list)
            {
                _dbproxy._hub_call_dbproxy_caller.get_object_infoex(obj.db, obj.collection, obj.query_json, obj.skip, obj.limit, obj.callbackid);
            }

            foreach (var obj in remove_obj_list)
            {
                _dbproxy._hub_call_dbproxy_caller.remove_object(obj.db, obj.collection, obj.query_json, obj.callbackid);
            }
        }

        struct create_obj
        {
            public string db;
            public string collection;
            public string object_info;
            public string callbackid;
        }
        private List<create_obj> create_obj_list;
        public void createPersistedObject(string _object_info, Action<bool> _handle)
        {
            var _callbackid = System.Guid.NewGuid().ToString("N");
            if (_dbproxy._hub_call_dbproxy_caller != null)
            {
                _dbproxy._hub_call_dbproxy_caller.create_persisted_object(_db, _collection, _object_info, _callbackid);
            }
            else
            {
                create_obj_list.Add(new create_obj() { db = _db, collection = _collection, object_info = _object_info, callbackid = _callbackid });
            }
            _dbproxy.create_callback.Add(_callbackid, _handle);
        }

        struct updat_obj
        {
            public string db;
            public string collection;
            public string query_json;
            public string updata_info;
            public string callbackid;
        }
        private List<updat_obj> updat_obj_list;
        public void updataPersistedObject(string _query_json, string _updata_info, Action<bool> _handle)
        {
            var _callbackid = System.Guid.NewGuid().ToString("N");
            if (_dbproxy._hub_call_dbproxy_caller != null)
            {
                _dbproxy._hub_call_dbproxy_caller.updata_persisted_object(_db, _collection, _query_json, _updata_info, _callbackid);
            }
            else
            {
                updat_obj_list.Add(new updat_obj() { db = _db, collection = _collection, query_json = _query_json, updata_info = _updata_info, callbackid = _callbackid });
            }
            _dbproxy.update_callback.Add(_callbackid, _handle);
        }

        struct count_obj
        {
            public string db;
            public string collection;
            public string query_json;
            public string callbackid;
        }
        private List<count_obj> count_obj_list;
        public void getObjectCount(string _query_json, Action<uint> _handle){
            var _callbackid = System.Guid.NewGuid().ToString("N");
            if (_dbproxy._hub_call_dbproxy_caller != null)
            {
                _dbproxy._hub_call_dbproxy_caller.get_object_count(_db, _collection, _query_json, _callbackid);
            }
            else
            {
                count_obj_list.Add(new count_obj() { db = _db, collection = _collection, query_json = _query_json, callbackid = _callbackid });
            }
            _dbproxy.count_callback.Add(_callbackid, _handle);
        }

        struct get_obj_obj
        {
            public string db;
            public string collection;
            public string query_json;
            public string callbackid;
        }
        private List<get_obj_obj> get_obj_obj_list;
        public void getObjectInfo(string _query_json, Action<string> _handle, Action _end){
            var _callbackid = System.Guid.NewGuid().ToString("N");
            if (_dbproxy._hub_call_dbproxy_caller != null)
            {
                _dbproxy._hub_call_dbproxy_caller.get_object_info(_db, _collection, _query_json, _callbackid);
            }
            else
            {
                get_obj_obj_list.Add(new get_obj_obj() { db = _db, collection = _collection, query_json = _query_json, callbackid = _callbackid });
            }
            _dbproxy.obj_callback.Add(_callbackid, _handle);
            _dbproxy.obj_end_callback.Add(_callbackid, _end);
        }

        struct get_objex_obj
        {
            public string db;
            public string collection;
            public string query_json;
            public int skip;
            public int limit;
            public string callbackid;
        }
        private List<get_objex_obj> get_objex_obj_list;
        public void getObjectInfoEx(string _query_json, int _skip, int _limit, Action<string> _handle, Action _end)
        {
            var _callbackid = System.Guid.NewGuid().ToString("N");
            if (_dbproxy._hub_call_dbproxy_caller != null)
            {
                _dbproxy._hub_call_dbproxy_caller.get_object_infoex(_db, _collection, _query_json, _skip, _limit, _callbackid);
            }
            else
            {
                get_objex_obj_list.Add(new get_objex_obj() { db = _db, collection = _collection, query_json = _query_json, skip = _skip, limit = _limit, callbackid = _callbackid });
            }
            _dbproxy.obj_callback.Add(_callbackid, _handle);
            _dbproxy.obj_end_callback.Add(_callbackid, _end);
        }

        struct remove_obj
        {
            public string db;
            public string collection;
            public string query_json;
            public string callbackid;
        }
        private List<remove_obj> remove_obj_list;
        public void removeObject(string _query_json, Action<bool> _handle)
        {
            var _callbackid = System.Guid.NewGuid().ToString("N");
            if (_dbproxy._hub_call_dbproxy_caller != null)
            {
                _dbproxy._hub_call_dbproxy_caller.remove_object(_db, _collection, _query_json, _callbackid);
            }
            else
            {
                remove_obj_list.Add(new remove_obj() { db = _db, collection = _collection, query_json = _query_json, callbackid = _callbackid });
            }
            _dbproxy.remove_callback.Add(_callbackid, _handle);
        }
    }
}
