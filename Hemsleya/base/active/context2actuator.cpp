/*
 * context2actuator.cpp
 *
 *  Created on: 2013-9-28
 *      Author: qianqians
 */
#include "context2actuator.h"
#include "actuator.h"

#include <Hemsleya/base/tools/tools.h>

namespace Hemsleya {
namespace active {

actuator * context2actuator(context::context * _context){
	return container_of(_context, actuator, _context);
}

} /* namespace active */
} /* namespace Hemsleya */