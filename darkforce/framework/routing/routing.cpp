/*
 * routing.cpp
 *
 *  Created on: 2015-1-17
 *      Author: qianqians
 */
#include "routing.h"

#include <boost/make_shared.hpp>

#include <config/config.h>

namespace Fossilizid{
namespace routing{
	
routing::routing(std::string filename, std::string key){
	isrun = true;

	boost::shared_ptr<config::config> _config = boost::make_shared<config::config>(filename);

	auto center_config = _config->get_value_dict("center");
	if (center_config == 0){
		throw std::exception("cannot find center config");
	} else{
		try{
			center_addr.first = center_config->get_value_string("ip");
			center_addr.second = (short)center_config->get_value_int("port");
		} catch(...){
			throw std::exception("center config field error");
		}
	}

	_service = juggle::create_service();
	_process = boost::make_shared<juggle::process>();
	_channelservice = boost::make_shared<achieve::channelservice>();

	_logicsessioncontainer = boost::make_shared<achieve::sessioncontainer>(_channelservice, _process);

	auto routing_config = _config->get_value_dict("key");
	if (routing_config == nullptr){
		throw std::exception("cannot find this config");
	} else{
		auto set = boost::make_shared<std::vector<std::pair<std::string, short> > >();
		auto dict = _config->get_value_dict("blacklist");
		for (size_t i = 0; i < dict->get_list_size(); i++){
			auto e = dict->get_list_dict(i);
			auto ip = e->get_value_string("ip");
			auto port = (short)e->get_value_int("port");
			set->push_back(std::make_pair(ip, port));
		}
		_writeacceptor = boost::make_shared<acceptor::writeacceptor>(routing_config->get_value_string("ip"), routing_config->get_value_int("port"), set, _channelservice, _logicsessioncontainer);
	}
	_timerservice = timer::timerservice::createinstance();

	_module = boost::make_shared<module::routing>(_process);
	_module->sigregister_user.connect(boost::bind(&routing::register_user, this, _1, _2, _3, _4));
	_module->sigunregister_user.connect(boost::bind(&routing::unregister_user, this, _1));
	_module->sigget_user.connect(boost::bind(&routing::get_user, this, _1));
}

routing::~routing(){
}

void routing::run(){
	_service->init();

	auto netrun = [this](){
		while (isrun.load()){
			_channelservice->poll(-1);
		}
	};

	th_net.create_thread(netrun);

	while (isrun.load()){
		auto btime = _service->unixtime();

		auto ptime = _timerservice->get_event_time() == 0 ? 0 : _timerservice->get_event_time() - btime;
		_channelservice->poll(ptime);

		_service->poll();

		_timerservice->poll(_service->unixtime());

		auto etime = timetmp = _service->unixtime();

		timetmp = etime - btime;
	}

	th_net.join_all();
}

void routing::cancle_routing(){
	isrun = false;
}

void routing::svr_disconn(boost::shared_ptr<juggle::channel> ch){
	logic_map.erase(ch);
}

void routing::register_user(std::string uuid, int gatenum, int logicnum, int dbnum){
	if (usermap.find(uuid) != usermap.end()){
		usermap[uuid].clear();
	}
	usermap[uuid].push_back(gatenum);
	usermap[uuid].push_back(logicnum);
	usermap[uuid].push_back(dbnum);
}

void routing::unregister_user(std::string uuid){
	if (usermap.find(uuid) != usermap.end()){
		usermap.erase(uuid);
	}
}

std::vector<int64_t> routing::get_user(std::string uuid){
	if (usermap.find(uuid) != usermap.end()){
		return usermap[uuid];
	}else{
		std::vector<int64_t> v;
		v.push_back(-1);
		v.push_back(-1);
		v.push_back(-1);

		return v;
	}
}

} /* namespace routing */
} /* namespace Fossilizid */

