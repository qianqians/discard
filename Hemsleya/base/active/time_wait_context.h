/*
 * time_wait_context.h
 *
 *  Created on: 2013-9-28
 *      Author: qianqians
 */

#ifndef TIME_WAIT_CONTEXT_H_
#define TIME_WAIT_CONTEXT_H_

#include <queue>

#include <boost/cstdint.hpp>

#include <Hemsleya/base/context/context.h>

namespace Hemsleya {
namespace active {

enum wait_state{
	time_wait,
	time_running,
};

struct time_wait_context{
	std::queue<uint32_t> _state_queue;
	uint64_t waittime;
	context::context * _context;
};

struct time_wait_handle{
	time_wait_context * handle;
};

bool operator < (time_wait_handle self, time_wait_handle other);

} /* namespace active */
} /* namespace Hemsleya */
#endif /* TIME_WAIT_CONTEXT_H_ */
