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

class routing: public Fossilizid::juggle::caller{
public:
	routing(boost::shared_ptr<Fossilizid::juggle::process> __process, boost::shared_ptr<Fossilizid::juggle::channel> ch) : caller(__process, ch, "routing"){
	}

	~routing(){
	}

	void register_user(std::string uuid,int64_t gatenum,int64_t logicnum,int64_t dbnum){
		boost::shared_ptr<boost::unordered_map<std::string, boost::any> > v = boost::make_shared<boost::unordered_map<std::string, boost::any> >();
		(*v)["uuid"] = uuid;
		(*v)["gatenum"] = gatenum;
		(*v)["logicnum"] = logicnum;
		(*v)["dbnum"] = dbnum;
		boost::shared_ptr<boost::unordered_map<std::string, boost::any> > r = call_module_method_sync("routing_register_user", v);

	}

	void unregister_user(std::string uuid){
		boost::shared_ptr<boost::unordered_map<std::string, boost::any> > v = boost::make_shared<boost::unordered_map<std::string, boost::any> >();
		(*v)["uuid"] = uuid;
		boost::shared_ptr<boost::unordered_map<std::string, boost::any> > r = call_module_method_sync("routing_unregister_user", v);

	}

	std::vector<int64_t>  get_user(std::string uuid){
		boost::shared_ptr<boost::unordered_map<std::string, boost::any> > v = boost::make_shared<boost::unordered_map<std::string, boost::any> >();
		(*v)["uuid"] = uuid;
		boost::shared_ptr<boost::unordered_map<std::string, boost::any> > r = call_module_method_sync("routing_get_user", v);
        std::vector<int64_t> ret;
        for(int i = 0; i < boost::any_cast<std::vector<boost::any> >((*r)["ret"]).size(); i++){
	        ret.push_back(boost::any_cast<int64_t>(boost::any_cast<std::vector<boost::any> >((*r)["ret"])[i]));
        }
        return ret;
	}

};

}

namespace async{

class routing: public Fossilizid::juggle::caller{
public:
	routing(boost::shared_ptr<Fossilizid::juggle::process> __process, boost::shared_ptr<Fossilizid::juggle::channel> ch) : caller(__process, ch, "routing"){
	}

	~routing(){
	}

	void register_user(std::string uuid, int64_t gatenum, int64_t logicnum, int64_t dbnum, boost::function<void(void)> callback){
		boost::shared_ptr<boost::unordered_map<std::string, boost::any> > v = boost::make_shared<boost::unordered_map<std::string, boost::any> >();
		(*v)["uuid"] = uuid;
		(*v)["gatenum"] = gatenum;
		(*v)["logicnum"] = logicnum;
		(*v)["dbnum"] = dbnum;
		auto cb = [this, callback](boost::shared_ptr<boost::unordered_map<std::string, boost::any> > r){
		};
		call_module_method_async("routing_register_user", v, cb);
	}

	void unregister_user(std::string uuid, boost::function<void(void)> callback){
		boost::shared_ptr<boost::unordered_map<std::string, boost::any> > v = boost::make_shared<boost::unordered_map<std::string, boost::any> >();
		(*v)["uuid"] = uuid;
		auto cb = [this, callback](boost::shared_ptr<boost::unordered_map<std::string, boost::any> > r){
		};
		call_module_method_async("routing_unregister_user", v, cb);
	}

	std::vector<int64_t>  get_user(std::string uuid, boost::function<void(std::vector<int64_t> )> callback){
		boost::shared_ptr<boost::unordered_map<std::string, boost::any> > v = boost::make_shared<boost::unordered_map<std::string, boost::any> >();
		(*v)["uuid"] = uuid;
		auto cb = [this, callback](boost::shared_ptr<boost::unordered_map<std::string, boost::any> > r){
           std::vector<int64_t> ret;
        for(int i = 0; i < boost::any_cast<std::vector<boost::any> >((*r)["ret"]).size(); i++){
	        ret.push_back(boost::any_cast<int64_t>(boost::any_cast<std::vector<boost::any> >((*r)["ret"])[i]));
        }
			callback(ret);
        };
		call_module_method_async("routing_get_user", v, cb);
	}

};

}

