/*
 * sessionimpl.cpp
 *   Created on: 2013-10-26
 *       Author: qianqians
 * connect
 */
#include "session.h"
#include "connservice.h"

namespace Hemsleya { 
namespace connect {

enum udp_state{
	try_link1,
	try_link2,
};

//sessionimpl
sessionimpl::sessionimpl(connect::connservice & _service, const async_net::TCP::socket _tcpsocket, const async_net::UDP::socket _udpsocket, uint32_t _sessionid) :
pservice(&_service), tcpsocket(_service._service), udpsocket(_service._service), ackid(0), sessionid(_sessionid){
	_state = wait_udp_conn;

	//send udp info
	//tcpsocket
}

sessionimpl::sessionimpl(connect::connservice & _service, const address & tcpaddr, const address & udpaddr) :
pservice(&_service), tcpsocket(_service._service), udpsocket(_service._service), ackid(0){
	_state = no_conn;

	localtcpaddr = tcpaddr;
	localudpaddr = udpaddr;

	tcpsocket.bind(tcpaddr);
	udpsocket.bind(udpaddr);

	tcpsocket._connectsignal->connect(boost::bind(&sessionimpl::onTCPConnect, this));
}

sessionimpl::~sessionimpl(){
}

void sessionimpl::connect(const address & remoteaddr){
	tcpsocket.async_connect(remoteaddr);
}

void sessionimpl::onTCPConnect(){
	_state = tcp_conn;

	sigConn();

	tcpsocket._recvsignal->connect(boost::bind(&sessionimpl::onTCPData, this, _1, _2));
}

void sessionimpl::safesend(char * buff, uint32_t len){
	if (_state == tcp_conn){
		tcpsocket.async_send(buff, len);
	} else if (_state == udp_conn){
		char * tmp = _mempool.allocate(sizeof(uint32_t) + package_size + len);
		uint32_t * _sessionid = (uint32_t *)tmp;
		*_sessionid = sessionid;

		package * _package = (package*)++_sessionid;
		_package->ackid = ackid++;
		_package->cmdid = udp_safe_cmd;
		_package->len = len;
		memcpy(_package->buf, buff, len);
		
		udpsocket.async_sendto(remoteudpaddr.getsockaddr(), buff, len);
	}
}

void sessionimpl::onsafesend(){
	sigSafesend();
}

void sessionimpl::unsafesend(char * buff, uint32_t len){
	if (_state == tcp_conn){
		tcpsocket.async_send(buff, len);
	} else if (_state == udp_conn){
		char * tmp = _mempool.allocate(sizeof(uint32_t)+package_size + len);
		uint32_t * _sessionid = (uint32_t *)tmp;
		*_sessionid = sessionid;

		package * _package = (package*)++_sessionid;
		_package->ackid = ackid++;
		_package->cmdid = udp_unsafe_cmd;
		_package->len = len;
		memcpy(_package->buf, buff, len);

		udpsocket.async_sendto(remoteudpaddr.getsockaddr(), buff, len);
	}
}

void sessionimpl::onunsafesend(){
	sigUnsafesend();
}

void sessionimpl::onTCPData(char * buff, uint32_t len){
	package * _package = (package*)buff;

	if (len >= _package->len){
		switch (_package->ackid){
		case udp_info:
			onUDPinfo(_package);
			break;

		default:
			break;
		}
	}
}

//client begin
void sessionimpl::onUDPinfo(package * _package){
	uint32_t * _sessionid = (uint32_t*)_package->buf;
	sessionid = *_sessionid;
	uint16_t * port = (uint16_t*)(_sessionid+1);
	char * ip = (char*)(port+1);
	remoteudpaddr = address(ip, *port);

	sendudpconnreq();

	_state = wait_udp_conn;
}

void sessionimpl::onUDPconnserverack(package * _package){
	sendudpconnclientconfirm();

	_state = wait_udp_conn_confirm;
}

void sessionimpl::onUDPconnserverconfirm(package * _package){
	_state = udp_conn;
}
//client end

//server begin
void sessionimpl::onUDPconnreq(package * _package, address _remoteudpaddr){
	remoteudpaddr = _remoteudpaddr;

	sendudpconnserverconfirm();

	_state = wait_udp_conn_confirm;
}

void sessionimpl::onUDPconnclientconfirm(package * _package){
	_state = udp_conn;
}
//server end 

//client begin
void sessionimpl::sendudpconnreq(){
	uint32_t len = sizeof(uint32_t)+package_size;
	char * tmp = _mempool.allocate(sizeof(uint32_t)+package_size);
	uint32_t * _sessionid = (uint32_t *)tmp;
	*_sessionid = sessionid;

	package * _package = (package*)++_sessionid;

	_package->ackid = ackid++;
	_package->cmdid = try_udp_remote_ip;
	_package->len = 0;

	udpsocket.async_sendto(remoteudpaddr.getsockaddr(), tmp, len);
}

void sessionimpl::sendudpconnclientconfirm(){
	uint32_t len = sizeof(uint32_t)+package_size;
	char * tmp = _mempool.allocate(sizeof(uint32_t)+package_size);
	uint32_t * _sessionid = (uint32_t *)tmp;
	*_sessionid = sessionid;

	package * _package = (package*)++_sessionid;

	_package->ackid = ackid++;
	_package->cmdid = try_udp_link_ack;
	_package->len = 0;

	udpsocket.async_sendto(remoteudpaddr.getsockaddr(), tmp, len);
}
//client end

//server begin
void sessionimpl::sendudpinfo(){
	uint32_t len = sizeof(uint32_t)+localudpaddr.addrlen();
	uint32_t package_len = package_size + len;

	char * tmp = _mempool.allocate(package_len);
	package * _package = (package*)tmp;

	_package->ackid = 0;
	_package->cmdid = udp_info;
	_package->len = len;

	uint32_t * _sessionid = (uint32_t*)tmp;
	*_sessionid = sessionid;
	char * ipbuf = (char*)++_sessionid;
	memcpy(ipbuf, localudpaddr.getsockaddr(), localudpaddr.addrlen());

	tcpsocket.async_send(tmp, package_len);

	_mempool.deallocate(tmp, package_len);
}

void sessionimpl::sendudpconnserverack(){
	uint32_t len = sizeof(uint32_t)+package_size;
	char * tmp = _mempool.allocate(sizeof(uint32_t) + package_size);
	uint32_t * _sessionid = (uint32_t *)tmp;
	*_sessionid = sessionid;

	package * _package = (package*)++_sessionid;

	_package->ackid = ackid++;
	_package->cmdid = try_udp_link;
	_package->len = 0;

	udpsocket.async_sendto(remoteudpaddr.getsockaddr(), tmp, len);
}

void sessionimpl::sendudpconnserverconfirm(){
	uint32_t len = sizeof(uint32_t)+package_size;
	char * tmp = _mempool.allocate(sizeof(uint32_t)+package_size);
	uint32_t * _sessionid = (uint32_t *)tmp;
	*_sessionid = sessionid;

	package * _package = (package*)++_sessionid;

	_package->ackid = ackid++;
	_package->cmdid = try_udp_link_confirm;
	_package->len = 0;

	udpsocket.async_sendto(remoteudpaddr.getsockaddr(), tmp, len);
}
//server end
//sessionimpl

//session
session::session(sessionimpl * impl) :
_impl(impl)
{
	sigConn = &impl->sigConn;
	sigUnsafesend = &impl->sigUnsafesend;
	sigSafesend = &impl->sigSafesend;
	sigData = &impl->sigData;
}

session::session(const session & _session) : 
_impl(_session._impl)
{
	sigConn = &_impl->sigConn;
	sigUnsafesend = &_impl->sigUnsafesend;
	sigSafesend = &_impl->sigSafesend;
	sigData = &_impl->sigData;
}

} //connect
} //Hemsleya
