/*
 * read_buff_pool.h
 * Created on: 2013-2-24
 *	   Author: qianqians
 * read_buff_pool ½Ó¿Ú
 */
#ifndef _READ_BUFF_POOL_H
#define _READ_BUFF_POOL_H

#include "sock_buff.h"

#include <Hemsleya/base/concurrent/abstract_factory/abstract_factory.h>

namespace Hemsleya {
namespace async_net {

namespace detail {

class ReadBuffPool{
public:
	static void Init(){}

	static read_buff * get(){
		return _read_buff_pool.create_product();
	}

	static void release(read_buff * _read_buff){
		_read_buff_pool.release_product(_read_buff, 1);
	}

private:
	static Hemsleya::abstract_factory::abstract_factory<read_buff> _read_buff_pool;

};

} //detail

} //async_net
} //Hemsleya

#endif //_READ_BUFF_POOL_H