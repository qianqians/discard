/*
 * readbuff.h
 *   Created on: 2012-10-16
 *       Author: qianqians
 * readbuff
 */
#ifndef _READBUFF_H
#define _READBUFF_H

#ifdef _WINDOWS
#include "windows/winhdef.h"
#elif __linux__ 
//class socket_base_linux;
#endif

#include <boost/cstdint.hpp>

namespace Hemsleya { 
namespace async_net { 

class readbuf{
public:
	readbuf(){
		max = 16384;
		len = 0;
	}

	~readbuf();

	char buff[16384];
	uint32_t len, max;

};

} //async_net
} //Hemsleya

#endif //_READBUFF_H
