/*
 * active_service.h
 *
 *  Created on: 2013-9-28
 *      Author: qianqians
 */

#ifndef ACTIVE_SERVICE_H_
#define ACTIVE_SERVICE_H_

#include <boost/function.hpp>

#include <Hemsleya/base/context/context.h>

namespace Hemsleya {
namespace active {

class active_service {
public:
	active_service(boost::function<context::context*(void)> _fn_scheduling_);
	~active_service();

	void run();

	void exit();

private:
	boost::function<context::context*(void)> _fn_scheduling;

};

} /* namespace active */
} /* namespace Hemsleya */
#endif /* ACTIVE_SERVICE_H_ */
