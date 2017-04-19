/*
 * active.h
 *
 *  Created on: 2013-1-21
 *      Author: qianqians
 */
#ifndef _ACTIVE_H
#define _ACTIVE_H

#include <boost/function.hpp>

#include <Hemsleya/base/context/context.h>

namespace Hemsleya {
namespace active {

struct task;

class actuator {
public:
	actuator();
	~actuator();

	context::context * context();
	task * current_task();

private:
	void work();

	context::context _context;
	task * _task;

	friend actuator * context2actuator(context::context * _context);

};

} /* namespace mirco_active */
} /* namespace Hemsleya */

#endif //_ACTIVE_H