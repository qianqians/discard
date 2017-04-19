/*
 * write_buff_pool.h
 * Created on: 2013-2-24
 *	   Author: qianqians
 * write_buff_pool ½Ó¿Ú
 */
#ifndef _WRITE_BUFF_POOL_H
#define _WRITE_BUFF_POOL_H

#include "sock_buff.h"
#include <Hemsleya/base/concurrent/abstract_factory/abstract_factory.h>

namespace Hemsleya {
namespace async_net {

namespace detail {

class WriteBuffPool{
public:
	static void Init(){}

	static write_buff * get(){
		return _write_buff_pool.create_product();
	}

	static void release(write_buff * _write_buff){
		_write_buff_pool.release_product(_write_buff, 1);
	}

private:
	static Hemsleya::abstract_factory::abstract_factory<write_buff > _write_buff_pool;

};

} //detail

} //async_net
} //Hemsleya

#endif // _write_buff_pool_h