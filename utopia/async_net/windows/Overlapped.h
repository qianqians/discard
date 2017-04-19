/*
 * Overlapped.h
 *  Created on: 2012-10-16
 *		Author: qianqians
 * 扩展Overlapped结构
 * 定义Overlapped对象池
 */
#ifndef _OVERLAPPED_H
#define _OVERLAPPED_H

#ifdef _WINDOWS

#include "winhdef.h"
#include "../error_code.h"

#include <Hemsleya/base/concurrent/abstract_factory/abstract_factory.h>

#include <boost/function.hpp>

namespace Hemsleya {
namespace async_net {

class socket_base;

namespace windows {

//扩展Overlapped结构
struct OverlappedEX {
	int type;
	OVERLAPPED overlap;
};

struct OverlappedEX_Accept{
	OverlappedEX overlapex;
	socket_base * socket_;
};

struct OverlappedEX_close{
	OverlappedEX overlapex;
	SOCKET fd;
};

namespace detail {

template<typename OverlappedEX_Type>
class OverlappedEXPool{
public:
	static void Init(){
	}

	static OverlappedEX_Type * get(){
		return _OverlappedEX_pool.create_product();
	}

	static void release(OverlappedEX_Type * _OverlappedEX){
		_OverlappedEX_pool.release_product(_OverlappedEX, 1);
	}

private:
	static Hemsleya::abstract_factory::abstract_factory<typename OverlappedEX_Type > _OverlappedEX_pool;

};

}// detail

} //windows
} //async_net
} //Hemsleya

#endif //_WINDOWS
#endif //_OVERLAPPED_H