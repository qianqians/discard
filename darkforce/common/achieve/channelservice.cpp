/*
 * channelservice.cpp
 *
 *  Created on: 2015-1-17
 *      Author: qianqians
 */
#include "channelservice.h"
#include "channel.h"

#include <boost/make_shared.hpp>

#include <JsonParser.h>

namespace Fossilizid{
namespace achieve{

/*
 * channelservice
 */

channelservice::channelservice(){
}

channelservice::~channelservice(){
}

void channelservice::init(){
	que = remoteq::queue();
}

void channelservice::poll(time_t timeout){
	remoteq::EVENT ev = remoteq::queue(que, timeout);
	
	for(auto cb : callbacks[ev.type]){
		cb(ev);
	}
}

void channelservice::reg_event(remoteq::event_type type, boost::function<void(remoteq::EVENT) > callback){
	callbacks[type].push_back(callback);
}

/*
 * sessioncontainer
 */

sessioncontainer::sessioncontainer(boost::shared_ptr<channelservice> _chservice, boost::shared_ptr<juggle::process> _process){
	process = _process;

	_chservice->reg_event(remoteq::event_type_ipv4_tcp_recv, boost::bind(&sessioncontainer::handle_recv, this, _1));
	_chservice->reg_event(remoteq::event_type_ipv4_tcp_disconnect, boost::bind(&sessioncontainer::handle_disconnection, this, _1));
}

sessioncontainer::~sessioncontainer(){
	mapchannel.clear();
}

boost::shared_ptr<juggle::channel> sessioncontainer::handle_session(remoteq::CHANNEL ch){
	if (ch != 0){
		boost::shared_ptr<juggle::channel> c = boost::make_shared<channel>(ch);
		mapchannel.insert(std::make_pair(ch, c));
		process->add_rpcsession(c);

		sigconn(c);

		return c;
	}

	return 0;
}

void sessioncontainer::handle_recv(remoteq::EVENT ev){
	remoteq::CHANNEL ch = ev.handle.ch;

	boost::any v;
	while (remoteq::ipv4::tcp::pop(ch, v, Fossilizid::JsonParse::unpacker)){
		if (boost::any_cast<boost::shared_ptr<boost::unordered::unordered_map<std::string, boost::any> > >(v)->find("suuid") == boost::any_cast<boost::shared_ptr<boost::unordered::unordered_map<std::string, boost::any> > >(v)->end()){
			continue;
		}
		
		boost::static_pointer_cast<channel>(mapchannel[ch])->handle_recv(boost::any_cast<boost::shared_ptr<boost::unordered::unordered_map<std::string, boost::any> > >(v));
	}
}

void sessioncontainer::handle_disconnection(remoteq::EVENT ev){
	auto ch = mapchannel[ev.handle.ch];
	sigdisconn(ch);
	process->remove_rpcsession(ch);
	mapchannel.erase(ev.handle.ch);
	remoteq::close(ev.handle.ch);
}

/*
 * acceptor
 */

acceptor::acceptor(boost::shared_ptr<channelservice> _chservice, boost::shared_ptr<sessioncontainer> _sc){
	sc = _sc;
	chservice = _chservice;
}

acceptor::~acceptor(){
}

void acceptor::init(char * ip, short port){
	acp = remoteq::ipv4::tcp::acceptor(chservice->que, remoteq::ipv4::endpoint(ip, port));
	chservice->reg_event(remoteq::event_type_ipv4_tcp_accept, boost::bind(&acceptor::handle_accept, this, _1));
}

void acceptor::handle_accept(remoteq::EVENT ev){
	remoteq::CHANNEL ch = remoteq::ipv4::tcp::accept(ev.handle.acp);
	auto ep = remoteq::ipv4::remotep(ch);

	if (!epfitle->isfitle(std::make_pair(remoteq::ipv4::ip(ep), remoteq::ipv4::port(ep)))){
		sc->handle_session(ch);
	}else{
		remoteq::close(ch);
	}
}

void acceptor::set_fitle(boost::shared_ptr<fitle::fitle<std::pair<std::string, short> > > _epfitle){
	epfitle = _epfitle;
}

/*
 * connector
 */

connector::connector(boost::shared_ptr<channelservice> _chservice, boost::shared_ptr<sessioncontainer> _sc){
	sc = _sc;
	chservice = _chservice;
}

connector::~connector(){
}

boost::shared_ptr<juggle::channel> connector::connect(const char * ip, short port){
	remoteq::CHANNEL ch = remoteq::ipv4::tcp::connect(remoteq::ipv4::endpoint(ip, port), chservice->que);
	return sc->handle_session(ch);
}

void connector::init(){
}

} /* namespace achieve */
} /* namespace Fossilizid */