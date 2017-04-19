/*
 * routing.h
 *
 *  Created on: 2015-7-26
 *      Author: qianqians
 */
#ifndef _routing_h
#define _routing_h

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

#include <juggle.h>

#include <module/routingmodule.h>
#include <caller/routingcaller.h>
#include <caller/gatecaller.h>
#include <caller/logiccaller.h>
#include <caller/dbcaller.h>

namespace Fossilizid{
namespace routing{

class routing{
public:
	routing(std::string filename, std::string key);
	~routing();

public:
	void run();

public:
	void register_user(std::string uuid, int gatenum, int logicnum, int dbnum);

	void unregister_user(std::string uuid);

	std::vector<int64_t> get_user(std::string uuid);

public:
	void cancle_routing();

	void svr_disconn(boost::shared_ptr<juggle::channel> ch);

private:
	uint64_t timetmp;

	std::pair<std::string, short> center_addr;
	boost::shared_ptr<juggle::channel> ch_center;
	
	boost::shared_ptr<acceptor::writeacceptor> _writeacceptor;
	boost::shared_ptr<achieve::sessioncontainer> _logicsessioncontainer;
	boost::unordered_map<boost::shared_ptr<juggle::channel>, boost::shared_ptr<sync::logic> > logic_map;

	boost::shared_ptr<timer::timerservice> _timerservice;
	boost::shared_ptr<achieve::channelservice> _channelservice;
	boost::shared_ptr<juggle::service> _service;
	boost::shared_ptr<juggle::process> _process;

	boost::unordered_map<std::string, std::vector<int64_t> > usermap;

	boost::shared_ptr<module::routing> _module;

	boost::atomic_bool isrun;
	boost::thread_group th_net;

};

} /* namespace routing */
} /* namespace Fossilizid */

#endif //_routing_h
