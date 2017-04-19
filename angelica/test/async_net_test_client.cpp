#include <angelica/async_net/async_net.h>
#include <iostream>
#include <fstream>
#include "Iphlpapi.h"

#include "msgh.h"
	
#pragma comment(lib, "async_net.lib")
#pragma comment(lib, "Iphlpapi.lib")
	
typedef angelica::async_net::sock_addr sock_addr;
typedef angelica::async_net::socket Socket;
	
char addr[32];
	
class session{
public:
	session(angelica::async_net::async_service & service) : s(service) {}

	~session(){}

	void start(){
		PIP_ADAPTER_ADDRESSES pAddresses;
		pAddresses = (IP_ADAPTER_ADDRESSES*) malloc(sizeof(IP_ADAPTER_ADDRESSES));
		ULONG outBufLen = 0;
		DWORD dwRetVal = 0;

		// Make an initial call to GetAdaptersAddresses to get the 
		// size needed into the outBufLen variable
		if (GetAdaptersAddresses(AF_INET, 
								 0, 
								 NULL, 
								 pAddresses, 
								 &outBufLen) == ERROR_BUFFER_OVERFLOW) {
			free(pAddresses);
			pAddresses = (IP_ADAPTER_ADDRESSES*) malloc(outBufLen);
		}

		// Make a second call to GetAdapters Addresses to get the
		// actual data we want
		if ((dwRetVal = GetAdaptersAddresses(AF_INET, 
											 0, 
											 NULL, 
											 pAddresses, 
											 &outBufLen)) == NO_ERROR) {
		}

		SOCKET_ADDRESS Address = pAddresses->FirstUnicastAddress->Address;
		
		if(s.bind(sock_addr(addr, 0)) == 0){
			s.async_connect(sock_addr(addr, 3311), 
				boost::bind(&session::onConnect, this, _1));
		}
	}

private:
	void onConnect(int err){
		if(err){
			std::cout << "error code: " << err << std::endl;
		}else{
			s.async_recv(boost::bind(&session::onRecv, this, _1, _2, _3), true);
		}
	}

	void onRecv(char * buff, unsigned int lenbuff, int err){
		if(err){
			std::cout << "error code: " << err << std::endl;
		}else{
			s.async_send(buff, lenbuff, boost::bind(&session::onSend, this, _1, _2, _3));
		}
	}

	void onSend(char * buff, unsigned int lenbuff, int err){
		if(err){
			std::cout << "error code: " << err << std::endl; 
		}
	}

private:
	Socket s;

};	
		
int main(){
	session * s[5000];

	angelica::async_net::async_service service;
	service.start();

	memset(addr, 0, 32);
	std::ifstream fin("client.txt");
	fin.getline(addr, 33);
	std::cout << addr << std::endl;

	for(int i = 0; i < 3000; i++){
		s[i] = new session(service);
		s[i]->start();
	}

	std::cout << "angelica async_net ���ܲ��� client" << std::endl;
	std::cout << "����q �˳�" << std::endl;
	
	char in;
	std::cin >> in;
	while(1){
		if (in == 'q')
			break;
	}

	service.stop();

	for(int i = 0; i < 3000; i++){
		delete s[i];
	}

	return 1;
}