#include "stdafx.h"
#include "UserSocket.h"
#include "NetThreadSvr.h"
#include "chatserver.h"


CUserSocket::CUserSocket()
{
	s = socket(AF_INET, SOCK_STREAM, 0);

	memset(recvbuff, 0, 16 * 1024 * 1024);
	recvbuffpos = 0;
}


CUserSocket::~CUserSocket()
{
}

void CUserSocket::process_recv(int bufferlen)
{
	recvbuffpos += bufferlen;

	while (recvbuffpos >= headersize)
	{
		//process net event
		int netbufferlen = *((int*)recvbuff);
		if (recvbuffpos < netbufferlen)
		{
			return;
		}

		int cmdid = *((int*)(recvbuff + 4));
		switch (cmdid)
		{
		case request_register_event:
			{
				WSABUF * sendwsabuf = new WSABUF;
				sendwsabuf->buf = new char[8];
				sendwsabuf->len = 8;

				*((int*)sendwsabuf->buf) = 8;
				*((int*)(sendwsabuf->buf+4)) = response_register_event;

				DWORD bytes = 0;
				overlappedex * ovlp = new overlappedex;
				ovlp->user = this;
				ovlp->event_type = 3;
				ovlp->sendbuff = sendwsabuf;
				OVERLAPPED * ovp = (OVERLAPPED *)(ovlp);
				memset(ovp, 0, sizeof(OVERLAPPED));
				WSASend(s, sendwsabuf, 1, &bytes, 0, ovp, 0);
			}
			break;

		case request_login2_event:
			{
				auto usernamestrlen = *((int*)(recvbuff + 8));
				char * username = recvbuff + 12;

				auto it = theApp.netsvr_interface.usermap.find(std::string(username, usernamestrlen));
				if (it != theApp.netsvr_interface.usermap.end())
				{
					WSABUF * sendwsabuf = new WSABUF;
					sendwsabuf->buf = new char[8];
					sendwsabuf->len = 8;

					*((int*)sendwsabuf->buf) = 8;
					*((int*)(sendwsabuf->buf + 4)) = error_reusername;

					DWORD bytes = 0;
					overlappedex * ovlp = new overlappedex;
					ovlp->user = this;
					ovlp->event_type = 3;
					ovlp->sendbuff = sendwsabuf;
					OVERLAPPED * ovp = (OVERLAPPED *)(ovlp);
					memset(ovp, 0, sizeof(OVERLAPPED));
					WSASend(s, sendwsabuf, 1, &bytes, 0, ovp, 0);

					break;
				}

				WSABUF * sendwsabuf = new WSABUF;
				sendwsabuf->buf = new char[8];
				sendwsabuf->len = 8;

				*((int*)sendwsabuf->buf) = 8;
				*((int*)(sendwsabuf->buf + 4)) = response_login2_event;

				DWORD bytes = 0;
				overlappedex * ovlp = new overlappedex;
				ovlp->user = this;
				ovlp->event_type = 3;
				ovlp->sendbuff = sendwsabuf;
				OVERLAPPED * ovp = (OVERLAPPED *)(ovlp);
				memset(ovp, 0, sizeof(OVERLAPPED));
				WSASend(s, sendwsabuf, 1, &bytes, 0, ovp, 0);

				for (auto user : theApp.netsvr_interface.usermap)
				{
					int size = headersize + usernamestrlen + 4;

					WSABUF * sendwsabuf = new WSABUF;
					sendwsabuf->buf = new char[size];
					sendwsabuf->len = size;

					*((int*)sendwsabuf->buf) = size;
					*((int*)(sendwsabuf->buf + 4)) = broadcast_userinfo;
					*((int*)(sendwsabuf->buf + 8)) = usernamestrlen;
					memcpy(sendwsabuf->buf + 12, username, usernamestrlen);

					DWORD bytes = 0;
					overlappedex * ovlp = new overlappedex;
					ovlp->user = this;
					ovlp->event_type = 3;
					ovlp->sendbuff = sendwsabuf;
					OVERLAPPED * ovp = (OVERLAPPED *)(ovlp);
					memset(ovp, 0, sizeof(OVERLAPPED));
					WSASend(user.second->s, sendwsabuf, 1, &bytes, 0, ovp, 0);
				}

				strusername = std::string(username, usernamestrlen);

				theApp.netsvr_interface.usermap.insert(std::make_pair(std::string(username, usernamestrlen), this));

				if (!theApp.netsvr_interface.usermap.empty())
				{
					int size = headersize + 4;
					for (auto user : theApp.netsvr_interface.usermap)
					{
						size += user.first.size() + 4;
					}

					WSABUF * sendwsabuf = new WSABUF;
					sendwsabuf->buf = new char[size];
					sendwsabuf->len = size;

					*((int*)sendwsabuf->buf) = size;
					*((int*)(sendwsabuf->buf + 4)) = post_userinfolist;

					*((int*)(sendwsabuf->buf + 8)) = theApp.netsvr_interface.usermap.size();
					int pos = 12;
					for (auto user : theApp.netsvr_interface.usermap)
					{
						*((int*)(sendwsabuf->buf + pos)) = user.first.length();
						pos += 4;
						memcpy(sendwsabuf->buf + pos, user.first.c_str(), user.first.length());
						pos += user.first.length();
					}

					DWORD bytes = 0;
					overlappedex * ovlp = new overlappedex;
					ovlp->user = this;
					ovlp->event_type = 3;
					ovlp->sendbuff = sendwsabuf;
					OVERLAPPED * ovp = (OVERLAPPED *)(ovlp);
					memset(ovp, 0, sizeof(OVERLAPPED));
					WSASend(s, sendwsabuf, 1, &bytes, 0, ovp, 0);
				}
			}
			break;

		case exit_client:
			{
				theApp.netsvr_interface.onUserExit(strusername);
			}
			break;

		case video_event:
			{
				for (auto user : theApp.netsvr_interface.usermap)
				{
					int size = netbufferlen;

					WSABUF * sendwsabuf = new WSABUF;
					sendwsabuf->buf = new char[size];
					sendwsabuf->len = size;

					memcpy(sendwsabuf->buf, recvbuff, size);

					DWORD bytes = 0;
					overlappedex * ovlp = new overlappedex;
					ovlp->user = this;
					ovlp->event_type = 3;
					ovlp->sendbuff = sendwsabuf;
					OVERLAPPED * ovp = (OVERLAPPED *)(ovlp);
					memset(ovp, 0, sizeof(OVERLAPPED));
					WSASend(user.second->s, sendwsabuf, 1, &bytes, 0, ovp, 0);
				}
			}
			break;

		default:
			break;
		}

		recvbuffpos -= netbufferlen;
	}
}

void CUserSocket::postrecv()
{
	recvwsabuf.buf = recvbuff + recvbuffpos;
	recvwsabuf.len = 16 * 1024 * 1024 - recvbuffpos;
	DWORD bytes = 0;
	DWORD flags = 0;
	overlappedex * ovlp = new overlappedex;
	ovlp->user = this;
	ovlp->event_type = 2;
	OVERLAPPED * ovp = (OVERLAPPED *)(ovlp);
	memset(ovp, 0, sizeof(OVERLAPPED));
	WSARecv(s, &recvwsabuf, 1, &bytes, &flags, ovp, 0);
}