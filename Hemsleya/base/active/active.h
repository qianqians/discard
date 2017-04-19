/*
 * active.h
 *
 *  Created on: 2013-1-21
 *      Author: qianqians
 */
#ifndef _ACTIVE_H
#define _ACTIVE_H

#include <boost/function.hpp>

#include "task.h"

namespace Hemsleya {
namespace active {

class active {
public:
	active(boost::function<void(void) > fn);
	~active();

	void join();

private:
	task task;

};

} /* namespace mirco_active */
} /* namespace Hemsleya */

#endif //_ACTIVE_H