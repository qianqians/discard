/*
 * active.cpp
 *
 *  Created on: 2013-1-21
 *      Author: qianqians
 */

#include "actuator.h"
#include "pending_task.h"
#include "exit.h"
#include "task.h"
#include "yield.h"

#include <boost/bind.hpp>
#include <boost/thread.hpp>

namespace Hemsleya {
namespace active {

actuator::actuator() : _context(boost::bind(&actuator::work, this)), _task(0){
}

actuator::~actuator(){
}

context::context * actuator::context(){
	return &_context;
}

void actuator::work(){
	while(is_run.load()){
		_task = next_task();
		if (_task != 0){
			_task->_state = running_task;
			_task->fn();
			_task->_state = done_task;
		}else{
			boost::this_thread::yield();
		}
	}

	yield();
}

task * actuator::current_task(){
	return _task;
}

} /* namespace active */
} /* namespace Hemsleya */