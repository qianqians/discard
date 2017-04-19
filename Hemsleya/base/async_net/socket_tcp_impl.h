/*
 * socket_tcp_impl.h
 *   Created on: 2013-10-4
 *       Author: qianqians
 * socket_tcp_impl
 */
#ifndef _SOCKET_TCP_IMPL_H
#define _SOCKET_TCP_IMPL_H

#include "async_service.h"
#include "address.h"
#include "writebuff.h"
#include "readbuff.h"
#include "enumdef.h"
#include "callbackdef.h"
#include "signaldef.h"
#include "socket_base_impl.h"
#include "netbuff.h"

#ifdef _WINDOWS
#include "windows/socket_function.h"
#elif __linux__ 
//class socket_base_linux;
#endif

#include <boost/function.hpp>
#include <boost/shared_ptr.hpp>

namespace Hemsleya { 
namespace async_net { 

namespace TCP{

class socket_impl : public async_net::socket_impl{
public:
	socket_impl();
	~socket_impl();

	socket_impl(async_net::async_service & _service);

public:
	void async_send(netbuff buff);
	sendsignal _sendsignal;

private:
	writebuff outbuff;
	void sendcallback();
	boost::atomic_flag _sendcallbackmutex;

public:
	void async_recv(recv_state _state);
	recvsignal _recvsignal;

private:
	readbuf inbuff;
	recv_state _revc_state;
	boost::atomic_flag _recvcallbackmutex;
	void recvcallback(char * buff, uint32_t len);

public:
	void async_connect(const address & addr);
	connectsignal _connectsignal;
	
private:
	boost::atomic_flag _connectcallbackmutex;
	connect_state _connect_state;
	void connectcallback();

private:
	friend class Adapter;

};  

socket_impl * create_socket_tcp_impl(async_service & _service);
void delete_socket_tcp_impl(socket_impl * _s);

}// TCP

} //async_net
} //Hemsleya


#endif //_SOCKET_TCP_IMPL_H
