/*
 * Adapter.h
 *   Created on: 2013-10-2
 *       Author: qianqians
 * Adapter
 */
#ifndef _ASYNC_NET_ADAPTER_H
#define _ASYNC_NET_ADAPTER_H

#ifdef _WINDOWS
#include "windows/winhdef.h"
#elif __linux__ 
//class socket_base_linux;
#endif

#include "async_service.h"
#include "address.h"
#include "socket.h"
#include "callbackdef.h"
#include "enumdef.h"
#include "signaldef.h"

#include <boost/function.hpp>
#include <boost/atomic.hpp>

#include <Hemsleya/base/concurrent/container/msque.h>
#include <Hemsleya/base/concurrent/abstract_factory/abstract_factory.h>

namespace Hemsleya { 
namespace async_net { 

namespace TCP {
	
class Adapter{
public:
	Adapter(async_service & _service, const address & addr);
	~Adapter();

	void async_accept(acceptstate _acceptstate);

private:
	void acceptcallback(socket & s);

public:
	acceptsignal _acceptsignal;

private:
	boost::atomic_flag _mutex;

private:
	ipstate _state;
	async_service * _pservice;
	SOCKET s;

	friend class async_service;

};  

} // TCP

} //async_net
} //Hemsleya

#endif //_ASYNC_NET_ADAPTER_H
