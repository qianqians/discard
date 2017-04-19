/*
 * async_service.h
 *   Created on: 2012-10-16
 *       Author: qianqians
 * async_service
 */
#ifndef _NET_SERVICE_H
#define _NET_SERVICE_H

#ifdef _WINDOWS
#include "windows/winhdef.h"
#include "windows/work_impl.h"
#elif __linux__ 
//class socket_base_linux;
#endif

#include "socket.h"
#include "Adapter.h"

namespace Hemsleya { 
namespace async_net { 

class async_service{
public:
	async_service();
	~async_service();

	void run();

	void addAdapter(TCP::Adapter & e);
	void addsocket(TCP::socket & s);
	void addsocket(UDP::socket & s);

private:
	work_impl _impl;

};  

} //async_net
} //Hemsleya

#endif //_NET_SERVICE_H
