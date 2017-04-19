#pragma once

#include <WinSock2.h>

#define WM_REMOVE_USER (WM_USER+95)
#define WM_REUSERNAME (WM_USER+96)
#define WM_ADD_USER (WM_USER+97)
#define WM_SOCKET_MSG_LOGIN2 (WM_USER+98)
#define WM_SOCKET_MSG_REGISTER (WM_USER+99)
#define WM_SOCKET_MSG_LOGIN (WM_USER+100)
#define WM_SOCKET_MSG_CHAT (WM_USER+101)
#define WM_SOCKET_MSG_GROUP_CHAT (WM_USER+102)
#define WM_SOCKET_MSG_BEGIN_AUDIO (WM_USER+103)
#define WM_SOCKET_MSG_AUDIO (WM_USER+104)
#define WM_SOCKET_MSG_END_AUDIO (WM_USER+105)
#define WM_SOCKET_MSG_BEGIN_GROUP_AUDIO (WM_USER+106)
#define WM_SOCKET_MSG_GROUP_AUDIO (WM_USER+107)
#define WM_SOCKET_MSG_END_GROUP_AUDIO (WM_USER+108)
#define WM_SOCKET_MSG_BEGIN_VIDEO (WM_USER+109)
#define WM_SOCKET_MSG_VIDEO (WM_USER+110)
#define WM_SOCKET_MSG_END_VIDEO (WM_USER+111)
#define WM_SOCKET_MSG_BEGIN_GROUP_VIDEO (WM_USER+112)
#define WM_SOCKET_MSG_GROUP_VIDEO (WM_USER+113)
#define WM_SOCKET_MSG_END_GROUP_VIDEO (WM_USER+114)

class CNetThread
{
public:
	CNetThread();
	~CNetThread();

	bool InitNet();

public:
	void sendregisterinfo(CString username, CString key);
	void sendlogin(CString username, CString key);
	void sendlogin2(CString username);
	void sendexit();
	void sendvideo(int width, int height, int bmpleng, void * bmp);

private:
	static UINT run(LPVOID p);

private:
	char buffer[16*1024*1024];
	int bufferpos;

	SOCKET s;

public:
	CString username;

};

