/*
 * center.h
 *
 *  Created on: 2015-7-26
 *      Author: qianqians
 */
#ifndef _center_h
#define _center_h

#include <string>
#include <list>

#include <boost/unordered_map.hpp>
#include <boost/thread.hpp>
#include <boost/shared_ptr.hpp>
#include <boost/atomic.hpp>

#include <timer/timerservice.h>
#include <acceptor/writeacceptor.h>
#include <achieve/achieve.h>

#include <juggle.h>

#include <module/centermodule.h>
#include <caller/routingcaller.h>
#include <caller/gatecaller.h>
#include <caller/logiccaller.h>
#include <caller/dbcaller.h>

namespace Fossilizid{
namespace center{

class center{
public:
	center(std::string filename, std::string key);
	~center();

public:
	void run();

public:
	int register_logic(std::string ip, int port);

	int register_gate(std::string ip, int port);

	int register_routing(std::string ip, int port);

	int register_db(std::string ip, int port);

public:
	void cancle_center();

	void svr_disconn(boost::shared_ptr<juggle::channel> ch);

private:
	uint64_t timetmp;

	std::pair<std::string, short> center_addr;
	boost::shared_ptr<acceptor::writeacceptor> _writeacceptor;
	boost::shared_ptr<achieve::sessioncontainer> _svrsessioncontainer;

	int servernum;

	boost::unordered_map<boost::shared_ptr<Fossilizid::juggle::channel>, std::pair<int, std::pair<std::string, short> > > gateaddrmap;
	boost::unordered_map<boost::shared_ptr<Fossilizid::juggle::channel>, boost::shared_ptr<sync::gate> > gatemap;
	boost::unordered_map<boost::shared_ptr<Fossilizid::juggle::channel>, std::pair<int, std::pair<std::string, short> > > logicaddrmap;
	boost::unordered_map<boost::shared_ptr<Fossilizid::juggle::channel>, boost::shared_ptr<sync::logic> > logicmap;
	boost::unordered_map<boost::shared_ptr<Fossilizid::juggle::channel>, std::pair<int, std::pair<std::string, short> > > routingaddrmap;
	boost::unordered_map<boost::shared_ptr<Fossilizid::juggle::channel>, boost::shared_ptr<sync::routing> > routingmap;
	boost::unordered_map<boost::shared_ptr<Fossilizid::juggle::channel>, std::pair<int, std::pair<std::string, short> > > dbaddrmap;
	boost::unordered_map<boost::shared_ptr<Fossilizid::juggle::channel>, boost::shared_ptr<sync::dbproxy> > dbmap;

	boost::shared_ptr<timer::timerservice> _timerservice;
	boost::shared_ptr<achieve::channelservice> _channelservice;
	boost::shared_ptr<juggle::service> _service;
	boost::shared_ptr<juggle::process> _process;

	boost::shared_ptr<module::center> _module;

	boost::atomic_bool isrun;

	boost::thread_group th_net;

};

} /* namespace logic */
} /* namespace Fossilizid */

#endif //_logic_h
