/*
 * locale_obj.cpp
 *
 *  Created on: 2014-10-8
 *      Author: qianqians
 */
#include "service.h"
#include "locale_obj.h"
#include "session.h"

namespace Fossilizid{
namespace reduce {

locale_obj::locale_obj(std::string _class_name_){
	_objid = UUID();
	_class_name = _class_name_;
}

locale_obj::~locale_obj(){
}

void locale_obj::register_rpc_mothed(std::pair<std::string, boost::function<Json::Value(Json::Value &) > > rpc_mothed){
	boost::mutex::scoped_lock lock(mu_maprpccall);
	maprpccall.insert(rpc_mothed);
}

void locale_obj::call_rpc_mothed(Json::Value & value){
	Json::Value _uuid = value.get("suuid", Json::nullValue);
	if (_uuid.isNull()){
		return;
	}

	Json::Value fnname = value.get("fnname", Json::nullValue);
	if (fnname.isNull()){
		Json::Reader reader;
		Json::Value ret;
		if (reader.parse(std::string("{\"exception\":\"fnname is null\"}"), ret)){
			ret["suuid"] = _uuid;
			return call_rpc_mothed_ret(ret);
		}
	}

	Json::Value fnargv = value.get("fnargv", Json::nullValue);
	if (fnargv.isNull()){
		Json::Reader reader;
		Json::Value ret;
		if (reader.parse(std::string("{\"exception\":\"fnargv is null\"}"), ret)){
			ret["suuid"] = _uuid;
			return call_rpc_mothed_ret(ret);
		}
	}

	std::map<std::string, boost::function<Json::Value(Json::Value &) > >::iterator find = maprpccall.find(fnname.asString());
	if (find == maprpccall.end()){
		Json::Reader reader;
		Json::Value ret;
		if (reader.parse(std::string("{\"exception\":\"fn is not find\"}"), ret)){
			ret["suuid"] = _uuid;
			return call_rpc_mothed_ret(ret);
		}
	}

	Json::Value fnret = find->second(fnargv);
	fnret["suuid"] = _uuid;
	call_rpc_mothed_ret(fnret);
}

} /* namespace reduce */
} /* namespace Fossilizid */