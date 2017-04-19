/*
 * overlappedconnect.h
 *   Created on: 2013-10-4
 *       Author: qianqians
 * overlappedconnect
 */
#ifndef _OVERLAPPEDCONNECT_H
#define _OVERLAPPEDCONNECT_H

#ifdef _WINDOWS

#include "winhdef.h"
#include "overlapped.h"

namespace Hemsleya { 
namespace async_net { 

struct overlappedconnect : public overlapped{
	boost::function<void(void) > fncallback;
};

} //async_net
} //Hemsleya

#endif //_WINDOWS

#endif //_OVERLAPPEDCONNECT_H
