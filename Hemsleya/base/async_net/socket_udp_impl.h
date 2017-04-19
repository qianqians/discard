/*
 * socket_udp_impl.h
 *   Created on: 2013-10-20
 *       Author: qianqians
 * socket_udp_impl
 */
#ifndef _SOCKET_UDP_IMPL_H
#define _SOCKET_UDP_IMPL_H

#include "async_service.h"
#include "address.h"
#include "writebuff.h"
#include "readbuff.h"
#include "enumdef.h"
#include "callbackdef.h"
#include "socket_base_impl.h"

#ifdef _WINDOWS
#include "windows/socket_function.h"
#elif __linux__ 
//class socket_base_linux;
#endif

#include <boost/function.hpp>
#include <boost/shared_ptr.hpp>

namespace Hemsleya { 
namespace async_net { 

namespace UDP{

class socket_impl : public async_net::socket_impl{
public:
	socket_impl();
	~socket_impl();

	socket_impl(async_net::async_service & _service);

public:
	void async_sendto(const sockaddr * addr, char * buf, uint32_t len);
	sendtosignal _sendtosignal;

private:
	writebuff outbuff;
	void sendtocallback();
	boost::atomic_flag _sendcallbackmutex;

public:
	void async_recvfrom(recv_state _state);
	recvfromsignal _recvfromsignal;

private:
	readbuf inbuff;
	recv_state _revc_state;
	void recvfromcallback(char * buff, uint32_t len, const address & addr);
	boost::atomic_flag _recvcallbackmutex;
	
private:
	friend class Adapter;

};  

socket_impl * create_socket_udp_impl(async_service & _service);
void delete_socket_udp_impl(socket_impl * _s);

}// UDP

} //async_net
} //Hemsleya


#endif //_SOCKET_UDP_IMPL_H
