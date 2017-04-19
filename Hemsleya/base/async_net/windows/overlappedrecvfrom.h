/*
 * overlappedrecvfrom.h
 *   Created on: 2013-10-4
 *       Author: qianqians
 * overlappedrecvfrom
 */
#ifndef _OVERLAPPEDRECVFROM_H
#define _OVERLAPPEDRECVFROM_H

#ifdef _WINDOWS

#include "winhdef.h"
#include "overlapped.h"

namespace Hemsleya { 
namespace async_net { 

struct overlappedrecvfrom : public overlapped{
	SOCKADDR_IN6 remoteaddr;
	int addrlen;
	boost::function<void(uint32_t recvlen, const address & addr) > fncallback;
};

} //async_net
} //Hemsleya

#endif //_WINDOWS

#endif //_OVERLAPPEDRECVFROM_H
