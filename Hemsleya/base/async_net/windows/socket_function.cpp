/*
 * socket_function.cpp
 *   Created on: 2013-10-2
 *       Author: qianqians
 * socket_function
 */
#ifdef _WINDOWS

#include "socket_function.h"

#include "overlapped.h"
#include "overlappedaccept.h"
#include "overlappedsend.h"
#include "overlappedconnect.h"
#include "overlappeddisconnect.h"
#include "overlappedrecv.h"
#include "overlappedrecvfrom.h"
#include "wsabuff.h"

// Exception
#include "../exception/AcceptException.h"
#include "../exception/ConnectException.h"
#include "../exception/SendException.h"
#include "../exception/RecvException.h"
#include "../exception/SendtoException.h"
#include "../exception/RecvfromException.h"
#include "../exception/DisConnectException.h"

#include <boost/tuple/tuple.hpp>

namespace Hemsleya { 
namespace async_net { 

void async_disconnect(SOCKET s){
	LPFN_DISCONNECTEX DisConnectEx;
	DWORD bytes = 0;

	GUID guid = WSAID_DISCONNECTEX;
	if (WSAIoctl(s, SIO_GET_EXTENSION_FUNCTION_POINTER, &guid, sizeof(guid), &DisConnectEx, sizeof(DisConnectEx), &bytes, 0, 0) != 0){
		throw exception::DisConnectException("Get LPFN_DISCONNECTEX fail", GetLastError());
	}

	overlappeddisconnect * poverlappeddisconnect = static_cast<overlappeddisconnect*>(GetOverlapped(event_disconnect));
	poverlappeddisconnect->s = s;
	if (!DisConnectEx(s, &poverlappeddisconnect->ovlap, 0, 0)){
		throw exception::DisConnectException("DisConnectEx fail", GetLastError());
	}
}

namespace TCP{

void aysnc_accept(SOCKET sl, SOCKET sa, char * recvbuff, boost::function<void() > acceptcallback){
	DWORD bytes = 0;

	overlappedaccept * poverlappedaccept = static_cast<overlappedaccept*>(GetOverlapped(event_accept));
	poverlappedaccept->fncallback = acceptcallback;
	BOOL bret = AcceptEx(sl, sa, recvbuff, 0, sizeof(SOCKADDR_IN6), sizeof(SOCKADDR_IN6), &bytes, &(poverlappedaccept->ovlap));
	if (!bret){
		int err = WSAGetLastError();
		if (err != ERROR_IO_PENDING){
			throw exception::AcceptException("AcceptEx fail", err);
		}
	}
}

void async_connect(SOCKET s, const sockaddr * addr, boost::function<void() > connectcallback){
	LPFN_CONNECTEX ConnectEx;
	DWORD bytes = 0;

	GUID guid = WSAID_CONNECTEX;
	if (WSAIoctl(s, SIO_GET_EXTENSION_FUNCTION_POINTER, &guid, sizeof(guid), &ConnectEx, sizeof(ConnectEx), &bytes, 0, 0) != 0){
		throw exception::ConnectException("Get LPFN_CONNECTEX fail", GetLastError());
	}

	overlappedconnect * poverlappedconnect = static_cast<overlappedconnect*>(GetOverlapped(event_connect));
	poverlappedconnect->fncallback = connectcallback;
	if (!ConnectEx(s, addr, sizeof(addr), 0, 0, &bytes, &(poverlappedconnect->ovlap))){
		int err = WSAGetLastError();
		if (err != ERROR_IO_PENDING){
			throw exception::ConnectException("ConnectEx fail", err);
		}
	}
}

void async_send(SOCKET s, const std::vector<buffstruct> * const buff, boost::function<void() > sendcallback){
	overlappedsend * poverlapped = static_cast<overlappedsend*>(GetOverlapped(event_send));

	WSABUF * pWSABUF = GetWSABUF(buff->size());
	for(uint32_t i = 0; i < buff->size(); i++){
		pWSABUF[i].buf = (*buff)[i].buff;
		pWSABUF[i].len = (*buff)[i].len;
	}
	poverlapped->vectbuff.assign(buff->begin(), buff->end());
	poverlapped->fncallback = sendcallback;

	DWORD bytes;
	if (SOCKET_ERROR == WSASend(s, pWSABUF, buff->size(), &bytes, 0, &poverlapped->ovlap, 0)){
		int err = WSAGetLastError();
		if (err != ERROR_IO_PENDING){
			throw exception::SendException("Send fail", err);
		}
	}
}

void async_recv(SOCKET s, char * buff, const uint32_t buflen, boost::function<void(char * buff, uint32_t recvlen) > recvcallback){
	WSABUF * pWSABUF = GetWSABUF(1);
	pWSABUF->buf = buff;
	pWSABUF->len = buflen;

	overlappedrecv * poverlappedrecv = static_cast<overlappedrecv*>(GetOverlapped(event_recv));
	poverlappedrecv->fncallback = boost::bind(recvcallback, buff, _1);

	DWORD bytes = 0;
	if (SOCKET_ERROR == WSARecv(s, pWSABUF, 1, &bytes, 0, &(poverlappedrecv->ovlap), 0)){
		int err = WSAGetLastError();
		if (err != ERROR_IO_PENDING){
			throw exception::RecvException("Recv fail", err);
		}
	}
}

} // TCP

namespace UDP{

void async_sendto(SOCKET s, const sockaddr * addr, const std::vector<buffstruct> * const buff, boost::function<void() > sendtocallback){
	WSABUF * pWSABUF = GetWSABUF(buff->size());
	for(uint32_t i = 0; i < buff->size(); i++){
		pWSABUF[i].buf = (*buff)[i].buff;
		pWSABUF[i].len = (*buff)[i].len;
	}

	overlappedsend * poverlapped = static_cast<overlappedsend*>(GetOverlapped(event_sendto));
	poverlapped->fncallback = sendtocallback;
	poverlapped->vectbuff.assign(buff->begin(), buff->end());

	DWORD bytes;
	if (SOCKET_ERROR == WSASendTo(s, pWSABUF, buff->size(), &bytes, 0, addr, sizeof(addr), &poverlapped->ovlap, 0)){
		int err = WSAGetLastError();
		if (err != ERROR_IO_PENDING){
			throw exception::SendtoException("Sendto fail", err);
		}
	}
}

void async_recvfrom(SOCKET s, char * buff, const uint32_t buflen, boost::function<void(char * buff, uint32_t len, const address & addr) > recvfromcallback){
	WSABUF * pWSABUF = GetWSABUF(1);
	pWSABUF->buf = buff;
	pWSABUF->len = buflen;

	overlappedrecvfrom * poverlappedrecvfrom = static_cast<overlappedrecvfrom*>(GetOverlapped(event_recv));
	poverlappedrecvfrom->fncallback = boost::bind(recvfromcallback, buff, _1, _2);
	
	DWORD bytes = 0;
	if (SOCKET_ERROR == WSARecvFrom(s, pWSABUF, 1, &bytes, 0, (sockaddr*)&poverlappedrecvfrom->remoteaddr, &poverlappedrecvfrom->addrlen, &poverlappedrecvfrom->ovlap, 0)){
		int err = WSAGetLastError();
		if (err != ERROR_IO_PENDING){
			throw exception::RecvfromException("Recvfrom fail", err);
		}
	}
}

} // UDP

} //async_net
} //Hemsleya

#endif //_WINDOWS