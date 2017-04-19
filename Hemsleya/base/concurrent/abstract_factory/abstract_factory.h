/*
 * abstract_factory.h
 *   Created on: 2013-8-25
 *	     Author: qianqians
 * abstract_factory
 */
#ifndef _abstract_factory_h
#define _abstract_factory_h

#include <vector>

#include <boost/atomic.hpp>
#include <boost/pool/pool_alloc.hpp>
#include <boost/shared_ptr.hpp>

#include <Hemsleya/base/tools/tools.h>

namespace Hemsleya {
namespace abstract_factory {

template <typename T, typename _Allocator_ = std::allocator<T> >
class abstract_factory{
private:
	struct mirco_pool{
		mirco_pool() : _pool(0){}
		~mirco_pool() {}

		void * _pool; 	
		boost::atomic_flag _mu;
	};

	typedef typename _Allocator_::template rebind<mirco_pool>::other _Allocator_mirco_pool_;

public:
	abstract_factory(){
		_pool_count = processors_count();
		_vector_mirco_pool = _Allocator_mirco_pool.allocate(_pool_count);
		for(uint32_t i = 0; i < _pool_count; i++){
			new(&_vector_mirco_pool[i]) mirco_pool();
		}

		min_count = (8+sizeof(T)-1)/sizeof(T);
	}

	~abstract_factory(){
		for(uint32_t i = 0; i != _pool_count; i++){
			void * _pool = _vector_mirco_pool[i]._pool; 
			while (_pool != 0){
				void * tmp = _pool;
				_pool = *((void**)_pool);
				_Allocator.deallocate((T*)tmp, min_count);
			}
		}
		_Allocator_mirco_pool.deallocate(_vector_mirco_pool, _pool_count);
	}

	template <typename ...Tlist>
	T * create_product(size_t count, Tlist&& ... var){
		T * pT = _alloc(count);
		
		while(count > 0){
			new(&pT[--count]) T(std::forward<Tlist>(var)...);
		}
		
		return pT;
	}

	template <typename ...Tlist>
	T * create_product(Tlist&& ... var){
		T * pT = _alloc(1);
		
		if (pT != 0){
			new(pT) T(std::forward<Tlist>(var)...);
		}
		
		return pT;
	}

	T * create_product(size_t count = 1){
		T * pT = _alloc(count);
		
		while(count > 0){
			new(&pT[--count]) T;
		}
		
		return pT;
	}

	void release_product(T * pT, size_t count = 1){
		while(count > 0){
			pT[--count].~T();
		}

		_free(pT, count);
	}

private:
	T * _alloc(size_t count){
		if (count <= min_count){
			for(uint32_t i = 0; i < _pool_count; i++){
				if (!_vector_mirco_pool[i]._mu.test_and_set()){
					if (_vector_mirco_pool[i]._pool != 0){
						T * tmp = (T*)_vector_mirco_pool[i]._pool;
						_vector_mirco_pool[i]._pool = *((void**)_vector_mirco_pool[i]._pool);
						_vector_mirco_pool[i]._mu.clear();

						return tmp;
					}
					_vector_mirco_pool[i]._mu.clear();
				}
			}
		}
		
		return _Allocator.allocate(count < min_count ? min_count : count);
	}

	void _free(T * mem, size_t count){
		if (count <= min_count){
			uint32_t i = 0;
			while(1){
				if (!_vector_mirco_pool[i]._mu.test_and_set()){
					*((void**)mem) = _vector_mirco_pool[i]._pool;
					_vector_mirco_pool[i]._pool = (void*)mem;
					_vector_mirco_pool[i]._mu.clear();

					break;
				}

				if (++i == _pool_count){
					i = 0;
				}
			}
		}else{
			_Allocator.deallocate(mem, count);
		}
	}

private:
	_Allocator_ _Allocator;
	_Allocator_mirco_pool_ _Allocator_mirco_pool;

	uint32_t _pool_count;
	mirco_pool * _vector_mirco_pool;

	size_t min_count;

};

} // abstract_factory
} // Hemsleya

#endif //_abstract_factory_h