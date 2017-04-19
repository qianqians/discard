/*
 * globalhandle.h
 *
 *  Created on: 2015-1-11
 *      Author: qianqians
 */
#ifndef _globalhandle_h
#define _globalhandle_h

#include <context/context.h>
#include "service.h"

#include <boost/shared_ptr.hpp>

namespace Fossilizid{
namespace juggle{

extern boost::shared_ptr<juggleservice> _service_handle;

extern context::context _main_context;

} /* namespace juggle */
} /* namespace Fossilizid */

#endif //_obj_h