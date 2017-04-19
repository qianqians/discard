/*
 * buff_pool.h
 * Created on: 2013-2-24
 *	   Author: qianqians
 * buff_pool ½Ó¿Ú
 */
#ifndef _BUFF_POOL_H
#define _BUFF_POOL_H

#include <Hemsleya/base/concurrent/abstract_factory/abstract_factory.h>

namespace Hemsleya {
namespace async_net {

namespace detail {

class BuffPool{
public:
	static void Init(size_t _page_size){
		_size = _page_size;
	}

	static char * get(size_t _size_){
		char * buff = _buff_pool.create_product(_size_);

#ifdef _DEBUG
		memset(buff, 0, _size);
#endif //_DEBUG

		return buff;
	}

	static void release(char * buff, size_t _size_){
		_buff_pool.release_product(buff, _size_);
	}

private:
	static size_t _size;
	static Hemsleya::abstract_factory::abstract_factory<char> _buff_pool;

};

extern unsigned int page_size;

} //detail

} //async_net
} //Hemsleya

#endif //_BUFF_POOL_H
