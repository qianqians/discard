/*
 * task.h
 *
 *  Created on: 2012-3-27
 *      Author: qianqians
 */

#ifndef TASK_H_
#define TASK_H_

#include <boost/function.hpp>

#include "time_wait_context.h"

namespace Hemsleya {
namespace active {

enum task_state{
	wait_task,
	time_wait_task,
	running_task,
	done_task,
};

struct task{
	task();
	~task();

	time_wait_context _wait_context;

	task_state _state; 
	boost::function<void() > fn;
};

} /* namespace active */
} /* namespace Hemsleya */
#endif /* TASK_H_ */
