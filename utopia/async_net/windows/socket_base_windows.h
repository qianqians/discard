/*
 * base_socket_WINDOWS.h
 *         Created on: 2012-10-16
 *			   Author: qianqians
 * base socket at windows
 */
#ifndef _BASE_SOCKET_WINDOWS_H
#define _BASE_SOCKET_WINDOWS_H

#ifdef _WINDOWS

#include "winhdef.h"

#include "../socket_base.h"

#include <boost/atomic.hpp>
#include <boost/function.hpp>

namespace Hemsleya {
namespace async_net {
namespace windows {

class socket_base_WINDOWS : public socket_base{
public:
	socket_base_WINDOWS(async_service & _impl);
	~socket_base_WINDOWS();

private:
	void operator =(const socket_base_WINDOWS & s){};

public:
	int bind(sock_addr addr);

	int opensocket();

	int closesocket();

public:
	int async_accpet(int num, bool bflag);

	int async_accpet(bool bflag);
	
	int async_recv(bool bflag);
	
	int async_connect(const sock_addr & addr);

	int async_send(char * buff, unsigned int lenbuff);

public:
	void OnAccept(socket_base * sClient, DWORD llen, _error_code err);
	
	void OnRecv(DWORD llen, _error_code err);
	
	void OnSend(_error_code err);
	
	void OnConnect(_error_code err);
	
	void onDeconnect(_error_code err);
	
	void onClose();

private:
	int do_async_accpet();
	
	int do_async_recv();

	int do_async_send();

	int do_async_connect();

	int do_disconnect();

private:
	SOCKET fd;

	bool isListen;
	
};

} //windows
} //async_net
} //Hemsleya

#endif //_WINDOWS
#endif //_BASE_SOCKET_WINDOWS_H
