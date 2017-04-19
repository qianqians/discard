/*
 * mirco_active_service.h
 *
 *  Created on: 2012-3-27
 *      Author: qianqians
 */

#ifndef SCHEDULING_H_
#define SCHEDULING_H_

#include <queue>
#include <list>

#include <boost/function.hpp>

#include <Hemsleya/base/concurrent/abstract_factory/abstract_factory.h>
#include <Hemsleya/base/context/context.h>

#include "time_wait_context.h"

namespace Hemsleya {
namespace active {

class actuator;

class scheduling {
public:
	scheduling(boost::function<context::context*(void)> _fn_scheduling_);
	~scheduling();

	void run();

private:
	void work();
	context::context* onscheduling();

public:
	void yield();
	void wait(uint64_t waittime);
	void post(context::context * _wait_context);

public:
	context::context * current_context();

private:
	context::context _main_context, * _current_context;

	std::priority_queue<time_wait_handle> time_wait_task_que;
	std::queue<context::context * > in_signal_context_list;
	std::queue<context::context * > low_priority_context_list;

	boost::function<context::context*(void)> _fn_scheduling;

	std::list<actuator * > _list_actuator;
	abstract_factory::abstract_factory<actuator> _abstract_factory_actuator;

};

} /* namespace active */
} /* namespace Hemsleya */
#endif /* MIRCO_ACTIVE_SERVICE_H_ */
