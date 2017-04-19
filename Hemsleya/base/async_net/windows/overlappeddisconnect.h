/*
 * overlappeddisconnect.h
 *   Created on: 2013-10-23
 *       Author: qianqians
 * overlappeddisconnect
 */
#ifndef _OVERLAPPEDDISCONNECT_H
#define _OVERLAPPEDDISCONNECT_H

#ifdef _WINDOWS

#include "winhdef.h"
#include "overlapped.h"

namespace Hemsleya { 
namespace async_net { 

struct overlappeddisconnect : public overlapped{
	SOCKET s;
};

} //async_net
} //Hemsleya

#endif //_WINDOWS

#endif //_OVERLAPPEDDISCONNECT_H
