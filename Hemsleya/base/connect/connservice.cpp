/*
 * connservice.cpp
 *   Created on: 2013-10-4
 *       Author: qianqians
 * connservice
 */
#include "connservice.h"
#include "session.h"

namespace Hemsleya { 
namespace connect {

connservice::connservice(){
}

connservice::~connservice(){
}

void connservice::run(){
	_service.run();
}

void connservice::addconn(session & _session){
	_service.addsocket(_session._impl->tcpsocket);
}

} //connect
} //Hemsleya
