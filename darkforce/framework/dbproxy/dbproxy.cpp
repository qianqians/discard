/*
 * gate.cpp
 *
 *  Created on: 2015-1-17
 *      Author: qianqians
 */
#include "dbproxy.h"

#include <config/config.h>

namespace Fossilizid{
namespace dbproxy{
	
dbproxy::dbproxy(std::string filename, std::string key){
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
	_sessioncontainer = boost::make_shared<achieve::sessioncontainer>(_channelservice, _process);

	_sessioncontainer->sigdisconn.connect(boost::bind(&dbproxy::logic_disconn, this, _1));

	auto dbproxy_config = _config->get_value_dict("key");
	if (dbproxy_config == 0){
		throw std::exception("cannot find this config");
	} else{
		auto writeset = boost::make_shared<std::vector<std::pair<std::string, short> > >();
		auto writedict = _config->get_value_dict("writelist");
		for (size_t i = 0; i < dbproxy_config->get_list_size(); i++){
			auto e = writedict->get_list_dict(i);
			auto ip = e->get_value_string("ip");
			auto port = (short)e->get_value_int("port");
			writeset->push_back(std::make_pair(ip, port));
		}

		_writeacceptor = boost::make_shared<acceptor::writeacceptor>(dbproxy_config->get_value_string("clusterip"), dbproxy_config->get_value_int("clusterport"), writeset, _channelservice, _sessioncontainer);
	}

	_timerservice = timer::timerservice::createinstance();

	_module = boost::make_shared<module::dbproxy>(_process);
	
	_module->sigadd_user.connect(boost::bind(&dbproxy::add_user, this));
	_module->sigcreate_index.connect(boost::bind(&dbproxy::create_index, this, _1, _2));
	_module->sigdrop_index.connect(boost::bind(&dbproxy::drop_index, this, _1, _2));
	_module->sigfind_indexes.connect(boost::bind(&dbproxy::find_indexes, this, _1));
	_module->sigcount.connect(boost::bind(&dbproxy::count, this, _1, _2, _3, _4));
	_module->siginsert.connect(boost::bind(&dbproxy::insert, this, _1, _2));
	_module->sigsave.connect(boost::bind(&dbproxy::save, this, _1, _2));
	_module->sigupdate.connect(boost::bind(&dbproxy::update, this, _1, _2, _3));
	_module->sigremove.connect(boost::bind(&dbproxy::remove, this, _1, _2));
	_module->sigfind.connect(boost::bind(&dbproxy::find, this, _1, _2, _3, _4, _5, _6));
	_module->sigfind_and_modify.connect(boost::bind(&dbproxy::find_and_modify, this, _1, _2, _3, _4, _5, _6, _7, _8));
	_module->sigvalidate.connect(boost::bind(&dbproxy::validate, this, _1));
	_module->sigstats.connect(boost::bind(&dbproxy::validate, this, _1));

	_mongoproxy = boost::make_shared<mongoproxy::dbproxy>(_config->get_value_string("db_ip"), _config->get_value_int("db_port"), _config->get_value_string("db_name"));

	_centersessioncontainer = boost::make_shared<achieve::sessioncontainer>(_channelservice, _process);
	_centerconnector = boost::make_shared<connector::connector>(_channelservice, _centersessioncontainer);
}

dbproxy::~dbproxy(){
}

void dbproxy::logic_disconn(boost::shared_ptr<juggle::channel> ch){
	auto it = logic_map.find(ch);
	logic_map.erase(ch);
}

void dbproxy::run(){
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
			if (ch_center != 0){
				ch_center = _centerconnector->connect(center_addr.first.c_str(), center_addr.second);
				center_caller = boost::make_shared<sync::center>(_process, ch_center);
				dbnum = center_caller->register_db(ip, port);
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

bool dbproxy::add_user(){
	if (timetmp < 100){
		return true;
	}
	return false;
}

void dbproxy::cancle_dbproxy(){
	isrun.store(false);
}

bool dbproxy::create_index(std::string collection_name, std::string keys){
	return _mongoproxy->create_index(collection_name, keys);
}
bool dbproxy::drop_index(std::string collection, std::string index_name){
	return _mongoproxy->drop_index(collection, index_name);
}

std::vector<boost::shared_ptr<boost::unordered_map<std::string, boost::any> > > dbproxy::find_indexes(std::string  collection){
	return _mongoproxy->find_indexes(collection);
}

int64_t dbproxy::count(std::string collection_name, boost::shared_ptr<boost::unordered_map<std::string, boost::any> > query, int skip, int limit){
	return _mongoproxy->count(collection_name, query, skip, limit);
}

bool dbproxy::insert(std::string collection, boost::shared_ptr<boost::unordered_map<std::string, boost::any> > document){
	return _mongoproxy->insert(collection, document);
}

bool dbproxy::save(std::string collection, boost::shared_ptr<boost::unordered_map<std::string, boost::any> > document){
	return _mongoproxy->save(collection, document);
}

bool dbproxy::update(std::string collection, boost::shared_ptr<boost::unordered_map<std::string, boost::any> > query, boost::shared_ptr<boost::unordered_map<std::string, boost::any> > update){
	return _mongoproxy->update(collection, query, update);
}

bool dbproxy::remove(std::string collection, boost::shared_ptr<boost::unordered_map<std::string, boost::any> > query){
	return _mongoproxy->remove(collection, query);
}

std::vector<boost::shared_ptr<boost::unordered_map<std::string, boost::any> > > dbproxy::find(std::string collection, int skip, int limit, int batch_size, boost::shared_ptr<boost::unordered_map<std::string, boost::any> > query, std::vector<std::string> fields){
	return _mongoproxy->find(collection, skip, limit, batch_size, query, fields);
}

boost::shared_ptr<boost::unordered_map<std::string, boost::any> > dbproxy::find_and_modify(std::string collection, boost::shared_ptr<boost::unordered_map<std::string, boost::any> > query, boost::shared_ptr<boost::unordered_map<std::string, boost::any> > sort, boost::shared_ptr<boost::unordered_map<std::string, boost::any> >  update, std::vector<std::string>  fields, bool _remove, bool upsert, bool _new){
	return _mongoproxy->find_and_modify(collection, query, sort, update, fields, _remove, upsert, _new);
}

boost::shared_ptr<boost::unordered_map<std::string, boost::any> > dbproxy::validate(std::string collection){
	return _mongoproxy->validate(collection);
}

boost::shared_ptr<boost::unordered_map<std::string, boost::any> > dbproxy::stats(std::string collection){
	return _mongoproxy->stats(collection);
}

} /* namespace routing */
} /* namespace Fossilizid */

