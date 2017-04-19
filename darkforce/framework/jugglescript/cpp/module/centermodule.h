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

class center: public Fossilizid::juggle::module{
public:
	center(boost::shared_ptr<Fossilizid::juggle::process> __process) : module(__process, "center", Fossilizid::uuid::UUID()){
		_module_func.push_back("center_register_logic");
		_module_func.push_back("center_register_gate");
		_module_func.push_back("center_register_routing");
		_module_func.push_back("center_register_db");
		__process->register_module_method("center_register_logic", boost::bind(&center::call_register_logic, this, _1, _2));
		__process->register_module_method("center_register_gate", boost::bind(&center::call_register_gate, this, _1, _2));
		__process->register_module_method("center_register_routing", boost::bind(&center::call_register_routing, this, _1, _2));
		__process->register_module_method("center_register_db", boost::bind(&center::call_register_db, this, _1, _2));
	}

	~center(){
	}

	boost::signals2::signal< int64_t(std::string ip,int64_t port)> sigregister_logic;

	void call_register_logic(boost::shared_ptr<Fossilizid::juggle::channel> ch, boost::shared_ptr<boost::unordered_map<std::string, boost::any> > v){
		auto ip = boost::any_cast<std::string>((*v)["ip"]);
		auto port = boost::any_cast<int64_t>((*v)["port"]);
		auto ret = sigregister_logic(ip, port);
		boost::shared_ptr<boost::unordered_map<std::string, boost::any> > r = boost::make_shared<boost::unordered_map<std::string, boost::any> >();
		(*r)["suuid"] = boost::any_cast<std::string>((*v)["suuid"]);
		(*r)["method"] = boost::any_cast<std::string>((*v)["method"]);
		(*r)["rpcevent"] = "reply_rpc_method";

		(*r)["ret"] = ret;
		ch->push(r);
	}

	boost::signals2::signal< int64_t(std::string ip,int64_t port)> sigregister_gate;

	void call_register_gate(boost::shared_ptr<Fossilizid::juggle::channel> ch, boost::shared_ptr<boost::unordered_map<std::string, boost::any> > v){
		auto ip = boost::any_cast<std::string>((*v)["ip"]);
		auto port = boost::any_cast<int64_t>((*v)["port"]);
		auto ret = sigregister_gate(ip, port);
		boost::shared_ptr<boost::unordered_map<std::string, boost::any> > r = boost::make_shared<boost::unordered_map<std::string, boost::any> >();
		(*r)["suuid"] = boost::any_cast<std::string>((*v)["suuid"]);
		(*r)["method"] = boost::any_cast<std::string>((*v)["method"]);
		(*r)["rpcevent"] = "reply_rpc_method";

		(*r)["ret"] = ret;
		ch->push(r);
	}

	boost::signals2::signal< int64_t(std::string ip,int64_t port)> sigregister_routing;

	void call_register_routing(boost::shared_ptr<Fossilizid::juggle::channel> ch, boost::shared_ptr<boost::unordered_map<std::string, boost::any> > v){
		auto ip = boost::any_cast<std::string>((*v)["ip"]);
		auto port = boost::any_cast<int64_t>((*v)["port"]);
		auto ret = sigregister_routing(ip, port);
		boost::shared_ptr<boost::unordered_map<std::string, boost::any> > r = boost::make_shared<boost::unordered_map<std::string, boost::any> >();
		(*r)["suuid"] = boost::any_cast<std::string>((*v)["suuid"]);
		(*r)["method"] = boost::any_cast<std::string>((*v)["method"]);
		(*r)["rpcevent"] = "reply_rpc_method";

		(*r)["ret"] = ret;
		ch->push(r);
	}

	boost::signals2::signal< int64_t(std::string ip,int64_t port)> sigregister_db;

	void call_register_db(boost::shared_ptr<Fossilizid::juggle::channel> ch, boost::shared_ptr<boost::unordered_map<std::string, boost::any> > v){
		auto ip = boost::any_cast<std::string>((*v)["ip"]);
		auto port = boost::any_cast<int64_t>((*v)["port"]);
		auto ret = sigregister_db(ip, port);
		boost::shared_ptr<boost::unordered_map<std::string, boost::any> > r = boost::make_shared<boost::unordered_map<std::string, boost::any> >();
		(*r)["suuid"] = boost::any_cast<std::string>((*v)["suuid"]);
		(*r)["method"] = boost::any_cast<std::string>((*v)["method"]);
		(*r)["rpcevent"] = "reply_rpc_method";

		(*r)["ret"] = ret;
		ch->push(r);
	}

};

}
