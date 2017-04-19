/*
 * socket_tcp_impl.cpp
 *   Created on: 2013-10-4
 *       Author: qianqians
 * socket_tcp_impl
 */
#include "socket_tcp_impl.h"

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

namespace TCP{

socket_impl::socket_impl() : _revc_state(_no_recv), _connect_state(_not_connect){
}
	
socket_impl::~socket_impl(){
}

socket_impl::socket_impl(async_net::async_service & _service) : async_net::socket_impl(_service), _revc_state(_no_recv), _connect_state(_not_connect){
}

void socket_impl::async_connect(const address & addr){
	if (s == INVALID_SOCKET){
		throw exception::ConnectException("invalid socket");
	}

	if (_connect_state == _connect){
		throw exception::ConnectException("is connected");
	}

	TCP::async_connect(s, addr.getsockaddr(), boost::bind(&socket_impl::connectcallback, this));
}

void socket_impl::connectcallback()
{
	_connectsignal();
}

void socket_impl::async_send(char * inbuf, uint32_t len){
	boost::function<void(void)> fn = boost::bind(&socket_impl::sendcallback, this);
	outbuff.write(inbuf, len, boost::bind(TCP::async_send, s, _1, fn));
}

void socket_impl::sendcallback()
{
	_sendsignal();
}

void socket_impl::async_recv(recv_state _state){
	if (_revc_state != _recv && _state == _recv){
		_revc_state = _state;

		TCP::async_recv(s, inbuff.buff, inbuff.max, boost::bind(&socket_impl::recvcallback, this, _1, _2));
	}
}

void socket_impl::recvcallback(char * buff, uint32_t len)
{
	_recvsignal(buff, len);
}

Hemsleya::abstract_factory::abstract_factory<socket_impl> _abstract_factory_socket_impl;

socket_impl * create_socket_tcp_impl(async_service & _service){
	return _abstract_factory_socket_impl.create_product(_service);
}

void delete_socket_tcp_impl(socket_impl * _s){
	_abstract_factory_socket_impl.release_product(_s);
}

} //TCP

} //async_net
} //Hemsleya
