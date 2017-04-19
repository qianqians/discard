/*
 * mirco_active.cpp
 *
 *  Created on: 2012-3-27
 *      Author: qianqians
 */
#include "pending_task.h"

#include <Hemsleya/base/concurrent/container/msque.h>

namespace Hemsleya {
namespace active {

namespace detail{
	
Hemsleya::container::msque<task* > _task_que;

} // detail

task * next_task(){
	task * _task = 0;
	detail::_task_que.pop(_task);
	return _task;
}

void push_task(task * _task){
	detail::_task_que.push(_task);
}

} /* namespace active */
} /* namespace Hemsleya */
