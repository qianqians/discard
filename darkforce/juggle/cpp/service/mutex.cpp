/*
 * mutex.cpp
 *
 *  Created on: 2014-11-3
 *      Author: qianqians
 */
#include "../interface/mutex.h"
#include "globalhandle.h"

namespace Fossilizid{
namespace juggle {

mutex::mutex(){
	_mutex = false;
}

mutex::~mutex(){
}

void mutex::lock(){
	if (_mutex){
		_mutex = true;
	} else {
		_service_handle->scheduler();
	}
}

void mutex::unlock(){
	if (_mutex){
		if (!wait_context_list.empty()){
			auto weak_up_ct = wait_context_list.back();
			wait_context_list.pop_back();
			_service_handle->wake_up_context(weak_up_ct);
		}
		_mutex = false;
	}
}

} /* namespace juggle */
} /* namespace Fossilizid */