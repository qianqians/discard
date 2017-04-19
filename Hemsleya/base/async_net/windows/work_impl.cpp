/*
 * work_impl.cpp
 *   Created on: 2013-10-2
 *       Author: qianqians
 * work_impl
 */
#ifdef _WINDOWS

#include "work_impl.h"
#include "overlapped.h"
#include "overlappedaccept.h"
#include "overlappedconnect.h"
#include "overlappeddisconnect.h"
#include "overlappedrecv.h"
#include "overlappedrecvfrom.h"
#include "overlappedsend.h"

#include "../exception/InitException.h"

#include <Hemsleya/base/tools/tools.h>

namespace Hemsleya { 
namespace async_net { 

work_impl::work_impl(){
	hiocp = CreateIoCompletionPort(0, 0, 0, processors_count());
	if (hiocp == 0){
		throw exception::InitException("CreateIoCompletionPort fail", WSAGetLastError());
	}
}

work_impl::~work_impl(){
	CloseHandle(hiocp);
}

void work_impl::run(){
	DWORD NumberOfBytes;
	ULONG_PTR CompletionKey;
	LPOVERLAPPED lpOverlapped;

	BOOL bret = GetQueuedCompletionStatus(hiocp, &NumberOfBytes, &CompletionKey, &lpOverlapped, 15);
	if (bret){
		overlapped * poverlapped = container_of(lpOverlapped, overlapped, ovlap);
		switch(poverlapped->type){
		case event_accept:
			{
				static_cast<overlappedaccept*>(poverlapped)->fncallback();
			}
			break;
		
		case event_disconnect:
			{
				closesocket(static_cast<overlappeddisconnect*>(poverlapped)->s);
			}	
			break;

		case event_connect:
			{
				static_cast<overlappedconnect*>(poverlapped)->fncallback();
			}
			break;
	
		case event_send:
			{
				static_cast<overlappedsend*>(poverlapped)->fncallback();
			}
			break;
	
		case event_recv:
			{
				static_cast<overlappedrecv*>(poverlapped)->fncallback(NumberOfBytes);
			}
			break;

		case event_sendto:
			{
				static_cast<overlappedsend*>(poverlapped)->fncallback();
			}
			break;
		
		case event_recvfrom:
			{
				overlappedrecvfrom * poverlappedrecvfrom = static_cast<overlappedrecvfrom*>(poverlapped);
				poverlappedrecvfrom->fncallback(NumberOfBytes, address((sockaddr*)(&poverlappedrecvfrom->remoteaddr), poverlappedrecvfrom->addrlen));
			}
			break;

		default:
			break;
		}
		ReleaseOverlapped(poverlapped);
	}
}

} //async_net
} //Hemsleya

#endif //_WINDOWS