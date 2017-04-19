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

class logic: public Fossilizid::juggle::module{
public:
	logic(boost::shared_ptr<Fossilizid::juggle::process> __process) : module(__process, "logic", Fossilizid::uuid::UUID()){
		_module_func.push_back("logic_register_user");
		_module_func.push_back("logic_unregister_user");
		_module_func.push_back("logic_register_gate");
		_module_func.push_back("logic_register_db");
		__process->register_module_method("logic_register_user", boost::bind(&logic::call_register_user, this, _1, _2));
		__process->register_module_method("logic_unregister_user", boost::bind(&logic::call_unregister_user, this, _1, _2));
		__process->register_module_method("logic_register_gate", boost::bind(&logic::call_register_gate, this, _1, _2));
		__process->register_module_method("logic_register_db", boost::bind(&logic::call_register_db, this, _1, _2));
	}

	~logic(){
	}

	boost::signals2::signal< bool(std::string uuid)> sigregister_user;

	void call_register_user(boost::shared_ptr<Fossilizid::juggle::channel> ch, boost::shared_ptr<boost::unordered_map<std::string, boost::any> > v){
		auto uuid = boost::any_cast<std::string>((*v)["uuid"]);
		auto ret = sigregister_user(uuid);
		boost::shared_ptr<boost::unordered_map<std::string, boost::any> > r = boost::make_shared<boost::unordered_map<std::string, boost::any> >();
		(*r)["suuid"] = boost::any_cast<std::string>((*v)["suuid"]);
		(*r)["method"] = boost::any_cast<std::string>((*v)["method"]);
		(*r)["rpcevent"] = "reply_rpc_method";

		(*r)["ret"] = ret;
		ch->push(r);
	}

	boost::signals2::signal< void(std::string uuid)> sigunregister_user;

	void call_unregister_user(boost::shared_ptr<Fossilizid::juggle::channel> ch, boost::shared_ptr<boost::unordered_map<std::string, boost::any> > v){
		auto uuid = boost::any_cast<std::string>((*v)["uuid"]);
		sigunregister_user(uuid);
		boost::shared_ptr<boost::unordered_map<std::string, boost::any> > r = boost::make_shared<boost::unordered_map<std::string, boost::any> >();
		(*r)["suuid"] = boost::any_cast<std::string>((*v)["suuid"]);
		(*r)["method"] = boost::any_cast<std::string>((*v)["method"]);
		(*r)["rpcevent"] = "reply_rpc_method";

		ch->push(r);
	}

	boost::signals2::signal< void(int64_t gatenum,std::string ip,int64_t port)> sigregister_gate;

	void call_register_gate(boost::shared_ptr<Fossilizid::juggle::channel> ch, boost::shared_ptr<boost::unordered_map<std::string, boost::any> > v){
		auto gatenum = boost::any_cast<int64_t>((*v)["gatenum"]);
		auto ip = boost::any_cast<std::string>((*v)["ip"]);
		auto port = boost::any_cast<int64_t>((*v)["port"]);
		sigregister_gate(gatenum, ip, port);
		boost::shared_ptr<boost::unordered_map<std::string, boost::any> > r = boost::make_shared<boost::unordered_map<std::string, boost::any> >();
		(*r)["suuid"] = boost::any_cast<std::string>((*v)["suuid"]);
		(*r)["method"] = boost::any_cast<std::string>((*v)["method"]);
		(*r)["rpcevent"] = "reply_rpc_method";

		ch->push(r);
	}

	boost::signals2::signal< void(int64_t dbnum,std::string ip,int64_t port)> sigregister_db;

	void call_register_db(boost::shared_ptr<Fossilizid::juggle::channel> ch, boost::shared_ptr<boost::unordered_map<std::string, boost::any> > v){
		auto dbnum = boost::any_cast<int64_t>((*v)["dbnum"]);
		auto ip = boost::any_cast<std::string>((*v)["ip"]);
		auto port = boost::any_cast<int64_t>((*v)["port"]);
		sigregister_db(dbnum, ip, port);
		boost::shared_ptr<boost::unordered_map<std::string, boost::any> > r = boost::make_shared<boost::unordered_map<std::string, boost::any> >();
		(*r)["suuid"] = boost::any_cast<std::string>((*v)["suuid"]);
		(*r)["method"] = boost::any_cast<std::string>((*v)["method"]);
		(*r)["rpcevent"] = "reply_rpc_method";

		ch->push(r);
	}

};

}
