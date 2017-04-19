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

#include <boost/unordered_map.hpp>
#include <boost/thread.hpp>
#include <boost/shared_ptr.hpp>
#include <boost/atomic.hpp>


#include <timer/timerservice.h>
#include <acceptor/writeacceptor.h>
#include <achieve/achieve.h>
#include <mongoproxy/mongoproxy.h>
#include <juggle.h>
#include <connector/connector.h>

#include <caller/centercaller.h>
#include <caller/logiccaller.h>
#include <module/dbmodule.h>

namespace Fossilizid{
namespace dbproxy{

class dbproxy{
public:
	dbproxy(std::string filename, std::string key);
	~dbproxy();

public:
	void run();

public:
	void cancle_dbproxy();

	void logic_disconn(boost::shared_ptr<juggle::channel> ch);

private:
	bool add_user();

	bool create_index(std::string collection_name, std::string keys);

	bool drop_index(std::string collection, std::string index_name);

	std::vector<boost::shared_ptr<boost::unordered_map<std::string, boost::any> > > find_indexes(std::string  collection);

	int64_t count(std::string collection_name, boost::shared_ptr<boost::unordered_map<std::string, boost::any> > query, int skip, int limit);

	bool insert(std::string collection, boost::shared_ptr<boost::unordered_map<std::string, boost::any> > document);

	bool save(std::string collection, boost::shared_ptr<boost::unordered_map<std::string, boost::any> > document);

	bool update(std::string collection, boost::shared_ptr<boost::unordered_map<std::string, boost::any> > query, boost::shared_ptr<boost::unordered_map<std::string, boost::any> > update);

	bool remove(std::string collection, boost::shared_ptr<boost::unordered_map<std::string, boost::any> > query);

	std::vector<boost::shared_ptr<boost::unordered_map<std::string, boost::any> > > find(std::string collection, int skip, int limit, int batch_size, boost::shared_ptr<boost::unordered_map<std::string, boost::any> > query, std::vector<std::string> fields);

	boost::shared_ptr<boost::unordered_map<std::string, boost::any> > find_and_modify(std::string collection, boost::shared_ptr<boost::unordered_map<std::string, boost::any> > query, boost::shared_ptr<boost::unordered_map<std::string, boost::any> > sort, boost::shared_ptr<boost::unordered_map<std::string, boost::any> >  update, std::vector<std::string>  fields, bool _remove, bool upsert, bool _new);

	boost::shared_ptr<boost::unordered_map<std::string, boost::any> > validate(std::string collection);

	boost::shared_ptr<boost::unordered_map<std::string, boost::any> > stats(std::string collection);

private:
	uint64_t timetmp;

	int dbnum;
	std::string ip;
	short port;

	std::pair<std::string, short> center_addr;
	boost::shared_ptr<juggle::channel> ch_center;
	boost::shared_ptr<sync::center> center_caller;

	boost::unordered_map<boost::shared_ptr<juggle::channel>, boost::tuple<std::string, short, boost::shared_ptr<sync::logic> > > logic_map;

	boost::shared_ptr<acceptor::writeacceptor> _writeacceptor;
	boost::shared_ptr<timer::timerservice> _timerservice;
	boost::shared_ptr<achieve::channelservice> _channelservice;
	boost::shared_ptr<juggle::service> _service;
	boost::shared_ptr<achieve::sessioncontainer> _sessioncontainer;
	
	boost::shared_ptr<juggle::process> _process;

	boost::shared_ptr<connector::connector> _centerconnector;
	boost::shared_ptr<achieve::sessioncontainer> _centersessioncontainer;

	boost::shared_ptr<module::dbproxy> _module;

	boost::shared_ptr<mongoproxy::dbproxy> _mongoproxy;

	boost::atomic_bool isrun;
	boost::thread_group th_net;

};

} /* namespace routing */
} /* namespace Fossilizid */

#endif //_routing_h
