/*
 * no_blocking_pool.h
 *   Created on: 2012-12-1
 *	     Author: qianqians
 * һ�������:����thread cache����ʵ�� 
 * ���ڶ�����ظ�ʹ�����߳��޹�,
 * �ٶ�cpu�߼�����ΪN��
 * thread cache����N��������listʵ�֣������̱߳��ش洢
 * һ���̷߳��ʶ���ز��������������try_lock�������ΪN��,��СΪ1��
 * ���Կ�����Ϊ����������write-free��.
 */
#ifndef _NO_BLOCKING_POOL_H
#define _NO_BLOCKING_POOL_H

#include <list>
#include <boost/thread/mutex.hpp>
#include <boost/atomic.hpp>

#ifdef _WIN32
#include <Windows.h>
#endif

namespace angelica {
namespace container {

template <typename T>
class no_blocking_pool{
	struct mirco_pool{
		std::list<typename T * > _pool; 	
		boost::mutex _mu;
	};

public:
	no_blocking_pool(){
#ifdef _WIN32
		SYSTEM_INFO info;
		GetSystemInfo(&info);
		_count = info.dwNumberOfProcessors;
#endif

		_size.store(0);
		_pool = new mirco_pool[_count];
	}

	~no_blocking_pool(){
		delete[] _pool;
	}

	T * pop(){
		T * ret = 0;

		while(_size.load() != 0){
			for(unsigned int i = 0; i < _count; i++){
				boost::mutex::scoped_lock lock(_pool[i]._mu, boost::try_to_lock);
				if (lock.owns_lock()){
					if (!_pool[i]._pool.empty()){
						ret = _pool[i]._pool.front();
						_pool[i]._pool.pop_front();
						_size--;

						return ret;
					}
				}
			}
		}

		return ret;
	}

	void put(T * ptr){
		unsigned int slide = 0;
		while(1){
			boost::mutex::scoped_lock lock(_pool[slide]._mu, boost::try_to_lock);
			if (lock.owns_lock()){
				_pool[slide]._pool.push_back(ptr);
				_size++; 
				break;
			}

			if(++slide == _count){
				slide = 0;
			}
		}
	}

	std::size_t size(){
		return _size.load();
	}

private:
	mirco_pool * _pool;
	unsigned int _count;

	boost::atomic_uint _size;

};

}// container
}// angelica

#endif //_NO_BLOCKING_POOL_H