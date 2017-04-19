/*
 * wait_any.h
 *  Created on: 2013-5-5
 *      Author: qianqians
 * wait_any
 */
#ifndef _WAIT_ANY_H
#define _WAIT_ANY_H

#include <ctime>
#include <string>

#include <boost/function.hpp>

#include <Hemsleya/base/concurrent/container/msque.h>
#include <Hemsleya/base/timer/clock.h>

namespace Hemsleya{
namespace scheduling{

extern uint64_t MaximumOuttime;

class signal{
public:
	signal(){}
	~signal(){}

	bool try_wait_any(){
		uint32_t _sig_ = _sig.load();
		while (_sig_ > 0){
			if (_sig.compare_exchange_weak(_sig_, _sig_-1)){
				return true;
			}
		}

		return false;
	}

	bool wait_any(boost::function<void() > DoWaitHandle, clock_t timeout = MaximumOuttime){
		uint64_t begin = timer::clock();

		while(1){
			DoWaitHandle();

			uint32_t _sig_ = _sig.load();
			while (_sig_ > 0){
				if (_sig.compare_exchange_weak(_sig_, _sig_-1)){
					return true;
				}
			}
		
			if ((begin + timer::clock()) < timeout){
				return false;
			}
		}

		return true;
	}

	void post(){
		_sig++;
	}
	
private:
	boost::atomic_uint32_t _sig;

};

namespace template_version{
template <typename T, class _Alloc = std::allocator<typename T> >
class signal{
public:
	signal(){}
	~signal(){}

	bool try_wait_any(T & data){
		_event _event_;

		if (_event_que.pop(_event_)){
			data = _event_.data;
			return true;
		}

		return false;
	}

	bool wait_any(T & data, boost::function<void() > DoWaitHandle, uint64_t timeout = MaximumOuttime){
		uint64_t begin = timer::clock();
		_event _event_;

		while(1){
			DoWaitHandle();

			if (_event_que.pop(_event_)){
				data = _event_.data;
				return true;
			}
		
			if ((begin + timer::clock()) < timeout){
				return false;
			}
		}

		return true;
	}

	void post(const T & data){
		_event_que.push(_event(data));
	}

private:
	struct _event{
		_event(T _data) : data(_data){}
		_event(){}

		T data;
	};

	typedef typename _Alloc::template rebind<_event>::other _Alloc_event;

	Hemsleya::container::msque<_event, _Alloc_event> _event_que;

};

}//template_version

}// scheduling
}// Hemsleya

#endif //_WAIT_ANY_H