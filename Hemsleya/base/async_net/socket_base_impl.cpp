/*
 * socket_base_impl.cpp
 *   Created on: 2013-10-23
 *       Author: qianqians
 * socket_base_impl
 */
#include "socket_base_impl.h"

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

socket_impl::socket_impl(){
}
	
socket_impl::~socket_impl(){
}

socket_impl::socket_impl(async_net::async_service & _service) : pservice(&_service), s(INVALID_SOCKET){
}

void socket_impl::cancelio(){
	CancelIo((HANDLE)s);
}

void socket_impl::bind(const address & addr){
	if (s == INVALID_SOCKET){
		if (addr.version() == ipv4){
			s = ::socket(AF_INET, SOCK_STREAM, 0);
		}else if (addr.version() == ipv6){
			s = ::socket(AF_INET6, SOCK_STREAM, 0);
		}
	}

	if (::bind(s, addr.getsockaddr(), addr.addrlen()) == SOCKET_ERROR){
		throw exception::bindException("bind fail", WSAGetLastError());
	}
}

void socket_impl::shutdown(){
	async_disconnect(s);
}

} //async_net
} //Hemsleya
