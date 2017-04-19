/*
 * session.cpp
 *
 *  Created on: 2014-10-8
 *      Author: qianqians
 */
#include "service.h"
#include "tempsession.h"
#include "rpcsession.h"
#include "obj.h"
#include "remote_obj.h"

#include "../../third_party/json/json_protocol.h"

namespace Fossilizid{
namespace reduce {

tempsession::tempsession(remoteq::CHANNEL _ch, std::string _service_class){
	ch = _ch;
	service_class = _service_class;
}

tempsession::~tempsession(){
}

void tempsession::do_time(boost::uint64_t time){
}

void tempsession::do_pop(boost::shared_ptr<session> session, Json::Value & value){
	Json::Value eventtype = value.get("eventtype", Json::nullValue);
	if (eventtype.isNull()){
		return;
	}
	
	if (eventtype.asString() == "rpc_event"){
	} else if (eventtype.asString() == "create_obj"){
	} else if (eventtype.asString() == "connect_server"){
		do_connect_server(session, value);
	} 
}

bool tempsession::do_async_push(boost::shared_ptr<session> session, Json::Value & value){
	if (session != 0){
		return remoteq::push(static_cast<tempsession*>(session.get())->ch, value, boost::bind(json_parser::json_to_buf, _1));
	}
	return false;
}

bool tempsession::do_sync_push(boost::shared_ptr<session> session, uuid _uuid, Json::Value & value, boost::shared_ptr<Json::Value> & ret, boost::uint64_t wait_time){
	if (session != 0){
		if (remoteq::push(static_cast<tempsession*>(session.get())->ch, value, boost::bind(json_parser::json_to_buf, _1))){
			context::context _context = _service_handle->get_current_context();

			{
				boost::mutex::scoped_lock lock(_service_handle->mu_wait_context_list);
				_service_handle->wait_context_list.insert(std::make_pair(_uuid, std::make_tuple(_uuid, _context, wait_time * 1000, ret)));
			}

			context::context * _tsp_loop_main_context = _service_handle->tsp_loop_main_context.get();
			if (_tsp_loop_main_context != 0){
				context::yield(*_tsp_loop_main_context);
			} else if (*_tsp_loop_main_context == _context){
				context::context _context = context::getcontext(_service_handle->_loop_main);
				_service_handle->set_current_context(_context);
				context::yield(_context);
			} else {
				throw std::exception("_tsp_loop_main_context is null");
			}

			ret = std::get<3>(_service_handle->wait_context_list[_uuid]);
			_service_handle->wait_context_list.erase(_uuid);

			return (*ret)["istimeout"].asBool() == false;
		}
	}
	return false;
}

void tempsession::do_logic(){
}

void tempsession::do_connect_server(boost::shared_ptr<session> _session, Json::Value & value){
	Json::Value _epuuid = value.get("epuuid", Json::nullValue);
	if (_epuuid.isNull()){
		return;
	}
	boost::shared_ptr<rpcsession> _rpc = boost::static_pointer_cast<rpcsession>(_service_handle->create_rpcsession(_epuuid.asString(), ch));
	
	Json::Value _suuid = value.get("suuid", Json::nullValue);
	if (_suuid.isNull()){
		return;
	}

	Json::Value globalobjarray = value.get("globalobjarray", Json::nullValue);
	if (globalobjarray.isArray()){
		for (int i = 0; i < (int)globalobjarray.size(); i++){
			Json::Value objinfo;
			objinfo["classname"] = globalobjarray[i]["classname"].asString();
			objinfo["objid"] = globalobjarray[i]["objid"].asString();
			_rpc->register_global_obj(boost::shared_ptr<obj>(new remote_obj(boost::static_pointer_cast<session>(_rpc), objinfo)));
		}
	}

	if (service_class == "acceptservice"){
		Json::Value ret;
		ret["epuuid"] = _epuuid.asString();
		ret["suuid"] = _suuid.asString();
		ret["eventtype"] = "connect_server";
		ret["globalobjarray"] = Json::Value(Json::arrayValue);

		_service_handle->global_obj_lock();
		for (service::global_obj_iterator it = _service_handle->global_obj_begin(); it != _service_handle->global_obj_end(); it++){
			Json::Value objinfo;
			objinfo["classname"] = it->second->class_name();
			objinfo["objid"] = it->second->objid();
			ret["globalobjarray"].append(objinfo);

			_rpc->register_global_obj(it->second);
		}
		_service_handle->global_obj_unlock();

		do_async_push(_session, ret);
	}
	else if (service_class == "connectservice"){
	}
}

} /* namespace reduce */
} /* namespace Fossilizid */