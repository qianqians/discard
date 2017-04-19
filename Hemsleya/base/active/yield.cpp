/*
 * yield.cpp
 *
 *  Created on: 2012-3-27
 *      Author: qianqians
 */
#include "yield.h"
#include "current_scheduling.h"
#include "scheduling.h"

#include <boost/thread.hpp>

namespace Hemsleya {
namespace active {

void yield(){
	scheduling * _scheduling = current_scheduling();
	if (_scheduling != 0){
		_scheduling->yield();
	}else{
		boost::this_thread::yield();
	}
}

} /* namespace active */
} /* namespace Hemsleya */
