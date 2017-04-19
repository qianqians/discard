/*
 * writebuff.h
 *   Created on: 2013-10-2
 *       Author: qianqians
 * async_service
 */
#ifndef _WRITEBUFF_H
#define _WRITEBUFF_H

#include "buffstruct.h"

#include <boost/thread.hpp>

#include <Hemsleya/base/concurrent/abstract_factory/abstract_factory.h>

namespace Hemsleya { 
namespace async_net { 

class writebuff{
public:
	writebuff();
	~writebuff();

	int write(char * buf, uint32_t len, boost::function<void(const std::vector<buffstruct> * const buff)> fn);

	void freebuff(char * buff, uint32_t buflen);

private:
	struct writebuffstruct{
		writebuffstruct(){
			buff = 0;
			len = 0;
			max = 0;
		}

		char * buff;
		uint32_t len;
		uint32_t max;
	};

	struct buflist{
		std::vector<writebuffstruct> * buf;
		buflist * next;
	};

	struct _buff{
		char buff[65536];
	};

private:
	boost::shared_mutex _mutex;
	boost::thread_specific_ptr<std::vector<writebuffstruct> > tss_buff;
	boost::atomic<buflist*> _list;

	abstract_factory::abstract_factory<_buff > _abstract_factory_buff;
	abstract_factory::abstract_factory<std::vector<writebuffstruct> > _abstract_factory_vector_buffstruct;

};  

} //async_net
} //Hemsleya


#endif //_WRITEBUFF_H
