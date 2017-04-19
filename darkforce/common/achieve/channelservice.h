/*
 * channelservice.h
 *
 *  Created on: 2015-1-17
 *      Author: qianqians
 */
#ifndef _channelservice_h
#define _channelservice_h

#include "channel.h"

#include "../fitle/fitle.h"

#include <boost/unordered_map.hpp>
#include <boost/signals2.hpp>
#include <juggle.h>

namespace Fossilizid{

namespace acceptor {
	class writeacceptor;
	class blackacceptor;
}

namespace achieve{

class channelservice{
public:
	channelservice();
	~channelservice();

	void init();

	void poll(time_t timeout);

private:
	void reg_event(remoteq::event_type type, boost::function<void(remoteq::EVENT) > callback);

	friend class sessioncontainer;
	friend class acceptor;
	friend class connector;

private:
	remoteq::QUEUE que;

	std::array<std::list<boost::function<void(remoteq::EVENT) > >, remoteq::event_size> callbacks;

};

class sessioncontainer{
public:
	sessioncontainer(boost::shared_ptr<channelservice> _chservice, boost::shared_ptr<juggle::process> _process);
	~sessioncontainer();

	boost::signals2::signal<void(boost::shared_ptr<juggle::channel>)> sigconn;

	boost::signals2::signal<void(boost::shared_ptr<juggle::channel>)> sigdisconn;

public:
	boost::shared_ptr<juggle::channel> handle_session(remoteq::CHANNEL ch);

private:
	void handle_recv(remoteq::EVENT ev);

	void handle_disconnection(remoteq::EVENT ev);

private:
	boost::unordered_map<remoteq::CHANNEL, boost::shared_ptr<juggle::channel> > mapchannel;
	boost::shared_ptr<juggle::process> process;
	boost::shared_ptr<channelservice> chservice;

};

class acceptor{
public:
	acceptor(boost::shared_ptr<channelservice> _chservice, boost::shared_ptr<sessioncontainer> _sc);
	~acceptor();

	void init(char * ip, short port);

	void handle_accept(remoteq::EVENT ev);

private:
	void set_fitle(boost::shared_ptr<fitle::fitle<std::pair<std::string, short> > > _epfitle);
	
	boost::shared_ptr<fitle::fitle<std::pair<std::string, short> > > epfitle;
	
	friend class Fossilizid::acceptor::writeacceptor;
	friend class Fossilizid::acceptor::blackacceptor;

private:
	remoteq::ACCEPTOR acp;

	boost::shared_ptr<sessioncontainer> sc;
	boost::shared_ptr<channelservice> chservice;

};

class connector{
public:
	connector(boost::shared_ptr<channelservice> _chservice, boost::shared_ptr<sessioncontainer> _sc);
	~connector();

	boost::shared_ptr<juggle::channel> connect(const char * ip, short port);

	void init();

private:
	boost::shared_ptr<sessioncontainer> sc;
	boost::shared_ptr<channelservice> chservice;

};

} /* namespace achieve */
} /* namespace Fossilizid */

#endif //_channelservice_h
