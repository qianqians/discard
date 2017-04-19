/*
 * juggle codegen code explain notes
 *
 * juggle is a rpc framework under the GPL v2 License
 * 
 * juggle codegen generate rcp code
 * only for send message and dispense message
 * so users need write code handle process message
 *
 * this notes create in 2015.4.26
 * made for juggle codegen
 * autor qianqians
 */

#include <juggle.h>
#include <boost/make_shared.hpp>

namespace sync{

class dbproxy: public Fossilizid::juggle::caller{
public:
	dbproxy(boost::shared_ptr<Fossilizid::juggle::process> __process, boost::shared_ptr<Fossilizid::juggle::channel> ch) : caller(__process, ch, "dbproxy"){
	}

	~dbproxy(){
	}

	bool add_user(){
		boost::shared_ptr<boost::unordered_map<std::string, boost::any> > v = boost::make_shared<boost::unordered_map<std::string, boost::any> >();
		boost::shared_ptr<boost::unordered_map<std::string, boost::any> > r = call_module_method_sync("dbproxy_add_user", v);
        return boost::any_cast<bool>((*r)["ret"]);
	}

	bool create_index(std::string collection_name,std::string keys){
		boost::shared_ptr<boost::unordered_map<std::string, boost::any> > v = boost::make_shared<boost::unordered_map<std::string, boost::any> >();
		(*v)["collection_name"] = collection_name;
		(*v)["keys"] = keys;
		boost::shared_ptr<boost::unordered_map<std::string, boost::any> > r = call_module_method_sync("dbproxy_create_index", v);
        return boost::any_cast<bool>((*r)["ret"]);
	}

	bool drop_index(std::string collection,std::string index_name){
		boost::shared_ptr<boost::unordered_map<std::string, boost::any> > v = boost::make_shared<boost::unordered_map<std::string, boost::any> >();
		(*v)["collection"] = collection;
		(*v)["index_name"] = index_name;
		boost::shared_ptr<boost::unordered_map<std::string, boost::any> > r = call_module_method_sync("dbproxy_drop_index", v);
        return boost::any_cast<bool>((*r)["ret"]);
	}

	std::vector<boost::shared_ptr<boost::unordered_map<std::string, boost::any> > >  find_indexes(std::string collection){
		boost::shared_ptr<boost::unordered_map<std::string, boost::any> > v = boost::make_shared<boost::unordered_map<std::string, boost::any> >();
		(*v)["collection"] = collection;
		boost::shared_ptr<boost::unordered_map<std::string, boost::any> > r = call_module_method_sync("dbproxy_find_indexes", v);
        std::vector<boost::shared_ptr<boost::unordered_map<std::string, boost::any> > > ret;
        for(int i = 0; i < boost::any_cast<std::vector<boost::any> >((*r)["ret"]).size(); i++){
            ret.push_back(boost::any_cast<boost::shared_ptr<boost::unordered_map<std::string, boost::any> > >(boost::any_cast<std::vector<boost::any> >((*r)["ret"])[i]));
        }
        return ret;
	}

	int64_t count(std::string collection_name,boost::shared_ptr<boost::unordered_map<std::string, boost::any> > query,int64_t skip,int64_t limit){
		boost::shared_ptr<boost::unordered_map<std::string, boost::any> > v = boost::make_shared<boost::unordered_map<std::string, boost::any> >();
		(*v)["collection_name"] = collection_name;
		(*v)["query"] = query;
		(*v)["skip"] = skip;
		(*v)["limit"] = limit;
		boost::shared_ptr<boost::unordered_map<std::string, boost::any> > r = call_module_method_sync("dbproxy_count", v);
        return boost::any_cast<int64_t>((*r)["ret"]);
	}

	bool insert(std::string collection,boost::shared_ptr<boost::unordered_map<std::string, boost::any> > document){
		boost::shared_ptr<boost::unordered_map<std::string, boost::any> > v = boost::make_shared<boost::unordered_map<std::string, boost::any> >();
		(*v)["collection"] = collection;
		(*v)["document"] = document;
		boost::shared_ptr<boost::unordered_map<std::string, boost::any> > r = call_module_method_sync("dbproxy_insert", v);
        return boost::any_cast<bool>((*r)["ret"]);
	}

	bool save(std::string collection,boost::shared_ptr<boost::unordered_map<std::string, boost::any> > document){
		boost::shared_ptr<boost::unordered_map<std::string, boost::any> > v = boost::make_shared<boost::unordered_map<std::string, boost::any> >();
		(*v)["collection"] = collection;
		(*v)["document"] = document;
		boost::shared_ptr<boost::unordered_map<std::string, boost::any> > r = call_module_method_sync("dbproxy_save", v);
        return boost::any_cast<bool>((*r)["ret"]);
	}

	bool update(std::string collection,boost::shared_ptr<boost::unordered_map<std::string, boost::any> > query,boost::shared_ptr<boost::unordered_map<std::string, boost::any> > update){
		boost::shared_ptr<boost::unordered_map<std::string, boost::any> > v = boost::make_shared<boost::unordered_map<std::string, boost::any> >();
		(*v)["collection"] = collection;
		(*v)["query"] = query;
		(*v)["update"] = update;
		boost::shared_ptr<boost::unordered_map<std::string, boost::any> > r = call_module_method_sync("dbproxy_update", v);
        return boost::any_cast<bool>((*r)["ret"]);
	}

	bool remove(std::string collection,boost::shared_ptr<boost::unordered_map<std::string, boost::any> > query){
		boost::shared_ptr<boost::unordered_map<std::string, boost::any> > v = boost::make_shared<boost::unordered_map<std::string, boost::any> >();
		(*v)["collection"] = collection;
		(*v)["query"] = query;
		boost::shared_ptr<boost::unordered_map<std::string, boost::any> > r = call_module_method_sync("dbproxy_remove", v);
        return boost::any_cast<bool>((*r)["ret"]);
	}

	std::vector<boost::shared_ptr<boost::unordered_map<std::string, boost::any> > >  find(std::string collection,int64_t skip,int64_t limit,int64_t batch_size,boost::shared_ptr<boost::unordered_map<std::string, boost::any> > query,std::vector<std::string>  fields){
		boost::shared_ptr<boost::unordered_map<std::string, boost::any> > v = boost::make_shared<boost::unordered_map<std::string, boost::any> >();
		(*v)["collection"] = collection;
		(*v)["skip"] = skip;
		(*v)["limit"] = limit;
		(*v)["batch_size"] = batch_size;
		(*v)["query"] = query;
		(*v)["fields"] = fields;
		boost::shared_ptr<boost::unordered_map<std::string, boost::any> > r = call_module_method_sync("dbproxy_find", v);
        std::vector<boost::shared_ptr<boost::unordered_map<std::string, boost::any> > > ret;
        for(int i = 0; i < boost::any_cast<std::vector<boost::any> >((*r)["ret"]).size(); i++){
            ret.push_back(boost::any_cast<boost::shared_ptr<boost::unordered_map<std::string, boost::any> > >(boost::any_cast<std::vector<boost::any> >((*r)["ret"])[i]));
        }
        return ret;
	}

	boost::shared_ptr<boost::unordered_map<std::string, boost::any> > find_and_modify(std::string collection,boost::shared_ptr<boost::unordered_map<std::string, boost::any> > query,boost::shared_ptr<boost::unordered_map<std::string, boost::any> > sort,boost::shared_ptr<boost::unordered_map<std::string, boost::any> > update,std::vector<std::string>  fields,bool _remove,bool upsert,bool _new){
		boost::shared_ptr<boost::unordered_map<std::string, boost::any> > v = boost::make_shared<boost::unordered_map<std::string, boost::any> >();
		(*v)["collection"] = collection;
		(*v)["query"] = query;
		(*v)["sort"] = sort;
		(*v)["update"] = update;
		(*v)["fields"] = fields;
		(*v)["_remove"] = _remove;
		(*v)["upsert"] = upsert;
		(*v)["_new"] = _new;
		boost::shared_ptr<boost::unordered_map<std::string, boost::any> > r = call_module_method_sync("dbproxy_find_and_modify", v);
        return boost::any_cast<boost::shared_ptr<boost::unordered_map<std::string, boost::any> > >((*r)["ret"]);
	}

	boost::shared_ptr<boost::unordered_map<std::string, boost::any> > validate(std::string collection){
		boost::shared_ptr<boost::unordered_map<std::string, boost::any> > v = boost::make_shared<boost::unordered_map<std::string, boost::any> >();
		(*v)["collection"] = collection;
		boost::shared_ptr<boost::unordered_map<std::string, boost::any> > r = call_module_method_sync("dbproxy_validate", v);
        return boost::any_cast<boost::shared_ptr<boost::unordered_map<std::string, boost::any> > >((*r)["ret"]);
	}

	boost::shared_ptr<boost::unordered_map<std::string, boost::any> > stats(std::string collection){
		boost::shared_ptr<boost::unordered_map<std::string, boost::any> > v = boost::make_shared<boost::unordered_map<std::string, boost::any> >();
		(*v)["collection"] = collection;
		boost::shared_ptr<boost::unordered_map<std::string, boost::any> > r = call_module_method_sync("dbproxy_stats", v);
        return boost::any_cast<boost::shared_ptr<boost::unordered_map<std::string, boost::any> > >((*r)["ret"]);
	}

};

}

namespace async{

class dbproxy: public Fossilizid::juggle::caller{
public:
	dbproxy(boost::shared_ptr<Fossilizid::juggle::process> __process, boost::shared_ptr<Fossilizid::juggle::channel> ch) : caller(__process, ch, "dbproxy"){
	}

	~dbproxy(){
	}

	bool add_user(boost::function<void(bool)> callback){
		boost::shared_ptr<boost::unordered_map<std::string, boost::any> > v = boost::make_shared<boost::unordered_map<std::string, boost::any> >();
		auto cb = [this, callback](boost::shared_ptr<boost::unordered_map<std::string, boost::any> > r){
   auto ret = boost::any_cast<bool>((*r)["ret"]);			callback(ret);
        };
		call_module_method_async("dbproxy_add_user", v, cb);
	}

	bool create_index(std::string collection_name, std::string keys, boost::function<void(bool)> callback){
		boost::shared_ptr<boost::unordered_map<std::string, boost::any> > v = boost::make_shared<boost::unordered_map<std::string, boost::any> >();
		(*v)["collection_name"] = collection_name;
		(*v)["keys"] = keys;
		auto cb = [this, callback](boost::shared_ptr<boost::unordered_map<std::string, boost::any> > r){
   auto ret = boost::any_cast<bool>((*r)["ret"]);			callback(ret);
        };
		call_module_method_async("dbproxy_create_index", v, cb);
	}

	bool drop_index(std::string collection, std::string index_name, boost::function<void(bool)> callback){
		boost::shared_ptr<boost::unordered_map<std::string, boost::any> > v = boost::make_shared<boost::unordered_map<std::string, boost::any> >();
		(*v)["collection"] = collection;
		(*v)["index_name"] = index_name;
		auto cb = [this, callback](boost::shared_ptr<boost::unordered_map<std::string, boost::any> > r){
   auto ret = boost::any_cast<bool>((*r)["ret"]);			callback(ret);
        };
		call_module_method_async("dbproxy_drop_index", v, cb);
	}

	std::vector<boost::shared_ptr<boost::unordered_map<std::string, boost::any> > >  find_indexes(std::string collection, boost::function<void(std::vector<boost::shared_ptr<boost::unordered_map<std::string, boost::any> > > )> callback){
		boost::shared_ptr<boost::unordered_map<std::string, boost::any> > v = boost::make_shared<boost::unordered_map<std::string, boost::any> >();
		(*v)["collection"] = collection;
		auto cb = [this, callback](boost::shared_ptr<boost::unordered_map<std::string, boost::any> > r){
           std::vector<boost::shared_ptr<boost::unordered_map<std::string, boost::any> > > ret;
        for(int i = 0; i < boost::any_cast<std::vector<boost::any> >((*r)["ret"]).size(); i++){
            ret.push_back(boost::any_cast<boost::shared_ptr<boost::unordered_map<std::string, boost::any> > >(boost::any_cast<std::vector<boost::any> >((*r)["ret"])[i]));
        }
			callback(ret);
        };
		call_module_method_async("dbproxy_find_indexes", v, cb);
	}

	int64_t count(std::string collection_name, boost::shared_ptr<boost::unordered_map<std::string, boost::any> > query, int64_t skip, int64_t limit, boost::function<void(int64_t)> callback){
		boost::shared_ptr<boost::unordered_map<std::string, boost::any> > v = boost::make_shared<boost::unordered_map<std::string, boost::any> >();
		(*v)["collection_name"] = collection_name;
		(*v)["query"] = query;
		(*v)["skip"] = skip;
		(*v)["limit"] = limit;
		auto cb = [this, callback](boost::shared_ptr<boost::unordered_map<std::string, boost::any> > r){
   auto ret = boost::any_cast<int64_t>((*r)["ret"]);			callback(ret);
        };
		call_module_method_async("dbproxy_count", v, cb);
	}

	bool insert(std::string collection, boost::shared_ptr<boost::unordered_map<std::string, boost::any> > document, boost::function<void(bool)> callback){
		boost::shared_ptr<boost::unordered_map<std::string, boost::any> > v = boost::make_shared<boost::unordered_map<std::string, boost::any> >();
		(*v)["collection"] = collection;
		(*v)["document"] = document;
		auto cb = [this, callback](boost::shared_ptr<boost::unordered_map<std::string, boost::any> > r){
   auto ret = boost::any_cast<bool>((*r)["ret"]);			callback(ret);
        };
		call_module_method_async("dbproxy_insert", v, cb);
	}

	bool save(std::string collection, boost::shared_ptr<boost::unordered_map<std::string, boost::any> > document, boost::function<void(bool)> callback){
		boost::shared_ptr<boost::unordered_map<std::string, boost::any> > v = boost::make_shared<boost::unordered_map<std::string, boost::any> >();
		(*v)["collection"] = collection;
		(*v)["document"] = document;
		auto cb = [this, callback](boost::shared_ptr<boost::unordered_map<std::string, boost::any> > r){
   auto ret = boost::any_cast<bool>((*r)["ret"]);			callback(ret);
        };
		call_module_method_async("dbproxy_save", v, cb);
	}

	bool update(std::string collection, boost::shared_ptr<boost::unordered_map<std::string, boost::any> > query, boost::shared_ptr<boost::unordered_map<std::string, boost::any> > update, boost::function<void(bool)> callback){
		boost::shared_ptr<boost::unordered_map<std::string, boost::any> > v = boost::make_shared<boost::unordered_map<std::string, boost::any> >();
		(*v)["collection"] = collection;
		(*v)["query"] = query;
		(*v)["update"] = update;
		auto cb = [this, callback](boost::shared_ptr<boost::unordered_map<std::string, boost::any> > r){
   auto ret = boost::any_cast<bool>((*r)["ret"]);			callback(ret);
        };
		call_module_method_async("dbproxy_update", v, cb);
	}

	bool remove(std::string collection, boost::shared_ptr<boost::unordered_map<std::string, boost::any> > query, boost::function<void(bool)> callback){
		boost::shared_ptr<boost::unordered_map<std::string, boost::any> > v = boost::make_shared<boost::unordered_map<std::string, boost::any> >();
		(*v)["collection"] = collection;
		(*v)["query"] = query;
		auto cb = [this, callback](boost::shared_ptr<boost::unordered_map<std::string, boost::any> > r){
   auto ret = boost::any_cast<bool>((*r)["ret"]);			callback(ret);
        };
		call_module_method_async("dbproxy_remove", v, cb);
	}

	std::vector<boost::shared_ptr<boost::unordered_map<std::string, boost::any> > >  find(std::string collection, int64_t skip, int64_t limit, int64_t batch_size, boost::shared_ptr<boost::unordered_map<std::string, boost::any> > query, std::vector<std::string>  fields, boost::function<void(std::vector<boost::shared_ptr<boost::unordered_map<std::string, boost::any> > > )> callback){
		boost::shared_ptr<boost::unordered_map<std::string, boost::any> > v = boost::make_shared<boost::unordered_map<std::string, boost::any> >();
		(*v)["collection"] = collection;
		(*v)["skip"] = skip;
		(*v)["limit"] = limit;
		(*v)["batch_size"] = batch_size;
		(*v)["query"] = query;
		(*v)["fields"] = fields;
		auto cb = [this, callback](boost::shared_ptr<boost::unordered_map<std::string, boost::any> > r){
           std::vector<boost::shared_ptr<boost::unordered_map<std::string, boost::any> > > ret;
        for(int i = 0; i < boost::any_cast<std::vector<boost::any> >((*r)["ret"]).size(); i++){
            ret.push_back(boost::any_cast<boost::shared_ptr<boost::unordered_map<std::string, boost::any> > >(boost::any_cast<std::vector<boost::any> >((*r)["ret"])[i]));
        }
			callback(ret);
        };
		call_module_method_async("dbproxy_find", v, cb);
	}

	boost::shared_ptr<boost::unordered_map<std::string, boost::any> > find_and_modify(std::string collection, boost::shared_ptr<boost::unordered_map<std::string, boost::any> > query, boost::shared_ptr<boost::unordered_map<std::string, boost::any> > sort, boost::shared_ptr<boost::unordered_map<std::string, boost::any> > update, std::vector<std::string>  fields, bool _remove, bool upsert, bool _new, boost::function<void(boost::shared_ptr<boost::unordered_map<std::string, boost::any> >)> callback){
		boost::shared_ptr<boost::unordered_map<std::string, boost::any> > v = boost::make_shared<boost::unordered_map<std::string, boost::any> >();
		(*v)["collection"] = collection;
		(*v)["query"] = query;
		(*v)["sort"] = sort;
		(*v)["update"] = update;
		(*v)["fields"] = fields;
		(*v)["_remove"] = _remove;
		(*v)["upsert"] = upsert;
		(*v)["_new"] = _new;
		auto cb = [this, callback](boost::shared_ptr<boost::unordered_map<std::string, boost::any> > r){
   auto ret = boost::any_cast<boost::shared_ptr<boost::unordered_map<std::string, boost::any> > >((*r)["ret"]);			callback(ret);
        };
		call_module_method_async("dbproxy_find_and_modify", v, cb);
	}

	boost::shared_ptr<boost::unordered_map<std::string, boost::any> > validate(std::string collection, boost::function<void(boost::shared_ptr<boost::unordered_map<std::string, boost::any> >)> callback){
		boost::shared_ptr<boost::unordered_map<std::string, boost::any> > v = boost::make_shared<boost::unordered_map<std::string, boost::any> >();
		(*v)["collection"] = collection;
		auto cb = [this, callback](boost::shared_ptr<boost::unordered_map<std::string, boost::any> > r){
   auto ret = boost::any_cast<boost::shared_ptr<boost::unordered_map<std::string, boost::any> > >((*r)["ret"]);			callback(ret);
        };
		call_module_method_async("dbproxy_validate", v, cb);
	}

	boost::shared_ptr<boost::unordered_map<std::string, boost::any> > stats(std::string collection, boost::function<void(boost::shared_ptr<boost::unordered_map<std::string, boost::any> >)> callback){
		boost::shared_ptr<boost::unordered_map<std::string, boost::any> > v = boost::make_shared<boost::unordered_map<std::string, boost::any> >();
		(*v)["collection"] = collection;
		auto cb = [this, callback](boost::shared_ptr<boost::unordered_map<std::string, boost::any> > r){
   auto ret = boost::any_cast<boost::shared_ptr<boost::unordered_map<std::string, boost::any> > >((*r)["ret"]);			callback(ret);
        };
		call_module_method_async("dbproxy_stats", v, cb);
	}

};

}

