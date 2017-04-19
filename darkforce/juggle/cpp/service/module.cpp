/*
 * module.cpp
 *
 *  Created on: 2015-1-14
 *      Author: qianqians
 */
#include "module.h"

#include <boost/make_shared.hpp>

namespace Fossilizid{
namespace juggle {

module::module(boost::shared_ptr<process> __process,  std::string modulename, uuid::uuid & moduleid){
	_process = __process;
	_module_name = modulename;
	_module_id = moduleid;
}

module::~module(){
}

std::string module::module_name(){
	return _module_name;
}

uuid::uuid module::module_id(){
	return _module_id;
}

void module::moudle_info(boost::shared_ptr<boost::unordered_map<std::string, boost::any> > & info){
	(*info)["module_name"] = module_name();
	(*info)["module_id"] = module_id();
	(*info)["module_func"] = _module_func;
}

void module::call_module_method(boost::shared_ptr<channel> ch, boost::shared_ptr<boost::unordered_map<std::string, boost::any> > value){
}

} /* namespace juggle */
} /* namespace Fossilizid */