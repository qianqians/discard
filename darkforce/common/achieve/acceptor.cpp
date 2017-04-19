/*
 * acceptor.cpp
 *
 *  Created on: 2015-1-17
 *      Author: qianqians
 */
#include "channelservice.h"

#include "../acceptor/blackacceptor.h"
#include "../acceptor/writeacceptor.h"

#include <boost/make_shared.hpp>

namespace Fossilizid{
namespace acceptor{

blackacceptor::blackacceptor(std::string ip, short port, boost::shared_ptr<std::vector<std::pair<std::string, short> > > _set, boost::shared_ptr<achieve::channelservice> _chservice, boost::shared_ptr<achieve::sessioncontainer> _sc){
	acp = boost::make_shared<achieve::acceptor>(_chservice, _sc);

	acp->init((char*)ip.c_str(), port);

	set = _set;
	
	epfitle = boost::make_shared<fitle::fitle<std::pair<std::string, short> > >();

	epfitle->setfitle(
		[this](std::pair<std::string, short> ep){
			for(auto e : *set){
				if (ep.first == e.first){
					if (e.second == 0){
						return true;
					}else if (e.second == ep.second){
						return true;
					}
				}
			}
			return false;
		}
	);

	acp->set_fitle(epfitle);
}

blackacceptor::~blackacceptor(){
}

writeacceptor::writeacceptor(std::string ip, short port, boost::shared_ptr<std::vector<std::pair<std::string, short> > > _set, boost::shared_ptr<achieve::channelservice> _chservice, boost::shared_ptr<achieve::sessioncontainer> _sc){
	acp = boost::make_shared<achieve::acceptor>(_chservice, _sc);
	
	acp->init((char*)ip.c_str(), port);

	set = _set;

	epfitle = boost::make_shared<fitle::fitle<std::pair<std::string, short> > >();

	epfitle->setfitle(
		[this](std::pair<std::string, short> ep){
			for(auto e : *set){
				if (ep.first == e.first){
					if (e.second == 0){
						return false;
					}else if (e.second == ep.second){
						return false;
					}
				}
			}
			return true;
		}
	);

	acp->set_fitle(epfitle);
}

writeacceptor::~writeacceptor(){
}

void writeacceptor::add_write_addr(std::string ip, short port){
	set->push_back(std::make_pair(ip, port));
}

} /* namespace acceptor */
} /* namespace Fossilizid */

