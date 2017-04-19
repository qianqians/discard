/*
 * overlappedsend.h
 *   Created on: 2013-10-4
 *       Author: qianqians
 * overlappedsend
 */
#ifndef _OVERLAPPEDSEND_H
#define _OVERLAPPEDSEND_H

#ifdef _WINDOWS

#include "winhdef.h"
#include "overlapped.h"

namespace Hemsleya { 
namespace async_net { 

struct buffstruct;

struct overlappedsend : public overlapped{
	std::vector<buffstruct> vectbuff;
	boost::function<void(void) > fncallback;
};

} //async_net
} //Hemsleya

#endif //_WINDOWS

#endif //_OVERLAPPEDSEND_H
