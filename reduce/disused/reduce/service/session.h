/*
 * session.h
 *
 *  Created on: 2014-10-21
 *      Author: qianqians
 */
#ifndef _session_h
#define _session_h

#include "../../third_party/json/json_protocol.h"

namespace Fossilizid{
namespace reduce{

class session{
public:
	/*
	 * sync push network package
	 */
	virtual bool do_sync_push(boost::shared_ptr<session> session, uuid _uuid, Json::Value & value, boost::shared_ptr<Json::Value> & ret, boost::uint64_t wait_time) = 0;

	/*
	 * async push network package
	 */
	virtual bool do_async_push(boost::shared_ptr<session> session, Json::Value & value) = 0;

public:
	virtual void do_time(boost::uint64_t time) = 0;

	virtual void do_pop(boost::shared_ptr<session> session, Json::Value & value) = 0;

	virtual void do_logic() = 0;

};

} /* namespace reduce */
} /* namespace Fossilizid */

#endif //_session_h