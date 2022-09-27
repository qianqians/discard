using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace abelkhan
{
    /*write event*/
    public class create_event
    {
        public create_event(mongodbproxy _mongodbproxy_, hubproxy _hubproxy_, string _db, string _collection, string _object_info, string _callbackid)
        {
            _mongodbproxy = _mongodbproxy_;
            _hubproxy = _hubproxy_;
            db = _db;
            collection = _collection;
            object_info = _object_info;
            callbackid = _callbackid;
        }

        public async Task do_event()
        {
            var is_create_sucess = await _mongodbproxy.save(db, collection, object_info);
            _hubproxy.ack_create_persisted_object(callbackid, is_create_sucess);

        }

        public mongodbproxy _mongodbproxy;
        public hubproxy _hubproxy;
        public string db;
        public string collection;
        public string object_info;
        public string callbackid;
    }

    public class update_event
    {
        public update_event(mongodbproxy _mongodbproxy_, hubproxy _hubproxy_, string _db, string _collection, string _query_json, string _object_info, string _callbackid)
        {
            _mongodbproxy = _mongodbproxy_;
            _hubproxy = _hubproxy_;
            db = _db;
            collection = _collection;
            query_json = _query_json;
            object_info = _object_info;
            callbackid = _callbackid;
        }

        public async Task do_event()
        {
            var is_update_sucess = await _mongodbproxy.update(db, collection, query_json, object_info);
            _hubproxy.ack_updata_persisted_object(callbackid, is_update_sucess);
        }

        public mongodbproxy _mongodbproxy;
        public hubproxy _hubproxy;
        public string db;
        public string collection;
        public string query_json;
        public string object_info;
        public string callbackid;
    }

    public class remove_event
    {
        public remove_event(mongodbproxy _mongodbproxy_, hubproxy _hubproxy_, string _db, string _collection, string _query_json, string _callbackid)
        {
            _mongodbproxy = _mongodbproxy_;
            _hubproxy = _hubproxy_;
            db = _db;
            collection = _collection;
            query_json = _query_json;
            callbackid = _callbackid;
        }

        public async Task do_event()
        {
            var is_remove_sucess = await _mongodbproxy.remove(db, collection, query_json);
            _hubproxy.ack_remove_object(callbackid, is_remove_sucess);
        }

        public mongodbproxy _mongodbproxy;
        public hubproxy _hubproxy;
        public string db;
        public string collection;
        public string query_json;
        public string callbackid;
    }

    /*read event*/
    public class count_event
    {
        public count_event(mongodbproxy _mongodbproxy_, hubproxy _hubproxy_, string _db, string _collection, string _query_json, string _callbackid)
        {
            _mongodbproxy = _mongodbproxy_;
            _hubproxy = _hubproxy_;
            db = _db;
            collection = _collection;
            query_json = _query_json;
            callbackid = _callbackid;
        }

        public async Task do_event()
        {
            var _count = await _mongodbproxy.count(db, collection, query_json);
            _hubproxy.ack_get_object_count(callbackid, _count);
        }

        public mongodbproxy _mongodbproxy;
        public hubproxy _hubproxy;
        public string db;
        public string collection;
        public string query_json;
        public string callbackid;
    }

    public class find_event
    {
        public find_event(mongodbproxy _mongodbproxy_, hubproxy _hubproxy_, string _db, string _collection, string _query_json, string _callbackid)
        {
            _mongodbproxy = _mongodbproxy_;
            _hubproxy = _hubproxy_;
            db = _db;
            collection = _collection;
            query_json = _query_json;
            callbackid = _callbackid;
        }

        public async Task do_event()
        {
            ArrayList _list = await _mongodbproxy.find(db, collection, query_json);

            int count = 0;
            ArrayList _datalist = new ArrayList();
            if (_list.Count == 0)
            {
                _hubproxy.ack_get_object_info(callbackid, Newtonsoft.Json.JsonConvert.SerializeObject(_datalist));
            }
            else
            {
                foreach (var data in _list)
                {
                    _datalist.Add(data);

                    count++;

                    if (count >= 100)
                    {
                        _hubproxy.ack_get_object_info(callbackid, Newtonsoft.Json.JsonConvert.SerializeObject(_datalist));

                        count = 0;
                        _datalist = new ArrayList();
                    }
                }
                if (count > 0 && count < 100)
                {
                    _hubproxy.ack_get_object_info(callbackid, Newtonsoft.Json.JsonConvert.SerializeObject(_datalist));
                }
            }
            _hubproxy.ack_get_object_info_end(callbackid);
        }

        public mongodbproxy _mongodbproxy;
        public hubproxy _hubproxy;
        public string db;
        public string collection;
        public string query_json;
        public string callbackid;
    }

    public class findex_event
    {
        public findex_event(mongodbproxy _mongodbproxy_, hubproxy _hubproxy_, string _db, string _collection, string _query_json, int _skip, int _limit, string _callbackid)
        {
            _mongodbproxy = _mongodbproxy_;
            _hubproxy = _hubproxy_;
            db = _db;
            collection = _collection;
            query_json = _query_json;
            skip = _skip;
            limit = _limit;
            callbackid = _callbackid;
        }

        public async Task do_event()
        {
            ArrayList _list = await _mongodbproxy.findex(db, collection, query_json, skip, limit);

            int count = 0;
            ArrayList _datalist = new ArrayList();
            if (_list.Count == 0)
            {
                _hubproxy.ack_get_object_info(callbackid, Newtonsoft.Json.JsonConvert.SerializeObject(_datalist));
            }
            else
            {
                foreach (var data in _list)
                {
                    _datalist.Add(data);

                    count++;

                    if (count >= 100)
                    {
                        _hubproxy.ack_get_object_info(callbackid, Newtonsoft.Json.JsonConvert.SerializeObject(_datalist));

                        count = 0;
                        _datalist = new ArrayList();
                    }
                }
                if (count > 0 && count < 100)
                {
                    _hubproxy.ack_get_object_info(callbackid, Newtonsoft.Json.JsonConvert.SerializeObject(_datalist));
                }
            }
            _hubproxy.ack_get_object_info_end(callbackid);
        }

        public mongodbproxy _mongodbproxy;
        public hubproxy _hubproxy;
        public string db;
        public string collection;
        public string query_json;
        public int skip;
        public int limit;
        public string callbackid;
    }

    public class db_collection_write_event
    {
        private closeHandle closeHandle;

        public db_collection_write_event(closeHandle _closeHandle)
        {
            closeHandle = _closeHandle;
        }

        private Queue<create_event> create_event_list = new Queue<create_event>();
        private Queue<update_event> updata_event_list = new Queue<update_event>();
        private Queue<remove_event> remove_event_list = new Queue<remove_event>();

        public void push_create_event(create_event _event)
        {
            lock(create_event_list)
            {
                create_event_list.Enqueue(_event);
            }
        }

        public void push_updata_event(update_event _event)
        {
            lock (updata_event_list)
            {
                updata_event_list.Enqueue(_event);
            }
        }

        public void push_remove_event(remove_event _event)
        {
            lock (remove_event_list)
            {
                remove_event_list.Enqueue(_event);
            }
        }

        public Thread start()
        {
            Thread t = new Thread(async () =>
            {
                while (true)
                {
                    bool do_nothing = true;

                    {
                        create_event _event = null;
                        lock (create_event_list)
                        {
                            if (create_event_list.Count > 0)
                            {
                                _event = create_event_list.Dequeue();
                                do_nothing = false;
                            }
                        }
                        if (_event != null)
                        {
                            await _event.do_event();
                        }
                    }

                    {
                        update_event _event = null;
                        lock (updata_event_list)
                        {
                            if (updata_event_list.Count > 0)
                            {
                                _event = updata_event_list.Dequeue();
                                do_nothing = false;
                            }
                        }
                        if (_event != null)
                        {
                            await _event.do_event();
                        }
                    }

                    {
                        remove_event _event = null;
                        lock (remove_event_list)
                        {
                            if (remove_event_list.Count > 0)
                            {
                                _event = remove_event_list.Dequeue();
                                do_nothing = false;
                            }
                        }
                        if (_event != null)
                        {
                            await _event.do_event();
                        }
                    }

                    if (do_nothing)
                    {
                        if (closeHandle.is_close)
                        {
                            break;
                        }
                        Thread.Sleep(5);
                    }
                }
            });
            t.Start();

            return t;
        }
    }

    public class dbevent
    {
        private closeHandle closeHandle;

        public dbevent(closeHandle _closeHandle)
        {
            closeHandle = _closeHandle;
        }

        private List<Thread> th_list = new List<Thread>();

        public void join_all()
        {
            foreach(var t in th_list)
            {
                t.Join();
            }
        }

        private Queue<count_event> count_event_list = new Queue<count_event>();
        private Queue<find_event> find_event_list = new Queue<find_event>();
        private Queue<findex_event> findex_event_list = new Queue<findex_event>();

        public void push_count_event(count_event _event)
        {
            lock (count_event_list)
            {
                count_event_list.Enqueue(_event);
            }
        }

        public void push_find_event(find_event _event)
        {
            lock (find_event_list)
            {
                find_event_list.Enqueue(_event);
            }
        }

        public void push_findex_event(findex_event _event)
        {
            lock (find_event_list)
            {
                findex_event_list.Enqueue(_event);
            }
        }

        public void start()
        {
            for (int i = 0; i < 4; i++)
            {
                Thread t = new Thread(async () =>
                {
                    while (true)
                    {
                        bool do_nothing = true;

                        {
                            count_event _event = null;
                            lock (count_event_list)
                            {
                                if (count_event_list.Count > 0)
                                {
                                    _event = count_event_list.Dequeue();
                                    do_nothing = false;
                                }
                            }
                            if (_event != null)
                            {
                                await _event.do_event();
                            }
                        }

                        {
                            find_event _event = null;
                            lock (find_event_list)
                            {
                                if (find_event_list.Count > 0)
                                {
                                    _event = find_event_list.Dequeue();
                                    do_nothing = false;
                                }
                            }
                            if (_event != null)
                            {
                                await _event.do_event();
                            }
                        }

                        {
                            findex_event _event = null;
                            lock (findex_event_list)
                            {
                                if (findex_event_list.Count > 0)
                                {
                                    _event = findex_event_list.Dequeue();
                                    do_nothing = false;
                                }
                            }
                            if (_event != null)
                            {
                                await _event.do_event();
                            }
                        }

                        if (do_nothing)
                        {
                            if (closeHandle.is_close)
                            {
                                break;
                            }
                            Thread.Sleep(5);
                        }
                    }
                });
                t.Start();

                th_list.Add(t);
            }
        }

        public void push_create_event(create_event _event)
        {
            if (!collection_write_event_list.ContainsKey(_event.collection))
            {
                start_write(_event.collection);
            }

            collection_write_event_list[_event.collection].push_create_event(_event);
        }

        public void push_updata_event(update_event _event)
        {
            if (!collection_write_event_list.ContainsKey(_event.collection))
            {
                start_write(_event.collection);
            }

            collection_write_event_list[_event.collection].push_updata_event(_event);
        }

        public void push_remove_event(remove_event _event)
        {
            if (!collection_write_event_list.ContainsKey(_event.collection))
            {
                start_write(_event.collection);
            }

            collection_write_event_list[_event.collection].push_remove_event(_event);
        }

        private void start_write(string collection)
        {
            lock (collection_write_event_list)
            {
                if (collection_write_event_list.ContainsKey(collection))
                {
                    return;
                }

                var _write_event_list = new db_collection_write_event(closeHandle);
                th_list.Add(_write_event_list.start());

                collection_write_event_list.Add(collection, _write_event_list);
            }
        }

        private Dictionary<string, db_collection_write_event> collection_write_event_list = new Dictionary<string, db_collection_write_event>();
    }
    
}

