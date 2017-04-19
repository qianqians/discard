/*
 * context2actuator.h
 *
 *  Created on: 2013-9-28
 *      Author: qianqians
 */
#ifndef _CONTEXT2ACTUATOR_H
#define _CONTEXT2ACTUATOR_H

#include <Hemsleya/base/context/context.h>

namespace Hemsleya {
namespace active {

class actuator;

actuator * context2actuator(context::context * _context);

} /* namespace mirco_active */
} /* namespace Hemsleya */

#endif //_CONTEXT2ACTUATOR_H