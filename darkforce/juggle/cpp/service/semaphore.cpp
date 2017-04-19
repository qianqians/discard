/*
 * semaphore.cpp
 *
 *  Created on: 2015-1-14
 *      Author: qianqians
 */
#include "../interface/semaphore.h"
#include <context/context.h>

#include "globalhandle.h"

namespace Fossilizid{
namespace juggle {

semaphore::semaphore(){
	wait_ct = 0;
}

semaphore::~semaphore(){
}

void semaphore::post(boost::shared_ptr<boost::unordered_map<std::string, boost::any> > signal){
	boost::mutex::scoped_lock l(mu_signal);
	_signal.push(signal);
	l.unlock();

	if (wait_ct != 0){
		_service_handle->wake_up_context(wait_ct);
	}
}

boost::shared_ptr<boost::unordered_map<std::string, boost::any> > semaphore::wait(time_t timeout){
	boost::shared_ptr<boost::unordered_map<std::string, boost::any> > _signal_ = 0;

	do{
		boost::mutex::scoped_lock l(mu_signal);
		if (!_signal.empty()){
			break;
		}
		wait_ct = _service_handle->get_current_context(); 
		_timeout = *((uint64_t*)&timeout);
		l.unlock();

		_service_handle->scheduler();
	} while (0);

	{
		boost::mutex::scoped_lock l(mu_signal);

		_signal_ = _signal.back();
		_signal.pop();
	}
	return _signal_;
}

} /* namespace juggle */
} /* namespace Fossilizid */