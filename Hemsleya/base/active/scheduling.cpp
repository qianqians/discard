/*
 * mirco_active.cpp
 *
 *  Created on: 2012-3-27
 *      Author: qianqians
 */
#include "scheduling.h"
#include "exit.h"
#include "task.h"
#include "actuator.h"
#include "context2actuator.h"

#include <boost/bind.hpp>

#include <Hemsleya/base/timer/clock.h>

namespace Hemsleya {
namespace active {

scheduling::scheduling(boost::function<context::context*(void)> _fn_scheduling_) : _main_context(boost::bind(&scheduling::work, this)) {
	_fn_scheduling = _fn_scheduling_;
}

scheduling::~scheduling(){
}

void scheduling::run(){
	_main_context();
}

void scheduling::work(){
	while(is_run.load()){
		_current_context = onscheduling();
		if (_current_context != 0){
			context::yield(*_current_context);
		}
	}

	for(std::list<actuator * >::iterator iter = _list_actuator.begin(); iter != _list_actuator.end(); iter++){
		_abstract_factory_actuator.release_product(*iter, 1);
	}
}

context::context* scheduling::onscheduling(){
	context::context * _context = 0;
	
	{
		while (!time_wait_task_que.empty()){
			uint64_t t = timer::clock();
			time_wait_handle top = time_wait_task_que.top();
			if(t > top.handle->waittime){
				time_wait_task_que.pop();

				uint32_t _state = top.handle->_state_queue.front();
				top.handle->_state_queue.pop();
				if (_state == time_running){
					continue;
				}

				_context = top.handle->_context;
				goto do_task;
			}
		}
	}
		
	{
		while (!in_signal_context_list.empty()){
			context::context * _context_ = in_signal_context_list.front();
			in_signal_context_list.pop();

			actuator * _actuator = context2actuator(_context_);
			if (_actuator == 0){
				continue;
			}

			task * _task = _actuator->current_task();
			if (_task == 0){
				continue;
			}

			if (_task->_state == time_wait_task){
				_task->_state = running_task;
				_task->_wait_context._state_queue.front() = time_running;
			}

			_context = _context_;
			goto do_task;
		}
	}

	{
		if (!_fn_scheduling.empty()){
			_context = _fn_scheduling();
			goto do_task;
		}
	}

	{
		if (!low_priority_context_list.empty()){
			_context = in_signal_context_list.front();
			in_signal_context_list.pop();
			goto do_task;
		}
	}

do_task:
	{
		if(_context == 0){
			actuator * _actuator = _abstract_factory_actuator.create_product();
			_context = _actuator->context();
			_list_actuator.push_back(_actuator);
		}
	}

	return _context;
}

void scheduling::yield(){
	low_priority_context_list.push(_current_context);
	context::yield(_main_context);
}

void scheduling::wait(uint64_t waittime){
	actuator * _actuator = context2actuator(_current_context);
	if (_actuator != 0){
		task * _task = _actuator->current_task();
		if (_task != 0){
			_task->_state = time_wait_task;
			_task->_wait_context._state_queue.push(time_wait);
			time_wait_handle _handle;
			_handle.handle = &_task->_wait_context;
			time_wait_task_que.push(_handle);
		}
	}
}

void scheduling::post(context::context * _wait_context){
	in_signal_context_list.push(_wait_context);
}

context::context * scheduling::current_context(){
	return _current_context;
}

} /* namespace active */
} /* namespace Hemsleya */
