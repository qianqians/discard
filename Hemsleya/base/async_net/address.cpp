/*
 * address.cpp
 *   Created on: 2013-10-4
 *       Author: qianqians
 * async_service
 */
#include "address.h"

#include "exception/AddresException.h"

#include <boost/cstdint.hpp>

namespace Hemsleya { 
namespace async_net { 

address::address(char * ip, short port){
	memset(&addr, 0, sizeof(sockaddr_in6));

	if (strchr(ip, '.') != 0){
		_ipstate = ipv4;

		((sockaddr_in*)addr)->sin_family = AF_INET;
		((sockaddr_in*)addr)->sin_port = ntohs(port);
		((sockaddr_in*)addr)->sin_addr.S_un.S_addr = inet_addr(ip);
	}else{
		_ipstate = ipv6;

		char buf[64];
		int err = 0;
		if ((err = RtlIpv6StringToAddress(ip, (PCSTR*)&buf, &(((sockaddr_in6*)addr)->sin6_addr))) != NO_ERROR){
			throw exception::AddresException("Error address", err);
		}

		((sockaddr_in6*)addr)->sin6_family = AF_INET6;
		((sockaddr_in6*)addr)->sin6_port = ntohs(port);
	}
}

address::address(sockaddr * _addr, int len){
	if (len == sizeof(sockaddr_in)){
		_ipstate = ipv4;
		memcpy(addr, _addr, sizeof(sockaddr_in));
	}else if (len == sizeof(sockaddr_in6)){
		_ipstate = ipv6;
		memcpy(addr, _addr, sizeof(sockaddr_in6));
	}
}

address::address(const address & _address){
	_ipstate = _address._ipstate;

	if (_ipstate == ipv4){
		memcpy(addr, _address.addr, sizeof(sockaddr_in));
	} else if (_ipstate == ipv6){
		memcpy(addr, _address.addr, sizeof(sockaddr_in6));
	}
}

address::~address(){
}

void address::operator = (const address & _address){
	_ipstate = _address._ipstate;

	if (_ipstate == ipv4){
		memcpy(addr, _address.addr, sizeof(sockaddr_in));
	} else if (_ipstate == ipv6){
		memcpy(addr, _address.addr, sizeof(sockaddr_in6));
	}
}

const sockaddr * address::getsockaddr() const{
	return ((sockaddr*)addr);
}

const ipstate address::version() const{
	return _ipstate;
}

const uint32_t address::addrlen() const{
	uint32_t len = 0;

	if (_ipstate == ipv4){
		len = sizeof(sockaddr_in);
	}else if (_ipstate == ipv6){
		len = sizeof(sockaddr_in6);
	}

	return len;
}

} //async_net
} //Hemsleya
