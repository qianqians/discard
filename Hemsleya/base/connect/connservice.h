/*
 * connservice.h
 *   Created on: 2013-10-26
 *       Author: qianqians
 * connservice
 */
#ifndef _CONNSERVICE_H
#define _CONNSERVICE_H

#include <Hemsleya/base/async_net/async_net.h>

namespace Hemsleya { 
namespace connect {

class session;

class connservice{
public:
	connservice();
	~connservice();

	void run();

	void addconn(session & _session);

private:
	async_net::async_service _service;

	friend class sessionimpl;
	friend class adapter;

};  

} //connect
} //Hemsleya


#endif //_CONNSERVICE_H
