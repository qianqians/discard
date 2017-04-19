/*
 * async_service.cpp
 *   Created on: 2012-11-14
 *       Author: qianqians
 * async_service
 */
#include "async_service.h"
#include "socket_tcp_impl.h"
#include "socket_udp_impl.h"
#include "socket_base_impl.h"

namespace Hemsleya { 
namespace async_net { 

async_service::async_service(){
}

async_service::~async_service(){
}

void async_service::run(){
	_impl.run();
}

void async_service::addAdapter(TCP::Adapter & e){
	_impl.addsocket(e.s);
}

void async_service::addsocket(TCP::socket & s){
	_impl.addsocket(s.sptr->s);
}

void async_service::addsocket(UDP::socket & s){
	_impl.addsocket(s.sptr->s);
}

} //async_net
} //Hemsleya
