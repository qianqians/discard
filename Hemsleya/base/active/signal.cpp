/*
 * signal.cpp
 *
 *  Created on: 2013-9-28
 *      Author: qianqians
 */
#include "signal.h"

namespace Hemsleya {
namespace active {

namespace no_template{

signal::signal() : _wait_flag(false), _signal(0), _scheduling(0), _wait_context(0){
}

signal::~signal(){
}

bool signal::wait_any(uint64_t outtime){
	uint64_t waittime = outtime + timer::clock();


	while (1){
		uint32_t _tempsignal = _signal.load();
		while (_tempsignal > 0){
			if (_signal.compare_exchange_strong(_tempsignal, _tempsignal-1)){
				return true;
			}
		}

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

void signal::post(){
	if (_wait_flag.exchange(false) == true){
		_scheduling->post(_wait_context);
	}

	_signal++;
}

}// no_template

} /* namespace active */
} /* namespace Hemsleya */
