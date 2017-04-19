/*
 * active_service.cpp
 *
 *  Created on: 2013-3-28
 *      Author: qianqians
 */
#include "active_service.h"
#include "scheduling.h"
#include "exit.h"

namespace Hemsleya {
namespace active {

active_service::active_service(boost::function<context::context*(void)> _fn_scheduling_){
	_fn_scheduling = _fn_scheduling_;
}

active_service::~active_service(){
}

void active_service::run(){
	if (!is_run.load()){
		is_run.store(true);
	}
	scheduling _scheduling(_fn_scheduling);
	_scheduling.run();
}

void active_service::exit(){
	is_run.store(false);
}

} /* namespace active */
} /* namespace Hemsleya */
