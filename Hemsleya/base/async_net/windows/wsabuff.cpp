/*
 * wsabuff.cpp
 *   Created on: 2013-10-2
 *       Author: qianqians
 * wsabuff
 */
#ifdef _WINDOWS

#include "wsabuff.h"

#include <Hemsleya/base/concurrent/abstract_factory/abstract_factory.h>

namespace Hemsleya { 
namespace async_net { 

abstract_factory::abstract_factory<WSABUF> _abstract_factory_WSABUF;

WSABUF * GetWSABUF(const uint32_t len){
	return _abstract_factory_WSABUF.create_product(len);
}

void ReleaseWSABUF(WSABUF *  _wsabuf, const uint32_t len){
	_abstract_factory_WSABUF.release_product(_wsabuf, len);
}

} //async_net
} //Hemsleya

#endif //_WINDOWS