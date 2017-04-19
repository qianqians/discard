/*
 * gate.h
 *
 *  Created on: 2015-7-26
 *      Author: qianqians
 */
#ifndef _gate_h
#define _gate_h

#include <uuid/uuid.h>

#include <string>
#include <list>

#include <unordered_map>
#include <boost/thread.hpp>
#include <boost/shared_ptr.hpp>
#include <boost/atomic.hpp>


#include <timer/timerservice.h>
#include <acceptor/blackacceptor.h>
#include <acceptor/writeacceptor.h>
#include <achieve/achieve.h>
#include <juggle.h>
#include <connector/connector.h>

#include <caller/centercaller.h>
#include <caller/routingcaller.h>
#include <caller/logiccaller.h>
#include <module/gatemodule.h>

#include "rpcproxy.h"

namespace Fossilizid{
namespace gate{

class gate{
public:
	gate(std::string filename, std::string key);
	~gate();

public:
	void run();

public:
	void cancle_gate();

	void user_disconn(boost::shared_ptr<juggle::channel> ch);

	void logic_disconn(boost::shared_ptr<juggle::channel> ch);

private:
	void on_logic_conn(boost::shared_ptr<juggle::channel> ch);

private:
	boost::shared_ptr<juggle::channel> get_channel(boost::shared_ptr<juggle::channel> ch, uuid::uuid user);

private:
	uint64_t timetmp;

	int gatenum;
	std::string ip;
	short port;

	std::pair<std::string, short> center_addr;
	boost::shared_ptr<juggle::channel> ch_center;
	boost::shared_ptr<sync::center> center_caller;

	std::vector< std::pair<std::pair<std::string, short>, boost::shared_ptr<juggle::channel> > > routing_server;
	boost::unordered_map<boost::shared_ptr<juggle::channel>, boost::tuple<std::string, short, int> > routing_map;

	boost::shared_ptr<timer::timerservice> _timerservice;
	boost::shared_ptr<achieve::channelservice> _channelservice;
	boost::shared_ptr<juggle::service> _service;
	boost::shared_ptr<juggle::process> _process;

	boost::shared_ptr<acceptor::blackacceptor> _blackacceptor;
	boost::shared_ptr<rpcproxy> _rpcproxy;
	boost::shared_ptr<achieve::sessioncontainer> _usersessioncontainer;
	
	boost::shared_ptr<acceptor::writeacceptor> _writeacceptor;
	boost::shared_ptr<achieve::sessioncontainer> _logicsessioncontainer;
	boost::unordered_map<boost::shared_ptr<juggle::channel>, boost::shared_ptr<sync::logic> > logic_map;

	boost::shared_ptr<connector::connector> _routingconnector;
	boost::shared_ptr<achieve::sessioncontainer> _routingsessioncontainer;

	boost::shared_ptr<connector::connector> _centerconnector;
	boost::shared_ptr<achieve::sessioncontainer> _centersessioncontainer;

	boost::shared_ptr<module::gate> _module;

	boost::unordered_map<boost::shared_ptr<juggle::channel>, std::pair<boost::shared_ptr<juggle::channel>, uuid::uuid> > ch_map;

	boost::atomic_bool isrun;
	boost::thread_group th_net;

};

} /* namespace routing */
} /* namespace Fossilizid */

#endif //_routing_h
