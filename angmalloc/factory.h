/*
 * factory.h
 *
 *  Created on: 2014-10-21
 *      Author: qianqians
 */
#ifndef _factory_h
#define _factory_h

#include <utility>
#include <memory>

#include "angmalloc.h"

namespace angelica{
namespace pool{

class factory{
public:
	/*
	 * create a objects type is T
	 */
	template<class T, typename ...Tlist>
	static std::shared_ptr<T> create(Tlist&& ... var){
		T * p = (T*)angmalloc(sizeof(T));
		new(p) T(std::forward<Tlist>(var)...);

		return std::shared_ptr<T>(p, factory::release<T>);
	}

private:
	/*
	 * release objects type is T
	 */
	template<class T>
	static void release(T * p){
		p->~T();
		angfree(p);
	}

};

} /* namespace pool */
} /* namespace angelica */

#endif //_factory_h