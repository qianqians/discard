/*
 * socket_pool.h
 * Created on: 2013-2-24
 *	   Author: qianqians
 * socket ½Ó¿Ú
 */
#ifndef _SOCKE_POOLT_H
#define _SOCKE_POOLT_H

#ifdef _WINDOWS
#include "windows/socket_base_WINDOWS.h"
#endif //_WINDOWS

#include <Hemsleya/base/concurrent/abstract_factory/abstract_factory.h>
#include <Hemsleya/base/concurrent/container/msque.h>

namespace Hemsleya {
namespace async_net {

namespace detail {
	


class SocketPool{
public:
	static void Init(){
	}

	static socket_base * get(async_service & _impl){
		socket_base * _socket = 0;
		if (!_socket_pool.pop(_socket)){
			_socket_factory.create_product(_impl);
		}
		_socket->isclosed = false;
		_socket->isrecv = false;
		_socket->isaccept = false;
		_socket->isdisconnect = true;
		return _socket;
	}

	static void release(socket_base * _socket){
		_socket_pool.push(_socket);
	}

private:	
#ifdef _WINDOWS
	static Hemsleya::abstract_factory::abstract_factory<windows::socket_base_WINDOWS > _socket_factory;
#endif
	static Hemsleya::container::msque<socket_base * > _socket_pool;

};

} //detail

} //async_net
} //Hemsleya

#endif //_SOCKE_POOLT_H