/*
 * adapter.cpp
 *   Created on: 2013-10-26
 *       Author: qianqians
 * adapter
 */
#include "adapter.h"
#include "address.h"
#include "connservice.h"

namespace Hemsleya { 
namespace connect {

adapter::adapter(connservice & _service, const address & tcpaddr, const address & udpaddr) :
_adapter(_service._service, tcpaddr), _socket(_service._service), iSessionID(0){
	_socket.bind(udpaddr);
	_adapter._acceptsignal.connect(boost::bind(&adapter::acceptcallback, this, _1));
}

adapter::~adapter(){
}

void adapter::accept(accept_state _state){
	if (_state == begin_accept){
		_adapter.async_accept(async_net::TCP::beginaccept);
	}else if (_state == end_accept){
		_adapter.async_accept(async_net::TCP::endaccept);
	}
}

void adapter::acceptcallback(async_net::TCP::socket & sa){

}

} //connect
} //Hemsleya
