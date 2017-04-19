/*
 * time_wait_context.cpp
 *
 *  Created on: 2013-9-28
 *      Author: qianqians
 */
#include "time_wait_context.h"

namespace Hemsleya {
namespace active {

bool operator < (time_wait_handle self, time_wait_handle other){
	return self.handle->waittime > other.handle->waittime;
}

} /* namespace active */
} /* namespace Hemsleya */
