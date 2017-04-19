/*
 * caller.cpp
 *
 *  Created on: 2014-10-8
 *      Author: qianqians
 */
#include "caller.h"
#include "service.h"
#include "globalhandle.h"

#include "../interface/semaphore.h"

#include <boost/make_shared.hpp>

namespace Fossilizid{
namespace juggle {

caller::caller(boost::shared_ptr<process> __process, boost::shared_ptr<channel> ch, std::string modulename){
	_process = __process;
	_module_name = modulename;
	_ch = ch;
}

caller::~caller(){
}

std::string caller::module_name(){
	return _module_name;
}

uuid::uuid caller::module_id(){
	return _module_id;
}

boost::shared_ptr<boost::unordered_map<std::string, boost::any> > caller::call_module_method_sync(std::string methodname, boost::shared_ptr<boost::unordered_map<std::string, boost::any> > value){
	semaphore s;
	call_module_method_async(methodname, value, boost::bind(&semaphore::post, &s, _1));
	return s.wait(-1);
}

void caller::call_module_method_async(std::string methodname, boost::shared_ptr<boost::unordered_map<std::string, boost::any> > value, boost::function<void(boost::shared_ptr<boost::unordered_map<std::string, boost::any> > ) > callback){
	(*value)["method"] = methodname;
	(*value)["suuid"] = uuid::UUID();
	(*value)["rpcevent"] = "call_rpc_method";

	_process->register_rpc_callback(boost::any_cast<std::string>((*value)["suuid"]), callback);
	
	_ch->push(value);
}

} /* namespace juggle */
} /* namespace Fossilizid */