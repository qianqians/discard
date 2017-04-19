/*
 * wait.cpp
 *
 *  Created on: 2013-9-28
 *      Author: qianqians
 */
#include "wait.h"
#include "current_scheduling.h"
#include "scheduling.h"

namespace Hemsleya {
namespace active {

void wait(uint64_t waittime){
	scheduling * _scheduling = current_scheduling();
	if (_scheduling != 0){
		_scheduling->wait(waittime);
	}
}

} /* namespace active */
} /* namespace Hemsleya */
