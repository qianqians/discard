/*
 * active.cpp
 *
 *  Created on: 2013-1-21
 *      Author: qianqians
 */

#include "active.h"
#include "pending_task.h"
#include "task.h"

#include <boost/thread.hpp>

namespace Hemsleya {
namespace active {

active::active(boost::function<void(void) > fn){
	task.fn = fn;
	push_task(&task);
}

active::~active(){
}

void active::join(){
	while(task._state != done_task){
		boost::this_thread::yield();
	}
}

} /* namespace active */
} /* namespace Hemsleya */