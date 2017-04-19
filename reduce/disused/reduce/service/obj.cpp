/*
 * obj.cpp
 *
 *  Created on: 2014-10-14
 *      Author: qianqians
 */
#include "service.h"
#include "obj.h"
#include "session.h"

namespace Fossilizid{
namespace reduce {

obj::obj(){
	states.store(obj_state_working);
}

obj::~obj(){
}

std::string obj::class_name(){
	return _class_name;
}

uuid obj::objid(){
	return _objid;
}

void obj::push_rpc_event(std::pair<boost::shared_ptr<session>, Json::Value> & _event){
	cmdque.push(_event);
}

void obj::call_do_logic(){
	if (states.load() == obj_state_pause){
		return;
	}

	std::pair<boost::shared_ptr<session>, Json::Value> cmd;
	while (cmdque.pop(cmd)){
		boost::shared_ptr<session> & session = cmd.first;
		Json::Value & value = cmd.second;

		call_rpc_event(session, value);
	}
}

void obj::call_rpc_event(boost::shared_ptr<session> session, Json::Value & value){
	Json::Value _uuid = value.get("suuid", Json::nullValue);
	if (_uuid.isNull()){
		return;
	}

	Json::Value rpctype = value.get("rpc_event_type", Json::nullValue);
	if (rpctype.isNull()){
		return;
	}
	if (rpctype.asString() == "call_rpc_mothed"){
		boost::mutex::scoped_lock lock(mu_call_rpc_mothed_ret_callback);
		call_rpc_mothed_ret_callback.insert(std::make_pair(_uuid.asString(), boost::bind(&obj::push_rcp_mothed_ret, this, session, _1)));

		call_rpc_mothed(value);
	}else if (rpctype.asString() == "call_rpc_mothed_ret"){
		call_rpc_mothed_ret(value);
	}
}

void obj::call_do_time(boost::uint64_t time){
	boost::mutex::scoped_lock lock(mu_call_time_callback);
	std::map<boost::uint64_t, boost::function<void(boost::uint64_t) > >::iterator it = call_time_callback.upper_bound(time);
	auto begin = call_time_callback.begin(); 
	for (; begin != it; begin++){
		begin->second(time);
	}
	call_time_callback.erase(begin, it);
}

void obj::call_add_time(boost::uint64_t time, boost::function<void(boost::uint64_t)> timefn){
	boost::mutex::scoped_lock lock(mu_call_time_callback);
	call_time_callback.insert(std::make_pair(time, timefn));
}

void obj::call_rpc_mothed(Json::Value & value){
}

void obj::call_rpc_mothed_ret(Json::Value & value){
	Json::Value _uuid = value.get("suuid", Json::nullValue);
	if (_uuid.isNull()){
		return;
	}

	std::unordered_map<uuid, boost::function<void(Json::Value &)> >::iterator it = call_rpc_mothed_ret_callback.find(_uuid.asString());
	if (it != call_rpc_mothed_ret_callback.end()){
		if (it->second != 0){
			it->second(value);
		}
	}
}

void obj::push_rcp_mothed_ret(boost::shared_ptr<session> session, Json::Value & value){
	Json::Value _epuuid = value.get("epuuid", Json::nullValue);
	if (_epuuid.isNull()){
		return;
	}

	do {
		if (session != 0){
			if (session->do_async_push(session, value)){
				break;
			}
		}

		session = _service_handle->get_rpcsession(_epuuid.asString());
		if (session != 0){
			session->do_async_push(session, value);
		}
	} while (0);
}

} /* namespace reduce */
} /* namespace Fossilizid */