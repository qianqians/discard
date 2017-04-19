#pragma once

#include <WinSock2.h>
#include <Mswsock.h>
#include <unordered_map>

#pragma comment(lib, "Ws2_32.lib")
#pragma comment(lib, "Mswsock.lib")

#include "UserSocket.h"

//protocl
//header
//int buffersize
//int cmdid 
#define headersize 8
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

#define error_reusername 110

#define otheruser_exit 111

#define video_event 112

struct overlappedex : public OVERLAPPED
{
	int event_type;
	/*
	 1 accept
	 2 recv
	 3 send
	 */

	CUserSocket * user;

	WSABUF * sendbuff;
};

class CNetThreadSvr
{
public:
	CNetThreadSvr();
	~CNetThreadSvr();

public:
	bool InitNet();

	static UINT run(LPVOID p);

	void onUserExit(std::string username);

private:
	HANDLE iocp_handle;

	SOCKET accept_socket;
	char accept_buff[1024];

public:
	std::unordered_map<std::string, CUserSocket *> usermap;

};

