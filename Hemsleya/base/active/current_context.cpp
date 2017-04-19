/*
 * current_context.cpp
 *
 *  Created on: 2013-9-28
 *      Author: qianqians
 */

#include "current_context.h"
#include "current_scheduling.h"
#include "scheduling.h"

namespace Hemsleya {
namespace active {

context::context * current_context(){
	scheduling * _scheduling = current_scheduling();
	if (_scheduling == 0){
		return 0;
	}

	return _scheduling->current_context();
}

} /* namespace active */
} /* namespace Hemsleya */
