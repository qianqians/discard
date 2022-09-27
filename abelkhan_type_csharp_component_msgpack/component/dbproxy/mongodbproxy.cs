using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace abelkhan
{
	public class mongodbproxy
	{
        private Func<MongoDB.Driver.MongoClient> createMongocLient;
        private List<MongoDB.Driver.MongoClient> client_pool = new List<MongoDB.Driver.MongoClient>();

		public mongodbproxy(String ip, short port)
		{
            createMongocLient = ()=>
            {
                var setting = new MongoDB.Driver.MongoClientSettings();
                setting.Server = new MongoDB.Driver.MongoServerAddress(ip, port);
                return new MongoDB.Driver.MongoClient(setting);
            };
        }

        public mongodbproxy(String url)
        {
            createMongocLient = () =>
            {
                var mongo_url = new MongoDB.Driver.MongoUrl(url);
                return new MongoDB.Driver.MongoClient(mongo_url);
            };
        }

        private MongoDB.Driver.MongoClient getMongoCLient()
        {
            lock(client_pool)
            {
                if (client_pool.Count > 0)
                {
                    var tmp = client_pool[0];
                    client_pool.Remove(tmp);
                    return tmp;
                }
            }

            return createMongocLient();
        }

        private void releaseMongoClient(MongoDB.Driver.MongoClient client)
        {
            lock (client_pool)
            {
                client_pool.Add(client);
            }
        }

        public void create_index(string db, string collection, string key, bool is_unique)
        {
            var _mongoclient = getMongoCLient();
            var _db = _mongoclient.GetDatabase(db);
            var _collection = _db.GetCollection<MongoDB.Bson.BsonDocument>(collection) as MongoDB.Driver.IMongoCollection<MongoDB.Bson.BsonDocument>;

            try
            {
                var builder = new MongoDB.Driver.IndexKeysDefinitionBuilder<MongoDB.Bson.BsonDocument>();
                var opt = new MongoDB.Driver.CreateIndexOptions();
                opt.Unique = is_unique;
                var indexModel = new MongoDB.Driver.CreateIndexModel<MongoDB.Bson.BsonDocument>(builder.Ascending(key), opt);
                _collection.Indexes.CreateOne(indexModel);
            }
            catch(System.Exception e)
            {
                log.error(new System.Diagnostics.StackFrame(), timerservice.Tick, "create_index faild, {0}", e.Message);
            }
            finally
            {
                releaseMongoClient(_mongoclient);
            }
        }

        public async Task<bool> save(string db, string collection, string json_data) 
		{
            var _mongoclient = getMongoCLient();
            var _db = _mongoclient.GetDatabase(db);
            var _collection = _db.GetCollection<MongoDB.Bson.BsonDocument> (collection) as MongoDB.Driver.IMongoCollection<MongoDB.Bson.BsonDocument>;

            try
            {
                MongoDB.Bson.BsonDocument _d = MongoDB.Bson.BsonDocument.Parse(json_data);
                await _collection.InsertOneAsync(_d);
            }
            catch(System.Exception e)
            {
                log.error(new System.Diagnostics.StackFrame(), timerservice.Tick, "save data faild, {0}", e.Message);
                return false;
            }
            finally
            {
                releaseMongoClient(_mongoclient);
            }

            return true;
		}

        public async Task<bool> update(string db, string collection, string json_query, string json_update)
        {
            var _mongoclient = getMongoCLient();
            var _db = _mongoclient.GetDatabase(db);
            var _collection = _db.GetCollection<MongoDB.Bson.BsonDocument>(collection) as MongoDB.Driver.IMongoCollection<MongoDB.Bson.BsonDocument>;

            try
            {
                var _bson_update = MongoDB.Bson.BsonDocument.Parse(json_update);
                var _bson_query = MongoDB.Bson.BsonDocument.Parse(json_query);
                var _query = new MongoDB.Driver.BsonDocumentFilterDefinition<MongoDB.Bson.BsonDocument>(_bson_query);
                var _bson_update_impl = new MongoDB.Bson.BsonDocument { { "$set", _bson_update } };
                var _update = new MongoDB.Driver.BsonDocumentUpdateDefinition<MongoDB.Bson.BsonDocument>(_bson_update_impl);

                await _collection.UpdateOneAsync(_query, _update);
            }
            catch (System.Exception e)
            {
                log.error(new System.Diagnostics.StackFrame(), timerservice.Tick, "update data faild, {0}", e.Message);
                return false;
            }
            finally
            {
                releaseMongoClient(_mongoclient);
            }

            return true;
		}

		public async Task<ArrayList> find(string db, string collection, string json_query)
        {
            ArrayList _list = new ArrayList();

            var _mongoclient = getMongoCLient();
            var _db = _mongoclient.GetDatabase(db);
            var _collection = _db.GetCollection<MongoDB.Bson.BsonDocument>(collection) as MongoDB.Driver.IMongoCollection<MongoDB.Bson.BsonDocument>;

            try
            {
                var _bson_query = MongoDB.Bson.BsonDocument.Parse(json_query);
                var _query = new MongoDB.Driver.BsonDocumentFilterDefinition<MongoDB.Bson.BsonDocument>(_bson_query);

                var c = await _collection.FindAsync<MongoDB.Bson.BsonDocument>(_query);

                do
                {
                    var _c = c.Current;

                    if (_c != null)
                    {
                        foreach (var data in _c)
                        {
                            var _data = data.ToHashtable();
                            _data.Remove("_id");
                            _list.Add(_data);
                        }
                    }
                } while (c.MoveNext());
            }
            catch (System.Exception e)
            {
                log.error(new System.Diagnostics.StackFrame(), timerservice.Tick, "find faild, {0}", e.Message);
                return _list;
            }
            finally
            {
                releaseMongoClient(_mongoclient);
            }

            return _list;
		}

        public async Task<ArrayList> findex(string db, string collection, string json_query, int skip, int limit)
        {
            ArrayList _list = new ArrayList();

            var _mongoclient = getMongoCLient();
            var _db = _mongoclient.GetDatabase(db);
            var _collection = _db.GetCollection<MongoDB.Bson.BsonDocument>(collection) as MongoDB.Driver.IMongoCollection<MongoDB.Bson.BsonDocument>;

            try
            {
                var _bson_query = MongoDB.Bson.BsonDocument.Parse(json_query);
                var _query = new MongoDB.Driver.BsonDocumentFilterDefinition<MongoDB.Bson.BsonDocument>(_bson_query);
                var _opt = new MongoDB.Driver.FindOptions<MongoDB.Bson.BsonDocument, MongoDB.Bson.BsonDocument>();
                _opt.Skip = skip;
                _opt.Limit = limit;

                var c = await _collection.FindAsync<MongoDB.Bson.BsonDocument>(_query, _opt);

                do
                {
                    var _c = c.Current;

                    if (_c != null)
                    {
                        foreach (var data in _c)
                        {
                            var _data = data.ToHashtable();
                            _data.Remove("_id");
                            _list.Add(_data);
                        }
                    }
                } while (c.MoveNext());
            }
            catch (System.Exception e)
            {
                log.error(new System.Diagnostics.StackFrame(), timerservice.Tick, "find faild, {0}", e.Message);
                return _list;
            }
            finally
            {
                releaseMongoClient(_mongoclient);
            }

            return _list;
        }

        public async Task<int> count(string db, string collection, string json_query)
        {
            long c = 0;

            var _mongoclient = getMongoCLient();
            var _db = _mongoclient.GetDatabase(db);
            var _collection = _db.GetCollection<MongoDB.Bson.BsonDocument>(collection) as MongoDB.Driver.IMongoCollection<MongoDB.Bson.BsonDocument>;

            try
            {
                var _bson_query = MongoDB.Bson.BsonDocument.Parse(json_query);
                var _query = new MongoDB.Driver.BsonDocumentFilterDefinition<MongoDB.Bson.BsonDocument>(_bson_query);

                c = await _collection.CountDocumentsAsync(_query);
            }
            catch (System.Exception e)
            {
                log.error(new System.Diagnostics.StackFrame(), timerservice.Tick, "count faild, {0}", e.Message);
                return 0;
            }
            finally
            {
                releaseMongoClient(_mongoclient);
            }

            return (int)c;
        }

		public async Task<bool> remove(string db, string collection, string json_query)
        {
            var _mongoclient = getMongoCLient();
            var _db = _mongoclient.GetDatabase(db);
            var _collection = _db.GetCollection<MongoDB.Bson.BsonDocument>(collection) as MongoDB.Driver.IMongoCollection<MongoDB.Bson.BsonDocument>;

            try
            {
                var _bson_query = MongoDB.Bson.BsonDocument.Parse(json_query);
                var _query = new MongoDB.Driver.BsonDocumentFilterDefinition<MongoDB.Bson.BsonDocument>(_bson_query);

                await _collection.DeleteOneAsync(_query);
            }
            catch (System.Exception e)
            {
                log.error(new System.Diagnostics.StackFrame(), timerservice.Tick, "remove faild, {0}", e.Message);
                return false;
            }
            finally
            {
                releaseMongoClient(_mongoclient);
            }

            return true;
		}

	}
}

