/*
 * signal.h
 *
 *  Created on: 2013-9-28
 *      Author: qianqians
 */

#ifndef SIGNAL_H_
#define SIGNAL_H_

#include <boost/atomic.hpp>

#include <Hemsleya/base/concurrent/container/msque.h>
#include <Hemsleya/base/timer/clock.h>

#include "current_scheduling.h"
#include "current_context.h"
#include "scheduling.h"

namespace Hemsleya {
namespace active {

class scheduling;

template <class T>
class signal {
public:
	signal() : _scheduling(0), _wait_flag(false) {}

	virtual ~signal(){}

	bool wait_any(T & data, uint64_t outtime){
		uint64_t waittime = outtime + timer::clock();

		while (!_queue.pop(data)){
			if (_scheduling == 0){
				_scheduling = current_scheduling();
			}

			if (_wait_context == 0){
				_wait_context = current_context();
			}

			if (waittime <= timer::clock()){
				return false;
			}

			_wait_flag.store(true);
				
			_scheduling->wait(waittime);
		}

		return true;
	}

	void post(T data){
		if (_wait_flag.exchange(false) == true){
			_scheduling->post(_wait_context);
		}

		_queue.push(data);
	}

private:
	boost::atomic_bool _wait_flag;
	container::msque<T> _queue;
	scheduling * _scheduling;
	context::context * _wait_context;

};

namespace no_template{
	
class signal {
public:
	signal();

	virtual ~signal();

	bool wait_any(uint64_t outtime);

	void post();

private:
	boost::atomic_bool _wait_flag;
	boost::atomic_uint32_t _signal;
	scheduling * _scheduling;
	context::context * _wait_context;

};

}

} /* namespace active */
} /* namespace Hemsleya */
#endif /* MIRCO_ACTIVE_SERVICE_H_ */
