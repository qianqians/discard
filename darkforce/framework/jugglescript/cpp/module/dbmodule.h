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
#include <string>

namespace module{

class dbproxy: public Fossilizid::juggle::module{
public:
	dbproxy(boost::shared_ptr<Fossilizid::juggle::process> __process) : module(__process, "dbproxy", Fossilizid::uuid::UUID()){
		_module_func.push_back("dbproxy_add_user");
		_module_func.push_back("dbproxy_create_index");
		_module_func.push_back("dbproxy_drop_index");
		_module_func.push_back("dbproxy_find_indexes");
		_module_func.push_back("dbproxy_count");
		_module_func.push_back("dbproxy_insert");
		_module_func.push_back("dbproxy_save");
		_module_func.push_back("dbproxy_update");
		_module_func.push_back("dbproxy_remove");
		_module_func.push_back("dbproxy_find");
		_module_func.push_back("dbproxy_find_and_modify");
		_module_func.push_back("dbproxy_validate");
		_module_func.push_back("dbproxy_stats");
		__process->register_module_method("dbproxy_add_user", boost::bind(&dbproxy::call_add_user, this, _1, _2));
		__process->register_module_method("dbproxy_create_index", boost::bind(&dbproxy::call_create_index, this, _1, _2));
		__process->register_module_method("dbproxy_drop_index", boost::bind(&dbproxy::call_drop_index, this, _1, _2));
		__process->register_module_method("dbproxy_find_indexes", boost::bind(&dbproxy::call_find_indexes, this, _1, _2));
		__process->register_module_method("dbproxy_count", boost::bind(&dbproxy::call_count, this, _1, _2));
		__process->register_module_method("dbproxy_insert", boost::bind(&dbproxy::call_insert, this, _1, _2));
		__process->register_module_method("dbproxy_save", boost::bind(&dbproxy::call_save, this, _1, _2));
		__process->register_module_method("dbproxy_update", boost::bind(&dbproxy::call_update, this, _1, _2));
		__process->register_module_method("dbproxy_remove", boost::bind(&dbproxy::call_remove, this, _1, _2));
		__process->register_module_method("dbproxy_find", boost::bind(&dbproxy::call_find, this, _1, _2));
		__process->register_module_method("dbproxy_find_and_modify", boost::bind(&dbproxy::call_find_and_modify, this, _1, _2));
		__process->register_module_method("dbproxy_validate", boost::bind(&dbproxy::call_validate, this, _1, _2));
		__process->register_module_method("dbproxy_stats", boost::bind(&dbproxy::call_stats, this, _1, _2));
	}

	~dbproxy(){
	}

	boost::signals2::signal< bool()> sigadd_user;

	void call_add_user(boost::shared_ptr<Fossilizid::juggle::channel> ch, boost::shared_ptr<boost::unordered_map<std::string, boost::any> > v){
		auto ret = sigadd_user();
		boost::shared_ptr<boost::unordered_map<std::string, boost::any> > r = boost::make_shared<boost::unordered_map<std::string, boost::any> >();
		(*r)["suuid"] = boost::any_cast<std::string>((*v)["suuid"]);
		(*r)["method"] = boost::any_cast<std::string>((*v)["method"]);
		(*r)["rpcevent"] = "reply_rpc_method";

		(*r)["ret"] = ret;
		ch->push(r);
	}

	boost::signals2::signal< bool(std::string collection_name,std::string keys)> sigcreate_index;

	void call_create_index(boost::shared_ptr<Fossilizid::juggle::channel> ch, boost::shared_ptr<boost::unordered_map<std::string, boost::any> > v){
		auto collection_name = boost::any_cast<std::string>((*v)["collection_name"]);
		auto keys = boost::any_cast<std::string>((*v)["keys"]);
		auto ret = sigcreate_index(collection_name, keys);
		boost::shared_ptr<boost::unordered_map<std::string, boost::any> > r = boost::make_shared<boost::unordered_map<std::string, boost::any> >();
		(*r)["suuid"] = boost::any_cast<std::string>((*v)["suuid"]);
		(*r)["method"] = boost::any_cast<std::string>((*v)["method"]);
		(*r)["rpcevent"] = "reply_rpc_method";

		(*r)["ret"] = ret;
		ch->push(r);
	}

	boost::signals2::signal< bool(std::string collection,std::string index_name)> sigdrop_index;

	void call_drop_index(boost::shared_ptr<Fossilizid::juggle::channel> ch, boost::shared_ptr<boost::unordered_map<std::string, boost::any> > v){
		auto collection = boost::any_cast<std::string>((*v)["collection"]);
		auto index_name = boost::any_cast<std::string>((*v)["index_name"]);
		auto ret = sigdrop_index(collection, index_name);
		boost::shared_ptr<boost::unordered_map<std::string, boost::any> > r = boost::make_shared<boost::unordered_map<std::string, boost::any> >();
		(*r)["suuid"] = boost::any_cast<std::string>((*v)["suuid"]);
		(*r)["method"] = boost::any_cast<std::string>((*v)["method"]);
		(*r)["rpcevent"] = "reply_rpc_method";

		(*r)["ret"] = ret;
		ch->push(r);
	}

	boost::signals2::signal< std::vector<boost::shared_ptr<boost::unordered_map<std::string, boost::any> > > (std::string collection)> sigfind_indexes;

	void call_find_indexes(boost::shared_ptr<Fossilizid::juggle::channel> ch, boost::shared_ptr<boost::unordered_map<std::string, boost::any> > v){
		auto collection = boost::any_cast<std::string>((*v)["collection"]);
		auto ret = sigfind_indexes(collection);
		boost::shared_ptr<boost::unordered_map<std::string, boost::any> > r = boost::make_shared<boost::unordered_map<std::string, boost::any> >();
		(*r)["suuid"] = boost::any_cast<std::string>((*v)["suuid"]);
		(*r)["method"] = boost::any_cast<std::string>((*v)["method"]);
		(*r)["rpcevent"] = "reply_rpc_method";

		(*r)["ret"] = ret;
		ch->push(r);
	}

	boost::signals2::signal< int64_t(std::string collection_name,boost::shared_ptr<boost::unordered_map<std::string, boost::any> > query,int64_t skip,int64_t limit)> sigcount;

	void call_count(boost::shared_ptr<Fossilizid::juggle::channel> ch, boost::shared_ptr<boost::unordered_map<std::string, boost::any> > v){
		auto collection_name = boost::any_cast<std::string>((*v)["collection_name"]);
		auto query = boost::any_cast<boost::shared_ptr<boost::unordered_map<std::string, boost::any> > >((*v)["query"]);
		auto skip = boost::any_cast<int64_t>((*v)["skip"]);
		auto limit = boost::any_cast<int64_t>((*v)["limit"]);
		auto ret = sigcount(collection_name, query, skip, limit);
		boost::shared_ptr<boost::unordered_map<std::string, boost::any> > r = boost::make_shared<boost::unordered_map<std::string, boost::any> >();
		(*r)["suuid"] = boost::any_cast<std::string>((*v)["suuid"]);
		(*r)["method"] = boost::any_cast<std::string>((*v)["method"]);
		(*r)["rpcevent"] = "reply_rpc_method";

		(*r)["ret"] = ret;
		ch->push(r);
	}

	boost::signals2::signal< bool(std::string collection,boost::shared_ptr<boost::unordered_map<std::string, boost::any> > document)> siginsert;

	void call_insert(boost::shared_ptr<Fossilizid::juggle::channel> ch, boost::shared_ptr<boost::unordered_map<std::string, boost::any> > v){
		auto collection = boost::any_cast<std::string>((*v)["collection"]);
		auto document = boost::any_cast<boost::shared_ptr<boost::unordered_map<std::string, boost::any> > >((*v)["document"]);
		auto ret = siginsert(collection, document);
		boost::shared_ptr<boost::unordered_map<std::string, boost::any> > r = boost::make_shared<boost::unordered_map<std::string, boost::any> >();
		(*r)["suuid"] = boost::any_cast<std::string>((*v)["suuid"]);
		(*r)["method"] = boost::any_cast<std::string>((*v)["method"]);
		(*r)["rpcevent"] = "reply_rpc_method";

		(*r)["ret"] = ret;
		ch->push(r);
	}

	boost::signals2::signal< bool(std::string collection,boost::shared_ptr<boost::unordered_map<std::string, boost::any> > document)> sigsave;

	void call_save(boost::shared_ptr<Fossilizid::juggle::channel> ch, boost::shared_ptr<boost::unordered_map<std::string, boost::any> > v){
		auto collection = boost::any_cast<std::string>((*v)["collection"]);
		auto document = boost::any_cast<boost::shared_ptr<boost::unordered_map<std::string, boost::any> > >((*v)["document"]);
		auto ret = sigsave(collection, document);
		boost::shared_ptr<boost::unordered_map<std::string, boost::any> > r = boost::make_shared<boost::unordered_map<std::string, boost::any> >();
		(*r)["suuid"] = boost::any_cast<std::string>((*v)["suuid"]);
		(*r)["method"] = boost::any_cast<std::string>((*v)["method"]);
		(*r)["rpcevent"] = "reply_rpc_method";

		(*r)["ret"] = ret;
		ch->push(r);
	}

	boost::signals2::signal< bool(std::string collection,boost::shared_ptr<boost::unordered_map<std::string, boost::any> > query,boost::shared_ptr<boost::unordered_map<std::string, boost::any> > update)> sigupdate;

	void call_update(boost::shared_ptr<Fossilizid::juggle::channel> ch, boost::shared_ptr<boost::unordered_map<std::string, boost::any> > v){
		auto collection = boost::any_cast<std::string>((*v)["collection"]);
		auto query = boost::any_cast<boost::shared_ptr<boost::unordered_map<std::string, boost::any> > >((*v)["query"]);
		auto update = boost::any_cast<boost::shared_ptr<boost::unordered_map<std::string, boost::any> > >((*v)["update"]);
		auto ret = sigupdate(collection, query, update);
		boost::shared_ptr<boost::unordered_map<std::string, boost::any> > r = boost::make_shared<boost::unordered_map<std::string, boost::any> >();
		(*r)["suuid"] = boost::any_cast<std::string>((*v)["suuid"]);
		(*r)["method"] = boost::any_cast<std::string>((*v)["method"]);
		(*r)["rpcevent"] = "reply_rpc_method";

		(*r)["ret"] = ret;
		ch->push(r);
	}

	boost::signals2::signal< bool(std::string collection,boost::shared_ptr<boost::unordered_map<std::string, boost::any> > query)> sigremove;

	void call_remove(boost::shared_ptr<Fossilizid::juggle::channel> ch, boost::shared_ptr<boost::unordered_map<std::string, boost::any> > v){
		auto collection = boost::any_cast<std::string>((*v)["collection"]);
		auto query = boost::any_cast<boost::shared_ptr<boost::unordered_map<std::string, boost::any> > >((*v)["query"]);
		auto ret = sigremove(collection, query);
		boost::shared_ptr<boost::unordered_map<std::string, boost::any> > r = boost::make_shared<boost::unordered_map<std::string, boost::any> >();
		(*r)["suuid"] = boost::any_cast<std::string>((*v)["suuid"]);
		(*r)["method"] = boost::any_cast<std::string>((*v)["method"]);
		(*r)["rpcevent"] = "reply_rpc_method";

		(*r)["ret"] = ret;
		ch->push(r);
	}

	boost::signals2::signal< std::vector<boost::shared_ptr<boost::unordered_map<std::string, boost::any> > > (std::string collection,int64_t skip,int64_t limit,int64_t batch_size,boost::shared_ptr<boost::unordered_map<std::string, boost::any> > query,std::vector<std::string>  fields)> sigfind;

	void call_find(boost::shared_ptr<Fossilizid::juggle::channel> ch, boost::shared_ptr<boost::unordered_map<std::string, boost::any> > v){
		auto collection = boost::any_cast<std::string>((*v)["collection"]);
		auto skip = boost::any_cast<int64_t>((*v)["skip"]);
		auto limit = boost::any_cast<int64_t>((*v)["limit"]);
		auto batch_size = boost::any_cast<int64_t>((*v)["batch_size"]);
		auto query = boost::any_cast<boost::shared_ptr<boost::unordered_map<std::string, boost::any> > >((*v)["query"]);
		std::vector<std::string> fields;
		for(int i = 0; i < boost::any_cast<std::vector<boost::any> >((*v)["fields"]).size(); i++){
			fields.push_back(boost::any_cast<std::string>(boost::any_cast<std::vector<boost::any> >((*v)["fields"])[i]));
		}
		auto ret = sigfind(collection, skip, limit, batch_size, query, fields);
		boost::shared_ptr<boost::unordered_map<std::string, boost::any> > r = boost::make_shared<boost::unordered_map<std::string, boost::any> >();
		(*r)["suuid"] = boost::any_cast<std::string>((*v)["suuid"]);
		(*r)["method"] = boost::any_cast<std::string>((*v)["method"]);
		(*r)["rpcevent"] = "reply_rpc_method";

		(*r)["ret"] = ret;
		ch->push(r);
	}

	boost::signals2::signal< boost::shared_ptr<boost::unordered_map<std::string, boost::any> >(std::string collection,boost::shared_ptr<boost::unordered_map<std::string, boost::any> > query,boost::shared_ptr<boost::unordered_map<std::string, boost::any> > sort,boost::shared_ptr<boost::unordered_map<std::string, boost::any> > update,std::vector<std::string>  fields,bool _remove,bool upsert,bool _new)> sigfind_and_modify;

	void call_find_and_modify(boost::shared_ptr<Fossilizid::juggle::channel> ch, boost::shared_ptr<boost::unordered_map<std::string, boost::any> > v){
		auto collection = boost::any_cast<std::string>((*v)["collection"]);
		auto query = boost::any_cast<boost::shared_ptr<boost::unordered_map<std::string, boost::any> > >((*v)["query"]);
		auto sort = boost::any_cast<boost::shared_ptr<boost::unordered_map<std::string, boost::any> > >((*v)["sort"]);
		auto update = boost::any_cast<boost::shared_ptr<boost::unordered_map<std::string, boost::any> > >((*v)["update"]);
		std::vector<std::string> fields;
		for(int i = 0; i < boost::any_cast<std::vector<boost::any> >((*v)["fields"]).size(); i++){
			fields.push_back(boost::any_cast<std::string>(boost::any_cast<std::vector<boost::any> >((*v)["fields"])[i]));
		}
		auto _remove = boost::any_cast<bool>((*v)["_remove"]);
		auto upsert = boost::any_cast<bool>((*v)["upsert"]);
		auto _new = boost::any_cast<bool>((*v)["_new"]);
		auto ret = sigfind_and_modify(collection, query, sort, update, fields, _remove, upsert, _new);
		boost::shared_ptr<boost::unordered_map<std::string, boost::any> > r = boost::make_shared<boost::unordered_map<std::string, boost::any> >();
		(*r)["suuid"] = boost::any_cast<std::string>((*v)["suuid"]);
		(*r)["method"] = boost::any_cast<std::string>((*v)["method"]);
		(*r)["rpcevent"] = "reply_rpc_method";

		(*r)["ret"] = ret;
		ch->push(r);
	}

	boost::signals2::signal< boost::shared_ptr<boost::unordered_map<std::string, boost::any> >(std::string collection)> sigvalidate;

	void call_validate(boost::shared_ptr<Fossilizid::juggle::channel> ch, boost::shared_ptr<boost::unordered_map<std::string, boost::any> > v){
		auto collection = boost::any_cast<std::string>((*v)["collection"]);
		auto ret = sigvalidate(collection);
		boost::shared_ptr<boost::unordered_map<std::string, boost::any> > r = boost::make_shared<boost::unordered_map<std::string, boost::any> >();
		(*r)["suuid"] = boost::any_cast<std::string>((*v)["suuid"]);
		(*r)["method"] = boost::any_cast<std::string>((*v)["method"]);
		(*r)["rpcevent"] = "reply_rpc_method";

		(*r)["ret"] = ret;
		ch->push(r);
	}

	boost::signals2::signal< boost::shared_ptr<boost::unordered_map<std::string, boost::any> >(std::string collection)> sigstats;

	void call_stats(boost::shared_ptr<Fossilizid::juggle::channel> ch, boost::shared_ptr<boost::unordered_map<std::string, boost::any> > v){
		auto collection = boost::any_cast<std::string>((*v)["collection"]);
		auto ret = sigstats(collection);
		boost::shared_ptr<boost::unordered_map<std::string, boost::any> > r = boost::make_shared<boost::unordered_map<std::string, boost::any> >();
		(*r)["suuid"] = boost::any_cast<std::string>((*v)["suuid"]);
		(*r)["method"] = boost::any_cast<std::string>((*v)["method"]);
		(*r)["rpcevent"] = "reply_rpc_method";

		(*r)["ret"] = ret;
		ch->push(r);
	}

};

}
