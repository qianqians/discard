/*
 * async_service.cpp
 *   Created on: 2012-11-14
 *       Author: qianqians
 * async_service
 */
#include "async_service.h"
#include "socket_pool.h"
#include "buff_pool.h"
#include "read_buff_pool.h"
#include "write_buff_pool.h"

namespace Hemsleya { 
namespace async_net { 

void async_service::Init(){
	detail::SocketPool::Init();
	detail::BuffPool::Init(detail::page_size);
	detail::ReadBuffPool::Init();
	detail::WriteBuffPool::Init();
}

void async_service::run(){
	thread_count++;

	try{
		network();
	}catch(...){
		//error log	
	}
	
	thread_count--;
}

} //async_net
} //Hemsleya
