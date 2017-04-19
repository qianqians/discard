/*
 * socket_udp_impl.cpp
 *   Created on: 2013-10-4
 *       Author: qianqians
 * socket_udp_impl
 */
#include "socket_udp_impl.h"

#include <Hemsleya/base/concurrent/abstract_factory/abstract_factory.h>

// Exception
#include "exception/ConnectException.h"
#include "exception/SendException.h"
#include "exception/RecvException.h"
#include "exception/SendtoException.h"
#include "exception/RecvfromException.h"
#include "exception/bindException.h"

namespace Hemsleya { 
namespace async_net { 

namespace UDP{

socket_impl::socket_impl() : _revc_state(_no_recv){
}
	
socket_impl::~socket_impl(){
}

socket_impl::socket_impl(async_net::async_service & _service) : async_net::socket_impl(_service), _revc_state(_no_recv){
}

void socket_impl::async_sendto(const sockaddr * addr, char * inbuf, uint32_t len){
	boost::function<void(void) > fn = boost::bind(&socket_impl::sendtocallback, this);
	outbuff.write(inbuf, len, boost::bind(UDP::async_sendto, s, addr, _1, fn));
}

void socket_impl::async_recvfrom(recv_state _state){
	if (_revc_state != _recv && _state == _recv){
		_revc_state = _state;

		UDP::async_recvfrom(s, inbuff.buff, inbuff.max, boost::bind(&socket_impl::recvfromcallback, this, _1, _2, _3));
	}
}

Hemsleya::abstract_factory::abstract_factory<socket_impl> _abstract_factory_socket_impl;

socket_impl * create_socket_udp_impl(async_service & _service){
	return _abstract_factory_socket_impl.create_product(_service);
}

void delete_socket_udp_impl(socket_impl * _s){
	_abstract_factory_socket_impl.release_product(_s);
}

} //UDP

} //async_net
} //Hemsleya
