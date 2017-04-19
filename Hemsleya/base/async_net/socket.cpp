/*
 * socket.cpp
 *   Created on: 2013-10-11
 *       Author: qianqians
 * async_service
 */
#include "socket.h"

#include "async_service.h"
#include "socket_tcp_impl.h"
#include "socket_udp_impl.h"

namespace Hemsleya { 
namespace async_net { 
	
namespace TCP{

socket::socket(async_service & _service) : 
	sptr(create_socket_tcp_impl(_service), 
	delete_socket_tcp_impl),
	_sendsignal(&sptr->_sendsignal),
	_recvsignal(&sptr->_recvsignal),
	_connectsignal(&sptr->_connectsignal)
{
}

socket::~socket(){
}

void socket::bind(const address & addr){
	sptr->bind(addr);
}

void socket::async_send(char * buf, uint32_t len){
	sptr->async_send(buf, len);
}

void socket::async_recv(recv_state _state){
	sptr->async_recv(_state);
}

void socket::async_connect(const address & addr){
	sptr->async_connect(addr);
}
	
void socket::shutdown(){
	sptr->shutdown();
}

void socket::cancelio(){
	sptr->cancelio();
}

} // TCP

namespace UDP{

socket::socket(async_service & _service) :
	sptr(create_socket_udp_impl(_service),
	delete_socket_udp_impl),
	_sendtosignal(&sptr->_sendtosignal),
	_recvfromsignal(&sptr->_recvfromsignal)
{
}

socket::~socket(){
}

void socket::bind(const address & addr){
	sptr->bind(addr);
}

void socket::shutdown(){
	sptr->shutdown();
}

void socket::cancelio(){
	sptr->cancelio();
}

} //UDP

} //async_net
} //Hemsleya
