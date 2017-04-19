/*
 * mutex.cpp
 *
 *  Created on: 2014-11-3
 *      Author: qianqians
 */
#include "mutex.h"

namespace Fossilizid{
namespace reduce {

mutex::mutex(){
	_mutex = false;
	_mutexid = UUID();
}

mutex::~mutex(){
}

void mutex::lock(){
	if (_mutex){
		_mutex = true;
	} else {
		context::context _context = _service_handle->get_current_context();

		context::context * _tsp_loop_main_context = _service_handle->tsp_loop_main_context.get();
		if (_tsp_loop_main_context != 0){
			context::yield(_tsp_loop_main_context);
		} else if (*_tsp_loop_main_context == _context){
			context::context _context = context::getcontext(_service_handle->_loop_main);
			_service_handle->set_current_context(_context);
			context::yield(_context);
		} else {
			throw std::exception("_tsp_loop_main_context is null");
		}
	}
}

void mutex::unlock(){
	if (_mutex){
		if (wait_context_list.empty()){
			boost::mutex::scoped_lock lock(_service_handle->mu_wake_up_vector);
			_service_handle->wait_weak_up_context.insert(std::make_pair(_mutexid, wait_context_list.back()));
			wait_context_list.pop_back();
		}
		_mutex = false;
	}
}

} /* namespace reduce */
} /* namespace Fossilizid */