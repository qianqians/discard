/*
 * rpcproxy.cpp
 *
 *  Created on: 2015-3-31
 *      Author: qianqians
 */
#include "rpcproxy.h"

#include <juggle.h>

#include <boost/bind.hpp>

namespace Fossilizid{
namespace gate{

rpcproxy::rpcproxy(boost::function<boost::shared_ptr<juggle::channel>(boost::shared_ptr<juggle::channel>, uuid::uuid) > _routing){
	routing = _routing;
}

rpcproxy::~rpcproxy(){
}

void rpcproxy::run(){
	{
		boost::mutex::scoped_lock lmu_channel(mu_channel);
		for (auto ch : set_channel){
			boost::shared_ptr<boost::unordered_map<std::string, boost::any> > cmd = ch->pop();
			if (cmd == 0){
				continue;
			}

			if (boost::any_cast<std::string>((*cmd)["rpcevent"]) == "call_rpc_method"){
				proxy(ch, cmd);
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
	
void rpcproxy::proxy(boost::shared_ptr<juggle::channel> ch, boost::shared_ptr<boost::unordered_map<std::string, boost::any> > value){
	routing(ch, boost::any_cast<std::string>((*value)["userid"]))->push(value);
	juggle::process::register_rpc_callback(boost::any_cast<std::string>((*value)["suuid"]), boost::bind(&rpcproxy::handle_callback, this, ch, _1));
}

void rpcproxy::handle_callback(boost::shared_ptr<juggle::channel> ch, boost::shared_ptr<boost::unordered_map<std::string, boost::any> > result){
	ch->push(result);
}

} /* namespace gate */
} /* namespace Fossilizid */

