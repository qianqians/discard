/*
 * logic.cpp
 *
 *  Created on: 2015-1-17
 *      Author: qianqians
 */
#include "center.h"

#include <boost/make_shared.hpp>

#include <uuid/uuid.h>

#include <config/config.h>

namespace Fossilizid{
namespace center{
	
center::center(std::string filename, std::string key){
	isrun = true;

	boost::shared_ptr<config::config> _config = boost::make_shared<config::config>(filename);
	if (_config == nullptr){
		throw std::exception(("cannot find config file" + filename).c_str());
	}


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

	servernum = 0;

	_service = juggle::create_service();
	_process = boost::make_shared<juggle::process>();
	_channelservice = boost::make_shared<achieve::channelservice>();

	_svrsessioncontainer = boost::make_shared<achieve::sessioncontainer>(_channelservice, _process);

	auto key_config = _config->get_value_dict("key");
	if (key_config == 0){
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
		_writeacceptor = boost::make_shared<acceptor::writeacceptor>(key_config->get_value_string("ip"), key_config->get_value_int("port"), set, _channelservice, _svrsessioncontainer);
	}
	_timerservice = timer::timerservice::createinstance();

	_module = boost::make_shared<module::center>(_process);
	_module->sigregister_db.connect(boost::bind(&center::register_db, this, _1, _2));
	_module->sigregister_gate.connect(boost::bind(&center::register_gate, this, _1, _2));
	_module->sigregister_routing.connect(boost::bind(&center::register_routing, this, _1, _2));
	_module->sigregister_logic.connect(boost::bind(&center::register_logic, this, _1, _2));
}

center::~center(){
}

void center::run(){
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

void center::cancle_center(){
	isrun = false;
}

void center::svr_disconn(boost::shared_ptr<juggle::channel> ch){
	if (gatemap.find(ch) != gatemap.end()){
		gatemap.erase(ch);
		gateaddrmap.erase(ch);
	}
	if (logicmap.find(ch) != logicmap.end()){
		logicmap.erase(ch);
		logicaddrmap.erase(ch);
	}
	if (routingmap.find(ch) != routingmap.end()){
		routingmap.erase(ch);
		routingaddrmap.erase(ch);
	}
	if (dbmap.find(ch) != dbmap.end()){
		dbmap.erase(ch);
		dbaddrmap.erase(ch);
	}
}

int center::register_logic(std::string ip, int port){
	auto ch = _service->get_current_channel();

	logicaddrmap.insert(std::make_pair(ch, std::make_pair(++servernum, std::make_pair(ip, port))));
	auto caller = boost::make_shared<sync::logic>(_process, ch);
	logicmap.insert(std::make_pair(ch, caller));

	for (auto cgate : gateaddrmap){
		caller->register_gate(cgate.second.first, cgate.second.second.first, cgate.second.second.second);
	}

	for (auto cgate : dbaddrmap){
		caller->register_gate(cgate.second.first, cgate.second.second.first, cgate.second.second.second);
	}

	return ++servernum;
}

int center::register_gate(std::string ip, int port){
	auto ch = _service->get_current_channel();

	int gatenum = ++servernum;

	gateaddrmap.insert(std::make_pair(ch, std::make_pair(gatenum, std::make_pair(ip, port))));
	gatemap.insert(std::make_pair(ch, boost::make_shared<sync::gate>(_process, ch)));

	for (auto caller : logicmap){
		caller.second->register_db(gatenum, ip, port);
	}

	return gatenum;
}

int center::register_routing(std::string ip, int port){
	auto ch = _service->get_current_channel();

	routingaddrmap.insert(std::make_pair(ch, std::make_pair(++servernum, std::make_pair(ip, port))));
	routingmap.insert(std::make_pair(ch, boost::make_shared<sync::routing>(_process, ch)));

	return servernum;
}

int center::register_db(std::string ip, int port){
	auto ch = _service->get_current_channel();

	int dbnum = ++servernum;

	dbaddrmap.insert(std::make_pair(ch, std::make_pair(dbnum, std::make_pair(ip, port))));
	dbmap.insert(std::make_pair(ch, boost::make_shared<sync::dbproxy>(_process, ch)));

	for (auto caller : logicmap){
		caller.second->register_db(dbnum, ip, port);
	}

	return dbnum;
}

} /* namespace routing */
} /* namespace Fossilizid */

