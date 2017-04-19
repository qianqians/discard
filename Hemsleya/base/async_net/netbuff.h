/*
 * netbuff.h
 *   Created on: 2012-10-16
 *       Author: qianqians
 * readbuff
 */
#ifndef _NETBUFF_H
#define _NETBUFF_H

#ifdef _WINDOWS
#include "windows/winhdef.h"
#elif __linux__ 
//class socket_base_linux;
#endif

#include <boost/cstdint.hpp>
#include <boost/pool/pool_alloc.hpp>

namespace Hemsleya { 
namespace async_net { 

class netbuff{
public:
	netbuff(uint32_t bufflen){
		max = bufflen;
		buff = buffpool.allocate(bufflen);
	}

	~netbuff(){
		buffpool.deallocate(buff, max);
	}

	char * buff;
	uint32_t len, max;

	static boost::pool_allocator<char> buffpool;

};

} //async_net
} //Hemsleya

#endif //_NETBUFF_H
