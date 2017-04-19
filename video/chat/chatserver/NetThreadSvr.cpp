#include "stdafx.h"
#include "NetThreadSvr.h"

#include <WS2tcpip.h>


CNetThreadSvr::CNetThreadSvr()
{
	WSADATA _WSADATA;
	WSAStartup(MAKEWORD(2, 2), &_WSADATA);
}


CNetThreadSvr::~CNetThreadSvr()
{
	WSACleanup();
}

bool CNetThreadSvr::InitNet()
{
	iocp_handle = CreateIoCompletionPort(INVALID_HANDLE_VALUE, 0, 0, 1);

	accept_socket = socket(AF_INET, SOCK_STREAM, IPPROTO_TCP);

	SOCKADDR_IN laddr;
	memset(&laddr, 0, sizeof(laddr));
	laddr.sin_family = AF_INET;
	inet_pton(AF_INET, "127.0.0.1", (void*)&laddr.sin_addr);
	laddr.sin_port = ::htons(4455);
	if (bind(accept_socket, (sockaddr*)(&laddr), sizeof(SOCKADDR_IN)) != 0) 
	{
		return FALSE;
	}

	if (listen(accept_socket, 10) != 0) 
	{
		return FALSE;
	}

	if (CreateIoCompletionPort((HANDLE)accept_socket, iocp_handle, 0, 1) != iocp_handle) 
	{
		return FALSE;
	}

	CUserSocket * user = new CUserSocket();

	memset(accept_buff, 0, 1024);

	overlappedex * ovlp = new overlappedex;
	memset(ovlp, 0, sizeof(overlappedex));
	ovlp->user = user;
	ovlp->event_type = 1;
	OVERLAPPED * ovp = (OVERLAPPED * )(ovlp);
	if (!AcceptEx(accept_socket, user->s, accept_buff, 0, sizeof(SOCKADDR_IN) + 16, sizeof(SOCKADDR_IN) + 16, 0, ovp)) {
		int error = WSAGetLastError();
		if (error != WSA_IO_PENDING) {
			return FALSE;
		}
	}

	AfxBeginThread(CNetThreadSvr::run, (LPVOID)this);

	return TRUE;
}

UINT CNetThreadSvr::run(LPVOID p)
{
	CNetThreadSvr * _pthis = (CNetThreadSvr *)p;

	while (1) 
	{
		DWORD bytes = 0;
		ULONG_PTR ptr = 0;
		LPOVERLAPPED ovp = 0;
		if (GetQueuedCompletionStatus(_pthis->iocp_handle, &bytes, &ptr, &ovp, 15)) 
		{
			overlappedex * ovlp = (overlappedex *)ovp;

			switch (ovlp->event_type)
			{
			case 1: //accept
				{	
					{
						auto accept_user = ovlp->user;
						
						CreateIoCompletionPort((HANDLE)accept_user->s, _pthis->iocp_handle, 0, 0);

						accept_user->postrecv();
					}

					CUserSocket * user = new CUserSocket();

					memset(_pthis->accept_buff, 0, 1024);

					overlappedex * ovlp = new overlappedex;
					ovlp->user = user;
					ovlp->event_type = 1;
					OVERLAPPED * ovp = (OVERLAPPED *)(ovlp);
					memset(ovp, 0, sizeof(OVERLAPPED));
					if (!AcceptEx(_pthis->accept_socket, user->s, _pthis->accept_buff, 0, sizeof(SOCKADDR_IN) + 16, sizeof(SOCKADDR_IN) + 16, 0, ovp)) {
						int error = WSAGetLastError();
						if (error != WSA_IO_PENDING) {
							return 0;
						}
					}
				}
				break;

			case 2: //recv
				{
					auto accept_user = ovlp->user;

					if (bytes == 0)
					{
						_pthis->onUserExit(accept_user->strusername);
					}

					accept_user->process_recv(bytes);
					accept_user->postrecv();
				}
				break;

			case 3: //send
				{
					delete[] ovlp->sendbuff->buf;
					delete ovlp->sendbuff;
				}
				break;

			default:
				break;
			}

			delete ovlp;
		}
	}

	return 1;
}

void CNetThreadSvr::onUserExit(std::string username)
{
	usermap.erase(username);

	for (auto user : usermap)
	{
		int size = headersize + username.size() + 4;

		WSABUF * sendwsabuf = new WSABUF;
		sendwsabuf->buf = new char[size];
		sendwsabuf->len = size;

		*((int*)sendwsabuf->buf) = size;
		*((int*)(sendwsabuf->buf + 4)) = otheruser_exit;
		*((int*)(sendwsabuf->buf + 8)) = username.size();
		memcpy(sendwsabuf->buf + 12, username.c_str(), username.size());

		DWORD bytes = 0;
		overlappedex * ovlp = new overlappedex;
		ovlp->user = user.second;
		ovlp->event_type = 3;
		ovlp->sendbuff = sendwsabuf;
		OVERLAPPED * ovp = (OVERLAPPED *)(ovlp);
		memset(ovp, 0, sizeof(OVERLAPPED));
		WSASend(user.second->s, sendwsabuf, 1, &bytes, 0, ovp, 0);
	}
}