/*
 * overlappedrecv.h
 *   Created on: 2013-10-4
 *       Author: qianqians
 * overlappedrecvfrom
 */
#ifndef _OVERLAPPEDRECV_H
#define _OVERLAPPEDRECV_H

#ifdef _WINDOWS

#include "winhdef.h"
#include "overlapped.h"

namespace Hemsleya { 
namespace async_net { 

struct overlappedrecv : public overlapped{
	boost::function<void(uint32_t recvlen) > fncallback;
};

} //async_net
} //Hemsleya

#endif //_WINDOWS

#endif //_OVERLAPPEDRECV_H
