/*
 * modules.cpp
 *
 *  Created on: 2015-1-14
 *      Author: qianqians
 */
#include "../interface/process.h"

#include "globalhandle.h"
#include "service.h"

namespace Fossilizid{
namespace juggle {

process::process(){
	(boost::static_pointer_cast<juggleservice>(_service_handle))->add_process(shared_from_this());
}

process::~process(){
	{
		boost::mutex::scoped_lock l(mu_method_map);
		method_map.clear();
	}

	{
		boost::mutex::scoped_lock l(mu_method_callback_map);
		method_callback_map.clear();
	}

	{
		boost::mutex::scoped_lock l(mu_channel);
		set_channel.clear();
	}

	{
		boost::mutex::scoped_lock l(mu_new_channel);
		array_new_channel.clear();
	}
}

void process::add_rpcsession(boost::shared_ptr<channel> ch){
	boost::mutex::scoped_lock l(mu_new_channel);
	array_new_channel.push_back(ch);
}
void process::remove_rpcsession(boost::shared_ptr<channel> ch){
	boost::mutex::scoped_lock l(mu_channel);
	set_channel.erase(ch);
}

void process::register_module_method(std::string methodname, boost::function<void(boost::shared_ptr<channel>, boost::shared_ptr<boost::unordered_map<std::string, boost::any> > ) > modulemethod){
	boost::mutex::scoped_lock l(mu_method_map);
	method_map.insert(std::make_pair(methodname, modulemethod));
}

void process::register_rpc_callback(uuid::uuid suuid, boost::function<void(boost::shared_ptr<boost::unordered_map<std::string, boost::any > > ) > callback){
	boost::mutex::scoped_lock l(mu_method_callback_map);
	method_callback_map.insert(std::make_pair(suuid, callback));
}

void process::run(){
	{
		boost::mutex::scoped_lock lmu_channel(mu_channel);
		for (auto ch : set_channel){
			(boost::static_pointer_cast<juggleservice>(_service_handle))->set_current_channel(ch);

			boost::shared_ptr<boost::unordered_map<std::string, boost::any> > cmd = ch->pop();
			if (cmd == 0){
				continue;
			}

			if (boost::any_cast<std::string>((*cmd)["rpcevent"]) == "call_rpc_method"){
				auto find = method_map.find(boost::any_cast<std::string>((*cmd)["rpcevent"]));
				if (find != method_map.end()){
					find->second(ch, cmd);
				}
			} else if (boost::any_cast<std::string>((*cmd)["rpcevent"]) == "reply_rpc_method"){
				auto find = method_callback_map.find(boost::any_cast<std::string>((*cmd)["rpcevent"]));
				if (find != method_callback_map.end()){
					find->second(cmd);
				}
			}
		}
	}

	{
		boost::mutex::scoped_lock lmu_new_channel(mu_new_channel);
		boost::mutex::scoped_lock lmu_channel(mu_channel);
		for (auto ch : array_new_channel){
			set_channel.insert(ch);
		}
	}
}

} /* namespace juggle */
} /* namespace Fossilizid */