/*
 * async_service_WINDOWS.cpp
 *   Created on: 2012-11-14
 *       Author: qianqians
 * async_service windows ʵ��
 */
#ifdef _WINDOWS

#include "winhdef.h"

#include <Hemsleya/base/exception/exception.h>

#include "../async_service.h"
#include "../socket.h"
#include "../socket_pool.h"
#include "../buff_pool.h"
#include "../read_buff_pool.h"
#include "../write_buff_pool.h"

#include "Overlapped.h"
#include "socket_base_WINDOWS.h"

namespace Hemsleya { 
namespace async_net { 

async_service::async_service() : nConnect(0), nMaxConnect(0xffff) {
	WSADATA data;
	WSAStartup(MAKEWORD(2,2), &data);

	hIOCP = CreateIoCompletionPort(INVALID_HANDLE_VALUE, NULL, NULL, 0);
	if(hIOCP == 0) {
		throw Hemsleya::exception::exception("Error: CreateIoCompletionPort failed.");
	}

	windows::detail::OverlappedEXPool<windows::OverlappedEX >::Init();
	windows::detail::OverlappedEXPool<windows::OverlappedEX_close>::Init();
	windows::detail::OverlappedEXPool<windows::OverlappedEX_Accept >::Init();

	Init();
}

async_service::~async_service(){
	CloseHandle(hIOCP);
	WSACleanup();
}

bool async_service::network() {
	DWORD nBytesTransferred = 0;
	socket_base * pHandle = 0;
	LPOVERLAPPED pOverlapped = 0;
	_error_code err = 0;

	BOOL bret = GetQueuedCompletionStatus(hIOCP, &nBytesTransferred, (PULONG_PTR)&pHandle, &pOverlapped, INFINITE);
	if(!bret) {
		err = GetLastError();
	}
			
	windows::OverlappedEX * pOverlappedEX = container_of(pOverlapped, windows::OverlappedEX, overlap);
	if (pOverlappedEX->type == win32_tcp_send_complete){
		((windows::socket_base_WINDOWS*)pHandle)->OnSend(err);
		windows::detail::OverlappedEXPool<windows::OverlappedEX >::release(pOverlappedEX);
	}else if (pOverlappedEX->type == win32_tcp_recv_complete){
		((windows::socket_base_WINDOWS*)pHandle)->OnRecv(nBytesTransferred, err);
		windows::detail::OverlappedEXPool<windows::OverlappedEX >::release(pOverlappedEX);
	}else if (pOverlappedEX->type == win32_tcp_connect_complete){
		((windows::socket_base_WINDOWS*)pHandle)->OnConnect(err);
		windows::detail::OverlappedEXPool<windows::OverlappedEX >::release(pOverlappedEX);
	}else if (pOverlappedEX->type == win32_tcp_accept_complete){
		windows::OverlappedEX_Accept * _OverlappedEXAccept = container_of(pOverlappedEX, windows::OverlappedEX_Accept, overlapex);
		((windows::socket_base_WINDOWS*)pHandle)->OnAccept(_OverlappedEXAccept->socket_, nBytesTransferred, err);
		windows::detail::OverlappedEXPool<windows::OverlappedEX_Accept >::release(_OverlappedEXAccept);
	}else if (pOverlappedEX->type == win32_tcp_close_complete){
		windows::OverlappedEX_close * _OverlappedEXClose = container_of(pOverlappedEX, windows::OverlappedEX_close, overlapex);
		((windows::socket_base_WINDOWS*)pHandle)->onClose();
		windows::detail::OverlappedEXPool<windows::OverlappedEX_close >::release(_OverlappedEXClose);
	}else if (pOverlappedEX->type == win32_stop_){
		windows::detail::OverlappedEXPool<windows::OverlappedEX >::release(pOverlappedEX);
		return false;
	}

	return true;
}

} //async_net
} //Hemsleya

#endif //_WINDOWS
