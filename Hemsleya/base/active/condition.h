/*
 * condition.h
 *
 *  Created on: 2013-9-28
 *      Author: qianqians
 */

#ifndef CONDITION_H_
#define CONDITION_H_

#include <boost/atomic.hpp>

#include <Hemsleya/base/concurrent/container/msque.h>
#include <Hemsleya/base/timer/clock.h>

#include "current_scheduling.h"
#include "current_context.h"
#include "scheduling.h"

namespace Hemsleya {
namespace active {

template <class T>
class condition{
public:
	condition() : _wait_flag(false), _signal_flag(false), _scheduling(0), _wait_context(0) {
	}

	virtual ~condition() {}

	bool wait_any(T & data, uint64_t outtime){
		uint64_t waittime = outtime + timer::clock();


		while (_signal_flag.exchange(false) != true){
			if (_scheduling == 0){
				_scheduling = current_scheduling();
			}

			if (_wait_context == 0){
				_wait_context = current_context();
			}

			if (waittime < timer::clock()){
				return false;
			}

			_wait_flag.store(true);
			_scheduling->wait(waittime);
		}
		
		while(_mutex.test_and_set());
		data = _data;
		_mutex.clear();

		return true;
	}

	void post(const T & data){
		if (_wait_flag.exchange(false) == true){
			_scheduling->post(_wait_context);
		}

		_signal_flag.store(true);
		
		while(_mutex.test_and_set());
		_data = data;
		_mutex.clear();
	}

private:
	boost::atomic_flag _mutex;
	T _data;
	boost::atomic_bool _wait_flag, _signal_flag;
	scheduling * _scheduling;
	context::context * _wait_context;

};

namespace no_template{

class condition{
public:
	condition();

	virtual ~condition();

	bool wait_any(uint64_t outtime);

	void post();

private:
	boost::atomic_bool _wait_flag, _signal_flag;
	scheduling * _scheduling;
	context::context * _wait_context;

};

}// no_template

} /* namespace active */
} /* namespace Hemsleya */
#endif /* CONDITION_H_ */
