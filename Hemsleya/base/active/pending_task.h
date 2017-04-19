/*
 * task.h
 *
 *  Created on: 2012-3-27
 *      Author: qianqians
 */

#ifndef PENDING_TASK_H_
#define PENDING_TASK_H_

namespace Hemsleya {
namespace active {

struct task;

task * next_task();

void push_task(task * _task);

} /* namespace active */
} /* namespace Hemsleya */
#endif /* TASK_H_ */
