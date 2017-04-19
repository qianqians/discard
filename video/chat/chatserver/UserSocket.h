#pragma once

#include <string>

class CUserSocket
{
public:
	CUserSocket();
	~CUserSocket();

private:
	void process_recv(int bufferlen);
	void postrecv();


private:
	SOCKET s;
	
	char recvbuff[16*1024*1024];
	int recvbuffpos;
	WSABUF recvwsabuf;

	std::string strusername;

	friend class CNetThreadSvr;

};

