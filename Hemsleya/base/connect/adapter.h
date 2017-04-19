/*
 * adapter.h
 *   Created on: 2012-10-16
 *       Author: qianqians
 * adapter
 */
#ifndef _ADAPTER_H
#define _ADAPTER_H

#include <boost/signals2.hpp>

#include <Hemsleya/base/async_net/async_net.h>
#include <Hemsleya/base/concurrent/container/small_hash_map.h>

#include "address.h"
#include "session.h"

namespace Hemsleya { 
namespace connect {

class connservice;

enum accept_state{
	begin_accept,
	end_accept,
};

class adapter{
public:
	adapter(connservice & _service, const address & tcpaddr, const address & udpaddr);
	~adapter();

	void accept(accept_state _state);

	boost::signals2::signal<void(session) > sigAccept;

private:
	void acceptcallback(async_net::TCP::socket & sa);

private:
	accept_state _accept_state;

	async_net::TCP::Adapter _adapter;
	async_net::UDP::socket _socket;

	boost::atomic_uint32_t iSessionID;

	container::small_hash_map<uint32_t, sessionimpl*> map_session;

};  

} //connect
} //Hemsleya


#endif //_ADAPTER_H
