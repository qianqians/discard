/*
 * overlappedaccept.h
 *   Created on: 2013-10-4
 *       Author: qianqians
 * overlappedaccept
 */
#ifndef _OVERLAPPEDACCEPT_H
#define _OVERLAPPEDACCEPT_H

#ifdef _WINDOWS

#include "winhdef.h"
#include "overlapped.h"

namespace Hemsleya { 
namespace async_net { 

struct overlappedaccept : public overlapped{
	boost::function<void(void) > fncallback;
};

} //async_net
} //Hemsleya

#endif //_WINDOWS

#endif //_OVERLAPPEDACCEPT_H
