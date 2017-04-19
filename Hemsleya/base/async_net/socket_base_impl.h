/*
 * socket_base_impl.h
 *   Created on: 2013-10-23
 *       Author: qianqians
 * socket_base_impl
 */
#ifndef _SOCKET_BASE_IMPL_H
#define _SOCKET_BASE_IMPL_H

#include "async_service.h"
#include "address.h"
#include "writebuff.h"
#include "readbuff.h"
#include "enumdef.h"
#include "callbackdef.h"

#ifdef _WINDOWS
#include "windows/socket_function.h"
#elif __linux__ 
//class socket_base_linux;
#endif

#include <boost/function.hpp>
#include <boost/shared_ptr.hpp>

namespace Hemsleya { 
namespace async_net { 

class socket_impl{
public:
	socket_impl();
	~socket_impl();

	socket_impl(async_net::async_service & _service);

public:
	void cancelio();

	void bind(const address & addr);
	
	void shutdown();
 
protected:
	SOCKET s;
	async_service * pservice;

	friend class Adapter;
	friend class async_service;

};  

} //async_net
} //Hemsleya


#endif //_SOCKET_BASE_IMPL_H
