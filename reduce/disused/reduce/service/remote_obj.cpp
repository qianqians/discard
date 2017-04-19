/*
 * remote_obj.cpp
 *
 *  Created on: 2014-10-8
 *      Author: qianqians
 */
#include "remote_obj.h"
#include "session.h"

namespace Fossilizid{
namespace reduce {

remote_obj::remote_obj(boost::shared_ptr<session> session, Json::Value & value){
	Json::Value _class_name_ = value.get("classname", Json::nullValue);
	if (!_class_name_.isNull() && _class_name_.isString()){
		_class_name = _class_name_.asString();
	}
	else{
		throw std::exception("error obj info");
	}

	Json::Value _objid_ = value.get("objid", Json::nullValue);
	if (!_objid_.isNull() && _objid_.isString()){
		_objid = _objid_.asString();
	}
	else {
		throw std::exception("error obj info");
	}

	reqsession = session;
}

remote_obj::~remote_obj(){
}

void remote_obj::call_rpc_mothed(Json::Value & value, boost::function<void(Json::Value &) > rpccallback){
	value["suuid"] = UUID();
	
	boost::mutex::scoped_lock lock(mu_call_rpc_mothed_ret_callback);
	call_rpc_mothed_ret_callback.insert(std::make_pair(value["suuid"].asString(), rpccallback));
	
	call_rpc_mothed(value);
}

void remote_obj::call_rpc_mothed(Json::Value & value){
	Json::Value _uuid = value.get("suuid", Json::nullValue);
	if (_uuid.isNull()){
		return;
	}

	if (reqsession != 0){
		reqsession->do_async_push(reqsession, value);
	}else{
		Json::Reader reader;
		Json::Value ret;
		if (reader.parse(std::string("{\"exception\":\"fnname is null\"}"), ret)){
			ret["suuid"] = _uuid;
			return call_rpc_mothed_ret(ret);
		}
	}
}

} /* namespace reduce */
} /* namespace Fossilizid */