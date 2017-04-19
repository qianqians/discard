/*
 * condition.cpp
 *
 *  Created on: 2013-3-29
 *      Author: qianqians
 */
#include "condition.h"

#include <Hemsleya/base/timer/clock.h>

namespace Hemsleya {
namespace active {

namespace no_template{

condition::condition() : _wait_flag(false), _signal_flag(false), _scheduling(0), _wait_context(0) {
}

condition::~condition() {
}

bool condition::wait_any(uint64_t outtime){
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

	return true;
}

void condition::post(){
	if (_wait_flag.exchange(false) == true){
		_scheduling->post(_wait_context);
	}

	_signal_flag.store(true);
}

}//no_template

} /* namespace active */
} /* namespace Hemsleya */
