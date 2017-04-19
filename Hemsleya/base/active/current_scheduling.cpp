/*
 * active_server.cpp
 *
 *  Created on: 2012-4-8
 *      Author: qianqians
 */

#include "active_service.h"
#include "scheduling.h"

#include <boost/thread.hpp>

namespace Hemsleya {
namespace active {

boost::thread_specific_ptr<scheduling> _tss_scheduling;

scheduling * current_scheduling(){
	return _tss_scheduling.get();
}

void current_scheduling(scheduling * _scheduling){
	_tss_scheduling.reset(_scheduling);
}

} /* namespace active */
} /* namespace Hemsleya */
