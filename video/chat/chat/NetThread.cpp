#include "stdafx.h"
#include "NetThread.h"
#include "chat.h"

CNetThread::CNetThread()
{
	memset(buffer, 0, 16 * 1024 * 1024);
	bufferpos = 0;
}


CNetThread::~CNetThread()
{
}

bool CNetThread::InitNet()
{
	s = socket(AF_INET, SOCK_STREAM, IPPROTO_TCP);

	SOCKADDR_IN addr;
	memset(&addr, 0, sizeof(addr));
	addr.sin_family = AF_INET;
	inet_pton(AF_INET, "127.0.0.1", (void*)&addr.sin_addr);
	addr.sin_port = ::htons(0);
	if (bind(s, (SOCKADDR*)&addr, sizeof(SOCKADDR_IN)) != 0) {
		int error = WSAGetLastError();
		printf("error %d\n", error);
		closesocket(s);
		return FALSE;
	}

	SOCKADDR_IN remateaddr;
	memset(&remateaddr, 0, sizeof(remateaddr));
	remateaddr.sin_family = AF_INET;
	inet_pton(AF_INET, "127.0.0.1", (void*)&remateaddr.sin_addr);
	remateaddr.sin_port = ::htons(4455);
	if (connect(s, (SOCKADDR*)&(remateaddr), sizeof(SOCKADDR_IN)) != 0) {
		int error = WSAGetLastError();
		printf("error %d\n", error);
		closesocket(s);
		return FALSE;
	}


	AfxBeginThread(CNetThread::run, (LPVOID)this);

	return TRUE;
}

//protocl
//header
//int buffersize
//int cmdid 
int headersize = 8;
//data

//cmd
#define request_register_event 101
#define response_register_event 102
#define request_login_event 103
#define response_login_event 104
#define request_login2_event 105
#define response_login2_event 106

#define broadcast_userinfo 107 
#define post_userinfolist 108

#define exit_client 109

#define reusername_error 110

#define otheruser_exit 111

#define video_event 112

UINT CNetThread::run(LPVOID p)
{
	CNetThread * _pthis = (CNetThread *)p;

	while (1) 
	{
		int bufferlen = recv(_pthis->s, _pthis->buffer, 1024*1024*16, 0);

		if (bufferlen == -1)
		{
			MessageBox(NULL, "ÍøÂç¶Ï¿ª", "´íÎóÌáÊ¾", MB_OK);
			exit(0);
		}

		_pthis->bufferpos += bufferlen;

		if (_pthis->bufferpos >= headersize) 
		{
			//process net event
			int netbufferlen = *((int*)_pthis->buffer);
			if (_pthis->bufferpos < netbufferlen) 
			{
				continue;
			}

			int cmdid = *((int*)(_pthis->buffer+4));
			switch (cmdid)
			{
			case response_register_event:
				//SendMessage(theApp.main_hwnd, WM_SOCKET_MSG_REGISTER, 0, 0);
				break;

			case response_login_event:
				
				//SendMessage(theApp.main_hwnd, WM_SOCKET_MSG_LOGIN, 0, 0);
				break;

			case response_login2_event:
				SendMessage(theApp.login_hwnd, WM_SOCKET_MSG_LOGIN2, 0, 0);
				break;

			case broadcast_userinfo:
				{
					auto usernamelen = *((int*)(_pthis->buffer + 8));
					theApp.user_dlg.addUser(CString(_pthis->buffer + 12, usernamelen));
				}
				break;

			case post_userinfolist:
				{
					auto usernum = *((int*)(_pthis->buffer + 8));
					int pos = 12;
					for (int i = 0; i < usernum; i++)
					{
						auto usernamestrlen = *((int*)(_pthis->buffer + pos));
						pos += 4;
						theApp.user_dlg.addUser(CString(_pthis->buffer + pos, usernamestrlen));
						pos += usernamestrlen;
					}
				}
				break;

			case reusername_error:
				SendMessage(theApp.login_hwnd, WM_REUSERNAME, 0, 0);
				break;

			case otheruser_exit:
				{
					auto usernamelen = *((int*)(_pthis->buffer + 8));
					theApp.user_dlg.removeUser(CString(_pthis->buffer + 12, usernamelen));
				}
				break;

			default:
				break;
			}

			_pthis->bufferpos += netbufferlen;
		}
	}

	return 1;
}

void CNetThread::sendregisterinfo(CString username, CString key)
{
	char buff[1024];
	memset(buff, 0, 1024);

	int size = headersize + username.GetLength() + key.GetLength() + 8;

	*((int*)buff) = size;
	*((int*)(buff+4)) = request_register_event;
	*((int*)(buff + 8)) = username.GetLength();
	memcpy(buff + 12, username.GetString(), username.GetLength());
	*((int*)(buff + 12 + username.GetLength())) = key.GetLength();
	memcpy(buff + 16 + username.GetLength(), key.GetString(), key.GetLength());

	send(s, buff, size, 0);
}

void CNetThread::sendlogin(CString username, CString key)
{
	char buff[1024];
	memset(buff, 0, 1024);

	int size = headersize + username.GetLength() + key.GetLength() + 8;

	*((int*)buff) = size;
	*((int*)(buff + 4)) = request_login_event;
	*((int*)(buff + 8)) = username.GetLength();
	memcpy(buff + 12, username.GetString(), username.GetLength());
	*((int*)(buff + 12 + username.GetLength())) = key.GetLength();
	memcpy(buff + 16 + username.GetLength(), key.GetString(), key.GetLength());

	send(s, buff, size, 0);
}

void CNetThread::sendlogin2(CString username)
{
	char buff[1024];
	memset(buff, 0, 1024);

	int size = headersize + username.GetLength() + 4;

	*((int*)buff) = size;
	*((int*)(buff + 4)) = request_login2_event;
	*((int*)(buff + 8)) = username.GetLength();
	memcpy(buff + 12, username.GetString(), username.GetLength());

	send(s, buff, size, 0);
}

void CNetThread::sendexit()
{
	char buff[1024];
	memset(buff, 0, 1024);

	*((int*)buff) = headersize;
	*((int*)(buff + 4)) = exit_client;

	send(s, buff, headersize, 0);
}

void CNetThread::sendvideo(int width, int height, int bmpleng, void * bmp)
{
	int size = headersize + bmpleng + 12 + 4 + username.GetLength();
	char *buff = new char[size];

	*((int*)buff) = size;
	*((int*)(buff + 4)) = video_event;
	*((int*)(buff + 8)) = username.GetLength();
	memcpy(buff + 12, username.GetString(), username.GetLength());
	*((int*)(buff + 12 + username.GetLength())) = width;
	*((int*)(buff + 16 + username.GetLength())) = height;
	*((int*)(buff + 20 + username.GetLength())) = bmpleng;
	memcpy(buff + 24 + username.GetLength(), bmp, bmpleng);

	send(s, buff, size, 0);

	delete[] buff;
}