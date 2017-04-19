/*
 * address.h
 *   Created on: 2012-10-16
 *       Author: qianqians
 * address
 */
#ifndef _ADDRESS_H
#define _ADDRESS_H

#ifdef _WINDOWS
#include "windows/winhdef.h"
#elif __linux__ 
//class socket_base_linux;
#endif

#include <boost/cstdint.hpp>

enum ipstate{
	ipv4,
	ipv6,
};

namespace Hemsleya { 
namespace async_net { 

class address{
public:
	address(){ memset(addr, 0, sizeof(sockaddr_in6)); }

	address(char * ip, short port);
	address(const address & _address);
	address(sockaddr * addr, int len);
	~address();

	void operator = (const address & _address);

public:
	const sockaddr * getsockaddr() const;

	const uint32_t addrlen() const;

	const ipstate version() const;

private:
	char addr[sizeof(sockaddr_in6)];

	ipstate _ipstate;

};  

} //async_net
} //Hemsleya

#endif //_ADDRESS_H
