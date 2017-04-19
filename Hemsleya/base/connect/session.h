/*
 * sessionimpl.h
 *   Created on: 2013-10-26
 *       Author: qianqians
 * sessionimpl
 */
#ifndef _SESSION_H
#define _SESSION_H

#include <Hemsleya/base/async_net/async_net.h>

#include <boost/signals2.hpp>
#include <boost/atomic.hpp>
#include <boost/unordered_map.hpp>
#include <boost/shared_ptr.hpp>
#include <boost/pool/pool_alloc.hpp>

#include "address.h"

namespace Hemsleya { 
namespace connect {

class connservice;

enum session_state{
	no_conn,
	tcp_conn,
	wait_udp_conn,
	wait_udp_conn_confirm,
	udp_conn,
};

enum cmd_serial{
	udp_info,
	try_udp_remote_ip,
	try_udp_link,
	try_udp_link_ack,
	try_udp_link_confirm,
	udp_safe_cmd,
	udp_safe_cmd_ack,
	udp_unsafe_cmd,
};

#pragma pack(push, 1)
struct package{
	uint32_t cmdid;
	uint32_t ackid;
	uint32_t len;
	char buf[1];
};
#define package_size sizeof(package) - sizeof(char)
#pragma pack(pop)

class sessionimpl{
private:
	typedef boost::pool_allocator<char> mempool;

public:
	//server begin
	sessionimpl(connect::connservice & _service, const async_net::TCP::socket _tcpsocket, const async_net::UDP::socket _udpsocket, uint32_t sessionid);
	//server end

	//client begin
	sessionimpl(connect::connservice & _service, const address & tcpaddr, const address & udpaddr);
	//client end

	~sessionimpl();

private:
	sessionimpl();

	//client begin
public:
	void connect(const address & remoteaddr);

	boost::signals2::signal<void() > sigConn;

private:
	void onTCPConnect();

	void onUDPConnect();

	void sendudpconnreq();

	void sendudpconnclientconfirm();
	//client end

	//server begin
	void sendudpinfo();

	void sendudpconnserverack();

	void sendudpconnserverconfirm();
	//server end

public:
	void unsafesend(char * buff, uint32_t len);

	boost::signals2::signal<void() > sigUnsafesend;

private:
	void onunsafesend();

public:
	void safesend(char * buff, uint32_t len);

	boost::signals2::signal<void() > sigSafesend;

private:
	void onsafesend();

public:
	boost::signals2::signal<void(char *, uint32_t) > sigData;

private:
	void onTCPData(char * buff, uint32_t len);

	void onUDPData(char * buff, uint32_t len, address _remoteudpaddr);

	//client begin
	void onUDPinfo(package * _package);

	void onUDPconnserverack(package * _package);

	void onUDPconnserverconfirm(package * _package);
	//client end

	//server begin
	void onUDPconnreq(package * _package, address remoteudpaddr);

	void onUDPconnclientconfirm(package * _package);
	//server end 

private:
	session_state _state;

	async_net::TCP::socket tcpsocket;
	async_net::UDP::socket udpsocket;

	boost::unordered_map<uint32_t, package * > mappackge;

	mempool _mempool;

	address remoteudpaddr, localtcpaddr, localudpaddr;

	connservice * pservice;

	boost::atomic_uint32_t ackid;
	uint32_t sessionid;

	friend class connservice;

};

class session{
public:
	session(const session & _session);

private:
	session(sessionimpl * _impl);

public:
	void connect(const address & remoteaddr);

	boost::signals2::signal<void() > * sigConn;

	void unsafesend(char * buff, uint32_t len);

	boost::signals2::signal<void() > * sigUnsafesend;

	void safesend(char * buff, uint32_t len);

	boost::signals2::signal<void() > * sigSafesend;

	boost::signals2::signal<void(char *, uint32_t) > * sigData;

private:
	boost::shared_ptr<sessionimpl> _impl;

	friend class connservice;

};

} //connect
} //Hemsleya


#endif //_SESSION_H
