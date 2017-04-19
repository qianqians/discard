/*
 * service.h
 *
 *  Created on: 2015-1-11
 *      Author: qianqians
 */
#ifndef _service_h
#define _service_h

#include <boost/shared_ptr.hpp>

#include "../interface/rpcchannel.h"

namespace Fossilizid{
namespace juggle{

class service{
public:
	/*
	 * initialise service
	 */
	virtual void init() = 0;

	/*
	 * drive service work
	 */
	virtual void poll() = 0;

	/*
	 * unixtime
	 */
	virtual uint64_t unixtime() = 0;

	/*
	 * current channel 
	 */
	virtual boost::shared_ptr<channel> get_current_channel() = 0;

};

boost::shared_ptr<service> create_service();

} /* namespace juggle */
} /* namespace Fossilizid */

#endif //_service_h