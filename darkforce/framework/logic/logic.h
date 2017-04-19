/*
 * logic.h
 *
 *  Created on: 2015-7-26
 *      Author: qianqians
 */
#ifndef _logic_h
#define _logic_h

#include <uuid/uuid.h>

#include <string>
#include <list>

#include <boost/unordered_map.hpp>
#include <boost/thread.hpp>
#include <boost/shared_ptr.hpp>
#include <boost/atomic.hpp>

#include <timer/timerservice.h>
#include <acceptor/writeacceptor.h>
#include <achieve/achieve.h>
#include <connector/connector.h>

#include <juggle.h>

#include <caller/centercaller.h>
#include <caller/routingcaller.h>
#include <caller/gatecaller.h>
#include <caller/dbcaller.h>
#include <caller/logiccaller.h>
#include <module/logicmodule.h>

namespace Fossilizid{
namespace logic{

class logic{
public:
	logic(std::string filename, std::string key);
	~logic();

public:
	void run();

public:
	bool register_user(std::string uuid);

	void unregister_user(std::string uuid);

	void register_gate(int gatenum, std::string ip, int port);

	void register_db(int dbnum, std::string ip, int port);

public:
	void cancle_logic();

	void gate_disconn(boost::shared_ptr<juggle::channel> ch);

	void db_disconn(boost::shared_ptr<juggle::channel> ch);

private:
	void keeplive_check(uint64_t t);

private:
	uint64_t timetmp;

	std::list<uuid::uuid> keeplive_check_list;

	std::pair<std::string, short> center_addr;
	boost::shared_ptr<juggle::channel> ch_center;
	boost::shared_ptr<sync::center> center_caller;
	boost::shared_ptr<connector::connector> _centerconnector;
	boost::shared_ptr<achieve::sessioncontainer> _centersessioncontainer;

	std::string ip;
	short port;
	boost::shared_ptr<acceptor::writeacceptor> _writeacceptor;
	boost::shared_ptr<achieve::sessioncontainer> _logicsessioncontainer;
	boost::unordered_map<boost::shared_ptr<juggle::channel>, boost::shared_ptr<sync::logic> > logic_map;
	       
	std::vector< std::pair<std::pair<std::string, short>, boost::shared_ptr<juggle::channel> > > routing_server;
	boost::unordered_map<boost::shared_ptr<juggle::channel>, boost::tuple<std::string, short, int> > routing_map;
	boost::unordered_map<boost::shared_ptr<juggle::channel>, boost::shared_ptr<sync::routing> > routing_caller;
	boost::shared_ptr<connector::connector> _routingconnector;
	boost::shared_ptr<achieve::sessioncontainer> _routingsessioncontainer;

	boost::shared_ptr<timer::timerservice> _timerservice;
	boost::shared_ptr<achieve::channelservice> _channelservice;
	boost::shared_ptr<juggle::service> _service;

	boost::shared_ptr<connector::connector> _gateconnector;
	boost::shared_ptr<achieve::sessioncontainer> _gatesessioncontainer;
	std::list<std::pair<int, std::pair<std::string, short> > > _gateaddr;
	boost::unordered_map<int, boost::shared_ptr<sync::gate> > _gateproxy;
	boost::unordered_map<boost::shared_ptr<juggle::channel>, int> _gateproxymap;

	boost::shared_ptr<connector::connector> _dbconnector;
	boost::shared_ptr<achieve::sessioncontainer> _dbsessioncontainer;
	std::list<std::pair<int, std::pair<std::string, short> > > _dbaddr;
	boost::unordered_map<int, boost::shared_ptr<sync::dbproxy> > _dbproxy;
	boost::unordered_map<boost::shared_ptr<juggle::channel>, int> _dbproxymap;

	boost::unordered_map < uuid::uuid, boost::shared_ptr<boost::unordered_map<std::string, boost::any> > > usermap;
	
	boost::shared_ptr<juggle::process> _process;

	boost::shared_ptr<module::logic> _module;

	boost::atomic_bool isrun;
	boost::thread_group th_net;

};

} /* namespace logic */
} /* namespace Fossilizid */

#endif //_logic_h
