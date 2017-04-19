/*
 * overlapped.h
 *   Created on: 2012-10-16
 *       Author: qianqians
 * overlapped
 */
#ifndef _OVERLAPPED_H
#define _OVERLAPPED_H

#ifdef _WINDOWS

#include "winhdef.h"

#include "../socket_tcp_impl.h"

namespace Hemsleya { 
namespace async_net { 

enum eventtype{
	event_accept,
	event_connect,
	event_disconnect,
	event_send,
	event_recv,
	event_sendto,
	event_recvfrom,
};

struct overlapped {
	eventtype type;
	OVERLAPPED ovlap;
};

overlapped * GetOverlapped(eventtype _eventtype);
void ReleaseOverlapped(overlapped * poverlapped);

} //async_net
} //Hemsleya

#endif //_WINDOWS

#endif //_OVERLAPPED_H
