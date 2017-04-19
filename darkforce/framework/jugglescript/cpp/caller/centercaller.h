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

class center: public Fossilizid::juggle::caller{
public:
	center(boost::shared_ptr<Fossilizid::juggle::process> __process, boost::shared_ptr<Fossilizid::juggle::channel> ch) : caller(__process, ch, "center"){
	}

	~center(){
	}

	int64_t register_logic(std::string ip,int64_t port){
		boost::shared_ptr<boost::unordered_map<std::string, boost::any> > v = boost::make_shared<boost::unordered_map<std::string, boost::any> >();
		(*v)["ip"] = ip;
		(*v)["port"] = port;
		boost::shared_ptr<boost::unordered_map<std::string, boost::any> > r = call_module_method_sync("center_register_logic", v);
        return boost::any_cast<int64_t>((*r)["ret"]);
	}

	int64_t register_gate(std::string ip,int64_t port){
		boost::shared_ptr<boost::unordered_map<std::string, boost::any> > v = boost::make_shared<boost::unordered_map<std::string, boost::any> >();
		(*v)["ip"] = ip;
		(*v)["port"] = port;
		boost::shared_ptr<boost::unordered_map<std::string, boost::any> > r = call_module_method_sync("center_register_gate", v);
        return boost::any_cast<int64_t>((*r)["ret"]);
	}

	int64_t register_routing(std::string ip,int64_t port){
		boost::shared_ptr<boost::unordered_map<std::string, boost::any> > v = boost::make_shared<boost::unordered_map<std::string, boost::any> >();
		(*v)["ip"] = ip;
		(*v)["port"] = port;
		boost::shared_ptr<boost::unordered_map<std::string, boost::any> > r = call_module_method_sync("center_register_routing", v);
        return boost::any_cast<int64_t>((*r)["ret"]);
	}

	int64_t register_db(std::string ip,int64_t port){
		boost::shared_ptr<boost::unordered_map<std::string, boost::any> > v = boost::make_shared<boost::unordered_map<std::string, boost::any> >();
		(*v)["ip"] = ip;
		(*v)["port"] = port;
		boost::shared_ptr<boost::unordered_map<std::string, boost::any> > r = call_module_method_sync("center_register_db", v);
        return boost::any_cast<int64_t>((*r)["ret"]);
	}

};

}

namespace async{

class center: public Fossilizid::juggle::caller{
public:
	center(boost::shared_ptr<Fossilizid::juggle::process> __process, boost::shared_ptr<Fossilizid::juggle::channel> ch) : caller(__process, ch, "center"){
	}

	~center(){
	}

	int64_t register_logic(std::string ip, int64_t port, boost::function<void(int64_t)> callback){
		boost::shared_ptr<boost::unordered_map<std::string, boost::any> > v = boost::make_shared<boost::unordered_map<std::string, boost::any> >();
		(*v)["ip"] = ip;
		(*v)["port"] = port;
		auto cb = [this, callback](boost::shared_ptr<boost::unordered_map<std::string, boost::any> > r){
   auto ret = boost::any_cast<int64_t>((*r)["ret"]);			callback(ret);
        };
		call_module_method_async("center_register_logic", v, cb);
	}

	int64_t register_gate(std::string ip, int64_t port, boost::function<void(int64_t)> callback){
		boost::shared_ptr<boost::unordered_map<std::string, boost::any> > v = boost::make_shared<boost::unordered_map<std::string, boost::any> >();
		(*v)["ip"] = ip;
		(*v)["port"] = port;
		auto cb = [this, callback](boost::shared_ptr<boost::unordered_map<std::string, boost::any> > r){
   auto ret = boost::any_cast<int64_t>((*r)["ret"]);			callback(ret);
        };
		call_module_method_async("center_register_gate", v, cb);
	}

	int64_t register_routing(std::string ip, int64_t port, boost::function<void(int64_t)> callback){
		boost::shared_ptr<boost::unordered_map<std::string, boost::any> > v = boost::make_shared<boost::unordered_map<std::string, boost::any> >();
		(*v)["ip"] = ip;
		(*v)["port"] = port;
		auto cb = [this, callback](boost::shared_ptr<boost::unordered_map<std::string, boost::any> > r){
   auto ret = boost::any_cast<int64_t>((*r)["ret"]);			callback(ret);
        };
		call_module_method_async("center_register_routing", v, cb);
	}

	int64_t register_db(std::string ip, int64_t port, boost::function<void(int64_t)> callback){
		boost::shared_ptr<boost::unordered_map<std::string, boost::any> > v = boost::make_shared<boost::unordered_map<std::string, boost::any> >();
		(*v)["ip"] = ip;
		(*v)["port"] = port;
		auto cb = [this, callback](boost::shared_ptr<boost::unordered_map<std::string, boost::any> > r){
   auto ret = boost::any_cast<int64_t>((*r)["ret"]);			callback(ret);
        };
		call_module_method_async("center_register_db", v, cb);
	}

};

}

