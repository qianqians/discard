/*
 * wsabuff.h
 *   Created on: 2013-10-2
 *       Author: qianqians
 * wsabuff
 */
#ifndef _WSABUFF_H
#define _WSABUFF_H

#ifdef _WINDOWS

#include "winhdef.h"
#include <boost/cstdint.hpp>

namespace Hemsleya { 
namespace async_net { 

WSABUF * GetWSABUF(const uint32_t len);
void ReleaseWSABUF(WSABUF *  _wsabuf, const uint32_t len);

} //async_net
} //Hemsleya

#endif //_WINDOWS

#endif //_WSABUFF_H
