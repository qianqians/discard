/*
 * logicservice.cpp
 *
 *  Created on: 2015-3-30
 *      Author: qianqians
 */
#include "logicservice.h"

#include <pool/mempool.h>

#include <boost/make_shared.hpp>

namespace Fossilizid{
namespace logic{

boost::shared_ptr<logicservice> logicservice::createinstance(){
	logicservice * _plogicservice = (logicservice *)pool::mempool::allocator(sizeof(logicservice));
	new (_plogicservice)logicservice();
	logicservice::_logicservice = boost::shared_ptr<logicservice>(_plogicservice, boost::bind(logicservice::releaselogicservice, _1));
	_logicservice = logicservice::_logicservice;
	return logicservice::_logicservice;
}

boost::shared_ptr<logicservice> logicservice::getinstance(){
	return logicservice::_logicservice;
}

void logicservice::releaselogicservice(logicservice * _plogicservice){
	_plogicservice->~logicservice();
	pool::mempool::deallocator(_plogicservice, sizeof(logicservice));
}

void logicservice::poll(){
	try{
		_service->poll();
	}catch(...){
		throw std::exception("logic trace");
	}
}

uint64_t logicservice::unixtime(){
	return _service->unixtime();
}

logicservice::logicservice(){
	_service = juggle::create_service();
}

logicservice::~logicservice(){
}

void logicservice::init(){
	_service->init();
}

} /* namespace logic */
} /* namespace Fossilizid */

