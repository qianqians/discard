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

class logic: public Fossilizid::juggle::caller{
public:
	logic(boost::shared_ptr<Fossilizid::juggle::process> __process, boost::shared_ptr<Fossilizid::juggle::channel> ch) : caller(__process, ch, "logic"){
	}

	~logic(){
	}

	bool register_user(std::string uuid){
		boost::shared_ptr<boost::unordered_map<std::string, boost::any> > v = boost::make_shared<boost::unordered_map<std::string, boost::any> >();
		(*v)["uuid"] = uuid;
		boost::shared_ptr<boost::unordered_map<std::string, boost::any> > r = call_module_method_sync("logic_register_user", v);
        return boost::any_cast<bool>((*r)["ret"]);
	}

	void unregister_user(std::string uuid){
		boost::shared_ptr<boost::unordered_map<std::string, boost::any> > v = boost::make_shared<boost::unordered_map<std::string, boost::any> >();
		(*v)["uuid"] = uuid;
		boost::shared_ptr<boost::unordered_map<std::string, boost::any> > r = call_module_method_sync("logic_unregister_user", v);

	}

	void register_gate(int64_t gatenum,std::string ip,int64_t port){
		boost::shared_ptr<boost::unordered_map<std::string, boost::any> > v = boost::make_shared<boost::unordered_map<std::string, boost::any> >();
		(*v)["gatenum"] = gatenum;
		(*v)["ip"] = ip;
		(*v)["port"] = port;
		boost::shared_ptr<boost::unordered_map<std::string, boost::any> > r = call_module_method_sync("logic_register_gate", v);

	}

	void register_db(int64_t dbnum,std::string ip,int64_t port){
		boost::shared_ptr<boost::unordered_map<std::string, boost::any> > v = boost::make_shared<boost::unordered_map<std::string, boost::any> >();
		(*v)["dbnum"] = dbnum;
		(*v)["ip"] = ip;
		(*v)["port"] = port;
		boost::shared_ptr<boost::unordered_map<std::string, boost::any> > r = call_module_method_sync("logic_register_db", v);

	}

};

}

namespace async{

class logic: public Fossilizid::juggle::caller{
public:
	logic(boost::shared_ptr<Fossilizid::juggle::process> __process, boost::shared_ptr<Fossilizid::juggle::channel> ch) : caller(__process, ch, "logic"){
	}

	~logic(){
	}

	bool register_user(std::string uuid, boost::function<void(bool)> callback){
		boost::shared_ptr<boost::unordered_map<std::string, boost::any> > v = boost::make_shared<boost::unordered_map<std::string, boost::any> >();
		(*v)["uuid"] = uuid;
		auto cb = [this, callback](boost::shared_ptr<boost::unordered_map<std::string, boost::any> > r){
   auto ret = boost::any_cast<bool>((*r)["ret"]);			callback(ret);
        };
		call_module_method_async("logic_register_user", v, cb);
	}

	void unregister_user(std::string uuid, boost::function<void(void)> callback){
		boost::shared_ptr<boost::unordered_map<std::string, boost::any> > v = boost::make_shared<boost::unordered_map<std::string, boost::any> >();
		(*v)["uuid"] = uuid;
		auto cb = [this, callback](boost::shared_ptr<boost::unordered_map<std::string, boost::any> > r){
		};
		call_module_method_async("logic_unregister_user", v, cb);
	}

	void register_gate(int64_t gatenum, std::string ip, int64_t port, boost::function<void(void)> callback){
		boost::shared_ptr<boost::unordered_map<std::string, boost::any> > v = boost::make_shared<boost::unordered_map<std::string, boost::any> >();
		(*v)["gatenum"] = gatenum;
		(*v)["ip"] = ip;
		(*v)["port"] = port;
		auto cb = [this, callback](boost::shared_ptr<boost::unordered_map<std::string, boost::any> > r){
		};
		call_module_method_async("logic_register_gate", v, cb);
	}

	void register_db(int64_t dbnum, std::string ip, int64_t port, boost::function<void(void)> callback){
		boost::shared_ptr<boost::unordered_map<std::string, boost::any> > v = boost::make_shared<boost::unordered_map<std::string, boost::any> >();
		(*v)["dbnum"] = dbnum;
		(*v)["ip"] = ip;
		(*v)["port"] = port;
		auto cb = [this, callback](boost::shared_ptr<boost::unordered_map<std::string, boost::any> > r){
		};
		call_module_method_async("logic_register_db", v, cb);
	}

};

}

