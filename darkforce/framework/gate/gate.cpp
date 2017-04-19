/*
 * gate.cpp
 *
 *  Created on: 2015-1-17
 *      Author: qianqians
 */
#include "gate.h"

#include <config/config.h>

namespace Fossilizid{
namespace gate{
	
gate::gate(std::string filename, std::string key){
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

	auto routing_config = _config->get_value_dict("routing");
	if (routing_config == 0){
		throw std::exception("cannot find routing config");
	} else{
		try{
			auto size = routing_config->get_list_size();
			routing_server.resize(size);
			for (uint32_t i = 0; i < size; i++){
				auto cfig = routing_config->get_list_dict(i);
				routing_server[cfig->get_value_int("serial ")].first.first = cfig->get_value_string("ip");
				routing_server[cfig->get_value_int("serial ")].first.second = cfig->get_value_int("port");
				routing_server[cfig->get_value_int("serial ")].second = nullptr;
			}
		} catch (...){
			throw std::exception("routing config field error");
		}
	}

	_service = juggle::create_service();
	
	_channelservice = boost::make_shared<achieve::channelservice>();
	_rpcproxy = boost::make_shared<rpcproxy>(boost::bind(&gate::get_channel, this, _1, _2));

	_usersessioncontainer = boost::make_shared<achieve::sessioncontainer>(_channelservice, _rpcproxy);
	_usersessioncontainer->sigdisconn.connect(boost::bind(&gate::user_disconn, this, _1));
	
	_process = boost::make_shared<juggle::process>();

	_logicsessioncontainer = boost::make_shared<achieve::sessioncontainer>(_channelservice, _process);
	_logicsessioncontainer->sigconn.connect(boost::bind(&gate::on_logic_conn, this, _1));
	_logicsessioncontainer->sigdisconn.connect(boost::bind(&gate::logic_disconn, this, _1));

	auto gate_config = _config->get_value_dict("key");
	if (gate_config == 0){
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
		_blackacceptor = boost::make_shared<acceptor::blackacceptor>(gate_config->get_value_string("ip"), gate_config->get_value_int("port"), set, _channelservice, _usersessioncontainer);
		
		auto writeset = boost::make_shared<std::vector<std::pair<std::string, short> > >();
		auto writedict = _config->get_value_dict("writelist");
		for (size_t i = 0; i < writedict->get_list_size(); i++){
			auto e = writedict->get_list_dict(i);
			auto ip = e->get_value_string("ip");
			auto port = (short)e->get_value_int("port");
			writeset->push_back(std::make_pair(ip, port));
		}
		ip = gate_config->get_value_string("clusterip");
		port = gate_config->get_value_int("clusterport");
		_writeacceptor = boost::make_shared<acceptor::writeacceptor>(ip, port, writeset, _channelservice, _logicsessioncontainer);
	}

	_timerservice = timer::timerservice::createinstance();
	_module = boost::make_shared<module::gate>(_process);

	_routingsessioncontainer = boost::make_shared<achieve::sessioncontainer>(_channelservice, _process);
	_routingconnector = boost::make_shared<connector::connector>(_channelservice, _routingsessioncontainer);

	_centersessioncontainer = boost::make_shared<achieve::sessioncontainer>(_channelservice, _process);
	_centerconnector = boost::make_shared<connector::connector>(_channelservice, _centersessioncontainer);
}

gate::~gate(){
}

void gate::user_disconn(boost::shared_ptr<juggle::channel> ch){
	auto it = ch_map.find(ch);
	auto lgc = logic_map.find(it->second.first);
	lgc->second->unregister_user(it->second.second);
}

void gate::logic_disconn(boost::shared_ptr<juggle::channel> ch){
	logic_map.erase(ch);
}

void gate::run(){
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
				gatenum = center_caller->register_gate(ip, port);
			}

			for (uint32_t i = 0; i < routing_server.size(); i++){
				auto addr = routing_server[i];
				if (addr.second.get() == nullptr){
					boost::shared_ptr<juggle::channel> ch = _routingconnector->connect(addr.first.first.c_str(), addr.first.second);
					if (ch != 0){
						addr.second = ch;
						auto c = boost::make_shared<sync::routing>(_process, ch);
						routing_map.insert(std::make_pair(ch, boost::make_tuple(addr.first.first, addr.first.second, i)));
					}
				}
			}
		}

		auto ptime = _timerservice->get_event_time() == 0 ? 0 : _timerservice->get_event_time() - btime;
		_channelservice->poll(ptime);

		_service->poll();

		_timerservice->poll(_service->unixtime());
		
		auto etime = timetmp = _service->unixtime();

		timetmp = etime - btime;
	}

	th_net.join_all();
}

void gate::cancle_gate(){
	isrun.store(false);
}

void gate::on_logic_conn(boost::shared_ptr<juggle::channel> ch){
	if (ch != 0){
		auto c = boost::make_shared<sync::logic>(_process, ch);
		logic_map.insert(std::make_pair(ch, c));
		c->register_gate(gatenum, ip, port);
	}
}

boost::shared_ptr<juggle::channel> gate::get_channel(boost::shared_ptr<juggle::channel> ch, uuid::uuid user){
	auto it = ch_map.find(ch);
	if (it != ch_map.end()){
		return it->first;
	}

	for (auto i : logic_map){
		if (i.second->register_user(user)){
			auto logic_ch = i.first;
			ch_map.insert(std::make_pair(ch, std::make_pair(logic_ch, user)));
			return logic_ch;
		}
	}
	return nullptr;
}

} /* namespace routing */
} /* namespace Fossilizid */

