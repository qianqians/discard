/*
 * logic.cpp
 *
 *  Created on: 2015-1-17
 *      Author: qianqians
 */
#include "logic.h"

#include <boost/make_shared.hpp>

#include <config/config.h>

namespace Fossilizid{
namespace logic{
	
logic::logic(std::string filename, std::string key){
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

	ip = _config->get_value_string("ip");
	port = _config->get_value_int("port");
	auto writeset = boost::make_shared<std::vector<std::pair<std::string, short> > >();
	auto writedict = _config->get_value_dict("writelist");
	for (size_t i = 0; i < writedict->get_list_size(); i++){
		auto e = writedict->get_list_dict(i);
		auto ip = e->get_value_string("ip");
		auto port = (short)e->get_value_int("port");
		writeset->push_back(std::make_pair(ip, port));
	}
	_writeacceptor = boost::make_shared<acceptor::writeacceptor>(ip, port, writeset, _channelservice, _logicsessioncontainer);

	_service = juggle::create_service();
	
	_process = boost::make_shared<juggle::process>();

	_channelservice = boost::make_shared<achieve::channelservice>();

	_gatesessioncontainer = boost::make_shared<achieve::sessioncontainer>(_channelservice, _process);
	_gatesessioncontainer->sigdisconn.connect(boost::bind(&logic::gate_disconn, this, _1));
	_gateconnector = boost::make_shared<connector::connector>(_channelservice, _gatesessioncontainer);

	_dbsessioncontainer = boost::make_shared<achieve::sessioncontainer>(_channelservice, _process);
	_dbsessioncontainer->sigdisconn.connect(boost::bind(&logic::db_disconn, this, _1));
	_dbconnector = boost::make_shared<connector::connector>(_channelservice, _dbsessioncontainer);
	
	_timerservice = timer::timerservice::createinstance();

	_module = boost::make_shared<module::logic>(_process);

	_module->sigregister_gate.connect(boost::bind(&logic::register_gate, this, _1, _2, _3));
	_module->sigregister_db.connect(boost::bind(&logic::register_db, this, _1, _2, _3));
	_module->sigregister_user.connect(boost::bind(&logic::register_user, this, _1));
	_module->sigunregister_user.connect(boost::bind(&logic::unregister_user, this, _1));
	
	_centersessioncontainer = boost::make_shared<achieve::sessioncontainer>(_channelservice, _process);
	_centerconnector = boost::make_shared<connector::connector>(_channelservice, _centersessioncontainer);

	add_timer(60 * 1000, boost::bind(&logic::keeplive_check, this, _1));
}

logic::~logic(){
}

void logic::keeplive_check(uint64_t t){
	for (auto id : keeplive_check_list){
		usermap.erase(id);
	}
	
	add_timer(60 * 1000, boost::bind(&logic::keeplive_check, this, _1));
}

bool logic::register_user(std::string uuid){
	if (timetmp > 100){
		return false;
	}

	{
		usermap.insert(std::make_pair(uuid, boost::make_shared<boost::unordered_map<std::string, boost::any> >()));
	}

	return true;
}

void logic::unregister_user(std::string uuid){
	keeplive_check_list.push_back(uuid);
}

void logic::register_gate(int gatenum, std::string ip, int port){
	auto ch = _gateconnector->connect(ip.c_str(), port);
	if (ch != 0){
		_gateproxy.insert(std::make_pair(gatenum, boost::make_shared<sync::gate>(_process, ch)));
		_gateproxymap.insert(std::make_pair(ch, gatenum));
	} else{
		_gateaddr.push_back(std::make_pair(gatenum, std::make_pair(ip, port)));
	}
}

void logic::register_db(int dbnum, std::string ip, int port){
	auto ch = _dbconnector->connect(ip.c_str(), port);
	if (ch != 0){
		_dbproxy.insert(std::make_pair(dbnum, boost::make_shared<sync::dbproxy>(_process, ch)));
		_dbproxymap.insert(std::make_pair(ch, dbnum));
	} else{
		_dbaddr.push_back(std::make_pair(dbnum, std::make_pair(ip, port)));
	}
}

void logic::gate_disconn(boost::shared_ptr<juggle::channel> ch){
	auto it = _gateproxymap.find(ch);
	if (it != _gateproxymap.end()){
		_gateproxy.erase(it->second);
		_gateproxymap.erase(it);
	}
}

void logic::db_disconn(boost::shared_ptr<juggle::channel> ch){
	auto it = _dbproxymap.find(ch);
	if (it != _dbproxymap.end()){
		_dbproxy.erase(it->second);
		_dbproxymap.erase(it);
	}
}

void logic::run(){
	_service->init();

	auto netrun = [this](){
		while (isrun.load()){
			_channelservice->poll(-1);
		}
	};

	th_net.create_thread(netrun);

	while (isrun.load()){
		auto btime = _service->unixtime();

		{
			if (ch_center == 0){
				ch_center = _centerconnector->connect(center_addr.first.c_str(), center_addr.second);
				center_caller = boost::make_shared<sync::center>(_process, ch_center);
				center_caller->register_logic(ip, port);
			}

			for (auto it = _gateaddr.begin(); it != _gateaddr.end();){
				auto ch = _gateconnector->connect(it->second.first.c_str(), it->second.second);
				if (ch != 0){
					auto c= boost::make_shared<sync::gate>(_process, ch);
					_gateproxy.insert(std::make_pair(it->first, c));
					_gateproxymap.insert(std::make_pair(ch, it->first));

					it = _gateaddr.erase(it);
				} else{
					it++;
				}
			}

			for (uint32_t i = 0; i < routing_server.size(); i++){
				auto addr = routing_server[i];
				if (addr.second.get() == nullptr){
					boost::shared_ptr<juggle::channel> ch = _routingconnector->connect(addr.first.first.c_str(), addr.first.second);
					if (ch != 0){
						addr.second = ch;
						auto c = boost::make_shared<sync::routing>(_process, ch);
						routing_map.insert(std::make_pair(ch, boost::make_tuple(addr.first.first, addr.first.second, i)));
						routing_caller.insert(std::make_pair(ch, c));
					}
				}
			}
		}

		auto ptime = _timerservice->get_event_time() == 0 ? 0 : _timerservice->get_event_time() - timetmp;
		_channelservice->poll(ptime);

		_service->poll();

		_timerservice->poll(_service->unixtime());

		auto etime = timetmp = _service->unixtime();

		timetmp = etime - btime;
	}

	th_net.join_all();
}


} /* namespace routing */
} /* namespace Fossilizid */

