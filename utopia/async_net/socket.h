/*
 * socket.h
 * Created on: 2013-2-24
 *	   Author: qianqians
 * socket �ӿ�
 */
#ifndef _SOCKET_H
#define _SOCKET_H

#include "socket_base.h"
#include <boost/atomic.hpp>

namespace Hemsleya {
namespace async_net {
namespace windows {
class socket_base_WINDOWS;
} // windows

class socket{
public:
	socket();

	socket(async_service & _impl);
	socket(const socket & _s);

	~socket();

	void operator =(const socket & _s);

	bool operator ==(const socket & _s);

	bool operator !=(const socket & _s);

public:
	void register_accpet_handle(AcceptHandle onAccpet);

	void register_recv_handle(RecvHandle onRecv);
	
	void register_connect_handle(ConnectHandle onConnect);
	
	void register_send_handle(SendHandle onSend);

public:
	int bind(sock_addr addr);

public:
	int opensocket(async_service & _impl);
	
	int closesocket();

public:
	int async_accpet(int num, bool bflag);

	int async_accpet(bool bflag);

	int async_recv(bool bflag);
	
	int async_connect(sock_addr addr);

	int async_send(char * buff, unsigned int lenbuff);

	sock_addr get_remote_addr();

private:
	socket_base * _socket;
	boost::atomic_uint * _ref;

	friend class async_service;
	friend class windows::socket_base_WINDOWS;

};

} //async_net
} //Hemsleya

#endif
